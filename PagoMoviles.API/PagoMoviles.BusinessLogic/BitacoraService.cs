using Microsoft.Extensions.Configuration;
using PagoMoviles.Abstract;
using System.Text;
using System.Text.Json;

namespace PagoMoviles.BusinessLogic
{
    public class BitacoraService : IBitacoraService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public BitacoraService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public async Task RegistrarAsync(string UsuarioAccion, string Descripcion, string token)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    var bitacoraUrl = _config["ServiciosExternos:BitacoraUrl"];
                    var client = _httpClientFactory.CreateClient();

                    // Usamos el token que viene del controlador (el del usuario real)
                    if (!string.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    }

                    var body = new
                    {
                        usuarioAccion = UsuarioAccion, 
                        descripcion = Descripcion      
                    };

                    var json = JsonSerializer.Serialize(body);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(bitacoraUrl, content);

                    if (response.IsSuccessStatusCode)
                        Console.WriteLine($"[Bitácora] OK: {UsuarioAccion} - {Descripcion}");
                    else
                        Console.WriteLine($"[Bitácora] Error: {response.StatusCode}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Bitácora] Excepción: {ex.Message}");
                }
            });
        }
    }
}