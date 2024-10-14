using CFRs.DAL.BANK_RECONCILE;
using CFRs.Model;
using System.Data;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.BLL.BANK_RECONCILE
{
    public class StatementImport_BLL
    {
        public DataTable ImportBLL(int MonthID, int Year, int BankId
            , string PathLocal, string PathS3, int UserID, DataTable dtImport
            , string RowHeader, string RowFooter)
        {
            try
            {
                return StatementImport_DAL.Instance.ImportDAL(MonthID, Year, BankId , PathLocal, PathS3, UserID, dtImport
                    , RowHeader, RowFooter);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<TStatementImport> GetData(TStatementImport filter)
        {
            try
            {
                return StatementImport_DAL.Instance.GetData(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public TStatementImport Delete(TStatementImport filter)
        {
            try
            {
                return StatementImport_DAL.Instance.Delete(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #region + Instance +
        private static StatementImport_BLL _instance;
        public static StatementImport_BLL Instance
        {
            get
            {
                _instance = new StatementImport_BLL();
                return _instance;
            }
        }
        #endregion
    }
}