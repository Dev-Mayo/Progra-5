using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagoMoviles.Abstract;

namespace PagoMoviles.API.Controllers
{
    [ApiController]
    [Route("api/reports")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IBitacoraService _bitacoraService;

        public ReportsController(IReportService reportService, IBitacoraService bitacoraService)
        {
            _reportService = reportService;
            _bitacoraService = bitacoraService;
        }

        /// <summary>
        /// SRV17 - AJUSTADO para SA12: Reporte de transacciones diarias
        /// Ahora la fecha es OBLIGATORIA (antes era opcional).
        /// GET /api/reports/transactions/daily?fecha=2026-03-14
        /// </summary>
        [HttpGet("transactions/daily")]
        public async Task<IActionResult> GetDailyReport([FromQuery] DateTime? fecha)
        {
            string tokenActual = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            string usuarioToken = User.Identity?.Name ?? "UsuarioDesconocido";

            // SA12: La fecha ahora es REQUERIDA
            if (!fecha.HasValue)
            {
                return BadRequest(new
                {
                    codigo = -1,
                    descripcion = "Debe indicar la fecha del reporte"
                });
            }

            var fechaBusqueda = fecha.Value;

            try
            {
                var result = await _reportService.GetDailyReport(fechaBusqueda);

                await _bitacoraService.RegistrarAsync(
                    usuarioToken,
                    $"Consulta de reporte diario - Fecha: {fechaBusqueda:yyyy-MM-dd}",
                    tokenActual
                );

                return Ok(new
                {
                    codigo = 0,
                    descripcion = "Reporte generado exitosamente",
                    data = result
                });
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarAsync(
                    usuarioToken,
                    $"ERROR TÉCNICO en GetDailyReport: {ex.Message}",
                    tokenActual
                );

                return StatusCode(500, new
                {
                    codigo = -1,
                    descripcion = "Error interno del servidor"
                });
            }
        }
    }
}