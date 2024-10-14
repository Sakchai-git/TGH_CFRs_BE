namespace CFRs.Model
{
    public class RBalance
    {
        public string series { get; set; } = string.Empty;
        public string systemCode { get; set; } = string.Empty;
        public string accountSap { get; set; } = string.Empty;
        public string bankCode { get; set; } = string.Empty;
        public string bankName { get; set; } = string.Empty;
        public string branchName { get; set; } = string.Empty;
        public string accountNo { get; set; } = string.Empty;
        public decimal sapDr { get; set; }
        public decimal sapCr { get; set; }
        public decimal glDr { get; set; }
        public decimal glCr { get; set; }
        public decimal diffDr { get; set; }
        public decimal diffCr { get; set; }
    }
}
