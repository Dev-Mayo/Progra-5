using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.BusinessLogic.Excepciones
{
    internal class ClienteYaExiste : Exception
    {
        public ClienteYaExiste(string mensaje) : base(mensaje) { }
    }
}
