using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.Helpers.BIReport
{
    public class BIReportHtmlRenderingService
    {

        private DataTable bIReportdataTable = null;
        private StringBuilder stringBuilder = null;
        private BIReportXMLData bIReportXMLData = new BIReportXMLData();
        private BITabularViewSettings bITabularViewSettings = new BITabularViewSettings();
        private List<string> measurmentColumns = new List<string>();
        private List<ExcelTotals> totalMeasurementColumnList = new List<ExcelTotals>();
        public BIReportTotalsService bIReportTotalsService;
        int pdfTotalCount = 0;
        private bool isValidTotalColumn = false;

        public BIReportHtmlRenderingService(DataTable bIReportdataTable, BIReportXMLData bIReportXMLData, int tenant)
        {
            this.bIReportdataTable = bIReportdataTable;
            this.bIReportXMLData = bIReportXMLData;
            bITabularViewSettings = bIReportXMLData.BITabularViewSettings;
            bIReportTotalsService = new BIReportTotalsService(bIReportXMLData, tenant);
        }


        public string Render()
        {
            string result = RenderReportToHtml();
            return result;
        }

        private string RenderReportToHtml()
        {
            stringBuilder = new StringBuilder("<!DOCTYPE html> <html> <head>" + GetBIReportTableStyle() + "</head><body>");
            stringBuilder.Append("<table>");
            BuildReportHeader();
            BuildReportMeasurementColumns();
            BuildTotalsMeasurementColumnsList();
            BuildReportRows();
            stringBuilder.Append("</table>");
            stringBuilder.Append("</body>");
            stringBuilder.Append("</html>");
            return stringBuilder.ToString();
        }

        private void BuildReportHeader()
        {
            stringBuilder.Append("<thead>");
            stringBuilder.Append("<tr>");
            BuildHeaderColumns();
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</thead>");
        }

        private void BuildHeaderColumns()
        {
            foreach (DataColumn column in bIReportdataTable.Columns)
            {
                stringBuilder.Append(" <th>" + column.ColumnName + "</th> ");
            }
        }

        private void BuildReportRows()
        {
            foreach (DataRow row in bIReportdataTable.Rows)
            {
                stringBuilder.Append(" <tr>");
                BuildDataColumns(row);
                stringBuilder.Append(" </tr>");
            }
            BuildTotalRow();
        }

        private void BuildDataColumns( DataRow row)
        {
            pdfTotalCount = 0;
            foreach (DataColumn column in bIReportdataTable.Columns)
            {
                AddToTotalsMeasurementValues(row, column);
                stringBuilder.Append("<td style=\"text-align:"+(isValidTotalColumn ? "right":"left")+"\">" + row[column.ColumnName].ToString() + "</td>");
            }
        }

        private string GetBIReportTableStyle()
        {
            return " <style> table {font-family: arial, sans-serif;border-collapse: collapse;width: 100%;} td, th {border: 1px solid #dddddd;text-align: left;padding: 8px;} tr:nth-child(even) { background-color: #dddddd;}</style>";
        }

        private void BuildReportMeasurementColumns()
        {
            measurmentColumns = bIReportTotalsService.GetReportMeasurementColumns();
        }

        private void BuildTotalsMeasurementColumnsList()
        {
            if (measurmentColumns == null || measurmentColumns.Count == 0) return;
            totalMeasurementColumnList = bIReportTotalsService.BuildTotalMeasurementColumnList();
        }

        private void AddToTotalsMeasurementValues(DataRow row, DataColumn column)
        {
            var agColumn = bITabularViewSettings.Columns.Where(a => a.Name == column.ColumnName).FirstOrDefault();
            isValidTotalColumn = bIReportTotalsService.ValidTotalColumn(agColumn);

            if (isValidTotalColumn)
            {
                AddValueToTotalMeasurementColumn(row, agColumn);
                pdfTotalCount += 1;
            }
        }

        private void AddValueToTotalMeasurementColumn(DataRow row, Column agColumn)
        {
            string value = row[agColumn.Name].ToString();
            if (!string.IsNullOrEmpty(value))
            {
                totalMeasurementColumnList[pdfTotalCount].Total += Double.Parse(value);
            }
        }

        private void BuildTotalRow()
        {
            if (totalMeasurementColumnList == null || totalMeasurementColumnList.Count == 0) return;
            stringBuilder.Append(" <tr>");
            foreach (DataColumn column in bIReportdataTable.Columns)
            {
                BuildTotalCell(column);
            }
            stringBuilder.Append(" </tr>");
        }

        private void BuildTotalCell(DataColumn column)
        {
            var pdfTotal = totalMeasurementColumnList.Where(d => d.FieldCode == column.ColumnName).FirstOrDefault();
            if (pdfTotal == null)
            {
                AppendTotalCellValue("");
                return;
            }
            var total = Math.Round(pdfTotal.Total, 2);
            var formattedTotal = String.Format("{0:#,##0.##}", total);
            AppendTotalCellValue(formattedTotal);
        }

        private void AppendTotalCellValue(string total)
        {
            stringBuilder.Append("<td " + GetTotalStyle() + ">" + total + "</td>");
        }

        private string GetTotalStyle()
        {
            return "style=\"text-align:right;font-weight:bold;color:#1a83b9;background-color:#f7f7f7\"";
        }
    }
}