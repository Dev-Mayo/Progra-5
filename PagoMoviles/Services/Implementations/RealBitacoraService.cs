using PagoMoviles.Services.Interfaces;

namespace PagoMoviles.Services.Implementations
{
    
    public class RealBitacoraService : IBitacoraService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RealBitacoraService> _logger;

        public RealBitacoraService(HttpClient httpClient, ILogger<RealBitacoraService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task RegistrarBitacoraAsync(string usuario, string descripcion, string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "/api/Bitacora");
                request.Headers.Add("Authorization", $"Bearer {token}");

                var body = new
                {
                    usuarioAccion = usuario,
                    descripcion = descripcion
                };

                request.Content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(body),
                    System.Text.Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.SendAsync(request);

                _logger.LogInformation("SRV18 - Bitácora registrada: {StatusCode}", response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SRV18 - Error al registrar bitácora");
            }
        }
    }
}