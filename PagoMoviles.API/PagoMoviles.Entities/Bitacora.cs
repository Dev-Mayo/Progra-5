using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PagoMoviles.Entities
{
    [Table("bitacoras")]
    public class Bitacora
    {
        [Key]
        [Column("bitacora_id")]
        public int BitacoraId { get; set; }

        [Column("fecha_bitacora")]
        public DateTime FechaBitacora { get; set; }

        [Column("usuario")]
        public string Usuario { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }
    }
}