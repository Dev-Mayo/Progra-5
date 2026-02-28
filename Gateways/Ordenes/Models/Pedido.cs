using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ordenes.Models
{
    public class Pedido
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string ProductoId { get; set; } = null!; // Aquí guardaremos el ID que viene de Catálogo
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
