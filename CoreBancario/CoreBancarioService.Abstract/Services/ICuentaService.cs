using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreBancarioService.Model;

namespace CoreBancarioService.Abstract.Services
{
    public interface ICuentaService //falta probar todo esto
    {
       CuentaResponse CrearCuenta(CuentaRequest request);
       CuentaResponse EditarCuenta(CuentaRequest request);
       CuentaResponse EliminarCuenta(int ClienteId,string NumeroCuenta);
       Task<IEnumerable<CuentaResponse>> ListarTodas();
       Task<IEnumerable<CuentaResponse>> ListarPorLlavePrimaria(string numeroCuenta);
       Task<IEnumerable<CuentaResponse>> ListarPorCliente(int ClienteID);
    }
}
