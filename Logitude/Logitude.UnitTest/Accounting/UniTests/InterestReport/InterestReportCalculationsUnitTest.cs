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
            //InterestReportPM interestReportPM = new InterestReportPM()
            //{
            //    OpenBalance = 200,
            //    CreateDateTime = new DateTime(2020, 2, 6),
            //    Tenant = 1,
            //    Id = "1-1",
            //    InterestCalculationDate = new DateTime(2020, 2, 1),
            //    GLAccountInterestCreditLimit=500,
            //};
            IInterestReportCalculationPreparations interestReportCalculationPreparations = new InterestReportCalculationFromCSVPreparations(@"Accounting\UniTests\InterestReport\");
            InterestReportTestResultGetter interestReportTestResultGetter = new InterestReportTestResultGetter();
            InterestReportPM interestReportPM = interestReportCalculationPreparations.GetInterestReportPM("", 1);
            List<InterestTransactionPM> interestTransactionPMs = interestReportCalculationPreparations.GetInterestTransactionsForGlAccountAndInterestValueDate("", DateTime.Now, 1);
            List<GLAccountInterestPeriodPM> gLAccountInterestPeriodPMs = interestReportCalculationPreparations.GetGlaccountInterestPeriods(interestReportPM);
            List<InterestBasesPeriodPM> interestBasesPeriodPMs = interestReportCalculationPreparations.GetAllInterestBasesPeriodPMs(1);
            List<InterestReportLinesByDatePM> resultInterestReportLinesByDatePMs = interestReportTestResultGetter.GetInterestReportLinesByDatePMsFromCSV(@"Accounting\UniTests\InterestReport\");

            InterestReportLinesByDateCreationService interestReportLinesByDateCreationService = new InterestReportLinesByDateCreationService();
            InterestReportLinesByDateCreationParams interestReportLinesByDateCreationParams = new InterestReportLinesByDateCreationParams(interestReportPM, 
                interestTransactionPMs, gLAccountInterestPeriodPMs, interestBasesPeriodPMs);
            List<InterestReportLinesByDatePM> interestReportLinesByDatePMs = interestReportLinesByDateCreationService.CreateInterestReportLinesByDate(interestReportLinesByDateCreationParams);

            AssertResults(resultInterestReportLinesByDatePMs, interestReportLinesByDatePMs);


        }

        private void AssertResults(List<InterestReportLinesByDatePM> expectedList, List<InterestReportLinesByDatePM> actualList)
        {
            Assert.AreEqual(expectedList.Count, actualList.Count);
            expectedList = expectedList.OrderByDescending(d => d.ToDate).ToList();
            for(int i = 0; i < expectedList.Count; i++)
            {

            }
        }
    }
}
