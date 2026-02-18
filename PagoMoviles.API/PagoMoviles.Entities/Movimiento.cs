using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PagoMoviles.Entities
{
    [Table("movimiento")]
    public class Movimiento
    {
        [Key]
        [Column("movimiento_id")]
        public int MovimientoId { get; set; }

        [Column("cuenta_id")]
        public int CuentaId { get; set; }

        [Column("tipo_movimiento")]
        public string TipoMovimiento { get; set; }

        [Column("monto")]
        public decimal Monto { get; set; }

        [Column("saldo_anterior")]
        public decimal SaldoAnterior { get; set; }

        [Column("saldo_actual")]
        public decimal SaldoActual { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("fecha_movimiento")]
        public DateTime FechaMovimiento { get; set; }
    }
}