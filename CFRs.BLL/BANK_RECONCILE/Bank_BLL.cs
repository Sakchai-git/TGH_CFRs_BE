using CFRs.DAL.BANK_RECONCILE;
using CFRs.Model;
using System.Data;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.BLL.BANK_RECONCILE
{
    public class Bank_BLL
    {

        public IEnumerable<MBank> GetData(MBank filter)
        {
            try
            {
                return Bank_DAL.Instance.GetData(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public MBank GetDataById(MBank filter)
        {
            try
            {
                return Bank_DAL.Instance.GetDataById(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public MBank Save(MBank data)
        {
            try
            {
                return Bank_DAL.Instance.Save(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public MBank UpdateIsActive0(MBank data)
        {
            try
            {
                return Bank_DAL.Instance.UpdateIsActive0(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #region + Instance +
        private static Bank_BLL? _instance;
        public static Bank_BLL Instance
        {
            get
            {
                _instance = new Bank_BLL();
                return _instance;
            }
        }
        #endregion
    }
}