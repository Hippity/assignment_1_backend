using Microsoft.EntityFrameworkCore;
using MovieBackend.Models;
namespace MovieBackend.Contexts;

public class MyMovieContext : DbContext
{
    public MyMovieContext(DbContextOptions<MyMovieContext> options)
        : base(options)
    {
    }

    public DbSet<MoveListDetails> Movies { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<MoveListDetails>()
            .HasKey(m => m.Id);
            
        modelBuilder.Entity<MoveListDetails>()
            .Property<string>("UserId")
            .IsRequired(false);
            
        modelBuilder.Entity<MoveListDetails>()
            .Property<DateTime>("DateAdded")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}