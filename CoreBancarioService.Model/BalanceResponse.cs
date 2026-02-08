using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.Model
{
    public class BalanceResponse
    {
        public string NumeroCuenta { get; set; } = null!;
        public decimal Saldo { get; set; }
        public DateTime FechaConsulta { get; set; }
    }
}
