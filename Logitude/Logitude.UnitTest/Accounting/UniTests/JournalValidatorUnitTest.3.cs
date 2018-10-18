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
        #region M_ButAccountCurrencyisDifferent

        [TestMethod]
        public void IsJournalValid0300_ActionTypeCode1CurrencyIdEURCreditAccountIdACCurUSD_errListCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="1",
                    CurrencyId="EUR",
                    CreditAccountId     ="ACCurUSD",
                    
                }
                }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ButAccountCurrencyisDifferent));
            var lineCode = "if (!String.IsNullOrWhiteSpace(pmAcc.CurrencyId) && jlCurrencyId != pmAcc.CurrencyId)";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        [TestMethod]
        public void IsJournalValid0301_ActionTypeCode1CurrencyIdUSDCreditAccountIdACCurUSD_errListNotCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="1",
                    CurrencyId="USD",
                    CreditAccountId     ="ACCurUSD",
                    
                }
                }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ButAccountCurrencyisDifferent));
            var lineCode = "if (!String.IsNullOrWhiteSpace(pmAcc.CurrencyId) && jlCurrencyId != pmAcc.CurrencyId)";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }






        [TestMethod]
        public void IsJournalValid0302_ActionTypeCode2CurrencyIdEURDebitAccountIdACCurUSD_errListCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="2",
                    CurrencyId="EUR",
                    DebitAccountId     ="ACCurUSD",
                    
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ButAccountCurrencyisDifferent));
            var lineCode = "if (!String.IsNullOrWhiteSpace(pmAcc.CurrencyId) && jlCurrencyId != pmAcc.CurrencyId)";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        [TestMethod]
        public void IsJournalValid0303_ActionTypeCode2CurrencyIdUSDDebitAccountIdACCurUSD_errListNotCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="2",
                    CurrencyId="USD",
                    DebitAccountId     ="ACCurUSD",
                    
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ButAccountCurrencyisDifferent));
            var lineCode = "if (!String.IsNullOrWhiteSpace(pmAcc.CurrencyId) && jlCurrencyId != pmAcc.CurrencyId)";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }




        [TestMethod]
        public void IsJournalValid0302_ActionTypeCode3CurrencyIdEURDebitAccountIdACCurUSD_errListCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="3",
                    CurrencyId="EUR",
                    DebitAccountId     ="ACCurUSD",
                    
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ButAccountCurrencyisDifferent));
            var lineCode = "if (!String.IsNullOrWhiteSpace(pmAcc.CurrencyId) && jlCurrencyId != pmAcc.CurrencyId)";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        [TestMethod]
        public void IsJournalValid0303_ActionTypeCode3CurrencyIdUSDDebitAccountIdACCurUSD_errListNotCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="3",
                    CurrencyId="USD",
                    DebitAccountId     ="ACCurUSD",
                    
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ButAccountCurrencyisDifferent));
            var lineCode = "if (!String.IsNullOrWhiteSpace(pmAcc.CurrencyId) && jlCurrencyId != pmAcc.CurrencyId)";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }





        [TestMethod]
        public void IsJournalValid0304_ActionTypeCode3CurrencyIdEURCreditAccountIdACCurUSD_errListCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="3",
                    CurrencyId="EUR",
                    CreditAccountId     ="ACCurUSD",
                    
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ButAccountCurrencyisDifferent));
            var lineCode = "if (!String.IsNullOrWhiteSpace(pmAcc.CurrencyId) && jlCurrencyId != pmAcc.CurrencyId)";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        [TestMethod]
        public void IsJournalValid0305_ActionTypeCode3CurrencyIdUSDCreditAccountIdACCurUSD_errListNotCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="3",
                    CurrencyId="USD",
                    CreditAccountId     ="ACCurUSD",
                    
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ButAccountCurrencyisDifferent));
            var lineCode = "if (!String.IsNullOrWhiteSpace(pmAcc.CurrencyId) && jlCurrencyId != pmAcc.CurrencyId)";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }







        #region MyRegion4



        [TestMethod]
        public void IsJournalValid0306_ActionTypeCode4CurrencyIdEURDebitAccountIdACCurUSD_errListCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="4",
                    CurrencyId="EUR",
                    DebitAccountId     ="ACCurUSD",
                    
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ButAccountCurrencyisDifferent));
            var lineCode = "if (!String.IsNullOrWhiteSpace(pmAcc.CurrencyId) && jlCurrencyId != pmAcc.CurrencyId)";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        [TestMethod]
        public void IsJournalValid0307_ActionTypeCode4CurrencyIdUSDDebitAccountIdACCurUSD_errListNotCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="4",
                    CurrencyId="USD",
                    DebitAccountId     ="ACCurUSD",
                    
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ButAccountCurrencyisDifferent));
            var lineCode = "if (!String.IsNullOrWhiteSpace(pmAcc.CurrencyId) && jlCurrencyId != pmAcc.CurrencyId)";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }





        [TestMethod]
        public void IsJournalValid0308_ActionTypeCode4CurrencyIdEURCreditAccountIdACCurUSD_errListCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="4",
                    CurrencyId="EUR",
                    CreditAccountId     ="ACCurUSD",
                    
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ButAccountCurrencyisDifferent));
            var lineCode = "if (!String.IsNullOrWhiteSpace(pmAcc.CurrencyId) && jlCurrencyId != pmAcc.CurrencyId)";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        [TestMethod]
        public void IsJournalValid0309_ActionTypeCode4CurrencyIdUSDCreditAccountIdACCurUSD_errListNotCotainsM_ButAccountCurrencyisDifferent()
        {

            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="4",
                    CurrencyId="USD",
                    CreditAccountId     ="ACCurUSD",
                    
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ButAccountCurrencyisDifferent));
            var lineCode = "if (!String.IsNullOrWhiteSpace(pmAcc.CurrencyId) && jlCurrencyId != pmAcc.CurrencyId)";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }

        #endregion    
        #endregion
    }
}
