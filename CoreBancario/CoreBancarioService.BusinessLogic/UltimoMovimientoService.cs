using CoreBancarioService.Abstract.Repositories;
using CoreBancarioService.BusinessLogic.Excepciones;
using CoreBancarioService.Abstract.Services;
using CoreBancarioService.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.BusinessLogic
{
    public class UltimoMovimientoService : IUltimoMovimientoService
    {
        private readonly IUltimoMovimientoRepository _movimientoRepo;

        public UltimoMovimientoService(IUltimoMovimientoRepository movimientoRepo)
        {
            _movimientoRepo = movimientoRepo;
        }

        public IEnumerable<MovimientoResponse> ConsultarUltimosMovimientos(
            string identificacion,
            string numeroCuenta)
        {
            try
            {
                return _movimientoRepo.ConsultarUltimosMovimientos(
                    identificacion,
                    numeroCuenta
                );
            }
            catch (SqlException ex) when (ex.Number == 50020)
            {
                throw new CuentaNoExisteException(ex.Message);
            }
        }
    }
}
