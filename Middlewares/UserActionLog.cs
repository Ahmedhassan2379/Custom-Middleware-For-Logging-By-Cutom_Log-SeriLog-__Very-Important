namespace CustomMiddlewareForLogging.Middlewares
{
    public class UserActionLog
    {
        public int Id { get; set; }
        public string? Parameters { get; set; }
        public string? UserIdentity { get; set; }
        public string? Action { get; set; }
        public string? Controller { get; set; }
        public string? ActionName { get; set; }
        public string? IPAddress { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}