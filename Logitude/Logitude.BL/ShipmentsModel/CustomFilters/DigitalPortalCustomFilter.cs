using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
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
        public static IQueryable<DigitalShipmentsDataView> GetDigtalFilteredQuery(QueryOperations operations, 
                                                                                  IQueryable<DigitalShipmentsDataView> queryableData,
                                                                                  ShipmentRepository shipmentRepository = null,
                                                                                  int _tenant = 0)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            bool showIsCancelled = false;
            bool showIsStandalonePickupDelivery = false;
            bool isMasterConnectedHouses = false;
            bool isAllShipments = false;

            var tenantquery = new TenantManagementQuery(_tenant);

            var data = tenantquery.GetSinglePM(_tenant);

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    DateTime currentDateTime = TenantServerConfigration.GetCurrentDateTime(_tenant).Date;
                    var createdDateTime = currentDateTime.AddMonths(-(data.DPArchiveShipmentCreateFilter.HasValue ? data.DPArchiveShipmentCreateFilter.Value : 12));
                    var arrivalDateTime = currentDateTime.AddMonths(-(data.DPArchiveShipmentArrivalFilter.HasValue ? data.DPArchiveShipmentArrivalFilter.Value : 3));
                    var departureDateTime = currentDateTime.AddMonths(-(data.DPArchiveShipmentDepartFilter.HasValue ? data.DPArchiveShipmentDepartFilter.Value : 3));

                    if (item.FieldName == "InProgress")
                    {
                        queryableData = queryableData.Where(a => a.IsCustomerArchived == false);
                        queryableData = ApplyArchivingFilter(queryableData, createdDateTime, arrivalDateTime, departureDateTime);
                    }                                            

                    if (item.FieldName == "InOrigin")
                    {
                        var allStatuses = GetAllDigitalAllowedStatus(_tenant);
                        var allowedStatusCode = allStatuses.Select(a => a.Code).ToList();
                        var departedCodeWeight = allStatuses.FirstOrDefault(a => a.Code == "SDEP")?.StatusWeight;
                        queryableData = queryableData.Where(a => allowedStatusCode.Contains(a.StatusCode)
                                                                 && a.StatusWeight < departedCodeWeight
                                                                 && a.IsCustomerArchived == false);
                        queryableData = ApplyArchivingFilter(queryableData, createdDateTime, arrivalDateTime, departureDateTime);
                    }

                    if (item.FieldName == "InTransit")
                    {
                        var allStatuses = GetAllDigitalAllowedStatus(_tenant);
                        var allowedStatusCode = allStatuses.Select(a => a.Code).ToList();
                        var departedCodeWeight = allStatuses.FirstOrDefault(a => a.Code == "SDEP")?.StatusWeight;
                        var arrivedAtDestinationCodeWeight = allStatuses.FirstOrDefault(a => a.Code == "SARR")?.StatusWeight;

                        queryableData = queryableData.Where(a => allowedStatusCode.Contains(a.StatusCode) 
                                                                 && a.StatusWeight >= departedCodeWeight
                                                                 && a.StatusWeight < arrivedAtDestinationCodeWeight
                                                                 && a.IsCustomerArchived == false);
                        queryableData = ApplyArchivingFilter(queryableData, createdDateTime, arrivalDateTime, departureDateTime);
                    }

                    if (item.FieldName == "AtDestination")
                    {
                        var allStatuses = GetAllDigitalAllowedStatus(_tenant); 
                        var allowedStatusCode = allStatuses.Select(a => a.Code).ToList();
                        var arrivedAtDestinationCodeWeight = allStatuses.FirstOrDefault(a => a.Code == "SARR")?.StatusWeight;
                        queryableData = queryableData.Where(a => allowedStatusCode.Contains(a.StatusCode) 
                                                                 && a.StatusWeight >= arrivedAtDestinationCodeWeight
                                                                 && a.IsCustomerArchived == false);
                        queryableData = ApplyArchivingFilter(queryableData, createdDateTime, arrivalDateTime, departureDateTime);
                    }

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
                        queryableData = queryableData.Where(a => a.ShipmentLevelCode != "C"
                                                                 && a.IsCancelled == false);
                    }

                    if (item.FieldName == "IsCustomerArchived")
                    {
                        queryableData = queryableData.Where(d => !d.IsCustomerArchived);
                    }

                    if (item.FieldName == "TransportModeShipmentTypeFilters")
                    {
                        var transportModesString = item.FieldValue as string;
                        var shipmentTypesString = item.FieldValue2 as string;
                        var shipmentSubTypesString = item.FieldValue3 as string;

                        List<string> transportModes = !string.IsNullOrWhiteSpace(transportModesString)
                                                      ? transportModesString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                                            .ToList()
                                                      : new List<string>();

                        List<string> shipmentTypes = !string.IsNullOrWhiteSpace(shipmentTypesString) 
                                                     ? shipmentTypesString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                                          .ToList()
                                                     : new List<string>();

                        List<string> shipmentSubTypes = !string.IsNullOrWhiteSpace(shipmentSubTypesString)
                                                        ? shipmentSubTypesString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                                                .ToList()
                                                        : new List<string>();
                        var values = new List<string>();
                        var subTypesList = new List<ShipmentSubType>();
                        if (shipmentSubTypes.Any())
                        {
                            subTypesList = GetShipmentTypes(_tenant);
                        }

                        foreach (var tm in transportModes)
                        {
                            if (tm.Equals("a", StringComparison.InvariantCultureIgnoreCase))
                            {
                                var airCodes = new List<string> { "Air" };

                                if (shipmentSubTypes.Any())
                                {
                                    var airSubTypesIds = subTypesList.Where(a => airCodes.Contains(a.ShipmentTypeCode, StringComparer.InvariantCultureIgnoreCase))
                                                                     .Select(a => a.Id)
                                                                     .ToList();

                                    var airSubTypes = shipmentSubTypes.Where(a => airSubTypesIds.Contains(a, StringComparer.InvariantCultureIgnoreCase))
                                                                      .ToList();
                                    if (airSubTypes.Any())
                                    {
                                        airSubTypes.ForEach(a => values.Add($"A:Air:{a}"));
                                    }
                                    else
                                    {
                                        values.Add($"A:Air");
                                    }
                                }
                                else
                                {
                                    values.Add($"A:Air");
                                }
                            }
                            else if (tm.Equals("o", StringComparison.InvariantCultureIgnoreCase))
                            {
                                var oceanCodes = new List<string> { "FCL", "FCLD", "LCL", "LCLD", "MyGO" };
                                var orderedOccen = shipmentTypes.Where(a => oceanCodes.Contains(a, StringComparer.InvariantCultureIgnoreCase))
                                                                .ToList();

                                if (shipmentSubTypes.Any())
                                {
                                    var oceanSubTypesIds = subTypesList.Where(a => oceanCodes.Contains(a.ShipmentTypeCode, StringComparer.InvariantCultureIgnoreCase))
                                                                       .Select(a => a.Id)
                                                                       .ToList();

                                    var oceanSubTypes = shipmentSubTypes.Where(a => oceanSubTypesIds.Contains(a, StringComparer.InvariantCultureIgnoreCase))
                                                                        .ToList();

                                    if (oceanSubTypes.Any())
                                    {
                                        foreach (var oceanSubType in oceanSubTypes)
                                        {
                                            orderedOccen.ForEach(a => values.Add($"O:{a}:{oceanSubType}"));
                                        }
                                    }
                                    else
                                    {
                                        orderedOccen.ForEach(a => values.Add($"O:{a}"));
                                    }
                                }
                                else
                                {
                                    if (orderedOccen.Any())
                                    {
                                        orderedOccen.ForEach(a => values.Add($"O:{a}"));
                                    }
                                    else
                                    {
                                        oceanCodes.ForEach(a => values.Add($"O:{a}"));
                                    }
                                }
                            }
                            else if (tm.Equals("i", StringComparison.InvariantCultureIgnoreCase))
                            {
                                var inlandCodes = new List<string> { "FTL", "LTL", "MyGI" };
                                var orderedInlnad = shipmentTypes.Where(a => inlandCodes.Contains(a, StringComparer.InvariantCultureIgnoreCase))
                                                                 .ToList();

                                if (shipmentSubTypes.Any())
                                {
                                    var inlandSubTypesIds = subTypesList.Where(a => inlandCodes.Contains(a.ShipmentTypeCode, StringComparer.InvariantCultureIgnoreCase))
                                                                        .Select(a => a.Id)
                                                                        .ToList();

                                    var inlandSubTypes = shipmentSubTypes.Where(a => inlandSubTypesIds.Contains(a, StringComparer.InvariantCultureIgnoreCase))
                                                                         .ToList();
                                    if (inlandSubTypes.Any())
                                    {
                                        foreach (var inlandSubtype in inlandSubTypes)
                                        {
                                            orderedInlnad.ForEach(a => values.Add($"I:{a}:{inlandSubtype}"));
                                        }
                                    }
                                    else
                                    {
                                        orderedInlnad.ForEach(a => values.Add($"I:{a}"));
                                    }

                                }
                                else
                                {
                                    if (orderedInlnad.Any())
                                    {
                                        orderedInlnad.ForEach(a => values.Add($"I:{a}"));
                                    }
                                    else
                                    {
                                        inlandCodes.ForEach(a => values.Add($"I:{a}"));
                                    }
                                }
                            }
                        }

                        queryableData = queryableData.Where(a => values.Any(v => (a.TransportModeId  + ":" + a.ShipmentTypeId + ":" + a.ShipmentSubTypeId).Contains(v)));
                    }
                }
            }

            if (isMasterConnectedHouses || isAllShipments)
            {
                return queryableData;
            }
            else
            {
                queryableData = queryableData.Where(d => d.IsCancelled == showIsCancelled 
                                                         && d.IsStandalonePickupDelivery == showIsStandalonePickupDelivery);

                return queryableData;
            }
        }

        public static IQueryable<DigitalShipmentsDataView> ApplyArchivingFilter(IQueryable<DigitalShipmentsDataView> shipmentDataViews, DateTime createdDateTime, DateTime arrivalDateTime, DateTime departureDateTime)
        {
            return shipmentDataViews.Where(a => !(System.Data.Entity.DbFunctions.TruncateTime(a.CreateDateTime) <= createdDateTime
                                                  || ((a.MainCarriageFinalDestinationATA.HasValue && System.Data.Entity.DbFunctions.TruncateTime(a.MainCarriageFinalDestinationATA) <= arrivalDateTime) && (a.DirectionId == "I" || a.DirectionId == "R" || a.DirectionId == "D"))
                                                  || ((a.MainCarriageATD.HasValue && System.Data.Entity.DbFunctions.TruncateTime(a.MainCarriageATD) <= departureDateTime) && a.DirectionId == "E")));
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

            if (digitalPortalSearchFields.Length < 3)
            {
                return queryableData;
            }

            queryableData = queryableData.Where(d => d.ShipperReference1.StartsWith(digitalPortalSearchFields)
                                                      || d.ShipperReference2.StartsWith(digitalPortalSearchFields)
                                                      || d.ConsigneeReference1.StartsWith(digitalPortalSearchFields)
                                                      || d.ConsigneeReference2.StartsWith(digitalPortalSearchFields)
                                                      || d.CustomerReference1.StartsWith(digitalPortalSearchFields)
                                                      || d.CustomerReference2.StartsWith(digitalPortalSearchFields)
                                                      || d.CustomerReference2.StartsWith(digitalPortalSearchFields)
                                                      || d.ShipmentNumber.Contains(digitalPortalSearchFields)
                                                      || d.MainCarriageCarrierNumber.StartsWith(digitalPortalSearchFields)
                                                      || d.House.StartsWith(digitalPortalSearchFields)
                                                      || d.Master.StartsWith(digitalPortalSearchFields)
                                                      || d.FromPortName.StartsWith(digitalPortalSearchFields)
                                                      || d.ToPortName.StartsWith(digitalPortalSearchFields)
                                                      || d.MainCarriageCarrierName.StartsWith(digitalPortalSearchFields));

            return queryableData;
        }

        private static List<ShipmentSubType> GetShipmentTypes(int tenant)
        {
            IShipmentsContext context = ShipmentsContext.GetContext(tenant);
            ShipmentSubTypeRepository shipmentSubTypeRepository = new ShipmentSubTypeRepository(context);
            IQueryable<ShipmentSubType> shipmentSubTypes = shipmentSubTypeRepository.GetShipmentSubTypes(tenant);
            return shipmentSubTypes?.ToList();
        }

        private static List<EntityStatusList> GetAllDigitalAllowedStatus(int tenant)
        {
            var entityStatusQuery = new EntityStatusQuery(tenant);
            var allStatuses = entityStatusQuery.GetDigitalPortalActiveStatuses(tenant)
                                               .ToList();
            return allStatuses;
        }
    }
}