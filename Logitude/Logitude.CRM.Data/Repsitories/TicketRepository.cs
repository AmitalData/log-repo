 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class TicketRepository:IRepository<Ticket>
   {
        
		public List<Ticket> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public string GetMaxTicketNumber(int tenant)
        {
            string myResult = context.Tickets.Where(d => d.Tenant == tenant).Select(s => s.TicketNumber).Max();
            return myResult;
        }

        public IQueryable<Ticket> GetAllFromIdList(List<string> ids, int tenant)
        {
            IQueryable<Ticket> entities = (from a in context.Tickets where a.Tenant == tenant && ids.Contains(a.Id) select a);
            return entities;
        }

        public Ticket GetTicketByGuidId(string  guidId, int tenant)
        {
            Ticket entity = (from a in context.Tickets where a.Tenant == tenant && a.GuidId ==guidId select a).FirstOrDefault();
            return entity;
        }

        public string GetTicketId(string ticketNumber, int tenant)
        {
            return (from a in context.Tickets where a.TicketNumber == ticketNumber && a.Tenant == tenant select a.Id).FirstOrDefault();
        }

        public string GetIdByQuoteId(string quoteId)
        {
            return (from a in context.Tickets where a.QuoteId == quoteId select a.Id).FirstOrDefault();
        }

    }

}
   