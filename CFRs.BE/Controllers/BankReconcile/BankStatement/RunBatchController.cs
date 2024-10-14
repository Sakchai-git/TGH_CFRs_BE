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
using CFRs.BE.Batch;
using System.Globalization;
using System.Net;

namespace CFRs.BE.Controllers.BankReconcile.BankStatement
{
    [Route("api/run-batch"), Authorize]
    [ApiController]
    public class RunBatchController : ControllerBase
    {

        [HttpGet]
        [Route("get")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult GetData(TRunBatch filter)
        {
            try
            {
                List<TRunBatch> data = new List<TRunBatch>();
                if (filter.year > 2500)
                {
                    filter.year = filter.year - 543;
                }
                if (filter.year == 0)
                {
                    DateTime dateStart = DateTime.Today;
                    DateTime dateEnd = dateStart.AddMonths(-3);
                    data = SetData(dateStart, dateEnd, filter);

                }
                else if (filter.monthId == 0)
                {
                    DateTime dateStart = new DateTime(filter.year, 12, 1);
                    DateTime dateEnd = new DateTime(filter.year, 1, 1);
                    data = SetData(dateStart, dateEnd, filter);
                }
                else
                {
                    DateTime dateStart = new DateTime(filter.year, filter.monthId, 1);
                    DateTime dateEnd = dateStart;
                    data = SetData(dateStart, dateEnd, filter);
                }
                return Ok(data);
            }
            catch (Exception ex)
            {

                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }
        private List<TRunBatch> SetData(DateTime dateStart, DateTime dateEnd, TRunBatch filter)
        {
            IEnumerable<MSystem> mSystems = System_BLL.Instance.GetData(new MSystem() { isActive = 1 });
            //bool inProgress = RunBatch_BLL.Instance.CheckInProgress();
            List<TRunBatch> data = new List<TRunBatch>();
            TRunBatch autoReconcile = new TRunBatch();
            if (filter.systemId != 0)
            {
                mSystems = mSystems.Where(it => it.id == filter.systemId);
                autoReconcile.systemId = filter.systemId;
            }
            int rowNumber = 0;
            for (DateTime item = dateStart; item >= dateEnd; item = item.AddMonths(-1))
            {
                autoReconcile = new TRunBatch();
                autoReconcile.systemId = filter.systemId;
                autoReconcile.year = item.Year;
                autoReconcile.monthId = item.Month;
                IEnumerable<TRunBatch> dataMonth = RunBatch_BLL.Instance.GetData(autoReconcile);
                if (dataMonth.Any())
                {
                    foreach (var system in mSystems)
                    {
                        if (dataMonth.Any(it => it.systemId == system.id))
                        {
                            TRunBatch aa = dataMonth.First(it => it.systemId == system.id);
                            aa.historyCount = 1;
                            aa.systemName = system.name;
                            aa.rowNumber = rowNumber;
                            data.Add(aa);
                            rowNumber++;
                        }
                        else
                        {
                            autoReconcile = new TRunBatch();
                            autoReconcile.year = item.Year;
                            autoReconcile.monthId = item.Month;
                            autoReconcile.systemId = system.id;
                            autoReconcile.status = RunBatchStatus.NoRun.Value;
                            autoReconcile.historyCount = 0;
                            autoReconcile.systemName = system.name;
                            autoReconcile.rowNumber = rowNumber;
                            data.Add(autoReconcile);
                            rowNumber++;
                        }

                    }
                }
                else
                {
                    foreach (var system in mSystems)
                    {
                        autoReconcile = new TRunBatch();
                        autoReconcile.year = item.Year;
                        autoReconcile.monthId = item.Month;
                        autoReconcile.systemId = system.id;
                        autoReconcile.status = RunBatchStatus.NoRun.Value;
                        autoReconcile.historyCount = 0;
                        autoReconcile.systemName = system.name;
                        autoReconcile.rowNumber = rowNumber;
                        data.Add(autoReconcile);
                        rowNumber++;
                    }

                }

            }
            return data;
        }

        [HttpGet]
        [Route("get-detail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult GetDataDetail(TRunBatchDetail filter)
        {
            try
            {
                IEnumerable<TRunBatchDetail> data = RunBatch_BLL.Instance.GetDataDetail(filter);

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
        public IActionResult Save([FromBody] TRunBatch data)
        {
            try
            {
                if (data.year > 2500)
                {
                    data.year = data.year - 543;
                }
                data = BaseHelper.SetId(data, Request.Headers.Authorization + string.Empty);

                data = RunBatch_BLL.Instance.Save(data);

                return Ok(data);
            }
            catch (Exception ex)
            {

                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }

        [HttpPost]
        [Route("run")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult Run([FromBody] TRunBatch data)
        {
            TRunBatch dataSave = data;
            try
            {
                if (data.year > 2500)
                {
                    data.year = data.year - 543;
                }
                DateTime dateStart = new DateTime(data.year, data.monthId, 1);
                DateTime dateEnd = dateStart.AddMonths(1).AddDays(-1);
                string dateS = dateStart.ToString("yyyyMMdd", new CultureInfo("en-US"));
                string dateE = dateEnd.ToString("yyyyMMdd", new CultureInfo("en-US"));
                switch (data.systemId)
                {
                    case 1: new Batch_DMS().Get(dateS, dateE); break;
                    case 2: new Batch_EBAO_GLS().Get(dateS, dateE); new Batch_KBANK_GLS().Get(dateS, dateE); break;
                    case 3: new Batch_EBAO_LS().Get(dateS, dateE); new Batch_KBANK_LS().Get(dateS, dateE); 
                        new Batch_EBAO_LS_RECEIVE().Get(dateS, dateE); new Batch_Group_Kru().Get(dateS, dateE); 
                        break;
                    case 4: new Batch_EBAO_LS_RECEIVE().Get(dateS, dateE); break;
                    case 5: new Batch_Group_Kru().Get(dateS, dateE); break;
                    case 6: new Batch_SAP().Get(dateS, dateE); break;
                    case 7: new Batch_SMART_RP().Get(dateS, dateE); break;
                    default:
                        break;
                }
                data.status = RunBatchStatus.Completed.Value;
                data.remark = string.Empty;
                data = RunBatch_BLL.Instance.Run(data);

                return Ok(data);
            }
            catch (Exception ex)
            {

                dataSave.status = RunBatchStatus.Fail.Value;
                dataSave.remark = ex.Message;
                dataSave = RunBatch_BLL.Instance.Run(dataSave);
                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }
    }
}