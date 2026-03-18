using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreBancarioService.Model;


namespace CoreBancarioService.Abstract.Services
{
    public interface ITransactionService
    {
        TransactionResponse AplicarTransaccion(TransactionRequest request);
    }
}
