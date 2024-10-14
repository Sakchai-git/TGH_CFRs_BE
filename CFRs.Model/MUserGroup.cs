namespace CFRs.Model
{
    public class MUserGroup
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public int isActive { get; set; } = 1;
        public int createBy { get; set; }
        public DateTime createDatetime { get; set; }
        public int updateBy { get; set; }
        public DateTime? updateDatetime { get; set; }
        public string updateDatetimeDisplay { get; set; } = string.Empty;
        public IEnumerable<MUserGroupList>? userList { get; set; }
        public IEnumerable<int>? users { get; set; }
        public IEnumerable<MPermisson>? permissons { get; set; }
        public string fullName { get; set; } = string.Empty;
        public string createByName { get; set; } = string.Empty;
        public string updateByName { get; set; } = string.Empty;
    }
}
