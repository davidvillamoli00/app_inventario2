using System;
using System.Collections.Generic;

namespace app_inventario2.Models;

public partial class Producto
{
    public int ProductoId { get; set; }

    public string Sku { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public bool EsInventariable { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<FacturaLinea> FacturaLineas { get; set; } = new List<FacturaLinea>();

    public virtual ICollection<MovimientoStock> MovimientoStocks { get; set; } = new List<MovimientoStock>();
}
