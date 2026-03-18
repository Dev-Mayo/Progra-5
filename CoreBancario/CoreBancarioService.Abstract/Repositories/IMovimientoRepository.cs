using CoreBancarioService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.Abstract.Repositories
{
    public interface IMovimientoRepository
    {
        void AplicarTransaccion(
            string numeroCuenta,
            string tipoMovimiento,
            decimal monto,
            string descripcion
        );
    }
    public interface IUltimoMovimientoRepository
    {
        Task<IEnumerable<MovimientoResponse>> ConsultarUltimosMovimientos(string identificacion, string numeroCuenta);
    }

}
