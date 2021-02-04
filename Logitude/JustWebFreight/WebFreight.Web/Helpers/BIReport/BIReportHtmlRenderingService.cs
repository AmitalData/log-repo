using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace WebFreight.Web.Helpers.BIReport
{
    public class BIReportHtmlRenderingService
    {

        private DataTable bIReportdataTable = null;
        private StringBuilder stringBuilder = null;

        public BIReportHtmlRenderingService(DataTable bIReportdataTable)
        {
            this.bIReportdataTable = bIReportdataTable;
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
        }

        private void BuildDataColumns( DataRow row)
        {
            foreach (DataColumn column in bIReportdataTable.Columns)
            {
                stringBuilder.Append("<td>" + row[column.ColumnName].ToString() + "</td>");
            }
        }

        private string GetBIReportTableStyle()
        {
            return " <style> table {font-family: arial, sans-serif;border-collapse: collapse;width: 100%;} td, th {border: 1px solid #dddddd;text-align: left;padding: 8px;} tr:nth-child(even) { background-color: #dddddd;}</style>";
        }

    }
}