using Logitude.ReportTests.Models;
using Logitude.ReportTests.Models.Builders;
using Logitude.Test.Base;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ReportTests.Services
{
    public class ReportFilterService
    {

        private readonly ReportService reportService;
        private readonly ReportFilterItemService reportFilterItemService;

        public ReportFilterService()
        {
            reportFilterItemService = new ReportFilterItemService();
            reportService = new ReportService();
        }


        public ReportFliter Create(Table reportTable)
        {
            var dataTable = reportTable.CreateDynamicInstance();
            var report = reportService.GetByCode(dataTable.Get<string>("Code"));
            return new ReportFliterBuilder()
                .WithDefualtValues()
                .ReportCode(dataTable.Get<string>("Code"))
                .ReportName(dataTable.Get<string>("Name"))
                .ReportId((string)report.Id)
                .DefaultTemplateId(reportService.GetDefaultTemplateId(dataTable.Get<string>("Template"), (string)report.Id, (string)report.DefaultTemplateId))
                .Build();
        }


        public List<ReportFliterItem> BuildFilterItems(Table filterTable)
        {
            return reportFilterItemService.Build(filterTable);
        }

    }
}
