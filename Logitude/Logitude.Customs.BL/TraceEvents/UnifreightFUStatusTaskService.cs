
using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Infrastructure.FuStatus;
using Logitude.AmitalMessaging.Infrastructure.Transmission;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Globalization;
using System.Transactions;
using System.Xml.Linq;
using Unifreight.BL.EntityPMs;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityUpdateServices;
using Unifreight.Data.AmitalModel;

namespace Logitude.Customs.BL.TraceEvents
{
    internal class UnifreightFUStatusTaskService
    {
        private AmitalContext _AmitalContext;
        private string _UnifreightUserId;

        public UnifreightFUStatusTaskService()
        {
        }

        public void DeleteINAFUStatus(int Tenant ,string CustomFileNo)
        {
            var myGDFDATAQueryService = new Unifreight.BL.EntityQueryServices.GDFDATAQueryService(AmitalContext.GetContext(Tenant));
            var def = myGDFDATAQueryService.GetSingle("ISRAEL", "GGG_DEL_INA", "NON", "NON", false, true);
            def = def ?? new GDFDATAPM();
            if (def.DEFDATA == "Y")
            {

                ContactRepository contactRepository = new ContactRepository(Tenant);
                var loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(Tenant), Tenant);
                string loggedContactId = "";
                if (loggedContact != null)
                {
                    loggedContactId = loggedContact.Id;
                }


                UpsertFUStatusLE2U(Tenant, loggedContactId, new UnifreightFUStatusParam()
                {
                    Entname = "CFIFILEM",
                    PrimaryNum = CustomFileNo,
                    Mode = UnifreightEventMode.del,
                    StatusCode = "INA",
                    StatusRemarks = "",

                });
                //SendINVAD(Tenant, CustomFileNo, loggedContactId);

            }

        }

        private static void SendINVAD(int Tenant, string CustomFileNo, string loggedContactId)
        {
            string unifrieghtEvent = "INAD";
            string eventRemarks = "";
            var MyUnifreightEventParam = new UnifreightEventParam()
            {
                Code = unifrieghtEvent,
                Mode = UnifreightEventMode.@new,
                EventDateTime = DateTime.Now,
                Entname = "CFIFILEM",
                PrimaryNum = CustomFileNo,
                EventRemarks = eventRemarks,
            };

            var myOpenUnifreighTask = new UnifreightEventTaskService();
            myOpenUnifreighTask.UpsertEventLE2U(
                Tenant,
                loggedContactId,
                MyUnifreightEventParam);
        }

        public void UpsertFUStatusLE2U(int tenant, string logitudeUserId, UnifreightFUStatusParam myUnifreightFUStatusParam)
        {
            

            if (!myUnifreightFUStatusParam.IsValid())
            {
                return;
            }
            _UnifreightUserId = GetUnifreightUserId(tenant, logitudeUserId);
            try
            {

                using (_AmitalContext = AmitalContext.GetContext(tenant))
                {
                    var myCCUQUELOCKQueryService = new CCUQUELOCKQueryService(_AmitalContext);
                    var myCCUQUELOCKUpdateService = new CCUQUELOCKUpdateService(_AmitalContext);
                    var myGGGQUpdateService = new GGGQUpdateService(_AmitalContext);
                    var myYCULTASKUpdateService = new YCULTASKUpdateService(_AmitalContext);



                    EnsureLockExist4Entity(myCCUQUELOCKQueryService, myCCUQUELOCKUpdateService, myUnifreightFUStatusParam);


                    
                    string requestData = GetMyFUStatusXML(tenant, myUnifreightFUStatusParam, myUnifreightFUStatusParam.EventDateTime?? DateTime.Now);


                    //string requestData = GetEventRequestDATA(myUnifreightFUStatusParam, unifreightUserId, true);
                    InsertEventTask4Entity(myUnifreightFUStatusParam, _UnifreightUserId, myYCULTASKUpdateService, requestData);

                    InsertGGGQ4Entity(myUnifreightFUStatusParam.Entname, myUnifreightFUStatusParam.PrimaryNum, myGGGQUpdateService);
                }


            }
            finally
            {
                //if (scope != null)
                //{
                //    scope.Dispose();
                //}
            }
        }

        private void InsertGGGQ4Entity(string ENTNAME, string ENTITYNUM, GGGQUpdateService myGGGQUpdateService)
        {
            var myGGGQPM_Packs = new GGGQPM()
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                ORIGINQUE = "LGT", //LugitudeRequest
                STATUS = "1",
                EXPTASKTIME = 5,
                EXECDATE = (new DualQueryService(_AmitalContext as AmitalContext)).GetServerDateTime() ?? DateTime.Now.AddMinutes(-20), //-20 because of time differences between the server where the code runs in and the DB server
                TRY = 9,
                PRIORITY = 8,
                ENTNAME = ENTNAME,
                PRIMARYNUM = ENTITYNUM,
                FORMID = "LGT_UPDATE_FCI",
                DEBUG = "F",
                DONEOPERATION = "A",

            };
            myGGGQUpdateService.DontAddTransaction = true;//we cant add a transaction with 
            myGGGQUpdateService.Update(myGGGQPM_Packs, true);
        }

        private static void InsertEventTask4Entity(UnifreightFUStatusParam myUnifreightEventParam, string unfreightUserId, YCULTASKUpdateService myYCULTASKUpdateService, string requestData)
        {


            var myYCULTASKPM_Packs = new YCULTASKPM()
            {
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                STATUS = "W",
                REQUESTDATA = requestData,
                ENTNAME = myUnifreightEventParam.Entname,
                PRIMARYNUM = myUnifreightEventParam.PrimaryNum,
                PRIORITY = YCULTASKPM.calcPriority("L2U"),

                TYPE = "L2U",
                USRCODE = unfreightUserId,
                ARCHIVE = "F",

            };

            myYCULTASKUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
            myYCULTASKUpdateService.Update(myYCULTASKPM_Packs, true);
            LogMessagingUtil.Instance.AppendLine("TASKID="+myYCULTASKPM_Packs.TASKID);
        }

        public string GetMyFUStatusXML(
            int tenant,
            UnifreightFUStatusParam myUnifreightFUStatusParam,
            DateTime statusDateTime) 
        {
            //requestData2 = GetMyFUStatusXML("INR", "INR", "", "new", DateTime.Now, false);
            string event_id = myUnifreightFUStatusParam.StatusCode;
            string status_id = myUnifreightFUStatusParam.StatusCode;
            string comments=myUnifreightFUStatusParam.StatusRemarks;

            string xmlStatus = myUnifreightFUStatusParam.Mode.ToString();//xmlStatus == new/del


            string loggingUserId = this._UnifreightUserId;
            if (String.IsNullOrWhiteSpace(xmlStatus))
            {
                xmlStatus = "new";
            }





            var myFUStatus = new AmitalEventTracerModel.FUStatus()
            {
                entname = myUnifreightFUStatusParam.Entname,          // "CFIFILEM",
                primary_number = myUnifreightFUStatusParam.PrimaryNum,//  this._DirtyDeclarationPM.CustomFileNo,
                status = "new",
                xml_status = xmlStatus,
                status_id = status_id,
                status_DateTime = statusDateTime,
                //status_save = "no_fail",
                comments = comments,
            };

            var myAmitalStatusTracerModel = new AmitalEventTracerModel();
            myAmitalStatusTracerModel.Tenant = tenant;
            myAmitalStatusTracerModel.UserId = loggingUserId;
            myAmitalStatusTracerModel.MyFUStatus = myFUStatus;

            var myGFUSTS = AmitalEventTracer.GetFUStatus(myAmitalStatusTracerModel);
            var xml = XmlGenericUtil<GFUSTS>.SerializeObject(myGFUSTS, true);
            
            return xml;
        }




        private static string GetUnifreightUserId(int tenant, string UserId)
        {
            string unfreightUserId = null;

            if (!String.IsNullOrWhiteSpace(UserId))
            {
                //unfreightUserId = AuthenticationUtil.ResolveUnifreightUserById(myAmitalEventTracer.UserId, myAmitalEventTracer.Tenant);
                var userRepository = new UserRepository(tenant);
                var myUser = userRepository.GetSingleUser(UserId, tenant, true);
                if (myUser != null)
                {
                    unfreightUserId = myUser.Code;
                }
            }

            if (String.IsNullOrWhiteSpace(unfreightUserId))
            {
                unfreightUserId = AuthenticationUtil.ResolveUnifreightUserId(tenant);
            }

            return unfreightUserId;
        }

        private static void EnsureLockExist4Entity(CCUQUELOCKQueryService myCCUQUELOCKQueryService, CCUQUELOCKUpdateService myCCUQUELOCKUpdateService, UnifreightFUStatusParam myUnifreightEventParam)
        {
            CCUQUELOCKPM myCCUQUELOCK = myCCUQUELOCKQueryService.GetSingle(myUnifreightEventParam.Entname, myUnifreightEventParam.PrimaryNum, false);
            if (myCCUQUELOCK == null)
            {
                var myCCUQUELOCKPM = new CCUQUELOCKPM()
                {
                    ChangeSetOp = ChangeSetOperation.Insert,
                    ENTNAME = myUnifreightEventParam.Entname,
                    FILENO = myUnifreightEventParam.PrimaryNum,
                };
                myCCUQUELOCKUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                myCCUQUELOCKUpdateService.Update(myCCUQUELOCKPM, true);
            }
        }

    }

    public enum UnifreightFUStatusMode

    {
        @new,//default
        del,

    }
    public class UnifreightFUStatusParam
    {
        public string StatusCode { get; set; }
        public UnifreightEventMode Mode { get; set; }
        public DateTime? EventDateTime { get; set; }
        public string StatusRemarks { get; set; }
        public string EventUser { get; set; }
        public string Entname { get; set; }
        public string PrimaryNum { get; set; }

        internal bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(StatusCode))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(Entname))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(PrimaryNum))
            {
                return false;
            }

            return true;

        }
    }
}