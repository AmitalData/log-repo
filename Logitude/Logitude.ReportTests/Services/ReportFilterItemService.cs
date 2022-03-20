using Logitude.ReportTests.Models;
using Logitude.ReportTests.Models.Builders;
using Logitude.Test.Base;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ReportTests.Services
{
    public class ReportFilterItemService
    {
        private  EntityService entityService;
        private List<ReportFliterItem> reportFliterItems;
        public ReportFilterItemService()
        {
            entityService = new EntityService();
            reportFliterItems = new List<ReportFliterItem>();
        }

        public List<ReportFliterItem> Build(Table filterTable)
        {
            filterTable.CreateDynamicSet().ToList().ForEach(reportFliterItem =>
            {
                reportFliterItems.Add(GetNewReportFliterItem(reportFliterItem));
            });
            return new List<ReportFliterItem>(reportFliterItems);
        }

        private ReportFliterItem GetNewReportFliterItem(ExpandoObject reportFliterItem)
        {
            return new ReportFliterItemBuilder()
               .WithDefualtValues()
               .FieldName(reportFliterItem.Get<string>("PropertyName"))
               .FieldValue(GetFilterValue(reportFliterItem))
               .FieldDataType(reportFliterItem.Get<string>("DataType"))
               .Build();
        }

        private object GetFilterValue(ExpandoObject reportFliterItem)
        {
            if (string.IsNullOrEmpty(reportFliterItem.Get<string>("PropertyName"))
                || string.IsNullOrEmpty(reportFliterItem.Get<string>("Value"))
                || string.IsNullOrEmpty(reportFliterItem.Get<string>("EntityName"))
                || string.IsNullOrEmpty(reportFliterItem.Get<string>("SearchBy")))
                return reportFliterItem.Get<string>("Value");

            return entityService.GetIdentity(
                reportFliterItem.Get<string>("Value"),
                 reportFliterItem.Get<string>("EntityName"),
                 reportFliterItem.Get<string>("SearchBy")
             );
        }

    }
}
