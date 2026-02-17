using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PagoMoviles.Models.Entities
{
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

        [Required]
        [Column("Email")]
        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;

        [Column("Tipo_Identificacion")]
        public int TipoIdentificacion { get; set; }

        [Column("Telefono")]
        public int Telefono { get; set; }

        [Column("Rol")]
        public int Rol { get; set; }

        [Column("ContrasenaHash")]
        public byte[] ContrasenaHash { get; set; } = Array.Empty<byte>();

        [Column("Estado")]
        public bool Estado { get; set; } = true;
    }
}