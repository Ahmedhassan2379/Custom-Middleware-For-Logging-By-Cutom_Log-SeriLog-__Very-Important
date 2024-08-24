using CustomMiddlewareForLogging.Models;

namespace CustomMiddlewareForLogging.Middlewares
{
    public interface IUserActionLogger
    {
        Task Log(UserActionLog log);
    }
}