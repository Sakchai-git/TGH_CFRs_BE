using Amazon.Runtime.Internal;
using CFRs.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Primitives;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace CFRs.BE.Helper
{
    public static class BaseHelper
    {
        public static string GetConfig(string name)
        {
            var builder = WebApplication.CreateBuilder();

            string value = builder.Configuration[name] + string.Empty;

            return value;

        }

        public static int GetId(string token)
        {
            //Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE3MDk2MjY2NDEsImlzcyI6IlNha2NoYWkiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjU3OTQyL2xvZ2luIn0.nVSKE4X6kLK_k_VvL50b9RbYRDFPFf15lrqmUGG2BSE

            token = token.Replace("Bearer ", "");
            var stream = token;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;
            return Convert.ToInt32(tokenS?.Claims.First(it => it.Type == "id").Value);
        }



        public static List<T> SetId<T>(List<T> data, string token)
        {
            int id = GetId(token);
            foreach (T item in data)
            {
                var type = typeof(T);
                var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                props.First(it => it.Name == "updateBy").SetValue(item, id);
            }
            return data;
        }

        public static T SetId<T>(T item, string token)
        {
            int id = GetId(token);

            var type = typeof(T);
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            props.First(it => it.Name == "updateBy").SetValue(item, id);

            return item;
        }

        public static ErrorMessage ReturnError(Exception ex)
        {
            if (ex.Data != null && ex.Data.Keys != null && ex.Data.Count > 0 && ex.Data.Keys.Count > 0)
            {
                string? statusCode = ex.Data.Keys.Cast<string>().FirstOrDefault();
                if (!string.IsNullOrEmpty(statusCode) && statusCode == "401")
                {
                    var statusMessage = ex.Data[statusCode] + string.Empty;
                    return new ErrorMessage() { statusCode = int.Parse(statusCode), message = statusMessage, stackTrace = ex.StackTrace, source = ex.Source };
                }

            }

            return new ErrorMessage() { statusCode = 500, message = ex.Message, stackTrace = ex.StackTrace, source = ex.Source };
        }

        public static void SetError401(string statusMessage)
        {
            var ex = new Exception(string.Format("{0}", statusMessage));
            ex.Data.Add("401", statusMessage);  // store "3" and "Invalid Parameters"
            throw ex;
        }

        public static object CloneObject(object objSource)
        {
            //step : 1 Get the type of source object and create a new instance of that type
            Type typeSource = objSource.GetType();
            object objTarget = Activator.CreateInstance(typeSource);
            //Step2 : Get all the properties of source object type
            PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            //Step : 3 Assign all source property to taget object 's properties
            foreach (PropertyInfo property in propertyInfo)
            {
                //Check whether property can be written to
                if (property.CanWrite)
                {
                    //Step : 4 check whether property type is value type, enum or string type
                    if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(typeof(System.String)))
                    {
                        property.SetValue(objTarget, property.GetValue(objSource, null), null);
                    }
                    //else property type is object/complex types, so need to recursively call this method until the end of the tree is reached
                    else
                    {
                        object objPropertyValue = property.GetValue(objSource, null);
                        if (objPropertyValue == null)
                        {
                            property.SetValue(objTarget, null, null);
                        }
                        else
                        {
                            property.SetValue(objTarget, CloneObject(objPropertyValue), null);
                        }
                    }
                }
            }
            return objTarget;
        }


    }
}