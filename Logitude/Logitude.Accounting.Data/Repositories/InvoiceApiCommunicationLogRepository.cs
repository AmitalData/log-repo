 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class InvoiceApiCommunicationLogRepository:IRepository<InvoiceApiCommunicationLog>
   {
        
		public List<InvoiceApiCommunicationLog> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public InvoiceApiCommunicationLog GetByExternalID(string externalId, int tenant)
        {
            return (from a in context.InvoiceApiCommunicationLogs
                    where a.ExternalID == externalId && a.Tenant == tenant
                    select a).FirstOrDefault();
        }
    }

}
   