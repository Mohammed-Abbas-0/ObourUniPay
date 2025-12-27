using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Obour_Uni_Pay.Models;

namespace Obour_Uni_Pay.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<QueueTurn> QueueTurns { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Student-Department Relationship
            builder.Entity<Student>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Students)
                .HasForeignKey(s => s.DepartmentId);

            // Seed Departments
            builder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "هندسة الحاسبات" },
                new Department { Id = 2, Name = "نظم المعلومات" },
                new Department { Id = 3, Name = "إدارة الأعمال" },
                new Department { Id = 4, Name = "المحاسبة" }
            );
        }
    }
}
