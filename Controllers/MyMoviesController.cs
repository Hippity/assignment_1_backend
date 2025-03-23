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
        public async Task<ActionResult<IEnumerable<MoveListDetails>>> GetMyMovies()
        {
            string userId = "";
            
            var movies = await _service.GetAllMoviesAsync(userId);
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MoveListDetails>> GetMovie(int id)
        {
            string userId = "";
            
            var movie = await _service.GetMovieByIdAsync(id, userId);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }
        
        [HttpGet("check/{id}")]
        public async Task<ActionResult<bool>> CheckMovie(int id)
        {
            string userId = "";
            
            var isInWatchlist = await _service.IsInMyMoviesAsync(id, userId);
            return Ok(isInWatchlist);
        }

        [HttpPost]
        public async Task<ActionResult<MoveListDetails>> AddMovie(MoveListDetails movie)
        {
            string userId = "";
            
            var addedMovie = await _service.AddMovieAsync(movie, userId);
            return CreatedAtAction(nameof(GetMovie), new { id = addedMovie.Id }, addedMovie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            string userId = "";
            
            var success = await _service.DeleteMovieAsync(id, userId);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}