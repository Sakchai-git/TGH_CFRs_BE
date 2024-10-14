using CFRs.DAL.BANK_RECONCILE;
using CFRs.Model;
using System.Data;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.BLL.BANK_RECONCILE
{
    public class Report_BLL
    {

        public IEnumerable<RBalance> Balance(RFilter filter)
        {
            try
            {
                return Report_DAL.Instance.Balance(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public RKTBTransfer KTBTransfer(RFilter filter)
        {
            try
            {
                return Report_DAL.Instance.KTBTransfer(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public IEnumerable<RBAACTransfer> BAACTransfer(RFilter filter)
        {
            try
            {
                return Report_DAL.Instance.BAACTransfer(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public IEnumerable<RUOBTransfer> UOBTransfer(RFilter filter)
        {
            try
            {
                return Report_DAL.Instance.UOBTransfer(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public IEnumerable<RKBANKMediaClearing> KBANKMediaClearing(RFilter filter)
        {
            try
            {
                return Report_DAL.Instance.KBANKMediaClearing(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #region + Instance +
        private static Report_BLL? _instance;
        public static Report_BLL Instance
        {
            get
            {
                _instance = new Report_BLL();
                return _instance;
            }
        }
        #endregion
    }
}