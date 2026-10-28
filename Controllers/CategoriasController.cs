using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PapeleriaMVC.Models;

namespace PapeleriaMVC.Controllers;

public class CategoriasController : Controller
{
    private readonly PapeleriaContext _context;

    public CategoriasController(PapeleriaContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Categorias.AsNoTracking().OrderBy(c => c.Nombre).ToListAsync());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre")] Categoria categoria)
    {
        if (!ModelState.IsValid) return View(categoria);
        _context.Add(categoria);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null) return NotFound();
        return View(categoria);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Categoria categoria)
    {
        if (id != categoria.Id) return NotFound();
        if (!ModelState.IsValid) return View(categoria);
        _context.Update(categoria);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var categoria = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        if (categoria == null) return NotFound();
        return View(categoria);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria != null)
        {
            try
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ViewBag.Error = "No se puede eliminar la categoría porque tiene productos relacionados.";
                return View("Delete", categoria);
            }
        }
        return RedirectToAction(nameof(Index));
    }
}
