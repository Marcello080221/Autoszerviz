using Autoszerviz.Data;
using Autoszerviz.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Autoszerviz.Pages.Ugyfelek;

public class IndexModel : PageModel
{
    private readonly AutoszervizDbContext _db;

    public IndexModel(AutoszervizDbContext db)
    {
        _db = db;
    }

    public IList<Ugyfel> Ugyfelek { get; set; } = new List<Ugyfel>();

    public async Task OnGetAsync()
    {
        Ugyfelek = await _db.Ugyfelek
            .OrderBy(u => u.Nev)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var ugyfel = await _db.Ugyfelek
            .Include(u => u.Javitasok)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (ugyfel is null)
        {
            return RedirectToPage();
        }

        if (ugyfel.Javitasok.Any())
        {
            TempData["Hiba"] = "Az ügyfélhez javítás tartozik, ezért nem törölhető.";
            return RedirectToPage();
        }

        _db.Ugyfelek.Remove(ugyfel);
        await _db.SaveChangesAsync();
        return RedirectToPage();
    }
}