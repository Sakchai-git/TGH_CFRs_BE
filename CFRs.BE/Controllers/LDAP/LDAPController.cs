using CFRs.BE.Helper;
using CFRs.BLL.BANK_RECONCILE;
using CFRs.BLL.CONFIG;
using CFRs.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RestSharp;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace CFRs.BE.Controllers.LDAP
{
    public class LDAPController : ControllerBase
    {
        [HttpGet]
        [Route("api/login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult CheckAD(string Username, string Password)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                bool isLdap = Convert.ToBoolean(BaseHelper.GetConfig("Config:isLdap"));
                bool isLoginLdap = true;
                if (isLdap)
                {
                    DataTable dtConfig = ConfigBLL.Instance.GetConfigBLL($" AND CONFIG_NAME = 'LDAP'");

                    if (dtConfig.Rows.Count == 0)
                        throw new Exception("Not found config LDAP.");

                    string json = string.Empty;
                    string URL = dtConfig.Rows[0]["CONFIG_VALUE_1"] + string.Empty;
                    string Caller = dtConfig.Rows[0]["CONFIG_VALUE_2"] + string.Empty;
                    string CallerPassword = dtConfig.Rows[0]["CONFIG_VALUE_3"] + string.Empty;

                    var options = new RestClientOptions(URL)
                    {
                        MaxTimeout = -1,
                    };
                    var client = new RestClient(options);
                    var request = new RestRequest("/WSLDAP/WSLDAPUserIsExists.asmx?WSDL", Method.Post);
                    request.AddHeader("Content-Type", "application/soap+xml");
                    var body = @"<?xml version=""1.0"" encoding=""utf-8""?>" + "\n" +
                    $@"<soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">" + "\n" +
                    $@"  <soap12:Body>" + "\n" +
                    $@"    <UserIsExists xmlns=""http://tempuri.org/"">" + "\n" +
                    $@"      <userinfo>" + "\n" +
                    $@"        <caller>{Caller}</caller>" + "\n" +
                    $@"        <callerpassword>{CallerPassword}</callerpassword>" + "\n" +
                    $@"        <username>{Username}</username>" + "\n" +
                    $@"        <password>{Password}</password>" + "\n" +
                    $@"      </userinfo>" + "\n" +
                    $@"      <IsExistsUser>ossakchaip</IsExistsUser>" + "\n" +
                    $@"    </UserIsExists>" + "\n" +
                    $@"  </soap12:Body>" + "\n" +
                    $@"</soap12:Envelope>" + "\n" +
                    $@"";
                    request.AddStringBody(body, DataFormat.Xml);
                    RestResponse res = client.Execute(request);
                    if (!(res.Content + string.Empty).Contains("<status>true</status>"))
                    {
                        isLoginLdap = false;
                        return BadRequest(new { statusCode = 400, message = "User และ Password ไม่ถูกต้อง", stackTrace = "", source = "" });

                    }
                }

                if (isLoginLdap)
                {
                    MUser data = User_BLL.Instance.GetDataByUserName(new MUser() { username = Username });
                    if (data != null && !string.IsNullOrEmpty(data.firstName))
                    {
                        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("IntersoftSolution"));

                        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                        DateTime expires = DateTime.Now.AddDays(1);
                        List<Claim> claims = new List<Claim>();
                        claims.Add(new Claim("id", data.userId + string.Empty));
                        claims.Add(new Claim("userName", data.username));
                        var jwtSecurityToken = new JwtSecurityToken(
                            issuer: "Sakchai",
                        audience: "http://localhost:57942/login",
                            claims: claims,
                            expires: expires,
                            signingCredentials: signinCredentials

                        );
                        string tokens = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                        return Ok(new { token = tokens, expires = expires });
                    }
                    else
                    {
                        return BadRequest(new { statusCode = 400, message = "User ไม่มีในระบบ Bank Reconcile", stackTrace = "", source = "" });
                    }
                }

                return Ok(new { });
            }
            catch (Exception ex)
            {
                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }
    }
}