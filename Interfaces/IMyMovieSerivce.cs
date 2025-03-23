using System.Collections.Generic;
using System.Threading.Tasks;
using MovieBackend.Models;

namespace MovieBackend.Interaces
{
    public interface IMyMoviesService
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task<Movie> GetMovieByIdAsync(long id);
        Task<Movie> AddMovieAsync(Movie movie);
        Task<bool> DeleteMovieAsync(long id);
    }
}
