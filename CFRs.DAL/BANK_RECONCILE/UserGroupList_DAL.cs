using BankReconcile.DAL.ConnectionDAL;
using CFRs.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.DAL.BANK_RECONCILE
{
    public partial class UserGroupList_DAL : CFRs_DAL
    {
        public IEnumerable<MUserGroupList> GetData(MUserGroupList filter)
        {
            List<MUserGroupList> data = new List<MUserGroupList>();

            try
            {
                string Query = $"SELECT M_USER_GROUP_LIST.*,M_USER_GROUP.NAME USER_GROUP_NAME FROM M_USER_GROUP_LIST LEFT JOIN M_USER_GROUP ON M_USER_GROUP_LIST.USER_GROUP_ID = M_USER_GROUP.ID WHERE M_USER_GROUP_LIST.IS_ACTIVE=1 AND (USER_ID = @USER_ID OR @USER_ID = 0) AND (USER_GROUP_ID = @USER_GROUP_ID OR @USER_GROUP_ID = 0)";

                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "USER_GROUP_ID",
                    Value = filter.userGroupId,
                });
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@USER_ID",
                    Value = filter.userId,
                });


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MUserGroupList objectData = new MUserGroupList();
                        objectData.id = reader.IsDBNull("ID") ? 0 : reader.GetInt32("ID");
                        objectData.userId = reader.IsDBNull("USER_ID") ? 0 : reader.GetInt32("USER_ID");
                        objectData.userGroupId = reader.IsDBNull("USER_GROUP_ID") ? 0 : reader.GetInt32("USER_GROUP_ID");
                        objectData.isActive = reader.IsDBNull("IS_ACTIVE") ? 0 : reader.GetInt32("IS_ACTIVE");
                        objectData.createBy = reader.IsDBNull("CREATE_BY") ? 0 : reader.GetInt32("CREATE_BY");
                        objectData.createDatetime = reader.IsDBNull("CREATE_DATETIME") ? DateTime.MinValue : reader.GetDateTime("CREATE_DATETIME");
                        objectData.updateBy = reader.IsDBNull("UPDATE_BY") ? 0 : reader.GetInt32("UPDATE_BY");
                        objectData.updateDatetime = reader.IsDBNull("UPDATE_DATETIME") ? null : reader.GetDateTime("UPDATE_DATETIME");
                        objectData.userGroupName = reader.IsDBNull("USER_GROUP_NAME") ? string.Empty : reader.GetString("USER_GROUP_NAME");
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

        /// <summary>
        /// isSaveUser = true หมายถึง Save มาจากหน้า User, = false คือ save มาจากหน้า User Group
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isSaveUser"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public MUserGroupList Save(List<MUserGroupList> data, SqlConnection SQLConn, SqlTransaction tr, bool isSaveUser = true)
        {
            bool isCommit = false;
            if (SQLConn.State != ConnectionState.Open)
            {
                SQLConn.Open();
            }
            if (tr == null)
            {
                tr = SQLConn.BeginTransaction();
                isCommit = true;
            }


            try
            {
                if (data.Any())
                {
                    Delete(SQLConn,tr, data.First(), isSaveUser);
                    foreach (MUserGroupList row in data)
                    {
                        Insert(SQLConn,tr, row);
                    }
                    if (isCommit)
                    {
                        tr.Commit();
                    }
                    
                }



            }
            catch (Exception ex)
            {
                tr.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (isCommit && SQLConn.State == ConnectionState.Open)
                {
                    SQLConn.Close();
                    SQLConn.Dispose();
                }

            }
            return data.First();

        }

        private void Insert(SqlConnection SQLConn, SqlTransaction transaction, MUserGroupList data)
        {
            string Query = $"INSERT INTO M_USER_GROUP_LIST (USER_ID ,USER_GROUP_ID ,IS_ACTIVE ,CREATE_BY ,CREATE_DATETIME ,UPDATE_BY ,UPDATE_DATETIME) VALUES (@USER_ID ,@USER_GROUP_ID ,@IS_ACTIVE ,@UPDATE_BY ,GETDATE() ,@UPDATE_BY ,GETDATE())";

            SqlCommand cmd = new SqlCommand(Query, SQLConn);

            cmd.Transaction = transaction;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "USER_ID",
                Value = data.userId,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "USER_GROUP_ID",
                Value = data.userGroupId,
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

            cmd.ExecuteNonQuery();

        }

        public void Delete(SqlConnection SQLConn, SqlTransaction transaction, MUserGroupList data, bool isSaveUser = true)
        {
            string Query = @$"DELETE M_USER_GROUP_LIST WHERE (USER_ID = @USER_ID OR @USER_ID = 0) AND (USER_GROUP_ID = @USER_GROUP_ID OR @USER_GROUP_ID = 0)";

            SqlCommand cmd = new SqlCommand(Query, SQLConn);

            cmd.Transaction = transaction;
            cmd.Parameters.Clear();

            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "USER_GROUP_ID",
                Value = isSaveUser ? 0 : data.userGroupId,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "@USER_ID",
                Value = isSaveUser ? data.userId : 0,
            });

            cmd.ExecuteNonQuery();

        }

        #region + Instance +
        private static UserGroupList_DAL? _instance;
        public static UserGroupList_DAL Instance
        {
            get
            {
                _instance = new UserGroupList_DAL();
                return _instance;
            }
        }
        #endregion
    }
}
