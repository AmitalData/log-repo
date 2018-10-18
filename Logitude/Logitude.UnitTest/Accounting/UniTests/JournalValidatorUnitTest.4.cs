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

        const string StatusCode_WaitingforApprove = "1";
        const string StatusCode_Approved = "2";

        #region "M_ButAccountCurrencyisDifferent WaitingforApprove"

        [TestMethod]
        public void IsJournalValid0400_WaitingforApproveLineAction1LocalAmount1_errListCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {
                StatusCode = StatusCode_WaitingforApprove,
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="1",
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
        public void IsJournalValid0401_WaitingforApproveLineAction1LocalAmount0_errListNotCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {
                StatusCode = StatusCode_WaitingforApprove,

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
        public void IsJournalValid0402_WaitingforApproveLineAction2LocalAmount1_errListCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {
                StatusCode = StatusCode_WaitingforApprove,
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
        public void IsJournalValid0403_WaitingforApproveLineAction2LocalAmount0_errListCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {
                StatusCode = StatusCode_WaitingforApprove,
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
        public void IsJournalValid0404_WaitingforApproveLineAction3LocalAmount1_errListNotCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {
                StatusCode = StatusCode_WaitingforApprove,
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
        public void IsJournalValid0405_WaitingforApproveLineAction4LocalAmount1_errListNotCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {
                StatusCode = StatusCode_WaitingforApprove,

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
        public void IsJournalValid0406_WaitingforApprove2LineAction3LocalAmount1_errListNotCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {
                StatusCode = StatusCode_WaitingforApprove,
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
        public void IsJournalValid0407_WaitingforApprove2LineAction4LocalAmount1_errListNotCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {
                StatusCode = StatusCode_WaitingforApprove,
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
        public void IsJournalValid0408_WaitingforApprove2LineAction1and2NotZero_errListCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {
                StatusCode = StatusCode_WaitingforApprove,
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
        public void IsJournalValid0409_WaitingforApprove2LineAction1and2Zero_errListNotCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {
                StatusCode = StatusCode_WaitingforApprove,
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

        [TestMethod]
        public void IsJournalValid0408_NotWaitingforApprove2LineAction1and2NotZero_errListNotCotainsM_ButAccountCurrencyisDifferent()
        {

            Trace.WriteLine("IsJournalValid0408_NotWaitingforApprove2LineAction1and2NotZero_errListNotCotainsM_ButAccountCurrencyisDifferent(): due StatusCode = 0 (not 1 or 2) r=there is no check");
            var myJournalPM = new JournalPM()
            {
                StatusCode = "0",
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
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
    }
}
