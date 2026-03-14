using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PagosMovilesWeb.Models
{
    public class Cuenta
    {
        public int clienteId { get; set; }

        public string numeroCuenta { get; set; }

        public string tipoCuenta { get; set; }

        public decimal saldo { get; set; }

        public bool estado { get; set; }

        public DateTime fechaCreacion { get; set; }
    }
}