using Logitude.Customs.BL.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.RequestParams;
using Logitude.CustomsMessaging.ResponseData;
using Logitude.CustomsMessaging.ResponseServices.DeclarationErrorPointer;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.StorageResponseFromWarehouseServiceReference;


namespace Logitude.CustomsMessaging.ResponseServices
{
    public class ST_NG_70_MSG3_StorageResponseFromWarehouseResponseService :
        ResponseServiceBase<INF_MSG_GenericResponseData, ST_NG_70_MSG3_StorageResponseFromWarehouse, GenericRequestParams>
    {

        DeclarationPM _MyDeclarationPM;

        public override INF_MSG_GenericResponseData GetResponse(
            ST_NG_70_MSG3_StorageResponseFromWarehouse customResponse, GenericRequestParams requestParams)
        {

            return this.MyResponseData;
        }

        public override void Update(
            ST_NG_70_MSG3_StorageResponseFromWarehouse customResponse,
            GenericRequestParams requestParams)
        {
            if (customResponse.StorageResponseFromWarehouse == null)
            {
                LogMessagingUtil.Instance.AppendLine("No Storage details in the Response " + requestParams.AppicationId);
                return;
            }
            var declarationNumber = customResponse.StorageResponseFromWarehouse.declerationNumber;
            var responseName = requestParams.ResponseName;

            var context = CustomContext.GetContext(requestParams.Tenant);
            var myQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);

            this._MyDeclarationPM = myQueryService.GetSingle(requestParams.AppicationId, true, false);

            if (this._MyDeclarationPM == null)
            {
                LogMessagingUtil.Instance.AppendLine("Can not found declaration" + requestParams.AppicationId);
                return;
            }
            if (this._MyDeclarationPM.DeclarationNumber != declarationNumber)
            {
                LogMessagingUtil.Instance.AppendLine("Declaration " + this._MyDeclarationPM.DeclarationNumber + " found by requestParams.AppicationId(" + requestParams.AppicationId + ") is different than the Declaration Number in the response " + declarationNumber);
                return;
            }
            LogMessagingUtil.Instance.AppendLine("Analyze Storage response" + requestParams.AppicationId);

            //Raise Storage event 
            string eventCode = "";
            string remarks = "";
            if(customResponse.StorageResponseFromWarehouse.responseStatus == 1)
            {
                eventCode = "BRA";
                remarks = "ספרור בקשה: " + customResponse.StorageResponseFromWarehouse.storageRequestMessageNum
                    + ", מספר הצהרה: " + customResponse.StorageResponseFromWarehouse.declerationNumber + 
                    ", גירסת המצהרה: " + customResponse.StorageResponseFromWarehouse.versionNumber ;
            }
            else if(customResponse.StorageResponseFromWarehouse.responseStatus == 2)
            {
                string rejectMess = "";
                switch (customResponse.storageRejectionReason[0])
                {
                    case 1:
                        rejectMess = ", סיבת דחייה: ביטוח לא תואם";
                        break;
                    case 2:
                        rejectMess = ", סיבת דחייה: אופי טובין לא מתאים";
                        break;
                    case 3:
                        rejectMess = ", סיבת דחייה: מיקום במחסן";
                        break;
                    case 0:
                        rejectMess = ", לא התקבל קוד דחייה ";
                        break;
                    default:
                        rejectMess = ", קוד דחייה לא ניתן לתרגום " + customResponse.storageRejectionReason[0];
                        break;
                }

                eventCode = "BRD";
                remarks = "ספרור בקשה: " + customResponse.StorageResponseFromWarehouse.storageRequestMessageNum
                   + ", מספר הצהרה: " + customResponse.StorageResponseFromWarehouse.declerationNumber +
                   ", גירסת המצהרה: " + customResponse.StorageResponseFromWarehouse.versionNumber + rejectMess;
            }
            if (eventCode != "")
            {
                RaiseStorageEvent(_MyDeclarationPM, requestParams.LoggingUserId, eventCode, remarks);
            }

            //_MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
            //_MyDeclarationPM.CurrentContextTag = Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService;
            //myDeclarationUpdateService.Update(_MyDeclarationPM, true);

            this.MyResponseData = new INF_MSG_GenericResponseData();
            MyResponseData.ApplicationID = requestParams.AppicationId;
            MyResponseData.Succeeded = true;

        }

        private void RaiseStorageEvent(DeclarationPM _MyDeclarationPM, string loggingUserId, string eventCode, string remarks)
        {

            try
            {
                string eventType="";
                if (eventCode == "BRA")
                {
                    eventType = "approved";
                }
                else if (eventCode == "BRA")
                {
                    eventType = "deny";
                }

                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
               {
                   Tenant = _MyDeclarationPM.Tenant,
                   objectTableName = "Customs.Declaration",
                   EventCode = eventCode,
                   notes = remarks,
                   CommunicationLoggingEntityReference = _MyDeclarationPM.DeclarationNumber,
                   EntityId = _MyDeclarationPM.Id,
                   UserId = loggingUserId,

                   CommunicationSubject = "FU Status " + eventCode + " from logitude (Declaration Storage " + eventType + ")",
                   MyFUStatus = new AmitalEventTracerModel.FUStatus()
                   {
                       entname = "CFIFILEM",
                       primary_number = _MyDeclarationPM.CustomFileNo,
                       status = "new",
                       xml_status = "new",
                       status_id = eventCode,
                       status_DateTime = DateTime.Now,
                       status_place = "FRA",
                       status_save = "no_fail",
                       comments = remarks,
                   }
               };

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent  eventCode =" + eventCode + "  CustomFileNo= " + _MyDeclarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);

            }
            catch (System.Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }
    }
}