using CFRs.DAL.BANK_RECONCILE;
using CFRs.Model;
using System.Data;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.BLL.BANK_RECONCILE
{
    public class AutoReconcile_BLL
    {

        public IEnumerable<TAutoReconcile> GetData(TAutoReconcile filter)
        {
            try
            {
                return AutoReconcile_DAL.Instance.GetData(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public IEnumerable<TAutoReconcileDetail> GetDataDetail(TAutoReconcileDetail filter)
        {
            try
            {
                return AutoReconcile_DAL.Instance.GetDataDetail(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }


        public IEnumerable<RAutoReconcile> Export(TAutoReconcile filter)
        {
            try
            {
                return AutoReconcile_DAL.Instance.Export(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public TAutoReconcile Save(TAutoReconcile data)
        {
            try
            {
                return AutoReconcile_DAL.Instance.Save(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public TAutoReconcile Run(TAutoReconcile data)
        {
            try
            {
                return AutoReconcile_DAL.Instance.Run(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #region + Instance +
        private static AutoReconcile_BLL? _instance;
        public static AutoReconcile_BLL Instance
        {
            get
            {
                _instance = new AutoReconcile_BLL();
                return _instance;
            }
        }
        #endregion
    }
}