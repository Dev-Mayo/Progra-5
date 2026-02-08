using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.Abstract.Services
{
    public interface IBitacoraService
    {
        void Registrar(string servicio, string accion, string resultado, string detalle);
    }
}
