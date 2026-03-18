using Microsoft.AspNetCore.Mvc;
using ProyectoWebAPI.Abstract;
using ProyectoWebAPI.DataAccess.Models;
using System.Threading.Tasks;
using System.Linq;

namespace ProyectoWebAPI.Controllers
{
    [ApiController]
    [Route("entidad")]
    public class EntidadesEndpoint : ControllerBase
    {
        private readonly IEntidadesService _service;

        public EntidadesEndpoint(IEntidadesService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] EntidadesBancarias entidad)
        {
            string usuario = HttpContext.Items["Usuario"]?.ToString() ?? "sistema";
            string token = HttpContext.Items["Token"]?.ToString() ?? "";

            await _service.CrearEntidadAsync(entidad, usuario, token);
            return Created("", entidad);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Modificar(int id, [FromBody] EntidadesBancarias entidad)
        {
            if (id != entidad.ID_Entidad)
                return BadRequest(new { mensaje = "El id no coincide" });

            string usuario = HttpContext.Items["Usuario"]?.ToString() ?? "sistema";
            string token = HttpContext.Items["Token"]?.ToString() ?? "";

            await _service.ModificarEntidadAsync(entidad, usuario, token);
            return Ok(new { mensaje = "Entidad modificada exitosamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            string usuario = HttpContext.Items["Usuario"]?.ToString() ?? "sistema";
            string token = HttpContext.Items["Token"]?.ToString() ?? "";

            await _service.EliminarEntidadAsync(id, usuario, token);
            return Ok(new { mensaje = "Entidad eliminada exitosamente" });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            string usuario = HttpContext.Items["Usuario"]?.ToString() ?? "sistema";
            string token = HttpContext.Items["Token"]?.ToString() ?? "";

            var entidades = await _service.ObtenerTodosAsync(usuario, token);
            return Ok(entidades);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            string usuario = HttpContext.Items["Usuario"]?.ToString() ?? "sistema";
            string token = HttpContext.Items["Token"]?.ToString() ?? "";

            var entidades = await _service.ObtenerEntidadPorIdAsync(id, usuario, token);
            if (entidades == null || !entidades.Any())
                return NotFound(new { mensaje = "Entidad no encontrada" });

            return Ok(entidades.First());
        }
    }
}