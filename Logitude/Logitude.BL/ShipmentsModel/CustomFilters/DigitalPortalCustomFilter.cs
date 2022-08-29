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
                    if (item.FieldName == "InTransit")
                    {
                        queryableData = DigitalPortalCustomFilter.ApplyInTransitFilter(item, queryableData, shipmentRepository);
                    }

                    if (item.FieldName == "DigitalPortalSearchFields")
                    {
                        queryableData = DigitalPortalCustomFilter.ApplyDigitalPortalSearchFilter(item, queryableData);
                    }

                    if (item.FieldName == "ConsigneeShipperIds")
                    {
                        queryableData = DigitalPortalCustomFilter.ApplyShipperConsigneeFilter(item, queryableData);
                    }

                    if (item.FieldName == "DigitalQuickSearch")
                    {
                        queryableData = DigitalCustomFilter.ApplyDigitalQuickSearchFilter(item, queryableData);
                    }

                    if (item.FieldName == "ActualDataDateYearMonth")
                    {
                        int year = Convert.ToInt32(item.FieldValue);
                        int month = Convert.ToInt32(item.FieldValue2);
                        if (year != 0)
                        {
                            queryableData = queryableData.Where(d => d.CreateDateTime.Year == year);
                        }
                        if (month != 0)
                        {
                            queryableData = queryableData.Where(d => d.CreateDateTime.Month == month);
                        }
                    }

                    if (item.FieldName == "MasterConnectedHouses")
                    {
                        isMasterConnectedHouses = true;
                    }

                    if (item.FieldName == "IsCancelled")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        if (value)
                        {
                            showIsCancelled = true;
                        }
                    }

                    if (item.FieldName == "IsStandalonePickupDelivery")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        if (value)
                        {
                            showIsStandalonePickupDelivery = true;
                        }
                    }

                    if (item.FieldName == "Client")
                    {
                        string value = item.FieldValue as string;
                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(d => d.ShipperName.ToUpper().StartsWith(value.ToUpper()));
                        }
                    }

                    if (item.FieldName == "OpenShipments" || item.FieldName == "OpenMasters")
                    {
                        queryableData = queryableData.Where(d => d.IsOperationalClosed == false);
                    }

                    if (item.FieldName == "ClosedShipments")
                    {
                        queryableData = queryableData.Where(d => d.IsOperationalClosed == true);
                    }

                    if (item.FieldName == "AllShipments")
                    {
                        isAllShipments = true;
                        queryableData = queryableData.Where(d => d.ShipmentLevelCode != "C" && d.IsCancelled == false);
                    }

                    if (item.FieldName == "AirlinesUpdates")
                    {
                        DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(_tenant).Date;
                        DateTime lastWeekDate = todayDate.AddDays(-7);

                        queryableData = queryableData.Where(d => d.ShipmentLevelCode != "H"
                                                                 && d.TransportModeId == "A"
                                                                 && d.CarrierLastStatusDate >= lastWeekDate
                                                                 && d.IsCancelled == false);
                    }

                    if (item.FieldName == "ActualDataDate")
                    {
                        if (item.FieldValue != null)
                        {
                            DateTime myDateTime = Convert.ToDateTime(item.FieldValue);//(DateTime)item.FieldValue;
                            if (myDateTime != null)
                            {
                                queryableData = queryableData.Where(d => d.CreateDateTime.Month == myDateTime.Month && d.CreateDateTime.Year == myDateTime.Year);
                            }
                        }
                    }

                    if (item.FieldName == "InvoiceNumber")
                    {
                        string value = Convert.ToString(item.FieldValue);
                        if (value != null)
                        {
                            ARInvoiceEntityRepository invoiceRep = new ARInvoiceEntityRepository(_tenant);
                            DigitalShipmentsDataView dataview = queryableData.FirstOrDefault();
                            List<ARInvoiceEntity> invoiceEntities = invoiceRep.GetInvoiceEntities(dataview.Tenant).Where(d => d.ARInvoice.InvoiceNumber.StartsWith(value)).ToList();
                            List<string> shipmentIds = (from a in invoiceEntities select a.EntityId).ToList();

                            queryableData = queryableData.Where(d => shipmentIds.Contains(d.Id));
                        }
                    }


                    if (item.FieldName == "LastMonthShipments")
                    {
                        if (item.FieldValue != null)
                        {
                            bool? value = item.FieldValue as bool?;
                            if (value == true)
                            {
                                DateTime? currentDateTime = TenantServerConfigration.GetCurrentDateTime(_tenant).Date;
                                int year = currentDateTime.Value.Year;
                                int month = currentDateTime.Value.Month;
                                if (month == 1)
                                {
                                    year--;
                                    month = 12;
                                }

                                else
                                {
                                    month--;
                                }

                                queryableData = queryableData.Where(d => d.CreateDateTime.Month == month && d.CreateDateTime.Year == year);
                            }
                        }
                    }

                    if (item.FieldName == "DailySpotlightFilter")
                    {
                        if (item.FieldValue != null)
                        {
                            string code = item.FieldValue.ToString();

                            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(_tenant).Date;
                            DateTime date1 = todayDate;
                            DateTime date2 = todayDate;

                            switch (code)
                            {
                                case "SH_TD":
                                    {
                                        date1 = todayDate.AddDays(0);
                                        date2 = todayDate.AddDays(0);
                                        break;
                                    }

                                case "SH_YS":
                                    {
                                        date1 = todayDate.AddDays(-1);
                                        date2 = todayDate.AddDays(-1);
                                        break;
                                    }

                                case "SH_LW":
                                    {
                                        date1 = todayDate.AddDays(-7);
                                        date2 = todayDate.AddDays(-1);
                                        break;
                                    }
                            }

                            queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) <= date2);
                        }
                    }

                    if (item.FieldName == "ViaPortId")
                    {
                        string value = item.FieldValue as string;

                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(d =>
                            (!string.IsNullOrEmpty(d.Transshipment1FromPortId) && d.Transshipment1FromPortId == value)
                            || (!string.IsNullOrEmpty(d.Transshipment2FromPortId) && d.Transshipment2FromPortId == value)
                            || (!string.IsNullOrEmpty(d.Transshipment3FromPortId) && d.Transshipment3FromPortId == value)
                            );
                        }
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
                queryableData = queryableData.Where(d => values.Contains(d.ShipperId) || values.Contains(d.ConsigneeId));
            }

            return queryableData;
        }

        public static IQueryable<DigitalShipmentsDataView> ApplyDigitalPortalSearchFilter(QueryFilterItem item, IQueryable<DigitalShipmentsDataView> queryableData)
        {
            string digitalPortalSearchFields = item.FieldValue as string;
            digitalPortalSearchFields = digitalPortalSearchFields.ToLower().Trim();
            queryableData = queryableData.Where(d =>
             d.ShipperReference1.Contains(digitalPortalSearchFields)
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
              || d.AgentName.ToLower().StartsWith(digitalPortalSearchFields)
              || d.CustomAgentImportName.ToLower().StartsWith(digitalPortalSearchFields)
              || d.ConsigneeName.ToLower().StartsWith(digitalPortalSearchFields)
              || d.ShipperName.ToLower().StartsWith(digitalPortalSearchFields)
              || d.CustomerName.ToLower().StartsWith(digitalPortalSearchFields)
              || d.FromPortName.ToLower().StartsWith(digitalPortalSearchFields)
              || d.ToPortName.ToLower().StartsWith(digitalPortalSearchFields)
              || d.MainCarriageCarrierName.ToLower().StartsWith(digitalPortalSearchFields)
           );

            return queryableData;
        }

        public static IQueryable<DigitalShipmentsDataView> ApplyInTransitFilter(QueryFilterItem item, IQueryable<DigitalShipmentsDataView> queryableData, Simplog.Data.ShipmentsModel.Repositories.ShipmentRepository shipmentRepository)
        {
            queryableData = (from shipment in queryableData
                                                      where (shipment.MainCarriageATD != null || shipment.Transshipment1ATD != null || shipment.Transshipment2ATD != null || shipment.Transshipment3ATD != null)
                                                      && (shipment.Transshipment3ToPortId == null || shipment.Transshipment3ATA == null)
                                                      && (shipment.Transshipment2ToPortId == null || shipment.Transshipment2ATA == null)
                                                      && (shipment.Transshipment1ToPortId == null || shipment.Transshipment1ATA == null)
                                                      && (shipment.MainCarriageToPortId == null || shipment.MainCarriageATA == null)
                                                      && !(shipmentRepository.context.ShipmentPickUpDeliveries.Any(delivery => delivery.ShipmentId == shipment.Id && delivery.PickUpDeliveryTypeCode == "DELV" && delivery.ATA != null))
                                                      select shipment);

            return queryableData;
        }
    }
}