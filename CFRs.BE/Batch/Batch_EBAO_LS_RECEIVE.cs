using BankReconcile.BLL.EBAO;
using CFRs.BE.Helper;
using CFRs.BLL.BANK_RECONCILE;
using System.Data;
using System.Reflection;

namespace CFRs.BE.Batch
{
    public class Batch_EBAO_LS_RECEIVE
    {
        static object lockupdate = new object();

        public void Get(string DateStart = "", string DateEnd = "")
        {
            lock (lockupdate)
            {
                //TO_CHAR(osc.cheque_no_date, 'dd/MM/yyyy HH:mi:ss')
                //try
                //{
                    LogHelper.WriteLog("EBAO_LS (Receive)", "INF", $"Batch_EBAO_LS_RECEIVE Start");
                    if (string.IsNullOrEmpty(DateStart))
                    {
                        DateStart = BatchHelper.GetBatchDateStart("EBAO_LS_RECEIVE_BatchFixDate");
                    }
                    if (string.IsNullOrEmpty(DateEnd))
                    {
                        DateEnd = BatchHelper.GetBatchDateEnd("EBAO_LS_RECEIVE_BatchFixDate");
                    }
                    string Mode = BatchHelper.GetBatchMode("EBAO_LS_RECEIVE_BatchFixDate");

                    //Get data from EBAO LS
                    LogHelper.WriteLog("EBAO LS (Receive)", "INF", $"Get data from EBAO LS (Receive) (Date : {DateStart} to {DateEnd})");
                    DataTable dtImport = EBAO_LS_Data_BLL.Instance.GetDataReceiveBLL(DateStart, DateEnd);

                    //Write to DB
                    LogHelper.WriteLog("EBAO LS (Receive)", "INF", $"Found Data {dtImport.Rows.Count} Records.");

                    if (dtImport.Rows.Count > 0)
                    {
                        LogHelper.WriteLog("EBAO LS (Receive)", "INF", $"Write to DB");

                        SourceImport_BLL.Instance.EBAO_LS_RECEIVE_ImportBLL(dtImport, DateStart, DateEnd, Mode);

                        LogHelper.WriteLog("EBAO LS (Receive)", "INF", $"Write to DB Success.");
                    }

                    LogHelper.WriteLog("EBAO LS (GReceive)", "INF", $"Batch_EBAO_LS_RECEIVE End");
                //}
                //catch (Exception ex)
                //{
                //    string MethodName = MethodBase.GetCurrentMethod().Name;
                //    LogHelper.WriteLog("EBAO LS (GReceive)", "ERR", $"{MethodName} : {ex.Message}");
                //}
            }
        }
    }
}