using Fanex.Bitcoin.Business.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fanex.Bitcoin.Business.Services
{
    public class ExcelServiceMonthlyImp : ExcelService
    {
        private readonly DateServiceProvider dateServiceProvider;

        public ExcelServiceMonthlyImp(DateServiceProvider dateServiceProvider)
        {
            this.dateServiceProvider = dateServiceProvider;
        }

        public string Export(Tuple<IEnumerable<BitcoinModel>, IEnumerable<BitcoinModel>> data)
        {
            var data1 = data.Item1.ToList();
            var data2 = data.Item2.ToList();
            var date = dateServiceProvider.Date;
            using var excelPackage = new ExcelPackage(new FileInfo($"Templates/Template.xlsx"));
            using var template = excelPackage.Workbook.Worksheets["Template"];
            using var sheet = excelPackage.Workbook.Worksheets.Add("Monthly");
            sheet.View.ShowGridLines = false;
            data1 = data1.OrderBy(o => o.Date).ToList();

            RenderTitle(sheet, template, date);

            if (data1.Count == 0)
            {
                template.Cells["B6:K6"].Copy(sheet.Cells["B6"]);
                sheet.Cells["B6:K6"].Value = "There is no data";
                sheet.Cells["B6:K6"].Merge = true;
            }
            else
            {
                RenderData1(sheet, template, data1);
                RenderData2(sheet, template, data1, data2);
            }

            var excelPath = SaveExceFile(excelPackage);

            return excelPath;
        }

        private static void RenderData2(ExcelWorksheet sheet, ExcelWorksheet template, List<BitcoinModel> data1, List<BitcoinModel> data2)
        {
            var startRow = 6 + data1.ToList().Count;
            var endRow = startRow + data2.ToList().Count - 1;
            sheet.Cells[startRow, 2].LoadFromCollection(data2, false);
            for (var rowIndex = startRow; rowIndex <= endRow; rowIndex++)
            {
                sheet.Row(rowIndex).Height = 19.5;
                for (int columnIndex = 2; columnIndex <= 11; columnIndex++)
                {
                    sheet.Cells[rowIndex, columnIndex].StyleID = template.Cells[8, columnIndex].StyleID;
                }
            }

            sheet.Cells[startRow, 2, endRow, 3].Merge = true;
            sheet.Cells[startRow, 2, endRow, 3].Value = "Total in";
        }

        private static void RenderData1(ExcelWorksheet sheet, ExcelWorksheet template, List<BitcoinModel> data1)
        {
            sheet.Cells["B6"].LoadFromCollection(data1, false);
            var startRow = 6;

            var startRowDate = startRow;
            var endRowDate = 0;
            var startRowCur = startRowDate;
            var endRowCur = 0;
            var dateCount = 0;

            data1.GroupBy(g => new { Date = g.Date, GameType = g.GameType })
                .ToList()
                .ForEach(date =>
                {
                    endRowDate = startRowDate + date.Count() - 1;
                    FormatDateContent(sheet, template, startRowDate, endRowDate, dateCount);

                    startRowCur = startRowDate;
                    date.GroupBy(d => new { Currency = d.Currency })
                    .ToList()
                    .ForEach(currency =>
                    {
                        endRowCur = startRowCur + currency.Count() - 1;
                        sheet.Cells[startRowCur, 4, endRowCur, 4].Merge = true;
                        startRowCur = endRowCur + 1;
                    });

                    sheet.Cells[startRowDate, 2, endRowDate, 2].Merge = true;
                    sheet.Cells[startRowDate, 3, endRowDate, 3].Merge = true;
                    startRowDate = endRowDate + 1;
                    dateCount++;
                });
        }

        private static void FormatDateContent(ExcelWorksheet sheet, ExcelWorksheet template, int startRowDate, int endRowDate, int dateCount)
        {
            for (var rowIndex = startRowDate; rowIndex <= endRowDate; rowIndex++)
            {
                sheet.Row(rowIndex).Height = 19.5;
                for (int columnIndex = 2; columnIndex <= 11; columnIndex++)
                {
                    if (dateCount % 2 == 0)
                    {
                        sheet.Cells[rowIndex, columnIndex].StyleID = template.Cells[6, columnIndex].StyleID;
                    }
                    else
                    {
                        sheet.Cells[rowIndex, columnIndex].StyleID = template.Cells[7, columnIndex].StyleID;
                    }
                }
            }
        }

        private static void RenderTitle(ExcelWorksheet sheet, ExcelWorksheet template, DateService date)
        {
            template.Cells["B2:K5"].Copy(sheet.Cells["B2"]);
            sheet.Cells["B2"].Value = $"Bitcoin Monthly Report - From {date.FromDate:MMM dd, yyyy} To {date.ToDate:MMM dd, yyyy} (GMT - 4)";
            sheet.View.FreezePanes(6, 5);
            for (int columnIndex = 1; columnIndex <= 11; columnIndex++)
            {
                sheet.Column(columnIndex).Width = template.Column(columnIndex).Width;
            }

            for (int rowIndex = 1; rowIndex <= 5; rowIndex++)
            {
                sheet.Row(rowIndex).Height = template.Row(rowIndex).Height;
            }
        }

        private string SaveExceFile(ExcelPackage excelPackage)
        {
            excelPackage.Workbook.Worksheets.Delete("Template");
            var date = dateServiceProvider.Date;
            var fileName = $"Bitcoin_Monthly_Report_{date.FromDate:MMddyyyy}_{date.ToDate:MMddyyyy}.xlsx";
            var path = $@"{AppConfigService.AppConfig.Path}/{date.ToDate:yyyy}/{date.ToDate:MM}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = $@"{path}/{fileName}";
            excelPackage.SaveAs(new FileInfo(path), AppConfigService.AppConfig.Password);

            return path;
        }
    }
}