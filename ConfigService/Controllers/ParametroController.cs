using System.Text.Json;
using ConfigService.Data;
using ConfigService.Models;
using ConfigService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConfigService.Controllers
{
    [ApiController]
    [Route("parametro")]
    public class ParametroController : ControllerBase
    {
        private readonly ConfigDb _db;
        private readonly AuthValidatorClient _auth;
        private readonly BitacoraClient _bitacora;
        private readonly JwtReader _jwtReader;

        public ParametroController(ConfigDb db, AuthValidatorClient auth, BitacoraClient bitacora, JwtReader jwtReader)
        {
            _db = db;
            _auth = auth;
            _bitacora = bitacora;
            _jwtReader = jwtReader;
        }

        // GET /parametro
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!await _auth.ValidateAsync(Request))
                return Unauthorized();

            var lista = await _db.GetAllAsync();

            // Bitácora: consulta (reenviando Authorization hacia AuditService)
            _ = _bitacora.TryLogAsync(
                Request,
                _jwtReader.GetUserIdFromRequest(Request) ?? "desconocido",
                "El usuario consulta parametros"
            );

            return Ok(lista);
        }

        // GET /parametro/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            if (!await _auth.ValidateAsync(Request))
                return Unauthorized();

            id = (id ?? string.Empty).Trim().ToUpperInvariant();
            if (!IsValidId(id))
                return BadRequest(new ErrorResponse("Identificador inválido: debe ser A-Z y longitud <= 10"));

            var item = await _db.GetByIdAsync(id);
            if (item is null)
                return NotFound(new ErrorResponse("Parámetro no encontrado"));

            // Bitácora: consulta
            _ = _bitacora.TryLogAsync(
                Request,
                _jwtReader.GetUserIdFromRequest(Request) ?? "desconocido",
                $"El usuario consulta parametro {id}"
            );

            return Ok(item);
        }

        // POST /parametro
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ParametroCreateDto dto)
        {
            if (!await _auth.ValidateAsync(Request))
                return Unauthorized();

            // Validaciones
            dto.ParametroId = (dto.ParametroId ?? string.Empty).Trim().ToUpperInvariant();
            dto.Valor = (dto.Valor ?? string.Empty).Trim();
            dto.Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion!.Trim();

            if (!IsValidId(dto.ParametroId))
                return BadRequest(new ErrorResponse("Identificador inválido: debe ser A-Z y longitud <= 10"));

            if (string.IsNullOrWhiteSpace(dto.Valor) || dto.Valor.Length > 500)
                return BadRequest(new ErrorResponse("Valor requerido y longitud <= 500"));

            if (await _db.ExistsAsync(dto.ParametroId))
                return BadRequest(new ErrorResponse("Parámetro ya existe"));

            await _db.CreateAsync(dto);

            // Bitácora: crear con JSON del nuevo registro
            var usuario = _jwtReader.GetUserIdFromRequest(Request) ?? "desconocido";
            var descripcion = $"Crear parametro: {JsonSerializer.Serialize(dto)}";
            _ = _bitacora.TryLogAsync(Request, usuario, descripcion);

            var creado = await _db.GetByIdAsync(dto.ParametroId);
            return Created($"/parametro/{dto.ParametroId}", creado);
        }

        // PUT /parametro/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] ParametroUpdateDto dto)
        {
            if (!await _auth.ValidateAsync(Request))
                return Unauthorized();

            id = (id ?? string.Empty).Trim().ToUpperInvariant();
            if (!IsValidId(id))
                return BadRequest(new ErrorResponse("Identificador inválido: debe ser A-Z y longitud <= 10"));

            dto.Valor = (dto.Valor ?? string.Empty).Trim();
            dto.Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion!.Trim();

            if (string.IsNullOrWhiteSpace(dto.Valor) || dto.Valor.Length > 500)
                return BadRequest(new ErrorResponse("Valor requerido y longitud <= 500"));

            var antes = await _db.GetByIdAsync(id);
            if (antes is null)
                return NotFound(new ErrorResponse("Parámetro no encontrado"));

            var afectados = await _db.UpdateAsync(id, dto);
            if (afectados == 0)
                return StatusCode(500, new ErrorResponse("No se pudo actualizar el parámetro"));

            var despues = await _db.GetByIdAsync(id);

            // Bitácora: actualización con JSON antes/después
            var usuario = _jwtReader.GetUserIdFromRequest(Request) ?? "desconocido";
            var detalle = new { antes, despues };
            _ = _bitacora.TryLogAsync(Request, usuario, $"Actualizar parametro {id}: {JsonSerializer.Serialize(detalle)}");

            return Ok(despues);
        }

        // DELETE /parametro/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (!await _auth.ValidateAsync(Request))
                return Unauthorized();

            id = (id ?? string.Empty).Trim().ToUpperInvariant();
            if (!IsValidId(id))
                return BadRequest(new ErrorResponse("Identificador inválido: debe ser A-Z y longitud <= 10"));

            var antes = await _db.GetByIdAsync(id);
            if (antes is null)
                return NotFound(new ErrorResponse("Parámetro no encontrado"));

            var afectados = await _db.DeleteAsync(id);
            if (afectados == 0)
                return StatusCode(500, new ErrorResponse("No se pudo eliminar el parámetro"));

            // Bitácora: eliminación con JSON del registro eliminado
            var usuario = _jwtReader.GetUserIdFromRequest(Request) ?? "desconocido";
            _ = _bitacora.TryLogAsync(Request, usuario, $"Eliminar parametro {id}: {JsonSerializer.Serialize(antes)}");

            // 200 OK según SRV: puedes usar 204 NoContent si prefieres
            return Ok(new { mensaje = "Eliminado" });
        }

        // ===== Helpers =====
        private static bool IsValidId(string id)
            => !string.IsNullOrWhiteSpace(id) && id.Length <= 10 && id.All(c => c >= 'A' && c <= 'Z');
    }
}