using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.Model
{
    public class TransactionRequest
    {
        public string NumeroCuenta { get; set; }
        public string TipoMovimiento { get; set; } // DEBITO | CREDITO
        public decimal Monto { get; set; }
        public string? Descripcion { get; set; }
    }
}
