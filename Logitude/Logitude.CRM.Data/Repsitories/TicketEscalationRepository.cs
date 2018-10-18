 
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
   public partial class TicketEscalationRepository:IRepository<TicketEscalation>
   {
		public List<TicketEscalation> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public TicketEscalation GetSingleByTenant(int tenant)
        {
            return (from a in context.TicketEscalations
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TicketEscalation> GetTicketEscalations(string ticketId, int tenant)
        {
            return (from a in context.TicketEscalations
                    where a.Tenant == tenant && a.TicketId == ticketId
                    select a);
        }

        public List<TicketEscalation> GetTicketEscalationsByListOfTicketIds(List<string> ticketIds, int tenant)
        {
            return (from a in context.TicketEscalations
                    where a.Tenant == tenant && ticketIds.Contains(a.TicketId) && a.IsSLAViolated == true
                    select a).ToList();
        }
   }
}
   