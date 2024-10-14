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
using SharpCompress;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace CFRs.BE.Controllers.UserReconcile.UserStatement
{
    [Route("api/user"), Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {

        [HttpGet]
        [Route("get")]
        public IActionResult GetData(MUser filter)
        {//AutoReconcile
            try
            {
                IEnumerable<MUser> data = User_BLL.Instance.GetData(filter);

                IEnumerable<MUserGroupList> userGroup = UserGroupList_BLL.Instance.GetData(new MUserGroupList() { });
                foreach (var item in data)
                {
                    var itemGroup = userGroup.Where(it => it.userId == item.userId).OrderBy(it=>it.userGroupName);
                    if (itemGroup.Any())
                    {
                        item.userGroupName = String.Join(",", itemGroup.Select(it => it.userGroupName));
                        if (item.userGroupName.StartsWith(","))
                        {
                            item.userGroupName = item.userGroupName.Remove(0, 1);
                        }
                    }
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }

        [HttpGet]
        [Route("get-id")]
        public IActionResult GetDataById(MUser filter)
        {//AutoReconcile
            try
            {
                MUser data = User_BLL.Instance.GetDataById(filter);

                data.userGroupList = UserGroupList_BLL.Instance.GetData(new MUserGroupList() { userId = filter.userId });

                data.userGroups = data.userGroupList.Select(it => it.userGroupId);
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
        public IActionResult Save([FromBody] MUser data)
        {
            try
            {
                Validation(data);
                data = BaseHelper.SetId(data, Request.Headers.Authorization + string.Empty);

                data = User_BLL.Instance.Save(data);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }

        private void Validation(MUser data)
        {
            if (data != null)
            {
                if (string.IsNullOrEmpty(data.username))
                {
                    BaseHelper.SetError401("Username ห้ามว่าง");
                }
            }
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult Delete([FromBody] MUser data)
        {
            try
            {
                data = BaseHelper.SetId(data, Request.Headers.Authorization + string.Empty);
                data.isActive = 0;
                data = User_BLL.Instance.UpdateIsActive0(data);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }

        [HttpGet]
        [Route("get-profile")]
        public IActionResult GetProfile()
        {//AutoReconcile
            try
            {
                MUser data = User_BLL.Instance.GetDataById(new MUser() { userId = BaseHelper.GetId(Request.Headers.Authorization + string.Empty) });

                data.userGroupList = UserGroupList_BLL.Instance.GetData(new MUserGroupList() { userId = data.userId });
                data.permissons = Permisson_BLL.Instance.GetDataByUserId(data.userId);

                data.userGroupName = String.Join(",", data.userGroupList.Select(it => it.userGroupName));
                if (data.userGroupName.StartsWith(","))
                {
                    data.userGroupName = data.userGroupName.Remove(0, 1);
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }
    }

}
