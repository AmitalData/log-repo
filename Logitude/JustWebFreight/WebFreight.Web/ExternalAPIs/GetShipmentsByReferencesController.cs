using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.APIDataContract;
using Logitude.BL.ShipmentsModel.CustomFilters;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Controllers.ShipmentsModel.ApiHelpers;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.ExternalAPIs
{
    public class GetShipmentsByReferencesController : ApiController
    {
        private int tenant;
        private string computingPartnerCode;
        private CardRepository cardRepository;
        private ComputingPartnerTranslationHelper computingPartnerTranslationHelper;

        public HttpResponseMessage PostGetDirectShipments(Query query)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.ValidateAPI();
                    Shipments shipments = this.GetShipmentsByReferences(query, "D");
                    return Request.CreateResponse(HttpStatusCode.OK, shipments);
                }
                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", query, apiExceptionResult.Exception, "RatesTable", null, "Rates Update API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage PostGetHouseShipments(Query query)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.ValidateAPI();
                    Shipments shipments = this.GetShipmentsByReferences(query, "H");                    
                    return Request.CreateResponse(HttpStatusCode.OK, shipments);
                }
                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", query, apiExceptionResult.Exception, "RatesTable", null, "Rates Update API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage PostGetMasterShipments(Query query)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.ValidateAPI();
                    Shipments shipments = this.GetShipmentsByReferences(query, "C");
                    return Request.CreateResponse(HttpStatusCode.OK, shipments);
                }
                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", query, apiExceptionResult.Exception, "RatesTable", null, "Rates Update API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        private void ValidateAPI()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticateAPICall(tenant);
            SecurityUtility.AuthenticateAccessibleAPI("Get Shipments by References", authToken.Tenant);
        }

        private Shipments GetShipmentsByReferences(Query query, string levelCode)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            this.tenant = authToken.Tenant;
            this.computingPartnerCode = query.ComputingPartnerCode;
            SecurityUtility.AuthenticateAPICall(tenant);

            this.ValidateEmptyCodeOrPartnerCode(query);
            this.ValidateEmptyReferences(query);
            this.ValidateNotSupportedReference2(query);                        

            this.cardRepository = new CardRepository(tenant);
            this.computingPartnerTranslationHelper = new ComputingPartnerTranslationHelper(tenant);

            List<QueryFilterItem> queryFilterItems = this.CreateQueryFilterItems(query, levelCode);
            QueryOperations queryOperations = this.CreateQueryOperations(queryFilterItems);

            Shipments shipments = new Shipments();
            shipments.ShipmentList = this.GetFilteredShipments(queryOperations);

            return shipments;
        }

        private void ValidateEmptyCodeOrPartnerCode(Query query)
        {
            this.ValidatePartnerMissingCodes(query.Agent, "Agent:");
            this.ValidatePartnerMissingCodes(query.Shipper, "Shipper:");
            this.ValidatePartnerMissingCodes(query.ShipperNotExporter, "ShipperNotExporter:");
            this.ValidatePartnerMissingCodes(query.Consignee, "Consignee:");
            this.ValidatePartnerMissingCodes(query.ConsigneeNotImporter, "ConsigneeNotImporter:");
            this.ValidatePartnerMissingCodes(query.Forwarder, "Forwarder:");
        }
        private void ValidatePartnerMissingCodes(QueryCard queryCard, string partnerType)
        {
            if (queryCard != null && string.IsNullOrEmpty(queryCard.Code) && string.IsNullOrEmpty(queryCard.PartnerCode))
            {
                throw new ApplicationException(partnerType + " Missing Code or Partner Code");
            }
        }
        private void ValidateEmptyReferences(Query query)
        {
            this.ValidatePartnerEmptyReferences(query.Agent, "Agent:");
            this.ValidatePartnerEmptyReferences(query.Shipper, "Shipper:");
            this.ValidatePartnerEmptyReferences(query.ShipperNotExporter, "ShipperNotExporter:");
            this.ValidatePartnerEmptyReferences(query.Consignee, "Consignee:");
            this.ValidatePartnerEmptyReferences(query.ConsigneeNotImporter, "ConsigneeNotImporter:");
            this.ValidatePartnerEmptyReferences(query.Forwarder, "Forwarder:");
        }
        private void ValidatePartnerEmptyReferences(QueryCard queryCard, string partnerType)
        {
            if (queryCard != null && string.IsNullOrEmpty(queryCard.Reference1) && string.IsNullOrEmpty(queryCard.Reference2))
            {
                throw new ApplicationException(partnerType + " Missing References");
            }
        }
        private void ValidateNotSupportedReference2(Query query)
        {
            if (query.Forwarder != null && !string.IsNullOrEmpty(query.Forwarder.Reference2))
            {
                throw new ApplicationException("Forwarder Reference 2 is not supported");
            }

            if (query.ConsigneeNotImporter != null && !string.IsNullOrEmpty(query.ConsigneeNotImporter.Reference2))
            {
                throw new ApplicationException("Consignee Not Importer Reference 2 is not supported");
            }
        }        
        
        private List<QueryFilterItem> CreateQueryFilterItems(Query query, string shipmentLevel)
        {
            string agentId = this.GetPartnerId(query.Agent);
            string shipperId = this.GetPartnerId(query.Shipper);
            string consigneeId = this.GetPartnerId(query.Consignee);
            string shipperNotExporterId = this.GetPartnerId(query.ShipperNotExporter);
            string consigneeNotImporterId = this.GetPartnerId(query.ConsigneeNotImporter);
            string forwarderId = this.GetPartnerId(query.Forwarder);

            List<QueryFilterItem> queryFilterItems = new List<QueryFilterItem>();
            if(!string.IsNullOrEmpty(query.Master)) queryFilterItems.Add(CreateQueryFilterItem("Master", query.Master));
            if (!string.IsNullOrEmpty(query.House)) queryFilterItems.Add(CreateQueryFilterItem("House", query.House));
            if (!string.IsNullOrEmpty(shipmentLevel)) queryFilterItems.Add(CreateQueryFilterItem("ShipmentLevelCode", shipmentLevel));

            if (query.Agent != null)
            {
                if (!string.IsNullOrEmpty(agentId)) queryFilterItems.Add(CreateQueryFilterItem("AgentId", agentId));
                if (!string.IsNullOrEmpty(query.Agent.Reference1)) queryFilterItems.Add(CreateQueryFilterItem("AgentReference1", query.Agent.Reference1));
                if (!string.IsNullOrEmpty(query.Agent.Reference2)) queryFilterItems.Add(CreateQueryFilterItem("AgentReference2", query.Agent.Reference2));
            }

            if (query.Shipper != null)
            {
                if (!string.IsNullOrEmpty(shipperId)) queryFilterItems.Add(CreateQueryFilterItem("ShipperId", shipperId));
                if (!string.IsNullOrEmpty(query.Shipper.Reference1)) queryFilterItems.Add(CreateQueryFilterItem("ShipperReference1", query.Shipper.Reference1));
                if (!string.IsNullOrEmpty(query.Shipper.Reference2)) queryFilterItems.Add(CreateQueryFilterItem("ShipperReference2", query.Shipper.Reference2));
            }

            if (query.Consignee != null)
            {
                if (!string.IsNullOrEmpty(consigneeId)) queryFilterItems.Add(CreateQueryFilterItem("ConsigneeId", consigneeId));
                if (!string.IsNullOrEmpty(query.Consignee.Reference1)) queryFilterItems.Add(CreateQueryFilterItem("ConsigneeReference1", query.Consignee.Reference1));
                if (!string.IsNullOrEmpty(query.Consignee.Reference2)) queryFilterItems.Add(CreateQueryFilterItem("ConsigneeReference2", query.Consignee.Reference2));
            }

            if (query.ShipperNotExporter != null)
            {
                if (!string.IsNullOrEmpty(shipperNotExporterId)) queryFilterItems.Add(CreateQueryFilterItem("ShipperNotExporterId", shipperNotExporterId));
                if (!string.IsNullOrEmpty(query.ShipperNotExporter.Reference1)) queryFilterItems.Add(CreateQueryFilterItem("ShipperNotExporterReference1", query.ShipperNotExporter.Reference1));
                if (!string.IsNullOrEmpty(query.ShipperNotExporter.Reference2)) queryFilterItems.Add(CreateQueryFilterItem("ShipperNotExporterReference2", query.ShipperNotExporter.Reference2));
            }

            if (query.ConsigneeNotImporter != null)
            {
                if (!string.IsNullOrEmpty(consigneeNotImporterId)) queryFilterItems.Add(CreateQueryFilterItem("ConsigneeNotImporterId", consigneeNotImporterId));
                if (!string.IsNullOrEmpty(query.ConsigneeNotImporter.Reference1)) queryFilterItems.Add(CreateQueryFilterItem("ConsigneeNotImporterReference", query.ConsigneeNotImporter.Reference1));
            }

            if (query.Forwarder != null)
            {
                if (!string.IsNullOrEmpty(forwarderId)) queryFilterItems.Add(CreateQueryFilterItem("FreightForwarderId", forwarderId));
                if (!string.IsNullOrEmpty(query.Forwarder.Reference1)) queryFilterItems.Add(CreateQueryFilterItem("FreightForwarderReference", query.Forwarder.Reference1));
            }

            return queryFilterItems;
        }
        private string GetPartnerId(QueryCard myCard)
        {
            string partnerId = null;

            if (myCard != null)
            {
                Card cardPOCO = null;

                if (!string.IsNullOrEmpty(myCard.PartnerCode))
                {
                    if (string.IsNullOrEmpty(computingPartnerCode)) throw new ApplicationException("ComputingPartnerCode is required");
                    var myCode = computingPartnerTranslationHelper.GetLogitudeCodeTranslation(myCard.PartnerCode, computingPartnerCode, "Card");

                    if (string.IsNullOrEmpty(myCode)) throw new ApplicationException("Card with Partner Code " + myCard.PartnerCode + " doesn't match any record");
                    cardPOCO = cardRepository.GetSingleCardByCode(myCode, tenant, false);

                    if (cardPOCO == null)
                    {
                        throw new ApplicationException("Card with Code " + myCard.Code + " doesn't exist");
                    }
                }

                else if (!string.IsNullOrEmpty(myCard.Code))
                {
                    cardPOCO = cardRepository.GetSingleCardByCode(myCard.Code, tenant, false);

                    if (cardPOCO == null)
                    {
                        throw new ApplicationException("Card with Code " + myCard.Code + " doesn't exist");
                    }
                }

                if (cardPOCO != null) partnerId = cardPOCO.Id;
            }

            return partnerId;
        }
        private QueryFilterItem CreateQueryFilterItem(string fieldName, string value)
        {
            return new QueryFilterItem()
            {
                FieldName = fieldName,
                FieldValue = value,
                Operator = "Equals"
            };
        }
        private QueryOperations CreateQueryOperations(List<QueryFilterItem> queryFilterItems)
        {
            return new QueryOperations()
            {
                ObjectTableName = "Shipment",
                PageIndex = 0,
                QuerySection = "Shipments",
                SortByColumnName = "ShipmentNumber",
                GetAll = true,
                QueryFilterItems = queryFilterItems,
            };
        }
        private List<ShipmentByReferences> GetFilteredShipments(QueryOperations queryOperations)
        {
            ShipmentAPiHelper.AddFilters(queryOperations, tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);

            GenericFilter genericFilter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            ShipmentCustomFilter customfilters = new ShipmentCustomFilter(tenant);
            IQueryable<ShipmentDataView> shipmentDataViews = shipmentRepository.GetShipmentViewsByTenant(tenant);
            shipmentDataViews = customfilters.GetFilteredQuery(queryOperations, shipmentDataViews);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            shipmentDataViews = genericFilter.GetFilteredQuery<ShipmentDataView>(nonListQueryOperation, shipmentDataViews);
            int skippedShipments = queryOperations.PageIndex;

            List<ShipmentByReferences> entityLists = this.GetIQueryableShipmentList(shipmentDataViews);

            return entityLists;
        }
        public List<ShipmentByReferences> GetIQueryableShipmentList(IQueryable<ShipmentDataView> shipments)
        {
            List<ShipmentByReferences> myResult = (from f in shipments
                           select new ShipmentByReferences()
                           {
                               ShipmentNumber = f.ShipmentNumber,
                               CreateDate = f.CreateDateTime,
                               Routing = f.Routing,
                               ShipperReference1 = f.ShipperReference1,
                               ShipperReference2 = f.ShipperReference2,
                               ConsigneeReference1 = f.ConsigneeReference1,
                               ConsigneeReference2 = f.ConsigneeReference2,
                               AgentReference1 = f.AgentReference1,
                               AgentReference2 = f.AgentReference2,
                               ShipperNotExporterReference1 = f.ShipperNotExporterReference1,
                               ShipperNotExporterReference2 = f.ShipperNotExporterReference2,
                               ConsigneeNotImporterReference = f.ConsigneeNotImporterReference,
                               ForwarderReference = f.FreightForwarderReference,
                           }).ToList();


            foreach(ShipmentByReferences item in myResult)
            {
                ShipmentDataView myShipment = shipments.Where(d => d.ShipmentNumber == item.ShipmentNumber).FirstOrDefault();
                item.Shipper = this.GetPartnerObject(myShipment.ShipperId);
                item.Consignee = this.GetPartnerObject(myShipment.ConsigneeId);
                item.Agent = this.GetPartnerObject(myShipment.AgentId);
                item.ShipperNotExporter = this.GetPartnerObject(myShipment.ShipperNotExporterId);
                item.ConsigneeNotImporter = this.GetPartnerObject(myShipment.ConsigneeNotImporterId);
                item.Forwarder = this.GetPartnerObject(myShipment.FreightForwarderId);
                item.Customer = this.GetPartnerObject(myShipment.CustomerId);
            }

            return myResult;
        }

        private CardByReferences GetPartnerObject(string partnerId)
        {
            CardByReferences cardByReferences = new CardByReferences();
            Card card = cardRepository.GetSingleCard(partnerId, tenant);
            if (card != null)
            {
                cardByReferences.EnglishName = card.EnglishName;
                cardByReferences.Code = card.Code;
            }

            return cardByReferences;
        }
    }
}