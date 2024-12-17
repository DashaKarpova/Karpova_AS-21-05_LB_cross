using Microsoft.AspNetCore.Mvc;
using Karpova_AS_21_05_LB_cross.Data;
using Karpova_AS_21_05_LB_cross.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/reports")] // Изменённый базовый маршрут
public class ReportController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReportController(AppDbContext context)
    {
        _context = context;
    }

    // Получение отчёта по всем фильмам (только для авторизованных пользователей)
    [HttpGet("movies")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Movie>>> GetAllMoviesReport()
    {
        var movies = await _context.Movies.ToListAsync();
        return Ok(movies);
    }

    // Добавление фильма в отчёт (только для администратора)
    [HttpPost("movies/add")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<Movie>> AddMovieToReport(Movie movie)
    {
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAllMoviesReport), new { id = movie.Id }, movie);
    }

    // Получение сводного отчета по жанрам фильмов
    [HttpGet("movies/summary")]
    [Authorize]
    public async Task<ActionResult<object>> GetMoviesSummaryReport()
    {
        var summary = await _context.Movies
            .GroupBy(m => m.Genre)
            .Select(g => new
            {
                Genre = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        return Ok(summary);
    }
}
