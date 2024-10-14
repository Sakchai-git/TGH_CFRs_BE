using BankReconcile.DAL.ConnectionDAL;
using CFRs.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.DAL.BANK_RECONCILE
{
    public partial class Paychqms_DAL : CFRs_DAL
    {
        public IEnumerable<VWOPaychqms> GetData(VWOPaychqms filter)
        {
            //filter.isActive = 1;
            List<VWOPaychqms> data = new List<VWOPaychqms>();
            try
            {
                string Query = $"SELECT TOP 1 * " +
                   $"            FROM VW_O_PAYCHQMS" +
                   $"            WHERE 1=1 AND (CHEQUE_NO = @CHEQUE_NO OR @CHEQUE_NO ='') AND BANK_CODE = @BANK_CODE";

                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "CHEQUE_NO",
                    Value = string.IsNullOrEmpty(filter.chequeNo) ? "" : filter.chequeNo,
                });
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "BANK_CODE",
                    Value = filter.bankCode,
                });


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        VWOPaychqms objectData = new VWOPaychqms();
                        objectData.bankCode = reader.IsDBNull("BANK_CODE") ? string.Empty : reader.GetString("BANK_CODE");
                        objectData.bankName = reader.IsDBNull("BANK_NAME") ? string.Empty : reader.GetString("BANK_NAME");
                        objectData.chequeNo = reader.IsDBNull("CHEQUE_NO") ? string.Empty : reader.GetString("CHEQUE_NO");
                        objectData.chequeDate = reader.IsDBNull("CHEQUE_DATE") ? string.Empty : reader.GetString("CHEQUE_DATE");
                        objectData.workDate = reader.IsDBNull("WORK_DATE") ? string.Empty : reader.GetString("WORK_DATE");
                        objectData.chequeAmt = reader.IsDBNull("CHEQUE_AMT") ? 0 : reader.GetDouble("CHEQUE_AMT");
                        objectData.paymentNo = reader.IsDBNull("PAYMENT_NO") ? string.Empty : reader.GetString("PAYMENT_NO");
                        objectData.chequeDetail = reader.IsDBNull("CHEQUE_DETAIL") ? string.Empty : reader.GetString("CHEQUE_DETAIL");
                        objectData.chequeName = reader.IsDBNull("CHEQUE_NAME") ? string.Empty : reader.GetString("CHEQUE_NAME");
                        objectData.chequeStatus = reader.IsDBNull("CHEQUE_STATUS") ? string.Empty : reader.GetString("CHEQUE_STATUS");
                        objectData.receiveOrCancelDate = reader.IsDBNull("RECEIVE_OR_CANCEL_DATE") ? string.Empty : reader.GetString("RECEIVE_OR_CANCEL_DATE");
                        objectData.remark1 = reader.IsDBNull("REMARK_1") ? string.Empty : reader.GetString("REMARK_1");
                        objectData.remark2 = reader.IsDBNull("REMARK_2") ? string.Empty : reader.GetString("REMARK_2");
                        data.Add(objectData);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (SQLConn.State == ConnectionState.Open)
                {
                    SQLConn.Close();
                    SQLConn.Dispose();
                }
            }
            
            return data;

        }


        #region + Instance +
        private static Paychqms_DAL _instance;
        public static Paychqms_DAL Instance
        {
            get
            {
                _instance = new Paychqms_DAL();
                return _instance;
            }
        }
        #endregion
    }
}
