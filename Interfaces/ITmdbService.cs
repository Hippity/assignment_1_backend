using MovieBackend.Models.Api;
using MovieBackend.Models;

namespace MovieBackend.Interaces
{
    public interface ITmdbService
    {
        Task<MovieDetails> GetMovieDetailsAsync(int movieId);
        Task<MovieListResponse> GetPopularMoviesAsync(int page = 1);
        Task<MovieListResponse> SearchMoviesAsync(string query, int page = 1);
        Task<GenreListResponse> GetGenresAsync();
        Task<MovieListResponse> GetMoviesByGenreAsync(int genreId, int page = 1);
        Task<MovieCreditResponse> GetMovieCredits(int movieId);

    }
}
