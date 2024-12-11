using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Karpova_AS_21_05_LB_cross.Data;
using Karpova_AS_21_05_LB_cross.Models;

namespace Karpova_AS_21_05_LB_cross.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CinemasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Cinemas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cinema>>> GetCinemas()
        {
            if (_context.Cinemas == null)
            {
                return NotFound();
            }
            return await _context.Cinemas.ToListAsync();
        }

        // GET: api/Cinemas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cinema>> GetCinema(int id)
        {
            if (_context.Cinemas == null)
            {
                return NotFound();
            }
            var cinema = await _context.Cinemas.FindAsync(id);

            if (cinema == null)
            {
                return NotFound();
            }

            return cinema;
        }


        // PUT: api/Cinemas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCinema(int id, Cinema cinema)
        {
            if (id != cinema.Id)
            {
                return BadRequest();
            }

            _context.Entry(cinema).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CinemaExists(id))
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


        // POST: api/Cinemas
        [HttpPost]
        public async Task<ActionResult<Cinema>> PostCinema(Cinema cinema)
        {
            if (_context.Cinemas == null)
            {
                return Problem("Entity set 'AppDbContext.Cinemas'  is null.");
            }
            _context.Cinemas.Add(cinema);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCinema", new { id = cinema.Id }, cinema);
        }

      
        // DELETE: api/Cinemas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCinema(int id)
        {
            if (_context.Cinemas == null)
            {
                return NotFound();
            }
            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema == null)
            {
                return NotFound();
            }

            _context.Cinemas.Remove(cinema);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CinemaExists(int id)
        {
            return (_context.Cinemas?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // GET: api/Cinemas/WithMovieCount/3
        [HttpGet("WithMovieCount/{minMovieCount}")]
        public async Task<ActionResult<IEnumerable<Cinema>>> GetCinemasWithMovieCount(int minMovieCount)
        {
            var cinemas = await _context.Cinemas
                .Where(c => _context.Movies.Count(m => m.CinemaId == c.Id) >= minMovieCount)
                .ToListAsync();

            if (!cinemas.Any())
            {
                return NotFound();
            }

            return cinemas;
        }

        // GET: api/Cinemas/ByMovie/5
        [HttpGet("ByMovie/{movieId}")]
        public async Task<ActionResult<IEnumerable<Cinema>>> GetCinemasByMovie(int movieId)
        {
            var cinemas = await _context.Cinemas
                .Where(c => _context.Movies.Any(m => m.CinemaId == c.Id && m.Id == movieId))
                .ToListAsync();

            if (!cinemas.Any())
            {
                return NotFound(new { Message = "Кинотеатры с указанным фильмом не найдены." });
            }

            return Ok(cinemas);
        }


    }
}