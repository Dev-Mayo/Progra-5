namespace PagoMoviles.Entities
{
    public class ConsultaSaldoResponse
    {
        public string Telefono { get; set; }
        public string Cuenta { get; set; }
        public decimal Saldo { get; set; }
        public string FechaConsulta { get; set; }
    }
}