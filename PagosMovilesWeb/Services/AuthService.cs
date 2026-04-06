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
        private const string BASE_URL = "https://localhost:7084/";
        //<add key="GatewayBaseUrl" value="https://localhost:7084"

        private static readonly HttpClient _httpClient = new HttpClient(
            new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true
            })
        {
            BaseAddress = new Uri(BASE_URL),
            Timeout = TimeSpan.FromSeconds(30)
        };

        public static async Task<(bool success, LoginResponse data, string errorMessage)> LoginAsync(string email, string password)
        {
            try
            {
                var body = new LoginRequest
                {
                    Email = email,
                    Password = password
                };

                var json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/gateway/auth/login", content);

                if ((int)response.StatusCode == 201)
                {
                    var jsonResp = await response.Content.ReadAsStringAsync();
                    var loginData = JsonConvert.DeserializeObject<LoginResponse>(jsonResp);

                    return (true, loginData, null);
                }
                else
                {
                    var errorJson = await response.Content.ReadAsStringAsync();

                    try
                    {
                        dynamic errorObj = JsonConvert.DeserializeObject(errorJson);
                        string mensaje = errorObj.message ?? "Usuario y/o contraseña incorrectos";

                        return (false, null, mensaje);
                    }
                    catch
                    {
                        return (false, null, "Usuario y/o contraseña incorrectos");
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, null, "Error al conectar con el servicio. Intente de nuevo.");
            }
        }

        public static async Task<bool> ValidateTokenAsync(string token)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, "api/validate"))
            {
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
        }
    }
}