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

    [BindProperty(SupportsGet = true)]
    public JavitasAllapot? Szuro { get; set; }

    public async Task OnGetAsync()
    {
        var lekerdezes = _db.Javitasok
            .Include(j => j.Ugyfel)
            .AsQueryable();

        if (Szuro.HasValue)
        {
            lekerdezes = lekerdezes.Where(j => j.Allapot == Szuro.Value);
        }

        Javitasok = await lekerdezes
            .OrderByDescending(j => j.FelvetelDatuma)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var javitas = await _db.Javitasok.FirstOrDefaultAsync(j => j.Id == id);

        if (javitas is null)
        {
            return RedirectToPage();
        }

        _db.Javitasok.Remove(javitas);
        await _db.SaveChangesAsync();
        return RedirectToPage();
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

            //javitas.Koltseg = koltseg;
        }

        javitas.Koltseg = koltseg;
        javitas.Allapot = ujAllapot;
        await _db.SaveChangesAsync();

        return RedirectToPage();
    }
}