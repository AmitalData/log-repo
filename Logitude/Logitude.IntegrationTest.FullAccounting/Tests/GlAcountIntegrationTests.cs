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
        public async Task Put()
        {
            GLAccountPM entityPM = await GetSingle();
            entityPM.LocalName = "GE:" + VariablesGenerater.GetRandomString(5);
            entityPM.EnglishName = "GE:" + VariablesGenerater.GetRandomString(5);
            HttpResponseMessage response = await RestClientService.PutAsync(entityPM, "GLAccounts");
            GLAccountPM GLAccountPM = RestClientService.ParseResponse<GLAccountPM>(response);
            Assert.AreEqual(GLAccountPM.Id, entityPM.Id);
        }
        [TestMethod]
        public  async Task Post()
        {
            GLAccountPM entityPM = GetNewGlAccountPM();
            HttpResponseMessage response = await RestClientService.PostAsync(entityPM, "GLAccounts");
            GLAccountPM gLAccountPM = RestClientService.ParseResponse<GLAccountPM>(response);
            Assert.IsNotNull(gLAccountPM.Id);
        }
        private static GLAccountPM GetNewGlAccountPM()
        {
          
            GLAccountPM gLAccountPM = new GLAccountPM();
            gLAccountPM.Tenant = IntegrationTestLoginParameters.Tenant;
            gLAccountPM.EnglishName = "GE:" + VariablesGenerater.GetCharactersRandomString(4);
            gLAccountPM.LocalName = "GE:" + VariablesGenerater.GetCharactersRandomString(4);
            gLAccountPM.DisplayNumber = "GE:" + VariablesGenerater.GetNumbersRandomString(4);
            gLAccountPM.SearchFields = gLAccountPM.EnglishName + "," + gLAccountPM.DisplayNumber;
            gLAccountPM.AccountTypeCode = "1";
            gLAccountPM.RevenueExpenseType = "3";
            gLAccountPM.ChartOfAccountsId = FullAccountingVariables.ChartOfAccountBankBK771Id;
            gLAccountPM.ChartOfAccountsTypeCode = "5";
            gLAccountPM.ReconcileMethodCode = "0";
            gLAccountPM.CurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            gLAccountPM.NewGLAccountCardId = FullAccountingVariables.CustomerTestGlCust12PMCSId;
            return gLAccountPM;
        }

        private async Task<GLAccountPM> GetSingle()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("GLAccounts/GetSingle?id=" + FullAccountingVariables.GLAccountVendor458GLPMId);
            GLAccountPM GLAccount = RestClientService.ParseResponse<GLAccountPM>(response);
            return GLAccount;
        }


    }
}
