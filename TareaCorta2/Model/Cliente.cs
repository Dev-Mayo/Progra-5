using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TareaCorta2.Model
{
    public class Cliente
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

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

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    }
}
