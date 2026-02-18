using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagoMoviles.Entities;

namespace PagoMoviles.Abstract
{
    public interface IReportService
    {
        // SRV17: Reporte de transacciones diarias
        Task<ReportResponse> GetDailyReport();
    }
}