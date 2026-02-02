using FakeItEasy;
using Logitude.Accounting.BL;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.UnitTest.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.UnitTest.Accounting.UniTests
{
    // Uses TestClass on main partial; keep this partial attribute-less to avoid duplicates.
    public partial class JournalUpdateOnUpdatingUnderTest
    {
        [TestMethod]
        public void OnUpdating_UpdateVoidActionReturnStornoWithEmptyID_ThrowM_CreateStornoAndSaveFailed()
        {
            ///arrange 
            var JournalId = "1-1";
            var tenant = 1;
            var createdAt = new DateTime(2016, 01, 01);
            var DateTimeNow = new DateTime(2016, 01, 30);
            var periodTypeCode = "1"; //regelur 
            var accountingPeriodByType = new List<AccountingPeriodPM>()
            {
                new AccountingPeriodPM(){ Tenant=tenant, 
                    PeriodTypeCode=periodTypeCode, 
                    Year= DateTimeNow.Date.Year,
                    ClosedMonth=0, OpenMonth =3}
            };

            var JournalPoco = new Journal()
            {
                Id = JournalId,
                Tenant = tenant,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.InProcessing).ToString(),

            };



            var JournalLinePM = new JournalLinePM()
            {
                JournalId = JournalId,
                Tenant = tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.None
            };

            var JPM = new JournalPM()
            {
                Id = JournalId,
                Tenant = tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                AccountingDate = createdAt,
                JournalLines = new List<JournalLinePM>() { JournalLinePM },
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.Voided).ToString(),
                //OriginalJournalId = "This is A strono"
            };
            var stornoPM = new JournalPM()
            {
                Id = "1-2",
                Tenant = tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                UpdateDate = DateTimeNow,
                CreateDate = DateTimeNow,
                JournalLines = new List<JournalLinePM>() { JournalLinePM },
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.InProcessing).ToString(),
                QueueId = "QueueId",//Streaamed !!!
            };

            //foreach (var myPM in checkes)
            {

                var fakeJournalStornoService = A.Fake<IJournalStornoService>();
                stornoPM.Id = "";//CauseExcption
                A.CallTo(() => fakeJournalStornoService.CreateStornoAndCommitUpdate())
                    .Returns(stornoPM);


                var fakeJournalUpdateOnUpdating = A.Fake<JournalUpdateOnUpdating>(
                    opt => opt.WithArgumentsForConstructor(new List<object>() { null, fakeJournalStornoService })
                    );
                var fakeJournalLineOnUpdate = A.Fake<JournalLineOnUpdate>();
                //A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM, JPM))
                //    .Invokes(() => { fakeJournalLineOnUpdateOnUpdate_done = true; });

                A.CallTo(() => fakeJournalUpdateOnUpdating.CreateJournalLineOnUpdate())
                    .Returns(fakeJournalLineOnUpdate);




                int step_JournalApproveParser = 1;





                ///Act
                ///
                var mychangTrack = GetChangeTarckOK(JournalId, tenant, createdAt);

                TestsUtil.AssertThrows<Exception>(() =>
                {
                    JournalPoco.QueueId = "QueueId";
                    fakeJournalUpdateOnUpdating.OnUpdating(JPM, JournalPoco, mychangTrack);
                },
                    JournalUpdateOnUpdating.M_CreateStornoAndSaveFailed,
                    "if (string.IsNullOrWhiteSpace(Storno.Id))");

                //check 


            }
        }




        [TestMethod]
        public void OnUpdating_VoidActionRetrurnGoodStorno_FillVoidedFields()
        {
            ///arrange 
            var JournalId = "1-1"; int tenant;
            DateTime createdAt;
            Journal JournalPoco;
            JournalPM JPM;
            JournalPM stornoPM;
            GetContext4Storno(JournalId, out tenant, out createdAt, out JournalPoco, out JPM, out stornoPM);


            var fakeJournalStornoService = A.Fake<IJournalStornoService>();
            stornoPM.Id = "1-2";//not CauseExcption
            A.CallTo(() => fakeJournalStornoService.CreateStornoAndCommitUpdate())
                .Returns(stornoPM);


            var fakeJournalUpdateOnUpdating = A.Fake<JournalUpdateOnUpdating>(
                opt => opt.WithArgumentsForConstructor(new List<object>() { null, fakeJournalStornoService })
                );
            var fakeJournalLineOnUpdate = A.Fake<JournalLineOnUpdate>();
            //A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM, JPM))
            //    .Invokes(() => { fakeJournalLineOnUpdateOnUpdate_done = true; });

            A.CallTo(() => fakeJournalUpdateOnUpdating.CreateJournalLineOnUpdate())
                .Returns(fakeJournalLineOnUpdate);

            A.CallTo(() => fakeJournalUpdateOnUpdating.GetLogContactId(JPM))
                .Returns("LogContactId");



            

            




            ///Act
            ///
            var mychangTrack = GetChangeTarckOK(JournalId, tenant, createdAt);


            fakeJournalUpdateOnUpdating.OnUpdating(JPM, JournalPoco, mychangTrack);

            //check 

            Assert.AreEqual(stornoPM.Id, JPM.VoidedByJournalId);
            Assert.AreEqual(true, JPM.IsVoided);
            Assert.AreEqual("LogContactId", JPM.VoidedByUserId);
            Assert.IsNotNull(JPM.VoidDate);
        }

        [TestMethod]
        public void OnUpdating_JournalNotStreamToAccounting_ThrowException()
        {
            ///arrange 
            var JournalId = "1-1"; int tenant;
            DateTime createdAt;
            Journal JournalPoco;
            JournalPM JPM;
            JournalPM stornoPM;
            GetContext4Storno(JournalId, out tenant, out createdAt, out JournalPoco, out JPM, out stornoPM);
            

            var fakeJournalStornoService = A.Fake<IJournalStornoService>();
            stornoPM.Id = "1-2";//not CauseExcption
            A.CallTo(() => fakeJournalStornoService.CreateStornoAndCommitUpdate())
                .Returns(stornoPM);


            var fakeJournalUpdateOnUpdating = A.Fake<JournalUpdateOnUpdating>(
                opt => opt.WithArgumentsForConstructor(new List<object>() { null, fakeJournalStornoService })
                );
            var fakeJournalLineOnUpdate = A.Fake<JournalLineOnUpdate>();
            //A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM, JPM))
            //    .Invokes(() => { fakeJournalLineOnUpdateOnUpdate_done = true; });

            A.CallTo(() => fakeJournalUpdateOnUpdating.CreateJournalLineOnUpdate())
                .Returns(fakeJournalLineOnUpdate);

            A.CallTo(() => fakeJournalUpdateOnUpdating.GetLogContactId(JPM))
                .Returns("LogContactId");

            A.CallTo(() => fakeJournalUpdateOnUpdating.GetLoggedContact(tenant))
                .Returns(new BL.CommonDataModel.EntityPMs.ContactPM() { });



            

            A.CallTo(() => fakeJournalUpdateOnUpdating.TranslateTextsClassTranslate
            //("1", tenant, 0, true)
            (A<string>.Ignored, A<int>.Ignored, A<bool>.Ignored))
                .ReturnsLazily(
                (string textCodeCode, int tenant1, bool getLocalDefaultText1) =>
                {
                    return textCodeCode;
                });
                

            




            ///Act
            ///
            var mychangTrack = GetChangeTarckOK(JournalId, tenant, createdAt);

            TestsUtil.AssertThrows<Exception>(
                () =>
                {
                    JournalPoco.QueueId = "";//JournalNotStreamToAccounting
                    fakeJournalUpdateOnUpdating.OnUpdating(JPM, JournalPoco, mychangTrack);
                },
                    
                    //"BLException :Approved NOT Streamed Journal Can Only Change To Failed"
                    "Accounting.O.CantVoidJouranlItDidntTurnedToTransactions"
                    );

            //check 

            
        }

        private static void GetContext4Storno(string JournalId, out int tenant, out DateTime createdAt, out Journal JournalPoco, out JournalPM JPM, out JournalPM stornoPM)
        {

            tenant = 1;
            createdAt = new DateTime(2016, 01, 01);

            var DateTimeNow = new DateTime(2016, 01, 30);
            var periodTypeCode = "1"; //regelur 
            var accountingPeriodByType = new List<AccountingPeriodPM>()
            {
                new AccountingPeriodPM(){ Tenant=tenant, 
                    PeriodTypeCode=periodTypeCode, 
                    Year= DateTimeNow.Date.Year,
                    ClosedMonth=0, OpenMonth =3}
            };

            JournalPoco = new Journal()
            {
                Id = JournalId,
                Tenant = tenant,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.InProcessing).ToString(),
                QueueId = "QueueId "

            };



            var JournalLinePM = new JournalLinePM()
            {
                JournalId = JournalId,
                Tenant = tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.None
            };

            JPM = new JournalPM()
            {
                Id = JournalId,
                Tenant = tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                AccountingDate = createdAt,
                JournalLines = new List<JournalLinePM>() { JournalLinePM },
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.Voided).ToString(),
                //OriginalJournalId = "This is A strono"
            };
            stornoPM = new JournalPM()
            {
                Id = "1-2",
                Tenant = tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                UpdateDate = DateTimeNow,
                CreateDate = DateTimeNow,
                JournalLines = new List<JournalLinePM>() { JournalLinePM },
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.InProcessing).ToString(),
            };



        }



    
    }
}

