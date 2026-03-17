using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PagosMovilesWeb.Models
{
    public class Cuenta
    {
        public string numeroCuenta { get; set; }

        public string identificacionCliente { get; set; }

        public string tipoCuenta { get; set; }

        public decimal saldo { get; set; }
    }
}