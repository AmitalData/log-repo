using Logitude.ReportTests.Models;
using Logitude.ReportTests.Models.Builders;
using Logitude.ReportTests.Services.Mapps;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ReportTests.Services
{

    public class ReportFilterItemService<T> where T : ReportFilterMapper
    {
        private readonly List<ReportFliterItem> reportFliterItems;
        private readonly T reportFilterMapper;
        public ReportFilterItemService()
        {
            reportFliterItems = new List<ReportFliterItem>();
            reportFilterMapper = (T)Activator.CreateInstance(typeof(T));
        }

        public List<ReportFliterItem> Build(Table filterTable)
        {
            filterTable.CreateDynamicSet().ToList().ForEach(reportFliterItem =>
            {
                reportFliterItems.Add(GetNewReportFliterItem(reportFliterItem));
            });
            return new List<ReportFliterItem>(reportFliterItems);
        }

        private ReportFliterItem GetNewReportFliterItem(ExpandoObject reportFliterItemObject)
        {
            var reportFliterItem = new ReportFliterItemBuilder().WithDefualtValues()
               .Name(reportFliterItemObject.Get<string>("Name"))
               .Map(reportFliterItemObject.Get<string>("Map"))
               .Value(reportFliterItemObject.Get<string>("Value"))
               .Build();

            return reportFilterMapper.RenderReportFliterItemValue(reportFliterItem);
        }
    }
}
