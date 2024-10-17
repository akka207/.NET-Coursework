using Microsoft.EntityFrameworkCore;

namespace PM.PostgreSQL
{
    public class PostgresDbContext : PM.DbContext.BaseDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("Host=40.69.56.108;UserId=postgres;Password=password;Database=personalmanager;Port=5432");
        }
    }
}