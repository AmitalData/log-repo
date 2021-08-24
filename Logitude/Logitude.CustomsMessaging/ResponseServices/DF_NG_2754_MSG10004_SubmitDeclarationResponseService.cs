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
using UnifreightIIG.Common.ImportDeclarationSubmitRequestServiceReference;
using Logitude.Customs.BL.BL;
using Logitude.Customs.BL.TraceEvents;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_NG_2754_MSG10004_SubmitDeclarationResponseService:
        ResponseServiceBase<INF_MSG_GenericResponseData, DF_NG_2754_MSG10004_ImportDeclarationResponse, GenericRequestParams>
    {
        DeclarationPM _MyDeclarationPM;
        private DF_NG_2754_MSG10004_ImportDeclarationResponseService _DF_NG_2754_MSG10004_ImportDeclarationResponseService;
        private INF_MSG_GenericResponseData _MyDefaultResponseData;
        //ITZIK+MIRT public UnifreightIIG.Common.CommonIIGInterface.IResponseHeaderOrFault _ResponseHeaderExeption { get; set; }

        public override void OnRequestFail(DF_NG_2754_MSG10004_ImportDeclarationResponse customResponse, GenericRequestParams requestParams)
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
                    if (customResponse.ResponseContentHeader!= null && customResponse.ResponseContentHeader.Exception!= null && customResponse.ResponseContentHeader.Exception.Count()>0)
                    {
                        MyUnifreightEventParam.EventRemarks = customResponse.ResponseContentHeader.Exception[0].ExeptionDescription;

                    }
                    else
                    {
                        MyUnifreightEventParam.EventRemarks = "כשלון בניתוח הגשת תשלום";

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


        public override void Update(DF_NG_2754_MSG10004_ImportDeclarationResponse customResponse, GenericRequestParams requestParams)
        {
            var context = CustomContext.GetContext(requestParams.Tenant);

            if (customResponse.ResponseContentHeader != null && 
                customResponse.Response == null && 
                customResponse.AddAGlobalScannedAttachmentToEntityResponse == null &&
                customResponse.CollateralRequestDetails == null )
            {
                //Remark "בקשת ההגשה נקלטה בהצלחה ותטופל בתאריך: 20/06/2014 10:00:00"	string
                if (!String.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Remark) 
                    && customResponse.ResponseContentHeader.Remark.Contains("ותטופל בתאריך"))
                {
                    var myQueryService = new DeclarationQueryService(context);
                    var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
                    DeclarationErrorPointerService mydDclarationErrorPointerService = new DeclarationErrorPointerService();

                    this._MyDefaultResponseData = new INF_MSG_GenericResponseData();
                    this._MyDefaultResponseData.ApplicationID = requestParams.AppicationId;
                    this._MyDefaultResponseData.Succeeded = true;
                    this._MyDefaultResponseData.UserMessage = customResponse.ResponseContentHeader.Remark;
                    this._MyDefaultResponseData.HasException = false;

                    this._MyDeclarationPM = myQueryService.GetSingle(requestParams.AppicationId, true, false);
                    //Create “DFP” Event + UNF Status
                    EventContextTagModel myInsertEventContextTagModel = new EventContextTagModel()
                    {
                        CallProccessID = EventContextTagModel.ProccessEnum.DF_NG_2754_MSG10004_SubmitDeclarationFuturePayment,
                        EventCode = "DFP",
                        EventRemarks = "Declaration Future Payment ",
                    };
                    this._MyDeclarationPM.CurrentContextTag = myInsertEventContextTagModel;


                    //Update ErrosXml field
                    var declarationException = new UnifreightIIG.Common.ImportDeclarationServiceReference.Exception();
                    declarationException.ExeptionDescription = customResponse.ResponseContentHeader.Remark;
                    this._MyDeclarationPM.ErrosXml = mydDclarationErrorPointerService.AddDeclarationException(this._MyDeclarationPM.ErrosXml, "Warning", declarationException);
                    
                    //Update Status- Future Payment(In case of sending DeclarationStatus message will fail)
                    this._MyDeclarationPM.DeclarationStatusTypeCode = "10";
                    if (_MyDeclarationPM.UserNotes == "LoadTestOnProgress")
                    {
                        _MyDeclarationPM.UserNotes = "LoadTest";
                    }
                    this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;

                

                    myDeclarationUpdateService.Update(this._MyDeclarationPM, true);

                    //Send interactive declaration Status request - will send from GetResponse
                    //SendDeclarationStatus();

                    return;
                }
            }

            var myQueryService2 = new DeclarationQueryService(requestParams.Tenant);

            this._MyDeclarationPM = myQueryService2.GetSingle(requestParams.AppicationId, true, false);


            if (customResponse.ResponseContentHeader!=null && customResponse.ResponseContentHeader.Exception!=null && customResponse.ResponseContentHeader.Exception.Count()>0)
            {
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
                        EventRemarks = customResponse.ResponseContentHeader.Exception[0].ExeptionDescription,
                    };
                    LogMessagingUtil.Instance.AppendLine("MyUnifreightEventParam = " + MyUnifreightEventParam ?? "NULL");
                    var myOpenUnifreighTask = new UnifreightEventTaskService();
                    myOpenUnifreighTask.UpsertEventLE2U(
                        _MyDeclarationPM.Tenant,
                       requestParams.LoggingUserId,
                        MyUnifreightEventParam);

                }
            }
            else 
            {
                var myDeclarationPaymentQueryService = new DeclarationPaymentQueryService(context);
                var declarationPaymentPM = myDeclarationPaymentQueryService.GetSingle(requestParams.AppicationId, true, false);


                if (declarationPaymentPM.AutomaticPayment == 1)
                {


                    if (customResponse.Response != null && customResponse.Response.Error != null  && customResponse.Response.Error.Count()>0 )
                    {
                        var MyUnifreightEventParam = new UnifreightEventParam()
                        {
                            Code = "APAYF",
                            Mode = UnifreightEventMode.@new,
                            EventDateTime = DateTime.Now,
                            Entname = "CFIFILEM",
                            PrimaryNum = _MyDeclarationPM.CustomFileNo,
                            EventRemarks = customResponse.Response.Error[0].ValidationCode.Value,
                        };
                        LogMessagingUtil.Instance.AppendLine("MyUnifreightEventParam = " + MyUnifreightEventParam ?? "NULL");
                        var myOpenUnifreighTask = new UnifreightEventTaskService();
                        myOpenUnifreighTask.UpsertEventLE2U(
                            _MyDeclarationPM.Tenant,
                           requestParams.LoggingUserId,
                            MyUnifreightEventParam);

                    }
                    else {



                        var MyUnifreightEventParam = new UnifreightEventParam()
                        {
                            Code = "APAY",
                            Mode = UnifreightEventMode.@new,
                            EventDateTime = DateTime.Now,
                            Entname = "CFIFILEM",
                            PrimaryNum = _MyDeclarationPM.CustomFileNo,
                            EventRemarks = "",
                        };
                        LogMessagingUtil.Instance.AppendLine("MyUnifreightEventParam = " + MyUnifreightEventParam ?? "NULL");
                        var myOpenUnifreighTask = new UnifreightEventTaskService();
                        myOpenUnifreighTask.UpsertEventLE2U(
                            _MyDeclarationPM.Tenant,
                           requestParams.LoggingUserId,
                            MyUnifreightEventParam);

                    }
                }

            }

            //[XmlType(AnonymousType = true, Namespace = "http://malam.com/customs/DealFile/Declaration/DF_MSG10000_ImportDeclaration")]
            //[XmlType(AnonymousType = true, Namespace = "http://malam.com/customs/DealFile/Declaration/DF_MSG10000_ImportDeclaration")]
            UnifreightIIG.Common.ImportDeclarationServiceReference.DF_NG_2754_MSG10004_ImportDeclarationResponse ser = null;
            var xml = XmlGenericUtil<DF_NG_2754_MSG10004_ImportDeclarationResponse>.SerializeObject(customResponse);
            ser = XmlGenericUtil<UnifreightIIG.Common.ImportDeclarationServiceReference.DF_NG_2754_MSG10004_ImportDeclarationResponse>.DeSerializeObject(xml);
            _DF_NG_2754_MSG10004_ImportDeclarationResponseService = new DF_NG_2754_MSG10004_ImportDeclarationResponseService();
            _DF_NG_2754_MSG10004_ImportDeclarationResponseService._IsSubmitDeclarationResponse = true;
            //ITZIK+MIRT _DF_NG_2754_MSG10004_ImportDeclarationResponseService._ResponseHeaderExeption = _ResponseHeaderExeption;

            _DF_NG_2754_MSG10004_ImportDeclarationResponseService.Update(ser, requestParams);
        }

        public override INF_MSG_GenericResponseData GetResponse(DF_NG_2754_MSG10004_ImportDeclarationResponse customResponse, GenericRequestParams requestParams)
        {
            //var respose = _DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData ?? _MyDefaultResponseData;
            //return respose;

            if (_MyDefaultResponseData == null && _DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData != null)
            {
                this._MyDefaultResponseData = _DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData;
            }
            else
            {
                //Send interactive declaration Status request
                if (!_MyDeclarationPM.IsCourierDeclaration)
                {
                    SendDeclarationStatus();
                }
            }
            return this._MyDefaultResponseData;
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
