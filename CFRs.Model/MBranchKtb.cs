namespace CFRs.Model
{
    public class MBranchKtb
    {
        public int branchKtbId { get; set; }
        public string bankCode { get; set; } = string.Empty;
        public string bankName { get; set; } = string.Empty;
        public string branchName { get; set; } = string.Empty;
        public string accountNo { get; set; } = string.Empty;
        public string accountSap { get; set; } = string.Empty;
        public int isActive { get; set; } = 1;
        public int createBy { get; set; }
        public DateTime createDatetime { get; set; }
        public int updateBy { get; set; }
        public DateTime? updateDatetime { get; set; }
        public string updateDatetimeDisplay { get; set; } = string.Empty;
        public string createByName { get; set; } = string.Empty;
        public string updateByName { get; set; } = string.Empty;
    }
}
