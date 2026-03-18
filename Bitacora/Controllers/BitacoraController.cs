using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LogService.Data;
using LogService.Models;
using LogService.DTOs;

namespace LogService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BitacoraController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BitacoraController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] BitacoraDto dto)
        {
            // Validaciones del SRV18: No vacíos ni espacios en blanco
            if (string.IsNullOrWhiteSpace(dto.UsuarioAccion) || string.IsNullOrWhiteSpace(dto.Descripcion))
            {
                return BadRequest(new { mensaje = "Todos los datos son requeridos y no pueden estar vacíos." });
            }

            var nuevaBitacora = new Bitacora
            {
                UsuarioAccion = dto.UsuarioAccion,
                Descripcion = dto.Descripcion,
                FechaBitacora = DateTime.Now // Fecha/hora actual según SRV18
            };

            _context.Bitacoras.Add(nuevaBitacora);
            await _context.SaveChangesAsync();

            return Created("api/bitacora", nuevaBitacora); // 201 Created según estándar
        }
        
    }
}
