namespace PagoMoviles.Entities
{
    /// <summary>
    /// Respuesta para SRV17 - Avance 2
    /// Muestra transacciones de un día específico con detalle individual
    /// </summary>
    public class DailyReportResponse
    {
        public DateTime Fecha { get; set; }
        public List<TransaccionDetalle> Transacciones { get; set; } = new();
        public decimal TotalMonto { get; set; }
    }

    /// <summary>
    /// Detalle de una transacción individual
    /// Según requisitos: Fecha, Teléfono origen, Teléfono destino, Monto
    /// </summary>
    public class TransaccionDetalle
    {
        public DateTime Fecha { get; set; }
        public string TelefonoOrigen { get; set; } = string.Empty;
        public string TelefonoDestino { get; set; } = string.Empty;
        public decimal Monto { get; set; }
    }
}