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
    public class CreateGlAcount
    {
        [TestMethod]
        public  async Task CreateGlAcount_Post_Successful()
        {
            GLAccountPM entityPM = GetNewGlAccountPM();
            HttpResponseMessage response = await RestClientService.PostAsync(entityPM, "GLAccounts");
            GLAccountPM gLAccountPM = RestClientService.ParseResponse<GLAccountPM>(response);
            Assert.IsNotNull(gLAccountPM);
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
 

    }
}
