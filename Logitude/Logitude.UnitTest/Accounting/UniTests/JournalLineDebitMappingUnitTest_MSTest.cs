using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logitude.Accounting.BL.CoreBL.Mapping;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.UnitTest.Utils;
using System.Collections.Generic;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Test.Infrastructure;
using FakeItEasy;

namespace Logitude.UnitTest.Accounting.UniTests
{
    /// <summary>
    /// Refactored version of JournalLineDebitMappingUnitTest using FakeItEasy.
    /// Migrated from MSTest Fakes to FakeItEasy for compatibility with Visual Studio Community.
    /// </summary>
    [TestClass]
    public class JournalLineDebitMappingUnitTest_MSTest : TestBase_MSTest
    {
        [TestMethod]
        public void JournalLineDebitMappingDoIt_ActionTypeCodeIs1_ExpectedException()
        {
            int tenant = 1;
            string id = "1-1";

            var myState =
                Tuple.Create<JournalLinePM, JournalPM>(
                    new JournalLinePM() { Tenant = tenant, JournalId = id, ActionTypeCode = /*Credit =*/ "1" }, 
                    new JournalPM { Tenant = 1, Id = id });

            // Arrange
            var JournalLineDebitMapping = GetVatExtractFalseDebitMap_FakeItEasy(myState.Item1, myState.Item2);
            
            // Act & Assert: Run the method under test and verify exception
            TestsUtil.AssertThrows<Exception>(delegate
            {
                JournalLineDebitMapping.DoIt();
            },
            expectedContainsMessage: "this.MyMappingTypeEnum != MappingTypeEnum.Credit");
        }

        private static JournalLineDebitMapping GetVatExtractFalseDebitMap_FakeItEasy(
            JournalLinePM journalLine, JournalPM journalPM, Func<string, GLAccountPM> func = null)
        {
            // Create fakes for interfaces using FakeItEasy
            var fakeIGLAccountDataProvider = A.Fake<IGLAccountDataProvider>();
            
            if (func != null)
            {
                // Configure the fake to use the provided function
                A.CallTo(() => fakeIGLAccountDataProvider.GetGLAccount(A<string>.Ignored, A<int>.Ignored))
                    .ReturnsLazily((string pId, int tenant) => func(pId));
            }
            
            var fakeIAccountingSettingResolver = A.Fake<IAccountingSettingResolver>();
            
            return new JournalLineDebitMapping(
                journalLine, 
                journalPM, 
                false, 
                fakeIGLAccountDataProvider, 
                fakeIAccountingSettingResolver);
        }
    }
}

