using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Movie_Ticketing.Models;


namespace Movie_Ticketing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors()]

    public class MovieController : Controller

    {
        private readonly IConfiguration _config;
        private readonly UserContext _context;
        public MovieController(IConfiguration config, UserContext context)
        {
            _config = config;
            _context = context;
        }
        [HttpPost("CreateMovie")]
        public IActionResult Create(Movie movie)
        {
            if (_context.Movies.Where(u => u.MovieID == movie.MovieID).FirstOrDefault() != null)
            {
                return Ok("AlreadyExist");
            }
            //movie.ReleaseDate = DateTime.Now;
            _context.Movies.Add(movie);
            _context.SaveChanges();
            return Ok("Success");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.MovieID)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllMovies()
        {
            var movies = await _context.Movies.ToListAsync();

            if (movies == null || movies.Count == 0)
            {
                return NotFound("No movies found in the database.");
            }

            return movies;
        }

        private bool MovieExist(int id)
        {
            return (_context.Movies?.Any(e => e.MovieID == id)).GetValueOrDefault();
        }

    }
}
