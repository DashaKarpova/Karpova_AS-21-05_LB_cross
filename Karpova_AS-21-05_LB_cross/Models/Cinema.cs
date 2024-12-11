namespace Karpova_AS_21_05_LB_cross.Models
{
    public class Cinema
    {
        public int Id { get; set; } // Уникальный идентификатор
        public string Name { get; set; } // Название кинотеатра
        public string Location { get; set; } // Адрес
        public List<Movie> Movies { get; set; } = new(); // Список фильмов, показываемых в кинотеатре

        // Бизнес-логика: добавить товар продавцу
        public void AddMovie(Movie movie)
        {
            movie.CinemaId = Id;
            Movies.Add(movie);
        }
    }
}
