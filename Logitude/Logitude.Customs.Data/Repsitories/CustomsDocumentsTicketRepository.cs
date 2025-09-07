
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using Logitude.Customs.Data.EntityKeys.Extended;

namespace Logitude.Customs.Data.Repsitories
{
    public partial class CustomsDocumentsTicketRepository : IRepository<CustomsDocumentsTicket>
    {

        public CustomsDocumentsTicketRepository()
        {

        }

        public List<CustomsDocumentsTicket> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }


        public List<CustomsDocumentsTicket> GetCustomsDocumentTickets(GetTicketsParams parameters, int tenant)
        {

            (context as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 
            var q = context.CustomsDocumentPointers.Where(a => a.Tenant == tenant);
            q = q.Where(a => a.ParentEntityCode == parameters.ParentEntityCode && a.ParentEntityId == parameters.ParentEntityId);
            var list = q.ToList();
            q = list.AsQueryable<CustomsDocumentPointer>();

            q = q.Where(a => ((a.Child1EntityCode ?? "_IsNull") == (parameters.Child1EntityCode ?? "_IsNull")) && ((a.Child1EntityId ?? "_IsNull") == (parameters.Child1EntityId ?? "_IsNull")));
            q = q.Where(a => ((a.Child2EntityCode ?? "_IsNull") == (parameters.Child2EntityCode ?? "_IsNull")) && ((a.Child2EntityId ?? "_IsNull") == (parameters.Child2EntityId ?? "_IsNull")));
            q = q.Where(a => ((a.Child3EntityCode ?? "_IsNull") == (parameters.Child3EntityCode ?? "_IsNull")) && ((a.Child3EntityId ?? "_IsNull") == (parameters.Child3EntityId ?? "_IsNull")));

            list = q.ToList();
            var ticketIds = (from a in /*q*/ list
                             group a by a.CustomsDocumentsTicketId into gr
                             select new { CustomsDocumentsTicketId = gr.Key });

            List<string> ids = new List<string>();
            foreach (var s in ticketIds)
            {
                ids.Add(s.CustomsDocumentsTicketId);
            }

            List<CustomsDocumentsTicket> parentTickets = (from a in context.CustomsDocumentsTickets
                                                          where ids.Contains(a.Id)
                                                          select a).ToList();
            return parentTickets;
        }

        public List<CustomsDocumentsTicket> GetCustomsDocumentsTicketPMsByEntityIdAndChilds(string entityId, string entity1ChildId, string entity2ChildId, string entity3ChildId, int tenant, string parentEntityCode, bool isAir)
        {
            IQueryable<CustomsDocumentPointer> iqurable = null;

            (context as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            if (!string.IsNullOrEmpty(entity3ChildId))
            {
                iqurable = from a in context.CustomsDocumentPointers
                           where a.ParentEntityId == entityId && a.Child1EntityId == entity1ChildId && a.Child2EntityId == entity2ChildId && a.Child3EntityId == entity3ChildId && a.Tenant == tenant && a.ParentEntityCode == parentEntityCode
                           select a;
            }
            else if (!string.IsNullOrEmpty(entity2ChildId))
            {
                iqurable = from a in context.CustomsDocumentPointers
                           where a.ParentEntityId == entityId && a.Child1EntityId == entity1ChildId && a.Child2EntityId == entity2ChildId && a.Tenant == tenant && a.ParentEntityCode == parentEntityCode
                           select a;
            }
            else if (!string.IsNullOrEmpty(entity1ChildId))
            {
                iqurable = from a in context.CustomsDocumentPointers
                           where a.ParentEntityId == entityId && a.Child1EntityId == entity1ChildId && a.Tenant == tenant && a.ParentEntityCode == parentEntityCode
                           select a;
            }
            else
            {

                iqurable = from a in context.CustomsDocumentPointers
                           where a.ParentEntityId == entityId && a.Tenant == tenant && a.ParentEntityCode == parentEntityCode && (a.Child1EntityCode == null || a.Child1EntityCode != "SIIRequest")
                           select a;
            }

            var sw = Stopwatch.StartNew();
            var qGroupby = (from a in iqurable
                            group a by a.CustomsDocumentsTicketId into gr
                            select new { CustomsDocumentsTicketId = gr.Key });

            var customsDocumentPointers = qGroupby.ToList();
            var time1 = sw.ElapsedMilliseconds;




            List<string> ticketIds = new List<string>();

            foreach (var s in customsDocumentPointers)
            {
                ticketIds.Add(s.CustomsDocumentsTicketId);
            }

            if (parentEntityCode == "ExportDeclarationClosingData" && isAir)
            {
                var iqurable2 = from a in context.CustomsDocumentPointers
                                where a.ParentEntityId == entityId && a.Tenant == tenant && a.ParentEntityCode == "Declaration"
                                select a;

                var qGroupby2 = (from a in iqurable2
                                 group a by a.CustomsDocumentsTicketId into gr
                                 select new { CustomsDocumentsTicketId = gr.Key });
                var customsDocumentPointers2 = qGroupby2.ToList();
                List<string> ticketIds2 = new List<string>();

                foreach (var s in customsDocumentPointers2)
                {
                    ticketIds2.Add(s.CustomsDocumentsTicketId);
                }

                List<CustomsDocumentsTicket> tickets2 = (from a in context.CustomsDocumentsTickets
                                                         where ticketIds.Contains(a.Id) || (ticketIds2.Contains(a.Id) && a.DocumentTypeCode == "419")
                                                         select a).ToList();
                return tickets2;
            }

            List<CustomsDocumentsTicket> tickets = (from a in context.CustomsDocumentsTickets
                                                    where ticketIds.Contains(a.Id)
                                                    select a).ToList();
            return tickets;

        }
        public List<string> GetIsConnectDec(string documentsfilingid, string entityId)
        {
            var query = (from a in context.CustomsDocumentsTickets
                         where a.DocumentsFilingId == documentsfilingid
                         select a.Id).ToList();

            return query;
        }
        public List<string> GetDocConnectTicket(string documentsfilingid, string entityId, int tenant)
        {
            var query = (
                from tickets in context.CustomsDocumentsTickets
                join pointers in context.CustomsDocumentPointers on tickets.Id equals pointers.CustomsDocumentsTicketId
                join d1 in context.Declarations on pointers.ParentEntityId equals d1.Id
                join d2 in context.Declarations on entityId equals d2.Id
                where tickets.Tenant == tenant
                && pointers.Tenant == tenant
                && d1.Tenant == tenant
                && d2.Tenant == tenant
                && tickets.DocumentsFilingId == documentsfilingid
                && d1.Id != entityId
                && d2.CustomFileNo != d1.CustomFileNo
                select d1.Id
            );

            List<string> result = query.ToList();

            return result;
        }

        public bool IsSendToCustomsAndNotConnectTicket(string documentsfilingid, string entityId, int tenant)
        {

            var query = (
                 from c in context.CustomsDocuments
                 where c.Tenant == tenant && !string.IsNullOrEmpty(c.CustomsDocId) && c.DocumentsFilingId == documentsfilingid
                 select c).FirstOrDefault();


            if (query != null)
            {
                var query2 = (
                     from ticket in context.CustomsDocumentsTickets
                     join p in context.CustomsDocumentPointers
                     on ticket.Id equals p.CustomsDocumentsTicketId
                     join d in context.Declarations
                     on p.ParentEntityId equals d.Id
                     where
                     ticket.DocumentsFilingId == documentsfilingid
                     && d.Id == entityId

                     select ticket
                     ).FirstOrDefault();
                if (query2 == null)
                {
                    return true;
                }

            }

            return false;
        }

        public int GetCountOfTicketsByDocFilingId(string docId, int tenant)
        {
            return (from a in context.CustomsDocumentsTickets
                    where a.DocumentsFilingId == docId && a.Tenant == tenant
                    select a).Count();
        }

        public Boolean GetIfThereRequestDocumentDocIdNotVerifiedByDeclarationId(string declarationId, string docTicketId)
        {
            var result = (from pointer in context.CustomsDocumentPointers
                          join ticket in context.CustomsDocumentsTickets on pointer.CustomsDocumentsTicketId equals ticket.Id
                          where pointer.ParentEntityId == declarationId && ticket.RequestedCustomsDocId != null && (ticket.VerificationStatusTypeCode != "4" && ticket.VerificationStatusTypeCode != "5") && ticket.Id != docTicketId
                          select ticket).Any();
            return result;

        }

        public List<CustomsDocumentsTicket> GetCustomsDocumentsTicketsByDocumentsFilingId(string documentsFilingId, int tenant)
        {
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 


            List<CustomsDocumentsTicket> customsDocumentsTicket;
            customsDocumentsTicket = (from a in context.CustomsDocumentsTickets
                                      where (a.DocumentsFilingId == documentsFilingId)
                                       && a.Tenant == tenant
                                      select a).ToList();

            return customsDocumentsTicket;
        }

        public List<CustomsDocumentsTicket> GetCustomsDocumentsTicketsIds(string ticketIds, int tenant)
        {
            string[] ids = ticketIds.Split(',');
            List<CustomsDocumentsTicket> customsDocumentsTicket;
            customsDocumentsTicket = (from a in context.CustomsDocumentsTickets
                                      where (ids.Contains(a.Id))
                                       && a.Tenant == tenant
                                      select a).ToList();

            return customsDocumentsTicket;
        }
        private const string PARENT_DECL = "Declaration";
        private const string CHILD1_SIIREQUEST = "SIIRequest";
        private const string CHILD2_SUPPINVOICE = "SupplierInvoice";
        private const string CHILD3_SUPPINVITEM = "SupplierInvoiceItem";

        public IList<PointerTicketDto> GetPointersWithFilingId(IReadOnlyCollection<SiiSelectedRowDto> items, int tenant)
        {
            var decIds = items.Select(i => i.DeclarationId).Where(id => id != null).Distinct().ToList();
            var sirIds = items.Select(i => i.SIIRequestID).Where(id => id != null).Distinct().ToList();
            var invIds = items.Select(i => i.InvoiceCounterKey.ToString()).Where(id => id != null).ToHashSet();
            var itemLines = items.Select(i => i.InvoiceItemLineNumber.ToString()).Where(id => id != null).Distinct().ToList();



            var query =
                from p in context.CustomsDocumentPointers
                join t in context.CustomsDocumentsTickets
                      on p.CustomsDocumentsTicketId equals t.Id

                join d in context.SIIDocumentTypes
                on t.DocumentTypeCode equals d.Code into dgrp
                from d in dgrp.DefaultIfEmpty()

                where p.Tenant == tenant
                      && p.ParentEntityCode == PARENT_DECL
                      && t.DocumentsFilingId != null
                      && decIds.Contains(p.ParentEntityId)

                      && p.Child1EntityCode == CHILD1_SIIREQUEST
                      && sirIds.Contains(p.Child1EntityId)

                      && (
                            (p.Child2EntityCode == null && p.Child2EntityId == null)

                         || (p.Child2EntityCode == CHILD2_SUPPINVOICE
                             && invIds.Contains(p.Child2EntityId)
                             && p.Child3EntityCode == null
                             && p.Child3EntityId == null)

                         || (p.Child2EntityCode == CHILD2_SUPPINVOICE
                             && invIds.Contains(p.Child2EntityId)
                             && p.Child3EntityCode == CHILD3_SUPPINVITEM
                             && itemLines.Contains(p.Child3EntityId))
                         )
                select new PointerTicketDto
                {
                    CustomsDocumentsTicketId = p.CustomsDocumentsTicketId,
                    DocumentsFilingId = t.DocumentsFilingId,
                    DocumentTypeCode = t.DocumentTypeCode,
                    DocumentTypeCodeName = d == null ? null : d.LocalName,   

                    ParentEntityId = p.ParentEntityId,
                    Child1EntityId = p.Child1EntityId,
                    Child2EntityId = p.Child2EntityId,
                    Child3EntityId = p.Child3EntityId
                };

            return query.ToList();
        }

    }
    public class GetTicketsParams
    {
        public string ParentEntityCode { get; set; }
        public string Child1EntityCode { get; set; }
        public string Child1EntityCode2 { get; set; }
        public string Child2EntityCode { get; set; }
        public string Child3EntityCode { get; set; }
        public string ParentEntityId { get; set; }
        public string Child1EntityId { get; set; }
        public string Child2EntityId { get; set; }
        public string Child3EntityId { get; set; }
    }

    public class PointerTicketDto
    {
        public string CustomsDocumentsTicketId { get; set; }
        public string DocumentsFilingId { get; set; }
        public string DocumentTypeCode { get; set; }
        public string DocumentTypeCodeName { get; set; }


        public string ParentEntityId { get; set; }
        public string Child1EntityId { get; set; }
        public string Child2EntityId { get; set; }
        public string Child3EntityId { get; set; }
    }


}
