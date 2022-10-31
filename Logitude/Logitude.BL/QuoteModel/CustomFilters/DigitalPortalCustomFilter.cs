using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Linq;

namespace Logitude.BL.QuoteModel.CustomFilters
{
    public class DigitalPortalCustomFilter
    {
        public static IQueryable<Quote> ApplyDigitalPortalSearchFilter(QueryFilterItem item, IQueryable<Quote> queryableData)
        {
            string digitalPortalSearchFields = item.FieldValue as string;
            digitalPortalSearchFields = digitalPortalSearchFields.ToLower().Trim();
            queryableData = queryableData.Where(d =>
                  d.QuoteNumber.Contains(digitalPortalSearchFields) ||
                  d.QuoteNumber.Contains(digitalPortalSearchFields)
           );

            return queryableData;
        }
    }
}
