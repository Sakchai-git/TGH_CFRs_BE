namespace CFRs.Model
{
    public class TStatementImport
    {
        public int statementImportId { get; set; }
        public int bankId { get; set; }
        public int monthId { get; set; }
        public int year { get; set; }
        public string pathLocal { get; set; } = string.Empty;
        public string pathS3 { get; set; } = string.Empty;
        public int isActive { get; set; }
        public string rowHeader { get; set; } = string.Empty;
        public string rowFooter { get; set; } = string.Empty;
        public int importBy { get; set; }
        public DateTime importDatetime { get; set; }
        public string importName { get; set; } = string.Empty;
    }
}
