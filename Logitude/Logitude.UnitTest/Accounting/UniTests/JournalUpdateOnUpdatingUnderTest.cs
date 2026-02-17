
    using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.Def.EntityPMs;
using FakeItEasy;
using Logitude.Accounting.BL;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.UnitTest.Utils;
using Logitude.Server.Tools;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.Validators;

namespace Logitude.UnitTest.Accounting.UniTests
{
    
    public partial class JournalUpdateOnUpdatingUnderTest
    {

        [TestMethod]
        public void OnUpdating_JournalUpdateOnCreatingLineOnUpdateHappned()
        {
            ///arrange 
            var JournalId = "1-1";
            var Tenant = 1;
            bool fakeJournalLineOnUpdateOnUpdate_done = false;
            var JPM = new JournalPM() { Id = JournalId, Tenant = Tenant, ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update };
            var JournalLinePM = new JournalLinePM() { JournalId = JournalId, Tenant = Tenant, ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update };
            JPM.JournalLines = new List<JournalLinePM>() { JournalLinePM };
            var JournalPoco = new Journal() { Id = JournalId, Tenant = Tenant };
            var fakeJournalUpdateOnUpdating = A.Fake<JournalUpdateOnUpdating>();
            var fakeJournalLineOnUpdate = A.Fake<JournalLineOnUpdate>();
            A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM, JPM))
                .Invokes(() => { fakeJournalLineOnUpdateOnUpdate_done = true; });

            A.CallTo(() => fakeJournalUpdateOnUpdating.CreateJournalLineOnUpdate())
                .Returns(fakeJournalLineOnUpdate);



            ///Act
            ///

            fakeJournalUpdateOnUpdating.OnUpdating(JPM, JournalPoco, new Server.Tools.EntityPM());

            //Check
            //A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM,JPM)).MustNotHaveHappened();    // 
            Assert.AreEqual(true, fakeJournalLineOnUpdateOnUpdate_done);

        }


        [TestMethod]
        public void OnUpdating_JournalPMChangeSetOpNone_ThrowsDontUpdateNothing()
        {
            ///arrange 
            var JournalId = "1-1";
            var Tenant = 1;
            bool fakeJournalLineOnUpdateOnUpdate_done = false;
            var JPM = new JournalPM()
            {
                Id = JournalId,
                Tenant = Tenant,
                //ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update 
            };
            var JournalLinePM = new JournalLinePM()
            {
                JournalId = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update
            };
            JPM.JournalLines = new List<JournalLinePM>() { JournalLinePM };
            var JournalPoco = new Journal() { Id = JournalId, Tenant = Tenant };
            var fakeJournalUpdateOnUpdating = A.Fake<JournalUpdateOnUpdating>();
            var fakeJournalLineOnUpdate = A.Fake<JournalLineOnUpdate>();
            A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM, JPM))
                .Invokes(() => { fakeJournalLineOnUpdateOnUpdate_done = true; });

            A.CallTo(() => fakeJournalUpdateOnUpdating.CreateJournalLineOnUpdate())
                .Returns(fakeJournalLineOnUpdate);

            TestsUtil.AssertThrows<Exception>(
                () =>
                {
                    ///Act
                    ///

                    fakeJournalUpdateOnUpdating.OnUpdating(JPM, JournalPoco, new Server.Tools.EntityPM());
                }
                    , "Don't Update Nothing", "if (journalPM.ChangeSetOp == ChangeSetOperation.None)");
            //Check
            //A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM,JPM)).MustNotHaveHappened();    // 


        }

        [TestMethod]
        public void OnUpdating_TryDeleteJournalPM_Throws()
        {
            ///arrange 
            var JournalId = "1-1";
            var Tenant = 1;
            bool fakeJournalLineOnUpdateOnUpdate_done = false;
            var JPM = new JournalPM()
            {
                Id = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete
            };
            var JournalLinePM = new JournalLinePM()
            {
                JournalId = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update
            };
            JPM.JournalLines = new List<JournalLinePM>() { JournalLinePM };
            var JournalPoco = new Journal() { Id = JournalId, Tenant = Tenant };
            var fakeJournalUpdateOnUpdating = A.Fake<JournalUpdateOnUpdating>();
            var fakeJournalLineOnUpdate = A.Fake<JournalLineOnUpdate>();
            A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM, JPM))
                .Invokes(() => { fakeJournalLineOnUpdateOnUpdate_done = true; });

            A.CallTo(() => fakeJournalUpdateOnUpdating.CreateJournalLineOnUpdate())
                .Returns(fakeJournalLineOnUpdate);

            TestsUtil.AssertThrows<Exception>(
                () =>
                {
                    ///Act
                    ///

                    fakeJournalUpdateOnUpdating.OnUpdating(JPM, JournalPoco, new Server.Tools.EntityPM());
                },
                     "I Don't think its good idea to delete Journal (ask yaron)",
                    "if (journalPM.ChangeSetOp == ChangeSetOperation.Delete)");
            //Check
            //A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM,JPM)).MustNotHaveHappened();    // 


        }


        [TestMethod]
        public void OnUpdating_JournalPOCOAlredyVoided_ThrowsJournalisvoided()
        {
            ///arrange 
            var JournalId = "1-1";
            var Tenant = 1;
            bool fakeJournalLineOnUpdateOnUpdate_done = false;
            var JPM = new JournalPM()
            {
                Id = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update
            };
            var JournalLinePM = new JournalLinePM()
            {
                JournalId = JournalId,
                Tenant = Tenant,

                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update
            };
            JPM.JournalLines = new List<JournalLinePM>() { JournalLinePM };
            var JournalPoco = new Journal()
            {
                Id = JournalId,
                Tenant = Tenant,
                StatusCode = "3",
            };
            var fakeJournalUpdateOnUpdating = A.Fake<JournalUpdateOnUpdating>();
            var fakeJournalLineOnUpdate = A.Fake<JournalLineOnUpdate>();
            A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM, JPM))
                .Invokes(() => { fakeJournalLineOnUpdateOnUpdate_done = true; });

            A.CallTo(() => fakeJournalUpdateOnUpdating.CreateJournalLineOnUpdate())
                .Returns(fakeJournalLineOnUpdate);

            TestsUtil.AssertThrows<Exception>(
                () =>
                {
                    ///Act
                    ///

                    fakeJournalUpdateOnUpdating.OnUpdating(JPM, JournalPoco, new Server.Tools.EntityPM());
                },
                     "Journal is voided (Change is not Allowed)",
                    "if (JournalPOCO.StatusCode == ((int)JournalStatusTypePM.StatusCodeEnum.Voided).ToString())");
            //Check
            //A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM,JPM)).MustNotHaveHappened();    // 


        }






        [TestMethod]
        public void OnUpdating_AccountingDateTimeTrancate2Date()
        {
            ///arrange 
            var JournalId = "1-1";
            var Tenant = 1;
            var createdAt = new DateTime(2016, 01, 01);
            var StatusCode = "1";
            var accDatetime = new DateTime(2017, 01, 12, 12, 35, 11);
            var JPM = new JournalPM()
            {
                Id = JournalId,
                Tenant = Tenant,
                AccountingDate =accDatetime ,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                StatusCode = StatusCode,
            };
            var JournalLinePM = new JournalLinePM() { JournalId = JournalId, Tenant = Tenant, ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update };
            JPM.JournalLines = new List<JournalLinePM>() { JournalLinePM };
            var JournalPoco = new Journal()
            {
                Id = JournalId,
                Tenant = Tenant,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                StatusCode = StatusCode,

            };
            var fakeJournalUpdateOnUpdating = A.Fake<JournalUpdateOnUpdating>();
            var fakeJournalLineOnUpdate = A.Fake<JournalLineOnUpdate>();
            //A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM, JPM))
            //    .Invokes(() => { fakeJournalLineOnUpdateOnUpdate_done = true; });

            A.CallTo(() => fakeJournalUpdateOnUpdating.CreateJournalLineOnUpdate())
                .Returns(fakeJournalLineOnUpdate);



            ///Act
            ///

            fakeJournalUpdateOnUpdating.OnUpdating(JPM, JournalPoco, new Server.Tools.EntityPM());

            //Check
            //A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM,JPM)).MustNotHaveHappened();    // 
            var excpted = new DateTime(2017, 01, 12);
            Assert.AreEqual(excpted, JPM.AccountingDate);
            

        }


        [TestMethod]
        public void OnUpdating_UdateDateChangeButCreatedDateNeverChange()
        {
            ///arrange 
            var JournalId = "1-1";
            var Tenant = 1;
            var createdAt = new DateTime(2016, 01, 01);
            var StatusCode = "1";

            var JPM = new JournalPM()
            {
                Id = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                StatusCode = StatusCode,
            };
            var JournalLinePM = new JournalLinePM() { JournalId = JournalId, Tenant = Tenant, ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update };
            JPM.JournalLines = new List<JournalLinePM>() { JournalLinePM };
            var JournalPoco = new Journal()
            {
                Id = JournalId,
                Tenant = Tenant,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                StatusCode = StatusCode,

            };
            var fakeJournalUpdateOnUpdating = A.Fake<JournalUpdateOnUpdating>();
            var fakeJournalLineOnUpdate = A.Fake<JournalLineOnUpdate>();
            //A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM, JPM))
            //    .Invokes(() => { fakeJournalLineOnUpdateOnUpdate_done = true; });

            A.CallTo(() => fakeJournalUpdateOnUpdating.CreateJournalLineOnUpdate())
                .Returns(fakeJournalLineOnUpdate);



            ///Act
            ///

            fakeJournalUpdateOnUpdating.OnUpdating(JPM, JournalPoco, new Server.Tools.EntityPM());

            //Check
            //A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM,JPM)).MustNotHaveHappened();    // 
            Assert.AreEqual(createdAt, JPM.CreateDate);
            Assert.AreNotEqual(createdAt, JPM.UpdateDate);

        }




        [TestMethod]
        public void OnUpdating_InsertVoidedJournal_ThrowsinsertJournalandimmediatllytovoided()
        {
            ///arrange 
            var JournalId = "1-1";
            var Tenant = 1;
            var createdAt = new DateTime(2016, 01, 01);
            var StatusCode = "3";

            var JPM = new JournalPM()
            {
                Id = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                StatusCode = StatusCode,
            };
            var JournalLinePM = new JournalLinePM() { JournalId = JournalId, Tenant = Tenant, ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update };
            JPM.JournalLines = new List<JournalLinePM>() { JournalLinePM };
            var JournalPoco = new Journal()
            {
                Id = JournalId,
                Tenant = Tenant,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                //StatusCode = StatusCode,

            };
            var fakeJournalUpdateOnUpdating = A.Fake<JournalUpdateOnUpdating>();
            var fakeJournalLineOnUpdate = A.Fake<JournalLineOnUpdate>();
            //A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM, JPM))
            //    .Invokes(() => { fakeJournalLineOnUpdateOnUpdate_done = true; });

            A.CallTo(() => fakeJournalUpdateOnUpdating.CreateJournalLineOnUpdate())
                .Returns(fakeJournalLineOnUpdate);

            TestsUtil.AssertThrows<Exception>(
                () =>
                {

                    ///Act
                    ///

                    fakeJournalUpdateOnUpdating.OnUpdating(JPM, JournalPoco, new Server.Tools.EntityPM());
                },
                //Check
            "I Don't think its good idea to insert Journal and immediatlly to voided him ?!?!?(ask yaron)",
            "if (journalPM.StatusCode == ((int)JournalStatusTypePM.StatusCodeEnum.Voided).ToString())");


        }




        [TestMethod]
        public void OnUpdating_NotApprovedStatusJournalMakeVoided_ThrowsVoidACTIONcanonlyaffectApprovedjournal()
        {
            ///arrange 
            var JournalId = "1-1";
            var Tenant = 1;
            var createdAt = new DateTime(2016, 01, 01);

            var checkes = new List<Tuple<Journal>>();
            var JPM = new JournalPM()
            {
                Id = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                StatusCode = "3",
            };
            var JournalLinePM = new JournalLinePM() { JournalId = JournalId, Tenant = Tenant, ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update };
            JPM.JournalLines = new List<JournalLinePM>() { JournalLinePM };
            var JournalPoco = new Journal()
            {
                Id = JournalId,
                Tenant = Tenant,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.Draft).ToString(),

            };

            checkes.Add(new Tuple<Journal>(JournalPoco));



            JournalPoco = new Journal()
            {
                Id = JournalId,
                Tenant = Tenant,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.Draft).ToString(),

            };
            checkes.Add(new Tuple<Journal>(JournalPoco));



            JournalPoco = new Journal()
            {
                Id = JournalId,
                Tenant = Tenant,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.WaitingforApprove).ToString(),

            };
            checkes.Add(new Tuple<Journal>(JournalPoco));
            foreach (var tuple in checkes)
            {


                var fakeJournalUpdateOnUpdating = A.Fake<JournalUpdateOnUpdating>();
                var fakeJournalLineOnUpdate = A.Fake<JournalLineOnUpdate>();
                //A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM, JPM))
                //    .Invokes(() => { fakeJournalLineOnUpdateOnUpdate_done = true; });

                A.CallTo(() => fakeJournalUpdateOnUpdating.CreateJournalLineOnUpdate())
                    .Returns(fakeJournalLineOnUpdate);

                TestsUtil.AssertThrows<Exception>(
                    () =>
                    {

                        ///Act
                        ///

                        JournalPoco = tuple.Item1;
                        fakeJournalUpdateOnUpdating.OnUpdating(JPM, JournalPoco, new Server.Tools.EntityPM());
                    },
                    //Check
                "BLException :Void ACTION can only affect Approved journal",
                "if (journalPM.StatusCode == ((int)JournalStatusTypePM.StatusCodeEnum.Voided).ToString())");

            }
        }






        [TestMethod]
        public void OnUpdating_pocoApprovedPmCanOnlyVoided_ThrowsApprovedJournalCanOnlyChangeToVoided()
        {
            ///arrange 
            var JournalId = "1-1";
            var Tenant = 1;
            var createdAt = new DateTime(2016, 01, 01);


            var JournalPoco = new Journal()
            {
                Id = JournalId,
                Tenant = Tenant,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.InProcessing).ToString(),
                 QueueId ="StreamedtoAcc"

            };

            var JournalLinePM = new JournalLinePM() { JournalId = JournalId, Tenant = Tenant, ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update };
            var checkes = new List<Tuple<JournalPM>>();
            var JPM = new JournalPM()
            {
                Id = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                JournalLines = new List<JournalLinePM>() { JournalLinePM },
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.Draft).ToString(),
            };
            checkes.Add(new Tuple<JournalPM>(JPM));



            JPM = new JournalPM()
            {
                Id = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                JournalLines = new List<JournalLinePM>() { JournalLinePM },
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.WaitingforApprove).ToString(),
            };
            checkes.Add(new Tuple<JournalPM>(JPM));



            foreach (var tuple in checkes)
            {


                var fakeJournalUpdateOnUpdating = A.Fake<JournalUpdateOnUpdating>();
                var fakeJournalLineOnUpdate = A.Fake<JournalLineOnUpdate>();
                //A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM, JPM))
                //    .Invokes(() => { fakeJournalLineOnUpdateOnUpdate_done = true; });

                A.CallTo(() => fakeJournalUpdateOnUpdating.CreateJournalLineOnUpdate())
                    .Returns(fakeJournalLineOnUpdate);

                TestsUtil.AssertThrows<Exception>(
                    () =>
                    {

                        ///Act
                        ///

                        JPM = tuple.Item1;
                        fakeJournalUpdateOnUpdating.OnUpdating(JPM, JournalPoco, new Server.Tools.EntityPM());
                    },
                //Check
                //"BLException :Approved Journal Can Only Change To Voided",
                "BLException :Approved Streamed Journal Can Only Change To Voided",
                "if (journalPM.StatusCode != ((int)JournalStatusTypePM.StatusCodeEnum.Voided).ToString())");

            }
        }




        [TestMethod]
        public void OnUpdating_pocoApprovedTryVoidedButIsStrono_ThrowsCannotStronoJournaltoStronoJournal()
        {
            ///arrange 
            var JournalId = "1-1";
            var Tenant = 1;
            var createdAt = new DateTime(2016, 01, 01);


            var JournalPoco = new Journal()
            {
                Id = JournalId,
                Tenant = Tenant,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.InProcessing).ToString(),
                QueueId = "StreamedtoAcc"

            };

            var JournalLinePM = new JournalLinePM() { JournalId = JournalId, Tenant = Tenant, ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update };
            var checkes = new List<Tuple<JournalPM>>();
            var JPM = new JournalPM()
            {
                Id = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                JournalLines = new List<JournalLinePM>() { JournalLinePM },
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.Voided).ToString(),
                OriginalJournalId = "This is A strono"
            };
            checkes.Add(new Tuple<JournalPM>(JPM));



            foreach (var tuple in checkes)
            {


                var fakeJournalUpdateOnUpdating = A.Fake<JournalUpdateOnUpdating>();
                var fakeJournalLineOnUpdate = A.Fake<JournalLineOnUpdate>();
                //A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM, JPM))
                //    .Invokes(() => { fakeJournalLineOnUpdateOnUpdate_done = true; });

                A.CallTo(() => fakeJournalUpdateOnUpdating.CreateJournalLineOnUpdate())
                    .Returns(fakeJournalLineOnUpdate);

                TestsUtil.AssertThrows<Exception>(
                    () =>
                    {

                        ///Act
                        ///

                        JPM = tuple.Item1;
                        fakeJournalUpdateOnUpdating.OnUpdating(JPM, JournalPoco, new Server.Tools.EntityPM());
                    },
                    //Check
                "BLException :Can not create Strono Journal to Strono Journal ",
                "if (!String.IsNullOrWhiteSpace(journalPM.OriginalJournalId))");

            }
        }



        [TestMethod]
        public void OnUpdating_pocoIsVoided_ThrowsentityPOCOIsVoided()
        {
            ///arrange 
            var JournalId = "1-1";
            var Tenant = 1;
            var createdAt = new DateTime(2016, 01, 01);


            var JournalPoco = new Journal()
            {
                Id = JournalId,
                Tenant = Tenant,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.InProcessing).ToString(),
                IsVoided = true,
                QueueId = "StreamedtoAcc"
            };

            var JournalLinePM = new JournalLinePM() { JournalId = JournalId, Tenant = Tenant, ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update };
            var checkes = new List<Tuple<JournalPM>>();
            var JPM = new JournalPM()
            {
                Id = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                JournalLines = new List<JournalLinePM>() { JournalLinePM },
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.Voided).ToString(),
                //OriginalJournalId = "This is A strono"
            };
            checkes.Add(new Tuple<JournalPM>(JPM));



            foreach (var tuple in checkes)
            {


                var fakeJournalUpdateOnUpdating = A.Fake<JournalUpdateOnUpdating>();
                var fakeJournalLineOnUpdate = A.Fake<JournalLineOnUpdate>();
                //A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM, JPM))
                //    .Invokes(() => { fakeJournalLineOnUpdateOnUpdate_done = true; });

                A.CallTo(() => fakeJournalUpdateOnUpdating.CreateJournalLineOnUpdate())
                    .Returns(fakeJournalLineOnUpdate);

                TestsUtil.AssertThrows<Exception>(
                    () =>
                    {

                        ///Act
                        ///

                        JPM = tuple.Item1;
                        fakeJournalUpdateOnUpdating.OnUpdating(JPM, JournalPoco, new Server.Tools.EntityPM());
                    },
                    //Check
                "already entityPOCO.IsVoided.GetValueOrDefault() ?!?!?",
                "if (JournalPOCO.IsVoided.GetValueOrDefault())");

            }
        }





        [TestMethod]
        public void OnUpdating_ApprovedPocoTryVoidedVButChangeTrack_ThrowsentityPOCOIsVoided()
        {
            ///arrange 
            var JournalId = "1-1";
            var Tenant = 1;
            var createdAt = new DateTime(2016, 01, 01);


            var JournalPoco = new Journal()
            {
                Id = JournalId,
                Tenant = Tenant,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.InProcessing).ToString(),
                QueueId = "StreamedtoAcc"
            };

            var JournalLinePM = new JournalLinePM() { JournalId = JournalId, Tenant = Tenant, ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update };

            var JPM = new JournalPM()
            {
                Id = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                JournalLines = new List<JournalLinePM>() { JournalLinePM },
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.Voided).ToString(),
                //OriginalJournalId = "This is A strono"
            };



            var checkes = new List<JournalPM>();








            var changTrack = GetChangeTarckOK(JournalId, Tenant, createdAt);
            var thisWillCauseException = new NotifyPropertyChangeValues() { PropertyName = JournalDataMapping.PMPropertyNames.ApproveDate.ToString() };
            changTrack.ChangedProperties.Add(thisWillCauseException);
            checkes.Add(changTrack);



            changTrack = GetChangeTarckOK(JournalId, Tenant, createdAt);
            thisWillCauseException = new NotifyPropertyChangeValues() { PropertyName = JournalDataMapping.PMPropertyNames.AccountingDate.ToString() };
            changTrack.ChangedProperties.Add(thisWillCauseException);
            checkes.Add(changTrack);

            foreach (var mychangTrack in checkes)
            {


                var fakeJournalUpdateOnUpdating = A.Fake<JournalUpdateOnUpdating>();
                var fakeJournalLineOnUpdate = A.Fake<JournalLineOnUpdate>();
                //A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM, JPM))
                //    .Invokes(() => { fakeJournalLineOnUpdateOnUpdate_done = true; });

                A.CallTo(() => fakeJournalUpdateOnUpdating.CreateJournalLineOnUpdate())
                    .Returns(fakeJournalLineOnUpdate);

                TestsUtil.AssertThrows<Exception>(
                    () =>
                    {

                        ///Act
                        ///


                        fakeJournalUpdateOnUpdating.OnUpdating(JPM, JournalPoco, mychangTrack);
                    },
                    //Check
                "BLException :Approved Journal Can Only Change To Voided Property",
                "if (propChanged.Any())");

            }
        }







        [TestMethod]
        public void OnUpdating_ApprovedPocoTryVoidedCanNotChangeJournalLines_ThrowsApprovedJournalCanOnlyChangeToVoidedProperty()
        {
            ///arrange 
            var JournalId = "1-1";
            var Tenant = 1;
            var createdAt = new DateTime(2016, 01, 01);


            var JournalPoco = new Journal()
            {
                Id = JournalId,
                Tenant = Tenant,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.InProcessing).ToString(),
                QueueId = "StreamedtoAcc"
            };


            var checkes = new List<JournalPM>();

            var JournalLinePM = new JournalLinePM()
            {
                JournalId = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update
            };

            var JPM = new JournalPM()
            {
                Id = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                JournalLines = new List<JournalLinePM>() { JournalLinePM },
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.Voided).ToString(),
                //OriginalJournalId = "This is A strono"
            };

            checkes.Add(JPM);


            JournalLinePM = new JournalLinePM()
            {
                JournalId = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert
            };

            JPM = new JournalPM()
            {
                Id = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                JournalLines = new List<JournalLinePM>() { JournalLinePM },
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.Voided).ToString(),
                //OriginalJournalId = "This is A strono"
            };
            checkes.Add(JPM);



            JournalLinePM = new JournalLinePM()
            {
                JournalId = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete
            };

            JPM = new JournalPM()
            {
                Id = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                JournalLines = new List<JournalLinePM>() { JournalLinePM },
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.Voided).ToString(),
                //OriginalJournalId = "This is A strono"
            };
            checkes.Add(JPM);










            foreach (var myPM in checkes)
            {


                var fakeJournalUpdateOnUpdating = A.Fake<JournalUpdateOnUpdating>();
                var fakeJournalLineOnUpdate = A.Fake<JournalLineOnUpdate>();
                //A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM, JPM))
                //    .Invokes(() => { fakeJournalLineOnUpdateOnUpdate_done = true; });

                A.CallTo(() => fakeJournalUpdateOnUpdating.CreateJournalLineOnUpdate())
                    .Returns(fakeJournalLineOnUpdate);

                TestsUtil.AssertThrows<Exception>(
                    () =>
                    {

                        ///Act
                        ///
                        var mychangTrack = GetChangeTarckOK(JournalId, Tenant, createdAt);

                        fakeJournalUpdateOnUpdating.OnUpdating(myPM, JournalPoco, mychangTrack);
                    },
                //Check
                "Approved Journal Can Only Change To Voided Property (Change JournalLines fix credrit or debit) line=",
                "if (propChanged.Any())");

            }
        }

        private static JournalPM GetChangeTarckOK(string JournalId, int Tenant, DateTime createdAt)
        {
            var changTrack = new JournalPM()
            {
                Id = JournalId,
                Tenant = Tenant,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.InProcessing).ToString(),
                IsVoided = true,


            };
            changTrack.ChangedProperties.Clear();
            changTrack.ChangedProperties.AddRange(new List<NotifyPropertyChangeValues>()
                {
                    ///Properties 4 Void - OK !! OK !!
                    new NotifyPropertyChangeValues(){ PropertyName =JournalDataMapping.PMPropertyNames.StatusCode.ToString()},
                    new NotifyPropertyChangeValues(){ PropertyName =JournalDataMapping.PMPropertyNames.IsVoided.ToString()},
                    new NotifyPropertyChangeValues(){ PropertyName =JournalDataMapping.PMPropertyNames.VoidDate.ToString()},
                    new NotifyPropertyChangeValues(){ PropertyName =JournalDataMapping.PMPropertyNames.VoidedByJournalId.ToString()},
                    new NotifyPropertyChangeValues(){ PropertyName =JournalDataMapping.PMPropertyNames.VoidedByUserId.ToString()},
                    new NotifyPropertyChangeValues(){ PropertyName =JournalDataMapping.PMPropertyNames.VoidedByUserName.ToString()},

                    ///
                });
            return changTrack;
        }





        [TestMethod]
        public void OnUpdating_InsertWithApprovedAction_MustHaveHappendJournalApproveParser()
        {
            ///arrange 
            var JournalId = "1-1";
            var Tenant = 1;
            var createdAt = new DateTime(2016, 01, 01);






            var JournalLinePM = new JournalLinePM()
            {
                JournalId = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert
            };

            var JPM = new JournalPM()
            {
                Id = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                JournalLines = new List<JournalLinePM>() { JournalLinePM },
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.InProcessing).ToString(),
                //OriginalJournalId = "This is A strono"
            };


            //foreach (var myPM in checkes)
            {


                var fakeJournalUpdateOnUpdating = A.Fake<JournalUpdateOnUpdating>();
                var fakeJournalLineOnUpdate = A.Fake<JournalLineOnUpdate>();
                //A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM, JPM))
                //    .Invokes(() => { fakeJournalLineOnUpdateOnUpdate_done = true; });

                A.CallTo(() => fakeJournalUpdateOnUpdating.CreateJournalLineOnUpdate())
                    .Returns(fakeJournalLineOnUpdate);
                int step_JournalApproveParser = 1;

                var fakeIJournalApproveParser = A.Fake<IJournalApproveParser>();
                A.CallTo(() => fakeIJournalApproveParser.OnApproveUpdatingFillArrangeJournalPMResetControlAccount())
                    .Invokes(() =>
                    {
                        if (step_JournalApproveParser == 2)
                        {
                            throw new Exception("OnApproveUpdatingFillArrangeJournalPMResetControlAccount:step_JournalApproveParser==2");
                        }
                        step_JournalApproveParser++;

                    });
                A.CallTo(() => fakeIJournalApproveParser.ParseIt())
                    .Invokes(() =>
                    {
                        if (step_JournalApproveParser != 2)
                        {
                            throw new Exception("ParseIt:step_JournalApproveParser!=2");
                        }
                        step_JournalApproveParser++;
                    });

                A.CallTo(() => fakeJournalUpdateOnUpdating.NewJournalApproveParser(JPM))
                    .Returns(fakeIJournalApproveParser);

                ///Act
                ///
                var mychangTrack = GetChangeTarckOK(JournalId, Tenant, createdAt);

                fakeJournalUpdateOnUpdating.OnUpdating(JPM, new Journal(), mychangTrack);
                //check 
                Assert.AreEqual(3, step_JournalApproveParser);

            }
        }


        [TestMethod]
        public void OnUpdating_UpdateApprovedAction_MustHaveHappendJournalApproveParser()
        {
            ///arrange 
            var JournalId = "1-1";
            var Tenant = 1;
            var createdAt = new DateTime(2016, 01, 01);


            var JournalPoco = new Journal()
            {
                Id = JournalId,
                Tenant = Tenant,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.WaitingforApprove).ToString(),

            };



            var JournalLinePM = new JournalLinePM()
            {
                JournalId = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update
            };

            var JPM = new JournalPM()
            {
                Id = JournalId,
                Tenant = Tenant,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
                UpdateDate = createdAt,
                CreateDate = createdAt,
                JournalLines = new List<JournalLinePM>() { JournalLinePM },
                StatusCode = ((int)JournalStatusTypePM.StatusCodeEnum.InProcessing).ToString(),
                //OriginalJournalId = "This is A strono"
            };


            //foreach (var myPM in checkes)
            {


                var fakeJournalUpdateOnUpdating = A.Fake<JournalUpdateOnUpdating>();
                var fakeJournalLineOnUpdate = A.Fake<JournalLineOnUpdate>();
                //A.CallTo(() => fakeJournalLineOnUpdate.OnUpdate(JournalLinePM, JPM))
                //    .Invokes(() => { fakeJournalLineOnUpdateOnUpdate_done = true; });

                A.CallTo(() => fakeJournalUpdateOnUpdating.CreateJournalLineOnUpdate())
                    .Returns(fakeJournalLineOnUpdate);
                int step_JournalApproveParser = 1;

                var fakeIJournalApproveParser = A.Fake<IJournalApproveParser>();
                A.CallTo(() => fakeIJournalApproveParser.OnApproveUpdatingFillArrangeJournalPMResetControlAccount())
                    .Invokes(() =>
                    {
                        if (step_JournalApproveParser == 2)
                        {
                            throw new Exception("OnApproveUpdatingFillArrangeJournalPMResetControlAccount:step_JournalApproveParser==2");
                        }
                        step_JournalApproveParser++;

                    });
                A.CallTo(() => fakeIJournalApproveParser.ParseIt())
                    .Invokes(() =>
                    {
                        if (step_JournalApproveParser != 2)
                        {
                            throw new Exception("ParseIt:step_JournalApproveParser!=2");
                        }
                        step_JournalApproveParser++;
                    });

                A.CallTo(() => fakeJournalUpdateOnUpdating.NewJournalApproveParser(JPM))
                    .Returns(fakeIJournalApproveParser);

                ///Act
                ///
                var mychangTrack = GetChangeTarckOK(JournalId, Tenant, createdAt);

                fakeJournalUpdateOnUpdating.OnUpdating(JPM, JournalPoco, mychangTrack);
                //check 
                Assert.AreEqual(3, step_JournalApproveParser);

            }
        }




       

    }


}
