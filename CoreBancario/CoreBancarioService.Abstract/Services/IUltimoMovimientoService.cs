using CoreBancarioService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.Abstract.Services
{
    public interface IUltimoMovimientoService
    {
        Task<IEnumerable<MovimientoResponse>> ConsultarUltimosMovimientos(string identificacion, string numeroCuenta);

    }
}
