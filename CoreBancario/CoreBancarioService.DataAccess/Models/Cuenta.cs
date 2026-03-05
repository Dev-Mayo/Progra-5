using System;
using System.Collections.Generic;

namespace CoreBancarioService.DataAccess.Models;

public class Cuenta 
{
    public int CuentaId { get; set; }

    public string NumeroCuenta { get; set; } = null!;

    public int ClienteId { get; set; }

    public decimal Saldo { get; set; }

    public bool? Estado { get; set; }
    public string TipoCuenta { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual ICollection<Movimiento> Movimiento { get; set; } = new List<Movimiento>();
}
