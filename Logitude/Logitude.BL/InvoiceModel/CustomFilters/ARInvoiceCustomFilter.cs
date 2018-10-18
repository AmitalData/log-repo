using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.CustomFilters
{
    public class ARInvoiceCustomFilter
    {
        public int Tenant { get; set; }
        public ARInvoiceCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public IQueryable<ARInvoice> GetFilteredQuery(QueryOperations operations, IQueryable<ARInvoice> queryableData)
        {
            InvoiceCustomFilter customFilters = new InvoiceCustomFilter(Tenant);
            IQueryable<ARInvoice> iQueryable = customFilters.GetFilteredQuery(operations, queryableData);
            return iQueryable;
        }
    }
}
