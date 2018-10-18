using Logitude.WarehouseLib.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.WarehouseLib.Data.CustomFilters
{
    public class WarehouseEntryCustomFilter
    {
        public static IQueryable<WarehouseEntry> GetFilteredQuery(QueryOperations operations, IQueryable<WarehouseEntry> queryableData, int tenant)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;
            
            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    switch (item.FieldName)
                    {
                        case "CreatedEntries":
                            {
                                queryableData = queryableData.Where(d => d.StatusCode == "CREA");
                                break;
                            }

                        case "EnterredEntries":
                            {
                                queryableData = queryableData.Where(d => d.StatusCode == "ENTE");
                                break;
                            }

                        case "ConnectedToShipments":
                            {
                                string fieldValue = "";
                                if (item.FieldValue != null)
                                {
                                    fieldValue = item.FieldValue.ToString().ToLower();
                                    if (fieldValue == "true")
                                    {
                                        queryableData = queryableData.Where(d => !string.IsNullOrEmpty(d.ShipmentId));
                                    }
                                    else if (fieldValue == "false")
                                    {
                                        queryableData = queryableData.Where(d => string.IsNullOrEmpty(d.ShipmentId));
                                    }
                                }

                                break;
                            }
                    }
                }
            }
            
            return queryableData;
        }
    }
}
