using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ApiGateway.Controllers
{
    [Route("api/resumen-orden")]
    [ApiController]
    public class ResumenController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public ResumenController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetResumen(string id)
        {
            var client = _clientFactory.CreateClient();

            try
            {
                // 1. Obtener la Orden
                var ordenRes = await client.GetFromJsonAsync<System.Text.Json.JsonElement>($"https://localhost:7165/api/pedidos/{id}");

                // 2. Extraer el ProductoId de forma segura
                // Nota: Asegúrate de que en el JSON de tu microservicio el campo se llame "productoId" o "ProductoId"
                if (!ordenRes.TryGetProperty("productoId", out var productoIdProp))
                {
                    // Intento con mayúscula por si acaso
                    if (!ordenRes.TryGetProperty("ProductoId", out productoIdProp))
                        return BadRequest("La orden no contiene un ID de producto válido.");
                }

                string productoId = productoIdProp.GetString();

                // 3. Obtener el Producto
                var prodRes = await client.GetFromJsonAsync<System.Text.Json.JsonElement>($"https://localhost:7273/api/productos/{productoId}");

                // 4. AGREGACIÓN: Combinamos todo
                return Ok(new
                {
                    OrdenId = id,
                    Fecha = ordenRes.TryGetProperty("fecha", out var f) ? f.ToString() : "Sin fecha",
                    DetalleProducto = prodRes
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al agregar datos: {ex.Message}");
            }
        }
    }
}
