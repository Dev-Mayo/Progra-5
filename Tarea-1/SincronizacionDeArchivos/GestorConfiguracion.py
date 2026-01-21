import json
import os

class GestorConfiguracion:
    def __init__(self, archivo_json="configuracion.json"):
        self.archivo_json = archivo_json
        self.datos = self.cargar()

    def cargar(self):
        home = os.path.expanduser("~")
        ruta_defecto = os.path.join(home, "Documents", "Sincroniza")
        
        if os.path.exists(self.archivo_json):
            try:
                with open(self.archivo_json, 'r', encoding="utf-8") as f:
                    return json.load(f)
            except:
                pass
        
        return {
            "ruta_carpeta": ruta_defecto,
            "ips_remotas": "127.0.0.1",
            "puerto_escucha": 5000,
            "puerto_remoto": 5001
        }

    def guardar(self, llave, valor):
        self.datos[llave] = valor
        with open(self.archivo_json, 'w', encoding="utf-8") as f:
            json.dump(self.datos, f, indent=4)

    def obtener(self, llave):
        return self.datos.get(llave)

    def obtener_lista_ips(self):
        cadena = self.datos.get("ips_remotas", "")
        return [ip.strip() for ip in cadena.split(",") if ip.strip()]




