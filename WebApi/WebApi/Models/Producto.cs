using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public int? IdProveedor { get; set; }

    public string? Marca { get; set; }

    public decimal? Precio { get; set; }

    public virtual Proveedore? IdProveedorNavigation { get; set; }
}
