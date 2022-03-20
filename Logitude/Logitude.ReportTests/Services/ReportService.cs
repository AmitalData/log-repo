using Logitude.ReportTests.Models;
using Logitude.ReportTests.Models.Builders;
using Logitude.Test.Base;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ReportTests.Services
{
    public class ReportService
    {
        private readonly EntityIdentityService entityIdentityService;
        private readonly ReportsTemplateService reportsTemplateService;
        public ReportService()
        {
            this.entityIdentityService = new EntityIdentityService();
            this.reportsTemplateService = new ReportsTemplateService();
        }

        public ReportFliter CreateFilterInstance(Table reportTable)
        {
            var dataTable = reportTable.CreateDynamicInstance();
            var report = GetReport(dataTable.Get<string>("Code"), dataTable.Get<string>("Name"));
            return new ReportFliterBuilder()
                .WithDefualtValues()
                .ReportCode(dataTable.Get<string>("Code"))
                .ReportName(dataTable.Get<string>("Name"))
                .ReportId((string)report.Id)
                .DefaultTemplateId(reportsTemplateService.GetDefaultTemplateId(dataTable.Get<string>("Template"), (string)report.Id, (string)report.DefaultTemplateId))
                .Build();
        }

        private dynamic GetReport(string code, string name)
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues()
              .Filter1Name("Tenant")
              .Filter1Operator("Equal")
              .Filter1Value(UserTenant.Tenant.ToString())
              .Filter2Name(string.IsNullOrEmpty(code) ? "Name" : "Code")
              .Filter2Operator("Equal")
              .Filter2Value(string.IsNullOrEmpty(code) ? name : code)
              .Build();
            ApiResponse<IEnumerable<dynamic>> response = APICaller.CallGetByFilters<IEnumerable<dynamic>>(Urls.ReportViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?["Id"];
        }

        public List<ReportFliterItem> BuildReportFliterItems(Table filterTable)
        {
            List<ReportFliterItem> reportFliterItems = new List<ReportFliterItem>();
            filterTable.CreateDynamicSet().ToList().ForEach(reportFliterItem =>
            {
                reportFliterItems.Add(GetReportFliterItem(reportFliterItem));
            });
            return new List<ReportFliterItem>(reportFliterItems);

        }

        private ReportFliterItem GetReportFliterItem(ExpandoObject reportFliterItem)
        {
            return new ReportFliterItemBuilder()
               .WithDefualtValues()
               .FieldName(reportFliterItem.Get<string>("Name"))
               .FieldValue(GetFilterValue(reportFliterItem))
               .FieldDataType(reportFliterItem.Get<string>("Type"))
               .Build();
        }

        private object GetFilterValue(ExpandoObject reportFliterItem)
        {
            if (string.IsNullOrEmpty(reportFliterItem.Get<string>("Name"))
                || string.IsNullOrEmpty(reportFliterItem.Get<string>("Value"))
                || string.IsNullOrEmpty(reportFliterItem.Get<string>("Table"))
                || string.IsNullOrEmpty(reportFliterItem.Get<string>("From")))
                return reportFliterItem.Get<string>("Value");

            return entityIdentityService.GetIdentity(new EntityIdentifier
            {
                Value = reportFliterItem.Get<string>("Value"),
                Table = reportFliterItem.Get<string>("Table"),
                From = reportFliterItem.Get<string>("From"),
            });
        }
    }
}
