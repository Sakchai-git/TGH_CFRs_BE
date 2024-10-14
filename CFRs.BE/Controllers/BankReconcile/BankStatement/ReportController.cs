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
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.StaticFiles;
using System.Globalization;
using GemBox.Pdf.Content;
using GemBox.Pdf;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.VisualBasic.FileIO;
using System.Reflection;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace CFRs.BE.Controllers.BankReconcile.BankStatement
{
    [Route("api/report")]//, Authorize
    [ApiController]
    public class ReportController : ControllerBase
    {

        [HttpGet]
        [Route("balance")]
        public IActionResult Balance(RFilter filter)
        {//AutoReconcile
            try
            {

                //Get Id ใน Header
                IEnumerable<RBalance> data = Report_BLL.Instance.Balance(filter);
                string FilePathSummary = $"{Directory.GetCurrentDirectory()}//FileFormat//Report//Balance.xls";
                ExcelFile workbookSummary = ExcelFile.Load(FilePathSummary);
                ExcelWorksheet worksheetSummary = workbookSummary.Worksheets[0];

                DateTime date = new DateTime(filter.year, filter.monthId, 1);
                string filename = "รายงานงบกระทบยอด " + date.ToString("MMM", new CultureInfo("en-Us")) + " " + filter.year;
                worksheetSummary.Rows[0].Cells[0].Value = filename;
                if (data.Count() > 2)
                {
                    worksheetSummary.Rows.InsertCopy(4, data.Count() - 2, worksheetSummary.Rows[4]);
                }
                int i = 3;
                foreach (var item in data)
                {
                    worksheetSummary.Rows[i].Cells["A"].Value = item.series;
                    worksheetSummary.Rows[i].Cells["B"].Value = item.systemCode;
                    worksheetSummary.Rows[i].Cells["C"].Value = item.accountSap;
                    worksheetSummary.Rows[i].Cells["D"].Value = item.bankCode;
                    worksheetSummary.Rows[i].Cells["E"].Value = item.bankName;
                    worksheetSummary.Rows[i].Cells["F"].Value = item.branchName;
                    worksheetSummary.Rows[i].Cells["G"].Value = item.accountNo;
                    worksheetSummary.Rows[i].Cells["H"].Value = item.sapDr;
                    worksheetSummary.Rows[i].Cells["I"].Value = item.sapCr;
                    worksheetSummary.Rows[i].Cells["J"].Value = item.glDr;
                    worksheetSummary.Rows[i].Cells["K"].Value = item.glCr;
                    worksheetSummary.Rows[i].Cells["L"].Value = item.diffDr;
                    worksheetSummary.Rows[i].Cells["M"].Value = item.diffCr;
                    i++;
                }

                string fileType = "xlsx";

                if (filter.reportType.ToLower() == "pdf")
                {
                    fileType = filter.reportType.ToLower();
                }
                string filereport = $"{Directory.GetCurrentDirectory()}//FileUpload//1Report//Balance{DateTime.Now.ToString("yyyyMMdd-HHmmsss")}.{fileType}";

                worksheetSummary.Rows.Remove(1);
                workbookSummary.Save(filereport);

                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(filereport, out var contenttype))
                {
                    contenttype = "application/octet-stream";
                }

                var bytes = System.IO.File.ReadAllBytes(filereport);

                return File(bytes, contenttype, filename + "." + fileType);

            }
            catch (Exception ex)
            {

                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }

        [HttpGet]
        [Route("ktb-transfer")]
        public IActionResult KTBTransfer(RFilter filter)
        {//AutoReconcile
            try
            {

                //Get Id ใน Header

                string bank = "KTBTransfer";
                if (filter.bankShortName == "UOB" || filter.bankShortName == "BAAC")
                {
                    bank = $"{filter.bankShortName}Transfer";
                }
                string FilePathSummary = $"{Directory.GetCurrentDirectory()}//FileFormat//Report//{bank}.xlsx";
                ExcelFile workbookSummary = ExcelFile.Load(FilePathSummary);
                ExcelWorksheet worksheetSummary = workbookSummary.Worksheets[0];

                DateTime date = new DateTime(filter.year, filter.monthId, 1);
                string filename = worksheetSummary.Rows[0].Cells[0].Value + string.Empty;
                filename = filename.Replace("{Bank}", filter.bankShortName);
                worksheetSummary.Rows[0].Cells[0].Value = filename.Replace("{Month}", date.ToString("MMM yyyy", new CultureInfo("en-Us")));

                string fileType = "xlsx";
                if (filter.reportType.ToLower() == "pdf")
                {
                    fileType = filter.reportType.ToLower();
                }

                string filereport = $"{Directory.GetCurrentDirectory()}//FileUpload//1Report//{bank}{DateTime.Now.ToString("yyyyMMdd-HHmmsss", new CultureInfo("en-US"))}.{fileType}";
                if (filter.bankShortName == "UOB")
                {
                    IEnumerable<RUOBTransfer> data = Report_BLL.Instance.UOBTransfer(filter);
                    if (data.Count() > 2)
                    {
                        worksheetSummary.Rows.InsertCopy(4, data.Count() - 2, worksheetSummary.Rows[4]);
                    }
                    int row = 3, col = 0;
                    foreach (var item in data)
                    {
                        Type myType = item.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        col = 0;
                        foreach (PropertyInfo prop in props)
                        {
                            object? propValue = prop.GetValue(item, null);
                            worksheetSummary.Rows[row].Cells[col].Value = propValue;
                            col++;
                            // Do something with propValue
                        }
                        row++;
                    }
                    worksheetSummary.Rows.Remove(1);
                    worksheetSummary.Calculate();
                    workbookSummary.Save(filereport);
                }
                else if (filter.bankShortName == "BAAC")
                {
                    IEnumerable<RBAACTransfer> data = Report_BLL.Instance.BAACTransfer(filter);
                    if (data.Count() > 2)
                    {
                        worksheetSummary.Rows.InsertCopy(4, data.Count() - 2, worksheetSummary.Rows[4]);
                    }
                    int row = 3, col = 0;
                    foreach (var item in data)
                    {
                        Type myType = item.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                        col = 0;
                        foreach (PropertyInfo prop in props)
                        {
                            object? propValue = prop.GetValue(item, null);
                            worksheetSummary.Rows[row].Cells[col].Value = propValue;
                            col++;
                            // Do something with propValue
                        }
                        row++;
                    }
                    worksheetSummary.Rows.Remove(1);
                    worksheetSummary.Calculate();
                    workbookSummary.Save(filereport);
                }
                else
                {
                    RKTBTransfer data = Report_BLL.Instance.KTBTransfer(filter);
                    if (data.KTBTransferIn.Count() > 2)
                    {
                        worksheetSummary.Rows.InsertCopy(4, data.KTBTransferIn.Count() - 2, worksheetSummary.Rows[4]);
                    }
                    int i = 3;

                    foreach (var item in data.KTBTransferIn)
                    {
                        worksheetSummary.Rows[i].Cells["A"].Value = item.bankCode;
                        worksheetSummary.Rows[i].Cells["B"].Value = item.accountSap;
                        worksheetSummary.Rows[i].Cells["C"].Value = item.branchName;
                        worksheetSummary.Rows[i].Cells["D"].Value = item.debit;
                        worksheetSummary.Rows[i].Cells["E"].Value = item.credit;

                        //worksheetSummary.Rows[i].Cells["A"].Style.Borders.SetBorders(MultipleBorders.Outside, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);
                        //worksheetSummary.Rows[i].Cells["B"].Style.Borders.SetBorders(MultipleBorders.Outside, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);
                        //worksheetSummary.Rows[i].Cells["C"].Style.Borders.SetBorders(MultipleBorders.Outside, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);
                        //worksheetSummary.Rows[i].Cells["D"].Style.Borders.SetBorders(MultipleBorders.Outside, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);
                        //worksheetSummary.Rows[i].Cells["E"].Style.Borders.SetBorders(MultipleBorders.Outside, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);

                        //SumDebit += item.debit;
                        //SumCredit += item.credit;

                        i++;
                    }

                    //i++;

                    //worksheetSummary.Cells[$"D{i}"].Value = SumDebit;
                    //worksheetSummary.Cells[$"E{i}"].Value = SumCredit;

                    //worksheetSummary.Cells[$"D{i}"].Style.Font.Weight = 800;
                    //worksheetSummary.Cells[$"E{i}"].Style.Font.Weight = 800;

                    //worksheetSummary.Cells[$"D{i}"].Style.Borders.SetBorders(MultipleBorders.Outside, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);
                    //worksheetSummary.Cells[$"E{i}"].Style.Borders.SetBorders(MultipleBorders.Outside, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);

                    ExcelWorksheet worksheetSummary2 = workbookSummary.Worksheets[1];
                    string filename2 = worksheetSummary2.Rows[0].Cells[0].Value + string.Empty;
                    filename2 = filename2.Replace("{Bank}", filter.bankShortName);
                    worksheetSummary2.Rows[0].Cells[0].Value = filename2.Replace("{Month}", date.ToString("MMM yyyy", new CultureInfo("en-Us")));
                    if (data.KTBTransferOut.Count() > 2)
                    {
                        worksheetSummary2.Rows.InsertCopy(4, data.KTBTransferOut.Count() - 2, worksheetSummary2.Rows[4]);
                    }
                    i = 3;

                    foreach (var item in data.KTBTransferOut)
                    {
                        worksheetSummary2.Rows[i].Cells["A"].Value = item.bankCode;
                        worksheetSummary2.Rows[i].Cells["B"].Value = item.accountSap;
                        worksheetSummary2.Rows[i].Cells["C"].Value = item.branchName;
                        worksheetSummary2.Rows[i].Cells["D"].Value = item.debit;
                        worksheetSummary2.Rows[i].Cells["E"].Value = item.credit;
                        i++;
                    }




                    worksheetSummary.Rows.Remove(1);
                    worksheetSummary2.Rows.Remove(1);
                    worksheetSummary.Calculate();
                    worksheetSummary2.Calculate();
                    if (filter.bankShortName == "KBANK")
                    {
                        worksheetSummary.Name = worksheetSummary.Name.Replace(" A7", " A2");
                        worksheetSummary2.Name = worksheetSummary2.Name.Replace(" A7", " A2");
                    }
                    workbookSummary.Save(filereport);
                    if (filter.reportType.ToLower() == "pdf")
                    {
                        string filereport2 = $"{Directory.GetCurrentDirectory()}//FileUpload//1Report//KTBTransfer{DateTime.Now.ToString("yyyyMMdd-HHmmsss", new CultureInfo("en-US"))}_2.{fileType}";
                        string filereportSum = $"{Directory.GetCurrentDirectory()}//FileUpload//1Report//KTBTransfer{DateTime.Now.ToString("yyyyMMdd-HHmmsss", new CultureInfo("en-US"))}_sum.{fileType}";
                        workbookSummary.Worksheets.Remove(0);
                        workbookSummary.Save(filereport2);
                        using (var document = new PdfDocument())
                        {

                            using (var source = PdfDocument.Load(filereport))
                            {
                                document.Pages.Kids.AddClone(source.Pages);
                            }
                            using (var source = PdfDocument.Load(filereport2))
                            {
                                document.Pages.Kids.AddClone(source.Pages);
                            }

                            document.Save(filereportSum);
                        }
                        filereport = filereportSum;
                    }

                }


                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(filereport, out var contenttype))
                {
                    contenttype = "application/octet-stream";
                }

                var bytes = System.IO.File.ReadAllBytes(filereport);

                return File(bytes, contenttype, filename + "." + fileType);

            }
            catch (Exception ex)
            {

                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }


        [HttpGet]
        [Route("media-clearing")]
        public IActionResult MediaClearing(RFilter filter)
        {//AutoReconcile
            try
            {

                //Get Id ใน Header

                string bank = $"{filter.bankShortName}MediaClearing";
                //if (filter.bankShortName == "UOB" || filter.bankShortName == "BAAC")
                //{
                //    bank = $"{filter.bankShortName}Transfer";
                //}
                string FilePathSummary = $"{Directory.GetCurrentDirectory()}//FileFormat//Report//{bank}.xlsx";
                ExcelFile workbookSummary = ExcelFile.Load(FilePathSummary);
                ExcelWorksheet worksheetSummary = workbookSummary.Worksheets[0];
                ExcelWorksheet worksheetTemplate = workbookSummary.Worksheets[1];
                //ExcelRow row4 = worksheetTemplate.Rows[4];
                //ExcelRow rowSum = worksheetTemplate.Rows[6];
                DateTime date = new DateTime(filter.year, filter.monthId, 1);
                string filename = worksheetSummary.Rows[0].Cells[0].Value + string.Empty;
                filename = filename.Replace("{Bank}", filter.bankShortName);
                worksheetSummary.Rows[0].Cells[0].Value = filename.Replace("{Month}", date.ToString("MMM yyyy", new CultureInfo("en-Us")));

                string fileType = "xlsx";
                if (filter.reportType.ToLower() == "pdf")
                {
                    fileType = filter.reportType.ToLower();
                }

                string filereport = $"{Directory.GetCurrentDirectory()}//FileUpload//1Report//{bank}{DateTime.Now.ToString("yyyyMMdd-HHmmsss", new CultureInfo("en-US"))}.{fileType}";
                if (filter.bankShortName == "UOB")
                {
                    //IEnumerable<RUOBTransfer> data = Report_BLL.Instance.UOBTransfer(filter);
                    //if (data.Count() > 2)
                    //{
                    //    worksheetSummary.Rows.InsertCopy(4, data.Count() - 2, worksheetSummary.Rows[4]);
                    //}
                    //int row = 3, col = 0;
                    //foreach (var item in data)
                    //{
                    //    Type myType = item.GetType();
                    //    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                    //    col = 0;
                    //    foreach (PropertyInfo prop in props)
                    //    {
                    //        object? propValue = prop.GetValue(item, null);
                    //        worksheetSummary.Rows[row].Cells[col].Value = propValue;
                    //        col++;
                    //        // Do something with propValue
                    //    }
                    //    row++;
                    //}
                }
                else if (filter.bankShortName == "BAAC")
                {
                    //IEnumerable<RBAACTransfer> data = Report_BLL.Instance.BAACTransfer(filter);
                    //if (data.Count() > 2)
                    //{
                    //    worksheetSummary.Rows.InsertCopy(4, data.Count() - 2, worksheetSummary.Rows[4]);
                    //}
                    //int row = 3, col = 0;
                    //foreach (var item in data)
                    //{
                    //    Type myType = item.GetType();
                    //    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                    //    col = 0;
                    //    foreach (PropertyInfo prop in props)
                    //    {
                    //        object? propValue = prop.GetValue(item, null);
                    //        worksheetSummary.Rows[row].Cells[col].Value = propValue;
                    //        col++;
                    //        // Do something with propValue
                    //    }
                    //    row++;
                    //}
                    //worksheetSummary.Rows.Remove(1);
                    //worksheetSummary.Calculate();
                    //workbookSummary.Save(filereport);
                }
                else
                {
                    int startRow = 4;
                    IEnumerable<RKBANKMediaClearing> data = Report_BLL.Instance.KBANKMediaClearing(filter);
                    IEnumerable<RKBANKMediaClearing> dataGroup = data.GroupBy(it => it.bankCode).Select(it => it.First());
                    foreach (var itemGroup in dataGroup)
                    {
                        IEnumerable<RKBANKMediaClearing> listItem = data.Where(it => it.bankCode == itemGroup.bankCode);
                        
                        CellRange cellrange = worksheetTemplate.Cells.GetSubrangeAbsolute(3, 0, 5, 5);
                        cellrange.CopyTo(worksheetSummary, startRow - 1, 0);
                        worksheetSummary.Rows.InsertCopy(startRow, listItem.Count() - 2 , worksheetSummary.Rows[startRow]);
                        int i = 0;
                        foreach (var item in listItem)
                        {
                            if (i == 0)
                            {
                                worksheetSummary.Rows[i + startRow - 1].Cells["A"].Value = item.bankCode;
                                worksheetSummary.Rows[i + startRow - 1].Cells["B"].Value = item.accountSap;
                                worksheetSummary.Rows[i + startRow - 1].Cells["C"].Value = item.accountNo;
                                worksheetSummary.Rows[i + startRow - 1].Cells["D"].Value = item.branchName;
                            }

                            worksheetSummary.Rows[i + startRow - 1].Cells["E"].Value = item.effectiveDate;
                            worksheetSummary.Rows[i + startRow - 1].Cells["F"].Value = item.adjAmount;
                            i++;
                        }
                        startRow += listItem.Count() + 2;
                    }
                }

                worksheetSummary.Rows.Remove(1);
                worksheetSummary.Calculate();
                workbookSummary.Worksheets.Remove(1);
                workbookSummary.Save(filereport);

                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(filereport, out var contenttype))
                {
                    contenttype = "application/octet-stream";
                }

                var bytes = System.IO.File.ReadAllBytes(filereport);

                return File(bytes, contenttype, filename + "." + fileType);

            }
            catch (Exception ex)
            {

                return BadRequest(BaseHelper.ReturnError(ex));
            }
        }

    }
}