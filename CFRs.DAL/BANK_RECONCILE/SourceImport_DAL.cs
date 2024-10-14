using BankReconcile.DAL.ConnectionDAL;
using CFRs.DAL.Helper;
using CFRs.Model;
using System.Data;
using System.Data.SqlClient;

namespace CFRs.DAL.BANK_RECONCILE
{
    public class SourceImport_DAL : CFRs_DAL
    {
        public DataTable EBAO_LS_ImportDAL(DataTable dtImport, string DateStart, string DateEnd, string Mode)
        {
            try
            {
                SQLConn.Open();

                string Query = String.Empty;

                Query = "TRUNCATE TABLE T_EBAO_LS_TEMP";

                SqlCommand cmdInserTemp = new SqlCommand(Query, SQLConn);
                cmdInserTemp.CommandType = CommandType.Text;

                cmdInserTemp.ExecuteNonQuery();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(GetSettingsHelper.CFRsConnectionString))
                {
                    bulkCopy.DestinationTableName = "T_EBAO_LS_TEMP";

                    bulkCopy.WriteToServer(dtImport);
                }

                DataTable dtReturn = new DataTable();
                Query = "USP_T_EBAO_LS_IMPORT";

                SqlTransaction tr = SQLConn.BeginTransaction();

                try
                {
                    SqlCommand cmdInsert = new SqlCommand(Query, SQLConn);
                    cmdInsert.CommandType = CommandType.StoredProcedure;
                    cmdInsert.CommandTimeout = 3600;

                    cmdInsert.Parameters.AddWithValue("@I_DATE_START", DateStart);
                    cmdInsert.Parameters.AddWithValue("@I_DATE_END", DateEnd);
                    cmdInsert.Parameters.AddWithValue("@I_MODE", Mode);

                    cmdInsert.Transaction = tr;

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmdInsert))
                    {
                        dataAdapter.Fill(dtReturn);
                    }

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    throw new Exception(ex.Message);
                }

                return dtReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (SQLConn != null && SQLConn.State == ConnectionState.Open)
                {
                    SQLConn.Close();
                    SQLConn.Dispose();
                }
            }
        }

        public DataTable EBAO_LS_Group_Kru_ImportDAL(DataTable dtImport, string DateStart, string DateEnd, string Mode)
        {
            try
            {
                SQLConn.Open();

                string Query = String.Empty;

                Query = "TRUNCATE TABLE T_GROUP_KRU_TEMP";

                SqlCommand cmdInserTemp = new SqlCommand(Query, SQLConn);
                cmdInserTemp.CommandType = CommandType.Text;

                cmdInserTemp.ExecuteNonQuery();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(GetSettingsHelper.CFRsConnectionString))
                {
                    bulkCopy.DestinationTableName = "T_GROUP_KRU_TEMP";

                    bulkCopy.WriteToServer(dtImport);
                }

                DataTable dtReturn = new DataTable();
                Query = "USP_T_GROUP_KRU_IMPORT";

                SqlTransaction tr = SQLConn.BeginTransaction();

                try
                {
                    SqlCommand cmdInsert = new SqlCommand(Query, SQLConn);
                    cmdInsert.CommandType = CommandType.StoredProcedure;
                    cmdInsert.CommandTimeout = 3600;

                    cmdInsert.Parameters.AddWithValue("@I_DATE_START", DateStart);
                    cmdInsert.Parameters.AddWithValue("@I_DATE_END", DateEnd);
                    cmdInsert.Parameters.AddWithValue("@I_MODE", Mode);

                    cmdInsert.Transaction = tr;

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmdInsert))
                    {
                        dataAdapter.Fill(dtReturn);
                    }

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    throw new Exception(ex.Message);
                }

                return dtReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (SQLConn != null && SQLConn.State == ConnectionState.Open)
                {
                    SQLConn.Close();
                    SQLConn.Dispose();
                }
            }
        }

        public DataTable EBAO_LS_RECEIVE_ImportDAL(DataTable dtImport, string DateStart, string DateEnd, string Mode)
        {
            try
            {
                SQLConn.Open();

                string Query = String.Empty;

                Query = "TRUNCATE TABLE T_EBAO_LS_RECEIVE_TEMP";

                SqlCommand cmdInserTemp = new SqlCommand(Query, SQLConn);
                cmdInserTemp.CommandType = CommandType.Text;

                cmdInserTemp.ExecuteNonQuery();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(GetSettingsHelper.CFRsConnectionString))
                {
                    bulkCopy.DestinationTableName = "T_EBAO_LS_RECEIVE_TEMP";

                    bulkCopy.WriteToServer(dtImport);
                }

                DataTable dtReturn = new DataTable();
                Query = "USP_T_EBAO_LS_RECEIVE_IMPORT";

                SqlTransaction tr = SQLConn.BeginTransaction();

                try
                {
                    SqlCommand cmdInsert = new SqlCommand(Query, SQLConn);
                    cmdInsert.CommandType = CommandType.StoredProcedure;
                    cmdInsert.CommandTimeout = 3600;

                    cmdInsert.Parameters.AddWithValue("@I_DATE_START", DateStart);
                    cmdInsert.Parameters.AddWithValue("@I_DATE_END", DateEnd);
                    cmdInsert.Parameters.AddWithValue("@I_MODE", Mode);

                    cmdInsert.Transaction = tr;

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmdInsert))
                    {
                        dataAdapter.Fill(dtReturn);
                    }

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    throw new Exception(ex.Message);
                }

                return dtReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (SQLConn != null && SQLConn.State == ConnectionState.Open)
                {
                    SQLConn.Close();
                    SQLConn.Dispose();
                }
            }
        }

        public DataTable EBAO_LS_KBANK_ImportDAL(DataTable dtImport, string DateStart, string DateEnd, string Mode)
        {
            try
            {
                SQLConn.Open();

                string Query = String.Empty;

                Query = "TRUNCATE TABLE T_KBANK_LS_TEMP";

                SqlCommand cmdInserTemp = new SqlCommand(Query, SQLConn);
                cmdInserTemp.CommandType = CommandType.Text;

                cmdInserTemp.ExecuteNonQuery();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(GetSettingsHelper.CFRsConnectionString))
                {
                    bulkCopy.DestinationTableName = "T_KBANK_LS_TEMP";

                    bulkCopy.WriteToServer(dtImport);
                }

                DataTable dtReturn = new DataTable();
                Query = "USP_T_KBANK_LS_IMPORT";

                SqlTransaction tr = SQLConn.BeginTransaction();

                try
                {
                    SqlCommand cmdInsert = new SqlCommand(Query, SQLConn);
                    cmdInsert.CommandType = CommandType.StoredProcedure;
                    cmdInsert.CommandTimeout = 3600;

                    cmdInsert.Parameters.AddWithValue("@I_DATE_START", DateStart);
                    cmdInsert.Parameters.AddWithValue("@I_DATE_END", DateEnd);
                    cmdInsert.Parameters.AddWithValue("@I_MODE", Mode);

                    cmdInsert.Transaction = tr;

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmdInsert))
                    {
                        dataAdapter.Fill(dtReturn);
                    }

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    throw new Exception(ex.Message);
                }

                return dtReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (SQLConn != null && SQLConn.State == ConnectionState.Open)
                {
                    SQLConn.Close();
                    SQLConn.Dispose();
                }
            }
        }

        public DataTable EBAO_GLS_KBANK_ImportDAL(DataTable dtImport, string DateStart, string DateEnd, string Mode, DataTable dtImportCLob)
        {
            try
            {
                SQLConn.Open();

                string Query = String.Empty;

                Query = "TRUNCATE TABLE T_KBANK_GLS_TEMP";

                SqlCommand cmdInserTemp = new SqlCommand(Query, SQLConn);
                cmdInserTemp.CommandType = CommandType.Text;

                cmdInserTemp.ExecuteNonQuery();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(GetSettingsHelper.CFRsConnectionString))
                {
                    bulkCopy.DestinationTableName = "T_KBANK_GLS_TEMP";

                    bulkCopy.WriteToServer(dtImport);
                }

                #region + Clob +
                Query = "TRUNCATE TABLE T_KBANK_GLS_TEMP_CLOB";

                SqlCommand cmdInserTempClob = new SqlCommand(Query, SQLConn);
                cmdInserTempClob.CommandType = CommandType.Text;

                cmdInserTempClob.ExecuteNonQuery();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(GetSettingsHelper.CFRsConnectionString))
                {
                    bulkCopy.DestinationTableName = "T_KBANK_GLS_TEMP_CLOB";

                    bulkCopy.WriteToServer(dtImportCLob);
                }
                #endregion

                DataTable dtReturn = new DataTable();
                Query = "USP_T_KBANK_GLS_IMPORT";

                SqlTransaction tr = SQLConn.BeginTransaction();

                try
                {
                    SqlCommand cmdInsert = new SqlCommand(Query, SQLConn);
                    cmdInsert.CommandType = CommandType.StoredProcedure;
                    cmdInsert.CommandTimeout = 3600;

                    cmdInsert.Parameters.AddWithValue("@I_DATE_START", DateStart);
                    cmdInsert.Parameters.AddWithValue("@I_DATE_END", DateEnd);
                    cmdInsert.Parameters.AddWithValue("@I_MODE", Mode);

                    cmdInsert.Transaction = tr;

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmdInsert))
                    {
                        dataAdapter.Fill(dtReturn);
                    }

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    throw new Exception(ex.Message);
                }

                return dtReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (SQLConn != null && SQLConn.State == ConnectionState.Open)
                {
                    SQLConn.Close();
                    SQLConn.Dispose();
                }
            }
        }

        public DataTable EBAO_GLS_ImportDAL(DataTable dtImport, string DateStart, string DateEnd, string Mode)
        {
            try
            {
                SQLConn.Open();

                string Query = String.Empty;

                Query = "TRUNCATE TABLE T_EBAO_GLS_TEMP";

                SqlCommand cmdInserTemp = new SqlCommand(Query, SQLConn);
                cmdInserTemp.CommandType = CommandType.Text;

                cmdInserTemp.ExecuteNonQuery();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(GetSettingsHelper.CFRsConnectionString))
                {
                    bulkCopy.DestinationTableName = "T_EBAO_GLS_TEMP";

                    bulkCopy.WriteToServer(dtImport);
                }

                DataTable dtReturn = new DataTable();
                Query = "USP_T_EBAO_GLS_IMPORT";

                SqlTransaction tr = SQLConn.BeginTransaction();

                try
                {
                    SqlCommand cmdInsert = new SqlCommand(Query, SQLConn);
                    cmdInsert.CommandType = CommandType.StoredProcedure;
                    cmdInsert.CommandTimeout = 3600;

                    cmdInsert.Parameters.AddWithValue("@I_DATE_START", DateStart);
                    cmdInsert.Parameters.AddWithValue("@I_DATE_END", DateEnd);
                    cmdInsert.Parameters.AddWithValue("@I_MODE", Mode);

                    cmdInsert.Transaction = tr;

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmdInsert))
                    {
                        dataAdapter.Fill(dtReturn);
                    }

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    throw new Exception(ex.Message);
                }

                return dtReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (SQLConn != null && SQLConn.State == ConnectionState.Open)
                {
                    SQLConn.Close();
                    SQLConn.Dispose();
                }
            }
        }

        public DataTable DMS_ImportDAL(DataTable dtImport, string DateStart, string DateEnd, string Mode)
        {
            try
            {
                SQLConn.Open();

                string Query = String.Empty;

                Query = "TRUNCATE TABLE T_DMS_TEMP";

                SqlCommand cmdInserTemp = new SqlCommand(Query, SQLConn);
                cmdInserTemp.CommandType = CommandType.Text;

                cmdInserTemp.ExecuteNonQuery();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(GetSettingsHelper.CFRsConnectionString))
                {
                    bulkCopy.DestinationTableName = "T_DMS_TEMP";

                    bulkCopy.WriteToServer(dtImport);
                }

                DataTable dtReturn = new DataTable();
                Query = "USP_T_DMS_IMPORT";

                SqlTransaction tr = SQLConn.BeginTransaction();

                try
                {
                    SqlCommand cmdInsert = new SqlCommand(Query, SQLConn);
                    cmdInsert.CommandType = CommandType.StoredProcedure;
                    cmdInsert.CommandTimeout = 3600;

                    cmdInsert.Parameters.AddWithValue("@I_DATE_START", DateStart);
                    cmdInsert.Parameters.AddWithValue("@I_DATE_END", DateEnd);
                    cmdInsert.Parameters.AddWithValue("@I_MODE", Mode);

                    cmdInsert.Transaction = tr;

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmdInsert))
                    {
                        dataAdapter.Fill(dtReturn);
                    }

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    throw new Exception(ex.Message);
                }

                return dtReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (SQLConn != null && SQLConn.State == ConnectionState.Open)
                {
                    SQLConn.Close();
                    SQLConn.Dispose();
                }
            }
        }

        public DataTable SMART_RP_ImportDAL(DataTable dtImport, string DateStart, string DateEnd, string Mode)
        {
            try
            {
                SQLConn.Open();

                string Query = String.Empty;

                Query = "TRUNCATE TABLE T_SMART_RP_TEMP";

                SqlCommand cmdInserTemp = new SqlCommand(Query, SQLConn);
                cmdInserTemp.CommandType = CommandType.Text;

                cmdInserTemp.ExecuteNonQuery();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(GetSettingsHelper.CFRsConnectionString))
                {
                    bulkCopy.DestinationTableName = "T_SMART_RP_TEMP";

                    bulkCopy.WriteToServer(dtImport);
                }

                DataTable dtReturn = new DataTable();
                Query = "USP_T_SMART_RP_IMPORT";

                SqlTransaction tr = SQLConn.BeginTransaction();

                try
                {
                    SqlCommand cmdInsert = new SqlCommand(Query, SQLConn);
                    cmdInsert.CommandType = CommandType.StoredProcedure;
                    cmdInsert.CommandTimeout = 3600;

                    cmdInsert.Parameters.AddWithValue("@I_DATE_START", DateStart);
                    cmdInsert.Parameters.AddWithValue("@I_DATE_END", DateEnd);
                    cmdInsert.Parameters.AddWithValue("@I_MODE", Mode);

                    cmdInsert.Transaction = tr;

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmdInsert))
                    {
                        dataAdapter.Fill(dtReturn);
                    }

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    throw new Exception(ex.Message);
                }

                return dtReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (SQLConn != null && SQLConn.State == ConnectionState.Open)
                {
                    SQLConn.Close();
                    SQLConn.Dispose();
                }
            }
        }

        public DataTable SAP_ImportDAL(DataTable dtImport, string DateStart, string DateEnd, string Mode)
        {
            try
            {
                SQLConn.Open();

                string Query = String.Empty;

                Query = "TRUNCATE TABLE T_SAP_TEMP";

                SqlCommand cmdInserTemp = new SqlCommand(Query, SQLConn);
                cmdInserTemp.CommandType = CommandType.Text;

                cmdInserTemp.ExecuteNonQuery();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(GetSettingsHelper.CFRsConnectionString))
                {
                    bulkCopy.DestinationTableName = "T_SAP_TEMP";

                    bulkCopy.WriteToServer(dtImport);
                }

                DataTable dtReturn = new DataTable();
                Query = "USP_T_SAP_IMPORT";

                SqlTransaction tr = SQLConn.BeginTransaction();

                try
                {
                    SqlCommand cmdInsert = new SqlCommand(Query, SQLConn);
                    cmdInsert.CommandType = CommandType.StoredProcedure;
                    cmdInsert.CommandTimeout = 3600;

                    cmdInsert.Parameters.AddWithValue("@I_DATE_START", DateStart);
                    cmdInsert.Parameters.AddWithValue("@I_DATE_END", DateEnd);
                    cmdInsert.Parameters.AddWithValue("@I_MODE", Mode);

                    cmdInsert.Transaction = tr;

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmdInsert))
                    {
                        dataAdapter.Fill(dtReturn);
                    }

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    throw new Exception(ex.Message);
                }

                return dtReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (SQLConn != null && SQLConn.State == ConnectionState.Open)
                {
                    SQLConn.Close();
                    SQLConn.Dispose();
                }
            }
        }

        public DataTable GetImportDAL(string Condition)
        {
            try
            {
                DataTable dtReturn = new DataTable();
                string Query = $"SELECT * " +
                   $"            FROM VW_T_SOURCE_IMPORT" +
                   $"            WHERE 1=1 " + Condition;

                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                {
                    dataAdapter.Fill(dtReturn);
                }

                return dtReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (SQLConn != null && SQLConn.State == ConnectionState.Open)
                {
                    SQLConn.Close();
                    SQLConn.Dispose();
                }
            }
        }

        public DataTable GetImportSAP_DAL(string Condition)
        {
            try
            {
                DataTable dtReturn = new DataTable();
                string Query = $"SELECT * " +
                   $"            FROM VW_T_SOURCE_IMPORT_SAP" +
                   $"            WHERE 1=1 " + Condition;

                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.CommandTimeout = 3600;

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                {
                    dataAdapter.Fill(dtReturn);
                }

                return dtReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (SQLConn != null && SQLConn.State == ConnectionState.Open)
                {
                    SQLConn.Close();
                    SQLConn.Dispose();
                }
            }
        }

        public VWTSourceImportData GetData(VWTSourceImportFilter filter)
        {
            VWTSourceImportData data = new VWTSourceImportData();
            data.vWTSourceImports = new List<VWTSourceImport>();
            try
            {
                string QueryFrom = @"FROM VW_T_SOURCE_IMPORT
WHERE 1=1 AND (SYSTEM_CODE = @SYSTEM_CODE OR (@SYSTEM_CODE = 'Smart RP' AND SYSTEM_CODE IN ('SMART-LS','SMART-GLS'))) AND (COL_DATE_WHERE BETWEEN @START_DATE AND @END_DATE) AND (BANK_SHORT_NAME = @BANK_SHORT_NAME OR @BANK_SHORT_NAME = '')";
                string Query = $@"SELECT * 
" + QueryFrom + $@"
ORDER BY SYSTEM_CODE, DETAIL_ID
OFFSET @SKIP*@TAKE ROWS
FETCH NEXT @TAKE ROWS ONLY
SELECT COUNT(*) TOTAL
" + QueryFrom;
                if (filter.isExport == 1)
                {
                    Query = "SELECT * " + QueryFrom;
                }
                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.CommandTimeout = 3600;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SYSTEM_CODE", filter.systemCode + string.Empty);
                cmd.Parameters.AddWithValue("@BANK_SHORT_NAME", filter.bankShortName + string.Empty);
                cmd.Parameters.AddWithValue("@START_DATE", filter.startDate);
                cmd.Parameters.AddWithValue("@END_DATE", filter.endDate);
                cmd.Parameters.AddWithValue("@TAKE", filter.take);
                cmd.Parameters.AddWithValue("@SKIP", filter.skip);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        VWTSourceImport objectData = new VWTSourceImport();
                        objectData.systemCode = reader.IsDBNull("SYSTEM_CODE") ? string.Empty : reader.GetString("SYSTEM_CODE");
                        objectData.detailId = reader.IsDBNull("DETAIL_ID") ? 0 : reader.GetInt32("DETAIL_ID");
                        //objectData.headerId = reader.IsDBNull("HEADER_ID") ? 0 : reader.GetInt32("HEADER_ID");
                        //objectData.colDateWhere = reader.IsDBNull("COL_DATE_WHERE") ? DateTime.MinValue : reader.GetDateTime("COL_DATE_WHERE");
                        objectData.reqnbcde = reader.IsDBNull("REQNBCDE") ? string.Empty : reader.GetString("REQNBCDE");
                        objectData.cheqno = reader.IsDBNull("CHEQNO") ? string.Empty : reader.GetString("CHEQNO");
                        objectData.capname = reader.IsDBNull("CAPNAME") ? string.Empty : reader.GetString("CAPNAME");
                        objectData.clntnum01 = reader.IsDBNull("CLNTNUM01") ? string.Empty : reader.GetString("CLNTNUM01");
                        objectData.reqnno = reader.IsDBNull("REQNNO") ? string.Empty : reader.GetString("REQNNO");
                        objectData.reqdate = reader.IsDBNull("REQDATE") ? string.Empty : reader.GetString("REQDATE");
                        objectData.payamt = reader.IsDBNull("PAYAMT") ? string.Empty : reader.GetString("PAYAMT");
                        objectData.tjobcode = reader.IsDBNull("TJOBCODE") ? string.Empty : reader.GetString("TJOBCODE");
                        objectData.workDate = reader.IsDBNull("WORK_DATE") ? string.Empty : reader.GetString("WORK_DATE");
                        objectData.accSts = reader.IsDBNull("ACC_STS") ? string.Empty : reader.GetString("ACC_STS");
                        objectData.reqnrev = reader.IsDBNull("REQNREV") ? string.Empty : reader.GetString("REQNREV");
                        objectData.chdrno01 = reader.IsDBNull("CHDRNO01") ? string.Empty : reader.GetString("CHDRNO01");
                        objectData.resflag = reader.IsDBNull("RESFLAG") ? string.Empty : reader.GetString("RESFLAG");
                        objectData.userid = reader.IsDBNull("USERID") ? string.Empty : reader.GetString("USERID");
                        objectData.bankCode = reader.IsDBNull("BANK_CODE") ? string.Empty : reader.GetString("BANK_CODE");
                        objectData.bankName = reader.IsDBNull("BANK_NAME") ? string.Empty : reader.GetString("BANK_NAME");
                        objectData.bankShortName = reader.IsDBNull("BANK_SHORT_NAME") ? string.Empty : reader.GetString("BANK_SHORT_NAME");
                        objectData.branchCode = reader.IsDBNull("BRANCH_CODE") ? string.Empty : reader.GetString("BRANCH_CODE");
                        objectData.branchName = reader.IsDBNull("BRANCH_NAME") ? string.Empty : reader.GetString("BRANCH_NAME");
                        objectData.acctNo = reader.IsDBNull("ACCT_NO") ? string.Empty : reader.GetString("ACCT_NO");
                        objectData.modeName = reader.IsDBNull("MODE_NAME") ? string.Empty : reader.GetString("MODE_NAME");
                        objectData.insertedBy = reader.IsDBNull("INSERTED_BY") ? string.Empty : reader.GetString("INSERTED_BY");
                        objectData.keyDate = reader.IsDBNull("KEY_DATE") ? string.Empty : reader.GetString("KEY_DATE");
                        objectData.payInDate = reader.IsDBNull("PAY_IN_DATE") ? string.Empty : reader.GetString("PAY_IN_DATE");
                        objectData.effDate = reader.IsDBNull("EFF_DATE") ? string.Empty : reader.GetString("EFF_DATE");
                        objectData.paidBy = reader.IsDBNull("PAID_BY") ? string.Empty : reader.GetString("PAID_BY");
                        objectData.paidDate = reader.IsDBNull("PAID_DATE") ? string.Empty : reader.GetString("PAID_DATE");
                        objectData.authenId = reader.IsDBNull("AUTHEN_ID") ? string.Empty : reader.GetString("AUTHEN_ID");
                        objectData.authenDate = reader.IsDBNull("AUTHEN_DATE") ? string.Empty : reader.GetString("AUTHEN_DATE");
                        objectData.collecCode = reader.IsDBNull("COLLEC_CODE") ? string.Empty : reader.GetString("COLLEC_CODE");
                        objectData.batchNo = reader.IsDBNull("BATCH_NO") ? string.Empty : reader.GetString("BATCH_NO");
                        objectData.batchId = reader.IsDBNull("BATCH_ID") ? string.Empty : reader.GetString("BATCH_ID");
                        objectData.glAccountName = reader.IsDBNull("GL_ACCOUNT_NAME") ? string.Empty : reader.GetString("GL_ACCOUNT_NAME");
                        objectData.accountingDate = reader.IsDBNull("ACCOUNTING_DATE") ? DateTime.MinValue : reader.GetDateTime("ACCOUNTING_DATE");
                        objectData.wd = reader.IsDBNull("WD") ? string.Empty : reader.GetString("WD");
                        objectData.typeor = reader.IsDBNull("TYPEOR") ? string.Empty : reader.GetString("TYPEOR");
                        objectData.bankareacode = reader.IsDBNull("BANKAREACODE") ? string.Empty : reader.GetString("BANKAREACODE");
                        objectData.reqncoy = reader.IsDBNull("REQNCOY") ? string.Empty : reader.GetString("REQNCOY");
                        data.vWTSourceImports.Add(objectData);
                    }
                    if (filter.isExport == 0)
                    {
                        reader.NextResult();
                        while (reader.Read())
                        {
                            data.total = reader.IsDBNull("TOTAL") ? 0 : reader.GetInt32("TOTAL");
                        }
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
        private static SourceImport_DAL _instance;
        public static SourceImport_DAL Instance
        {
            get
            {
                _instance = new SourceImport_DAL();
                return _instance;
            }
        }
        #endregion
    }
}