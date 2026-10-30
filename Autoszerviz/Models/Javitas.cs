using System.ComponentModel.DataAnnotations;

namespace Autoszerviz.Models;

public class Javitas
{
    public int Id { get; set; }

    [Required]
    public int UgyfelId { get; set; }

    public Ugyfel Ugyfel { get; set; } = null!;

    [Required]
    [StringLength(10)]
    public string Rendszam { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Leiras { get; set; }

    public decimal? Koltseg { get; set; }

    public JavitasAllapot Allapot { get; set; } = JavitasAllapot.Felvéve;

    public DateTime FelvetelDatuma { get; set; } = DateTime.Now;
}