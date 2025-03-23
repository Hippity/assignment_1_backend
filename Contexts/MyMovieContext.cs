using Microsoft.EntityFrameworkCore;
using MovieBackend.Models;
namespace MovieBackend.Contexts;

public class MyMovieContext : DbContext
{
    public MyMovieContext(DbContextOptions<MyMovieContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; } = null!;
}