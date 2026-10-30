using Autoszerviz.Data;
using Autoszerviz.Models;
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
}