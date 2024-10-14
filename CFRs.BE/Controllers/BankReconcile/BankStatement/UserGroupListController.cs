using CFRs.BE.Helper;
using CFRs.BLL.BANK_RECONCILE;
using CFRs.ENT.BankStatement;
using Ionic.Zip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using System.Data;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System;
using CFRs.Model;
using System.Net;

namespace CFRs.BE.Controllers.BankReconcile.BankStatement
{
    [Route("api/user-group-list"), Authorize]
    [ApiController]
    public class UserGroupListController : ControllerBase
    {

        [HttpGet]
        [Route("get")]
        public IActionResult GetData(MBank filter)
        {//AutoReconcile
            try
            {
                IEnumerable<MBank> data = Bank_BLL.Instance.GetData(filter);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }

    }
}