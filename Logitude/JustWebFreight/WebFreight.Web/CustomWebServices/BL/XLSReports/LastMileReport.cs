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

    public class LastMileReport
    {
        public int tenant;
        public LastMileReport(string tenant)
        {
            this.tenant = int.Parse(tenant);
        }
        public byte[] GetLastMileReport(string hatraFromDate, string hatraToDate, string lastMileFromDate, string LastMileToDate, string airline, string trucker, string mawb)
        {
            var context = CustomContext.GetContext((int)tenant);
            DataTable dt = SetDataTableForLastMileReport();
            var settingCol = SetSettingColForLastMileReport();
            CourierMasterQueryService courierMasterQueryService = new CourierMasterQueryService(this.tenant);
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(this.tenant);
            Nullable<DateTime> hatraFrom, hatraTo, lastMileFrom, lastMileTo = null;
            hatraFrom=this.TryParse(hatraFromDate);
            hatraTo = this.TryParse(hatraToDate);
            lastMileFrom = this.TryParse(lastMileFromDate);
            lastMileTo = this.TryParse(LastMileToDate);
            var lastMileReportDataList = new List<LastMileReportData>();
            var OpenCourierMasters = courierMasterQueryService.GetAllCourierMasterForLastmileReport(hatraFrom, hatraTo, lastMileFrom, lastMileTo, airline, trucker, mawb, tenant);
            var qs = new DeclarationCourierStatusQueryService(context);
            int MawbCountr = 0;
            foreach (var item in OpenCourierMasters)
            {
                var lastMileReportData = new LastMileReportData();
                lastMileReportData.IntegratorName = item.GetType().GetProperty("IntegratorName").GetValue(item, null);
                lastMileReportData.CourierHawb= item.GetType().GetProperty("CourierHAWB").GetValue(item, null);
                lastMileReportData.LastMileServiceType = item.GetType().GetProperty("LastMileServiceType").GetValue(item, null);
                lastMileReportData.Trucker = item.GetType().GetProperty("TruckerName").GetValue(item, null);
                lastMileReportData.LastMileStatusName = item.GetType().GetProperty("LastMileStatusName").GetValue(item, null);
                lastMileReportData.TerminalReleaseDate = item.GetType().GetProperty("TerminalReleaseDate").GetValue(item, null);
                lastMileReportData.HatraDate = item.GetType().GetProperty("HatraDate").GetValue(item, null);
                lastMileReportData.EstimatedArrivalDate = item.GetType().GetProperty("EstimatedArrivalDate").GetValue(item, null);
                lastMileReportData.LastMileStatusDate = item.GetType().GetProperty("LastMileStatusDate").GetValue(item, null);
                if (lastMileReportDataList.Find(x => x.CourierHawb == item.GetType().GetProperty("CourierHAWB").GetValue(item, null)) == null)
                {
                    MawbCountr++;
                }
                lastMileReportDataList.Add(lastMileReportData);
            }
            if (lastMileReportDataList.Count > 0)
            {
                dt = this.ExportData(lastMileReportDataList, dt, MawbCountr);
            }
            var xls = new ExportToExcelHelper();
            var res = xls.ExportDataTableToExcel(dt, this.tenant, settingCol);
            return res;
        }
        private DataTable ExportData(List<LastMileReportData> lastMileReportDataList, DataTable dt,int MawbCountr)
        {
            lastMileReportDataList.ForEach(r =>
            {
                var newrow = dt.NewRow();
                newrow[0] = r.CourierHawb;
                newrow[1] = r.IntegratorName;
                newrow[2] = r.Trucker;
                newrow[3] = r.LastMileServiceType;
                newrow[4] = r.LastMileStatusName;
                newrow[5] = r.LastMileStatusDate;
                newrow[6] = r.EstimatedArrivalDate;
                newrow[7] = r.HatraDate;
                newrow[8] = r.TerminalReleaseDate;

                dt.Rows.Add(newrow);
            });            
            var Lastrow = dt.NewRow();
            Lastrow[0] = ":כמות שטרי מטען בלדר";
            Lastrow[1] = MawbCountr;
            dt.Rows.Add(Lastrow);
            return dt;
        }

        private BITabularViewSettings SetSettingColForLastMileReport()
        {
            var settingCol = new BITabularViewSettings()
            {
                Columns = new List<Column>()
            };

            settingCol.Columns.Add(GetColumn(1, "CourierHawb", "CourierHawb", "string", 180));
            settingCol.Columns.Add(GetColumn(2, "IntegratorName", "IntegratorName", "string", 150));
            settingCol.Columns.Add(GetColumn(3, "TruckerId", "TruckerId", "string", 150));
            settingCol.Columns.Add(GetColumn(4, "LastMileServiceType", "LastMileServiceType", "string", 150));
            settingCol.Columns.Add(GetColumn(5, "LastMileStatusName", "LastMileStatusName", "string", 150));
            settingCol.Columns.Add(GetColumn(6, "LastMileStatusDate", "LastMileStatusDate", "DateTime?", 150));
            settingCol.Columns.Add(GetColumn(7, "EstimatedArrivalDate", "EstimatedArrivalDate", "DateTime", 150));
            settingCol.Columns.Add(GetColumn(8, "HatraDate", "HatraDate", "string", 150));
            settingCol.Columns.Add(GetColumn(9, "TerminalReleaseDate", "TerminalReleaseDate", "string", 150));

            return settingCol;
        }
        private DataTable SetDataTableForLastMileReport()
        {
            var dt = new DataTable("דוח הפצה");
            dt.Columns.Add(GetDataColumn("מספר שטר מטען בלדר", "CourierHawb", "System.String"));
            dt.Columns.Add(GetDataColumn("שם אינטגרטור ", "IntegratorName", "System.String"));
            dt.Columns.Add(GetDataColumn("שם מפיץ", "TruckerId", "System.String"));
            dt.Columns.Add(GetDataColumn("סוג שירות", "LastMileServiceType", "System.String"));
            dt.Columns.Add(GetDataColumn("שם סטטוס אחרון", "LastMileStatusName", "System.String"));
            dt.Columns.Add(GetDataColumn("תאריך ושעת הסטטוס", "LastMileStatusDate", "System.String"));
            dt.Columns.Add(GetDataColumn("תאריך הגעה משוער", "EstimatedArrivalDate", "System.DateTime"));
            dt.Columns.Add(GetDataColumn("תאריך התרה", "HatraDate", "System.String"));
            dt.Columns.Add(GetDataColumn("תאריך יציאה ממסוף", "TerminalReleaseDate", "System.String"));
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
    public class LastMileReportData
    {
        public DateTime LastMileDate { get; set; }
        public string CourierHawb { get; set; }
        public string IntegratorName { get; set; }
        public string Trucker { get; set; }
        public string LastMileServiceType { get; set; }
        public  string LastMileStatusName { get; set; }
        public DateTime? LastMileStatusDate { get; set; }
        public  DateTime? EstimatedArrivalDate { get; set; }
        public DateTime? HatraDate { get; set; }
        public DateTime? TerminalReleaseDate { get; set; }
      
    }
}