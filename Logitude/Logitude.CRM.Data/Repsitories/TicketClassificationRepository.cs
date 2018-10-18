 
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
using System.IO;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class TicketClassificationRepository:IRepository<TicketClassification>
   {
        
		public List<TicketClassification> GetMulti(EntityKeyFields entityKeys)
        {
			throw new NotImplementedException();
        }

        public TicketClassification GetTicketClassificationByName(string name, int tenant)
        {
            return (from a in context.TicketClassifications where a.Name == name && a.Tenant == tenant && a.Id == tenant.ToString() select a).FirstOrDefault();
        }

        public List<string> GetAllIds(int tenant)
        {
            List<string> myResult = context.TicketClassifications.Where(d => d.Tenant == tenant).Select(s => s.Id).ToList();
            return myResult;
        }

        public bool IsTicketClassificationExists(int tenant)
        {
            return context.TicketClassifications.Where(d => d.Tenant == tenant && d.Name == "General").Any();
        }

        public TicketClassification GetMainTicketClassificationByTenant( int tenant)
        {
            return (from a in context.TicketClassifications where a.Name == "General" && a.Tenant == tenant && a.Id == tenant.ToString() select a).FirstOrDefault();
        }
   }
}
   