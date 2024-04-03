using System;
using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;

using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;

using System.ServiceModel.DomainServices.Server;
using System.IO;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using System.Reflection;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.CustomFilters;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        public List<ShipmentPayablePM> GetInvoiceOpenAmountPayables(string shipmentId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            ShipmentPayableQuery shipmentPayableQuery = new ShipmentPayableQuery(tenant);

            List<ShipmentPayablePM> myResult = shipmentPayableQuery.GetInvoiceOpenAmountPayables(shipmentId, tenant);

            return myResult;
        }

        public List<ShipmentReceivablePM> GetInvoiceOpenAmountReceivables(string invoiceTypeCode, string entityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            List<ShipmentReceivablePM> result = new List<ShipmentReceivablePM>();
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            ShipmentReceivableQuery shipmentReceivableQuery = new ShipmentReceivableQuery(tenant);
            Shipment shipment = shipmentRepository.GetSingleShipment(entityId, tenant);

            if (shipment != null)
            {
                if (invoiceTypeCode == "MN")
                {
                    foreach (ShipmentReceivablePM item in shipmentReceivableQuery.GetShipmentReceivablePMsByShipmentId(entityId, tenant).Where(d => d.ShipmentReceivableLineStatusCode == "OAMT"))
                    {
                        result.Add(item);
                    }

                    List<Shipment> housesShipments = shipmentRepository.GetHouseShipmentsForMaster(entityId, tenant);
                    foreach (Shipment house in housesShipments)
                    {
                        foreach (ShipmentReceivablePM item in shipmentReceivableQuery.GetShipmentReceivablePMsByShipmentId(house.Id, tenant).Where(d => d.ShipmentReceivableLineStatusCode == "OAMT"))
                        {
                            result.Add(item);
                        }
                    }
                }

                else
                {
                    foreach (ShipmentReceivablePM item in shipmentReceivableQuery.GetShipmentReceivablePMsByShipmentId(entityId, tenant).Where(d => d.ShipmentReceivableLineStatusCode == "OAMT"))
                    {
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        public List<ShipmentReceivablePM> GetMasterReceivables(string masterId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Master", "READ", tenant);

            List<ShipmentReceivablePM> result = new List<ShipmentReceivablePM>();
            
            shipmentQuery = new ShipmentQuery(tenant);
            ShipmentPM masterPM = shipmentQuery.GetSinglePM(masterId, tenant);

            foreach (ShipmentReceivablePM item in masterPM.ShipmentReceivables.Where(d => d.ShipmentReceivableLineStatusCode == "OAMT" && d.UnitPrice > 0))
            {
                result.Add(item);
            }

            foreach (ConsoleShipmentPM item in masterPM.ShipmentConsoleShipments)
            {
                ShipmentPM shipmentPM = shipmentQuery.GetSinglePM(item.Id, tenant);
                if (shipmentPM != null)
                {
                    foreach (ShipmentReceivablePM rec in shipmentPM.ShipmentReceivables.Where(d => d.ShipmentReceivableLineStatusCode == "OAMT" && d.UnitPrice > 0))
                    {
                        result.Add(rec);
                    }
                }
            }
            return result;
        }

        public List<ShipmentReceivablePM> GetAllMasterHousesReceivables(List<string> housesIds, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Master", "READ", tenant);

            List<ShipmentReceivablePM> myResult = new List<ShipmentReceivablePM>();
            ShipmentReceivableQuery receivablesQuery = new ShipmentReceivableQuery(tenant);

            foreach (string houseId in housesIds)
            {
                List<ShipmentReceivablePM> houseReceivables = receivablesQuery.GetShipmentReceivablePMsByShipmentId(houseId, tenant);

                foreach (ShipmentReceivablePM item in houseReceivables.Where(d => d.Quantity != null && d.UnitPrice != null))
                {
                    myResult.Add(item);
                }
            }

            return myResult;
        }

        public List<ShipmentPayablePM> GetAllMasterHousesPayables(List<string> housesIds, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("Master", "READ", tenant);

            List<ShipmentPayablePM> myResult = new List<ShipmentPayablePM>();
            ShipmentPayableQuery query = new ShipmentPayableQuery(tenant);

            foreach (string houseId in housesIds)
            {
                List<ShipmentPayablePM> housePayables = query.GetShipmentPayablePMsByShipment(houseId, tenant);

                foreach (ShipmentPayablePM item in housePayables.Where(d => d.Quantity != null && d.UnitPrice != null))
                {
                    myResult.Add(item);
                }
            }

            return myResult;
        }

        public List<ShipmentReceivablePM> GetMasterReceivablesForProfit(string masterId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("Master", "READ", tenant);

            List<ShipmentReceivablePM> result = new List<ShipmentReceivablePM>();
            shipmentQuery = new ShipmentQuery(tenant);
            ShipmentPM masterPM = shipmentQuery.GetSinglePM(masterId, tenant);

            foreach (ShipmentReceivablePM item in masterPM.ShipmentReceivables.Where(d => d.Quantity != null && d.UnitPrice != null))
            {
                result.Add(item);
            }

            foreach (ConsoleShipmentPM item in masterPM.ShipmentConsoleShipments)
            {
                ShipmentPM shipmentPM = shipmentQuery.GetSinglePM(item.Id, tenant);
                if (shipmentPM != null)
                {
                    foreach (ShipmentReceivablePM rec in shipmentPM.ShipmentReceivables.Where(d => d.Quantity != null && d.UnitPrice != null))
                    {
                        result.Add(rec);
                    }
                }
            }
            return result;
        }

        public List<ShipmentPayablePM> GetMasterPayablesForProfit(string masterId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Master", "READ", tenant);

            List<ShipmentPayablePM> result = new List<ShipmentPayablePM>();
            shipmentQuery = new ShipmentQuery(tenant);
            ShipmentPM masterPM = shipmentQuery.GetSinglePM(masterId, tenant);

            foreach (ConsoleShipmentPM item in masterPM.ShipmentConsoleShipments)
            {
                ShipmentPM shipmentPM = shipmentQuery.GetSinglePM(item.Id, tenant);
                if (shipmentPM != null)
                {
                    foreach (ShipmentPayablePM payable in shipmentPM.ShipmentPayables.Where(d => d.Quantity != null && d.UnitPrice != null))
                    {
                        result.Add(payable);
                    }
                }
            }
            return result;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ShipmentList> GetMasterFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Master", "READ", tenant);

            shipmentRepository = new ShipmentRepository(tenant);
           
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            #region Restrictions region
            AddRestrictionFilters(queryOperations, "Master", tenant);
            #endregion

            ShipmentCustomFilter customfilters = new ShipmentCustomFilter(tenant);
            IQueryable<ShipmentDataView> shipments = shipmentRepository.GetMasterViewsByTenant(tenant);
            shipments = customfilters.GetFilteredQuery(queryOperations, shipments);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            shipments = filter.GetFilteredQuery<ShipmentDataView>(nonListQueryOperation, shipments);
            int skippedShipments = queryOperations.PageIndex; 

            var query2 = from f in shipments
                         select new ShipmentList()
                         {                             
                             AWBPrint = f.AWBPrint,
                             IsOperationalClosed = f.IsOperationalClosed,
                             ShipmentViewId = f.Id,
                             Id = f.Id,
                             Shipper = f.ShipperName,
                             Consignee = f.ConsigneeName,
                             DirectionId = f.DirectionId,
                             DirectionName = f.DirectionName,
                             TransportModeName = f.TransportModeName,
                             MasterShipmentNumber = f.MasterShipmentNumber,
                             House = f.House,
                             MainCarriageATD = f.MainCarriageATD,
                             CreateDateTime = f.CreateDateTime,
                             ShipmentNumber = f.ShipmentNumber,
                             ShipmentType = !string.IsNullOrEmpty(f.ShipmentTypeName) ? f.ShipmentTypeName + " " + f.ShipmentLevelName : f.ShipmentLevelName,                             
                             AgentName = f.AgentName,
                             CustomerReference1 = f.CustomerReference1,
                             CustomerReference2 = f.CustomerReference2,
                             TransportModeId = f.TransportModeId,
                             Field1 = f.Field1,
                             Field2 = f.Field2,
                             Field3 = f.Field3,
                             Field4 = f.Field4,
                             Field5 = f.Field5,
                             Field6 = f.Field6,
                             Field7 = f.Field7,
                             Field9 = f.Field9,
                             Field8 = f.Field8,
                             Field10 = f.Field10,
                             ChargeableWeightInKG = f.ChargeableWeightInKG,
                             ChargeableWeight = f.ChargeableWeight,
                             GrossWeight = f.GrossWeight,
                             ShipperReference1 = f.ShipperReference1/*, Master = f.Master*/,                             
                             Master = f.Master,
                             OpenReceivablesInLocalCurrency = f.OpenReceivablesInLocalCurrency,
                             AccountedReceivablesInLocalCurrency = f.AccountedReceivablesInLocalCurrency,
                             OpenPayablesInLocalCurrency = f.OpenPayablesInLocalCurrency,
                             AccountedPayablesInLocalCurrency = f.AccountedPayablesInLocalCurrency,
                             AccountedPayablesInProfitCurrency = f.AccountedPayablesInProfitCurrency,
                             AccountedReceivablesInProfitCurrency = f.AccountedReceivablesInProfitCurrency,
                             OpenPayablesInProfitCurrency = f.OpenPayablesInProfitCurrency,
                             OpenReceivablesInProfitCurrency = f.OpenReceivablesInProfitCurrency,
                             ShipmentPayableStatusCode = f.ShipmentPayableStatusCode,
                             ShipmentReceivableStatusCode = f.ShipmentReceivableStatusCode,
                             ShipmentPayableStatusName = f.ShipmentPayableStatusName,
                             ShipmentReceivableStatusName = f.ShipmentReceivableStatusName,
                             ProfitInLocalCurrency = f.ProfitInLocalCurrency,
                             BranchId = f.BranchId,
                             DepartmentId = f.DepartmentId,
                             MainCarriageETA = f.MainCarriageETA,
                             LocalCurrencyCode = currentTenant.CurrencyCode,
                             NextETA = f.NextETA,
                             NextETD = f.NextETD,
                             NextLegName = f.NextLegName,
                             FromPortId = f.MainCarriageFromPortId,
                             ToPortId = f.MainCarriageToPortId,
                             FromPort = !string.IsNullOrEmpty(f.MainCarriageFromPortCode) ? f.MainCarriageFromPortCode : f.FromPortCode,
                             FromPortName = !string.IsNullOrEmpty(f.MainCarriageFromPortName) ? f.MainCarriageFromPortName : f.FromPortName,
                             FromPortCountry = f.MainCarriageFromPortCountryName,
                             ToPort = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationPortCode) ? f.MainCarriageFinalDestinationPortCode : f.ToPortCode,
                             ToPortName = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationPortName) ? f.MainCarriageFinalDestinationPortName : f.ToPortName,
                             ToPortCountry = f.MainCarriageToPortCountryName,
                             MasterShipmentDataId = f.MasterShipmentDataId,
                             BranchName = f.BranchName,
                             CustomerName = f.CustomerName,
                             GrossWeightInKG = f.GrossWeightInKG,
                             ShipmentLevelCode = f.ShipmentLevelCode,
                             ShipmentLevelName = f.ShipmentLevelName,
                             VolumetricWeight = f.VolumetricWeight,                             
                             MainCarriageETD = f.MainCarriageETD,
                             MainCarriageCarrierId = f.MainCarriageCarrierId,
                             MainCarriageCarrierNumber = f.MainCarriageCarrierNumber,
                             TruckNumber = f.TruckNumber,
                             FHLStatusCode = f.FHLStatusCode,
                             FHLStatusName = f.FHLStatusName,
                             FHLStatusDate = f.FHLStatusDate,
                             FWBStatusCode = f.FWBStatusCode,
                             FWBStatusName = f.FWBStatusName,
                             FWBStatusDate = f.FWBStatusDate,
                             CargonautFHLStatusCode = f.CargonautFHLStatusCode,
                             CargonautFHLStatusName = f.CargonautFHLStatusName,
                             CargonautFHLStatusDate = f.CargonautFHLStatusDate,
                             CargonautFWBStatusCode = f.CargonautFWBStatusCode,
                             CargonautFWBStatusName = f.CargonautFWBStatusName,
                             CargonautFWBStatusDate = f.CargonautFWBStatusDate,
                             StatusId = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusId : f.ShipmentStatusId) : (f.ShipmentStatusId),
                             StatusDate = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusDate : f.ShipmentStatusDate) : (f.ShipmentStatusDate),
                             StatusName = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusName : f.ShipmentStatusName) : (f.ShipmentStatusName),
                             StatusLocation = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusLocation : f.ShipmentStatusLocation) : (f.ShipmentStatusLocation),
                             LongMaster = f.TransportModeId == "A" ? (!string.IsNullOrEmpty(f.AirlinePrefix) && !string.IsNullOrEmpty(f.Master) ? f.AirlinePrefix + "-" + f.Master : "") : f.Master,
                         };

            query2 = filter.GetFilteredQuery<ShipmentList>(listQueryOperation, query2);
            
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ShipmentList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Master", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.CreateDateTime);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.CreateDateTime);
            }

            query2 = query2.Skip(skippedShipments);
            query2 = query2.Take(queryOperations.PageSize);

            List<ShipmentList> listQuery = query2.ToList();

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("Shipment", tenant, listQuery.Cast<object>().ToList());

            return listQuery.AsQueryable();
        }

        public int GetMasterFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Master", "READ", tenant);

            shipmentRepository = new ShipmentRepository(tenant);
            
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            #region Restrictions region
            AddRestrictionFilters(queryOperations, "Master", tenant);
            #endregion

            GenericFilter filter = new GenericFilter();
            ShipmentCustomFilter customfilters = new ShipmentCustomFilter(tenant);
            IQueryable<ShipmentDataView> shipments = shipmentRepository.GetMasterViewsByTenant(tenant);
            shipments = customfilters.GetFilteredQuery(queryOperations, shipments);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            shipments = filter.GetFilteredQuery<ShipmentDataView>(nonListQueryOperation, shipments);
            int skippedShipments = queryOperations.PageIndex;

            var query2 = from f in shipments
                         select new ShipmentList()
                         {
                             AWBPrint = f.AWBPrint,
                             IsOperationalClosed = f.IsOperationalClosed,
                             ShipmentViewId = f.Id,
                             Id = f.Id,
                             Shipper = f.ShipperName,
                             Consignee = f.ConsigneeName,
                             DirectionId = f.DirectionId,
                             DirectionName = f.DirectionName,
                             TransportModeName = f.TransportModeName,
                             MasterShipmentNumber = f.MasterShipmentNumber,
                             House = f.House,
                             CreateDateTime = f.CreateDateTime,
                             ShipmentNumber = f.ShipmentNumber,
                             ShipmentType = f.ShipmentTypeName,
                             TransportModeId = f.TransportModeId,
                             Field1 = f.Field1,
                             Field2 = f.Field2,
                             Field3 = f.Field3,
                             Field4 = f.Field4,
                             Field5 = f.Field5,
                             Field6 = f.Field6,
                             Field7 = f.Field7,
                             Field9 = f.Field9,
                             Field8 = f.Field8,
                             Field10 = f.Field10,
                             ChargeableWeightInKG = f.ChargeableWeightInKG,
                             ChargeableWeight = f.ChargeableWeight,
                             GrossWeight = f.GrossWeight,
                             ShipperReference1 = f.ShipperReference1/*, Master = f.Master*/,
                             MainCarriageATD = f.MainCarriageATD,                             
                             Master = f.Master,
                             OpenReceivablesInLocalCurrency = f.OpenReceivablesInLocalCurrency,
                             AccountedReceivablesInLocalCurrency = f.AccountedReceivablesInLocalCurrency,
                             OpenPayablesInLocalCurrency = f.OpenPayablesInLocalCurrency,
                             AccountedPayablesInLocalCurrency = f.AccountedPayablesInLocalCurrency,
                             AccountedPayablesInProfitCurrency = f.AccountedPayablesInProfitCurrency,
                             AccountedReceivablesInProfitCurrency = f.AccountedReceivablesInProfitCurrency,
                             OpenPayablesInProfitCurrency = f.OpenPayablesInProfitCurrency,
                             OpenReceivablesInProfitCurrency = f.OpenReceivablesInProfitCurrency,
                             ShipmentPayableStatusCode = f.ShipmentPayableStatusCode,
                             ShipmentReceivableStatusCode = f.ShipmentReceivableStatusCode,
                             ShipmentPayableStatusName = f.ShipmentPayableStatusName,
                             ShipmentReceivableStatusName = f.ShipmentReceivableStatusName,
                             ProfitInLocalCurrency = f.ProfitInLocalCurrency,
                             BranchId = f.BranchId,
                             DepartmentId = f.DepartmentId,
                             MainCarriageETA = f.MainCarriageETA,
                             NextETA = f.NextETA,
                             NextETD = f.NextETD,
                             NextLegName = f.NextLegName,
                             FromPortId = f.MainCarriageFromPortId,
                             ToPortId = f.MainCarriageToPortId,
                             FromPort = !string.IsNullOrEmpty(f.MainCarriageFromPortCode) ? f.MainCarriageFromPortCode : f.FromPortCode,
                             FromPortName = !string.IsNullOrEmpty(f.MainCarriageFromPortName) ? f.MainCarriageFromPortName : f.FromPortName,
                             FromPortCountry = f.MainCarriageFromPortCountryName,
                             ToPort = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationPortCode) ? f.MainCarriageFinalDestinationPortCode : f.ToPortCode,
                             ToPortName = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationPortName) ? f.MainCarriageFinalDestinationPortName : f.ToPortName,
                             ToPortCountry = f.MainCarriageToPortCountryName,
                             MasterShipmentDataId = f.MasterShipmentDataId,
                             BranchName = f.BranchName,
                             CustomerName = f.CustomerName,
                             GrossWeightInKG = f.GrossWeightInKG,
                             ShipmentLevelCode = f.ShipmentLevelCode,
                             ShipmentLevelName = f.ShipmentLevelName,
                             VolumetricWeight = f.VolumetricWeight,                            
                             AgentName = f.AgentName,
                             CustomerReference1 = f.CustomerReference1,
                             CustomerReference2 = f.CustomerReference2,
                             MainCarriageETD = f.MainCarriageETD,
                             MainCarriageCarrierId = f.MainCarriageCarrierId,
                             MainCarriageCarrierNumber = f.MainCarriageCarrierNumber,
                             TruckNumber = f.TruckNumber,
                             FHLStatusCode = f.FHLStatusCode,
                             FHLStatusName = f.FHLStatusName,
                             FHLStatusDate = f.FHLStatusDate,
                             FWBStatusCode = f.FWBStatusCode,
                             FWBStatusName = f.FWBStatusName,
                             FWBStatusDate = f.FWBStatusDate,
                             CargonautFHLStatusCode = f.CargonautFHLStatusCode,
                             CargonautFHLStatusName = f.CargonautFHLStatusName,
                             CargonautFHLStatusDate = f.CargonautFHLStatusDate,
                             CargonautFWBStatusCode = f.CargonautFWBStatusCode,
                             CargonautFWBStatusName = f.CargonautFWBStatusName,
                             CargonautFWBStatusDate = f.CargonautFWBStatusDate,
                             StatusId = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusId : f.ShipmentStatusId) : (f.ShipmentStatusId),
                             StatusDate = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusDate : f.ShipmentStatusDate) : (f.ShipmentStatusDate),
                             StatusName = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusName : f.ShipmentStatusName) : (f.ShipmentStatusName),
                             StatusLocation = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusLocation : f.ShipmentStatusLocation) : (f.ShipmentStatusLocation),
                             LongMaster = f.TransportModeId == "A" ? (!string.IsNullOrEmpty(f.AirlinePrefix) && !string.IsNullOrEmpty(f.Master) ? f.AirlinePrefix + "-" + f.Master : "") : f.Master,
                         };

            query2 = filter.GetFilteredQuery<ShipmentList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public IQueryable<ShipmentList> GetFollowUpsByMastersFilter(byte[] xmlFilters, int tenant)
        {
            return null;
        }

        public int GetFollowUpsByMastersFilterCount(byte[] xmlFilters, int tenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);

            //ContactQuery contactRep = new ContactQuery(tenant);
            //ContactPM contact = contactRep.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);
            //MemoryStream memorystream = new MemoryStream(xmlFilters);
            //XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            //QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            //QueryOperations nonListQueryOperation = new QueryOperations();
            //nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            //QueryOperations listQueryOperation = new QueryOperations();
            //listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            //GenericFilter filter = new GenericFilter();
            //FollowUpsCustomFilter customfilters = new FollowUpsCustomFilter(tenant);
            //FollowUpQuery followUpsRepository = new FollowUpQuery(tenant);
            //queryOperations.QueryFilterItems.Add(new QueryFilterItem() { FieldName = "FollowUpOwnerUserId", FieldValue = contact.Id });
            //IQueryable<ShipmentList> shipmentFollowUps = followUpsRepository.GetFollowUpsByTenantForMasterFilter(tenant, null);
            //shipmentFollowUps = customfilters.GetShipmentFollowUpFilteredQuery(queryOperations, shipmentFollowUps);

            //shipmentFollowUps = filter.GetFilteredQuery<ShipmentList>(listQueryOperation, shipmentFollowUps);
            int count = 0;//shipmentFollowUps.Count();
            return count;
        }

        public List<ShipmentPM> GetConnectedShipmentsByMasterIdAndTenant(string masterId, int tenant)
        {
            shipmentQuery = new ShipmentQuery(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Master", "READ", tenant);
            return shipmentQuery.GetShipmentPMsByMasterIdAndTenant(masterId, tenant);
        }

    }
}