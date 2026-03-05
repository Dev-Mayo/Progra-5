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
       CuentaResponse EliminarCuenta(CuentaRequest request);
       CuentaResponse ListarTodas(CuentaRequest request);
       CuentaResponse ListarPorLlavePrimaria(CuentaRequest request);
       CuentaResponse ListarPorCliente(CuentaRequest request);
    }
}
