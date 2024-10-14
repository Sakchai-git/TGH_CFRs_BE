using CFRs.DAL.CONFIG;
using System.Data;

namespace CFRs.BLL.CONFIG
{
    public class ConfigBLL
    {
        public DataTable GetConfigBLL(string Condition)
        {
            try
            {
                return ConfigDAL.Instance.GetConfigDAL(Condition);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region + Instance +
        private static ConfigBLL _instance;
        public static ConfigBLL Instance
        {
            get
            {
                _instance = new ConfigBLL();
                return _instance;
            }
        }
        #endregion
    }
}