using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagoMoviles.Abstract;

namespace PagoMoviles.API.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    [Authorize] // SRV13 y SRV11 requieren token válido
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
            string tokenActual = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            string usuarioToken = User.Identity?.Name ?? "UsuarioDesconocido";
            if (string.IsNullOrWhiteSpace(telefono) || string.IsNullOrWhiteSpace(identificacion))
            {
                return BadRequest(new
                {
                    codigo = -1,
                    descripcion = "Debe enviar los datos completos y válidos"
                });
            }
            try
            {
               
                var result = await _accountService.GetBalance(telefono, identificacion);

                
                await _bitacoraService.RegistrarAsync(usuarioToken, $"El usuario consulta saldo del tel: {telefono}", tokenActual);

                return Ok(new { codigo = 0, descripcion = "Consulta exitosa", data = result });
            }
            catch (Exception ex)
            {
                // CUMPLE REGISTRO DE ERRORES TÉCNICOS 
                await _bitacoraService.RegistrarAsync(usuarioToken, $"ERROR TÉCNICO en GetBalance: {ex.Message}", tokenActual);

                if (ex.Message.Contains("no asociado")) return NotFound(new { codigo = -1, descripcion = "Cliente no asociado a pagos móviles" });
                return StatusCode(500, new { codigo = -1, descripcion = "Error interno" });
            }
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions([FromQuery] string telefono, [FromQuery] string identificacion)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(telefono) || string.IsNullOrWhiteSpace(identificacion))
                {
                    return BadRequest(new { codigo = -1, descripcion = "Debe enviar los datos completos y válidos" });
                }

                var result = await _accountService.GetLast5Transactions(telefono, identificacion);

                
                string usuarioToken = User.Identity?.Name ?? "UsuarioDesconocido";
                string tokenActual = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                await _bitacoraService.RegistrarAsync(usuarioToken, $"El usuario consulta movimientos del tel: {telefono}", tokenActual);
               

                return Ok(new { codigo = 0, descripcion = "Consulta exitosa", data = result });
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new { codigo = -1, descripcion = "Error interno" });
            }
        }
    }
    
}













