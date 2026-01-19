import os;

class config:
    carpeta_compartida= "C:/Carpeta_cuc_A"
    puerto = 5000
    IP_del_otro_equipo = ["127.0.0.1"]  # IP de la otra PC

    os.makedirs(carpeta_compartida, exist_ok=True)