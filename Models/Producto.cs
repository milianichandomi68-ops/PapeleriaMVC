using System;
using System.Collections.Generic;

namespace PapeleriaMVC.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public int CategoriaId { get; set; }

    public int? ProveedorId { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;

    public virtual Proveedore? Proveedor { get; set; }
}
