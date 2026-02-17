using PagoMoviles.Services.Interfaces;

namespace PagoMoviles.Services.Implementations
{
    /// <summary>
    /// Implementación REAL del servicio de validación de tokens (SRV5).
    /// Llama al endpoint GET /api/validate del AuthService de Diego.
    /// </summary>
    public class RealTokenValidationService : ITokenValidationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RealTokenValidationService> _logger;

        public RealTokenValidationService(HttpClient httpClient, ILogger<RealTokenValidationService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "/api/validate");
                request.Headers.Add("Authorization", $"Bearer {token}");

                var response = await _httpClient.SendAsync(request);

                _logger.LogInformation("SRV5 - Validación de token: {StatusCode}", response.StatusCode);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SRV5 - Error al validar token contra AuthService");
                return false;
            }
        }
    }
}