namespace PagoMoviles.Entities
{
    public class ReportResponse
    {
        public DateTime Fecha { get; set; }
        public int TotalGeneralTransacciones { get; set; }
        public decimal MontoTotalGeneral { get; set; }
        public List<TransaccionResumen> Detalle { get; set; } = new();
    }

    public class TransaccionResumen
    {
        public string EntidadOrigen { get; set; } = string.Empty;
        public string EntidadDestino { get; set; } = string.Empty;
        public int TotalTransacciones { get; set; }
        public decimal MontoTotal { get; set; }
    }
}