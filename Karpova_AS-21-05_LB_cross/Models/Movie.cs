namespace Karpova_AS_21_05_LB_cross.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int DurationMinutes { get; set; }

        // Связь с кинотеатром через CinemaId
        public int CinemaId { get; set; }
    }

}
