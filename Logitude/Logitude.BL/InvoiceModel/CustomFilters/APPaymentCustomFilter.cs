using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InvoiceModel.EntityPOCOs;

using Logitude.BL.DataContracts;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.BL.InvoiceModel.CustomFilters
{
    public class APPaymentCustomFilter
    {
        public int Tenant { get; set; }
        public APPaymentCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public IQueryable<APPayment> GetFilteredQuery(QueryOperations operations, IQueryable<APPayment> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "DraftPayments")
                    {
                        queryableData = queryableData.Where(d => d.StatusCode == "DR");
                    }

                    else if (item.FieldName == "OpenPayments")
                    {
                        queryableData = queryableData.Where(d => d.StatusCode != "DR" && d.StatusCode != "VD" && d.IsClosed == false);
                    }

                    else if (item.FieldName == "NotReadyPayments")
                    {
                        queryableData = queryableData.Where(d => d.StatusCode != "DR" && d.StatusCode != "VD" && d.TransferStatusCode == "NR");
                    }

                    else if (item.FieldName == "MarkedAsBlockedForTransfer")
                    {
                        queryableData = queryableData.Where(d => d.StatusCode != "DR" && d.StatusCode != "VD" && d.TransferStatusCode == "BL");
                    }

                    else if (item.FieldName == "SearchReadyPayments")
                    {
                        string searchText = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(searchText))
                        {
                            queryableData =
                                (from r in queryableData
                                 where
                                 !string.IsNullOrEmpty(r.PaymentNo) && r.PaymentNo.ToUpper().Contains(searchText.ToUpper())
                                 ||
                                 r.VendorCard != null && r.VendorCard.EnglishName.ToUpper().Contains(searchText.ToUpper())
                                 select r);
                        }
                    }
                }
            }

            return queryableData;
        }

    }
}
