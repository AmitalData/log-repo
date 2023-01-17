using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Azure;
using WebFreight.Web.CommonDataModel.DomainServices;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;
using Simplog.Data.QuoteModel.Repositories;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Server.Tools.Helpers;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.BusinessUnitFilters;
using System.Web;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.BusinessUnitFilters;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.QuoteModel.CustomFilters;
using Logitude.BL.QuoteModel;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
using Logitude.BL.CommonDataModel.BusinessUnitFilters;
using System.Data.Entity.Core;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityUpdateServices;
using Logitude.CRM.Data;

namespace WebFreight.Web.QuoteModel.DomainServices
{
    //[RequiresAuthentication]
    [EnableClientAccess()]
    public partial class QuotesDomainService : LogitudeDomainService
    {
        private FollowUpQuery followUpQuery;
        private IQuotesContext objectContext;
        private QuoteRepository quoteRepository;
        private QuoteQuery quoteQuery;
        private QuoteTypeQuery quoteTypeQuery;
        private MarkUpTypeQuery markUpTypeQuery;
        private ValidByTypeQuery validByTypeQuery;
        private QuoteCustomerTypeQuery quoteCustomerTypeQuery;
        private QuoteClosingReasonQuery quoteClosingReasonQuery;

        public void InsertQuotePM(QuotePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckFeatureAccessLevelPermission("Quote", "NEW", entityPM.SalesmanUserId, entityPM.BusinessUnitId, entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(entityPM.Tenant);
            }

            QuoteService service = new QuoteService(objectContext, entityPM.Tenant);
            service.Create(entityPM);
        }

        public void UpdateQuotePM(QuotePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);

            quoteRepository = new QuoteRepository(entityPM.Tenant);
            Quote entity_Poco = quoteRepository.GetSingleQuote(entityPM.Id, entityPM.Tenant);

            SecurityUtility.CheckFeatureAccessLevelPermission("Quote", "UPDATE", entity_Poco.SalesmanUserId, entity_Poco.BusinessUnitId, entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(entityPM.Tenant);
            }

            List<QuoteChargePM> quoteChargesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.QuoteCharges).Cast<QuoteChargePM>().ToList();
            foreach (QuoteChargePM itemPM in quoteChargesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    case ChangeOperation.Update:
                        {
                            itemPM.ChangeSetOp = ChangeSetOperation.Update;

                            itemPM.QuoteChargePriceStepsChangeSet = ChangeSet.GetAssociatedChanges(itemPM, d => d.QuoteChargePriceSteps).Cast<QuotePriceStepsPM>().ToList();
                            foreach (QuotePriceStepsPM insideItemPM in itemPM.QuoteChargePriceStepsChangeSet)
                            {
                                switch (ChangeSet.GetChangeOperation(insideItemPM))
                                {
                                    case ChangeOperation.Insert: { insideItemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                                    case ChangeOperation.Update: { insideItemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                                    case ChangeOperation.Delete: { insideItemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                                    default: { insideItemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                                }
                            }

                            break;
                        }

                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            List<QuoteFollowUpPM> quoteFollowUpsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.FollowUps).Cast<QuoteFollowUpPM>().ToList();
            foreach (QuoteFollowUpPM itemPM in quoteFollowUpsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            List<QuotePackagePM> quotePackageChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.QuotePackages).Cast<QuotePackagePM>().ToList();
            foreach (QuotePackagePM itemPM in quotePackageChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            List<QuoteDocumentVersionPM> quoteDocumentVersionChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.QuoteDocumentVersions).Cast<QuoteDocumentVersionPM>().ToList();
            foreach (QuoteDocumentVersionPM itemPM in quoteDocumentVersionChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            QuoteService service = new QuoteService(objectContext, entityPM.Tenant);
            service.SetChangeSet(quoteChargesChangeSet, quoteFollowUpsChangeSet, quotePackageChangeSet, quoteDocumentVersionChangeSet);
            service.Update(entityPM);

            if (this.ChangeSet != null)
            {
                this.ChangeSet.Associate(entityPM, service.entityPoco, MapQuotePMToQuote);
            }            
        }

        public void MapQuotePMToQuote(QuotePM quotepm, Quote quote)
        {
            quotepm.LastModified = quote.LastModified;

            string myIncotermCode = null;
            string myIncotermName = null;
            if (!string.IsNullOrEmpty(quote.IncotermId))
            {
                IncotermRepository incotermRepository = new IncotermRepository(quote.Tenant);
                Incoterm incoterm = incotermRepository.GetSingleIncoterm(quote.IncotermId, quote.Tenant);
                if (incoterm != null)
                {
                    myIncotermCode = incoterm.Code;
                    myIncotermName = incoterm.Name;
                }
            }

            quotepm.IncotermCode = myIncotermCode;
            quotepm.IncotermName = myIncotermName;
        }

        public QuotePM GetSingleQuotePMById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Quote", "READ", tenant);

            quoteQuery = new QuoteQuery(tenant);
            return quoteQuery.GetSinglePM(id, tenant);
        }

        public IQueryable<QuoteList> GetShipmentFollowUpsForQuote(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Quote", "READ", tenant);
 
            FollowUpRepository followUpsRepository = new FollowUpRepository(tenant);
            followUpQuery = new FollowUpQuery(followUpsRepository);

            IQueryable<QuoteList> myResult = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), followUpQuery.GetFollowUpsForQuotes(tenant), tenant);

            myResult = ProductPermitionsFilter.AddUserProductRestrictionFilters<QuoteList>(new QueryOperations(), myResult, tenant);

            QuoteBusinessUnitFilter filter = new QuoteBusinessUnitFilter(tenant);
            myResult = filter.RunFilter(myResult);

            return myResult;            
        }

        public IQueryable<QuoteList> GetQuotesByOpportunityId(string oportunityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Quote", "READ", tenant);

            quoteRepository = new QuoteRepository(tenant);
            quoteQuery = new QuoteQuery(quoteRepository);

            IQueryable<Quote> myResult = quoteRepository.GetQuotes(tenant).Where(d => d.OpportunityId == oportunityId);

            myResult = ProductPermitionsFilter.AddUserProductRestrictionFilters<Quote>(new QueryOperations(), myResult, tenant);

            QuoteBusinessUnitFilter businessUnitFilter = new QuoteBusinessUnitFilter(tenant);
            myResult = businessUnitFilter.RunFilter(myResult);

            IQueryable<QuoteList> query2 = quoteQuery.GetIQueryableEntityList(myResult);

            return query2;
        }

        public int GetShipmentFollowUpsCountForQuote(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            FollowUpRepository followUpsRepository = new FollowUpRepository(tenant);

            IQueryable<QuoteList> myResult = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), followUpQuery.GetFollowUpsForQuotes(tenant), tenant);

            myResult = ProductPermitionsFilter.AddUserProductRestrictionFilters<QuoteList>(new QueryOperations(), myResult, tenant);

            QuoteBusinessUnitFilter filter = new QuoteBusinessUnitFilter(tenant);
            myResult = filter.RunFilter(myResult);

            return myResult.Count();
        }

        public QuotePM GetSingleQuotePresentationModel(string quotePMId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Quote", "READ", tenant);

            quoteQuery = new QuoteQuery(tenant);
            QuotePM result = quoteQuery.GetSinglePM(quotePMId, tenant);
            return result;
        }

        public IQueryable<QuoteDocumentVersionPM> GetQuoteDocumentVersionsByQuoteId(string quoteId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
             
            QuoteDocumentVersionQuery quoteDocumentVersionQuery = new QuoteDocumentVersionQuery(tenant);
            return quoteDocumentVersionQuery.GetQuoteDocumentVersionPMsByQuoteId(quoteId, tenant);
        }

        private void AddRestrictionFilters(QueryOperations queryOperations, string objectTableName, int tenant)
        {
            RestrictionRepository restrictionsRep = new RestrictionRepository(tenant);

            ObjectTablePM objectTable = ObjectTableQuery.GetObjectTableByCode(objectTableName, tenant);
            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactTenantQuery contactTenantQuery = new ContactTenantQuery(tenant);
            ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);
            ContactTenantPM contactTenant = null;
            if (contact == null)
            {
                contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), 0, true);
            }
            contactTenant = contactTenantQuery.GetContactTenantForUser(contact.Id, tenant);
            if (contactTenant != null)
            {

                RestrictionQuery restrictionQuery = new RestrictionQuery(tenant);
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
        }

        public QuoteList GetQuoteListsForFollowUps(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Quote", "READ", tenant);

            FollowUpRepository followUpsRepository = new FollowUpRepository(tenant);
            followUpQuery = new FollowUpQuery(followUpsRepository);

            QuoteList myResult = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), followUpQuery.GetQuoteFollowUpsByQuoteId(id, tenant), tenant);

            myResult = ProductPermitionsFilter.AddUserProductRestrictionFilters<QuoteList>(new QueryOperations(), myResult, tenant);

            QuoteBusinessUnitFilter filter = new QuoteBusinessUnitFilter(tenant);
            myResult = filter.RunFilter(myResult);

            return myResult;
        }

        public QuoteList GetSingleQuoteList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Quote", "READ", tenant);

            quoteRepository = new QuoteRepository(tenant);
            QuoteList quoteList = null;
            Quote f = quoteRepository.GetSingleQuote(id, tenant);

            if (f != null)
            {
                QuoteBusinessUnitFilter filter = new QuoteBusinessUnitFilter(tenant);
                f = filter.RunFilter(f);
            }

            if (f != null)
            {
                quoteList = new QuoteList()
                {
                    TransportModeName = f.TransportMode.Name,
                    IsClosed = f.IsClosed,
                    DirectionName = f.Direction.Name,
                    Id = f.Id,
                    Shipper = f.ShipperName,
                    Consignee = f.ConsigneeName,
                    QuoteViewId = f.Id,
                    CustomerName = f.CustomerName,
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
                    ShipperReference1 = f.ShipperReference1,
                    LastModified = f.LastModified,
                    CreatedByUser = f.CreatedByUser.Contact.EnglishName,
                    UpdatedByUser = f.UpdatedByUser.Contact.EnglishName,
                    QuoteTypeCode = f.QuoteTypeCode,
                    ShipmentType = f.ShipmentType == null ? null : f.ShipmentType.Name,
                    DirectionId = f.DirectionId,
                    TransportModeId = f.TransportModeId,
                    ShipmentTypeId = f.ShipmentTypeId,
                    ShipperId = f.ShipperId,
                    FromPortId = f.FromPortId,
                    ToPortId = f.ToPortId,
                    QuoteNumber = f.QuoteNumber,
                    OpenDate = f.OpenDate,
                    ExpirationDate = f.ExpirationDate,
                    MainCarriageCarrierName = f.MainCarriageCarrierCard == null ? "" : f.MainCarriageCarrierCard.EnglishName,
                    MainCarriageCarrierId = f.MainCarriageCarrierId,
                    QuoteTypeName = f.QuoteType.Name,
                    CarrierName = f.MainCarriageCarrierCard == null ? "" : f.MainCarriageCarrierCard.EnglishName,
                    ChargeableWeight = f.ChargeableWeight,
                    GrossWeight = f.GrossWeight,
                    IsCancelled = f.IsCancelled,
                    SearchFields = f.SearchFields,
                    Notes = f.Notes,
                    BranchId = f.BranchId,
                    DepartmentId = f.DepartmentId,
                    BranchName = f.Branch == null ? null : f.Branch.EnglishName,
                    DepartmentName = f.Department == null ? null : f.Department.EnglishName,
                    NumberOfContainers = f.NumberOfContainers,
                    FromPartnerId = f.FromPartnerId,
                    ToPartnerId = f.ToPartnerId,
                    FromPartnerAddressId = f.FromPartnerAddressId,
                    ToPartnerAddressId = f.ToPartnerAddressId,
                    SentDate = f.SentDate,
                    AcceptedDate = f.AcceptedDate,
                    DeclinedDate = f.DeclinedDate,
                    StageId = f.StageId,
                    StageName = f.Stage == null ? "" : f.Stage.Name,
                    StageMaxDays = f.Stage == null ? null : f.Stage.MaxDays,
                    StageDueDate = f.StageDueDate,
                    RatingCode = f.RatingCode,
                    RatingName = f.Rating == null ? "" : f.Rating.Name,
                    RatingIndexOrder = f.Rating == null ? 0 : f.Rating.IndexOrder,
                    LastActivityDate = f.LastActivityDate,
                    LastActivitySubject = f.LastActivitySubject,
                    LastActivityTypeCode = f.LastActivityTypeCode,
                    NextActivityDate = f.NextActivityDate,
                    NextActivitySubject = f.NextActivitySubject,
                    NextActivityTypeCode = f.NextActivityTypeCode,                   
                    LastActivityTypeName = f.LastActivityTypeCode == "CL" ? "Call" : (f.LastActivityTypeCode == "TS" ? "Task" : (f.LastActivityTypeCode == "AP" ? "Appointment" : (f.LastActivityTypeCode == "EO" ? "Email Out" : "Email In"))),
                    NextActivityTypeName = f.NextActivityTypeCode == "CL" ? "Call" : (f.NextActivityTypeCode == "TS" ? "Task" : (f.NextActivityTypeCode == "AP" ? "Appointment" : (f.NextActivityTypeCode == "EO" ? "Email Out" : "Email In"))),
                    OpportunityId = f.OpportunityId,
                    IsAutomaticallyClosed = f.IsAutomaticallyClosed,
                    AutomaticallyCloseDate = f.AutomaticallyCloseDate,
                    AutomaticallyCloseDays = f.AutomaticallyCloseDays,
                    UpdatedByUserId = f.UpdatedByUserId,
                    UpdateDate = f.UpdateDate,
                    BusinessUnitId = f.BusinessUnitId,
                    BusinessUnitName = f.BusinessUnit!=null?f.BusinessUnit.Name:null,
                    FromPortName = f.FromPort == null ? "" : f.FromPort.EnglishName,
                    ToPortName = f.ToPort == null ? "" : f.ToPort.EnglishName,
                    QuoteClosingReasonCode = f.QuoteClosingReasonCode,
                    QuoteClosingReasonName = f.QuoteClosingReason == null ? null : f.QuoteClosingReason.Name,
                    IncotermCode = f.Incoterm == null ? "" : f.Incoterm.Code,
                    LastStageDate = f.LastStageDate,

                    FromPort = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                    (f.FromPartnerAddress != null ? f.FromPartnerAddress.City : "")
                    :
                    (f.FromPort != null ? f.FromPort.Code : ""),

                    FromCountryCode = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                    ((f.FromPartnerAddress != null && f.FromPartnerAddress.Country != null ? f.FromPartnerAddress.Country.Code : ""))
                    :
                    ((f.FromPort != null && f.FromPort.Country != null ? f.FromPort.Country.Code : "")),

                    FromPortCountry = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                    ((f.FromPartnerAddress != null && f.FromPartnerAddress.Country != null ? f.FromPartnerAddress.Country.EnglishName : ""))
                    :
                    ((f.FromPort != null && f.FromPort.Country != null ? f.FromPort.Country.EnglishName : "")),

                    ToPort = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                    (f.ToPartnerAddress != null ? f.ToPartnerAddress.City : "")
                    :
                    (f.ToPort != null ? f.ToPort.Code : ""),

                    ToCountryCode = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                    ((f.ToPartnerAddress != null && f.ToPartnerAddress.Country != null ? f.ToPartnerAddress.Country.Code : ""))
                    :
                    ((f.ToPort != null && f.ToPort.Country != null ? f.ToPort.Country.Code : "")),

                    ToPortCountry = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                    ((f.ToPartnerAddress != null && f.ToPartnerAddress.Country != null ? f.ToPartnerAddress.Country.EnglishName : ""))
                    :
                    ((f.ToPort != null && f.ToPort.Country != null ? f.ToPort.Country.EnglishName : "")),

                    Routing = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                    ((f.FromPartnerAddress == null ? "" : f.FromPartnerAddress.City) + " > " + (f.ToPartnerAddress == null ? "" : f.ToPartnerAddress.City))
                    :
                    ((f.FromPort == null ? "" : f.FromPort.Code) + " > " + (f.ToPort == null ? "" : f.ToPort.Code)),

                    Subject = f.Subject,
                    IsSubjectEdited = f.IsSubjectEdited,
                    SalesmanUserId = f.SalesmanUserId,
                    Salesman = f.SalesmanUser == null ? "" : (f.SalesmanUser.Contact == null ? "" : f.SalesmanUser.Contact.EnglishName),
                    SalesmanName = f.SalesmanUser == null ? "" : (f.SalesmanUser.Contact == null ? "" : f.SalesmanUser.Contact.EnglishName),
                    TransitTime = f.TransitTime,
                    DepartureFrequency = f.DepartureFrequency,
                    ETD = f.ETD,
                    ETA = f.ETA,
                    AgentId = f.AgentId,
                    AgentAddressId = f.AgentAddressId,
                    AgentContactId = f.AgentContactId,
                    AgentReference1 = f.AgentReference1,
                    AgentReference2 = f.AgentReference2,
                    NumberOfPackages = f.NumberOfPackages,
                    AgentName = f.AgentCard == null ? null : f.AgentCard.EnglishName,
                    TEU = f.TEU,
                    IsSaleCurrencySameAsCost = f.IsSaleCurrencySameAsCost,
                };

                List<ObjectField> customFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName("Quote", tenant).ToList();

                CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
                foreach (ObjectField field in customFields)
                {
                    PropertyInfo propInfo = typeof(QuoteList).GetProperty(field.FieldName);
                    object newValue = customFieldResolver.GetFieldValue(quoteList, field, tenant);
                    propInfo.SetValue(quoteList, newValue, null);
                }
            }

            return quoteList;
        }

        public IQueryable<QuoteList> GetFollowUpsByQuotesFilter(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ContactQuery contactRep = new ContactQuery(tenant);
            ContactPM contact = contactRep.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);
            quoteRepository = new QuoteRepository(tenant);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            QuoteFollowUpsCustomFilter customfilters = new QuoteFollowUpsCustomFilter(tenant);
            
            BranchPermitionsFilter.AddUserBranchRestrictionFilters(listQueryOperation, tenant);
            ProductPermitionsFilter.AddUserProductRestrictionFilters(listQueryOperation, tenant);

            IQueryable<QuoteFollowUpDataView> quoteFollowUps = quoteRepository.GetQuoteFollowUpDataViewByTenant(tenant);
            quoteFollowUps = customfilters.GetQuoteFollowUpFilteredQuery(queryOperations, quoteFollowUps);
            int skippedQuotes = queryOperations.PageIndex;

            IQueryable<QuoteList> query2 = from f in quoteFollowUps
                                           select new QuoteList()
                                           {
                                               IsClosed = f.IsClosed,
                                               Id = f.Id,
                                               Shipper = f.ShipperName,
                                               Consignee = f.ConsigneeName,
                                               QuoteViewId = f.Id + f.FollowUpId,
                                               FromPort = f.FromPortCode,
                                               ToPort = f.ToPortCode,
                                               CustomerName = f.CustomerName,
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
                                               ShipperReference1 = f.ShipperReference1,
                                               LastModified = f.LastModified,
                                               QuoteTypeCode = f.QuoteTypeCode,
                                               ShipmentType = f.ShipmentTypeName,
                                               DirectionId = f.DirectionId,
                                               TransportModeId = f.TransportModeId,
                                               ShipmentTypeId = f.ShipmentTypeId,
                                               ShipperId = f.ShipperId,
                                               ShipperName = f.ShipperName,
                                               FromPortId = f.FromPortId,
                                               ToPortId = f.ToPortId,
                                               QuoteNumber = f.QuoteNumber,
                                               OpenDate = f.OpenDate,
                                               ExpirationDate = f.ExpirationDate,
                                               MainCarriageCarrierName = f.MainCarriageCarrierName,
                                               MainCarriageCarrierId = f.MainCarriageCarrierId,
                                               QuoteTypeName = f.QuoteTypeName,
                                               CarrierName = f.MainCarriageCarrierName,
                                               ChargeableWeight = f.ChargeableWeight,
                                               GrossWeight = f.GrossWeight,
                                               IsCancelled = f.IsCancelled,
                                               SearchFields = f.SearchFields,
                                               Notes = f.Notes,
                                               BranchId = f.BranchId,
                                               DepartmentId = f.DepartmentId,
                                               NumberOfContainers = f.NumberOfContainers,
                                               FromPartnerId = f.FromPartnerId,
                                               ToPartnerId = f.ToPartnerId,
                                               FromPartnerAddressId = f.FromPartnerAddressId,
                                               ToPartnerAddressId = f.ToPartnerAddressId,
                                               FollowUpDate = f.FollowUpDate,
                                               FollowUpType = f.FollowUpType,
                                               FollowUpNotes = f.FollowUpNotes,
                                               FollowUpOwner = f.FollowUpOwner,                                                                                             
                                               Subject = f.Subject,
                                               IsSubjectEdited = f.IsSubjectEdited,
                                               StageId = f.StageId,
                                               StageName = f.StageName,
                                               StageDueDate = f.StageDueDate,
                                               RatingCode = f.RatingCode,
                                               LastActivityDate = f.LastActivityDate,
                                               LastActivitySubject = f.LastActivitySubject,
                                               LastActivityTypeCode = f.LastActivityTypeCode,
                                               NextActivityDate = f.NextActivityDate,
                                               NextActivitySubject = f.NextActivitySubject,
                                               NextActivityTypeCode = f.NextActivityTypeCode,
                                               RatingName = f.RatingCode == "C" ? "Cold" : (f.RatingCode == "H" ? "Hot" : f.RatingCode == "N" ? "Neutral" : "Warm"),
                                               LastActivityTypeName = f.LastActivityTypeCode == "CL" ? "Call" : (f.LastActivityTypeCode == "TS" ? "Task" : (f.LastActivityTypeCode == "AP" ? "Appointment" : (f.LastActivityTypeCode == "EO" ? "Email Out" : "Email In"))),
                                               NextActivityTypeName = f.NextActivityTypeCode == "CL" ? "Call" : (f.NextActivityTypeCode == "TS" ? "Task" : (f.NextActivityTypeCode == "AP" ? "Appointment" : (f.NextActivityTypeCode == "EO" ? "Email Out" : "Email In"))),
                                               OpportunityId = f.OpportunityId,
                                               IsAutomaticallyClosed = f.IsAutomaticallyClosed,
                                               AutomaticallyCloseDays = f.AutomaticallyCloseDays,
                                               AutomaticallyCloseDate = f.AutomaticallyCloseDate,
                                               UpdateDate = f.UpdateDate,   
                                               BusinessUnitId=f.BusinessUnitId,
                                               BusinessUnitName=f.BusinessUnitName,
                                               QuoteClosingReasonCode = f.QuoteClosingReasonCode,
                                               QuoteClosingReasonName = f.QuoteClosingReasonName,
                                               SalesmanUserId = f.SalesmanUserId,
                                               IncotermCode = f.IncotermCode,
                                           };

            query2 = filter.GetFilteredQuery<QuoteList>(listQueryOperation, query2);

            QuoteBusinessUnitFilter businessUnitFilter = new QuoteBusinessUnitFilter(tenant);
            query2 = businessUnitFilter.RunFilter(query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QuoteList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> quoteObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Quote", tenant).ToList();

                ObjectField objectField = (from a in quoteObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteList, int>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.OpenDate);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.OpenDate);
            }

            query2 = query2.Skip(skippedQuotes);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<QuoteList> GetQuoteFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Quote", "READ", tenant);

            quoteRepository = new QuoteRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            
            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);
            ProductPermitionsFilter.AddUserProductRestrictionFilters(queryOperations, tenant);

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            #region Restrictions region
            AddRestrictionFilters(queryOperations, "Quote", tenant);
            #endregion

            QuoteCustomFilter customfilters = new QuoteCustomFilter(tenant);
            IQueryable<Quote> quotes = quoteRepository.GetQuoteByTenant(tenant, null);
            quotes = customfilters.GetFilteredQuery(queryOperations, quotes);

            QuoteBusinessUnitFilter businessUnitFilter = new QuoteBusinessUnitFilter(tenant);
            quotes = businessUnitFilter.RunFilter(quotes);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            quotes = filter.GetFilteredQuery<Quote>(nonListQueryOperation, quotes);
            int numOfQuotes = quotes.Count();
            int skippedQuotes = queryOperations.PageIndex;

            quoteQuery = new QuoteQuery(tenant);
            IQueryable<QuoteList> query2 = quoteQuery.GetIQueryableEntityList(quotes);
            query2 = filter.GetFilteredQuery<QuoteList>(listQueryOperation, query2);
           
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QuoteList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> quoteObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Quote", tenant).ToList();

                ObjectField objectField = (from a in quoteObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<QuoteList, string>(queryOperations, query2);
                    }
                    else
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "text":
                                {
                                    query2 = sortClass.GetSorterQuery<QuoteList, string>(queryOperations, query2);
                                    break;
                                }
                            case "double":
                                {
                                    query2 = sortClass.GetSorterQuery<QuoteList, double>(queryOperations, query2);
                                    break;
                                }
                            case "datetime":
                                {
                                    query2 = sortClass.GetSorterQuery<QuoteList, DateTime>(queryOperations, query2);
                                    break;
                                }
                            case "integer":
                                {
                                    query2 = sortClass.GetSorterQuery<QuoteList, int>(queryOperations, query2);
                                    break;
                                }
                            case "boolean":
                                {
                                    query2 = sortClass.GetSorterQuery<QuoteList, bool>(queryOperations, query2);
                                    break;
                                }
                            case "lookup":
                                {
                                    query2 = sortClass.GetSorterQuery<QuoteList, string>(queryOperations, query2);
                                    break;
                                }
                            default:
                                {
                                    query2 = query2.OrderByDescending(d => d.OpenDate);
                                    break;
                                }
                        }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.OpenDate);
            }

            query2 = query2.Skip(skippedQuotes);
            query2 = query2.Take(queryOperations.PageSize);

            List<QuoteList> listQuery = query2.ToList();
            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("Quote", tenant, listQuery.Cast<object>().ToList());

            //List<QuoteList> listQuery = query2.ToList();
            //List<ObjectField> customFields = ObjectFieldsRepository.GetCustomObjectFieldsByObjectTableName("Quote", tenant).ToList();

            //CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            //foreach (ObjectField field in customFields)
            //{
            //    foreach (QuoteList quoteList in listQuery)
            //    {
            //        PropertyInfo propInfo = typeof(QuoteList).GetProperty(field.FieldName);
            //        object newValue = customFieldResolver.GetFieldValue(quoteList, field, tenant);
            //        propInfo.SetValue(quoteList, newValue, null);
            //    }
            //}

            return listQuery.AsQueryable();
        }

        public int GetQuoteFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Quote", "READ", tenant);

            quoteRepository = new QuoteRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            
            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);
            ProductPermitionsFilter.AddUserProductRestrictionFilters(queryOperations, tenant);

            #region Restrictions region
            AddRestrictionFilters(queryOperations, "Quote", tenant);
            #endregion

            GenericFilter filter = new GenericFilter();
            QuoteCustomFilter customfilters = new QuoteCustomFilter(tenant);
            IQueryable<Quote> quotes = quoteRepository.GetQuoteByTenant(tenant, null);
            quotes = customfilters.GetFilteredQuery(queryOperations, quotes);

            QuoteBusinessUnitFilter businessUnitFilter = new QuoteBusinessUnitFilter(tenant);
            quotes = businessUnitFilter.RunFilter(quotes);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            quotes = filter.GetFilteredQuery<Quote>(nonListQueryOperation, quotes);
            int numOfQuotes = quotes.Count();
            int skippedQuotes = queryOperations.PageIndex;

            quoteQuery = new QuoteQuery(tenant);

            IQueryable<QuoteList> query2 = quoteQuery.GetIQueryableEntityList(quotes);
            query2 = filter.GetFilteredQuery<QuoteList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public int GetFollowUpsByQuotesFilterCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ContactRepository contactRep = new ContactRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            ContactQuery contactQuery = new ContactQuery(contactRep);
            ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
             
            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);
            ProductPermitionsFilter.AddUserProductRestrictionFilters(queryOperations, tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            GenericFilter filter = new GenericFilter();
            QuoteFollowUpsCustomFilter customfilters = new QuoteFollowUpsCustomFilter(tenant);
            BranchPermitionsFilter.AddUserBranchRestrictionFilters(listQueryOperation, tenant);
            quoteRepository = new QuoteRepository(tenant);

            IQueryable<QuoteFollowUpDataView> quoteFollowUps = quoteRepository.GetQuoteFollowUpDataViewByTenant(tenant);
            quoteFollowUps = customfilters.GetQuoteFollowUpFilteredQuery(queryOperations, quoteFollowUps);
            int skippedQuotes = queryOperations.PageIndex;

            var query2 = from f in quoteFollowUps
                         select new QuoteList()
                         {
                             IsClosed = f.IsClosed,
                             Id = f.Id,
                             Shipper = f.ShipperName,
                             Consignee = f.ConsigneeName,
                             QuoteViewId = f.Id + f.FollowUpId,
                             FromPort = f.FromPortCode,
                             ToPort = f.ToPortCode,
                             CustomerName = f.CustomerName,
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
                             ShipperReference1 = f.ShipperReference1,
                             LastModified = f.LastModified,
                             QuoteTypeCode = f.QuoteTypeCode,
                             ShipmentType = f.ShipmentTypeName,
                             DirectionId = f.DirectionId,
                             TransportModeId = f.TransportModeId,
                             ShipmentTypeId = f.ShipmentTypeId,
                             ShipperId = f.ShipperId,
                             FromPortId = f.FromPortId,
                             ToPortId = f.ToPortId,
                             QuoteNumber = f.QuoteNumber,
                             OpenDate = f.OpenDate,
                             ExpirationDate = f.ExpirationDate,
                             MainCarriageCarrierName = f.MainCarriageCarrierName,
                             MainCarriageCarrierId = f.MainCarriageCarrierId,
                             QuoteTypeName = f.QuoteTypeName,
                             CarrierName = f.MainCarriageCarrierName,
                             ChargeableWeight = f.ChargeableWeight,
                             GrossWeight = f.GrossWeight,
                             IsCancelled = f.IsCancelled,
                             SearchFields = f.SearchFields,
                             Notes = f.Notes,
                             BranchId = f.BranchId,
                             DepartmentId = f.DepartmentId,
                             NumberOfContainers = f.NumberOfContainers,
                             FromPartnerId = f.FromPartnerId,
                             ToPartnerId = f.ToPartnerId,
                             FromPartnerAddressId = f.FromPartnerAddressId,
                             ToPartnerAddressId = f.ToPartnerAddressId,
                             FollowUpDate = f.FollowUpDate,
                             FollowUpType = f.FollowUpType,
                             FollowUpNotes = f.FollowUpNotes,
                             FollowUpOwner = f.FollowUpOwner,
                             StageId = f.StageId,
                             StageName = f.StageName,
                             StageDueDate = f.StageDueDate,
                             SalesmanUserId = f.SalesmanUserId,
                             BusinessUnitId = f.BusinessUnitId,
                             BusinessUnitName = f.BusinessUnitName,
                             QuoteClosingReasonCode = f.QuoteClosingReasonCode,
                             QuoteClosingReasonName = f.QuoteClosingReasonName,
                         };

            query2 = filter.GetFilteredQuery<QuoteList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
     
        public void UpdateQuoteList(QuoteList entity)
        {

        }

        public List<QuotePM> GetQuotesByStringIds(string quoteIds, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Quote", "READ", tenant);

            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();
            List<QuotePM> quotePMList = new List<QuotePM>();
            quoteIds = quoteIds.TrimStart(',');
            string[] idArray = quoteIds.Split(',');
            
            foreach (string quoteId in idArray)
            {
                QuotePM quotepm = GetSingleQuotePresentationModel(quoteId, tenant);
                quotePMList.Add(quotepm);
            }

            return quotePMList;
        }

        [Invoke]
        public DateTime InsertQuoteTraceEvent(QuotePM entityPM, DateTime? eventDate, string note, string eventTypeId, string objectTableId, string userId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            string entityId = entityPM.Id;

            //if (objectContext == null)
            //{
            //    objectContext = QuotesContext.GetContext(tenant);
            //}

            //quoteRepository = new QuoteRepository(objectContext);
            //EventTypeRepository eventTypeRep = new EventTypeRepository(tenant);
            //EventType eventType = eventTypeRep.GetSingleEventType(eventTypeId, tenant);
            //WebFreightDomainService webFreightService = new WebFreightDomainService();
            //Quote quote = quoteRepository.GetSingleQuote(quotePM.Id, quotePM.Tenant);
            
            TraceEvent newTraceEvent = new TraceEvent();
            newTraceEvent.Id = Guid.NewGuid().ToString();
            newTraceEvent.LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            newTraceEvent.Notes = note;
            newTraceEvent.ObjectTableId = objectTableId;
            newTraceEvent.Tenant = tenant;
            newTraceEvent.UserId = userId;
            newTraceEvent.EventTypeId = eventTypeId;
            newTraceEvent.EventDateTime = eventDate != null ? eventDate.Value : TenantServerConfigration.GetCurrentDateTime(tenant);
            newTraceEvent.EntityId = entityId;
            newTraceEvent.Deleted = false;
            newTraceEvent.IsAddedManually = true;

            TraceEventRepository traceEventRep = new TraceEventRepository(tenant);
            traceEventRep.Add(newTraceEvent);
            traceEventRep.SubmitChanges();
            
            //quoteRepository.Update(quote);
            //quoteRepository.SubmitChanges();

            return newTraceEvent.LogDateTime;
        }

        [Invoke]
        public void DeleteQuoteTraceEvent(QuotePM entityPM, string traceEventId, int tenant, bool external)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);             
            TraceEventRepository traceEventRep = new TraceEventRepository(tenant);
            TraceEvent traceEvent = traceEventRep.GetSingleTraceEvent(traceEventId);
            traceEvent.Deleted = true;
            traceEventRep.Update(traceEvent);
            traceEventRep.SubmitChanges();
        }

        public List<EntityPartner> GetEntityPartners(string entityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            quoteRepository = new QuoteRepository(tenant);
            List<EntityPartner> list = new List<EntityPartner>();

            quoteQuery = new QuoteQuery(tenant);
            QuotePM quote = quoteQuery.GetSinglePM(entityId, tenant);

            int idCounter = 0;

            if (quote.CustomerId != null)
            {
                list.Add(new EntityPartner()
                {
                    Id = idCounter++,
                    PartnerId = quote.CustomerId,
                    PartnerType = "Customer",
                    PartnerContactId = quote.CustomerContactId
                });
            }

            if (quote.ShipperId != null )
            {
                list.Add(new EntityPartner()
                {
                    Id = idCounter++,
                    PartnerId = quote.ShipperId,
                    PartnerType = "Shipper",
                    PartnerContactId = quote.ShipperContactId
                });
            }

            if (quote.ConsigneeId != null )
            {
                list.Add(new EntityPartner()
                {
                    Id = idCounter++,
                    PartnerId = quote.ConsigneeId,
                    PartnerType = "Consignee",
                    PartnerContactId = quote.ConsigneeContactId
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

        protected override bool PersistChangeSet()
        {
            try
            {
                objectContext.SaveChanges();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new Exception("Sorry you can't update this record right now it's being updated by another user");
            }
            return base.PersistChangeSet();
        }

        //[InvokeAttribute]
        public bool GetNewPartners(CustomerPM shipper)
        {
            //PartnersDomainService partnersDomain = new PartnersDomainService();
            //if (partnersDomain.ObjectContext == null)
            //{
            //    partnersDomain.ObjectContext = CommonDataContext.GetContext(shipper.Tenant);
            //}

           // CustomerService customerService = new CustomerService(partnersDomain.ObjectContext, shipper.Tenant); Mohammad : I saw that this service is not used.
            //the comment below is before the comment above.
            //shipper
            //if (helper.ShipperPM != null)
            //{
            //    customerService.Create(helper.ShipperPM);
            //}


            return true;

        }

        public IQueryable<QuoteList> GetQuoteLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Quote", "READ", tenant);

            quoteRepository = new QuoteRepository(tenant);
            quoteQuery = new QuoteQuery(quoteRepository);

            IQueryable<Quote> myResult = quoteRepository.GetQuotes(tenant);

            myResult = BranchPermitionsFilter.AddUserBranchRestrictionFilters<Quote>(new QueryOperations(), myResult, tenant);
            myResult = ProductPermitionsFilter.AddUserProductRestrictionFilters<Quote>(new QueryOperations(), myResult, tenant);

            QuoteBusinessUnitFilter businessUnitFilter = new QuoteBusinessUnitFilter(tenant);
            myResult = businessUnitFilter.RunFilter(myResult);

            IQueryable<QuoteList> query2 = quoteQuery.GetIQueryableEntityList(myResult);

            return query2;
        }

        public CRMSummary GetQuotesSummary(string ownerId, string businessUnitId, int tenant, string directionId, string transportModeId)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Quote", "READ", tenant);

            CRMSummary summaryClass = new CRMSummary();

            quoteRepository = new QuoteRepository(tenant);

            QuoteBusinessUnitFilter businessUnitFilter = new QuoteBusinessUnitFilter(tenant);

            IQueryable<Quote> allQuotes = quoteRepository.GetQuotes(tenant);
            allQuotes = BranchPermitionsFilter.AddUserBranchRestrictionFilters<Quote>(new QueryOperations(), allQuotes, tenant);
            allQuotes = ProductPermitionsFilter.AddUserProductRestrictionFilters<Quote>(new QueryOperations(), allQuotes, tenant);
            allQuotes = businessUnitFilter.RunFilter(allQuotes);

            IQueryable<QuoteFollowUpDataView> allFollowups = quoteRepository.GetQuoteFollowUpDataViewByTenant(tenant);
            allFollowups = BranchPermitionsFilter.AddUserBranchRestrictionFilters<QuoteFollowUpDataView>(new QueryOperations(), allFollowups, tenant);
            allFollowups = ProductPermitionsFilter.AddUserProductRestrictionFilters<QuoteFollowUpDataView>(new QueryOperations(), allFollowups, tenant);
            allFollowups = businessUnitFilter.RunFilter(allFollowups);

            string loggedUser = AuthenticationUtil.GetAuthenticatedUser();
            ContactRepository contactRep = new ContactRepository(tenant);
            Contact loggedContact = contactRep.GetSingleContactByEmail(loggedUser, tenant);

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

            summaryClass.Quotes_All = allQuotes.Where(d => !d.IsCancelled).Count();
            summaryClass.Quotes_My = allQuotes.Where(d => d.SalesmanUserId == loggedContact.Id && !d.IsCancelled).Count();
            summaryClass.Quotes_Cancelled = allQuotes.Where(d => d.IsCancelled).Count();

            allQuotes = allQuotes.Where(d => d.IsCancelled == false);

            summaryClass.Quotes_Created = allQuotes.Where(d => d.Stage.Code == "QTCR").Count();
            summaryClass.Quotes_Draft = allQuotes.Where(d => d.Stage.Code == "QTDR").Count();
            summaryClass.Quotes_Expired = allQuotes.Where(d => d.ExpirationDate < DateTime.Now && !d.IsClosed).Count();
            summaryClass.Quotes_Accepted = allQuotes.Where(d => d.Stage.Code == "QTAC").Count();
            summaryClass.Quotes_AcceptedNOShip = allQuotes.Where(d => d.Stage.Code == "QTAC" && (d.UsageCount == 0 || d.UsageCount == null)).Count();
            summaryClass.Quotes_Sent = allQuotes.Where(d => d.Stage.Code == "QTST").Count();

            summaryClass.Quotes_AllFollowups = allFollowups.Count();
            summaryClass.Quotes_MyFollowups = allFollowups.Where(d => d.FollowUpOwnerId == loggedContact.Id).Count();

            return summaryClass;
        }

        public List<QuoteList> GetRecentQuotes(string ownerId, string businessUnitId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Quote", "READ", tenant);

            string mail = SecurityUtility.GetAuthenticatedUser();
            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM contact = contactQuery.GetContactByEmailOnly(mail, tenant);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Quote", 0, true);

            quoteQuery = new QuoteQuery(tenant);
            IQueryable<QuoteList> first = quoteQuery.GetRecentEntityLists(ownerId, businessUnitId, tenant, contact.Id, objectTable.Id).AsQueryable();

            IQueryable<QuoteList> myResult = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), first, tenant);
            myResult = ProductPermitionsFilter.AddUserProductRestrictionFilters<QuoteList>(new QueryOperations(), myResult, tenant);

            QuoteBusinessUnitFilter businessUnitFilter = new QuoteBusinessUnitFilter(tenant);
            myResult = businessUnitFilter.RunFilter(myResult);

            return myResult.ToList();
        }

        public void UpdateChartingDataClass(ChartingDataClass entity)
        {

        }
        public List<ChartingDataClass> GetQuotesChartDataCustom(DateTime?FromDate,DateTime?ToDate, string ownerId, string businessUnitId, string chartCode, int tenant)
        {
            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Quote", "READ", tenant);

            quoteQuery = new QuoteQuery(tenant);

            List<ChartingDataClass> data = quoteQuery.GetQuotesChartDataCustom(FromDate,ToDate, ownerId, businessUnitId, chartCode, tenant);

            foreach (ChartingDataClass item in data)
            {
                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    GroupedId = item.GroupedId,
                    DataTypeCode = item.DataTypeCode,
                    StringProperty = item.StringProperty,
                    IntegerProperty = item.IntegerProperty,
                    OwnerId = ownerId,
                    BusinessUnitId = businessUnitId,
                });
            }

            return myResult;
        }

        public List<ChartingDataClass> GetQuotesChartData(string code, string ownerId, string businessUnitId, string chartCode, int tenant)
        {
            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Quote", "READ", tenant);

            quoteQuery = new QuoteQuery(tenant);

            List<ChartingDataClass> data = quoteQuery.GetQuotesChartData(code, ownerId, businessUnitId, chartCode, tenant);

            string str = code;
            if (str.Contains("_1"))
            {
                str = str.Replace("_1", "");
            }

            int days = Convert.ToInt32(str);

            foreach (ChartingDataClass item in data)
            {
                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    GroupedId = item.GroupedId,
                    DataTypeCode = item.DataTypeCode,
                    StringProperty = item.StringProperty,
                    IntegerProperty = item.IntegerProperty,
                    Day = days,
                    OwnerId = ownerId,
                    BusinessUnitId = businessUnitId,
                    Code = code,
                });
            }

            return myResult;
        }

        public List<ChartingDataClass> GetQuotesGroupBySalesman(string code, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Quote", "READ", tenant);

            quoteQuery = new QuoteQuery(tenant);
            List<ChartingDataClass> myResult = quoteQuery.GetQuotesGroupBySalesman(code, ownerId, businessUnitId, fieldCode, tenant, isTopTen);

            foreach (ChartingDataClass item in myResult)
            {
                if (item.StringProperty != null)
                {
                    string[] nameString = item.StringProperty.Split(' ');
                    item.ShortLabelProperty = nameString[0];
                }
            }

            return myResult;
        }
        public List<ChartingDataClass> GetQuotesGroupBySalesmanCustom(DateTime? FromDate,DateTime? ToDate, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Quote", "READ", tenant);

            quoteQuery = new QuoteQuery(tenant);
            List<ChartingDataClass> myResult = quoteQuery.GetQuotesGroupBySalesmanCustom(FromDate,ToDate, ownerId, businessUnitId, fieldCode, tenant, isTopTen);

            foreach (ChartingDataClass item in myResult)
            {
                if (item.StringProperty != null)
                {
                    string[] nameString = item.StringProperty.Split(' ');
                    item.ShortLabelProperty = nameString[0];
                }
            }

            return myResult;
        }

        [Invoke]
        public void ConnectQuotesToOpportunity(string opportunityId, List<string> quotesIds, int tenant)
        {
            QuoteRepository myQuoteRepository = new QuoteRepository(tenant);
            IQueryable<Quote> myQuotes = myQuoteRepository.GetQuotes(tenant).Where(d => quotesIds.Contains(d.Id));

            ICRMContext cRMContext = CRMContext.GetContext(tenant);
            OpportunityUpdateService service = new OpportunityUpdateService(cRMContext, new Dictionary<string, IContext>(), tenant);
            OpportunityQueryService opportunityQuery = new OpportunityQueryService(cRMContext);
            var opportunity = opportunityQuery.GetSingle(opportunityId, false, false);

            if (opportunity != null)
            {
                string email = HttpContext.Current.User.Identity.Name;
                ContactRepository contactRepository = new ContactRepository(tenant);
                string loggedContactId = contactRepository.GetSingleContactByEmail(email, tenant).Id;

                string myEventNotes = "Connected:";

                foreach (Quote item in myQuotes)
                {
                    item.OpportunityId = opportunityId;
                    item.ConnectedToOpportunity = true;
                    myEventNotes += "\n" + item.QuoteNumber;
                    this.UpdateOpportunity(opportunity);
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "QTOP",
                    UserId = loggedContactId,
                    EntityId = opportunityId,
                    ObjectTableName = "Opportunity",
                    Notes = myEventNotes,
                });

                myQuoteRepository.SubmitChanges();
                service.InitializeEntityPM(opportunity);
                opportunity.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                service.Update(opportunity, true);
            }
        }
        private void UpdateOpportunity(OpportunityPM opportunity)
        {
            if (opportunity.NumberOfConnectedQuotes == null)
            {
                opportunity.NumberOfConnectedQuotes = 1;
            }

            else
            {
                opportunity.NumberOfConnectedQuotes += 1;
            }
        }

        public List<ChartingDataClass> GetStageFunnelData(string ownerId, string businessUnitId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Quote", "READ", tenant);

            quoteQuery = new QuoteQuery(tenant);

            List<ChartingDataClass> data = quoteQuery.GetStageFunnelData(ownerId, businessUnitId, tenant, null);

            List<ChartingDataClass> result = new List<ChartingDataClass>();

            foreach (ChartingDataClass item in data)
            {
                result.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    LabelProperty = item.LabelProperty,
                    DecimalProperty = item.DecimalProperty,
                    IntegerProperty = item.IntegerProperty,
                    OwnerId = ownerId,
                    BusinessUnitId = businessUnitId,
                    GroupedId = item.GroupedId,
                });
            }

            return result.OrderBy(d => d.IntegerProperty).ToList();
        }

        [Invoke]
        public string GetQuoteAutomaticSubject(QuotePM entityPM)
        {
            int tenant = entityPM.Tenant;
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Quote", "READ", tenant);

            QuoteSubjectService iSubjectService = new QuoteSubjectService(entityPM);
            string mySubject = iSubjectService.GetSubject();
            return mySubject;
        }

    }
}
