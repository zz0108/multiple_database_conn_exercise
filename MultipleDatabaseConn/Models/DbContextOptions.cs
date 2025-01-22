namespace MultipleDatabaseConn.Models
{
    public class DbContextOptions
    {
        public int MaxPoolSize { get; set; }
        public int MinPoolSize { get; set; }
        public int CommandTimeout { get; set; }
        public int PoolSize { get; set; }
    }
}
