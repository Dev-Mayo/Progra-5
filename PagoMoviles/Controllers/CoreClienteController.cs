using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PagoMoviles.Helpers;
using PagoMoviles.Models.DTOs;
using PagoMoviles.Services.Interfaces;

namespace PagoMoviles.Controllers
{
    /// <summary>
    /// SRV19 - Controlador para verificar si un cliente existe en el core bancario.
    /// Endpoint: /core/client-exists
    /// </summary>
    [ApiController]
    [Route("core")]
    public class CoreClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly ITokenValidationService _tokenService;
        private readonly IBitacoraService _bitacoraService;
        private readonly ILogger<CoreClienteController> _logger;

        public CoreClienteController(
            IClienteService clienteService,
            ITokenValidationService tokenService,
            IBitacoraService bitacoraService,
            ILogger<CoreClienteController> logger)
        {
            _clienteService = clienteService;
            _tokenService = tokenService;
            _bitacoraService = bitacoraService;
            _logger = logger;
        }

        /// <summary>
        /// POST /core/client-exists
        /// Verifica si un cliente existe en el core bancario.
        /// Requiere token de autorización en el header Authorization.
        /// </summary>
        [HttpPost("client-exists")]
        public async Task<IActionResult> ClienteExiste([FromBody] ClienteExistsDto dto)
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

                // ─── Validar datos requeridos ───
                if (!ValidationHelper.EsTextoValido(dto.Identificacion))
                {
                    return BadRequest(ApiResponse.Error("Debe enviar los datos completos y válidos"));
                }

                // ─── Consultar si el cliente existe ───
                var existe = await _clienteService.ClienteExisteAsync(dto.Identificacion!.Trim());

                // ─── Registrar bitácora ───
                await _bitacoraService.RegistrarBitacoraAsync(
                    "sistema",
                    $"El usuario consulta existencia de cliente - {JsonSerializer.Serialize(new { dto.Identificacion, existe })}",
                    token);

                // ─── Responder con resultado ───
                return Ok(new ApiResponse<ClienteExistsResponse>
                {
                    Codigo = 0,
                    Descripcion = "Consulta realizada",
                    Data = new ClienteExistsResponse { Existe = existe }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SRV19 - Error interno al verificar cliente");
                return StatusCode(500, ApiResponse.Error("Error interno del servidor"));
            }
        }
    }
}
