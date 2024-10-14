namespace CFRs.Model
{
    public class RKTBTransferOut
    {
        public int orderNo { get; set; } 
        public string bankCode { get; set; } = string.Empty;
        public string accountSap { get; set; } = string.Empty;
        public string branchName { get; set; } = string.Empty;
        public decimal credit { get; set; }
        public decimal debit { get; set; } 
    }
}
