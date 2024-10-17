using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PM.SQLite
{
    public class SQLiteDbContext : PM.DbContext.BaseDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("FileName=C:/Users/Alex/Staff.db");
        }
    }
}
