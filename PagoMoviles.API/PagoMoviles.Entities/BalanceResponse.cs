namespace PagoMoviles.Entities
{
    public class BalanceResponse
    {
        public string NumeroCuenta { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
    }
}