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

        public async Task<IEnumerable<MoveListDetails>> GetAllMoviesAsync(string userId = "")
        {
            if (string.IsNullOrEmpty(userId))
            {
                return await _context.Movies.ToListAsync();
            }
            else
            {
                return await _context.Movies
                    .Where(m => EF.Property<string>(m, "UserId") == userId)
                    .ToListAsync();
            }
        }

        public async Task<MoveListDetails> GetMovieByIdAsync(int id, string userId = "")
        {
            if (string.IsNullOrEmpty(userId))
            {
                return await _context.Movies.FindAsync(id);
            }
            else
            {
                return await _context.Movies
                    .Where(m => m.Id == id && EF.Property<string>(m, "UserId") == userId)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<MoveListDetails> AddMovieAsync(MoveListDetails movie, string userId = "")
        {
            // Check if movie is already in watchlist
            var existingMovie = await IsInMyMoviesAsync(movie.Id, userId);
            if (existingMovie)
            {
                return await GetMovieByIdAsync(movie.Id, userId);
            }

            // Set the user ID and date added
            if (!string.IsNullOrEmpty(userId))
            {
                _context.Entry(movie).Property("UserId").CurrentValue = userId;
            }
            
            _context.Entry(movie).Property("DateAdded").CurrentValue = DateTime.UtcNow;
            
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return movie;
        }

        public async Task<bool> DeleteMovieAsync(int id, string userId = "")
        {
            MoveListDetails movie;
            
            if (string.IsNullOrEmpty(userId))
            {
                movie = await _context.Movies.FindAsync(id);
            }
            else
            {
                movie = await _context.Movies
                    .Where(m => m.Id == id && EF.Property<string>(m, "UserId") == userId)
                    .FirstOrDefaultAsync();
            }
            
            if (movie == null)
            {
                return false;
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> IsInMyMoviesAsync(int id, string userId = "")
        {
            if (string.IsNullOrEmpty(userId))
            {
                return await _context.Movies.AnyAsync(m => m.Id == id);
            }
            else
            {
                return await _context.Movies
                    .AnyAsync(m => m.Id == id && EF.Property<string>(m, "UserId") == userId);
            }
        }
    }
}