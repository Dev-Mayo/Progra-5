using System.ComponentModel.DataAnnotations.Schema;

namespace PagoMoviles.Entities
{
    [Table("PAM_TRANSACCION_TB")]
    public class Transaccion
    {
        [Column("TRX_ID")]
        public int TrxId { get; set; }

        [Column("TRX_FECHA")]
        public DateTime TrxFecha { get; set; }

        [Column("TRX_ENT_ORIGEN")]
        public string TrxEntOrigen { get; set; } = string.Empty;

        [Column("TRX_ENT_DESTINO")]
        public string TrxEntDestino { get; set; } = string.Empty;

        // ✅ AGREGAR ESTAS COLUMNAS (faltaban)
        [Column("TRX_TEL_ORIGEN")]
        public string TrxTelOrigen { get; set; } = string.Empty;

        [Column("TRX_NOM_ORIGEN")]
        public string TrxNomOrigen { get; set; } = string.Empty;

        [Column("TRX_TEL_DESTINO")]
        public string TrxTelDestino { get; set; } = string.Empty;

        [Column("TRX_MONTO")]
        public decimal TrxMonto { get; set; }

        [Column("TRX_DESCRIPCION")]
        public string TrxDescripcion { get; set; } = string.Empty;

        [Column("TRX_RUTA")]
        public string? TrxRuta { get; set; }

        [Column("TRX_ESTADO")]
        public string TrxEstado { get; set; } = string.Empty;

        [Column("TRX_COD_RESP")]
        public int? TrxCodResp { get; set; }

        [Column("TRX_DESC_RESP")]
        public string? TrxDescResp { get; set; }
    }
}