using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Karpova_AS_21_05_LB_cross.Models
{
    public class CinemaMovie
    {
        public int CinemaId { get; set; }
        [JsonIgnore]
        public Cinema Cinema { get; set; } // Навигационное свойство для Cinema

        public int MovieId { get; set; }
        [JsonIgnore]
        public Movie Movie { get; set; } // Навигационное свойство для Movie
    }

    public class CinemaMovieDto
    {
        public int CinemaId { get; set; }
        public int MovieId { get; set; }
    }

    public class CinemaWithMoviesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<MovieDto> Movies { get; set; }
    }

    public class MovieWithCinemasDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int DurationInMinutes { get; set; }

        [JsonIgnore]
        public List<CinemaDto> Cinemas { get; set; }
    }



}
