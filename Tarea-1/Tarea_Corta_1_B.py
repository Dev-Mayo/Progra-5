import threading;
import os;
import socket;
from datetime import date;
from bitacora_service import BitacoraService
from Configuracion_A import config as configA
from Configuracion_B import config
import time

class archivo:
    def __init__(self, nombre, ruta, tamano, fecha_modificacion):
        self.__nombre = nombre
        self.__ruta = ruta
        self.__tamano = tamano
        self.__fecha_modificacion = fecha_modificacion

    @property
    def nombre(self):
        return self.__nombre

    @property
    def ruta(self):
        return self.__ruta

    @property
    def tamano(self):
        return self.__tamano

    @property
    def fecha_modificacion(self):
        return self.__fecha_modificacion

    def __str__(self):
        return f"{self.nombre} | {self.tamano} bytes | {self.fecha_modificacion}"

def servidor():
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.bind(("", config.puerto))
    s.listen(5)
    print("[SERVIDOR] Escuchando conexiones...")

    while True:
        conn, addr = s.accept()
        threading.Thread(target=recibir_archivo, args=(conn, addr)).start()


def recibir_archivo(conn, addr):
    try:
        header = conn.recv(1024).decode()
        nombre, tamano = header.split("|")
        tamano = int(tamano)

        ruta = os.path.join(config.carpeta_compartida, nombre)

        with open(ruta, "wb") as f:
            recibido = 0
            while recibido < tamano:
                data = conn.recv(4096)
                if not data:
                    break
                f.write(data)
                recibido += len(data)

        print(f"[RECIBIDO] {nombre} desde {addr}")

    except Exception as e:
        print("Error recibiendo archivo:", e)

    finally:
        conn.close()


def enviar_archivo(ip, archivo_obj):
    try:
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        s.connect((ip, configA.puerto))

        s.send(f"{archivo_obj.nombre}|{archivo_obj.tamano}".encode())

        with open(archivo_obj.ruta, "rb") as f:
            s.sendall(f.read())

        s.close()
        print(f"[ENVIADO] {archivo_obj.nombre} a {ip}")
    except Exception as e:
        print("Error enviando archivo", e)

def archivo_listo(ruta, espera=0.5, intentos=5):
    tamaño_anterior = -1

    for _ in range(intentos):
        try:
            tamaño_actual = os.path.getsize(ruta)
            if tamaño_actual == tamaño_anterior:
                return True
            tamaño_anterior = tamaño_actual
            time.sleep(espera)
        except PermissionError:
            time.sleep(espera)

    return False

 def monitorear():\
    bitacora = BitacoraService()
    conocidos = set(os.listdir(config.carpeta_compartida))

    while True:
        actuales = set(os.listdir(config.carpeta_compartida))
        nuevos = actuales - conocidos

        for nombre in nuevos:
            ruta = os.path.join(config.carpeta_compartida, nombre)

            if not archivo_listo(ruta):
                print(f"[ESPERA] Archivo aún en uso: {nombre}")
                continue

            info = archivo(
                nombre,
                ruta,
                os.path.getsize(ruta),
                date.fromtimestamp(os.path.getmtime(ruta))
            )

            for peer in config.IP_del_otro_equipo:
                enviar_archivo(peer, info)

        bitacora.registrar_agregado(nombre) 
            print(f"[BITACORA] Registrado desde B: {nombre}")
          

        conocidos = actuales



if __name__ == "__main__":
    threading.Thread(target=servidor, daemon=True).start()
    monitorear()

