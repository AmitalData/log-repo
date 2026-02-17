using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.Data.CustomFilters
{
    class TicketClassificationCustomFilter
    {
        public static IQueryable<TicketClassification> GetFilteredQuery(QueryOperations operations, IQueryable<TicketClassification> queryableData, int tenant)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "ParentId")
                    {

                        string value = Convert.ToString(item.FieldValue);
                        if (!string.IsNullOrEmpty(value))
                        {
                            string myCheck = value.Split(';')[1];
                            string myId = value.Split(';')[0];

                            if (myCheck == "F")
                            {
                                queryableData = queryableData.Where(d => d.ParentId.StartsWith(value));
                            }

                            else if(myCheck == "S")
                            {
                                queryableData = queryableData.Where(d => d.ParentId == myId);
                            }
                        }
                    }
                }
            }

            return queryableData;
        }
    }
}
