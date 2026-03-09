using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.Model
{
    public class CuentaRequest
    {
        public int ClienteId { get; set; }
        public string NumeroCuenta { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public bool Estado {  get; set; }
        public string TipoCuenta { get; set; }
    }
}
