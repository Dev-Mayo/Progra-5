using System;

namespace CoreBancarioService.BusinessLogic.Excepciones
{
    public class ClienteNoExiste : Exception
    {
        public ClienteNoExiste(string message)
            : base("El cliente no existe")
        {
        }
    }
}
