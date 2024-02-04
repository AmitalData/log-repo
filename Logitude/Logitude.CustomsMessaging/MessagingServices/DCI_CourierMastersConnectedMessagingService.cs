using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnifreightIIG.Common.ClientAddMessageServiceReference;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.MessageLib.Claim;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCI_CourierMastersConnectedMessagingService : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        SYSTBL_NG_9000_MSG_SystemTableRequest,
        DCI_CourierMastersConnectedResponseContentHeader,
        DCAInCustomReturnNullRequestService,
        DCI_CourierMastersConnectedResponseService, DCAInRequestHeader>
    {
        protected override DCI_CourierMastersConnectedResponseContentHeader CallWS(SYSTBL_NG_9000_MSG_SystemTableRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new System.NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "CourierMastersConnected"; }
        }

        public string CreateCRS(DCI_CourierMastersConnectedResponseContentHeader myDCAInUCBClosePendingWithResponseContentHeader, out string RequestInProgressListOut)
        {
            var RequestInProgressList = CheckHaveReqInQ("Customs.CourierMaster", myDCAInUCBClosePendingWithResponseContentHeader.tenant, myDCAInUCBClosePendingWithResponseContentHeader.courierMasterId);
            if (RequestInProgressList != null && RequestInProgressList.Count > 0)
            {
                RequestInProgressListOut = string.Join(",", RequestInProgressList.Select(request => request.Id.ToString())); ;

                return "קיים מסר זהה בתהליך";
            }
            

            LogMessagingUtil.Instance.AppendLine($"Build !!!Requestsheet  with Interface Type  = ${MainInterfaceCode} !!!");

            myDCAInUCBClosePendingWithResponseContentHeader.ResponseContentHeader = new DefaultResponseContentHeader() { TransmitionDateTime = DateTime.Now };

            string body = XmlGenericUtil<DCI_CourierMastersConnectedResponseContentHeader>.SerializeObject(myDCAInUCBClosePendingWithResponseContentHeader);
            RequestInProgressListOut = "";
            return SendToQ(body, myDCAInUCBClosePendingWithResponseContentHeader.tenant, out  RequestInProgressListOut);
        }


        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DCI_CourierMastersConnectedResponseContentHeader customsResponse)
        {
            var tableName = "Customs.CourierMaster";
            var genericRequestParams = new GenericRequestParams()
            {
                Tenant = customsResponse.tenant,
                AppicationId = customsResponse.courierMasterId,
                LoggingEnabled = true,
                InterfaceTypeCode = MainInterfaceCode,
                MainInterfaceCode = MainInterfaceCode,
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                LoggingEntityId = customsResponse.courierMasterId,
                LoggingEntityReference = customsResponse.MAWB,
                LoggingUserId = null,
                RequestName = $" קישור הצהרות ל " + customsResponse.MAWB + " ",
            };

            if (customsResponse.ServerSplitDeclarationsList == null || (customsResponse.ServerSplitDeclarationsList != null && customsResponse.ServerSplitDeclarationsList.Count == 0))
            {
                genericRequestParams.RequestName += " ראשי - מפצל";
                genericRequestParams.SplitterModeLetCreateMyType = false;
            }
            else
            {
                genericRequestParams.RequestName += " מפוצל";
                genericRequestParams.SplitterModeLetCreateMyType = true;
            }

            return genericRequestParams;
        }


        private List<CustomsRequestsSheetPM> CheckHaveReqInQ(string objectTableName, int tenant, string entityId)
        {
            var objectTableId = ObjectTableRepository.GetObjectTableByName(objectTableName);
            var customsRequestsSheetQS = new CustomsRequestsSheetQueryService(tenant);
            var RequestInProgressList = customsRequestsSheetQS.GetRequestInProgress(tenant, MainInterfaceCode, objectTableId, entityId, null, null, null, true);
            return RequestInProgressList;
        }

        private string SendToQ(string body, int tenant, out string RequestInProgressListOut)
        {
            string uniComm = null;
            string fileName = null;
            var transmitionDateTime = DateTime.Now;
            string xmlESBResponseXmlClass = null;
            body = body.Substring(body.IndexOf(Environment.NewLine));
            var myESBResponseXmlClass = new ESBResponseXmlClass();
            var extrenalId = "62833ff7-1cd3-4faa-85a6-a4312ae4797a";
            extrenalId = uniComm ?? Guid.NewGuid().ToString();
            xmlESBResponseXmlClass = myESBResponseXmlClass.Get(Guid.NewGuid().ToString(), extrenalId, body);
            var transTime = "2016-04-19_13-35-13-481";

            transTime = transmitionDateTime.ToString("s").Replace("T", "_").Replace(":", "-");
            transTime += "-";
            transTime += transmitionDateTime.Millisecond.ToString();

            fileName = "DcaPrefixName.IL941079089." + transTime + "." + extrenalId + ".PLT.xml";
            //var messService = new Logitude.CustomsMessaging.MessagingServices.DF_MSG10000_ImportDeclarationMessagingService();
            //var responseData = messService.SendSheet(genericRequestParams);

            var ourRef = "";
            using (var trans = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    var InterfaceManagementQS = new InterfaceManagementQueryService(tenant);
                    var InterfaceManagementPM = InterfaceManagementQS.GetSingleInterfaceManagementwithDefinition(
                        this.MainInterfaceCode, tenant);
                    fileName = fileName.Replace("DcaPrefixName.", InterfaceManagementPM.DcaPrefixName);
                    ourRef = this.DcaReceivedCustomResponseCorrelation(InterfaceManagementPM, tenant, new Customs.BL.Utils.DCAFileModel()
                    {
                        SelectedFileDownload = fileName,
                        TimStamp = transmitionDateTime

                    }, xmlESBResponseXmlClass);

                    trans.Complete();
                    RequestInProgressListOut = "";
                    return "המסר נבנה בהצלחה וישלח בתהליך רקע";
                }
                catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
                {
                    if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(" ClosePending SameRequestInProgress!! " + myCustomsRequestsSheetServiceException.Message);
                    }
                    else if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.NoAvailableSignServer)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("ClosePending SameRequestInProgress!!  " + myCustomsRequestsSheetServiceException.Message);
                    }
                    RequestInProgressListOut = myCustomsRequestsSheetServiceException.CustomsRequestsSheetId;
                    return "קיים מסר זהה בתהליך";
                }
            }
        }
    }

    [XmlRoot(Namespace = "http://amital.com/customs/Prod/DCI_CourierMastersConnectedResponseContentHeader", IsNullable = false)]
    [XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Prod/DCI_CourierMastersConnectedResponseContentHeader")]
    public class DCI_CourierMastersConnectedResponseContentHeader : IINF_MSG_Generic
    {
        public string courierMasterId { get; set; }
        public int tenant { get; set; }
        public string MAWB { get; set; }
        public bool connectedAll { get; set; }
        public bool disconnectedAll { get; set; }
        public string[] connectedItems { get; set; }
        public string[] disconnectedItems { get; set; }
        public List<string> ServerSplitDeclarationsList { get; set; }
        public bool Connect { get; set; }
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        public string LoggingUserId { get; set; }
        public DefaultResponseContentHeader ResponseContentHeader { get; set; }
        public IResponseContentHeader GetResponseContentHeader()
        {
            return ResponseContentHeader;
        }
    }
}
