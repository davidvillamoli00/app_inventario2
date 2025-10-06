using System;
using System.Collections.Generic;

namespace app_inventario2.Models;

public partial class Almacen
{
    public int AlmacenId { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public virtual ICollection<MovimientoStock> MovimientoStocks { get; set; } = new List<MovimientoStock>();
}
