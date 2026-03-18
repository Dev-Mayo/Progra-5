using System.ComponentModel.DataAnnotations;

namespace LogService.Models
{
    public class Bitacora
    {
        [Key]
        public int IdBitacora { get; set; }

        [Required]
        public string UsuarioAccion { get; set; } = string.Empty;

        [Required]
        public string Descripcion { get; set; } = string.Empty;

        public DateTime FechaBitacora { get; set; } = DateTime.Now;
    }
}
