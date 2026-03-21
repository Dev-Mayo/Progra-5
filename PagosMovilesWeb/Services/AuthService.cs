using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PagosMovilesWeb.Services
{
    public class LoginResponse
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public DateTime expires_in { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public static class AuthService
    {
        // *** Cambiar si Diego usa otro puerto ***
        private const string BASE_URL = "https://localhost:7160/";

        private static HttpClient CrearCliente()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    (msg, cert, chain, errors) => true
            };
            return new HttpClient(handler)
            {
                BaseAddress = new Uri(BASE_URL)
            };
        }

        public static async Task<LoginResponse> LoginAsync(
            string email, string password)
        {
            using (var client = CrearCliente())
            {
                var body = new LoginRequest
                {
                    Email = email,
                    Password = password
                };

                var json = JsonConvert.SerializeObject(body);
                var content = new StringContent(
                    json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("api/login", content);

                if ((int)response.StatusCode == 201)
                {
                    var jsonResp = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<LoginResponse>(jsonResp);
                }

                return null;
            }
        }

        public static async Task<bool> ValidateTokenAsync(string token)
        {
            using (var client = CrearCliente())
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer", token);

                var response = await client.GetAsync("api/validate");
                return response.IsSuccessStatusCode;
            }
        }
    }
}