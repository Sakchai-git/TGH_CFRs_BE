using BankReconcile.DAL.ConnectionDAL;
using CFRs.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.DAL.BANK_RECONCILE
{
    public partial class BranchKtb_DAL : CFRs_DAL
    {
        public IEnumerable<MBranchKtb> GetData(MBranchKtb filter)
        {
            List<MBranchKtb> data = new List<MBranchKtb>();

            try
            {
                string Query = @$"SELECT M_BRANCH_KTB.*,CREATEBY.FIRST_NAME + ' ' + CREATEBY.LAST_NAME CREATE_BY_NAME,UPDATEBY.FIRST_NAME + ' ' + UPDATEBY.LAST_NAME UPDATE_BY_NAME FROM M_BRANCH_KTB 
LEFT JOIN M_USER CREATEBY ON M_BRANCH_KTB.CREATE_BY = CREATEBY.USER_ID 
LEFT JOIN M_USER UPDATEBY ON M_BRANCH_KTB.UPDATE_BY = UPDATEBY.USER_ID
WHERE 1=1  AND (M_BRANCH_KTB.IS_ACTIVE = @IS_ACTIVE OR @IS_ACTIVE = -1)";

                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "IS_ACTIVE",
                    Value = filter.isActive,
                });

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MBranchKtb objectData = new MBranchKtb();
                        objectData.branchKtbId = reader.IsDBNull("BRANCH_KTB_ID") ? 0 : reader.GetInt32("BRANCH_KTB_ID");
                        objectData.bankCode = reader.IsDBNull("BANK_CODE") ? string.Empty : reader.GetString("BANK_CODE");
                        objectData.bankName = reader.IsDBNull("BANK_NAME") ? string.Empty : reader.GetString("BANK_NAME");
                        objectData.branchName = reader.IsDBNull("BRANCH_NAME") ? string.Empty : reader.GetString("BRANCH_NAME");
                        objectData.accountNo = reader.IsDBNull("ACCOUNT_NO") ? string.Empty : reader.GetString("ACCOUNT_NO");
                        objectData.accountSap = reader.IsDBNull("ACCOUNT_SAP") ? string.Empty : reader.GetString("ACCOUNT_SAP");
                        objectData.isActive = reader.IsDBNull("IS_ACTIVE") ? 0 : reader.GetInt32("IS_ACTIVE");
                        objectData.createBy = reader.IsDBNull("CREATE_BY") ? 0 : reader.GetInt32("CREATE_BY");
                        objectData.createDatetime = reader.IsDBNull("CREATE_DATETIME") ? DateTime.MinValue : reader.GetDateTime("CREATE_DATETIME");
                        objectData.updateBy = reader.IsDBNull("UPDATE_BY") ? 0 : reader.GetInt32("UPDATE_BY");
                        objectData.updateDatetime = reader.IsDBNull("UPDATE_DATETIME") ? null : reader.GetDateTime("UPDATE_DATETIME");
                        //objectData.updateDatetimeDisplay = objectData.updateDatetime.ToString("dd/MM/yyyy HH:mm", new CultureInfo("en-US"));
                        objectData.createByName = reader.IsDBNull("CREATE_BY_NAME") ? string.Empty : reader.GetString("CREATE_BY_NAME");
                        objectData.updateByName = reader.IsDBNull("UPDATE_BY_NAME") ? string.Empty : reader.GetString("UPDATE_BY_NAME");
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


        public MBranchKtb GetDataById(MBranchKtb filter)
        {
            MBranchKtb data = new MBranchKtb();

            try
            {
                string Query = @$"SELECT M_BRANCH_KTB.*,CREATEBY.FIRST_NAME + ' ' + CREATEBY.LAST_NAME CREATE_BY_NAME,UPDATEBY.FIRST_NAME + ' ' + UPDATEBY.LAST_NAME UPDATE_BY_NAME FROM M_BRANCH_KTB 
LEFT JOIN M_USER CREATEBY ON M_BRANCH_KTB.CREATE_BY = CREATEBY.USER_ID 
LEFT JOIN M_USER UPDATEBY ON M_BRANCH_KTB.UPDATE_BY = UPDATEBY.USER_ID
WHERE M_BRANCH_KTB.IS_ACTIVE=1 AND (M_BRANCH_KTB.BRANCH_KTB_ID=@BRANCH_KTB_ID)";

                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "BRANCH_KTB_ID",
                    Value = filter.branchKtbId,
                });


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MBranchKtb objectData = data;
                        objectData.branchKtbId = reader.IsDBNull("BRANCH_KTB_ID") ? 0 : reader.GetInt32("BRANCH_KTB_ID");
                        objectData.bankCode = reader.IsDBNull("BANK_CODE") ? string.Empty : reader.GetString("BANK_CODE");
                        objectData.bankName = reader.IsDBNull("BANK_NAME") ? string.Empty : reader.GetString("BANK_NAME");
                        objectData.branchName = reader.IsDBNull("BRANCH_NAME") ? string.Empty : reader.GetString("BRANCH_NAME");
                        objectData.accountNo = reader.IsDBNull("ACCOUNT_NO") ? string.Empty : reader.GetString("ACCOUNT_NO");
                        objectData.accountSap = reader.IsDBNull("ACCOUNT_SAP") ? string.Empty : reader.GetString("ACCOUNT_SAP");
                        objectData.isActive = reader.IsDBNull("IS_ACTIVE") ? 0 : reader.GetInt32("IS_ACTIVE");
                        objectData.createBy = reader.IsDBNull("CREATE_BY") ? 0 : reader.GetInt32("CREATE_BY");
                        objectData.createDatetime = reader.IsDBNull("CREATE_DATETIME") ? DateTime.MinValue : reader.GetDateTime("CREATE_DATETIME");
                        objectData.updateBy = reader.IsDBNull("UPDATE_BY") ? 0 : reader.GetInt32("UPDATE_BY");
                        objectData.updateDatetime = reader.IsDBNull("UPDATE_DATETIME") ? null : reader.GetDateTime("UPDATE_DATETIME");
                        objectData.createByName = reader.IsDBNull("CREATE_BY_NAME") ? string.Empty : reader.GetString("CREATE_BY_NAME");
                        objectData.updateByName = reader.IsDBNull("UPDATE_BY_NAME") ? string.Empty : reader.GetString("UPDATE_BY_NAME");
                        //data.Add(objectData);
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

        public MBranchKtb Save(MBranchKtb data)
        {

            SQLConn.Open();
            SqlTransaction tr = SQLConn.BeginTransaction();

            try
            {
                CheckName(tr, data);
                if (data.branchKtbId == 0)
                {
                    Insert(tr, data);
                }
                else
                {
                    Update(tr, data);
                }

                tr.Commit();
            }
            catch (Exception ex)
            {
                tr.Rollback();
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
        private void Insert(SqlTransaction transaction, MBranchKtb data)
        {
            string Query = $"INSERT INTO M_BRANCH_KTB (BANK_CODE ,BANK_NAME ,BRANCH_NAME ,ACCOUNT_NO ,ACCOUNT_SAP,IS_ACTIVE ,CREATE_BY ,CREATE_DATETIME ,UPDATE_BY ,UPDATE_DATETIME) VALUES (@BANK_CODE ,@BANK_NAME ,@BRANCH_NAME ,@ACCOUNT_NO ,@ACCOUNT_SAP,@IS_ACTIVE ,@UPDATE_BY ,GETDATE() ,@UPDATE_BY ,GETDATE()) SET @ID = SCOPE_IDENTITY()";

            SqlCommand cmd = new SqlCommand(Query, SQLConn);

            cmd.Transaction = transaction;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "BANK_CODE",
                Value = data.bankCode,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "BANK_NAME",
                Value = data.bankName,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "BRANCH_NAME",
                Value = data.branchName,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "ACCOUNT_NO",
                Value = data.accountNo,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "ACCOUNT_SAP",
                Value = data.accountSap,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "IS_ACTIVE",
                Value = data.isActive,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "UPDATE_BY",
                Value = data.updateBy,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "@ID",
                Direction = ParameterDirection.Output,
                SqlDbType = SqlDbType.Int
            });

            cmd.ExecuteNonQuery();
            data.branchKtbId = Convert.ToInt32(cmd.Parameters["@ID"].Value);

        }
        private void Update(SqlTransaction transaction, MBranchKtb data)
        {
            string Query = @$"UPDATE M_BRANCH_KTB SET BANK_CODE = @BANK_CODE
,BANK_NAME = @BANK_NAME
,BRANCH_NAME = @BRANCH_NAME
,ACCOUNT_NO = @ACCOUNT_NO
,ACCOUNT_SAP = @ACCOUNT_SAP
,IS_ACTIVE = @IS_ACTIVE
,UPDATE_BY = @UPDATE_BY
,UPDATE_DATETIME = GETDATE() WHERE BRANCH_KTB_ID = @BRANCH_KTB_ID";

            SqlCommand cmd = new SqlCommand(Query, SQLConn);

            cmd.Transaction = transaction;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "BANK_CODE",
                Value = data.bankCode,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "BANK_NAME",
                Value = data.bankName,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "BRANCH_NAME",
                Value = data.branchName,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "ACCOUNT_NO",
                Value = data.accountNo,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "ACCOUNT_SAP",
                Value = data.accountSap,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "IS_ACTIVE",
                Value = data.isActive,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "UPDATE_BY",
                Value = data.updateBy,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "@BRANCH_KTB_ID",
                Value = data.branchKtbId,
            });

            cmd.ExecuteNonQuery();

        }
        public MBranchKtb UpdateIsActive0(MBranchKtb data)
        {

            SQLConn.Open();
            SqlTransaction tr = SQLConn.BeginTransaction();

            try
            {
                string Query = @$"UPDATE M_BRANCH_KTB SET IS_ACTIVE = @IS_ACTIVE,UPDATE_BY = @UPDATE_BY,UPDATE_DATETIME = GETDATE() WHERE BRANCH_KTB_ID = @BRANCH_KTB_ID";

                SqlCommand cmd = new SqlCommand(Query, SQLConn);

                cmd.Transaction = tr;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "IS_ACTIVE",
                    Value = data.isActive,
                });
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "UPDATE_BY",
                    Value = data.updateBy,
                });
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@BRANCH_KTB_ID",
                    Value = data.branchKtbId,
                });

                cmd.ExecuteNonQuery();
                tr.Commit();
            }
            catch (Exception ex)
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

        private void CheckName(SqlTransaction transaction, MBranchKtb data)
        {
            bool isDuplicate = false;
            string Query = $"SELECT TOP 1 * FROM M_BRANCH_KTB WHERE IS_ACTIVE=1 AND BRANCH_KTB_ID<>@BRANCH_KTB_ID AND (trim(LOWER(BANK_CODE))=trim(LOWER(@BANK_CODE)) AND trim(LOWER(ACCOUNT_NO))=trim(LOWER(@ACCOUNT_NO)))";
            SqlCommand cmd = new SqlCommand(Query, SQLConn);
            cmd.Transaction = transaction;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "BANK_CODE",
                Value = data.bankCode,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "ACCOUNT_NO",
                Value = data.accountNo,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "BRANCH_KTB_ID",
                Value = data.branchKtbId,
            });

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    isDuplicate = true;
                }
            }

            if (isDuplicate)
            {
                string statusMessage = "รหัสธนาคารและ Account No. Duplicate ไม่สามารถเพิ่มได้";
                var ex = new Exception(string.Format("{0}", statusMessage));
                ex.Data.Add("401", statusMessage);  // store "3" and "Invalid Parameters"
                throw ex;
            }
        }

        #region + Instance +
        private static BranchKtb_DAL? _instance;
        public static BranchKtb_DAL Instance
        {
            get
            {
                _instance = new BranchKtb_DAL();
                return _instance;
            }
        }
        #endregion
    }
}
