using CFRs.DAL.BANK_RECONCILE;
using CFRs.Model;
using System.Data;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.BLL.BANK_RECONCILE
{
    public class UserGroupList_BLL
    {

        public IEnumerable<MUserGroupList> GetData(MUserGroupList filter)
        {
            try
            {
                return UserGroupList_DAL.Instance.GetData(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }

        #region + Instance +
        private static UserGroupList_BLL? _instance;
        public static UserGroupList_BLL Instance
        {
            get
            {
                _instance = new UserGroupList_BLL();
                return _instance;
            }
        }
        #endregion
    }
}