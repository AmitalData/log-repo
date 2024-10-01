using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.MessageLib.PhysicalCheck;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInUCB2755E_MsgMessagingService
        : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        SYSTBL_NG_9000_MSG_SystemTableRequest,
        DCAInUCB2755EWithResponseContentHeader,
        DCAInCustomReturnNullRequestService,
        UniExportBatchSend2755E_MsgResponseService, RequestHeader>
    {

        public override string MainInterfaceCode
        {
            get { return "UCB2755E"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DCAInUCB2755EWithResponseContentHeader customsResponse)
        {
            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var genericRequestParams = new GenericRequestParams()
            {
                Tenant = customsResponse.tenant,
                AppicationId = customsResponse.BatchId,
                LoggingEnabled = true,
                InterfaceTypeCode = this.MainInterfaceCode,
                MainInterfaceCode = this.MainInterfaceCode,

                LoggingObjectTableId = objectTableId,
                LoggingEntityId = customsResponse.BatchId,
                LoggingEntityReference = "Decl", // customsResponse.master,

                LoggingUserId = customsResponse.LoggingUserId,
                RequestName = $" שידור הצהרות יצוא " // RequestName = $" שידור הצהרות יצוא " + customsResponse.master + " "
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

        protected override DCAInUCB2755EWithResponseContentHeader CallWS(SYSTBL_NG_9000_MSG_SystemTableRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public string CreateCRS(int tenant, SendDeclarationBatchRequestParams sendDeclarationBatchRequestParams,
            out string RequestInProgressListOut)
        {
            string batchId = Guid.NewGuid().ToString();

            // todo: how to check if request in progress ?
            /*
            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var customsRequestsSheetQS = new CustomsRequestsSheetQueryService(tenant);
            var RequestInProgressList = customsRequestsSheetQS.GetRequestInProgress(tenant, this.MainInterfaceCode, objectTableId, batchId, null, null, null, true);
            if (RequestInProgressList != null && RequestInProgressList.Count > 0)
            {
                RequestInProgressListOut = string.Join(",", RequestInProgressList.Select(request => request.Id.ToString())); ;
                return "קיים מסר זהה בתהליך";
            }
            */
            LogMessagingUtil.Instance.AppendLine("Build !!!Requestsheet  with Interface Type  = UCB2755E  !!!");

            string uniComm = null;
            string fileName = null;
            var transmitionDateTime = DateTime.Now;
            string xmlESBResponseXmlClass = null;

            var myDCAInUCB2755EWithResponseContentHeader = new DCAInUCB2755EWithResponseContentHeader()
            {
                BatchId = batchId,
                LoggingUserId = sendDeclarationBatchRequestParams.LoggingUserId,
                tenant = tenant,
                MyMoreParams = "",
                ResponseContentHeader = new DefaultResponseContentHeader()
                {
                    TransmitionDateTime = transmitionDateTime
                },
            };
            if (sendDeclarationBatchRequestParams.IsAllSelected)
            {
                myDCAInUCB2755EWithResponseContentHeader.ExcludedIds = sendDeclarationBatchRequestParams.SelectedIds;
                myDCAInUCB2755EWithResponseContentHeader.QueryOperations = sendDeclarationBatchRequestParams.QueryOperations;
            }
            else
            {
                myDCAInUCB2755EWithResponseContentHeader.ClientFilterDeclarationsList = sendDeclarationBatchRequestParams.SelectedIds;
            }

            var body = XmlGenericUtil<DCAInUCB2755EWithResponseContentHeader>.SerializeObject(myDCAInUCB2755EWithResponseContentHeader);
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
                /*
                 * todo?
                catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
                when (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.CourierForceSignException)
                {
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(" CourierForceSignException!! " + myCustomsRequestsSheetServiceException.Message);
                    RequestInProgressListOut = myCustomsRequestsSheetServiceException.CustomsRequestsSheetId;
                    return myCustomsRequestsSheetServiceException.InnerException.Message;
                }*/
                catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
                {
                    if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("UCB2755E SameRequestInProgress!! " + myCustomsRequestsSheetServiceException.Message);
                    }
                    else if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.NoAvailableSignServer)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("UCB2755E SameRequestInProgress!!  " + myCustomsRequestsSheetServiceException.Message);
                    }
                    RequestInProgressListOut = myCustomsRequestsSheetServiceException.CustomsRequestsSheetId;
                    return "קיים מסר זהה בתהליך";
                    //throw;
                }
            }
        }
    }



    [XmlRoot(Namespace = "http://amital.com/customs/Prod/DCAInUCB2755EWithResponseContentHeader", IsNullable = false)]
    [XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Prod/DCAInUCB2755EWithResponseContentHeader")]
    public class DCAInUCB2755EWithResponseContentHeader : IINF_MSG_Generic
    {

        public IResponseContentHeader GetResponseContentHeader()
        {

            return ResponseContentHeader;
        }
        public Logitude.CustomsMessaging.Testers.Messages.DefaultResponseContentHeader ResponseContentHeader { get; set; }

        public int tenant { get; set; }
        public string LoggingUserId { get; set; }
        public string BatchId { get; set; }
        public List<string> ClientFilterDeclarationsList { get; set; }
        public List<string> ServerSplitDeclarationsList { get; set; }
        public List<string> ExcludedIds { get; set; }
        public QueryOperations QueryOperations { get; set; }
        public string MyMoreParams { get; set; }
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
