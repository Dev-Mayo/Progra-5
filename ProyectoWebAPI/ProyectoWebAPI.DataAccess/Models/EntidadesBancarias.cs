using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWebAPI.DataAccess.Models
{
    public class EntidadesBancarias
    {
        public int ID_Entidad { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
}
