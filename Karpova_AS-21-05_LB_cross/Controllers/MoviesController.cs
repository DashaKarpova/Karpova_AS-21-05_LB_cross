using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Karpova_AS_21_05_LB_cross.Data;
using Karpova_AS_21_05_LB_cross.Models;

namespace Karpova_AS_21_05_LB_cross.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MoviesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var movies = await _context.Movies
                .Include(m => m.CinemaMovies)    // Загружаем связанные CinemaMovies
                .ThenInclude(cm => cm.Cinema)    // Загружаем связанные Cinema для каждого CinemaMovie
                .ToListAsync();

            var movieDtos = movies.Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Genre = m.Genre,
                DurationInMinutes = m.DurationInMinutes,
                CinemaMovies = m.CinemaMovies
                    .Where(cm => cm.Cinema != null)  // Проверяем, что Cinema существует
                    .Select(cm => new CinemaMovieDto
                    {
                        CinemaId = cm.CinemaId,
                        MovieId = cm.MovieId
                    }).ToList()
            }).ToList();

            return movieDtos;
        }


// GET: api/Movies/5
[HttpGet("{id}")]
public async Task<ActionResult<MovieDto>> GetMovie(int id)
{
    var movie = await _context.Movies
        .Include(m => m.CinemaMovies)    // Загружаем связанные CinemaMovies
        .ThenInclude(cm => cm.Cinema)    // Загружаем связанные Cinema для каждого CinemaMovie
        .FirstOrDefaultAsync(m => m.Id == id);   // Используем FirstOrDefaultAsync для поиска по id

    if (movie == null)
    {
        return NotFound();
    }

    var movieDto = new MovieDto
    {
        Id = movie.Id,
        Title = movie.Title,
        Genre = movie.Genre,
        DurationInMinutes = movie.DurationInMinutes,
        CinemaMovies = movie.CinemaMovies
            .Where(cm => cm.Cinema != null)  // Проверяем, что Cinema существует
            .Select(cm => new CinemaMovieDto
            {
                CinemaId = cm.CinemaId,
                MovieId = cm.MovieId
            }).ToList()
    };

    return movieDto;
}


        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<MovieDto>> PostMovie(MovieDto movieDto)
        {
            // Создаем новый объект Movie на основе MovieDto
            var movie = new Movie
            {
                Title = movieDto.Title,
                Genre = movieDto.Genre,
                DurationInMinutes = movieDto.DurationInMinutes
            };

            // Добавляем новый Movie в контекст
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();  // Сохраняем изменения, чтобы получить Id фильма

            // Если есть связь с кинотеатрами, добавляем их в промежуточную таблицу CinemaMovies
            if (movieDto.CinemaMovies != null && movieDto.CinemaMovies.Any())
            {
                foreach (var cinemaMovieDto in movieDto.CinemaMovies)
                {
                    // Создаем новую связь между фильмом и кинотеатром
                    var cinemaMovie = new CinemaMovie
                    {
                        CinemaId = cinemaMovieDto.CinemaId, // ID кинотеатра из переданных данных
                        MovieId = movie.Id // ID созданного фильма
                    };

                    // Добавляем связь в промежуточную таблицу CinemaMovies
                    _context.CinemaMovies.Add(cinemaMovie);
                }

                // Сохраняем изменения в базе данных
                await _context.SaveChangesAsync();  // Сохраняем связи в промежуточной таблице
            }

            // Возвращаем новый объект MovieDto с обновленными данными
            var newMovieDto = new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Genre = movie.Genre,
                DurationInMinutes = movie.DurationInMinutes,
                CinemaMovies = movieDto.CinemaMovies // Возвращаем переданные данные о связях
            };

            return CreatedAtAction("GetMovie", new { id = movie.Id }, newMovieDto);
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieDto movieDto)
        {
            if (id != movieDto.Id)
            {
                return BadRequest();
            }

            var movie = await _context.Movies
                .Include(m => m.CinemaMovies)  // Загружаем текущие связи с CinemaMovies
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            // Обновляем данные фильма
            movie.Title = movieDto.Title;
            movie.Genre = movieDto.Genre;
            movie.DurationInMinutes = movieDto.DurationInMinutes;

            // Обновляем связи CinemaMovies
            // Очищаем текущие связи, если они существуют
            movie.CinemaMovies.Clear();

            // Добавляем новые связи, если они есть в DTO
            foreach (var cinemaMovieDto in movieDto.CinemaMovies)
            {
                var cinemaMovie = new CinemaMovie
                {
                    MovieId = movie.Id,
                    CinemaId = cinemaMovieDto.CinemaId
                };

                movie.CinemaMovies.Add(cinemaMovie);
            }

            _context.Entry(movie).State = EntityState.Modified;

            return NoContent();
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
