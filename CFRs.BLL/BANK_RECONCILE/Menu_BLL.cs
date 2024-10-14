using CFRs.DAL.BANK_RECONCILE;
using CFRs.Model;
using System.Data;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.BLL.BANK_RECONCILE
{
    public class Menu_BLL
    {

        public IEnumerable<MMenu> GetData(MMenu filter)
        {
            try
            {
                return Menu_DAL.Instance.GetData(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }

        #region + Instance +
        private static Menu_BLL? _instance;
        public static Menu_BLL Instance
        {
            get
            {
                _instance = new Menu_BLL();
                return _instance;
            }
        }
        #endregion
    }
}