using Karpova_AS_21_05_LB_cross.Data;
using Karpova_AS_21_05_LB_cross.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<CinemaDto>>> GetCinemas()
        {
            if (_context.Cinemas == null)
            {
                return NotFound();
            }

            // Загружаем все кинотеатры с фильмами
            var cinemas = await _context.Cinemas
                .Include(c => c.CinemaMovies)  // Включаем связанные CinemaMovies
                .ThenInclude(cm => cm.Movie)   // Включаем Movie, связанные с CinemaMovies
                .ToListAsync();

            // Преобразуем Cinema в CinemaDto
            var cinemaDtos = cinemas.Select(cinema => new CinemaDto
            {
                Id = cinema.Id,
                Name = cinema.Name,
                Address = cinema.Address,
                CinemaMovies = cinema.CinemaMovies
                    .Where(cm => cm.Movie != null)  // Проверяем, что Movie существует
                    .Select(cm => new CinemaMovieDto
                    {
                        CinemaId = cm.CinemaId,
                        MovieId = cm.MovieId,
                    }).ToList(),  // Преобразуем все связанные CinemaMovies в список CinemaMovieDto
            }).ToList();

            return cinemaDtos;
        }

        // GET: api/Cinemas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CinemaDto>> GetCinema(int id)
        {
            if (_context.Cinemas == null)
            {
                return NotFound();
            }

            var cinema = await _context.Cinemas
                .Include(c => c.CinemaMovies)
                .ThenInclude(cm => cm.Movie)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cinema == null)
            {
                return NotFound();
            }

            var cinemaDto = new CinemaDto
            {
                Id = cinema.Id,
                Name = cinema.Name,
                Address = cinema.Address,
                // Используем CinemaMovies вместо Movies
                CinemaMovies = cinema.CinemaMovies
                    .Where(cm => cm.Movie != null)
                    .Select(cm => new CinemaMovieDto
                    {
                        CinemaId = cm.CinemaId,
                        MovieId = cm.MovieId
                    })
                    .ToList()
            };

            return cinemaDto;
        }


        // PUT: api/Cinemas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCinema(int id, CinemaDto cinemaDto)
        {
            if (id != cinemaDto.Id)
            {
                return BadRequest();
            }

            var cinema = await _context.Cinemas
                .Include(c => c.CinemaMovies)  // Загружаем текущие связи с фильмами
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cinema == null)
            {
                return NotFound();
            }

            // Обновляем данные кинотеатра
            cinema.Name = cinemaDto.Name;
            cinema.Address = cinemaDto.Address;

            // Обновляем связи CinemaMovies
            // Очищаем текущие связи, если они существуют
            cinema.CinemaMovies.Clear();

            // Добавляем новые связи
            foreach (var cinemaMovieDto in cinemaDto.CinemaMovies)
            {
                var cinemaMovie = new CinemaMovie
                {
                    CinemaId = cinema.Id,
                    MovieId = cinemaMovieDto.MovieId
                };

                cinema.CinemaMovies.Add(cinemaMovie);
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
        public async Task<ActionResult<CinemaDto>> PostCinema(CinemaDto cinemaDto)
        {
            // Создаем новый объект Cinema на основе CinemaDto
            var cinema = new Cinema
            {
                Name = cinemaDto.Name,
                Address = cinemaDto.Address
            };

            // Добавляем новый Cinema в контекст
            _context.Cinemas.Add(cinema);
            await _context.SaveChangesAsync();  // Сохраняем изменения, чтобы получить Id кинотеатра

            // Если есть связь с фильмами, добавляем их в промежуточную таблицу CinemaMovies
            if (cinemaDto.CinemaMovies != null && cinemaDto.CinemaMovies.Any())
            {
                foreach (var cinemaMovieDto in cinemaDto.CinemaMovies)
                {
                    // Создаем новую связь между кинотеатром и фильмом
                    var cinemaMovie = new CinemaMovie
                    {
                        CinemaId = cinema.Id, // ID созданного кинотеатра
                        MovieId = cinemaMovieDto.MovieId // ID фильма из переданных данных
                    };

                    // Добавляем связь в список CinemaMovies
                    cinema.CinemaMovies.Add(cinemaMovie);
                }

                // Сохраняем изменения, чтобы обновить CinemaMovies в базе данных
                await _context.SaveChangesAsync();  // Сохраняем изменения в промежуточной таблице
            }

            // Возвращаем новый объект CinemaDto с обновленными данными
            var newCinemaDto = new CinemaDto
            {
                Id = cinema.Id,
                Name = cinema.Name,
                Address = cinema.Address,
                CinemaMovies = cinemaDto.CinemaMovies // Возвращаем переданные данные о связях
            };

            return CreatedAtAction("GetCinema", new { id = cinema.Id }, newCinemaDto);
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
    }
}

