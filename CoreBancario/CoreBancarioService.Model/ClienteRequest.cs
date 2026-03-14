using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string Identificacion { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public DateOnly? fecha_nacimiento { get; set; }
        public int? TipoIdentificacion { get; set; }
        public int? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Contrasena { get; set; }
    }
}
