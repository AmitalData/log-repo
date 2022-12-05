using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel;
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

            HybridPartnerQuery hybridPartnerQuery = new HybridPartnerQuery(tenant);
            var selectedHybridPartner = hybridPartnerQuery.GetSinglePMByPartnerTenant(signUpInfo.Tenant);
            if (selectedHybridPartner == null) return;

            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            CustomerTenantAccessRequestService service = new CustomerTenantAccessRequestService(commonContext, tenant);
            CustomerTenantAccessRequestPM customerTenantAccessRequestPM = new CustomerTenantAccessRequestPM
            {
                ForwarderId = selectedHybridPartner.Id,
                Tenant = tenant,
            };
            service.Create(customerTenantAccessRequestPM);

            customerTenantAccessRequestPM.RequestStatus = "W";
            service.Update(customerTenantAccessRequestPM);
        }
    }
}