using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PagoMoviles.Models.Entities
{
    /// <summary>
    /// Entidad que representa la tabla cliente del Core Bancario.
    /// Esta tabla ya existe en la BD del core (creada por otro compañero).
    /// Se usa en SRV19 para verificar si un cliente existe.
    /// </summary>
    [Table("cliente")]
    public class Cliente
    {
        [Key]
        [Column("cliente_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClienteId { get; set; }

        [Required]
        [Column("identificacion")]
        [MaxLength(20)]
        public string Identificacion { get; set; } = string.Empty;

        [Required]
        [Column("nombre")]
        [MaxLength(15)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Column("apellido")]
        [MaxLength(25)]
        public string Apellido { get; set; } = string.Empty;

        [Column("estado")]
        public bool Estado { get; set; } = true;

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
