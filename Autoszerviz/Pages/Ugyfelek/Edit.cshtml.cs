using System.ComponentModel.DataAnnotations;
using Autoszerviz.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autoszerviz.Pages.Ugyfelek;

public class EditModel : PageModel
{
    private readonly AutoszervizDbContext _db;

    public EditModel(AutoszervizDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public UgyfelInput Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var ugyfel = await _db.Ugyfelek.FindAsync(id);

        if (ugyfel is null)
        {
            return NotFound();
        }

        Input = new UgyfelInput
        {
            Id = ugyfel.Id,
            Nev = ugyfel.Nev,
            Telefon = ugyfel.Telefon,
            Email = ugyfel.Email
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var ugyfel = await _db.Ugyfelek.FindAsync(id);

        if (ugyfel is null)
        {
            return NotFound();
        }

        ugyfel.Nev = Input.Nev;
        ugyfel.Telefon = Input.Telefon;
        ugyfel.Email = Input.Email;

        await _db.SaveChangesAsync();

        return RedirectToPage("Index");
    }

    public class UgyfelInput
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A név megadása kötelező.")]
        [StringLength(100)]
        public string Nev { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Telefon { get; set; }

        [EmailAddress(ErrorMessage = "Adj meg érvényes e-mail-címet.")]
        public string? Email { get; set; }
    }
}