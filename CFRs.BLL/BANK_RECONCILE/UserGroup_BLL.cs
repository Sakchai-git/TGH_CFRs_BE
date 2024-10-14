using CFRs.DAL.BANK_RECONCILE;
using CFRs.Model;
using System.Data;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.BLL.BANK_RECONCILE
{
    public class UserGroup_BLL
    {

        public IEnumerable<MUserGroup> GetData(MUserGroup filter)
        {
            try
            {
                return UserGroup_DAL.Instance.GetData(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public MUserGroup GetDataById(MUserGroup filter)
        {
            try
            {
                return UserGroup_DAL.Instance.GetDataById(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public MUserGroup Save(MUserGroup data)
        {
            try
            {
                return UserGroup_DAL.Instance.Save(data);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public MUserGroup UpdateIsActive0(MUserGroup data)
        {
            try
            {
                return UserGroup_DAL.Instance.UpdateIsActive0(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #region + Instance +
        private static UserGroup_BLL? _instance;
        public static UserGroup_BLL Instance
        {
            get
            {
                _instance = new UserGroup_BLL();
                return _instance;
            }
        }
        #endregion
    }
}