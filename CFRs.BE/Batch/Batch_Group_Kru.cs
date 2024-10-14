using BankReconcile.BLL.EBAO;
using CFRs.BE.Helper;
using CFRs.BLL.BANK_RECONCILE;
using GemBox.Spreadsheet;
using System.Data;
using System.Reflection;

namespace CFRs.BE.Batch
{
    public class Batch_Group_Kru
    {
        static object lockupdate = new object();

        public void Get(string DateStart = "", string DateEnd = "")
        {
            lock (lockupdate)
            {
                //DateStart = "20240101";
                //DateEnd = "20240131";

                //TO_CHAR(osc.cheque_no_date, 'dd/MM/yyyy HH:mi:ss')
                //try
                //{
                LogHelper.WriteLog("EBAO_LS (Group Kru)", "INF", $"Batch_Group_Kru Start");
                if (string.IsNullOrEmpty(DateStart))
                {
                    DateStart = BatchHelper.GetBatchDateStart("GROUP_KRU_BatchFixDate");
                }
                if (string.IsNullOrEmpty(DateEnd))
                {
                    DateEnd = BatchHelper.GetBatchDateEnd("GROUP_KRU_BatchFixDate");
                }
                string Mode = BatchHelper.GetBatchMode("GROUP_KRU_BatchFixDate");

                //Get data from EBAO LS
                LogHelper.WriteLog("EBAO LS (Group Kru)", "INF", $"Get data from EBAO LS (Group Kru) (Date : {DateStart} to {DateEnd})");
                DataTable dtImport = EBAO_LS_Data_BLL.Instance.GetDataGroupKruBLL(DateStart, DateEnd);

                string FilePath = $"{Directory.GetCurrentDirectory()}//GroupKruExcel";
                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }

                dtImport.Columns.Add("BANK_BRANCH_CODE");
                dtImport.Columns.Add("SEG_BANK_ACCOUNT");
                dtImport.Columns.Add("PAY_IN_DATE");
                dtImport.Columns.Add("AMOUNT");

                int RowCount = dtImport.Rows.Count;

                //Convert Binary to xls
                for (int i = 0; i < RowCount; i++)
                {
                    byte[] bytData = (byte[])dtImport.Rows[i]["FILE2_LOB"];

                    string FullPath = FilePath + "//" + dtImport.Rows[i]["FILE2_NAME"].ToString() + ".xls";

                    FileStream fs = new FileStream(FullPath, FileMode.OpenOrCreate, FileAccess.Write);
                    BinaryWriter br = new BinaryWriter(fs);
                    br.Write(bytData);
                    fs.Dispose();

                    ExcelFile workbook = ExcelFile.Load(FullPath);
                    ExcelWorksheet worksheet = workbook.Worksheets[0];

                    for (int j = 2; j <= worksheet.Rows.Count; j++)
                    {
                        if (worksheet.Cells[$"A{j}"].Value != null
                            && worksheet.Cells[$"B{j}"].Value != null
                            && worksheet.Cells[$"C{j}"].Value != null
                            && worksheet.Cells[$"D{j}"].Value != null)
                        {
                            if (!string.Equals(worksheet.Cells[$"A{j}"].Value.ToString(), string.Empty)
                             && !string.Equals(worksheet.Cells[$"B{j}"].Value.ToString(), string.Empty)
                             && !string.Equals(worksheet.Cells[$"C{j}"].Value.ToString(), string.Empty)
                             && !string.Equals(worksheet.Cells[$"D{j}"].Value.ToString(), string.Empty))
                            {
                                if (j == 2)
                                {
                                    dtImport.Rows[i]["BANK_BRANCH_CODE"] = worksheet.Cells[$"A{j}"].Value.ToString();
                                    dtImport.Rows[i]["SEG_BANK_ACCOUNT"] = worksheet.Cells[$"B{j}"].Value.ToString();
                                    dtImport.Rows[i]["PAY_IN_DATE"] = worksheet.Cells[$"C{j}"].Value.ToString();
                                    dtImport.Rows[i]["AMOUNT"] = worksheet.Cells[$"D{j}"].Value.ToString();
                                }
                                else
                                {
                                    DataRow dr = dtImport.NewRow();
                                    for (int k = 0; k < dtImport.Columns.Count; k++)
                                    {
                                        if (string.Equals(dtImport.Columns[k].ColumnName, "FILE2_LOB"))
                                            continue;

                                        dr[k] = dtImport.Rows[i][k].ToString();
                                    }

                                    dr["BANK_BRANCH_CODE"] = worksheet.Cells[$"A{j}"].Value.ToString();
                                    dr["SEG_BANK_ACCOUNT"] = worksheet.Cells[$"B{j}"].Value.ToString();
                                    dr["PAY_IN_DATE"] = worksheet.Cells[$"C{j}"].Value.ToString();
                                    dr["AMOUNT"] = worksheet.Cells[$"D{j}"].Value.ToString();

                                    dtImport.Rows.Add(dr);
                                }
                            }
                        }
                    }
                }

                //Write to DB
                //LogHelper.WriteLog("EBAO LS (Group Kru)", "INF", $"Found Data {dtImport.Rows.Count} Records.");

                if (dtImport.Rows.Count > 0)
                {
                    dtImport.Columns.Remove("FILE2_LOB");

                    LogHelper.WriteLog("EBAO LS (Group Kru)", "INF", $"Write to DB");

                    SourceImport_BLL.Instance.EBAO_LS_GROUP_KRU_ImportBLL(dtImport, DateStart, DateEnd, Mode);

                    LogHelper.WriteLog("EBAO LS (Group Kru)", "INF", $"Write to DB Success.");
                }

                LogHelper.WriteLog("EBAO LS (Group Kru)", "INF", $"Batch_Group_Kru End");
                //}
                //catch (Exception ex)
                //{
                //    string MethodName = MethodBase.GetCurrentMethod().Name;
                //    LogHelper.WriteLog("EBAO LS (Group Kru)", "ERR", $"{MethodName} : {ex.Message}");
                //}
            }
        }
    }
}
