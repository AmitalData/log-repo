#undef CHECK
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logitude.Accounting.BL.CoreBL.Mapping;
using Logitude.Accounting.Def.EntityPMs;
//using Microsoft.QualityTools.Testing.Fakes;
//using Logitude.Accounting.BL.CoreBL.Mapping.Fakes;
using Logitude.UnitTest.Utils;
using System.Collections.Generic;
using System.Diagnostics;
//using Logitude.Accounting.BL.CoreBL.Fakes;
using Logitude.Accounting.BL.Validators;
using FakeItEasy;



namespace Logitude.UnitTest.Accounting.UniTests
{
    [TestClass]
    public class JournalLineCreditMappingUnitTest
    {
        

        [TestMethod]
        public void journalLineCreditMappingDoItValueRange_JournalPMAndJournalLinePMNotDSame_ExpectedExceptionKeyAreDifferent()
        {

            var listOfState = new List<Tuple<JournalLinePM,JournalPM>>();


            //Case Equal Keys:  listOfDiffrentKey.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 2, JournalId = "1-1" }, new JournalPM() { Tenant = 2, Id = "1-1" }));


            listOfState.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 1 }, new JournalPM() { Tenant = 1, Id = "1-1" }));
            listOfState.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 2 }, new JournalPM() { Tenant = 1, Id = "1-1" }));
            listOfState.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 2 }, new JournalPM() { Tenant = 2, Id = "1-1" }));

            listOfState.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 1, CreditAccountId="1-1" }, new JournalPM() { Tenant = 2, Id = "1-1" }));

            listOfState.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 1 ,JournalId="1-3" }, new JournalPM() { Tenant = 1, Id = "1-1" }));
            listOfState.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 2, JournalId = "1-3" }, new JournalPM() { Tenant = 1, Id = "1-1" }));
            listOfState.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 2, JournalId = "1-3" }, new JournalPM() { Tenant = 2, Id = "1-1" }));


            listOfState.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 1, JournalId = "1-3" }, new JournalPM() { Tenant = 1, }));
            listOfState.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 2, JournalId = "1-3" }, new JournalPM() { Tenant = 1, }));
            listOfState.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 2, JournalId = "1-3" }, new JournalPM() { Tenant = 2, }));

            foreach (var item in listOfState)
            {
                TestsUtil.AssertThrows<Exception>(delegate
                {
                    // Arrange
                    var journalLineCreditMapping = new JournalLineCreditMapping(item.Item1, item.Item2, GetDefaultFakeGLAccountDataProvider());
                    // Act: Run the method under test:
                    journalLineCreditMapping.DoIt();

                },
                    // Assert: Verify the result:
                "if (journalLineKey !=journalKey )");
            }
        }


        [TestMethod]
        //[ExpectedException(typeof(Exception), "this.MyMappingTypeEnum != MappingTypeEnum.Debit")]
        public void journalLineCreditMappingDoIt_ActionTypeCodeIs2_ExpectedException()
        {
            int tenant = 1;
            string id = "1-1";
            //GLAccountTotalByMonthPM currGLAccountTotalByMounth, LedgerTransactionPM currLedgerTransaction

            var myState =
                Tuple.Create<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = tenant, JournalId = id, ActionTypeCode = /*Debit =*/ "2" }, new JournalPM { Tenant = 1, Id = id });

            TestsUtil.AssertThrows<Exception>(delegate
            {
                // Arrange
                var journalLineCreditMapping = new JournalLineCreditMapping(myState.Item1, myState.Item2, GetDefaultFakeGLAccountDataProvider());
                // Act: Run the method under test:
                journalLineCreditMapping.DoIt();

            },
                // Assert: Verify the result:
              expectedContainsMessage: "this.MyMappingTypeEnum != MappingTypeEnum.Debit");

        }

        [TestMethod]
        //[ExpectedException(typeof(Exception), "case MyJournalActionTypeEnum.NotValid")]
        public void journalLineCreditMappingDoIt_ActionTypeCodeIs3_ExpectedException()
        {

            // Arrange
            int tenant = 1;
            string id = "1-1";
            JournalLinePM journalLine = new JournalLinePM()
            {
                Tenant = tenant,
                JournalId = id,
                ActionTypeCode = "0",
                //ActionTypeCodeEnum = MyJournalActionTypeEnum.NotValid 
            };
            JournalPM journalPM = new JournalPM { Tenant = 1, Id = id };

            var journalLineCreditMapping = new JournalLineCreditMapping(journalLine, journalPM, GetDefaultFakeGLAccountDataProvider());


            TestsUtil.AssertThrows<Exception>(delegate
            {
                // Act: Run the method under test:
                journalLineCreditMapping.DoIt();

            },
                // Assert: Verify the result:
             expectedContainsMessage: "case MyJournalActionTypeEnum.NotValid");
        }
      
        
    
        


     


        #region DefaultMapLedgerTransactionPMFromJournal


        [TestMethod]
        public void journalLineCreditMappingDoItRangeCheck_InitBasicProperties_DefaultMapLedgerTransactionPMFromJournal()
        {

            var listOfState2Check = GetStateToCheckLedgerTransactionBasicPropertiesinheritedFromJournal();


            //using (ShimsContext.Create())
            {

                //ShimJournalLineMappingBase.AllInstances.GetGLAccountPMString = (myRepo, pId) =>
                //{
                //    return new GLAccountPM() { };
                //};
                foreach (var state2Check in listOfState2Check)
                {
                    var journalLineCreditMapping = new JournalLineCreditMapping(
                         journalLine: state2Check.Item2,
                          journalPM: state2Check.Item1,
                          myGLAccountPMProvider:GetDefaultFakeGLAccountDataProvider()
                          );
                    // Act: Run the method under test:
                    journalLineCreditMapping.DoIt();
                    // Assert: Verify the result:
                    var myLedgerTransaction = journalLineCreditMapping.MyLedgerTransaction;
                    LedgerTransactionBasicPropertiesinheritedFromJournalPostCondition(journalLine: state2Check.Item2, journalPM: state2Check.Item1, MyLedgerTransaction: myLedgerTransaction);

                }

            }

        }

        private static List<Tuple<JournalPM, JournalLinePM>> GetStateToCheckLedgerTransactionBasicPropertiesinheritedFromJournal()
        {
            int tenant = 1;
            string id = "1-1";
            decimal localAmount = 1;
            decimal foreignAmount = 3;

            int jline = 1;
            DateTime jCreateDate = new DateTime(2016, 10, 26).AddDays(-3);
            //GLAccountTotalByMonthPM currGLAccountTotalByMounth, LedgerTransactionPM currLedgerTransaction

            DateTime jAccountingDate = new DateTime(2016, 10, 26).AddDays(-2);
            DateTime jlDocumentDate = new DateTime(2016, 10, 26).AddDays(-4);
            var jlDueDate = jlDocumentDate.AddDays(1);
            string jlCurrencyId = "1-1";
            string jlReference1 = "Reference1";
            string jlReference2 = "Reference2";
            string jlReference3 = "Reference3";
            string jlNotes = "Notes";
            var listOfState = new List<Tuple<JournalPM, JournalLinePM>>();
            AddTestValue4LedgerTransactionBasicPropertiesinheritedFromJournal(tenant, id, localAmount, foreignAmount, jline, jCreateDate, jAccountingDate, jlDocumentDate, jlDueDate, jlCurrencyId, jlReference1, jlReference2, jlReference3, jlNotes, listOfState);
            tenant++;//change Tenant
            AddTestValue4LedgerTransactionBasicPropertiesinheritedFromJournal(tenant, id, localAmount, foreignAmount, jline, jCreateDate, jAccountingDate, jlDocumentDate, jlDueDate, jlCurrencyId, jlReference1, jlReference2, jlReference3, jlNotes, listOfState);
            id = "3-44";//change Tenant
            AddTestValue4LedgerTransactionBasicPropertiesinheritedFromJournal(tenant, id, localAmount, foreignAmount, jline, jCreateDate, jAccountingDate, jlDocumentDate, jlDueDate, jlCurrencyId, jlReference1, jlReference2, jlReference3, jlNotes, listOfState);

            localAmount = 0;//change Tenant
            AddTestValue4LedgerTransactionBasicPropertiesinheritedFromJournal(tenant, id, localAmount, foreignAmount, jline, jCreateDate, jAccountingDate, jlDocumentDate, jlDueDate, jlCurrencyId, jlReference1, jlReference2, jlReference3, jlNotes, listOfState);

            localAmount = 1000;//change Tenant
            AddTestValue4LedgerTransactionBasicPropertiesinheritedFromJournal(tenant, id, localAmount, foreignAmount, jline, jCreateDate, jAccountingDate, jlDocumentDate, jlDueDate, jlCurrencyId, jlReference1, jlReference2, jlReference3, jlNotes, listOfState);

            localAmount = 50;//change Tenant
            AddTestValue4LedgerTransactionBasicPropertiesinheritedFromJournal(tenant, id, localAmount, foreignAmount, jline, jCreateDate, jAccountingDate, jlDocumentDate, jlDueDate, jlCurrencyId, jlReference1, jlReference2, jlReference3, jlNotes, listOfState);

            foreignAmount = 50;//change Tenant
            AddTestValue4LedgerTransactionBasicPropertiesinheritedFromJournal(tenant, id, localAmount, foreignAmount, jline, jCreateDate, jAccountingDate, jlDocumentDate, jlDueDate, jlCurrencyId, jlReference1, jlReference2, jlReference3, jlNotes, listOfState);
            foreignAmount = -100;//change Tenant
            AddTestValue4LedgerTransactionBasicPropertiesinheritedFromJournal(tenant, id, localAmount, foreignAmount, jline, jCreateDate, jAccountingDate, jlDocumentDate, jlDueDate, jlCurrencyId, jlReference1, jlReference2, jlReference3, jlNotes, listOfState);

            jline++;//change Tenant
            AddTestValue4LedgerTransactionBasicPropertiesinheritedFromJournal(tenant, id, localAmount, foreignAmount, jline, jCreateDate, jAccountingDate, jlDocumentDate, jlDueDate, jlCurrencyId, jlReference1, jlReference2, jlReference3, jlNotes, listOfState);
            jCreateDate = jCreateDate.AddDays(-7);//change 
            AddTestValue4LedgerTransactionBasicPropertiesinheritedFromJournal(tenant, id, localAmount, foreignAmount, jline, jCreateDate, jAccountingDate, jlDocumentDate, jlDueDate, jlCurrencyId, jlReference1, jlReference2, jlReference3, jlNotes, listOfState);

            jCreateDate = jCreateDate.AddDays(-40);//change Tenant
            AddTestValue4LedgerTransactionBasicPropertiesinheritedFromJournal(tenant, id, localAmount, foreignAmount, jline, jCreateDate, jAccountingDate, jlDocumentDate, jlDueDate, jlCurrencyId, jlReference1, jlReference2, jlReference3, jlNotes, listOfState);
            jAccountingDate = jAccountingDate.AddDays(-40);//change Tenant
            AddTestValue4LedgerTransactionBasicPropertiesinheritedFromJournal(tenant, id, localAmount, foreignAmount, jline, jCreateDate, jAccountingDate, jlDocumentDate, jlDueDate, jlCurrencyId, jlReference1, jlReference2, jlReference3, jlNotes, listOfState);

            jlDocumentDate = jlDocumentDate.AddDays(-40);//change 
            AddTestValue4LedgerTransactionBasicPropertiesinheritedFromJournal(tenant, id, localAmount, foreignAmount, jline, jCreateDate, jAccountingDate, jlDocumentDate, jlDueDate, jlCurrencyId, jlReference1, jlReference2, jlReference3, jlNotes, listOfState);

            jlDueDate = jlDueDate.AddDays(-40);//change 
            AddTestValue4LedgerTransactionBasicPropertiesinheritedFromJournal(tenant, id, localAmount, foreignAmount, jline, jCreateDate, jAccountingDate, jlDocumentDate, jlDueDate, jlCurrencyId, jlReference1, jlReference2, jlReference3, jlNotes, listOfState);


            jlCurrencyId = "1-5";//change 
            AddTestValue4LedgerTransactionBasicPropertiesinheritedFromJournal(tenant, id, localAmount, foreignAmount, jline, jCreateDate, jAccountingDate, jlDocumentDate, jlDueDate, jlCurrencyId, jlReference1, jlReference2, jlReference3, jlNotes, listOfState);


            jlReference1 += "11";//change 
            AddTestValue4LedgerTransactionBasicPropertiesinheritedFromJournal(tenant, id, localAmount, foreignAmount, jline, jCreateDate, jAccountingDate, jlDocumentDate, jlDueDate, jlCurrencyId, jlReference1, jlReference2, jlReference3, jlNotes, listOfState);

            jlReference2 += "22";//change 
            AddTestValue4LedgerTransactionBasicPropertiesinheritedFromJournal(tenant, id, localAmount, foreignAmount, jline, jCreateDate, jAccountingDate, jlDocumentDate, jlDueDate, jlCurrencyId, jlReference1, jlReference2, jlReference3, jlNotes, listOfState);

            jlReference3 += "33";//change 
            AddTestValue4LedgerTransactionBasicPropertiesinheritedFromJournal(tenant, id, localAmount, foreignAmount, jline, jCreateDate, jAccountingDate, jlDocumentDate, jlDueDate, jlCurrencyId, jlReference1, jlReference2, jlReference3, jlNotes, listOfState);

            jlNotes += "nnn";//change 
            AddTestValue4LedgerTransactionBasicPropertiesinheritedFromJournal(tenant, id, localAmount, foreignAmount, jline, jCreateDate, jAccountingDate, jlDocumentDate, jlDueDate, jlCurrencyId, jlReference1, jlReference2, jlReference3, jlNotes, listOfState);

            jlNotes += "nnn";//change 
            AddTestValue4LedgerTransactionBasicPropertiesinheritedFromJournal(tenant, id, localAmount, foreignAmount, jline, jCreateDate, jAccountingDate, jlDocumentDate, jlDueDate, jlCurrencyId, jlReference1, jlReference2, jlReference3, jlNotes, listOfState);
            return listOfState;
        }

        private static void AddTestValue4LedgerTransactionBasicPropertiesinheritedFromJournal(int tenant, string id, decimal localAmount, decimal foreignAmount, int jline, DateTime jCreateDate, DateTime jAccountingDate, DateTime jlDocumentDate, DateTime jlDueDate, string jlCurrencyId, string jlReference1, string jlReference2, string jlReference3, string jlNotes, List<Tuple<JournalPM, JournalLinePM>> listOfState)
        {
            listOfState.Add(new Tuple<JournalPM, JournalLinePM>(
                new JournalPM { Tenant = tenant, Id = id, CreateDate = jCreateDate, AccountingDate = jAccountingDate, },
                new JournalLinePM()
                {
                    Tenant = tenant,
                    JournalId = id,
                    ActionTypeCode = ((int)MyJournalActionTypeEnum.Credit).ToString(),
                    AccountingDate = jAccountingDate,
                    DueDate = jlDueDate,
                    LocalAmount = localAmount,
                    ForeignAmount = foreignAmount,
                    Line = jline,
                    DocumentDate = jlDocumentDate,
                    CurrencyId = jlCurrencyId,
                    Reference1 = jlReference1,
                    Reference2 = jlReference2,
                    Reference3 = jlReference3,
                    Notes = jlNotes,

                }));
        }

        private static void LedgerTransactionBasicPropertiesinheritedFromJournalPostCondition(JournalLinePM journalLine, JournalPM journalPM, 
            LedgerTransactionPM MyLedgerTransaction)
        {
            Assert.AreEqual(journalPM.Tenant, MyLedgerTransaction.Tenant,
                "MyLedgerTransaction.Tenant get fromjournalPM");
            Assert.AreEqual(journalPM.Id, MyLedgerTransaction.JournalId,
                "MyLedgerTransaction.JournalId = _JournalLine.JournalId;");

            Assert.AreEqual(journalPM.CreateDate, MyLedgerTransaction.CreateDate,
        "MyLedgerTransaction.CreateDate = _JournalPM.CreateDate;");



            Assert.AreEqual(journalPM.AccountingDate.Date.Year, MyLedgerTransaction.AccountingDate.Year,
        "MyLedgerTransaction.AccountingDate.Date.Year = _JournalPM.AccountingDate.Date.Year;");

            Assert.AreEqual(journalPM.AccountingDate.Date.Month, MyLedgerTransaction.AccountingDate.Month,
        "MyLedgerTransaction.AccountingDate.Date.Month = _JournalPM.AccountingDate.Date.Month;");


            Assert.AreEqual(journalLine.AccountingDate.Date, MyLedgerTransaction.AccountingDate,
"MyLedgerTransaction.AccountingDate = journalLine.AccountingDate.Date");

            Assert.AreEqual(journalLine.Line, MyLedgerTransaction.JournalLineNumber,
                "MyLedgerTransaction.Line = _JournalLine.Line;");

            Assert.AreEqual(journalLine.DocumentDate, MyLedgerTransaction.DocumentDate,
                "MyLedgerTransaction.DocumentDate= _JournalLine.DocumentDate;");

            Assert.AreEqual(journalLine.DueDate, MyLedgerTransaction.DueDate,
                "MyLedgerTransaction.DueDate = (DateTime)_JournalLine.DueDate;");


            Assert.AreEqual(journalLine.CurrencyId, MyLedgerTransaction.CurrencyId,
                "MyLedgerTransaction.CurrencyId = _JournalLine.CurrencyId;");

            Assert.AreEqual(journalLine.Reference1, MyLedgerTransaction.Reference1,
                "MyLedgerTransaction.Reference1 = _JournalLine.Reference1;");
            Assert.AreEqual(journalLine.Reference2, MyLedgerTransaction.Reference2,
                "MyLedgerTransaction.Reference2 = _JournalLine.Reference2;");

            Assert.AreEqual(journalLine.Reference3, MyLedgerTransaction.Reference3,
                "MyLedgerTransaction.Reference3 = _JournalLine.Reference3;");
        }

        #endregion


        #region JournalLineCreditMapping.MapIt
        [TestMethod]
        public void journalLineCreditMappingDoItRangeCheck_InitBasicProperties_JournalLineCreditMappingMapIt()
        {
            //MyLedgerTransaction.AccountId = _JournalLine.CreditAccountId;
            //MyLedgerTransaction.OppositeAccountId = _JournalLine.DebitAccountId;
            //MyLedgerTransaction.LocalAmountDebit = 0;
            //MyLedgerTransaction.LocalAmountCredit = (decimal)_JournalLine.LocalAmount;
            //MyLedgerTransaction.CurrencyId = _JournalLine.CurrencyId;
            //MyLedgerTransaction.ForeignAmountDebit = 0;
            //MyLedgerTransaction.ForeignAmountCredit = (decimal)_JournalLine.ForeignAmount;


            var listOfState2Check = GetState2Check_JournalLineCreditMappingMapIt();


            //using (ShimsContext.Create())
            {

                //ShimJournalLineMappingBase.AllInstances.GetGLAccountPMString = (myRepo, pId) =>
                //{
                //    return new GLAccountPM() { };
                //};
                int i = 0;
                foreach (var state2Check in listOfState2Check)
                {
                    var journalLineCreditMapping = new JournalLineCreditMapping(
                         journalLine: state2Check.Item2,
                          journalPM: state2Check.Item1,
                          myGLAccountPMProvider:GetDefaultFakeGLAccountDataProvider());
                    // Act: Run the method under test:
                    journalLineCreditMapping.DoIt();
                    // Assert: Verify the result:
                    var myLedgerTransaction = journalLineCreditMapping.MyLedgerTransaction;
                    JournalLineCreditMappingMapItPostCondition(journalLine: state2Check.Item2, journalPM: state2Check.Item1, MyLedgerTransaction: myLedgerTransaction);
                    i++;

                }

            }

        }

        private void JournalLineCreditMappingMapItPostCondition(JournalLinePM journalLine, JournalPM journalPM, LedgerTransactionPM MyLedgerTransaction)
        {
            const string lineat="JournalLineCreditMappingMapIt";
            LedgerTransactionBasicPropertiesinheritedFromJournalPostCondition(journalLine, journalPM,
            MyLedgerTransaction);

            //MyLedgerTransaction.AccountId = _JournalLine.CreditAccountId;
            Assert.AreEqual(journalLine.CreditAccountId, MyLedgerTransaction.AccountId,
            lineat);
            //MyLedgerTransaction.OppositeAccountId = _JournalLine.DebitAccountId;
            Assert.AreEqual(journalLine.DebitAccountId, MyLedgerTransaction.OppositeAccountId,
            lineat);
            //MyLedgerTransaction.LocalAmountDebit = 0;
            Assert.AreEqual(0, MyLedgerTransaction.LocalAmountDebit,
            lineat);
            //MyLedgerTransaction.LocalAmountCredit = (decimal)_JournalLine.LocalAmount;
            var  roundLocalAmount= System.Math.Round((decimal)journalLine.LocalAmount, 2);
            Assert.AreEqual(roundLocalAmount, MyLedgerTransaction.LocalAmountCredit,
            lineat);
            //MyLedgerTransaction.CurrencyId = _JournalLine.CurrencyId;
            Assert.AreEqual(journalLine.CurrencyId, MyLedgerTransaction.CurrencyId,
            lineat);
            //MyLedgerTransaction.ForeignAmountDebit = 0;
            Assert.AreEqual(0, MyLedgerTransaction.ForeignAmountDebit,
            lineat);
            //MyLedgerTransaction.ForeignAmountCredit = (decimal)_JournalLine.ForeignAmount;
            var roundedFA = System.Math.Round((decimal)journalLine.ForeignAmount, 2);
            Assert.AreEqual(roundedFA, MyLedgerTransaction.ForeignAmountCredit,
            lineat);

            decimal ExchangeRate;
            if (journalLine.ExchangeRate != null)
            {
                ExchangeRate = (decimal)journalLine.ExchangeRate;
            }
            else
            {
                ExchangeRate = (decimal)journalLine.LocalAmount / (decimal)journalLine.ForeignAmount;
                //ExchangeRate = roundLocalAmount/ roundedFA;
            }
            Assert.AreEqual(ExchangeRate, MyLedgerTransaction.ExchangeRate,
            lineat);
        
        }

        private List<Tuple<JournalPM, JournalLinePM>> GetState2Check_JournalLineCreditMappingMapIt()
        {

            var listOfState = new List<Tuple<JournalPM, JournalLinePM>>();


            //JournalLineCreditMappingMapIt Check !!!

            //MyLedgerTransaction.AccountId = _JournalLine.CreditAccountId;
            string jlCreditAccountId = "C1-1";
            //MyLedgerTransaction.OppositeAccountId = _JournalLine.DebitAccountId;
            string jlDebitAccountId = "D1-1";
            //MyLedgerTransaction.LocalAmountDebit = 0;

            //MyLedgerTransaction.LocalAmountCredit = (decimal)_JournalLine.LocalAmount;
            decimal jlLocalAmount = 400;

            //MyLedgerTransaction.ForeignAmountDebit = 0;
            //MyLedgerTransaction.ForeignAmountCredit = (decimal)_JournalLine.ForeignAmount;
            decimal jlForeignAmount = 100;
            jlLocalAmount = -33.444m;
            listOfState.Add(NewStateJournalLineCreditMappingMapIt(jlCreditAccountId, jlDebitAccountId, jlLocalAmount, jlForeignAmount));
            jlForeignAmount = 1;
            listOfState.Add(NewStateJournalLineCreditMappingMapIt(jlCreditAccountId, jlDebitAccountId, jlLocalAmount, jlForeignAmount));
            jlForeignAmount = -11;
            listOfState.Add(NewStateJournalLineCreditMappingMapIt(jlCreditAccountId, jlDebitAccountId, jlLocalAmount, jlForeignAmount));
            jlCreditAccountId = jlCreditAccountId + "c";
            listOfState.Add(NewStateJournalLineCreditMappingMapIt(jlCreditAccountId, jlDebitAccountId, jlLocalAmount, jlForeignAmount));
            jlCreditAccountId = jlCreditAccountId + "D";
            listOfState.Add(NewStateJournalLineCreditMappingMapIt(jlCreditAccountId, jlDebitAccountId, jlLocalAmount, jlForeignAmount));
            jlLocalAmount = 1000.99m;
            listOfState.Add(NewStateJournalLineCreditMappingMapIt(jlCreditAccountId, jlDebitAccountId, jlLocalAmount, jlForeignAmount));
            jlLocalAmount = -33.444m;
            listOfState.Add(NewStateJournalLineCreditMappingMapIt(jlCreditAccountId, jlDebitAccountId, jlLocalAmount, jlForeignAmount));
            jlLocalAmount = -0.133444m;
            listOfState.Add(NewStateJournalLineCreditMappingMapIt(jlCreditAccountId, jlDebitAccountId, jlLocalAmount, jlForeignAmount));
            jlDebitAccountId = jlDebitAccountId + "ff";
            listOfState.Add(NewStateJournalLineCreditMappingMapIt(jlCreditAccountId, jlDebitAccountId, jlLocalAmount, jlForeignAmount));


            return listOfState;
        }

        private static Tuple<JournalPM, JournalLinePM> NewStateJournalLineCreditMappingMapIt(string jlCreditAccountId ,string jlDebitAccountId ,
            decimal jlLocalAmount, decimal jlForeignAmount)
        {
            int tenant = 1;
            string id = "1-1";
             

            int jline = 1;
            DateTime jCreateDate = new DateTime(2016, 10, 26).AddDays(-3);
            //GLAccountTotalByMonthPM currGLAccountTotalByMounth, LedgerTransactionPM currLedgerTransaction

            DateTime jAccountingDate = new DateTime(2016, 10, 26).AddDays(-2);
            DateTime jlAccountingDate = new DateTime(2016, 10, 26).AddDays(-3);
            DateTime jlDocumentDate = new DateTime(2016, 10, 26).AddDays(-4);
            var jlDueDate = jlDocumentDate.AddDays(1);
            string jlCurrencyId = "1-1";
            string jlReference1 = "Reference1";
            string jlReference2 = "Reference2";
            string jlReference3 = "Reference3";
            string jlNotes = "Notes";
            return new Tuple<JournalPM, JournalLinePM>(
               new JournalPM
               {
                   Tenant = tenant,
                   Id = id,
                   CreateDate = jCreateDate,
                   AccountingDate = jAccountingDate,

               },
               new JournalLinePM()
               {
                   Tenant = tenant,
                   JournalId = id,
                   ActionTypeCode = ((int)MyJournalActionTypeEnum.Credit).ToString(),
                   DueDate = jlDueDate,
                   AccountingDate= jlAccountingDate,
                   Line = jline,
                   DocumentDate = jlDocumentDate,
                   CurrencyId = jlCurrencyId,
                   Reference1 = jlReference1,
                   Reference2 = jlReference2,
                   Reference3 = jlReference3,
                   Notes = jlNotes,


                   CreditAccountId= jlCreditAccountId,
                   DebitAccountId = jlDebitAccountId,
                   LocalAmount = jlLocalAmount,
                   ForeignAmount = jlForeignAmount,

               });
        }



        #endregion



        #region CheckParentAccount
        


        [TestMethod]
        public void journalLineCreditMappingDoIt_ParentAccountIsCard_ControlAccountIdIsNull()
        {


            var listOfState2Check = new List<Tuple<JournalLinePM, JournalPM, string>>();



            listOfState2Check.Add(new Tuple<JournalLinePM, JournalPM, string>(
                new JournalLinePM()
                {
                    Tenant = 1,
                    JournalId = "1-1",
                    ActionTypeCode = ((int)MyJournalActionTypeEnum.Credit).ToString(),
                    DueDate = new DateTime(2016, 3, 3),
                    AccountingDate = new DateTime(2016, 3, 3),
                    LocalAmount = 1,
                    ForeignAmount = 1,


                    CreditAccountId = "1-1",

                },
                new JournalPM() { Tenant = 1, Id = "1-1" },
                "CreditAccountId is 1-1 Will Get myGLAccountTypeEnumCard"
                ));
            listOfState2Check.Add(new Tuple<JournalLinePM, JournalPM, string>(
                new JournalLinePM()
                {
                    Tenant = 1,
                    JournalId = "1-1",
                    ActionTypeCode = ((int)MyJournalActionTypeEnum.Credit).ToString(),
                    DueDate = new DateTime(2016, 3, 3),
                    AccountingDate = new DateTime(2016, 3, 3),
                    LocalAmount = 1,
                    ForeignAmount = 1,

                    CreditAccountId = "1-3",

                },
                new JournalPM() { Tenant = 1, Id = "1-1" },
                "CreditAccountId is null "
                ));


            var myGLAccountTypeEnumCard = ((int)GLAccountTypePM.GLAccountTypeEnum.Card).ToString();
            //using (ShimsContext.Create())
            {
                var myfun = new Func<FakeItEasy.Core.IFakeObjectCall, GLAccountPM> (
                //ShimJournalLineMappingBase.AllInstances.GetGLAccountPMString 
                //    = (myRepo, pId) =>
                (ObjectCall)=>
                {
                    string pId = ObjectCall.Arguments[0] as string;
                    if (pId == "1-3") return new GLAccountPM() { AccountTypeCode = null, ControlAccountId = "1-0" };
                    return new GLAccountPM() { AccountTypeCode = myGLAccountTypeEnumCard, ControlAccountId = "1-0" };
                });
                int i = 0;
                foreach (var state2Check in listOfState2Check)
                {
                    Debug.WriteLine(state2Check.Item3);
                    var journalLineCreditMapping = new JournalLineCreditMapping(
                         journalLine: state2Check.Item1,
                          journalPM: state2Check.Item2
                          , myGLAccountPMProvider:GetFakeGLAccountDataProvider(myfun));
                    // Act: Run the method under test:
                    journalLineCreditMapping.DoIt();
                    // Assert: Verify the result:
                    var myLedgerTransaction = journalLineCreditMapping.MyLedgerTransaction;

                    Assert.AreEqual(null, myLedgerTransaction.ControlAccountId);
                    i++;

                }

            }
        }



        [TestMethod]
        public void journalLineCreditMappingDoItRange_ParentAccountIsNotCard_ControlAccountIdIsGetCreditparentAccount()
        {
            var expectedControlAccountId = "expectedControlAccountId";

            var listOfState2Check = new List<Tuple<JournalLinePM, JournalPM, string>>();


            listOfState2Check.Add(new Tuple<JournalLinePM, JournalPM, string>(
                new JournalLinePM()
                {
                    Tenant = 1,
                    JournalId = "1-1",
                    ActionTypeCode = ((int)MyJournalActionTypeEnum.Credit).ToString(),
                    DueDate = new DateTime(2016, 3, 3),
                    AccountingDate = new DateTime(2016, 3, 3),
                    LocalAmount = 1,
                    ForeignAmount = 1,

                    
                    CreditAccountId = "Client",
                    CreditControlAccountId = expectedControlAccountId ,

                },
                new JournalPM() { Tenant = 1, Id = "1-1" },
                "CreditAccountId  type is Client"
                ));



            listOfState2Check.Add(new Tuple<JournalLinePM, JournalPM, string>(
                new JournalLinePM()
                {
                    Tenant = 1,
                    JournalId = "1-1",
                    ActionTypeCode = ((int)MyJournalActionTypeEnum.Credit).ToString(),
                    DueDate = new DateTime(2016, 3, 3),
                    AccountingDate = new DateTime(2016, 3, 3),
                    LocalAmount = 1,
                    ForeignAmount = 1,

                    CreditAccountId = "Vendor",
                    CreditControlAccountId = expectedControlAccountId,

                },
                new JournalPM() { Tenant = 1, Id = "1-1" },
                "CreditAccountId  type is Vendor"
                ));



            listOfState2Check.Add(new Tuple<JournalLinePM, JournalPM, string>(
                new JournalLinePM()
                {
                    Tenant = 1,
                    JournalId = "1-1",
                    ActionTypeCode = ((int)MyJournalActionTypeEnum.Credit).ToString(),
                    DueDate = new DateTime(2016, 3, 3),
                    AccountingDate = new DateTime(2016, 3, 3),
                    LocalAmount = 1,
                    ForeignAmount = 1,

                    CreditAccountId = "Job",
                    CreditControlAccountId = expectedControlAccountId,

                },
                new JournalPM() { Tenant = 1, Id = "1-1" },
                "CreditAccountId  type is Job"
                ));

            listOfState2Check.Add(new Tuple<JournalLinePM, JournalPM, string>(
                new JournalLinePM()
                {
                    Tenant = 1,
                    JournalId = "1-1",
                    ActionTypeCode = ((int)MyJournalActionTypeEnum.Credit).ToString(),
                    DueDate = new DateTime(2016, 3, 3),
                    AccountingDate = new DateTime(2016, 3, 3),
                    LocalAmount = 1,
                    ForeignAmount = 1,

                    CreditAccountId = "File",
                    CreditControlAccountId = expectedControlAccountId,

                },
                new JournalPM() { Tenant = 1, Id = "1-1" },
                "CreditAccountId  type is File"
                ));

            var myGLAccountTypeEnumCard = ((int)GLAccountTypePM.GLAccountTypeEnum.Card).ToString();
            
            //using (ShimsContext.Create())
            {
                Func<FakeItEasy.Core.IFakeObjectCall, GLAccountPM> myFunc = new Func<FakeItEasy.Core.IFakeObjectCall,GLAccountPM>(
                //ShimJournalLineMappingBase.AllInstances.GetGLAccountPMString = (myRepo, pId) =>
                (ObjectCall) =>
                {
                    var pId = ObjectCall.Arguments[0] as string;
                    myGLAccountTypeEnumCard = TranslateGLAccounntTypeIdToEnum(pId);
                    return new GLAccountPM()
                    {
                        Id = pId,
                        AccountTypeCode = myGLAccountTypeEnumCard,
                        ControlAccountId = expectedControlAccountId
                    };
                });
                
                int i = 0;
                foreach (var state2Check in listOfState2Check)
                {
                    Debug.WriteLine("state Test: " + state2Check.Item3);
                    var journalLineCreditMapping = new JournalLineCreditMapping(
                         journalLine: state2Check.Item1,
                          journalPM: state2Check.Item2,
                          myGLAccountPMProvider:GetFakeGLAccountDataProvider(myFunc));
                    // Act: Run the method under test:
                    journalLineCreditMapping.DoIt();
                    // Assert: Verify the result:
                    var myLedgerTransaction = journalLineCreditMapping.MyLedgerTransaction;

                    Assert.AreEqual(expectedControlAccountId, myLedgerTransaction.ControlAccountId);
                    i++;

                }

            }
        }

        private static string TranslateGLAccounntTypeIdToEnum(string pId)
        {
            string myGLAccountTypeEnumCard="";
            switch (pId)
            {
                //Card=1,
                case "Client":
                    myGLAccountTypeEnumCard = "2";
                    break;
                case "Vendor":
                    myGLAccountTypeEnumCard = "3";
                    break;
                case "Job":
                    myGLAccountTypeEnumCard = "4";
                    break;
                case "File":
                    myGLAccountTypeEnumCard = "5";
                    break;



                default:
                    break;
            }
            return myGLAccountTypeEnumCard;
        }



        [TestMethod]
        public void journalLineCreditMappingDoItRange_ParentAccountReconcileMethodCodeISLocalCurrency_LedgerTransactionOpenAmountNisOpenAmount()
        {
            var myGLAccountTypeEnumCard = ((int)GLAccountTypePM.GLAccountTypeEnum.Card).ToString();
            var expectedControlAccountId = "expectedControlAccountId";
            var ExpectedOpenAmountCurrencyId = "NIS";
            var listOfState2Check = new List<Tuple<JournalLinePM, JournalPM, string>>();


            listOfState2Check.Add(new Tuple<JournalLinePM, JournalPM, string>(
                new JournalLinePM()
                {
                    Tenant = 1,
                    JournalId = "1-1",
                    ActionTypeCode = ((int)MyJournalActionTypeEnum.Credit).ToString(),
                    DueDate = new DateTime(2016, 3, 3),
                    AccountingDate = new DateTime(2016, 3, 3),
                    LocalAmount = 11,
                    ForeignAmount = 1,


                    CreditAccountId = "Client",
                    CreditControlAccountId = expectedControlAccountId,
                },
                new JournalPM() { Tenant = 1, Id = "1-1" },
                "Check Local Amount"
                ));



            listOfState2Check.Add(new Tuple<JournalLinePM, JournalPM, string>(
                new JournalLinePM()
                {
                    Tenant = 1,
                    JournalId = "1-1",
                    ActionTypeCode = ((int)MyJournalActionTypeEnum.Credit).ToString(),
                    DueDate = new DateTime(2016, 3, 3),
                    AccountingDate = new DateTime(2016, 3, 3),
                    LocalAmount = 22.54353m,
                    ForeignAmount = 1,

                    CreditAccountId = "Vendor",
                    CreditControlAccountId = expectedControlAccountId,

                },
                new JournalPM() { Tenant = 1, Id = "1-1" },
                "Check Local Amount"
                ));



            listOfState2Check.Add(new Tuple<JournalLinePM, JournalPM, string>(
                new JournalLinePM()
                {
                    Tenant = 1,
                    JournalId = "1-1",
                    ActionTypeCode = ((int)MyJournalActionTypeEnum.Credit).ToString(),
                    DueDate = new DateTime(2016, 3, 3),
                    AccountingDate = new DateTime(2016, 3, 3),
                    LocalAmount = 231.44m,
                    ForeignAmount = 1,
                    CreditControlAccountId = expectedControlAccountId,
                    CreditAccountId = "Job",

                },
                new JournalPM() { Tenant = 1, Id = "1-1" },
                "Check Local Amount"
                ));

            listOfState2Check.Add(new Tuple<JournalLinePM, JournalPM, string>(
                new JournalLinePM()
                {
                    Tenant = 1,
                    JournalId = "1-1",
                    ActionTypeCode = ((int)MyJournalActionTypeEnum.Credit).ToString(),
                    DueDate = new DateTime(2016, 3, 3),
                    AccountingDate = new DateTime(2016, 3, 3),
                    LocalAmount = -4531.55m,
                    ForeignAmount = 1,

                    CreditAccountId = "File",
                    CreditControlAccountId = expectedControlAccountId,
                    IsExternalReconcile = true,
                },
                new JournalPM() { Tenant = 1, Id = "1-1" },
                "Check Local Amount"
                ));
          
            
            //using (ShimsContext.Create())
            {
                

                int i = 0;
                foreach (var state2Check in listOfState2Check)
                {
                    Debug.WriteLine("state Test: " + state2Check.Item3);
                    
                    var fakeJournalLineCreditMapping = CreateFake(state2Check);

                    Fake4GetGLAccountPM(fakeJournalLineCreditMapping, expectedControlAccountId, myGLAccountTypeEnumCard,
                        ReconcileMethodPM.ReconcileMethodEnum.LocalCurrency);
                    Fake4ResolveAccountingCurrencyId(fakeJournalLineCreditMapping,"NIS");

                    // Act: Run the method under test:
                    fakeJournalLineCreditMapping.DoIt();
                    // Assert: Verify the result:
                    var myLedgerTransaction = fakeJournalLineCreditMapping.MyLedgerTransaction;

                    Assert.AreEqual(Math.Round(-1* state2Check.Item1.LocalAmount,2), myLedgerTransaction.OpenAmount);
                    Assert.AreEqual(state2Check.Item1.IsExternalReconcile, myLedgerTransaction.IsExternalReconcile);
                    if (ExpectedOpenAmountCurrencyId != myLedgerTransaction.OpenAmountCurrencyId)
                    {
                    }
                    Assert.AreEqual(ExpectedOpenAmountCurrencyId , myLedgerTransaction.OpenAmountCurrencyId);
                    i++;

                }

            }
        }

        private static void Fake4ResolveAccountingCurrencyId(JournalLineCreditMapping fakeJournalLineCreditMapping,string currencyId)
        {
            A.CallTo(fakeJournalLineCreditMapping).Where(c => c.Method.Name == "ResolveAccountingCurrencyId")
                .WithReturnType<string>()
                .Returns(currencyId);
        }

       



        [TestMethod]
        public void journalLineCreditMappingDoItRange_ParentAccountReconcileMethodCodeISForeignCurrency_LedgerTransactionOpenAmountForeignAmount()
        {

            var expectedControlAccountId = "expectedControlAccountId";
            var listOfState2Check = new List<Tuple<JournalLinePM, JournalPM, string>>();
            
            listOfState2Check.Add(new Tuple<JournalLinePM, JournalPM, string>(
                new JournalLinePM()
                {
                    Tenant = 1,
                    JournalId = "1-1",
                    ActionTypeCode = ((int)MyJournalActionTypeEnum.Credit).ToString(),
                    DueDate = new DateTime(2016, 3, 3),
                    AccountingDate = new DateTime(2016, 3, 3),
                    LocalAmount = 11,
                    ForeignAmount = 13121.9999m,
                    CurrencyId = null,

                    CreditAccountId = "Client",
                    CreditControlAccountId = expectedControlAccountId,

                },
                new JournalPM() { Tenant = 1, Id = "1-1" },
                "Check ForeignAmount"
                ));



            listOfState2Check.Add(new Tuple<JournalLinePM, JournalPM, string>(
                new JournalLinePM()
                {
                    Tenant = 1,
                    JournalId = "1-1",
                    ActionTypeCode = ((int)MyJournalActionTypeEnum.Credit).ToString(),
                    DueDate = new DateTime(2016, 3, 3),
                    AccountingDate = new DateTime(2016, 3, 3),
                    LocalAmount = 22.54353m,
                    ForeignAmount = 1322.4355m,
                    CurrencyId = "NIS",
                    CreditAccountId = "Vendor",
                    CreditControlAccountId = expectedControlAccountId,

                },
                new JournalPM() { Tenant = 1, Id = "1-1" },
                "Check ForeignAmount"
                ));



            listOfState2Check.Add(new Tuple<JournalLinePM, JournalPM, string>(
                new JournalLinePM()
                {
                    Tenant = 1,
                    JournalId = "1-1",
                    ActionTypeCode = ((int)MyJournalActionTypeEnum.Credit).ToString(),
                    DueDate = new DateTime(2016, 3, 3),
                    AccountingDate = new DateTime(2016, 3, 3),
                    LocalAmount = 231.44m,
                    ForeignAmount = 1234.9m,
                    CurrencyId = "EUR",
                    CreditAccountId = "Job",
                    CreditControlAccountId = expectedControlAccountId,

                },
                new JournalPM() { Tenant = 1, Id = "1-1" },
                "Check ForeignAmount"
                ));

            listOfState2Check.Add(new Tuple<JournalLinePM, JournalPM, string>(
                new JournalLinePM()
                {
                    Tenant = 1,
                    JournalId = "1-1",
                    ActionTypeCode = ((int)MyJournalActionTypeEnum.Credit).ToString(),
                    DueDate = new DateTime(2016, 3, 3),
                    AccountingDate = new DateTime(2016, 3, 3),
                    LocalAmount = -4531.55m,
                    ForeignAmount = -2341.533m,


                    CurrencyId="USD" ,
                    CreditAccountId = "File",
                    CreditControlAccountId = expectedControlAccountId,

                },
                new JournalPM() { Tenant = 1, Id = "1-1" },
                "Check ForeignAmount"
                ));
            
            var myGLAccountTypeEnumCard = ((int)GLAccountTypePM.GLAccountTypeEnum.Card).ToString();


            
            {
                
                int i = 0;
                foreach (var state2Check in listOfState2Check)
                {
                    Debug.WriteLine("state Test: " + state2Check.Item3);

                    var fakeJournalLineCreditMapping = CreateFake(state2Check);

                    Fake4GetGLAccountPM(fakeJournalLineCreditMapping,expectedControlAccountId, myGLAccountTypeEnumCard , 
                        ReconcileMethodPM.ReconcileMethodEnum.ForeignCurrency);
                    A.CallTo(() => fakeJournalLineCreditMapping.ResolveAccountingCurrencyId(1))
                        .Throws((() => new 
                            Exception("Due ReconcileMethodPM.ReconcileMethodEnum.ForeignCurrency !! I Dont Need to call ResolveAccountingCurrencyId")));
                        
                    // Act: Run the method under test:
                    fakeJournalLineCreditMapping.DoIt();
                    // Assert: Verify the result:
                    var myLedgerTransaction = fakeJournalLineCreditMapping.MyLedgerTransaction;

                    //Assert.AreNotEqual(-1 * state2Check.Item1.ForeignAmount, myLedgerTransaction.OpenAmount,"Roundit ");
                    Assert.AreEqual(Math.Round(-1 * state2Check.Item1.ForeignAmount,2), myLedgerTransaction.OpenAmount, "Roundit ");
                    
                    Assert.AreEqual(state2Check.Item1.CurrencyId, myLedgerTransaction.OpenAmountCurrencyId);
                    i++;

                }

            }
        }

        private static JournalLineCreditMapping CreateFake(Tuple<JournalLinePM, JournalPM, string> state2Check)
        {
            var journalLineCreditMapping =
                A.Fake<JournalLineCreditMapping>(
                opt => opt
                    .CallsBaseMethods()
                    .WithArgumentsForConstructor(
                    () => new JournalLineCreditMapping(
state2Check.Item1, state2Check.Item2,
null
                    )
                  ));
            return journalLineCreditMapping;
        }

        private static void Fake4GetGLAccountPM(JournalLineCreditMapping fakejournalLineCreditMapping ,
            string expectedControlAccountId, string myGLAccountTypeEnumCard ,
            ReconcileMethodPM.ReconcileMethodEnum ReconcileMethodEnum)
        {
            A.CallTo(fakejournalLineCreditMapping)
                    .Where(c => c.Method.Name == "GetGLAccountPM")
                    .WithReturnType<GLAccountPM>()
                    .ReturnsLazily(callObj =>
                    {
                        var AccountId = callObj.Arguments[0] as string;
                        return new GLAccountPM()
                        {
                            Id = AccountId,
                            AccountTypeCode = myGLAccountTypeEnumCard,
                            ControlAccountId = expectedControlAccountId,
                            ReconcileMethodCode = ((int)
                            //Logitude.Accounting.Def.EntityPMs.ReconcileMethodPM.ReconcileMethodEnum.ForeignCurrency
                            ReconcileMethodEnum
                            ).ToString()
                        };
                    }
            );
        }
        #endregion




  

        #region AddGLAccountTotalByMounth
    


        [TestMethod]
        public void journalLineCreditMappingDoItRangeCheck_InitBasicProperties_TotalByMounthREqual()
        {

            var listOfState2Check = GetStateToCheckLedgerTransactionBasicPropertiesinheritedFromJournal();


            //using (ShimsContext.Create())
            {

                //ShimJournalLineMappingBase.AllInstances.GetGLAccountPMString = (myRepo, pId) =>
                //{
                //    return new GLAccountPM() { };
                //};
                foreach (var state2Check in listOfState2Check)
                {
                    var journalLine = state2Check.Item2;
                    var journalPM = state2Check.Item1;
                    var journalLineCreditMapping = new JournalLineCreditMapping(
                         journalLine: state2Check.Item2,
                          journalPM: state2Check.Item1,
                          myGLAccountPMProvider: GetDefaultFakeGLAccountDataProvider()
                          );
                    // Act: Run the method under test:
                    journalLineCreditMapping.DoIt();
                    // Assert: Verify the result:
                    var myGLAccountTotalByMonth = journalLineCreditMapping.MyGLAccountTotalByMonth;//MyGLAccountTotalByMonthByAccountingDate;



                    Assert.AreEqual(journalPM.Tenant, myGLAccountTotalByMonth.Tenant);
                    Assert.AreEqual(journalPM.AccountingDate.Year, myGLAccountTotalByMonth.Year);
                    Assert.AreEqual(journalPM.AccountingDate.Month, myGLAccountTotalByMonth.Month);

                    Assert.AreEqual(journalLine.CreditAccountId, myGLAccountTotalByMonth.AccountId);
                    Assert.AreEqual(journalLine.CurrencyId, myGLAccountTotalByMonth.CurrencyId);


                    Assert.AreEqual(ToDbRound2(journalLine.LocalAmount), myGLAccountTotalByMonth.LocalAmountCredit);

                    Assert.AreEqual(ToDbRound2(journalLine.ForeignAmount), myGLAccountTotalByMonth.ForeignAmountCredit);

                    Assert.AreEqual(0, myGLAccountTotalByMonth.LocalAmountDebit);
                    Assert.AreEqual(0, myGLAccountTotalByMonth.ForeignAmountDebit);
                    

                    
                    

                }

            }

        }



        public static decimal? ToDbRound2(decimal? dec)
        {
            if (dec.HasValue)
            {
                decimal my = dec.Value;
                return (System.Math.Round(my, 2) as decimal?);
            }
            else
            {
                return null;
            }

        }

        #endregion

        private IGLAccountDataProvider GetDefaultFakeGLAccountDataProvider()
        {
            return FakeItEasy.A.Fake<IGLAccountDataProvider>();
        }

        private IGLAccountDataProvider GetFakeGLAccountDataProvider(string expectedControlAccountId,  string myGLAccountTypeEnumCard)
        {
            myGLAccountTypeEnumCard = "";

            var fake= FakeItEasy.A.Fake<IGLAccountDataProvider>();
            FakeItEasy.A
                .CallTo(() => fake.GetGLAccount(FakeItEasy.A<string>.Ignored, FakeItEasy.A<int>.Ignored))
                .ReturnsLazily(
                objectCall =>
                {
                    string pId = objectCall.Arguments[0] as string;
 
                    return GetAccount(expectedControlAccountId, pId,  myGLAccountTypeEnumCard);
                }
                );
                //GLAccountPM GetGLAccount(string GLAccountId, int tenant)
            return fake;
        
        }
        private IGLAccountDataProvider GetFakeGLAccountDataProvider(Func<FakeItEasy.Core.IFakeObjectCall, GLAccountPM> func)
        {
            

            var fake = FakeItEasy.A.Fake<IGLAccountDataProvider>();
            FakeItEasy.A
                .CallTo(() => fake.GetGLAccount(FakeItEasy.A<string>.Ignored, FakeItEasy.A<int>.Ignored))
                .ReturnsLazily(func);
            
            return fake;

        }
        private static GLAccountPM GetAccount(string expectedControlAccountId, string pId, string myGLAccountTypeEnumCard)
        {
            myGLAccountTypeEnumCard = "";
            switch (pId)
            {
                //Card=1,
                case "Client":
                    myGLAccountTypeEnumCard = "2";
                    break;
                case "Vendor":
                    myGLAccountTypeEnumCard = "3";
                    break;
                case "Job":
                    myGLAccountTypeEnumCard = "4";
                    break;
                case "File":
                    myGLAccountTypeEnumCard = "5";
                    break;



                default:
                    break;
            }
            return new GLAccountPM()
            {
                Id = pId,
                AccountTypeCode = myGLAccountTypeEnumCard,
                ControlAccountId = expectedControlAccountId,
                ReconcileMethodCode = ((int)Logitude.Accounting.Def.EntityPMs.ReconcileMethodPM.ReconcileMethodEnum.LocalCurrency).ToString()
            };
        }
        

    }
}
