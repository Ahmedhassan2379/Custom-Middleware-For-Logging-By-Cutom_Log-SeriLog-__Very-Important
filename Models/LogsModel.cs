namespace CustomMiddlewareForLogging.Models
{
    public class LogsModel
    {
        public int Id { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public DateTime Time { get; set; }
    }
}
