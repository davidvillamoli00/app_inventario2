using System;
using System.Collections.Generic;

namespace app_inventario2.Models;

public partial class Cliente
{
    public int ClienteId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}
