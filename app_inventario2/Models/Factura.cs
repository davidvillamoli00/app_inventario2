using System;
using System.Collections.Generic;

namespace app_inventario2.Models;

public partial class Factura
{
    public long FacturaId { get; set; }

    public string Numero { get; set; } = null!;

    public int ClienteId { get; set; }

    public DateTime Fecha { get; set; }

    public decimal TotalBase { get; set; }

    public decimal TotalImpuesto { get; set; }

    public decimal TotalTotal { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual ICollection<FacturaLinea> FacturaLineas { get; set; } = new List<FacturaLinea>();
}
