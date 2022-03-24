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

    public class SlaConcentratedReport
    {
        public int tenant;
        public List<GGGHDAY> holidayList;
        public SlaConcentratedReport(string tenant)
        {
            this.tenant = int.Parse(tenant);
        }
        public byte[] GetConcentratedReport(string fromDate, string toDate, string integratorCode)
        {
            var context = CustomContext.GetContext(this.tenant);
            DataTable dt = SetDataTableForConcentratedReport();
            var settingCol = SetSettingColForConcentratedReport();
            CourierMasterQueryService courierMasterQueryService = new CourierMasterQueryService(this.tenant);
            var toDateTime = DateTime.Parse(toDate);
            var fromDateTime = DateTime.Parse(fromDate);
            var OpenCourierMasters = courierMasterQueryService.AllCourierMastersWithLandingDateBetweenTwoDates(tenant, fromDateTime, toDateTime, integratorCode);
            var qs = new DeclarationCourierStatusQueryService(context);
            var GGGHDAYRepository = new GGGHDAYRepository(this.tenant);
            this.holidayList = GGGHDAYRepository.All();
            var concentratedReportDataList = new List<ConcentratedReportData>();
            foreach (var item in OpenCourierMasters)
            {
                if (item.LandingDate.HasValue)
                {
                    var concentratedReportData = new ConcentratedReportData();
                    concentratedReportData.MAWB = item.MAWB;
                    concentratedReportData.NoOfCourierHawb = item.NoOfCourierHawb;
                    concentratedReportData.LandingDate = item.LandingDate?.Date.ToShortDateString();
                    concentratedReportData.SlaDaysCount = new SlaDaysCounted();
                    var DeclarationDataForSlaReportList = qs.GetDeclarationDataForSlaReportByMasterId(this.tenant, item.Id);
                    foreach (DeclarationDataForSlaReport DeclarationDataForSlaReport in DeclarationDataForSlaReportList)
                    {
                        var days = GetSlaDaysForDecId(DeclarationDataForSlaReport, item.LandingDate);
                        concentratedReportData = SetConcentratedSlaDays(concentratedReportData, days);
                    }
                    concentratedReportData = GetConcentratedSlaProzents(concentratedReportData);
                    concentratedReportDataList.Add(concentratedReportData);
                }
            }
            if (concentratedReportDataList.Count > 0)
            {
                dt = this.ExportData(concentratedReportDataList, dt);
            }
            var xls = new ExportToExcelHelper();
            var res = xls.ExportDataTableToExcel(dt, this.tenant, settingCol);
            return res;
        }
        private DataTable ExportData(List<ConcentratedReportData> concentratedReportDataList, DataTable dt)
        {
            concentratedReportDataList.ForEach(r =>
            {
                var newrow = dt.NewRow();
                newrow[0] = r.MAWB;
                newrow[1] = r.NoOfCourierHawb;
                newrow[2] = r.LandingDate;
                newrow[3] = r.OneToTwoProzent;
                newrow[4] = r.TwoToFourProzent;
                newrow[5] = r.FourToEightProzent;
                newrow[6] = r.EightPlusProzent;
                dt.Rows.Add(newrow);
            });
            return dt;
        }
        private ConcentratedReportData SetConcentratedSlaDays(ConcentratedReportData concentratedReportData, int days)
        {
            if (days >= 8) // get 8+
            {
                concentratedReportData.SlaDaysCount.EightPlusDays++;
            }
            if (days > 0 && days < 2) // get 1-2
            {
                concentratedReportData.SlaDaysCount.OneToTwoDays++;
            }
            if (days >= 2 && days < 4) // get 3-4
            {
                concentratedReportData.SlaDaysCount.TwoToFourDays++;
            }
            if (days >= 4 && days < 8) // get 5-6-7-8
            {
                concentratedReportData.SlaDaysCount.FourToEightDays++;
            }
            concentratedReportData.SlaDaysCount.TotalSlaDays++;
            return concentratedReportData;
        }
        private ConcentratedReportData GetConcentratedSlaProzents(ConcentratedReportData report)
        {
            int totalDays = report.SlaDaysCount.TotalSlaDays;
            report.OneToTwoProzent = this.ConvertToProzent((int)Math.Round((double)(100 * report.SlaDaysCount.OneToTwoDays) / totalDays));
            report.TwoToFourProzent = this.ConvertToProzent((int)Math.Round((double)(100 * report.SlaDaysCount.TwoToFourDays) / totalDays));
            report.FourToEightProzent = this.ConvertToProzent((int)Math.Round((double)(100 * report.SlaDaysCount.FourToEightDays) / totalDays));
            report.EightPlusProzent = this.ConvertToProzent((int)Math.Round((double)(100 * report.SlaDaysCount.EightPlusDays) / totalDays));
            return report;
        }

        private string ConvertToProzent(double d)
        {
            return (d.ToString() + "%");
        }

        private int GetSlaDaysForDecId(DeclarationDataForSlaReport declarationDataForSlaReport, DateTime? landingDate)
        {
            int SlaDays = 0;

            if (declarationDataForSlaReport.Delivered && declarationDataForSlaReport.LastMileStatusDate.HasValue)
            {
                double daysBetween = (declarationDataForSlaReport.LastMileStatusDate.Value - landingDate.Value).TotalDays;
                if (daysBetween > 0)
                {
                    DateTime day = (DateTime)(landingDate?.Date);
                    while (daysBetween >= 0)
                    {
                        if (daysBetween > 1 && day.DayOfWeek != DayOfWeek.Friday && day.DayOfWeek != DayOfWeek.Saturday && this.holidayList.Find(x => x.HOLIDAY.Day == day.Day && x.HOLIDAY.Month == day.Month && x.HOLIDAY.Year == day.Year) == null)
                        {
                            SlaDays++;
                        }
                        daysBetween--;
                        day = day.AddDays(1);
                    }
                }

            }
            return SlaDays;
        }
        private BITabularViewSettings SetSettingColForConcentratedReport()
        {
            var settingCol = new BITabularViewSettings()
            {
                Columns = new List<Column>()
            };

            settingCol.Columns.Add(GetColumn(1, "MAWB", "MAWB", "string", 180));

            settingCol.Columns.Add(GetColumn(2, "NoOfCourierHawb", "NoOfCourierHawb", "string", 150));

            settingCol.Columns.Add(GetColumn(3, "LandingDate", "LandingDate", "string", 150));

            settingCol.Columns.Add(GetColumn(4, "1-2", "1-2", "string", 150));

            settingCol.Columns.Add(GetColumn(5, "2-4", "2-4", "string", 150));

            settingCol.Columns.Add(GetColumn(6, "4-8", "4-8", "string", 150));

            settingCol.Columns.Add(GetColumn(7, "8+", "8+", "string", 150));

            return settingCol;
        }
        private DataTable SetDataTableForConcentratedReport()
        {
            var dt = new DataTable("Sla Report");
            dt.Columns.Add(GetDataColumn("MAWB", "MAWB", "System.String"));
            dt.Columns.Add(GetDataColumn("NoOfCourierHawb", "NoOfCourierHawb", "System.String"));
            dt.Columns.Add(GetDataColumn("LandingDate", "LandingDate", "System.String"));
            dt.Columns.Add(GetDataColumn("1-2", "1-2", "System.String"));
            dt.Columns.Add(GetDataColumn("2-4", "2-4", "System.String"));
            dt.Columns.Add(GetDataColumn("4-8", "4-8", "System.String"));
            dt.Columns.Add(GetDataColumn("8+", "8+", "System.String"));
            return dt;
        }

        private Column GetColumn(int index, string code, string name, string datatype, int width)
        {
            return new Column() { Index = index, Code = code, Name = name, DataTypeCode = datatype, Width = width, };
        }
        private DataColumn GetDataColumn(string caption, string columnName, string dateType)
        {
            return new DataColumn() { Caption = caption, ColumnName = columnName, DataType = System.Type.GetType(dateType) };
        }

        public class ConcentratedReportData
        {
            public string MAWB { get; set; }
            public string NoOfCourierHawb { get; set; }
            public string LandingDate { get; set; }
            public string OneToTwoProzent { get; set; }
            public string TwoToFourProzent { get; set; }
            public string FourToEightProzent { get; set; }
            public string EightPlusProzent { get; set; }
            public SlaDaysCounted SlaDaysCount { get; set; }
        }
        public class SlaDaysCounted
        {
            public int TotalSlaDays { get; set; }
            public int OneToTwoDays { get; set; }
            public int TwoToFourDays { get; set; }
            public int FourToEightDays { get; set; }
            public int EightPlusDays { get; set; }
        }
    }
}