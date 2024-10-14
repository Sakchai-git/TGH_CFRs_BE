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
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.StaticFiles;
using System.Globalization;
using System.Reflection;
using System.Diagnostics;

namespace CFRs.BE.Controllers.BankReconcile.BankStatement
{
    [Route("api/auto-reconcile"), Authorize]
    [ApiController]
    public class AutoReconcileController : ControllerBase
    {

        [HttpGet]
        [Route("get")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult GetData(TAutoReconcile filter)
        {
            try
            {
                List<TAutoReconcile> data = new List<TAutoReconcile>();
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
        private List<TAutoReconcile> SetData(DateTime dateStart, DateTime dateEnd, TAutoReconcile filter)
        {
            List<MBank> mBanks = Bank_BLL.Instance.GetData(new MBank() { isActive = 1 }).ToList();
            MBank kbank = mBanks.First(it=>it.bankCode == "004");
            MBank kbank2 = (MBank)BaseHelper.CloneObject(kbank);
            kbank.kbankTypeId = 1;
            kbank.bankShortName = kbank.bankShortName + " - ขารับ";

            kbank2.bankId = kbank.bankId * 1000;
            kbank2.kbankTypeId = 2;
            kbank2.bankShortName = kbank2.bankShortName + " - ขาจ่าย";
            mBanks.Insert(2, kbank2);
            List<TAutoReconcile> data = new List<TAutoReconcile>();
            TAutoReconcile autoReconcile = new TAutoReconcile();
            if (filter.bankId != 0)
            {
                mBanks = mBanks.Where(it => it.bankId == filter.bankId).ToList();
                autoReconcile.bankId = filter.bankId;
            }
            int rowNumber = 0;
            for (DateTime item = dateStart; item >= dateEnd; item = item.AddMonths(-1))
            {
                autoReconcile = new TAutoReconcile();
                autoReconcile.bankId = filter.bankId;
                autoReconcile.year = item.Year;
                autoReconcile.monthId = item.Month;
                IEnumerable<TAutoReconcile> dataMonth = AutoReconcile_BLL.Instance.GetData(autoReconcile);
                if (dataMonth.Any())
                {
                    foreach (var bank in mBanks)
                    {
                        if (dataMonth.Any(it => it.bankId == bank.bankId))
                        {
                            TAutoReconcile aa = dataMonth.First(it => it.bankId == bank.bankId);
                            aa.historyCount = 1;
                            aa.bankShortName = bank.bankShortName;
                            aa.rowNumber = rowNumber;
                            data.Add(aa);
                            rowNumber++;
                        }
                        else
                        {
                            autoReconcile = new TAutoReconcile();
                            autoReconcile.year = item.Year;
                            autoReconcile.monthId = item.Month;
                            autoReconcile.bankId = bank.bankId;
                            autoReconcile.status = AutoReconcileStatus.NoRun.Value;
                            autoReconcile.historyCount = 0;
                            autoReconcile.bankShortName = bank.bankShortName;
                            autoReconcile.rowNumber = rowNumber;
                            autoReconcile.kbankTypeId = bank.kbankTypeId;
                            data.Add(autoReconcile);
                            rowNumber++;
                        }

                    }
                }
                else
                {
                    foreach (var bank in mBanks)
                    {
                        autoReconcile = new TAutoReconcile();
                        autoReconcile.year = item.Year;
                        autoReconcile.monthId = item.Month;
                        autoReconcile.bankId = bank.bankId;
                        autoReconcile.status = AutoReconcileStatus.NoRun.Value;
                        autoReconcile.historyCount = 0;
                        autoReconcile.bankShortName = bank.bankShortName;
                        autoReconcile.kbankTypeId = bank.kbankTypeId;
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
        public IActionResult GetDataDetail(TAutoReconcileDetail filter)
        {
            try
            {
                IEnumerable<TAutoReconcileDetail> data = AutoReconcile_BLL.Instance.GetDataDetail(filter);

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
        public IActionResult Save([FromBody] TAutoReconcile data)
        {
            try
            {
                if (data.year > 2500)
                {
                    data.year = data.year - 543;
                }
                data.remark = string.Empty;
                data = BaseHelper.SetId(data, Request.Headers.Authorization + string.Empty);
                data = AutoReconcile_BLL.Instance.Save(data);

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
        public IActionResult Run([FromBody] TAutoReconcile data)
        {
            try
            {
                if (data.year > 2500)
                {
                    data.year = data.year - 543;
                }
                data = AutoReconcile_BLL.Instance.Run(data);

                return Ok(data);
            }
            catch (Exception ex)
            {

                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }

        [HttpGet]
        [Route("export")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult Export(TAutoReconcile filter)
        {
            try
            {
                //skip=0&take=20
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                IEnumerable<RAutoReconcile> data = AutoReconcile_BLL.Instance.Export(filter);
                stopWatch.Stop();
                string FilePathSummary = $"{Directory.GetCurrentDirectory()}//FileFormat//AutoReconcile.xlsx";
                ExcelFile workbookSummary = ExcelFile.Load(FilePathSummary);
                ExcelWorksheet worksheetSummary = workbookSummary.Worksheets[0];
                DateTime date = new DateTime(filter.year, filter.monthId, 1);
                stopWatch.Start();
                worksheetSummary.Name = $"{date.ToString("MMMM yyyy", new CultureInfo("en-US"))}";
                if (data.Count() > 1)
                {
                    worksheetSummary.Rows.InsertCopy(1, data.Count() - 1, worksheetSummary.Rows[1]);
                }
                stopWatch.Stop();
                int row = 1, col = 0;
                stopWatch.Start();
                foreach (var item in data)
                {
                    //worksheetSummary.Rows[i].Height = worksheetSummary.Rows[0].Height;

                    Type myType = item.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                    col = 0;
                    foreach (PropertyInfo prop in props)
                    {
                        object propValue = prop.GetValue(item, null);
                        worksheetSummary.Rows[row].Cells[col].Value = propValue;
                        col++;
                        // Do something with propValue
                    }
                    row++;
                }
                stopWatch.Stop();
                //foreach (var item in worksheetSummary.Columns)
                //{
                //    item.AutoFit();
                //}
                string fileType = "xlsx";
                string filereport = $"{Directory.GetCurrentDirectory()}//FileUpload//1Report//AutoReconcile{DateTime.Now.ToString("yyyyMMdd-HHmmsss")}.{fileType}";

                workbookSummary.Save(filereport);

                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(filereport, out var contenttype))
                {
                    contenttype = "application/octet-stream";
                }

                var bytes = System.IO.File.ReadAllBytes(filereport);

                return File(bytes, contenttype, $"AutoReconcile เดือน {date.ToString("MMMM yyyy", new CultureInfo("en-US"))}.{fileType}");
            }
            catch (Exception ex)
            {

                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }
    }


}