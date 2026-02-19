using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class InvoiceApiCommunicationLogQueryService
    {
        public InvoiceApiCommunicationLog GetByExternalID(string externalId, int tenant)
        {
            InvoiceApiCommunicationLogRepository invoiceApiCommunicationLogRepository = new InvoiceApiCommunicationLogRepository(tenant);
            InvoiceApiCommunicationLog invoiceApiCommunicationLog = invoiceApiCommunicationLogRepository.GetByExternalID(externalId, tenant);
            return invoiceApiCommunicationLog;
        }

        
    }
}
