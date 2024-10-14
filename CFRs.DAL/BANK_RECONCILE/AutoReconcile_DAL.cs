using BankReconcile.DAL.ConnectionDAL;
using CFRs.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.DAL.BANK_RECONCILE
{
    public partial class AutoReconcile_DAL : CFRs_DAL
    {
        public IEnumerable<TAutoReconcile> GetData(TAutoReconcile filter)
        {
            //filter.isActive = 1;
            List<TAutoReconcile> data = new List<TAutoReconcile>();
            try
            {
                string Query = $@"SELECT T_AUTO_RECONCILE.*,CREATEBY.FIRST_NAME + ' ' + CREATEBY.LAST_NAME CREATE_BY_NAME,UPDATEBY.FIRST_NAME + ' ' + UPDATEBY.LAST_NAME UPDATE_BY_NAME FROM T_AUTO_RECONCILE 
LEFT JOIN M_USER CREATEBY ON T_AUTO_RECONCILE.CREATE_BY = CREATEBY.USER_ID 
LEFT JOIN M_USER UPDATEBY ON T_AUTO_RECONCILE.UPDATE_BY = UPDATEBY.USER_ID  
WHERE (YEAR = @YEAR OR @YEAR = 0) AND (MONTH_ID = @MONTH_ID OR @MONTH_ID = 0) AND (BANK_ID = @BANK_ID OR @BANK_ID = 0)";

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
                    ParameterName = "BANK_ID",
                    Value = filter.bankId,
                });


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TAutoReconcile objectData = new TAutoReconcile();
                        objectData.id = reader.IsDBNull("ID") ? 0 : reader.GetInt32("ID");
                        objectData.year = reader.IsDBNull("YEAR") ? 0 : reader.GetInt32("YEAR");
                        objectData.monthId = reader.IsDBNull("MONTH_ID") ? 0 : reader.GetInt32("MONTH_ID");
                        objectData.bankId = reader.IsDBNull("BANK_ID") ? 0 : reader.GetInt32("BANK_ID");
                        objectData.status = reader.IsDBNull("STATUS") ? string.Empty : reader.GetString("STATUS");
                        objectData.remark = reader.IsDBNull("REMARK") ? string.Empty : reader.GetString("REMARK");
                        objectData.createBy = reader.IsDBNull("CREATE_BY") ? 0 : reader.GetInt32("CREATE_BY");
                        objectData.createDatetime = reader.IsDBNull("CREATE_DATETIME") ? DateTime.MinValue : reader.GetDateTime("CREATE_DATETIME");
                        objectData.updateBy = reader.IsDBNull("UPDATE_BY") ? 0 : reader.GetInt32("UPDATE_BY");
                        objectData.updateDatetime = reader.IsDBNull("UPDATE_DATETIME") ? null : reader.GetDateTime("UPDATE_DATETIME");
                        objectData.kbankTypeId = reader.IsDBNull("KBANK_TYPE_ID") ? 0 : reader.GetInt32("KBANK_TYPE_ID");
                        objectData.createByName = reader.IsDBNull("CREATE_BY_NAME") ? string.Empty : reader.GetString("CREATE_BY_NAME");
                        objectData.createBy = reader.IsDBNull("CREATE_BY") ? 0 : reader.GetInt32("CREATE_BY");
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

        public IEnumerable<TAutoReconcileDetail> GetDataDetail(TAutoReconcileDetail filter)
        {
            //filter.isActive = 1;
            List<TAutoReconcileDetail> data = new List<TAutoReconcileDetail>();
            try
            {
                string Query = $"SELECT * FROM T_AUTO_RECONCILE_DETAIL WHERE (AUTO_RECONCILE_ID = @AUTO_RECONCILE_ID) ORDER BY UPDATE_DATETIME DESC";

                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "AUTO_RECONCILE_ID",
                    Value = filter.autoReconcileId,
                });


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TAutoReconcileDetail objectData = new TAutoReconcileDetail();
                        objectData.id = reader.IsDBNull("ID") ? 0 : reader.GetInt32("ID");
                        objectData.autoReconcileId = reader.IsDBNull("AUTO_RECONCILE_ID") ? 0 : reader.GetInt32("AUTO_RECONCILE_ID");
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

        public bool CheckInProgress()
        {
            bool count = false;
            try
            {
                string Query = $"SELECT TOP 1 1 FROM T_AUTO_RECONCILE WHERE STATUS = 'In progress'";

                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.Parameters.Clear();



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
            return count;

        }
        public TAutoReconcile Save(TAutoReconcile data)
        {
            //filter.isActive = 1;
            //กด Run ทุกครั้ง สถานะจะเป็น InProgress เสมอ
            data.status = AutoReconcileStatus.InProgress.Value;
            SQLConn.Open();
            SqlTransaction tr = SQLConn.BeginTransaction();

            try
            {
                if (CheckInProgress(tr, data))
                {
                    string statusMessage = "ธนาคาร " + data.bankShortName + " In Progress ไม่สามารถ Run ซ้ำได้ไม่สามารถ Run ธนาคาร " + data.bankShortName + " หลายเดือนพร้อมกันได้";
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
                data.status = AutoReconcileStatus.Fail.Value;
                tr.Rollback();
                throw ;
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

        public TAutoReconcile Run(TAutoReconcile data)
        {
            //filter.isActive = 1;

            SQLConn.Open();
            SqlTransaction tr = SQLConn.BeginTransaction();

            try
            {
                AutoReconcile(tr, data);
                data.status = AutoReconcileStatus.Completed.Value;
                data.remark = string.Empty;
                Update(tr, data);
                InsertDetail(tr, data);
                tr.Commit();
            }
            catch (Exception ex)
            {
                tr.Rollback();
                tr = SQLConn.BeginTransaction();
                data.status = AutoReconcileStatus.Fail.Value;
                data.remark = ex.Message;
                Update(tr, data);
                InsertDetail(tr, data);
                tr.Commit();
                throw ;
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
        private void Insert(SqlTransaction transaction, TAutoReconcile data)
        {
            string Query = $"INSERT INTO T_AUTO_RECONCILE (YEAR ,MONTH_ID ,BANK_ID ,STATUS ,REMARK ,KBANK_TYPE_ID ,CREATE_BY ,CREATE_DATETIME ,UPDATE_BY ,UPDATE_DATETIME) VALUES (@YEAR ,@MONTH_ID ,@BANK_ID ,@STATUS ,@REMARK ,@KBANK_TYPE_ID ,@USER_ID ,GETDATE() ,@USER_ID ,GETDATE()) SET @ID = SCOPE_IDENTITY()";

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
                ParameterName = "BANK_ID",
                Value = data.bankId,
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
                ParameterName = "KBANK_TYPE_ID",
                Value = data.kbankTypeId,
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

        private void Update(SqlTransaction transaction, TAutoReconcile data)
        {
            string Query = $"UPDATE T_AUTO_RECONCILE SET STATUS = @STATUS ,REMARK = @REMARK ,UPDATE_BY = @USER_ID ,UPDATE_DATETIME = GETDATE() WHERE ID = @ID";

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


        private void InsertDetail(SqlTransaction transaction, TAutoReconcile data)
        {
            string Query = $"INSERT INTO T_AUTO_RECONCILE_DETAIL (AUTO_RECONCILE_ID ,STATUS ,REMARK ,CREATE_BY ,CREATE_DATETIME ,UPDATE_BY ,UPDATE_DATETIME) VALUES (@AUTO_RECONCILE_ID ,@STATUS ,@REMARK ,@USER_ID ,GETDATE() ,@USER_ID ,GETDATE()) SET @ID = SCOPE_IDENTITY()";

            SqlCommand cmd = new SqlCommand(Query, SQLConn);

            cmd.Transaction = transaction;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "AUTO_RECONCILE_ID",
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

        private void AutoReconcile(SqlTransaction transaction, TAutoReconcile data)
        {
            string Query = $"";
            if (data.kbankTypeId > 0)
            {
                Query = $"USP_T_AUTO_RECONCILE_KBANK_TYPE_" + data.kbankTypeId;
            } else
            {
                Query = $"USP_T_AUTO_RECONCILE_" + data.bankShortName;
            }
            SqlCommand cmd = new SqlCommand(Query, SQLConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = transaction;
            cmd.CommandTimeout = 3600;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "@I_MONTH_ID",
                Value = data.monthId,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "@I_YEAR",
                Value = data.year,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "@I_USER_ID",
                Value = data.updateBy,
            });
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

        }

        public bool CheckInProgress(SqlTransaction transaction, TAutoReconcile data)
        {
            bool count = false;
            try
            {
                 string Query = $"SELECT TOP 1 1 FROM T_AUTO_RECONCILE WHERE STATUS = 'In progress' AND BANK_ID = @BANK_ID";

                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.Transaction = transaction;
                cmd.Parameters.Clear();

                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "BANK_ID",
                    Value = data.bankId,
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



        public IEnumerable<RAutoReconcile> Export(TAutoReconcile filter)
        {
            List<RAutoReconcile> data = new List<RAutoReconcile>();

            try
            {
                string Query = $"USP_R_BAAC_AUTO_RECONCILE";
                if (filter.bankId == 2)
                {
                    Query = $"USP_R_KBANK_AUTO_RECONCILE_TYPE_1";
                }
                else if (filter.bankId == 2000)
                {
                    Query = $"USP_R_KBANK_AUTO_RECONCILE_TYPE_2";
                }
                else if (filter.bankId == 3)
                {
                    Query = $"USP_R_KTB_AUTO_RECONCILE";
                }
                else if (filter.bankId == 4)
                {
                    Query = $"USP_R_UOB_AUTO_RECONCILE";
                }
                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                //cmd.Parameters.Add(new SqlParameter()
                //{
                //    ParameterName = "I_YEAR_EN",
                //    Value = filter.year,
                //});
                //cmd.Parameters.Add(new SqlParameter()
                //{
                //    ParameterName = "I_MONTH_ID",
                //    Value = filter.monthId,
                //});


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RAutoReconcile objectData = new RAutoReconcile();
                        objectData.refId = reader.IsDBNull("REF_ID") ? 0 : reader.GetInt32("REF_ID"); 
                        objectData.bankAs400 = reader.IsDBNull("BANK_AS400") ? string.Empty : reader.GetString("BANK_AS400");
                        objectData.systemCode = reader.IsDBNull("SYSTEM_CODE") ? string.Empty : reader.GetString("SYSTEM_CODE");
                        objectData.reqnbcde = reader.IsDBNull("REQNBCDE") ? string.Empty : reader.GetString("REQNBCDE");
                        objectData.cheqno = reader.IsDBNull("CHEQNO") ? string.Empty : reader.GetString("CHEQNO");
                        objectData.capname = reader.IsDBNull("CAPNAME") ? string.Empty : reader.GetString("CAPNAME");
                        objectData.clntnum01 = reader.IsDBNull("CLNTNUM01") ? string.Empty : reader.GetString("CLNTNUM01");
                        objectData.reqnno = reader.IsDBNull("REQNNO") ? string.Empty : reader.GetString("REQNNO");
                        objectData.reqdate = reader.IsDBNull("REQDATE") ? string.Empty : reader.GetString("REQDATE");
                        objectData.payamt = reader.IsDBNull("PAYAMT") ? null : reader.GetDecimal("PAYAMT");
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
                        objectData.paidBy = reader.IsDBNull("PAID_BY") ?  string.Empty : reader.GetString("PAID_BY");
                        objectData.paidDate = reader.IsDBNull("PAID_DATE") ? string.Empty : reader.GetString("PAID_DATE");
                        objectData.authenId = reader.IsDBNull("AUTHEN_ID") ? string.Empty : reader.GetString("AUTHEN_ID");
                        objectData.authenDate = reader.IsDBNull("AUTHEN_DATE") ? string.Empty : reader.GetString("AUTHEN_DATE");
                        objectData.collecCode = reader.IsDBNull("COLLEC_CODE") ? string.Empty : reader.GetString("COLLEC_CODE");
                        objectData.batchNo = reader.IsDBNull("BATCH_NO") ? string.Empty : reader.GetString("BATCH_NO");
                        objectData.batchId = reader.IsDBNull("BATCH_ID") ? string.Empty : reader.GetString("BATCH_ID");
                        objectData.glAccountName = reader.IsDBNull("GL_ACCOUNT_NAME") ? string.Empty : reader.GetString("GL_ACCOUNT_NAME");
                        objectData.accountingDate = reader.IsDBNull("ACCOUNTING_DATE") ? string.Empty : reader.GetString("ACCOUNTING_DATE");
                        objectData.wd = reader.IsDBNull("WD") ? string.Empty : reader.GetString("WD");
                        objectData.typeor = reader.IsDBNull("TYPEOR") ? string.Empty : reader.GetString("TYPEOR");
                        objectData.bankareacode = reader.IsDBNull("BANKAREACODE") ? string.Empty : reader.GetString("BANKAREACODE");
                        objectData.remark = reader.IsDBNull("REMARK") ? string.Empty : reader.GetString("REMARK");
                        objectData.isReconcile = reader.IsDBNull("IS_RECONCILE") ? null : reader.GetInt32("IS_RECONCILE");
                        objectData.reconcileDatetime = reader.IsDBNull("RECONCILE_DATETIME") ? null : reader.GetDateTime("RECONCILE_DATETIME");
                        objectData.reconcileTypeId = reader.IsDBNull("RECONCILE_TYPE_ID") ? null : reader.GetInt32("RECONCILE_TYPE_ID");
                        objectData.reconcileTypeCode = reader.IsDBNull("RECONCILE_TYPE_CODE") ? string.Empty : reader.GetString("RECONCILE_TYPE_CODE");
                        objectData.reconcileTypeName = reader.IsDBNull("RECONCILE_TYPE_NAME") ? string.Empty : reader.GetString("RECONCILE_TYPE_NAME");
                        objectData.reconcileRemark = reader.IsDBNull("RECONCILE_REMARK") ? string.Empty : reader.GetString("RECONCILE_REMARK");
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
        private static AutoReconcile_DAL? _instance;
        public static AutoReconcile_DAL Instance
        {
            get
            {
                _instance = new AutoReconcile_DAL();
                return _instance;
            }
        }
        #endregion
    }
}
