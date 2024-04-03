using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class ReportMapping
    {
        public static void MapEntity(ReportPM reportPM, Report report, bool isNewState)
        {
            report.Name = reportPM.Name;
            report.FilterControlName = reportPM.FilterControlName;
            report.Description = reportPM.Description;
            report.SearchFields = reportPM.Tenant + "," + reportPM.Name + "," + reportPM.Description;
            report.Code = reportPM.Code;
            report.FeatureId = reportPM.FeatureId;
            report.Tenant = reportPM.Tenant;
            report.InActive = reportPM.InActive;
            report.FilterHtmlComponentUrl = reportPM.FilterHtmlComponentUrl;
            report.DefaultTemplateId = reportPM.DefaultTemplateId;
            report.DefaultMessageTemplateId = reportPM.DefaultMessageTemplateId;
            report.FeatureUniqeCode = reportPM.FeatureUniqeCode;
            report.DisablePreview = reportPM.DisablePreview;
            report.DefaultExcelTemplateId = reportPM.DefaultExcelTemplateId;
        }
    }
}
