using CFRs.DAL.BANK_RECONCILE;
using CFRs.Model;
using System.Data;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.BLL.BANK_RECONCILE
{
    public class System_BLL
    {

        public IEnumerable<MSystem> GetData(MSystem filter)
        {
            try
            {
                return System_DAL.Instance.GetData(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }

        #region + Instance +
        private static System_BLL _instance;
        public static System_BLL Instance
        {
            get
            {
                _instance = new System_BLL();
                return _instance;
            }
        }
        #endregion
    }
}