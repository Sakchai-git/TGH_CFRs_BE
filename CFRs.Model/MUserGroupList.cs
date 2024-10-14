namespace CFRs.Model
{
    public class MUserGroupList
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int userGroupId { get; set; }
        public int isActive { get; set; } = 1;
        public int createBy { get; set; }
        public DateTime createDatetime { get; set; }
        public int updateBy { get; set; }
        public DateTime? updateDatetime { get; set; }
        public string userGroupName { get; set; } = string.Empty;
    }
}
