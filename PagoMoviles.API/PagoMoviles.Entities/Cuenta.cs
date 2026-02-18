using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PagoMoviles.Entities
{
    [Table("cuenta")]
    public class Cuenta
    {
        [Key]
        [Column("cuenta_id")]
        public int CuentaId { get; set; }

        [Column("numero_cuenta")]
        public string NumeroCuenta { get; set; }

        [Column("cliente_id")]
        public int ClienteId { get; set; }

        [Column("saldo")]
        public decimal Saldo { get; set; }

        [Column("estado")]
        public bool Estado { get; set; }
    }
}