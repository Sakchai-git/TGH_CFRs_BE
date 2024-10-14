using CFRs.DAL.BANK_RECONCILE;
using CFRs.Model;
using System.Data;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.BLL.BANK_RECONCILE
{
    public class User_BLL
    {

        public IEnumerable<MUser> GetData(MUser filter)
        {
            try
            {
                return User_DAL.Instance.GetData(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public MUser GetDataById(MUser filter)
        {
            try
            {
                return User_DAL.Instance.GetDataById(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public MUser GetDataByUserName(MUser filter)
        {
            try
            {
                return User_DAL.Instance.GetDataByUserName(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public MUser Save(MUser data)
        {
            try
            {
                return User_DAL.Instance.Save(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public MUser UpdateIsActive0(MUser data)
        {
            try
            {
                return User_DAL.Instance.UpdateIsActive0(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #region + Instance +
        private static User_BLL? _instance;
        public static User_BLL Instance
        {
            get
            {
                _instance = new User_BLL();
                return _instance;
            }
        }
        #endregion
    }
}