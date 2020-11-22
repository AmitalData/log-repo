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

namespace Logitude.IntegrationTest.FullAccounting.Tests.Journal
{
    [TestClass]
    public class CreateJournal
    {
        [TestMethod]
        public async Task CreateJournal_Post_Successful()
        {
            JournalPM entityPM = GetNewJournal();
            HttpResponseMessage response = await RestClientService.PostAsync(entityPM, "Journals");
            JournalPM JournalPM = RestClientService.ParseResponse<JournalPM>(response);
            Assert.IsNotNull(JournalPM);
            Assert.IsNotNull(JournalPM.Id);
        }
   
        private JournalPM GetNewJournal()
        {
            JournalPM JournalPM = new JournalPM();
            JournalPM.Tenant = IntegrationTestLoginParameters.Tenant;
            JournalPM.CreateDate = DateTime.UtcNow;
            JournalPM.AccountingDate = DateTime.UtcNow;
            JournalPM.TypeCode = "0";
            JournalPM.StatusCode = "1";
            JournalPM.CreatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            JournalPM.CreatedByUserName = IntegrationTestLoginParameters.LoginUserName;
            JournalPM.AccountingEntityCode = "1";
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

            JournalLinePM CreditLine = new JournalLinePM();
            CreditLine.Tenant = IntegrationTestLoginParameters.Tenant;
            CreditLine.Line = 1;
            CreditLine.ActionCode ="1";
            CreditLine.CreditAccountId = FullAccountingVariables.GLAccountCustomer54l4CSPMId;
            CreditLine.DocumentDate = DateTime.UtcNow;
            CreditLine.AccountingDate = DateTime.UtcNow;
            CreditLine.DueDate = DateTime.UtcNow;
            CreditLine.LocalAmount = 100;
            CreditLine.CurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            CreditLine.ForeignAmount = 100;
            CreditLine.ExchangeRate = 1;
            CreditLine.ActionTypeCode = null;

            JournalPM.JournalLines.Add(CreditLine);

            JournalLinePM DebitLine = new JournalLinePM();
            DebitLine.Tenant = IntegrationTestLoginParameters.Tenant;
            DebitLine.Line = 1;
            DebitLine.ActionCode = "2";
            DebitLine.DebitAccountId = FullAccountingVariables.GLAccountVendor458GLPMId;
            DebitLine.DocumentDate = DateTime.UtcNow;
            DebitLine.AccountingDate = DateTime.UtcNow;
            DebitLine.DueDate = DateTime.UtcNow;
            DebitLine.LocalAmount = 100;
            DebitLine.CurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            DebitLine.ForeignAmount = 100;
            DebitLine.ExchangeRate = 1;
            DebitLine.ActionTypeCode = null;

            JournalPM.JournalLines.Add(DebitLine);

            return JournalPM;
        }

    }
}
