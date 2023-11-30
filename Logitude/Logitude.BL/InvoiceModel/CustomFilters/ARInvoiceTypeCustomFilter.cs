using Logitude.BL.Security;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System.Linq;

namespace Logitude.BL.InvoiceModel.CustomFilters
{
    public class ARInvoiceTypeCustomFilter
    {
        public int Tenant { get; set; }
        public ARInvoiceTypeCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public IQueryable<ARInvoiceType> GetFilteredQuery(QueryOperations operations, IQueryable<ARInvoiceType> queryableData)
        {
            InvoiceCustomFilter customFilters = new InvoiceCustomFilter(Tenant);
            bool IsInterestFeatureVlid= SecurityUtility.CheckFeature("InterestReport", "Module", this.Tenant);
            if (!IsInterestFeatureVlid)
            {
                queryableData=queryableData.Where(s => s.Code != "IT");
            }
            
            return queryableData;
        }
    }
}
