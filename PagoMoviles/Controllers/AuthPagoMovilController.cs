using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PagoMoviles.Models.DTOs;
using PagoMoviles.Services.Interfaces;

namespace PagoMoviles.Controllers
{
    /// <summary>
    /// Controlador para inscripción (SRV9) y desinscripción (SRV10) de pagos móviles.
    /// Endpoints:
    ///   POST /auth/register           → SRV9
    ///   POST /auth/cancel-subscription → SRV10
    /// </summary>
    [ApiController]
    [Route("auth")]
    public class AuthPagoMovilController : ControllerBase
    {
        private readonly IPagoMovilService _pagoMovilService;
        private readonly ITokenValidationService _tokenService;
        private readonly IBitacoraService _bitacoraService;
        private readonly ILogger<AuthPagoMovilController> _logger;

        public AuthPagoMovilController(
            IPagoMovilService pagoMovilService,
            ITokenValidationService tokenService,
            IBitacoraService bitacoraService,
            ILogger<AuthPagoMovilController> logger)
        {
            _pagoMovilService = pagoMovilService;
            _tokenService = tokenService;
            _bitacoraService = bitacoraService;
            _logger = logger;
        }

        /// <summary>
        /// POST /auth/register
        /// SRV9 - Inscribe un cliente al servicio de pagos móviles.
        /// Asocia un número de teléfono a una cuenta bancaria.
        /// Requiere token de autorización.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Inscribir([FromBody] PagoMovilRegistroDto dto)
        {
            try
            {
                // ─── Validar token de autorización ───
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
                if (string.IsNullOrWhiteSpace(token))
                {
                    return Unauthorized(ApiResponse.Error("No autorizado"));
                }

                var tokenValido = await _tokenService.ValidateTokenAsync(token);
                if (!tokenValido)
                {
                    return Unauthorized(ApiResponse.Error("No autorizado"));
                }

                // ─── Ejecutar inscripción ───
                var resultado = await _pagoMovilService.InscribirAsync(dto);

                // ─── Registrar bitácora ───
                var descripcionBitacora = resultado.Codigo == 0
                    ? $"Inscripción a pagos móviles realizada - {JsonSerializer.Serialize(dto)}"
                    : $"Inscripción a pagos móviles fallida: {resultado.Descripcion} - {JsonSerializer.Serialize(dto)}";

                await _bitacoraService.RegistrarBitacoraAsync("sistema", descripcionBitacora, token);

                // ─── Retornar respuesta con código HTTP adecuado ───
                if (resultado.Codigo == 0)
                    return StatusCode(201, resultado); // 201 Created
                else
                    return BadRequest(resultado); // 400 Bad Request
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SRV9 - Error interno al inscribir");
                return StatusCode(500, ApiResponse.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// POST /auth/cancel-subscription
        /// SRV10 - Desinscribe un cliente del servicio de pagos móviles.
        /// Deshabilita la asociación del teléfono con la cuenta.
        /// Requiere token de autorización.
        /// </summary>
        [HttpPost("cancel-subscription")]
        public async Task<IActionResult> Desinscribir([FromBody] PagoMovilRegistroDto dto)
        {
            try
            {
                // ─── Validar token de autorización ───
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
                if (string.IsNullOrWhiteSpace(token))
                {
                    return Unauthorized(ApiResponse.Error("No autorizado"));
                }

                var tokenValido = await _tokenService.ValidateTokenAsync(token);
                if (!tokenValido)
                {
                    return Unauthorized(ApiResponse.Error("No autorizado"));
                }

                // ─── Ejecutar desinscripción ───
                var resultado = await _pagoMovilService.DesinscribirAsync(dto);

                // ─── Registrar bitácora ───
                var descripcionBitacora = resultado.Codigo == 0
                    ? $"Desinscripción de pagos móviles realizada - {JsonSerializer.Serialize(dto)}"
                    : $"Desinscripción de pagos móviles fallida: {resultado.Descripcion} - {JsonSerializer.Serialize(dto)}";

                await _bitacoraService.RegistrarBitacoraAsync("sistema", descripcionBitacora, token);

                // ─── Retornar respuesta con código HTTP adecuado ───
                if (resultado.Codigo == 0)
                    return Ok(resultado); // 200 OK
                else
                    return BadRequest(resultado); // 400 Bad Request
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SRV10 - Error interno al desinscribir");
                return StatusCode(500, ApiResponse.Error("Error interno del servidor"));
            }
        }
    }
}
