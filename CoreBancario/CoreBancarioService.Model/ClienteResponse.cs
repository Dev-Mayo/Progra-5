using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.Model
{
    public class ClienteResponse
    {
        public string identificacion { get; set; }
        public string? nombre { get; set; }
        public string? apellido { get; set; }
        public DateOnly? fecha_nacimiento { get; set; }
        public int? TipoIdentificacion { get; set; }
        public int? Telefono { get; set; }
    }
}
