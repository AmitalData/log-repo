
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
using UnifreightIIG.Common.MessageLib.PhysicalCheck;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInUCBCTML_MsgMessagingService
        : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        SYSTBL_NG_9000_MSG_SystemTableRequest,
        DCAInUCBCTMLWithResponseContentHeader,
        DCAInCustomReturnNullRequestService,
        UniCourierBatchSendUCBCTML_MsgResponseService, RequestHeader>

    {
         
        public override string MainInterfaceCode
        {
            get { return "UCBCTML"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DCAInUCBCTMLWithResponseContentHeader customsResponse)
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
                //RequestVIA = SendRequestVIA.WebServiceBatch,
                LoggingEnabled = true,
                InterfaceTypeCode = this.MainInterfaceCode,
                MainInterfaceCode = this.MainInterfaceCode,


                LoggingObjectTableId = objectTableId,
                LoggingEntityId = customsResponse.CourierMasterId,
                LoggingEntityReference = customsResponse.master,

                LoggingUserId = customsResponse.LoggingUserId,
                RequestName = $" שידור הגשה/מסוף בלדר  " + customsResponse.master + " "
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

        protected override DCAInUCBCTMLWithResponseContentHeader CallWS(SYSTBL_NG_9000_MSG_SystemTableRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }






        public string CreateCRS(int tenant, string LoggingUserId,
            //string CourierMasterId, string master,string courierDeclarationStatusCode, List<string> DeclarationsList = null)
            SendALLCorrectRequestParams mySendALLCorrectRequestParams)
        {

            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
            if (mySendALLCorrectRequestParams.IsWorkSheetFromExcel)
            {
                objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CourierHawbFromExcel");
            }
            var customsRequestsSheetQS = new CustomsRequestsSheetQueryService(tenant);
            var RequestInProgressList = customsRequestsSheetQS.GetRequestInProgress(tenant, this.MainInterfaceCode, objectTableId, mySendALLCorrectRequestParams.CourierMasterId, null, null, null, true,null, mySendALLCorrectRequestParams.IsWorkSheetFromExcel,mySendALLCorrectRequestParams.LoggingUserId);
            if (RequestInProgressList != null && RequestInProgressList.Count > 0)
            {

                ///throw new System.Exception("Requestsheet  with Interface Type  = UCBCTML  already in progress  !!!");
                return "קיים מסר זהה בתהליך";

            }
            LogMessagingUtil.Instance.AppendLine("Build !!!Requestsheet  with Interface Type  = UCBCTML  !!!");



            string uniComm = null;
            string fileName = null;
            var transmitionDateTime = DateTime.Now;
            string xmlESBResponseXmlClass = null;

            var myDCAInUCBCTMLWithResponseContentHeader = new DCAInUCBCTMLWithResponseContentHeader()
            {
                CourierMasterId = mySendALLCorrectRequestParams.CourierMasterId,
                LoggingUserId = LoggingUserId,
                master = mySendALLCorrectRequestParams.HAWB,
                IsWorkSheetFromExcel= mySendALLCorrectRequestParams.IsWorkSheetFromExcel,
                tenant = tenant,
                MyMoreParams = "",
                ResponseContentHeader = new DefaultResponseContentHeader()
                {
                    TransmitionDateTime = transmitionDateTime
                },
            };



            var body = XmlGenericUtil<DCAInUCBCTMLWithResponseContentHeader>.SerializeObject(myDCAInUCBCTMLWithResponseContentHeader);
            //
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
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(" UCBCTML SameRequestInProgress!! " + myCustomsRequestsSheetServiceException.Message);


                    }
                    else if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.NoAvailableSignServer)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("UCBCTML SameRequestInProgress!!  " + myCustomsRequestsSheetServiceException.Message);
                    }
                    return "קיים מסר זהה בתהליך";
                    //throw;
                }
            }



        }


    }



    [XmlRoot(Namespace = "http://amital.com/customs/Prod/DCAInUCBCTMLWithResponseContentHeader", IsNullable = false)]
    [XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Prod/DCAInUCBCTMLWithResponseContentHeader")]
    public class DCAInUCBCTMLWithResponseContentHeader : IINF_MSG_Generic
    {

        public IResponseContentHeader GetResponseContentHeader()
        {

            return ResponseContentHeader;
        }
        public Logitude.CustomsMessaging.Testers.Messages.DefaultResponseContentHeader ResponseContentHeader { get; set; }

        public int tenant { get; set; }
        public string LoggingUserId { get; set; }
        public string CourierMasterId { get; set; }
        public string master { get; set; }
        public bool IsWorkSheetFromExcel { get; set; }


        public string MyMoreParams { get; set; }
        public List<string> ServerSplitDeclarationsList { get;  set; }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
