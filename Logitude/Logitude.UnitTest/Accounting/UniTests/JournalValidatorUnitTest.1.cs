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
        #region pmAcc.Inactive.GetValueOrDefault

        [TestMethod]
        public void IsJournalValid0100_ActionTypeCode2DebitAccountIdACBLOCK_errListCotainsM_BlockedGLAccount()
        {

            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="2",
                    DebitAccountId     ="ACBLOCK"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_BlockedGLAccount));
            var lineCode = "if (pmAcc.IsControlAccount.GetValueOrDefault())";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        [TestMethod]
        public void IsJournalValid0101_ActionTypeCode2DebitAccountId1m1_errListNotCotainsM_BlockedGLAccount()
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


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_BlockedGLAccount));
            var lineCode = "if (pmAcc.IsControlAccount.GetValueOrDefault())";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }




        [TestMethod]
        public void IsJournalValid0102_ActionTypeCode1CreditAccountIdACBLOCK_errListCotainsM_BlockedGLAccount()
        {

            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="1",
                    CreditAccountId     ="ACBLOCK"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_BlockedGLAccount));
            var lineCode = "if (pmAcc.IsControlAccount.GetValueOrDefault())";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        [TestMethod]
        public void IsJournalValid0103_ActionTypeCode1CreditAccountId1m1_errListNotCotainsM_BlockedGLAccount()
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


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_BlockedGLAccount));
            var lineCode = "if (pmAcc.IsControlAccount.GetValueOrDefault())";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }






        [TestMethod]
        public void IsJournalValid0104_ActionTypeCode3CreditAccountIdACBLOCK_errListCotainsM_BlockedGLAccount()
        {
            var documentDate = new DateTime(2016, 1, 1).AddSeconds(1);
            var dueDate = new DateTime(2016, 1, 1);
            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="3",
                    CreditAccountId     ="ACBLOCK"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_BlockedGLAccount));
            var lineCode = "if (pmAcc.IsControlAccount.GetValueOrDefault())";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        [TestMethod]
        public void IsJournalValid0105_ActionTypeCode3CreditAccountId1m1_errListNotCotainsM_BlockedGLAccount()
        {
            var documentDate = new DateTime(2016, 1, 1).AddSeconds(1);
            var dueDate = new DateTime(2016, 1, 1);
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


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_BlockedGLAccount));
            var lineCode = "if (pmAcc.IsControlAccount.GetValueOrDefault())";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }



        [TestMethod]
        public void IsJournalValid0106_ActionTypeCode4CreditAccountIdACBLOCK_errListCotainsM_BlockedGLAccount()
        {
            var documentDate = new DateTime(2016, 1, 1).AddSeconds(1);
            var dueDate = new DateTime(2016, 1, 1);
            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="4",
                    CreditAccountId     ="ACBLOCK"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_BlockedGLAccount));
            var lineCode = "if (pmAcc.IsControlAccount.GetValueOrDefault())";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        [TestMethod]
        public void IsJournalValid0107_ActionTypeCode4CreditAccountId1m1_errListNotCotainsM_BlockedGLAccount()
        {
            var documentDate = new DateTime(2016, 1, 1).AddSeconds(1);
            var dueDate = new DateTime(2016, 1, 1);
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


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_BlockedGLAccount));
            var lineCode = "if (pmAcc.IsControlAccount.GetValueOrDefault())";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }


        [TestMethod]
        public void IsJournalValid0108_ActionTypeCode4DebitAccountIdACBLOCK_errListCotainsM_BlockedGLAccount()
        {
            var documentDate = new DateTime(2016, 1, 1).AddSeconds(1);
            var dueDate = new DateTime(2016, 1, 1);
            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="4",
                    DebitAccountId     ="ACBLOCK"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_BlockedGLAccount));
            var lineCode = "if (pmAcc.IsControlAccount.GetValueOrDefault())";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        [TestMethod]
        public void IsJournalValid0109_ActionTypeCode4DebitAccountId1m1_errListNotCotainsM_BlockedGLAccount()
        {
            var documentDate = new DateTime(2016, 1, 1).AddSeconds(1);
            var dueDate = new DateTime(2016, 1, 1);
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


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_BlockedGLAccount));
            var lineCode = "if (pmAcc.IsControlAccount.GetValueOrDefault())";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        #endregion
    }
}
