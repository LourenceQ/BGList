using Microsoft.EntityFrameworkCore;

namespace MyBgList.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BoardGames_Domains>()
            .HasKey(i => new { i.BoardGameId, i.DomainId });

        modelBuilder.Entity<BoardGames_Mechanics>()
            .HasKey(i => new { i.BoardGameId, i.MechanicId });

    }
}
