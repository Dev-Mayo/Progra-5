namespace ConfigService.Services
{
    public class AuthValidatorClient
    {
        private readonly HttpClient _http;
        private readonly string _authBaseUrl;

        public AuthValidatorClient(HttpClient http, IConfiguration cfg)
        {
            _http = http;
            _authBaseUrl = cfg["Auth:BaseUrl"] ?? throw new InvalidOperationException("Auth:BaseUrl no configurado");
        }

        /// <summary>
        /// Devuelve true si el token es válido (AuthService /validate == 200 y body true).
        /// </summary>
        public async Task<bool> ValidateAsync(HttpRequest request)
        {
            // Extraer token del header Authorization: Bearer
            var auth = request.Headers["Authorization"].ToString();
            string? token = null;
            if (!string.IsNullOrWhiteSpace(auth) && auth.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                token = auth["Bearer ".Length..].Trim();

            if (string.IsNullOrWhiteSpace(token))
                return false;

            var url = $"{_authBaseUrl.TrimEnd('/')}/validate?token={Uri.EscapeDataString(token)}";
            try
            {
                using var resp = await _http.GetAsync(url);
                if (!resp.IsSuccessStatusCode) return false;

                var text = await resp.Content.ReadAsStringAsync();
                // El AuthService devuelve 'true' (JSON literal o texto)
                return text.Trim().Equals("true", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                // Si no se puede validar, negar acceso
                return false;
            }
        }
    }
}
