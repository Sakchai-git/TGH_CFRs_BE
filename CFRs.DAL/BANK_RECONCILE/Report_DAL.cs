using BankReconcile.DAL.ConnectionDAL;
using CFRs.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.DAL.BANK_RECONCILE
{
    public partial class Report_DAL : CFRs_DAL
    {
        public IEnumerable<RBalance> Balance(RFilter filter)
        {
            List<RBalance> data = new List<RBalance>();

            try
            {
                string Query = $"USP_R_BALANCE_SHEET";

                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "I_YEAR_EN",
                    Value = filter.year,
                });
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "I_MONTH_ID",
                    Value = filter.monthId,
                });


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RBalance objectData = new RBalance();
                        objectData.series = reader.IsDBNull("SERIES") ? string.Empty : reader.GetString("SERIES");
                        objectData.systemCode = reader.IsDBNull("SYSTEM_CODE") ? string.Empty : reader.GetString("SYSTEM_CODE");
                        objectData.accountSap = reader.IsDBNull("ACCOUNT_SAP") ? string.Empty : reader.GetString("ACCOUNT_SAP");
                        objectData.bankCode = reader.IsDBNull("BANK_CODE") ? string.Empty : reader.GetString("BANK_CODE");
                        objectData.bankName = reader.IsDBNull("BANK_NAME") ? string.Empty : reader.GetString("BANK_NAME");
                        objectData.branchName = reader.IsDBNull("BRANCH_NAME") ? string.Empty : reader.GetString("BRANCH_NAME");
                        objectData.accountNo = reader.IsDBNull("ACCOUNT_NO") ? string.Empty : reader.GetString("ACCOUNT_NO");
                        objectData.sapDr = reader.IsDBNull("SAP_DR") ? 0 : reader.GetDecimal("SAP_DR");
                        objectData.sapCr = reader.IsDBNull("SAP_CR") ? 0 : reader.GetDecimal("SAP_CR");
                        objectData.glDr = reader.IsDBNull("GL_DR") ? 0 : reader.GetDecimal("GL_DR");
                        objectData.glCr = reader.IsDBNull("GL_CR") ? 0 : reader.GetDecimal("GL_CR");
                        objectData.diffDr = reader.IsDBNull("DIFF_DR") ? 0 : reader.GetDecimal("DIFF_DR");
                        objectData.diffCr = reader.IsDBNull("DIFF_CR") ? 0 : reader.GetDecimal("DIFF_CR");
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

        public RKTBTransfer KTBTransfer(RFilter filter)
        {

            RKTBTransfer data = new RKTBTransfer();
            data.KTBTransferIn = new List<RKTBTransferIn>();
            data.KTBTransferOut = new List<RKTBTransferOut>();

            try
            {
                //                  1   ธนาคารเพื่อการเกษตรและสหกรณ์การเกษตร BAAC
                //                  2   ธนาคารกสิกรไทย KBANK
                //                  3   ธนาคารกรุงไทย KTB
                //                  4   ธนาคารยูโอบี UOB
                string Query = $"USP_R_{filter.bankShortName}_STATEMENT_BEFORE_REC";

                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "I_YEAR_EN",
                    Value = filter.year,
                });
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "I_MONTH_ID",
                    Value = filter.monthId,
                });


                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        RKTBTransferIn objectData = new RKTBTransferIn();
                        objectData.orderNo = reader.IsDBNull("ORDER_NO") ? 0 : reader.GetInt32("ORDER_NO");
                        objectData.bankCode = reader.IsDBNull("BANK_CODE") ? string.Empty : reader.GetString("BANK_CODE");
                        objectData.accountSap = reader.IsDBNull("ACCOUNT_SAP") ? string.Empty : reader.GetString("ACCOUNT_SAP");
                        objectData.branchName = reader.IsDBNull("BRANCH_NAME") ? string.Empty : reader.GetString("BRANCH_NAME");
                        objectData.debit = reader.IsDBNull("DEBIT") ? 0 : reader.GetDecimal("DEBIT");
                        objectData.credit = reader.IsDBNull("CREDIT") ? 0 : reader.GetDecimal("CREDIT");
                        data.KTBTransferIn.Add(objectData);
                    }

                    reader.NextResult();
                    while (reader.Read())
                    {
                        RKTBTransferOut objectDataOut = new RKTBTransferOut();
                        objectDataOut.orderNo = reader.IsDBNull("ORDER_NO") ? 0 : reader.GetInt32("ORDER_NO");
                        objectDataOut.bankCode = reader.IsDBNull("BANK_CODE") ? string.Empty : reader.GetString("BANK_CODE");
                        objectDataOut.accountSap = reader.IsDBNull("ACCOUNT_SAP") ? string.Empty : reader.GetString("ACCOUNT_SAP");
                        objectDataOut.branchName = reader.IsDBNull("BRANCH_NAME") ? string.Empty : reader.GetString("BRANCH_NAME");
                        objectDataOut.debit = reader.IsDBNull("DEBIT") ? 0 : reader.GetDecimal("DEBIT");
                        objectDataOut.credit = reader.IsDBNull("CREDIT") ? 0 : reader.GetDecimal("CREDIT");
                        data.KTBTransferOut.Add(objectDataOut);
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
        public IEnumerable<RBAACTransfer> BAACTransfer(RFilter filter)
        {
            List<RBAACTransfer> data = new List<RBAACTransfer>();

            try
            {
                string Query = $"USP_R_{filter.bankShortName}_STATEMENT_BEFORE_REC";

                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "I_YEAR_EN",
                    Value = filter.year,
                });
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "I_MONTH_ID",
                    Value = filter.monthId,
                });


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RBAACTransfer objectData = new RBAACTransfer();
                        objectData.bankCode = reader.IsDBNull("BANK_CODE") ? string.Empty : reader.GetString("BANK_CODE");
                        objectData.accountSap = reader.IsDBNull("ACCOUNT_SAP") ? string.Empty : reader.GetString("ACCOUNT_SAP");
                        objectData.accountNo = reader.IsDBNull("ACCOUNT_NO") ? string.Empty : reader.GetString("ACCOUNT_NO");
                        objectData.branchName = reader.IsDBNull("BRANCH_NAME") ? string.Empty : reader.GetString("BRANCH_NAME");
                        objectData.transactionCode = reader.IsDBNull("TRANSACTION_CODE") ? string.Empty : reader.GetString("TRANSACTION_CODE");
                        objectData.sumAmount = reader.IsDBNull("SUM_AMOUNT") ? null : reader.GetDecimal("SUM_AMOUNT");
                        //objectData.valueDate = reader.IsDBNull("VALUE_DATE") ? DateTime.MinValue : reader.GetDateTime("VALUE_DATE");
                        //objectData.transactionDate = reader.IsDBNull("TRANSACTION_DATE") ? DateTime.MinValue : reader.GetDateTime("TRANSACTION_DATE");
                        //objectData.transactionTime = reader.IsDBNull("TRANSACTION_TIME") ? string.Empty : reader.GetString("TRANSACTION_TIME");
                        //objectData.description = reader.IsDBNull("DESCRIPTION") ? string.Empty : reader.GetString("DESCRIPTION");
                        //objectData.withdrawal = reader.IsDBNull("WITHDRAWAL") ? string.Empty : reader.GetString("WITHDRAWAL");
                        //objectData.adjAmount = reader.IsDBNull("ADJ_AMOUNT") ? string.Empty : reader.GetString("ADJ_AMOUNT");
                        //objectData.balance = reader.IsDBNull("BALANCE") ? string.Empty : reader.GetString("BALANCE");
                        //objectData.remark = reader.IsDBNull("REMARK") ? string.Empty : reader.GetString("REMARK");
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

        public IEnumerable<RUOBTransfer> UOBTransfer(RFilter filter)
        {
            List<RUOBTransfer> data = new List<RUOBTransfer>();

            try
            {
                string Query = $"USP_R_{filter.bankShortName}_STATEMENT_BEFORE_REC";

                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "I_YEAR_EN",
                    Value = filter.year,
                });
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "I_MONTH_ID",
                    Value = filter.monthId,
                });


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RUOBTransfer objectData = new RUOBTransfer();
                        objectData.valueDate = reader.IsDBNull("VALUE_DATE") ? string.Empty : reader.GetString("VALUE_DATE");
                        objectData.transactionDate = reader.IsDBNull("TRANSACTION_DATE") ? string.Empty : reader.GetString("TRANSACTION_DATE");
                        objectData.transactionTime = reader.IsDBNull("TRANSACTION_TIME") ? string.Empty : reader.GetString("TRANSACTION_TIME");
                        objectData.description = reader.IsDBNull("DESCRIPTION") ? string.Empty : reader.GetString("DESCRIPTION");
                        objectData.withdrawal = reader.IsDBNull("WITHDRAWAL") ? string.Empty : reader.GetString("WITHDRAWAL");
                        objectData.adjAmount = reader.IsDBNull("ADJ_AMOUNT") ? null : reader.GetDecimal("ADJ_AMOUNT");
                        objectData.balance = reader.IsDBNull("BALANCE") ? null : reader.GetDecimal("BALANCE");
                        objectData.remark = reader.IsDBNull("REMARK") ? string.Empty : reader.GetString("REMARK");
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
        public IEnumerable<RKBANKMediaClearing> KBANKMediaClearing(RFilter filter)
        {
            List<RKBANKMediaClearing> data = new List<RKBANKMediaClearing>();

            try
            {
                string Query = $"USP_R_{filter.bankShortName}_MEDIA_CLEARING";

                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "I_YEAR_EN",
                    Value = filter.year,
                });
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "I_MONTH_ID",
                    Value = filter.monthId,
                });


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RKBANKMediaClearing objectData = new RKBANKMediaClearing();
                        objectData.bankCode = reader.IsDBNull("BANK_CODE") ? string.Empty : reader.GetString("BANK_CODE");
                        objectData.accountSap = reader.IsDBNull("ACCOUNT_SAP") ? string.Empty : reader.GetString("ACCOUNT_SAP");
                        objectData.accountNo = reader.IsDBNull("ACCOUNT_NO") ? string.Empty : reader.GetString("ACCOUNT_NO");
                        objectData.branchName = reader.IsDBNull("BRANCH_NAME") ? string.Empty : reader.GetString("BRANCH_NAME");
                        objectData.effectiveDate = reader.IsDBNull("EFFECTIVE_DATE") ? string.Empty : reader.GetString("EFFECTIVE_DATE");
                        objectData.adjAmount = reader.IsDBNull("ADJ_AMOUNT") ? null : reader.GetDecimal("ADJ_AMOUNT");
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
        private static Report_DAL? _instance;
        public static Report_DAL Instance
        {
            get
            {
                _instance = new Report_DAL();
                return _instance;
            }
        }
        #endregion
    }
}
