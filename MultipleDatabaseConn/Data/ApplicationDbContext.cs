using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;
using MultipleDatabaseConn.Models;

namespace MultipleDatabaseConn.Data
{
    public class ApplicationDbContext : DynamicDbContext
    {
        private readonly string? _connectionString;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext(string connectionString)
            : base()
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_connectionString);
            }
        }

        public DbSet<Member> Members { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(e => e.Id).HasName($"member_pk");
                entity.ToTable($"member");
                entity.Property(e => e.Id)
                    .IsUnicode(false);
                entity.Property(e => e.ChangedBy).HasMaxLength(20);
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Name).HasMaxLength(20);
            });
        }
    }

}
