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
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInUCBUCADPE_MsgMessagingService : MessagingServiceBase<
       GenericRequestParams,
       INF_MSG_GenericResponseData,
       SYSTBL_NG_9000_MSG_SystemTableRequest,
       DCAInUCBUCADPEResponseContentHeader,
       DCAInCustomReturnNullRequestService,
       UniCourierBatchSendUCADPE_MsgResponseService, RequestHeader>
    {

        public override string MainInterfaceCode
        {
            get { return "UCADPE"; }
        }

        protected override DCAInUCBUCADPEResponseContentHeader CallWS(SYSTBL_NG_9000_MSG_SystemTableRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }


        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DCAInUCBUCADPEResponseContentHeader customsResponse)
        {
            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
            var genericRequestParams = new GenericRequestParams()
            {
                Tenant = customsResponse.tenant,
                AppicationId = customsResponse.requestParamsData.courierMasterId,
                LoggingEnabled = true,
                InterfaceTypeCode = this.MainInterfaceCode,
                MainInterfaceCode = this.MainInterfaceCode,
                LoggingObjectTableId = objectTableId,
                LoggingEntityId = customsResponse.requestParamsData.courierMasterId,
                //LoggingUserId = customsResponse.LoggingUserId,

            };

            genericRequestParams.RequestName = "הזנה גורפת PENDING";

            return genericRequestParams;
        }

        public string CreateCRS(int tenant, AddMultiPendingsRequestParams requestParamsData, QueryOperations queryOperations)
        {
            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
            var customsRequestsSheetQS = new CustomsRequestsSheetQueryService(tenant);
            var RequestInProgressList = customsRequestsSheetQS.GetRequestInProgress(tenant, this.MainInterfaceCode, objectTableId, requestParamsData.courierMasterId, null, null, null, true);
            if (RequestInProgressList != null && RequestInProgressList.Count > 0)
            {
                return "קיים מסר זהה בתהליך";
            }
            LogMessagingUtil.Instance.AppendLine("Build !!!Requestsheet  with Interface Type  = UCADPE  !!!");

            string uniComm = null;
            string fileName = null;
            var transmitionDateTime = DateTime.Now;
            string xmlESBResponseXmlClass = null;
            var myDCAInUCBUCADPEResponseContentHeader = new DCAInUCBUCADPEResponseContentHeader()
            {
                tenant = tenant,
                requestParamsData = requestParamsData,
                queryOperations = queryOperations,
                ResponseContentHeader = new DefaultResponseContentHeader()
                {
                    TransmitionDateTime = transmitionDateTime
                },
            };

            var body = XmlGenericUtil<DCAInUCBUCADPEResponseContentHeader>.SerializeObject(myDCAInUCBUCADPEResponseContentHeader);
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
                    var InterfaceManagementPM = InterfaceManagementQS.GetSingleInterfaceManagementwithDefinition(this.MainInterfaceCode, tenant);
                    fileName = fileName.Replace("DcaPrefixName.", InterfaceManagementPM.DcaPrefixName);
                    ourRef = this.DcaReceivedCustomResponseCorrelation(InterfaceManagementPM, tenant, new Customs.BL.Utils.DCAFileModel()
                    {
                        SelectedFileDownload = fileName,
                        TimStamp = transmitionDateTime
                    }, xmlESBResponseXmlClass);

                    trans.Complete();
                    return "תהליך יתעדכן ברקע";
                }
                catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
                {
                    if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(" DCAUAC SameRequestInProgress!! " + myCustomsRequestsSheetServiceException.Message);
                    }
                    else if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.NoAvailableSignServer)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("DCAUAC SameRequestInProgress!!  " + myCustomsRequestsSheetServiceException.Message);
                    }
                    return "קיים מסר זהה בתהליך";
                }
            }
        }
    }


    [XmlRoot(Namespace = "http://amital.com/customs/Prod/DCAInUCBUCADPEResponseContentHeader", IsNullable = false)]
    [XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Prod/DCAInUCBUCADPEResponseContentHeader")]
    public class DCAInUCBUCADPEResponseContentHeader : IINF_MSG_Generic
    {
        public IResponseContentHeader GetResponseContentHeader()
        {
            return ResponseContentHeader;
        }
        public Logitude.CustomsMessaging.Testers.Messages.DefaultResponseContentHeader ResponseContentHeader { get; set; }

        public int tenant { get; set; }
        public AddMultiPendingsRequestParams requestParamsData { get; set; }
        public QueryOperations queryOperations { get; set; }
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
