using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class BranchWcfCaller
    {
        public static Response CallBranchUpsert()
        {
            BranchPM entityPM = new BranchPM()
            {
                Code = HybridData.BranchCode,
                EnglishName = "Hybrid Branch",
                LocalName = "Hybrid Branch",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Branch",
                ServiceOperation = "Upsert",
                ServiceType = typeof(BranchPM),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            if(!serviceResponse.HasError && serviceResponse.Result != null)
                HybridData.BranchId = serviceResponse.Result;
            return serviceResponse;
        }
        public static Response PrepareBranch()
        {

            Assert.Inconclusive("Test from branch");
            if (HybridData.BranchId == null)
            {
                return CallBranchUpsert();
            }
            return new Response();
        }
    }
}
