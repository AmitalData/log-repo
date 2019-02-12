using System;
using System.Collections.Generic;
using FakeItEasy;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.UnitTest.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.UnitTest.Accounting.UniTests.BankAccountTests
{
    [TestClass]
    public class BankAccountCustomDataMappingUnitTest
    {
        [TestMethod]
        public void POCOToPM_CheckCustomMappedFieldsMatchExpected_Success()
        {
            GLAccountPM glAccountPM = new GLAccountPM()
            {
                Id = "GLA1",
                IsMultiCurrency = false,
                CurrencyId = "CUR1",
                DisplayNumber = "123456",
                Tenant = 1,
            };

            GLAccountPM defferedGlAccountPM = new GLAccountPM()
            {
                Id = "GLA2",
                IsMultiCurrency = false,
                CurrencyId = "CUR1",
                DisplayNumber = "654321",
                Tenant = 1,
            };

            BankCodePM bankCodePM = new BankCodePM()
            {
                Id="BA1",
                Code="MyBA1",
                Tenant = 1,
            };

            BankAccount bankAccount = new BankAccount()
            {
                Tenant=1,
                GLAccountId = glAccountPM.Id,
                DeferredGLAccountId=defferedGlAccountPM.Id,
                BankId=bankCodePM.Id,

            };

            BankAccountPM bankAccountPM = new BankAccountPM()
            {
                Tenant = 1,
                GLAccountId = glAccountPM.Id,
                DeferredGLAccountId = defferedGlAccountPM.Id,
                BankId = bankCodePM.Id,
            };

            var bankAccountCustomDataMapping = A.Fake<BankAccountCustomDataMapping>(option => option.CallsBaseMethods());
            A.CallTo(() => bankAccountCustomDataMapping.GetSingleBankCodePM(bankAccount.BankId,bankAccount.Tenant,A<bool>.Ignored)).Returns(bankCodePM);
            A.CallTo(() => bankAccountCustomDataMapping.GetSingleGLAccountPM(bankAccount.GLAccountId, bankAccount.Tenant, A<bool>.Ignored)).Returns(glAccountPM);
            A.CallTo(() => bankAccountCustomDataMapping.GetSingleGLAccountPM(bankAccount.DeferredGLAccountId, bankAccount.Tenant, A<bool>.Ignored)).Returns(defferedGlAccountPM);

            bankAccountCustomDataMapping.POCOToPM(bankAccountPM, bankAccount, new List<BankAccountDataMapping.PMPropertyNames>());

           
            Assert.AreEqual(glAccountPM.CurrencyId, bankAccountPM.GLAccountCurrencyId);
            Assert.AreEqual(glAccountPM.DisplayNumber,bankAccountPM.GLAccountNumber);
            Assert.AreEqual(defferedGlAccountPM.DisplayNumber,bankAccountPM.DeferedGLAccountNumber);
            Assert.AreEqual(bankCodePM.Code,bankAccountPM.BankCode);

        }

        [TestMethod]
        public void PMToPOCO_CheckCustomMappedFieldsMatchExpected_Success()
        {
            BankAccount bankAccount = new BankAccount()
            {

            };

            BankAccountPM bankAccountPM = new BankAccountPM()
            {
                Id="MyId",
                Tenant = 1,
                GLAccountId = "GL1",
                DeferredGLAccountId = "DGL2",
                BankId ="BA1",
                SearchFields="my Search fields",
                ChangeSetOp=Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            };

            var bankAccountCustomDataMapping = A.Fake<BankAccountCustomDataMapping>(option => option.CallsBaseMethods());
            bankAccountCustomDataMapping.PMToPOCO(bankAccountPM, bankAccount, new List<BankAccountDataMapping.POCOPropertyNames>());

            Assert.AreEqual(bankAccountPM.Tenant, bankAccount.Tenant);
            Assert.AreEqual(bankAccountPM.Id, bankAccount.Id);
            Assert.AreEqual(bankAccountPM.SearchFields, bankAccount.SearchFields);

        }
    }
}
