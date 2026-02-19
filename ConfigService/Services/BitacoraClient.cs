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
            _auditBaseUrl = cfg["Audit:BaseUrl"] ?? throw new InvalidOperationException("Audit:BaseUrl no configurado");
        }

        /// <summary>
        /// Envía la bitácora a AuditService reenviando el Authorization que llega en la request original.
        /// </summary>
        public async Task TryLogAsync(HttpRequest originalRequest, string usuario, string descripcion)
        {
            var url = $"{_auditBaseUrl.TrimEnd('/')}/bitacora";
            var payload = new { Usuario = usuario, Descripcion = descripcion };

            try
            {
                var json = JsonSerializer.Serialize(payload);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Reenviar el header Authorization si existe
                if (originalRequest.Headers.TryGetValue("Authorization", out var authValue) &&
                    !string.IsNullOrWhiteSpace(authValue))
                {
                    // usa HttpRequestMessage para setear headers
                    using var req = new HttpRequestMessage(HttpMethod.Post, url);
                    req.Content = content;
                    req.Headers.TryAddWithoutValidation("Authorization", authValue.ToString());
                    using var resp = await _http.SendAsync(req);
                    // No romper si Audit está caído
                }
                else
                {
                    // Si no hay Authorization, no intentamos (cumple SRV18: requiere token)
                }
            }
            catch
            {
                // Silencioso: no interrumpir el flujo de ConfigService
            }
        }
    }
}