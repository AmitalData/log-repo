using Simplog.Data.CommonDataModel.Repositories;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.Helpers
{
    public partial class ExportToExcelHelper
    {
        public byte[] ExportDataTableToExcel(DataTable dataTable, int tenant, BITabularViewSettings bITabularViewSettings)
        {
            ///////////////////////////////////////////////////////////




            System.IO.MemoryStream memory = new System.IO.MemoryStream();
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet sheet = workbook.Worksheets[0];

            int count = dataTable.Columns.Count;
            List<DataColumn> deletedColumns = new List<DataColumn>();

            var columnNames = bITabularViewSettings.Columns.OrderBy(a => a.Index).Select(d => d.Code).ToList();
            int columnIndex = 0;
            foreach (var columnName in columnNames)
            {
                dataTable.Columns[columnName].SetOrdinal(columnIndex);
                columnIndex++;
            }

            if (deletedColumns.Count > 0)
            {
                foreach (var item in deletedColumns)
                {
                    dataTable.Columns.Remove(item);
                }
            }
            RemoveEqualFromAllColumns(dataTable);//avoid exception "David" is not valid named range due value:"= david 33"

            sheet.ImportDataTable(dataTable, true, 1, 1);

            // sheet Format - Width 
            for (var i = 0; i < dataTable.Columns.Count; i++)
            {
                var agColumn = bITabularViewSettings.Columns.Where(a => a.Name == dataTable.Columns[i].ColumnName).FirstOrDefault();
                if (agColumn != null)
                {
                    sheet.Columns[i].ColumnWidth = agColumn.Width / 7.5;
                }
            }

            TenantRepository tenantRepoitory = new TenantRepository(tenant);
            var CurTenant = tenantRepoitory.GetSingleByTenant(tenant);
            var rows = dataTable.Rows.Count;
            if (rows > 0)
            {
                for (int j = 1; j <= dataTable.Columns.Count; j++)
                {
                    var agColumn = bITabularViewSettings.Columns.Where(a => a.Name == dataTable.Columns[j - 1].ColumnName).FirstOrDefault();
                    if (agColumn != null)
                    {

                        var writeRange = sheet.Range[2, j, rows + 1, j];
                        switch (agColumn.DataTypeCode)
                        {
                            case "Constant":
                            case "Text":
                                writeRange.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                                break;
                            case "DateTime":
                                string datetimeformat = @"dd\/MM\/yyyy";
                                if (!string.IsNullOrEmpty(CurTenant.DateTimeFormat))
                                {
                                    datetimeformat = CurTenant.DateTimeFormat;
                                }
                                writeRange.NumberFormat = datetimeformat;
                                break;
                            case "DateTime?":
                                string longdatetimeformat = @"dd\/MM\/yyyy HH:mm";
                                if (!string.IsNullOrEmpty(CurTenant.DateTimeFormat))
                                {
                                    datetimeformat = CurTenant.DateTimeFormat;
                                }
                                writeRange.NumberFormat = longdatetimeformat;
                                break;
                            case "Decimal":
                            case "Double":
                                writeRange.HorizontalAlignment = ExcelHAlign.HAlignRight;
                                writeRange.NumberFormat = "###,##0.00";
                                break;
                            case "Integer":
                                writeRange.HorizontalAlignment = ExcelHAlign.HAlignRight;
                                writeRange.NumberFormat = "###,##";
                                break;

                            default:
                                writeRange.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                                break;
                        }
                    }
                }
            }
            sheet.Range[1, 1, 1, dataTable.Columns.Count + 1].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
            sheet.Range[1, 1, 1, dataTable.Columns.Count + 1].CellStyle.Font.Bold = true;
            ///workbook.Version = ExcelVersion.Excel2007;
            workbook.SaveAs(memory, ExcelSaveType.SaveAsXLS);
            return memory.ToArray();
        }

        private static void RemoveEqualFromAllColumns(DataTable dataTable)
        {
            bool exportIt = false;
            if (exportIt)
            {
                string fileName = Guid.NewGuid() + ".xml";
                string filePath = Path.Combine(Path.GetTempPath(), fileName);
                dataTable.WriteXml(filePath);
            }
            foreach (DataRow row in dataTable.Rows)
            {
                foreach (DataColumn column in dataTable.Columns)
                {
                    string columnName = column.ColumnName.ToString();
                    string value = (row[columnName]) != null ? (row[columnName]).ToString() : "";
                    if (value.StartsWith("="))
                    {
                        row[columnName] = value.Substring(1);
                    }
                }
            }
        }
    }
}