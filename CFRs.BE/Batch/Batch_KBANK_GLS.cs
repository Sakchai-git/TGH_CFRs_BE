using BankReconcile.BLL.EBAO;
using CFRs.BE.Helper;
using CFRs.BLL.BANK_RECONCILE;
using CFRs.BLL.EBAO;
using System.Data;
using System.Reflection;

namespace CFRs.BE.Batch
{
    public class Batch_KBANK_GLS
    {
        static object lockupdate = new object();

        public void Get(string DateStart = "", string DateEnd = "")
        {
            lock (lockupdate)
            {
                //try
                //{
                    LogHelper.WriteLog("KBANK_GLS", "INF", $"Batch_KBANK_GLS Start");
                    if (string.IsNullOrEmpty(DateStart))
                    {
                        DateStart = BatchHelper.GetBatchDateStart("KBANK_GLS_BatchFixDate");
                    }
                    if (string.IsNullOrEmpty(DateEnd))
                    {
                        DateEnd = BatchHelper.GetBatchDateEnd("KBANK_GLS_BatchFixDate");
                    };
                    string Mode = BatchHelper.GetBatchMode("KBANK_GLS_BatchFixDate");

                    //Get data from EBAO LS
                    LogHelper.WriteLog("KBANK_GLS", "INF", $"Get data from KBANK_GLS (Date : {DateStart} to {DateEnd})");
                    DataTable dtImport = EBAO_GLS_Data_BLL.Instance.GetDataKBankGLS_BLL(DateStart, DateEnd);
                    DataTable dtImportCLob = EBAO_GLS_Data_BLL.Instance.GetDataKBankGLS_Clob_BLL(DateStart, DateEnd);

                    //Write to DB
                    LogHelper.WriteLog("KBANK_GLS", "INF", $"Found Data {dtImport.Rows.Count} Records.");

                    if (dtImport.Rows.Count > 0)
                    {
                        LogHelper.WriteLog("KBANK_GLS", "INF", $"Write to DB");

                        SourceImport_BLL.Instance.EBAO_GLS_KBANK_ImportBLL(dtImport, DateStart, DateEnd, Mode, dtImportCLob);

                        LogHelper.WriteLog("KBANK_GLS", "INF", $"Write to DB Success.");
                    }

                    LogHelper.WriteLog("KBANK_GLS", "INF", $"Batch_KBANK_GLS End");
                //}
                //catch (Exception ex)
                //{
                //    string MethodName = MethodBase.GetCurrentMethod().Name;
                //    LogHelper.WriteLog("KBANK_GLS", "ERR", $"{MethodName} : {ex.Message}");
                //}
            }
        }
    }
}