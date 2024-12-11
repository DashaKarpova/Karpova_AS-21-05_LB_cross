namespace Karpova_AS_21_05_LB_cross.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        // Коллекция фильмов в кинотеатре
        public List<Movie> Movies { get; set; } = new List<Movie>();

        // Метод для добавления фильма
        public void AddMovie(Movie movie)
        {
            movie.CinemaId = Id;  // Устанавливаем cinemaId фильма
            Movies.Add(movie);  // Добавляем фильм в список кинотеатра
        }
    }

}
