using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.Def.ClosedTable;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.CustomsMessaging.Common.Gen;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace WebFreight.Web.CustomWebServices
{
    /// <summary>
    /// Summary description for ServerStateWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ServerStateWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public string GetClientProgressBarIndicatorCurrentStage(string PBId)
        {
            return ClientProgressBarIndicatorService.GetClientProgressBarIndicatorCurrentStage(PBId);

        }
        [WebMethod]
        public byte[] GetSubscribeSignServerList(int tenant)
        {
            List<SubscribeSignServer> mySubscribeSignServerList = SignQueue.Instance.GetCopyOfMySubscribeSignServerList(tenant);
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(mySubscribeSignServerList.GetType());
            ser.Serialize(memstream, mySubscribeSignServerList);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;

        }
        [WebMethod]
        public string GetSubscribeSignServerListXml(int tenant)
        {
            List<SubscribeSignServer> mySubscribeSignServerList = SignQueue.Instance.GetCopyOfMySubscribeSignServerList(tenant);
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(mySubscribeSignServerList.GetType());
            ser.Serialize(memstream, mySubscribeSignServerList);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            return content;
        }

        [WebMethod]
        public string GetSignQueueXml(int tenant)
        {
            var signQueueList = SignQueue.Instance.GetSignQueueList(tenant);

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(signQueueList.GetType());
            ser.Serialize(memstream, signQueueList);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            return content;

        }

        public bool ClientIsAngular { get; set; }

        [WebMethod]
        public int CalcClientProgressBarIndicatorCurrentStage(
            int tenant, string CustomsRequestsSheetId,
            out  bool stopMeNow,
            out  bool continueInBackground,
            out  string responseDataXml)
        {
            // tenant = 202;
            //CustomsRequestsSheetId = "1-15015";
            var c = Calc(tenant, CustomsRequestsSheetId,
            out  stopMeNow,
            out  continueInBackground,
            out  responseDataXml);
            return (int)c;
        }
        public CustomsStepEnum Calc(int tenant, string CustomsRequestsSheetId,
            out  bool stopMeNow,
            out  bool continueInBackground,
            out  string responseDataXml)
        {
            string RequestVIAChangeDue = "";
            //CustomsStepEnum myCustomsStepEnum;
            List<CommunicationLogStepList> stepList = null;
            continueInBackground = stopMeNow = false;
            responseDataXml = null;
            var curStep = CustomsStepEnum.StartRequestParams;
            //var _CommunicationLogStepRepository = new  CommunicationLogStepRepository(tenant); 
            //this._CommunicationLogStepList = _CommunicationLogStepRepository.GetMultiCommunicationLog(this._CommunicationLog.Id, this._CommunicationLog.Tenant);
            bool toGetXml = false;
            if (string.IsNullOrWhiteSpace(CustomsRequestsSheetId))
            {
                stopMeNow = true;
                responseDataXml = "CustomsRequestsSheetId is missing";
                return curStep;
            }


            var cacheRequestStopedNoteClient = Simplog.Server.Infrastructure.Helpers.CacheManager.CacheWrapper.Get("Customs.General.RequestStopedNoteClient," + CustomsRequestsSheetId) as string;
            if (!string.IsNullOrWhiteSpace(cacheRequestStopedNoteClient))
            {
                responseDataXml = cacheRequestStopedNoteClient;
                stopMeNow = true;
                curStep = CustomsStepEnum.StartRequestParams;
                return curStep;
            }
            //stopMeNow = true;
            //responseDataXml = "CustomsRequestsSheetId not found in db";


            CustomsRequestsSheetPM customsRequestsSheetPM = null;
            var cacheSetInCustomsRequestsSheetUpdateServiceOnAfterUpdating = true;
            customsRequestsSheetPM = CustomsRequestsSheetUpdateService.GetFromCache(CustomsRequestsSheetId);
            if (customsRequestsSheetPM == null) 
            {
                var qs = new CustomsRequestsSheetQueryService(tenant);

                customsRequestsSheetPM = qs.GetSingle(CustomsRequestsSheetId, false, false);
            }

            if (customsRequestsSheetPM == null)
            {
                var cacheRequestInProgress = Simplog.Server.Infrastructure.Helpers.CacheManager.CacheWrapper.Get("Customs.General.RequestInProgressNoteClient," + CustomsRequestsSheetId) as string;
                if (!string.IsNullOrWhiteSpace(cacheRequestInProgress))
                {
                    responseDataXml = cacheRequestInProgress;
                    stopMeNow = true;

                }
                //stopMeNow = true;
                //responseDataXml = "CustomsRequestsSheetId not found in db";
                curStep = CustomsStepEnum.StartRequestParams;
                return curStep;
            }
            
            //pm.RequestComminicationId

            //StepNumber	Name
            //30	AnalyzeResponseData
            //0	StartRequestParams
            List<int> reqDataList = new List<int>();
            reqDataList.Add((int)CustomsStepEnum.StartRequestParams);
            var closeTable = new Logitude.Customs.Def.ClosedTable.CustomsRequestsSheetStatusDetails();
            var sts = closeTable.GetAll().FirstOrDefault(rec => rec.Code == customsRequestsSheetPM.RequestStatusCode);
            ///curStep = customsRequestsSheetPM.RequestStatusEnum;
            switch (customsRequestsSheetPM.RequestStatusEnum)
            {
                case SheetStatusEnum.SendFailed:
                    curStep = CustomsStepEnum.StartRequestParams;
                    stopMeNow = true;
                    responseDataXml = GetExceptionMessage(tenant, customsRequestsSheetPM.RequestComminicationId) ?? sts.LocalName;
                    if (string.IsNullOrWhiteSpace(responseDataXml))
                    {
                        responseDataXml = sts.LocalName;
                    }

                    return curStep;
                    break;
                case SheetStatusEnum.ReceivedFailed:
                    curStep = CustomsStepEnum.ReceivedCustomResponseCorrelation;
                    stopMeNow = true;
                    responseDataXml = sts.LocalName;
                    return curStep;
                    break;
                case SheetStatusEnum.SentResponseOnDCA:
                    curStep = CustomsStepEnum.ReceivedCustomResponseCorrelation;
                    stopMeNow = true;
                    responseDataXml = "בעקבות עומס במכס המשוב יתקבל בכספת";//sts.LocalName;
                    return curStep;
                case SheetStatusEnum.AnalyzeFailed:
                case SheetStatusEnum.Analyzed:
                    stopMeNow = true;
                    toGetXml = true;

                    reqDataList.Add((int)CustomsStepEnum.AnalyzeResponseData);
                    break;
                case SheetStatusEnum.Cancelled:
                    stopMeNow = true;
                    curStep = CustomsStepEnum.StartRequestParams;
                    responseDataXml = "Request Cancelled ";
                    return curStep;

                    break;
                case SheetStatusEnum.InProcess:

                    break;
                default:
                    break;
            }
            //if (reqDataList.Count>0 )//always
            //{
            var communicationLogStepQuery = new CommunicationLogStepQuery(tenant);
            stepList = communicationLogStepQuery
                .GetCommunicationLogStepsDocumentData(customsRequestsSheetPM.RequestComminicationId, tenant, reqDataList.ToArray(), true);


            var inProgress = stepList.FirstOrDefault(r => r.Status == CommStatusEnum.P.ToString());
            if (inProgress != null)
            {
                inProgress.Status = CommStatusEnum.W.ToString();
            }



            stepList = stepList.OrderBy(rec => rec.StepNumber).ToList();
            var stepWaitOrFail =
                //stepList.FirstOrDefault(rec => rec.Status == "W" || rec.Status == "F"); ;
                stepList.FirstOrDefault(rec => rec.Status == CommStatusEnum.W.ToString() || rec.Status == CommStatusEnum.F.ToString()); ;
            if (stepWaitOrFail != null)
            {
                curStep = (CustomsStepEnum)stepWaitOrFail.StepNumber;
                responseDataXml = curStep.ToString() + " Failed";
            }


            //}
            SendRequestVIA? curSendRequestVIA = null;
            var stepReq = stepList.FirstOrDefault(rec => rec.StepNumber == (int)CustomsStepEnum.StartRequestParams);
            if (stepReq != null)
            {
                var requestParamXml = stepReq.DocumentData;
                if (!string.IsNullOrWhiteSpace(requestParamXml))
                {
                    var xdoc = XDocument.Parse(requestParamXml);
                    var eleRequestVIA = xdoc.Descendants("RequestVIA").FirstOrDefault();
                    SendRequestVIA SendRequestVIA;
                    if (Enum.TryParse<SendRequestVIA>(eleRequestVIA.Value, out SendRequestVIA))
                    {
                        curSendRequestVIA = SendRequestVIA;
                    }
                    var RequestVIAChangeDueEle = xdoc.Descendants("RequestVIAChangeDue").FirstOrDefault();
                    if (RequestVIAChangeDueEle != null)
                    {
                        RequestVIAChangeDue = RequestVIAChangeDueEle.Value;
                    }
                }
            }
            var stepResponse = stepList.FirstOrDefault(rec => rec.StepNumber == (int)CustomsStepEnum.AnalyzeResponseData);
            if (stepResponse != null)
            {
                responseDataXml = stepResponse.DocumentData;
                if (!string.IsNullOrWhiteSpace(responseDataXml))
                {
                    if(this.ClientIsAngular)
                    {
                        IMessagingServiceInterfaceType messagingService = null;
                        if (!String.IsNullOrWhiteSpace(customsRequestsSheetPM.InterfaceTypeCode))
                        {
                            messagingService = MessagingServiceFactoryHelper.GetMessagingService(customsRequestsSheetPM.InterfaceTypeCode);
                        }
                        if (messagingService != null)
                        {
                            responseDataXml = messagingService.ConvertStepDataToJSON(30, responseDataXml);
                        }

                    }
                    curStep = CustomsStepEnum.AnalyzeResponseData;
                    if (String.IsNullOrWhiteSpace( responseDataXml))
                    {
                        responseDataXml = "log:" +stepResponse.Log;
                    }
                    stopMeNow = true;
                    return curStep;
                }

            }
            if (curSendRequestVIA.HasValue)
            {
                switch (curSendRequestVIA.GetValueOrDefault())
                {
                    case SendRequestVIA.DCABatch:
                        ///case SendRequestVIA.DCABatch:
                        stopMeNow = true;
                        continueInBackground = true;
                        if (!String.IsNullOrWhiteSpace(RequestVIAChangeDue))
                        {
                            responseDataXml = RequestVIAChangeDue;
                        }
                        return curStep;
                        break;

                    case SendRequestVIA.WebServiceBatch:
                        ///case SendRequestVIA.DCABatch:
                        stopMeNow = true;
                        continueInBackground = true;
                        if (!String.IsNullOrWhiteSpace(RequestVIAChangeDue))
                        {
                            responseDataXml = RequestVIAChangeDue;
                        }
                        return curStep;
                        break;
                    default:
                        break;
                }
            }
            if (customsRequestsSheetPM.IsDCA)
            {
                var stepDCAInProgressUploading = stepList.FirstOrDefault(rec => rec.StepNumber == (int)CustomsStepEnum.DCAInProgressUploading);
                if (stepDCAInProgressUploading != null)
                {
                    if (stepDCAInProgressUploading.Status == "D")
                    {
                        stopMeNow = true;
                        continueInBackground = true;
                        return curStep;
                    }
                }

            }


            return curStep;
        }

        private string GetExceptionMessage(int tenant, string requestComminicationId)
        {
            var communicationLogSQuery = new CommunicationLogQuery(tenant);
            var commlog = communicationLogSQuery.GetSinglePM(requestComminicationId, tenant);
            if (commlog != null && !String.IsNullOrWhiteSpace(commlog.ExceptionMessage))
            {
                return commlog.ExceptionMessage;
            }
            return null;

        }
    }
}
