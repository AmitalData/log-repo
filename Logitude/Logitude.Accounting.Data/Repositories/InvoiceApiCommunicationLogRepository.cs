 
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

        public InvoiceApiCommunicationLog GetByCommunicationId(string communicationId, int tenant)
        {
            return context.InvoiceApiCommunicationLogs
                .FirstOrDefault(x => x.CommunicationId == communicationId && x.Tenant == tenant);
        }

    }

}
   