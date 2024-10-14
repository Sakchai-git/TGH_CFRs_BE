namespace CFRs.Model
{
    public class MUser
    {
        public int userId { get; set; }
        public string username { get; set; } = string.Empty;
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string position { get; set; } = string.Empty;
        public string remark { get; set; } = string.Empty;
        public int isActive { get; set; } = 1;
        public int createBy { get; set; }
        public DateTime createDatetime { get; set; }
        public int updateBy { get; set; }
        public DateTime? updateDatetime { get; set; }
        public string updateDatetimeDisplay { get; set; } = string.Empty;
        public IEnumerable<MUserGroupList>? userGroupList { get; set; }
        public string userGroupName { get; set; } = string.Empty;
        public IEnumerable<MPermisson>? permissons { get; set; }
        public string fullName { get; set; } = string.Empty;
        public string createByName { get; set; } = string.Empty;
        public string updateByName { get; set; } = string.Empty;
        public IEnumerable<int>? userGroups { get; set; }


    }
}
