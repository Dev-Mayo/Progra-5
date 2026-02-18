using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagoMoviles.Entities;

namespace PagoMoviles.Abstract
{
    public interface IAccountService
    {
        // SRV13: Consulta de saldo
        Task<BalanceResponse> GetBalance(string telefono, string identificacion);

        // SRV11: Últimos 5 movimientos
        Task<TransactionResponse> GetLast5Transactions(string telefono, string identificacion);
    }
}