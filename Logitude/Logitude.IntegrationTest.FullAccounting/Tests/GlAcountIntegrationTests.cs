using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Logitude.IntegrationTest.FullAccounting;
using Logitude.Server.Tools.Counters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.FullAccounting.Tests
{
    [TestClass]
    public class GlAcountIntegrationTests
    {
        [TestMethod]
        public void GlACcount_Vendor458GLPM_Update()
        {
            Task.Run(async () =>
            {
                GLAccountPM entityPM = await GlACcount_Vendor458GLPM_GetSingle();
                entityPM.LocalName = "GE:" + RestClientService.GetRandomString(5);
                entityPM.EnglishName = "GE:" + RestClientService.GetRandomString(5);
                HttpResponseMessage response = await RestClientService.PutAsync(entityPM, "GLAccounts");


                GLAccountPM GLAccountVendor458GLPM = RestClientService.ParseResponse<GLAccountPM>(response);
                Assert.AreEqual(GLAccountVendor458GLPM.Id, entityPM.Id);
            }).GetAwaiter().GetResult();
        }
        private async Task<GLAccountPM> GlACcount_Vendor458GLPM_GetSingle()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("GLAccounts/GetSingle?id=" + FullAccountingVariables.GLAccountVendor458GLPMId);
            GLAccountPM GLAccountVendor458GLPM = RestClientService.ParseResponse<GLAccountPM>(response);
            return GLAccountVendor458GLPM;
        }

    }
}
