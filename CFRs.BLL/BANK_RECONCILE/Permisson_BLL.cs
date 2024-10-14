using CFRs.DAL.BANK_RECONCILE;
using CFRs.Model;
using System.Data;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.BLL.BANK_RECONCILE
{
    public class Permisson_BLL
    {

        public IEnumerable<MPermisson> GetData(MPermisson filter)
        {
            try
            {
                return Permisson_DAL.Instance.GetData(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public IEnumerable<MPermisson> GetDataByUserId(int userId)
        {
            try
            {
                return Permisson_DAL.Instance.GetDataByUserId(userId);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #region + Instance +
        private static Permisson_BLL? _instance;
        public static Permisson_BLL Instance
        {
            get
            {
                _instance = new Permisson_BLL();
                return _instance;
            }
        }
        #endregion
    }
}