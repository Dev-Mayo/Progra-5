using System;
using System.Collections.Generic;

namespace CoreBancarioService.DataAccess.Models;

public partial class Cliente
{
    public int ClienteId { get; set; }

    public string Identificacion { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int TipoIdentificacion { get; set; }

    public int Telefono { get; set; }

    public int Rol { get; set; }

    public byte[] ContrasenaHash { get; set; } = null!;

    public bool Estado { get; set; }

    public virtual ICollection<Cuenta> Cuenta { get; set; } = new List<Cuenta>();
}
