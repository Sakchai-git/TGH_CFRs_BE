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
using static System.Runtime.InteropServices.JavaScript.JSType;
using GemBox.Spreadsheet;
using System.Globalization;

namespace CFRs.BE.Controllers.BankReconcile.BankStatement
{
    [Route("api/bank-statement"), Authorize]
    [ApiController]
    public class ImportFileController : ControllerBase
    {
        [HttpPost]//,Authorize
        [Route("import")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult Import(int MonthID
            , int Year
            , int BankId
            , string BankShortName
            , int UserID
            //, [FromBody] BankStatementImportEnt[] bankStatementImportEnt
            , IFormFile[] fileUpload, [FromServices] Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            string json = string.Empty;

            try
            {
                UserID = BaseHelper.GetId(Request.Headers.Authorization + string.Empty);
                LogHelper.WriteLog("CFRs", "INF", "Call.. api/BankStatement/ImportFile/Import");
                if (Year > 2500)
                {
                    Year = Year - 543;
                }
                List<BankStatementImportEnt> bankStatementImportEnt = new List<BankStatementImportEnt>();
                foreach (var item in fileUpload)
                {
                    string[] arrFileName = item.FileName.Split('.');
                    string fileInputName = item.FileName; //fileUpload.FileName.Replace(".xlsx", DateTime.Now.ToString("_yyyyMMdd_HHmmsss") + ".xlsx");
                    string fileNameUser = item.FileName;

                    string filSave = $"{hostingEnvironment.ContentRootPath}\\FileUpload\\{fileInputName}";
                    if (System.IO.File.Exists(filSave))
                    {
                        System.IO.File.Delete(filSave);
                    }

                    using (FileStream fileStream = System.IO.File.Create(filSave))
                    {
                        item.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                    bankStatementImportEnt.Add(new BankStatementImportEnt()
                    {
                        FilePath = filSave
                    });
                }



                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                if (!string.Equals(BankShortName.ToUpper(), "BAAC")
                    && !BankShortName.ToUpper().Contains("KBANK")
                    && !string.Equals(BankShortName.ToUpper(), "KTB")
                    && !string.Equals(BankShortName.ToUpper(), "UOB"))
                {
                    LogHelper.WriteLog("CFRs", "ERR", $"Bank {BankShortName} not found logic import file.");
                    throw new Exception($"Bank {BankShortName} not found logic import file.");
                }

                DataTable dtReturn = new DataTable();
                dtReturn.Columns.Add("ZIP_FILE_NAME");
                dtReturn.Columns.Add("FILE_PATH");
                dtReturn.Columns.Add("FILE_NAME");
                dtReturn.Columns.Add("RESULT");
                dtReturn.Columns.Add("MESSAGE");

                //Check Case Unzip (.zip, .rar)
                for (int k = 0; k < bankStatementImportEnt.Count; k++)
                {
                    string FilePath = bankStatementImportEnt[k].FilePath;
                    string FileName = Path.GetFileName(FilePath);
                    string FileExt = Path.GetExtension(FilePath);

                    if (string.Equals(FileExt.ToLower(), ".zip"))
                    {
                        LogHelper.WriteLog("CFRs", "INF", $"Unzip : {FilePath}");

                        using (ZipFile zipFile = ZipFile.Read(FilePath))
                        {
                            string PathZip = FilePath.Replace(".zip", "") + DateTime.Now.ToString("_yyyyMMdd_HHmmsss");

                            zipFile.ExtractAll(PathZip,
                            Ionic.Zip.ExtractExistingFileAction.DoNotOverwrite);

                            DirectoryInfo directoryInfo = new DirectoryInfo(PathZip);
                            FileInfo[] Files = directoryInfo.GetFiles("*.txt");

                            foreach (FileInfo file in Files)
                            {
                                DataRow dr = dtReturn.NewRow();
                                dr["ZIP_FILE_NAME"] = FileName;
                                dr["FILE_PATH"] = file.FullName;
                                dr["FILE_NAME"] = Path.GetFileName(file.FullName);

                                dtReturn.Rows.Add(dr);

                                LogHelper.WriteLog("CFRs", "INF", $"Unzip files : {file.FullName}");
                            }
                        }
                    }
                    else if (string.Equals(FileExt.ToLower(), ".rar"))
                    {
                        LogHelper.WriteLog("CFRs", "INF", $"Unrar : {FilePath}");

                        string PathRar = FilePath.Replace(".rar", "");
                        if (!System.IO.Directory.Exists(PathRar))
                        {
                            System.IO.Directory.CreateDirectory(PathRar);
                        }

                        var archive = RarArchive.Open(FilePath);

                        foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                            entry.WriteToDirectory(PathRar);

                        DirectoryInfo directoryInfo = new DirectoryInfo(PathRar);
                        FileInfo[] Files = directoryInfo.GetFiles("*.txt");

                        foreach (FileInfo file in Files)
                        {
                            DataRow dr = dtReturn.NewRow();
                            dr["ZIP_FILE_NAME"] = FileName;
                            dr["FILE_PATH"] = file.FullName;
                            dr["FILE_NAME"] = Path.GetFileName(file.FullName);

                            dtReturn.Rows.Add(dr);

                            LogHelper.WriteLog("CFRs", "INF", $"Unrar files : {file.FullName}");
                        }
                        archive.Dispose();
                    }
                    else if (string.Equals(FileExt.ToLower(), ".txt") || FileExt.ToLower().Contains(".xls"))
                    {
                        DataRow dr = dtReturn.NewRow();
                        dr["FILE_PATH"] = FilePath;
                        dr["FILE_NAME"] = FileName;

                        dtReturn.Rows.Add(dr);

                        LogHelper.WriteLog("CFRs", "INF", $"files : {FilePath}");
                    }
                }

                for (int k = 0; k < dtReturn.Rows.Count; k++)
                {
                    try
                    {
                        string FilePath = dtReturn.Rows[k]["FILE_PATH"] + string.Empty;
                        string FileName = dtReturn.Rows[k]["FILE_NAME"] + string.Empty;

                        DataTable dtImport = new DataTable();

                        if (!System.IO.File.Exists(FilePath))
                        {
                            dtReturn.Rows[k]["RESULT"] = "E";
                            dtReturn.Rows[k]["MESSAGE"] = "File not found.";

                            LogHelper.WriteLog("CFRs", "ERR", $"File not found. : {FilePath}");

                            continue;
                        }

                        if (string.Equals(BankShortName.ToUpper(), "BAAC"))
                        {
                            string[] lines = System.IO.File.ReadAllLines(FilePath, System.Text.Encoding.GetEncoding(874));

                            if (lines.Length > 0)
                            {
                                for (int i = 0; i < lines.Length; i++)
                                {
                                    string[] cols = lines[i].Split('|');

                                    if (i == 0)
                                    {// Create DataTable Column
                                        for (int j = 0; j < cols.Length; j++)
                                        {
                                            dtImport.Columns.Add($"COL_{j + 1}");
                                        }
                                    }

                                    dtImport.Rows.Add(
                                              cols[0].Trim()
                                            , cols[1].Trim()
                                            , cols[2].Trim()
                                            , cols[3].Trim()
                                            , cols[4].Trim()
                                            , cols[5].Trim()
                                            , cols[6].Trim()
                                            , cols[7].Trim()
                                            , cols[8].Trim()
                                            , cols[9].Trim()
                                            , cols[10].Trim()
                                            , cols[11].Trim()
                                            , cols[12].Trim()
                                            , cols[13].Trim()
                                            , cols[14].Trim()
                                            , cols[15].Trim()
                                            , cols[16].Trim()
                                            , cols[17].Trim()
                                            , cols[18].Trim()
                                            , cols[19].Trim()
                                            );
                                }

                                //Insert to DB
                                DataTable dtImportResult = StatementImport_BLL.Instance.ImportBLL(MonthID, Year, BankId
                                                                , FilePath
                                                                , string.Empty
                                                                , UserID
                                                                , dtImport
                                                                , string.Empty
                                                                , string.Empty);

                                dtReturn.Rows[k]["RESULT"] = dtImportResult.Rows[0]["TYPE"].ToString();
                                dtReturn.Rows[k]["MESSAGE"] = dtImportResult.Rows[0]["MESSAGE"].ToString();
                            }
                        }
                        else if (BankShortName.ToUpper().Contains("KBANK"))
                        {
                            string Header = "-";
                            string Footer = "-";
                            if (FilePath.Contains(".xls"))
                            {
                                ExcelFile workbookSummary = ExcelFile.Load(FilePath);
                                ExcelWorksheet worksheetSummary = workbookSummary.Worksheets[0];
                                for (int i = 1; i <= 10; i++)
                                {
                                    dtImport.Columns.Add($"COL_{i}");

                                }
                                for (var i = 1; i < worksheetSummary.Rows.Count; i++)
                                {
                                    DataRow dr = dtImport.NewRow();
                                    dr[$"COL_1"] = FileName.Substring(0, 10);
                                    string value = worksheetSummary.Rows[i].Cells[0].Value + string.Empty;
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        value = DateTime.ParseExact(value, "dd-MMM-yyyy", new CultureInfo("th-TH")).ToString("dd/MM/yyyy", new CultureInfo("en-US"));
                                    }
                                    dr[$"COL_2"] = value;
                                    for (int j = 1; j < 9; j++)
                                    {
                                        value = worksheetSummary.Rows[i].Cells[j].Value + string.Empty;
                                        dr[$"COL_{j + 2}"] = value;

                                    }
                                    dtImport.Rows.Add(dr);
                                }

                            }
                            else
                            {
                                string[] lines = System.IO.File.ReadAllLines(FilePath, System.Text.Encoding.GetEncoding(874));

                                if (lines.Length > 1)
                                {//Have Heaader & Footer
                                    Header = lines[0];
                                    Footer = lines[lines.Length - 1];

                                    dtImport.Columns.Add("COL_1");
                                    dtImport.Columns.Add("COL_2");
                                    dtImport.Columns.Add("COL_3");
                                    dtImport.Columns.Add("COL_4");
                                    dtImport.Columns.Add("COL_5");

                                    for (int i = 1; i < lines.Length - 1; i++)
                                    {
                                        dtImport.Rows.Add(
                                              lines[i].Substring(0, 65).Trim()
                                            , lines[i].Substring(65, 4).Trim()
                                            , lines[i].Substring(69, 36).Trim()
                                            , lines[i].Substring(105, 12).Trim()
                                            , lines[i].Substring(118, 10).Trim()
                                            );
                                    }


                                }
                            }
                            //Insert to DB
                            DataTable dtImportResult = StatementImport_BLL.Instance.ImportBLL(MonthID, Year, BankId
                                                            , FilePath
                                                            , string.Empty
                                                            , UserID
                                                            , dtImport
                                                            , Header
                                                            , Footer);

                            dtReturn.Rows[k]["RESULT"] = dtImportResult.Rows[0]["TYPE"].ToString();
                            dtReturn.Rows[k]["MESSAGE"] = dtImportResult.Rows[0]["MESSAGE"].ToString();


                        }
                        else if (string.Equals(BankShortName.ToUpper(), "KTB"))
                        {
                            string[] lines = System.IO.File.ReadAllLines(FilePath, System.Text.Encoding.GetEncoding(874));

                            if (lines.Length > 1)
                            {
                                string Header = lines[0];
                                string Footer = string.Empty;

                                dtImport.Columns.Add("COL_1");
                                dtImport.Columns.Add("COL_2");
                                dtImport.Columns.Add("COL_3");
                                dtImport.Columns.Add("COL_4");

                                for (int i = 1; i < lines.Length; i++)
                                {
                                    dtImport.Rows.Add(
                                          lines[i].Substring(0, 26).Trim()
                                        , lines[i].Substring(26, 58).Trim()
                                        , lines[i].Substring(84, 26).Trim()
                                        , lines[i].Substring(110, 220).Trim()
                                        );
                                }

                                //Insert to DB
                                DataTable dtImportResult = StatementImport_BLL.Instance.ImportBLL(MonthID, Year, BankId
                                                                , FilePath
                                                                , string.Empty
                                                                , UserID
                                                                , dtImport
                                                                , Header
                                                                , Footer);

                                dtReturn.Rows[k]["RESULT"] = dtImportResult.Rows[0]["TYPE"].ToString();
                                dtReturn.Rows[k]["MESSAGE"] = dtImportResult.Rows[0]["MESSAGE"].ToString();
                            }
                        }
                        else if (string.Equals(BankShortName.ToUpper(), "UOB"))
                        {
                            string[] lines = System.IO.File.ReadAllLines(FilePath, System.Text.Encoding.GetEncoding(874));

                            if (lines.Length > 1)
                            {//Have Heaader & Footer
                                string Header = lines[0];
                                string Footer = lines[lines.Length - 1];

                                dtImport.Columns.Add("COL_1");
                                dtImport.Columns.Add("COL_2");
                                dtImport.Columns.Add("COL_3");
                                dtImport.Columns.Add("COL_4");
                                dtImport.Columns.Add("COL_5");

                                for (int i = 1; i < lines.Length - 1; i++)
                                {
                                    dtImport.Rows.Add(
                                          lines[i].Substring(0, 66).Trim()
                                        , lines[i].Substring(66, 55).Trim()
                                        , lines[i].Substring(121, 60).Trim()
                                        , lines[i].Substring(181, 159).Trim()
                                        , lines[i].Substring(340, 1009).Trim()
                                        );
                                }

                                //Insert to DB
                                DataTable dtImportResult = StatementImport_BLL.Instance.ImportBLL(MonthID, Year, BankId
                                                                , FilePath
                                                                , string.Empty
                                                                , UserID
                                                                , dtImport
                                                                , Header
                                                                , Footer);

                                dtReturn.Rows[k]["RESULT"] = dtImportResult.Rows[0]["TYPE"].ToString();
                                dtReturn.Rows[k]["MESSAGE"] = dtImportResult.Rows[0]["MESSAGE"].ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        dtReturn.Rows[k]["RESULT"] = "E";
                        dtReturn.Rows[k]["MESSAGE"] = "Exception : " + ex.Message;

                        LogHelper.WriteLog("CFRs", "ERR", $"Exception : {ex.Message}");
                        return BadRequest(BaseHelper.ReturnError(ex));
                    }



                    //Upload File to S3
                    //for (int i = 0; i < dtReturn.Rows.Count; i++)
                    //{
                    //    Stream stream = System.IO.File.OpenRead(dtReturn.Rows[i]["FILE_PATH"].ToString());

                    //    AmazonHelper.UploadFileToS3(
                    //          stream
                    //        , dtReturn.Rows[i]["FILE_NAME"].ToString()
                    //        , Path.GetDirectoryName(dtReturn.Rows[i]["FILE_NAME"].ToString())
                    //        );
                    //}
                }

                json = JsonConvert.SerializeObject(dtReturn, Formatting.None);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");

                return Ok(json);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("CFRs", "ERR", "Call.. api/BankStatement/ImportFile/Import exception : " + ex.Message);

                return BadRequest(BaseHelper.ReturnError(ex));
            }
            finally
            {
                LogHelper.WriteLog("CFRs", "INF", "Call Ended.. api/BankStatement/ImportFile/Import");
            }
        }

        [HttpGet]
        [Route("get")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult GetData(TStatementImport filter)
        {
            try
            {
                IEnumerable<TStatementImport> data = StatementImport_BLL.Instance.GetData(filter);

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
        public IActionResult Delete([FromBody] TStatementImport filter)
        {
            try
            {
                TStatementImport data = StatementImport_BLL.Instance.Delete(filter);

                return Ok(data);
            }
            catch (Exception ex)
            {

                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }
        private void UploadFileToS3()
        {
            try
            {

            }
            catch (Exception ex)
            {
            }
        }
    }
}