using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.CustomFilters
{
    public class CustomsTransferHeaderCustomFilter
    {
        public int Tenant { get; set; }
        public CustomsTransferHeaderCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public IQueryable<CustomsTransferHeader> GetFilteredQuery(QueryOperations operations, IQueryable<CustomsTransferHeader> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    //if (item.FieldName == "ARInvoiceTransferHistory")
                    //{
                    //    queryableData = queryableData.Where(d => d.CustomsTransferTypeCode == "ARIN");
                    //}

                    //else if (item.FieldName == "APInvoiceTransferHistory")
                    //{
                    //    queryableData = queryableData.Where(d => d.CustomsTransferTypeCode == "APIN");
                    //}                    
                }
            }

            return queryableData;
        }
    }
}
