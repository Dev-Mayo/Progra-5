using System.ComponentModel.DataAnnotations;

namespace TareaCorta2.DTO
{
    public class ClienteCreateDto
    {
        [Required]
        public string NombreCompleto { get; set; }

        [Required]
        public string TipoIdentificacion { get; set; }

        [Required]
        public string Identificacion { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
