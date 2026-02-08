import logging
import getpass
import os

class BitacoraService:
   
    def __init__(self, ruta_log="bitacora_operaciones.log"):
        self.ruta_log = ruta_log
        self._configurar_logger()

    def _configurar_logger(self):
        """Configura el formato de escritura en el archivo."""
      
        logging.basicConfig(
            filename=self.ruta_log,
            level=logging.INFO,
            format='%(asctime)s | %(message)s', # La fecha se pone sola aquí
            datefmt='%Y-%m-%d %H:%M:%S',
            encoding='utf-8',
            filemode='a'
        )

    def registrar_agregado(self, nombre_archivo: str):
        """
        Cumple los Puntos 1 y 2:
        1. Mensaje: 'Se ha agregado el nuevo archivo...'
        2. Metadatos: Usuario del SO y Fecha.
        """
        try:
            # Obtiene usuario de Windows/Linux automáticamente
            usuario = getpass.getuser()

            # El mensaje exacto que pide el PDF
            mensaje = f"Usuario: {usuario} | Se ha agregado el nuevo archivo {nombre_archivo}"

          
            logging.info(mensaje)
            print(f"✅ Log guardado: {mensaje}")

        except Exception as e:
            self.registrar_error(e) # Paola: Llamada para registrar el error técnico

    def registrar_error(self, excepcion: Exception): # Paola: Inicio del HU2 Punto 4
        """
        CUMPLIMIENTO PUNTO 4 - Historia 2: 
        Registra la excepción ocurrida a nivel técnico.
        """
        try:
            usuario = getpass.getuser()
            tipo_error = type(excepcion).__name__
            mensaje_tecnico = str(excepcion)
            # Paola: Formato de error técnico para la bitácora
            mensaje_final = f"ERROR | Usuario: {usuario} | Excepción: {tipo_error}: {mensaje_tecnico}"
            
            logging.error(mensaje_final)
            print(f"❌ Error técnico registrado en bitácora: {mensaje_final}")
        except Exception as e:
            print(f"Error crítico en bitácora: {e}") # Paola: Fin del Punto 4