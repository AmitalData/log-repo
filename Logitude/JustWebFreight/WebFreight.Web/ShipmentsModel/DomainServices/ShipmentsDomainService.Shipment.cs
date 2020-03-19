using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.Security;
using System.ServiceModel.DomainServices.Server;
using System.IO;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using System.Reflection;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using WebFreight.Web.InfrastructureModel.DomainServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Counters;
using WebFreight.Web.WebServices;
using System.Threading;
using System.Web;
using System.Transactions;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.ShipmentsModel.CustomFilters;
using Logitude.Server.Tools.Helpers;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.Tools.Validating;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using WebFreight.Web.WcfApi;
using System.Data.SqlClient;
using Logitude.XSD.FSR;
using System.Text.RegularExpressions;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        public ShipmentPM GetSingleShipmentPresentationModel(string shipmentPMId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            shipmentQuery = new ShipmentQuery(tenant);
            string s = SecurityUtility.GetAuthenticatedUser();
            ShipmentPM result = shipmentQuery.GetSinglePM(shipmentPMId, tenant);
            return result;
        }

        public ShipmentPM GetSingleShipmentPMWithoutComposition(string shipmentpmid, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            shipmentQuery = new ShipmentQuery(tenant);
            return shipmentQuery.GetSinglePMWithoutComposition(shipmentpmid, tenant);
        }

        public ShipmentPM GetSingleShipmentPM(string shipmentId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            shipmentQuery = new ShipmentQuery(tenant);
            return shipmentQuery.GetSinglePM(shipmentId, tenant);
        }

        public ShipmentsSummary GetShipmentsDashBoardSummary(int tenant, string directionId, string transportModeId)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            bool hasETDFeature = SecurityUtility.CheckTableContactFeature("Shipment", "EXPECTEDDEPATURE", tenant);
            bool hasFollowupsFeature = SecurityUtility.CheckTableContactFeature("Shipment", "MYFOLLOWUPS", tenant) || SecurityUtility.CheckTableContactFeature("Shipment", "ALLFOLLOWUPS", tenant);
            bool hasExpDepNotTransmittedFeature = SecurityUtility.CheckTableContactFeature("Shipment", "ExpectedDeparturesNotTransmitted", tenant);
            bool hasShippingInstructionsLast7DaysFeature = SecurityUtility.CheckTableContactFeature("Shipment", "ShippingInstructionsLast7Days", tenant);
            bool hasContainerStatusLast7DaysFeature = SecurityUtility.CheckTableContactFeature("Shipment", "ContainerStatusLast7Days", tenant);
            bool hasEBookingInProgressFeature = SecurityUtility.CheckTableContactFeature("Shipment", "Shipment.Q.EBookingInProgress", tenant);

            string loggedUserEmail = ServiceContext.User.Identity.Name;
            string loggedContactId = null;
            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM loggedContact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
            if (loggedContact != null)
            {
                loggedContactId = loggedContact.Id;
            }

            shipmentQuery = new ShipmentQuery(tenant);

            ShipmentsSummary myResult = shipmentQuery.GetShipmentsDashBoardSummary(tenant, directionId, transportModeId, loggedContactId, hasETDFeature, hasFollowupsFeature, hasExpDepNotTransmittedFeature, hasShippingInstructionsLast7DaysFeature, hasContainerStatusLast7DaysFeature, hasEBookingInProgressFeature);

            return myResult;
        }

        public List<FlightSummary> GetShipmentsDashBoardDeparturesArrivals(int tenant, string directionId, string transportModeId)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            string loggedUserEmail = ServiceContext.User.Identity.Name;
            string loggedContactId = null;
            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM loggedContact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
            if (loggedContact != null)
            {
                loggedContactId = loggedContact.Id;
            }

            shipmentQuery = new ShipmentQuery(tenant);

            List<FlightSummary> myResult = shipmentQuery.GetShipmentsDashBoardDeparturesArrivals(tenant, directionId, transportModeId);

            return myResult;
        }

        public List<ShipmentList> GetShipmentListForFollowUps(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            FollowUpQuery followUpsRepository = new FollowUpQuery(tenant);
            List<ShipmentList> followups = followUpsRepository.GetShipmentFollowUpsByShipmentId(id, tenant);

            followups = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), followups.AsQueryable<ShipmentList>(), tenant).ToList();

            if (SecurityUtility.CheckTableContactFeature("User", "PRODUCTS", tenant))
            {
                followups = ProductPermitionsFilter.AddUserProductRestrictionFilters(new QueryOperations(), followups.AsQueryable<ShipmentList>(), tenant).ToList();
            }

            return followups.ToList();
        }

        public ShipmentList GetSingleShipmentList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            if (this.objectContext == null)
            {
                this.objectContext = ShipmentsContext.GetContext(tenant);
            }


            shipmentRepository = new ShipmentRepository(objectContext);
            ShipmentQuery myShipmentQuery = new ShipmentQuery(shipmentRepository);

            ShipmentDataView f = shipmentRepository.GetSingleShipmentDataView(id, tenant);

            ShipmentList myResult = myShipmentQuery.GetSingleShipmentList(f, tenant);

            return myResult;
        }

        public IQueryable<ShipmentList> GetFollowUpsByShipmentsFilter(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ContactQuery contactRep = new ContactQuery(tenant);
            ContactPM contact = contactRep.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);
            shipmentRepository = new ShipmentRepository(tenant);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            FollowUpsCustomFilter customfilters = new FollowUpsCustomFilter(tenant);

            BranchPermitionsFilter.AddUserBranchRestrictionFilters(listQueryOperation, tenant);

            if (SecurityUtility.CheckTableContactFeature("User", "PRODUCTS", tenant))
            {
                ProductPermitionsFilter.AddUserProductRestrictionFilters(listQueryOperation, tenant);
            }

            IQueryable<ShipmentFollowUpDataView> shipmentFollowUps = shipmentRepository.GetShipmentFollowUpDataViewByTenant(tenant);
            shipmentFollowUps = customfilters.GetShipmentFollowUpFilteredQuery(queryOperations, shipmentFollowUps);
            int skippedShipments = queryOperations.PageIndex;

            var query2 = from f in shipmentFollowUps
                         select new ShipmentList()
                         {
                             FNAReason = f.FNAReason,
                             CarrierLastStatusDate = f.CarrierLastStatusDate,
                             CarrierLastStatusName = f.CarrierLastStatusName,
                             CarrierLastStatusCode = f.CarrierLastStatusCode,
                             ShipmentViewId = f.Id + f.FollowUpId,
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
                             ChargeableWeightInKG = f.ChargeableWeightInKG,
                             ChargeableWeight = f.ChargeableWeight,
                             GrossWeight = f.GrossWeight,
                             ShipperReference1 = f.ShipperReference1/*, Master = f.Master*/,
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
                             NextETA = f.NextETA,
                             NextETD = f.NextETD,
                             NextLegName = f.NextLegName,
                             Routing = f.Routing,//(f.DirectionId!="D"&&f.TransportModeId!="I")?((f.PreCarriageFromPortCode != null ? f.PreCarriageFromPortCode + " > " : "") + (f.MasterShipmentDataId != null ? (f.MainCarriageFromPortCode != null ? f.MainCarriageFromPortCode + " > " : "") + (f.MainCarriageFinalDestinationPortCode != null ? (f.OnCarriageToPortCode != null ? f.MainCarriageFinalDestinationPortCode + " > " + f.OnCarriageToPortCode : f.MainCarriageFinalDestinationPortCode) : "") : ((f.FromPortCode != null ? f.FromPortCode + " > " : "") + (f.ToPortCode != null ? (f.OnCarriageToPortCode != null ? f.ToPortCode + " > " + f.OnCarriageToPortCode : f.ToPortCode) : "")))):(""),
                             FromPortId = !string.IsNullOrEmpty(f.MainCarriageFromPortId) ? f.MainCarriageFromPortId : f.FromPortId,
                             ToPortId = !string.IsNullOrEmpty(f.MainCarriageToPortId) ? f.MainCarriageToPortId : f.ToPortId,
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
                             AirlinePrefix = f.AirlinePrefix,
                             AccountedPayablesInLocalCurrency = f.AccountedPayablesInLocalCurrency,
                             AccountedPayablesInProfitCurrency = f.AccountedPayablesInProfitCurrency,
                             AccountedReceivablesInProfitCurrency = f.AccountedReceivablesInProfitCurrency,
                             MainCarriageFromPortId = f.MainCarriageFromPortId,
                             MainCarriageFromPortName = f.MainCarriageFromPortName,
                             MainCarriageATA = f.MainCarriageATA,
                             MainCarriageETD = f.MainCarriageETD,
                             IncotermId = f.IncotermId,
                             OpenPayablesInProfitCurrency = f.OpenPayablesInProfitCurrency,
                             CustomerReference1 = f.CustomerReference1,
                             CustomerReference2 = f.CustomerReference2,
                             IssuingCarrierAgentId = f.IssuingCarrierAgentId,
                             IncotermCode = f.IncotermCode,
                             AsAgreedFreight = f.AsAgreedFreight,
                             AsAgreedOtherCharges = f.AsAgreedOtherCharges,
                             AccountNumber = f.AccountNumber,
                             FollowUpDate = f.FollowUpDate,
                             FollowUpId = f.FollowUpId,
                             FollowUpNotes = f.FollowUpNotes,
                             FollowUpOwner = f.FollowUpOwner,
                             FollowUpOwnerId = f.FollowUpOwnerId,
                             FollowUpType = f.FollowUpType,
                             FollowUpTypeId = f.FollowUpTypeId,
                             VolumeInCBM = f.VolumeInCBM,
                             AgentId = f.AgentId,
                             ARInvoiceIssued = f.ARInvoiceIssued,
                             CreditNoteIssued = f.CreditNoteIssued,
                             FreightForwarderId = f.FreightForwarderId,
                             FreightForwarderName = f.FreightForwarderName,
                             ProductCode = f.ProductCode,
                             IsAccountingClosed = f.IsAccountingClosed,
                             IsOperationalClosed = f.IsOperationalClosed,
                             OperationalCloseDate = f.OperationalCloseDate,
                             AccountingCloseDate = f.AccountingCloseDate,
                             PackagesQuantity = f.PackagesQuantity,
                             LocalCustomsTransmissionsStatusCode = f.LocalCustomsTransmissionsStatusCode,
                             LocalCustomsTransmissionsStatusName = f.LocalCustomsTransmissionsStatusName,
                             LocalCustomsTransmissionsStatusError = f.LocalCustomsTransmissionsStatusError,
                             LocalCustomsTransmissionsStatusDate = f.LocalCustomsTransmissionsStatusDate,
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
                             NumberOfInsidePackages = f.NumberOfInsidePackages,
                             NumberOfInsidePackagesDetails = f.NumberOfInsidePackagesDetails,
                             ConsolidatorId = f.ConsolidatorId,
                             ConsolidatorName = f.ConsolidatorName,
                             ConsolidatorNote = f.ConsolidatorNote,
                             ConsolidatorAddressId = f.ConsolidatorAddressId,
                             ConsolidatorContactId = f.ConsolidatorContactId,
                             ConsolidatorReference = f.ConsolidatorReference,
                             ManifestReason = f.ManifestReason,
                             ManifestStatusCode = f.ManifestStatusCode,
                             FromPortCountryCode = f.FromPortCountryCode,
                             ToPortCountryCode = f.ToPortCountryCode,
                             StatusId = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusId : f.ShipmentStatusId) : (f.ShipmentStatusId),
                             StatusDate = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusDate : f.ShipmentStatusDate) : (f.ShipmentStatusDate),
                             StatusName = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusName : f.ShipmentStatusName) : (f.ShipmentStatusName),
                             StatusLocation = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusLocation : f.ShipmentStatusLocation) : (f.ShipmentStatusLocation),
                             LongMaster = f.TransportModeId == "A" ? (!string.IsNullOrEmpty(f.AirlinePrefix) && !string.IsNullOrEmpty(f.Master) ? f.AirlinePrefix + "-" + f.Master : "") : f.Master,
                             CustomsDeclarationNumber = f.CustomsDeclarationNumber,
                             OperationalDate = f.OperationalDate,
                             CutoffDate = f.CutoffDate,
                             NumberOfHouses = f.NumberOfHouses,
                             WarehouseLegLastFreeDate = f.WarehouseLegLastFreeDate,
                             LastFinalDestination = f.LastFinalDestination,
                             EstimatedFinalArrivalDate = f.EstimatedFinalArrivalDate,
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
            return query2;
        }

        private void AddRestrictionFilters(QueryOperations queryOperations, string objectTableName, int tenant)
        {
            try
            {
                ContactQuery contactRep = new ContactQuery(tenant);
                ContactTenantQuery contactTenantsRepository = new ContactTenantQuery(tenant);
                RestrictionQuery restrictionQuery = new RestrictionQuery(tenant);

                ObjectTablePM objectTable = ObjectTableQuery.GetObjectTableByCode(objectTableName, tenant);
                ContactPM contact = contactRep.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);
                ContactTenantPM contactTenant = contactTenantsRepository.GetContactTenantForUser(contact.Id, tenant);
                List<RestrictionPM> restrictions = restrictionQuery.GetResitrictionsByObjectTableAndContact(contactTenant.Id, objectTable.Id, tenant).ToList();
                var query = from restriction in restrictions
                            group restriction by restriction.ObjectFieldCode into objectTableGroup
                            select new
                            {
                                key = objectTableGroup.Key,
                                ObjectTableGroup = objectTableGroup
                            };
                foreach (var item in query)
                {
                    string values = "";
                    foreach (RestrictionPM restriction in item.ObjectTableGroup)
                    {
                        values = values + restriction.Value + ",";
                    }
                    queryOperations.QueryFilterItems.Add(new QueryFilterItem() { FieldName = item.ObjectTableGroup.FirstOrDefault().ObjectFieldName, Operator = "InList", FieldValue = values.TrimEnd(',') });
                }
            }
            catch
            {
            }
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ShipmentList> GetShipmentFilters_00(byte[] xmlFilters, int tenant)
        {
            IQueryable<ShipmentList> myResult = null;

            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            var shipmentContext = ShipmentsContext.GetContext(tenant);
            var commonContext = CommonDataContext.GetContext(tenant);
            var freightContext = WebFreightContext.GetContext(tenant);

            #region Join
            IQueryable<ShipmentList> iQueryable = (from myShipment in shipmentContext.Shipments

                                                   // Join Shipments --> ShipmentMasterDatas
                                                   join db_Masters in shipmentContext.ShipmentMasterDatas on myShipment.MasterShipmentDataId equals db_Masters.Id into ShipmentsMasters
                                                   from myMaster in ShipmentsMasters.DefaultIfEmpty()

                                                   // Join Shipments --> FHLStatus
                                                   join db_FHLStatuses in shipmentContext.FHLStatus on myShipment.FHLStatusCode equals db_FHLStatuses.Code into ShipmentFHLStatuses
                                                   from myFHLStatus in ShipmentFHLStatuses.DefaultIfEmpty()

                                                   // Join Shipments --> FHLStatus (Cargonaut|DEXX)
                                                   join db_FHLCargonautStatuses in shipmentContext.FHLStatus on myShipment.CargonautFHLStatusCode equals db_FHLCargonautStatuses.Code into ShipmentFHLCargonautStatuses
                                                   from myFHLCargonautStatus in ShipmentFHLCargonautStatuses.DefaultIfEmpty()

                                                   // Join Shipments --> AWBStatus (Last Status)
                                                   join db_AWBLastStatuses in shipmentContext.AWBStatus on myShipment.CarrierLastStatusCode equals db_AWBLastStatuses.Code into ShipmentCarrierLastStatuses
                                                   from myLastStatus in ShipmentCarrierLastStatuses.DefaultIfEmpty()

                                                   // Join Shipments --> ShipmentComputedFields
                                                   //join db_ComputedFields in shipmentContext.ShipmentComputedFields on myShipment.Id equals db_ComputedFields.Id into myShipmentComputedFields
                                                   //from myShipmentComputedField in myShipmentComputedFields.DefaultIfEmpty()

                                                   // Join ShipmentMasterDatas --> FWBStatus
                                                   //join fwbStatuses in shipmentContext.FWBStatus on myMaster.FWBStatusCode equals fwbStatuses.Code into ShipmentFWBStatuses
                                                   //from myFWBStatus in ShipmentFWBStatuses.DefaultIfEmpty()

                                                   // Join ShipmentMasterDatas --> FWBStatus (Cargonaut|DEXX)
                                                   //join fwbCargonautStatuses in shipmentContext.FWBStatus on myMaster.CargonautFWBStatusCode equals fwbCargonautStatuses.Code into ShipmentFWBCargonautStatuses
                                                   //from myFWBCargonautStatus in ShipmentFWBCargonautStatuses.DefaultIfEmpty()



                                                   //join directions in allDirections on shipment.DirectionId equals directions.Id into ShipmentDirections
                                                   //from myMirection in ShipmentDirections.DefaultIfEmpty()

                                                   //join transportModes in freightContext.TransportModes on shipment.TransportModeId equals transportModes.Id into ShipmentTransportModes
                                                   //from myTransportMode in ShipmentTransportModes.DefaultIfEmpty()

                                                   where myShipment.Tenant == tenant

                                                   select new ShipmentList()
                                                   {
                                                       #region Shipment Fields
                                                       Id = myShipment.Id,
                                                       ShipmentViewId = myShipment.Id,
                                                       Tenant = myShipment.Tenant,
                                                       DirectionId = myShipment.DirectionId,
                                                       TransportModeId = myShipment.TransportModeId,
                                                       AccountedPayablesInLocalCurrency = myShipment.AccountedPayablesInLocalCurrency,
                                                       AccountedPayablesInProfitCurrency = myShipment.AccountedPayablesInProfitCurrency,
                                                       AccountedReceivablesInLocalCurrency = myShipment.AccountedReceivablesInLocalCurrency,
                                                       AccountedReceivablesInProfitCurrency = myShipment.AccountedReceivablesInProfitCurrency,
                                                       AccountManagerUserId = myShipment.AccountManagerUserId,
                                                       AccountNumber = myShipment.AccountNumber,
                                                       AgentId = myShipment.AgentId,
                                                       AgentReference1 = myShipment.AgentReference1,
                                                       AgentReference2 = myShipment.AgentReference2,
                                                       AMSBL = myShipment.AMSBL,
                                                       ARInvoiceIssued = myShipment.ARInvoiceIssued,
                                                       AsAgreedFreight = myShipment.AsAgreedFreight,
                                                       AsAgreedOtherCharges = myShipment.AsAgreedOtherCharges,
                                                       AWBPrint = myShipment.AWBPrint,
                                                       BookingNumberOfPackages = myShipment.BookingNumberOfPackages,
                                                       BranchId = myShipment.BranchId,
                                                       CASSCode = myShipment.CASSCode,
                                                       LocalCustomsTransmissionsStatusCode = myShipment.LocalCustomsTransmissionsStatusCode,
                                                       LocalCustomsTransmissionsStatusError = myShipment.LocalCustomsTransmissionsStatusError,
                                                       LocalCustomsTransmissionsStatusDate = myShipment.LocalCustomsTransmissionsStatusDate,
                                                       FHLStatusCode = myShipment.FHLStatusCode,
                                                       FHLStatusDate = myShipment.FHLStatusDate,
                                                       CargonautFHLStatusCode = myShipment.CargonautFHLStatusCode,
                                                       CargonautFHLStatusDate = myShipment.CargonautFHLStatusDate,
                                                       CarrierLastStatusCode = myShipment.CarrierLastStatusCode,
                                                       CarrierLastStatusDate = myShipment.CarrierLastStatusDate,
                                                       ChargeableWeight = myShipment.ChargeableWeight,
                                                       ChargeableWeightInKG = myShipment.ChargeableWeightInKG,
                                                       ChargeableWeightUnitCode = myShipment.ChargeableWeightUnitCode,
                                                       ConsigneeId = myShipment.ConsigneeId,
                                                       ConsigneeReference1 = myShipment.ConsigneeReference1,
                                                       ConsigneeReference2 = myShipment.ConsigneeReference2,
                                                       ConsolidatorAddressId = myShipment.ConsolidatorAddressId,
                                                       ConsolidatorContactId = myShipment.ConsolidatorContactId,
                                                       ConsolidatorId = myShipment.ConsolidatorId,
                                                       ConsolidatorReference = myShipment.ConsolidatorReference,
                                                       CreateDateTime = myShipment.CreateDateTime,
                                                       CreditNoteIssued = myShipment.CreditNoteIssued,
                                                       CustomerId = myShipment.CustomerId,
                                                       CustomerReference1 = myShipment.CustomerReference1,
                                                       CustomerReference2 = myShipment.CustomerReference2,
                                                       CustomerShipmentNumber = myShipment.CustomerShipmentNumber,
                                                       CustomFileId = myShipment.CustomFileId,
                                                       CustomFileNumber = myShipment.CustomFileNumber,
                                                       CustomsDeclarationNumber = myShipment.CustomsDeclarationNumber,
                         
                                                       DeliveryOrder = myShipment.DeliveryOrder,
                                                       DepartmentId = myShipment.DepartmentId,
                                                       EstimateProfitInLocalCurrency = myShipment.EstimateProfitInLocalCurrency,
                                                       EstimateProfitInProfitCurrency = myShipment.EstimateProfitInProfitCurrency,
                                                       ExceptionDate = myShipment.ExceptionDate,
                                                       ExceptionDescription = myShipment.ExceptionDescription,
                                                       ExceptionResolvedDescription = myShipment.ExceptionResolvedDescription,
                                                       LastExceptionDescription = myShipment.LastExceptionDescription,
                                                       Field1 = myShipment.Field1,
                                                       Field2 = myShipment.Field2,
                                                       Field3 = myShipment.Field3,
                                                       Field4 = myShipment.Field4,
                                                       Field5 = myShipment.Field5,
                                                       Field6 = myShipment.Field6,
                                                       Field7 = myShipment.Field7,
                                                       Field8 = myShipment.Field8,
                                                       Field9 = myShipment.Field9,
                                                       Field10 = myShipment.Field10,
                                                       FinalArrivalDate = myShipment.FinalArrivalDate,
                                                       FNAReason = myShipment.FNAReason,
                                                       ForwarderShipmentNumber = myShipment.ForwarderShipmentNumber,
                                                       FreelancerAddressId = myShipment.FreelancerAddressId,
                                                       FreelancerContactId = myShipment.FreelancerContactId,
                                                       FreelancerId = myShipment.FreelancerId,
                                                       FreightForwarderId = myShipment.FreightForwarderId,
                                                       FreightLocationId = myShipment.FreightLocationId,
                                                       FromPortId = myShipment.FromPortId,
                                                       GrossWeight = myShipment.GrossWeight,
                                                       GrossWeightInKG = myShipment.GrossWeightInKG,
                                                       House = myShipment.House,
                                                       IncotermId = myShipment.IncotermId,
                                                       IsAccountingClosed = myShipment.IsAccountingClosed,
                                                       IsFSRSent = myShipment.IsFSRSent,
                                                       IsMultipleCommodities = myShipment.IsMultipleCommodities,
                                                       IsOperationalClosed = myShipment.IsOperationalClosed,
                                                       OperationalCloseDate = myShipment.OperationalCloseDate,
                                                       AccountingCloseDate = myShipment.AccountingCloseDate,
                                                       IssuingCarrierAgentId = myShipment.IssuingCarrierAgentId,
                                                       LastFSRStatusRequestDate = myShipment.LastFSRStatusRequestDate,
                                                       LastStatusLogDate = myShipment.LastStatusLogDate,
                                                       LastUpdate = myShipment.LastUpdateDate,
                                                       LastUpdateDate = myShipment.LastUpdateDate,
                                                       MasterShipmentDataId = myShipment.MasterShipmentDataId,
                                                       NextETA = myShipment.NextETA,
                                                       NextETD = myShipment.NextETD,
                                                       NextLegCode = myShipment.NextLegCode,
                                                       NumberOfContainers = myShipment.NumberOfContainers,
                                                       NumberOfInsidePackages = myShipment.NumberOfInsidePackages,
                                                       NumberOfInsidePackagesDetails = myShipment.NumberOfInsidePackagesDetails,
                                                       NumberOfPackages = myShipment.NumberOfPackages,
                                                       OpenPayablesInLocalCurrency = myShipment.OpenPayablesInLocalCurrency,
                                                       OpenPayablesInProfitCurrency = myShipment.OpenPayablesInProfitCurrency,
                                                       OpenReceivablesInLocalCurrency = myShipment.OpenReceivablesInLocalCurrency,
                                                       OpenReceivablesInProfitCurrency = myShipment.OpenReceivablesInProfitCurrency,
                                                       OrderChargeableWeight = myShipment.OrderChargeableWeight,
                                                       OrderVolumetricWeight = myShipment.OrderVolumetricWeight,
                                                       PackagesQuantity = myShipment.PackagesQuantity,
                                                       PreCarriageETD = myShipment.PreCarriageETD,
                                                       ProductCode = myShipment.ProductCode,
                                                       ProfitExchangeRate = myShipment.ProfitExchangeRate,
                                                       ProfitInLocalCurrency = myShipment.ProfitInLocalCurrency,
                                                       ProfitInProfitCurrency = myShipment.ProfitInProfitCurrency,
                                                       QuoteId = myShipment.QuoteId,
                                                       Routing = myShipment.Routing,
                                                       SalesmanUserId = myShipment.SalesmanUserId,
                                                       SearchFields = myShipment.SearchFields,
                                                       ShipmentLevelCode = myShipment.ShipmentLevelCode,
                                                       ShipmentNumber = myShipment.ShipmentNumber,
                                                       ShipmentPayableStatusCode = myShipment.ShipmentPayableStatusCode,
                                                       ShipmentReceivableStatusCode = myShipment.ShipmentReceivableStatusCode,
                                                       ShipperId = myShipment.ShipperId,
                                                       ShipperReference1 = myShipment.ShipperReference1,
                                                       ShipperReference2 = myShipment.ShipperReference2,
                                                       SpecialServicesTypeId = myShipment.SpecialServicesTypeId,
                                                       StatusDate = myShipment.StatusDate,
                                                       StatusId = myShipment.StatusId,
                                                       TEU = myShipment.TEU,
                                                       ToPortId = myShipment.ToPortId,
                                                       TransportDocumentNumber = myShipment.TransportDocumentNumber,
                                                       UpdatedByUserId = myShipment.UpdatedByUserId,
                                                       VolumeInCBM = myShipment.VolumeInCBM,
                                                       VolumetricWeight = myShipment.VolumetricWeight,
                                                       NumberOfFollowUps = myShipment.NumberOfFollowUps,
                                                       ArchivedText = myShipment.IsOperationalClosed ? "Archived" : "",
                                                       #endregion

                                                       #region Master Fields
                                                       CutoffDate = myMaster.CutoffDate,
                                                       AirlinePrefix = myMaster.AirlinePrefix,
                                                       MainCarriageATA = myMaster.MainCarriageATA,
                                                       FWBStatusCode = myMaster.FWBStatusCode,
                                                       FWBStatusDate = myMaster.FWBStatusDate,
                                                       CargonautFWBStatusCode = myMaster.CargonautFWBStatusCode,
                                                       CargonautFWBStatusDate = myMaster.CargonautFWBStatusDate,
                                                       ImportManifest = myMaster.ImportManifest,
                                                       MainCarriageATD = myMaster.MainCarriageATD,
                                                       MainCarriageCarrierId = myMaster.MainCarriageCarrierId,
                                                       MainCarriageCarrierNumber = myMaster.MainCarriageCarrierNumber,
                                                       MainCarriageCarrierPrefix = myMaster.MainCarriageCarrierPrefix,
                                                       MainCarriageETA = myMaster.MainCarriageETA,
                                                       MainCarriageETD = myMaster.MainCarriageETD,
                                                       MainCarriageFromAddressId = myMaster.MainCarriageFromAddressId,
                                                       MainCarriageFromPartnerId = myMaster.MainCarriageFromPartnerId,
                                                       MainCarriageFromPortId = myMaster.MainCarriageFromPortId,
                                                       MainCarriageToAddressId = myMaster.MainCarriageToAddressId,
                                                       MainCarriageToPartnerId = myMaster.MainCarriageToPartnerId,
                                                       ManifestReason = myMaster.ManifestReason,
                                                       ManifestStatusCode = myMaster.ManifestStatusCode,
                                                       Master = myMaster.Master,
                                                       MasterShipmentNumber = myMaster.MasterShipmentNumber,
                                                       TruckNumber = myMaster.TruckNumber,
                                                       Transshipment1CarrierPrefix = myMaster.Transshipment1CarrierPrefix,
                                                       Transshipment2CarrierPrefix = myMaster.Transshipment2CarrierPrefix,
                                                       Transshipment3CarrierPrefix = myMaster.Transshipment3CarrierPrefix,
                                                       LongMaster = myShipment.TransportModeId == "A" ? (myMaster.AirlinePrefix != null && myMaster.Master != null ? myMaster.AirlinePrefix + "-" + myMaster.Master : myMaster.Master) : myMaster.Master,
                                                       #endregion

                                                       CarrierLastStatusName = myLastStatus.Name,
                                                       FHLStatusName = myFHLStatus.Name,
                                                       CargonautFHLStatusName = myFHLCargonautStatus.Name,
                                                       //DocumentsSearchFields = myShipmentComputedField.DocumentsSearchFields,
                                                       //IsMissingDocument = myShipmentComputedField.IsMissingDocuments,

                                                       //FWBStatusName = myFWBStatus.Name,
                                                       // CargonautFWBStatusName = myFWBCargonautStatus.Name,



                                                       // un-known
                                                       hasChanges = false,
                                                       HasException = false,
                                                       IsAnyConversation = false,
                                                       IsException = false,
                                                       IsOccurChange = false,
                                                       IsShipmentTracking = false,
                                                       LastModified = null,
                                                       NewMessage = false,
                                                       NumberOfShipments = 0,
                                                       ResultFromDocument = false,

                                                       // maybe Not uses
                                                       Field1Id = "",
                                                       CustomFieldId = "",
                                                       BasketId = "",
                                                       ActivityDate = null,
                                                       ActivityByUserName = "",
                                                       ActivityTypeName = "",
                                                       AWBChargeAmount = null,
                                                       AWBCommodityItemNumber = "",
                                                       CarrierTransportDocumentNumber = "",
                                                       FollowUpDate = null,
                                                       FollowUpId = "",
                                                       FollowUpNotes = "",
                                                       FollowUpOwnerId = "",
                                                       FollowUpTypeId = "",
                                                       FollowUpOwner = "",
                                                       FollowUpType = "",
                                                       MainCarriageFinalDestinationATA = null,
                                                       MainCarriageFinalDestinationETA = null,
                                                       Transshipment1FullCarrierNumber = "",
                                                       Transshipment2FullCarrierNumber = "",
                                                       Transshipment3FullCarrierNumber = "",

                                                       // Other Context                                                       
                                                       Consignee = "",
                                                       ConsigneeName = "",
                                                       ConsolidatorName = "",
                                                       CreatedByUserName = "",
                                                       CustomerName = "",
                                                       ConsolidatorNote = "",
                                                       CarrierNumber = "", // multi fields
                                                       BranchName = "", //branch
                                                       AgentName = "",    //card
                                                       AWBCurrencyCode = "",  //currency
                                                       AccountManagerUserName = "",   //user
                                                       DirectionName = "", //direction
                                                       TransportModeName = "", //TransportMode


                                                       FreightForwarderName = "",
                                                       FromCountryCode = "",
                                                       FromPort = "",
                                                       FromPortCountry = "",
                                                       FromPortCountryCode = "",
                                                       FromPortCountryName = "",
                                                       FromPortName = "",
                                                       IncotermCode = "",
                                                       LocalCurrencyCode = "",
                                                       MainCarriageFromCity = "",
                                                       MainCarriageCarrierCode = "",
                                                       MainCarriageCarrierName = "",
                                                       MainCarriageFromCountryCode = "",
                                                       MainCarriageFromPortName = "",
                                                       MainCarriageFullCarrierNumber = "",
                                                       MainCarriageToCity = "",
                                                       MainCarriageToCountryCode = "",
                                                       ProfitCurrencyCode = "",
                                                       SalesmanUserName = "",
                                                       ShipmentLevelName = "",
                                                       ShipmentType = "",
                                                       ShipmentPayableStatusName = "",
                                                       ShipmentReceivableStatusName = "",
                                                       Shipper = "",
                                                       ShipperName = "",
                                                       SpecialServicesTypeName = "",
                                                       StatusName = "",
                                                       ToCountryCode = "",
                                                       ToPort = "",
                                                       ToPortCountry = "",
                                                       ToPortCountryCode = "",
                                                       ToPortCountryName = "",
                                                       ToPortName = "",
                                                       NextLegName = "",
                                                   });
            #endregion

            return iQueryable;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ShipmentList> GetShipmentFullTextSearch(byte[] xmlFilters, int tenant)
        {
            List<ShipmentList> myResult = new List<ShipmentList>();

            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            int skip = queryOperations.PageIndex;
            int take = queryOperations.PageSize;
            if (take == 0)
            {
                take = 10;
            }

            #region Restrictions region
            AddRestrictionFilters(queryOperations, "Shipment", tenant);
            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);

            if (SecurityUtility.CheckTableContactFeature("User", "PRODUCTS", tenant))
            {
                ProductPermitionsFilter.AddUserProductRestrictionFilters(queryOperations, tenant);
            }
            #endregion

            string mySearchFields = null;
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@Tenant", tenant));
            string Where = "Where Shipments.Tenant = @Tenant";

            foreach (QueryFilterItem item in queryOperations.QueryFilterItems)
            {
                if (item.FieldValue != null)
                {
                    switch (item.FieldName)
                    {
                        case "BranchId":
                            {
                                parameters.Add(new SqlParameter("@BranchId", item.FieldValue));
                                Where += " and Shipments.BranchId = @BranchId";
                                break;
                            }

                        case "ProductCode":
                            {
                                parameters.Add(new SqlParameter("@ProductCode", item.FieldValue));
                                Where += " and Shipments.ProductCode = @ProductCode";
                                break;
                            }

                        case "CustomerId":
                            {
                                parameters.Add(new SqlParameter("@CustomerId", item.FieldValue));
                                Where += " and Shipments.CustomerId = @CustomerId";
                                break;
                            }

                        case "DirectionId":
                            {
                                parameters.Add(new SqlParameter("@DirectionId", item.FieldValue));
                                Where += " and Shipments.DirectionId = @DirectionId";
                                break;
                            }

                        case "TransportModeId":
                            {
                                parameters.Add(new SqlParameter("@TransportModeId", item.FieldValue));
                                Where += " and Shipments.TransportModeId = @TransportModeId";
                                break;
                            }

                        case "SearchFields":
                            {
                                mySearchFields = item.FieldValue.ToString();
                                parameters.Add(new SqlParameter("@SearchFields", "\"" + item.FieldValue + "*\""));
                                //Where += " and Contains(Shipments.SearchFields,@SearchFields)";
                                Where += " and Shipments.Id in (SELECT top 10 Shipments.Id FROM Shipments Where Shipments.Tenant = @Tenant and Contains(Shipments.SearchFields, @SearchFields))";
                                break;
                            }
                    }
                }
            }

            //TenantQuery tenantQuery = new TenantQuery(tenant);
            //TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);

            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            //ContactRepository contactRepository = new ContactRepository(tenant);

            IShipmentsContext context = ShipmentsContext.GetContext(tenant);
            ShipmentsContext activeContext = context.GetActiveDbContext() as ShipmentsContext;

            IQueryable<OutlookShipmentView> shipments = activeContext.Database.SqlQuery<OutlookShipmentView>("WITH OutlookShipmentView AS(SELECT top 10 Shipments.Id,Shipments.CreateDateTime, Shipments.SearchFields, Shipments.Tenant, Shipments.ShipmentNumber,Shipments.CustomerId,Shipments.Routing,ShipmentMasterDatas.AirlinePrefix, Shipments.AccountManagerUserId,Shipments.IsOperationalClosed,ShipmentMasterDatas.Master,Shipments.DirectionId, Shipments.TransportModeId ,CustomerCards.EnglishName AS CustomerName FROM Shipments LEFT OUTER JOIN ShipmentMasterDatas ON ShipmentMasterDatas.Id = Shipments.MasterShipmentDataId LEFT OUTER JOIN  Cards AS CustomerCards ON Shipments.CustomerId = CustomerCards.Id " + Where + " ) select * from OutlookShipmentView", parameters.ToArray()).AsQueryable();

            myResult = (from f in shipments
                        select new ShipmentList()
                        {
                            Id = f.Id,
                            ShipmentViewId = f.Id,
                            DirectionId = f.DirectionId,
                            DirectionName = f.DirectionId == "I" ? "Import" : "Export",
                            TransportModeName = f.TransportModeId == "I" ? "Inland" : f.TransportModeId == "A" ? "Air" : "Ocean",
                            ShipmentNumber = f.ShipmentNumber,
                            TransportModeId = f.TransportModeId,
                            Routing = f.Routing,
                            CustomerName = f.CustomerName,
                            LongMaster = f.TransportModeId == "A" ? (!string.IsNullOrEmpty(f.AirlinePrefix) && !string.IsNullOrEmpty(f.Master) ? f.AirlinePrefix + "-" + f.Master : "") : f.Master,
                            SearchFields = f.SearchFields,
                            CustomerId = f.CustomerId,
                        }).ToList();

            //if (!string.IsNullOrEmpty(mySearchFields))
            //{
            //    foreach (var list in myResult)
            //    {
            //        list.SearchFieldsText = SearchFieldsFinder.Find(list.SearchFields, mySearchFields);
            //    }
            //}

            return myResult.AsQueryable();

            //SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            //shipmentRepository = new ShipmentRepository(tenant);

            ////TenantQuery tenantQuery = new TenantQuery(tenant);
            ////TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);

            //MemoryStream memorystream = new MemoryStream(xmlFilters);
            //XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            //QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            //GenericFilter filter = new GenericFilter();
            //GenericSort sortClass = new GenericSort();

            //#region Restrictions region
            //AddRestrictionFilters(queryOperations, "Shipment", tenant);
            //BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);

            //if (SecurityUtility.CheckTableContactFeature("User", "PRODUCTS", tenant))
            //{
            //    ProductPermitionsFilter.AddUserProductRestrictionFilters(queryOperations, tenant);
            //}
            //#endregion
            ///////////////////////////////////////////////////////////
            //string SearchFilterAsWhere = "";
            //List<SqlParameter> parameters = new List<SqlParameter>();
            //var SearchFilter = queryOperations.QueryFilterItems.Where(a => a.FieldName == "SearchFields").FirstOrDefault();
            //if (SearchFilter != null)
            //{
            //    SearchFilterAsWhere = "Id in (SELECT Id FROM Shipments Where Tenant = @p__linq__0 and Contains(SearchFields,@SearchFields))";
            //    parameters.Add(new SqlParameter("@SearchFields", "\"" + SearchFilter.FieldValue + "*\""));
            //    queryOperations.QueryFilterItems.Remove(SearchFilter);
            //}
            /////////////////////////////////////////////////////////////
            //ShipmentCustomFilter customfilters = new ShipmentCustomFilter(tenant);
            //IQueryable<ShipmentDataView> shipments = shipmentRepository.GetShipmentViewsByTenant(tenant);
            //shipments = customfilters.GetFilteredQuery(queryOperations, shipments);

            //QueryOperations nonListQueryOperation = new QueryOperations();
            //nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            //QueryOperations listQueryOperation = new QueryOperations();
            //listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            //var dddddd = Thread.CurrentPrincipal;

            //shipments = filter.GetFilteredQuery<ShipmentDataView>(nonListQueryOperation, shipments);
            //int skippedShipments = queryOperations.PageIndex;

            //ShipmentQuery myShipmentQuery = new ShipmentQuery(shipmentRepository);

            //var query2 = myShipmentQuery.GetIQueryableShipmentList(shipments, tenant);

            //query2 = filter.GetFilteredQuery<ShipmentList>(listQueryOperation, query2);

            //if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            //{
            //    PropertyInfo propInfo = typeof(ShipmentList).GetProperty(queryOperations.SortByColumnName);
            //    List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Shipment", tenant).ToList();

            //    ObjectField objectField = (from a in shipmentObjectFields
            //                               where a.FieldName == queryOperations.SortByColumnName
            //                               select a).FirstOrDefault();

            //    if (objectField != null)
            //    {
            //        if (!objectField.IsCustom)
            //        {
            //            switch (objectField.DataTypeCode.ToLower())
            //            {
            //                case "text":
            //                    {
            //                        query2 = sortClass.GetSorterQuery<ShipmentList, string>(queryOperations, query2);
            //                        break;
            //                    }
            //                case "double":
            //                    {
            //                        query2 = sortClass.GetSorterQuery<ShipmentList, double>(queryOperations, query2);
            //                        break;
            //                    }
            //                case "datetime":
            //                    {
            //                        query2 = sortClass.GetSorterQuery<ShipmentList, DateTime>(queryOperations, query2);
            //                        break;
            //                    }
            //                case "integer":
            //                    {
            //                        query2 = sortClass.GetSorterQuery<ShipmentList, int>(queryOperations, query2);
            //                        break;
            //                    }
            //                case "lookup":
            //                    {
            //                        query2 = sortClass.GetSorterQuery<ShipmentList, string>(queryOperations, query2);
            //                        break;
            //                    }
            //                case "boolean":
            //                    {
            //                        query2 = sortClass.GetSorterQuery<ShipmentList, bool>(queryOperations, query2);
            //                        break;
            //                    }
            //                default:
            //                    {
            //                        query2 = query2.OrderByDescending(d => d.CreateDateTime);
            //                        break;
            //                    }
            //            }
            //        }
            //        else
            //        {
            //            query2 = sortClass.GetSorterQuery<ShipmentList, string>(queryOperations, query2);
            //        }
            //    }
            //}
            //else
            //{
            //    query2 = query2.OrderByDescending(d => d.CreateDateTime);
            //}

            //query2 = query2.Skip(skippedShipments);
            //query2 = query2.Take(queryOperations.PageSize);

            //List<ShipmentList> listQuery;// = query2.ToList();
            //TraceStringValues MySql;
            //MySql = IQueryableExtensions.ToTraceString<ShipmentList>(query2);//.ToString().Replace("\r\n", "").ToLower();
            //if (!string.IsNullOrEmpty(SearchFilterAsWhere) && MySql.TSQL.ToLower().Contains("where"))
            //{
            //    var regex = new Regex(Regex.Escape("WHERE"), RegexOptions.IgnoreCase);
            //    MySql.TSQL = regex.Replace(MySql.TSQL, "WHERE " + SearchFilterAsWhere + " AND ", 1);

            //    var regex1 = new Regex(Regex.Escape("WHERE [Project1].[row_number] > "), RegexOptions.IgnoreCase);
            //    MySql.TSQL = regex1.Replace(MySql.TSQL, "WHERE [Project1].[row_number] > 0 --", 1);
            //    //MySql = MySql.Replace("where ", SearchFilterAsWhere + " and ");
            //}
            ////parameters.Concat();
            //foreach (var item in MySql.TSQLParams)
            //{
            //    parameters.Add(new SqlParameter(item.Name, item.Value));
            //}
            //IShipmentsContext context = ShipmentsContext.GetContext(tenant);
            //ShipmentsContext activeContext = context.GetActiveDbContext() as ShipmentsContext; 
            //listQuery = activeContext.Database.SqlQuery<ShipmentList>(MySql.TSQL, parameters.ToArray()).ToList();

            //CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            //customFieldResolver.SetCustomFieldsValues("Shipment", tenant, listQuery.Cast<object>().ToList());

            //return listQuery.AsQueryable();
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ShipmentList> GetShipmentFilters(byte[] xmlFilters, int tenant)
        {
            //using (TransactionScope scope = TransactionFactory.GetTransaction())
            //{
            //var user = HttpContext.Current.User;
            //var threadUser = Thread.CurrentPrincipal;
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            shipmentRepository = new ShipmentRepository(tenant);

            //TenantQuery tenantQuery = new TenantQuery(tenant);
            //TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            #region Restrictions region
            AddRestrictionFilters(queryOperations, "Shipment", tenant);
            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);

            if (SecurityUtility.CheckTableContactFeature("User", "PRODUCTS", tenant))
            {
                ProductPermitionsFilter.AddUserProductRestrictionFilters(queryOperations, tenant);
            }
            #endregion

            ShipmentCustomFilter customfilters = new ShipmentCustomFilter(tenant);
            IQueryable<ShipmentDataView> shipments = shipmentRepository.GetShipmentViewsByTenant(tenant);
            var MySearchFilter = queryOperations.QueryFilterItems.Where(a => a.FieldName == "SearchFields").FirstOrDefault();

            if (MySearchFilter != null)
            {
                var SearchTerm = MySearchFilter.FieldValue.ToString();
                shipments = shipments.Where(a => a.SearchFields.Contains(SearchTerm));
                queryOperations.QueryFilterItems.Remove(MySearchFilter);
            }
            shipments = customfilters.GetFilteredQuery(queryOperations, shipments);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            var dddddd = Thread.CurrentPrincipal;

            shipments = filter.GetFilteredQuery<ShipmentDataView>(nonListQueryOperation, shipments);
            int skippedShipments = queryOperations.PageIndex;

            ShipmentQuery myShipmentQuery = new ShipmentQuery(shipmentRepository);

            var query2 = myShipmentQuery.GetIQueryableShipmentList(shipments, tenant);

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
                query2 = query2.OrderByDescending(d => d.CreateDateTime);
            }

            query2 = System.Data.Entity.QueryableExtensions.Skip(query2, () => skippedShipments);
            query2 = System.Data.Entity.QueryableExtensions.Take(query2, () => queryOperations.PageSize);
            //query2 = query2.Skip(skippedShipments);
            //query2 = query2.Take(queryOperations.PageSize);

            List<ShipmentList> listQuery = query2.ToList();

            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            customFieldResolver.SetCustomFieldsValues("Shipment", tenant, listQuery.Cast<object>().ToList());

            return listQuery.AsQueryable();
            //}
        }

        public List<ShipmentList> GetRecentActivityShipments(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);
            shipmentQuery = new ShipmentQuery(tenant);
            string mail = SecurityUtility.GetAuthenticatedUser();
            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM contact = contactQuery.GetContactByEmailOnly(mail, tenant);
            ObjectTableRepository objecttableRepository = new ObjectTableRepository(tenant);
            ObjectTable objecttable = objecttableRepository.GetObjectTableByName("Shipment", 0, true);
            IQueryable<ShipmentList> first = shipmentQuery.GetLastActivityShipments(tenant, contact.Id, objecttable.Id).AsQueryable();
            IQueryable<ShipmentList> list = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), first, tenant);

            if (SecurityUtility.CheckTableContactFeature("User", "PRODUCTS", tenant))
            {
                list = ProductPermitionsFilter.AddUserProductRestrictionFilters(new QueryOperations(), first, tenant);
            }

            return list.ToList();
        }

        public InvoiceEntityFields GetInvoiceEntityFields(string entityId, int tenant)
        {
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            shipmentQuery = new ShipmentQuery(tenant);
            InvoiceEntityFields result = shipmentQuery.GetInvoiceEntityFields(entityId, tenant);
            return result;
        }

        public int GetShipmentFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            shipmentRepository = new ShipmentRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            #region Restrictions region
            AddRestrictionFilters(queryOperations, "Shipment", tenant);
            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);

            if (SecurityUtility.CheckTableContactFeature("User", "PRODUCTS", tenant))
            {
                ProductPermitionsFilter.AddUserProductRestrictionFilters(queryOperations, tenant);
            }
            #endregion

            GenericFilter filter = new GenericFilter();
            ShipmentCustomFilter customfilters = new ShipmentCustomFilter(tenant);
            IQueryable<ShipmentDataView> shipments = shipmentRepository.GetShipmentViewsByTenant(tenant);
            shipments = customfilters.GetFilteredQuery(queryOperations, shipments);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            shipments = filter.GetFilteredQuery<ShipmentDataView>(nonListQueryOperation, shipments);
            int skippedShipments = queryOperations.PageIndex;


            ShipmentQuery myShipmentQuery = new ShipmentQuery(shipmentRepository);

            var query2 = myShipmentQuery.GetIQueryableShipmentList(shipments, tenant);

            query2 = filter.GetFilteredQuery<ShipmentList>(listQueryOperation, query2);

            int count;
            if (queryOperations.GetAll)
            {
                count = query2.Count();
            }
            else
            {
                count = query2.Take(1001).Count();
            }

            return count;
        }

        public int GetShipmentFiltersCount_00(byte[] xmlFilters, int tenant)
        {
            int myResult = 0;
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            shipmentRepository = new ShipmentRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            #region Restrictions region
            AddRestrictionFilters(queryOperations, "Shipment", tenant);
            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);

            if (SecurityUtility.CheckTableContactFeature("User", "PRODUCTS", tenant))
            {
                ProductPermitionsFilter.AddUserProductRestrictionFilters(queryOperations, tenant);
            }
            #endregion

            GenericFilter filter = new GenericFilter();
            ShipmentCustomFilter customfilters = new ShipmentCustomFilter(tenant);

            IQueryable<ShipmentList> iQueryableData = this.GetJoinedShipmentLists(tenant);
            iQueryableData = customfilters.GetFilteredQuery(queryOperations, iQueryableData);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryableData = filter.GetFilteredQuery<ShipmentList>(nonListQueryOperation, iQueryableData);
            int skippedShipments = queryOperations.PageIndex;

            iQueryableData = filter.GetFilteredQuery<ShipmentList>(listQueryOperation, iQueryableData);
            myResult = iQueryableData.Count();
            return myResult;
        }

        private IQueryable<ShipmentList> GetJoinedShipmentLists(int tenant)
        {
            var shipmentContext = ShipmentsContext.GetContext(tenant);

            IQueryable<ShipmentList> iQueryable = (from myShipment in shipmentContext.Shipments

                                                   // Join Shipments --> ShipmentMasterDatas
                                                   join db_Masters in shipmentContext.ShipmentMasterDatas on myShipment.MasterShipmentDataId equals db_Masters.Id into ShipmentsMasters
                                                   from myMaster in ShipmentsMasters.DefaultIfEmpty()

                                                   // Join Shipments --> FHLStatus
                                                   join db_FHLStatuses in shipmentContext.FHLStatus on myShipment.FHLStatusCode equals db_FHLStatuses.Code into ShipmentFHLStatuses
                                                   from myFHLStatus in ShipmentFHLStatuses.DefaultIfEmpty()

                                                   // Join Shipments --> FHLStatus (Cargonaut|DEXX)
                                                   join db_FHLCargonautStatuses in shipmentContext.FHLStatus on myShipment.CargonautFHLStatusCode equals db_FHLCargonautStatuses.Code into ShipmentFHLCargonautStatuses
                                                   from myFHLCargonautStatus in ShipmentFHLCargonautStatuses.DefaultIfEmpty()

                                                   // Join Shipments --> AWBStatus (Last Status)
                                                   join db_AWBLastStatuses in shipmentContext.AWBStatus on myShipment.CarrierLastStatusCode equals db_AWBLastStatuses.Code into ShipmentCarrierLastStatuses
                                                   from myLastStatus in ShipmentCarrierLastStatuses.DefaultIfEmpty()

                                                   // Join Shipments --> ShipmentComputedFields
                                                   //join db_ComputedFields in shipmentContext.ShipmentComputedFields on myShipment.Id equals db_ComputedFields.Id into myShipmentComputedFields
                                                   //from myShipmentComputedField in myShipmentComputedFields.DefaultIfEmpty()

                                                   // Join ShipmentMasterDatas --> FWBStatus
                                                   join fwbStatuses in shipmentContext.FWBStatus on myMaster.FWBStatusCode equals fwbStatuses.Code into ShipmentFWBStatuses
                                                   from myFWBStatus in ShipmentFWBStatuses.DefaultIfEmpty()

                                                   // Join ShipmentMasterDatas --> FWBStatus (Cargonaut|DEXX)
                                                   join fwbCargonautStatuses in shipmentContext.FWBStatus on myMaster.CargonautFWBStatusCode equals fwbCargonautStatuses.Code into ShipmentFWBCargonautStatuses
                                                   from myFWBCargonautStatus in ShipmentFWBCargonautStatuses.DefaultIfEmpty()


                                                   //join directions in allDirections on shipment.DirectionId equals directions.Id into ShipmentDirections
                                                   //from myMirection in ShipmentDirections.DefaultIfEmpty()

                                                   //join transportModes in freightContext.TransportModes on shipment.TransportModeId equals transportModes.Id into ShipmentTransportModes
                                                   //from myTransportMode in ShipmentTransportModes.DefaultIfEmpty()

                                                   where myShipment.Tenant == tenant

                                                   select new ShipmentList()
                                                   {
                                                       #region Shipment Fields
                                                       Id = myShipment.Id,
                                                       ShipmentViewId = myShipment.Id,
                                                       Tenant = myShipment.Tenant,
                                                       DirectionId = myShipment.DirectionId,
                                                       TransportModeId = myShipment.TransportModeId,
                                                       AccountedPayablesInLocalCurrency = myShipment.AccountedPayablesInLocalCurrency,
                                                       AccountedPayablesInProfitCurrency = myShipment.AccountedPayablesInProfitCurrency,
                                                       AccountedReceivablesInLocalCurrency = myShipment.AccountedReceivablesInLocalCurrency,
                                                       AccountedReceivablesInProfitCurrency = myShipment.AccountedReceivablesInProfitCurrency,
                                                       AccountManagerUserId = myShipment.AccountManagerUserId,
                                                       AccountNumber = myShipment.AccountNumber,
                                                       AgentId = myShipment.AgentId,
                                                       AgentReference1 = myShipment.AgentReference1,
                                                       AgentReference2 = myShipment.AgentReference2,
                                                       AMSBL = myShipment.AMSBL,
                                                       ARInvoiceIssued = myShipment.ARInvoiceIssued,
                                                       AsAgreedFreight = myShipment.AsAgreedFreight,
                                                       AsAgreedOtherCharges = myShipment.AsAgreedOtherCharges,
                                                       AWBPrint = myShipment.AWBPrint,
                                                       BookingNumberOfPackages = myShipment.BookingNumberOfPackages,
                                                       BranchId = myShipment.BranchId,
                                                       CASSCode = myShipment.CASSCode,
                                                       LocalCustomsTransmissionsStatusCode = myShipment.LocalCustomsTransmissionsStatusCode,
                                                       LocalCustomsTransmissionsStatusError = myShipment.LocalCustomsTransmissionsStatusError,
                                                       LocalCustomsTransmissionsStatusDate = myShipment.LocalCustomsTransmissionsStatusDate,
                                                       FHLStatusCode = myShipment.FHLStatusCode,
                                                       FHLStatusDate = myShipment.FHLStatusDate,
                                                       CargonautFHLStatusCode = myShipment.CargonautFHLStatusCode,
                                                       CargonautFHLStatusDate = myShipment.CargonautFHLStatusDate,
                                                       CarrierLastStatusCode = myShipment.CarrierLastStatusCode,
                                                       CarrierLastStatusDate = myShipment.CarrierLastStatusDate,
                                                       ChargeableWeight = myShipment.ChargeableWeight,
                                                       ChargeableWeightInKG = myShipment.ChargeableWeightInKG,
                                                       ChargeableWeightUnitCode = myShipment.ChargeableWeightUnitCode,
                                                       ConsigneeId = myShipment.ConsigneeId,
                                                       ConsigneeReference1 = myShipment.ConsigneeReference1,
                                                       ConsigneeReference2 = myShipment.ConsigneeReference2,
                                                       ConsolidatorAddressId = myShipment.ConsolidatorAddressId,
                                                       ConsolidatorContactId = myShipment.ConsolidatorContactId,
                                                       ConsolidatorId = myShipment.ConsolidatorId,
                                                       ConsolidatorReference = myShipment.ConsolidatorReference,
                                                       CreateDateTime = myShipment.CreateDateTime,
                                                       CreditNoteIssued = myShipment.CreditNoteIssued,
                                                       CustomerId = myShipment.CustomerId,
                                                       CustomerReference1 = myShipment.CustomerReference1,
                                                       CustomerReference2 = myShipment.CustomerReference2,
                                                       CustomerShipmentNumber = myShipment.CustomerShipmentNumber,
                                                       CustomFileId = myShipment.CustomFileId,
                                                       CustomFileNumber = myShipment.CustomFileNumber,
                                                       CustomsDeclarationNumber = myShipment.CustomsDeclarationNumber,
                                                    
                                                       DeliveryOrder = myShipment.DeliveryOrder,
                                                       DepartmentId = myShipment.DepartmentId,
                                                       EstimateProfitInLocalCurrency = myShipment.EstimateProfitInLocalCurrency,
                                                       EstimateProfitInProfitCurrency = myShipment.EstimateProfitInProfitCurrency,
                                                       ExceptionDate = myShipment.ExceptionDate,
                                                       ExceptionDescription = myShipment.ExceptionDescription,
                                                       ExceptionResolvedDescription = myShipment.ExceptionResolvedDescription,
                                                       LastExceptionDescription = myShipment.LastExceptionDescription,
                                                       Field1 = myShipment.Field1,
                                                       Field2 = myShipment.Field2,
                                                       Field3 = myShipment.Field3,
                                                       Field4 = myShipment.Field4,
                                                       Field5 = myShipment.Field5,
                                                       Field6 = myShipment.Field6,
                                                       Field7 = myShipment.Field7,
                                                       Field8 = myShipment.Field8,
                                                       Field9 = myShipment.Field9,
                                                       Field10 = myShipment.Field10,
                                                       FinalArrivalDate = myShipment.FinalArrivalDate,
                                                       FNAReason = myShipment.FNAReason,
                                                       ForwarderShipmentNumber = myShipment.ForwarderShipmentNumber,
                                                       FreelancerAddressId = myShipment.FreelancerAddressId,
                                                       FreelancerContactId = myShipment.FreelancerContactId,
                                                       FreelancerId = myShipment.FreelancerId,
                                                       FreightForwarderId = myShipment.FreightForwarderId,
                                                       FreightLocationId = myShipment.FreightLocationId,
                                                       FromPortId = myShipment.FromPortId,
                                                       GrossWeight = myShipment.GrossWeight,
                                                       GrossWeightInKG = myShipment.GrossWeightInKG,
                                                       House = myShipment.House,
                                                       IncotermId = myShipment.IncotermId,
                                                       IsAccountingClosed = myShipment.IsAccountingClosed,
                                                       IsFSRSent = myShipment.IsFSRSent,
                                                       IsMultipleCommodities = myShipment.IsMultipleCommodities,
                                                       IsOperationalClosed = myShipment.IsOperationalClosed,
                                                       OperationalCloseDate = myShipment.OperationalCloseDate,
                                                       AccountingCloseDate = myShipment.AccountingCloseDate,
                                                       IssuingCarrierAgentId = myShipment.IssuingCarrierAgentId,
                                                       LastFSRStatusRequestDate = myShipment.LastFSRStatusRequestDate,
                                                       LastStatusLogDate = myShipment.LastStatusLogDate,
                                                       LastUpdate = myShipment.LastUpdateDate,
                                                       LastUpdateDate = myShipment.LastUpdateDate,
                                                       MasterShipmentDataId = myShipment.MasterShipmentDataId,
                                                       NextETA = myShipment.NextETA,
                                                       NextETD = myShipment.NextETD,
                                                       NextLegCode = myShipment.NextLegCode,
                                                       NumberOfContainers = myShipment.NumberOfContainers,
                                                       NumberOfInsidePackages = myShipment.NumberOfInsidePackages,
                                                       NumberOfInsidePackagesDetails = myShipment.NumberOfInsidePackagesDetails,
                                                       NumberOfPackages = myShipment.NumberOfPackages,
                                                       OpenPayablesInLocalCurrency = myShipment.OpenPayablesInLocalCurrency,
                                                       OpenPayablesInProfitCurrency = myShipment.OpenPayablesInProfitCurrency,
                                                       OpenReceivablesInLocalCurrency = myShipment.OpenReceivablesInLocalCurrency,
                                                       OpenReceivablesInProfitCurrency = myShipment.OpenReceivablesInProfitCurrency,
                                                       OrderChargeableWeight = myShipment.OrderChargeableWeight,
                                                       OrderVolumetricWeight = myShipment.OrderVolumetricWeight,
                                                       PackagesQuantity = myShipment.PackagesQuantity,
                                                       PreCarriageETD = myShipment.PreCarriageETD,
                                                       ProductCode = myShipment.ProductCode,
                                                       ProfitExchangeRate = myShipment.ProfitExchangeRate,
                                                       ProfitInLocalCurrency = myShipment.ProfitInLocalCurrency,
                                                       ProfitInProfitCurrency = myShipment.ProfitInProfitCurrency,
                                                       QuoteId = myShipment.QuoteId,
                                                       Routing = myShipment.Routing,
                                                       SalesmanUserId = myShipment.SalesmanUserId,
                                                       SearchFields = myShipment.SearchFields,
                                                       ShipmentLevelCode = myShipment.ShipmentLevelCode,
                                                       ShipmentNumber = myShipment.ShipmentNumber,
                                                       ShipmentPayableStatusCode = myShipment.ShipmentPayableStatusCode,
                                                       ShipmentReceivableStatusCode = myShipment.ShipmentReceivableStatusCode,
                                                       ShipperId = myShipment.ShipperId,
                                                       ShipperReference1 = myShipment.ShipperReference1,
                                                       ShipperReference2 = myShipment.ShipperReference2,
                                                       SpecialServicesTypeId = myShipment.SpecialServicesTypeId,
                                                       StatusDate = myShipment.StatusDate,
                                                       StatusId = myShipment.StatusId,
                                                       TEU = myShipment.TEU,
                                                       ToPortId = myShipment.ToPortId,
                                                       TransportDocumentNumber = myShipment.TransportDocumentNumber,
                                                       UpdatedByUserId = myShipment.UpdatedByUserId,
                                                       VolumeInCBM = myShipment.VolumeInCBM,
                                                       VolumetricWeight = myShipment.VolumetricWeight,
                                                       IsCancelled = myShipment.IsCancelled,
                                                       CancelledDate = myShipment.CancelledDate,
                                                       NoFreightFile = myShipment.NoFreightFile,
                                                       ShipmentMasterDataId = myShipment.MasterShipmentDataId,
                                                       NumberOfFollowUps = myShipment.NumberOfFollowUps,
                                                       #endregion

                                                       #region Master Fields
                                                       CutoffDate = myMaster.CutoffDate,
                                                       AirlinePrefix = myMaster.AirlinePrefix,
                                                       MainCarriageATA = myMaster.MainCarriageATA,
                                                       FWBStatusCode = myMaster.FWBStatusCode,
                                                       FWBStatusDate = myMaster.FWBStatusDate,
                                                       CargonautFWBStatusCode = myMaster.CargonautFWBStatusCode,
                                                       CargonautFWBStatusDate = myMaster.CargonautFWBStatusDate,
                                                       ImportManifest = myMaster.ImportManifest,
                                                       MainCarriageATD = myMaster.MainCarriageATD,
                                                       MainCarriageCarrierId = myMaster.MainCarriageCarrierId,
                                                       MainCarriageCarrierNumber = myMaster.MainCarriageCarrierNumber,
                                                       MainCarriageCarrierPrefix = myMaster.MainCarriageCarrierPrefix,
                                                       MainCarriageETA = myMaster.MainCarriageETA,
                                                       MainCarriageETD = myMaster.MainCarriageETD,
                                                       MainCarriageFromAddressId = myMaster.MainCarriageFromAddressId,
                                                       MainCarriageFromPartnerId = myMaster.MainCarriageFromPartnerId,
                                                       MainCarriageFromPortId = myMaster.MainCarriageFromPortId,
                                                       MainCarriageToAddressId = myMaster.MainCarriageToAddressId,
                                                       MainCarriageToPartnerId = myMaster.MainCarriageToPartnerId,
                                                       ManifestReason = myMaster.ManifestReason,
                                                       ManifestStatusCode = myMaster.ManifestStatusCode,
                                                       Master = myMaster.Master,
                                                       MasterShipmentNumber = myMaster.MasterShipmentNumber,
                                                       TruckNumber = myMaster.TruckNumber,
                                                       Transshipment1CarrierPrefix = myMaster.Transshipment1CarrierPrefix,
                                                       Transshipment2CarrierPrefix = myMaster.Transshipment2CarrierPrefix,
                                                       Transshipment3CarrierPrefix = myMaster.Transshipment3CarrierPrefix,
                                                       LongMaster = myShipment.TransportModeId == "A" ? (myMaster.AirlinePrefix != null && myMaster.Master != null ? myMaster.AirlinePrefix + "-" + myMaster.Master : myMaster.Master) : myMaster.Master,
                                                       #endregion

                                                       CarrierLastStatusName = myLastStatus.Name,
                                                       FHLStatusName = myFHLStatus.Name,
                                                       CargonautFHLStatusName = myFHLCargonautStatus.Name,
                                                       //DocumentsSearchFields = myShipmentComputedField.DocumentsSearchFields,
                                                       //IsMissingDocument = myShipmentComputedField.IsMissingDocuments,

                                                       //FWBStatusName = myFWBStatus.Name,
                                                       // CargonautFWBStatusName = myFWBCargonautStatus.Name,



                                                       #region un-known
                                                       hasChanges = false,
                                                       HasException = false,
                                                       IsAnyConversation = false,
                                                       IsException = false,
                                                       IsOccurChange = false,
                                                       IsShipmentTracking = false,
                                                       LastModified = null,
                                                       NewMessage = false,
                                                       NumberOfShipments = 0,
                                                       ResultFromDocument = false,
                                                       #endregion

                                                       #region maybe Not uses
                                                       Field1Id = "",
                                                       CustomFieldId = "",
                                                       BasketId = "",
                                                       ActivityDate = null,
                                                       ActivityByUserName = "",
                                                       ActivityTypeName = "",
                                                       AWBChargeAmount = null,
                                                       AWBCommodityItemNumber = "",
                                                       CarrierTransportDocumentNumber = "",
                                                       FollowUpDate = null,
                                                       FollowUpId = "",
                                                       FollowUpNotes = "",
                                                       FollowUpOwnerId = "",
                                                       FollowUpTypeId = "",
                                                       FollowUpOwner = "",
                                                       FollowUpType = "",
                                                       MainCarriageFinalDestinationATA = null,
                                                       MainCarriageFinalDestinationETA = null,
                                                       Transshipment1FullCarrierNumber = "",
                                                       Transshipment2FullCarrierNumber = "",
                                                       Transshipment3FullCarrierNumber = "",
                                                       #endregion

                                                       // Other Context                                                       
                                                       Consignee = "",
                                                       ConsigneeName = "",
                                                       ConsolidatorName = "",
                                                       CreatedByUserName = "",
                                                       CustomerName = "",
                                                       ConsolidatorNote = "",
                                                       CarrierNumber = "", // multi fields
                                                       BranchName = "", //branch
                                                       AgentName = "",    //card
                                                       AWBCurrencyCode = "",  //currency
                                                       AccountManagerUserName = "",   //user
                                                       DirectionName = "", //direction
                                                       TransportModeName = "", //TransportMode


                                                       FreightForwarderName = "",
                                                       FromCountryCode = "",
                                                       FromPort = "",
                                                       FromPortCountry = "",
                                                       FromPortCountryCode = "",
                                                       FromPortCountryName = "",
                                                       FromPortName = "",
                                                       IncotermCode = "",
                                                       LocalCurrencyCode = "",
                                                       MainCarriageFromCity = "",
                                                       MainCarriageCarrierCode = "",
                                                       MainCarriageCarrierName = "",
                                                       MainCarriageFromCountryCode = "",
                                                       MainCarriageFromPortName = "",
                                                       MainCarriageFullCarrierNumber = "",
                                                       MainCarriageToCity = "",
                                                       MainCarriageToCountryCode = "",
                                                       ProfitCurrencyCode = "",
                                                       SalesmanUserName = "",
                                                       ShipmentLevelName = "",
                                                       ShipmentType = "",
                                                       ShipmentPayableStatusName = "",
                                                       ShipmentReceivableStatusName = "",
                                                       Shipper = "",
                                                       ShipperName = "",
                                                       SpecialServicesTypeName = "",
                                                       StatusName = "",
                                                       ToCountryCode = "",
                                                       ToPort = "",
                                                       ToPortCountry = "",
                                                       ToPortCountryCode = "",
                                                       ToPortCountryName = "",
                                                       ToPortName = "",
                                                       NextLegName = "",
                                                   });
            return iQueryable;
        }

        public int GetFollowUpsByShipmentsFilterCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ContactQuery contactRep = new ContactQuery(tenant);
            ContactPM contact = contactRep.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);
            shipmentRepository = new ShipmentRepository(tenant);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            FollowUpsCustomFilter customfilters = new FollowUpsCustomFilter(tenant);

            BranchPermitionsFilter.AddUserBranchRestrictionFilters(listQueryOperation, tenant);

            if (SecurityUtility.CheckTableContactFeature("User", "PRODUCTS", tenant))
            {
                ProductPermitionsFilter.AddUserProductRestrictionFilters(listQueryOperation, tenant);
            }

            IQueryable<ShipmentFollowUpDataView> shipmentFollowUps = shipmentRepository.GetShipmentFollowUpDataViewByTenant(tenant);
            shipmentFollowUps = customfilters.GetShipmentFollowUpFilteredQuery(queryOperations, shipmentFollowUps);
            int skippedShipments = queryOperations.PageIndex;

            var query2 = from f in shipmentFollowUps
                         select new ShipmentList()
                         {
                             FNAReason = f.FNAReason,
                             CarrierLastStatusDate = f.CarrierLastStatusDate,
                             CarrierLastStatusName = f.CarrierLastStatusName,
                             CarrierLastStatusCode = f.CarrierLastStatusCode,
                             ShipmentViewId = f.Id + f.FollowUpId,
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
                             ChargeableWeightInKG = f.ChargeableWeightInKG,
                             ChargeableWeight = f.ChargeableWeight,
                             GrossWeight = f.GrossWeight,
                             ShipperReference1 = f.ShipperReference1/*, Master = f.Master*/,
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
                             Routing = f.Routing,//(f.DirectionId!="D"&&f.TransportModeId!="I")?((f.PreCarriageFromPortCode != null ? f.PreCarriageFromPortCode + " > " : "") + (f.MasterShipmentDataId != null ? (f.MainCarriageFromPortCode != null ? f.MainCarriageFromPortCode + " > " : "") + (f.MainCarriageFinalDestinationPortCode != null ? (f.OnCarriageToPortCode != null ? f.MainCarriageFinalDestinationPortCode + " > " + f.OnCarriageToPortCode : f.MainCarriageFinalDestinationPortCode) : "") : ((f.FromPortCode != null ? f.FromPortCode + " > " : "") + (f.ToPortCode != null ? (f.OnCarriageToPortCode != null ? f.ToPortCode + " > " + f.OnCarriageToPortCode : f.ToPortCode) : "")))):(""),
                             FromPortId = !string.IsNullOrEmpty(f.MainCarriageFromPortId) ? f.MainCarriageFromPortId : f.FromPortId,
                             ToPortId = !string.IsNullOrEmpty(f.MainCarriageToPortId) ? f.MainCarriageToPortId : f.ToPortId,
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
                             AirlinePrefix = f.AirlinePrefix,
                             AccountedPayablesInLocalCurrency = f.AccountedPayablesInLocalCurrency,
                             AccountedPayablesInProfitCurrency = f.AccountedPayablesInProfitCurrency,
                             AccountedReceivablesInProfitCurrency = f.AccountedReceivablesInProfitCurrency,
                             MainCarriageFromPortId = f.MainCarriageFromPortId,
                             MainCarriageFromPortName = f.MainCarriageFromPortName,
                             MainCarriageATA = f.MainCarriageATA,
                             MainCarriageETD = f.MainCarriageETD,
                             IncotermId = f.IncotermId,
                             OpenPayablesInProfitCurrency = f.OpenPayablesInProfitCurrency,
                             CustomerReference1 = f.CustomerReference1,
                             CustomerReference2 = f.CustomerReference2,
                             IssuingCarrierAgentId = f.IssuingCarrierAgentId,
                             IncotermCode = f.IncotermCode,
                             AsAgreedFreight = f.AsAgreedFreight,
                             AsAgreedOtherCharges = f.AsAgreedOtherCharges,
                             AccountNumber = f.AccountNumber,
                             FollowUpDate = f.FollowUpDate,
                             FollowUpId = f.FollowUpId,
                             FollowUpNotes = f.FollowUpNotes,
                             FollowUpOwner = f.FollowUpOwner,
                             FollowUpOwnerId = f.FollowUpOwnerId,
                             FollowUpType = f.FollowUpType,
                             FollowUpTypeId = f.FollowUpTypeId,
                             VolumeInCBM = f.VolumeInCBM,
                             SpecialServicesTypeId = f.SpecialServicesTypeId,
                             SpecialServicesTypeName = f.SpecialServicesTypeName,
                             ARInvoiceIssued = f.ARInvoiceIssued,
                             CreditNoteIssued = f.CreditNoteIssued,
                             ProductCode = f.ProductCode,
                             IsAccountingClosed = f.IsAccountingClosed,
                             IsOperationalClosed = f.IsOperationalClosed,
                             OperationalCloseDate = f.OperationalCloseDate,
                             AccountingCloseDate = f.AccountingCloseDate,
                             PackagesQuantity = f.PackagesQuantity,
                             LocalCustomsTransmissionsStatusCode = f.LocalCustomsTransmissionsStatusCode,
                             LocalCustomsTransmissionsStatusName = f.LocalCustomsTransmissionsStatusName,
                             LocalCustomsTransmissionsStatusError = f.LocalCustomsTransmissionsStatusError,
                             LocalCustomsTransmissionsStatusDate = f.LocalCustomsTransmissionsStatusDate,
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
                             FromPortCountryCode = f.FromPortCountryCode,
                             ToPortCountryCode = f.ToPortCountryCode,
                             StatusId = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusId : f.ShipmentStatusId) : (f.ShipmentStatusId),
                             StatusDate = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusDate : f.ShipmentStatusDate) : (f.ShipmentStatusDate),
                             StatusName = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusName : f.ShipmentStatusName) : (f.ShipmentStatusName),
                             StatusLocation = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusLocation : f.ShipmentStatusLocation) : (f.ShipmentStatusLocation),
                             LongMaster = f.TransportModeId == "A" ? (!string.IsNullOrEmpty(f.AirlinePrefix) && !string.IsNullOrEmpty(f.Master) ? f.AirlinePrefix + "-" + f.Master : "") : f.Master,
                             CustomsDeclarationNumber = f.CustomsDeclarationNumber,
                             NumberOfHouses = f.NumberOfHouses,
                         };

            query2 = filter.GetFilteredQuery<ShipmentList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        [Invoke]
        public string CheckHousesOpenAmounts(string masterId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            bool hasOpenPayables = false;
            bool hasOpenReceivables = false;

            shipmentQuery = new ShipmentQuery(tenant);
            ShipmentPM masterPM = shipmentQuery.GetSinglePM(masterId, tenant);

            if (masterPM != null)
            {
                foreach (ConsoleShipmentPM consoleShipmentPM in masterPM.ShipmentConsoleShipments)
                {
                    ShipmentPM consoleShipment = shipmentQuery.GetSinglePM(consoleShipmentPM.Id, tenant);

                    if (consoleShipment != null)
                    {
                        if (!hasOpenReceivables)
                        {
                            #region
                            if (consoleShipment.ShipmentReceivables.Count > 0)
                            {
                                foreach (ShipmentReceivablePM item in consoleShipment.ShipmentReceivables)
                                {
                                    if (item.ShipmentReceivableLineStatusCode != "ACCT" && item.ShipmentReceivableLineStatusCode != "EMPT")
                                    {
                                        if (item.TotalAmount != null && item.TotalAmount != 0)
                                        {
                                            hasOpenReceivables = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            #endregion
                        }

                        if (!hasOpenPayables)
                        {
                            #region
                            if (consoleShipment.ShipmentPayables.Count > 0)
                            {
                                foreach (ShipmentPayablePM item in consoleShipment.ShipmentPayables)
                                {
                                    if (item.ShipmentPayableLineStatusCode != "ACCT" && item.ShipmentPayableLineStatusCode != "EMPT" && item.ShipmentPayableParentId == null)
                                    {
                                        if (item.ShipmentPayableAmountTypeCode == "NEXP")
                                        {
                                            if (item.AccountedAmount != null && item.AccountedAmount != 0)
                                            {
                                                hasOpenPayables = true;
                                                break;
                                            }
                                        }

                                        else
                                        {
                                            if (item.ExpectedAmount != null && item.ExpectedAmount != 0)
                                            {
                                                hasOpenPayables = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
            }

            string myResult = "";
            if (hasOpenPayables)
            {
                myResult += "P";
            }

            if (hasOpenReceivables)
            {
                myResult += "R";
            }

            return myResult;
        }

        [Invoke]
        public int GetShipmentsQuotesCount(int tenant)
        {
            shipmentRepository = new ShipmentRepository(tenant);
            int result = 0;
            int shipmentsCount = shipmentRepository.GetShipmentsCount(tenant);

            QuoteRepository quoteRepository = new QuoteRepository(tenant);
            int quotesCount = quoteRepository.GetQuotesCount(tenant);
            result = shipmentsCount + quotesCount;
            return result;
        }

        [Invoke]
        public int GetShipmentsCountByQuoteId(string quoteId, int tenant)
        {
            shipmentRepository = new ShipmentRepository(tenant);
            int result = shipmentRepository.GetShipmentsCountByQuoteId(quoteId, tenant);

            return result;
        }

        [Invoke]
        public void InvokeCloseMaster(string masterId, int tenant)
        {
            shipmentRepository = new ShipmentRepository(tenant);
            Shipment master = shipmentRepository.GetSingleShipment(masterId, tenant);
            master.IsOperationalClosed = true;
            master.OperationalCloseDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            shipmentRepository.Update(master);

            List<Shipment> housesList = shipmentRepository.GetHouseShipmentsForMaster(masterId, tenant);
            foreach (Shipment house in housesList)
            {
                house.IsOperationalClosed = true;
                house.OperationalCloseDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                shipmentRepository.Update(house);
            }
            shipmentRepository.SubmitChanges();
        }

        [Invoke]
        public void InvokeReopeneMaster(string masterId, int tenant)
        {
            shipmentRepository = new ShipmentRepository(tenant);
            Shipment master = shipmentRepository.GetSingleShipment(masterId, tenant);
            master.IsOperationalClosed = false;
            master.OperationalCloseDate = null;
            shipmentRepository.Update(master);

            List<Shipment> housesList = shipmentRepository.GetHouseShipmentsForMaster(masterId, tenant);
            foreach (Shipment house in housesList)
            {
                house.IsOperationalClosed = false;
                house.OperationalCloseDate = null;
                shipmentRepository.Update(house);
            }
            shipmentRepository.SubmitChanges();
        }

        public void UpdateShipmentList(ShipmentList entity)
        {

        }

        public List<ShipmentPackagePM> GetShipmentConsolidationPackages(string masterId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            List<ShipmentPackagePM> myResult = new List<ShipmentPackagePM>();
            shipmentQuery = new ShipmentQuery(tenant);
            myResult = shipmentQuery.GetShipmentConsolidationPackages(masterId, tenant);
            return myResult;
        }

        [Invoke]
        public DateTime InsertShipmentTraceEvent(ShipmentPM entityPM, DateTime? eventDate, string note, string eventTypeId, string objectTableId, string userId, int tenant)
        {
            string entityId = entityPM.Id;
            SecurityUtility.AuthenticationOnTenant(tenant);
            IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);
            TraceEventRepository traceEventRep = new TraceEventRepository(webFreightContext);

            TraceEvent newTraceEvent = new TraceEvent();
            newTraceEvent.Id = Guid.NewGuid().ToString();
            newTraceEvent.LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            newTraceEvent.Notes = note;
            newTraceEvent.ObjectTableId = objectTableId;
            newTraceEvent.Tenant = tenant;
            newTraceEvent.UserId = userId;
            newTraceEvent.EventTypeId = eventTypeId;
            newTraceEvent.EventDateTime = eventDate != null ? eventDate.Value : TenantServerConfigration.GetCurrentDateTime(tenant);
            newTraceEvent.Deleted = false;
            newTraceEvent.IsAddedManually = true;
            newTraceEvent.EntityId = entityId;
            traceEventRep.Add(newTraceEvent);
            traceEventRep.SubmitChanges();

            objectContext = ShipmentsContext.GetContext(tenant);
            shipmentRepository = new ShipmentRepository(objectContext);
            Shipment entityPOCO = shipmentRepository.GetSingleShipment(entityId, tenant);
            
            if (entityPOCO != null)
            {
                EventTypeRepository eventTypeRep = new EventTypeRepository(webFreightContext);
                EventType eventType = eventTypeRep.GetSingleEventType(eventTypeId, tenant);
                if (eventType != null)
                {                    
                    if (!string.IsNullOrEmpty(eventType.EntityStatusId))
                    {
                        #region
                        if (string.IsNullOrEmpty(entityPOCO.StatusId))
                        {
                            entityPOCO.StatusId = eventType.EntityStatusId;
                            entityPOCO.StatusDate = newTraceEvent.EventDateTime;
                            entityPOCO.StatusLocation = null;
                            entityPOCO.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                            if (entityPOCO.ShipmentLevelCode == "D" || entityPOCO.ShipmentLevelCode == "C")
                            {
                                ShipmentMasterDataRepository shipmentMasterDataRepository = new ShipmentMasterDataRepository(objectContext);
                                ShipmentMasterData entityMasterData = shipmentMasterDataRepository.GetSingleMasterData(entityPOCO.MasterShipmentDataId);
                                entityMasterData.StatusId = entityPOCO.StatusId;
                                entityMasterData.StatusDate = entityPOCO.StatusDate;
                                entityMasterData.StatusLocation = entityPOCO.StatusLocation;
                                shipmentMasterDataRepository.Update(entityMasterData);
                            }
                        }

                        else
                        {
                            string oldStatusId = entityPOCO.StatusId;
                            string newStatusId = eventType.EntityStatusId;
                            EntityStatusRepository entityStatusRepository = new EntityStatusRepository(tenant);
                            EntityStatus oldStatus = EntityStatusRepository.GetSingleEntityStatus(oldStatusId, tenant, true);
                            EntityStatus newStatus = EntityStatusRepository.GetSingleEntityStatus(newStatusId, tenant, true);

                            if (newStatus.StatusWeight >= oldStatus.StatusWeight)
                            {
                                entityPOCO.StatusId = newStatusId;
                                entityPOCO.StatusDate = newTraceEvent.EventDateTime;
                                entityPOCO.StatusLocation = null;
                                entityPOCO.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                                if (entityPOCO.ShipmentLevelCode == "D" || entityPOCO.ShipmentLevelCode == "C")
                                {
                                    ShipmentMasterDataRepository shipmentMasterDataRepository = new ShipmentMasterDataRepository(objectContext);
                                    ShipmentMasterData entityMasterData = shipmentMasterDataRepository.GetSingleMasterData(entityPOCO.MasterShipmentDataId);
                                    entityMasterData.StatusId = entityPOCO.StatusId;
                                    entityMasterData.StatusDate = entityPOCO.StatusDate;
                                    entityMasterData.StatusLocation = entityPOCO.StatusLocation;
                                    shipmentMasterDataRepository.Update(entityMasterData);
                                }
                            }
                        }
                        #endregion
                    }

                    if (eventType.Code == "EXCE")
                    {
                        #region
                        ContactsUnseenEntitiesHelper.AddUnseenEntityRecord(newTraceEvent.Id, tenant);
                        entityPOCO.ExceptionDate = newTraceEvent.EventDateTime;

                        if (!string.IsNullOrEmpty(newTraceEvent.Notes) && newTraceEvent.Notes.Length > 500) entityPOCO.ExceptionDescription = newTraceEvent.Notes.Substring(0, 499);
                        else entityPOCO.ExceptionDescription = newTraceEvent.Notes;

                        entityPOCO.LastExceptionDescription = newTraceEvent.Notes;
                        entityPOCO.HasException = true;
                        entityPOCO.ExceptionResolvedDescription = null;

                        if (entityPOCO.ShipmentLevelCode == "A")
                        {
                            List<Shipment> connectedShipments = shipmentRepository.GetConnectedCustomShipments(tenant, entityId).ToList();
                            foreach (Shipment sh in connectedShipments)
                            {
                                sh.HasException = entityPOCO.HasException;
                                sh.ExceptionDate = entityPOCO.ExceptionDate;
                                sh.ExceptionDescription = entityPOCO.ExceptionDescription;
                                sh.LastExceptionDescription = entityPOCO.LastExceptionDescription;
                                sh.ExceptionResolvedDescription = entityPOCO.ExceptionResolvedDescription;
                                shipmentRepository.Update(sh);
                            }
                        }
                        #endregion
                    }

                    else if (eventType.Code == "EXRE")
                    {
                        #region
                        if (!string.IsNullOrEmpty(newTraceEvent.Notes) && newTraceEvent.Notes.Length > 500)  entityPOCO.ExceptionResolvedDescription = newTraceEvent.Notes.Substring(0, 499);
                        else entityPOCO.ExceptionResolvedDescription = newTraceEvent.Notes;

                        entityPOCO.HasException = false;
                        entityPOCO.ExceptionDescription = null;
                        entityPOCO.ExceptionDate = null;

                        if (entityPOCO.ShipmentLevelCode == "A")
                        {
                            List<Shipment> connectedShipments = shipmentRepository.GetConnectedCustomShipments(tenant, entityId).ToList();
                            foreach (Shipment sh in connectedShipments)
                            {
                                sh.ExceptionResolvedDescription = entityPOCO.ExceptionResolvedDescription;
                                sh.HasException = entityPOCO.HasException;
                                sh.ExceptionDescription = entityPOCO.ExceptionDescription;
                                sh.ExceptionDate = entityPOCO.ExceptionDate;
                                sh.LastExceptionDescription = entityPOCO.ExceptionDescription;
                                shipmentRepository.Update(sh);
                            }
                        }
                        #endregion
                    }

                    shipmentRepository.Update(entityPOCO);
                    shipmentRepository.SubmitChanges();
                }
            }

            return newTraceEvent.LogDateTime;
        }

        [Invoke]
        public void DeleteShipmentTraceEvent(ShipmentPM entityPM, string traceEventId, int tenant, bool external)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ShipmentTracing.DeleteShipmentTraceEvent(entityPM, traceEventId, tenant, external);
        }

        public IQueryable<ShipmentList> GetShipmentFollowUpsForShipment(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            if (this.objectContext == null)
            {
                this.objectContext = ShipmentsContext.GetContext(tenant);
            }

            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);

            shipmentRepository = new ShipmentRepository(objectContext);

            IQueryable<ShipmentFollowUpDataView> shipmentFollowUps = shipmentRepository.GetShipmentFollowUpDataViewByTenant(tenant);

            var query2 = from f in shipmentFollowUps
                         select new ShipmentList()
                         {
                             FNAReason = f.FNAReason,
                             CarrierLastStatusDate = f.CarrierLastStatusDate,
                             CarrierLastStatusName = f.CarrierLastStatusName,
                             CarrierLastStatusCode = f.CarrierLastStatusCode,
                             ShipmentViewId = f.Id + f.FollowUpId,
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
                             ChargeableWeightInKG = f.ChargeableWeightInKG,
                             ChargeableWeight = f.ChargeableWeight,
                             GrossWeight = f.GrossWeight,
                             ShipperReference1 = f.ShipperReference1/*, Master = f.Master*/,
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
                             Routing = f.Routing,//(f.DirectionId!="D"&&f.TransportModeId!="I")?((f.PreCarriageFromPortCode != null ? f.PreCarriageFromPortCode + " > " : "") + (f.MasterShipmentDataId != null ? (f.MainCarriageFromPortCode != null ? f.MainCarriageFromPortCode + " > " : "") + (f.MainCarriageFinalDestinationPortCode != null ? (f.OnCarriageToPortCode != null ? f.MainCarriageFinalDestinationPortCode + " > " + f.OnCarriageToPortCode : f.MainCarriageFinalDestinationPortCode) : "") : ((f.FromPortCode != null ? f.FromPortCode + " > " : "") + (f.ToPortCode != null ? (f.OnCarriageToPortCode != null ? f.ToPortCode + " > " + f.OnCarriageToPortCode : f.ToPortCode) : "")))):(""),
                             FromPortId = !string.IsNullOrEmpty(f.MainCarriageFromPortId) ? f.MainCarriageFromPortId : f.FromPortId,
                             ToPortId = !string.IsNullOrEmpty(f.MainCarriageToPortId) ? f.MainCarriageToPortId : f.ToPortId,
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
                             AirlinePrefix = f.AirlinePrefix,
                             AccountedPayablesInLocalCurrency = f.AccountedPayablesInLocalCurrency,
                             AccountedPayablesInProfitCurrency = f.AccountedPayablesInProfitCurrency,
                             AccountedReceivablesInProfitCurrency = f.AccountedReceivablesInProfitCurrency,
                             MainCarriageFromPortId = f.MainCarriageFromPortId,
                             MainCarriageFromPortName = f.MainCarriageFromPortName,
                             MainCarriageATA = f.MainCarriageATA,
                             MainCarriageETD = f.MainCarriageETD,
                             IncotermId = f.IncotermId,
                             OpenPayablesInProfitCurrency = f.OpenPayablesInProfitCurrency,
                             CustomerReference1 = f.CustomerReference1,
                             CustomerReference2 = f.CustomerReference2,
                             IssuingCarrierAgentId = f.IssuingCarrierAgentId,
                             IncotermCode = f.IncotermCode,
                             FinalArrivalDate = f.FinalArrivalDate,
                             AsAgreedFreight = f.AsAgreedFreight,
                             AsAgreedOtherCharges = f.AsAgreedOtherCharges,
                             AccountNumber = f.AccountNumber,
                             FollowUpDate = f.FollowUpDate,
                             FollowUpId = f.FollowUpId,
                             FollowUpNotes = f.FollowUpNotes,
                             FollowUpOwner = f.FollowUpOwner,
                             FollowUpOwnerId = f.FollowUpOwnerId,
                             FollowUpType = f.FollowUpType,
                             FollowUpTypeId = f.FollowUpTypeId,
                             VolumeInCBM = f.VolumeInCBM,
                             //CarrierNumber = f.TransportModeId == "A" ? (f.MainCarriageCarrierCode + f.MainCarriageCarrierNumber) : f.TransportModeId == "O" ? (f.MainCarriageVesselName + "/" + f.MainCarriageCarrierNumber) : f.TransportModeId == "I" ? (f.MainCarriageCarrierNumber) : null,
                             ARInvoiceIssued = f.ARInvoiceIssued,
                             CreditNoteIssued = f.CreditNoteIssued,
                             FreightForwarderId = f.FreightForwarderId,
                             FreightForwarderName = f.FreightForwarderName,
                             ProductCode = f.ProductCode,
                             IsAccountingClosed = f.IsAccountingClosed,
                             IsOperationalClosed = f.IsOperationalClosed,
                             OperationalCloseDate = f.OperationalCloseDate,
                             AccountingCloseDate = f.AccountingCloseDate,
                             PackagesQuantity = f.PackagesQuantity,
                             LocalCustomsTransmissionsStatusCode = f.LocalCustomsTransmissionsStatusCode,
                             LocalCustomsTransmissionsStatusName = f.LocalCustomsTransmissionsStatusName,
                             LocalCustomsTransmissionsStatusError = f.LocalCustomsTransmissionsStatusError,
                             LocalCustomsTransmissionsStatusDate = f.LocalCustomsTransmissionsStatusDate,
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
                             NumberOfInsidePackages = f.NumberOfInsidePackages,
                             NumberOfInsidePackagesDetails = f.NumberOfInsidePackagesDetails,
                             ConsolidatorId = f.ConsolidatorId,
                             ConsolidatorName = f.ConsolidatorName,
                             ConsolidatorNote = f.ConsolidatorNote,
                             ConsolidatorAddressId = f.ConsolidatorAddressId,
                             ConsolidatorContactId = f.ConsolidatorContactId,
                             ConsolidatorReference = f.ConsolidatorReference,
                             ManifestReason = f.ManifestReason,
                             ManifestStatusCode = f.ManifestStatusCode,
                             FromPortCountryCode = f.FromPortCountryCode,
                             ToPortCountryCode = f.ToPortCountryCode,
                             StatusId = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusId : f.ShipmentStatusId) : (f.ShipmentStatusId),
                             StatusDate = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusDate : f.ShipmentStatusDate) : (f.ShipmentStatusDate),
                             StatusName = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusName : f.ShipmentStatusName) : (f.ShipmentStatusName),
                             StatusLocation = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusLocation : f.ShipmentStatusLocation) : (f.ShipmentStatusLocation),
                             LongMaster = f.TransportModeId == "A" ? (!string.IsNullOrEmpty(f.AirlinePrefix) && !string.IsNullOrEmpty(f.Master) ? f.AirlinePrefix + "-" + f.Master : "") : f.Master,
                             CustomsDeclarationNumber = f.CustomsDeclarationNumber,
                             OperationalDate = f.OperationalDate,
                             CutoffDate = f.CutoffDate,
                             NumberOfHouses = f.NumberOfHouses,
                         };

            return query2;
        }

        public IQueryable<ShipmentList> GetShipmentFollowUpsForMaster(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Master", "READ", tenant);

            FollowUpQuery followUpsRepository = new FollowUpQuery(tenant);
            return followUpsRepository.GetFollowUpsByTenantForMasterFilter(tenant, null);
        }

        public void UpdateEntityPartner(EntityPartner currentEntity)
        {

        }

        public List<EntityPartner> GetEntityPartners(string entityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            shipmentQuery = new ShipmentQuery(tenant);

            List<EntityPartner> list = new List<EntityPartner>();
            ShipmentPM shipment = shipmentQuery.GetSinglePM(entityId, tenant);
            int idCounter = 0;

            if (shipment.CustomerId != null )
            {
                list.Add(new EntityPartner()
                {
                    Id = idCounter++,
                    PartnerId = shipment.CustomerId,
                    PartnerType = "Customer",
                    PartnerContactId = shipment.CustomerContactId
                });
            }

            if (shipment.ShipperId != null )
            {
                list.Add(new EntityPartner()
                {
                    Id = idCounter++,
                    PartnerId = shipment.ShipperId,
                    PartnerType = "Shipper",
                    PartnerContactId = shipment.ShipperContactId
                });
            }

            if (shipment.ConsigneeId != null)
            {
                list.Add(new EntityPartner()
                {
                    Id = idCounter++,
                    PartnerId = shipment.ConsigneeId,
                    PartnerType = "Consignee",
                    PartnerContactId = shipment.ConsigneeContactId
                });
            }

            if (shipment.AgentId != null )
            {
                list.Add(new EntityPartner()
                {
                    Id = idCounter++,
                    PartnerId = shipment.AgentId,
                    PartnerType = "Agent",
                    PartnerContactId = shipment.AgentContactId
                });
            }

            if (shipment.CustomAgentImportId != null )
            {
                list.Add(new EntityPartner()
                {
                    Id = idCounter++,
                    PartnerId = shipment.CustomAgentImportId,
                    PartnerType = "Custom Agent Import",
                    PartnerContactId = shipment.CustomAgentImportContactId
                });
            }

            if (shipment.CustomAgentExportId != null )
            {
                list.Add(new EntityPartner()
                {
                    Id = idCounter++,
                    PartnerId = shipment.CustomAgentExportId,
                    PartnerType = "Custom Agent Export",
                    PartnerContactId = shipment.CustomAgentExportContactId
                });
            }

            if (shipment.Notify1Id != null )
            {
                list.Add(new EntityPartner()
                {
                    Id = idCounter++,
                    PartnerId = shipment.Notify1Id,
                    PartnerType = "Notify1",
                    PartnerContactId = shipment.Notify1ContactId
                });
            }

            if (shipment.Notify2Id != null )
            {
                list.Add(new EntityPartner()
                {
                    Id = idCounter++,
                    PartnerId = shipment.Notify2Id,
                    PartnerType = "Notify2",
                    PartnerContactId = shipment.Notify2ContactId
                });
            }

            if (shipment.ShipperNotExporterId != null)
            {
                list.Add(new EntityPartner()
                {
                    Id = idCounter++,
                    PartnerId = shipment.ShipperNotExporterId,
                    PartnerType = "Shipper Not Exporter",
                    PartnerContactId = shipment.ShipperNotExporterContactId
                });
            }

            if (shipment.ConsigneeNotImporterId != null )
            {
                list.Add(new EntityPartner()
                {
                    Id = idCounter++,
                    PartnerId = shipment.ConsigneeNotImporterId,
                    PartnerType = "Consignee Not Importer",
                    PartnerContactId = shipment.ConsigneeNotImporterContactId
                });
            }

            if (shipment.FreightForwarderId != null)
            {
                list.Add(new EntityPartner()
                {
                    Id = idCounter++,
                    PartnerId = shipment.FreightForwarderId,
                    PartnerType = "Freight Forwarder",
                    PartnerContactId = shipment.FreightForwarderContactId
                });
            }

            if (shipment.ConsolidatorId != null )
            {
                list.Add(new EntityPartner()
                {
                    Id = idCounter++,
                    PartnerId = shipment.ConsolidatorId,
                    PartnerType = "Consolidator",
                    PartnerContactId = shipment.ConsolidatorContactId
                });
            }

            return list;
        }

        public bool CompareTimeStamps(byte[] timeStamp1, byte[] timestamp2)
        {
            bool equal = true;
            if (timeStamp1.Length != timestamp2.Length)
            {
                equal = false;
            }
            else
            {
                for (int i = 0; i < timeStamp1.Length; i++)
                {
                    if (timeStamp1[i] != timestamp2[i])
                    {
                        equal = false;
                    }
                }
            }
            return equal;
        }

        [Invoke]
        public bool SendFSR(ShipmentPM entityPM, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);

            FSRManager fSRManager = new FSRManager(tenant, ServiceContext.User.Identity.Name);
            FSRResultClass myResult = fSRManager.SendShipmentFSR(entityPM);
            return myResult.IsUpgradingChamp;
        }

        [Invoke]
        public string ValidateShipmentMasterFieldExistance(string entityId, string myBookingId, string myMasterField, string myAirlinePrefixField, string myDirectionId, string myTransportModeId, string myShipmentLevelCode, bool isCancelled, int myTenant)
        {
            string myResult = null;

            bool isFieldExists = ShipmentValidating.IsMasterFieldUsedByAnotherShipment(entityId, myMasterField, myAirlinePrefixField, myDirectionId, myTransportModeId, myShipmentLevelCode, isCancelled, myTenant);
            if (isFieldExists)
            {
                myResult = "Master field already used in another Shipment";
            }

            else
            {
                isFieldExists = ShipmentValidating.IsMasterFieldUsedByAnotherBooking(myBookingId, myMasterField, myAirlinePrefixField, myDirectionId, myTransportModeId, myShipmentLevelCode, isCancelled, myTenant);
                if (isFieldExists)
                {
                    myResult = "Master field already used in another Booking";
                }
            }

            return myResult;
        }

        public ImporterQueriesDataCounts GetShipmentsQueriesCounts(int tenant, string transportModeId, string SearchFilter,string TypeCode = null)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            shipmentQuery = new ShipmentQuery(tenant);
            ImporterQueriesDataCounts result = shipmentQuery.GetShipmentsQueriesCounts(tenant, transportModeId, SearchFilter, ServiceContext.User.Identity.Name, TypeCode);
            return result;
        }

        public ShipmentPM GetUnSecuredShipmentPMTest(int tenant)
        {
            Random r = new Random();

            return new ShipmentPM()
            {
                Id = Guid.NewGuid().ToString(),
                Tenant = tenant + 1,
            };
        }

        public IQueryable<ShipmentList> GetShipmentsByQuoteId(string quoteId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            shipmentRepository = new ShipmentRepository(tenant);
            shipmentQuery = new ShipmentQuery(shipmentRepository);

            IQueryable<ShipmentDataView> shipments = shipmentRepository.GetShipmentsByQuoteId(quoteId, tenant);

            IQueryable<ShipmentList> query2 = from f in shipments
                                              select new ShipmentList()
                                              {
                                                  Id = f.Id,
                                                  ShipmentViewId = f.Id,
                                                  Tenant = f.Tenant,
                                                  ShipmentNumber = f.ShipmentNumber,
                                                  ShipmentType = !string.IsNullOrEmpty(f.ShipmentTypeName) ? f.ShipmentTypeName + " " + f.ShipmentLevelName : f.ShipmentLevelName,
                                                  CreateDateTime = f.CreateDateTime,
                                                  Shipper = f.ShipperName,
                                                  Consignee = f.ConsigneeName,
                                                  DirectionId = f.DirectionId,
                                                  DirectionName = f.DirectionName,
                                                  TransportModeName = f.TransportModeName,
                                                  House = f.House,
                                                  TransportModeId = f.TransportModeId,
                                                  ChargeableWeightInKG = f.ChargeableWeightInKG,
                                                  ChargeableWeight = f.ChargeableWeight,
                                                  GrossWeight = f.GrossWeight,
                                                  ShipperReference1 = f.ShipperReference1,
                                                  Master = f.Master,
                                                  BranchId = f.BranchId,
                                                  DepartmentId = f.DepartmentId,
                                                  MainCarriageETA = f.MainCarriageETA,
                                                  MainCarriageATD = f.MainCarriageATD,
                                                  Routing = f.Routing,
                                                  FromPortId = !string.IsNullOrEmpty(f.MainCarriageFromPortId) ? f.MainCarriageFromPortId : f.FromPortId,
                                                  ToPortId = !string.IsNullOrEmpty(f.MainCarriageToPortId) ? f.MainCarriageToPortId : f.ToPortId,
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
                                                  AirlinePrefix = f.AirlinePrefix,
                                                  AccountedPayablesInLocalCurrency = f.AccountedPayablesInLocalCurrency,
                                                  AccountedPayablesInProfitCurrency = f.AccountedPayablesInProfitCurrency,
                                                  AccountedReceivablesInProfitCurrency = f.AccountedReceivablesInProfitCurrency,
                                                  MainCarriageFromPortId = f.MainCarriageFromPortId,
                                                  MainCarriageFromPortName = f.MainCarriageFromPortName,
                                                  MainCarriageATA = f.MainCarriageATA,
                                                  MainCarriageETD = f.MainCarriageETD,
                                                  IncotermId = f.IncotermId,
                                                  VolumeInCBM = f.VolumeInCBM,
                                                  FromPortCountryCode = f.FromPortCountryCode,
                                                  ToPortCountryCode = f.ToPortCountryCode,
                                                  StatusId = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusId : f.ShipmentStatusId) : (f.ShipmentStatusId),
                                                  StatusDate = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusDate : f.ShipmentStatusDate) : (f.ShipmentStatusDate),
                                                  StatusName = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusName : f.ShipmentStatusName) : (f.ShipmentStatusName),
                                                  StatusLocation = f.MasterShipmentDataId != null ? (f.ShipmentMasterDataStatusWeight > f.ShipmentStatusWeight ? f.ShipmentMasterDataStatusLocation : f.ShipmentStatusLocation) : (f.ShipmentStatusLocation),
                                                  LongMaster = f.TransportModeId == "A" ? (!string.IsNullOrEmpty(f.AirlinePrefix) && !string.IsNullOrEmpty(f.Master) ? f.AirlinePrefix + "-" + f.Master : "") : f.Master,
                                                  IsNewARInvoiceBlocked = f.IsNewARInvoiceBlocked,
                                                  NumberOfHouses = f.NumberOfHouses,
                                              };

            return query2;
        }

        [Invoke]
        public string ValidateShipmentNumberFieldExistance(string number, int tenant)
        {
            string myResult = null;

            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);

            Shipment myShipment = shipmentRepository.GetSingleShipmentByShipmentNumber(number, tenant);

            if (myShipment == null)
            {
                myResult = "Shipment Number not Exist";
            }

            return myResult;
        }

      
        public List<string> GetShipmentIdsForFullTextSearch(byte[] xmlFilters, int tenant)
        {
            List<ShipmentList> myResult = new List<ShipmentList>();

            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
             
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@Tenant", tenant));
            string Where = "Where Shipments.Tenant = @Tenant";

            foreach (QueryFilterItem item in queryOperations.QueryFilterItems)
            {
                if (item.FieldValue != null)
                {
                    switch (item.FieldName)
                    { 
                        case "SearchFields":
                            {
                                string mySearchFields = null;
                                mySearchFields = item.FieldValue.ToString();
                                parameters.Add(new SqlParameter("@SearchFields", "\"" + item.FieldValue + "*\""));
                                Where += " and Contains(Shipments.SearchFields,@SearchFields)";
                                break;
                            }
                    }
                }
            }
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
             

            IShipmentsContext context = ShipmentsContext.GetContext(tenant);
            ShipmentsContext activeContext = context.GetActiveDbContext() as ShipmentsContext;

            List<string> shipmentsIds = activeContext.Database.SqlQuery<string>(("SELECT Shipments.Id FROM Shipments " + Where), parameters.ToArray()).ToList();

            

           

            return shipmentsIds;
        }
    }
}
