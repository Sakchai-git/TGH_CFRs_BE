using BankReconcile.DAL.ConnectionDAL;
using CFRs.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.DAL.BANK_RECONCILE
{
    public partial class Menu_DAL : CFRs_DAL
    {
        public IEnumerable<MMenu> GetData(MMenu filter)
        {
            List<MMenu> data = new List<MMenu>();

            try
            {
                string Query = $"SELECT * FROM M_MENU WHERE 1=1 AND IS_ACTIVE = @IS_ACTIVE ORDER BY ROW_ORDER";

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
                        MMenu objectData = new MMenu();
                        objectData.id = reader.IsDBNull("ID") ? 0 : reader.GetInt32("ID");
                        objectData.rowOrder = reader.IsDBNull("ROW_ORDER") ? 0 : reader.GetInt32("ROW_ORDER");
                        objectData.code = reader.IsDBNull("CODE") ? string.Empty : reader.GetString("CODE");
                        objectData.name = reader.IsDBNull("NAME") ? string.Empty : reader.GetString("NAME");
                        objectData.parentMenuId = reader.IsDBNull("PARENT_MENU_ID") ? 0 : reader.GetInt32("PARENT_MENU_ID");
                        objectData.url = reader.IsDBNull("URL") ? string.Empty : reader.GetString("URL");
                        objectData.cssClass = reader.IsDBNull("CSS_CLASS") ? string.Empty : reader.GetString("CSS_CLASS");
                        objectData.imageUrl = reader.IsDBNull("IMAGE_URL") ? string.Empty : reader.GetString("IMAGE_URL");
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


        #region + Instance +
        private static Menu_DAL? _instance;
        public static Menu_DAL Instance
        {
            get
            {
                _instance = new Menu_DAL();
                return _instance;
            }
        }
        #endregion
    }
}
