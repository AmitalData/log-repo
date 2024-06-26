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
using Simplog.Server.Infrastructure.Helpers;
using FakeItEasy;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.BL.CoreBL;

namespace Logitude.UnitTest.Accounting.UniTests
{
    [TestClass]
    public class JournalLineDebitMappingUnitTest
    {
        

        [TestMethod]
        public void JournalLineDebitMappingDoIt_ActionTypeCodeIs1_ExpectedException()
        {
            int tenant = 1;
            string id = "1-1";
            

            var myState =
                Tuple.Create<JournalLinePM, JournalPM>(new JournalLinePM() { Tenant = tenant, JournalId = id, ActionTypeCode = /*Credit =*/ "1" }, new JournalPM { Tenant = 1, Id = id });

            TestsUtil.AssertThrows<Exception>(delegate
            {
                // Arrange
                var JournalLineDebitMapping = GetVatExtractFalseDebitMap(myState.Item1, myState.Item2);
                // Act: Run the method under test:
                JournalLineDebitMapping.DoIt();

            },
                // Assert: Verify the result:
              expectedContainsMessage: "this.MyMappingTypeEnum != MappingTypeEnum.Credit");

        }

        private static JournalLineDebitMapping GetVatExtractFalseDebitMap(
            JournalLinePM journalLine, JournalPM journalPM, Func<string, GLAccountPM> func = null)
        {
            var fakeIGLAccountDataProvider = A.Fake<IGLAccountDataProvider>();
            if (func!=null)
            {
                A.CallTo(() => fakeIGLAccountDataProvider.GetGLAccount(A<string>.Ignored, A<int>.Ignored))
                    .ReturnsLazily(objecCall =>
                    {
                        var pId = objecCall.Arguments[0] as string;
                        //return GetGLAccount(pId);
                        return func(pId);
                    });    
            }
            return new JournalLineDebitMapping(journalLine, journalPM, false, fakeIGLAccountDataProvider, A.Fake<IAccountingSettingResolver>());
        }

#if CHECK        
        [TestMethod]
        public void JournalLineDebitMappingDoIt_withoutInitJournalLineDueDate_ExpectedExceptionNullableobjectmusthaveavalue()
        {
            // Arrange State
            int tenant = 1;
            string id = "1-1";
            JournalLinePM journalLine = new JournalLinePM()
            {
                Tenant = tenant,
                JournalId = id,
                ActionTypeCode = "2",//ActionTypeCodeEnum = MyJournalActionTypeEnum.Debit
            };
            JournalPM journalPM = new JournalPM { Tenant = 1, Id = id };
            var JournalLineDebitMapping = GetVatExtractFalseDebitMap(journalLine, journalPM);
            //JournalLineDebitMapping.DoIt();

            TestsUtil.AssertThrows<InvalidOperationException>(delegate
            {
                // Act: Run the method under test:
                JournalLineDebitMapping.DoIt();

            },
                // Assert: Verify the result:
             expectedContainsMessage: null,
             lineErrorIs: "MyLedgerTransaction.DueDate = (DateTime)_JournalLine.DueDate;");

        }

        
#endif
   
        

        #region DefaultMapLedgerTransactionPMFromJournal

#if CHECK
        [TestMethod]
        public void JournalLineDebitMappingDoItRangeCheck_InitBasicProperties_DefaultMapLedgerTransactionPMFromJournal()
        {

            var listOfState2Check = GetStateToCheckLedgerTransactionBasicPropertiesinheritedFromJournal();


            //using (ShimsContext.Create())
            {

              
                foreach (var state2Check in listOfState2Check)
                {
                    var JournalLineDebitMapping = GetVatExtractFalseDebitMap(
                         journalLine: state2Check.Item2,
                          journalPM: state2Check.Item1);
                    // Act: Run the method under test:
                    JournalLineDebitMapping.DoIt();
                    // Assert: Verify the result:
                    var myLedgerTransaction = JournalLineDebitMapping.MyLedgerTransaction;
                    LedgerTransactionBasicPropertiesinheritedFromJournalPostCondition(journalLine: state2Check.Item2, journalPM: state2Check.Item1, MyLedgerTransaction: myLedgerTransaction);

                }

            }

        }

        
#endif
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
                    ActionTypeCode = ((int)JournalActionTypeEnum.Credit).ToString(),

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



            Assert.AreEqual(journalPM.AccountingDate, MyLedgerTransaction.AccountingDate,
        "MyLedgerTransaction.AccountingDate = _JournalPM.AccountingDate;");


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


        #region JournalLineDebitMapping.MapIt
#if CHECK
        [TestMethod]
        public void JournalLineDebitMappingDoItRangeCheck_InitBasicProperties_JournalLineDebitMappingMapIt()
        {
            //MyLedgerTransaction.AccountId = _JournalLine.CreditAccountId;
            //MyLedgerTransaction.OppositeAccountId = _JournalLine.DebitAccountId;
            //MyLedgerTransaction.LocalAmountDebit = 0;
            //MyLedgerTransaction.LocalAmountCredit = (decimal)_JournalLine.LocalAmount;
            //MyLedgerTransaction.CurrencyId = _JournalLine.CurrencyId;
            //MyLedgerTransaction.ForeignAmountDebit = 0;
            //MyLedgerTransaction.ForeignAmountCredit = (decimal)_JournalLine.ForeignAmount;


            var listOfState2Check = GetState2Check_JournalLineDebitMappingMapIt();


            try //using (ShimsContext.Create())
            {

                //ShimJournalLineMappingBase.AllInstances.GetGLAccountPMString = (myRepo, pId) =>
                //{
                //    return new GLAccountPM() { };
                //};
                int i = 0;
                foreach (var state2Check in listOfState2Check)
                {
                    var JournalLineDebitMapping = GetVatExtractFalseDebitMap(
                         journalLine: state2Check.Item2,
                          journalPM: state2Check.Item1);
                    // Act: Run the method under test:
                    JournalLineDebitMapping.DoIt();
                    // Assert: Verify the result:
                    var myLedgerTransaction = JournalLineDebitMapping.MyLedgerTransaction;
                    JournalLineDebitMappingMapItPostCondition(journalLine: state2Check.Item2, journalPM: state2Check.Item1, MyLedgerTransaction: myLedgerTransaction);
                    i++;

                }

            }
            finally { }

        }

        
#endif
        private void JournalLineDebitMappingMapItPostCondition(JournalLinePM journalLine, JournalPM journalPM, LedgerTransactionPM MyLedgerTransaction)
        {
            const string lineat = "JournalLineDebitMappingMapIt";
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
                //ExchangeRate = roundLocalAmount/ roundedFA;
            }
            Assert.AreEqual(ExchangeRate, MyLedgerTransaction.ExchangeRate,
            lineat);

        }

        private List<Tuple<JournalPM, JournalLinePM>> GetState2Check_JournalLineDebitMappingMapIt()
        {

            var listOfState = new List<Tuple<JournalPM, JournalLinePM>>();


            //JournalLineDebitMappingMapIt Check !!!

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
            listOfState.Add(NewStateJournalLineDebitMappingMapIt(jlCreditAccountId, jlDebitAccountId, jlLocalAmount, jlForeignAmount));
            jlForeignAmount = 1;
            listOfState.Add(NewStateJournalLineDebitMappingMapIt(jlCreditAccountId, jlDebitAccountId, jlLocalAmount, jlForeignAmount));
            jlForeignAmount = -11;
            listOfState.Add(NewStateJournalLineDebitMappingMapIt(jlCreditAccountId, jlDebitAccountId, jlLocalAmount, jlForeignAmount));
            jlCreditAccountId = jlCreditAccountId + "c";
            listOfState.Add(NewStateJournalLineDebitMappingMapIt(jlCreditAccountId, jlDebitAccountId, jlLocalAmount, jlForeignAmount));
            jlCreditAccountId = jlCreditAccountId + "D";
            listOfState.Add(NewStateJournalLineDebitMappingMapIt(jlCreditAccountId, jlDebitAccountId, jlLocalAmount, jlForeignAmount));
            jlLocalAmount = 1000.99m;
            listOfState.Add(NewStateJournalLineDebitMappingMapIt(jlCreditAccountId, jlDebitAccountId, jlLocalAmount, jlForeignAmount));
            jlLocalAmount = -33.444m;
            listOfState.Add(NewStateJournalLineDebitMappingMapIt(jlCreditAccountId, jlDebitAccountId, jlLocalAmount, jlForeignAmount));
            jlLocalAmount = -0.133444m;
            listOfState.Add(NewStateJournalLineDebitMappingMapIt(jlCreditAccountId, jlDebitAccountId, jlLocalAmount, jlForeignAmount));
            jlDebitAccountId = jlDebitAccountId + "ff";
            listOfState.Add(NewStateJournalLineDebitMappingMapIt(jlCreditAccountId, jlDebitAccountId, jlLocalAmount, jlForeignAmount));


            return listOfState;
        }

        private static Tuple<JournalPM, JournalLinePM> NewStateJournalLineDebitMappingMapIt(string jlCreditAccountId, string jlDebitAccountId,
            decimal jlLocalAmount, decimal jlForeignAmount)
        {
            int tenant = 1;
            string id = "1-1";


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



        #region CheckParentAccount

#if CHECK

        [TestMethod]
        public void JournalLineDebitMappingDoIt_ParentAccountIsCard_ControlAccountIdIsNull()
        {


            var listOfState2Check = new List<Tuple<JournalLinePM, JournalPM, string>>();



            listOfState2Check.Add(new Tuple<JournalLinePM, JournalPM, string>(
                new JournalLinePM()
                {
                    Tenant = 1,
                    JournalId = "1-1",
                    ActionTypeCode = ((int)MyJournalActionTypeEnum.Credit).ToString(),
                    DueDate = new DateTime(2016, 3, 3),
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
                    LocalAmount = 1,
                    ForeignAmount = 1,

                    CreditAccountId = "1-3",

                },
                new JournalPM() { Tenant = 1, Id = "1-1" },
                "CreditAccountId is null "
                ));


            
            try//using (ShimsContext.Create())
            {

                //ShimJournalLineMappingBase.AllInstances.GetGLAccountPMString = (myRepo, pId) =>
                
                

                int i = 0;
                foreach (var state2Check in listOfState2Check)
                {
                   NetCommonHelper.Logger.DevLog.Instance.WriteDebug(state2Check.Item3);
                    var JournalLineDebitMapping = GetVatExtractFalseDebitMap(
                         journalLine: state2Check.Item1,
                          journalPM: state2Check.Item2,
                          func:(pId )=>
                {
                    var myGLAccountTypeEnumCard = ((int)GLAccountTypePM.GLAccountTypeEnum.Card).ToString();
                    if (pId == "1-3") return new GLAccountPM() { AccountTypeCode = null, ControlAccountId = "1-0" };
                    return new GLAccountPM() { AccountTypeCode = myGLAccountTypeEnumCard, ControlAccountId = "1-0" };
                }
                          );
                    // Act: Run the method under test:
                    JournalLineDebitMapping.DoIt();
                    // Assert: Verify the result:
                    var myLedgerTransaction = JournalLineDebitMapping.MyLedgerTransaction;

                    Assert.AreEqual(null, myLedgerTransaction.ControlAccountId);
                    i++;

                }

            }
            finally { }
        }



        
#endif


#if CHECK

        [TestMethod]
        public void JournalLineDebitMappingDoItRange_ParentAccountIsNotCard_ControlAccountIdIsGetCreditparentAccount()
        {


            var listOfState2Check = new List<Tuple<JournalLinePM, JournalPM, string>>();


            listOfState2Check.Add(new Tuple<JournalLinePM, JournalPM, string>(
                new JournalLinePM()
                {
                    Tenant = 1,
                    JournalId = "1-1",
                    ActionTypeCode = ((int)MyJournalActionTypeEnum.Credit).ToString(),
                    DueDate = new DateTime(2016, 3, 3),
                    LocalAmount = 1,
                    ForeignAmount = 1,


                    CreditAccountId = "Client",

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
                    LocalAmount = 1,
                    ForeignAmount = 1,

                    CreditAccountId = "Vendor",

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
                    LocalAmount = 1,
                    ForeignAmount = 1,

                    CreditAccountId = "Job",

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
                    LocalAmount = 1,
                    ForeignAmount = 1,

                    CreditAccountId = "File",

                },
                new JournalPM() { Tenant = 1, Id = "1-1" },
                "CreditAccountId  type is File"
                ));

            
            var expectedControlAccountId = "expectedControlAccountId";
            try//using (ShimsContext.Create())
            {
                
                
                int i = 0;
                foreach (var state2Check in listOfState2Check)
                {
                   NetCommonHelper.Logger.DevLog.Instance.WriteDebug("state Test: " + state2Check.Item3);
                    var JournalLineDebitMapping = GetVatExtractFalseDebitMap(
                         journalLine: state2Check.Item1,
                          journalPM: state2Check.Item2,
                func : (pId) =>
                {
                    var myGLAccountTypeEnumCard = ((int)GLAccountTypePM.GLAccountTypeEnum.Card).ToString();
                    return new GLAccountPM()
                    {
                        Id = pId,
                        AccountTypeCode = myGLAccountTypeEnumCard,
                        ControlAccountId = expectedControlAccountId
                    };
                })
                    ;
                    // Act: Run the method under test:
                    JournalLineDebitMapping.DoIt();
                    // Assert: Verify the result:
                    var myLedgerTransaction = JournalLineDebitMapping.MyLedgerTransaction;

                    Assert.AreEqual(expectedControlAccountId, myLedgerTransaction.ControlAccountId);
                    i++;

                }

            }
            finally { }
        }

        
#endif

#if CHECK
        [TestMethod]
        public void JournalLineDebitMappingDoItRange_ParentAccountReconcileMethodCodeISLocalCurrency_LedgerTransactionOpenAmountNisOpenAmount()
        {


            var listOfState2Check = new List<Tuple<JournalLinePM, JournalPM, string>>();


            listOfState2Check.Add(new Tuple<JournalLinePM, JournalPM, string>(
                new JournalLinePM()
                {
                    Tenant = 1,
                    JournalId = "1-1",
                    ActionTypeCode = ((int)MyJournalActionTypeEnum.Credit).ToString(),
                    DueDate = new DateTime(2016, 3, 3),
                    LocalAmount = 11,
                    ForeignAmount = 1,


                    CreditAccountId = "Client",

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
                    LocalAmount = 22.54353m,
                    ForeignAmount = 1,

                    CreditAccountId = "Vendor",

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
                    LocalAmount = 231.44m,
                    ForeignAmount = 1,

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
                    LocalAmount = -4531.55m,
                    ForeignAmount = 1,

                    CreditAccountId = "File",

                },
                new JournalPM() { Tenant = 1, Id = "1-1" },
                "Check Local Amount"
                ));

            var myGLAccountTypeEnumCard = ((int)GLAccountTypePM.GLAccountTypeEnum.Card).ToString();
            var expectedControlAccountId = "expectedControlAccountId";
            //using (ShimsContext.Create())
            try
            {
                CacheUtil.Instance.Clear();
                CacheUtil.Instance.InitCacheAccountingCurrencyId(_Tenant: 1, CurrencyId: "NIS");

            
            
                
                int i = 0;
                foreach (var state2Check in listOfState2Check)
                {
                   NetCommonHelper.Logger.DevLog.Instance.WriteDebug("state Test: " + state2Check.Item3);
                    var JournalLineDebitMapping = GetVatExtractFalseDebitMap(
                         journalLine: state2Check.Item1,
                          journalPM: state2Check.Item2,
                          func: (pId) =>
                          {
                              //var myGLAccountTypeEnumCard = ((int)GLAccountTypePM.GLAccountTypeEnum.Card).ToString();
                              return new GLAccountPM()
                              {
                                  Id = pId,
                                  AccountTypeCode = myGLAccountTypeEnumCard,
                                  ControlAccountId = expectedControlAccountId,
                                  ReconcileMethodCode = ((int)Logitude.Accounting.Def.EntityPMs.ReconcileMethodPM.ReconcileMethodEnum.LocalCurrency).ToString()
                              };
                          });
                    // Act: Run the method under test:
                    JournalLineDebitMapping.DoIt();
                    // Assert: Verify the result:
                    var myLedgerTransaction = JournalLineDebitMapping.MyLedgerTransaction;

                    Assert.AreEqual(-1 * state2Check.Item1.LocalAmount, myLedgerTransaction.OpenAmount);
                    Assert.AreEqual("NIS", myLedgerTransaction.OpenAmountCurrencyId);
                    i++;

                }

            
            }
            finally
            {
                CacheUtil.Instance.Clear();
            }
        }


        
#endif
#if CHECK
        [TestMethod]
        public void JournalLineDebitMappingDoItRange_ParentAccountReconcileMethodCodeISForeignCurrency_LedgerTransactionOpenAmountForeignAmount()
        {


            var listOfState2Check = new List<Tuple<JournalLinePM, JournalPM, string>>();


            listOfState2Check.Add(new Tuple<JournalLinePM, JournalPM, string>(
                new JournalLinePM()
                {
                    Tenant = 1,
                    JournalId = "1-1",
                    ActionTypeCode = ((int)MyJournalActionTypeEnum.Credit).ToString(),
                    DueDate = new DateTime(2016, 3, 3),
                    LocalAmount = 11,
                    ForeignAmount = 13121.9999m,
                    CurrencyId = null,

                    CreditAccountId = "Client",

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
                    LocalAmount = 22.54353m,
                    ForeignAmount = 1322.4355m,
                    CurrencyId = "NIS",
                    CreditAccountId = "Vendor",

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
                    LocalAmount = 231.44m,
                    ForeignAmount = 1234.9m,
                    CurrencyId = "EUR",
                    CreditAccountId = "Job",

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
                    LocalAmount = -4531.55m,
                    ForeignAmount = -2341.533m,


                    CurrencyId = "USD",
                    CreditAccountId = "File",

                },
                new JournalPM() { Tenant = 1, Id = "1-1" },
                "Check ForeignAmount"
                ));

            var myGLAccountTypeEnumCard = ((int)GLAccountTypePM.GLAccountTypeEnum.Card).ToString();
            var expectedControlAccountId = "expectedControlAccountId";
            try//using (ShimsContext.Create())
            {

               
                int i = 0;
                foreach (var state2Check in listOfState2Check)
                {
                   NetCommonHelper.Logger.DevLog.Instance.WriteDebug("state Test: " + state2Check.Item3);
                    var JournalLineDebitMapping = GetVatExtractFalseDebitMap(
                         journalLine: state2Check.Item1,
                          journalPM: state2Check.Item2,
                          func: (pId) =>
                          {
                              //var myGLAccountTypeEnumCard = ((int)GLAccountTypePM.GLAccountTypeEnum.Card).ToString();
                              return new GLAccountPM()
                              {
                                  Id = pId,
                                  AccountTypeCode = myGLAccountTypeEnumCard,
                                  ControlAccountId = expectedControlAccountId,
                                  ReconcileMethodCode = ((int)Logitude.Accounting.Def.EntityPMs.ReconcileMethodPM.ReconcileMethodEnum.LocalCurrency).ToString()
                              };
                          });
                    // Act: Run the method under test:
                    JournalLineDebitMapping.DoIt();
                    // Assert: Verify the result:
                    var myLedgerTransaction = JournalLineDebitMapping.MyLedgerTransaction;

                    Assert.AreEqual(-1 * state2Check.Item1.ForeignAmount, myLedgerTransaction.OpenAmount);
                    Assert.AreEqual(state2Check.Item1.CurrencyId, myLedgerTransaction.OpenAmountCurrencyId);
                    i++;

                }

            }
            finally { }
        }

        
#endif
        #endregion






        #region AddGLAccountTotalByMounth

#if CHECK

        [TestMethod]
        public void JournalLineDebitMappingDoItRangeCheck_InitBasicProperties_TotalByMounthREqual()
        {

            var listOfState2Check = GetStateToCheckLedgerTransactionBasicPropertiesinheritedFromJournal();


            try//using (ShimsContext.Create())
            {

                
                foreach (var state2Check in listOfState2Check)
                {
                    var journalLine = state2Check.Item2;
                    var journalPM = state2Check.Item1;
                    var JournalLineDebitMapping = GetVatExtractFalseDebitMap(
                         journalLine: state2Check.Item2,
                          journalPM: state2Check.Item1);
                    // Act: Run the method under test:
                    JournalLineDebitMapping.DoIt();
                    // Assert: Verify the result:
                    var myGLAccountTotalByMonth = JournalLineDebitMapping.MyGLAccountTotalByMonth;// ByAccountingDate;



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
            finally { }

        }


        
#endif

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



    }
}
