using Autoszerviz.Data;
using Autoszerviz.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Autoszerviz.Pages.Ugyfelek;

public class DetailsModel : PageModel
{
    private readonly AutoszervizDbContext _db;

    public DetailsModel(AutoszervizDbContext db)
    {
        _db = db;
    }

    public Ugyfel Ugyfel { get; set; } = null!;

    public IList<Javitas> Javitasok { get; set; } = new List<Javitas>();

    public decimal OsszKoltseg { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var ugyfel = await _db.Ugyfelek.FirstOrDefaultAsync(u => u.Id == id);

        if (ugyfel is null)
        {
            return NotFound();
        }

        Ugyfel = ugyfel;

        Javitasok = await _db.Javitasok
            .Where(j => j.UgyfelId == id)
            .OrderByDescending(j => j.FelvetelDatuma)
            .ToListAsync();

        OsszKoltseg = Javitasok.Where(j => j.Koltseg.HasValue).Sum(j => j.Koltseg!.Value);

        return Page();
    }
}