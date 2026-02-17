using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.BL.InvoiceModel.CustomFilters
{
    public class TransferHeaderCustomFilter
    {
        public int Tenant { get; set; }
        public TransferHeaderCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public IQueryable<AccountingTransferHeader> GetFilteredQuery(QueryOperations operations, IQueryable<AccountingTransferHeader> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "ARInvoiceTransferHistory")
                    {
                        queryableData = queryableData.Where(d => d.AccountingTransferTypeCode == "ARIN");
                    }

                    else if (item.FieldName == "APInvoiceTransferHistory")
                    {
                        queryableData = queryableData.Where(d => d.AccountingTransferTypeCode == "APIN");
                    }

                    else if (item.FieldName == "ARPaymentTransferHistory")
                    {
                        queryableData = queryableData.Where(d => d.AccountingTransferTypeCode == "ARPA");
                    }

                    else if (item.FieldName == "APPaymentTransferHistory")
                    {
                        queryableData = queryableData.Where(d => d.AccountingTransferTypeCode == "APPA");
                    }
                }
            }

            return queryableData;
        }

    }
}
