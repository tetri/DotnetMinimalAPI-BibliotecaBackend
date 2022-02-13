using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public class BibliotecaDB : DbContext
{
    public BibliotecaDB(DbContextOptions<BibliotecaDB> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var valueComparer = new ValueComparer<string[]>((c1, c2) => Equals(c1, c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())));

        modelBuilder.Entity<Obra>()
            .Property(e => e.autores)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries))
            .Metadata
            .SetValueComparer(valueComparer);
    }

    public DbSet<Obra> Obras => Set<Obra>();
}


public record Obra
{
    public int id { get; set; }
    [Required]
    public string? titulo { get; set; }
    [Required]
    public string? editora { get; set; }
    public string? foto { get; set; }
    [Required]
    public string[]? autores { get; set; }
}