using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Logitude.Test.Infrastructure;
using FakeItEasy;

namespace Logitude.UnitTest.Accounting.UniTests.PaymentChequeTests
{
    /// <summary>
    /// Refactored version of PaymentChequeDataMappingUnitTest using FakeItEasy.
    /// Migrated from MSTest Fakes to FakeItEasy for compatibility with Visual Studio Community.
    /// </summary>
    [TestClass]
    public class PaymentChequeDataMappingUnitTest_MSTest : TestBase_MSTest
    {
        [TestMethod]
        public void PMToPOCO_CheckCustomMappedFieldsMatchExpected_Success()
        {
            string expected_Id = "111";
            int expected_tenant = 1;
            string Expected_SearchFields = "search";

            PaymentChequePM entityPM = new PaymentChequePM()
            {
                Id = expected_Id,
                Tenant = expected_tenant,
                SearchFields = Expected_SearchFields,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert
            };
            PaymentCheque entityPOCO = new PaymentCheque();

            var paymentChequeCustomDataMapping = A.Fake<PaymentChequeCustomDataMapping>(option => option.CallsBaseMethods());
            paymentChequeCustomDataMapping.PMToPOCO(entityPM, entityPOCO, new List<PaymentChequeDataMapping.POCOPropertyNames>());

            Assert.AreEqual(expected_Id, entityPOCO.Id, "Id not matches expected Id");
            Assert.AreEqual(expected_tenant, entityPOCO.Tenant, "Tenant not matches expected tenant");
            Assert.AreEqual(Expected_SearchFields, entityPOCO.SearchFields, "search fields not matches expected search fields ");
        }

        [TestMethod]
        public void POCOToPM_CheckCustomMappedFieldsMatchExpected_Success()
        {
            JournalPM journal = new JournalPM()
            {
                Id = "115",
                JournalNumber = "1",
                Tenant = 1,
            };
            GLAccountPM PayToGLAccount = new GLAccountPM()
            {
                Id = "GLA1",
                IsMultiCurrency = false,
                CurrencyId = "CUR1",
                DisplayNumber = "123456",
                Tenant = 1,
            };

            GLAccountPM BankAccountGLAccountPM = new GLAccountPM()
            {
                Id = "GLA2",
                IsMultiCurrency = false,
                CurrencyId = "CUR1",
                DisplayNumber = "654321",
                Tenant = 1,
            };

            BankAccountPM BankAccount = new BankAccountPM()
            {
                Id = "BA1",
                BankId = "11",
                Tenant = 1,
            };

            PaymentChequeStatusPM PaymentChequeStatusPM = new PaymentChequeStatusPM()
            {
                Code = "1",
                LocalName = "Draft"
            };

            PaymentChequePM paymentChequePM = new PaymentChequePM()
            {
                Tenant = 1,
                PayToGLAccountId = PayToGLAccount.Id,
                BankAccountGLAccountId = BankAccountGLAccountPM.Id,
                BankAccountId = BankAccount.Id,
                PaymentChequeStatusCode = PaymentChequeStatusPM.Code,
                JournalId = journal.Id,
                GLAccountNumber = PayToGLAccount.DisplayNumber,
                JournalNumber = journal.JournalNumber,
            };

            PaymentCheque paymentCheque = new PaymentCheque()
            {
                Tenant = 1,
                PayToGLAccountId = PayToGLAccount.Id,
                BankAccountGLAccountId = BankAccountGLAccountPM.Id,
                BankAccountId = BankAccount.Id,
            };

            var paymentChequeCustomDataMapping = A.Fake<PaymentChequeCustomDataMapping>(option => option.CallsBaseMethods());
            A.CallTo(() => paymentChequeCustomDataMapping.GetSingleBankAccountPM(BankAccount.Id, paymentCheque.Tenant, A<bool>.Ignored)).Returns(BankAccount);
            A.CallTo(() => paymentChequeCustomDataMapping.GetSingleGLAccountPM(PayToGLAccount.Id, paymentCheque.Tenant)).Returns(PayToGLAccount);
            A.CallTo(() => paymentChequeCustomDataMapping.GetSingleGLAccountPM(BankAccountGLAccountPM.Id, paymentCheque.Tenant)).Returns(BankAccountGLAccountPM);
            A.CallTo(() => paymentChequeCustomDataMapping.GetSingleJournalPM(journal.Id, paymentCheque.Tenant)).Returns(journal);

            paymentChequeCustomDataMapping.POCOToPM(paymentChequePM, paymentCheque, new List<PaymentChequeDataMapping.PMPropertyNames>());

            Assert.AreEqual(PayToGLAccount.CurrencyId, paymentChequePM.GLAccountCurrencyId, "CurrencyId not matches expected CurrencyId ");
            Assert.AreEqual(PayToGLAccount.DisplayNumber, paymentChequePM.GLAccountNumber, "GLAccountNumber not matches expected GLAccountNumber ");
            Assert.AreEqual(BankAccountGLAccountPM.CurrencyId, paymentChequePM.BankGLAccountCurrencyId, "BankGLAccountCurrencyId not matches expected BankGLAccountCurrencyId ");
            Assert.AreEqual(BankAccount.LocalName, paymentChequePM.BankLocalName, "BankLocalName not matches expected BankLocalName ");
            Assert.AreEqual(BankAccount.EnglishName, paymentChequePM.BankEnglishName, "BankEnglishName not matches expected BankEnglishName ");
            Assert.AreEqual(journal.Id, paymentChequePM.JournalId, "JournalId not matches expected JournalId ");
            Assert.AreEqual(journal.JournalNumber, paymentChequePM.JournalNumber, "JournalNumber not matches expected JournalNumber ");
        }
    }
}

