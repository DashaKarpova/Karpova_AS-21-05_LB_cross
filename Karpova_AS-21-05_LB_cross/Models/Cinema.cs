using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Karpova_AS_21_05_LB_cross.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

      
        public List<CinemaMovie> CinemaMovies { get; set; } = new List<CinemaMovie>();
    

        // Бизнес-логика

        // Добавить фильм
        public void AddMovie(Movie movie)
        {
            if (!CinemaMovies.Any(cm => cm.MovieId == movie.Id))
            {
                CinemaMovies.Add(new CinemaMovie { CinemaId = Id, MovieId = movie.Id, Movie = movie });
            }
        }
    }

    public class CinemaDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<CinemaMovieDto> CinemaMovies { get; set; } = new List<CinemaMovieDto>();
    }



}
