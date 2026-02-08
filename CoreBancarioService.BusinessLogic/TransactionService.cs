using CoreBancarioService.Abstract.Repositories;
using CoreBancarioService.Abstract.Services;
using CoreBancarioService.BusinessLogic.Excepciones;
using CoreBancarioService.Model;
using Microsoft.Data.SqlClient;

namespace CoreBancarioService.BusinessLogic
{
    public class TransactionService : ITransactionService
    {
        private readonly ICuentaRepository _cuentaRepo;
        private readonly IMovimientoRepository _movimientoRepo;

        public TransactionService(
            ICuentaRepository cuentaRepo,
            IMovimientoRepository movimientoRepo)
        {
            _cuentaRepo = cuentaRepo;
            _movimientoRepo = movimientoRepo;
        }

        public TransactionResponse AplicarTransaccion(TransactionRequest request)
        {
            try
            {
                _movimientoRepo.AplicarTransaccion(
                    request.NumeroCuenta,
                    request.TipoMovimiento,
                    request.Monto,
                    request.Descripcion
                );
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50001)
                    throw new CuentaNoExisteException(ex.Message);

                if (ex.Number == 50003)
                    throw new SaldoInsuficienteException(ex.Message);

                throw;
            }

            return new TransactionResponse
            {
                NumeroCuenta = request.NumeroCuenta,
                TipoMovimiento = request.TipoMovimiento,
                Monto = request.Monto,
                Fecha = DateTime.Now
            };
        }
    }
}
