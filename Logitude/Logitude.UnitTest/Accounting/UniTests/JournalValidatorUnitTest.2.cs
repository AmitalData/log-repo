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
using Logitude.Accounting.BL.Validators;


namespace Logitude.UnitTest.Accounting.UniTests
{
    public partial class JournalValidatorUnitTest
    {
        #region M_GLAccountIsControl

        [TestMethod]
        public void IsJournalValid0200_ActionTypeCode2DebitAccountIdACControl_errListCotainsM_GLAccountIsControl()
        {

            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="2",
                    DebitAccountId     ="ACControl"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_GLAccountIsControl));
            var lineCode = "if (pmAcc.IsControlAccount.GetValueOrDefault())";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        [TestMethod]
        public void IsJournalValid0201_ActionTypeCode2DebitAccountId1m1_errListNotCotainsM_GLAccountIsControl()
        {

            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="2",
                    DebitAccountId     ="1-1"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_GLAccountIsControl));
            var lineCode = "if (pmAcc.IsControlAccount.GetValueOrDefault())";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }




        [TestMethod]
        public void IsJournalValid0202_ActionTypeCode1CreditAccountIdACControl_errListCotainsM_GLAccountIsControl()
        {

            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="1",
                    CreditAccountId     ="ACControl"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_GLAccountIsControl));
            var lineCode = "if (pmAcc.IsControlAccount.GetValueOrDefault())";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        [TestMethod]
        public void IsJournalValid0203_ActionTypeCode1CreditAccountId1m1_errListNotCotainsM_GLAccountIsControl()
        {

            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="2",
                    CreditAccountId     ="1-1"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_GLAccountIsControl));
            var lineCode = "if (pmAcc.IsControlAccount.GetValueOrDefault())";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }






        [TestMethod]
        public void IsJournalValid0204_ActionTypeCode3CreditAccountIdACControl_errListCotainsM_GLAccountIsControl()
        {
       
            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="3",
                    CreditAccountId     ="ACControl"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_GLAccountIsControl));
            var lineCode = "if (pmAcc.IsControlAccount.GetValueOrDefault())";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        [TestMethod]
        public void IsJournalValid0205_ActionTypeCode3CreditAccountId1m1_errListNotCotainsM_GLAccountIsControl()
        {
            
            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="3",
                    CreditAccountId     ="1-1"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_GLAccountIsControl));
            var lineCode = "if (pmAcc.IsControlAccount.GetValueOrDefault())";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }



        [TestMethod]
        public void IsJournalValid0206_ActionTypeCode4CreditAccountIdACControl_errListCotainsM_GLAccountIsControl()
        {
            
            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="4",
                    CreditAccountId     ="ACControl"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_GLAccountIsControl));
            var lineCode = "if (pmAcc.IsControlAccount.GetValueOrDefault())";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        [TestMethod]
        public void IsJournalValid0207_ActionTypeCode4CreditAccountId1m1_errListNotCotainsM_GLAccountIsControl()
        {
            
            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="4",
                    CreditAccountId     ="1-1"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_GLAccountIsControl));
            var lineCode = "if (pmAcc.IsControlAccount.GetValueOrDefault())";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }


        [TestMethod]
        public void IsJournalValid0208_ActionTypeCode4DebitAccountIdACControl_errListCotainsM_GLAccountIsControl()
        {
            
            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="4",
                    DebitAccountId     ="ACControl"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_GLAccountIsControl));
            var lineCode = "if (pmAcc.IsControlAccount.GetValueOrDefault())";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        [TestMethod]
        public void IsJournalValid0209_ActionTypeCode4DebitAccountId1m1_errListNotCotainsM_GLAccountIsControl()
        {
            
            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="4",
                    DebitAccountId     ="1-1"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_GLAccountIsControl));
            var lineCode = "if (pmAcc.IsControlAccount.GetValueOrDefault())";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        #endregion
    }
}
