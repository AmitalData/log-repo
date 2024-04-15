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

        
        #region "Without Errors"




        [TestMethod]
        public void IsJournalValid0600_GoodJournalPM_ValidationResultIsNull()
        {

            var myJournalPM = new JournalPM()
            {
                AccountingDate= new DateTime(2016,3,1), //ClosedMonth = 1, OpenMonth = 9 
                StatusCode = "0",
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,

                     DebitAccountId="Card1-1", CreditAccountId="Card1-1",
                     AccountingDate= new DateTime(2016,3,1), //ClosedMonth = 1, OpenMonth = 9 
                     DueDate = new DateTime(2016,3,1), 
                     DocumentDate = new DateTime(2016,3,1), 

                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);

            Assert.IsNull(validationresult, "validationresult = null");
        }

        [TestMethod]
        public void IsJournalValid0601_GoodJournalPM_ValidationResultIsNull()
        {

            var myJournalPM = new JournalPM()
            {
                AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                StatusCode = "0",
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="4",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     AccountingDate = new DateTime(2016, 3, 1), 
                     DebitAccountId="Card1-1", CreditAccountId="Card1-1",

                     DueDate = new DateTime(2016,3,1), 
                     DocumentDate = new DateTime(2016,3,1), 
                     
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);

            Assert.IsNull(validationresult, "validationresult = null");
        }

        [Ignore]
        [TestMethod]
        public void IsJournalValid0602_ForeignAmountIsNotEqualAndBadRate_ErrContains_FAMltiExchangerateNELA()
        {
            var lineCode="if (div != currJournalLinePM.ForeignAmount)";
            
            var myJournalPM = new JournalPM()
            {
                AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                StatusCode = "6",
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="1",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=3,

                     //DebitAccountId="1-1"//, 
                     

                     AccountingDate = new DateTime(2016, 3, 1), 
                     DebitAccountId="Card1-1", CreditAccountId="Card1-1"
                     
                },
                new JournalLinePM(){ 
                    Line=2,
                    ActionTypeCode="2",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,

                     

                     AccountingDate = new DateTime(2016, 3, 1), 
                     DebitAccountId="Card1-1", CreditAccountId="Card1-1"
                     
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            
            Assert.IsNotNull(validationresult, lineCode);

            var errList = new List<String>(validationresult.MemberNames);
            var messageExist = errList.Exists(m => m.Contains(JournalValidator.M_FAMltiExchangerateNELA));

            Assert.IsTrue(messageExist, " Expected Have Line But Get Error Of " + lineCode);
        }
   

        [TestMethod]
        public void IsJournalValid0602p1_GoodJournalPMEvenForeignAmountIsNotEqual_ValidationResultIsNull()
        {

            var myJournalPM = new JournalPM()
            {
                AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                StatusCode = "6",
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="1",
                    LocalAmount=3 ,
                     ExchangeRate=3,
                     ForeignAmount=1,

                     //DebitAccountId="1-1"//, 
                     

                     AccountingDate = new DateTime(2016, 3, 1), 
                     DueDate = new DateTime(2016, 3, 1), 
                     DocumentDate = new DateTime(2016, 3, 1), 
                     DebitAccountId="Card1-1", CreditAccountId="Card1-1"
                     
                },
                new JournalLinePM(){ 
                    Line=2,
                    ActionTypeCode="2",
                    LocalAmount=3 ,
                     ExchangeRate=1,
                     ForeignAmount=3,

                     

                     AccountingDate = new DateTime(2016, 3, 1),
                     DueDate = new DateTime(2016, 3, 1), 
                     DocumentDate = new DateTime(2016, 3, 1), 
                     DebitAccountId="Card1-1", CreditAccountId="Card1-1"
                     
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            
            Assert.IsNull(validationresult, "validationresult = null");
        }

        [TestMethod]
        public void IsJournalValid0603_GoodJournalPMExchangeRate_ValidationResultIsNull()
        {

            var myJournalPM = new JournalPM()
            {
                AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                StatusCode = "0",
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=29536.27m ,
                     ExchangeRate=4.04697M,
                     ForeignAmount=7298.36m,

                     DebitAccountId="Card1-1", CreditAccountId="Card1-1",
                     AccountingDate= new DateTime(2016,3,1), //ClosedMonth = 1, OpenMonth = 9 
                     DueDate = new DateTime(2016,3,1), 
                     DocumentDate = new DateTime(2016,3,1), 

                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);

            Assert.IsNull(validationresult, "validationresult = null");
        }

        [TestMethod]
        public void IsJournalValid0604_GoodJournalPMExchangeRate_ValidationResultIsNull()
        {

            var myJournalPM = new JournalPM()
            {
                AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                StatusCode = "0",
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=29536.27m ,
                     ExchangeRate=null,
                     ForeignAmount=7298.36m,

                     DebitAccountId="Card1-1", CreditAccountId="Card1-1",
                     AccountingDate= new DateTime(2016,3,1), //ClosedMonth = 1, OpenMonth = 9 
                     DueDate = new DateTime(2016,3,1), 
                     DocumentDate = new DateTime(2016,3,1), 

                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);

            Assert.IsNull(validationresult, "validationresult = null");
        }
        #endregion

    }
}
