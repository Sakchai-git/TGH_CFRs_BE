namespace CFRs.Model
{
    public class TRunBatch
    {
        public int id { get; set; }
        public int year { get; set; }
        public int monthId { get; set; }
        public int systemId { get; set; }
        public string status { get; set; } = string.Empty;
        public string remark { get; set; } = string.Empty;
        public int createBy { get; set; }
        public DateTime createDatetime { get; set; }
        public int updateBy { get; set; }
        public DateTime? updateDatetime { get; set; }
        public int historyCount { get; set; }
        public string systemName { get; set; } = string.Empty;
        public string monthName { get; set; } = string.Empty;
        public int rowNumber { get; set; }
    }

    public class RunBatchStatus
    {
        private RunBatchStatus(string value) { Value = value; }

        public string Value { get; private set; }

        public static RunBatchStatus NoRun { get { return new RunBatchStatus("No Run"); } }
        public static RunBatchStatus InProgress { get { return new RunBatchStatus("In progress"); } }
        public static RunBatchStatus Completed { get { return new RunBatchStatus("Completed"); } }
        public static RunBatchStatus Fail { get { return new RunBatchStatus("Fail"); } }
    }
}
