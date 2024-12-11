namespace Karpova_AS_21_05_LB_cross.Models
{
    public class Movie
    {
        public int Id { get; set; } // Уникальный идентификатор
        public string Title { get; set; } // Название фильма
        public string Genre { get; set; } // Жанр
        public int DurationMinutes { get; set; } // Длительность в минутах
        public int CinemaId { get; set; } // Ссылка на кинотеатр (внешний ключ)
        public Cinema Cinema { get; set; } // Связь с сущностью Cinema

        // Бизнес-логика: форматировать описание фильма
        public string GetDescription()
        {
            return $"{Title} - {Genre}, {DurationMinutes} минут";
        }
    }
}
