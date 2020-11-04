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

namespace Logitude.IntegrationTest.FullAccounting.Tests.CashBook
{
    [TestClass]
    public class CreateCashBook
    {
        [TestMethod]
        public  async Task CreateCashBook_Post_Successful()
        {
            CashBookPM entityPM = GetNewCashBook1421PM();
            HttpResponseMessage response = await RestClientService.PostAsync(entityPM, "CashBooks");
            CashBookPM CashBookPM = RestClientService.ParseResponse<CashBookPM>(response);
            Assert.IsNotNull(CashBookPM);
            Assert.IsNotNull(CashBookPM.Id);
        }
        private static CashBookPM GetNewCashBook1421PM()
        {
            CashBookPM CashBookPM = new CashBookPM();
            CashBookPM.Tenant = IntegrationTestLoginParameters.Tenant;
            CashBookPM.EnglishName = "CashBook1421";
            CashBookPM.LocalName = "CashBook1421";
            CashBookPM.CurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            CashBookPM.CashBookTypeCode = "1";
            CashBookPM.BranchId = FullAccountingVariables.NewBranchCashBookBK14Id;
            CashBookPM.AccountId = FullAccountingVariables.GLAccountBank1414BKPMId;
            CashBookPM.TotalAmount = 50000;
            CashBookPM.UpdateDate = DateTime.UtcNow;
            CashBookPM.UpdatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            CashBookPM.CreateDate = DateTime.UtcNow;
            CashBookPM.CreatedByUserId = IntegrationTestLoginParameters.LoginUserId;

            return CashBookPM;
        }


    }
}
