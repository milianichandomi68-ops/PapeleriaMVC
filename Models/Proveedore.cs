using System;
using System.Collections.Generic;

namespace PapeleriaMVC.Models;

public partial class Proveedore
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Telefono { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
