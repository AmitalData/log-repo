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
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInUCAApproveAllPending_MsgMessagingService : MessagingServiceBase<
       GenericRequestParams,
       INF_MSG_GenericResponseData,
       SYSTBL_NG_9000_MSG_SystemTableRequest,
       DCAInUCBApproveAllPendingWithResponseContentHeader,
       DCAInCustomReturnNullRequestService,
       UniCourierBatchApproveAllPending_MsgResponseService, RequestHeader>

    {

        public override string MainInterfaceCode
        {
            get { return "DCAInUCBApproveAllPending"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DCAInUCBApproveAllPendingWithResponseContentHeader customsResponse)
        {
            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
            if (customsResponse.IsWorkSheetFromExcel)
            {
                objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CourierHawbFromExcel");
            }

            var genericRequestParams = new GenericRequestParams()
            {
                Tenant = customsResponse.tenant,
                AppicationId = customsResponse.CourierMasterId,
                LoggingEnabled = true,
                InterfaceTypeCode = this.MainInterfaceCode,
                MainInterfaceCode = this.MainInterfaceCode,
                LoggingObjectTableId = objectTableId,
                LoggingEntityId = customsResponse.CourierMasterId,
                LoggingEntityReference = customsResponse.MAWB,

                LoggingUserId = customsResponse.LoggingUserId,
                
            };
            genericRequestParams.RequestName = $" {customsResponse.MAWB} שידור אישור PENDING לבלדר ";

           
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

        public string CreateCRS(int tenant, string LoggingUserId, PendingRequestParams myPendingRequestParams)
        {

            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
            if (myPendingRequestParams.IsWorkSheetFromExcel)
            {
                objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CourierHawbFromExcel");
            }
            var customsRequestsSheetQS = new CustomsRequestsSheetQueryService(tenant);
            var RequestInProgressList = customsRequestsSheetQS.GetRequestInProgress(tenant, this.MainInterfaceCode, objectTableId, myPendingRequestParams.CourierMasterId, null, null, null, true, null,myPendingRequestParams.IsWorkSheetFromExcel, myPendingRequestParams.LoggingUserId);
            if (RequestInProgressList != null && RequestInProgressList.Count > 0)
            {
                return "קיים מסר זהה בתהליך";
            }
            LogMessagingUtil.Instance.AppendLine("Build !!!Requestsheet  with Interface Type  = DCAInUCBApproveAllPending  !!!");

            string uniComm = null;
            string fileName = null;
            var transmitionDateTime = DateTime.Now;
            string xmlESBResponseXmlClass = null;
            var myDCAInUCBClosePendingWithResponseContentHeader = new DCAInUCBApproveAllPendingWithResponseContentHeader()
            {
                CourierMasterId = myPendingRequestParams.CourierMasterId,
                LoggingUserId = LoggingUserId,
                MAWB = myPendingRequestParams.MAWB,
                PendingCode= myPendingRequestParams.PendingCode?.Length > 0?myPendingRequestParams.PendingCode[0]:null,
                tenant = tenant,
                MyMoreParams = "",
                IsWorkSheetFromExcel = myPendingRequestParams.IsWorkSheetFromExcel,
                declarationList = myPendingRequestParams.DeclarationsList,
                ResponseContentHeader = new DefaultResponseContentHeader()
                {
                    TransmitionDateTime = transmitionDateTime
                },
            };


            var body = XmlGenericUtil<DCAInUCBApproveAllPendingWithResponseContentHeader>.SerializeObject(myDCAInUCBClosePendingWithResponseContentHeader);
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
                    return "המסר נבנה בהצלחה וישלח בתהליך רקע";
                }
                catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
                {
                    if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("DCAInUCBApproveAllPending SameRequestInProgress!! " + myCustomsRequestsSheetServiceException.Message);
                    }
                    else if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.NoAvailableSignServer)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("DCAInUCBApproveAllPending SameRequestInProgress!!  " + myCustomsRequestsSheetServiceException.Message);
                    }
                    return "קיים מסר זהה בתהליך";
                }
            }
        }

        protected override DCAInUCBApproveAllPendingWithResponseContentHeader CallWS(SYSTBL_NG_9000_MSG_SystemTableRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }
    }
    [XmlRoot(Namespace = "http://amital.com/customs/Prod/DCAInUCBClosePendingWithResponseContentHeader", IsNullable = false)]
    [XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Prod/DCAInUCBClosePendingWithResponseContentHeader")]
    public class DCAInUCBApproveAllPendingWithResponseContentHeader : IINF_MSG_Generic
    {
        public IResponseContentHeader GetResponseContentHeader()
        {
            return ResponseContentHeader;
        }
        public Logitude.CustomsMessaging.Testers.Messages.DefaultResponseContentHeader ResponseContentHeader { get; set; }
        public int tenant { get; set; }
        public string LoggingUserId { get; set; }
        public string CourierMasterId { get; set; }
        public bool IsWorkSheetFromExcel { get; set; }
        public string PendingCode { get; set; }
        public string MAWB { get; set; }
        public string[] declarationList { get; set; }
        public string MyMoreParams { get; set; }
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        public List<string> ServerSplitDeclarationsList { get; set; }
    }
}
