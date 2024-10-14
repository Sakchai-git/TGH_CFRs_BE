namespace CFRs.Model
{
    public class RKBANKMediaClearing
    {
        public string bankCode { get; set; } = string.Empty;
        public string accountSap { get; set; } = string.Empty;
        public string accountNo { get; set; } = string.Empty;
        public string branchName { get; set; } = string.Empty;
        public string effectiveDate { get; set; } = string.Empty;
        public decimal? adjAmount { get; set; }
    }
}
