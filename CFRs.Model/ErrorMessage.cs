namespace CFRs.Model
{
    public class ErrorMessage
    {
        public int statusCode { get; set; }
        public string message { get; set; } = string.Empty;
        public string? stackTrace { get; set; }
        public string? source { get; set; }
        
    }
}
