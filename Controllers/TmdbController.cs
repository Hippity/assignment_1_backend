using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieBackend.Interaces;
using System.Threading.Tasks;

namespace MovieBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TmdbController : ControllerBase
    {
        private readonly ILogger<TmdbController> _logger;
        private readonly ITmdbService _tmdbService;

        public TmdbController(ILogger<TmdbController> logger, ITmdbService tmdbService)
        {
            _logger = logger;
            _tmdbService = tmdbService;
        }

        [HttpGet("movie/{id}")]
        public async Task<IActionResult> GetMovie(int id)
        {
            var movieDetailsJson = await _tmdbService.GetMovieDetailsAsync(id);
            return Ok(movieDetailsJson);
        }

        [HttpGet("movies/popular/{page}")]
        public async Task<IActionResult> GetPopularMovies(int page)
        {
            var movieDetailsJson = await _tmdbService.GetPopularMoviesAsync(page);
            return Ok(movieDetailsJson);
        }

        [HttpGet("movies/search/{query}&{page}")]
        public async Task<IActionResult> SearchMovies(string query, int page = 1)
        {
            var searchResults = await _tmdbService.SearchMoviesAsync(query, page);
            return Ok(searchResults);
        }

        [HttpGet("genres")]
        public async Task<IActionResult> GetGenres()
        {
            var genres = await _tmdbService.GetGenresAsync();
            return Ok(genres);
        }

        [HttpGet("movies/genre/{genreId}&{page}")]
        public async Task<IActionResult> GetMoviesByGenre(int genreId, int page = 1)
        {
            var moviesByGenre = await _tmdbService.GetMoviesByGenreAsync(genreId, page);
            return Ok(moviesByGenre);
        }

        [HttpGet("movie/{movieId}/credits")]
        public async Task<IActionResult> GetMovieCredits(int movieId)
        {
            var movieCredits = await _tmdbService.GetMovieCredits(movieId);
            return Ok(movieCredits);
        }

    }
}
