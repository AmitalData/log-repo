using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Def.EntityPMs;
using System.Collections.Generic;
using Logitude.Accounting.BL.CoreBL;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.Diagnostics;
using Logitude.Accounting.BL.Validators;


namespace Logitude.UnitTest.Accounting.UniTests
{
    public partial class JournalValidatorUnitTest
    {



        //Journal Is Not Valid: Journal.M.AllAccountingDateMustEqual;Journal.M.ControlAccountIdIsMust1-1;Journal.M.ControlAccountIdIsMust1-1



        [TestMethod]
        public void IsJournalValid0700_JournalPMRange_ErrorContainsFutureDateForbidden ()
        {

            
            var listOfState2Check = new List<Tuple<JournalPM, string>>();
            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    AccountingDate = new DateTime(2017, 01, 13), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "0",
                    JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,

                     DebitAccountId="1-1", CreditAccountId="1-1"
                     
                }
            }
                }, "JournalLine without AccountingDate"));

            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    AccountingDate = new DateTime(2017, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "0",
                    JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     AccountingDate = new DateTime(2013, 3, 2), 
                     DebitAccountId="1-1", CreditAccountId="1-1"
                     
                }
            }
                }, "JournalLine.AccountingDate diffrent in 1 days"));

            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    //AccountingDate = new DateTime(2017, 1, 1), //ClosedMonth = 1, OpenMonth = 9 
                    AccountingDate = new DateTime(2017, 1, 20), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "0",
                    JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     AccountingDate = new DateTime(2017, 1, 20), 
                     DebitAccountId="1-1", CreditAccountId="1-1"
                     
                }
            }
                }, "JournalLine.AccountingDate diffrent in 1 days"));

            foreach (var state in listOfState2Check)
            {
                var myJournalPM = state.Item1;

                var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
                
                ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);

                Assert.IsNotNull(validationresult);
                var errList = new List<String>(validationresult.MemberNames);
                var messageExist = errList.Exists(m => m.Contains(JournalValidator.M_AccountingFutureDateForbidden));
                var lineCode = "if (currDate.GetValueOrDefault().Date < myJournalPM.AccountingDate.Date)";
                Assert.IsTrue(messageExist, state.Item2 + " Expected Have Line But Get Error Of " + lineCode);
            }
        }



        [TestMethod]
        public void IsJournalValid0700_JournalPMRange_ErrorContainsAllAccountingDateMustEqual()
        {


            var listOfState2Check = new List<Tuple<JournalPM, string>>();
            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
            {
                AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                StatusCode = "0",
                JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,

                     DebitAccountId="1-1", CreditAccountId="1-1"
                     
                }
            }
            }, "JournalLine without AccountingDate"));

            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "0",
                    JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     AccountingDate = new DateTime(2013, 3, 2), 
                     DebitAccountId="1-1", CreditAccountId="1-1"
                     
                }
            }
                }, "JournalLine.AccountingDate diffrent in 1 days"));

            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    //AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "0",
                    JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     AccountingDate = new DateTime(2016, 2, 1,1,1,1), 
                     DebitAccountId="1-1", CreditAccountId="1-1"
                     
                }
            }
                }, "JournalLinePM.AccountingDate is null"));
            foreach (var state in listOfState2Check)
            {
                var myJournalPM = state.Item1;

                var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
                ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);

                Assert.IsNotNull(validationresult);
                var errList = new List<String>(validationresult.MemberNames);
                var messageExist = errList.Exists(m => m.Contains(JournalValidator.M_JLAccountingDateMustWithinJournalMonth));
                var lineCode = "if (myJournalPM.JournalLines.Any(l => l.AccountingDate.Date != myJournalPM.AccountingDate.Date))";
                Assert.IsTrue(messageExist, state.Item2 + " Expected Have Line But Get Error Of " + lineCode);
            }
        }



        [TestMethod]
        public void IsJournalValid0701_JournalPMRangeLessADayDiffrent_ErrorNotContainsM_JLAccountingDateMustWithinJournalMonth()
        {


            var listOfState2Check = new List<Tuple<JournalPM, string>>();


            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "0",
                    JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     AccountingDate = new DateTime(2016, 3, 10,1,1,1), 
                     DebitAccountId="1-1", CreditAccountId="1-1"
                     
                }
            }
                }, "JournalLine.AccountingDate diffrent in Even Hour"));


            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "0",
                    JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     AccountingDate = new DateTime(2016, 3, 22,20,1,1), 
                     DebitAccountId="1-1", CreditAccountId="1-1"
                     
                }
            }
                }, "JournalLine.AccountingDate diffrent in Even 20 Hour"));

            foreach (var state in listOfState2Check)
            {
                var myJournalPM = state.Item1;

                var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
                ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);

                Assert.IsNotNull(validationresult);
                var errList = new List<String>(validationresult.MemberNames);
                var messageExist = errList.Exists(m => m.Contains(JournalValidator.M_JLAccountingDateMustWithinJournalMonth));
                var lineCode = "if (myJournalPM.JournalLines.Any(l => l.AccountingDate.Date != myJournalPM.AccountingDate.Date))";
                Assert.IsFalse(messageExist, state.Item2 + " Expected Have Line But Get Error Of " + lineCode);
            }
        }




        [TestMethod]
        public void IsJournalValid0702_JournalPMCard_JournalIsValid()
        {
            var listOfState2Check = new List<Tuple<JournalPM, string>>();


            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "0",
                    JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     AccountingDate = new DateTime(2016, 3, 1,1,1,1), 
                     DebitAccountId="Card1-1", CreditAccountId="Card1-1",
                     DueDate = new DateTime(2016,3,1), 
                     DocumentDate = new DateTime(2016,3,1), 
                     
                }
            }
                }, "Card no need to connect to  Control Account "));



            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "0",
                    JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=1 ,
                     //ExchangeRate=,
                     ForeignAmount=0,
                     AccountingDate = new DateTime(2016, 3, 1,1,1,1), 
                     DebitAccountId="Card1-1", CreditAccountId="Card1-1",
                     DueDate = new DateTime(2016,3,1), 
                     DocumentDate = new DateTime(2016,3,1), 
                     
                }
            }
                }, "ForeignAmount=0 not cause divide by zeroo"));

            foreach (var state in listOfState2Check)
            {
                var myJournalPM = state.Item1;

                var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
                ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
                var lineCode = "if (String.IsNullOrWhiteSpace(pmAcc.ControlAccountId))";

                Assert.IsNull(validationresult, lineCode);

                //var errList = new List<String>(validationresult.MemberNames);
                //var messageExist = errList.Exists(m => m.Contains(JournalValidator.M_ControlAccountIdIsMust));
                
                //Assert.IsFalse(messageExist, state.Item2 + " Expected Have Line But Get Error Of " + lineCode);
            }
        }

        [TestMethod]
        public void IsJournalValid0703_JournalPMNotCard_ErrContains_ControlAccountIdIsMust()
        {
            var lineCode = "if (String.IsNullOrWhiteSpace(pmAcc.ControlAccountId))";
            var listOfState2Check = new List<Tuple<JournalPM, string>>();
            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "0",
                    JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     AccountingDate = new DateTime(2016, 3, 1,1,1,1), 
                     DebitAccountId="VendorWOControl", CreditAccountId="Card1-1"
                     
                }
            }
                }, "VendorWOControl need to connect to  Control Account "));



            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "0",
                    JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     AccountingDate = new DateTime(2016, 3, 1,1,1,1), 
                     DebitAccountId="CustomerWOControl", CreditAccountId="Card1-1"
                     
                }
            }
                }, "CustomerWOControl need to connect to  Control Account "));


            foreach (var state in listOfState2Check)
            {
                var myJournalPM = state.Item1;

                var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
                ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);
                

                Assert.IsNotNull(validationresult, lineCode);

                var errList = new List<String>(validationresult.MemberNames);
                var messageExist = errList.Exists(m => m.Contains(JournalValidator.M_ControlAccountIdIsMust));

                Assert.IsTrue(messageExist, state.Item2 + " Expected Have Line But Get Error Of " + lineCode);
            }
        }


        [TestMethod]
        public void IsJournalValid0705_JournalApprovedBadControlAccount_ErrContains_ControlAccountIdIsNotMatch()
        {

            var lineCode = "if (String.IsNullOrWhiteSpace(pmAcc.ControlAccountId))";
            var listOfState2Check = new List<Tuple<JournalPM, string>>();
            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "6", //var isApproved=myJournalPM.StatusCode=="2";
                    JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     AccountingDate = new DateTime(2016, 3, 1,1,1,1), 
                     DebitAccountId="Card1-1",  CreditAccountId="CustomerWithControl",
                     CreditControlAccountId="BAD CreditControlAccountId"
                     
                }
            }
                }, "'BAD CreditControlAccountId' !=  'ControlAccountId' "));


            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "6",
                    JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     AccountingDate = new DateTime(2016, 3, 1,1,1,1), 
                     DebitAccountId="Card1-1",  CreditAccountId="VendorWithControl",
                     CreditControlAccountId="BAD VendorWithControl"
                     
                }
            }
                }, "'BAD VendorWithControl' !=  'ControlAccountId' "));


            foreach (var state in listOfState2Check)
            {
                var myJournalPM = state.Item1;

                var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
                ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);


                Assert.IsNotNull(validationresult, lineCode);

                var errList = new List<String>(validationresult.MemberNames);
                var messageExist = errList.Exists(m => m.Contains(JournalValidator.M_ControlAccountIdIsNotMatch));

                Assert.IsTrue(messageExist, state.Item2 + " Expected Have Line But Get Error Of " + lineCode);
            }

        }



        [TestMethod]
        public void IsJournalValid0706_JournalApprovedGoodControlAccount_ErrNotContains_ControlAccountIdIsNotMatch()
        {

            var lineCode = "if (String.IsNullOrWhiteSpace(pmAcc.ControlAccountId))";
            var listOfState2Check = new List<Tuple<JournalPM, string>>();
            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "6", //var isApproved=myJournalPM.StatusCode=="2";
                    JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     AccountingDate = new DateTime(2016, 3, 1,1,1,1), 
                     DebitAccountId="Card1-1",  CreditAccountId="CustomerWithControl",
                     DueDate = new DateTime(2016, 3, 1), 
                     DocumentDate = new DateTime(2016, 3, 1), 
                     CreditControlAccountId="ControlAccountId"
                     
                }
            }
                }, "good ... 'ControlAccountId' ==  'ControlAccountId' "));


            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "6",
                    JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     AccountingDate = new DateTime(2016, 3, 1,1,1,1), 
                     DebitAccountId="Card1-1",  CreditAccountId="VendorWithControl",
                     CreditControlAccountId="ControlAccountId",
                     DueDate = new DateTime(2016, 3, 1), 
                     DocumentDate = new DateTime(2016, 3, 1), 
                }
            }
                }, "good ... 'ControlAccountId' ==  'ControlAccountId' "));


            foreach (var state in listOfState2Check)
            {
                var myJournalPM = state.Item1;

                var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
                ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);


                Assert.IsNull(validationresult, lineCode);

                //var errList = new List<String>(validationresult.MemberNames);
                //var messageExist = errList.Exists(m => m.Contains(JournalValidator.M_ControlAccountIdIsNotMatch));

                //Assert.IsFalse(messageExist, state.Item2 + " Expected Have Line But Get Error Of " + lineCode);
            }

        }


        [TestMethod]
        public void IsJournalValid0708Range_JournalPMRangeLessADayDiffrent_ErrorContainsM_AllDateMustInit()
        {

            var lineCode = "if (myJournalPM.JournalLines.Any(l => l.AccountingDate == DateTime.MinValue))";
            var listOfState2Check = new List<Tuple<JournalPM, string>>();


            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "0",
                    JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     //AccountingDate = new DateTime(2016, 3, 1,1,1,1), 
                     DebitAccountId="1-1", CreditAccountId="1-1",
                 DueDate=    new DateTime(2016, 3, 1),
                 DocumentDate=    new DateTime(2016, 3, 1),
                }
            }
                }, "JournalLine.AccountingDate == DateTime.MinValue"));


            
            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "0",
                    JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     //AccountingDate = new DateTime(2016, 3, 1,1,1,1), 
                     DebitAccountId="1-1", CreditAccountId="1-1",
                     AccountingDate = new DateTime(2016, 3, 1), //
                 //DueDate=    new DateTime(2016, 3, 1),
                 DocumentDate=    new DateTime(2016, 3, 1),
                }
            }
                }, "JournalLine.DueDate== DateTime.MinValue"));



            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "0",
                    JournalLines = new List<JournalLinePM>() { 
                new JournalLinePM(){ 
                    Line=1,
                    ActionTypeCode="3",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     //AccountingDate = new DateTime(2016, 3, 1,1,1,1), 
                     DebitAccountId="1-1", CreditAccountId="1-1",
                     AccountingDate = new DateTime(2016, 3, 1), //
                 DueDate=    new DateTime(2016, 3, 1),
                 //DocumentDate=    new DateTime(2016, 3, 1),
                }
            }
                }, "JournalLine.DocumentDate== DateTime.MinValue"));

            foreach (var state in listOfState2Check)
            {
                var myJournalPM = state.Item1;

                var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
                ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);

                Assert.IsNotNull(validationresult);
                var errList = new List<String>(validationresult.MemberNames);
                var messageExist = errList.Exists(m => m.Contains(JournalValidator.M_AllDateMustInit));
                
                Assert.IsTrue(messageExist, state.Item2 + " Expected Have Line But Get Error Of " + lineCode);
            }
        }




        [Ignore]
        [TestMethod]
        public void IsJournalValid07x01_regJornal2rowsSameOppositeAccountSameReference1_ErrorContainsM()
        {

            var lineCode = "if (myJournalPM.JournalLines.Any(l => l.AccountingDate == DateTime.MinValue))";
            var listOfState2Check = new List<Tuple<JournalPM, string>>();


            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    TypeCode= "0" ,//REGULAR JOURNAL
                    AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "0",
                    JournalLines = new List<JournalLinePM>() {
                new JournalLinePM(){
                    Line=1,
                    ActionTypeCode="1",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     //AccountingDate = new DateTime(2016, 3, 1,1,1,1), 
                     DebitAccountId="1-1",
                    CreditAccountId ="1-1",
                    Reference1="Reference1",
                 DueDate=    new DateTime(2016, 3, 1),
                 DocumentDate=    new DateTime(2016, 3, 1),
                },
                new JournalLinePM(){
                    Line=1,
                    ActionTypeCode="1",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     //AccountingDate = new DateTime(2016, 3, 1,1,1,1), 
                     DebitAccountId="1-1",
                    CreditAccountId ="1-1",
                    Reference1="Reference1",
                 DueDate=    new DateTime(2016, 3, 1),
                 DocumentDate=    new DateTime(2016, 3, 1),
                }
            }
                }, "REGULAR JOURNAL SAME CREDIT SAME REFERENCE"));




            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    TypeCode = "0",//REGULAR JOURNAL
                    AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "0",
                    JournalLines = new List<JournalLinePM>() {
                new JournalLinePM(){
                    Line=1,
                    ActionTypeCode="2",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     //AccountingDate = new DateTime(2016, 3, 1,1,1,1), 
                     DebitAccountId="1-1",
                    CreditAccountId ="1-1",
                    Reference1="Reference1",
                 DueDate=    new DateTime(2016, 3, 1),
                 DocumentDate=    new DateTime(2016, 3, 1),
                },
                new JournalLinePM(){
                    Line=1,
                    ActionTypeCode="2",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     //AccountingDate = new DateTime(2016, 3, 1,1,1,1), 
                     DebitAccountId="1-1",
                    CreditAccountId ="1-1",
                    Reference1="Reference1",
                 DueDate=    new DateTime(2016, 3, 1),
                 DocumentDate=    new DateTime(2016, 3, 1),
                }
            }
                }, "REGULAR JOURNAL SAME DEBIT SAME REFERENCE"));


            foreach (var state in listOfState2Check)
            {
                var myJournalPM = state.Item1;

                var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
                ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);

                Assert.IsNotNull(validationresult);
                var errList = new List<String>(validationresult.MemberNames);
                var messageExist = errList.Exists(m => m.Contains(JournalValidator.M_AccountingSameOppositeReference));

                Assert.IsTrue(messageExist, state.Item2 + " SameOppositeReference Of " + lineCode);
            }
        }


        [Ignore]
        [TestMethod]
        public void IsJournalValid07x02_NotRegJornal2rowsSameOppositeAccountSameReference1_NotContainsErrorContainsM()
        {

            var lineCode = "if (myJournalPM.JournalLines.Any(l => l.AccountingDate == DateTime.MinValue))";
            var listOfState2Check = new List<Tuple<JournalPM, string>>();


            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    TypeCode = "1",//template JOURNAL
                    AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "0",
                    JournalLines = new List<JournalLinePM>() {
                new JournalLinePM(){
                    Line=1,
                    ActionTypeCode="1",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     //AccountingDate = new DateTime(2016, 3, 1,1,1,1), 
                     DebitAccountId="1-1",
                    CreditAccountId ="1-1",
                    Reference1="Reference1",
                 DueDate=    new DateTime(2016, 3, 1),
                 DocumentDate=    new DateTime(2016, 3, 1),
                },
                new JournalLinePM(){
                    Line=1,
                    ActionTypeCode="1",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     //AccountingDate = new DateTime(2016, 3, 1,1,1,1), 
                     DebitAccountId="1-1",
                    CreditAccountId ="1-1",
                    Reference1="Reference1",
                 DueDate=    new DateTime(2016, 3, 1),
                 DocumentDate=    new DateTime(2016, 3, 1),
                }
            }
                }, "REGULAR JOURNAL SAME CREDIT SAME REFERENCE"));




            listOfState2Check.Add(new Tuple<JournalPM, string>(
                new JournalPM()
                {
                    TypeCode = "1",//template JOURNAL
                    AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                    StatusCode = "0",
                    JournalLines = new List<JournalLinePM>() {
                new JournalLinePM(){
                    Line=1,
                    ActionTypeCode="2",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     //AccountingDate = new DateTime(2016, 3, 1,1,1,1), 
                     DebitAccountId="1-1",
                    CreditAccountId ="1-1",
                    Reference1="Reference1",
                 DueDate=    new DateTime(2016, 3, 1),
                 DocumentDate=    new DateTime(2016, 3, 1),
                },
                new JournalLinePM(){
                    Line=1,
                    ActionTypeCode="2",
                    LocalAmount=1 ,
                     ExchangeRate=1,
                     ForeignAmount=1,
                     //AccountingDate = new DateTime(2016, 3, 1,1,1,1), 
                     DebitAccountId="1-1",
                    CreditAccountId ="1-1",
                    Reference1="Reference1",
                 DueDate=    new DateTime(2016, 3, 1),
                 DocumentDate=    new DateTime(2016, 3, 1),
                }
            }
                }, "REGULAR JOURNAL SAME DEBIT SAME REFERENCE"));


            foreach (var state in listOfState2Check)
            {
                var myJournalPM = state.Item1;

                var myNewJournalValidatorContext = GetValidationContext(myJournalPM);
                ValidationResult validationresult = JournalValidator.IsJournalValid(myJournalPM, myNewJournalValidatorContext);

                Assert.IsNotNull(validationresult);
                var errList = new List<String>(validationresult.MemberNames);
                var messageExist = errList.Exists(m => m.Contains(JournalValidator.M_AccountingSameOppositeReference));

                Assert.IsFalse(messageExist, state.Item2 + " SameOppositeReference Of " + lineCode);
            }
        }


    }

}

