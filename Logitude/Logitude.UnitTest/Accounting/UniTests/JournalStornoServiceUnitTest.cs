using Logitude.Accounting.Def.EntityPMs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.CoreBL;
using Logitude.UnitTest.Utils;
using FakeItEasy;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Def.EntityUpdateServicesExt;

namespace Logitude.UnitTest.Accounting.UniTests
{
    [TestClass]
    public class JournalStornoServiceUnitTest
    {
        private DateTime _accDate;
        private string refExt;
        private DateTime DateTimeNow;
        private string _Id;
        private DateTime _AccountDateClose;
        public JournalStornoServiceUnitTest()
        {
            _accDate = new DateTime(2016, 10, 1);
            _AccountDateClose = new DateTime(2016, 06, 1);
            DateTimeNow = new DateTime(2017, 1, 1);
            refExt = "refExt";
        }
        [TestMethod]
        public void CreateStornoAndCommitUpdate_StornoOverrideMIsMust()
        {

            JournalPM entityPM = GetDefaultJournal();

            TestsUtil.AssertThrows<Exception>(
                () =>
                {
                    StornoOverrideM stornoOverrideM = null;
                    var myJournalStornoService = new JournalStornoService();

                    myJournalStornoService.Init(entityPM, stornoOverrideM, null,null);
                    myJournalStornoService.CreateStornoAndCommitUpdate();
                },
                 "stornoOverrideM is must (good2 remember values in properties r not Must )"
            );

        }




        [TestMethod]
        public void CreateStornoAndCommitUpdate_JournalNotChange()
        {

            JournalPM baseJournal = GetDefaultJournal();
            JournalPM defaultJournal = GetDefaultJournal();
            //var JournalStornoService = new JournalStornoService(baseJournal, new StornoOverrideM());
            var fakeJournalStornoService = GetJournalStornoService(baseJournal, new StornoOverrideM());
            fakeJournalStornoService.CreateStornoAndCommitUpdate();

            Assert.AreEqual(defaultJournal.AccountingDate, baseJournal.AccountingDate);
            Assert.AreEqual(defaultJournal.Tenant, baseJournal.Tenant);

            Assert.AreEqual(defaultJournal.ExternalNo, baseJournal.ExternalNo);


            Assert.AreEqual(2, defaultJournal.JournalLines.Count);
            foreach (var jlDefault in defaultJournal.JournalLines)
            {

                var jlStorno = baseJournal.JournalLines.First(r => r.Line == jlDefault.Line);

                Assert.AreEqual(Simplog.Server.Infrastructure.ChangeSetOperation.Insert, jlStorno.ChangeSetOp);
                Assert.AreEqual(jlStorno.AccountingDate, jlDefault.AccountingDate);
                Assert.AreEqual(jlStorno.CurrencyId, jlDefault.CurrencyId);
                Assert.AreEqual(jlStorno.CreditAccountId, jlDefault.CreditAccountId);

                Assert.AreEqual(jlStorno.DebitAccountId, jlDefault.DebitAccountId);
                Assert.AreEqual(jlStorno.DocumentDate, jlDefault.DocumentDate);
                Assert.AreEqual(jlStorno.DueDate, jlDefault.DueDate);

                Assert.AreEqual(jlStorno.ExchangeRate, jlDefault.ExchangeRate);
                Assert.AreEqual(jlStorno.JournalId, jlDefault.JournalId);
                Assert.AreEqual(jlStorno.Line, jlDefault.Line);
                Assert.AreEqual(jlStorno.Tenant, jlDefault.Tenant);
                Assert.AreEqual(jlStorno.Reference1, jlDefault.Reference1);

                Assert.AreEqual(jlStorno.Reference2, jlDefault.Reference2);
                Assert.AreEqual(jlStorno.Reference3, jlDefault.Reference3);


                Assert.AreEqual(jlStorno.Notes, jlDefault.Notes);



                Assert.AreEqual(jlStorno.ForeignAmount, jlDefault.ForeignAmount);
                Assert.AreEqual(jlStorno.LocalAmount, jlDefault.LocalAmount);
            }



            Assert.AreEqual(defaultJournal.Id, baseJournal.Id);
            Assert.AreEqual(defaultJournal.OriginalJournalId, baseJournal.OriginalJournalId);
            Assert.AreEqual("2", baseJournal.StatusCode);


            Assert.AreEqual(defaultJournal.QueueId, baseJournal.QueueId);
            Assert.AreEqual(defaultJournal.JournalNumber, baseJournal.JournalNumber);


            Assert.AreEqual(defaultJournal.AccountingEntityReference, baseJournal.AccountingEntityReference);
            Assert.AreEqual(defaultJournal.AccountingEntityId, baseJournal.AccountingEntityId);
            Assert.AreEqual(defaultJournal.AccountingEntityCode, baseJournal.AccountingEntityCode);
            Assert.AreEqual(defaultJournal.StatusCode, baseJournal.StatusCode);





        }

        private JournalStornoService GetJournalStornoService(JournalPM baseJournal, StornoOverrideM myStornoOverrideM)
        {

            var fakeJournalUpdateService = A.Fake<IJournalUpdateService>();

            var fakeJournalStornoPrepareJReconcileService = A.Fake<IJournalStornoPrepareJReconcileService>();

            A.CallTo(
                () => fakeJournalStornoPrepareJReconcileService.CreateJournalReconcileFromStorno(A<JournalPM>.Ignored))
                //.Returns<bool>( return false; );
                .ReturnsLazily(
                (JournalPM j) =>
                {
                    return false;
                }
                );
                
            


            A.CallTo(fakeJournalUpdateService)
                .Where( call=> call.Method.Name=="Update")
            //A.CallTo(() => fakeJournalUpdateService.Update(baseJournal, true))
                .DoesNothing();
                
            var fakeJournalStornoService = A.Fake<JournalStornoService>();
                //opt => opt.CallsBaseMethods()
                //    .WithArgumentsForConstructor(new List<object>() { baseJournal, myStornoOverrideM, fakeJournalUpdateService })
                //    );
            fakeJournalStornoService.Init(baseJournal, myStornoOverrideM, fakeJournalUpdateService
                , fakeJournalStornoPrepareJReconcileService
                );
            var typeregular = "1"; //1	Regular	רגיל	1,Regular,רגיל	0


            A.CallTo(() => fakeJournalStornoService.GetAccountingPeriodByType(typeregular, baseJournal.Tenant))
                .Returns(new List<AccountingPeriodPM>()
                    {
                        new AccountingPeriodPM { PeriodTypeCode= typeregular, Tenant=baseJournal.Tenant, 
                            Year =baseJournal.AccountingDate.Date.Year,
                            OpenMonth=_accDate.Date.Month+1, 
                            ClosedMonth=_accDate.Date.Month-1, ///MonthClose !!!AccountingDate is DateTime(2016, 2, 20);
                        }
                    });

            A.CallTo(() => fakeJournalStornoService.TextCodesTranslatorTranslateText(JournalValidator.M_ClosedMonth, baseJournal.Tenant))
                .Returns(JournalValidator.M_ClosedMonth);
            
           
                
            
            return fakeJournalStornoService;
        }



        [TestMethod]
        public void CreateStorno_Regular()
        {

            JournalPM entityPM = GetDefaultJournal();
            JournalPM defaultJournal = GetDefaultJournal();
            var fakeJournalStornoService = GetJournalStornoService(entityPM, new StornoOverrideM());
            var storno = fakeJournalStornoService.CreateStornoAndCommitUpdate();

            Assert.IsNotNull(storno);
            Assert.IsNotNull(storno.JournalLines);

            Assert.AreEqual(defaultJournal.AccountingDate, storno.AccountingDate);
            Assert.AreEqual(defaultJournal.Tenant, storno.Tenant);

            Assert.AreEqual(defaultJournal.ExternalNo, storno.ExternalNo);


            Assert.AreEqual(2, defaultJournal.JournalLines.Count);
            foreach (var jlDefault in defaultJournal.JournalLines)
            {

                var jlStorno = storno.JournalLines.First(r => r.Line == jlDefault.Line);

                Assert.AreEqual(Simplog.Server.Infrastructure.ChangeSetOperation.Insert, jlStorno.ChangeSetOp);
                Assert.AreEqual(jlStorno.AccountingDate, jlDefault.AccountingDate);
                Assert.AreEqual(jlStorno.CurrencyId, jlDefault.CurrencyId);
                Assert.AreEqual(jlStorno.CreditAccountId, jlDefault.CreditAccountId);

                Assert.AreEqual(jlStorno.DebitAccountId, jlDefault.DebitAccountId);
                Assert.AreEqual(jlStorno.DocumentDate, jlDefault.DocumentDate);
                Assert.AreEqual(jlStorno.DueDate, jlDefault.DueDate);

                Assert.AreEqual(jlStorno.ExchangeRate, jlDefault.ExchangeRate);
                Assert.AreEqual(jlStorno.JournalId, jlDefault.JournalId);
                Assert.AreEqual(jlStorno.Line, jlDefault.Line);
                Assert.AreEqual(jlStorno.Tenant, jlDefault.Tenant);
                Assert.AreEqual(jlStorno.Reference1, jlDefault.Reference1);

                Assert.AreEqual(jlStorno.Reference2, jlDefault.Reference2);
                Assert.AreEqual(jlStorno.Reference3, jlDefault.Reference3);


                Assert.AreEqual(jlStorno.Notes, jlDefault.Notes);



                Assert.AreEqual(jlStorno.ForeignAmount, -1 * jlDefault.ForeignAmount);
                Assert.AreEqual(jlStorno.LocalAmount, -1 * jlDefault.LocalAmount);
            }
            Assert.AreEqual(null, storno.JournalNumber);



            Assert.AreEqual(defaultJournal.Id, storno.OriginalJournalId);
            Assert.AreEqual("2", storno.StatusCode);

            Assert.IsNull(storno.QueueId);
            Assert.AreNotEqual(defaultJournal.QueueId, storno.QueueId);
            Assert.AreNotEqual(defaultJournal.JournalNumber, storno.JournalNumber);


            Assert.AreEqual(defaultJournal.AccountingEntityReference, storno.AccountingEntityReference);
            Assert.AreEqual(defaultJournal.AccountingEntityId, storno.AccountingEntityId);
            Assert.AreEqual(defaultJournal.AccountingEntityCode, storno.AccountingEntityCode);
            Assert.AreEqual(defaultJournal.StatusCode, storno.StatusCode);





        }


        [TestMethod]
        public void CreateStorno_WithStornoOverrideM()
        {

            JournalPM entityPM = GetDefaultJournal();
            JournalPM defaultJournal = GetDefaultJournal();

            var stornoOverrideM = new StornoOverrideM()
                {
                    AccountingEntityCode = "myAccountingEntityCode",
                    AccountingEntityId = "myAccountingEntityId",
                    AccountingEntityReference = "OverrideAccountingEntityId"
                };
            var fakeJournalStornoService = GetJournalStornoService(entityPM, stornoOverrideM);
            var storno = fakeJournalStornoService.CreateStornoAndCommitUpdate();
            //var storno = entityPM.CreateStorno(stornoOverrideM);
            Assert.IsNotNull(storno);
            Assert.IsNotNull(storno.JournalLines);

            Assert.AreEqual(defaultJournal.AccountingDate, storno.AccountingDate);
            Assert.AreEqual(defaultJournal.Tenant, storno.Tenant);



            Assert.AreEqual(defaultJournal.TypeCode, storno.TypeCode);

            Assert.AreEqual(2, defaultJournal.JournalLines.Count);

            Assert.AreEqual(null, storno.JournalNumber);


            Assert.AreEqual(stornoOverrideM.AccountingEntityReference, storno.AccountingEntityReference);
            Assert.AreEqual(stornoOverrideM.AccountingEntityId, storno.AccountingEntityId);
            Assert.AreEqual(stornoOverrideM.AccountingEntityCode, storno.AccountingEntityCode);
            Assert.AreEqual(defaultJournal.StatusCode, storno.StatusCode);


        }


        [TestMethod]
        public void CreateStorno_WithStornoOverrideM_AccountingDate()
        {

            JournalPM entityPM = GetDefaultJournal();
            JournalPM defaultJournal = GetDefaultJournal();

            var stornoOverrideM = new StornoOverrideM()
            {
                AccountingEntityCode = "myAccountingEntityCode",
                AccountingEntityId = "myAccountingEntityId",
                AccountingEntityReference = "OverrideAccountingEntityId",

                AccountingDate = new DateTime(2019, 11, 11),
                LineNotes= "override Notes"
            };
            var fakeJournalStornoService = GetJournalStornoService(entityPM, stornoOverrideM);
            var storno = fakeJournalStornoService.CreateStornoAndCommitUpdate();
            //var storno = entityPM.CreateStorno(stornoOverrideM);
            Assert.IsNotNull(storno);
            Assert.IsNotNull(storno.JournalLines);

            
            Assert.AreEqual(defaultJournal.Tenant, storno.Tenant);



            Assert.AreEqual(defaultJournal.TypeCode, storno.TypeCode);

            Assert.AreEqual(2, defaultJournal.JournalLines.Count);

            Assert.AreEqual(null, storno.JournalNumber);


            Assert.AreEqual(stornoOverrideM.AccountingEntityReference, storno.AccountingEntityReference);
            Assert.AreEqual(stornoOverrideM.AccountingEntityId, storno.AccountingEntityId);
            Assert.AreEqual(stornoOverrideM.AccountingEntityCode, storno.AccountingEntityCode);
            Assert.AreEqual(stornoOverrideM.AccountingDate, storno.AccountingDate);
            Assert.IsTrue(storno.JournalLines.TrueForAll(r => r.Notes == stornoOverrideM.LineNotes));


        }
        private JournalPM GetDefaultJournal()
        {

            var j = new Logitude.Accounting.Def.EntityPMs.JournalPM()
            {
                Id = _Id,
                AccountingDate = _accDate,
                Tenant = 989,
                JournalNumber = "1003",
                //StatusCode = "2",
                TypeCode = "0",//0,Manual,ידנית
                AccountingEntityCode = "1",//1	פקודת יומן	Journal
                StatusCodeEnum = Logitude.Accounting.Def.EntityPMs.JournalStatusTypePM.StatusCodeEnum.Approved,
                UpdatedByUserId = "1-1",
                UpdateDate = DateTimeNow,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                CreateDate = DateTimeNow,
                CreatedByUserId = "1-1",
                AccountingEntityReference = "AER" + refExt,
                AccountingEntityId = "1-69",

                QueueId = "QueueId",

            };



            j.JournalLines = new List<Logitude.Accounting.Def.EntityPMs.JournalLinePM> 
                    {
                        new Logitude.Accounting.Def.EntityPMs.JournalLinePM
                    {
                        
                      AccountingDate = j.AccountingDate,
                      //ActionName = "1", 
                      ActionTypeCodeEnum= MyJournalActionTypeEnum.Credit,
                      ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert, 
                      CreditAccountId = "GetCreditAccountId()",
                      //DebitAccountId = "35",
                      DebitAccountId = null,
  
                      CurrencyId ="USD",
                      DocumentDate=DateTimeNow, 
                      DueDate= DateTimeNow, 
                      ForeignAmount = 25, 
                      LocalAmount = 100, 
                      ExchangeRate=4,
                      JournalId=j.Id, 
                      Line=1, 
                      Tenant=j.Tenant
                      ,
                      Reference1="ref1" +refExt ,
                      Reference2="ref2" +refExt ,
                      Reference3="ref3" +refExt ,
                      Notes ="Notes" +refExt ,
                    },
                        new Logitude.Accounting.Def.EntityPMs.JournalLinePM
                    {
                      AccountingDate = j.AccountingDate,
                      //ActionName = "2", 
                      ActionTypeCodeEnum= MyJournalActionTypeEnum.Debit,
                      ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert, 
                      //CreditAccountId = "35", 
                      CurrencyId =("USD"),
                      CreditAccountId = null, 
                      DebitAccountId = "GetDebitAccountId()",
                      DocumentDate=DateTimeNow, 
                      DueDate= DateTimeNow, 
                      ForeignAmount = 25, 
                      LocalAmount = 100, 
                      ExchangeRate=4,
                      JournalId=j.Id, 
                      Line=2, 
                      Tenant=j.Tenant
                      ,
                      Reference1="ref1" +refExt ,
                      Reference2="ref2" +refExt ,
                      Reference3="ref3" +refExt ,
                      Notes ="Notes" +refExt ,
                    },
                    
                  
                    };
            return j;
        }


        [TestMethod]
        public void OnUpdating_VoidActionWhileMonthClose_throwscloseMonth()
        {
            ///arrange 
            JournalPM baseJournal = GetDefaultJournal();
            JournalPM defaultJournal = GetDefaultJournal();

            baseJournal.AccountingDate = _AccountDateClose;
            //var JournalStornoService = new JournalStornoService(baseJournal, new StornoOverrideM());
            var fakeJournalStornoService = GetJournalStornoService(baseJournal, new StornoOverrideM());













            ///Act
            ///


            TestsUtil.AssertThrows<Exception>(() =>
            {
                //fakeJournalUpdateOnUpdating.OnUpdating(JPM, JournalPoco, mychangTrack);
                fakeJournalStornoService.CreateStornoAndCommitUpdate();
            },
                JournalValidator.M_ClosedMonth,
                "if (string.IsNullOrWhiteSpace(Storno.Id))");

            //check 


        }
    }



}

