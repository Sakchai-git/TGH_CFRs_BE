using Amazon.S3.Model;
using CFRs.BE.Helper;
using CFRs.BLL.BANK_RECONCILE;
using CFRs.Model;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.StaticFiles;

namespace CFRs.BE.Controllers.BankReconcile.GetImportData
{
    public class GetImportDataController : ControllerBase
    {
        [HttpGet]
        [Route("api/BankStatement/GetImportData/Get")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult Get(string SystemCode, string BankShortName
            , string FromDate, string ToDate
            , string IsExport)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            string json = string.Empty;

            try
            {
                LogHelper.WriteLog("CFRs", "INF", "Call.. api/BankStatement/GetImportData/Get");

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                string Condition = string.Empty;
                string ConditionDateEBAO_LS = string.Empty;
                string ConditionDateEBAO_GLS = string.Empty;
                string ConditionDateDMS = string.Empty;
                string ConditionDateSmartRP = string.Empty;

                if (!string.IsNullOrEmpty(SystemCode))
                    Condition += $" AND SYSTEM_CODE = '{SystemCode}'";

                if (!string.IsNullOrEmpty(BankShortName))
                    Condition += $" AND BANK_SHORT_NAME = '{BankShortName}'";

                if (!string.IsNullOrEmpty(FromDate))
                {
                    Condition += $" AND COL_DATE_WHERE BETWEEN CONVERT(DATE, '{FromDate}', 103) AND CONVERT(DATE, '{ToDate}', 103)";
                }

                DataTable dtReturn = SourceImport_BLL.Instance.GetImportBLL(Condition);

                if (string.Equals(IsExport, "0"))
                {
                    json = JsonConvert.SerializeObject(dtReturn, Formatting.None);
                    response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }
                else
                {
                    dtReturn.Columns.Remove("DETAIL_ID");
                    dtReturn.Columns.Remove("HEADER_ID");
                    dtReturn.Columns.Remove("COL_DATE_WHERE");

                    ExcelFile workbook = ExcelFile.Load(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\FileFormat\\ImportData.xlsx");
                    ExcelWorksheet worksheet = workbook.Worksheets["Sheet1"];

                    int Row = 1;
                    int Column = 0;

                    for (int i = 0; i < dtReturn.Rows.Count; i++)
                    {
                        Column = 0;

                        for (int j = 0; j < dtReturn.Columns.Count; j++)
                        {
                            if (i == 0)
                            {//Write Header Text
                                worksheet.Cells[i, j].Value = dtReturn.Columns[j].ColumnName.ToUpper();
                            }

                            if (string.Equals(dtReturn.Columns[j].ColumnName, "DUE_YEAR"))
                            {
                                worksheet.Cells[Row, Column].Value = int.Parse(dtReturn.Rows[i][j].ToString());
                            }
                            else if (string.Equals(dtReturn.Columns[j].ColumnName, "PAYAMT"))
                            {
                                worksheet.Cells[Row, Column].Value = decimal.Parse(dtReturn.Rows[i][j].ToString());
                            }
                            else
                                worksheet.Cells[Row, Column].Value = dtReturn.Rows[i][j].ToString();

                            if (i > 0)
                            {//Copy Format
                                worksheet.Cells[Row, Column].Style = worksheet.Cells[Row - 1, Column].Style;
                            }

                            Column++;
                        }

                        Row++;
                    }

                    int columnCount = worksheet.CalculateMaxUsedColumns();
                    for (int i = 0; i < columnCount; i++)
                        worksheet.Columns[i].AutoFit(1, worksheet.Rows[0], worksheet.Rows[worksheet.Rows.Count - 1]);

                    string Path = this.CheckPath();

                    string DateNow = DateTime.Now.ToString("yyyyMMdd_HHmmss", new CultureInfo("en-US"));
                    string FileName = $"ImportData_" + DateNow + ".xlsx";

                    var location = new Uri($"{Request.Scheme}://{Request.Host}");
                    var url = location.AbsoluteUri;

                    workbook.Save(Path + "\\" + FileName);

                    dtReturn = new DataTable();
                    dtReturn.Columns.Add("EXPORT_PATH");

                    //dtReturn.Rows.Add(Path + "\\" + FileName);
                    dtReturn.Rows.Add($"{url}Export/{FileName}");

                    json = JsonConvert.SerializeObject(dtReturn, Formatting.None);
                    response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                return Ok(json);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("CFRs", "ERR", "Call.. api/BankStatement/GetImportData/Get exception : " + ex.Message);

                return BadRequest(BaseHelper.ReturnError(ex));
            }
            finally
            {
                LogHelper.WriteLog("CFRs", "INF", "Call Ended.. api/BankStatement/GetImportData/Get");
            }
        }

        [HttpGet]
        [Route("api/BankStatement/GetImportData/SAPGet")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult SAPGet(string SystemCode
            , string FromDate, string ToDate
            , string IsExport)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            string json = string.Empty;

            try
            {
                LogHelper.WriteLog("CFRs", "INF", "Call.. api/BankStatement/GetImportData/SAPGet");

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                string Condition = string.Empty;
                string ConditionSAP = string.Empty;

                if (!string.IsNullOrEmpty(SystemCode))
                    Condition += $" AND SYSTEM_CODE = '{SystemCode}'";

                //if (!string.IsNullOrEmpty(BankCode))
                //    Condition += $" AND BANK_CODE = '{BankCode}'";

                if (!string.IsNullOrEmpty(FromDate))
                {
                    Condition += $" AND COL_DATE_WHERE BETWEEN CONVERT(DATE, '{FromDate}', 103) AND CONVERT(DATE, '{ToDate}', 103)";
                }

                DataTable dtReturn = SourceImport_BLL.Instance.GetImportSAP_BLL(Condition);

                if (string.Equals(IsExport, "0"))
                {
                    json = JsonConvert.SerializeObject(dtReturn, Formatting.None);
                    response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }
                else
                {
                    dtReturn.Columns.Remove("DETAIL_ID");
                    dtReturn.Columns.Remove("HEADER_ID");
                    dtReturn.Columns.Remove("COL_DATE_WHERE");

                    ExcelFile workbook = ExcelFile.Load(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\FileFormat\\ImportDataSAP.xlsx");
                    ExcelWorksheet worksheet = workbook.Worksheets["Sheet1"];

                    int Row = 1;
                    int Column = 0;

                    for (int i = 0; i < dtReturn.Rows.Count; i++)
                    {
                        Column = 0;

                        for (int j = 0; j < dtReturn.Columns.Count; j++)
                        {
                            if (i == 0)
                            {//Write Header Text
                                worksheet.Cells[i, j].Value = dtReturn.Columns[j].ColumnName.ToUpper();
                            }

                            if (string.Equals(dtReturn.Columns[j].ColumnName, "DUE_YEAR"))
                            {
                                worksheet.Cells[Row, Column].Value = int.Parse(dtReturn.Rows[i][j].ToString());
                            }
                            else if (string.Equals(dtReturn.Columns[j].ColumnName, "PAYAMT"))
                            {
                                worksheet.Cells[Row, Column].Value = decimal.Parse(dtReturn.Rows[i][j].ToString());
                            }
                            else
                                worksheet.Cells[Row, Column].Value = dtReturn.Rows[i][j].ToString();

                            if (i > 0)
                            {//Copy Format
                                worksheet.Cells[Row, Column].Style = worksheet.Cells[Row - 1, Column].Style;
                            }

                            Column++;
                        }

                        Row++;
                    }

                    int columnCount = worksheet.CalculateMaxUsedColumns();
                    for (int i = 0; i < columnCount; i++)
                        worksheet.Columns[i].AutoFit(1, worksheet.Rows[0], worksheet.Rows[worksheet.Rows.Count - 1]);

                    string Path = this.CheckPath();

                    string DateNow = DateTime.Now.ToString("yyyyMMdd_HHmmss", new CultureInfo("en-US"));
                    string FileName = $"ImportDataSAP_" + DateNow + ".xlsx";

                    var location = new Uri($"{Request.Scheme}://{Request.Host}");
                    var url = location.AbsoluteUri;

                    workbook.Save(Path + "\\" + FileName);

                    dtReturn = new DataTable();
                    dtReturn.Columns.Add("EXPORT_PATH");

                    //dtReturn.Rows.Add(Path + "\\" + FileName);
                    dtReturn.Rows.Add($"{url}Export/{FileName}");

                    json = JsonConvert.SerializeObject(dtReturn, Formatting.None);
                    response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                return Ok(json);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("CFRs", "ERR", "Call.. api/BankStatement/GetImportData/SAPGet exception : " + ex.Message);

                return BadRequest(BaseHelper.ReturnError(ex));
            }
            finally
            {
                LogHelper.WriteLog("CFRs", "INF", "Call Ended.. api/BankStatement/GetImportData/SAPGet");
            }
        }


        [HttpGet]
        [Route("api/batch/gl/get")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult GetData(VWTSourceImportFilter filter)
        {
            try
            {
                //skip=0&take=20
                DateTime dateTime = new DateTime(filter.year > 2500 ? filter.year - 543 : filter.year, filter.monthId, 1);
                string startDate = dateTime.ToString("yyyy-MM-dd", new CultureInfo("en-US"));
                string ensDate = (new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month))).ToString("yyyy-MM-dd", new CultureInfo("en-US"));
                filter.startDate = startDate;
                filter.endDate = ensDate;
                VWTSourceImportData data = SourceImport_BLL.Instance.GetData(filter);

                return Ok(data);
            }
            catch (Exception ex)
            {

                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }

        [HttpGet]
        [Route("api/batch/gl/excel")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult Excel(VWTSourceImportFilter filter)
        {
            try
            {
                //skip=0&take=20
                DateTime dateTime = new DateTime(filter.year > 2500 ? filter.year - 543 : filter.year, filter.monthId, 1);
                string startDate = dateTime.ToString("yyyy-MM-dd", new CultureInfo("en-US"));
                string ensDate = (new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month))).ToString("yyyy-MM-dd", new CultureInfo("en-US"));
                filter.startDate = startDate;
                filter.endDate = ensDate;
                VWTSourceImportData data = SourceImport_BLL.Instance.GetData(filter);
                string FilePathSummary = $"{Directory.GetCurrentDirectory()}//FileFormat//GL.xlsx";
                ExcelFile workbookSummary = ExcelFile.Load(FilePathSummary);
                ExcelWorksheet worksheetSummary = workbookSummary.Worksheets[0];
                DateTime date = new DateTime(filter.year, filter.monthId, 1);
                worksheetSummary.Name = $"{date.ToString("MMMM yyyy", new CultureInfo("en-US"))}";
                if (data.vWTSourceImports.Count() > 1)
                {
                    worksheetSummary.Rows.InsertCopy(1, data.vWTSourceImports.Count() - 1, worksheetSummary.Rows[1]);
                }
                int row = 1, col = 0;
                foreach (var item in data.vWTSourceImports)
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

                string fileType = "xlsx";
                string filereport = $"{Directory.GetCurrentDirectory()}//FileUpload//1Report//GL{DateTime.Now.ToString("yyyyMMdd-HHmmsss")}.{fileType}";

                workbookSummary.Save(filereport);

                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(filereport, out var contenttype))
                {
                    contenttype = "application/octet-stream";
                }

                var bytes = System.IO.File.ReadAllBytes(filereport);

                return File(bytes, contenttype, $"{filter.systemCode} เดือน {date.ToString("MMMM yyyy", new CultureInfo("en-US"))}.{fileType}");
            }
            catch (Exception ex)
            {

                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }
        private string CheckPath()
        {
            try
            {
                string PathExport = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\" + "Export";
                if (!Directory.Exists(PathExport))
                    Directory.CreateDirectory(PathExport);

                return PathExport;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}