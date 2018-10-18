using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Logitude.UnitTest.Utils;
using Logitude.Accounting.Data.EntityPOCOs;
using Newtonsoft.Json;
using Logitude.Accounting.Def.EntityPMs;
using FakeItEasy;
using Logitude.Server.Tools.Counters;

namespace Logitude.UnitTest.Accounting.IntegrationTests
{
    [TestClass]
    public class JournalUpdateUnitTest
    {

        [TestMethod]
        [Ignore]
        public void JournalUpdateApproved_ApprovedJournalCanOnlyChangeToVoided_ThrowBLException()
        {
            int JournalCounter = 1;
            try
            {

                var date = new DateTime(201612 - 21);
                var journalPM = GetJournalPM();
                var fakeAC = (new AccountingFakeFactory()).CreateFakeIAccountingContext(null, null, null);
                var fakeUs = (new AccountingFakeFactory()).CreateFakeJournalUpdateService(journalPM.Tenant, fakeAC);

                //fakeUs.OverrideJournalValidationContext= 
                fakeUs.Update(journalPM, true);

            }
            finally
            {
                //IdCounter.FakeOverrideIIdCounter = null;
            }


        }


        [TestMethod]
        [Ignore]
        public void OnUpdating_ApprovedJournalCanOnlyChangeToVoided_ThrowBLException()
        {
            throw new Exception("BLException =Approved Journal Can Only Change To Voided");
        }

        [TestMethod]
        [Ignore]
        public void OnUpdating_ApprovedJournalChangeToVoided_GetStorno()
        {
            throw new Exception("OnUpdating_ApprovedJournalChangeToVoided_GetStorno");
        }

        JournalPM GetJournalPM()
        {
            var json = @"{
  ""Id"": null,
  ""Tenant"": 989,
  ""JournalNumber"": ""1003"",
  ""CreateDate"": ""2016-12-21T15:49:23.5506058+02:00"",
  ""AccountingDate"": ""2016-12-21T15:49:23.5476055+02:00"",
  ""TypeCode"": ""0"",
  ""StatusCode"": ""2"",
  ""CreatedByUserId"": ""1-1"",
  ""AccountingEntityCode"": ""1"",
  ""AccountingEntityId"": ""1-69"",
  ""ExternalNo"": null,
  ""TypeName"": null,
  ""StatusName"": null,
  ""CreatedByUserName"": null,
  ""AccountingEntityName"": null,
  ""JournalLines"": [
    {
      ""JournalId"": null,
      ""Tenant"": 989,
      ""Line"": 1,
      ""ActionCode"": null,
      ""DebitControlAccountId"": null,
      ""DebitAccountId"": ""1-64"",
      ""CreditControlAccountId"": null,
      ""CreditAccountId"": ""1-63"",
      ""DocumentDate"": ""2016-12-21T15:49:23.5536061+02:00"",
      ""AccountingDate"": ""2016-12-21T15:49:23.5476055+02:00"",
      ""DueDate"": ""2016-12-21T15:49:23.5536061+02:00"",
      ""LocalAmount"": 100,
      ""CurrencyId"": ""1-4438"",
      ""ForeignAmount"": 25,
      ""ExchangeRate"": 4,
      ""Reference1"": ""ref1636179321635486056"",
      ""Reference2"": ""ref2636179321635486056"",
      ""Reference3"": ""ref3636179321635486056"",
      ""ActionName"": null,
      ""DebitControlAccountName"": null,
      ""CreditAccountName"": null,
      ""DebitAccountName"": null,
      ""CreditControlAccountName"": null,
      ""CreditControlAccountNumber"": null,
      ""DebitControlAccountNumber"": null,
      ""CreditAccountNumber"": null,
      ""DebitAccountNumber"": null,
      ""CurrencyName"": null,
      ""Notes"": ""Notes636179321635486056"",
      ""CurrencyCode"": null,
      ""ActionTypeCode"": ""4"",
      ""ExternalOpenAmount"": null,
      ""IsCreditAccountMulti"": null,
      ""IsDebitAccountMulti"": null,
      ""ActionTypeCodeEnum"": 4,
      ""ChangeSetOp"": 1,
      ""EncodeBase64NVARCHARFieldsBy"": null
    }
  ],
  ""UpdateDate"": ""2016-12-21T15:49:23.5506058+02:00"",
  ""UpdatedByUserId"": ""1-1"",
  ""ApproveDate"": null,
  ""ApprovedByUserId"": null,
  ""UpdatedByUserName"": null,
  ""ApprovedByUserName"": null,
  ""SearchFields"": null,
  ""AccountingEntityReference"": ""AER636179321635486056"",
  ""OriginalJournalId"": null,
  ""VoidedByUserId"": null,
  ""VoidDate"": null,
  ""OriginalJournalName"": null,
  ""VoidedByUserName"": null,
  ""IsVoided"": null,
  ""VoidedByJournalId"": null,
  ""ExternalSystem"": null,
  ""QueueId"": null,
  ""StatusLocalName"": null,
  ""ChangeSetOp"": 1,
  ""EncodeBase64NVARCHARFieldsBy"": null
}";

            return JsonConvert.DeserializeObject<JournalPM>(json);
        }
    }
}
