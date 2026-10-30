using System.ComponentModel.DataAnnotations;
using Autoszerviz.Data;
using Autoszerviz.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Autoszerviz.Pages.Javitasok;

public class CreateModel : PageModel
{
    private readonly AutoszervizDbContext _db;

    public CreateModel(AutoszervizDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public JavitasInput Input { get; set; } = new();

    public SelectList UgyfelLista { get; set; } = default!;

    public async Task OnGetAsync()
    {
        await BetoltUgyfelLista();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Input.UgyfelId <= 0)
        {
            ModelState.AddModelError("Input.UgyfelId", "Kérlek, válassz ügyfelet a listából.");
        }

        if (!ModelState.IsValid)
        {
            await BetoltUgyfelLista();
            return Page();
        }

        var ugyfelLetezik = await _db.Ugyfelek.AnyAsync(u => u.Id == Input.UgyfelId);
        if (!ugyfelLetezik)
        {
            ModelState.AddModelError("Input.UgyfelId", "Kérlek, válassz érvényes ügyfelet a listából.");
            await BetoltUgyfelLista();
            return Page();
        }

        var javitas = new Javitas
        {
            UgyfelId = Input.UgyfelId,
            Rendszam = Input.Rendszam.Trim().ToUpperInvariant(),
            Leiras = Input.Leiras,
            Allapot = JavitasAllapot.Felveve,
            FelvetelDatuma = DateTime.Now
        };

        _db.Javitasok.Add(javitas);
        await _db.SaveChangesAsync();

        return RedirectToPage("Index");
    }

    private async Task BetoltUgyfelLista()
    {
        var ugyfelek = await _db.Ugyfelek
            .OrderBy(u => u.Nev)
            .Select(u => new { u.Id, u.Nev })
            .ToListAsync();

        UgyfelLista = new SelectList(ugyfelek, "Id", "Nev", Input.UgyfelId);
    }

    public class JavitasInput
    {
        public int? UgyfelId { get; set; }

        [Required(ErrorMessage = "A rendszám megadása kötelező.")]
        [StringLength(10)]
        [RegularExpression(@"^[A-Z]{3}-\d{3}$", ErrorMessage = "A rendszám formátuma ABC-123 legyen.")]
        public string Rendszam { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Leiras { get; set; }
    }
}