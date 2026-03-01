using System.Net.Http;
using System.Threading.Tasks;
using ProyectoWebAPI.Abstract;
using ProyectoWebAPI.BusinessLogic.Helpers;

namespace ProyectoWebAPI.BusinessLogic
{
    public class TokenService : ITokenService
    {
        private readonly HttpClient _httpClient;

        public TokenService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ValidarTokenAsync(string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "api/validate");
                request.Headers.Add("Authorization", $"Bearer {token}");

                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public Task<string> ObtenerUsuarioDelTokenAsync(string token)
        {
            var usuario = TokenHelper.ObtenerUsuarioDelToken(token);
            return Task.FromResult(usuario);
        }
    }
}