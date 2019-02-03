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
    public  class EmailLimitationHelper
    {
        public static EmailLimitationResult CheckEmailSendingQuotaForTenant(int tenant)
        {
            EmailLimitationResult result = new EmailLimitationResult();
            TenantRepository tenantRepository = new TenantRepository(tenant);
            int tenantEmailSendingQuota = tenantRepository.GetTenantEmailSendingQuota(tenant);
            if (tenantEmailSendingQuota == 0) tenantEmailSendingQuota = LogitudeSettings.EmailSendingQuota;
            if (tenantEmailSendingQuota > 0)
            {
                CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(tenant);
                int communicationLogCount = communicationLogRep.GetCommunicationLogCountForTenantInLasthour(tenant);
                if (communicationLogCount > tenantEmailSendingQuota)
                {
                    result.IsQuotaExceeded = true;

                    var message = tenantEmailSendingQuota == 1 ? "email" : "emails";
                    result.ExceptionMessage = "Quota exceeded. Can't send more than "+ tenantEmailSendingQuota+ " "+ message + " in one hour";
                }
            }

            return result;
        }
    }

    public class EmailLimitationResult
    {
        public string ExceptionMessage { get; set; }
        public bool IsQuotaExceeded { get; set; }
    }

}
