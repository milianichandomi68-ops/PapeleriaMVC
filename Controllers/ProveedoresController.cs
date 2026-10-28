using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PapeleriaMVC.Models;

namespace PapeleriaMVC.Controllers;

public class ProveedoresController : Controller
{
    private readonly PapeleriaContext _context;

    public ProveedoresController(PapeleriaContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Proveedores.AsNoTracking().OrderBy(p => p.Nombre).ToListAsync());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,Telefono")] Proveedore proveedor)
    {
        if (!ModelState.IsValid) return View(proveedor);
        _context.Add(proveedor);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var proveedor = await _context.Proveedores.FindAsync(id);
        if (proveedor == null) return NotFound();
        return View(proveedor);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Telefono")] Proveedore proveedor)
    {
        if (id != proveedor.Id) return NotFound();
        if (!ModelState.IsValid) return View(proveedor);
        _context.Update(proveedor);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var proveedor = await _context.Proveedores.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        if (proveedor == null) return NotFound();
        return View(proveedor);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var proveedor = await _context.Proveedores.FindAsync(id);
        if (proveedor != null)
        {
            try
            {
                _context.Proveedores.Remove(proveedor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ViewBag.Error = "No se puede eliminar el proveedor porque tiene productos relacionados.";
                return View("Delete", proveedor);
            }
        }
        return RedirectToAction(nameof(Index));
    }
}
