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
    }
}
