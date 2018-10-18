using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.CustomFilters
{
     public class ClientCustomFilters
    {

         public IQueryable<Client> GetFilteredQuery(QueryOperations operations, IQueryable<Client> queryableData)
         {
             List<QueryFilterItem> queryFilters = operations.QueryFilterItems;



             foreach (QueryFilterItem item in queryFilters)
             {
                 if (item.FieldName == "PassportNumber")
                 {
                     queryableData = queryableData.Where(d => (d.PassportNumber != null));

                 }

                 if (item.FieldName == "Code")
                 {
                     queryableData = queryableData.Where(d => (d.Code == null || d.Code == "Empty" || d.Code != null));

                 }

             }

             return queryableData;


         }
    }
}
