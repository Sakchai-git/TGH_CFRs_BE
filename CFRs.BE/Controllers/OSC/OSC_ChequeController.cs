﻿using BankReconcile.BLL.OSC;
using CFRs.BE.Helper;
using CFRs.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using System.Text;

namespace CFRs.BE.OSC
{
    public class OSC_ChequeController : ControllerBase
    {
        [HttpGet, Authorize]
        [Route("api/OSC/GetCheque")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult GetCheque()
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                string json = string.Empty;

                DataTable dtReturn = new DataTable();

                dtReturn = OSC_Cheque_BLL.Instance.GetChequeBLL();

                json = JsonConvert.SerializeObject(dtReturn, Formatting.None);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");

                return Ok(json);
            }
            catch (Exception ex)
            {
                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }
    }
}