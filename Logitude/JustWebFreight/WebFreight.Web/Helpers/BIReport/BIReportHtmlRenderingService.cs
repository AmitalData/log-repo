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
                stringBuilder.Append("<td>" + row[column.ColumnName].ToString() + "</td>");
                AddToTotalsMeasurementValues(row, column);
            }
        }

        private string GetBIReportTableStyle()
        {
            return " <style> table {font-family: arial, sans-serif;border-collapse: collapse;width: 100%;} td, th {border: 1px solid #dddddd;text-align: left;padding: 8px;} tr:nth-child(even) { background-color: #dddddd;}</style>";
        }

        private void BuildReportMeasurementColumns()
        {
            if (!bIReportXMLData.IncludeTotals) return;
            measurmentColumns = bIReportTotalsService.GetReportMeasurementColumns();
        }

        private void BuildTotalsMeasurementColumnsList()
        {
            if (!bIReportXMLData.IncludeTotals || measurmentColumns == null || measurmentColumns.Count == 0) return;
            totalMeasurementColumnList = bIReportTotalsService.BuildTotalMeasurementColumnList();
        }

        private void AddToTotalsMeasurementValues(DataRow row, DataColumn column)
        {
            if (!bIReportXMLData.IncludeTotals) return;

            var agColumn = bITabularViewSettings.Columns.Where(a => a.Name == column.ColumnName).FirstOrDefault();

            if (bIReportTotalsService.ValidTotalColumn(agColumn))
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
            if (!bIReportXMLData.IncludeTotals || totalMeasurementColumnList == null || totalMeasurementColumnList.Count == 0) return;
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
            var total = pdfTotal != null ? Math.Round(pdfTotal.Total, 3).ToString() : "";
            string cellStyle = total != "" ? "style=\"background-color:#FFAC1C\"" : "";
            stringBuilder.Append("<td "+ cellStyle + ">" + total + "</td>");
        }
    }
}