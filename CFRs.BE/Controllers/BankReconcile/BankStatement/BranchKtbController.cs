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
    [Route("api/branch-ktb"), Authorize]
    [ApiController]
    public class BranchKtbController : ControllerBase
    {

        [HttpGet]
        [Route("get")]
        public IActionResult GetData(MBranchKtb filter)
        {//AutoReconcile
            try
            {
                IEnumerable<MBranchKtb> data = BranchKtb_BLL.Instance.GetData(filter);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }


        [HttpGet]
        [Route("get-id")]
        public IActionResult GetDataById(MBranchKtb filter)
        {//AutoReconcile
            try
            {
                MBranchKtb data = BranchKtb_BLL.Instance.GetDataById(filter);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }

        [HttpPost]
        [Route("save")]
        public IActionResult Save([FromBody] MBranchKtb data)
        {
            try
            {
                data = BaseHelper.SetId(data, Request.Headers.Authorization + string.Empty);
                data = BranchKtb_BLL.Instance.Save(data);

                return Ok(data);
            }
            catch (Exception ex)
            {

                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult Delete([FromBody] MBranchKtb data)
        {
            try
            {
                data = BaseHelper.SetId(data, Request.Headers.Authorization + string.Empty);
                data.isActive = 0;
                data = BranchKtb_BLL.Instance.UpdateIsActive0(data);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }

    }
}