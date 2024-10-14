namespace CFRs.Model
{
    public class MPermisson
    {
        public int id { get; set; }
        public int userGroupId { get; set; }
        public int menuId { get; set; }
        public bool isView { get; set; }
        public bool isEdit { get; set; }
        public int isActive { get; set; }
        public int createBy { get; set; }
        public DateTime createDatetime { get; set; }
        public int updateBy { get; set; }
        public DateTime? updateDatetime { get; set; }
        public string menuCode { get; set; } = string.Empty;
        public string menuName { get; set; } = string.Empty;
    }
}
