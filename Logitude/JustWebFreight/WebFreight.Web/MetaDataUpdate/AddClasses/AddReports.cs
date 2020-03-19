using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.MetaDataUpdate.DetailClasses;

namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddReports
    {

        public static ReportGroup AddReportGroup(ReportGroupDetails ReportGroupDetails, ReportGroupRepository ReportRepository, Dictionary<string, ReportGroup> tenantReportGroups)
        {
            if (tenantReportGroups.Keys.Contains(ReportGroupDetails.Code))
            {
                ReportGroup ReportGroup = tenantReportGroups[ReportGroupDetails.Code];
                ReportGroup.Code = ReportGroupDetails.Code;
                ReportGroup.EnglishName = ReportGroupDetails.EnglishName;
                ReportGroup.LocalName = ReportGroupDetails.LocalName;
                ReportGroup.Tenant = ReportGroupDetails.Tenant;
                ReportGroup.OrderNumber = ReportGroupDetails.OrderNumber;
               // ReportGroup.SearchFields = ReportGroupDetails.Code + "," + ReportGroupDetails.Name + "," + ReportGroupDetails.FilterControlName + "," + ReportGroupDetails.Description;
                ReportRepository.Update(ReportGroup);
                return ReportGroup;
            }

            else
            {
                ReportGroup newReport = new ReportGroup()
                {
                    Code = ReportGroupDetails.Code,
                    Tenant = ReportGroupDetails.Tenant,
                    EnglishName= ReportGroupDetails.EnglishName,
                    LocalName = ReportGroupDetails.LocalName,
                    Id = IdCounter.GetNumber("ReportGroup", ReportGroupDetails.Tenant).ToString(),
                    OrderNumber = ReportGroupDetails.OrderNumber,                    
                };

                ReportRepository.Add(newReport);
                return newReport;
            }
        }

        public static void AddReport(ReportDetails ReportDetails, ReportRepository ReportRepository, Dictionary<string, Report> tenantReports)
        {
            if (tenantReports.Keys.Contains(ReportDetails.Code))
            {
                Report Report = tenantReports[ReportDetails.Code];
                Report.Code = ReportDetails.Code;
                Report.Description = ReportDetails.Description;
                Report.FilterControlName = ReportDetails.FilterControlName;
                Report.Name = ReportDetails.Name;
                Report.LocalName = ReportDetails.LocalName;
                Report.ReportGroupId = ReportDetails.ReportGroupId;
                Report.Tenant = ReportDetails.Tenant;
                Report.SearchFields = ReportDetails.Code + "," + ReportDetails.Name + "," + ReportDetails.FilterControlName + "," + ReportDetails.Description;
                Report.FeatureId = ReportDetails.FeatureId;
                Report.FilterHtmlComponentUrl = ReportDetails.FilterHtmlComponentUrl;
                Report.FeatureUniqeCode = ReportDetails.FeatureUniqeCode;
                ReportRepository.Update(Report);
            }

            else
            {
                Report newReport = new Report()
                {
                    Code = ReportDetails.Code,
                    Tenant = ReportDetails.Tenant,
                    Description = ReportDetails.Description,
                    Name = ReportDetails.Name,
                    ReportGroupId = ReportDetails.ReportGroupId,
                    FilterControlName = ReportDetails.FilterControlName,
                    SearchFields = ReportDetails.Code + "," + ReportDetails.Name + "," + ReportDetails.FilterControlName + "," + ReportDetails.Description,
                    Id = IdCounter.GetNumber("Report", ReportDetails.Tenant).ToString(),
                    FeatureId = ReportDetails.FeatureId,
                    FilterHtmlComponentUrl = ReportDetails.FilterHtmlComponentUrl,
                    FeatureUniqeCode = ReportDetails.FeatureUniqeCode,

                };

                ReportRepository.Add(newReport);
            }
        }
    }
}