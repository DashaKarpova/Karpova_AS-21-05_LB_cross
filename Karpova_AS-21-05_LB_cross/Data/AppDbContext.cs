using Microsoft.EntityFrameworkCore;
using Karpova_AS_21_05_LB_cross.Models;

namespace Karpova_AS_21_05_LB_cross.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<CinemaMovie> CinemaMovies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Указываем составной ключ для CinemaMovie
            modelBuilder.Entity<CinemaMovie>()
                .HasKey(cm => new { cm.CinemaId, cm.MovieId });

            // Настраиваем связи
            modelBuilder.Entity<CinemaMovie>()
                .HasOne(cm => cm.Cinema)
                .WithMany(c => c.CinemaMovies)
                .HasForeignKey(cm => cm.CinemaId);

            modelBuilder.Entity<CinemaMovie>()
                .HasOne(cm => cm.Movie)
                .WithMany(m => m.CinemaMovies)
                .HasForeignKey(cm => cm.MovieId);
        }
    }
}
