using Logitude.Accounting.Data.EntityPOCOs;
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

        public InvoiceApiCommunicationLogPM GetByCommunicationId(string communicationId, int tenant)
        {
            InvoiceApiCommunicationLog poco = this.repository.GetByCommunicationId(communicationId, tenant);
            if (poco != null)
            {
                return GetEntityPM(poco);
            }
            else
            {
                return null;
            }
        }

    }
}
