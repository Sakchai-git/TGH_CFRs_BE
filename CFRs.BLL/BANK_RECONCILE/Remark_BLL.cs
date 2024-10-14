using CFRs.DAL.BANK_RECONCILE;
using CFRs.Model;
using System.Data;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.BLL.BANK_RECONCILE
{
    public class Remark_BLL
    {

        public IEnumerable<MRemark> GetData(MRemark filter)
        {
            try
            {
                return Remark_DAL.Instance.GetData(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public MRemark GetDataById(MRemark filter)
        {
            try
            {
                return Remark_DAL.Instance.GetDataById(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public MRemark UpdateIsActive0(MRemark data)
        {
            try
            {
                return Remark_DAL.Instance.UpdateIsActive0(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public MRemark Save(MRemark data)
        {
            try
            {
                return Remark_DAL.Instance.Save(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #region + Instance +
        private static Remark_BLL? _instance;
        public static Remark_BLL Instance
        {
            get
            {
                _instance = new Remark_BLL();
                return _instance;
            }
        }
        #endregion
    }
}