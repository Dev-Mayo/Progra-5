using System.Net.Http.Headers;
using CoreBancarioService.Abstract.Security;

namespace CoreBancarioService.BusinessLogic.Security
{
    public class TokenValidationService : ITokenValidationService
    {
        private readonly HttpClient _httpClient;

        public TokenValidationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("/api/validate");

            return response.IsSuccessStatusCode;
        }
    }
}
