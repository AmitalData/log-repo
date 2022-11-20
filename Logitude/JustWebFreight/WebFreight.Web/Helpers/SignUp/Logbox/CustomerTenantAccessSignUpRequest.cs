using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.QueueService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.InfrastructureModel;

namespace WebFreight.Web.Helpers.SignUp.Logbox
{
    public class CustomerTenantAccessSignUpRequest
    {
        public static void Send(SignUpInfoClass signUpInfo, int tenant)
        {
            if (!signUpInfo.IsCreateLogboxTenantFromCloud) return;

            HybridPartnerQuery hybridPartnerQuery = new HybridPartnerQuery(signUpInfo.Tenant);
            var hybridPartners = hybridPartnerQuery.GetHybridPartnerLists(signUpInfo.Tenant);
            if (hybridPartners == null) return;
            var selectedHybridPartner = hybridPartners.Where(hybridPartner => hybridPartner.PartnerTenant == signUpInfo.Tenant).FirstOrDefault();
            if (selectedHybridPartner == null) return;

            IQueueService queue = new DbQueueService();
            queue.InitializeQueue("CustomerTenantAccessRequestQueue", 0);
            queue.Send(new Dictionary<string, string>() { { "RequestId", selectedHybridPartner.Id }, { "Tenant", tenant.ToString() } }, tenant);
        }
    }
}