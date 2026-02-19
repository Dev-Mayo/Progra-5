using System.Text;
using System.Text.Json;

namespace ConfigService.Services
{
    public class BitacoraClient
    {
        private readonly HttpClient _http;
        private readonly string _auditBaseUrl;

        public BitacoraClient(HttpClient http, IConfiguration cfg)
        {
            _http = http;
            _auditBaseUrl = cfg["Audit:BaseUrl"] ?? "http://localhost"; // opcional
        }

        public async Task TryLogAsync(string usuario, string descripcion)
        {
            var url = $"{_auditBaseUrl.TrimEnd('/')}/bitacora";
            var payload = new { Usuario = usuario, Descripcion = descripcion };

            try
            {
                var json = JsonSerializer.Serialize(payload);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                using var resp = await _http.PostAsync(url, content);
                // ignorar errores, SRV18 define 200/201; si falla, no bloquear
            }
            catch
            {
                // Silent fail (asíncrono)
            }
        }
    }
}