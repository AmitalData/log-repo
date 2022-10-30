using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Data;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Unifreight.Data.AmitalModel.Repsitories;
using WebFreight.Web.CustomWebServices.BL.XLSReports.SlaReportTypes;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
namespace WebFreight.Web.CustomWebServices.BL.XLSReports
{

    public class ExportReport
    {
        public int tenant;
        public ExportReport(string tenant)
        {
            this.tenant = int.Parse(tenant);
        }
        public byte[] GetExportReport( string ExportFromDate, string ExportToDate)
        {
            var context = CustomContext.GetContext((int)tenant);
            List<DataTable> dataTables = new List<DataTable>();
            List <BITabularViewSettings> settingCols= new List<BITabularViewSettings>();
            //report1
            DataTable dt1 = SetDataTableForExportReport1();
            var settingCol1 = SetSettingColForExportReport1();
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(this.tenant);
            Nullable<DateTime> hatraFrom, hatraTo, ExportFrom, ExportTo = null;
         
            ExportFrom = this.TryParse(ExportFromDate);
            ExportTo = this.TryParse(ExportToDate);
            var ExportReportDataList1 = new List<ExportReport1>();
            var ExportReportDataL1 = declarationQueryService.GetReportDeclarationForExportReport1( ExportFrom, ExportTo);           
            foreach (var item in ExportReportDataL1)
            {
                var ExportReportData = new ExportReport1();
                ExportReportData.company = item.company;
                ExportReportData.submit = item.submit;
                ExportReportData.release = item.release;
                ExportReportData.closed = item.closed;
                ExportReportData.notSubmit = item.notSubmit;

  
                ExportReportDataList1.Add(ExportReportData);
            }
            if (ExportReportDataList1.Count > 0)
            {
                dt1 = this.ExportData1(ExportReportDataList1, dt1);
            }
            dataTables.Add(dt1);
            settingCols.Add(settingCol1);
            //report2
            DataTable dt2 = SetDataTableForExportReport2();
            var settingCol2 = SetSettingColForExportReport2();
                               
            var ExportReportDataList2 = new List<ExportReport2>();
            var ExportReportDataL2 = declarationQueryService.GetReportDeclarationForExportReport2(ExportFrom, ExportTo);
            foreach (var item in ExportReportDataL2)
            {
                var ExportReportData = new ExportReport2();
                ExportReportData.company = item.company;
                ExportReportData.localname = item.localname;
                ExportReportData.count = item.count;
              
               
                ExportReportDataList2.Add(ExportReportData);
            }
            if (ExportReportDataList2.Count > 0)
            {
                dt2 = this.ExportData2(ExportReportDataList2, dt2);
            }


            dataTables.Add(dt2);
            settingCols.Add(settingCol2);
            var xls = new ExportToExcelHelper();
           

            var res = xls.ExportDataTableToExcelForExportStorage(dataTables, this.tenant, settingCols, 2);
            return res;
        }
        private DataTable ExportData1(List<ExportReport1> ExportReportDataList, DataTable dt)
        {
            ExportReportDataList.ForEach(r =>
            {
                var newrow = dt.NewRow();
                newrow[0] = r.company;
                newrow[1] = r.submit;
                newrow[2] = r.release;
                newrow[3] = r.closed;
                newrow[4] = r.notSubmit;
             

                dt.Rows.Add(newrow);
            });            
            return dt;
        }
        private DataTable ExportData2(List<ExportReport2> ExportReportDataList, DataTable dt)
        {
            ExportReportDataList.ForEach(r =>
            {
                var newrow = dt.NewRow();
                newrow[0] = r.company;
                newrow[1] = r.localname;
                newrow[2] = r.count;
           


                dt.Rows.Add(newrow);
            });
            return dt;
        }

        private BITabularViewSettings SetSettingColForExportReport1()
        {
            var settingCol = new BITabularViewSettings()
            {
                Columns = new List<Column>()
            };

            settingCol.Columns.Add(GetColumn(1, "company", "company", "string", 180));
            settingCol.Columns.Add(GetColumn(2, "submit", "submit", "Int32", 150));
            settingCol.Columns.Add(GetColumn(3, "release", "release", "Int32", 150));
            settingCol.Columns.Add(GetColumn(4, "closed", "closed", "Int32", 150));
            settingCol.Columns.Add(GetColumn(5, "notSubmit", "notSubmit", "Int32", 150));

            return settingCol;
        }

        private BITabularViewSettings SetSettingColForExportReport2()
        {
            var settingCol = new BITabularViewSettings()
            {
                Columns = new List<Column>()
            };

            settingCol.Columns.Add(GetColumn(1, "company", "company", "string", 180));
            settingCol.Columns.Add(GetColumn(2, "localname", "localname", "string", 150));
            settingCol.Columns.Add(GetColumn(3, "count", "count", "Int32", 150));
           

            return settingCol;
        }
        private DataTable SetDataTableForExportReport1()
        {
            var dt = new DataTable("דוח יצוא");
            dt.Columns.Add(GetDataColumn("company", "company", "System.String"));
            dt.Columns.Add(GetDataColumn("submit", "submit", "System.Int32"));
            dt.Columns.Add(GetDataColumn("release", "release", "System.Int32"));
            dt.Columns.Add(GetDataColumn("closed", "closed", "System.Int32"));
            dt.Columns.Add(GetDataColumn("notSubmit", "notSubmit", "System.Int32"));
            return dt;
        }
        private DataTable SetDataTableForExportReport2()
        {
            var dt = new DataTable("דוח יצוא");
            dt.Columns.Add(GetDataColumn("company", "company", "System.String"));
            dt.Columns.Add(GetDataColumn("localname", "localname", "System.String"));
            dt.Columns.Add(GetDataColumn("count", "count", "System.Int32"));
         
            return dt;
        }
        private DateTime? TryParse(string date)
        {
            DateTime? parseDate;
            if (string.IsNullOrEmpty(date) == false)
            {
                var dtValue = new DateTime();
                if (DateTime.TryParse(date, out dtValue))
                {
                    parseDate = dtValue;
                }
                else
                {
                    parseDate = null;
                }
            }
            else
            {
                parseDate = null;
            }
            return parseDate;
        }
        private Column GetColumn(int index, string code, string name, string datatype, int width)
        {
            return new Column() { Index = index, Code = code, Name = name, DataTypeCode = datatype, Width = width, };
        }
        private DataColumn GetDataColumn(string caption, string columnName, string dateType)
        {
            return new DataColumn() { Caption = caption, ColumnName = columnName, DataType = System.Type.GetType(dateType) };
        }
    }
    public class ExportReport1
    {
        public string company { get; set; }
        public int submit { get; set; }
        public int release { get; set; }

        public int closed { get; set; }
        public int notSubmit { get; set; }

    }
    public class ExportReport2
    {
        public string company { get; set; }
        public string localname { get; set; }
        public int count { get; set; }


    }
}