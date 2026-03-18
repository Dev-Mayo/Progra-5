using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

public class Cliente
{
    public int cliente_id { get; set; }
    public string identificacion { get; set; }
    public string nombre { get; set; }
    public string apellido { get; set; }
    public string Email { get; set; }
    public int Tipo_Identificacion { get; set; }
    public int Telefono { get; set; }
    public int Rol { get; set; }

    [JsonIgnore]
    public byte[]? ContrasenaHash { get; set; }

    [NotMapped]
    public string? Password { get; set; }

    public bool Estado { get; set; }
}
