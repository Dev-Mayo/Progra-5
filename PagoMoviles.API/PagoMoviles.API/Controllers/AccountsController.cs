using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagoMoviles.Abstract;

namespace PagoMoviles.API.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IBitacoraService _bitacoraService;

        public AccountsController(IAccountService accountService, IBitacoraService bitacoraService)
        {
            _accountService = accountService;
            _bitacoraService = bitacoraService;
        }

     
        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance([FromQuery] string telefono, [FromQuery] string identificacion)
        {
            //VALIDACIÓN AL INICIO (PDF línea 5: todos los datos son requeridos)
            if (string.IsNullOrWhiteSpace(telefono) || string.IsNullOrWhiteSpace(identificacion))
            {
                return BadRequest(new
                {
                    codigo = -1,
                    descripcion = "Debe enviar los datos completos y válidos"
                });
            }

            string tokenActual = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            string usuarioToken = User.Identity?.Name ?? "UsuarioDesconocido";

            try
            {
                var result = await _accountService.GetBalance(telefono, identificacion);

                // ✅ BITÁCORA (PDF SRV18)
                await _bitacoraService.RegistrarAsync(
                    usuarioToken,
                    $"El usuario consulta saldo del tel: {telefono}",
                    tokenActual
                );

                // ✅ RESPUESTA EXITOSA
                return Ok(new
                {
                    codigo = 0,
                    descripcion = "Consulta exitosa",
                    data = result
                });
            }
            catch (Exception ex)
            {
                // Bitácora de errores técnicos
                await _bitacoraService.RegistrarAsync(
                    usuarioToken,
                    $"ERROR TÉCNICO en GetBalance: {ex.Message}",
                    tokenActual
                );

                // ✅ MENSAJE EXACTO (PDF línea 6)
                if (ex.Message.Contains("no asociado"))
                {
                    return NotFound(new
                    {
                        codigo = -1,
                        descripcion = "Cliente no asociado a pagos móviles"
                    });
                }

                // ✅ ERROR 500
                return StatusCode(500, new
                {
                    codigo = -1,
                    descripcion = "Error interno del servidor"
                });
            }
        }

 
        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions([FromQuery] string telefono, [FromQuery] string identificacion)
        {
            // ✅ VALIDACIÓN AL INICIO (PDF: Todos los datos son requeridos)
            if (string.IsNullOrWhiteSpace(telefono) || string.IsNullOrWhiteSpace(identificacion))
            {
                return BadRequest(new
                {
                    codigo = -1,
                    descripcion = "Debe enviar los datos completos y válidos"
                });
            }

            string tokenActual = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            string usuarioToken = User.Identity?.Name ?? "UsuarioDesconocido";

            try
            {
                var result = await _accountService.GetLast5Transactions(telefono, identificacion);

                // ✅ BITÁCORA (PDF SRV18)
                await _bitacoraService.RegistrarAsync(
                    usuarioToken,
                    $"El usuario consulta movimientos del tel: {telefono}",
                    tokenActual
                );

                // ✅ RESPUESTA EXITOSA
                return Ok(new
                {
                    codigo = 0,
                    descripcion = "Consulta exitosa",
                    data = result
                });
            }
            catch (Exception ex)
            {
                // Bitácora de errores técnicos
                await _bitacoraService.RegistrarAsync(
                    usuarioToken,
                    $"ERROR TÉCNICO en GetTransactions: {ex.Message}",
                    tokenActual
                );

                if (ex.Message.Contains("no asociado"))
                {
                    return NotFound(new
                    {
                        codigo = -1,
                        descripcion = "Cliente no asociado a pagos móviles"
                    });
                }

                return StatusCode(500, new
                {
                    codigo = -1,
                    descripcion = "Error interno del servidor"
                });
            }
        }
    }
}









