namespace CFRs.Model
{
    public class MMenu
    {
        public int id { get; set; }
        public int rowOrder { get; set; }
        public string code { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public int parentMenuId { get; set; }
        public string url { get; set; } = string.Empty;
        public string cssClass { get; set; } = string.Empty;
        public string imageUrl { get; set; } = string.Empty;
        public int createBy { get; set; }
        public DateTime createDatetime { get; set; }
        public int updateBy { get; set; }
        public DateTime? updateDatetime { get; set; }
        public int isActive { get; set; } = 1;
    }
}
