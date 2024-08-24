
using CustomMiddlewareForLogging.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomMiddlewareForLogging.Middlewares
{
    public class UserActionLogger : IUserActionLogger
    {
        private readonly LogsDbContext _logsDbContext;
        public UserActionLogger(LogsDbContext logsDbContext)
        {
            _logsDbContext = logsDbContext;
        }
        public async Task Log(UserActionLog log)
        {
            _logsDbContext.Logs_New.Add(log);
            await _logsDbContext.SaveChangesAsync();
        }
    }
}
