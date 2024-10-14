using BankReconcile.DAL.ConnectionDAL;
using CFRs.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection.PortableExecutable;

namespace CFRs.DAL.BANK_RECONCILE
{
    public partial class System_DAL : CFRs_DAL
    {
        public IEnumerable<MSystem> GetData(MSystem filter)
        {
            //filter.isActive = 1;

            List<MSystem> data = new List<MSystem>();

            try
            {
                string Query = $"SELECT * " +
                       $"            FROM M_SYSTEM" +
                       $"            WHERE 1=1 AND IS_ACTIVE = @IS_ACTIVE";

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
                        MSystem objectData = new MSystem();
                        objectData.id = reader.IsDBNull("ID") ? 0 : reader.GetInt32("ID");
                        objectData.name = reader.IsDBNull("NAME") ? string.Empty : reader.GetString("NAME");
                        objectData.code = reader.IsDBNull("CODE") ? string.Empty : reader.GetString("CODE");
                        objectData.isActive = reader.IsDBNull("IS_ACTIVE") ? 0 : reader.GetInt32("IS_ACTIVE");
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
        private static System_DAL _instance;
        public static System_DAL Instance
        {
            get
            {
                _instance = new System_DAL();
                return _instance;
            }
        }
        #endregion
    }
}
