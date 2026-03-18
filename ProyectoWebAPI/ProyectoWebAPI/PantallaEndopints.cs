using Microsoft.AspNetCore.Mvc;
using ProyectoWebAPI.Abstract;
using ProyectoWebAPI.DataAccess.Models;
using System.Threading.Tasks;
using System.Linq;

namespace ProyectoWebAPI.Controllers
{
    [ApiController]
    [Route("screen")]
    public class PantallaEndpoint : ControllerBase
    {
        private readonly IPantallaService _service;

        public PantallaEndpoint(IPantallaService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Pantalla pantalla)
        {
            string usuario = HttpContext.Items["Usuario"]?.ToString() ?? "sistema";
            string token = HttpContext.Items["Token"]?.ToString() ?? "";

            await _service.CrearPantallaAsync(pantalla, usuario, token);
            return Created("", pantalla);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Modificar(int id, [FromBody] Pantalla pantalla)
        {
            if (id != pantalla.ID_Pantalla)
                return BadRequest(new { mensaje = "El id no coincide" });

            string usuario = HttpContext.Items["Usuario"]?.ToString() ?? "sistema";
            string token = HttpContext.Items["Token"]?.ToString() ?? "";

            await _service.ModificarPantallaAsync(pantalla, usuario, token);
            return Ok(new { mensaje = "Pantalla modificada exitosamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            string usuario = HttpContext.Items["Usuario"]?.ToString() ?? "sistema";
            string token = HttpContext.Items["Token"]?.ToString() ?? "";

            await _service.EliminarPantallaAsync(id, usuario, token);
            return Ok(new { mensaje = "Pantalla eliminada exitosamente" });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            string usuario = HttpContext.Items["Usuario"]?.ToString() ?? "sistema";
            string token = HttpContext.Items["Token"]?.ToString() ?? "";

            var pantallas = await _service.ObtenerTodosAsync(usuario, token);
            return Ok(pantallas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            string usuario = HttpContext.Items["Usuario"]?.ToString() ?? "sistema";
            string token = HttpContext.Items["Token"]?.ToString() ?? "";

            var pantallas = await _service.ObtenerPantallaPorIdAsync(id, usuario, token);
            if (pantallas == null || !pantallas.Any())
                return NotFound(new { mensaje = "Pantalla no encontrada" });

            return Ok(pantallas.First());
        }
    }
}