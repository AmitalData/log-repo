using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FakeItEasy;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Interfaces;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.UnitTest.Utils;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplog.Server.Infrastructure;
using System.Linq;

namespace Logitude.UnitTest.Accounting.UniTests.Reconciliation
{
    [TestClass]
    public class ReconciliationValidatorUnitTest
    {
        

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext) {

            var fake = A.Fake<ILoggedContactUtil>();
            
                 A.CallTo(() => fake.GetLoggedContact(1))
                 .Returns<ContactPM>(new ContactPM() {  DontShowLocal=true});
            ContainerAccessor.Container.RegisterType<ILoggedContactUtil, ILoggedContactUtil>("LoggedContactUtil", new InjectionFactory(c => fake));


            var textCodeTranslatorFake = A.Fake<ITextCodeTranslator>();
            A.CallTo(() => textCodeTranslatorFake.Translate(A<string>.Ignored, A<int>.Ignored))
                .ReturnsLazily(
                (string textCodeCode, int tenant) =>
                {
                    return textCodeCode;
                }
            );
            ReconciliationValidator.OverrideITextCodeTranslator = textCodeTranslatorFake;

        }

        [ClassCleanup()]
        public static void MyClassCleanup() { ReconciliationValidator.OverrideITextCodeTranslator = null; }


       
        public static ValidationContext MyTestInitialize(ReconciliationPM reconciliationPM, IReconciliationValidatorContextDataProvider fakeIReconciliationValidatorContextDataProvider) {

            
            var contextItems = new Dictionary<object, object>            {
                //    { "AccountingPeriodsByTypeRegular", _AccountingPeriodsByTypeRegular }
            };
            var contextServiceProvider = new AccountingValidationContextServiceProvider();


            contextServiceProvider.AddService<IReconciliationValidatorContextDataProvider>(fakeIReconciliationValidatorContextDataProvider);
            return  new System.ComponentModel.DataAnnotations.ValidationContext(reconciliationPM, contextServiceProvider, contextItems);
            

        }

        [TestMethod]
        public void IsReconciliationValid001_EmptyLines_ValidationresultErrorYouShouldSelectTwoTransactions()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant=myTenant,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {

                }
            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList( new List<string>() { "AccId1" } , myTenant))
            .Returns<List<LedgerTransactionPM>>(new List<LedgerTransactionPM>() { new LedgerTransactionPM() { Tenant = myTenant } });
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_YouShouldSelectTwoTransactions);
            var lineCode = "if (myReconciliationPM.ReconciliationLines.Count == 0)";
            Assert.IsTrue(messageExist, lineCode); 



        }

        [TestMethod]
        public void IsReconciliationValid002_Only1ReconciliationLine_ValidationresultErrorYouShouldSelectTwoTransactions()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){ }
                }
            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(new List<string>() { "AccId1" }, myTenant))
            .Returns<List<LedgerTransactionPM>>(new List<LedgerTransactionPM>() { new LedgerTransactionPM() { Tenant = myTenant } });
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_YouShouldSelectTwoTransactions);
            var lineCode = "if (myReconciliationPM.ReconciliationLines.Count == 1)";
            Assert.IsTrue(messageExist, lineCode);



        }


        [TestMethod]
        public void IsReconciliationValid003_MultiCurrencyReconcile_ValidationresultErrorNoMultiCurrencyReconcile()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD" },
                    new ReconciliationLinePM(){  CurrencyId ="EUR" },
                }
            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(new List<string>() { "AccId1" }, myTenant))
            .Returns<List<LedgerTransactionPM>>(new List<LedgerTransactionPM>() { new LedgerTransactionPM() { Tenant = myTenant } });
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_SaidnoMultiCurrencyReconcile);
            var lineCode = "var listCurrency = myReconciliationPM.ReconciliationLines.Select(rec => rec.CurrencyId).Distinct().ToList();";
            Assert.IsTrue(messageExist, lineCode);



        }




        [TestMethod]
        public void IsReconciliationValid004_TransactionIdfewtimes_ValidationresultErrorReconciliationLinesHaveuseTransactionIdfewtimes()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" },
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" },
                }
            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(new List<string>() { "AccId1" }, myTenant))
            .Returns<List<LedgerTransactionPM>>(new List<LedgerTransactionPM>() { new LedgerTransactionPM() { Tenant = myTenant } });
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_ReconciliationLinesHaveuseTransactionIdfewtimes);
            var lineCode = "var repeating = myReconciliationPM.ReconciliationLines.GroupBy(r => r.TransactionId).Where(g => g.Count() > 1).ToList();";
            Assert.IsTrue(messageExist, lineCode);



        }




        [TestMethod]
        public void IsReconciliationValid005_DeletedReconciliationLines_ValidationresultErrorDeletedReconciliationLinesisnotallowed()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" },
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t2" },

                },
                DeletedReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t3" },
                }

            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(new List<string>() { "AccId1" }, myTenant))
            .Returns<List<LedgerTransactionPM>>(new List<LedgerTransactionPM>() { new LedgerTransactionPM() { Tenant = myTenant } });
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_DeletedReconciliationLinesisnotallowed);
            var lineCode = "if (myReconciliationPM.DeletedReconciliationLines.Any())";
            Assert.IsTrue(messageExist, lineCode);



        }



        [TestMethod]
        public void IsReconciliationValid006_UpdateReconciliationLines_ValidationresultErrorM_ReconciliationLinescanonlyinsert()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                 ChangeSetOp = ChangeSetOperation.Update,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" },
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t2" , ChangeSetOp = ChangeSetOperation.Update},

                },
               

            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(new List<string>() { "AccId1" }, myTenant))
            .Returns<List<LedgerTransactionPM>>(new List<LedgerTransactionPM>() { new LedgerTransactionPM() { Tenant = myTenant } });
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_ReconciliationLinescanonlyinsert);
            var lineCode = "if (myReconciliationPM.ReconciliationLines.Any(r => r.ChangeSetOp != ChangeSetOperation.None))";
            Assert.IsTrue(messageExist, lineCode);



        }


        
        [TestMethod]
        public void IsReconciliationValid007_ARPaymentorAPPaymentandnotstorno3_ValidationresultErrorM_CantIncludeTwoOrMorePayment()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" , ChangeSetOp = ChangeSetOperation.Insert},
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t2" , ChangeSetOp = ChangeSetOperation.Insert},

                },


            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });


            List<string> transactionsId = reconciliationPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(tt => tt.TransactionId).ToList();

            var ltList = new List<LedgerTransactionPM>() {
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="3", OriginalJournalId=null},
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="3",OriginalJournalId=null},

            };

            //A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(A<List<string>>.Ignored, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_CantIncludeTwoOrMorePayment);
            var lineCode = "paymentsCount = transactionsPMList.Count(d => (d.SourceTypeCode == '3' || d.SourceTypeCode == '5') && d.OriginalJournalId == null); // 3- ARPayment , or 5- APPayment , and not storno";
            Assert.IsTrue(messageExist, lineCode);



        }


        [TestMethod]
        public void IsReconciliationValid008_ARPaymentorAPPaymentandnotstorno3ButStorno_ValidationresultError_NotContains_M_CantIncludeTwoOrMorePayment()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" , ChangeSetOp = ChangeSetOperation.Insert},
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t2" , ChangeSetOp = ChangeSetOperation.Insert},

                },


            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });


            List<string> transactionsId = reconciliationPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(tt => tt.TransactionId).ToList();

            var ltList = new List<LedgerTransactionPM>() {
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="3", OriginalJournalId="s1"},
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="3",OriginalJournalId="s1"},

            };

            //A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(A<List<string>>.Ignored, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_CantIncludeTwoOrMorePayment);
            var lineCode = "paymentsCount = transactionsPMList.Count(d => (d.SourceTypeCode == '3' || d.SourceTypeCode == '5') && d.OriginalJournalId == null); // 3- ARPayment , or 5- APPayment , and not storno";
            Assert.IsFalse(messageExist, lineCode);



        }


        [TestMethod]
        public void IsReconciliationValid009_ARPaymentorAPPaymentandnotstorno5_ValidationresultErrorM_CantIncludeTwoOrMorePayment()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" , ChangeSetOp = ChangeSetOperation.Insert},
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t2" , ChangeSetOp = ChangeSetOperation.Insert},

                },


            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });


            List<string> transactionsId = reconciliationPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(tt => tt.TransactionId).ToList();

            var ltList = new List<LedgerTransactionPM>() {
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="3", OriginalJournalId=null},
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="3",OriginalJournalId=null},

            };

            //A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(A<List<string>>.Ignored, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_CantIncludeTwoOrMorePayment);
            var lineCode = "paymentsCount = transactionsPMList.Count(d => (d.SourceTypeCode == '3' || d.SourceTypeCode == '5') && d.OriginalJournalId == null); // 3- ARPayment , or 5- APPayment , and not storno";
            Assert.IsTrue(messageExist, lineCode);



        }


        [TestMethod]
        public void IsReconciliationValid010_ARPaymentorAPPaymentandnotstorno5ButStorno_ValidationresultError_NotContains_M_CantIncludeTwoOrMorePayment()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" , ChangeSetOp = ChangeSetOperation.Insert},
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t2" , ChangeSetOp = ChangeSetOperation.Insert},

                },


            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });


            List<string> transactionsId = reconciliationPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(tt => tt.TransactionId).ToList();

            var ltList = new List<LedgerTransactionPM>() {
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="5", OriginalJournalId="s1"},
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="5",OriginalJournalId="s1"},

            };

            //A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(A<List<string>>.Ignored, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_CantIncludeTwoOrMorePayment);
            var lineCode = "paymentsCount = transactionsPMList.Count(d => (d.SourceTypeCode == '3' || d.SourceTypeCode == '5') && d.OriginalJournalId == null); // 3- ARPayment , or 5- APPayment , and not storno";
            Assert.IsFalse(messageExist, lineCode);



        }




        [TestMethod]
        public void IsReconciliationValid011_ReconciliationLineUpdate_ValidationresultError_M_ReconciliationLinescanonlybeinsertmode()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" , ChangeSetOp = ChangeSetOperation.Insert},
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t2" , ChangeSetOp = ChangeSetOperation.Update},

                },


            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });


            List<string> transactionsId = reconciliationPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(tt => tt.TransactionId).ToList();

            var ltList = new List<LedgerTransactionPM>() {
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="5", OriginalJournalId="s1"},
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="5",OriginalJournalId="s1"},

            };

            //A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(A<List<string>>.Ignored, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_ReconciliationLinescanonlybeinsertmode);
            var lineCode = "if (reconciliationLine.ChangeSetOp != ChangeSetOperation.Insert)";
            Assert.IsTrue(messageExist, lineCode);



        }



        [TestMethod]
        public void IsReconciliationValid012_NoTransaction_ValidationresultError_M_reconciliationLineTransactionIdIsnull()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" , ChangeSetOp = ChangeSetOperation.Insert},
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="" , ChangeSetOp = ChangeSetOperation.Insert},

                },


            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });


            List<string> transactionsId = reconciliationPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(tt => tt.TransactionId).ToList();

            var ltList = new List<LedgerTransactionPM>() {
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="5", OriginalJournalId="s1"},
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="5",OriginalJournalId="s1"},

            };

            //A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(A<List<string>>.Ignored, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_reconciliationLineTransactionIdIsnull);
            var lineCode = "if (String.IsNullOrWhiteSpace(reconciliationLine.TransactionId)) ";
            Assert.IsTrue(messageExist, lineCode);



        }


        [TestMethod]
        public void IsReconciliationValid013_TransactionIdNotinDB_ValidationresultError_M_TransactionIdNotinDB()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" , ChangeSetOp = ChangeSetOperation.Insert},
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="TransactionIdNotinDB" , ChangeSetOp = ChangeSetOperation.Insert},

                },


            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });


            List<string> transactionsId = reconciliationPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(tt => tt.TransactionId).ToList();

            var ltList = new List<LedgerTransactionPM>() {
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="5", OriginalJournalId="s1"},
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="5",OriginalJournalId="s1"},

            };

            //A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(A<List<string>>.Ignored, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_TransactionIdNotinDB);
            var lineCode = "var ledgerTransactionPM = ledgerTransactionPMs.FirstOrDefault(rec => rec.Id == reconciliationLine.TransactionId);";
            Assert.IsTrue(messageExist, lineCode);



        }


        [TestMethod]
        public void IsReconciliationValid014_ledgerTransactionPMIsReconciled_ValidationresultError_M_ledgerTransactionalreadyReconciled()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" , ChangeSetOp = ChangeSetOperation.Insert},
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t2" , ChangeSetOp = ChangeSetOperation.Insert},

                },


            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });


            List<string> transactionsId = reconciliationPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(tt => tt.TransactionId).ToList();

            var ltList = new List<LedgerTransactionPM>() {
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="5", OriginalJournalId="s1" , IsReconciled=true},
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="5",OriginalJournalId="s1"},

            };

            //A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(A<List<string>>.Ignored, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_ledgerTransactionalreadyReconciled);
            var lineCode = "if (ledgerTransactionPM.IsReconciled)";
            Assert.IsTrue(messageExist, lineCode);



        }



        [TestMethod]
        public void IsReconciliationValid015_OpenAmountCurrencyIdDiffreconciliationLineCurrencyId_ValidationresultError_M_OpenAmountCurrencyIdDiffreconciliationLineCurrencyId()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" , ChangeSetOp = ChangeSetOperation.Insert},
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t2" , ChangeSetOp = ChangeSetOperation.Insert},

                },


            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });


            List<string> transactionsId = reconciliationPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(tt => tt.TransactionId).ToList();

            var ltList = new List<LedgerTransactionPM>() {
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="5", OriginalJournalId="s1" , OpenAmountCurrencyId="USD"},
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t2"  , SourceTypeCode ="5",OriginalJournalId="s1", OpenAmountCurrencyId="EUR"},

            };

            //A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(A<List<string>>.Ignored, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_OpenAmountCurrencyIdDiffreconciliationLineCurrencyId);
            var lineCode = "if (ledgerTransactionPM.OpenAmountCurrencyId != reconciliationLine.CurrencyId)";
            Assert.IsTrue(messageExist, lineCode);



        }




        [TestMethod]
        public void IsReconciliationValid016_transInsertNotReconcile_ValidationresultError_M_InsertledgerTransactionbutIsnotReconciled()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" , ChangeSetOp = ChangeSetOperation.Insert},
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t2" , ChangeSetOp = ChangeSetOperation.Insert},

                },


            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });


            List<string> transactionsId = reconciliationPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(tt => tt.TransactionId).ToList();

            var ltList = new List<LedgerTransactionPM>() {
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="5", OriginalJournalId="s1" , OpenAmountCurrencyId="USD" ,ChangeSetOp= ChangeSetOperation.Insert } ,
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t2"  , SourceTypeCode ="5",OriginalJournalId="s1", OpenAmountCurrencyId="EUR"},

            };

            //A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(A<List<string>>.Ignored, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_InsertledgerTransactionbutIsnotReconciled);
            var lineCode = "if (!ledgerTransactionPM.IsReconciled)";
            Assert.IsTrue(messageExist, lineCode);



        }


        [TestMethod]
        public void IsReconciliationValid017_OpenAmountis0ReconcileNot_ValidationresultError_M_InsertledgerTransactionbutIsnotReconciled()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" , ChangeSetOp = ChangeSetOperation.Insert, ReconciliationAmount=-1},
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t2" , ChangeSetOp = ChangeSetOperation.Insert},

                },


            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });


            List<string> transactionsId = reconciliationPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(tt => tt.TransactionId).ToList();

            var ltList = new List<LedgerTransactionPM>() {
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="5", OriginalJournalId="s1" , OpenAmountCurrencyId="USD" , OpenAmount=0} ,
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t2"  , SourceTypeCode ="5",OriginalJournalId="s1", OpenAmountCurrencyId="EUR"},

            };

            //A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(A<List<string>>.Ignored, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_OpenAmountis0ReconcileNot);
            var lineCode = @"if (ledgerTransactionPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
                        {
                            if (!ledgerTransactionPM.IsReconciled)
                            {
                                AddError(errorsList, M_InsertledgerTransactionbutIsnotReconciled);
                            }
    }";
            Assert.IsTrue(messageExist, lineCode);



        }


        [TestMethod]
        public void IsReconciliationValid018_OpenAmountis0ReconcileisZero_ValidationresultErrornotContains_M_InsertledgerTransactionbutIsnotReconciled()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" , ChangeSetOp = ChangeSetOperation.Insert, ReconciliationAmount=0},
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t2" , ChangeSetOp = ChangeSetOperation.Insert},

                },


            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });


            List<string> transactionsId = reconciliationPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(tt => tt.TransactionId).ToList();

            var ltList = new List<LedgerTransactionPM>() {
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="5", OriginalJournalId="s1" , OpenAmountCurrencyId="USD" , OpenAmount=0} ,
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t2"  , SourceTypeCode ="5",OriginalJournalId="s1", OpenAmountCurrencyId="EUR"},

            };

            //A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(A<List<string>>.Ignored, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_OpenAmountis0ReconcileNot);
            var lineCode = @"if (ledgerTransactionPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
                        {
                            if (!ledgerTransactionPM.IsReconciled)
                            {
                                AddError(errorsList, M_InsertledgerTransactionbutIsnotReconciled);
                            }
    }";
            Assert.IsFalse(messageExist, lineCode);



        }



        [TestMethod]
        public void IsReconciliationValid019_OpenAmountGreater0ReconcileNot0toOpenAmout_ValidationresultError_M_OpenAmountGreater0ReconcileNot0toOpenAmout()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" , ChangeSetOp = ChangeSetOperation.Insert, ReconciliationAmount=150},
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t2" , ChangeSetOp = ChangeSetOperation.Insert},

                },


            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });


            List<string> transactionsId = reconciliationPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(tt => tt.TransactionId).ToList();

            var ltList = new List<LedgerTransactionPM>() {
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="5", OriginalJournalId="s1" , OpenAmountCurrencyId="USD" , OpenAmount=100} ,
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t2"  , SourceTypeCode ="5",OriginalJournalId="s1", OpenAmountCurrencyId="EUR"},

            };

            //A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(A<List<string>>.Ignored, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_OpenAmountGreater0ReconcileNot0toOpenAmout);
            var lineCode = @"else if (ledgerTransactionPM.OpenAmount > 0)
                        {
                            if (reconciliationLine.ReconciliationAmount <= 0 || reconciliationLine.ReconciliationAmount > ledgerTransactionPM.OpenAmount)
                            {
                                AddError(errorsList, M_OpenAmountGreater0ReconcileNot0toOpenAmout);
                            }
    }";
            Assert.IsTrue(messageExist, lineCode);



        }

        [TestMethod]
        public void IsReconciliationValid020_OpenAmountGreater0ReconcileNot0toOpenAmout_ValidationresultError_M_OpenAmountGreater0ReconcileNot0toOpenAmout()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" , ChangeSetOp = ChangeSetOperation.Insert, ReconciliationAmount=-1},
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t2" , ChangeSetOp = ChangeSetOperation.Insert},

                },


            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });


            List<string> transactionsId = reconciliationPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(tt => tt.TransactionId).ToList();

            var ltList = new List<LedgerTransactionPM>() {
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="5", OriginalJournalId="s1" , OpenAmountCurrencyId="USD" , OpenAmount=100} ,
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t2"  , SourceTypeCode ="5",OriginalJournalId="s1", OpenAmountCurrencyId="EUR"},

            };

            //A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(A<List<string>>.Ignored, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_OpenAmountGreater0ReconcileNot0toOpenAmout);
            var lineCode = @"else if (ledgerTransactionPM.OpenAmount > 0)
                        {
                            if (reconciliationLine.ReconciliationAmount <= 0 || reconciliationLine.ReconciliationAmount > ledgerTransactionPM.OpenAmount)
                            {
                                AddError(errorsList, M_OpenAmountGreater0ReconcileNot0toOpenAmout);
                            }
    }";
            Assert.IsTrue(messageExist, lineCode);



        }



        [TestMethod]
        public void IsReconciliationValid021_OpenAmountLess0ReconcileNoOpenAmoutto0_ValidationresultError_M_OpenAmountLess0ReconcileNoOpenAmoutto0()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" , ChangeSetOp = ChangeSetOperation.Insert, ReconciliationAmount=-150},
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t2" , ChangeSetOp = ChangeSetOperation.Insert},

                },


            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });


            List<string> transactionsId = reconciliationPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(tt => tt.TransactionId).ToList();

            var ltList = new List<LedgerTransactionPM>() {
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="5", OriginalJournalId="s1" , OpenAmountCurrencyId="USD" , OpenAmount=-100} ,
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t2"  , SourceTypeCode ="5",OriginalJournalId="s1", OpenAmountCurrencyId="USD"},

            };

            //A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(A<List<string>>.Ignored, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_OpenAmountLess0ReconcileNoOpenAmoutto0);
            var lineCode = @"else //if (ledgerTransactionPM.OpenAmount < 0)
                        {
                            if (reconciliationLine.ReconciliationAmount > 0 || reconciliationLine.ReconciliationAmount < ledgerTransactionPM.OpenAmount)
                            {
                                
                                AddError(errorsList, M_OpenAmountLess0ReconcileNoOpenAmoutto0 );
                            }
    }";
            Assert.IsTrue(messageExist, lineCode);



        }



        [TestMethod]
        public void IsReconciliationValid022_OpenAmountLess0ReconcileNoOpenAmoutto0_ValidationresultError_M_OpenAmountLess0ReconcileNoOpenAmoutto0()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" , ChangeSetOp = ChangeSetOperation.Insert, ReconciliationAmount=1},
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t2" , ChangeSetOp = ChangeSetOperation.Insert},

                },


            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });


            List<string> transactionsId = reconciliationPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(tt => tt.TransactionId).ToList();

            var ltList = new List<LedgerTransactionPM>() {
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="5", OriginalJournalId="s1" , OpenAmountCurrencyId="USD" , OpenAmount=-100} ,
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t2"  , SourceTypeCode ="5",OriginalJournalId="s1", OpenAmountCurrencyId="USD"},

            };

            //A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(A<List<string>>.Ignored, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_OpenAmountLess0ReconcileNoOpenAmoutto0);
            var lineCode = @"else //if (ledgerTransactionPM.OpenAmount < 0)
                        {
                            if (reconciliationLine.ReconciliationAmount > 0 || reconciliationLine.ReconciliationAmount < ledgerTransactionPM.OpenAmount)
                            {
                                
                                AddError(errorsList, M_OpenAmountLess0ReconcileNoOpenAmoutto0 );
                            }
    }";
            Assert.IsTrue(messageExist, lineCode);



        }



        [TestMethod]
        public void IsReconciliationValid023_sumofAmounttoreconcilenOT0_ValidationresultError_M_sumofAmounttoreconcilemUST0()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" , ChangeSetOp = ChangeSetOperation.Insert, ReconciliationAmount=1},
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t2" , ChangeSetOp = ChangeSetOperation.Insert},

                },


            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });


            List<string> transactionsId = reconciliationPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(tt => tt.TransactionId).ToList();

            var ltList = new List<LedgerTransactionPM>() {
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="5", OriginalJournalId="s1" , OpenAmountCurrencyId="USD" , OpenAmount=-100} ,
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t2"  , SourceTypeCode ="5",OriginalJournalId="s1", OpenAmountCurrencyId="USD"},

            };

            //A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(A<List<string>>.Ignored, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNotNull(validationresult);
            Assert.IsNotNull(validationresult.ErrorMessage);
            //var errList = new List<String>(validationresult.MemberNames);
            var messageExist = validationresult.ErrorMessage.Contains(ReconciliationValidator.M_sumofAmounttoreconcilemUST0);
            var lineCode = @"if (sum != 0)
            {
                
                AddError(errorsList, M_sumofAmounttoreconcilemUST0);
            }";
            Assert.IsTrue(messageExist, lineCode);



        }




        [TestMethod]
        public void IsReconciliationValid024_pERFECT_ValidationresultError_isnull()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" , ChangeSetOp = ChangeSetOperation.Insert, ReconciliationAmount=100},
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t2" , ChangeSetOp = ChangeSetOperation.Insert, ReconciliationAmount=-100},

                },


            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });


            List<string> transactionsId = reconciliationPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(tt => tt.TransactionId).ToList();

            var ltList = new List<LedgerTransactionPM>() {
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="5", OriginalJournalId="s1" , OpenAmountCurrencyId="USD" , OpenAmount=100} ,
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t2"  , SourceTypeCode ="5",OriginalJournalId="s1", OpenAmountCurrencyId="USD", OpenAmount=-100},

            };

            //A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(A<List<string>>.Ignored, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNull(validationresult);
        }




        [TestMethod]
        public void IsReconciliationValid025_pERFECT_ValidationresultError_isnull()
        {
            int myTenant = 1;

            var reconciliationPM = new ReconciliationPM()
            {
                Tenant = myTenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ReconciliationLines = new List<ReconciliationLinePM>()
                {
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t1" , ChangeSetOp = ChangeSetOperation.Insert, ReconciliationAmount=100},
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t2" , ChangeSetOp = ChangeSetOperation.Insert, ReconciliationAmount=-50},
                    new ReconciliationLinePM(){  CurrencyId ="USD", TransactionId ="t3" , ChangeSetOp = ChangeSetOperation.Insert, ReconciliationAmount=-50},

                },


            };
            var fakeIReconciliationValidatorContextDataProvider = A.Fake<IReconciliationValidatorContextDataProvider>();

            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetGLAccount("AccId1", myTenant))
            .Returns<GLAccountPM>(new GLAccountPM() { Id = "AccId1", Tenant = myTenant });


            List<string> transactionsId = reconciliationPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(tt => tt.TransactionId).ToList();

            var ltList = new List<LedgerTransactionPM>() {
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t1"  , SourceTypeCode ="5", OriginalJournalId="s1" , OpenAmountCurrencyId="USD" , OpenAmount=100} ,
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t2"  , SourceTypeCode ="5",OriginalJournalId="s1", OpenAmountCurrencyId="USD", OpenAmount=-50},
                new LedgerTransactionPM() { Tenant = myTenant ,Id="t3"  , SourceTypeCode ="5",OriginalJournalId="s1", OpenAmountCurrencyId="USD", OpenAmount=-50},

            };

            //A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            A.CallTo(() => fakeIReconciliationValidatorContextDataProvider.GetLedgerTransactionPMsByIdList(A<List<string>>.Ignored, myTenant)).Returns<List<LedgerTransactionPM>>(ltList);
            var a = MyTestInitialize(reconciliationPM, fakeIReconciliationValidatorContextDataProvider);
            var validationresult = ReconciliationValidator.IsReconciliationValid(reconciliationPM, a);
            Assert.IsNull(validationresult);
        }
    }
}
