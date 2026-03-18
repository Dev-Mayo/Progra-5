using Microsoft.AspNetCore.Mvc;
using ProyectoWebAPI.Abstract;
using ProyectoWebAPI.DataAccess.Models;
using System.Threading.Tasks;
using System.Linq;

namespace ProyectoWebAPI.Controllers
{
    [ApiController]
    [Route("user")]
    public class ClienteEndpoint : ControllerBase
    {
        private readonly IClienteService _service;

        public ClienteEndpoint(IClienteService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Cliente cliente)
        {
            string usuario = HttpContext.Items["Usuario"]?.ToString() ?? "sistema";
            string token = HttpContext.Items["Token"]?.ToString() ?? "";

            await _service.CrearClienteAsync(cliente, usuario, token);
            return Created("", cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Modificar(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.cliente_id)
                return BadRequest(new { mensaje = "El id no coincide" });

            string usuario = HttpContext.Items["Usuario"]?.ToString() ?? "sistema";
            string token = HttpContext.Items["Token"]?.ToString() ?? "";

            await _service.ModificarClienteAsync(cliente, usuario, token);
            return Ok(new { mensaje = "Cliente modificado exitosamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            string usuario = HttpContext.Items["Usuario"]?.ToString() ?? "sistema";
            string token = HttpContext.Items["Token"]?.ToString() ?? "";

            await _service.EliminarClienteAsync(id, usuario, token);
            return Ok(new { mensaje = "Cliente eliminado exitosamente" });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            string usuario = HttpContext.Items["Usuario"]?.ToString() ?? "sistema";
            string token = HttpContext.Items["Token"]?.ToString() ?? "";

            var clientes = await _service.ObtenerTodosAsync(usuario, token);
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            string usuario = HttpContext.Items["Usuario"]?.ToString() ?? "sistema";
            string token = HttpContext.Items["Token"]?.ToString() ?? "";

            var clientes = await _service.ObtenerClientePorIdAsync(id, usuario, token);
            if (clientes == null || !clientes.Any())
                return NotFound(new { mensaje = "Cliente no encontrado" });

            return Ok(clientes.First());
        }

        [HttpGet("filtro")]
        public async Task<IActionResult> Filtrar(string identificacion, string nombre, string tipo)
        {
            string usuario = HttpContext.Items["Usuario"]?.ToString() ?? "sistema";
            string token = HttpContext.Items["Token"]?.ToString() ?? "";

            var clientes = await _service.FiltrarAsync(identificacion, nombre, tipo, usuario, token);
            return Ok(clientes);
        }
    }
}