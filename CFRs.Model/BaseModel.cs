using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CFRs.Model
{
    public class BaseModel
    {
        public int GetId(StringValues Authorization)
        {
            //Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE3MDk2MjY2NDEsImlzcyI6IlNha2NoYWkiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjU3OTQyL2xvZ2luIn0.nVSKE4X6kLK_k_VvL50b9RbYRDFPFf15lrqmUGG2BSE
            string token = Authorization.ToString();
            token = token.Replace("Bearer ","");
            var stream = token;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            return 0;
        }
        public int GetConfig(StringValues Authorization)
        {
            //var builder = WebApplication.CreateBuilder();

            //string DateStart = builder.Configuration[$"{Type}:DateStart"];

            return 0;
        }

        //public List<T> ConvertSqlDataReader<T>(SqlDataReader reader)
        //{
        //    List<T> data = new List<T>();
        //    while (reader.Read())
        //    {
        //        T item = GetItem<T>(reader);
        //        data.Add(item);
        //    }
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        T item = GetItem<T>(row);
        //        data.Add(item);
        //    }
        //    return data;
        //}
        //private T GetItem<T>(SqlDataReader dr)
        //{
        //    Type temp = typeof(T);
        //    T obj = Activator.CreateInstance<T>();


        //        foreach (PropertyInfo pro in temp.GetProperties())
        //        {
        //            if (pro.Name == dr.co)
        //                pro.SetValue(obj, dr[column.ColumnName], null);
        //            else
        //                continue;
        //        }

        //    return obj;
        //}

        //private string ChangeName(string name)
        //{

        //        string dataOutput = string.Empty;
        //        string[] columns = name.Split('_');
        //        for (int i = 0; i < columns.Length; i++)
        //        {
        //            if (i == 0)
        //            {
        //                dataOutput += columns[i].ToLower();
        //            }
        //            else
        //            {
        //                dataOutput += columns[i].First().ToString().ToUpper() + columns[i].Substring(1).ToLower();
        //            }

        //        }

        //    return dataOutput;


        //}
    }
}
