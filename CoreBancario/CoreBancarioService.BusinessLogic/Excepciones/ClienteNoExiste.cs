using System;

namespace CoreBancarioService.BusinessLogic.Excepciones
{
    public class ClienteNoExiste : Exception
    {
        public ClienteNoExiste()
            : base("El cliente no existe")
        {
        }
    }
}
