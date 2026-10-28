using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PapeleriaMVC.Models;

namespace PapeleriaMVC.Controllers;

public class ProductosController : Controller
{
    private readonly PapeleriaContext _context;

    public ProductosController(PapeleriaContext context)
    {
        _context = context;
    }

    private async Task CargarListas(int? categoriaId = null, int? proveedorId = null)
    {
        var categorias = await _context.Categorias.AsNoTracking().OrderBy(c => c.Nombre).ToListAsync();
        var proveedores = await _context.Proveedores.AsNoTracking().OrderBy(p => p.Nombre).ToListAsync();
        ViewBag.CategoriaId = new SelectList(categorias, "Id", "Nombre", categoriaId);
        ViewBag.ProveedorId = new SelectList(proveedores, "Id", "Nombre", proveedorId);
    }

    public async Task<IActionResult> Index()
    {
        var productos = await _context.Productos.AsNoTracking().OrderBy(p => p.Nombre).ToListAsync();
        ViewBag.Categorias = await _context.Categorias.AsNoTracking().ToDictionaryAsync(c => c.Id, c => c.Nombre);
        ViewBag.Proveedores = await _context.Proveedores.AsNoTracking().ToDictionaryAsync(p => p.Id, p => p.Nombre);
        return View(productos);
    }

    public async Task<IActionResult> Create()
    {
        await CargarListas();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,Precio,Stock,CategoriaId,ProveedorId")] Producto producto)
    {
        if (!ModelState.IsValid)
        {
            await CargarListas(producto.CategoriaId, producto.ProveedorId);
            return View(producto);
        }
        _context.Add(producto);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null) return NotFound();
        await CargarListas(producto.CategoriaId, producto.ProveedorId);
        return View(producto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Precio,Stock,CategoriaId,ProveedorId")] Producto producto)
    {
        if (id != producto.Id) return NotFound();
        if (!ModelState.IsValid)
        {
            await CargarListas(producto.CategoriaId, producto.ProveedorId);
            return View(producto);
        }
        _context.Update(producto);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var producto = await _context.Productos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        if (producto == null) return NotFound();
        return View(producto);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto != null)
        {
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
