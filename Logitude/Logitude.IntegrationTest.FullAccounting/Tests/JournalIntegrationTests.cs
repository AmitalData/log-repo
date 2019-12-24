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
    public class JournalIntegrationTests
    {
        [TestMethod]
        public async Task Post()
        {
                JournalPM entityPM = GetNewJourna();
                HttpResponseMessage response = await RestClientService.PostAsync(entityPM, "Journals");
                JournalPM JournalPM = RestClientService.ParseResponse<JournalPM>(response);
                Assert.AreEqual(JournalPM.Id, entityPM.Id);
        }

        private JournalPM GetNewJourna()
        {
            JournalPM JournalPM = new JournalPM();
            JournalPM.Tenant = IntegrationTestLoginParameters.Tenant;
            JournalPM.CreateDate = DateTime.UtcNow;
            JournalPM.AccountingDate = DateTime.UtcNow;
            JournalPM.TypeCode = "0";
            JournalPM.StatusCode = "2";
            JournalPM.CreatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            JournalPM.CreatedByUserName = IntegrationTestLoginParameters.LoginUserName;
            JournalPM.AccountingEntityCode = "0";
            JournalPM.UpdateDate = DateTime.UtcNow;
            JournalPM.UpdatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            JournalPM.UpdatedByUserName = IntegrationTestLoginParameters.LoginUserName;
            JournalPM.ApproveDate = DateTime.UtcNow;
            JournalPM.ApprovedByUserId = IntegrationTestLoginParameters.LoginUserId;
            var RandomString = VariablesGenerater.GetRandomString(5);
            JournalPM.SearchFields = "GE:JO:"+ RandomString;
            JournalPM.AccountingEntityReference = "GE:JO"+ RandomString;
            JournalPM.IsVoided = false;
            JournalPM.IsLedgerCreated = true;
            JournalPM.JournalLines = new List<JournalLinePM>();
            JournalLinePM Line = new JournalLinePM();
            Line.Tenant = IntegrationTestLoginParameters.Tenant;
            Line.Line = 1;
            Line.ActionCode ="3";
            Line.DebitAccountId = FullAccountingVariables.GLAccountVendor458GLPMId;
            Line.CreditAccountId = FullAccountingVariables.GLAccountCustomer54l4CSPMId;
            Line.DocumentDate = DateTime.UtcNow;
            Line.AccountingDate = DateTime.UtcNow;
            Line.DueDate = DateTime.UtcNow;
            Line.LocalAmount = 100;
            Line.CurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            Line.ForeignAmount = 100;
            Line.ExchangeRate = 1;
            Line.ActionTypeCode = "3";
            JournalPM.JournalLines.Add(Line);
            return JournalPM;
        }

    }
}
