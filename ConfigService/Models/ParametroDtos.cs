namespace ConfigService.Models
{
    public class ParametroDto
    {
        public string ParametroId { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public DateTimeOffset UpdatedAtUtc { get; set; }
    }

    public class ParametroCreateDto
    {
        public string ParametroId { get; set; } = string.Empty; // A-Z, <=10
        public string Valor { get; set; } = string.Empty;       // <=500
        public string? Descripcion { get; set; }
    }

    public class ParametroUpdateDto
    {
        public string Valor { get; set; } = string.Empty;       // <=500
        public string? Descripcion { get; set; }
    }

    public class ErrorResponse
    {
        public string mensaje { get; set; } = string.Empty;
        public ErrorResponse() { }
        public ErrorResponse(string msg) { mensaje = msg; }
    }
}