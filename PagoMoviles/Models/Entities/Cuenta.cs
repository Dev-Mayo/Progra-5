using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PagoMoviles.Models.Entities
{
    /// <summary>
    /// Entidad que representa la tabla cuenta del Core Bancario.
    /// Ya existe en la BD del core.
    /// </summary>
    [Table("cuenta")]
    public class Cuenta
    {
        [Key]
        [Column("cuenta_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CuentaId { get; set; }

        [Required]
        [Column("numero_cuenta")]
        [MaxLength(20)]
        public string NumeroCuenta { get; set; } = string.Empty;

        [Column("cliente_id")]
        public int ClienteId { get; set; }

        [Column("saldo")]
        public decimal Saldo { get; set; } = 0;

        [Column("estado")]
        public bool Estado { get; set; } = true;

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [ForeignKey("ClienteId")]
        public virtual Cliente? Cliente { get; set; }
    }
}
