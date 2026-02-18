using System.ComponentModel.DataAnnotations.Schema;

namespace PagoMoviles.Entities
{
    [Table("PAM_TRANSACCION_TB")] // Nombre tabla de David
    public class Transaccion
    {
        [Column("TRX_ID")]
        public int TrxId { get; set; }

        [Column("TRX_FECHA")]
        public DateTime TrxFecha { get; set; }

        [Column("TRX_MONTO")]
        public decimal TrxMonto { get; set; }

        [Column("TRX_ENT_ORIGEN")]
        public string TrxEntOrigen { get; set; } = string.Empty;

        [Column("TRX_ENT_DESTINO")]
        public string TrxEntDestino { get; set; } = string.Empty;

        [Column("TRX_ESTADO")]
        public string TrxEstado { get; set; } = string.Empty;
    }
}