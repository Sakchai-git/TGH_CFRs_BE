namespace CFRs.Model
{
    public class VWTSourceImportFilter
    {

        public int year { get; set; }
        public int monthId { get; set; }
        public string systemCode { get; set; } = string.Empty;
        public string bankShortName { get; set; } = string.Empty;
        public string startDate { get; set; } = string.Empty;
        public string endDate { get; set; } = string.Empty;

        public int skip { get; set; }
        public int take { get; set; }

        public int isExport { get; set; }
    }
}
