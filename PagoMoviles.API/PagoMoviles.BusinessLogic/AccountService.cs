using Microsoft.EntityFrameworkCore;
using PagoMoviles.Abstract;
using PagoMoviles.DataAccess.Contexts;
using PagoMoviles.Entities;

namespace PagoMoviles.BusinessLogic
{
    public class AccountService : IAccountService
    {
        
        private readonly CoreBancarioContext _coreContext;       // Base de Diego (Cuentas)
        private readonly AfiliacionContext _afiliacionContext;   // Base de Geancarlo (Teléfonos)

        // Ambas bases de datos se inyectan a través del constructor
        public AccountService(CoreBancarioContext coreContext, AfiliacionContext afiliacionContext)
        {
            _coreContext = coreContext;
            _afiliacionContext = afiliacionContext;
        }

        // SRV13: Consulta de Saldo
        public async Task<BalanceResponse> GetBalance(string telefono, string identificacion)
        {
            // Busca el teléfono en la base de GEANCARLO
           
            var monedero = await _afiliacionContext.Monedero
                .FirstOrDefaultAsync(m =>
                    m.NumeroTelefono == telefono &&
                    m.Identificacion == identificacion &&
                    m.Estado == true);

            if (monedero == null)
                throw new Exception("Cliente no asociado a pagos móviles ");

            //busc el saldo en la base de diego
            var cuenta = await _coreContext.Cuentas
                .FirstOrDefaultAsync(c => c.NumeroCuenta == monedero.NumeroCuenta);

            if (cuenta == null)
                throw new Exception("Cuenta no encontrada en Core Bancario");

           
            return new BalanceResponse
            {
                NumeroCuenta = cuenta.NumeroCuenta,
                Saldo = cuenta.Saldo,
                Telefono = telefono,
                Identificacion = identificacion
            };
        }

        // SRV11: Últimos 5 Movimientos
        public async Task<TransactionResponse> GetLast5Transactions(string telefono, string identificacion)
        {
            // Valida afiliación Base Geancarlo
            var monedero = await _afiliacionContext.Monedero
                .FirstOrDefaultAsync(m =>
                    m.NumeroTelefono == telefono &&
                    m.Identificacion == identificacion);

            if (monedero == null)
                throw new Exception("Cliente no asociado ");

            // Buscar cuenta Base diego
            var cuenta = await _coreContext.Cuentas
                .FirstOrDefaultAsync(c => c.NumeroCuenta == monedero.NumeroCuenta);

            if (cuenta == null) throw new Exception("Cuenta no encontrada");

            // Sacar movimientos base diego
            var movimientos = await _coreContext.Movimientos
                .Where(m => m.CuentaId == cuenta.CuentaId)
                .OrderByDescending(m => m.FechaMovimiento)
                .Take(5)
                .Select(m => new MovimientoDto
                {
                    MovimientoId = m.MovimientoId,
                    TipoMovimiento = m.TipoMovimiento,
                    Monto = m.Monto,
                    SaldoAnterior = m.SaldoAnterior,
                    Descripcion = m.Descripcion,
                    FechaMovimiento = m.FechaMovimiento,
                    SaldoActual = m.SaldoActual
                })
                .ToListAsync();

            return new TransactionResponse
            {
                NumeroCuenta = cuenta.NumeroCuenta,
                Telefono = telefono,
                Movimientos = movimientos
            };
        }
    }
}