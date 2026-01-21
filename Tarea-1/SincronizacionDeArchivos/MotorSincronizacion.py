import socket
import os
import threading
import time

class ArchivoObjeto:
    def __init__(self, nombre, ruta, tamano):
        self.nombre = nombre
        self.ruta = ruta
        self.tamano = tamano

class MotorSincronizacion:
    def __init__(self, config, bitacora):
        self.config = config
        self.bitacora = bitacora
        self.activo = False
        self.pausado = False
        self.pendientes = set()

    def iniciar(self):
        if not self.activo:
            self.activo = True
            threading.Thread(target=self.servidor, daemon=True).start()
            threading.Thread(target=self.monitorear, daemon=True).start()
            self.bitacora.registrar_accion("Servicio de sincronizacion INICIADO")

    def registrar_cambio_pausa(self):
        estado = "PAUSADO" if self.pausado else "REANUDADO"
        self.bitacora.registrar_accion(f"SISTEMA {estado} por el usuario")

    def servidor(self):
        puerto = int(self.config.obtener("puerto_escucha"))
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
            s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
            try:
                s.bind(('0.0.0.0', puerto))
                s.listen(5)
                while self.activo:
                    conn, addr = s.accept()
                    threading.Thread(target=self.recibir_archivo, args=(conn, addr)).start()
            except Exception as e:
                self.bitacora.registrar_error(e)

    def recibir_archivo(self, conn, addr):
        try:
            with conn:
                header = conn.recv(1024).decode('utf-8')
                if not header: return
                nombre, tamano = header.split("|")
                tamano = int(tamano)
                
                ruta_carpeta = self.config.obtener("ruta_carpeta")
                if not os.path.exists(ruta_carpeta): os.makedirs(ruta_carpeta)
                
                ruta_final = os.path.join(ruta_carpeta, nombre)
                with open(ruta_final, "wb") as f:
                    recibido = 0
                    while recibido < tamano:
                        data = conn.recv(4096)
                        if not data: break
                        f.write(data)
                        recibido += len(data)
                
                print(f"\n[AVISO] Archivo '{nombre}' recibido con exito")
                self.bitacora.registrar_agregado(nombre)
        except Exception as e:
            self.bitacora.registrar_error(e)

    def enviar_archivo(self, ip, archivo_obj):
        try:
            with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
                s.settimeout(5)
                puerto_destino = int(self.config.obtener("puerto_remoto"))
                s.connect((ip, puerto_destino))
                
                header = f"{archivo_obj.nombre}|{archivo_obj.tamano}"
                s.send(header.encode('utf-8'))
                time.sleep(0.1)
                
                with open(archivo_obj.ruta, "rb") as f:
                    s.sendall(f.read())
                print(f"\n[AVISO] Archivo '{archivo_obj.nombre}' enviado a {ip}")
        except Exception as e:
            self.bitacora.registrar_error(f"Fallo envio a {ip}: {str(e)}")

    def monitorear(self):
        ruta = self.config.obtener("ruta_carpeta")
        if not os.path.exists(ruta): os.makedirs(ruta)
        conocidos = set(os.listdir(ruta))
        
        while self.activo:
            time.sleep(2)
            if not os.path.exists(ruta): continue
            
            actuales = set(os.listdir(ruta))
            nuevos = actuales - conocidos
            
            for nombre in nuevos:
                ruta_comp = os.path.join(ruta, nombre)
                if not os.path.isfile(ruta_comp): continue

                if self.pausado:
                    print(f"\n[PAUSA] Detectado: {nombre}. Queda en pendientes.")
                    self.pendientes.add(nombre)
                else:
                    self.procesar_envio(nombre, ruta_comp)
            
            conocidos = actuales

    def procesar_envio(self, nombre, ruta_comp):
        tam = os.path.getsize(ruta_comp)
        obj = ArchivoObjeto(nombre, ruta_comp, tam)
        lista_ips = self.config.obtener_lista_ips()
        for ip in lista_ips:
            threading.Thread(target=self.enviar_archivo, args=(ip, obj)).start()

    def reanudar_pendientes(self):
        ruta = self.config.obtener("ruta_carpeta")
        while self.pendientes:
            nombre = self.pendientes.pop()
            ruta_comp = os.path.join(ruta, nombre)
            if os.path.exists(ruta_comp):
                self.procesar_envio(nombre, ruta_comp)