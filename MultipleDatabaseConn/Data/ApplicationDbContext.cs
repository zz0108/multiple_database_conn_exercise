using Microsoft.EntityFrameworkCore;
using MultipleDatabaseConn.Models;

namespace MultipleDatabaseConn.Data
{
    public class ApplicationDbContext : DynamicDbContext
    {
        private readonly string _connectionString;

        // 加入這個建構函式來支援 pooling
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // 原本的建構函式
        public ApplicationDbContext(string connectionString)
            : base()
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)  // 只有在沒有配置時才設定連線字串
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
