using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.BusinessLogic.Excepciones
{
    public class CuentaYaExiste : Exception
    {
        public CuentaYaExiste(string mensaje) : base(mensaje) { }
    }
}
