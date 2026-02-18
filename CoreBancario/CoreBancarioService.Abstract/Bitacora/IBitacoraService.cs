using CoreBancarioService.Model;
using CoreBancarioService.Model.CoreBancarioService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.Abstract.Bitacora.Bitacora
{
    public interface IBitacoraService
    {
        Task RegistrarEventoAsync(BitacoraRequest request);
    }
}
