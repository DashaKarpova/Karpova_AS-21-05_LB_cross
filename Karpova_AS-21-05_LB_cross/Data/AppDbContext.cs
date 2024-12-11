using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Karpova_AS_21_05_LB_cross.Models;

namespace Karpova_AS_21_05_LB_cross.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Таблицы в базе данных
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Movie> Movies { get; set; }
    }
}
