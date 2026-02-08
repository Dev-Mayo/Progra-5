using System;
using System.Collections.Generic;

namespace CoreBancarioService.DataAccess.Models;

public partial class Movimiento
{
    public int MovimientoId { get; set; }

    public int CuentaId { get; set; }

    public string? TipoMovimiento { get; set; }

    public decimal Monto { get; set; }

    public decimal SaldoAnterior { get; set; }

    public decimal SaldoActual { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? FechaMovimiento { get; set; }

    public virtual Cuenta Cuenta { get; set; } = null!;
}
