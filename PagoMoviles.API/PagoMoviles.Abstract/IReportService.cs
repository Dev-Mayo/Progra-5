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
     
        Task<DailyReportResponse> GetDailyReport(DateTime fecha);
    }
}