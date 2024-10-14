namespace CFRs.Model
{
    public class TAutoReconcileDetail
    {
        public int id { get; set; }
        public int autoReconcileId { get; set; }
        public string status { get; set; } = string.Empty;
        public string remark { get; set; } = string.Empty;
        public int createBy { get; set; }
        public DateTime createDatetime { get; set; }
        public int updateBy { get; set; }
        public DateTime? updateDatetime { get; set; }
        public int year { get; set; }
        public int monthId { get; set; }
        public string bankShortName { get; set; } = string.Empty;

    }
}
