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
    public partial class Bank_DAL : CFRs_DAL
    {
        public IEnumerable<MBank> GetData(MBank filter)
        {
            List<MBank> data = new List<MBank>();

            try
            {
                string Query = @$"SELECT M_BANK.*,CREATEBY.FIRST_NAME + ' ' + CREATEBY.LAST_NAME CREATE_BY_NAME,UPDATEBY.FIRST_NAME + ' ' + UPDATEBY.LAST_NAME UPDATE_BY_NAME FROM M_BANK 
LEFT JOIN M_USER CREATEBY ON M_BANK.CREATE_BY = CREATEBY.USER_ID 
LEFT JOIN M_USER UPDATEBY ON M_BANK.UPDATE_BY = UPDATEBY.USER_ID
WHERE 1=1 AND (M_BANK.IS_ACTIVE = @IS_ACTIVE OR @IS_ACTIVE = -1)";

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
                        MBank objectData = new MBank();
                        objectData.bankId = reader.IsDBNull("BANK_ID") ? 0 : reader.GetInt32("BANK_ID");
                        objectData.bankFullName = reader.IsDBNull("BANK_FULL_NAME") ? string.Empty : reader.GetString("BANK_FULL_NAME");
                        objectData.bankShortName = reader.IsDBNull("BANK_SHORT_NAME") ? string.Empty : reader.GetString("BANK_SHORT_NAME");
                        objectData.bankCode = reader.IsDBNull("BANK_CODE") ? string.Empty : reader.GetString("BANK_CODE");
                        objectData.bankShortName2 = reader.IsDBNull("BANK_SHORT_NAME_2") ? string.Empty : reader.GetString("BANK_SHORT_NAME_2");
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


        public MBank GetDataById(MBank filter)
        {
            MBank data = new MBank();

            try
            {
                string Query = @$"SELECT M_BANK.*,CREATEBY.FIRST_NAME + ' ' + CREATEBY.LAST_NAME CREATE_BY_NAME,UPDATEBY.FIRST_NAME + ' ' + UPDATEBY.LAST_NAME UPDATE_BY_NAME FROM M_BANK 
LEFT JOIN M_USER CREATEBY ON M_BANK.CREATE_BY = CREATEBY.USER_ID 
LEFT JOIN M_USER UPDATEBY ON M_BANK.UPDATE_BY = UPDATEBY.USER_ID
WHERE M_BANK.IS_ACTIVE=1 AND (M_BANK.BANK_ID=@BANK_ID)";

                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "BANK_ID",
                    Value = filter.bankId,
                });


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MBank objectData = data;
                        objectData.bankId = reader.IsDBNull("BANK_ID") ? 0 : reader.GetInt32("BANK_ID");
                        objectData.bankFullName = reader.IsDBNull("BANK_FULL_NAME") ? string.Empty : reader.GetString("BANK_FULL_NAME");
                        objectData.bankShortName = reader.IsDBNull("BANK_SHORT_NAME") ? string.Empty : reader.GetString("BANK_SHORT_NAME");
                        objectData.bankCode = reader.IsDBNull("BANK_CODE") ? string.Empty : reader.GetString("BANK_CODE");
                        objectData.bankShortName2 = reader.IsDBNull("BANK_SHORT_NAME_2") ? string.Empty : reader.GetString("BANK_SHORT_NAME_2");
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

        public MBank Save(MBank data)
        {

            SQLConn.Open();
            SqlTransaction tr = SQLConn.BeginTransaction();

            try
            {
                CheckName(tr, data);
                if (data.bankId == 0)
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

        private void Insert(SqlTransaction transaction, MBank data)
        {
            string Query = $"INSERT INTO M_BANK (BANK_FULL_NAME ,BANK_SHORT_NAME ,BANK_CODE ,BANK_SHORT_NAME_2 ,IS_ACTIVE ,CREATE_BY ,CREATE_DATETIME ,UPDATE_BY ,UPDATE_DATETIME) VALUES (@BANK_FULL_NAME ,@BANK_SHORT_NAME ,@BANK_CODE ,@BANK_SHORT_NAME_2 ,@IS_ACTIVE ,@UPDATE_BY ,GETDATE() ,@UPDATE_BY ,GETDATE()) SET @ID = SCOPE_IDENTITY()";

            SqlCommand cmd = new SqlCommand(Query, SQLConn);

            cmd.Transaction = transaction;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "BANK_FULL_NAME",
                Value = data.bankFullName,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "BANK_SHORT_NAME",
                Value = data.bankShortName,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "BANK_CODE",
                Value = data.bankCode,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "BANK_SHORT_NAME_2",
                Value = data.bankShortName2,
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
            data.bankId = Convert.ToInt32(cmd.Parameters["@ID"].Value);

        }

        private void Update(SqlTransaction transaction, MBank data)
        {
            string Query = @$"UPDATE M_BANK SET BANK_FULL_NAME = @BANK_FULL_NAME
,BANK_SHORT_NAME = @BANK_SHORT_NAME
,BANK_CODE = @BANK_CODE
,BANK_SHORT_NAME_2 = @BANK_SHORT_NAME_2
,IS_ACTIVE = @IS_ACTIVE
,UPDATE_BY = @UPDATE_BY
,UPDATE_DATETIME = GETDATE() WHERE BANK_ID = @BANK_ID";

            SqlCommand cmd = new SqlCommand(Query, SQLConn);

            cmd.Transaction = transaction;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "BANK_FULL_NAME",
                Value = data.bankFullName,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "BANK_SHORT_NAME",
                Value = data.bankShortName,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "BANK_CODE",
                Value = data.bankCode,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "BANK_SHORT_NAME_2",
                Value = data.bankShortName2,
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
                ParameterName = "@BANK_ID",
                Value = data.bankId,
            });

            cmd.ExecuteNonQuery();

        }

        public MBank UpdateIsActive0(MBank data)
        {

            SQLConn.Open();
            SqlTransaction tr = SQLConn.BeginTransaction();

            try
            {
                string Query = @$"UPDATE M_BANK SET IS_ACTIVE = @IS_ACTIVE,UPDATE_BY = @UPDATE_BY,UPDATE_DATETIME = GETDATE() WHERE BANK_ID = @BANK_ID";

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
                    ParameterName = "@BANK_ID",
                    Value = data.bankId,
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

        private void CheckName(SqlTransaction transaction, MBank data)
        {
            bool isDuplicate = false;
            string Query = $"SELECT TOP 1 * FROM M_BANK WHERE IS_ACTIVE=1 AND BANK_ID<>@BANK_ID AND (trim(LOWER(BANK_CODE))=trim(LOWER(@BANK_CODE)) OR trim(LOWER(BANK_SHORT_NAME))=trim(LOWER(@BANK_SHORT_NAME)) OR trim(LOWER(BANK_FULL_NAME))=trim(LOWER(@BANK_FULL_NAME)) OR trim(LOWER(BANK_SHORT_NAME_2))=trim(LOWER(@BANK_SHORT_NAME_2)))";
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
                ParameterName = "BANK_FULL_NAME",
                Value = data.bankFullName,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "BANK_SHORT_NAME",
                Value = data.bankShortName,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "BANK_SHORT_NAME_2",
                Value = data.bankShortName2,
            });

            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "BANK_ID",
                Value = data.bankId,
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
                string statusMessage = "รหัส,ชื่อ หรือชื่อย่อ Duplicate ไม่สามารถเพิ่มได้";
                var ex = new Exception(string.Format("{0}", statusMessage));
                ex.Data.Add("401", statusMessage);  // store "3" and "Invalid Parameters"
                throw ex;
            }
        }

        #region + Instance +
        private static Bank_DAL? _instance;
        public static Bank_DAL Instance
        {
            get
            {
                _instance = new Bank_DAL();
                return _instance;
            }
        }
        #endregion
    }
}
