namespace CFRs.Model
{
    public class VWOPaychqms
    {
        public string bankCode { get; set; } = string.Empty;
        public string bankName { get; set; } = string.Empty;
        public string chequeNo { get; set; } = string.Empty;
        public string chequeDate { get; set; } = string.Empty;
        public string workDate { get; set; } = string.Empty;
        public double chequeAmt { get; set; } 
        public string paymentNo { get; set; } = string.Empty;
        public string chequeDetail { get; set; } = string.Empty;
        public string chequeName { get; set; } = string.Empty;
        public string chequeStatus { get; set; } = string.Empty;
        public string receiveOrCancelDate { get; set; } = string.Empty;
        public string remark1 { get; set; } = string.Empty;
        public string remark2 { get; set; } = string.Empty;
    }
}
