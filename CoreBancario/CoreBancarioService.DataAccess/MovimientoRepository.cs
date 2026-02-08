using CoreBancarioService.Abstract.Repositories;
using CoreBancarioService.DataAccess.Models;
using CoreBancarioService.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CoreBancarioService.DataAccess.Repositories
{
    public class MovimientoRepository : IMovimientoRepository
    {
        private readonly CoreBancarioContext _context;

        public MovimientoRepository(CoreBancarioContext context)
        {
            _context = context;
        }

        public void AplicarTransaccion(
            string numeroCuenta,
            string tipoMovimiento,
            decimal monto,
            string descripcion)
        {
            var parametros = new[]
            {
                new SqlParameter("@NumeroCuenta", numeroCuenta),
                new SqlParameter("@TipoMovimiento", tipoMovimiento),
                new SqlParameter("@Monto", monto),
                new SqlParameter("@Descripcion", descripcion ?? (object)DBNull.Value)
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC sp_AplicarTransaccionPorNumeroCuenta @NumeroCuenta, @TipoMovimiento, @Monto, @Descripcion",
                parametros
            );
        }
    }

    public class UltimoMovimientoRepository : IUltimoMovimientoRepository
    {
        private readonly CoreBancarioContext _context;

        public UltimoMovimientoRepository(CoreBancarioContext context)
        {
            _context = context;
        }

        public IEnumerable<MovimientoResponse> ConsultarUltimosMovimientos(
            string identificacion,
            string numeroCuenta)
        {
            return _context.Database
                .SqlQuery<MovimientoResponse>(
                    $"EXEC sp_ConsultarUltimosMovimientos @Identificacion={identificacion}, @NumeroCuenta={numeroCuenta}")
                .AsEnumerable()
                .ToList();
        }
    }
}
