using System.ComponentModel.DataAnnotations;

namespace Autoszerviz.Models;

public class Ugyfel
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nev { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Telefon { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public ICollection<Javitas> Javitasok { get; set; } = new List<Javitas>();
}