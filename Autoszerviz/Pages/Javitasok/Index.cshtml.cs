using Autoszerviz.Data;
using Autoszerviz.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Autoszerviz.Pages.Javitasok;

public class IndexModel : PageModel
{
    private readonly AutoszervizDbContext _db;

    public IndexModel(AutoszervizDbContext db)
    {
        _db = db;
    }

    public IList<Javitas> Javitasok { get; set; } = new List<Javitas>();

    public async Task OnGetAsync()
    {
        Javitasok = await _db.Javitasok
            .Include(j => j.Ugyfel)
            .OrderByDescending(j => j.FelvetelDatuma)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostAllapotAsync(int id, JavitasAllapot ujAllapot, decimal? koltseg)
    {
        var javitas = await _db.Javitasok.FirstOrDefaultAsync(j => j.Id == id);

        if (javitas is null)
        {
            TempData["Hiba"] = "A javítás nem található.";
            return RedirectToPage();
        }

        if (ujAllapot == JavitasAllapot.Kesz)
        {
            if (koltseg is null || koltseg <= 0)
            {
                TempData["Hiba"] = "A javítás csak pozitív költséggel állítható készre.";
                return RedirectToPage();
            }

            javitas.Koltseg = koltseg;
        }

        javitas.Allapot = ujAllapot;
        await _db.SaveChangesAsync();

        return RedirectToPage();
    }
}