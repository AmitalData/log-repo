using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.Repositories;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.Helpers.BIReport
{
    public class ExportBIReportExcelService
    {
        List<string> ChargesFactMeasurementFields = new List<string>()
       {"Gross Weight Per Ton", "Order Gross Weight", "Order Gross Weight in Ton",
        "Order Volume", "Total Volume (CBM)", "Volumetric Weight", "Number of Packages", 
        "Order Number of Packages", "Gross Weight (KG)","Open Payables ( Foreign )","Open Receivable ( Foreign )","Invoice Line Amount (Foreign)","Invoice Exchange Rate","Expected Payable Amount"
        };
        List<string> ARInvoicesFactMeasurementFields = new List<string>()
        {
            "Line Amount (Foreign)","Line VAT Percentage","Invoice Currency Exchange Rate","Regional Tax Percentage","Foreign Exchange Rate"
        };

        string FactTable; 
        public  byte[] Run(BIReportXMLData bIReportXMLData, DataTable dataTable, int tenant)
        {
            byte[] reportData;

            BIReportQueryService query = new BIReportQueryService(tenant);
            BIReportPM biReportEntityPM = query.GetSingle(bIReportXMLData.BIReportId, false, false);
            var factTable = biReportEntityPM.FactTableName;
            var bITabularViewSettings = LogitudeXmlSerializer.DeserializeObject<BITabularViewSettings>(biReportEntityPM.AGGridOptionsXML);
            List<string> MeasurmentColumns = null;
            List<ExcelTotals> excelTotals = new List<ExcelTotals>();
            this.FactTable = biReportEntityPM.FactTableName;
            DWObjectFieldQuery dWObjectFieldQuery = new DWObjectFieldQuery(tenant);
            List<DWObjectFieldPM> dWObjectMaxMeasurementFieldPMs =  dWObjectFieldQuery.GetDWObjectFieldByDWObjectTableCode(0, this.FactTable).Where(dwField => dwField.IsMeasurement && dwField.AggregationTypeCode == "MAX").ToList();
            if (bIReportXMLData.IncludeTotals)
            {
                MeasurmentColumns = bITabularViewSettings.Columns.Where(c => (c.DataTypeCode == "Double" || c.DataTypeCode == "Decimal" || c.DataTypeCode == "Integer") && IncludeColumnInTotal(c, dWObjectMaxMeasurementFieldPMs)).Select(c => c.Code).ToList();
               
            }

            System.IO.MemoryStream memory = new System.IO.MemoryStream();
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            workbook.Version = ExcelVersion.Excel2007;
            IWorksheet sheet = workbook.Worksheets[0];

            int count = dataTable.Columns.Count;
            List<DataColumn> deletedColumns = new List<DataColumn>();

            var columnNames = bITabularViewSettings.Columns.OrderBy(a => a.Index).Select(d => d.Code).ToList();
            int columnIndex = 0;
            foreach (var columnName in columnNames)
            {
                dataTable.Columns[columnName].SetOrdinal(columnIndex);

                if (bIReportXMLData.IncludeTotals)
                {
                    var measurmentColumn = MeasurmentColumns.Where(c => c == columnName).FirstOrDefault();
                    if (measurmentColumn != null) excelTotals.Add(new ExcelTotals(measurmentColumn, 0, columnIndex));
                }
                columnIndex++;
            }

            if (deletedColumns.Count > 0)
            {
                foreach (var item in deletedColumns)
                {
                    dataTable.Columns.Remove(item);
                }
            }

            dataTable = RemoveTenantColumnFromDataTableColumns(dataTable);

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

            TenantRepository TenantRepoitory = new TenantRepository(tenant);
            var CurTenant = TenantRepoitory.GetSingleByTenant(tenant);
            var rows = dataTable.Rows.Count;
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
                        case "Time":
                            writeRange.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                            writeRange.NumberFormat = "h:mm";
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


            int cellRow = 2;
            foreach (DataRow row in dataTable.Rows)
            {
                int cellCol = 1;
                int excelTotalCount = 0;
                for (int j = 1; j <= dataTable.Columns.Count; j++)
                {
                    var agColumn = bITabularViewSettings.Columns.Where(a => a.Name == dataTable.Columns[j - 1].ColumnName).FirstOrDefault();
                    if (agColumn != null)
                    {
                        if (agColumn.DataTypeCode == "Text")
                        {
                            string value = Convert.ToString(row[agColumn.Name]);
                            sheet.Range[cellRow, cellCol].Text = value;
                        }
                        else if (bIReportXMLData.IncludeTotals && ((agColumn.DataTypeCode == "Double" || agColumn.DataTypeCode == "Decimal" || agColumn.DataTypeCode == "Integer") && IncludeColumnInTotal(agColumn, dWObjectMaxMeasurementFieldPMs)))
                        {
                            string value = row[agColumn.Name].ToString();
                            if (!string.IsNullOrEmpty(value))
                            {
                                excelTotals[excelTotalCount].Total += Double.Parse(value);
                            }
                            excelTotalCount += 1;
                        }
                    }
                    cellCol++;

                }
                cellRow++;
            }
            if (bIReportXMLData.IncludeTotals)
            {
                foreach (var ex in excelTotals)
                {
                    sheet.Range[rows + 2, ex.IndexOrder + 1].Cells[0].CellStyle.Color = Color.Orange;
                    sheet.Range[rows + 2, ex.IndexOrder + 1].Value = ex.Total.ToString();
                }
            }

            workbook.SaveAs(memory);
            workbook.Close();
            excelEngine.Dispose();
            reportData = memory.ToArray();
            return reportData;
        }

        private bool IncludeColumnInTotal(Column selectedColumn, List<DWObjectFieldPM> dWObjectMaxMeasurementFieldPMs)
        {
            if(this.FactTable == "Fact_Charges" || this.FactTable == "Fact_MasterCharges")
            {
                return !ChargesFactMeasurementFields.Any(f => f == selectedColumn.Name);
            }
            else if (this.FactTable == "Fact_ARInvoices")
            {
                return !ARInvoicesFactMeasurementFields.Any(f => f == selectedColumn.Name) && !dWObjectMaxMeasurementFieldPMs.Any(f => f.Code == selectedColumn.FieldCode);
            }

            return !dWObjectMaxMeasurementFieldPMs.Any(f => f.Code == selectedColumn.FieldCode);
        }
          
        private DataTable RemoveTenantColumnFromDataTableColumns(DataTable dataTable)
        {
            DataTable dataTableWithoutTenantColumn = dataTable;
            if (dataTable.Columns.Contains("Tenant"))
            {
                dataTableWithoutTenantColumn.Columns.Remove("Tenant");
            }

            return dataTableWithoutTenantColumn;
        }
    }
}