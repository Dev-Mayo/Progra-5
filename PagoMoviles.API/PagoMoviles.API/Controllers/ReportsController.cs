using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagoMoviles.Abstract;

namespace PagoMoviles.API.Controllers
{
    [ApiController]
    [Route("api/reports")]
    [Authorize] // ← SRV17 requiere token válido
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IBitacoraService _bitacoraService;

        public ReportsController(IReportService reportService, IBitacoraService bitacoraService)
        {
            _reportService = reportService;
            _bitacoraService = bitacoraService;
        }

        //SRV17: Reporte de transacciones diarias

        [HttpGet("transactions/daily")]
        public async Task<IActionResult> GetDailyReport()
        {
            try
            {
                var tokenActual = Request.Headers["Authorization"]
                                    .ToString()
                                    .Replace("Bearer ", "");

                
                var result = await _reportService.GetDailyReport();

                // Bitácora
                await _bitacoraService.RegistrarAsync(
                    User.Identity?.Name ?? "Desconocido",
                    "Consulta de reporte diario",
                    tokenActual
                );

                return Ok(new
                {
                    codigo = 0,
                    descripcion = "Reporte generado exitosamente",
                    data = result
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    codigo = -1,
                    descripcion = "Error interno del servidor"
                });
            }
        }

    }
}