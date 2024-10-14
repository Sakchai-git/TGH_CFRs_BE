using BankReconcile.DAL.EBAO;
using System.Data;

namespace BankReconcile.BLL.EBAO
{
    public class EBAO_LS_Data_BLL
    {
        public DataTable GetDataBLL(string DateStart, string DateEnd)
        {
            try
            {
                return EBAO_LS_Data_DAL.Instance.GetDataDAL(DateStart, DateEnd);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetDataGroupKruBLL(string DateStart, string DateEnd)
        {
            try
            {
                return EBAO_LS_Data_DAL.Instance.GetDataGroupKruDAL(DateStart, DateEnd);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetDataReceiveBLL(string DateStart, string DateEnd)
        {
            try
            {
                return EBAO_LS_Data_DAL.Instance.GetDataReceiveDAL(DateStart, DateEnd);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetDataKBankLS_BLL(string DateStart, string DateEnd)
        {
            try
            {
                return EBAO_LS_Data_DAL.Instance.GetDataKBankLS_DAL(DateStart, DateEnd);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region + Instance +
        private static EBAO_LS_Data_BLL _instance;
        public static EBAO_LS_Data_BLL Instance
        {
            get
            {
                _instance = new EBAO_LS_Data_BLL();
                return _instance;
            }
        }
        #endregion
    }
}