using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.GatepassFeedbackMServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class GP_1035_GatepassFeedbackMessageResponseService : ResponseServiceBase<GatepassFeedbackMessageResponseData, GP_NG_1035_MSG2_GatepassFeedbackMessage, GatepassRequestMessageRequestParams>
    {
        GatepassRequestPM _GatepassRequestPM;

        public override GatepassFeedbackMessageResponseData GetResponse(GP_NG_1035_MSG2_GatepassFeedbackMessage customResponse, GatepassRequestMessageRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(GP_NG_1035_MSG2_GatepassFeedbackMessage customResponse, GatepassRequestMessageRequestParams requestParams)
        {

            ICustomContext customContext = CustomContext.GetContext(requestParams.Tenant);
            var gatepassRequestQueryService = new GatepassRequestQueryService(customContext);
            var gatepassRequestUpdateService = new GatepassRequestUpdateService(customContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);

            this.MyResponseData = new GatepassFeedbackMessageResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "שאילתא לדרישת חוקיות לפרט מכס בוצעה בהצלחה";

            if (customResponse.GatepassFeedbackMessage == null || (customResponse.GatepassFeedbackMessage != null && customResponse.GatepassFeedbackMessage.Count() == 0))
            {
                LogMessagingUtil.Instance.AppendLine("GatepassFeedbackMessage is empty");
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "לא התקבלו נתונים מהמכס";
                return;
            }

            foreach (var gatepassFeedbackMessageItem in customResponse.GatepassFeedbackMessage)
            {
                this._GatepassRequestPM = gatepassRequestQueryService.GetGatepassRequestByGatepassNumber(gatepassFeedbackMessageItem.gatepassNumber, requestParams.Tenant);
                if (this._GatepassRequestPM != null)
                {
                    if(this._GatepassRequestPM.CustomsUpdateDateTime != null && this._GatepassRequestPM.CustomsUpdateDateTime.Value.Date > gatepassFeedbackMessageItem.dateTime)
                    {
                        LogMessagingUtil.Instance.AppendLine("GatepassFeedbackMessage was rejected because it is out of date");
                        this.MyResponseData.HasException = true;
                        this.MyResponseData.UserMessage = "המסר נדחה בגלל שהוא עדכני לשעה " + gatepassFeedbackMessageItem.dateTime.Date.ToString("dd/MM/yyyy") + " ויש עדכון משעה " + this._GatepassRequestPM.CustomsUpdateDateTime.Value.Date.ToString("dd/MM/yyyy");
                        return;
                    }
                    this._GatepassRequestPM.GatepassRequestStatus = gatepassFeedbackMessageItem.gatepassStatus.ToString();
                    this._GatepassRequestPM.CustomsUpdateDateTime = gatepassFeedbackMessageItem.dateTime;
                    this._GatepassRequestPM.ChangeSetOp = ChangeSetOperation.Update;

                    EventContextTagModel myEventContextTagModel = new EventContextTagModel()
                    {
                        StatusDateTime = DateTime.Now,
                    };

                    switch(gatepassFeedbackMessageItem.recordCategory)
                    {
                        case 1: // מסר משוב על קליטת בקשת העברה
                            if(gatepassFeedbackMessageItem.gatepassStatus == 1)
                            {

                            }
                            else if (gatepassFeedbackMessageItem.gatepassStatus == 2)
                            {

                            }
                            break;
                        case 2: // מסר משוב על קליטת בקשת ביטול
                            break;
                        case 3: // מסר אישור/דחייה בקשת העברה
                            break;
                        case 4: // מסר אישור/דחייה בקשת ביטול
                            break;
                    }
                    RaiseEvent(myEventContextTagModel);

                    gatepassRequestUpdateService.Update(this._GatepassRequestPM, true);


                }
                else
                {
                    LogMessagingUtil.Instance.AppendLine("Can not find GatepassRequest for GatepassNumber: " + gatepassFeedbackMessageItem.gatepassNumber.ToString());
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.UserMessage = "לא נמצא גייטפס";
                    return;
                }
            }
        }

        private void RaiseEvent(EventContextTagModel myEventContextTagModel)
        {
            string loggingUserId = AuthenticationUtil.ResolveUserId(_GatepassRequestPM.Tenant);
            try
            {
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = _GatepassRequestPM.Tenant,
                    objectTableName = "Customs.CourierMaster",
                    EventCode = myEventContextTagModel.EventCode,
                    notes = myEventContextTagModel.EventRemarks,
                    CommunicationLoggingEntityReference = _GatepassRequestPM.GatepassNumber.ToString(),
                    EntityId = _GatepassRequestPM.MasterCourierId,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status " + myEventContextTagModel.EventCode + " from logitude ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        status = "new",
                        xml_status = "new",
                        status_id = myEventContextTagModel.EventCode,
                        status_DateTime = DateTime.Now,
                        status_save = "no_fail",
                        comments = myEventContextTagModel.EventRemarks,
                    }
                };

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent  eventCode =" + myEventContextTagModel.EventCode + "  GatepassNumber= " + _GatepassRequestPM.GatepassNumber + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel, true);

            }
            catch (System.Exception ex)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }

        }
    }
}
