using BankReconcile.DAL.ConnectionDAL;
using CFRs.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.DAL.BANK_RECONCILE
{
    public partial class Permisson_DAL : CFRs_DAL
    {
        public IEnumerable<MPermisson> GetData(MPermisson filter)
        {
            List<MPermisson> data = new List<MPermisson>();

            try
            {
                string Query = $"SELECT * FROM M_PERMISSON WHERE 1=1 AND (USER_GROUP_ID = @USER_GROUP_ID)";

                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "USER_GROUP_ID",
                    Value = filter.userGroupId,
                });


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MPermisson objectData = new MPermisson();
                        objectData.id = reader.IsDBNull("ID") ? 0 : reader.GetInt32("ID");
                        objectData.userGroupId = reader.IsDBNull("USER_GROUP_ID") ? 0 : reader.GetInt32("USER_GROUP_ID");
                        objectData.menuId = reader.IsDBNull("MENU_ID") ? 0 : reader.GetInt32("MENU_ID");
                        objectData.isView = reader.IsDBNull("IS_VIEW") ? false : reader.GetBoolean("IS_VIEW");
                        objectData.isEdit = reader.IsDBNull("IS_EDIT") ? false : reader.GetBoolean("IS_EDIT");
                        objectData.isActive = reader.IsDBNull("IS_ACTIVE") ? 0 : reader.GetInt32("IS_ACTIVE");
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

        public IEnumerable<MPermisson> GetDataByUserId(int userId)
        {
            List<MPermisson> data = new List<MPermisson>();

            try
            {
                string Query = @$"SELECT M_PERMISSON.MENU_ID,M_MENU.CODE MENU_CODE,M_MENU.NAME MENU_NAME,CAST(MAX(CAST(IS_VIEW as INT)) AS BIT) IS_VIEW,CAST(MAX(CAST(IS_EDIT as INT)) AS BIT) IS_EDIT FROM M_PERMISSON 
LEFT JOIN M_USER_GROUP_LIST ON M_PERMISSON.USER_GROUP_ID = M_USER_GROUP_LIST.USER_GROUP_ID
LEFT JOIN M_MENU ON M_PERMISSON.MENU_ID = M_MENU.ID
WHERE USER_ID = @USER_ID
GROUP BY M_PERMISSON.MENU_ID,M_MENU.CODE,M_MENU.NAME, M_MENU.ROW_ORDER
ORDER BY M_MENU.ROW_ORDER ";

                SQLConn.Open();
                SqlCommand cmd = new SqlCommand(Query, SQLConn);
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "USER_ID",
                    Value = userId,
                });


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MPermisson objectData = new MPermisson();
                        objectData.menuId = reader.IsDBNull("MENU_ID") ? 0 : reader.GetInt32("MENU_ID");
                        objectData.isView = reader.IsDBNull("IS_VIEW") ? false : reader.GetBoolean("IS_VIEW");
                        objectData.isEdit = reader.IsDBNull("IS_EDIT") ? false : reader.GetBoolean("IS_EDIT");
                        objectData.menuCode = reader.IsDBNull("MENU_CODE") ? string.Empty : reader.GetString("MENU_CODE");
                        objectData.menuName = reader.IsDBNull("MENU_NAME") ? string.Empty : reader.GetString("MENU_NAME");
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
        public MPermisson Save(List<MPermisson> data, SqlConnection SQLConn, SqlTransaction tr)
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
                    Delete(SQLConn, tr, data.First());
                    foreach (MPermisson row in data)
                    {
                        Insert(SQLConn, tr, row);
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

        private void Insert(SqlConnection SQLConn, SqlTransaction transaction, MPermisson data)
        {
            string Query = $"INSERT INTO M_PERMISSON (USER_GROUP_ID ,MENU_ID ,IS_VIEW ,IS_EDIT ,IS_ACTIVE ,CREATE_BY ,CREATE_DATETIME ,UPDATE_BY ,UPDATE_DATETIME) VALUES (@USER_GROUP_ID ,@MENU_ID ,@IS_VIEW ,@IS_EDIT ,@IS_ACTIVE ,@UPDATE_BY ,GETDATE() ,@UPDATE_BY ,GETDATE())";

            SqlCommand cmd = new SqlCommand(Query, SQLConn);

            cmd.Transaction = transaction;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "USER_GROUP_ID",
                Value = data.userGroupId,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "MENU_ID",
                Value = data.menuId,
            });

            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "IS_VIEW",
                Value = data.isView ? 1 : 0,
            });
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "IS_EDIT",
                Value = data.isEdit ? 1 : 0,
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

        private void Delete(SqlConnection SQLConn, SqlTransaction transaction, MPermisson data)
        {
            string Query = @$"DELETE M_PERMISSON WHERE (USER_GROUP_ID = @USER_GROUP_ID)";

            SqlCommand cmd = new SqlCommand(Query, SQLConn);

            cmd.Transaction = transaction;
            cmd.Parameters.Clear();

            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "USER_GROUP_ID",
                Value = data.userGroupId,
            });

            cmd.ExecuteNonQuery();

        }

        #region + Instance +
        private static Permisson_DAL? _instance;
        public static Permisson_DAL Instance
        {
            get
            {
                _instance = new Permisson_DAL();
                return _instance;
            }
        }
        #endregion
    }
}
