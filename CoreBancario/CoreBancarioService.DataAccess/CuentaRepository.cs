using CoreBancarioService.Abstract.Repositories;
using CoreBancarioService.DataAccess.Models;
using CoreBancarioService.Model;
using CoreBancarioService.Model.CoreBancarioService.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CoreBancarioService.DataAccess
{
    public class CuentaRepository : ICuentaRepository
    {
        private readonly CoreBancarioContext _context;

        public CuentaRepository(CoreBancarioContext context)
        {
            _context = context;
        }

        public CuentaModel ObtenerCuentaPorIdentificacion(string identificacion)
        {
            var cuenta = _context.Cuenta
                .FirstOrDefault(c => c.Cliente.Identificacion == identificacion);

            if (cuenta == null)
                return null;

            return new CuentaModel
            {
                CuentaId = cuenta.CuentaId,
                NumeroCuenta = cuenta.NumeroCuenta,
                Saldo = cuenta.Saldo
            };
        }

        public void ActualizarSaldo(int cuentaId, decimal nuevoSaldo)
        {
            var cuenta = _context.Cuenta.Find(cuentaId);
            if (cuenta == null) return;

            cuenta.Saldo = nuevoSaldo;
            _context.SaveChanges();
        }

        public decimal ConsultarSaldo(string identificacion, string numeroCuenta)
        {
            var saldo = _context.Database
                .SqlQuery<decimal>($@"
            EXEC sp_ConsultarSaldoPorCuenta 
                @Identificacion = {identificacion},
                @NumeroCuenta = {numeroCuenta}")
                .AsEnumerable()
                .Single();

            return saldo;
        }

        private class SaldoResult
        {
            public decimal Saldo { get; set; }
        }
        //-----------------------------------------------SA11------------------
        //Falta probar SPs
        public void CrearCuenta(
            int ClienteId,
            string TipoCuenta)
        {
            var parametros = new[]
            {
                new SqlParameter("@Identificacion", ClienteId),
                new SqlParameter("@TipoCuenta", TipoCuenta)
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC sp_CrearCuenta @Identificacion, @TipoCuenta", // falta crear SP
                parametros
            );
        }

        public void EditarCuenta(
            int ClienteId,
            string NumeroCuenta,
            string TipoCuenta)
        {
            var parametros = new[]
            {
                new SqlParameter("@ClienteId", ClienteId),
                new SqlParameter("@NumeroCuenta", NumeroCuenta),
                new SqlParameter("@TipoCuenta", TipoCuenta) // falta agregar este campo a la BD
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC sp_EditarCuenta @ClienteId, @NumeroCuenta, @TipoCuenta", // falta crear SP
                parametros
            );
        }

        public void EliminarCuenta (
            int ClienteId,
            string NumeroCuenta)
        {
            var parametros = new[]
            {
                new SqlParameter("@ClienteId", ClienteId),
                new SqlParameter("@NumeroCuenta", NumeroCuenta)
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC sp_EliminarCuenta @ClienteId, @NumeroCuenta", // falta probar SP
                parametros
            );
        }

        public void ListarTodas()
        {            
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_ListarTodas" // falta probar SP
            );
        }

        public void ListarPorLlavePrimaria(
            string NumeroCuenta)
        {
            var parametros = new[]
            {
                new SqlParameter("@NumeroCuenta", NumeroCuenta)
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC sp_ListarPorLlavePrimaria @NumeroCuenta", // falta probar SP
                parametros
            );
        }

        public void ListarPorCliente(
            int ClienteId)
        {
            var parametros = new[]
            {
                new SqlParameter("@ClienteId", ClienteId)
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC sp_ListarPorCliente @ClienteId", // falta probar SP
                parametros
            );
        }
    }
}
