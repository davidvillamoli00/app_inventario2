using System;
using System.Collections.Generic;

namespace app_inventario2.Models;

public partial class MovimientoStock
{
    public long MovimientoId { get; set; }

    public int AlmacenId { get; set; }

    public int ProductoId { get; set; }

    public DateTime Fecha { get; set; }

    public decimal Cantidad { get; set; }

    public string Origen { get; set; } = null!;

    public string? Referencia { get; set; }

    public virtual Almacen Almacen { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;
}
