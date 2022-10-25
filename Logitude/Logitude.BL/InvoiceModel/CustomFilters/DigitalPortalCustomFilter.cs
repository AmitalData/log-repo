using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System.Linq;

namespace Logitude.BL.InvoiceModel.CustomFilters
{
    public class DigitalPortalCustomFilter
    {
        public static IQueryable<ARInvoice> ApplyDigitalPortalSearchFilter(QueryFilterItem item, IQueryable<ARInvoice> queryableData)
        {
            string digitalPortalSearchFields = item.FieldValue as string;
            digitalPortalSearchFields = digitalPortalSearchFields.ToLower().Trim();
            queryableData = queryableData.Where(d =>
             d.MainEntityReference.StartsWith(digitalPortalSearchFields)
              || d.InvoiceNumber.Contains(digitalPortalSearchFields)
              || (d.BillTo != null && d.BillTo.EnglishName.StartsWith(digitalPortalSearchFields))
           );

            return queryableData;
        }

        public static IQueryable<ARInvoice> GetDigtalCustomInvlicesFilteredQuery(QueryFilterItem item, IQueryable<ARInvoice> queryableData)
        {
            var values = item.FieldValue == null ? null : item.FieldValue.ToString().Split(',').ToList();

            var searchItems = values.Select(a => $"PaidStatus:{a}").ToList();

            if (values != null && values.Any())
            {
                queryableData = queryableData.Where(d => searchItems.Any(a => a.Contains("PaidStatus:" + d.PaidStatus)));
            }

            return queryableData;
        }

    }
}
