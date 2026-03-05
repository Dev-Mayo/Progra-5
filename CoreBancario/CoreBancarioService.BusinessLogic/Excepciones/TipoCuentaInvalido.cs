using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.BusinessLogic.Excepciones
{
    public class TipoCuentaInvalido : Exception
    {
        public TipoCuentaInvalido(string mensaje) : base(mensaje) { }
    }
}
