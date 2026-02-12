using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Infrastructure.Transmission;
using Logitude.AmitalMessaging.Utils;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Xml.Linq;
using Unifreight.BL.EntityPMs;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityUpdateServices;
using Unifreight.Data.AmitalModel;

namespace Logitude.Customs.BL.TraceEvents
{
    public class UnifreightEventTaskService
    {
        private AmitalContext _AmitalContext;

        public UnifreightEventTaskService()
        {
        }

        public void UpsertEventLE2U(int tenant, string UserId, UnifreightEventParam myUnifreightEventParam)
        {
  

            if (!myUnifreightEventParam.IsValid())
            {
                return;
            }

            var mySetting = EntityQueryServices.CustomsSettingQueryService.GetSettingByTenant(tenant);

            if (mySetting.StandAlone)
                return;

            string unifreightUserId = GetUnifreightUserId(tenant, UserId);
            try
            {
                if (myUnifreightEventParam.Entname == "CFIFILEM")
                {
                    _AmitalContext = AmitalContext.GetContext(tenant);

                    var myCCUQUELOCKQueryService = new CCUQUELOCKQueryService(_AmitalContext);
                    var myCCUQUELOCKUpdateService = new CCUQUELOCKUpdateService(_AmitalContext);
                    var myGGGQUpdateService = new GGGQUpdateService(_AmitalContext);
                    var myYCULTASKUpdateService = new YCULTASKUpdateService(_AmitalContext);



                    EnsureLockExist4Entity(myCCUQUELOCKQueryService, myCCUQUELOCKUpdateService, myUnifreightEventParam,tenant);

                    
                    InsertGGGQ4Entity(myUnifreightEventParam.Entname, myUnifreightEventParam.PrimaryNum, myGGGQUpdateService, mySetting.IsConnectedToUniFreight, tenant);
                    

                    string requestData = GetEventRequestDATA(myUnifreightEventParam, unifreightUserId, true);
                    InsertEventTask4Entity(myUnifreightEventParam, unifreightUserId, tenant, requestData);




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

        private void InsertGGGQ4Entity(string ENTNAME, string ENTITYNUM, GGGQUpdateService myGGGQUpdateService, bool isConnectedToUniFreight, int tenant)
        {

            var myGGGQPM_Packs = new GGGQPM()
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                ORIGINQUE = "LGT", //LugitudeRequest
                STATUS = "1",
                EXPTASKTIME = 5,
                EXECDATE = DateTime.Now, 
                TRY = 9,
                PRIORITY = 8,
                ENTNAME = ENTNAME,
                PRIMARYNUM = ENTITYNUM,
                FORMID = "LGT_UPDATE_FCI",
                DEBUG = "F",
                DONEOPERATION = "D",

            };
            myGGGQUpdateService.DontAddTransaction = true;//we cant add a transaction with 
            
            myGGGQPM_Packs.Tenant = tenant;
            
            myGGGQUpdateService.Update(myGGGQPM_Packs, true);
        }

        private static void InsertEventTask4Entity(UnifreightEventParam myUnifreightEventParam, string unfreightUserId, int tenant, string requestData)
        {

           


            var myYCULTASKPM_Packs = new YCULTASKPM()
            {
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                STATUS = "W",
                REQUESTDATA = requestData,
                ENTNAME = myUnifreightEventParam.Entname,
                PRIMARYNUM = myUnifreightEventParam.PrimaryNum,
                PRIORITY = YCULTASKPM.calcPriority("LE2U"),

                TYPE = "LE2U",
                USRCODE = unfreightUserId,
                ARCHIVE = "F",

            };
           
            myYCULTASKPM_Packs.Tenant = tenant;
            
            AmitalContext  _AmitalContext = AmitalContext.GetContext(tenant);
            var myYCULTASKUpdateService = new YCULTASKUpdateService(_AmitalContext);
            myYCULTASKUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.

            myYCULTASKUpdateService.Update(myYCULTASKPM_Packs, true);
            
           
            
        }

        public static string GetEventRequestDATA(
            UnifreightEventParam MyUnifreightEventParam, string unfreightUserId, bool asXDocument)
        {

            string EventDate = "";
            string EventTime = "";
            var UnifreightEventDateTime = MyUnifreightEventParam.EventDateTime;

            if (UnifreightEventDateTime.HasValue)
            {
                DateTime dateTime = UnifreightEventDateTime.GetValueOrDefault();

                EventDate = $"{dateTime.Date.Day:00}.{dateTime.Date.Month:00}.{dateTime.Date.Year:00}"; ;  //dateTime.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                EventTime = $"{dateTime.Hour:00}:{dateTime.Minute:00}"; ;
            }

            XElement EventsXElement = GetEventsXElement(MyUnifreightEventParam, unfreightUserId, EventDate, EventTime);
            string requestData = "";
            if (asXDocument)
            {
                var doc =
                new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("OpenUnifreighTaskService.UpsertEventLE2U"),
                EventsXElement);
                requestData = doc.ToString(SaveOptions.None);
            }
            else
            {
                requestData = EventsXElement.ToString(SaveOptions.None);
            }


            //return outPut;


            //transmission mytransmission = GetTransmission(myCFIPACKS, "AMITAL", "Customs packs from logitude");
            //var xmltransmission = XmlGenericUtil<transmission>.SerializeObject(mytransmission, true);

            return requestData;
        }

        private static XElement GetEventsXElement(UnifreightEventParam MyUnifreightEventParam, string unfreightUserId, string EventDate, string EventTime)
        {
            var EventsXElement = new XElement("Events");
            if (MyUnifreightEventParam.PrimaryNumList != null && MyUnifreightEventParam.PrimaryNumList.Count > 0)
            {
                EventsXElement =
                            //new XDocument(
                            //new XDeclaration("1.0", "utf-8", "yes"),
                            //new XComment("OpenUnifreighTaskService.UpsertEventLE2U"),
                            new XElement("Events",
                            from PrimaryNumParam in MyUnifreightEventParam.PrimaryNumList
                            select new XElement("Event",
                                    new XElement("Code", MyUnifreightEventParam.Code),
                                    new XElement("Mode", MyUnifreightEventParam.Mode.ToString()),

                                    new XElement("EventDate", EventDate),
                                    new XElement("EventTime", EventTime),

                                    new XElement("EventRemarks", MyUnifreightEventParam.EventRemarks),
                                    new XElement("EventUser", unfreightUserId),
                                    new XElement("Entname", MyUnifreightEventParam.Entname),
                                    new XElement("PrimaryNum", PrimaryNumParam)
                                    )
                            );
            }
            else
            {
                EventsXElement =
                            //new XDocument(
                            //new XDeclaration("1.0", "utf-8", "yes"),
                            //new XComment("OpenUnifreighTaskService.UpsertEventLE2U"),
                            new XElement("Events",
                                new XElement("Event",
                                    new XElement("Code", MyUnifreightEventParam.Code),
                                    new XElement("Mode", MyUnifreightEventParam.Mode.ToString()),

                                    new XElement("EventDate", EventDate),
                                    new XElement("EventTime", EventTime),

                                    new XElement("EventRemarks", MyUnifreightEventParam.EventRemarks),
                                    new XElement("EventUser", unfreightUserId),
                                    new XElement("Entname", MyUnifreightEventParam.Entname),
                                    new XElement("PrimaryNum", MyUnifreightEventParam.PrimaryNum)
                                    )
                            );
            }
            return EventsXElement;
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

        private static void EnsureLockExist4Entity(CCUQUELOCKQueryService myCCUQUELOCKQueryService, CCUQUELOCKUpdateService myCCUQUELOCKUpdateService, UnifreightEventParam myUnifreightEventParam,int tenant)
        {
            CCUQUELOCKPM myCCUQUELOCK = myCCUQUELOCKQueryService.GetSingle(myUnifreightEventParam.Entname, myUnifreightEventParam.PrimaryNum,tenant, false);
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

    public enum UnifreightEventMode
    {
        @new,//default
        del,

    }
    public class UnifreightEventParam
    {
        public string Code { get; set; }
        public UnifreightEventMode Mode { get; set; }
        public DateTime? EventDateTime { get; set; }
        public string EventRemarks { get; set; }
        public string EventUser { get; set; }
        public string Entname { get; set; }
        public string PrimaryNum { get; set; }
        public List<string> PrimaryNumList { get; set; }

        internal bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Code))
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