using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.Model
{
   public class BalanceRequest
    {
        public string Identificacion { get; set; } = null!;
        public string NumeroCuenta { get; set; } = null!;
    }
}
