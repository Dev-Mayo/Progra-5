using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PagosMovilesWeb.Models
{
    public class Cuenta
    {
        public string NumeroCuenta { get; set; }
        public int ClienteId { get; set; }
        public string TipoCuenta { get; set; }
        public decimal Saldo { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}