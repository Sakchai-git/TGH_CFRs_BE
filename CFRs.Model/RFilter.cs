namespace CFRs.Model
{
    public class RFilter
    {
        public int year { get; set; }
        public int monthId { get; set; }
        public int bankId { get; set; }
        public string bankShortName { get; set; } = string.Empty;
        public int systemId { get; set; }
        public string reportType { get; set; } = string.Empty;
    }
}
