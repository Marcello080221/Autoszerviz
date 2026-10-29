using System.ComponentModel.DataAnnotations;
using Autoszerviz.Data;
using Autoszerviz.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autoszerviz.Pages.Ugyfelek;

public class CreateModel : PageModel
{
    private readonly AutoszervizDbContext _db;

    public CreateModel(AutoszervizDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public UgyfelInput Input { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var ugyfel = new Ugyfel
        {
            Nev = Input.Nev,
            Telefon = Input.Telefon,
            Email = Input.Email
        };

        _db.Ugyfelek.Add(ugyfel);
        await _db.SaveChangesAsync();

        return RedirectToPage("Index");
    }

    public class UgyfelInput
    {
        [Required(ErrorMessage = "A név megadása kötelező.")]
        [StringLength(100)]
        public string Nev { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Telefon { get; set; }

        [EmailAddress(ErrorMessage = "Adj meg érvényes e-mail-címet.")]
        public string? Email { get; set; }
    }
}