using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomsRequestsSheetQueryService : EntityQueryService<CustomsRequestsSheet, CustomsRequestsSheetKeys, CustomsRequestsSheetPM, object, CustomsRequestsSheetKeys>

    {


        public IQueryable<CustomsRequestsSheet> GetQSheetStatusInProcess(int tenant)
        {
            var listSheetStatusInProcess = new List<string>();
            foreach (var item in Enum.GetValues(typeof(SheetStatusInProcessEnum)))
            {
                listSheetStatusInProcess.Add(((int)item).ToString());
            }


            var q = this.repository.GetAll(tenant)
                .Where(rec => rec.Tenant == tenant)
                .Where(rec => listSheetStatusInProcess.Contains(rec.RequestStatusCode)
                    //rec.RequestStatusCode == "1" /*EnglishName	LocalName Created	בקשה נרשמה */
                    //||
                    //rec.RequestStatusCode == "2" /*EnglishName	LocalName In Process	באמצע טיפול*/
                    ////||
                    ////rec.RequestStatusCode == "5" /* Waiting For Signing	ממתין לחתימה */ //Yuval Chalup 06.08.2015 TASK-15156 (Remarked)
                    //||
                    //rec.RequestStatusCode == "20" /* Sent	נשלח */ //Yuval Chalup 06.08.2015 TASK-15156
                    //||
                    //rec.RequestStatusCode == "21" /* Received	התקבלה תשובה */ //Yuval Chalup 06.08.2015 TASK-15156
                    );
            return q;
        }

        public List<CustomsRequestsSheetList> GetCustomsRequestsSheetByCustomFileNumber(string customFileNumber, int tenant)
        {
            List<CustomsRequestsSheet> requests = repository.GetCustomsRequestsSheetByCustomFileNumber(customFileNumber, tenant);
            List<CustomsRequestsSheetList> requestsPMs = new List<CustomsRequestsSheetList>();
            foreach (CustomsRequestsSheet a in requests)
            {
                CustomsRequestsSheetList requestSheet = new CustomsRequestsSheetList()
                {
                    Id = a.Id,
                    Tenant = a.Tenant,

                    AnswerCreateDate = a.AnswerCreateDate,
                    CorrelationId = a.CorrelationId,
                    IsDCA = a.IsDCA,
                    CustomFileNo = a.CustomFileNo,
                    EntityId1 = a.EntityId1,
                    EntityId2 = a.EntityId2,
                    EntityReference = a.EntityReference,
                    InterfaceTypeCode = a.InterfaceTypeCode,
                    ObjectTableId1 = a.ObjectTableId1,
                    ObjectTableId2 = a.ObjectTableId2,
                    RequestComminicationId = a.RequestComminicationId,
                    RequestCreateDate = a.RequestCreateDate,
                    RequestDescription = a.RequestDescription,
                    RequestOwnerId = a.RequestOwnerId,
                    RequestStatusCode = a.RequestStatusCode,
                    InterfaceTypeName = a.InterfaceManagement != null ? a.InterfaceManagement.Description : null,
                    RequestStatusName = a.CustomsRequestsSheetStatus != null ? a.CustomsRequestsSheetStatus.LocalName : null,
                    RequestOwnerName = a.User != null ? a.User.Contact.EnglishName : null,
                };
                requestsPMs.Add(requestSheet);
            }

            return requestsPMs;
        }

        public List<CustomsRequestsSheetPM> GetCustomsRequestsSheetByCustomFileNumberPM(string customFileNumber, int tenant)
        {
            List<CustomsRequestsSheet> requests = repository.GetCustomsRequestsSheetByCustomFileNumber(customFileNumber, tenant);
            List<CustomsRequestsSheetPM> requestsPMs = requests.Select(r => this.GetEntityPM(r)).ToList();

            return requestsPMs;
        }


        public List<CustomsRequestsSheetList> GetEntityRequestsSheets(string objectTableId, string entityId, int tenant)
        {
            List<CustomsRequestsSheet> requests = repository.GetEntityRequestsSheets(objectTableId, entityId, tenant);
            List<CustomsRequestsSheetList> requestsPMs = new List<CustomsRequestsSheetList>();
            foreach (CustomsRequestsSheet a in requests)
            {
                CustomsRequestsSheetList requestSheet = new CustomsRequestsSheetList()
                {
                    Id = a.Id,
                    Tenant = a.Tenant,

                    AnswerCreateDate = a.AnswerCreateDate,
                    CorrelationId = a.CorrelationId,
                    IsDCA = a.IsDCA,
                    CustomFileNo = a.CustomFileNo,
                    EntityId1 = a.EntityId1,
                    EntityId2 = a.EntityId2,
                    EntityReference = a.EntityReference,
                    InterfaceTypeCode = a.InterfaceTypeCode,
                    ObjectTableId1 = a.ObjectTableId1,
                    ObjectTableId2 = a.ObjectTableId2,
                    RequestComminicationId = a.RequestComminicationId,
                    RequestCreateDate = a.RequestCreateDate,
                    RequestDescription = a.RequestDescription,
                    RequestOwnerId = a.RequestOwnerId,
                    RequestStatusCode = a.RequestStatusCode,
                    InterfaceTypeName = a.InterfaceManagement != null ? a.InterfaceManagement.Description : null,
                    RequestStatusName = a.CustomsRequestsSheetStatus != null ? a.CustomsRequestsSheetStatus.LocalName : null,
                    RequestOwnerName = a.User != null ? a.User.Contact.EnglishName : null,
                };
                requestsPMs.Add(requestSheet);
            }

            return requestsPMs;
        }

        public List<CustomsRequestsSheetPM> GetRequestInProgressByIds(
            int Tenant,
            string InterfaceTypeCode,
            string ObjectTableId1, List<string> EntityId1, bool displayOnlyMode
          )
        {
            if (!displayOnlyMode && string.IsNullOrWhiteSpace(InterfaceTypeCode))
            {
                throw new Exception("GetRequestInProgress !displayOnlyMode && string.IsNullOrWhiteSpace(InterfaceTypeCode) ");
            }

            var intrefaceTypeList =
                new string[]
                {
"2715",//CustomsDocument Request
"2750",// - מסר הצהרה יוצא
"2754",// - משוב להגשה/הצהרה
"2755",// - מסר הגשה
"8211",// - מסר דרישה לבטוחה
"8212",// - מסר מענה לדרישה לבטוחה
"8214",// -מסר בקשה של סוכן לאישור אילוצים 
"8215",// - מסר של החלטת גורם מאשר לאישור האילוץ
"8216",// - מסר תשובה של סוכן עם נימוקים לאישור האילוץ
"8227",// - מסר מסמך נדרש
"US2L01I",// - תהליך SIVUG BATCH 
//"UCUW2L",// - פתיחת הצהרה ממסר אינטגרטור
"UCTZIP",// - תהליך BuildCustomTableZip
"UCB2750",//,Batch Send 2750 per CourierMasterId
"UCB2755",//,Batch Send 2755 per CourierMasterId
"UCB1170",//,Batch Send 1170 per CourierMasterId
"UCB8250",//,Batch Send 8250 per CourierMasterId
"UCBUD2LT",///UniCourierBatchSendUCBUD2LT_MsgResponseService
"UCB8212",/// Batch Send Collateral
"8250",
"2892",

"UCBNDCD",///  Send bonded filing
///"8302", //בקשה לטופס הצהרה

"2751"//הצהרת יצוא- מסר יוצא
,"2757", //הצהרת יצוא - מסר נכנס
///"8302" //בקשה לטופס הצהרה


            };

            string[] intrefaceTypeListDisplayOnly = GetintrefaceTypeListDisplayOnly();
            if (!displayOnlyMode && !intrefaceTypeList.Contains(InterfaceTypeCode))
            {
                return new List<CustomsRequestsSheetPM>();
            }
            var listSheetStatusInProcess = new List<string>();
            foreach (var item in Enum.GetValues(typeof(SheetStatusInProcessEnum)))
            {
                listSheetStatusInProcess.Add(((int)item).ToString());
            }

            //List<CustomsRequestsSheet> requests = repository.GetCustomsRequestsSheetByCustomFileNumber(customFileNumber, tenant);
            var q = //context.CustomsRequestsSheets
                this.repository.GetAll(Tenant)
                .Where(rec => rec.Tenant == Tenant)
                .Where(rec => listSheetStatusInProcess.Contains(rec.RequestStatusCode)

                    //rec.RequestStatusCode == "1" /*EnglishName	LocalName Created	בקשה נרשמה */
                    //||
                    //rec.RequestStatusCode == "2" /*EnglishName	LocalName In Process	באמצע טיפול*/
                    ////||
                    ////rec.RequestStatusCode == "5" /* Waiting For Signing	ממתין לחתימה */ //Yuval Chalup 06.08.2015 TASK-15156 (Remarked)
                    //||
                    //rec.RequestStatusCode == "20" /* Sent	נשלח */ //Yuval Chalup 06.08.2015 TASK-15156
                    //||
                    //rec.RequestStatusCode == "21" /* Received	התקבלה תשובה */ //Yuval Chalup 06.08.2015 TASK-15156
                    );
            if (displayOnlyMode)
            {
                q = q.Where(rec => intrefaceTypeListDisplayOnly.Contains(rec.InterfaceTypeCode));
            }
            else
            {
                q = q.Where(rec => rec.InterfaceTypeCode == InterfaceTypeCode);
            }


            var haveFilter = false;
            //if (!string.IsNullOrWhiteSpace(CustomFileNo))
            //{
            //    haveFilter = true;
            //    q = q.Where(rec => rec.CustomFileNo == CustomFileNo);
            //}

            if (EntityId1 != null && !string.IsNullOrWhiteSpace(ObjectTableId1))
            {
                haveFilter = true;
                q = q.Where(rec => EntityId1.Contains(rec.EntityId1) && rec.ObjectTableId1 == ObjectTableId1);
                //if (!string.IsNullOrWhiteSpace(EntityId2) && !string.IsNullOrWhiteSpace(ObjectTableId2))
                //{
                //    q = q.Where(rec => rec.EntityId2 == EntityId2 && rec.ObjectTableId2 == ObjectTableId2);
                //}
            }
            if (!haveFilter)
            {
                return new List<CustomsRequestsSheetPM>();
            }

            var pmList = q.ToList().Select(rec => this.GetEntityPM(rec)).ToList();
            return pmList;
        }
        public List<CustomsRequestsSheetPM> GetRequestInProgress(
            int Tenant,
            string InterfaceTypeCode,
            string ObjectTableId1, string EntityId1,
            string ObjectTableId2, string EntityId2,
            string CustomFileNo,
            bool displayOnlyMode = false)
        {
            return GetRequestInProgress(new RequestInProgressParams()
            {
                Tenant = Tenant,
                InterfaceTypeCode = InterfaceTypeCode,
                ObjectTableId1 = ObjectTableId1,
                EntityId1 = EntityId1,
                ObjectTableId2 = ObjectTableId2,
                EntityId2 = EntityId2,
                CustomFileNo = CustomFileNo,
                DisplayOnlyMode = displayOnlyMode
            });
        }
        public List<CustomsRequestsSheetPM> GetRequestInProgress(
            //int tenant,
            //string InterfaceTypeCode,
            //string ObjectTableId1, string EntityId1,
            //string ObjectTableId2, string EntityId2,
            //string CustomFileNo,
            //bool displayOnlyMode = false
            RequestInProgressParams requestInProgressParams
            )
        {
            if (!requestInProgressParams.DisplayOnlyMode && string.IsNullOrWhiteSpace(requestInProgressParams.InterfaceTypeCode))
            {
                throw new Exception("GetRequestInProgress !displayOnlyMode && string.IsNullOrWhiteSpace(InterfaceTypeCode) ");
            }
            requestInProgressParams.InterfaceTypeCode = requestInProgressParams.InterfaceTypeCode.Trim();// angular send " 2750" why  ??
            var intrefaceTypeList =
                new string[]
                {
"2715",//CustomsDocument Request
"2750",// - מסר הצהרה יוצא
"2754",// - משוב להגשה/הצהרה
"2755",// - מסר הגשה
"8211",// - מסר דרישה לבטוחה
"8212",// - מסר מענה לדרישה לבטוחה
"8214",// -מסר בקשה של סוכן לאישור אילוצים 
"8215",// - מסר של החלטת גורם מאשר לאישור האילוץ
"8216",// - מסר תשובה של סוכן עם נימוקים לאישור האילוץ
"8227",// - מסר מסמך נדרש
"US2L01I",// - תהליך SIVUG BATCH 
//"UCUW2L",// - פתיחת הצהרה ממסר אינטגרטור
"UCTZIP",// - תהליך BuildCustomTableZip
"UCB2750",//,Batch Send 2750 per CourierMasterId
"UCB2755",//,Batch Send 2755 per CourierMasterId
"UCB1170",//,Batch Send 1170 per CourierMasterId
"UCB8250",//,Batch Send 8250 per CourierMasterId
"UCBUD2LT",///UniCourierBatchSendUCBUD2LT_MsgResponseService
"UCB8212",/// Batch Send Collateral
"8250",
"2892",

"UCBNDCD",///  Send bonded filing
///"8302", //בקשה לטופס הצהרה

"2751"//הצהרת יצוא- מסר יוצא
,"2757", //הצהרת יצוא - מסר נכנס
///"8302" //בקשה לטופס הצהרה


            };

            string[] intrefaceTypeListDisplayOnly = GetintrefaceTypeListDisplayOnly();
            if (!requestInProgressParams.DisplayOnlyMode && !intrefaceTypeList.Contains(requestInProgressParams.InterfaceTypeCode))
            {
                return new List<CustomsRequestsSheetPM>();
            }
            var listSheetStatusInProcess = new List<string>();
            foreach (var item in Enum.GetValues(typeof(SheetStatusInProcessEnum)))
            {
                listSheetStatusInProcess.Add(((int)item).ToString());
            }

            //List<CustomsRequestsSheet> requests = repository.GetCustomsRequestsSheetByCustomFileNumber(customFileNumber, tenant);
            var q = //context.CustomsRequestsSheets
                this.repository.GetAll(requestInProgressParams.Tenant)
                .Where(rec => rec.Tenant == requestInProgressParams.Tenant)
                .Where(rec => listSheetStatusInProcess.Contains(rec.RequestStatusCode)
                    //rec.RequestStatusCode == "1" /*EnglishName	LocalName Created	בקשה נרשמה */
                    //||
                    //rec.RequestStatusCode == "2" /*EnglishName	LocalName In Process	באמצע טיפול*/
                    ////||
                    ////rec.RequestStatusCode == "5" /* Waiting For Signing	ממתין לחתימה */ //Yuval Chalup 06.08.2015 TASK-15156 (Remarked)
                    //||
                    //rec.RequestStatusCode == "20" /* Sent	נשלח */ //Yuval Chalup 06.08.2015 TASK-15156
                    //||
                    //rec.RequestStatusCode == "21" /* Received	התקבלה תשובה */ //Yuval Chalup 06.08.2015 TASK-15156
                    );
            if (requestInProgressParams.Include8250IsShaam)
            {
                var my = intrefaceTypeListDisplayOnly.ToList();
                my.Add("8250");
                intrefaceTypeListDisplayOnly = my.ToArray();
            }
            if (requestInProgressParams.DisplayOnlyMode)
            {
                q = q.Where(rec => intrefaceTypeListDisplayOnly.Contains(rec.InterfaceTypeCode));
            }
            else
            {
                q = q.Where(rec => rec.InterfaceTypeCode == requestInProgressParams.InterfaceTypeCode);
            }


            var haveFilter = false;
            if (!string.IsNullOrWhiteSpace(requestInProgressParams.CustomFileNo))
            {
                haveFilter = true;
                q = q.Where(rec => rec.CustomFileNo == requestInProgressParams.CustomFileNo);
            }

            if (!string.IsNullOrWhiteSpace(requestInProgressParams.EntityId1) && !string.IsNullOrWhiteSpace(requestInProgressParams.ObjectTableId1))
            {
                haveFilter = true;
                q = q.Where(rec => rec.EntityId1 == requestInProgressParams.EntityId1 && rec.ObjectTableId1 == requestInProgressParams.ObjectTableId1);
                if (!string.IsNullOrWhiteSpace(requestInProgressParams.EntityId2) && !string.IsNullOrWhiteSpace(requestInProgressParams.ObjectTableId2))
                {
                    q = q.Where(rec => rec.EntityId2 == requestInProgressParams.EntityId2 && rec.ObjectTableId2 == requestInProgressParams.ObjectTableId2);
                }
            }
            if (!haveFilter)
            {
                return new List<CustomsRequestsSheetPM>();
            }

            var pmList = q.ToList().Select(rec => this.GetEntityPM(rec)).ToList();
            return pmList;
        }

        public static string[] GetintrefaceTypeListDisplayOnly()
        {
            var intrefaceTypeListDisplayOnly
 =
                new string[]
                {

"2750",// - מסר הצהרה יוצא
"2754",// - משוב להגשה/הצהרה
"2755",// - מסר הגשה
"8211",// - מסר דרישה לבטוחה
"8212",// - מסר מענה לדרישה לבטוחה
"8214",// -מסר בקשה של סוכן לאישור אילוצים 
"8215",// - מסר של החלטת גורם מאשר לאישור האילוץ
"8216",// - מסר תשובה של סוכן עם נימוקים לאישור האילוץ
"8227",// - מסר מסמך נדרש
"US2L01I",// - תהליך SIVUG BATCH 
//"UCUW2L",// - פתיחת הצהרה ממסר אינטגרטור
"UCTZIP",// - תהליך BuildCustomTableZip
"UCB2750",//,Batch Send 2750 per CourierMasterId
"UCB2755",//,Batch Send 2755 per CourierMasterId
"UCB1170",//,Batch Send 2750 per CourierMasterId
"UCBCMSS",//,Batch Send change StorageSite per CourierMasterId
"1170", // - מסר מצהר
"1171", // - מסר תשובה מצהר
"1172", // - מסר תשובה מצהר - נדחף
"8373",//"שאילתא לשחזור נתוני הצהרה"

 "UCB8212"
 ,"2892" ,
"UCB9999",
"2751",
"2757",
//"8302" //בקשה לטופס הצהרה
};


            return intrefaceTypeListDisplayOnly;
        }


        public List<CustomsRequestsSheetPM> GetRequestByInterfaceTypeCode(
            int Tenant,
            string InterfaceTypeCode,
            string ObjectTableId1, string EntityId1,
            string CustomFileNo
            )
        {
            if (string.IsNullOrWhiteSpace(InterfaceTypeCode))
            {
                throw new Exception("GetRequestOfInterfaceTypeCode but  string.IsNullOrWhiteSpace(InterfaceTypeCode) ");
            }


            //List<CustomsRequestsSheet> requests = repository.GetCustomsRequestsSheetByCustomFileNumber(customFileNumber, tenant);
            var q = //context.CustomsRequestsSheets
                this.repository.GetAll(Tenant)
                .Where(rec => rec.Tenant == Tenant)
                .Where(rec => rec.InterfaceTypeCode == InterfaceTypeCode);




            var haveFilter = false;
            if (!string.IsNullOrWhiteSpace(CustomFileNo))
            {
                haveFilter = true;
                q = q.Where(rec => rec.CustomFileNo == CustomFileNo);
            }

            if (!string.IsNullOrWhiteSpace(EntityId1) && !string.IsNullOrWhiteSpace(ObjectTableId1))
            {
                haveFilter = true;
                q = q.Where(rec => rec.EntityId1 == EntityId1 && rec.ObjectTableId1 == ObjectTableId1);

            }
            if (!haveFilter)
            {

                throw new Exception("GetRequestOfInterfaceTypeCode :entity filter is must !!!");

            }

            var pmList = q.ToList().Select(rec => this.GetEntityPM(rec)).ToList();
            return pmList;
        }

        public List<CustomsRequestsSheetPM> SameInterfaceCodePerEntity_InProgress(int tenant, string InterfaceTypeCode,
            string ObjectTableId1, string EntityId1,
            string CustomFileNo)
        {


            if (string.IsNullOrWhiteSpace(InterfaceTypeCode))
            {
                throw new Exception("GetRequestInProgress !displayOnlyMode && string.IsNullOrWhiteSpace(InterfaceTypeCode) ");
            }





            var listSheetStatusInProcess = new List<string>();
            foreach (var item in Enum.GetValues(typeof(SheetStatusInProcessEnum)))
            {
                listSheetStatusInProcess.Add(((int)item).ToString());
            }

            //List<CustomsRequestsSheet> requests = repository.GetCustomsRequestsSheetByCustomFileNumber(customFileNumber, tenant);
            var q = //context.CustomsRequestsSheets
                this.repository.GetAll(Tenant)
                .Where(rec => rec.Tenant == Tenant)
                .Where(rec => listSheetStatusInProcess.Contains(rec.RequestStatusCode)
                    );
            q = q.Where(rec => rec.InterfaceTypeCode == InterfaceTypeCode);



            var haveFilter = false;
            if (!string.IsNullOrWhiteSpace(CustomFileNo))
            {
                haveFilter = true;
                q = q.Where(rec => rec.CustomFileNo == CustomFileNo);
            }

            if (!string.IsNullOrWhiteSpace(EntityId1) && !string.IsNullOrWhiteSpace(ObjectTableId1))
            {
                haveFilter = true;
                q = q.Where(rec => rec.EntityId1 == EntityId1 && rec.ObjectTableId1 == ObjectTableId1);

            }
            if (!haveFilter)
            {
                throw new Exception("SameInterfaceTypeCodePerEntity_InProgress nul arguments !!");
            }

            var pmList = q.ToList().Select(rec => this.GetEntityPM(rec)).ToList();
            return pmList;
        }

        internal List<CustomsRequestsSheetPM> GetWaitingForSigningListIncludeSignStepName(int tenant)
        {

            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            var stepRepo = new CommunicationLogStepRepository(tenant);

            int istage = (int)SheetStatusEnum.WaitingForSigning;
            var stage = istage.ToString();
            var CustomRequestSignEqual2 = (int)CustomsStepEnum.CustomRequestSign;
            var diffrentContext = true;
            if (!diffrentContext)
            {
                JoinNotWorkDiffrenContext(tenant, stepRepo, stage, CustomRequestSignEqual2);
            }
            var q =
                from rs in
                    this.repository.GetAll(tenant)
                    .Where(rec => rec.RequestStatusCode.Equals(stage))
                select rs;

            var pocoList = q.ToList();

            var pmList = pocoList.Select(rec => this.GetEntityPM(rec)).ToList();
            var RequestComminicationIdList = pmList.Select(rec => rec.RequestComminicationId);

            var stepJoinList = stepRepo
                .GetCommunicationSteps(tenant)
                .Where(step => RequestComminicationIdList.Contains(step.CommunicationLogId))
                .Where(rec => rec.StepNumber == CustomRequestSignEqual2)
                .ToList();

            foreach (var item in pmList)
            {
                var currCrossJoin = stepJoinList.First(rec => rec.CommunicationLogId == item.RequestComminicationId);
                item.SignStepName = currCrossJoin.Name;
            }
            return pmList;
        }

        private void JoinNotWorkDiffrenContext(int tenant, CommunicationLogStepRepository stepRepo, string stage, int CustomRequestSignEqual2)
        {
            var q =
            from rs in this.repository.GetAll(tenant).Where(rec => rec.RequestStatusCode.Equals(stage))
            join myCommunicationLogStep in stepRepo.GetCommunicationSteps(tenant).Where(rec => rec.StepNumber == CustomRequestSignEqual2)
                on rs.RequestComminicationId equals myCommunicationLogStep.CommunicationLogId
            select new { rs = rs, StepName = myCommunicationLogStep.Name };
            var theJoinList = q.ToList();
            var pmList = theJoinList.Select(rec => this.GetEntityPM(rec.rs)).ToList();
            foreach (var item in pmList)
            {
                var currCrossJoin = theJoinList.First(rec => rec.rs.Id == item.Id);
                item.SignStepName = currCrossJoin.StepName;
            }
        }


        public string GetDailyStatistic(int tenant, DateTime? fromDateN = null, DateTime? totoDayN = null)
        {
            DateTime fromDate = fromDateN ?? DateTime.Now.AddDays(-1).Date;
            DateTime toDay = totoDayN ?? DateTime.Now.Date;
            var sb = new StringBuilder();
            sb.Append("tenant=" + tenant.ToString())
                .Append("fromDate=").Append(fromDate)
                .Append("toDay=").Append(toDay);

            var requestStatusCode =
                this.repository.GetAll(tenant)
                .Where(rec => rec.RequestCreateDate >= fromDate)
                .Where(rec => rec.RequestCreateDate <= toDay)
                .GroupBy(rec => rec.RequestStatusCode)
                ;
            var list = requestStatusCode.ToList();
            //private IQueryable<IGrouping<string, CustomsRequestsSheet>> requestStatusCode;
            foreach (var itemG in list)
            {
                SheetStatusEnum requestStatusEnum;
                if (!Enum.TryParse<SheetStatusEnum>(itemG.Key, out requestStatusEnum))
                {
                    throw new Exception("!Enum.TryParse<SheetStatusEnum> " + itemG.Key);
                }
                sb.Append(requestStatusEnum.ToString()).Append("=")
                    .Append(itemG.Key)
                    .Append("=")
                    .AppendLine(itemG.Count().ToString());
            }
            return sb.ToString();
        }

        public List<CustomsRequestsSheetPM> GetAllReceivedDca(string dcaAnalyzeAggregateKey, int tenant)
        {
            var stage = ((int)SheetStatusEnum.Received).ToString();//21  ==SheetStatusEnum.Received
            var q =
            from rs in this.repository.GetAll(tenant)
            where
            rs.IsDCA &&
            rs.AnalyzeDcaAggregateKey == dcaAnalyzeAggregateKey &&
            rs.RequestStatusCode.Equals(stage)
            select rs;



            var theList = q.ToList();
            var pmList = theList.Select(rec => this.GetEntityPM(rec)).ToList();

            return pmList;

        }

        public CustomsRequestsSheetPM GetByEntityId2(int tenant, string InterfaceTypeCode, string RequestStatusCode, string ObjectTableId2, string EntityId2)
        {
            var q =
            (from rs in this.repository.GetAll(tenant)
             where
             rs.EntityId2 == EntityId2
             && rs.ObjectTableId2 == ObjectTableId2
             && rs.RequestStatusCode == RequestStatusCode//"30" 
             && rs.InterfaceTypeCode == InterfaceTypeCode//"2715"
             orderby rs.RequestCreateDate descending
             select rs
             );
            var poco = q.FirstOrDefault();
            var pm = this.GetEntityPM(poco);
            return pm;
        }

        public List<CustomsRequestsSheetPM> GetGeneralRequestInProgress(
            int Tenant,
            string InterfaceTypeCode,
            string ObjectTableId1, string EntityId1,
            string ObjectTableId2, string EntityId2,
            string CustomFileNo)
        {
            if (string.IsNullOrWhiteSpace(InterfaceTypeCode))
            {
                throw new Exception("GetGeneralRequestInProgress string.IsNullOrWhiteSpace(InterfaceTypeCode) ");
            }

            var listSheetStatusInProcess = new List<string>();
            foreach (var item in Enum.GetValues(typeof(SheetStatusInProcessEnum)))
            {
                listSheetStatusInProcess.Add(((int)item).ToString());
            }

            var q = //context.CustomsRequestsSheets
                this.repository.GetAll(Tenant)
                .Where(rec => rec.Tenant == Tenant)
                .Where(rec => listSheetStatusInProcess.Contains(rec.RequestStatusCode)
                    //rec.RequestStatusCode == "1" /*EnglishName	LocalName Created	בקשה נרשמה */
                    //||
                    //rec.RequestStatusCode == "2" /*EnglishName	LocalName In Process	באמצע טיפול*/
                    ////||
                    ////rec.RequestStatusCode == "5" /* Waiting For Signing	ממתין לחתימה */ //Yuval Chalup 06.08.2015 TASK-15156 (Remarked)
                    //||
                    //rec.RequestStatusCode == "20" /* Sent	נשלח */ //Yuval Chalup 06.08.2015 TASK-15156
                    //||
                    //rec.RequestStatusCode == "21" /* Received	התקבלה תשובה */ //Yuval Chalup 06.08.2015 TASK-15156
                    );

            q = q.Where(rec => rec.InterfaceTypeCode == InterfaceTypeCode);

            var haveFilter = false;
            if (!string.IsNullOrWhiteSpace(CustomFileNo))
            {
                haveFilter = true;
                q = q.Where(rec => rec.CustomFileNo == CustomFileNo);
            }

            if (!string.IsNullOrWhiteSpace(EntityId1) && !string.IsNullOrWhiteSpace(ObjectTableId1))
            {
                haveFilter = true;
                q = q.Where(rec => rec.EntityId1 == EntityId1 && rec.ObjectTableId1 == ObjectTableId1);
                if (!string.IsNullOrWhiteSpace(EntityId2) && !string.IsNullOrWhiteSpace(ObjectTableId2))
                {
                    q = q.Where(rec => rec.EntityId2 == EntityId2 && rec.ObjectTableId2 == ObjectTableId2);
                }
            }
            if (!haveFilter)
            {
                return new List<CustomsRequestsSheetPM>();
            }

            var pmList = q.ToList().Select(rec => this.GetEntityPM(rec)).ToList();
            return pmList;
        }
    }

    public class RequestInProgressParams
    {




        public int Tenant { get; set; }
        public string InterfaceTypeCode { get; set; }
        public string ObjectTableId1 { get; set; }
        public string EntityId1 { get; set; }
        public string ObjectTableId2 { get; set; }
        public string EntityId2 { get; set; }
        public bool DisplayOnlyMode { get; set; }
        public string CustomFileNo { get;  set; }
        public bool Include8250IsShaam { get; set; }
    }
}
