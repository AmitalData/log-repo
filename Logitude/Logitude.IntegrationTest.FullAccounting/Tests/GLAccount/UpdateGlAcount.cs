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

namespace Logitude.IntegrationTest.FullAccounting.Tests.GLAccount
{
    [TestClass]
    public class UpdateGlAcount
    {
        [TestMethod]
        public async Task UpdateGlAcount_Put_Successful()
        {
            GLAccountPM entityPM = await GetSingle();
            entityPM.LocalName = "GE:" + VariablesGenerater.GetRandomString(5);
            entityPM.EnglishName = "GE:" + VariablesGenerater.GetRandomString(5);
            HttpResponseMessage response = await RestClientService.PutAsync(entityPM, "GLAccounts");
            GLAccountPM GLAccountPM = RestClientService.ParseResponse<GLAccountPM>(response);
            Assert.IsNotNull(GLAccountPM);
            Assert.AreEqual(GLAccountPM.Id, entityPM.Id);
        }
       
        private async Task<GLAccountPM> GetSingle()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("GLAccounts/GetSingle?id=" + FullAccountingVariables.GLAccountVendor458GLPMId);
            GLAccountPM GLAccount = RestClientService.ParseResponse<GLAccountPM>(response);
            return GLAccount;
        }


    }
}
