using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Customs.BL.AzureSearch
{
    public class JournalAzureSearchService : FastSearchService
    {
       

        protected override string ManipulateFilters(List<QueryFilterItem> additionalFilters, string filters, int tenant)
        {
            if (string.IsNullOrEmpty(filters))
                filters = $"tenant eq {tenant}";
            else if (!filters.Contains("tenant eq"))
                filters += $" and tenant eq {tenant}";

            return filters;
        }
        
    }
}