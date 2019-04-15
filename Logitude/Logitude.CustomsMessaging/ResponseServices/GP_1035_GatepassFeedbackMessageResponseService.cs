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
            var myCourierMasterQueryService = new CourierMasterQueryService(customContext);
            var gatepassRequestUpdateService = new GatepassRequestUpdateService(customContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);

            this.MyResponseData = new GatepassFeedbackMessageResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "משוב לבקשת העברה";

            if (customResponse.GatepassFeedbackMessage == null || (customResponse.GatepassFeedbackMessage != null && customResponse.GatepassFeedbackMessage.Count() == 0))
            {
                LogMessagingUtil.Instance.AppendLine("GatepassFeedbackMessage is empty");
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "לא התקבלו נתונים מהמכס";
                return;
            }

            //Checking for Exceptions
            if (customResponse.ResponseContentHeader.Exception != null)
            {
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                LogMessagingUtil.Instance.AppendLine(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription);
            }

            foreach (var gatepassFeedbackMessageItem in customResponse.GatepassFeedbackMessage)
            {
                this._GatepassRequestPM = gatepassRequestQueryService.GetGatepassRequestByGatepassNumber(gatepassFeedbackMessageItem.gatepassNumber, requestParams.Tenant);
                if (this._GatepassRequestPM != null)
                {
                    CourierMasterPM courierMasterPM = myCourierMasterQueryService.GetSingle(_GatepassRequestPM.MasterCourierId, false, false);
                    if (courierMasterPM != null)
                    {
                        this.MyResponseData.UserMessage = " משוב לבקשת העברה" + courierMasterPM.AirlinePrefix + "-" + courierMasterPM.MAWB;
                    }
                    if (this._GatepassRequestPM.CustomsUpdateDateTime != null && this._GatepassRequestPM.CustomsUpdateDateTime.Value.Date > gatepassFeedbackMessageItem.dateTime)
                    {
                        LogMessagingUtil.Instance.AppendLine("GatepassFeedbackMessage was rejected because it is out of date");
                        this.MyResponseData.HasException = true;
                        this.MyResponseData.UserMessage = "המסר נדחה בגלל שהוא עדכני לשעה " + gatepassFeedbackMessageItem.dateTime.Date.ToString("g") + " ויש עדכון משעה " + this._GatepassRequestPM.CustomsUpdateDateTime.Value.Date.ToString("g");
                        return;
                    }
                    
                    this._GatepassRequestPM.ChangeSetOp = ChangeSetOperation.Update;
                    this._GatepassRequestPM.CustomsUpdateDateTime = gatepassFeedbackMessageItem.dateTime;

                    EventContextTagModel myEventContextTagModel = new EventContextTagModel()
                    {
                        StatusDateTime = DateTime.Now,
                    };

                    switch(gatepassFeedbackMessageItem.recordCategory)
                    {
                        case 1: // מסר משוב על קליטת בקשת העברה
                            if(gatepassFeedbackMessageItem.gatepassStatus == 1)
                            {
                                this._GatepassRequestPM.GatepassRequestStatus = "2";
                                myEventContextTagModel.EventCode = "VGE";
                                myEventContextTagModel.EventRemarks = GetException(gatepassFeedbackMessageItem.Exception);
                                this.MyResponseData.HasException = true;
                                this.MyResponseData.UserMessage = string.Concat(this.MyResponseData.UserMessage, "\n", myEventContextTagModel.EventRemarks);
                            }
                            else if (gatepassFeedbackMessageItem.gatepassStatus == 2)
                            {
                                this._GatepassRequestPM.GatepassRequestStatus = "1";
                                myEventContextTagModel.EventCode = "VGR";
                                myEventContextTagModel.EventRemarks = "בקשת העברה ממתינה לאישור";
                            }
                            break;
                        case 2: // מסר משוב על קליטת בקשת ביטול
                            if (gatepassFeedbackMessageItem.gatepassStatus == 1)
                            {
                                this._GatepassRequestPM.GatepassRequestStatus = "6";
                                myEventContextTagModel.EventCode = "VGE";
                                myEventContextTagModel.EventRemarks = GetException(gatepassFeedbackMessageItem.Exception);
                                this.MyResponseData.HasException = true;
                                this.MyResponseData.UserMessage = string.Concat(this.MyResponseData.UserMessage, "\n", myEventContextTagModel.EventRemarks);
                            }
                            else if (gatepassFeedbackMessageItem.gatepassStatus == 2)
                            {
                                this._GatepassRequestPM.GatepassRequestStatus = "5";
                                myEventContextTagModel.EventCode = "VGR";
                                myEventContextTagModel.EventRemarks = "בקשת ביטול העברה ממתינה לאישור";
                            }
                            break;
                        case 3: // מסר אישור/דחייה בקשת העברה
                            if (gatepassFeedbackMessageItem.gatepassStatus == 2)
                            {
                                switch (gatepassFeedbackMessageItem.gatepassReturnCode)
                                {
                                    case 2:
                                        this._GatepassRequestPM.GatepassRequestStatus = "3";
                                        myEventContextTagModel.EventCode = "VGA";
                                        myEventContextTagModel.EventRemarks = GetReturnCode(gatepassFeedbackMessageItem.gatepassReturnCode, _GatepassRequestPM.Tenant);
                                        break;
                                    case 3:
                                    case 4:
                                        this._GatepassRequestPM.GatepassRequestStatus = "4";
                                        myEventContextTagModel.EventCode = "VGD";
                                        myEventContextTagModel.EventRemarks = GetReturnCode(gatepassFeedbackMessageItem.gatepassReturnCode, _GatepassRequestPM.Tenant);

                                        break;
                                }
                            }
                            break;
                        case 4: // מסר אישור/דחייה בקשת ביטול
                            if (gatepassFeedbackMessageItem.gatepassStatus == 2)
                            {
                                switch (gatepassFeedbackMessageItem.gatepassReturnCode)
                                {
                                    case 5:
                                        this._GatepassRequestPM.GatepassRequestStatus = "7";
                                        myEventContextTagModel.EventCode = "VGA";
                                        myEventContextTagModel.EventRemarks = GetReturnCode(gatepassFeedbackMessageItem.gatepassReturnCode, _GatepassRequestPM.Tenant);
                                        break;
                                    case 6:
                                    case 7:
                                        this._GatepassRequestPM.GatepassRequestStatus = "8";
                                        myEventContextTagModel.EventCode = "VGD";
                                        myEventContextTagModel.EventRemarks = GetReturnCode(gatepassFeedbackMessageItem.gatepassReturnCode, _GatepassRequestPM.Tenant);

                                        break;
                                }
                            }
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

        private string GetReturnCode(int? gatepassReturnCode, int tenant)
        {
            string returnCodeName = "";
            if (gatepassReturnCode != null)
            {
                GatepassReturnCodeQueryService gatepassReturnCodeQueryService = new GatepassReturnCodeQueryService(tenant);
                GatepassReturnCodePM gatepassReturnCodePM = gatepassReturnCodeQueryService.GetSingle(gatepassReturnCode.ToString(), false, true);
                if (gatepassReturnCodePM != null)
                {
                    returnCodeName = gatepassReturnCodePM.LocalName;
                }
            }
            return returnCodeName;
        }

        private string GetException(UnifreightIIG.Common.GatepassFeedbackMServiceReference.Exception[] exception)
        {
            string exceptionDescription = "";
            if (exception != null)
            {
                foreach (var item in exception)
                {
                    exceptionDescription += string.Concat(exceptionDescription, item.ExceptionLevel, ": ", item.ExeptionDescription, "\n");
                }
            }
            return exceptionDescription;
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
