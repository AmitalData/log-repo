using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.ResponseServices.DeclarationErrorPointer;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Utils;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ImportDeclarationServiceReference;
using UnifreightIIG.Common.MessageLib.Fault;
using UnifreightIIG.Common.MessageLib.ID;
using UnifreightIIG.Common.MessageLib.Collateral;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_NG_5117_ImportDeclerationAmendmentReplyResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg, GenericRequestParams>
    {
        DeclarationPM _MyDeclarationPM;
        private DeclarationPrintResponseData _SendDeclarationPrintResponse;

        public override void Update(DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg customResponse, GenericRequestParams requestParams)
        {
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();
            DeclarationCorrectionsPointerService myDeclarationCorrectionsPointerService = new DeclarationCorrectionsPointerService();

            requestParams.AppicationId = myDeclarationQueryService.GetIdByDeclarationNumber(customResponse.Response.Declaration.ID.Value, requestParams.Tenant);
            if (string.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                //if still not found search by ExternalDeclarationNumber
                requestParams.AppicationId = myDeclarationQueryService.GetIdByExternalDeclarationNumber(customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value, requestParams.Tenant);
            }

            var declarationNumber = "";
            if (customResponse.Response.Declaration != null && customResponse.Response.Declaration.ID != null && customResponse.Response.Declaration.ID.Value != null && customResponse.Response.Declaration.ID.Value.Substring(2, 2) == "99")
            {
                _MyDeclarationPM = myDeclarationUpdateService.GetSertByConvertedDeclarationNumber(customResponse.Response.Declaration.ID.Value, requestParams.Tenant);
               // declarationNumber = customResponse.Response.Declaration.ID.Value;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(requestParams.AppicationId))
                {
                    LogMessagingUtil.Instance.AppendLine("Can not find declaration- DeclarationNumber: " + customResponse.Response.Declaration.ID.Value + " ExternalDeclarationNumber: " + customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value);
                    this.MyResponseData.ApplicationID = requestParams.AppicationId;
                    this.MyResponseData.Succeeded = false;
                    this.MyResponseData.UserMessage = "Can not find declaration- DeclarationNumber: " + customResponse.Response.Declaration.ID.Value + " ExternalDeclarationNumber: " + customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value;
                    return;
                }
                //declarationNumber = requestParams.AppicationId;
            }
            declarationNumber = customResponse.Response.Declaration.ID.Value;

            //var key = "ResponseService,declarationNumber:" + declarationNumber + ",tenant:" + requestParams.Tenant.ToString();
            string key = ProcessLockTableUtil.Instance.GetKey4Declaration(declarationNumber, requestParams.Tenant);
            using (var disposableToken =
                //ProcessLockTableUtil.Instance.LockItAndGetReleaseToken(key, "5117ResponseService.Update")
                ProcessLockTableUtil.Instance.GetProcessLockTableDisposable(requestParams.Tenant, true, key, "5117ResponseService.Update")
                )
            {
                if (!(customResponse.Response.Declaration != null && customResponse.Response.Declaration.ID != null && customResponse.Response.Declaration.ID.Value != null && customResponse.Response.Declaration.ID.Value.Substring(2, 2) == "99"))
                {
                    this._MyDeclarationPM = myDeclarationQueryService.GetSingle(requestParams.AppicationId, true, false);
                }

                if (this._MyDeclarationPM == null)
                {
                    LogMessagingUtil.Instance.AppendLine("Can not find declaration" + requestParams.AppicationId);
                    this.MyResponseData.ApplicationID = requestParams.AppicationId;
                    this.MyResponseData.Succeeded = false;
                    this.MyResponseData.UserMessage = "Can not find declaration" + requestParams.AppicationId;
                    return;
                }

                var myUpdateEventContextTagModel = new EventContextTagModel()
                {
                    CallProccessID = EventContextTagModel.ProccessEnum.DF_NG_5117_ImportDeclerationAmendmentReplyResponseService,
                    EventCode = "DCH",
                    EventRemarks = "Declaration Changed By Customs",
                    FUStatusRemarks = "בוצע תיקון הצהרה" + this._MyDeclarationPM.DeclarationNumber,
                };

                this._MyDeclarationPM.CurrentContextTag = myUpdateEventContextTagModel;
                List<string> currentXmlVersionId = myDeclarationCorrectionsPointerService.GetVersionIdFromCorrectionXML(this._MyDeclarationPM.CorrectionsXml);
                if (currentXmlVersionId == null || !currentXmlVersionId.Contains(customResponse.Response.Declaration.DMExtensions.VersionID.Value))
                {

                    var customResponseResponseXml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.ID.Response>
                        .SerializeObject(customResponse.Response);

                    var importDeclarationServiceReferenceResponse = XmlGenericUtil<UnifreightIIG.Common.ImportDeclarationServiceReference.Response>
                        .DeSerializeObject(customResponseResponseXml);
                    ////5117 5117 5117 5117 5117
                    List<error> systemMessagesList = new List<error>();
                    if (customResponse.CollateralRequests != null && customResponse.CollateralRequests.Count() > 0) // Update Declaration Correction Pointer
                    {
                        foreach (var collateralRequestItem in customResponse.CollateralRequests)
                        {
                            var myError = new error();
                            myError.ListVersionID = "A";
                            myError.MessageError = "המשוב להצהרה כולל דרישה לבטוחה " + " - מספר בטוחה " + collateralRequestItem.collateralRequestNumber;
                            systemMessagesList.Add(myError);
                        }
                    }
                    this._MyDeclarationPM.CorrectionsXml = myDeclarationCorrectionsPointerService.AnalyzeCorrectionsPointer(this._MyDeclarationPM.CorrectionsXml, importDeclarationServiceReferenceResponse, systemMessagesList, requestParams.Tenant);
                }
                if (_MyDeclarationPM.UserNotes == "LoadTestOnProgress")
                {
                    _MyDeclarationPM.UserNotes = "LoadTest";
                }

                //<--- Yuval Chalup 18.10.2016 TASK-23113
                if (customResponse.ProceduralFaults != null)
                {
                    //Transfer ProceduralFaultDetails of 5117 to ProceduralFaultDetails of 8218
                    var ProceduralFaultDetailsXml_5117 = XmlGenericUtil<UnifreightIIG.Common.MessageLib.ID.ProceduralFaultDetails[]>
                        .SerializeObject(customResponse.ProceduralFaults);

                    var customResponse_8218 = new EV_NG_8218_MSG14100_ProceduralFaultMsg() { };
                    customResponse_8218.ProceduralFaultDetails = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Fault.ProceduralFaultDetails[]>
                        .DeSerializeObject(ProceduralFaultDetailsXml_5117);

                    var ResponseService_8218 = new EV_NG_8218_MSG14100_ProceduralFaultMsgResponseService();
                    ResponseService_8218.Update(customResponse_8218, requestParams);
                }
                //Yuval Chalup 18.10.2016 TASK-23113 --->

                this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                myDeclarationUpdateService.Update(this._MyDeclarationPM, true);

                this.MyResponseData.ApplicationID = requestParams.AppicationId;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = false;
                this.MyResponseData.UserMessage = "מענה לתיקון הצהרה  " + this._MyDeclarationPM.DeclarationNumber + " נקלט בהצלחה";

                this.MyRequestSheetParam = new RequestSheetParam();
                this.MyRequestSheetParam.CustomFileNo = this._MyDeclarationPM.CustomFileNo;
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId1 = this._MyDeclarationPM.Id;
                this.MyRequestSheetParam.RequestDescription = "מענה לתיקון הצהרה  " + this._MyDeclarationPM.DeclarationNumber;

                bool sendDeclarationPrintSync = false;// ConfigurationManager.AppSettings["20180307.5117SendDeclarationPrintSync"] =="1";
                if (sendDeclarationPrintSync)
                {
                    SendDeclarationPrintSync(requestParams);
                }
                else
                {
                    SendDeclarationPrint(requestParams);
                }

                if (customResponse.CollateralRequests != null) // Create Collateral
                {
                    LogMessagingUtil.Instance.AppendLine("ImportDeclarationAmendmentReplyMsg: Create Collateral");
                    var requestXml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.ID.CollateralRequestDetails[]>.SerializeObject(customResponse.CollateralRequests);
                    var collateralArry = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.CollateralRequestDetails[]>.DeSerializeObject(requestXml);

                    COLT_NG_8211_MSG10040_CollateralRequestMsg myCOLT_NG_8211_MSG10040_CollateralRequestMsg = new COLT_NG_8211_MSG10040_CollateralRequestMsg();
                    var responseContentHeader = customResponse.GetResponseContentHeader();
                    myCOLT_NG_8211_MSG10040_CollateralRequestMsg.ResponseContentHeader = new UnifreightIIG.Common.MessageLib.Collateral.ResponseContentHeader();
                    if (responseContentHeader != null)
                    {
                        myCOLT_NG_8211_MSG10040_CollateralRequestMsg.ResponseContentHeader.ApplicationID = responseContentHeader.ApplicationID;
                        myCOLT_NG_8211_MSG10040_CollateralRequestMsg.ResponseContentHeader.Remark = responseContentHeader.Remark;
                        myCOLT_NG_8211_MSG10040_CollateralRequestMsg.ResponseContentHeader.TransmitionDateTime = responseContentHeader.TransmitionDateTime;
                    }
                    myCOLT_NG_8211_MSG10040_CollateralRequestMsg.CollateralRequestDetails = collateralArry;
                    var xml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.COLT_NG_8211_MSG10040_CollateralRequestMsg>
                        .SerializeObject(myCOLT_NG_8211_MSG10040_CollateralRequestMsg);

                    var ser = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.COLT_NG_8211_MSG10040_CollateralRequestMsg>.DeSerializeObject(xml);
                    var DF_MSG10040_CollateralRequestMsgResponseService = new DF_8211_CollateralRequestMsgResponseService();
                    DF_MSG10040_CollateralRequestMsgResponseService.Update(ser, requestParams);
                }

            }
        }
        bool SendDeclarationPrintSync(GenericRequestParams requestParams)
        {

            bool SendDeclarationPrintDone = false;
            // Use this line to throw UnauthorizedAccessException, which we handle.
            Task<bool> task1 = Task<bool>.Factory.StartNew(() => SendDeclarationPrint(requestParams));

            // Use this line to throw an exception that is not handled. 
            //  Task task1 = Task.Factory.StartNew(() => { throw new IndexOutOfRangeException(); } ); 
            try
            {
                task1.Wait();
                SendDeclarationPrintDone = task1.Result;
            }
            catch (AggregateException ae)
            {
                if (true)
                {
                    throw ae.Flatten();
                }

                ae.Handle((x) =>
                {
                    if (x is UnauthorizedAccessException) // This we know how to handle.
                    {
                        Console.WriteLine("You do not have permission to access all folders in this path.");
                        Console.WriteLine("See your network administrator or try another path.");
                        return true;
                    }
                    return false; // Let anything else stop the application.
                });

            }

            Console.WriteLine("task1 has completed.");
            return SendDeclarationPrintDone;
        }

        public override INF_MSG_GenericResponseData GetResponse(DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }
        
        public bool SendDeclarationPrint(GenericRequestParams requestParams)
        {
            LogMessagingUtil.Instance.AppendLine("SendDeclarationPrint");
            string decNum = this._MyDeclarationPM.DeclarationNumber;
            var decNumList = new List<string>();
            decNumList.Add(decNum);
            DF_NG_8302_Web03_DeclarationPrintRequestParams searchParams = new DF_NG_8302_Web03_DeclarationPrintRequestParams()
            {
                LoggingEnabled = true,
                CustomFileNo = this._MyDeclarationPM.CustomFileNo,
                DeclarationNumber = decNumList, //declarationPM.DeclarationNumber,
                Tenant = this._MyDeclarationPM.Tenant,
                RequestName = "Declaration Print(5117)",
                ResponseName = "Declaration Print(5117)",
                LoggingEntityId = this._MyDeclarationPM.Id,
                RequestVIA = SendRequestVIA.WebServiceBatch,


                LoggingUserId = requestParams.LoggingUserId ,//HD CALL#298426
            };

            var myRequestMessagingService = new DF_NG_8302_Web03_DeclarationPrintMessagingService();
            var resData = myRequestMessagingService.Send(searchParams);
            _SendDeclarationPrintResponse = resData;
            if (!resData.Succeeded)
            {
                LogMessagingUtil.Instance.AppendLine("Declaration Print Request Failed " + resData.CustomsRequestsSheetId + ", Message: " + resData.UserMessage);
                return false;
            }
            LogMessagingUtil.Instance.AppendLine("Declaration Print Request Succeeded " + resData.CustomsRequestsSheetId);
            return true;
        }
    }
}
