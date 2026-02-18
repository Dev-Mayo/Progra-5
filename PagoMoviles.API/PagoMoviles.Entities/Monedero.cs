using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PagoMoviles.Entities
{
    // 1. Aquí ponemos el nombre real de la tabla en la base de Gencarlo
    [Table("monedero_movil")]
    public class Monedero
    {
        [Key]
        [Column("monedero_id")] // Nombre exacto de la foto
        public int MonederoId { get; set; }

        [Column("numero_cuenta")]
        public string NumeroCuenta { get; set; }

        [Column("identificacion")]
        public string Identificacion { get; set; }

        [Column("numero_telefono")]
        public string NumeroTelefono { get; set; }

        [Column("estado")]
        public bool? Estado { get; set; } // El '?' es porque la foto dice que acepta nulos

        [Column("fecha_creacion")]
        public DateTime? FechaCreacion { get; set; }
    }
}