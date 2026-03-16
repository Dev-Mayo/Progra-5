using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    public class CuentaLista
    {
        [Display(Name = "Identificacion")]
        public int clienteId { get; set; }

        [Display(Name = "Número de Cuenta")]
        public string numeroCuenta { get; set; }

        [Display(Name = "Tipo de Cuenta")]
        public string tipoCuenta { get; set; }

        [Display(Name = "Saldo")]
        public decimal saldo { get; set; }

        [Display(Name = "Estado")]
        public bool estado { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime fechaCreacion { get; set; }
    }
}