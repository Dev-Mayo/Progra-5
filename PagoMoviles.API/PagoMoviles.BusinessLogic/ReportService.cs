using Microsoft.EntityFrameworkCore;
using PagoMoviles.Abstract;
using PagoMoviles.DataAccess.Contexts;
using PagoMoviles.Entities;

namespace PagoMoviles.BusinessLogic
{
    public class ReportService : IReportService
    {
        private readonly PagosMovilesContext _pagosContext;

        public ReportService(PagosMovilesContext pagosContext)
        {
            _pagosContext = pagosContext;
        }

        /// <summary>
        /// SRV17 - Avance 2: Reporte de transacciones por fecha
        /// Muestra todas las transacciones de un día específico con:
        /// - Fecha, Teléfono origen, Teléfono destino, Monto
        /// - Suma total del día
        /// </summary>
        public async Task<DailyReportResponse> GetDailyReport(DateTime fecha)
        {
            // Obtener solo la fecha sin hora para comparar
            var fechaBusqueda = fecha.Date;

            // Consultar transacciones del día específico
            var transacciones = await _pagosContext.Transacciones
                .Where(t => t.TrxFecha.Date == fechaBusqueda)
                .OrderBy(t => t.TrxFecha)
                .Select(t => new TransaccionDetalle
                {
                    Fecha = t.TrxFecha,
                    TelefonoOrigen = t.TrxTelOrigen,
                    TelefonoDestino = t.TrxTelDestino,
                    Monto = t.TrxMonto
                })
                .ToListAsync();

            // Calcular el total del día
            var totalMonto = transacciones.Sum(t => t.Monto);

            return new DailyReportResponse
            {
                Fecha = fechaBusqueda,
                Transacciones = transacciones,
                TotalMonto = totalMonto
            };
        }
    }
}