using CFRs.DAL.BANK_RECONCILE;
using CFRs.Model;
using System.Data;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.BLL.BANK_RECONCILE
{
    public class Paychqms_BLL
    {

        public IEnumerable<VWOPaychqms> GetData(VWOPaychqms filter)
        {
            try
            {
                return Paychqms_DAL.Instance.GetData(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }

        #region + Instance +
        private static Paychqms_BLL _instance;
        public static Paychqms_BLL Instance
        {
            get
            {
                _instance = new Paychqms_BLL();
                return _instance;
            }
        }
        #endregion
    }
}