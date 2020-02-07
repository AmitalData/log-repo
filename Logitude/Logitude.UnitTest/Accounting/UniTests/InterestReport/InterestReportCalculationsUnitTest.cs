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
        public void PrivateCreateInterestReportLinesByDate_InputInterestTransactionsByDate_ResultInInterestReportLinesByDate()
        {
            InterestReportPM interestReportPM = new InterestReportPM()
            {
                OpenBalance = 200,
                CreateDateTime = new DateTime(2020, 2, 6),
                Tenant = 1,
                Id = "1-1",
                InterestCalculationDate = new DateTime(2020, 2, 1),
                GLAccountInterestCreditLimit=500,
            };

            GLAccountInterestPeriodPM glaccountInterestPeriodPM = new GLAccountInterestPeriodPM()
            {
                CreditInterestRateBaseId = "CreditBaseId",
                StandardInterestRateBaseId = "StandardBaseId",
                ExceptionalInterestRateBaseId = "ExceptionalBaseId",
            };

             var interestReportCalculationPreparations = A.Fake<InterestReportCalculationPreparations>(option => option.CallsBaseMethods());
            A.CallTo(() => interestReportCalculationPreparations.GetInterestReportPM(A<string>.Ignored, 1)).Returns(interestReportPM);
            A.CallTo(() => interestReportCalculationPreparations.GetGLAccountInterestPeriodPMWithinStartInterestDate(interestReportPM, A<DateTime>.Ignored)).Returns(glaccountInterestPeriodPM);
            A.CallTo(() => interestReportCalculationPreparations.CalculateInterestBasesTypePercentage(glaccountInterestPeriodPM.StandardInterestRateBaseId, A<DateTime>.Ignored, A<int>.Ignored)).Returns(3);
            A.CallTo(() => interestReportCalculationPreparations.CalculateInterestBasesTypePercentage(glaccountInterestPeriodPM.ExceptionalInterestRateBaseId, A<DateTime>.Ignored, A<int>.Ignored)).Returns(5);
            A.CallTo(() => interestReportCalculationPreparations.CalculateInterestBasesTypePercentage(glaccountInterestPeriodPM.CreditInterestRateBaseId, A<DateTime>.Ignored, A<int>.Ignored)).Returns(2);
            A.CallTo(() => interestReportCalculationPreparations.GetInterestTransactionsGroupedByDate(A<List<InterestTransactionPM>>.Ignored)).Returns(GetInterestTransactionsGroupedByDates());
            A.CallTo(() => interestReportCalculationPreparations.GetInterestTransactionsForGlAccountAndInterestValueDate(A<string>.Ignored, A<DateTime>.Ignored, 1)).Returns(null);

            InterestReportArgs interestReportArgs = new InterestReportArgs()
            {
                InterestReportId = "1-1",
                Tenant = 1,
                CalculationPreparations = interestReportCalculationPreparations,
            };

            InterestReportDataCalculations interestReportDataCalculations = new InterestReportDataCalculations(interestReportArgs);
            interestReportDataCalculations.GetInterestReportAndInterestTransactionsForCalculations();
            List<InterestReportLinesByDatePM> interestReportLinesByDatePMs = interestReportDataCalculations.CreateInterestReportLinesByDate();

            decimal lastAccumulatedAmount = interestReportLinesByDatePMs.LastOrDefault().AccumulatedAmount;
            Dictionary<string, decimal> expectedStandardInterestAmountsForGroups = new Dictionary<string, decimal>();
            expectedStandardInterestAmountsForGroups.Add("Group1", 400);
            expectedStandardInterestAmountsForGroups.Add("Group2", 100);
            expectedStandardInterestAmountsForGroups.Add("Group3", 500);
            expectedStandardInterestAmountsForGroups.Add("Group4", 0);

            Dictionary<string, decimal> expectedExceptionalInterestAmountsForGroups = new Dictionary<string, decimal>();
            expectedExceptionalInterestAmountsForGroups.Add("Group1", 0);
            expectedExceptionalInterestAmountsForGroups.Add("Group2", 0);
            expectedExceptionalInterestAmountsForGroups.Add("Group3", 500);
            expectedExceptionalInterestAmountsForGroups.Add("Group4", 0);

            Dictionary<string, decimal> expectedCreditInterestAmountsForGroups = new Dictionary<string, decimal>();
            expectedCreditInterestAmountsForGroups.Add("Group1", 0);
            expectedCreditInterestAmountsForGroups.Add("Group2", 0);
            expectedCreditInterestAmountsForGroups.Add("Group3", 0);
            expectedCreditInterestAmountsForGroups.Add("Group4", -100);

            Dictionary<string, decimal> expectedAccumulatedAmountsForGroups = new Dictionary<string, decimal>();
            expectedAccumulatedAmountsForGroups.Add("Group1", 400);
            expectedAccumulatedAmountsForGroups.Add("Group2", 100);
            expectedAccumulatedAmountsForGroups.Add("Group3", 1000);
            expectedAccumulatedAmountsForGroups.Add("Group4", -100);

            decimal expectedGroup1CalculatedStandardAmount = Convert.ToDecimal(400.0 * (3.0 / 365) * 9);
            expectedGroup1CalculatedStandardAmount = Math.Round(expectedGroup1CalculatedStandardAmount, 4);
            decimal expectedGroup2CalculatedStandardAmount = Convert.ToDecimal(100.0 * (3.0 / 365) * 12);
            expectedGroup2CalculatedStandardAmount = Math.Round(expectedGroup2CalculatedStandardAmount, 4);
            decimal expectedGroup3CalculatedStandardAmount = Convert.ToDecimal( 500.0 * (3.0 / 365) * 2);
            expectedGroup3CalculatedStandardAmount = Math.Round(expectedGroup3CalculatedStandardAmount, 4);
            decimal expectedGroup4CalculatedStandardAmount = 0;

            decimal expectedGroup1CalculatedExceptionalAmount = 0;
            decimal expectedGroup2CalculatedExceptionalAmount = 0;
            decimal expectedGroup3CalculatedExceptionalAmount = Convert.ToDecimal(500.0 * (5.0 / 365) * 2);
            expectedGroup3CalculatedExceptionalAmount = Math.Round(expectedGroup3CalculatedExceptionalAmount, 4);
            decimal expectedGroup4CalculatedExceptionalAmount = 0;

            decimal expectedGroup1CalculatedCreditAmount = 0;
            decimal expectedGroup2CalculatedCreditAmount = 0;
            decimal expectedGroup3CalculatedCreditAmount = 0;
            decimal expectedGroup4CalculatedCreditAmount = Convert.ToDecimal (- 100.0 * (2.0 / 365) * 8);
            expectedGroup4CalculatedCreditAmount = Math.Round(expectedGroup4CalculatedCreditAmount, 4);

            decimal expectedClosedBalance = -100;
            Assert.AreEqual(expectedStandardInterestAmountsForGroups["Group1"], interestReportLinesByDatePMs[0].StandardInterestAmount);
            Assert.AreEqual(expectedStandardInterestAmountsForGroups["Group2"], interestReportLinesByDatePMs[1].StandardInterestAmount);
            Assert.AreEqual(expectedStandardInterestAmountsForGroups["Group3"], interestReportLinesByDatePMs[2].StandardInterestAmount);
            Assert.AreEqual(expectedStandardInterestAmountsForGroups["Group4"], interestReportLinesByDatePMs[3].StandardInterestAmount);


            Assert.AreEqual(expectedExceptionalInterestAmountsForGroups["Group1"], interestReportLinesByDatePMs[0].ExceptionalInterestAmount);
            Assert.AreEqual(expectedExceptionalInterestAmountsForGroups["Group2"], interestReportLinesByDatePMs[1].ExceptionalInterestAmount);
            Assert.AreEqual(expectedExceptionalInterestAmountsForGroups["Group3"], interestReportLinesByDatePMs[2].ExceptionalInterestAmount);
            Assert.AreEqual(expectedExceptionalInterestAmountsForGroups["Group4"], interestReportLinesByDatePMs[3].ExceptionalInterestAmount);

            Assert.AreEqual(expectedCreditInterestAmountsForGroups["Group1"], interestReportLinesByDatePMs[0].CreditInterestAmount);
            Assert.AreEqual(expectedCreditInterestAmountsForGroups["Group2"], interestReportLinesByDatePMs[1].CreditInterestAmount);
            Assert.AreEqual(expectedCreditInterestAmountsForGroups["Group3"], interestReportLinesByDatePMs[2].CreditInterestAmount);
            Assert.AreEqual(expectedCreditInterestAmountsForGroups["Group4"], interestReportLinesByDatePMs[3].CreditInterestAmount);

            Assert.AreEqual(expectedAccumulatedAmountsForGroups["Group1"], interestReportLinesByDatePMs[0].AccumulatedAmount);
            Assert.AreEqual(expectedAccumulatedAmountsForGroups["Group2"], interestReportLinesByDatePMs[1].AccumulatedAmount);
            Assert.AreEqual(expectedAccumulatedAmountsForGroups["Group3"], interestReportLinesByDatePMs[2].AccumulatedAmount);
            Assert.AreEqual(expectedAccumulatedAmountsForGroups["Group4"], interestReportLinesByDatePMs[3].AccumulatedAmount);


            Assert.AreEqual(expectedGroup1CalculatedStandardAmount, interestReportLinesByDatePMs[0].CalculatedStandInterestAmount);
            Assert.AreEqual(expectedGroup2CalculatedStandardAmount, interestReportLinesByDatePMs[1].CalculatedStandInterestAmount);
            Assert.AreEqual(expectedGroup3CalculatedStandardAmount, interestReportLinesByDatePMs[2].CalculatedStandInterestAmount);
            Assert.AreEqual(expectedGroup4CalculatedStandardAmount, interestReportLinesByDatePMs[3].CalculatedStandInterestAmount);

            Assert.AreEqual(expectedGroup1CalculatedExceptionalAmount, interestReportLinesByDatePMs[0].CalculatedExcepInterestAmount);
            Assert.AreEqual(expectedGroup2CalculatedExceptionalAmount, interestReportLinesByDatePMs[1].CalculatedExcepInterestAmount);
            Assert.AreEqual(expectedGroup3CalculatedExceptionalAmount, interestReportLinesByDatePMs[2].CalculatedExcepInterestAmount);
            Assert.AreEqual(expectedGroup4CalculatedExceptionalAmount, interestReportLinesByDatePMs[3].CalculatedExcepInterestAmount);

            Assert.AreEqual(expectedGroup1CalculatedCreditAmount, interestReportLinesByDatePMs[0].CalculatedCreditInterestAmount);
            Assert.AreEqual(expectedGroup2CalculatedCreditAmount, interestReportLinesByDatePMs[1].CalculatedCreditInterestAmount);
            Assert.AreEqual(expectedGroup3CalculatedCreditAmount, interestReportLinesByDatePMs[2].CalculatedCreditInterestAmount);
            Assert.AreEqual(expectedGroup4CalculatedCreditAmount, interestReportLinesByDatePMs[3].CalculatedCreditInterestAmount);

            Assert.AreEqual(expectedClosedBalance, interestReportLinesByDatePMs[3].AccumulatedAmount);

        }


        private List<InterestTransactionsGroupedByDate> GetInterestTransactionsGroupedByDates()
        {
            List<InterestTransactionsGroupedByDate> interestTransactionsGroupedByDates = new List<InterestTransactionsGroupedByDate>();
            interestTransactionsGroupedByDates.Add(new InterestTransactionsGroupedByDate() { GroupInterestValueDate = new DateTime(2020, 1, 1), TotalLocalAmount = 200 });
            interestTransactionsGroupedByDates.Add(new InterestTransactionsGroupedByDate() { GroupInterestValueDate = new DateTime(2020, 1, 10), TotalLocalAmount = -300 });
            interestTransactionsGroupedByDates.Add(new InterestTransactionsGroupedByDate() { GroupInterestValueDate = new DateTime(2020, 1, 22), TotalLocalAmount = 900 });
            interestTransactionsGroupedByDates.Add(new InterestTransactionsGroupedByDate() { GroupInterestValueDate = new DateTime(2020, 1, 24), TotalLocalAmount = -1100 });

            return interestTransactionsGroupedByDates;
        }



    }
}
