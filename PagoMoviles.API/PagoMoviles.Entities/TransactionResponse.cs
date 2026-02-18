namespace PagoMoviles.Entities
{
    public class TransactionResponse
    {
        public string NumeroCuenta { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public List<MovimientoDto> Movimientos { get; set; } = new();
    }

    public class MovimientoDto
    {
        public int MovimientoId { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal SaldoActual { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaMovimiento { get; set; }
    }
}