import os;

class config:
    carpeta_compartida= "C:/Carpeta_cuc_B"
    puerto = 5001
    IP_del_otro_equipo = ["127.0.0.1"]  # IP de la otra pc

    os.makedirs(carpeta_compartida, exist_ok=True)