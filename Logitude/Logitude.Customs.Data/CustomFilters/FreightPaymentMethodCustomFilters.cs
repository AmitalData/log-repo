using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.CustomFilters
{
    public class FreightPaymentMethodCustomFilters
    {
        public IQueryable<FreightPaymentMethod> GetFilteredQuery(QueryOperations operations, IQueryable<FreightPaymentMethod> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;



            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.FieldName == "PaymentMethodCode")
                {
                    List<string> values = new List<string>() { "CC","CA","NC","PO","PP" };
                    //queryableData = queryableData.Where(d => (d.HatraDate == null));
                    queryableData = queryableData.Where(d => values.Contains(d.Code));

                }
               
            }

            return queryableData;


        }
    }
}
