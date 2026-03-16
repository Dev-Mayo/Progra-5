using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PagosMovilesWeb.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Pantalla> Pantallas { get; set; }
    }
}