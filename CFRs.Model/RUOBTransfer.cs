namespace CFRs.Model
{
    public class RUOBTransfer
    {
        public string valueDate { get; set; } = string.Empty;
        public string transactionDate { get; set; } = string.Empty;
        public string transactionTime { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string withdrawal { get; set; } = string.Empty;
        public decimal? adjAmount { get; set; }
        public decimal? balance { get; set; } 
        public string remark { get; set; } = string.Empty;
    }
}
