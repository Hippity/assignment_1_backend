using Microsoft.AspNetCore.Mvc;
using MovieBackend.Models;
using MovieBackend.Interaces;

namespace MovieBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MyMoviesController : ControllerBase
    {
        private readonly ILogger<MyMoviesController> _logger;
        private readonly IMyMoviesService _service;

        public MyMoviesController(ILogger<MyMoviesController> logger, IMyMoviesService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMyMovies()
        {
            var movies = await _service.GetAllMoviesAsync();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(long id)
        {
            var movie = await _service.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        [HttpPost]
        public async Task<ActionResult<Movie>> AddMovie(Movie movie)
        {
            var createdMovie = await _service.AddMovieAsync(movie);
            return CreatedAtAction(nameof(GetMovie), new { id = createdMovie.Id }, createdMovie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(long id)
        {
            var success = await _service.DeleteMovieAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
