using System;

namespace CoreBancarioService.BusinessLogic.Excepciones
{
    public class SaldoInsuficienteException : Exception
    {
        public SaldoInsuficienteException(string mensaje) : base(mensaje) { }
    }
}
