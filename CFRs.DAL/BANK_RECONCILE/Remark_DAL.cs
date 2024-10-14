using BankReconcile.DAL.ConnectionDAL;
using CFRs.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.DAL.BANK_RECONCILE
{
    public partial class Remark_DAL : CFRs_DAL
    {
        public IEnumerable<MRemark> GetData(MRemark filter)
        {
            List<MRemark> data = new List<MRemark>();

            try
            {
                string Query = @$"SELECT M_REMARK.*,CREATEBY.FIRST_NAME + ' ' + CREATEBY.LAST_NAME CREATE_BY_NAME,UPDATEBY.FIRST_NAME + ' ' + UPDATEBY.LAST_NAME UPDATE_BY_NAME FROM M_REMARK 
LEFT JOIN M_USER CREATEBY ON M_REMARK.CREATE_BY = CREATEBY.USER_ID 
LEFT JOIN M_USER UPDATEBY ON M_REMARK.UPDATE_BY = UPDATEBY.USER_ID
WHERE 1=1  AND (M_REMARK.IS_ACTIVE = @IS_ACTIVE OR @IS_ACTIVE = -1)";

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
                        MRemark objectData = new MRemark();
                        objectData.id = reader.IsDBNull("ID") ? 0 : reader.GetInt32("ID");
                        objectData.remcode = reader.IsDBNull("REMCODE") ? string.Empty : reader.GetString("REMCODE");
                        objectData.remdesc = reader.IsDBNull("REMDESC") ? string.Empty : reader.GetString("REMDESC");
                        objectData.isActive = reader.IsDBNull("IS_ACTIVE") ? 0 : reader.GetInt32("IS_ACTIVE");
                        objectData.createBy = reader.IsDBNull("CREATE_BY") ? 0 : reader.GetInt32("CREATE_BY");
                        objectData.createDatetime = reader.IsDBNull("CREATE_DATETIME") ? DateTime.MinValue : reader.GetDateTime("CREATE_DATETIME");
                        objectData.updateBy = reader.IsDBNull("UPDATE_BY") ? 0 : reader.GetInt32("UPDATE_BY");
                        objectData.updateDatetime = reader.IsDBNull("UPDATE_DATETIME") ? null : reader.GetDateTime("UPDATE_DATETIME");
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


        public MRemark GetDataById(MRemark filter)
        {
            MRemark data = new MRemark();

            try
            {
                string Query = @$"SELECT M_REMARK.*,CREATEBY.FIRST_NAME + ' ' + CREATEBY.LAST_NAME CREATE_BY_NAME,UPDATEBY.FIRST_NAME + ' ' + UPDATEBY.LAST_NAME UPDATE_BY_NAME FROM M_REMARK 
LEFT JOIN M_USER CREATEBY ON M_REMARK.CREATE_BY = CREATEBY.USER_ID 
LEFT JOIN M_USER UPDATEBY ON M_REMARK.UPDATE_BY = UPDATEBY.USER_ID
WHERE M_REMARK.IS_ACTIVE=1 AND (M_REMARK.ID=@ID)";

                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "ID",
                    Value = filter.id,
                });


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MRemark objectData = data;
                        objectData.id = reader.IsDBNull("ID") ? 0 : reader.GetInt32("ID");
                        objectData.remcode = reader.IsDBNull("REMCODE") ? string.Empty : reader.GetString("REMCODE");
                        objectData.remdesc = reader.IsDBNull("REMDESC") ? string.Empty : reader.GetString("REMDESC");
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

        public MRemark Save(MRemark data)
        {

            SQLConn.Open();
            SqlTransaction tr = SQLConn.BeginTransaction();

            try
            {
                if (data.id == 0)
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
        private void Insert(SqlTransaction transaction, MRemark data)
        {
            string Query = $"INSERT INTO M_REMARK (REMCODE ,REMDESC ,IS_ACTIVE ,CREATE_BY ,CREATE_DATETIME ,UPDATE_BY ,UPDATE_DATETIME) VALUES (@REMCODE ,@REMDESC ,@IS_ACTIVE ,@UPDATE_BY ,@GETDATE() ,@UPDATE_BY ,@GETDATE()) SET @ID = SCOPE_IDENTITY()";

            SqlCommand cmd = new SqlCommand(Query, SQLConn);

            cmd.Transaction = transaction;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "REMCODE",
                Value = data.remcode,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "REMDESC",
                Value = data.remdesc,
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
            data.id = Convert.ToInt32(cmd.Parameters["@ID"].Value);

        }
        private void Update(SqlTransaction transaction, MRemark data)
        {
            string Query = @$"UPDATE M_REMARK SET REMCODE = @REMCODE
,REMDESC = @REMDESC
,IS_ACTIVE = @IS_ACTIVE
,UPDATE_BY = @UPDATE_BY
,UPDATE_DATETIME = GETDATE() WHERE ID = @ID";

            SqlCommand cmd = new SqlCommand(Query, SQLConn);

            cmd.Transaction = transaction;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "REMCODE",
                Value = data.remcode,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "REMDESC",
                Value = data.remdesc,
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
                Value = data.id,
            });

            cmd.ExecuteNonQuery();

        }
        public MRemark UpdateIsActive0(MRemark data)
        {

            SQLConn.Open();
            SqlTransaction tr = SQLConn.BeginTransaction();

            try
            {
                string Query = @$"UPDATE M_REMARK SET IS_ACTIVE = @IS_ACTIVE,UPDATE_BY = @UPDATE_BY,UPDATE_DATETIME = GETDATE() WHERE ID = @ID";

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
                    ParameterName = "@ID",
                    Value = data.id,
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

        private void CheckName(SqlTransaction transaction, MRemark data)
        {
            bool isDuplicate = false;
            string Query = $"SELECT TOP 1 * FROM M_REMARK WHERE IS_ACTIVE=1 AND ID<>@ID AND (trim(LOWER(REMCODE))=trim(LOWER(@REMCODE)) OR trim(LOWER(REMDESC))=trim(LOWER(@REMDESC)))";
            SqlCommand cmd = new SqlCommand(Query, SQLConn);
            cmd.Transaction = transaction;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "REMCODE",
                Value = data.remcode,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "REMDESC",
                Value = data.remdesc,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "@ID",
                Value = data.id,
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
                string statusMessage = "รหัสหมายเหตุหรือหมายเหตุ Duplicate ไม่สามารถเพิ่มได้";
                var ex = new Exception(string.Format("{0}", statusMessage));
                ex.Data.Add("401", statusMessage);  // store "3" and "Invalid Parameters"
                throw ex;
            }
        }

        #region + Instance +
        private static Remark_DAL? _instance;
        public static Remark_DAL Instance
        {
            get
            {
                _instance = new Remark_DAL();
                return _instance;
            }
        }
        #endregion
    }
}
