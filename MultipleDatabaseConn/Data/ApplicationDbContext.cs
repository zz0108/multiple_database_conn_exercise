using Microsoft.EntityFrameworkCore;
using MultipleDatabaseConn.Models;

namespace MultipleDatabaseConn.Data
{
    public class ApplicationDbContext(string connectionString) : DynamicDbContext
    {
        public string ConnectionString { get; } = connectionString;
        public DbSet<Member> Members { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);
        }

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
    };

}
