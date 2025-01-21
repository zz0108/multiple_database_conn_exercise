using System.ComponentModel.DataAnnotations;

namespace MultipleDatabaseConn.Models
{
    public class Member : IEntity
    {
        public Guid Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; } = null!;
        [MaxLength(50)]
        public string Email { get; set; } = null!;
        public int Age { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        [MaxLength(20)]
        public string? CreatedBy { get; set; }

        public DateTimeOffset? ChangedAt { get; set; }
        [MaxLength(20)]
        public string? ChangedBy { get; set; }
    }
}
