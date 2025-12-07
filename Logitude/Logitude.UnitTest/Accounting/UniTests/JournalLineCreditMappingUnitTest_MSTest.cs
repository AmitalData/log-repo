using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logitude.Accounting.BL.CoreBL.Mapping;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.UnitTest.Utils;
using System.Collections.Generic;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Test.Infrastructure;
using FakeItEasy;

namespace Logitude.UnitTest.Accounting.UniTests
{
    /// <summary>
    /// Refactored version of JournalLineCreditMappingUnitTest using FakeItEasy.
    /// Migrated from MSTest Fakes to FakeItEasy for compatibility with Visual Studio Community.
    /// </summary>
    [TestClass]
    public class JournalLineCreditMappingUnitTest_MSTest : TestBase_MSTest
    {
        [TestMethod]
        public void journalLineCreditMappingDoItValueRange_JournalPMAndJournalLinePMNotDSame_ExpectedExceptionKeyAreDifferent()
        {
            var listOfState = new List<Tuple<JournalLinePM, JournalPM>>();

            //Case Equal Keys:  listOfDiffrentKey.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 2, JournalId = "1-1" }, new JournalPM() { Tenant = 2, Id = "1-1" }));

            listOfState.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 1 }, new JournalPM() { Tenant = 1, Id = "1-1" }));
            listOfState.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 2 }, new JournalPM() { Tenant = 1, Id = "1-1" }));
            listOfState.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 2 }, new JournalPM() { Tenant = 2, Id = "1-1" }));

            listOfState.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 1, CreditAccountId = "1-1" }, new JournalPM() { Tenant = 2, Id = "1-1" }));

            listOfState.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 1, JournalId = "1-3" }, new JournalPM() { Tenant = 1, Id = "1-1" }));
            listOfState.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 2, JournalId = "1-3" }, new JournalPM() { Tenant = 1, Id = "1-1" }));
            listOfState.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 2, JournalId = "1-3" }, new JournalPM() { Tenant = 2, Id = "1-1" }));

            listOfState.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 1, JournalId = "1-3" }, new JournalPM() { Tenant = 1 }));
            listOfState.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 2, JournalId = "1-3" }, new JournalPM() { Tenant = 1 }));
            listOfState.Add(new Tuple<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = 2, JournalId = "1-3" }, new JournalPM() { Tenant = 2 }));

            foreach (var item in listOfState)
            {
                TestsUtil.AssertThrows<Exception>(delegate
                {
                    // Arrange
                    var journalLineCreditMapping = new JournalLineCreditMapping(
                        item.Item1, 
                        item.Item2, 
                        GetDefaultFakeGLAccountDataProvider_FakeItEasy(), 
                        GetDefaultIAccountingSettingResolver_FakeItEasy());
                    // Act: Run the method under test:
                    journalLineCreditMapping.DoIt();

                },
                    // Assert: Verify the result:
                "if (journalLineKey !=journalKey )");
            }
        }

        [TestMethod]
        public void journalLineCreditMappingDoIt_ActionTypeCodeIs2_ExpectedException()
        {
            int tenant = 1;
            string id = "1-1";

            var myState =
                Tuple.Create<JournalLinePM, JournalPM>(
                    new JournalLinePM() { Tenant = tenant, JournalId = id, ActionTypeCode = /*Debit =*/ "2" }, 
                    new JournalPM { Tenant = 1, Id = id });

            TestsUtil.AssertThrows<Exception>(delegate
            {
                // Arrange
                var journalLineCreditMapping = new JournalLineCreditMapping(
                    myState.Item1, 
                    myState.Item2, 
                    GetDefaultFakeGLAccountDataProvider_FakeItEasy(), 
                    GetDefaultIAccountingSettingResolver_FakeItEasy());
                // Act: Run the method under test:
                journalLineCreditMapping.DoIt();

            },
                // Assert: Verify the result:
              expectedContainsMessage: "this.MyMappingTypeEnum != MappingTypeEnum.Debit");
        }

        [TestMethod]
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

            var journalLineCreditMapping = new JournalLineCreditMapping(
                journalLine, 
                journalPM,
               GetDefaultFakeGLAccountDataProvider_FakeItEasy(), 
               GetDefaultIAccountingSettingResolver_FakeItEasy());

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

            foreach (var state2Check in listOfState2Check)
            {
                var journalLineCreditMapping = new JournalLineCreditMapping(
                     journalLine: state2Check.Item2,
                      journalPM: state2Check.Item1,
                      myGLAccountPMProvider: GetDefaultFakeGLAccountDataProvider_FakeItEasy(), 
                      myIAccountingSettingResolver: GetDefaultIAccountingSettingResolver_FakeItEasy());
                // Act: Run the method under test:
                journalLineCreditMapping.DoIt();
                // Assert: Verify the result:
                var myLedgerTransaction = journalLineCreditMapping.MyLedgerTransaction;
                LedgerTransactionBasicPropertiesinheritedFromJournalPostCondition(
                    journalLine: state2Check.Item2, 
                    journalPM: state2Check.Item1, 
                    MyLedgerTransaction: myLedgerTransaction);
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
                    ActionTypeCode = ((int)JournalActionTypeEnum.Credit).ToString(),
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
            var listOfState2Check = GetState2Check_JournalLineCreditMappingMapIt();

            int i = 0;
            foreach (var state2Check in listOfState2Check)
            {
                var journalLineCreditMapping = new JournalLineCreditMapping(
                     journalLine: state2Check.Item2,
                      journalPM: state2Check.Item1,
                      myGLAccountPMProvider: GetDefaultFakeGLAccountDataProvider_FakeItEasy(),
                      myIAccountingSettingResolver: GetDefaultIAccountingSettingResolver_FakeItEasy());
                // Act: Run the method under test:
                journalLineCreditMapping.DoIt();
                // Assert: Verify the result:
                var myLedgerTransaction = journalLineCreditMapping.MyLedgerTransaction;
                JournalLineCreditMappingMapItPostCondition(journalLine: state2Check.Item2, journalPM: state2Check.Item1, MyLedgerTransaction: myLedgerTransaction);
                i++;
            }
        }

        private void JournalLineCreditMappingMapItPostCondition(JournalLinePM journalLine, JournalPM journalPM, LedgerTransactionPM MyLedgerTransaction)
        {
            const string lineat = "JournalLineCreditMappingMapIt";
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
            var roundLocalAmount = System.Math.Round((decimal)journalLine.LocalAmount, 2);
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

        private static Tuple<JournalPM, JournalLinePM> NewStateJournalLineCreditMappingMapIt(string jlCreditAccountId, string jlDebitAccountId,
            decimal jlLocalAmount, decimal jlForeignAmount)
        {
            int tenant = 1;
            string id = "1-1";

            int jline = 1;
            DateTime jCreateDate = new DateTime(2016, 10, 26).AddDays(-3);
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
                   ActionTypeCode = ((int)JournalActionTypeEnum.Credit).ToString(),
                   DueDate = jlDueDate,
                   AccountingDate = jlAccountingDate,
                   Line = jline,
                   DocumentDate = jlDocumentDate,
                   CurrencyId = jlCurrencyId,
                   Reference1 = jlReference1,
                   Reference2 = jlReference2,
                   Reference3 = jlReference3,
                   Notes = jlNotes,
                   CreditAccountId = jlCreditAccountId,
                   DebitAccountId = jlDebitAccountId,
                   LocalAmount = jlLocalAmount,
                   ForeignAmount = jlForeignAmount,
               });
        }

        #endregion

        #region AddGLAccountTotalByMounth

        [TestMethod]
        public void journalLineCreditMappingDoItRangeCheck_InitBasicProperties_TotalByMounthREqual()
        {
            var listOfState2Check = GetStateToCheckLedgerTransactionBasicPropertiesinheritedFromJournal();

            foreach (var state2Check in listOfState2Check)
            {
                var journalLine = state2Check.Item2;
                var journalPM = state2Check.Item1;
                var journalLineCreditMapping = new JournalLineCreditMapping(
                     journalLine: state2Check.Item2,
                      journalPM: state2Check.Item1,
                      myGLAccountPMProvider: GetDefaultFakeGLAccountDataProvider_FakeItEasy(),
                      myIAccountingSettingResolver: GetDefaultIAccountingSettingResolver_FakeItEasy());
                // Act: Run the method under test:
                journalLineCreditMapping.DoIt();
                // Assert: Verify the result:
                var myGLAccountTotalByMonth = journalLineCreditMapping.MyGLAccountTotalByMonth;

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

        private IGLAccountDataProvider GetDefaultFakeGLAccountDataProvider_FakeItEasy()
        {
            // Create fake for IGLAccountDataProvider using FakeItEasy
            return A.Fake<IGLAccountDataProvider>();
        }

        private static IAccountingSettingResolver GetDefaultIAccountingSettingResolver_FakeItEasy()
        {
            // Create fake for IAccountingSettingResolver using FakeItEasy
            return A.Fake<IAccountingSettingResolver>();
        }
    }
}

