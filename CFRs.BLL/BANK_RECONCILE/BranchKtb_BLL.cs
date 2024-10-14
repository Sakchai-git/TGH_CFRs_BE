using CFRs.DAL.BANK_RECONCILE;
using CFRs.Model;
using System.Data;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.BLL.BANK_RECONCILE
{
    public class BranchKtb_BLL
    {

        public IEnumerable<MBranchKtb> GetData(MBranchKtb filter)
        {
            try
            {
                return BranchKtb_DAL.Instance.GetData(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public MBranchKtb GetDataById(MBranchKtb filter)
        {
            try
            {
                return BranchKtb_DAL.Instance.GetDataById(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public MBranchKtb UpdateIsActive0(MBranchKtb data)
        {
            try
            {
                return BranchKtb_DAL.Instance.UpdateIsActive0(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public MBranchKtb Save(MBranchKtb data)
        {
            try
            {
                return BranchKtb_DAL.Instance.Save(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #region + Instance +
        private static BranchKtb_BLL? _instance;
        public static BranchKtb_BLL Instance
        {
            get
            {
                _instance = new BranchKtb_BLL();
                return _instance;
            }
        }
        #endregion
    }
}