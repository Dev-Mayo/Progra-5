using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.Model
{
    public class ClienteRequest
    {
        public string identificacion { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public DateOnly fecha_nacimiento { get; set; }
        public int TipoIdentificacion { get; set; }
        public int Telefono {  get; set; }
        public string Email {  get; set; }
        public string Contrasena { get; set; }

    }

    public class ClienteRequestEdit
    {
        public string identificacion { get; set; }
        public string? nombre { get; set; }
        public string? apellido { get; set; }
        public DateOnly? fecha_nacimiento { get; set; }
        public int? TipoIdentificacion { get; set; }
        public int? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Contrasena { get; set; }

    }
}
