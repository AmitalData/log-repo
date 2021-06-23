using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.BL.EntityUpdateServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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

        public QuotesRequestService(int tenant, QuotesRequestEmailFeedback emailFeedback)
        {
            this.tenant = tenant;
            this.quotesRequestFilters = quotesRequestFilters;
            documentTypeId = new DocumentTypeRepository(tenant).GetDocumentTypeIdByCode(documentTypeCode, tenant);
            ticketObjectTableId = ObjectTableRepository.GetObjectTableByName("Ticket");
            quoteObjectTableId = ObjectTableRepository.GetObjectTableByName("Quote");

        }


        public void SendEmail(QuotesRequestEmailFeedback emailFeedback)
        {
            ICRMContext MyContext = CRMContext.GetContext(this.tenant);
            TicketQueryService ticketQueryService = new TicketQueryService(MyContext);
            TicketPM ticketPM = ticketQueryService.GetSingle(emailFeedback.QuotesRequest.Id, true, false);
            ticketPM.QuoteRequestComments = emailFeedback.QuotesRequest.Comments;
            ticketPM.QuoteRequestFeedback = emailFeedback.QuotesRequest.Feedback;
            TicketUpdateService ticketUpdateService = new TicketUpdateService(MyContext, new Dictionary<string, IContext>(), this.tenant);
            ticketPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            ticketUpdateService.Update(ticketPM, true);
            UserQuery userQuery = new UserQuery(this.tenant);
            UserPM userPM = userQuery.GetSinglePM(ticketPM.OwnerId, this.tenant);
            HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
            ContactQuery contactQuery = new ContactQuery(this.tenant);
            ContactPM contactPM = contactQuery.GetContactByEmailOnly(userPM.Email, this.tenant);
            StringBuilder HtmlTemplate = new StringBuilder();
            HtmlTemplate.Append("<div style='text-align:left;'>");
            HtmlTemplate.Append("<br />");
            HtmlTemplate.Append("Hi " + userPM.EnglishName + ",");
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append(contactPM.EnglishName + " ");
            HtmlTemplate.Append("has " + emailFeedback.QuotesRequest.Feedback + " ");
            HtmlTemplate.Append("quote request " + emailFeedback.QuotesRequest.ReferenceNumber + " ");
            if (!string.IsNullOrEmpty(emailFeedback.QuotesRequest.Comments))
            {
                HtmlTemplate.Append("with the following comments: ");
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append(emailFeedback.QuotesRequest.Comments);
            }
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("Best Regards,");
            HtmlTemplate.Append("<br />");
            HtmlTemplate.Append("Logitude Team");
            HtmlTemplate.Append("<br /><br />");

            System.Text.UTF8Encoding uTF8Encoding = new System.Text.UTF8Encoding();
            byte[] htmlData = uTF8Encoding.GetBytes(HtmlTemplate.ToString());
            string emailSubject = "Quote Request " + emailFeedback.QuotesRequest.ReferenceNumber + " " + emailFeedback.QuotesRequest.Feedback;
            htmlEditorHelper.SendHtmlDocument(htmlData, null, null, this.tenant, userPM.Email, emailSubject, "", "", null, emailFeedback.QuotesRequest.Id, ticketObjectTableId, emailFeedback.QuotesRequest.DocumentId, "", emailFeedback.From, "");
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
            QuotesRequest quotesRequest = new QuotesRequest() 
            { 
                Id = ticketList.Id,
                CreateDate = ticketList.CreateDate, 
                ReferenceNumber = ticketList.TicketNumber,
                Comments = ticketList.QuoteRequestComments,
                Feedback = ticketList.QuoteRequestFeedback
            };
            if (documentsFilings.Where(d => d.EntityId == ticketList.Id).Any())
            {
                quotesRequest.QuotationDocumentFiling = GetLatestCreatedDocumentsFilingByEntityId(ticketList.Id);
            }
            return quotesRequest;

        }

        private DocumentsFilingList GetLatestCreatedDocumentsFilingByEntityId(string entityId)
        {
            return documentsFilings.Where(d => d.EntityId == entityId).OrderByDescending(d => d.CreateDate).FirstOrDefault();
        }
    }

    public class QuotesRequest
    {
        public string Id { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Feedback { get; set; }
        public string Comments { get; set; }
        public string DocumentId { get; set; }
        public DocumentsFilingList QuotationDocumentFiling { get; set; }

    }

    public class QuotesRequestFilters
    {
        public string PartnerId { get; set; }
        public string SearchField { get; set; }
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