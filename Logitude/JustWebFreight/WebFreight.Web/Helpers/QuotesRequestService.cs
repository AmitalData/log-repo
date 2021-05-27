using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public List<QuotesRequest> Get()
        {
            QueryOperations queryOperations = GetQueryOperations();
            tickets = GetTickets(queryOperations);
            documentsFilings = GetDocumentsFilings();
            List<QuotesRequest> quotesRequests = BuildQuotesRequests();
            return quotesRequests;
        }

        private QueryOperations GetQueryOperations()
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

        private List<DocumentsFilingList> GetDocumentsFilings()
        {
            if (tickets.Count == 0 || string.IsNullOrEmpty(documentTypeId)) return null;
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
            QuotesRequest quotesRequest = new QuotesRequest() { CreateDate = ticketList.CreateDate, ReferenceNumber = ticketList.TicketNumber };
            if (documentsFilings.Where(d => d.EntityId == ticketList.Id).Any())
            {
                quotesRequest.QuotationUpdateDate = documentsFilings.Where(d => d.EntityId == ticketList.Id).OrderByDescending(d => d.CreateDate).Select(d => d.CreateDate).FirstOrDefault() ;
            }
            return quotesRequest;

        }


    }

    public class QuotesRequest
    {
        public string ReferenceNumber { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? QuotationUpdateDate { get; set; }

    }

    public class QuotesRequestFilters
    {
        public string PartnerId { get; set; }
        public string SearchField { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

    }

}