using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System.Configuration;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Customs.BL.BL;
using Logitude.Customs.Data.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.Def.ClosedTable;
using System.Xml.Linq;
 

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CustomsRequestsSheetUpdateService : EntityUpdateService<CustomsRequestsSheet, CustomsRequestsSheetPM, EntityPM>
    {
        protected override void OnCreating(CustomsRequestsSheetPM entityPM, EntityPM entityParentPM)
        {
            //entityPM.Id = IdCounter.GetNumber("Customs.CustomsRequestsSheet", entityPM.Tenant);
        }

        public void CancelRequests(List<CustomsRequestsSheetList> entityLists,int  tenant , ICustomContext customContext)
        {
             var us = new CustomsRequestsSheetUpdateService(customContext, new Dictionary<string, IContext>(),tenant);
            us.CommLogStepCanCancelledAction = CommLogStepCanCancelled;
            List<CustomsRequestsSheetPM> entityPMs= new List<CustomsRequestsSheetPM>();
            var qs = new CustomsRequestsSheetQueryService(tenant);
            string errors = "";
            entityLists.ForEach(x =>
            {
                try
                {
   CustomsRequestsSheetPM customsRequestsSheetPM = qs.GetSingle(x.Id, false,false);
               
                customsRequestsSheetPM.ChangeSetOp = ChangeSetOperation.Update;
                customsRequestsSheetPM.RequestStatusCode = "99";
                us.Update(customsRequestsSheetPM, true);
          //      entityPMs.Add(customsRequestsSheetPM);
                }
                catch (Exception ex)
                {

                    errors += ex.Message + '\n';
                }

             
            });

            if(errors!="")
            {
                throw new Exception(errors);
            }
         //    us.UpdateMulti(entityPMs, null, null, true);
        }

        public static void CommLogStepCanCancelled(CustomsRequestsSheet entityPOCO,
    CustomsRequestsSheetPM entityPM, DateTime? nowIs
    )
        {
            if (entityPOCO.RequestStatusCode == "30")
            {
                throw new Exception("Request already analyzed (CorrelationId: " + entityPM.CorrelationId);
            }
            nowIs = nowIs ?? DateTime.Now;
            bool isinteractive = false;
            var communicationLogStepQuery = new CommunicationLogStepQuery(entityPM.Tenant);
            var stepList = communicationLogStepQuery
                .GetCommunicationLogStepListsByLogId(entityPM.RequestComminicationId, entityPM.Tenant);
            DateTime? canCancellAtTime = null;

            if (!entityPM.IsDCA)
            {
                var stepReq = stepList.FirstOrDefault(rec => rec.StepNumber == (int)CustomsStepEnum.StartRequestParams);
                if (stepReq != null)
                {
                    var requestParamXml = stepReq.DocumentData;
                    isinteractive = Isinteractive(entityPM.Tenant, entityPM.RequestComminicationId);
                    if (isinteractive)
                    {
                        var startAt = entityPM.RequestCreateDate.GetValueOrDefault();
                        if (entityPM.FutureSendDateTime.HasValue)
                        {
                            if (entityPM.FutureSendDateTime.Value > entityPM.RequestCreateDate)
                            {
                                startAt = entityPM.FutureSendDateTime.Value;
                            }
                        }
                        if (nowIs.GetValueOrDefault().Subtract(startAt) > TimeSpan.FromMinutes(10))
                        {
                            return;//can cancell
                        }
                        canCancellAtTime = startAt.AddMinutes(10);
                    }
                }
            }
            if (!ResponseDataBase.RequestSheetCanCancelled(entityPM.RequestStatusCode, entityPM.IsDCA))
            {
                if (!isinteractive)//whill exec later !!
                {
                    ///throw new Exception("Unable to cancel request. It has already been sent (Batch proccess)");
                }

                var currStep = stepList.OrderBy(r => r.StepNumber).FirstOrDefault(r => r.Status != CommStatusEnum.D.ToString());
                if (currStep != null)
                {
                    if (currStep.StepNumber == (int)CustomsStepEnum.ReceivedCustomResponseCorrelation)
                    {
                        if (currStep.Status != CommStatusEnum.W.ToString())
                        {
                            if (canCancellAtTime != null)
                            {
                                throw new Exception(GetMessage(canCancellAtTime)+" (CorrelationId: " + entityPM.CorrelationId);
                            }
                            throw new Exception("Unable to cancel request. It has already been sent (CorrelationId: " + entityPM.CorrelationId);
                        }
                    }
                    else if (currStep.StepNumber > (int)CustomsStepEnum.ReceivedCustomResponseCorrelation)
                    {
                        if (canCancellAtTime != null)
                        {
                            throw new Exception(GetMessage(canCancellAtTime));
                        }
                        throw new Exception("Unable to cancel request. It has already been sent (CorrelationId: " + entityPM.CorrelationId);

                    }
                }
            }
        }


        public static bool Isinteractive(int tenant, string RequestComminicationId)
        {
            SendRequestVIA? curSendRequestVIA = null;
            var communicationLogStepQuery = new CommunicationLogStepQuery(tenant);
            var requestParamXml = communicationLogStepQuery.GetStartRequestParams(tenant, RequestComminicationId);
            if (!string.IsNullOrWhiteSpace(requestParamXml))
            {
                var xdoc = XDocument.Parse(requestParamXml);
                var eleRequestVIA = xdoc.Descendants("RequestVIA").FirstOrDefault();
                var eleRequestVIAValue = eleRequestVIA.Value;
                if (!String.IsNullOrWhiteSpace(eleRequestVIAValue))
                {
                    SendRequestVIA SendRequestVIA;
                    if (Enum.TryParse<SendRequestVIA>(eleRequestVIAValue, out SendRequestVIA))
                    {
                        curSendRequestVIA = SendRequestVIA;
                    }
                    //var RequestVIAChangeDueEle = xdoc.Descendants("RequestVIAChangeDue").FirstOrDefault();
                    //if (RequestVIAChangeDueEle != null)
                    //{
                    //    RequestVIAChangeDue = RequestVIAChangeDueEle.Value;
                    //}
                }
            }
            if (!curSendRequestVIA.HasValue)
            {
                return true;//default 
            }
            switch (curSendRequestVIA.Value)
            {
                case SendRequestVIA.WebServiceInteractive:
                    return true;//default 
                    break;
                case SendRequestVIA.Default:
                case SendRequestVIA.WebServiceBatch:
                case SendRequestVIA.DCABatch:
                default:
                    return false;//default 
                    break;
            }
        }


        private static string GetMessage(DateTime? canCancellMoreTime)
        {
            return $"  נשלח בתהליך אינטראקטיבי יתאפשר ביטול החל מהשעה {canCancellMoreTime.GetValueOrDefault().ToShortTimeString()}";
        }
        protected override void OnUpdating(CustomsRequestsSheetPM entityPM, CustomsRequestsSheet entityPOCO)
        {


            if (entityPOCO.RequestStatusCode == "99" && entityPM.RequestStatusCode != "99")
            {
                throw new CustomsRequestsSheetDomainModelServiceException(
                        CustomsRequestsSheetDomainModelServiceException.WhereEnum.RequestCancelled,
                        CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue,
                        @"CustomsRequestsSheetUpdateService.OnUpdating() No Way RequestCancelled can change !!",
                        null);
            }
            if (entityPM.RequestStatusCode == "99")
            {


                bool tryConcurrentKiller = true;/// ConfigurationManager.AppSettings["20180718.ConcurrentKiller"] == "1";
                if (tryConcurrentKiller)
                {
                    if (CustomsRequestsSheetQueryService.GetintrefaceTypeListDisplayOnly().ToList().Contains(entityPOCO.InterfaceTypeCode))
                    {
                        CustomsRequestsSheetDomainModelUtil.ReleaseConcurrentVirtualKey(new GenericRequestParams()
                        {
                            Tenant = entityPM.Tenant,
                            LoggingEntityId = entityPM.EntityId1,
                            LoggingObjectTableId = entityPM.ObjectTableId1,
                            InterfaceTypeCode = entityPM.InterfaceTypeCode,
                            //MainInterfaceCode = entityPM.InterfaceTypeCode,

                        });
                    }
                }
                bool movetoCommLogStepCanCancelledAction = true;
                if (!movetoCommLogStepCanCancelledAction)
                {
                    if (!ResponseDataBase.RequestSheetCanCancelled(entityPOCO.RequestStatusCode, entityPOCO.IsDCA))
                    {
                        throw new Exception("Unable to cancel request. It has already been sent");
                    }
                }




                if (CommLogStepCanCancelledAction == null)
                {
                    throw new Exception("While canceling its must Set CommLogStepCanCancellAction !!!");
                }
                else
                {
                    CommLogStepCanCancelledAction(entityPOCO, entityPM, null);
                }
                
                Request2715(entityPOCO);
                RequestCourier(entityPOCO);

                base.OnUpdating(entityPM, entityPOCO);
            }
        }

        private void RequestCourier(CustomsRequestsSheet entityPOCO)
        {
            if (entityPOCO.InterfaceTypeCode == "2755" || entityPOCO.InterfaceTypeCode == "2750" || entityPOCO.InterfaceTypeCode == "1170")
            {
                if (entityPOCO.ObjectTableId1 == ObjectTableRepository.GetObjectTableByName("Customs.Declaration") && !String.IsNullOrWhiteSpace(entityPOCO.EntityId1))
                {
                    var customContext = MainContext as ICustomContext;
                    DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(customContext);
                    DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(entityPOCO.EntityId1, true, false);
                    if (currentDeclarationCourierStatusPM != null)
                    {
                        string prevVal = null;
                        string currvVal = null;
                        CalculateDeclarationCourierStatus calculateDeclarationCourierStatus = new CalculateDeclarationCourierStatus(null, entityPOCO.EntityId1, entityPOCO.Tenant);
                        switch (entityPOCO.InterfaceTypeCode)
                        {
                            case "2755":
                                prevVal = currentDeclarationCourierStatusPM.CourierPaymentStatusCode;
                                calculateDeclarationCourierStatus.CalcCourierPaymentStatusCode(currentDeclarationCourierStatusPM);
                                currvVal = currentDeclarationCourierStatusPM.CourierPaymentStatusCode;
                                break;
                            case "2750":
                                prevVal = currentDeclarationCourierStatusPM.CourierDeclarationStatusCode;
                                calculateDeclarationCourierStatus.CalcCourierDeclarationStatusCode(currentDeclarationCourierStatusPM);
                                currvVal = currentDeclarationCourierStatusPM.CourierDeclarationStatusCode;
                                break;
                            case "1170":
                                prevVal = currentDeclarationCourierStatusPM.CourierManifestStatusCode;
                                calculateDeclarationCourierStatus.CalcCourierManifestStatusCode(currentDeclarationCourierStatusPM);
                                currvVal = currentDeclarationCourierStatusPM.CourierManifestStatusCode;
                                break;
                            default:
                                break;
                        }
                        if (prevVal != currvVal)
                        {
                            DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(customContext, new Dictionary<string, IContext>(), entityPOCO.Tenant);
                            currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                            declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
                        }
                    }
                }
            }
        }

        private void Request2715(CustomsRequestsSheet entityPOCO)
        {
            if (entityPOCO.InterfaceTypeCode == "2715")//InterfaceTypeCode = "2715"
            {

                /*
                1	Sent	נשלח	1	0
                2	Fail	נכשל	2	0
                3	Needed	נדרש	3	0
                4	Verified	אומת	4	0
                5	Verified With Customer Presents	אומת בנוכחות הלקוח	5	0
                6	Verify Rejected	נדחה אימות	6	0
                7	Sent Without Answer	נשלח ללא תשובה	7	0
                 */
                //CustomsDocumentPM customsDocumentPM =null;
                //customsDocumentPM.DocumentStatusCode=null;
                var customContext = MainContext as ICustomContext;
                var customsDocumentQueryService = new CustomsDocumentQueryService(entityPOCO.Tenant);

                var LoggingObjectTableId2 = entityPOCO.ObjectTableId2; ;//
                if (LoggingObjectTableId2 != ObjectTableRepository.GetObjectTableByName("Customs.CustomsDocument"))
                {
                    throw new Exception("Unable to cancel request. in 2715 ObjectTableId2 must be Customs.CustomsDocument! ");
                }
                var documentsfilingid = entityPOCO.EntityId2;//= entityPM.DocumentsFilingId,
                var customsDocumentPM = customsDocumentQueryService.GetSingle(documentsfilingid, false, false);
                if (customsDocumentPM == null)
                {
                    throw new Exception("Unable to cancel request. Current customsDocumentPM==null ");
                }
                if (customsDocumentPM.DocumentStatusCode != "7")
                {
                    throw new Exception("Unable to cancel request. Current customsDocumentPM.DocumentStatusCode!=7 ");
                }
                var featureDocumentStatusCodeShouldNOTChange = ConfigurationManager.AppSettings["20180624.DocumentStatusCodeShouldNOTChange"] == "1";

                if (featureDocumentStatusCodeShouldNOTChange &&
                    !String.IsNullOrWhiteSpace(customsDocumentPM.CustomsDocId))
                {
                    throw new Exception("Unable to cancel request. Current customsDocumentPM.CustomsDocId is not null  === Send !!!!");
                }
                var customsDocumentUpdateService = new CustomsDocumentUpdateService(customContext, new Dictionary<string, IContext>(), entityPOCO.Tenant);
                customsDocumentPM.ChangeSetOp = ChangeSetOperation.Update;
                customsDocumentPM.DocumentStatusCode = null;
                customsDocumentPM.CurrentContextTag = CustomsDocumentUpdateService.SetCustomsRequestSheetStatus;
                customsDocumentUpdateService.Update(customsDocumentPM, true);

            }
        }


        public Action<CustomsRequestsSheet, CustomsRequestsSheetPM,DateTime?> CommLogStepCanCancelledAction  { get; set; }
        

        protected override void AfterUpdating(CustomsRequestsSheetPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete)
            {
                string entityKeyString = GetUniqCacheKey(entityPM.Id);
                CacheManager.CacheWrapper.Insert(entityKeyString, EntityPM);
            }
            base.AfterUpdating(entityPM, entityParentPM);
        }
        public static CustomsRequestsSheetPM GetFromCache(String CustomsRequestsSheetId)
        {
            string entityKeyString = GetUniqCacheKey(CustomsRequestsSheetId);
            var cacheObj = CacheManager.CacheWrapper.Get(entityKeyString);
            if (cacheObj != null)
            {
                return cacheObj as CustomsRequestsSheetPM;
            }
            else
            {
                return null;
            }
        }
        private static string GetUniqCacheKey(String CustomsRequestsSheetId)
        {
            var entityKeys = new CustomsRequestsSheetKeys() { Id = CustomsRequestsSheetId };
            string entityKeyString = entityKeys.GetEntityPMName() + "_CustomsRequestsSheetUpdateService_" + entityKeys.GetFullKey();
            return entityKeyString;
        }

      
    }

}
