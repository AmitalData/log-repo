using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Def.EntityPMs;
using System.Collections.Generic;
//using Logitude.Server.Tools.Helpers.Fakes;
using Logitude.Accounting.BL.CoreBL;
//using Logitude.Accounting.Def.EntityPMs.Fakes;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.Diagnostics;
using Logitude.Accounting.BL.Validators;


namespace Logitude.UnitTest.Accounting.UniTests
{
    public partial class JournalValidatorUnitTest
    {


        #region "M_ButAccountCurrencyisDifferent Approved"

        [TestMethod]
        public void IsJournalValidValueRange_StatusApprovedDebitTotalNotEqual2CreditTotal_ValidationResultContainsM_JournalAmountNotMatched()
        {
            var listGenerateDataForThisMethod = new List<Tuple<int, string>>() { 
                new Tuple<int, string>(1, ((int)MyJournalActionTypeEnum.Credit).ToString()) ,
                new Tuple<int, string>(-1, ((int)MyJournalActionTypeEnum.Credit).ToString()) ,
                new Tuple<int, string>(10, ((int)MyJournalActionTypeEnum.Credit).ToString()) ,
                new Tuple<int, string>(-10, ((int)MyJournalActionTypeEnum.Credit).ToString()) ,
                new Tuple<int, string>(1099, ((int)MyJournalActionTypeEnum.Credit).ToString()) ,
                new Tuple<int, string>(-1099, ((int)MyJournalActionTypeEnum.Credit).ToString()) ,


                new Tuple<int, string>(1, ((int)MyJournalActionTypeEnum.Debit).ToString()) ,
                new Tuple<int, string>(-1, ((int)MyJournalActionTypeEnum.Debit).ToString()) ,
                new Tuple<int, string>(10, ((int)MyJournalActionTypeEnum.Debit).ToString()) ,
                new Tuple<int, string>(-10, ((int)MyJournalActionTypeEnum.Debit).ToString()) ,
                new Tuple<int, string>(1099, ((int)MyJournalActionTypeEnum.Debit).ToString()) ,
                new Tuple<int, string>(-1099, ((int)MyJournalActionTypeEnum.Debit).ToString()) ,
            };
            foreach (var item in listGenerateDataForThisMethod)
            {
                IsJournalValid_StatusApprovedDebitTotalNotEqualCreditTotal_ValidationResultContainsM_JournalAmountNotMatched(item.Item2, item.Item1);
            }
        }

        // Parameterized test method:
        public void IsJournalValid_StatusApprovedDebitTotalNotEqualCreditTotal_ValidationResultContainsM_JournalAmountNotMatched
            (string actionTypeCode, int localAmount)
        {
            // Arrange - Set up the initial state:
            var myJournalPM = new JournalPM()
            {
                StatusCode = StatusCode_Approved,
                JournalLines = new List<JournalLinePM>() { new JournalLinePM(){    ActionTypeCode=actionTypeCode,LocalAmount=localAmount}}
            };

            // Act - Exercise the method under test:

            var validationresult = JournalValidator.IsJournalValid(myJournalPM, GetValidationContext(myJournalPM));

            

            // Assert - Verify the outcome:
            Assert.IsNotNull(validationresult);
            var errList = new List<String>(validationresult.MemberNames);
            var messageExist = errList.Exists(m => m.Contains(JournalValidator.M_JournalAmountNotMatched));
            var lineCode = "if (debitTotal != creditTotal)";
            Assert.IsTrue(messageExist, "Expected Have Line But Get Error Of " + lineCode);
            
        }

       


        [TestMethod]
        public void IsJournalValid0501_ApprovedLineAction1LocalAmount0_errListNotCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {
                StatusCode = StatusCode_Approved,

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="1",
                     LocalAmount=0
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_JournalAmountNotMatched));
            var lineCode = "if (debitTotal != creditTotal)";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }


        [TestMethod]
        public void IsJournalValid0502_ApprovedLineAction2LocalAmount1_errListCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {
                StatusCode = StatusCode_Approved,
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="2",
                     LocalAmount=1
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_JournalAmountNotMatched));
            var lineCode = "if (debitTotal != creditTotal)";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }


        [TestMethod]
        public void IsJournalValid0503_ApprovedLineAction2LocalAmount0_errListCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {
                StatusCode = StatusCode_Approved,
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="2",
                     LocalAmount=0
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_JournalAmountNotMatched));
            var lineCode = "if (debitTotal != creditTotal)";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }


        [TestMethod]
        public void IsJournalValid0504_ApprovedLineAction3LocalAmount1_errListNotCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {
                StatusCode = StatusCode_Approved,
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="3",
                     LocalAmount=1
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_JournalAmountNotMatched));
            var lineCode = "if (debitTotal != creditTotal)";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }


        [TestMethod]
        public void IsJournalValid0505_ApprovedLineAction4LocalAmount1_errListNotCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {
                StatusCode = StatusCode_Approved,

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="4",
                     LocalAmount=1
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_JournalAmountNotMatched));
            var lineCode = "if (debitTotal != creditTotal)";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }


        [TestMethod]
        public void IsJournalValid0506_Approved2LineAction3LocalAmount1_errListNotCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {
                StatusCode = StatusCode_Approved,
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="3",
                     LocalAmount=1
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_JournalAmountNotMatched));
            var lineCode = "if (debitTotal != creditTotal)";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }

        [TestMethod]
        public void IsJournalValid0507_Approved2LineAction4LocalAmount1_errListNotCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {
                StatusCode = StatusCode_Approved,
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="4",
                     LocalAmount=1
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_JournalAmountNotMatched));
            var lineCode = "if (debitTotal != creditTotal)";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }



        [TestMethod]
        public void IsJournalValid0508_Approved2LineAction1and2NotZero_errListCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {
                StatusCode = StatusCode_Approved,
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="1",
                     LocalAmount=1
                },
                new JournalLinePM(){  
                    ActionTypeCode="2",
                     LocalAmount=3
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_JournalAmountNotMatched));
            var lineCode = "if (debitTotal != creditTotal)";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }


        [TestMethod]
        public void IsJournalValid0509_Approved2LineAction1and2Zero_errListNotCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {
                StatusCode = StatusCode_Approved,
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="1",
                     LocalAmount=1
                },
                new JournalLinePM(){  
                    ActionTypeCode="2",
                     LocalAmount=1
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_JournalAmountNotMatched));
            var lineCode = "if (debitTotal != creditTotal)";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }





        #endregion

    }
}
