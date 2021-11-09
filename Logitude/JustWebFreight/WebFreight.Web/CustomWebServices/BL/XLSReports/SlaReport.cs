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

    public class SlaReport
    {
        public byte[] GetSlaReport(string fromDate, string toDate, string integratorCode, string reportType,string tenant)
        {
            byte[] report;
            if (reportType == "1")
            {
                var detailedReport = new SlaDetailedReport(tenant);
                return report = detailedReport.GetDetailedReport(fromDate, toDate, integratorCode);
            }
            if (reportType == "2")
            {
                var concentratedReport = new SlaConcentratedReport(tenant);
                return report = concentratedReport.GetConcentratedReport(fromDate, toDate, integratorCode);
            }
            return null;
        }
    }
}