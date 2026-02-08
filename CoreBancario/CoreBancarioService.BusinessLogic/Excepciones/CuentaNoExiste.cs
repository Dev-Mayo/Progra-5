using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.BusinessLogic.Excepciones
{
    public class CuentaNoExisteException : Exception
    {
        public CuentaNoExisteException(string mensaje) : base(mensaje) { }
    }
}
