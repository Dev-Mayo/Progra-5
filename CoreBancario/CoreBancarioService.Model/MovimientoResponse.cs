using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.Model
{
    public class MovimientoResponse
    {
        public string TipoMovimiento { get; set; }
        public decimal Monto { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal SaldoActual { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaMovimiento { get; set; }
    }
}
