using System;
using System.Collections.Generic;

namespace app_inventario2.Models;

public partial class FacturaLinea
{
    public long LineaId { get; set; }

    public long FacturaId { get; set; }

    public int Nlinea { get; set; }

    public int ProductoId { get; set; }

    public decimal Cantidad { get; set; }

    public decimal PrecioUnit { get; set; }

    public decimal IvaPct { get; set; }

    public virtual Factura Factura { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;
}
