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
        /// SRV17 - Avance 2: Reporte de transacciones por fecha
        /// GET /api/reports/transactions/daily?fecha=2026-03-14
        /// Recibe una fecha y devuelve todas las transacciones de ese día
        /// Muestra: Fecha, Teléfono origen, Teléfono destino, Monto
        /// Al final: Suma total del día
        /// </summary>
        [HttpGet("transactions/daily")]
        public async Task<IActionResult> GetDailyReport([FromQuery] DateTime? fecha)
        {
            string tokenActual = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            string usuarioToken = User.Identity?.Name ?? "UsuarioDesconocido";

            // Si no se envía fecha, usar la fecha de hoy
            var fechaBusqueda = fecha ?? DateTime.Today;

            try
            {
                var result = await _reportService.GetDailyReport(fechaBusqueda);

                // Bitácora
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
                // Bitácora de errores
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