using BankReconcile.DAL.ConnectionDAL;
using CFRs.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Transactions;

namespace CFRs.DAL.BANK_RECONCILE
{
    public partial class StatementImport_DAL : CFRs_DAL
    {
        public DataTable ImportDAL(int MonthID, int Year, int BankId
            , string PathLocal, string PathS3, int UserID, DataTable dtImport
            , string RowHeader, string RowFooter)
        {
            SQLConn.Open();
            SqlTransaction tr = SQLConn.BeginTransaction();

            try
            {
                DataTable dtReturn = new DataTable();
                string Query = "USP_T_STATEMENT_IMPORT";

                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;

                DataSet ds = new DataSet();
                dtImport.TableName = "dt";
                ds.Tables.Add(dtImport.Copy());
                ds.DataSetName = "ds";

                cmd.Transaction = tr;

                cmd.Parameters.AddWithValue("@I_MONTH_ID", MonthID);
                cmd.Parameters.AddWithValue("@I_YEAR", Year);
                cmd.Parameters.AddWithValue("@I_BANK_ID", BankId);
                cmd.Parameters.AddWithValue("@I_PATH_LOCAL", PathLocal);
                cmd.Parameters.AddWithValue("@I_PATH_S3", PathS3);
                cmd.Parameters.AddWithValue("@I_USER_ID", UserID);
                cmd.Parameters.AddWithValue("@I_XML", ds.GetXml());
                cmd.Parameters.AddWithValue("@I_ROW_HEADER", RowHeader);
                cmd.Parameters.AddWithValue("@I_ROW_FOOTER", RowFooter);

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                {
                    dataAdapter.Fill(dtReturn);
                }

                tr.Commit();

                return dtReturn;
            }
            catch (Exception ex)
            {
                tr.Rollback();
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

        public IEnumerable<TStatementImport> GetData(TStatementImport filter)
        {
            List<TStatementImport> data = new List<TStatementImport>();
            try
            {
                string Query = $"SELECT T_STATEMENT_IMPORT.* ,CREATEBY.FIRST_NAME + ' ' + CREATEBY.LAST_NAME IMPORT_NAME " +
                   $"            FROM T_STATEMENT_IMPORT\r\nLEFT JOIN M_USER CREATEBY ON T_STATEMENT_IMPORT.IMPORT_BY = CREATEBY.USER_ID " +
                   $"            WHERE T_STATEMENT_IMPORT.IS_ACTIVE = 1 AND YEAR = @YEAR AND (MONTH_ID = @MONTH_ID OR @MONTH_ID = 0) AND (BANK_ID = @BANK_ID OR @BANK_ID = 0)";

                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.CommandTimeout = 3600;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@YEAR", filter.year);
                cmd.Parameters.AddWithValue("@MONTH_ID", filter.monthId);
                cmd.Parameters.AddWithValue("@BANK_ID", filter.bankId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TStatementImport statementImport = new TStatementImport();
                        statementImport.statementImportId = reader.GetInt32("STATEMENT_IMPORT_ID");
                        statementImport.bankId = reader.GetInt32("BANK_ID");
                        statementImport.monthId = reader.GetInt32("MONTH_ID");
                        statementImport.year = reader.GetInt32("YEAR");
                        statementImport.pathLocal = reader.GetString("PATH_LOCAL") + string.Empty;
                        statementImport.pathS3 = reader.GetString("PATH_S3") + string.Empty;
                        statementImport.isActive = reader.GetInt32("IS_ACTIVE");
                        statementImport.rowHeader = reader.IsDBNull("ROW_HEADER") ? string.Empty : reader.GetString("ROW_HEADER");
                        statementImport.rowFooter = reader.IsDBNull("ROW_FOOTER") ? string.Empty : reader.GetString("ROW_FOOTER");
                        statementImport.importBy = reader.GetInt32("IMPORT_BY");
                        statementImport.importName = reader.IsDBNull("IMPORT_NAME") ? string.Empty : reader.GetString("IMPORT_NAME");
                        statementImport.importDatetime = reader.GetDateTime("IMPORT_DATETIME");
                        data.Add(statementImport);
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

        public TStatementImport Delete(TStatementImport data)
        {

            SQLConn.Open();
            SqlTransaction tr = SQLConn.BeginTransaction();

            try
            {

                string Query = $"USP_T_STATEMENT_DELETE";

                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Transaction = tr;
                cmd.CommandTimeout = 3600;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@I_MONTH_ID",
                    Value = data.monthId,
                });
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@I_YEAR_EN",
                    Value = data.year,
                });
                //cmd.Parameters.Add(new SqlParameter()
                //{
                //    ParameterName = "@I_USER_ID",
                //    Value = data.updateBy,
                //});
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@I_BANK_ID",
                    Value = data.bankId,
                });
                //cmd.Parameters.Add(new SqlParameter()
                //{
                //    ParameterName = "@I_KBANK_TYPE_ID",
                //    Value = data.kbankTypeId,
                //});

                cmd.ExecuteNonQuery();

                tr.Commit();
            }
            catch (Exception)
            {
                tr.Rollback();
                throw;
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
        private static StatementImport_DAL _instance;
        public static StatementImport_DAL Instance
        {
            get
            {
                _instance = new StatementImport_DAL();
                return _instance;
            }
        }
        #endregion
    }
}
