namespace CFRs.Model
{
    public class RBAACTransfer
    {
        public string bankCode { get; set; } = string.Empty;
        public string accountSap { get; set; } = string.Empty;
        public string accountNo { get; set; } = string.Empty;
        public string branchName { get; set; } = string.Empty;
        public string transactionCode { get; set; } = string.Empty;
        public decimal? sumAmount { get; set; }
    }
}
