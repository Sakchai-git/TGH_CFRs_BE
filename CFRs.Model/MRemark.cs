namespace CFRs.Model
{
    public class MRemark
    {
        public int id { get; set; }
        public string remcode { get; set; } = string.Empty;
        public string remdesc { get; set; } = string.Empty;
        public int isActive { get; set; } = 1;
        public int createBy { get; set; }
        public DateTime createDatetime { get; set; }
        public int updateBy { get; set; }
        public DateTime? updateDatetime { get; set; }
        public string createByName { get; set; } = string.Empty;
        public string updateByName { get; set; } = string.Empty;
    }
}
