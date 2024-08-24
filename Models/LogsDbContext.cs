using CustomMiddlewareForLogging.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace CustomMiddlewareForLogging.Models
{
    public class LogsDbContext : DbContext
    {
        public LogsDbContext(DbContextOptions<LogsDbContext> options):base(options) 
        {
            
        }
        public DbSet<LogsModel> Logs { get; set; }
        public DbSet<UserActionLog> Logs_New { get; set; }
    }
}
