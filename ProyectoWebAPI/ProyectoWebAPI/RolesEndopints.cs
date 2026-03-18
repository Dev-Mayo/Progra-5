using Microsoft.AspNetCore.Mvc;
using ProyectoWebAPI.Abstract;
using ProyectoWebAPI.DataAccess.Models;
using System.Threading.Tasks;
using System.Linq;

namespace ProyectoWebAPI.Controllers
{
    [ApiController]
    [Route("rol")]
    public class RolEndpoint : ControllerBase
    {
        private readonly IRolService _service;

        public RolEndpoint(IRolService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Roles rol)
        {
            string usuario = HttpContext.Items["Usuario"]?.ToString() ?? "sistema";
            string token = HttpContext.Items["Token"]?.ToString() ?? "";

            await _service.CrearRolAsync(rol, usuario, token);
            return Created("", rol);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Modificar(int id, [FromBody] Roles rol)
        {
            if (id != rol.ID_Rol)
                return BadRequest(new { mensaje = "El id no coincide" });

            string usuario = HttpContext.Items["Usuario"]?.ToString() ?? "sistema";
            string token = HttpContext.Items["Token"]?.ToString() ?? "";

            await _service.ModificarRolAsync(rol, usuario, token);
            return Ok(new { mensaje = "Rol modificado exitosamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            string usuario = HttpContext.Items["Usuario"]?.ToString() ?? "sistema";
            string token = HttpContext.Items["Token"]?.ToString() ?? "";

            await _service.EliminarRolAsync(id, usuario, token);
            return Ok(new { mensaje = "Rol eliminado exitosamente" });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            string usuario = HttpContext.Items["Usuario"]?.ToString() ?? "sistema";
            string token = HttpContext.Items["Token"]?.ToString() ?? "";

            var roles = await _service.ObtenerTodosAsync(usuario, token);
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            string usuario = HttpContext.Items["Usuario"]?.ToString() ?? "sistema";
            string token = HttpContext.Items["Token"]?.ToString() ?? "";

            var roles = await _service.ObtenerRolPorIdAsync(id, usuario, token);
            if (roles == null || !roles.Any())
                return NotFound(new { mensaje = "Rol no encontrado" });

            return Ok(roles.First());
        }
    }
}