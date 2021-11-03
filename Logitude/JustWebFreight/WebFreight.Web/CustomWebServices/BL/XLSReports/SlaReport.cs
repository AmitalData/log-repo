using System;
using System.Collections.Generic;
using System.Data;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
namespace WebFreight.Web.CustomWebServices.BL.XLSReports
{

    public class SlaReport
    {
        public byte[] GetSlaReport(string tenant,string fromDate,string toDate,string integratorCode,string reportType)
        {
            DataTable dt = null;
            var settingCol = new BITabularViewSettings()
            {
                Columns = new List<Column>()
            };
            dt = new DataTable("Supplier Invioce Item Certificat Errors");

            settingCol.Columns.Add(new Column() { Index = 1, Code = "ExcelRow", Name = "ExcelRow", DataTypeCode = "String", Width = 150, });
            dt.Columns.Add(new DataColumn() { Caption = /*"ExcelRow"*/"שטר מטען ראשי", ColumnName = "ExcelRow", DataType = System.Type.GetType("System.String"), });

            settingCol.Columns.Add(new Column() { Index = 2, Code = "CustomfileNr", Name = "CustomfileNr", DataTypeCode = "String", Width = 150, });
            dt.Columns.Add(new DataColumn() { Caption = /*"CustomfileNr"*/"אינטגרטור", ColumnName = "CustomfileNr", DataType = System.Type.GetType("System.String"), });

            settingCol.Columns.Add(new Column() { Index = 3, Code = "DeclarationId", Name = "DeclarationId", DataTypeCode = "String", Width = 150, });
            dt.Columns.Add(new DataColumn() { Caption = /*"DeclarationId"*/"שטר מטען בלדר", ColumnName = "DeclarationId", DataType = System.Type.GetType("System.String"), });

            settingCol.Columns.Add(new Column() { Index = 4, Code = "SupplierItemInvoice", Name = "SupplierItemInvoice", DataTypeCode = "String", Width = 150, });
            dt.Columns.Add(new DataColumn() { Caption = /*"SupplierItemInvoice"*/"תאריך נחיתה", ColumnName = "SupplierItemInvoice", DataType = System.Type.GetType("System.String"), });

            settingCol.Columns.Add(new Column() { Index = 5, Code = "Model", Name = "Model", DataTypeCode = "String", Width = 150, });
            dt.Columns.Add(new DataColumn() { Caption = /*"Model"*/"תאריך התרה", ColumnName = "Model", DataType = System.Type.GetType("System.String"), });

            settingCol.Columns.Add(new Column() { Index = 6, Code = "Errors", Name = "Errors", DataTypeCode = "String", Width = 150, });
            dt.Columns.Add(new DataColumn() { Caption = /*"Errors"*/"תאריך יציאה ממן", ColumnName = "Errors", DataType = System.Type.GetType("System.String"), });

            settingCol.Columns.Add(new Column() { Index = 6, Code = "Errors", Name = "Errors", DataTypeCode = "String", Width = 150, });
            dt.Columns.Add(new DataColumn() { Caption = /*"Errors"*/"תאריך הפצה", ColumnName = "Errors", DataType = System.Type.GetType("System.String"), });
            
            settingCol.Columns.Add(new Column() { Index = 6, Code = "SLA", Name = "SLA", DataTypeCode = "int", Width = 150, });
            dt.Columns.Add(new DataColumn() { Caption = /*"Errors"*/"SLA", ColumnName = "SLA", DataType = System.Type.GetType("System.int"), });
            /*errorsList.ForEach(r =>
            {
                var newrow = dt.NewRow();
                newrow[0] = r.ExcelRow;
                newrow[1] = r.CustomfileNr;
                newrow[2] = r.DeclarationId;
                newrow[3] = r.SupplierItemInvoice;
                newrow[4] = r.Model;
                newrow[5] = r.Errors;
                dt.Rows.Add(newrow);
            });*/
            var xls = new ExportToExcelHelper();
            var res = xls.ExportDataTableToExcel(dt, int.Parse(tenant), settingCol);
            return res;
        }
    }
    public class ReportsSetting
    {
        public int Tenant { get; set; }
        public DateTime FromRequestCreateDate { get; set; }
        public DateTime ToRequestCreateDate { get; set; }
        public string IntegratorCode { get; set; }
        public string ReportType { get; set; }

    }
}