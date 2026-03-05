using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.Model
{
    public class CuentaResponse
    {
        public int ClienteId { get; set; }
        public string NumeroCuenta { get; set; }
        public string TipoCuenta { get; set; }
        public decimal Saldo { get; set; }
        public bool? Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}
