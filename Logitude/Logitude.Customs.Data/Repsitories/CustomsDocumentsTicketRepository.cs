 
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

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CustomsDocumentsTicketRepository:IRepository<CustomsDocumentsTicket>
   {
        
		public List<CustomsDocumentsTicket> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }


        public List<CustomsDocumentsTicket> GetCustomsDocumentTickets(GetTicketsParams parameters, int tenant)
        {
            var q = context.CustomsDocumentPointers.Where(a => a.Tenant == tenant);
            q = q.Where(a => a.ParentEntityCode == parameters.ParentEntityCode && a.ParentEntityId == parameters.ParentEntityId);
            //q = q.Where(a => a.Child1EntityCode == parameters.Child1EntityCode && a.Child1EntityId == parameters.Child1EntityId);
            //q = q.Where(a => a.Child2EntityCode == parameters.Child2EntityCode && a.Child2EntityId == parameters.Child2EntityId);
            //q = q.Where(a => a.Child3EntityCode == parameters.Child3EntityCode && a.Child3EntityId == parameters.Child3EntityId);

            /*if (!string.IsNullOrWhiteSpace(parameters.Child1EntityCode) && !string.IsNullOrWhiteSpace(parameters.Child1EntityId))
            {
                q = q.Where(a => a.Child1EntityCode == parameters.Child1EntityCode && a.Child1EntityId == parameters.Child1EntityId);
            }
            if (!string.IsNullOrWhiteSpace(parameters.Child2EntityCode) && !string.IsNullOrWhiteSpace(parameters.Child2EntityId))
            {
                q = q.Where(a => a.Child2EntityCode == parameters.Child2EntityCode && a.Child2EntityId == parameters.Child2EntityId);
            }
            if (!string.IsNullOrWhiteSpace(parameters.Child3EntityCode) && !string.IsNullOrWhiteSpace(parameters.Child3EntityId))
            {
                q = q.Where(a => a.Child3EntityCode == parameters.Child3EntityCode && a.Child3EntityId == parameters.Child3EntityId);
            }*/

            q = q.Where(a => ((a.Child1EntityCode ?? "_IsNull") == (parameters.Child1EntityCode ?? "_IsNull")) && ((a.Child1EntityId ?? "_IsNull") == (parameters.Child1EntityId ?? "_IsNull")));
            q = q.Where(a => ((a.Child2EntityCode ?? "_IsNull") == (parameters.Child2EntityCode ?? "_IsNull")) && ((a.Child2EntityId ?? "_IsNull") == (parameters.Child2EntityId ?? "_IsNull")));
            q = q.Where(a => ((a.Child3EntityCode ?? "_IsNull") == (parameters.Child3EntityCode ?? "_IsNull")) && ((a.Child3EntityId ?? "_IsNull") == (parameters.Child3EntityId ?? "_IsNull")));


            var ticketIds = (from a in q
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

        /*public List<CustomsDocumentsTicket> GetCustomsDocumentTickets(GetTicketsParams parameters, int tenant)
        {
            var ticketIds=(from a in context.CustomsDocumentPointers
                           where a.ParentEntityCode == parameters.ParentEntityCode && a.ParentEntityId == parameters.ParentEntityId && a.Tenant == tenant
                           && a.Child1EntityCode == parameters.Child1EntityCode && a.Child2EntityCode == parameters.Child2EntityCode && a.Child3EntityCode == parameters.Child3EntityCode
                           && a.Child1EntityId == parameters.Child1EntityId && a.Child2EntityId == parameters.Child2EntityId && a.Child3EntityId == parameters.Child3EntityId
                           group a by a.CustomsDocumentsTicketId into gr
                           select new { CustomsDocumentsTicketId = gr.Key});
#endif

            List<string> ids = new List<string>();
            foreach (var s in ticketIds)
            {
                ids.Add(s.CustomsDocumentsTicketId);
            }

            List<CustomsDocumentsTicket> parentTickets = (from a in context.CustomsDocumentsTickets
                                                          where ids.Contains(a.Id)
                                                          select a).ToList();
            return parentTickets;
        }*/


        public List<CustomsDocumentsTicket> GetCustomsDocumentsTicketPMsByEntityIdAndChilds(string entityId, string entity1ChildId, string entity2ChildId, string entity3ChildId, int tenant, string parentEntityCode)
        {
            IQueryable<CustomsDocumentPointer> iqurable = null;
          

            if (!string.IsNullOrEmpty(entity3ChildId))
            {
                iqurable = from a in context.CustomsDocumentPointers
                           where a.ParentEntityId == entityId && a.Child1EntityId==entity1ChildId&&a.Child2EntityId==entity2ChildId&&a.Child3EntityId==entity3ChildId && a.Tenant==tenant&&a.ParentEntityCode==parentEntityCode
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
                           where a.ParentEntityId == entityId && a.Tenant == tenant && a.ParentEntityCode == parentEntityCode
                           select a;
            }

            var customsDocumentPointers = (from a in iqurable
                                           group a by a.CustomsDocumentsTicketId into gr
                                           select new { CustomsDocumentsTicketId=gr.Key }).ToList();

            List<string> ticketIds = new List<string>();

            foreach (var s in customsDocumentPointers)
            {
                ticketIds.Add(s.CustomsDocumentsTicketId);
            }
            List<CustomsDocumentsTicket> tickets = (from a in context.CustomsDocumentsTickets
                                                    where ticketIds.Contains(a.Id)
                                                    select a).ToList();
            return tickets;
            
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
    }
   public class GetTicketsParams
   {
       public string ParentEntityCode { get; set; }
       public string Child1EntityCode { get; set; }
       public string Child2EntityCode { get; set; }
       public string Child3EntityCode { get; set; }
       public string ParentEntityId { get; set; }
       public string Child1EntityId { get; set; }
       public string Child2EntityId { get; set; }
       public string Child3EntityId { get; set; }
   }

}
   