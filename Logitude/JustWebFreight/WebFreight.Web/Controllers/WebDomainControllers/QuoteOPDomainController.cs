using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data.Repsitories;
using Logitude.BL.Helpers;
using Logitude.BL.QuoteModel.BusinessUnitFilters;
using Logitude.BL.Security;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class QuoteOPDomainController : ApiController
    {
        public HttpResponseMessage GetQuotesCounts(string ownerId, string businessUnitId, string directionId, string transportModeId, string RecordsTypeCode)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("QuoteOP", "READ", tenant);

                ownerId = this.FixFilter(ownerId);
                businessUnitId = this.FixFilter(businessUnitId);
                RecordsTypeCode = this.FixFilter(RecordsTypeCode);

                var quoteOPRepository = new QuoteOPRepository(tenant);
                var businessUnitFilter = new QuoteBusinessUnitFilter(tenant);

                IQueryable<QuoteOP> allQuotes = quoteOPRepository.GetQuotes(tenant);
                allQuotes = BranchPermitionsFilter.AddUserBranchRestrictionFilters<QuoteOP>(new QueryOperations(), allQuotes, tenant);
                allQuotes = ProductPermitionsFilter.AddUserProductRestrictionFilters<QuoteOP>(new QueryOperations(), allQuotes, tenant);
                allQuotes = businessUnitFilter.RunFilter(allQuotes);

                IQueryable<QuoteOPFollowUpDataView> allFollowups = quoteRepository.GetQuoteFollowUpDataViewByTenant(tenant);
                allFollowups = BranchPermitionsFilter.AddUserBranchRestrictionFilters<QuoteFollowUpDataView>(new QueryOperations(), allFollowups, tenant);
                allFollowups = ProductPermitionsFilter.AddUserProductRestrictionFilters<QuoteFollowUpDataView>(new QueryOperations(), allFollowups, tenant);
                allFollowups = businessUnitFilter.RunFilter(allFollowups);

                if (!string.IsNullOrEmpty(directionId))
                {
                    allQuotes = allQuotes.Where(d => d.DirectionId == directionId);
                    allFollowups = allFollowups.Where(d => d.DirectionId == directionId);
                }

                if (!string.IsNullOrEmpty(transportModeId))
                {
                    allQuotes = allQuotes.Where(d => d.TransportModeId == transportModeId);
                    allFollowups = allFollowups.Where(d => d.TransportModeId == transportModeId);
                }

                CRMSummary myResult = new CRMSummary();

                if (RecordsTypeCode == "C")
                {
                    if (!string.IsNullOrEmpty(ownerId))
                    {
                        allQuotes = allQuotes.Where(d => d.CreatedByUserId == ownerId);
                        allFollowups = allFollowups.Where(d => d.CreatedByUserId == ownerId);
                    }
                }

                else
                {
                    if (!string.IsNullOrEmpty(ownerId))
                    {
                        allQuotes = allQuotes.Where(d => d.SalesmanUserId == ownerId);
                        allFollowups = allFollowups.Where(d => d.SalesmanUserId == ownerId);
                    }

                    if (!string.IsNullOrEmpty(businessUnitId))
                    {
                        allQuotes = allQuotes.Where(d => d.BusinessUnitId == businessUnitId);
                        allFollowups = allFollowups.Where(d => d.BusinessUnitId == businessUnitId);
                    }
                }

                string loggedUserId = this.GetLoggedUserId(loggedUserEmail, tenant);

                myResult.Quotes_Cancelled = allQuotes.Where(d => d.IsCancelled == true).Count();

                allQuotes = allQuotes.Where(d => d.IsCancelled == false);

                myResult.Quotes_All = allQuotes.Count();
                myResult.Quotes_My = allQuotes.Where(d => d.SalesmanUserId == loggedUserId).Count();
                myResult.Quotes_Created = allQuotes.Where(d => d.Stage.Code == "QTCR").Count();
                myResult.Quotes_Draft = allQuotes.Where(d => d.Stage.Code == "QTDR").Count();
                myResult.Quotes_Expired = allQuotes.Where(d => d.ExpirationDate < DateTime.Now && !d.IsClosed).Count();
                myResult.Quotes_Accepted = allQuotes.Where(d => d.Stage.Code == "QTAC").Count();
                myResult.Quotes_AcceptedNOShip = allQuotes.Where(d => d.Stage.Code == "QTAC" && (d.UsageCount == 0 || d.UsageCount == null)).Count();
                myResult.Quotes_Sent = allQuotes.Where(d => d.Stage.Code == "QTST").Count();

                myResult.Quotes_AllFollowups = allFollowups.Count();
                myResult.Quotes_MyFollowups = allFollowups.Where(d => d.FollowUpOwnerId == loggedUserId).Count();

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetRecentQuotes(string ownerId, string businessUnitId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                if (ownerId == "all" || ownerId == "null")
                {
                    ownerId = null;
                }

                if (businessUnitId == "all" || businessUnitId == "null")
                {
                    businessUnitId = null;
                }

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("QuoteOP", "READ", tenant);

                ContactQuery contactQuery = new ContactQuery(tenant);
                ContactPM contact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);

                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
                ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("QuoteOP", 0, true);

                QuoteQuery quoteQuery = new QuoteQuery(tenant);
                IQueryable<QuoteList> first = quoteQuery.GetRecentEntityLists(ownerId, businessUnitId, tenant, contact.Id, objectTable.Id).AsQueryable();

                IQueryable<QuoteList> myResult = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), first, tenant);
                myResult = ProductPermitionsFilter.AddUserProductRestrictionFilters<QuoteList>(new QueryOperations(), myResult, tenant);

                QuoteBusinessUnitFilter businessUnitFilter = new QuoteBusinessUnitFilter(tenant);
                myResult = businessUnitFilter.RunFilter(myResult);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetStageFunnelData(string OwnerId, string BusinessUnitId, string RecordsTypeCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("QuoteOP", "READ", tenant);

                OwnerId = this.FixFilter(OwnerId);
                BusinessUnitId = this.FixFilter(BusinessUnitId);
                RecordsTypeCode = this.FixFilter(RecordsTypeCode);

                QuoteQuery quoteQuery = new QuoteQuery(tenant);

                List<ChartingDataClass> data = quoteQuery.GetStageFunnelData(OwnerId, BusinessUnitId, tenant, RecordsTypeCode);

                List<ChartingDataClass> myResult = new List<ChartingDataClass>();

                foreach (ChartingDataClass item in data)
                {
                    myResult.Add(new ChartingDataClass()
                    {
                        Id = item.Id,
                        LabelProperty = item.LabelProperty,
                        DecimalProperty = item.DecimalProperty,
                        IntegerProperty = item.IntegerProperty,
                        OwnerId = OwnerId,
                        BusinessUnitId = BusinessUnitId,
                        GroupedId = item.GroupedId,
                        RecordsTypeCode = RecordsTypeCode,
                    });
                }

                myResult = myResult.OrderBy(d => d.IntegerProperty).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetConnectQuotesToOpportunity(string opportunityId, string quotesIds)
        {
            try
            {

                if (opportunityId == "null")
                    opportunityId = null;


                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("QuoteStage", "READ", authToken.Tenant);

                QuotesDomainService domainService = new QuotesDomainService();
                string[] Ids = quotesIds.Split(':');
                domainService.ConnectQuotesToOpportunity(opportunityId, Ids.ToList(), authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetSingleQuoteStageListByCode(string code)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("QuoteStage", "READ", authToken.Tenant);

                QuoteStageQuery entityQuery = new QuoteStageQuery(authToken.Tenant);
                QuoteStageList myResult = entityQuery.GetSingleListByCode(code, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetQuotesByOpportunityId(string oportunityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                QuotesDomainService domainService = new QuotesDomainService();
                List<QuoteList> myResult = domainService.GetQuotesByOpportunityId(oportunityId, authToken.Tenant).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }



        }
        public HttpResponseMessage PostQuoteAutomaticSubject(QuotePM entityPM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("QuoteOP", "READ", tenant);

                QuoteSubjectService iSubjectService = new QuoteSubjectService(entityPM);
                string mySubject = iSubjectService.GetSubject();

                return Request.CreateResponse(HttpStatusCode.OK, mySubject);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetActivitiesByQuoteId(string entityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                CRMDomainService domainService = new CRMDomainService();
                List<ActivityList> myResult = domainService.GetActivitiesByQuoteId(entityId, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetIsQuoteConnectedToShipment(string quoteId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                ShipmentRepository shipmentRepository = new ShipmentRepository(authToken.Tenant);

                bool result = shipmentRepository.IsQuoteConnectedToShipment(quoteId, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetQuoteSettings()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                QuoteSettingPM entityPM = null;
                QuoteSettingRepository myRepository = new QuoteSettingRepository(tenant);
                QuoteSetting myPOCO = myRepository.GetSingleQuoteSetting(tenant);

                if (myPOCO == null)
                {
                    entityPM = new QuoteSettingPM()
                    {
                        Tenant = tenant,
                        AutomaticallyCloseDays = 30,
                        QuoteExpirationDays = 30,
                    };
                }

                else
                {
                    entityPM = new QuoteSettingPM()
                    {
                        Id = myPOCO.Id,
                        Tenant = myPOCO.Tenant,
                        CopyShipper = myPOCO.CopyShipper,
                        CopyConsignee = myPOCO.CopyConsignee,
                        CopyMainCarriage = myPOCO.CopyMainCarriage,
                        CopyPickup = myPOCO.CopyPickup,
                        CopyDelivery = myPOCO.CopyDelivery,
                        CopyChargesTypes = myPOCO.CopyChargesTypes,
                        CopyChargesCost = myPOCO.CopyChargesCost,
                        CopyChargesSale = myPOCO.CopyChargesSale,
                        EditMainCarriage = myPOCO.EditMainCarriage,
                        CopyAgent = myPOCO.CopyAgent,
                        CopyNotify = myPOCO.CopyNotify,
                        IsSaleAsCostCurrency = myPOCO.IsSaleAsCostCurrency,
                        CopyExchangeRates = myPOCO.CopyExchangeRates,
                        AutomaticallyCloseDays = myPOCO.AutomaticallyCloseDays,

                        IsMultiCurrency = myPOCO.IsMultiCurrency,

                        QuoteExpirationDays = myPOCO.QuoteExpirationDays,
                    };
                }

                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage Put(QuoteSettingPM entityPM)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    if (entityPM != null)
                    {
                        IQuotesContext objectContext = QuotesContext.GetContext(tenant);
                        QuoteSettingService myService = new QuoteSettingService(objectContext, tenant);
                        QuoteSettingRepository myRepository = new QuoteSettingRepository(objectContext);
                        QuoteSetting myPOCO = myRepository.GetSingleQuoteSetting(tenant);

                        if (myPOCO == null)
                        {
                            myService.Create(entityPM);
                        }

                        else if (entityPM.Id == null)
                        {
                            myService.Create(entityPM);
                        }

                        else
                        {
                            myService.Update(entityPM);
                        }
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private string FixFilter(string filter)
        {
            string myResult = filter;

            if (myResult != null)
            {
                switch (myResult.ToLower())
                {
                    case "all":
                    case "null":
                    case "undefined":
                        {
                            myResult = null;
                            break;
                        }
                }
            }

            return myResult;
        }
        private string GetLoggedUserId(string loggedUserEmail, int tenant)
        {
            string loggedUserId = null;
            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM loggedContactPM = contactQuery.GetContactByNameAndTenant(loggedUserEmail, tenant, true);
            if (loggedContactPM == null)
            {
                loggedContactPM = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
            }

            if (loggedContactPM != null)
            {
                loggedUserId = loggedContactPM.Id;
            }

            return loggedUserId;
        }

        public HttpResponseMessage GetQuoteConnectedEntities(string quoteId, string opportunityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                List<QuoteOPConnectedEntity> myResult = new List<QuoteOPConnectedEntity>();

                IShipmentsContext shipmentContext = ShipmentsContext.GetContext(tenant);
                ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentContext);
                IQueryable<ShipmentDataView> myShipments = shipmentRepository.GetShipmentsByQuoteId(quoteId, tenant);
                foreach (ShipmentDataView item in myShipments)
                {
                    myResult.Add(new QuoteOPConnectedEntity()
                    {
                        EntityId = item.Id,
                        EntityNumber = item.ShipmentNumber,
                        ObjectTable = "Shipment",
                        EntityStatus = item.StatusName,
                        ShipmentType = item.ShipmentTypeName + " " + item.ShipmentLevelName,
                        OpenDate = item.CreateDateTime,
                        House = item.House,
                        Master = item.Master,
                        Customer = item.CustomerName,
                        From = item.ShipmentLevelCode == "H" ? item.FromPortCode : item.MainCarriageFromPortCode,
                        To = item.ShipmentLevelCode == "H" ? item.ToPortCode : item.MainCarriageFinalDestinationPortCode,
                        GrossWeight = item.GrossWeight,
                        VolumeInKG = item.Volume,
                    });
                }

                ICRMContext crmContext = CRMContext.GetContext(tenant);
                TicketListQueryService listService = new TicketListQueryService(crmContext);
                List<TicketList> myTickets = listService.GetTicketListByQuoteIdList(quoteId, tenant);
                CustomFieldResolver customFieldResolver = new CustomFieldResolver();
                customFieldResolver.SetCustomFieldsValues("Ticket", tenant, myTickets.Cast<object>().ToList());

                foreach (TicketList item in myTickets)
                {
                    myResult.Add(new QuoteOPConnectedEntity()
                    {
                        EntityId = item.Id,
                        ObjectTable = "Ticket",
                        EntityNumber = item.TicketNumber,
                        EntityStatus = item.StageName,
                    });
                }

                if (!string.IsNullOrEmpty(opportunityId))
                {
                    OpportunityListQueryService opportunityListQueryService = new OpportunityListQueryService(crmContext);
                    OpportunityList myOpportunity = opportunityListQueryService.GetSingleOpportunityByQuote(opportunityId, tenant);
                    if (myOpportunity != null)
                    {
                        myResult.Add(new QuoteOPConnectedEntity()
                        {
                            EntityId = myOpportunity.Id,
                            ObjectTable = "Opportunity",
                            EntityStatus = myOpportunity.StageName,
                            EntityOwner = myOpportunity.OwnerName,
                            EntityClosingDate = myOpportunity.EstimatedClosingDate,
                            EntityNumber = myOpportunity.Subject,
                        });
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


    }
}

public class QuoteOPConnectedEntity
{
    public string EntityId { get; set; }
    public string EntityNumber { get; set; }
    public string ObjectTable { get; set; }
    public string EntityStatus { get; set; }
    public string ShipmentType { get; set; }
    public DateTime? OpenDate { get; set; }
    public string House { get; set; }
    public string Master { get; set; }
    public string Customer { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public Double? GrossWeight { get; set; }
    public Double? VolumeInKG { get; set; }
    public string EntityOwner { get; set; }
    public DateTime? EntityClosingDate { get; set; }
}