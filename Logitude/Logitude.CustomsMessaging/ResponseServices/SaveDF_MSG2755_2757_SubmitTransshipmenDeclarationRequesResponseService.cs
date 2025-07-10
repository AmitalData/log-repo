using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices.DeclarationErrorPointer;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.BL.BL;
using Logitude.Customs.BL.TraceEvents;
using UnifreightIIG.Common.SubmitTransshipmenDeclarationRequestServiceReference;
using Logitude.Customs.BL.Utils;
using Logitude.CustomsMessaging.MessagingServices;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class SaveDF_MSG2755_2757_SubmitTransshipmenDeclarationRequesResponseService :
            ResponseServiceBase<INF_MSG_GenericResponseData, DF_NG_2757_MSG10004_ExportDeclarationResponse, GenericRequestParams>
    {
        DeclarationPM _MyDeclarationPM;
        
        private DF_NG_2754_MSG10004_SubmitExportDeclarationResponseService _DF_NG_2754_MSG10004_SubmitExportDeclarationResponseService = new DF_NG_2754_MSG10004_SubmitExportDeclarationResponseService();
        private INF_MSG_GenericResponseData _MyDefaultResponseData;
        //ITZIK+MIRT public UnifreightIIG.Common.CommonIIGInterface.IResponseHeaderOrFault _ResponseHeaderExeption { get; set; }

        public override void OnRequestFail(DF_NG_2757_MSG10004_ExportDeclarationResponse customResponse, GenericRequestParams requestParams)
        {
            if (!String.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                var customContext = CustomContext.GetContext(requestParams.Tenant);
                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(customContext);
                DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(requestParams.AppicationId, true, false);
                if (currentDeclarationCourierStatusPM != null)
                {
                    string prevVal = null;
                    string currvVal = null;
                    CalculateDeclarationCourierStatus calculateDeclarationCourierStatus = new CalculateDeclarationCourierStatus(null, requestParams.AppicationId, requestParams.Tenant);
                    prevVal = currentDeclarationCourierStatusPM.CourierPaymentStatusCode;
                    calculateDeclarationCourierStatus.CalcCourierPaymentStatusCode(currentDeclarationCourierStatusPM);
                    currvVal = currentDeclarationCourierStatusPM.CourierPaymentStatusCode;

                    if (prevVal != currvVal)
                    {
                        DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(customContext, new Dictionary<string, IContext>(), requestParams.Tenant);
                        currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                        declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
                    }
                }
                var myQueryService = new DeclarationQueryService(requestParams.Tenant);

                this._MyDeclarationPM = myQueryService.GetSingle(requestParams.AppicationId, true, false);

                var myDeclarationPaymentQueryService = new DeclarationPaymentQueryService(_MyDeclarationPM.Tenant);
                var declarationPaymentPM = myDeclarationPaymentQueryService.GetSingle(_MyDeclarationPM.Id, true, false);

                if (declarationPaymentPM.AutomaticPayment == 1)
                {


                    var MyUnifreightEventParam = new UnifreightEventParam()
                    {
                        Code = "APAYF",
                        Mode = UnifreightEventMode.@new,
                        EventDateTime = DateTime.Now,
                        Entname = "CFIFILEM",
                        PrimaryNum = _MyDeclarationPM.CustomFileNo,
                    };
                    if (customResponse.ResponseContentHeader != null && customResponse.ResponseContentHeader.Exception != null && customResponse.ResponseContentHeader.Exception.Count() > 0)
                    {
                        MyUnifreightEventParam.EventRemarks = customResponse.ResponseContentHeader.Exception[0].ExeptionDescription;

                    }
                    else
                    {
                        MyUnifreightEventParam.EventRemarks = "כשלון בניתוח הגשת שטעון";

                    }
                    LogMessagingUtil.Instance.AppendLine("MyUnifreightEventParam = " + MyUnifreightEventParam ?? "NULL");
                    var myOpenUnifreighTask = new UnifreightEventTaskService();
                    myOpenUnifreighTask.UpsertEventLE2U(
                        _MyDeclarationPM.Tenant,
                       requestParams.LoggingUserId,
                        MyUnifreightEventParam);

                }

            }
            base.OnRequestFail(customResponse, requestParams);
        }


        public override void Update(DF_NG_2757_MSG10004_ExportDeclarationResponse customResponse, GenericRequestParams requestParams)
        {
            UnifreightIIG.Common.SubmitExportDeclarationRequestServiceReference.DF_NG_2757_MSG10004_ExportDeclarationResponse castCustomResponse =
                Serializer.CastXML<UnifreightIIG.Common.SubmitExportDeclarationRequestServiceReference.DF_NG_2757_MSG10004_ExportDeclarationResponse, DF_NG_2757_MSG10004_ExportDeclarationResponse>(customResponse);

            _DF_NG_2754_MSG10004_SubmitExportDeclarationResponseService.Update(castCustomResponse, requestParams);

            MyResponseData = _DF_NG_2754_MSG10004_SubmitExportDeclarationResponseService.MyResponseData;
        }

        public override INF_MSG_GenericResponseData GetResponse(DF_NG_2757_MSG10004_ExportDeclarationResponse customResponse, GenericRequestParams requestParams)
        {
            if (_MyDefaultResponseData == null && _DF_NG_2754_MSG10004_SubmitExportDeclarationResponseService._DF_NG_2757_MSG10004_ExportDeclarationResponseService_GetOnly.MyResponseData != null)
                _MyDefaultResponseData = _DF_NG_2754_MSG10004_SubmitExportDeclarationResponseService._DF_NG_2757_MSG10004_ExportDeclarationResponseService_GetOnly.MyResponseData;

            else if (_MyDeclarationPM != null && !_MyDeclarationPM.IsCourierDeclaration)
                SendDeclarationStatus();

            return _MyDefaultResponseData;
        }

        public void SendDeclarationStatus()
        {
            try
            {
                DeclarationStatusRequestParams searchParams = new DeclarationStatusRequestParams()
                {
                    LoggingEnabled = true,

                    CustomFileNo = this._MyDeclarationPM.CustomFileNo,
                    DeclarationNumber = this._MyDeclarationPM.DeclarationNumber,
                    Tenant = this._MyDeclarationPM.Tenant,
                    RequestName = "Declaration Status Search",
                    ResponseName = "Declaration Status Search",
                    SuppressSplitWR = true
                };

                searchParams.RequestVIA = SendRequestVIA.WebServiceInteractive;
                var resData = Logitude.CustomsMessaging.MessagingServices.DF_NG_8250_Web01_DeclarationStatus_RequestMessagingService.SendInteractive(searchParams);
                if (!resData.Succeeded)
                {
                    LogMessagingUtil.Instance.AppendLine("Request Failed " + resData.CustomsRequestsSheetId + ", Message: " + resData.UserMessage);
                }
            }
            catch
            {
                LogMessagingUtil.Instance.AppendLine("Sending Declaration Status Request Failed !!!");
            }

        }
    }
}
