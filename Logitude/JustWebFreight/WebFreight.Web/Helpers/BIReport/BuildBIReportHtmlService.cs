using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace WebFreight.Web.Helpers.BIReport
{
    public class BuildBIReportHtmlService
    {
        private DataTable bIReportdataTable = null;
        public BuildBIReportHtmlService(DataTable bIReportdataTable)
        {
            this.bIReportdataTable = bIReportdataTable;
        }


        public string Run()
        {
            string result =  GetBIReportDataHtml();
            return result;
        }

        private string GetBIReportDataHtml()
        {
            StringBuilder stringBuilder = new StringBuilder("<!DOCTYPE html> <html> <head>" + GetBIReportTableStyle() + "</head><body>");
            stringBuilder.Append("<table>");
            stringBuilder.Append(GetBIReportHtmlHeaderTable());
            stringBuilder.Append(GetBIReportHtmlRowsTable());
            stringBuilder.Append("</table>");
            stringBuilder.Append("</body>");
            stringBuilder.Append("</html>");

            return stringBuilder.ToString();
        }

        private string GetBIReportHtmlHeaderTable()
        {
            StringBuilder stringBuilder = new StringBuilder("<thead><tr>");
            foreach (DataColumn column in bIReportdataTable.Columns)
            {
                stringBuilder.Append(" <th>" + column.ColumnName + "</th> ");
            }
            stringBuilder.Append("</tr></thead>");

            return stringBuilder.ToString();
        }

        private string GetBIReportHtmlRowsTable()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (DataRow row in bIReportdataTable.Rows)
            {
                stringBuilder.Append(" <tr>");

                foreach (DataColumn column in bIReportdataTable.Columns)
                {
                    stringBuilder.Append("<td>" + row[column.ColumnName].ToString() + "</td>");
                }

                stringBuilder.Append(" </tr>");

            }

            return stringBuilder.ToString();
        }

        private string GetBIReportTableStyle()
        {
            return " <style> table {font-family: arial, sans-serif;border-collapse: collapse;width: 100%;} td, th {border: 1px solid #dddddd;text-align: left;padding: 8px;} tr:nth-child(even) { background-color: #dddddd;}</style>";
        }



    }
}