namespace CFRs.Model
{
    public class VWTSourceImportData
    {
        public List<VWTSourceImport> vWTSourceImports { get; set; } = new List<VWTSourceImport>();
        public int total { get; set; }
    }
    public class VWTSourceImport
    {
        public int detailId { get; set; }
        public string systemCode { get; set; } = string.Empty;
        //public int headerId { get; set; }
        //public DateTime colDateWhere { get; set; }
        public string reqnbcde { get; set; } = string.Empty;
        public string cheqno { get; set; } = string.Empty;
        public string capname { get; set; } = string.Empty;
        public string clntnum01 { get; set; } = string.Empty;
        public string reqnno { get; set; } = string.Empty;
        public string reqdate { get; set; } = string.Empty;
        public string payamt { get; set; } = string.Empty;
        public string tjobcode { get; set; } = string.Empty;
        public string workDate { get; set; } = string.Empty;
        public string accSts { get; set; } = string.Empty;
        public string reqnrev { get; set; } = string.Empty;
        public string chdrno01 { get; set; } = string.Empty;
        public string resflag { get; set; } = string.Empty;
        public string userid { get; set; } = string.Empty;
        public string bankCode { get; set; } = string.Empty;
        public string bankName { get; set; } = string.Empty;
        public string bankShortName { get; set; } = string.Empty;
        public string branchCode { get; set; } = string.Empty;
        public string branchName { get; set; } = string.Empty;
        public string acctNo { get; set; } = string.Empty;
        public string modeName { get; set; } = string.Empty;
        public string insertedBy { get; set; } = string.Empty;
        public string keyDate { get; set; } = string.Empty;
        public string payInDate { get; set; } = string.Empty;
        public string effDate { get; set; } = string.Empty;
        public string paidBy { get; set; } = string.Empty;
        public string paidDate { get; set; } = string.Empty;
        public string authenId { get; set; } = string.Empty;
        public string authenDate { get; set; } = string.Empty;
        public string collecCode { get; set; } = string.Empty;
        public string batchNo { get; set; } = string.Empty;
        public string batchId { get; set; } = string.Empty;
        public string glAccountName { get; set; } = string.Empty;
        public DateTime accountingDate { get; set; }
        public string wd { get; set; } = string.Empty;
        public string typeor { get; set; } = string.Empty;
        public string bankareacode { get; set; } = string.Empty;
        public string reqncoy { get; set; } = string.Empty;
    }


}
