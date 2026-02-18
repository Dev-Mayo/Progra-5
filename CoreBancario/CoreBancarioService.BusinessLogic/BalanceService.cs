using CoreBancarioService.Abstract.Repositories;
using CoreBancarioService.Abstract.Services;
using CoreBancarioService.BusinessLogic.Excepciones;
using CoreBancarioService.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.BusinessLogic
{
    public class BalanceService : IBalanceService
    {
        private readonly ICuentaRepository _cuentaRepo;

        public BalanceService(ICuentaRepository cuentaRepo)
        {
            _cuentaRepo = cuentaRepo;
        }

        public BalanceResponse ConsultarSaldo(BalanceRequest request)
        {
            try
            {
                var saldo = _cuentaRepo.ConsultarSaldo(
                    request.Identificacion,
                    request.NumeroCuenta
                );

                return new BalanceResponse
                {
                    NumeroCuenta = request.NumeroCuenta,
                    Saldo = saldo,
                    FechaConsulta = DateTime.Now
                };
            }
            catch (SqlException ex) when (ex.Number == 50010)
            {
                throw new CuentaNoExisteException(
                    "La cuenta no existe o no pertenece al cliente"
                );
            }


        }

    }
}
