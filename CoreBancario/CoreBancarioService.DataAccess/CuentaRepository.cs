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
    }
}
