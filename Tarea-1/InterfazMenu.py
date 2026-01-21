import os
import time
import sys
from GestorConfiguracion import GestorConfiguracion
from ServicioBitacora import ServicioBitacora
from MotorSincronizacion import MotorSincronizacion

class InterfazMenu:
    def __init__(self, archivo_config="configuracion.json"):
        self.config = GestorConfiguracion(archivo_config)
        self.bitacora = ServicioBitacora()
        self.motor = MotorSincronizacion(self.config, self.bitacora)

    def limpiar_pantalla(self):
        if os.name == 'nt':
            os.system('cls')
        else:
            os.system('clear')
        print("\033[H\033[J", end="")

    def mostrar(self):
        while True:
            self.limpiar_pantalla()
            print("========================================")
            print("   SINCRONIZADOR DE ARCHIVOS MODULAR    ")
            print(f"   Config: {self.config.archivo_json}")
            print("========================================")
            
            estado_serv = "[ACTIVO]" if self.motor.activo else "[APAGADO]"
            print(f" SERVICIO: {estado_serv}")
            
            if self.motor.pausado:
                print(" ESTADO:   >>> [ SISTEMA EN PAUSA ] <<<")
            else:
                print(" ESTADO:   [ FUNCIONANDO ]")

            print(f" CARPETA:  {self.config.obtener('ruta_carpeta')}")
            print(f" ESCUCHA:  Puerto {self.config.obtener('puerto_escucha')}")
            print(f" REMOTO:   Puerto {self.config.obtener('puerto_remoto')}")
            print(f" DESTINOS: {self.config.obtener('ips_remotas')}")
            print("----------------------------------------")
            print("1. Configurar Ruta de Carpeta")
            print("2. Configurar IPs de Destino")
            print("3. Configurar Puertos (Escucha/Remoto)")
            print("4. INICIAR Servicio")
            print("5. PAUSAR / REANUDAR")
            print("6. Salir")
            print("========================================")
            
            op = input("Seleccione opcion: ")
            
            if op == "1":
                ruta = input("Nueva ruta (ej: C:/Prueba): ").replace("\\", "/")
                self.config.guardar("ruta_carpeta", ruta)
            elif op == "2":
                ips = input("IPs (separadas por coma): ")
                self.config.guardar("ips_remotas", ips)
            elif op == "3":
                p_esc = input("Mi puerto de escucha: ")
                p_rem = input("Puerto del companero: ")
                self.config.guardar("puerto_escucha", int(p_esc))
                self.config.guardar("puerto_remoto", int(p_rem))
            elif op == "4":
                self.motor.iniciar()
                print("Iniciando servicio...")
                time.sleep(1)
            elif op == "5":
                self.motor.pausado = not self.motor.pausado
                self.motor.registrar_cambio_pausa()
                if not self.motor.pausado:
                    print("Reanudando y enviando pendientes...")
                    self.motor.reanudar_pendientes()
                time.sleep(1)
            elif op == "6":
                self.motor.activo = False
                break

if __name__ == "__main__":
    nombre_config = sys.argv[1] if len(sys.argv) > 1 else "configuracion.json"
    app = InterfazMenu(nombre_config)
    app.mostrar()
