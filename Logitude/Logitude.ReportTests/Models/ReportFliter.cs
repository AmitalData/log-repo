using System;
using System.Collections.Generic;

namespace Logitude.ReportTests.Models
{
    public class ReportFliter
    {
        public string DefaultTemplateId { get; set; }
        public int DefaultTemplateVsersion { get; set; }
        public int NumberOfPage { get; set; }
        public string ReportCode { get; set; }
        public string ReportId { get; set; }
        public string ReportName { get; set; }
        public bool ReportsRunUsingWR { get; set; }
        public int Tenant { get; set; }
        public string UserId { get; set; }
        public string ProcessType { get; set; }
        public string ReportKey { get; set; }
        public List<ReportFliterItem> QueryFilterItemLists { get; set; }

    }

    public class ReportFliterItem : ReportFliterItemEntity
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }


    public class ReportFliterItemEntity
    {
        public ReportFliterItemEntity(string map, string entityName, string searchKeyName, string searchKeyValue)
        {
            Map = map;
            EntityName = entityName;
            SearchKeyName = searchKeyName;
            SearchKeyValue = searchKeyValue;
        }

        public ReportFliterItemEntity()
        {
        }

        public string Map { get; set; }
        public string EntityName { get; set; }
        public string SearchKeyName { get; set; }
        public string SearchKeyValue { get; set; }
    }
}
