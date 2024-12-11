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
}
