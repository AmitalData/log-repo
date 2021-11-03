using System;
using System.Collections.Generic;
using System.Data;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
namespace WebFreight.Web.CustomWebServices.BL.XLSReports
{

    public class SlaReport
    {
        public byte[] GetSlaReport(ReportsSetting setting)
        {
            DataTable dt = null;
            var settingCol = new BITabularViewSettings()
            {
                Columns = new List<Column>()
            };
            dt = new DataTable("Supplier Invioce Item Certificat Errors");

            settingCol.Columns.Add(new Column() { Index = 1, Code = "ExcelRow", Name = "ExcelRow", DataTypeCode = "String", Width = 150, });
            dt.Columns.Add(new DataColumn() { Caption = /*"ExcelRow"*/"שורה בבקשה", ColumnName = "ExcelRow", DataType = System.Type.GetType("System.String"), });

            settingCol.Columns.Add(new Column() { Index = 2, Code = "CustomfileNr", Name = "CustomfileNr", DataTypeCode = "String", Width = 150, });
            dt.Columns.Add(new DataColumn() { Caption = /*"CustomfileNr"*/"מס' תיק", ColumnName = "CustomfileNr", DataType = System.Type.GetType("System.String"), });

            settingCol.Columns.Add(new Column() { Index = 3, Code = "DeclarationId", Name = "DeclarationId", DataTypeCode = "String", Width = 150, });
            dt.Columns.Add(new DataColumn() { Caption = /*"DeclarationId"*/"מס' הצהרה", ColumnName = "DeclarationId", DataType = System.Type.GetType("System.String"), });

            settingCol.Columns.Add(new Column() { Index = 4, Code = "SupplierItemInvoice", Name = "SupplierItemInvoice", DataTypeCode = "String", Width = 150, });
            dt.Columns.Add(new DataColumn() { Caption = /*"SupplierItemInvoice"*/"מס' חשבון ספק", ColumnName = "SupplierItemInvoice", DataType = System.Type.GetType("System.String"), });

            settingCol.Columns.Add(new Column() { Index = 5, Code = "Model", Name = "Model", DataTypeCode = "String", Width = 150, });
            dt.Columns.Add(new DataColumn() { Caption = /*"Model"*/"דגם", ColumnName = "Model", DataType = System.Type.GetType("System.String"), });

            settingCol.Columns.Add(new Column() { Index = 6, Code = "Errors", Name = "Errors", DataTypeCode = "String", Width = 150, });
            dt.Columns.Add(new DataColumn() { Caption = /*"Errors"*/"הודעת שגיאה", ColumnName = "Errors", DataType = System.Type.GetType("System.String"), });
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
            var res = xls.ExportDataTableToExcel(dt, setting.Tenant, settingCol);
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