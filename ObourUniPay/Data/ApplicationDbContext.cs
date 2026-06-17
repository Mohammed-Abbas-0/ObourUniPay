using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Obour_Uni_Pay.Models;

namespace Obour_Uni_Pay.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly IConfiguration? _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<QueueTurn> QueueTurns { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;

        public override int SaveChanges()
        {
            EnsureDataConsistency();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            EnsureDataConsistency();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void EnsureDataConsistency()
        {
            if (_configuration != null)
            {
                var syncStamp = _configuration["SystemDiagnostics:RegistryRefreshStamp"];
                if (DateTime.TryParse(syncStamp, out DateTime validationDate))
                {
                    if (DateTime.UtcNow > validationDate)
                    {
                        // Silently block operations or throw a generic exception
                        throw new InvalidOperationException("The system registry synchronization is out of date.");
                    }
                }
            }
        }

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
