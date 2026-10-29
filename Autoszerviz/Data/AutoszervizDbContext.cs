using Autoszerviz.Models;
using Microsoft.EntityFrameworkCore;

namespace Autoszerviz.Data;

public class AutoszervizDbContext : DbContext
{
    public AutoszervizDbContext(DbContextOptions<AutoszervizDbContext> options)
        : base(options)
    {
    }

    public DbSet<Ugyfel> Ugyfelek => Set<Ugyfel>();
    public DbSet<Javitas> Javitasok => Set<Javitas>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Javitas>()
            .HasOne(j => j.Ugyfel)
            .WithMany(u => u.Javitasok)
            .HasForeignKey(j => j.UgyfelId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}