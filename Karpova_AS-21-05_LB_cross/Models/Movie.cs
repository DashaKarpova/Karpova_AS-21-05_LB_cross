using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Karpova_AS_21_05_LB_cross.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int DurationInMinutes { get; set; }

        
        public List<CinemaMovie> CinemaMovies { get; set; } = new List<CinemaMovie>();

        // Бизнес-логика

        // Проверка, длинный ли фильм
        public bool IsLongMovie()
        {
            return DurationInMinutes > 120;
        }
    }

    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int DurationInMinutes { get; set; }
        public List<CinemaMovieDto> CinemaMovies { get; set; } = new List<CinemaMovieDto>();
    }


}
