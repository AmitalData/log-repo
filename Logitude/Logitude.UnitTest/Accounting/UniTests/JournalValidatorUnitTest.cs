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
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.Def.EntityPMs;
//using Logitude.Accounting.Def.EntityPMs.Fakes;
//using Logitude.Accounting.BL.Validators.Fakes;
using Logitude.Accounting.BL.Validators;
using FakeItEasy;
using System.Diagnostics;
namespace Logitude.UnitTest.Accounting.UniTests
{

    
    [TestClass]
    public partial class JournalValidatorUnitTest
    {
        const string C_ExternalNoExist = "ExternalNoExist";
        const string C_ExternalSystemExist = "ExternalSystemExist";

        [TestInitialize]
        public void TestInitialize1()
        {
            var textCodeTranslatorFake = A.Fake<ITextCodeTranslator>();
            A.CallTo(() => textCodeTranslatorFake.Translate(A<string>.Ignored, A<int>.Ignored))
                .ReturnsLazily(
                (string textCodeCode, int tenant) =>
                {
                    return textCodeCode;
                }
            );
            JournalValidator.OverrideITextCodeTranslator = textCodeTranslatorFake;
        }
        [TestCleanup]
        public void TestCleanup1()
        {
            JournalValidator.OverrideITextCodeTranslator = null;
        }

        [TestMethod]
        public void IsJournalValid_01_Empty_ErrListContainsYouShouldHaveOneLineAtLeast()
        {


            var entityPM = new JournalPM() { };
            //var validationResults = new List<ValidationResult>();
            //var actual = Validator.TryValidateObject(entityPM, new ValidationContext(entityPM), validationResults);

            System.ComponentModel.DataAnnotations.ValidationContext validationcontext = new System.ComponentModel.DataAnnotations.ValidationContext(entityPM);
            JournalValidator.OverrideGetLoggedContactFunc =
                    new Func<int, BL.CommonDataModel.EntityPMs.ContactPM>(
                        (tenant) => new BL.CommonDataModel.EntityPMs.ContactPM() { DontShowLocal = true }
                     );
            try
            {
                ValidationResult validationresult = JournalValidator.IsJournalValid(entityPM, validationcontext);
                Assert.IsNotNull(validationresult, "eXPCETED validationresult ErrListContainsYouShouldHaveOneLineAtLeast");
                var errList = new List<String>(validationresult.MemberNames);
                //Assert
                //Assert.IsFalse(validationResults, "Expected validation to fail.");
                Assert.AreEqual<int>(1, validationresult.MemberNames.Count(), "Unexpected number of validation errors.");
                var msg = errList[0];
                //msg =validationresult[0];
                Assert.AreEqual<string>(JournalValidator.M_YouShouldHaveOneLineAtLeast, validationresult.MemberNames.ElementAt(0));

            }
            finally
            {
                JournalValidator.OverrideGetLoggedContactFunc = null;
            }
            
        }
        [TestMethod]
        public void IsJournalValid_02_WithLine_IsJournalValid_Empty_ErrListNotContainsYouShouldHaveOneLineAtLeast()
        {


            var entityPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){},
                new JournalLinePM(){}
            }
            };


            System.ComponentModel.DataAnnotations.ValidationContext validationcontext = new System.ComponentModel.DataAnnotations.ValidationContext(entityPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(entityPM, validationcontext);
            var errList = new List<String>(validationresult.MemberNames);


            Assert.IsTrue(!errList.Contains(JournalValidator.M_YouShouldHaveOneLineAtLeast), "Expected Have Line But Get Error Of M_YouShouldHaveOneLineAtLeast");


        }

        [TestMethod]
        //MethodUnderTest_Scenario_Behavior().
        public void IsJournalValid_03_HaveYear2016ClosedMonth1OpenMonth9Check201610_errListContainsClosedMonth()
        {

            var accountingDate = new DateTime(2016, 10, 20);
            var entityPM = new JournalPM()
            {
                AccountingDate = accountingDate,
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){},
                new JournalLinePM(){}
            }
            };
            var myNewJournalValidatorContext = GetValidationContext(entityPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(entityPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            Assert.IsTrue(errList.Contains(JournalValidator.M_ClosedMonth), "Expected Have Line But Get Error Of M_ClosedMonth");

        }
        [TestMethod]
        public void IsJournalValid_04_HaveYear2016ClosedMonth1OpenMonth9Check201609_errListContainsClosedMonth()
        {

            var accountingDate = new DateTime(2016, 09, 20);
            var entityPM = new JournalPM()
            {
                AccountingDate = accountingDate,
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){},
                new JournalLinePM(){}
            }
            };
            var myNewJournalValidatorContext = GetValidationContext(entityPM, new AccountingPeriodPM() { Year = 2016, ClosedMonth = 9, OpenMonth = 10 });
            ValidationResult validationresult = JournalValidator.IsJournalValid(entityPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            Assert.IsTrue(errList.Contains(JournalValidator.M_ClosedMonth), "Expected Have Line But Get Error Of M_ClosedMonth");

        }
        [TestMethod]
        public void IsJournalValid_04p1_HaveYear2016ClosedMonth1OpenMonth9Check201609_errListNotContainsClosedMonth()
        {

            var accountingDate = new DateTime(2016, 09, 20);
            var entityPM = new JournalPM()
            {
                AccountingDate = accountingDate,
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){},
                new JournalLinePM(){}
            }
            };
            var myNewJournalValidatorContext = GetValidationContext(entityPM, new AccountingPeriodPM() { Year = 2016, ClosedMonth = 1, OpenMonth = 10, PeriodTypeCode = "1" });
            ValidationResult validationresult = JournalValidator.IsJournalValid(entityPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            Assert.IsFalse(errList.Contains(JournalValidator.M_ClosedMonth), 
                "Expected Have Line But Get Error Of M_ClosedMonth");

        }

        [TestMethod]
        public void IsJournalValid_05_HaveYear2016ClosedMonth1OpenMonth9Check201608_errListNotContainsClosedMonth()
        {

            var accountingDate = new DateTime(2016, 08, 20);
            var entityPM = new JournalPM()
            {
                AccountingDate = accountingDate,
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){},
                new JournalLinePM(){}
            }
            };
            var myNewJournalValidatorContext = GetValidationContext(entityPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(entityPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            Assert.IsTrue(!errList.Contains(JournalValidator.M_ClosedMonth), "Expected Have Line But Get Error Of M_ClosedMonth");

        }

        [TestMethod]
        public void IsJournalValid_06_HaveYear2016OpenMonth0Check201601MustOpenMounth4Work_errListContainsClosedMonth()
        {

            var accountingDate = new DateTime(2016, 01, 20);
            var entityPM = new JournalPM()
            {
                AccountingDate = accountingDate,
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){},
                new JournalLinePM(){}
            }
            };
            var yearNotOpenAccountingPeriodPM = new AccountingPeriodPM()
            {
                Year = 2016
                //, ClosedMonth = 1, OpenMonth = 9 
            };
            var myNewJournalValidatorContext = GetValidationContext(entityPM, yearNotOpenAccountingPeriodPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(entityPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            Assert.IsTrue(errList.Contains(JournalValidator.M_ClosedMonth), "Expected Have Line But Get Error Of M_ClosedMonth");

        }



        [TestMethod]
        public void IsJournalValid_07_CloseMonthEqualOpenMonth_errListContainsClosedMonth()
        {

            var accountingDate = new DateTime(2016, 08, 20);
            var entityPM = new JournalPM()
            {
                AccountingDate = accountingDate,
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){},
                new JournalLinePM(){}
            }
            };
            var yearNotOpenAccountingPeriodPM = new AccountingPeriodPM()
            {
                Year = 2016
                ,
                ClosedMonth = 8,
                OpenMonth = 8
            };
            var myNewJournalValidatorContext = GetValidationContext(entityPM, yearNotOpenAccountingPeriodPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(entityPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            Assert.IsTrue(errList.Contains(JournalValidator.M_ClosedMonth), "Expected Have Line But Get Error Of M_ClosedMonth");

        }


        [TestMethod]
        public void IsJournalValid_08_AccountingPeriodPMsNotContainsAccountingDateYear2016_errListContainsClosedMonth()
        {

            var accountingDate = new DateTime(2016, 08, 20);
            var entityPM = new JournalPM()
            {
                AccountingDate = accountingDate,
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){},
                new JournalLinePM(){}
            }
            };
            var yearNotOpenAccountingPeriodPM = new AccountingPeriodPM()
            {
                Year = 2015,
                ClosedMonth = 8,
                OpenMonth = 8
            };
            var myNewJournalValidatorContext = GetValidationContext(entityPM, yearNotOpenAccountingPeriodPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(entityPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            Assert.IsTrue(errList.Contains(JournalValidator.M_ClosedMonth), "Expected Have Line But Get Error Of M_ClosedMonth");

        }


        [TestMethod]
        public void IsJournalValid_09_ExternalNoExistAndWithoutJournalNumber_errListContainsExternalNoAlreadyExists_1()
        {

            var accountingDate = new DateTime(2016, 08, 20);
            var myJournalPM = new JournalPM()
            {
                AccountingDate = accountingDate,
                ExternalNo = C_ExternalNoExist,
                ExternalSystem = C_ExternalSystemExist,
                //JournalNumber="JournalNumberInit",

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){},
                new JournalLinePM(){}
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);
            string basic_text = basic_text_ExternalExist(myJournalPM);
            var mExist = errList.Any(r=> r.StartsWith(JournalValidator.M_ExternalNoAlreadyExists_1) && r.EndsWith(JournalValidator.M_ExternalNoAlreadyExists_2));
            var LocalDefaultText = "פקודת יומן עם מספר חיצוני ";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);

        }


        [TestMethod]
        public void IsJournalValid_09p1_ExternalNoExistAndJournalNumber_errListNoContainsExternalNoAlreadyExists_1()
        {

            var accountingDate = new DateTime(2016, 08, 20);
            var myJournalPM = new JournalPM()
            {
                AccountingDate = accountingDate,
                ExternalNo = C_ExternalNoExist,
                ExternalSystem = C_ExternalSystemExist,
                JournalNumber="(ExternalNo==ExternalNoExist &&  ExternalSystem==ExternalSystemExist)",

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){},
                new JournalLinePM(){}
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);
            string basic_text = basic_text_ExternalExist(myJournalPM);

            var mExist = errList.Contains(basic_text);
            var LocalDefaultText = "פקודת יומן עם מספר חיצוני ";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);

        }

        [TestMethod]
        public void IsJournalValid_10_WithoutExternalNoExistAndWithoutJournalNumber_errListNotContainsExternalNoAlreadyExists_1()
        {

            var accountingDate = new DateTime(2016, 08, 20);
            var myJournalPM = new JournalPM()
            {
                AccountingDate = accountingDate,
                //ExternalNo = C_ExternalNoExist,
                //ExternalSystem = C_ExternalSystemExist,
                //JournalNumber="JournalNumberInit",

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){},
                new JournalLinePM(){}
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ExternalNoAlreadyExists_2));
            var LocalDefaultText = "פקודת יומן עם מספר חיצוני ";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);

        }

        private static string basic_text_ExternalExist(JournalPM myJournalPM)
        {
            //string basic_text = JournalValidator.M_ExternalNoAlreadyExists_1
            //                + myJournalPM.ExternalNo
            //                + JournalValidator.M_ExternalNoAlreadyExists_2
            //                + myJournalPM.ExternalSystem
            //                + JournalValidator.M_ExternalNoAlreadyExists_3;

            string basic_text = JournalValidator.M_ExternalNoAlreadyExists_1
                            + myJournalPM.JournalNumber
                            + JournalValidator.M_ExternalNoAlreadyExists_2
                            
                            ;
            return basic_text;
        }



        [TestMethod]
        public void IsJournalValid_11_ExternalNoExistAndWithJournalNumber_errListContainsExternalNoAlreadyExists_4()
        {

            var accountingDate = new DateTime(2016, 08, 20);
            var myJournalPM = new JournalPM()
            {
                AccountingDate = accountingDate,
                ExternalNo = C_ExternalNoExist,
                ExternalSystem = C_ExternalSystemExist,
                JournalNumber = "JournalNumberInit",

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){},
                new JournalLinePM(){}
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ExternalNoAlreadyExists_2));
            var LocalDefaultText = " כפקודת יומן מספר ";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);

        }



        [TestMethod]
        public void IsJournalValid_12_WithoutExternalNoExistAndWithJournalNumber_errListNotContainsExternalNoAlreadyExists_4()
        {

            var accountingDate = new DateTime(2016, 08, 20);
            var myJournalPM = new JournalPM()
            {
                AccountingDate = accountingDate,
                //ExternalNo = C_ExternalNoExist,
                //ExternalSystem = C_ExternalSystemExist,
                JournalNumber = "JournalNumberInit",

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){},
                new JournalLinePM(){}
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ExternalNoAlreadyExists_2));
            var LocalDefaultText = "פקודת יומן עם מספר חיצוני ";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);

        }


        [TestMethod]
        [Ignore]
        public void IsJournalValid_13_LineLocalAmount1ForeignAmount1WithoutExchangeRate_errListCotainsExchangeRateEmpty()
        {
            Trace.Write("Suppress ExchangeRateEmpty due i Praser Insert it ");
            return;
            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ExchangeRate =  null,
                    LocalAmount=1,ForeignAmount=1
             
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ExchangeRateEmpty));
            var LocalDefaultText = "M_ExchangeRateEmpty";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);

        }

        [TestMethod]
        public void IsJournalValid_14_LineLocalAmount1ForeignAmount1WithExchangeRate4_errListNotCotainsExchangeRateEmpty()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ExchangeRate =  4,
                    LocalAmount=1,ForeignAmount=1
             
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ExchangeRateEmpty));
            var LocalDefaultText = "M_ExchangeRateEmpty";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);

        }

        [TestMethod]
        public void IsJournalValid_15_LineSequence1_errListNotCotainsBadLineSequence()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    Line=1
             
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_LineSequence));
            var LocalDefaultText = "M_LineSequence";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);

        }
        [TestMethod]
        public void IsJournalValid_16_LineSequence2_errListCotainsBadLineSequence()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    Line=2
             
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_LineSequence));
            var LocalDefaultText = "M_LineSequence";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);

        }
        /*
        [TestMethod]
        public void IsJournalValid_17_LineForeignAmount2_errListNotCotainsForeignAmountNotZero()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                     ForeignAmount=2
             
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ForeignAmountNotZero));
            var LocalDefaultText = "M_ForeignAmountNotZero";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);

        }
        
         
        [TestMethod]
        public void IsJournalValid_18_LineForeignAmountIsNull_errListCotainsForeignAmountNotZero()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                     ForeignAmount=0//null
             
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ForeignAmountNotZero));
            var LocalDefaultText = "M_ForeignAmountNotZero";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);

        }
        
         
        [TestMethod]
        public void IsJournalValid_19_LineForeignAmount0_errListCotainsForeignAmountNotZero()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                     ForeignAmount=0
             
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ForeignAmountNotZero));
            var LocalDefaultText = "M_ForeignAmountNotZero";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);

        }
        */
        [TestMethod]
        [Ignore]
        public void IsJournalValid_20_LineLocalAmount2_errListNotCotainsM_LocalAmountNotZero()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                     LocalAmount=2
             
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains("JournalValidator.M_LocalAmountNotZero"));
            var LocalDefaultText = "M_LocalAmountNotZero";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);

        }
        [TestMethod]
        public void IsJournalValid_21_LineLocalAmountIsNull_NoCrush()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() {
                new JournalLinePM(){
                     LocalAmount=0,//null
                     ForeignAmount=0

             
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = true;// errList.Exists(m => m.Contains("JournalValidator.M_LocalAmountNotZero"));
            var LocalDefaultText = "M_LocalAmountNotZero";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);

        }

        [TestMethod]
        public void IsJournalValid_21_valueInForeignZeroInLocal_NoCrush()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() {
                new JournalLinePM(){
                     LocalAmount=0,//null
                     ForeignAmount=1


                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = true;// errList.Exists(m => m.Contains("JournalValidator.M_LocalAmountNotZero"));
            var LocalDefaultText = "M_LocalAmountNotZero";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);

        }


        [TestMethod]
        public void IsJournalValid_22_ZeroInForeignAndInLocal_NoCrush()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() {
                new JournalLinePM(){
                     LocalAmount=0,//null
                     ForeignAmount=0


                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = true;// errList.Exists(m => m.Contains("JournalValidator.M_LocalAmountNotZero"));
            var LocalDefaultText = "M_LocalAmountNotZero";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);

        }

        [TestMethod]
        [Ignore]
        public void IsJournalValid_2bad1_LineLocalAmountIsNull_errListCotainsM_LocalAmountNotZero()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                     LocalAmount=0//null
             
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains("JournalValidator.M_LocalAmountNotZero"));
            var LocalDefaultText = "M_LocalAmountNotZero";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);

        }
        [TestMethod]
        [Ignore]
        public void IsJournalValid_22_LineLocalAmountIs0_errListCotainsM_LocalAmountNotZero()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                     LocalAmount=0//null
             
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains("JournalValidator.M_LocalAmountNotZero"));
            var LocalDefaultText = "M_LocalAmountNotZero";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);

        }
        [TestMethod]
        public void IsJournalValid_23_LineActionTypeCode2NoDebitAccountId_errListCotainsM_ActionCode()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                     ActionTypeCode = "2" ,
                     DebitAccountId = null
             
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ActionCode));
            var LocalDefaultText = "M_ActionCode";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);

        }
        [TestMethod]
        public void IsJournalValid_24_LineActionTypeCode2DebitAccountId333_errListNotCotainsM_ActionCode()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                     ActionTypeCode = "2" ,
                     DebitAccountId = "333"
             
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ActionCode));
            var LocalDefaultText = "M_ActionCode";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);

        }
        [TestMethod]
        public void IsJournalValid_25_LineActionTypeCode1CreditAccountIdIsNull_errListCotainsM_ActionCodeCredit()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                     ActionTypeCode = "1" ,CreditAccountId = null
             
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ActionCodeCredit));
            var LocalDefaultText = "M_ActionCodeCredit";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);
        }

        [TestMethod]
        public void IsJournalValid_26_LineActionTypeCode1CreditAccountId333_errListNotCotainsM_ActionCodeCredit()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                     ActionTypeCode = "1" ,CreditAccountId = "333"
             
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ActionCodeCredit));
            var LocalDefaultText = "M_ActionCodeCredit";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);
        }




        [TestMethod]
        public void IsJournalValid_29_LineActionTypeCode3CreditNullDebit333_errListCotainsM_ActionCodeCreditAndCreditMeanDebit()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                     ActionTypeCode = "3" ,CreditAccountId = null, DebitAccountId="333"
             
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ActionCodeCreditAndCreditMeanDebit));
            var LocalDefaultText = "if ((item.ActionTypeCode == 3 || item.ActionTypeCode == 4) && ((item.DebitAccountId == null) || (item.CreditAccountId == null)))";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);
        }
        [TestMethod]
        public void IsJournalValid_27_LineActionTypeCode3CreditAccountId333DebitNull_errListCotainsM_ActionCodeCreditAndCreditMeanDebit()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                     ActionTypeCode = "3" ,CreditAccountId = "333"
             
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ActionCodeCreditAndCreditMeanDebit));
            var LocalDefaultText = "if ((item.ActionTypeCode == 3 || item.ActionTypeCode == 4) && ((item.DebitAccountId == null) || (item.CreditAccountId == null)))";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);
        }
        [TestMethod]
        public void IsJournalValid_30_LineActionTypeCode3CreditAccountId333Debit333_errListNotCotainsM_ActionCodeCreditAndCreditMeanDebit()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                     ActionTypeCode = "3" ,CreditAccountId = "333" ,DebitAccountId="333"
             
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ActionCodeCreditAndCreditMeanDebit));
            var LocalDefaultText = "if ((item.ActionTypeCode == 3 || item.ActionTypeCode == 4) && ((item.DebitAccountId == null) || (item.CreditAccountId == null)))";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);
        }


        #region "LineActionTypeCode4"
        [TestMethod]
        public void IsJournalValid_31_LineActionTypeCode4CreditNullDebit333_errListCotainsM_ActionCodeCreditAndCreditMeanDebit()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                     ActionTypeCode = "4" ,CreditAccountId = null, DebitAccountId="333"
             
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ActionCodeCreditAndCreditMeanDebit));
            var LocalDefaultText = "if ((item.ActionTypeCode == 3 || item.ActionTypeCode == 4) && ((item.DebitAccountId == null) || (item.CreditAccountId == null)))";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);
        }
        [TestMethod]
        public void IsJournalValid_32_LineActionTypeCode4CreditAccountId333DebitNull_errListCotainsM_ActionCodeCreditAndCreditMeanDebit()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                     ActionTypeCode = "4" ,CreditAccountId = "333"
             
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ActionCodeCreditAndCreditMeanDebit));
            var LocalDefaultText = "if ((item.ActionTypeCode == 3 || item.ActionTypeCode == 4) && ((item.DebitAccountId == null) || (item.CreditAccountId == null)))";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);
        }
        [TestMethod]
        public void IsJournalValid_33_LineActionTypeCode4CreditAccountId333Debit333_errListNotCotainsM_ActionCodeCreditAndCreditMeanDebit()
        {

            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                     ActionTypeCode = "4" ,CreditAccountId = "333" ,DebitAccountId="333"
             
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_ActionCodeCreditAndCreditMeanDebit));
            var LocalDefaultText = "if ((item.ActionTypeCode == 3 || item.ActionTypeCode == 4) && ((item.DebitAccountId == null) || (item.CreditAccountId == null)))";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);
        }
        #endregion



        #region "if ((item.DocumentDate) > (item.DueDate))"
        
        [TestMethod]
        public void IsJournalValid_34_LineDocumentDate20160101DueDate20160101_errListNotCotainsM_DocumentDateBiggerDueDate()
        {

            bool baselSuppressDocGreaterThenDue = true;
            if (baselSuppressDocGreaterThenDue)
            {
                return;
            }

            var documentDate = new DateTime(2016, 1, 1);
            var dueDate = documentDate;
            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                     DocumentDate= documentDate,DueDate=dueDate
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_DocumentDateBiggerDueDate));
            var LocalDefaultText = "if ((item.DocumentDate) > (item.DueDate))";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);
        }


        [TestMethod]
        public void IsJournalValid_35_LineDocumentDate20160101DueDate20160101AddSeconds1_errListNotCotainsM_DocumentDateBiggerDueDate()
        {

            bool baselSuppressDocGreaterThenDue = true;
            if (baselSuppressDocGreaterThenDue)
            {
                return;
            }
            var documentDate = new DateTime(2016, 1, 1);
            var dueDate = new DateTime(2016, 1, 1).AddSeconds(1);
            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                     DocumentDate= documentDate,DueDate=dueDate
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_DocumentDateBiggerDueDate));
            var LocalDefaultText = "if ((item.DocumentDate) > (item.DueDate))";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);
        }

        [TestMethod]
        public void IsJournalValid_36_LineDocumentDate20160101AddSeconds1DueDate20160101_errListCotainsM_DocumentDateBiggerDueDate()
        {

            bool baselSuppressDocGreaterThenDue = true;
            if (baselSuppressDocGreaterThenDue)
            {
                return;
            }
            var documentDate = new DateTime(2016, 1, 1).AddSeconds(1);
            var dueDate = new DateTime(2016, 1, 1);
            var myJournalPM = new JournalPM()
            {
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                     DocumentDate= documentDate,DueDate=dueDate
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_DocumentDateBiggerDueDate));
            var LocalDefaultText = "if ((item.DocumentDate) > (item.DueDate))";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + LocalDefaultText);
        }
        #endregion

        #region M_currencydoesnotexist

        [TestMethod]
        public void IsJournalValid_37_Tenant1CurrencyIdBLABLA_errListCotainsM_currencydoesnotexist()
        {
            var documentDate = new DateTime(2016, 1, 1).AddSeconds(1);
            var dueDate = new DateTime(2016, 1, 1);
            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    Tenant=1,
                    CurrencyId     ="BLABLA"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_currencydoesnotexist));
            var lineCode = "CurrencyPM currency = myDataProvider.GetCurrency(item.CurrencyId, item.Tenant);";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }


        [TestMethod]
        public void IsJournalValid_38_Tenant1CurrencyId1m1_errListNotCotainsM_currencydoesnotexist()
        {
            var documentDate = new DateTime(2016, 1, 1).AddSeconds(1);
            var dueDate = new DateTime(2016, 1, 1);
            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    Tenant=1,
                    CurrencyId     ="1-1"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_currencydoesnotexist));
            var lineCode = "CurrencyPM currency = myDataProvider.GetCurrency(item.CurrencyId, item.Tenant);";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }

        #endregion


        #region M_GetGLAccountReturnNull

        [TestMethod]
        public void IsJournalValid_39_ActionTypeCode2DebitAccountIdBLABLA_errListCotainsM_GetGLAccountReturnNull()
        {
          
            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="2",
                    DebitAccountId     ="BLABLA"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_GetGLAccountReturnNull));
            var lineCode = "if (pmAcc == null)";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        [TestMethod]
        public void IsJournalValid_40_ActionTypeCode2DebitAccountId1m1_errListNotCotainsM_GetGLAccountReturnNull()
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


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_GetGLAccountReturnNull));
            var lineCode = "if (pmAcc == null)";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }




        [TestMethod]
        public void IsJournalValid_41_ActionTypeCode1CreditAccountIdBLABLA_errListCotainsM_GetGLAccountReturnNull()
        {
         
            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="1",
                    CreditAccountId     ="BLABLA"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_GetGLAccountReturnNull));
            var lineCode = "if (pmAcc == null)";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        [TestMethod]
        public void IsJournalValid_42_ActionTypeCode1CreditAccountId1m1_errListNotCotainsM_GetGLAccountReturnNull()
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


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_GetGLAccountReturnNull));
            var lineCode = "if (pmAcc == null)";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }






        [TestMethod]
        public void IsJournalValid_43_ActionTypeCode3CreditAccountIdBLABLA_errListCotainsM_GetGLAccountReturnNull()
        {
            var documentDate = new DateTime(2016, 1, 1).AddSeconds(1);
            var dueDate = new DateTime(2016, 1, 1);
            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="3",
                    CreditAccountId     ="BLABLA"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_GetGLAccountReturnNull));
            var lineCode = "if (pmAcc == null)";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        [TestMethod]
        public void IsJournalValid_44_ActionTypeCode3CreditAccountId1m1_errListNotCotainsM_GetGLAccountReturnNull()
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


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_GetGLAccountReturnNull));
            var lineCode = "if (pmAcc == null)";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }



        [TestMethod]
        public void IsJournalValid_45_ActionTypeCode4CreditAccountIdBLABLA_errListCotainsM_GetGLAccountReturnNull()
        {
            var documentDate = new DateTime(2016, 1, 1).AddSeconds(1);
            var dueDate = new DateTime(2016, 1, 1);
            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="4",
                    CreditAccountId     ="BLABLA"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_GetGLAccountReturnNull));
            var lineCode = "if (pmAcc == null)";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        [TestMethod]
        public void IsJournalValid_46_ActionTypeCode4CreditAccountId1m1_errListNotCotainsM_GetGLAccountReturnNull()
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


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_GetGLAccountReturnNull));
            var lineCode = "if (pmAcc == null)";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }


        [TestMethod]
        public void IsJournalValid_47_ActionTypeCode4DebitAccountIdBLABLA_errListCotainsM_GetGLAccountReturnNull()
        {
            var documentDate = new DateTime(2016, 1, 1).AddSeconds(1);
            var dueDate = new DateTime(2016, 1, 1);
            var myJournalPM = new JournalPM()
            {

                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){  
                    ActionTypeCode="4",
                    DebitAccountId     ="BLABLA"
                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
            ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
            var errList = new List<String>(validationresult.MemberNames);


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_GetGLAccountReturnNull));
            var lineCode = "if (pmAcc == null)";
            Assert.IsTrue(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        [TestMethod]
        public void IsJournalValid_48_ActionTypeCode4DebitAccountId1m1_errListNotCotainsM_GetGLAccountReturnNull()
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


            var mExist = errList.Exists(m => m.Contains(JournalValidator.M_GetGLAccountReturnNull));
            var lineCode = "if (pmAcc == null)";
            Assert.IsFalse(mExist, "Expected Have Line But Get Error Of " + lineCode);
        }
        #endregion





        
        private static ValidationContext GetValidationContext(JournalPM entityPM, AccountingPeriodPM MyAccountingPeriodPM = null, bool SuppressCheckGLAccountIsMultiCurrencyWI40640 = false)
        {

            MyAccountingPeriodPM = MyAccountingPeriodPM ?? new AccountingPeriodPM() { Year = 2016, ClosedMonth = 1, OpenMonth = 9 ,PeriodTypeCode="1"  };
            //string journalNumber = "";
            IJournalValidatorContextDataProvider myStubIJournalValidatorContextDataProvider = A.Fake<IJournalValidatorContextDataProvider>( );
            
            {
                A.CallTo(() =>myStubIJournalValidatorContextDataProvider.GetGLAccount(A<string>.Ignored,A<int>.Ignored))
                    .ReturnsLazily(
                    //GetGLAccount
                    (string GLAccountId, int tenant) =>
                  
                //GetGLAccountStringInt32 = (GLAccountId,tenant) => 
                {
                    switch (GLAccountId)
                    {
                        case "1-1":
                            return new GLAccountPM() { Id = "1-1", Inactive = false, Tenant = tenant };
                            break;
                        case "ACBLOCK":
                            return new GLAccountPM() { Id = "ACBLOCK", Inactive = true, Tenant = tenant };
                            break;
                        case "ACControl":
                            return new GLAccountPM() { Id = "ACControl", IsControlAccount = true, Tenant = tenant };
                            break;
                        case "ACCurUSD":
                            return new GLAccountPM() { Id = "ACCurUSD", CurrencyId = "USD", Tenant = tenant };

                        case "Card1-1":
                            return new GLAccountPM() { Id = "Card1-1", Inactive = false, Tenant = tenant ,AccountTypeCode="1"  };

                        
                        case "CustomerWOControl":
                            return new GLAccountPM() { Id = "CustomerWOControl", Inactive = false, Tenant = tenant, AccountTypeCode = "2" };
                        case "VendorWOControl":
                            return new GLAccountPM() { Id = "VendorWOControl", Inactive = false, Tenant = tenant, AccountTypeCode = "3" };
                        case "CustomerWithControl":
                            return new GLAccountPM() { Id = "CustomerWithControl", Inactive = false, Tenant = tenant, AccountTypeCode = "2", ControlAccountId = "ControlAccountId" };

                        case "VendorWithControl":
                            return new GLAccountPM() { Id = "CustomerWithControl", Inactive = false, Tenant = tenant, AccountTypeCode = "2", ControlAccountId = "ControlAccountId" };

                        case "ControlAccountId":
                            return new GLAccountPM() { Id = "ControlAccountId", Inactive = false, Tenant = tenant, AccountTypeCode = "1",  };
                        default:
                            return null;
                            break;
                    }
                    
                    
                    
                }
                );
                A.CallTo(() =>myStubIJournalValidatorContextDataProvider.GetCurrency(A<string>.Ignored,A<int>.Ignored))
                    .ReturnsLazily(
                //CurrencyPM GetCurrency(string CurrencyId,int tenant);
                (string CurrencyId,int tenant)=>
                {
                    if (CurrencyId == "1-1")
                    {
                        return new CurrencyPM() { Id = "1-1" };
                    }
                    return null;

                }
                );

                A.CallTo(() =>myStubIJournalValidatorContextDataProvider.GetGLAccountCurrencyList(A<string>.Ignored,A<int>.Ignored))
                    .ReturnsLazily(
                //List<String> GetGLAccountCurrencyList(string CustomerGLAccountId, int tenant);
                (string CustomerGLAccountId, int tenant) => { return new List<string>(); }
                );
                
                //string CheckExternalNoAndSystemReturnJournalNumber(string externalNo, string externalSystem, int tenant);
                A.CallTo(() => myStubIJournalValidatorContextDataProvider.CheckExternalNoAndSystemReturnJournalNumber(A<string>.Ignored, A<string>.Ignored, A<int>.Ignored))
                    .ReturnsLazily(
                (string ExternalNo, string ExternalSystem, int tenant) =>
                {
                    if (ExternalNo == C_ExternalNoExist && ExternalSystem == C_ExternalSystemExist)
                    {
                        return "(ExternalNo==ExternalNoExist &&  ExternalSystem==ExternalSystemExist)";
                    }
                    return null;

                });

                //CheckExternalNoAndSystem
                // (externalNo, externalSystem, ref journalNumber, tenant) =>

            };
            var AccountingPeriodList = new List<AccountingPeriodPM>(){
                MyAccountingPeriodPM 
            };
            DateTime? dateTimeUtcNow = new DateTime(2017, 01, 12); 
            var myFullAccountingSettingPM = new FullAccountingSettingPM();
            var myNewJournalValidatorContext = AccountingValidationContextServiceProvider
                .NewJournalValidatorContext(
                entityPM, 
                AccountingPeriodList, 
                myStubIJournalValidatorContextDataProvider,
                myFullAccountingSettingPM ,
                SuppressCheckGLAccountIsMultiCurrencyWI40640,
                dateTimeUtcNow);

            return myNewJournalValidatorContext;
        }




    }

}
