using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.Model
{
    public class TransactionResponse
    {
        public string NumeroCuenta { get; set; }
        public string TipoMovimiento { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
    }
}
