using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieBackend.Contexts;
using MovieBackend.Interaces;
using MovieBackend.Models;

namespace MovieBackend.Services
{
    public class MyMoviesService : IMyMoviesService
    {
        private readonly MyMovieContext _context;

        public MyMoviesService(MyMovieContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task<Movie> GetMovieByIdAsync(long id)
        {
            return await _context.Movies.FindAsync(id);
        }

        public async Task<Movie> AddMovieAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return movie;
        }

        public async Task<bool> DeleteMovieAsync(long id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return false;
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
