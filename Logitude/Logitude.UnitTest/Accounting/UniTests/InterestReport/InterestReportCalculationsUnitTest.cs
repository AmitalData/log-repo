using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplog.Server.Infrastructure.Helpers;
using System.Collections.Generic;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.UnitTest.Utils;
using FakeItEasy;
using Logitude.Accounting.BL.CoreBL.InterestReport;
using Logitude.Accounting.Data.Utilities;
using Logitude.Accounting.BL.Validators;
using Logitude.Server.Tools.Helpers;

namespace Logitude.UnitTest.Accounting.UniTests.InterestReport
{
    [TestClass]
    public class InterestReportCalculationsUnitTest
    {
        [TestInitialize]
        public void InterestReportCalculationsTestInitialize()
        {
           
        }

        [TestMethod]
        public void CreateInterestReportLinesByDate_InputInterestTransactionsByDate_ResultInInterestReportLinesByDate()
        {
           
            IInterestReportCalculationPreparations interestReportCalculationPreparations = new InterestReportCalculationFromCSVPreparations(@"Accounting\UniTests\InterestReport\");
            InterestReportTestResultGetter interestReportTestResultGetter = new InterestReportTestResultGetter();
            InterestReportPM interestReportPM = interestReportCalculationPreparations.GetInterestReportPM("", 1);
            GLAccountPM gLAccountPM = interestReportCalculationPreparations.GetGLAccount("", 1);
            InterestTransactionGetParameters interestTransactionGetParameters = new InterestTransactionGetParameters(interestReportPM.InterestCalculationDate, 1,new List<string>() { interestReportPM.GLAccountId }, gLAccountPM.InterestCalculationStartDate);
            List<InterestTransactionPM> interestTransactionPMs = interestReportCalculationPreparations.GetInterestTransactionsForGlAccountAndInterestValueDate(interestTransactionGetParameters);
            List<GLAccountInterestPeriodPM> gLAccountInterestPeriodPMs = interestReportCalculationPreparations.GetGlaccountInterestPeriods(interestReportPM);
            List<InterestBasesPeriodPM> interestBasesPeriodPMs = interestReportCalculationPreparations.GetAllInterestBasesPeriodPMs(1);
            List<InterestReportLinesByDatePM> resultInterestReportLinesByDatePMs = interestReportTestResultGetter.GetInterestReportLinesByDatePMsFromCSV(@"Accounting\UniTests\InterestReport\");

            InterestReportLinesByDateCreationService interestReportLinesByDateCreationService = new InterestReportLinesByDateCreationService();
            InterestReportLinesByDateCreationParams interestReportLinesByDateCreationParams = new InterestReportLinesByDateCreationParams(interestReportPM, 
                interestTransactionPMs, gLAccountInterestPeriodPMs, interestBasesPeriodPMs,null);
            List<InterestReportLinesByDatePM> interestReportLinesByDatePMs = interestReportLinesByDateCreationService.CreateInterestReportLinesByDate(interestReportLinesByDateCreationParams);

            AssertResults(resultInterestReportLinesByDatePMs, interestReportLinesByDatePMs);


        }


        [TestMethod]
        public void CreateInterestReportLinesByDate_InputInterestTransactionsByDateWithNoTransactions_NoInterestReportLinesByDate()
        {

            IInterestReportCalculationPreparations interestReportCalculationPreparations = new InterestReportCalculationFromCSVPreparations(@"Accounting\UniTests\InterestReport\");
            InterestReportPM interestReportPM = interestReportCalculationPreparations.GetInterestReportPM("", 1);
            GLAccountPM gLAccountPM = interestReportCalculationPreparations.GetGLAccount("", 1);
            List<InterestTransactionPM> interestTransactionPMs = new List<InterestTransactionPM>();
            List<GLAccountInterestPeriodPM> gLAccountInterestPeriodPMs = interestReportCalculationPreparations.GetGlaccountInterestPeriods(interestReportPM);
            List<InterestBasesPeriodPM> interestBasesPeriodPMs = interestReportCalculationPreparations.GetAllInterestBasesPeriodPMs(1);

            InterestReportLinesByDateCreationService interestReportLinesByDateCreationService = new InterestReportLinesByDateCreationService();
            InterestReportLinesByDateCreationParams interestReportLinesByDateCreationParams = new InterestReportLinesByDateCreationParams(interestReportPM,
                interestTransactionPMs, gLAccountInterestPeriodPMs, interestBasesPeriodPMs,null);
            List<InterestReportLinesByDatePM> interestReportLinesByDatePMs = interestReportLinesByDateCreationService.CreateInterestReportLinesByDate(interestReportLinesByDateCreationParams);

            Assert.AreEqual(0, interestReportLinesByDatePMs.Count);

        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "InterestReport.O.NoGlAccountPeriod")]
        [Ignore]
        public void CreateInterestReportLinesByDate_InputNoGlaccountInterestPeriods_NoInterestReportLinesByDate()
        {

            IInterestReportCalculationPreparations interestReportCalculationPreparations = new InterestReportCalculationFromCSVPreparations(@"Accounting\UniTests\InterestReport\");
            InterestReportTestResultGetter interestReportTestResultGetter = new InterestReportTestResultGetter();
            InterestReportPM interestReportPM = interestReportCalculationPreparations.GetInterestReportPM("", 1);
            GLAccountPM gLAccountPM = interestReportCalculationPreparations.GetGLAccount("", 1);
            InterestTransactionGetParameters interestTransactionGetParameters = new InterestTransactionGetParameters(interestReportPM.InterestCalculationDate, 1, new List<string>() { interestReportPM.GLAccountId }, gLAccountPM.InterestCalculationStartDate);
            List<InterestTransactionPM> interestTransactionPMs = interestReportCalculationPreparations.GetInterestTransactionsForGlAccountAndInterestValueDate(interestTransactionGetParameters);
            List<GLAccountInterestPeriodPM> gLAccountInterestPeriodPMs = new List<GLAccountInterestPeriodPM>();
            List<InterestBasesPeriodPM> interestBasesPeriodPMs = interestReportCalculationPreparations.GetAllInterestBasesPeriodPMs(1);
            List<InterestReportLinesByDatePM> resultInterestReportLinesByDatePMs = interestReportTestResultGetter.GetInterestReportLinesByDatePMsFromCSV(@"Accounting\UniTests\InterestReport\");

            var interestReportLinesByDateCreationService = A.Fake<InterestReportLinesByDateCreationService>();//new InterestReportLinesByDateCreationService();
            A.CallTo(() =>
            interestReportLinesByDateCreationService.ThrowValidationError("InterestReport.O.NoGlAccountPeriod"
            , A<int>.Ignored, A<bool>.Ignored)).Throws(new ApplicationException("InterestReport.O.NoGlAccountPeriod"));

            InterestReportLinesByDateCreationParams interestReportLinesByDateCreationParams = new InterestReportLinesByDateCreationParams(interestReportPM,
                interestTransactionPMs, gLAccountInterestPeriodPMs, interestBasesPeriodPMs,null);
            List<InterestReportLinesByDatePM> interestReportLinesByDatePMs = interestReportLinesByDateCreationService.CreateInterestReportLinesByDate(interestReportLinesByDateCreationParams);

            //AssertResults(resultInterestReportLinesByDatePMs, interestReportLinesByDatePMs);
            

        }

        private void AssertResults(List<InterestReportLinesByDatePM> expectedList, List<InterestReportLinesByDatePM> actualList)
        {
            Assert.AreEqual(expectedList.Count, actualList.Count);
            expectedList = expectedList.OrderBy(d => d.ToDate).ToList();
            actualList = actualList.OrderBy(d => d.ToDate).ToList();
            for(int i = 0; i < expectedList.Count; i++)
            {
                Assert.AreEqual(expectedList[i].AccumulatedAmount, actualList[i].AccumulatedAmount);
                Assert.AreEqual(expectedList[i].TotalAmount, actualList[i].TotalAmount);
                Assert.AreEqual(expectedList[i].StandardInterestPercentage, actualList[i].StandardInterestPercentage);
                Assert.AreEqual(expectedList[i].ExceptionalInterestPercentage, actualList[i].ExceptionalInterestPercentage);
                Assert.AreEqual(expectedList[i].CreditInterestPercentage, actualList[i].CreditInterestPercentage);
                Assert.AreEqual(expectedList[i].StandardInterestAmount, actualList[i].StandardInterestAmount);
                Assert.AreEqual(expectedList[i].ExceptionalInterestAmount, actualList[i].ExceptionalInterestAmount);
                Assert.AreEqual(expectedList[i].CreditInterestAmount, actualList[i].CreditInterestAmount);
                Assert.AreEqual(expectedList[i].CalculatedStandInterestAmount, actualList[i].CalculatedStandInterestAmount);
                Assert.AreEqual(expectedList[i].CalculatedExcepInterestAmount, actualList[i].CalculatedExcepInterestAmount);
                Assert.AreEqual(expectedList[i].CalculatedCreditInterestAmount, actualList[i].CalculatedCreditInterestAmount);
                Assert.AreEqual(expectedList[i].TotalInterestDays, actualList[i].TotalInterestDays);
                
            }
        }

        [TestCleanup]
        public void TestCleanup1()
        {
            JournalValidatorNotStatic.OverrideITextCodeTranslator = null;
        }
    }
}
