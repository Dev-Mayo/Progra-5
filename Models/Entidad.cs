using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PagosMovilesWeb.Models
{
    public class Entidad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string UrlExterna { get; set; }
        public string Estado { get; set; }
    }
}