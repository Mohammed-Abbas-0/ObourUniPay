using Microsoft.EntityFrameworkCore;
using Obour_Uni_Pay.Models;

namespace Obour_Uni_Pay.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<QueueTurn> QueueTurns { get; set; }
    }
}
