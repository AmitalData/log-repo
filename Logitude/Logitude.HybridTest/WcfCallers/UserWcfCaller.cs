using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class UserWcfCaller
    {
        public static Response CallUserUpsert()
        {
            Response prepareResponse = BranchWcfCaller.PrepareBranch();
            if (!prepareResponse.HasError)
            {
                prepareResponse = DepartmentWcfCaller.PrepareDepartment();
                if (!prepareResponse.HasError)
                {
                    UserPM entityPM = new UserPM()
                    {
                        Code = HybridData.UserCode,
                        EnglishName = "Hybrid User",
                        LocalName = "Hybrid User",
                        Email = "Hybrid@fnarsoft.com",
                        Password = "!H0",
                        BusinessUnitId = TestEnvironmentGlobalParameters.Tenant.ToString(),
                        BranchId = HybridData.BranchCode,
                        DepartmentId = HybridData.DepartmentCode,
                        Tenant = TestEnvironmentGlobalParameters.Tenant,
                        DocumentFilingInbox = "HybridInbox"
                    };

                    InvokedProperties serviceProperties = new InvokedProperties
                    {
                        ServiceName = "User",
                        ServiceOperation = "Upsert",
                        ServiceType = typeof(UserPM),
                        ServiceFilterType = null,
                    };

                    Response serviceResponse = new Response();
                    object[] serviceParameters = new object[] { entityPM, false };
                    WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
                    if (serviceResponse.Result != null)
                        HybridData.UserId = serviceResponse.Result;
                    return serviceResponse;
                }
            }
            return prepareResponse;
        }
        public static Response PrepareUser()
        {
            if (HybridData.UserId == null)
            {
                return CallUserUpsert();
            }
            return new Response();
        }
    }
}
