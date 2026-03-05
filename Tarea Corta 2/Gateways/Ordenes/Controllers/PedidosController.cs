using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Ordenes.Models;
using System.Net;


namespace Ordenes.Controllers
{
    [Route("api/[controller]")] // Esto hace que la ruta sea /api/pedidos
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly IMongoCollection<Pedido> _pedidos;
        private readonly IHttpClientFactory _httpClientFactory;

        public PedidosController(IMongoCollection<Pedido> pedidos, IHttpClientFactory httpClientFactory)
        {
            _pedidos = pedidos;
            _httpClientFactory = httpClientFactory;
        }

        // GET: api/pedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> Get()
        {
            var lista = await _pedidos.Find(_ => true).ToListAsync();
            return Ok(lista);
        }

        // POST: api/pedidos
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Pedido nuevoPedido)
        {
            // 1. COMUNICACIÓN INTER-SERVICIO
            // Creamos un cliente HTTP para consultar al microservicio de Catálogo
            var client = _httpClientFactory.CreateClient("CatalogoService");

            // Consultamos: GET https://localhost:5001/api/productos/{id}
            var response = await client.GetAsync($"api/productos/{nuevoPedido.ProductoId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return BadRequest($"Error: El producto con ID {nuevoPedido.ProductoId} no existe en el catálogo.");
            }

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(500, "Error al comunicar con el servicio de Catálogo.");
            }

            // 2. PERSISTENCIA EN BD PROPIA
            await _pedidos.InsertOneAsync(nuevoPedido);

            return CreatedAtAction(nameof(Get), new { id = nuevoPedido.Id }, nuevoPedido);
        }

        // GET: api/pedidos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> Get(string id)
        {
            // Buscamos el pedido en MongoDB por su ID
            var pedido = await _pedidos.Find(p => p.Id == id).FirstOrDefaultAsync();

            if (pedido == null)
            {
                return NotFound(); // Si no existe, devolvemos 404
            }

            return Ok(pedido); // Si existe, lo devolvemos
        }
    }
}
