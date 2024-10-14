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
    public partial class UserGroup_DAL : CFRs_DAL
    {
        public IEnumerable<MUserGroup> GetData(MUserGroup filter)
        {
            List<MUserGroup> data = new List<MUserGroup>();

            try
            {
                string Query = @$"SELECT M_USER_GROUP.*,CREATEBY.FIRST_NAME + ' ' + CREATEBY.LAST_NAME CREATE_BY_NAME,UPDATEBY.FIRST_NAME + ' ' + UPDATEBY.LAST_NAME UPDATE_BY_NAME FROM M_USER_GROUP 
LEFT JOIN M_USER CREATEBY ON M_USER_GROUP.CREATE_BY = CREATEBY.USER_ID 
LEFT JOIN M_USER UPDATEBY ON M_USER_GROUP.UPDATE_BY = UPDATEBY.USER_ID 
WHERE 1=1 AND (M_USER_GROUP.IS_ACTIVE = @IS_ACTIVE OR @IS_ACTIVE = -1)";

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
                        MUserGroup objectData = new MUserGroup();
                        objectData.id = reader.IsDBNull("ID") ? 0 : reader.GetInt32("ID");
                        objectData.name = reader.IsDBNull("NAME") ? string.Empty : reader.GetString("NAME");
                        objectData.isActive = reader.IsDBNull("IS_ACTIVE") ? 0 : reader.GetInt32("IS_ACTIVE");
                        objectData.createBy = reader.IsDBNull("CREATE_BY") ? 0 : reader.GetInt32("CREATE_BY");
                        objectData.createDatetime = reader.IsDBNull("CREATE_DATETIME") ? DateTime.MinValue : reader.GetDateTime("CREATE_DATETIME");
                        objectData.updateBy = reader.IsDBNull("UPDATE_BY") ? 0 : reader.GetInt32("UPDATE_BY");
                        objectData.updateDatetime = reader.IsDBNull("UPDATE_DATETIME") ? null : reader.GetDateTime("UPDATE_DATETIME");
                        //objectData.updateDatetimeDisplay = objectData.updateDatetime.ToString("dd/MM/yyyy HH:mm",new CultureInfo("en-US"));
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


        public MUserGroup GetDataById(MUserGroup filter)
        {
            MUserGroup data = new MUserGroup();

            try
            {
                string Query = @$"SELECT M_USER_GROUP.*,CREATEBY.FIRST_NAME + ' ' + CREATEBY.LAST_NAME CREATE_BY_NAME,UPDATEBY.FIRST_NAME + ' ' + UPDATEBY.LAST_NAME UPDATE_BY_NAME FROM M_USER_GROUP 
LEFT JOIN M_USER CREATEBY ON M_USER_GROUP.CREATE_BY = CREATEBY.USER_ID 
LEFT JOIN M_USER UPDATEBY ON M_USER_GROUP.UPDATE_BY = UPDATEBY.USER_ID  WHERE 1=1 AND M_USER_GROUP.ID=@ID";

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
                        MUserGroup objectData = data;
                        objectData.id = reader.IsDBNull("ID") ? 0 : reader.GetInt32("ID");
                        objectData.name = reader.IsDBNull("NAME") ? string.Empty : reader.GetString("NAME");
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

        public MUserGroup Save(MUserGroup data)
        {

            SQLConn.Open();
            SqlTransaction tr = SQLConn.BeginTransaction();

            try
            {
                CheckUserName(tr, data);
                if (data.id == 0)
                {
                    Insert(tr, data);
                }
                else
                {
                    Update(tr, data);
                }
                List<MUserGroupList> users = new List<MUserGroupList>();
                if (data.users != null && data.users.Any())
                {

                    foreach (var item in data.users)
                    {
                        users.Add(new MUserGroupList() { userGroupId = data.id, updateBy = data.updateBy, userId = item });
                        //item.userGroupId = data.id;
                        //item.updateBy = data.updateBy;
                    }
                    UserGroupList_DAL.Instance.Save(users, SQLConn, tr, false);
                }
                else
                {
                    users.Add(new MUserGroupList() { userGroupId = data.id, updateBy = data.updateBy });
                    UserGroupList_DAL.Instance.Delete( SQLConn, tr, users.First(), false);
                }
                if (data.permissons != null && data.permissons.Any())
                {
                    foreach (var item in data.permissons)
                    {
                        item.userGroupId = data.id;
                        item.updateBy = data.updateBy;
                    }
                    Permisson_DAL.Instance.Save(data.permissons.ToList(), SQLConn, tr);
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

        private void Insert(SqlTransaction transaction, MUserGroup data)
        {
            string Query = $"INSERT INTO M_USER_GROUP (NAME ,IS_ACTIVE,CREATE_BY ,CREATE_DATETIME ,UPDATE_BY ,UPDATE_DATETIME) VALUES (@NAME ,@IS_ACTIVE,@UPDATE_BY ,GETDATE() ,@UPDATE_BY ,GETDATE()) SET @ID = SCOPE_IDENTITY()";

            SqlCommand cmd = new SqlCommand(Query, SQLConn);

            cmd.Transaction = transaction;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "NAME",
                Value = data.name,
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

        private void Update(SqlTransaction transaction, MUserGroup data)
        {
            string Query = @$"UPDATE M_USER_GROUP SET NAME = @NAME,IS_ACTIVE = @IS_ACTIVE
,UPDATE_BY = @UPDATE_BY
,UPDATE_DATETIME = GETDATE() WHERE ID = @ID";

            SqlCommand cmd = new SqlCommand(Query, SQLConn);

            cmd.Transaction = transaction;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "NAME",
                Value = data.name,
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

        public MUserGroup UpdateIsActive0(MUserGroup data)
        {

            SQLConn.Open();
            SqlTransaction tr = SQLConn.BeginTransaction();

            try
            {
                string Query = @$"UPDATE M_USER_GROUP SET IS_ACTIVE = @IS_ACTIVE,UPDATE_BY = @UPDATE_BY,UPDATE_DATETIME = GETDATE() WHERE ID = @USER_GROUP_ID UPDATE M_USER_GROUP_LIST SET IS_ACTIVE = @IS_ACTIVE,UPDATE_BY = @UPDATE_BY,UPDATE_DATETIME = GETDATE() WHERE USER_GROUP_ID = @USER_GROUP_ID";

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
                    ParameterName = "@USER_GROUP_ID",
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

        private void CheckUserName(SqlTransaction transaction, MUserGroup data)
        {
            bool isDuplicate = false;
            string Query = $"SELECT TOP 1 * FROM M_USER_GROUP WHERE IS_ACTIVE=1 AND ID<>@ID AND (trim(LOWER(NAME))=trim(LOWER(@NAME)))";
            SqlCommand cmd = new SqlCommand(Query, SQLConn);
            cmd.Transaction = transaction;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "NAME",
                Value = data.name,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "ID",
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
                string statusMessage = "User Group Duplicate ไม่สามารถเพิ่มได้";
                var ex = new Exception(string.Format("{0}", statusMessage));
                ex.Data.Add("401", statusMessage);  // store "3" and "Invalid Parameters"
                throw ex;
            }
        }

        #region + Instance +
        private static UserGroup_DAL? _instance;
        public static UserGroup_DAL Instance
        {
            get
            {
                _instance = new UserGroup_DAL();
                return _instance;
            }
        }
        #endregion
    }
}
