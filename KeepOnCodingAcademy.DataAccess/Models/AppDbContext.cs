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
    }
}