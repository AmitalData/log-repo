using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Data;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Unifreight.Data.AmitalModel.Repsitories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.CustomWebServices.BL.XLSReports.SlaReportTypes
{
    public class SlaDetailedReport
    {
        public int tenant;
        public List<GGGHDAY> holidayList;
        public SlaDetailedReport(string tenant)
        {
            this.tenant = int.Parse(tenant);
        }
        public byte[] GetDetailedReport(string fromDate, string toDate, string integratorCode)
        {
            var context = CustomContext.GetContext(this.tenant);
            DataTable dt = SetDataTableForDetailedReport();
            var settingCol = SetSettingColForDetailedReport();
            CourierMasterQueryService courierMasterQueryService = new CourierMasterQueryService(this.tenant);
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(this.tenant);
            var toDateTime = DateTime.Parse(toDate);
            var fromDateTime = DateTime.Parse(fromDate);
            var OpenCourierMasters = courierMasterQueryService.AllCourierMastersWithLandingDateBetweenTwoDates(tenant, fromDateTime, toDateTime, integratorCode);
            var qs = new DeclarationCourierStatusQueryService(context);
            var GGGHDAYRepository = new GGGHDAYRepository(this.tenant);
            this.holidayList = GGGHDAYRepository.All();
            var detailedReportDataList = new List<DetailedReportData>();
            foreach (var item in OpenCourierMasters)
            {
                if (item.LandingDate.HasValue)
                {
                    var DeclarationDataForSlaReportList = qs.GetDeclarationDataForSlaReportByMasterId(this.tenant, item.Id);
                    foreach (DeclarationDataForSlaReport DeclarationDataForSlaReport in DeclarationDataForSlaReportList)
                    {
                        var detailedReportData = new DetailedReportData();
                        detailedReportData.MAWB = item.MAWB;
                        detailedReportData.IntegratorName = this.GetIntegratorName(integratorCode);
                        detailedReportData.LandingDate = item.LandingDate?.Date.ToShortDateString();
                        detailedReportData = GetSlaDaysForDecId(DeclarationDataForSlaReport, item.LandingDate, qs, detailedReportData);
                        detailedReportData.HatraDate = DeclarationDataForSlaReport.HatraDate.ToString();
                        detailedReportDataList.Add(detailedReportData);

                    }
                }
            }
            if (detailedReportDataList.Count > 0)
            {
                dt = this.ExportData(detailedReportDataList, dt);
            }
            var xls = new ExportToExcelHelper();
            var res = xls.ExportDataTableToExcel(dt, this.tenant, settingCol);
            return res;
        }
        private string GetIntegratorName(string IntegratorCode)
        {
            if (IntegratorCode != null)
            {
                CardQuery cardQuery = new CardQuery(this.tenant);
                CardPM cardPM = cardQuery.GetSinglePMFromCache(IntegratorCode, this.tenant);
                if (cardPM != null)
                {
                    return cardPM.EnglishName
                        ;
                }
            }
            return "";
        }
        private DetailedReportData GetSlaDaysForDecId(DeclarationDataForSlaReport declarationDataForSlaReport, DateTime? landingDate, DeclarationCourierStatusQueryService qs, DetailedReportData report)
        {
            int SlaDays = 0;
            report.TerminalReleaseDate = declarationDataForSlaReport.TerminalReleaseDate?.ToShortDateString();
            if (declarationDataForSlaReport.Delivered && declarationDataForSlaReport.LastMileStatusDate.HasValue)
            {
                report.LastMileDate = declarationDataForSlaReport.LastMileStatusDate?.ToShortDateString();
                double daysBetween = (declarationDataForSlaReport.LastMileStatusDate.Value - landingDate.Value).TotalDays;
                if (daysBetween > 0)
                {
                    DateTime day = (DateTime)(landingDate?.Date);
                    while (daysBetween >= 0)
                    {
                        if (day.DayOfWeek != DayOfWeek.Friday && day.DayOfWeek != DayOfWeek.Saturday && this.holidayList.Find(x => x.HOLIDAY.Day == day.Day && x.HOLIDAY.Month == day.Month && x.HOLIDAY.Year == day.Year) == null)
                        {
                            SlaDays++;
                        }
                        daysBetween--;
                        day = day.AddDays(1);
                    }
                }
            }

            report.CourierHawb = declarationDataForSlaReport.CourierHawb;

            report.Sla = SlaDays.ToString();
            return report;
        }
        private DataTable SetDataTableForDetailedReport()
        {
            var dt = new DataTable("Sla Report");
            dt.Columns.Add(GetDataColumn("MAWB", "MAWB", "System.String"));
            dt.Columns.Add(GetDataColumn("IntegratorName", "IntegratorName", "System.String"));
            dt.Columns.Add(GetDataColumn("CourierHawb", "CourierHawb", "System.String"));
            dt.Columns.Add(GetDataColumn("LandingDate", "LandingDate", "System.String"));
            dt.Columns.Add(GetDataColumn("HatraDate", "HatraDate", "System.String"));
            dt.Columns.Add(GetDataColumn("TerminalReleaseDate", "TerminalReleaseDate", "System.String"));
            dt.Columns.Add(GetDataColumn("LastMileDate", "LastMileDate", "System.String"));
            dt.Columns.Add(GetDataColumn("SLA", "SLA", "System.String"));
            return dt;
        }
        private BITabularViewSettings SetSettingColForDetailedReport()
        {
            var settingCol = new BITabularViewSettings()
            {
                Columns = new List<Column>()
            };

            settingCol.Columns.Add(GetColumn(1, "MAWB", "MAWB", "string", 150));

            settingCol.Columns.Add(GetColumn(2, "IntegratorName", "IntegratorName", "string", 150));

            settingCol.Columns.Add(GetColumn(3, "CourierHawb", "CourierHawb", "string", 150));

            settingCol.Columns.Add(GetColumn(4, "LandingDate", "LandingDate", "string", 150));

            settingCol.Columns.Add(GetColumn(5, "HatraDate", "HatraDate", "string", 150));

            settingCol.Columns.Add(GetColumn(6, "TerminalReleaseDate", "TerminalReleaseDate", "string", 150));

            settingCol.Columns.Add(GetColumn(7, "LastMileDate", "LastMileDate", "string", 150));

            settingCol.Columns.Add(GetColumn(7, "SLA", "SLA", "string", 150));


            return settingCol;
        }
        private Column GetColumn(int index, string code, string name, string datatype, int width)
        {
            return new Column() { Index = index, Code = code, Name = name, DataTypeCode = datatype, Width = width, };
        }
        private DataColumn GetDataColumn(string caption, string columnName, string dateType)
        {
            return new DataColumn() { Caption = caption, ColumnName = columnName, DataType = System.Type.GetType(dateType) };
        }
        private DataTable ExportData(List<DetailedReportData> detailedReportDataList, DataTable dt)
        {
            detailedReportDataList.ForEach(r =>
            {
                var newrow = dt.NewRow();
                newrow[0] = r.MAWB;
                newrow[1] = r.IntegratorName;
                newrow[2] = r.CourierHawb;
                newrow[3] = r.LandingDate;
                newrow[4] = r.HatraDate;
                newrow[5] = r.TerminalReleaseDate;
                newrow[6] = r.LastMileDate;
                newrow[7] = r.Sla;

                dt.Rows.Add(newrow);
            });
            return dt;
        }

    }
    public class DetailedReportData
    {
        public string MAWB { get; set; }
        public string IntegratorName { get; set; }
        public string CourierHawb { get; set; }
        public string LandingDate { get; set; }
        public string HatraDate { get; set; }
        public string TerminalReleaseDate { get; set; }
        public string LastMileDate { get; set; }
        public string Sla { get; set; }
    }
}