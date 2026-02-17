using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PagoMoviles.Models.Entities
{
    /// <summary>
    /// Tabla que asocia un teléfono a una cuenta bancaria para pagos móviles.
    /// Usada por SRV9 (inscripción) y SRV10 (desinscripción).
    /// </summary>
    [Table("monedero_movil")]
    public class MonederoMovil
    {
        [Key]
        [Column("monedero_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MonederoId { get; set; }

        [Required]
        [Column("numero_cuenta")]
        [MaxLength(20)]
        public string NumeroCuenta { get; set; } = string.Empty;

        [Required]
        [Column("identificacion")]
        [MaxLength(20)]
        public string Identificacion { get; set; } = string.Empty;

        [Required]
        [Column("numero_telefono")]
        [MaxLength(15)]
        public string NumeroTelefono { get; set; } = string.Empty;

        /// <summary>
        /// Estado del monedero:
        /// 1 = Habilitado (inscrito y activo)
        /// 0 = Deshabilitado (desinscrito)
        /// </summary>
        [Column("estado")]
        public bool Estado { get; set; } = true;

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Column("fecha_modificacion")]
        public DateTime? FechaModificacion { get; set; }
    }
}
