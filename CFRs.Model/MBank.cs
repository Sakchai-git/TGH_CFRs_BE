namespace CFRs.Model
{
    public class MBank
    {
        public int bankId { get; set; }
        public string bankFullName { get; set; } = string.Empty;
        public string bankShortName { get; set; } = string.Empty;
        public string bankCode { get; set; } = string.Empty;
        public string bankShortName2 { get; set; } = string.Empty;
        public int isActive { get; set; } = 1;
        public int createBy { get; set; }
        public DateTime createDatetime { get; set; }
        public int updateBy { get; set; }
        public DateTime? updateDatetime { get; set; }
        public string updateDatetimeDisplay { get; set; } = string.Empty;
        public string createByName { get; set; } = string.Empty;
        public string updateByName { get; set; } = string.Empty;
        public int kbankTypeId { get; set; }
    }
}
