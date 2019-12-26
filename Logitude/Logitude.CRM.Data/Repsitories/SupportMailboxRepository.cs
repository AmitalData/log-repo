 
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
   public partial class SupportMailboxRepository:IRepository<SupportMailbox>
   {
        
		public List<SupportMailbox> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public bool CheckIfDefaultMailBoxCreated(int tenant)
        {
            return (from a in context.SupportMailboxes
                      where a.IsDefault && a.Tenant == tenant
                      select a).Any();
        }

        public SupportMailbox GetDefaultMailBox(int tenant)
        {
            return (from a in context.SupportMailboxes
                    where a.IsDefault && a.Tenant == tenant
                    select a).FirstOrDefault();
        }
    }

}
   