using System.Text;
using System.Text.Json;
using ProyectoWebAPI.Abstract;

namespace ProyectoWebAPI.BusinessLogic
{
    public class BitacoraClient : IBitacoraClient
    {
        private readonly HttpClient _httpClient;

        public BitacoraClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task RegistrarAsync(string usuario, string descripcion, string token)
        {
            try
            {
                var data = new
                {
                    usuarioAccion = usuario,
                    descripcion = descripcion
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "api/Bitacora");
                request.Headers.Add("Authorization", $"Bearer {token}");
                request.Content = new StringContent(
                    JsonSerializer.Serialize(data),
                    Encoding.UTF8,
                    "application/json"
                );

                await _httpClient.SendAsync(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en bitácora: {ex.Message}");
            }
        }
    }
}