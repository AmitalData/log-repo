using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using System.Diagnostics;
using System;
using Logitude.AmitalMessaging.Utils;
using System.Collections.Generic;
using System.IO;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Customs.BL.EntityPMs;
using Logitude.Server.Tools.ExternalServices;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityKeys;
using Logitude.CustomsMessaging.Common.DCAParams;
using System.Threading;
using Logitude.Customs.BL.ClosedTable;
using Logitude.CustomsMessaging.Common.RequestParams;
using System.Text;


namespace Logitude.CustomsMessaging.MessagingServices
{
    public abstract partial class MessagingServiceBase<TRequestParams, TResponseData, TCustomsRequest, TCustomsResponse, TRequestService, TResponseService, TRequestHeader>
: Logitude.CustomsMessaging.MessagingServices.IMessagingServiceBase<TRequestParams, TResponseData, TCustomsRequest, TCustomsResponse, TRequestHeader>
    {
#if false
        public string DcaReceivedCustomResponseCorrelationCrashIfNotValid(
           Logitude.Customs.BL.EntityPMs.InterfaceManagementPM currentDCAInInterfaceManagementPM, int tenant,
           string selectedFile, byte[] messageBytes
           , bool pseudo = false)
        {

            string result = System.Text.Encoding.UTF8.GetString(messageBytes);
            var myESBResponseParser = new UnifreightIIG.Common.MessageLib.General.ESBResponseParser(result);
            var errorMessage = "";
            var prased = myESBResponseParser.Procces(out errorMessage);
            if (!prased)
            {
                throw new Exception("Bad XML FILE Can not Parse myESBResponseParser.Procces()" + errorMessage);
            }

            string externalId = GetExternalId(selectedFile);
            if (externalId != myESBResponseParser.ResponseHeader.ExternalId)
            {
                throw new Exception("it must be GetExternalId(selectedFile) (" + externalId + ")  == myESBResponseParser.ResponseHeader.ExternalId (" + myESBResponseParser.ResponseHeader.ExternalId + ")");
            }
            var sb = new StringBuilder();
            sb
                .AppendLine("currentDCAInInterfaceTypePM.Code=" + currentDCAInInterfaceManagementPM.Code)
                .AppendLine("this.UnityId=" + this.MainInterfaceCode)
                .AppendLine("DCAFileName=" + selectedFile + "/tenant=" + tenant.ToString());
            LogMessagingUtil.Instance.AppendLine(sb.ToString());
            //LogMessagingUtil.Instance.AppendLine("currentDCAInInterfaceTypePM.Code=" + currentDCAInInterfaceManagementPM.Code);
            //LogMessagingUtil.Instance.AppendLine("this.UnityId=" + this.MainInterfaceCode);
            //LogMessagingUtil.Instance.AppendLine("selectedFile=" + selectedFile + "/tenant=" + tenant.ToString());

            if (currentDCAInInterfaceManagementPM.Code != this.MainInterfaceCode)
            {
                LogMessagingUtil.Instance.AppendLine("Please note this is return message !!! DCA Callback !!!");
            }
            else
            {
                LogMessagingUtil.Instance.AppendLine("Please note this is absoloutly -DCA In !!!!");
            }
            _CorrelationId = myESBResponseParser.ResponseHeader.CorrelationId;
            TCustomsResponse customsResponse = null;
            LogMessagingUtil.Instance.AppendLine("<TCustomsResponse>.DeserilazeObject if fail the XSD not valid !!!(ask itzik to refresh XSD )");
            customsResponse = XmlGenericUtil<TCustomsResponse>.DeSerializeObject(myESBResponseParser.Body);
            LogMessagingUtil.Instance.AppendLine("<TCustomsResponse>.DeserilazeObject XSD is valid !!!");



            TRequestParams defaultRequestParamsFromCustomsResponse = null;
            defaultRequestParamsFromCustomsResponse = CreateDefaultRequestParamsFromCustomsResponse(customsResponse);
            if (currentDCAInInterfaceManagementPM.InOut == "I")//Can be  DCAIn =No callback!!!
            {
                defaultRequestParamsFromCustomsResponse.RequestVIA = Common.RequestParams.SendRequestVIA.DCABatch;
                defaultRequestParamsFromCustomsResponse.Tenant = tenant;
                defaultRequestParamsFromCustomsResponse.InterfaceTypeCode = currentDCAInInterfaceManagementPM.Code;
                defaultRequestParamsFromCustomsResponse.LoggingEnabled = true;
            }
            //defaultRequestParamsFromCustomsResponse.DCAFileName = selectedFile;
            //defaultRequestParamsFromCustomsResponse.LoggingUserId = //system@tenant1.co.il
            //defaultRequestParamsFromCustomsResponse.RequestName= messageDCA.ClassName 



            CustomsRequestsSheetService<TRequestParams>.Seed(externalId, tenant, defaultRequestParamsFromCustomsResponse, out _CustomsRequestsSheetService);
            //_CustomsRequestsSheetService = customsRequestsSheetService;
            if (_CustomsRequestsSheetService.StartCustomsRequestStepEnum == CustomsStepEnum.DCAInProgressUploaded)
            {
                _CustomsRequestsSheetService.StartStep(CustomsStepEnum.DCAInProgressUploaded, _CustomsStateMachineProcess.CurrentCommand);
                var dCAServerUploadStatus = new DCAServerUploadStatus();
                //dCAServerUploadStatus.TheDCAServerUploadResponse = dCAServerUploadResponse;
                dCAServerUploadStatus.DcaMessage = "Stage  DCAInProgressUploaded is close due the response arrived!!!";
                dCAServerUploadStatus.UnifreightQueueOutStatus = "SENT";
                LogMessagingUtil.Instance.AppendLine(dCAServerUploadStatus.DcaMessage);

                var memdCAServerUploadStatus = XmlGenericUtil<DCAServerUploadStatus>.MemoryStreamSerialize(dCAServerUploadStatus);
                LogMessagingUtil.Instance.AppendLine("Send Via DCA now we need to wait for reply from mehes dca !!");
                _CustomsRequestsSheetService.EndStep(memdCAServerUploadStatus);
            }

            _CustomsRequestsSheetService.StartStep(CustomsStepEnum.ReceivedCustomResponseCorrelation, CustomsCommandEnum.CustomsCommandDownloadDcaReceiveCorrelationWR);


            var memCustomsResponse = XmlGenericUtil<TCustomsResponse>.MemoryStreamSerializeWithDefaultNamespace(customsResponse);
            //SetBlob(requestParams, comm.Document, memCustomsResponse, ".TCustomsResponse");
            LogMessagingUtil.Instance.AppendLine(sb.ToString()); //DCAFileNAme: אנא בדוק שאתה שומר את שם הקובץ בכספת , בלוג
            _CustomsRequestsSheetService.SetCorrelationId(_CorrelationId);
            _CustomsRequestsSheetService.EndStep(memCustomsResponse);



            if (!pseudo)
            {
                LogMessagingUtil.Instance.AppendLine("Now we have to creat SBQUEUEU");
                if (!_CustomsRequestsSheetService.GetRequestParams<TRequestParams>().SuppressSplitWR)
                {
                    SBQMessageService.CreateBasic<CustomsCommandEnum>(
                         CustomsCommandEnum.CustomsCommandAnalyzeResponseWR,
                        tenant,
                        this.MainInterfaceCode,
                        _CustomsRequestsSheetService.CustomsRequestsSheet.Id);
                }
                else
                {
                    SBQMessageService.CreateBasic(
                                        SBQueueNames.CustomsMessagingSheetBQ, tenant,
                        ///not the receive _CustomsRequestsSheetService.CustomsRequestsSheet.InterfaceTypeCode ,
                                        this.MainInterfaceCode,
                                        _CustomsRequestsSheetService.CustomsRequestsSheet.Id);
                }

            }
            return _CustomsRequestsSheetService.CustomsRequestsSheet.Id;
        }
#endif


    }
}
