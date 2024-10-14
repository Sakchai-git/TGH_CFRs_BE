using BankReconcile.BLL.EBAO;
using CFRs.BE.Helper;
using CFRs.BLL.BANK_RECONCILE;
using System.Data;
using System.Reflection;

namespace CFRs.BE.Batch
{
    public class Batch_KBANK_LS
    {
        static object lockupdate = new object();

        public void Get(string DateStart = "", string DateEnd = "")
        {
            lock (lockupdate)
            {
                //try
                //{
                    LogHelper.WriteLog("KBANK_LS", "INF", $"Batch_KBANK_LS Start");
                    if (string.IsNullOrEmpty(DateStart))
                    {
                        DateStart = BatchHelper.GetBatchDateStart("KBANK_LS_BatchFixDate");
                    }
                    if (string.IsNullOrEmpty(DateEnd))
                    {
                        DateEnd = BatchHelper.GetBatchDateEnd("KBANK_LS_BatchFixDate");
                    }
                    string Mode = BatchHelper.GetBatchMode("KBANK_LS_BatchFixDate");

                    //Get data from EBAO LS
                    LogHelper.WriteLog("KBANK_LS", "INF", $"Get data from KBANK_LS (Date : {DateStart} to {DateEnd})");
                    DataTable dtImport = EBAO_LS_Data_BLL.Instance.GetDataKBankLS_BLL(DateStart, DateEnd);

                    //Write to DB
                    LogHelper.WriteLog("KBANK_LS", "INF", $"Found Data {dtImport.Rows.Count} Records.");

                    if (dtImport.Rows.Count > 0)
                    {
                        LogHelper.WriteLog("KBANK_LS", "INF", $"Write to DB");

                        SourceImport_BLL.Instance.EBAO_LS_KBANK_ImportBLL(dtImport, DateStart, DateEnd, Mode);

                        LogHelper.WriteLog("KBANK_LS", "INF", $"Write to DB Success.");
                    }

                    LogHelper.WriteLog("KBANK_LS", "INF", $"Batch_KBANK_LS End");
                //}
                //catch (Exception ex)
                //{
                //    string MethodName = MethodBase.GetCurrentMethod().Name;
                //    LogHelper.WriteLog("KBANK_LS", "ERR", $"{MethodName} : {ex.Message}");
                //}
            }
        }
    }
}