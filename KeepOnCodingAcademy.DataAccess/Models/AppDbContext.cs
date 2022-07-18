using Microsoft.EntityFrameworkCore;

namespace KeepOnCodingAcademy.DataAccess.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionCategory> QuestionCategories { get; set; }
        public DbSet<QuestionDifficulty> QuestionDifficulties { get; set; }

    }
}