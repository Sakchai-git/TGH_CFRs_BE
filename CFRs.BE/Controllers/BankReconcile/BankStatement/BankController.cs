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
    [Route("api/bank"), Authorize]
    [ApiController]
    public class BankController : ControllerBase
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

        [HttpGet]
        [Route("get-id")]
        public IActionResult GetDataById(MBank filter)
        {//AutoReconcile
            try
            {
                MBank data = Bank_BLL.Instance.GetDataById(filter);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }

        [HttpPost]
        [Route("save")]
        public IActionResult Save([FromBody] MBank data)
        {
            try
            {
                data = BaseHelper.SetId(data, Request.Headers.Authorization + string.Empty);
                data = Bank_BLL.Instance.Save(data);

                return Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult Delete([FromBody] MBank data)
        {
            try
            {
                data = BaseHelper.SetId(data, Request.Headers.Authorization + string.Empty);
                data.isActive = 0;
                data = Bank_BLL.Instance.UpdateIsActive0(data);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }

    }
}