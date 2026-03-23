using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PagosMovilesWeb.Models
{
    public class Cliente
    {
        public string identificacion { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string fecha_nacimiento { get; set; }
        public string tipoIdentificacion { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
    }
}