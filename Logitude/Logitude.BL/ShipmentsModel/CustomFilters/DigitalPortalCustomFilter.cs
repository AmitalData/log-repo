using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Logitude.BL.ShipmentsModel.CustomFilters
{
    public static class DigitalPortalCustomFilter
    {
        public static IQueryable<DigitalShipmentsDataView> GetDigtalFilteredQuery(QueryOperations operations, IQueryable<DigitalShipmentsDataView> queryableData, ShipmentRepository shipmentRepository = null, int _tenant = 0)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            bool showIsCancelled = false;
            bool showIsStandalonePickupDelivery = false;
            bool isMasterConnectedHouses = false;
            bool isAllShipments = false;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "DigitalPortalSearchFields")
                    {
                        queryableData = ApplyDigitalPortalSearchFilter(item, queryableData);
                    }

                    if (item.FieldName == "ConsigneeShipperIds")
                    {
                        queryableData = ApplyShipperConsigneeFilter(item, queryableData);
                    }

                    if (item.FieldName == "AllShipments")
                    {
                        isAllShipments = true;
                        queryableData = queryableData.Where(d => d.ShipmentLevelCode != "C" && d.IsCancelled == false);
                    }

                    if (item.FieldName == "TransportModeShipmentTypeFilters")
                    {
                        var transportModesString = item.FieldValue as string;
                        var shipmentTypesString = item.FieldValue2 as string;

                        List<string> transportModes = transportModesString.Split(',').ToList();
                        List<string> shipmentTypes = shipmentTypesString.Split(',').ToList();

                        var values = new List<string>();

                        foreach (var tm in transportModes)
                        {
                            if (tm.Equals("a", StringComparison.InvariantCultureIgnoreCase))
                            {
                                values.Add($"A:Air");
                            }
                            else if (tm.Equals("o", StringComparison.InvariantCultureIgnoreCase))
                            {
                                var oceanCodes = new List<string> { "FCLD", "LCLD", "MyGO" };
                                var orderedOccen = shipmentTypes.Where(a => oceanCodes
                                                                            .Contains(a, StringComparer.InvariantCultureIgnoreCase))
                                                                .ToList();
                                orderedOccen.ForEach(a => values.Add($"O:{a}"));
                            }
                            else if (tm.Equals("i", StringComparison.InvariantCultureIgnoreCase))
                            {
                                var inlandCodes = new List<string> { "FTL", "LTL", "MyGI" };
                                var orderedInlnad = shipmentTypes.Where(a => inlandCodes
                                                                             .Contains(a, StringComparer.InvariantCultureIgnoreCase))
                                                                 .ToList();
                                orderedInlnad.ForEach(a => values.Add($"I:{a}"));
                            }
                        }

                        queryableData = queryableData.Where(a => values.Contains(a.TransportModeId + ":" + a.ShipmentTypeId));
                    }
                }
            }

            if (isMasterConnectedHouses || isAllShipments)
            {
                return queryableData;
            }
            else
            {
                queryableData = queryableData.Where(d => d.IsCancelled == showIsCancelled && d.IsStandalonePickupDelivery == showIsStandalonePickupDelivery);

                return queryableData;
            }
        }

        public static IQueryable<DigitalShipmentsDataView> ApplyShipperConsigneeFilter(QueryFilterItem item, IQueryable<DigitalShipmentsDataView> queryableData)
        {
            string[] values = item.FieldValue == null ? null : item.FieldValue.ToString().Split(',');
            
            if (values != null && values.Length > 0)
            {
                queryableData = queryableData.Where(d => values.Contains(d.ShipperId) 
                                                         || values.Contains(d.ConsigneeId));
            }

            return queryableData;
        }

        public static IQueryable<DigitalShipmentsDataView> ApplyDigitalPortalSearchFilter(QueryFilterItem item, IQueryable<DigitalShipmentsDataView> queryableData)
        {
            string digitalPortalSearchFields = item.FieldValue as string;
            digitalPortalSearchFields = digitalPortalSearchFields.ToLower().Trim();
            queryableData = queryableData.Where(d => d.ShipperReference1.Contains(digitalPortalSearchFields)
                                                      || d.ShipperReference2.Contains(digitalPortalSearchFields)
                                                      || d.ConsigneeReference1.Contains(digitalPortalSearchFields)
                                                      || d.ConsigneeReference2.Contains(digitalPortalSearchFields)
                                                      || d.CustomerReference1.Contains(digitalPortalSearchFields)
                                                      || d.CustomerReference2.Contains(digitalPortalSearchFields)
                                                      || d.CustomerReference2.Contains(digitalPortalSearchFields)
                                                      || d.ShipmentNumber.Contains(digitalPortalSearchFields)
                                                      || d.MainCarriageCarrierNumber.Contains(digitalPortalSearchFields)
                                                      || d.House.Contains(digitalPortalSearchFields)
                                                      || d.Master.Contains(digitalPortalSearchFields)
                                                      || d.FromPortName.StartsWith(digitalPortalSearchFields)
                                                      || d.ToPortName.StartsWith(digitalPortalSearchFields)
                                                      || d.MainCarriageCarrierName.StartsWith(digitalPortalSearchFields)
           );

            return queryableData;
        }
    }
}