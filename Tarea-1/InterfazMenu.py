import os
from GestorConfiguracion import GestorConfiguracion
from ServicioBitacora import ServicioBitacora
from MotorSincronizacion import MotorSincronizacion

class InterfazMenu:
    def __init__(self):
        self.config = GestorConfiguracion()
        self.bitacora = ServicioBitacora()
        self.motor = MotorSincronizacion(self.config, self.bitacora)

    def limpiar_pantalla(self):
        os.system('cls' if os.name == 'nt' else 'clear')

    def mostrar(self):
        while True:
            self.limpiar_pantalla()
            print("========================================")
            print("        SISTEMA DE SINCRONIZACION       ")
            print("========================================")
            print(f" ESTADO: {'ACTIVO' if self.motor.activo else 'APAGADO'}")
            print(f" PAUSA:  {'SI' if self.motor.pausado else 'NO'}")
            print(f" RUTA:   {self.config.obtener('ruta_carpeta')}")
            print(f" IPs:    {self.config.obtener('ips_remotas')}")
            print("----------------------------------------")
            print("1. Configurar Ruta de Carpeta")
            print("2. Configurar IPs de Destino")
            print("3. INICIAR Servicio")
            print("4. PAUSAR / REANUDAR")
            print("5. Salir")
            print("========================================")
            
            op = input("Seleccione una opcion: ")
            
            if op == "1":
                ruta = input("Ingrese la nueva ruta: ")
                self.config.guardar("ruta_carpeta", ruta)
            elif op == "2":
                ips = input("Ingrese IPs separadas por coma: ")
                self.config.guardar("ips_remotas", ips)
            elif op == "3":
                self.motor.iniciar()
                print("Servicio iniciado...")
                time.sleep(1)
            elif op == "4":
                self.motor.pausado = not self.motor.pausado
            elif op == "5":
                break

if __name__ == "__main__":
    import time
    menu = InterfazMenu()
    menu.mostrar()




