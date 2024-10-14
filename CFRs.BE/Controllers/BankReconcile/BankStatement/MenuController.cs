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
using Microsoft.Extensions.Primitives;

namespace CFRs.BE.Controllers.BankReconcile.BankStatement
{
    [Route("api/menu"), Authorize]
    [ApiController]
    public class ManuController : ControllerBase
    {

        [HttpGet, Authorize]
        [Route("get")]
        public IActionResult GetData(MMenu filter)
        {//AutoReconcile
            try
            {
                
                

                //Get Id ใน Header
                IEnumerable<MMenu> data = Menu_BLL.Instance.GetData(filter);

                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }

    }
}