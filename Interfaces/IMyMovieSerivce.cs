using System.Collections.Generic;
using System.Threading.Tasks;
using MovieBackend.Models;

namespace MovieBackend.Interaces
{
    public interface IMyMoviesService
    {
        Task<IEnumerable<MoveListDetails>> GetAllMoviesAsync(string userId = "");
        Task<MoveListDetails> GetMovieByIdAsync(int id, string userId = "");
        Task<MoveListDetails> AddMovieAsync(MoveListDetails movie, string userId = "");
        Task<bool> DeleteMovieAsync(int id, string userId = "");
        Task<bool> IsInMyMoviesAsync(int id, string userId = "");
    }
}