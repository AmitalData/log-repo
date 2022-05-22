using Logitude.ReportTests.Models;
using Logitude.ReportTests.Models.Builders;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System.Dynamic;
using Logitude.ReportTests.Services.Mapps;

namespace Logitude.ReportTests.Services
{
    public class ReportFilterService<T> where T : ReportFilterMapper
    {

        private readonly ReportService reportService;
        private readonly ReportFilterItemService<T> reportFilterItemService;

        public ReportFilterService()
        {
            reportFilterItemService = new ReportFilterItemService<T>();
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
