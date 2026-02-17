namespace PagoMoviles.Models.DTOs
{
    /// <summary>
    /// DTO para inscripción a pagos móviles (SRV9) y desinscripción (SRV10).
    /// Ambos endpoints reciben los mismos datos.
    /// </summary>
    public class PagoMovilRegistroDto
    {
        public string? NumeroCuenta { get; set; }
        public string? Identificacion { get; set; }
        public string? NumeroTelefono { get; set; }
    }

    /// <summary>
    /// DTO para verificar si un cliente existe en el core (SRV19).
    /// </summary>
    public class ClienteExistsDto
    {
        public string? Identificacion { get; set; }
    }

    /// <summary>
    /// Respuesta del servicio SRV19 - verificar cliente.
    /// </summary>
    public class ClienteExistsResponse
    {
        public bool Existe { get; set; }
    }
}
