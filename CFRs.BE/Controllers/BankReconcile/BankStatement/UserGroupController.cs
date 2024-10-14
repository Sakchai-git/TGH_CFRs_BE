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
    [Route("api/user-group"), Authorize]
    [ApiController]
    public class UserGroupController : ControllerBase
    {

        [HttpGet]
        [Route("get")]
        public IActionResult GetData(MUserGroup filter)
        {//AutoReconcile
            try
            {
                IEnumerable<MUserGroup> data = UserGroup_BLL.Instance.GetData(filter);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }

        [HttpGet]
        [Route("get-id")]
        public IActionResult GetDataById(MUserGroup filter)
        {//AutoReconcile
            try
            {
                MUserGroup data = UserGroup_BLL.Instance.GetDataById(filter);

                data.userList = UserGroupList_BLL.Instance.GetData(new MUserGroupList() { userGroupId = filter.id });
                data.users = data.userList.Select(it => it.userId);
                data.permissons = Permisson_BLL.Instance.GetData(new MPermisson() { userGroupId = filter.id });

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }

        [HttpPost]
        [Route("save")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult Save([FromBody] MUserGroup data)
        {
            try
            {
                
                data = BaseHelper.SetId(data, Request.Headers.Authorization + string.Empty);
                
                data = UserGroup_BLL.Instance.Save(data);

                return Ok(data);
            }
            catch (Exception ex)
            {

                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }

        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult Delete([FromBody] MUserGroup data)
        {
            try
            {
                data = BaseHelper.SetId(data, Request.Headers.Authorization + string.Empty);
                data.isActive = 0;
                data = UserGroup_BLL.Instance.UpdateIsActive0(data);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }

    }
}