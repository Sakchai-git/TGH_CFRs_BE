namespace CFRs.Model
{
    public class TAutoReconcile
    {
        public int id { get; set; }
        public int year { get; set; }
        public int monthId { get; set; }
        public int bankId { get; set; }
        public string status { get; set; } = string.Empty;
        public string remark { get; set; } = string.Empty;
        public int createBy { get; set; }
        public DateTime createDatetime { get; set; }
        public int updateBy { get; set; }
        public DateTime? updateDatetime { get; set; }
        public int historyCount { get; set; }
        public string bankShortName { get; set; } = string.Empty;
        public int rowNumber { get; set; }
        public int kbankTypeId { get; set; }
        public string createByName { get; set; } = string.Empty;
        public string updateByName { get; set; } = string.Empty;
    }

   
    public class AutoReconcileStatus
    {
        private AutoReconcileStatus(string value) { Value = value; }

        public string Value { get; private set; }

        public static AutoReconcileStatus NoRun { get { return new AutoReconcileStatus("No Run"); } }
        public static AutoReconcileStatus InProgress { get { return new AutoReconcileStatus("In progress"); } }
        public static AutoReconcileStatus Completed { get { return new AutoReconcileStatus("Completed"); } }
        public static AutoReconcileStatus Fail { get { return new AutoReconcileStatus("Fail"); } }
    }
}
