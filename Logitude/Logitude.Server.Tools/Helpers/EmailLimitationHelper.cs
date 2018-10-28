using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Helpers
{
    public static class EmailLimitationHelper
    {
        public static bool CheckEmailSendingQuotaForTenant(int tenant)
        {
            bool result = true;
            TenantRepository tenantRepository = new TenantRepository(tenant);
            int tenantEmailSendingQuota =  tenantRepository.GetTenantEmailSendingQuota(tenant);
            if (tenantEmailSendingQuota == 0) tenantEmailSendingQuota = LogitudeSettings.EmailSendingQuota;
            if (tenantEmailSendingQuota > 0)
            {
                CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(tenant);
                int communicationLogCount = communicationLogRep.GetCommunicationLogCountForTenantInLasthour(tenant);
                if (communicationLogCount > tenantEmailSendingQuota)
                {
                    result = false;
                }
            }

            return result;
        }

    }
}
