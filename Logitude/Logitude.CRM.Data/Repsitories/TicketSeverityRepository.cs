 
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
   public partial class TicketSeverityRepository:IRepository<TicketSeverity>
   {
        
		public List<TicketSeverity> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public TicketSeverity GetTicketSeverityByCode(string code, int tenant)
        {
            return (from a in context.TicketSeverities where a.Code == code && a.Tenant == tenant select a).FirstOrDefault();
        }
   }

}
   