import logging
import getpass
import threading
import os

class ServicioBitacora:
    def __init__(self, archivo_log="bitacora_operaciones.log"):
        self.archivo_log = archivo_log
        self.lock = threading.Lock()
        self._configurar_logger()

    def _configurar_logger(self):
        logging.basicConfig(
            filename=self.archivo_log,
            level=logging.INFO,
            format='%(asctime)s | %(message)s',
            datefmt='%Y-%m-%d %H:%M:%S',
            encoding='utf-8',
            filemode='a'
        )

    def registrar_accion(self, mensaje):
        try:
            usuario = getpass.getuser()
            entrada = f"INFO | Usuario: {usuario} | {mensaje}"
            with self.lock:
                logging.info(entrada)
        except Exception as e:
            self.registrar_error(e)

    def registrar_agregado(self, nombre_archivo):
        try:
            usuario = getpass.getuser()
            mensaje = f"Usuario: {usuario} | Se ha agregado el nuevo archivo {nombre_archivo}"
            with self.lock:
                logging.info(mensaje)
        except Exception as e:
            self.registrar_error(e)

    def registrar_error(self, excepcion):
        try:
            usuario = getpass.getuser()
            tipo_error = type(excepcion).__name__
            mensaje_final = f"ERROR | Usuario: {usuario} | Excepcion: {tipo_error}: {str(excepcion)}"
            with self.lock:
                logging.error(mensaje_final)
        except:
            pass