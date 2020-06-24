using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;
namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext _context;

        public MoviesController(MovieContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovie()
        {
            List<Movie> movies = new List<Movie>();
            HttpClient client = new HttpClient();
            var result = client.GetAsync("https://data.sfgov.org/resource/yitu-d5am.json").Result;
            if (result.IsSuccessStatusCode)
            {
                movies = result.Content.ReadAsAsync<List<Movie>>().Result;
            }
            return movies;
        }

        // GET: api/Movies/title
        [HttpGet("{title}")]
        public async Task<ActionResult<Movie>> GetMovie(string title)
        {
            IEnumerable<Movie> movies = new List<Movie>();
            HttpClient client = new HttpClient();
            var result = client.GetAsync("https://data.sfgov.org/resource/yitu-d5am.json").Result;
            if (result.IsSuccessStatusCode)
            {
                movies = result.Content.ReadAsAsync<List<Movie>>().Result;
            }

            movies = movies.Where(x => x.Title == title);

            if (movies == null)
            {
                return NotFound();
            }

            return Ok(movies);
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(long id, Movie movie)
        {
            if (id != movie.Id)
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
                if (!MovieExists(id))
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

        // POST: api/Movies
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movie.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Movie>> DeleteMovie(long id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();

            return movie;
        }

        private bool MovieExists(long id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
