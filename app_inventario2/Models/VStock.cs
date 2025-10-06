using System;
using System.Collections.Generic;

namespace app_inventario2.Models;

public partial class VStock
{
    public int ProductoId { get; set; }

    public int AlmacenId { get; set; }

    public decimal? Stock { get; set; }
}
