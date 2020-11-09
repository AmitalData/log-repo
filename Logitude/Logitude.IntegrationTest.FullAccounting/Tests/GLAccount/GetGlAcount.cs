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
    public class GetGlAcount
    {
         
        [TestMethod]
        public async Task GetGlAcount_Get_Successful()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("GLAccounts/GetSingle?id=" + FullAccountingVariables.GLAccountVendor458GLPMId);
            GLAccountPM GLAccount = RestClientService.ParseResponse<GLAccountPM>(response);
            Assert.IsNotNull(GLAccount);
            Assert.AreEqual(GLAccount.Id, FullAccountingVariables.GLAccountVendor458GLPMId);
        }
        

    }
}
