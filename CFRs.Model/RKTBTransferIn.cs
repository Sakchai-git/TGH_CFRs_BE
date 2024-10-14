namespace CFRs.Model
{
    public class RKTBTransfer
    {
        public List<RKTBTransferIn> KTBTransferIn { get; set; } = new List<RKTBTransferIn>();
        public List<RKTBTransferOut> KTBTransferOut { get; set; } = new List<RKTBTransferOut>();

    }
    public class RKTBTransferIn
    {
        public int orderNo { get; set; } 
        public string bankCode { get; set; } = string.Empty;
        public string accountSap { get; set; } = string.Empty;
        public string branchName { get; set; } = string.Empty;
        public decimal debit { get; set; } 
        public decimal credit { get; set; }
    }
}
