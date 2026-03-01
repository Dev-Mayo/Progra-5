using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWebAPI.DataAccess.Models
{
    public class Pantalla
    {
        public int ID_Pantalla { get; set; }
        public string Nombre_Pantalla { get; set; }
        public string Descripcion { get; set; }
        public string Ruta_Acceso { get; set; }
        public bool Estado { get; set; }
    }

}
