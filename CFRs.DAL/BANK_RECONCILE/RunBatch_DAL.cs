using BankReconcile.DAL.ConnectionDAL;
using CFRs.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

namespace CFRs.DAL.BANK_RECONCILE
{
    public partial class RunBatch_DAL : CFRs_DAL
    {
        public IEnumerable<TRunBatch> GetData(TRunBatch filter)
        {
            //filter.isActive = 1;
            List<TRunBatch> data = new List<TRunBatch>();
            try
            {
                string Query = $"SELECT * FROM T_RUN_BATCH WHERE (YEAR = @YEAR OR @YEAR = 0) AND (MONTH_ID = @MONTH_ID OR @MONTH_ID = 0) AND (SYSTEM_ID = @SYSTEM_ID OR @SYSTEM_ID = 0)";

                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "YEAR",
                    Value = filter.year,
                });
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "MONTH_ID",
                    Value = filter.monthId,
                });
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "SYSTEM_ID",
                    Value = filter.systemId,
                });


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TRunBatch objectData = new TRunBatch();
                        objectData.id = reader.IsDBNull("ID") ? 0 : reader.GetInt32("ID");
                        objectData.year = reader.IsDBNull("YEAR") ? 0 : reader.GetInt32("YEAR");
                        objectData.monthId = reader.IsDBNull("MONTH_ID") ? 0 : reader.GetInt32("MONTH_ID");
                        objectData.systemId = reader.IsDBNull("SYSTEM_ID") ? 0 : reader.GetInt32("SYSTEM_ID");
                        objectData.status = reader.IsDBNull("STATUS") ? string.Empty : reader.GetString("STATUS");
                        objectData.remark = reader.IsDBNull("REMARK") ? string.Empty : reader.GetString("REMARK");
                        objectData.createBy = reader.IsDBNull("CREATE_BY") ? 0 : reader.GetInt32("CREATE_BY");
                        objectData.createDatetime = reader.IsDBNull("CREATE_DATETIME") ? DateTime.MinValue : reader.GetDateTime("CREATE_DATETIME");
                        objectData.updateBy = reader.IsDBNull("UPDATE_BY") ? 0 : reader.GetInt32("UPDATE_BY");
                        objectData.updateDatetime = reader.IsDBNull("UPDATE_DATETIME") ? null : reader.GetDateTime("UPDATE_DATETIME");
                        //objectData.bankShortName = reader.IsDBNull("BANK_SHORT_NAME") ? string.Empty : reader.GetString("BANK_SHORT_NAME");
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

        public IEnumerable<TRunBatchDetail> GetDataDetail(TRunBatchDetail filter)
        {
            //filter.isActive = 1;
            List<TRunBatchDetail> data = new List<TRunBatchDetail>();
            try
            {
                string Query = $"SELECT * FROM T_RUN_BATCH_DETAIL WHERE (RUN_BATCH_ID = @RUN_BATCH_ID) ORDER BY UPDATE_DATETIME DESC";

                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "RUN_BATCH_ID",
                    Value = filter.runBatchId,
                });


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TRunBatchDetail objectData = new TRunBatchDetail();
                        objectData.id = reader.IsDBNull("ID") ? 0 : reader.GetInt32("ID");
                        objectData.runBatchId = reader.IsDBNull("RUN_BATCH_ID") ? 0 : reader.GetInt32("RUN_BATCH_ID");
                        objectData.status = reader.IsDBNull("STATUS") ? string.Empty : reader.GetString("STATUS");
                        objectData.remark = reader.IsDBNull("REMARK") ? string.Empty : reader.GetString("REMARK");
                        objectData.createBy = reader.IsDBNull("CREATE_BY") ? 0 : reader.GetInt32("CREATE_BY");
                        objectData.createDatetime = reader.IsDBNull("CREATE_DATETIME") ? DateTime.MinValue : reader.GetDateTime("CREATE_DATETIME");
                        objectData.updateBy = reader.IsDBNull("UPDATE_BY") ? 0 : reader.GetInt32("UPDATE_BY");
                        objectData.updateDatetime = reader.IsDBNull("UPDATE_DATETIME") ? null : reader.GetDateTime("UPDATE_DATETIME");
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

        public bool CheckInProgress(SqlTransaction transaction, TRunBatch data)
        {
            bool count = false;
            try
            {
                string Query = $"SELECT TOP 1 1 FROM T_RUN_BATCH WHERE STATUS = 'In progress' AND YEAR = @YEAR AND MONTH_ID = @MONTH_ID AND SYSTEM_ID = @SYSTEM_ID";

                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.Transaction = transaction;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "YEAR",
                    Value = data.year,
                });
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "MONTH_ID",
                    Value = data.monthId,
                });
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "SYSTEM_ID",
                    Value = data.systemId,
                });



                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        count = true;
                    }
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            return count;

        }
        public TRunBatch Save(TRunBatch data)
        {
            //filter.isActive = 1;
            //กด Run ทุกครั้ง สถานะจะเป็น InProgress เสมอ
            data.status = RunBatchStatus.InProgress.Value;
            SQLConn.Open();
            SqlTransaction tr = SQLConn.BeginTransaction();

            try
            {
                if (CheckInProgress(tr, data))
                {
                    string statusMessage = "ระบบ " + data.systemName + " เดือน " + data.monthName + " " + data.year + " In Progress ไม่สามารถ Run ซ้ำได้";
                    var ex = new Exception(string.Format("{0}", statusMessage));
                    ex.Data.Add("401", statusMessage);  // store "3" and "Invalid Parameters"
                    throw ex;
                }

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
                data.status = RunBatchStatus.Fail.Value;
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

        public TRunBatch Run(TRunBatch data)
        {
            //filter.isActive = 1;

            SQLConn.Open();
            SqlTransaction tr = SQLConn.BeginTransaction();

            try
            {
                //RunBatch(tr, data);
                //data.status = status;
                //data.remark = reamrk;
                Update(tr, data);
                InsertDetail(tr, data);
                tr.Commit();
            }
            catch (Exception ex)
            {
                tr.Rollback();
                tr = SQLConn.BeginTransaction();
                data.status = RunBatchStatus.Fail.Value;
                data.remark = ex.Message;
                Update(tr, data);
                InsertDetail(tr, data);
                tr.Commit();
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
        private void Insert(SqlTransaction transaction, TRunBatch data)
        {
            string Query = $"INSERT INTO T_RUN_BATCH (YEAR ,MONTH_ID ,SYSTEM_ID ,STATUS ,REMARK ,CREATE_BY ,CREATE_DATETIME ,UPDATE_BY ,UPDATE_DATETIME) VALUES (@YEAR ,@MONTH_ID ,@SYSTEM_ID ,@STATUS ,@REMARK ,@USER_ID ,GETDATE() ,@USER_ID ,GETDATE()) SET @ID = SCOPE_IDENTITY()";

            SqlCommand cmd = new SqlCommand(Query, SQLConn);

            cmd.Transaction = transaction;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "YEAR",
                Value = data.year,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "MONTH_ID",
                Value = data.monthId,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "SYSTEM_ID",
                Value = data.systemId,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "STATUS",
                Value = data.status,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "REMARK",
                Value = data.remark,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "USER_ID",
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

        private void Update(SqlTransaction transaction, TRunBatch data)
        {
            string Query = $"UPDATE T_RUN_BATCH SET STATUS = @STATUS ,REMARK = @REMARK ,UPDATE_BY = @USER_ID ,UPDATE_DATETIME = GETDATE() WHERE ID = @ID";

            SqlCommand cmd = new SqlCommand(Query, SQLConn);

            cmd.Transaction = transaction;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "STATUS",
                Value = data.status,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "REMARK",
                Value = data.remark,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "USER_ID",
                Value = data.updateBy,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "@ID",
                Value = data.id,
            });

            cmd.ExecuteNonQuery();

        }



        private void InsertDetail(SqlTransaction transaction, TRunBatch data)
        {
            string Query = $"INSERT INTO T_RUN_BATCH_DETAIL (RUN_BATCH_ID ,STATUS ,REMARK ,CREATE_BY ,CREATE_DATETIME ,UPDATE_BY ,UPDATE_DATETIME) VALUES (@RUN_BATCH_ID ,@STATUS ,@REMARK ,@USER_ID ,GETDATE() ,@USER_ID ,GETDATE()) SET @ID = SCOPE_IDENTITY()";

            SqlCommand cmd = new SqlCommand(Query, SQLConn);

            cmd.Transaction = transaction;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "RUN_BATCH_ID",
                Value = data.id,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "STATUS",
                Value = data.status,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "REMARK",
                Value = data.remark,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "USER_ID",
                Value = data.updateBy,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "@ID",
                Direction = ParameterDirection.Output,
                SqlDbType = SqlDbType.Int
            });

            cmd.ExecuteNonQuery();

        }

        //private void RunBatch(SqlTransaction transaction, TRunBatch data)
        //{
        //    string Query = $"USP_T_RUN_BATCH";

        //    SqlCommand cmd = new SqlCommand(Query, SQLConn);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Transaction = transaction;
        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(new SqlParameter()
        //    {
        //        ParameterName = "@I_MONTH_ID",
        //        Value = data.monthId,
        //    });
        //    cmd.Parameters.Add(new SqlParameter()
        //    {
        //        ParameterName = "@I_YEAR",
        //        Value = data.year,
        //    });
        //    cmd.Parameters.Add(new SqlParameter()
        //    {
        //        ParameterName = "@I_USER_ID",
        //        Value = data.updateBy,
        //    });
        //    cmd.Parameters.Add(new SqlParameter()
        //    {
        //        ParameterName = "@I_SYSTEM_ID",
        //        Value = data.systemId,
        //    });

        //    cmd.ExecuteNonQuery();

        //}


        #region + Instance +
        private static RunBatch_DAL? _instance;
        public static RunBatch_DAL Instance
        {
            get
            {
                _instance = new RunBatch_DAL();
                return _instance;
            }
        }
        #endregion
    }
}
