using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Http;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using WebFreight.Web.WebServices;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.CustomFilters;
using Logitude.BL.InfrastructureModel.EntityQueries;
using System.IdentityModel.Protocols.WSTrust;
using Logitude.SystemLogs;
using WebFreight.Web.InfrastructureModel;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using System.Threading;
using System.Runtime.Serialization.Json;
using System.Xml;
using Newtonsoft.Json;

namespace WebFreight.Web.App_Code.AngularJS_App_Code
{
    public class ngShipmentsController : ApiController
    {
        public ngShipmentsController()
        {

        }

        public List<ShipmentList> GetFilteredShipmentsList(int tenant, int PageSize, int PageIndex, string sortby, string sortDir, string searchfields)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            PageSize = PageSize == 0 ? 10 : PageSize;

            List<ShipmentList> listQuery = new List<ShipmentList>();
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);

            ObjectTableRepository rep = new ObjectTableRepository(tenant);
            ObjectTable table = rep.GetObjectTableByName("Shipment", 0, false);
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact contact = contactRepository.GetSingleContactByEmail(email, tenant);
            ContactsUnseenEntitieRepository contactsUnseenRepository = new ContactsUnseenEntitieRepository(tenant);


            QueryOperations queryOperations = new QueryOperations();
            queryOperations.QueryFilterItems = new List<QueryFilterItem>();
            queryOperations.SortByColumnName = sortby;
            queryOperations.PageIndex = PageIndex;
            queryOperations.PageSize = PageSize;

            string direction;
            switch (sortDir)
            {

                case "asc":
                    {
                        direction = "Ascending";
                        break;
                    }
                case "desc":
                    {
                        direction = "Descending";
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
            queryOperations.SortDirectin = direction;

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            ShipmentCustomFilter customfilters = new ShipmentCustomFilter(tenant);
            IQueryable<ShipmentDataView> shipments = shipmentRepository.GetShipmentViewsByTenant(tenant);
            shipments = customfilters.GetFilteredQuery(queryOperations, shipments);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            shipments = filter.GetFilteredQuery<ShipmentDataView>(nonListQueryOperation, shipments);
            int skippedShipments = queryOperations.PageIndex;

            var query2 = from f in shipments
                         where f.Tenant == tenant
                         select new ShipmentList()
                         {
                             CarrierLastStatusDate = f.CarrierLastStatusDate,
                             CarrierLastStatusName = f.CarrierLastStatusName,
                             CarrierLastStatusCode = f.CarrierLastStatusCode,
                             IsOperationalClosed = f.IsOperationalClosed,
                             ShipmentViewId = f.Id,
                             Id = f.Id,

                             Tenant = f.Tenant,

                             DirectionId = f.DirectionId,
                             DirectionName = f.DirectionName,
                             TransportModeName = f.TransportModeName,
                             MasterShipmentNumber = f.MasterShipmentNumber,
                             House = f.House,
                             CreateDateTime = f.CreateDateTime,
                             ShipmentNumber = f.ShipmentNumber,
                             ShipmentType = !string.IsNullOrEmpty(f.ShipmentTypeName) ? f.ShipmentTypeName + " " + f.ShipmentLevelName : f.ShipmentLevelName,
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
                             Field11 = f.Field11,
                             Field12 = f.Field12,
                             Field13 = f.Field13,
                             Field14 = f.Field14,
                             Field15 = f.Field15,
                             Field16 = f.Field16,
                             Field17 = f.Field17,
                             Field18 = f.Field18,
                             Field19 = f.Field19,
                             Field20 = f.Field20,
                             Field21 = f.Field21,
                             Field22 = f.Field22,
                             Field23 = f.Field23,
                             Field24 = f.Field24,
                             Field25 = f.Field25,
                             Field26 = f.Field26,
                             Field27 = f.Field27,
                             Field28 = f.Field28,
                             Field29 = f.Field29,
                             Field30 = f.Field30,
                             Field31 = f.Field31,
                             Field32 = f.Field32,
                             Field33 = f.Field33,
                             Field34 = f.Field34,
                             Field35 = f.Field35,
                             Field36 = f.Field36,
                             Field37 = f.Field37,
                             Field38 = f.Field38,
                             Field39 = f.Field39,
                             Field40 = f.Field40,
                             ChargeableWeightInKG = f.ChargeableWeightInKG,
                             ChargeableWeight = f.ChargeableWeight,
                             GrossWeight = f.GrossWeight,
                             Master = f.Master,
                             OpenReceivablesInLocalCurrency = f.OpenReceivablesInLocalCurrency,
                             OpenReceivablesInProfitCurrency = f.OpenReceivablesInProfitCurrency,
                             AccountedReceivablesInLocalCurrency = f.AccountedReceivablesInLocalCurrency,
                             OpenPayablesInLocalCurrency = f.OpenPayablesInLocalCurrency,
                             ShipmentPayableStatusCode = f.ShipmentPayableStatusCode,
                             ShipmentReceivableStatusCode = f.ShipmentReceivableStatusCode,
                             ShipmentPayableStatusName = f.ShipmentPayableStatusName,
                             ShipmentReceivableStatusName = f.ShipmentReceivableStatusName,
                             ProfitInLocalCurrency = f.ProfitInLocalCurrency,
                             ProfitInProfitCurrency = f.ProfitInProfitCurrency,
                             EstimateProfitInLocalCurrency = f.EstimateProfitInLocalCurrency,
                             EstimateProfitInProfitCurrency = f.EstimateProfitInProfitCurrency,
                             BranchId = f.BranchId,
                             DepartmentId = f.DepartmentId,
                             MainCarriageETA = f.MainCarriageETA,
                             MainCarriageATD = f.MainCarriageATD,
                             LocalCurrencyCode = currentTenant.CurrencyCode,
                             ProfitCurrencyCode = currentTenant.ProfitCurrencyCode,
                             NextETA = f.NextETA,
                             NextETD = f.NextETD,
                             NextLegName = f.NextLegName,
                             Routing = f.Routing,
                             MasterShipmentDataId = f.MasterShipmentDataId,
                             BranchName = f.BranchName,
                             GrossWeightInKG = f.GrossWeightInKG,
                             ShipmentLevelCode = f.ShipmentLevelCode,
                             ShipmentLevelName = f.ShipmentLevelName,
                             VolumetricWeight = f.VolumetricWeight,
                             AccountedPayablesInLocalCurrency = f.AccountedPayablesInLocalCurrency,
                             AccountedPayablesInProfitCurrency = f.AccountedPayablesInProfitCurrency,
                             AccountedReceivablesInProfitCurrency = f.AccountedReceivablesInProfitCurrency,
                             MainCarriageATA = f.MainCarriageATA,
                             MainCarriageETD = f.MainCarriageETD,
                             IncotermId = f.IncotermId,
                             OpenPayablesInProfitCurrency = f.OpenPayablesInProfitCurrency,
                             IssuingCarrierAgentId = f.IssuingCarrierAgentId,
                             IncotermCode = f.IncotermCode,
                             MainCarriageCarrierId = f.MainCarriageCarrierId,
                             AsAgreedFreight = f.AsAgreedFreight,
                             AsAgreedOtherCharges = f.AsAgreedOtherCharges,
                             AccountNumber = f.AccountNumber,
                             AWBPrint = f.AWBPrint,
                             FHLStatusCode = f.FHLStatusCode,
                             FHLStatusName = f.FHLStatusName,
                             FWBStatusCode = f.FWBStatusCode,
                             FWBStatusName = f.FWBStatusName,
                             FNAReason = f.FNAReason,
                             FinalArrivalDate = f.FinalArrivalDate,
                             Shipper = f.ShipperName,
                             Consignee = f.ConsigneeName,
                             ShipperReference1 = f.ShipperReference1,
                             ShipperReference2 = f.ShipperReference2,
                             ConsigneeReference1 = f.ConsigneeReference1,
                             ConsigneeReference2 = f.ConsigneeReference2,
                             CustomerId = f.CustomerId,
                             CustomerName = f.CustomerName,
                             CustomerReference1 = f.CustomerReference1,
                             CustomerReference2 = f.CustomerReference2,
                             FromPortId = !string.IsNullOrEmpty(f.MainCarriageFromPortId) ? f.MainCarriageFromPortId : f.FromPortId,
                             FromPort = !string.IsNullOrEmpty(f.MainCarriageFromPortCode) ? f.MainCarriageFromPortCode : f.FromPortCode,
                             FromPortName = !string.IsNullOrEmpty(f.MainCarriageFromPortName) ? f.MainCarriageFromPortName : f.FromPortName,
                             FromPortCountry = f.MainCarriageFromPortCountryName,
                             MainCarriageFromPortId = f.MainCarriageFromPortId,
                             MainCarriageFromPortName = f.MainCarriageFromPortName,
                             ToPortId = !string.IsNullOrEmpty(f.MainCarriageToPortId) ? f.MainCarriageToPortId : f.ToPortId,
                             ToPort = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationPortCode) ? f.MainCarriageFinalDestinationPortCode : f.ToPortCode,
                             ToPortName = !string.IsNullOrEmpty(f.MainCarriageFinalDestinationPortName) ? f.MainCarriageFinalDestinationPortName : f.ToPortName,
                             ToPortCountry = f.MainCarriageToPortCountryName,
                             FromCountryCode = f.ShipmentLevelCode == "H" && string.IsNullOrEmpty(f.MasterShipmentDataId) ? f.FromPortCountryCode : f.MainCarriageFromPortCountryCode,
                             ToCountryCode = f.ShipmentLevelCode == "H" && string.IsNullOrEmpty(f.MasterShipmentDataId) ? f.ToPortCountryCode : f.MainCarriageToPortCountryCode,
                             ChargeableWeightUnitCode = f.ChargeableWeightUnitCode,
                             NumberOfPackages = f.NumberOfPackages,
                             NumberOfContainers = f.NumberOfContainers,
                             BookingNumberOfPackages = f.BookingNumberOfPackages,
                             OrderChargeableWeight = f.OrderChargeableWeight,
                             MainCarriageFromCity = f.MainCarriageFromCity,
                             MainCarriageFromCountryCode = f.MainCarriageFromCountryCode,
                             MainCarriageToCity = f.MainCarriageToCity,
                             MainCarriageToCountryCode = f.MainCarriageToCountryCode,
                             MainCarriageCarrierCode = f.MainCarriageCarrierCode,
                             MainCarriageCarrierName = f.MainCarriageCarrierName,
                             MainCarriageCarrierNumber = f.MainCarriageCarrierNumber,
                             AgentName = f.AgentName,
                             AgentReference1 = f.AgentReference1,
                             AgentReference2 = f.AgentReference2,
                             LastUpdateDate = f.LastUpdateDate,
                             LastFSRStatusRequestDate = f.LastFSRStatusRequestDate,
                             CarrierNumber = f.TransportModeId == "A" ? (f.MainCarriageCarrierCode + f.MainCarriageCarrierNumber) : f.TransportModeId == "O" ? (f.MainCarriageVesselName + "/" + f.MainCarriageCarrierNumber) : f.TransportModeId == "I" ? (f.MainCarriageCarrierNumber) : null,
                             LastStatusLogDate = f.LastStatusLogDate,
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
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Shipment", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    if (!objectField.IsCustom)
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
                            case "lookup":
                                {
                                    query2 = sortClass.GetSorterQuery<ShipmentList, string>(queryOperations, query2);
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
                    else
                    {
                        query2 = sortClass.GetSorterQuery<ShipmentList, string>(queryOperations, query2);
                    }
                }
            }
            else
            {
                //query2 = query2.OrderByDescending(d => d.LastStatusLogDate);
                query2 = query2.OrderByDescending(d => d.StatusDate);
            }
            //query2 = query2.Skip(skippedShipments);
            //query2 = query2.Take(queryOperations.PageSize);
            query2 = query2.Skip(PageIndex);
            query2 = query2.Take(PageSize);

            listQuery = query2.ToList().Select(f => new ShipmentList()
            {
                CarrierLastStatusDate = f.CarrierLastStatusDate,
                CarrierLastStatusName = f.CarrierLastStatusName,
                CarrierLastStatusCode = f.CarrierLastStatusCode,
                IsOperationalClosed = f.IsOperationalClosed,
                ShipmentViewId = f.Id,
                Id = f.Id,

                Tenant = f.Tenant,

                DirectionId = f.DirectionId,
                DirectionName = f.DirectionName,
                TransportModeName = f.TransportModeName,
                MasterShipmentNumber = f.MasterShipmentNumber,
                House = f.House,
                CreateDateTime = f.CreateDateTime.ToUniversalTime(),
                ShipmentNumber = f.ShipmentNumber,
                ShipmentType = f.ShipmentType,
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
                Field11 = f.Field11,
                Field12 = f.Field12,
                Field13 = f.Field13,
                Field14 = f.Field14,
                Field15 = f.Field15,
                Field16 = f.Field16,
                Field17 = f.Field17,
                Field18 = f.Field18,
                Field19 = f.Field19,
                Field20 = f.Field20,
                Field21 = f.Field21,
                Field22 = f.Field22,
                Field23 = f.Field23,
                Field24 = f.Field24,
                Field25 = f.Field25,
                Field26 = f.Field26,
                Field27 = f.Field27,
                Field28 = f.Field28,
                Field29 = f.Field29,
                Field30 = f.Field30,
                Field31 = f.Field31,
                Field32 = f.Field32,
                Field33 = f.Field33,
                Field34 = f.Field34,
                Field35 = f.Field35,
                Field36 = f.Field36,
                Field37 = f.Field37,
                Field38 = f.Field38,
                Field39 = f.Field39,
                Field40 = f.Field40,
                ChargeableWeightInKG = f.ChargeableWeightInKG,
                ChargeableWeight = f.ChargeableWeight,
                GrossWeight = f.GrossWeight,

                StatusName = f.StatusName,
                StatusId = f.StatusId,
                Master = f.Master,
                OpenReceivablesInLocalCurrency = f.OpenReceivablesInLocalCurrency,
                OpenReceivablesInProfitCurrency = f.OpenReceivablesInProfitCurrency,
                AccountedReceivablesInLocalCurrency = f.AccountedReceivablesInLocalCurrency,
                OpenPayablesInLocalCurrency = f.OpenPayablesInLocalCurrency,
                ShipmentPayableStatusCode = f.ShipmentPayableStatusCode,
                ShipmentReceivableStatusCode = f.ShipmentReceivableStatusCode,
                ShipmentPayableStatusName = f.ShipmentPayableStatusName,
                ShipmentReceivableStatusName = f.ShipmentReceivableStatusName,
                ProfitInLocalCurrency = f.ProfitInLocalCurrency,
                ProfitInProfitCurrency = f.ProfitInProfitCurrency,
                EstimateProfitInLocalCurrency = f.EstimateProfitInLocalCurrency,
                EstimateProfitInProfitCurrency = f.EstimateProfitInProfitCurrency,
                BranchId = f.BranchId,
                DepartmentId = f.DepartmentId,
                MainCarriageETA = f.MainCarriageETA,
                MainCarriageATD = f.MainCarriageATD,
                LocalCurrencyCode = currentTenant.CurrencyCode,
                ProfitCurrencyCode = currentTenant.ProfitCurrencyCode,
                NextETA = f.NextETA,
                NextETD = f.NextETD,
                NextLegName = f.NextLegName,
                Routing = f.Routing,
                MasterShipmentDataId = f.MasterShipmentDataId,
                BranchName = f.BranchName,
                GrossWeightInKG = f.GrossWeightInKG,
                ShipmentLevelCode = f.ShipmentLevelCode,
                ShipmentLevelName = f.ShipmentLevelName,
                VolumetricWeight = f.VolumetricWeight,
                LongMaster = f.LongMaster,
                AccountedPayablesInLocalCurrency = f.AccountedPayablesInLocalCurrency,
                AccountedPayablesInProfitCurrency = f.AccountedPayablesInProfitCurrency,
                AccountedReceivablesInProfitCurrency = f.AccountedReceivablesInProfitCurrency,
                MainCarriageATA = f.MainCarriageATA,
                MainCarriageETD = f.MainCarriageETD,
                IncotermId = f.IncotermId,
                OpenPayablesInProfitCurrency = f.OpenPayablesInProfitCurrency,

                IssuingCarrierAgentId = f.IssuingCarrierAgentId,
                IncotermCode = f.IncotermCode,
                MainCarriageCarrierId = f.MainCarriageCarrierId,
                AsAgreedFreight = f.AsAgreedFreight,
                AsAgreedOtherCharges = f.AsAgreedOtherCharges,
                AccountNumber = f.AccountNumber,
                AWBPrint = f.AWBPrint,
                FHLStatusCode = f.FHLStatusCode,
                FHLStatusName = f.FHLStatusName,
                FWBStatusCode = f.FWBStatusCode,
                FWBStatusName = f.FWBStatusName,
                FNAReason = f.FNAReason,
                FinalArrivalDate = f.FinalArrivalDate,
                Shipper = f.Shipper,
                Consignee = f.Consignee,
                ShipperReference1 = f.ShipperReference1,
                ShipperReference2 = f.ShipperReference2,
                ConsigneeReference1 = f.ConsigneeReference1,
                ConsigneeReference2 = f.ConsigneeReference2,
                CustomerId = f.CustomerId,
                CustomerName = f.CustomerName,
                CustomerReference1 = f.CustomerReference1,
                CustomerReference2 = f.CustomerReference2,
                FromPortId = f.FromPortId,
                FromPort = f.FromPort,
                FromPortName = f.FromPortName,
                FromPortCountry = f.FromPortCountry,
                MainCarriageFromPortId = f.MainCarriageFromPortId,
                MainCarriageFromPortName = f.MainCarriageFromPortName,

                ToPortId = f.ToPortId,
                ToPort = f.ToPort,
                ToPortName = f.ToPortName,
                ToPortCountry = f.ToPortCountry,

                FromCountryCode = f.FromCountryCode,
                ToCountryCode = f.ToCountryCode,

                ChargeableWeightUnitCode = f.ChargeableWeightUnitCode,
                NumberOfPackages = f.NumberOfPackages,
                NumberOfContainers = f.NumberOfContainers,
                BookingNumberOfPackages = f.BookingNumberOfPackages,
                OrderChargeableWeight = f.OrderChargeableWeight,
                StatusDate = f.StatusDate,
                MainCarriageFromCity = f.MainCarriageFromCity,
                MainCarriageFromCountryCode = f.MainCarriageFromCountryCode,
                MainCarriageToCity = f.MainCarriageToCity,
                MainCarriageToCountryCode = f.MainCarriageToCountryCode,

                MainCarriageCarrierCode = f.MainCarriageCarrierCode,
                MainCarriageCarrierName = f.MainCarriageCarrierName,
                MainCarriageCarrierNumber = f.MainCarriageCarrierNumber,

                AgentName = f.AgentName,
                AgentReference1 = f.AgentReference1,
                AgentReference2 = f.AgentReference2,

                LastUpdateDate = f.LastUpdateDate,
                LastFSRStatusRequestDate = f.LastFSRStatusRequestDate,
                CarrierNumber = f.CarrierNumber,
                LastStatusLogDate = f.LastStatusLogDate,
                // IsShipmentTracking = f.i
            }).ToList();

            return listQuery;

        }

        [OperationContract]
        [WebGet(UriTemplate = "getcount/{tenant}")]
        public int GetShipmentsListCount(int tenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);

            int shipmentsCount = shipmentRepository.GetShipmentsCount(tenant);

            return shipmentsCount;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getsinglepm/{id}/{tenant}")]
        public ShipmentPM GetSingleShipmentPM(string id, int tenant)
        {
            //Thread.Sleep(new TimeSpan(0, 0, 0, 10));
            //SecurityUtility.AuthenticationOnTenant(tenant);

            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            ShipmentPM pm = shipmentQuery.GetSinglePM(id, tenant);
            //CheckSharedContactAuthenticationForShipment(pm.AgentId, pm.CustomerId, tenant);

            return pm;
        }

        [WebInvoke(
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            Method = "POST"
        )]


        public ShipmentPM post(ShipmentPM shipmentPM)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            string email = SecurityUtility.GetAuthenticatedUser();
            Contact contact = null;
            if (!string.IsNullOrEmpty(email))
            {
                ContactRepository contactrep = new ContactRepository(shipmentPM.Tenant);
                contact = contactrep.GetSingleContactByEmail(email, shipmentPM.Tenant);
            }

            TenantRepository tenantRepository = new TenantRepository(shipmentPM.Tenant);
            TenantQuery tenantQuery = new TenantQuery(tenantRepository);
            var tenantPM = tenantQuery.GetSinglePM(shipmentPM.Tenant);

            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTimeKind.Utc);

            //shipmentPM.Tenant = shipmentPM.Tenant;
            shipmentPM.IsOperationalClosed = false;
            //shipmentPM.CreatedByUserId = contact.Id;
            //shipmentPM.DepartmentId = contact.User.DepartmentId;
            //shipmentPM.BranchId = contact.User.BranchId;
            shipmentPM.StatusId = EntityStatusRepository.GetSingleEntityStatusByCode("SHOR", shipmentPM.Tenant, true).Id;
            shipmentPM.Field1 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field1" };
            shipmentPM.Field2 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field2" };
            shipmentPM.Field3 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field3" };
            shipmentPM.Field4 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field4" };
            shipmentPM.Field5 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field5" };
            shipmentPM.Field6 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field6" };
            shipmentPM.Field7 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field7" };
            shipmentPM.Field8 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field8" };
            shipmentPM.Field9 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field9" };
            shipmentPM.Field10 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field10" };
            shipmentPM.Field11 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field11" };
            shipmentPM.Field12 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field12" };
            shipmentPM.Field13 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field13" };
            shipmentPM.Field14 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field14" };
            shipmentPM.Field15 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field15" };
            shipmentPM.Field16 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field16" };
            shipmentPM.Field17 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field17" };
            shipmentPM.Field18 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field18" };
            shipmentPM.Field19 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field19" };
            shipmentPM.Field20 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field20" };
            shipmentPM.Field21 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field21" };
            shipmentPM.Field22 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field22" };
            shipmentPM.Field23 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field23" };
            shipmentPM.Field24 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field24" };
            shipmentPM.Field25 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field25" };
            shipmentPM.Field26 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field26" };
            shipmentPM.Field27 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field27" };
            shipmentPM.Field28 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field28" };
            shipmentPM.Field29 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field29" };
            shipmentPM.Field30 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field30" };
            shipmentPM.Field31 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field31" };
            shipmentPM.Field32 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field32" };
            shipmentPM.Field33 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field33" };
            shipmentPM.Field34 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field34" };
            shipmentPM.Field35 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field35" };
            shipmentPM.Field36 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field36" };
            shipmentPM.Field37 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field37" };
            shipmentPM.Field38 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field38" };
            shipmentPM.Field39 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field39" };
            shipmentPM.Field40 = new CustomFieldClass { TableName = "Shipment", FieldName = "Field40" };

            shipmentPM.FHLStatusCode = "NSEN";
            shipmentPM.FWBStatusCode = "NSEN";
            shipmentPM.ManifestStatusCode = "NSEN";
            shipmentPM.FHLStatusName = "Not Sent";
            shipmentPM.FWBStatusName = "Not Sent";
            shipmentPM.DimensionsUnitCode = tenantPM.DimensionsUnitCode;
            shipmentPM.VolumeUnitCode = tenantPM.VolumeUnitCode;
            shipmentPM.GrossWeightUnitCode = tenantPM.GrossWeightUnitCode;
            shipmentPM.ChargeableWeightUnitCode = tenantPM.ChargeableWeightUnitCode;
            shipmentPM.AWBCurrencyId = tenantPM.FreightCurrencyId;
            shipmentPM.ProfitCurrencyId = tenantPM.ProfitCurrencyId;
            shipmentPM.NewConcurrencyGUID = Guid.NewGuid().ToString();
            shipmentPM.CreateDateTime = todayDate;
            shipmentPM.LastUpdateDate = todayDate;
            shipmentPM.StatusDate = todayDate;

            IShipmentsContext context = ShipmentsContext.GetContext(shipmentPM.Tenant);

            shipmentPM.FreightPrepaidCollectId = "P";
            shipmentPM.OtherPrepaidCollectId = "P";
            shipmentPM.OpenPayablesInLocalCurrency = 0;
            shipmentPM.ShipmentLevelCode = "D";

            ShipmentService service = new ShipmentService(context, shipmentPM, SecurityUtility.GetAuthenticatedUser());

            service.Create();

            ShipmentQuery query = new ShipmentQuery(shipmentPM.Tenant);

            return query.GetSinglePM(shipmentPM.Id, shipmentPM.Tenant);
        }

        //[OperationContract]
        //[WebGet(UriTemplate = "UpdateShipment")]
        [WebInvoke(
            //UriTemplate = "api/Shipments/UpdateShipment",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            Method = "PUT"
        )]


        public HttpResponseMessage put(ShipmentPM entityPM)
        {
            try
            {
                //SecurityUtility.AuthenticationOnTenant(tenant);
                string email = SecurityUtility.GetAuthenticatedUser();
                Contact contact = null;
                if (!string.IsNullOrEmpty(email))
                {
                    ContactRepository contactrep = new ContactRepository(entityPM.Tenant);
                    contact = contactrep.GetSingleContactByEmail(email, entityPM.Tenant);
                }

                //DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ShipmentPM));

                //ShipmentPM shipmentPM1 = (ShipmentPM)serializer.ReadObject(shipmentPM);
                //ShipmentPM shipmentPM1 = JsonConvert.DeserializeObject<ShipmentPM>(shipmentPM);

                //ShipmentQuery query = new ShipmentQuery(entityPM.Tenant);
                //List<ShipmentPackagePM> packages = new List<ShipmentPackagePM>();

                //var pm = query.GetSinglePM(entityPM.Id, entityPM.Tenant);
                //packages = pm.ShipmentPackages.ToList();
                //pm.Notes = entityPM.Notes;

                IShipmentsContext objectContext = ShipmentsContext.GetContext(entityPM.Tenant);
                ShipmentService service = new ShipmentService(objectContext, entityPM, SecurityUtility.GetAuthenticatedUser());
                service.SetChangeSet(entityPM.ShipmentPackages, new List<ShipmentOrderPackagePM>(), new List<ShipmentPickUpPM>(), new List<ShipmentDeliveryPM>(), entityPM.ShipmentReceivables, entityPM.ShipmentPayables, new List<ShipmentFollowUpPM>(), new List<ShipmentAWBPrintOnlyPM>(), new List<ConsoleShipmentPM>(), new List<ShipmentCarrierStatusPM>(), new List<AWBOCIPM>(), new List<ShipmentCommodityPM>(), new List<ShipmentAssemblyPM>(), new List<ShipmentStoragePricingPM>(),new List<ShipmentProductItemPM>(), new List<ShipmentUnassignedFieldPM>());
                service.Update();
                //query = new ShipmentQuery(shipmentPM.Tenant);
                //var shipmentPM2 = query.GetSinglePM(shipmentPM.Id, shipmentPM.Tenant);
                 
                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


    }
}