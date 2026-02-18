using Microsoft.EntityFrameworkCore;
using PagoMoviles.Abstract;
using PagoMoviles.DataAccess.Contexts;
using PagoMoviles.Entities;

namespace PagoMoviles.BusinessLogic
{
    public class ReportService : IReportService
    {
        private readonly PagosMovilesContext _pagosContext; // Usa la base de David

        public ReportService(PagosMovilesContext pagosContext)
        {
            _pagosContext = pagosContext;
        }

        public async Task<ReportResponse> GetDailyReport()
        {
            var hoy = DateTime.Today;

            // Consultar y agrupar transacciones del día
            var transacciones = await _pagosContext.Transacciones
                .Where(t => t.TrxFecha.Date == hoy)
                .GroupBy(t => new { t.TrxEntOrigen, t.TrxEntDestino })
                .Select(g => new TransaccionResumen
                {
                    EntidadOrigen = g.Key.TrxEntOrigen,
                    EntidadDestino = g.Key.TrxEntDestino,
                    TotalTransacciones = g.Count(),
                    MontoTotal = g.Sum(t => t.TrxMonto)
                })
                .ToListAsync();

            return new ReportResponse
            {
                Fecha = hoy,
                TotalGeneralTransacciones = transacciones.Sum(t => t.TotalTransacciones),
                MontoTotalGeneral = transacciones.Sum(t => t.MontoTotal),
                Detalle = transacciones
            };
        }
    }
}