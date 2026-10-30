using System.ComponentModel.DataAnnotations;
using Autoszerviz.Data;
using Autoszerviz.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Autoszerviz.Pages.Javitasok;

public class EditModel : PageModel
{
    private readonly AutoszervizDbContext _db;

    public EditModel(AutoszervizDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public JavitasInput Input { get; set; } = new();

    public SelectList UgyfelLista { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var javitas = await _db.Javitasok.FindAsync(id);

        if (javitas is null)
        {
            return NotFound();
        }

        Input = new JavitasInput
        {
            Id = javitas.Id,
            UgyfelId = javitas.UgyfelId,
            Rendszam = javitas.Rendszam,
            Leiras = javitas.Leiras
        };

        await BetoltUgyfelLista();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
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

        var javitas = await _db.Javitasok.FindAsync(id);

        if (javitas is null)
        {
            return NotFound();
        }

        javitas.UgyfelId = Input.UgyfelId;
        javitas.Rendszam = Input.Rendszam.Trim().ToUpperInvariant();
        javitas.Leiras = Input.Leiras;

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
        public int Id { get; set; }

        public int UgyfelId { get; set; }

        [Required(ErrorMessage = "A rendszám megadása kötelező.")]
        [StringLength(10)]
        [RegularExpression(@"^[A-Z]{3}-\d{3}$", ErrorMessage = "A rendszám formátuma ABC-123 legyen.")]
        public string Rendszam { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Leiras { get; set; }
    }
}