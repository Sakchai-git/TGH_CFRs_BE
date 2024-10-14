using CFRs.DAL.BANK_RECONCILE;
using CFRs.Model;
using System.Data;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.BLL.BANK_RECONCILE
{
    public class RunBatch_BLL
    {

        public IEnumerable<TRunBatch> GetData(TRunBatch filter)
        {
            try
            {
                return RunBatch_DAL.Instance.GetData(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public IEnumerable<TRunBatchDetail> GetDataDetail(TRunBatchDetail filter)
        {
            try
            {
                return RunBatch_DAL.Instance.GetDataDetail(filter);
            }
            catch (Exception)
            {
                throw;
            }

        }


        //public bool CheckInProgress()
        //{
        //    try
        //    {
        //        return RunBatch_DAL.Instance.CheckInProgress();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}

        public TRunBatch Save(TRunBatch data)
        {
            try
            {
                return RunBatch_DAL.Instance.Save(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public TRunBatch Run(TRunBatch data)
        {
            try
            {
                return RunBatch_DAL.Instance.Run(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #region + Instance +
        private static RunBatch_BLL? _instance;
        public static RunBatch_BLL Instance
        {
            get
            {
                _instance = new RunBatch_BLL();
                return _instance;
            }
        }
        #endregion
    }
}