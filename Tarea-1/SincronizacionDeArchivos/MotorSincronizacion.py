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
        self.activo = True
        threading.Thread(target=self.servidor, daemon=True).start()
        threading.Thread(target=self.monitorear, daemon=True).start()

    def servidor(self):
        puerto = int(self.config.obtener("puerto_escucha"))
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
            s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
            s.bind(('0.0.0.0', puerto))
            s.listen(5)
            while self.activo:
                try:
                    conn, addr = s.accept()
                    threading.Thread(target=self.recibir_archivo, args=(conn,)).start()
                except: break

    def recibir_archivo(self, conn):
        try:
            with conn:
                header = conn.recv(1024).decode()
                nombre, tamano = header.split("|")
                tamano = int(tamano)
                ruta = os.path.join(self.config.obtener("ruta_carpeta"), nombre)
                
                with open(ruta, "wb") as f:
                    recibido = 0
                    while recibido < tamano:
                        data = conn.recv(4096)
                        if not data: break
                        f.write(data)
                        recibido += len(data)
                self.bitacora.registrar_agregado(nombre)
        except Exception as e:
            self.bitacora.registrar_error(e)

    def enviar_archivo(self, ip, archivo_obj):
        try:
            with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
                s.settimeout(10)
                puerto_destino = int(self.config.obtener("puerto_remoto"))
                s.connect((ip, puerto_destino))
                header = f"{archivo_obj.nombre}|{archivo_obj.tamano}"
                s.send(header.encode())
                time.sleep(0.2)
                with open(archivo_obj.ruta, "rb") as f:
                    s.sendall(f.read())
        except Exception as e:
            self.bitacora.registrar_error(e)

    def monitorear(self):
        ruta = self.config.obtener("ruta_carpeta")
        if not os.path.exists(ruta): os.makedirs(ruta)
        conocidos = set(os.listdir(ruta))
        
        while self.activo:
            time.sleep(2)
            actuales = set(os.listdir(ruta))
            nuevos = actuales - conocidos
            
            for nombre in nuevos:
                if not self.pausado:
                    ruta_comp = os.path.join(ruta, nombre)
                    if os.path.isfile(ruta_comp):
                        tam = os.path.getsize(ruta_comp)
                        obj = ArchivoObjeto(nombre, ruta_comp, tam)
                        for ip in self.config.obtener_lista_ips():
                            self.enviar_archivo(ip, obj)
                else:
                    self.pendientes.add(nombre)
            conocidos = actuales



