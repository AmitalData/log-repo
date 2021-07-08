using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.BL.EntityUpdateServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class QuotesRequestService
    {
        private QuotesRequestFilters quotesRequestFilters = null;
        private List<TicketList> tickets = null;
        private List<DocumentsFilingList> documentsFilings = null;
        private int tenant;
        private ICRMContext iCRMContext;
        private string documentTypeId = string.Empty;
        private string documentTypeCode = "QUTD";
        private string ticketObjectTableId = string.Empty;
        private string quoteObjectTableId = string.Empty;


        public QuotesRequestService(int tenant, QuotesRequestFilters quotesRequestFilters)
        {
            this.tenant = tenant;
            this.quotesRequestFilters = quotesRequestFilters;
            documentTypeId = new DocumentTypeRepository(tenant).GetDocumentTypeIdByCode(documentTypeCode, tenant);
            ticketObjectTableId = ObjectTableRepository.GetObjectTableByName("Ticket");
            quoteObjectTableId = ObjectTableRepository.GetObjectTableByName("Quote");

        }

        public QuotesRequestService(int tenant)
        {
            this.tenant = tenant;
            this.quotesRequestFilters = quotesRequestFilters;
            this.iCRMContext = CRMContext.GetContext(this.tenant);
            documentTypeId = new DocumentTypeRepository(tenant).GetDocumentTypeIdByCode(documentTypeCode, tenant);
            ticketObjectTableId = ObjectTableRepository.GetObjectTableByName("Ticket");
            quoteObjectTableId = ObjectTableRepository.GetObjectTableByName("Quote");

        }


        public void UpdateQuotesRequestAndSendEmailFeedback(QuotesRequestEmailFeedback emailFeedback)
        {
            TicketPM ticketPM = GetSelectedTicketPM(emailFeedback);
            MapTicketPMQuoteRequestFields(emailFeedback, ticketPM);
            UpdateSelectedTicketPM(ticketPM);
            UserPM ownerPM = GetTicketPMOwnerUser(ticketPM);
            ContactPM loggedContactPM = GetLoggedContactPM(emailFeedback);
            StringBuilder quotesRequestTemplateBody = GetQuotesRequestTemplateBody(emailFeedback, ownerPM, loggedContactPM);
            byte[] quotesRequestTemplateBodyData = ConvertStringBodyToArrayOfByte(quotesRequestTemplateBody);
            string emailSubject = GetQuotesRequestEmailSubject(emailFeedback);
            HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
            htmlEditorHelper.SendHtmlDocument(quotesRequestTemplateBodyData, null, null, this.tenant, ownerPM.Email, emailSubject, "", "", null, emailFeedback.QuotesRequest.Id, ticketObjectTableId, emailFeedback.QuotesRequest.DocumentId, "", emailFeedback.From, "");
        }

        private TicketPM GetSelectedTicketPM(QuotesRequestEmailFeedback emailFeedback)
        {
            TicketQueryService ticketQueryService = new TicketQueryService(this.iCRMContext);
            TicketPM ticketPM = ticketQueryService.GetSingle(emailFeedback.QuotesRequest.Id, true, false);
            return ticketPM;
        }

        private void MapTicketPMQuoteRequestFields(QuotesRequestEmailFeedback emailFeedback, TicketPM ticketPM)
        {
            ticketPM.QuoteRequestComments = emailFeedback.QuotesRequest.Comments;
            ticketPM.QuoteRequestFeedback = emailFeedback.QuotesRequest.Feedback;
            MapTicketCustomField(ticketPM, "CustomerStatus", emailFeedback.QuotesRequest.Feedback);
        }

        private void MapTicketCustomField(TicketPM ticketPM, string fieldCode, string fieldValue)
        {
            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            CustomFieldResolverArgs customFieldResolverArgs = new CustomFieldResolverArgs
            {
                ObjectTableName = "Ticket",
                EntityPM = ticketPM,
                FieldCode = fieldCode,
                FieldValue = fieldValue,
                Tenant = this.tenant
            };
            customFieldResolver.SetFieldValue(customFieldResolverArgs);
        }

        private void UpdateSelectedTicketPM(TicketPM ticketPM)
        {
            TicketUpdateService ticketUpdateService = new TicketUpdateService(this.iCRMContext, new Dictionary<string, IContext>(), this.tenant);
            ticketPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            ticketUpdateService.Update(ticketPM, true);
        }

        private UserPM GetTicketPMOwnerUser(TicketPM ticketPM)
        {
            UserQuery userQuery = new UserQuery(this.tenant);
            UserPM userPM = userQuery.GetSinglePM(ticketPM.OwnerId, this.tenant);
            return userPM;
        }

        private ContactPM GetLoggedContactPM(QuotesRequestEmailFeedback emailFeedback)
        {
            ContactQuery contactQuery = new ContactQuery(this.tenant);
            ContactPM contactPM = contactQuery.GetContactByEmailOnly(emailFeedback.From, this.tenant);
            return contactPM;
        }

        private static StringBuilder GetQuotesRequestTemplateBody(QuotesRequestEmailFeedback emailFeedback, UserPM ownerPM, ContactPM loggedContactPM)
        {
            StringBuilder quotesRequestTemplateBody = new StringBuilder();
            quotesRequestTemplateBody.Append("<div style='text-align:left;'>");
            quotesRequestTemplateBody.Append("<br />");
            quotesRequestTemplateBody.Append("Hi " + ownerPM.EnglishName + ",");
            quotesRequestTemplateBody.Append("<br /><br />");
            quotesRequestTemplateBody.Append(loggedContactPM.EnglishName + " ");
            quotesRequestTemplateBody.Append("has " + emailFeedback.QuotesRequest.Feedback + " ");
            quotesRequestTemplateBody.Append("quote request " + emailFeedback.QuotesRequest.QuoteNumber + " ");
            if (!string.IsNullOrEmpty(emailFeedback.QuotesRequest.Comments))
            {
                quotesRequestTemplateBody.Append("with the following comments: ");
                quotesRequestTemplateBody.Append("<br />");
                quotesRequestTemplateBody.Append(emailFeedback.QuotesRequest.Comments);
            }
            quotesRequestTemplateBody.Append("<br /><br />");
            quotesRequestTemplateBody.Append("Best Regards,");
            quotesRequestTemplateBody.Append("<br />");
            quotesRequestTemplateBody.Append("Logitude Team");
            quotesRequestTemplateBody.Append("<br /><br />");
            return quotesRequestTemplateBody;
        }

        private static byte[] ConvertStringBodyToArrayOfByte(StringBuilder quotesRequestTemplateBody)
        {
            System.Text.UTF8Encoding uTF8Encoding = new System.Text.UTF8Encoding();
            byte[] htmlData = uTF8Encoding.GetBytes(quotesRequestTemplateBody.ToString());
            return htmlData;
        }

        private static string GetQuotesRequestEmailSubject(QuotesRequestEmailFeedback emailFeedback)
        {
            return "Quote Request " + emailFeedback.QuotesRequest.QuoteNumber + " " + emailFeedback.QuotesRequest.Feedback;
        }

        public List<QuotesRequest> Get()
        {
            QueryOperations queryOperations = BuildQueryOperations();
            tickets = GetTickets(queryOperations);
            documentsFilings = GetTicketsDocumentsFilings();
            List<QuotesRequest> quotesRequests = BuildQuotesRequests();
            return quotesRequests;
        }

        private QueryOperations BuildQueryOperations()
        {
            QueryOperations queryOperations = new QueryOperations()
            {
                PageSize = quotesRequestFilters.PageSize,
                PageIndex = quotesRequestFilters.PageIndex,
            };
            queryOperations.SetFilter("EntityType", quoteObjectTableId, false, "Equals", null, false);
            queryOperations.SetFilter("CompanyId", quotesRequestFilters.PartnerId, false, "Equals", null, false);
            queryOperations.SetFilter("SearchFields", quotesRequestFilters.SearchField, false, "Contains", null, false);
            //if (quotesRequestFilters.OnlyOpened)
            //{
            //    queryOperations.SetFilter("StageId", "Created", false, "Equals", null, false);
            //}
            if (!string.IsNullOrEmpty(quotesRequestFilters.RequestedBy) && quotesRequestFilters.RequestedBy != "All")
            {
                queryOperations.SetFilter("CustomerContactId", quotesRequestFilters.RequestedBy, false, "Equals", null, false);
            }
            //List<ObjectField> ticketCustomFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName("Ticket", this.tenant);
            //ObjectField objectCustomField = ticketCustomFields.Where(f => f.Code == "CustomerStatus").FirstOrDefault();
            //if (objectCustomField != null)
            //{
            //    string fieldCode = objectCustomField.FieldCode.Split('.')[2];
            //    queryOperations.SetFilter(fieldCode, quotesRequestFilters.CustomerStatus, false, "Equals", null, false);
            //}
            //Create Date
            return queryOperations;
        }

        private List<TicketList> GetTickets(QueryOperations queryOperations)
        {
            ICRMContext MyContext = CRMContext.GetContext(tenant);
            TicketListQueryService ticketQuery = new TicketListQueryService(MyContext);
            return ticketQuery.GetList(queryOperations, tenant);
        }

        private List<DocumentsFilingList> GetTicketsDocumentsFilings()
        {
            if (tickets.Count == 0 || string.IsNullOrEmpty(documentTypeId))
            {
                return new List<DocumentsFilingList>();
            }
            List<string> ticketIds = tickets.Select(d => d.Id).ToList();
            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
            List<DocumentsFilingList> documentsFilings = documentsFilingQuery.GetDocumentsFilingThatHasFileByEntityIdsAndObjectTableIdAndDocumentTypeId(ticketIds, ticketObjectTableId, documentTypeId).Where(d => d.Tenant == tenant).ToList();
            return documentsFilings;
        }

        private List<QuotesRequest> BuildQuotesRequests()
        {
            List<QuotesRequest> quotesRequests = new List<QuotesRequest>();
            foreach (TicketList ticketList in tickets)
            {
                quotesRequests.Add(GetNewInStanceFromQuotesRequest(ticketList));
            }

            return quotesRequests;
        }

        private QuotesRequest GetNewInStanceFromQuotesRequest(TicketList ticketList)
        {
            List<ObjectField> ticketCustomFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName("Ticket", this.tenant);
            
            QuotesRequest quotesRequest = new QuotesRequest()
            {
                Id = ticketList.Id,
                CreateDate = ticketList.CreateDate,
                QuoteNumber = ticketList.TicketNumber,
                Comments = ticketList.QuoteRequestComments,
                Feedback = ticketList.QuoteRequestFeedback,
                ContactName = ticketList.ContactName,
                OwnerName = ticketList.OwnerName,
                Subject = ticketList.Subject,
                Status = GetCustomFieldValueByEntityAndCode(ticketList, ticketCustomFields, "CustomerStatus"),
                ReferenceNumber = GetCustomFieldValueByEntityAndCode(ticketList, ticketCustomFields, "ReferenceNumber"),
                PONumber = GetCustomFieldValueByEntityAndCode(ticketList, ticketCustomFields, "PO"),
                Brand = GetCustomFieldValueByEntityAndCode(ticketList, ticketCustomFields, "Brand"),
            };
            if (documentsFilings.Where(d => d.EntityId == ticketList.Id).Any())
            {
                quotesRequest.QuotationDocumentFiling = GetLatestCreatedDocumentsFilingByEntityId(ticketList.Id);
            }
            return quotesRequest;

        }

        private string GetCustomFieldValueByEntityAndCode(object entity, List<ObjectField> entityCustomFields, string customFieldCode)
        {
            ObjectField objectCustomField = entityCustomFields.Where(f => f.Code == customFieldCode).FirstOrDefault();
            if (objectCustomField != null)
                return GetCustomFieldValue(objectCustomField, entity, this.tenant);
            else
                return "";
        }

        private string GetCustomFieldValue(ObjectField objectField, object entity, int tenant)
        {
            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            object customFieldValue = customFieldResolver.GetFieldValue(entity, objectField, tenant);
            if (customFieldValue != null)
                return customFieldValue.ToString();
            return "";
        }

        private DocumentsFilingList GetLatestCreatedDocumentsFilingByEntityId(string entityId)
        {
            return documentsFilings.Where(d => d.EntityId == entityId).OrderByDescending(d => d.CreateDate).FirstOrDefault();
        }
    }

    public class QuotesRequest
    {
        public string Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Feedback { get; set; }
        public string Comments { get; set; }
        public string DocumentId { get; set; }
        public string ContactName { get; set; }
        public string OwnerName { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }
        public string ReferenceNumber { get; set; }
        public string PONumber { get; set; }
        public string Brand { get; set; }
        public string QuoteNumber { get; set; }
        public DocumentsFilingList QuotationDocumentFiling { get; set; }

    }

    public class QuotesRequestFilters
    {
        public string PartnerId { get; set; }
        public string SearchField { get; set; }
        public string RequestedBy { get; set; }
        public string CustomerStatus { get; set; }
        public bool OnlyOpened { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

    }

    public class QuotesRequestEmailFeedback
    {
        public string PartnerId { get; set; }
        public string From { get; set; }
        public QuotesRequest QuotesRequest { get; set; }

    }

}