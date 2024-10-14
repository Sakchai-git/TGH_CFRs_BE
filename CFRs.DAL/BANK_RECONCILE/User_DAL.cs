using BankReconcile.DAL.ConnectionDAL;
using CFRs.Model;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Transactions;

namespace CFRs.DAL.BANK_RECONCILE
{
    public partial class User_DAL : CFRs_DAL
    {
        public IEnumerable<MUser> GetData(MUser filter)
        {
            List<MUser> data = new List<MUser>();

            try
            {
                string Query = @$"SELECT M_USER.*,M_USER.FIRST_NAME + ' ' + M_USER.LAST_NAME FULL_NAME,CREATEBY.FIRST_NAME + ' ' + CREATEBY.LAST_NAME CREATE_BY_NAME,UPDATEBY.FIRST_NAME + ' ' + UPDATEBY.LAST_NAME UPDATE_BY_NAME FROM M_USER 
LEFT JOIN M_USER CREATEBY ON M_USER.CREATE_BY = CREATEBY.USER_ID 
LEFT JOIN M_USER UPDATEBY ON M_USER.UPDATE_BY = UPDATEBY.USER_ID 
WHERE 1=1 AND (M_USER.IS_ACTIVE = @IS_ACTIVE OR @IS_ACTIVE = -1)";

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
                        MUser objectData = new MUser();
                        objectData.userId = reader.IsDBNull("USER_ID") ? 0 : reader.GetInt32("USER_ID");
                        objectData.username = reader.IsDBNull("USERNAME") ? string.Empty : reader.GetString("USERNAME");
                        objectData.firstName = reader.IsDBNull("FIRST_NAME") ? string.Empty : reader.GetString("FIRST_NAME");
                        objectData.lastName = reader.IsDBNull("LAST_NAME") ? string.Empty : reader.GetString("LAST_NAME");
                        objectData.position = reader.IsDBNull("POSITION") ? string.Empty : reader.GetString("POSITION");
                        objectData.remark = reader.IsDBNull("REMARK") ? string.Empty : reader.GetString("REMARK");
                        objectData.isActive = reader.IsDBNull("IS_ACTIVE") ? 0 : reader.GetInt32("IS_ACTIVE");
                        objectData.createBy = reader.IsDBNull("CREATE_BY") ? 0 : reader.GetInt32("CREATE_BY");
                        objectData.createDatetime = reader.IsDBNull("CREATE_DATETIME") ? DateTime.MinValue : reader.GetDateTime("CREATE_DATETIME");
                        objectData.updateBy = reader.IsDBNull("UPDATE_BY") ? 0 : reader.GetInt32("UPDATE_BY");
                        objectData.updateDatetime = reader.IsDBNull("UPDATE_DATETIME") ? null : reader.GetDateTime("UPDATE_DATETIME");
                        //objectData.updateDatetimeDisplay = objectData.updateDatetime.ToString("dd/MM/yyyy HH:mm", new CultureInfo("en-US"));
                        objectData.fullName = reader.IsDBNull("FULL_NAME") ? string.Empty : reader.GetString("FULL_NAME");
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

        public MUser GetDataById(MUser filter)
        {
            MUser data = new MUser();

            try
            {
                string Query = @$"SELECT TOP 1 M_USER.* ,M_USER.FIRST_NAME + ' ' + M_USER.LAST_NAME FULL_NAME,CREATEBY.FIRST_NAME + ' ' + CREATEBY.LAST_NAME CREATE_BY_NAME,UPDATEBY.FIRST_NAME + ' ' + UPDATEBY.LAST_NAME UPDATE_BY_NAME FROM M_USER 
LEFT JOIN M_USER CREATEBY ON M_USER.CREATE_BY = CREATEBY.USER_ID 
LEFT JOIN M_USER UPDATEBY ON M_USER.UPDATE_BY = UPDATEBY.USER_ID  WHERE M_USER.IS_ACTIVE=1 AND (M_USER.USER_ID=@USER_ID)";

                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "USER_ID",
                    Value = filter.userId,
                });


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MUser objectData = data;
                        objectData.userId = reader.IsDBNull("USER_ID") ? 0 : reader.GetInt32("USER_ID");
                        objectData.username = reader.IsDBNull("USERNAME") ? string.Empty : reader.GetString("USERNAME");
                        objectData.firstName = reader.IsDBNull("FIRST_NAME") ? string.Empty : reader.GetString("FIRST_NAME");
                        objectData.lastName = reader.IsDBNull("LAST_NAME") ? string.Empty : reader.GetString("LAST_NAME");
                        objectData.position = reader.IsDBNull("POSITION") ? string.Empty : reader.GetString("POSITION");
                        objectData.remark = reader.IsDBNull("REMARK") ? string.Empty : reader.GetString("REMARK");
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

        public MUser GetDataByUserName(MUser filter)
        {
            MUser data = new MUser();

            try
            {
                string Query = $"SELECT TOP 1 * FROM M_USER WHERE IS_ACTIVE=1 AND (trim(USERNAME)=trim(@USERNAME))";
                if (SQLConn.State != ConnectionState.Open)
                    SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "USERNAME",
                    Value = filter.username,
                });


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MUser objectData = data;
                        objectData.userId = reader.IsDBNull("USER_ID") ? 0 : reader.GetInt32("USER_ID");
                        objectData.username = reader.IsDBNull("USERNAME") ? string.Empty : reader.GetString("USERNAME");
                        objectData.firstName = reader.IsDBNull("FIRST_NAME") ? string.Empty : reader.GetString("FIRST_NAME");
                        objectData.lastName = reader.IsDBNull("LAST_NAME") ? string.Empty : reader.GetString("LAST_NAME");
                        objectData.position = reader.IsDBNull("POSITION") ? string.Empty : reader.GetString("POSITION");
                        objectData.remark = reader.IsDBNull("REMARK") ? string.Empty : reader.GetString("REMARK");
                        objectData.isActive = reader.IsDBNull("IS_ACTIVE") ? 0 : reader.GetInt32("IS_ACTIVE");
                        objectData.createBy = reader.IsDBNull("CREATE_BY") ? 0 : reader.GetInt32("CREATE_BY");
                        objectData.createDatetime = reader.IsDBNull("CREATE_DATETIME") ? DateTime.MinValue : reader.GetDateTime("CREATE_DATETIME");
                        objectData.updateBy = reader.IsDBNull("UPDATE_BY") ? 0 : reader.GetInt32("UPDATE_BY");
                        objectData.updateDatetime = reader.IsDBNull("UPDATE_DATETIME") ? null : reader.GetDateTime("UPDATE_DATETIME");
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
        public MUser Save(MUser data)
        {

            SQLConn.Open();
            SqlTransaction tr = SQLConn.BeginTransaction();

            try
            {
                CheckUserName(tr, data);
                if (data.userId == 0)
                {
                    Insert(tr, data);
                }
                else
                {
                    Update(tr, data);
                }

                List<MUserGroupList> users = new List<MUserGroupList>();
                if (data.userGroups != null && data.userGroups.Any())
                {

                    foreach (var item in data.userGroups)
                    {
                        users.Add(new MUserGroupList() { userGroupId = item, updateBy = data.updateBy, userId = data.userId });
                        //item.userGroupId = data.id;
                        //item.updateBy = data.updateBy;
                    }
                    UserGroupList_DAL.Instance.Save(users, SQLConn, tr, true);
                } else
                {
                    users.Add(new MUserGroupList() { userId = data.userId, updateBy = data.updateBy });
                    UserGroupList_DAL.Instance.Delete(SQLConn, tr, users.First(), true);
                }
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

        private void Insert(SqlTransaction transaction, MUser data)
        {
            string Query = $"INSERT INTO M_USER (USERNAME ,FIRST_NAME ,LAST_NAME ,POSITION ,REMARK ,IS_ACTIVE ,CREATE_BY ,CREATE_DATETIME ,UPDATE_BY ,UPDATE_DATETIME) VALUES (@USERNAME ,@FIRST_NAME ,@LAST_NAME ,@POSITION ,@REMARK ,@IS_ACTIVE ,@UPDATE_BY ,GETDATE() ,@UPDATE_BY ,GETDATE()) SET @ID = SCOPE_IDENTITY()";

            SqlCommand cmd = new SqlCommand(Query, SQLConn);

            cmd.Transaction = transaction;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "USERNAME",
                Value = data.username,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "FIRST_NAME",
                Value = data.firstName,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "LAST_NAME",
                Value = data.lastName,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "POSITION",
                Value = data.position,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "REMARK",
                Value = data.remark,
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
            data.userId = Convert.ToInt32(cmd.Parameters["@ID"].Value);

        }

        private void Update(SqlTransaction transaction, MUser data)
        {
            string Query = @$"UPDATE M_USER SET USERNAME = @USERNAME
,FIRST_NAME = @FIRST_NAME
,LAST_NAME = @LAST_NAME
,POSITION = @POSITION
,REMARK = @REMARK
,IS_ACTIVE = @IS_ACTIVE
,UPDATE_BY = @UPDATE_BY
,UPDATE_DATETIME = GETDATE() WHERE USER_ID = @USER_ID";

            SqlCommand cmd = new SqlCommand(Query, SQLConn);

            cmd.Transaction = transaction;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "USERNAME",
                Value = data.username,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "FIRST_NAME",
                Value = data.firstName,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "LAST_NAME",
                Value = data.lastName,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "POSITION",
                Value = data.position,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "REMARK",
                Value = data.remark,
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
                ParameterName = "@USER_ID",
                Value = data.userId,
            });

            cmd.ExecuteNonQuery();

        }

        public MUser UpdateIsActive0(MUser data)
        {

            SQLConn.Open();
            SqlTransaction tr = SQLConn.BeginTransaction();

            try
            {
                string Query = @$"UPDATE M_USER SET IS_ACTIVE = @IS_ACTIVE,UPDATE_BY = @UPDATE_BY,UPDATE_DATETIME = GETDATE() WHERE USER_ID = @USER_ID UPDATE M_USER_GROUP_LIST SET IS_ACTIVE = @IS_ACTIVE,UPDATE_BY = @UPDATE_BY,UPDATE_DATETIME = GETDATE() WHERE USER_ID = @USER_ID";

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
                    ParameterName = "@USER_ID",
                    Value = data.userId,
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

        private void CheckUserName(SqlTransaction transaction, MUser data)
        {
            bool isDuplicate = false;
            string Query = $"SELECT TOP 1 * FROM M_USER WHERE IS_ACTIVE=1 AND USER_ID<>@USER_ID AND (trim(LOWER(USERNAME))=trim(LOWER(@USERNAME)))";
            SqlCommand cmd = new SqlCommand(Query, SQLConn);
            cmd.Transaction = transaction;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "USERNAME",
                Value = data.username,
            });

            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "USER_ID",
                Value = data.userId,
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
                string statusMessage = "User Duplicate ไม่สามารถเพิ่มได้";
                var ex = new Exception(string.Format("{0}", statusMessage));
                ex.Data.Add("401", statusMessage);  // store "3" and "Invalid Parameters"
                throw ex;
            }
        }

        #region + Instance +
        private static User_DAL? _instance;
        public static User_DAL Instance
        {
            get
            {
                _instance = new User_DAL();
                return _instance;
            }
        }
        #endregion
    }
}
