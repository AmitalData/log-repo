
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
    public class DCAInUCB1170_MsgMessagingService
        : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        SYSTBL_NG_9000_MSG_SystemTableRequest,
        DCAInUCB1170WithResponseContentHeader,
        DCAInCustomReturnNullRequestService,
        UniCourierBatchSend1170_MsgResponseService, RequestHeader>

    {

        public override string MainInterfaceCode
        {
            get { return "UCB1170"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DCAInUCB1170WithResponseContentHeader customsResponse)
        {
            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
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
                RequestName = $" שידור מצהר בלדר " + customsResponse.master + " "
            };
            return genericRequestParams;
        }

        protected override DCAInUCB1170WithResponseContentHeader CallWS(SYSTBL_NG_9000_MSG_SystemTableRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }




        public string CreateCRS_Old(int tenant, string LoggingUserId, string CourierMasterId, string master)
        {

            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
            var customsRequestsSheetQS = new CustomsRequestsSheetQueryService(tenant);
            var RequestInProgressList = customsRequestsSheetQS.GetRequestInProgress(tenant, this.MainInterfaceCode, objectTableId, CourierMasterId, null, null, null, true);
            if (RequestInProgressList != null && RequestInProgressList.Count > 0)
            {

                ///throw new System.Exception("Requestsheet  with Interface Type  = UCB1170  already in progress  !!!");
                return "קיים מסר זהה בתהליך";

            }
            LogMessagingUtil.Instance.AppendLine("Build !!!Requestsheet  with Interface Type  = UCB1170  !!!");





            var genericRequestParams = new GenericRequestParams()
            {
                Tenant = tenant,
                AppicationId = CourierMasterId,
                RequestVIA = SendRequestVIA.WebServiceBatch,
                LoggingEnabled = true,
                InterfaceTypeCode = "UCB1170",
                MainInterfaceCode = "UCB1170",


                LoggingObjectTableId = objectTableId,
                LoggingEntityId = CourierMasterId,
                LoggingEntityReference = master,

                LoggingUserId = LoggingUserId,
                RequestName = $" שידור מצהר בלדר" + master + " "
            };

            //var messService = new Logitude.CustomsMessaging.MessagingServices.DF_MSG10000_ImportDeclarationMessagingService();
            //var responseData = messService.SendSheet(genericRequestParams);


            using (var trans = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    bool test = false;
                    if (test)
                    {
                        genericRequestParams.RequestVIA = SendRequestVIA.WebServiceInteractive;
                        var res = this.Send(genericRequestParams);
                    }
                    else
                    {
                        SBQMessageService.CreateSheetSBQMessage<Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams>(genericRequestParams
                            , false//, DateTime.Now.AddMinutes(5)
                            );
                    }

                    trans.Complete();
                    return "המסר נבנה בהצלחה וישלח בתהליך רקע";
                }
                catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
                {
                    if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(" UCB1170 SameRequestInProgress!! " + myCustomsRequestsSheetServiceException.Message);


                    }
                    else if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.NoAvailableSignServer)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("UCB1170 SameRequestInProgress!!  " + myCustomsRequestsSheetServiceException.Message);
                    }
                    return "קיים מסר זהה בתהליך";
                    //throw;
                }
            }



        }



        public string CreateCRS(int tenant, string LoggingUserId,
            //string CourierMasterId, string master, string courierDeclarationStatusCode, List<string> DeclarationsList = null)
            SendALLCorrectRequestParams requestParamsData)
        {

            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
            var customsRequestsSheetQS = new CustomsRequestsSheetQueryService(tenant);
            var RequestInProgressList = customsRequestsSheetQS.GetRequestInProgress(tenant, this.MainInterfaceCode, objectTableId, requestParamsData.CourierMasterId, null, null, null, true);
            if (RequestInProgressList != null && RequestInProgressList.Count > 0)
            {

                ///throw new System.Exception("Requestsheet  with Interface Type  = UCB1170  already in progress  !!!");
                return "קיים מסר זהה בתהליך";

            }
            LogMessagingUtil.Instance.AppendLine("Build !!!Requestsheet  with Interface Type  = UCB1170  !!!");






            string uniComm = null;
            string fileName = null;
            var transmitionDateTime = DateTime.Now;
            string xmlESBResponseXmlClass = null;

            var myDCAInUCB1170WithResponseContentHeader = new DCAInUCB1170WithResponseContentHeader()
            {
                CourierMasterId = requestParamsData.CourierMasterId,
                LoggingUserId = LoggingUserId,
                master = requestParamsData.HAWB,
                CourierDeclarationStatusCode = requestParamsData.CourierDeclarationStatusCode,
                SelectedAvailableValue = requestParamsData.SelectedAvailableValue,
                SelectedBOLValue = requestParamsData.SelectedBOLValue,
                SelectedStatusValue = requestParamsData.SelectedStatusValue,
                SelectedTotalInvoiceValue = requestParamsData.SelectedTotalInvoiceValue,
                SelectedFastIndividualProcessValue = requestParamsData.SelectedFastIndividualProcessValue,
                SelectedCustomStatusValue = requestParamsData.SelectedCustomStatusValue,
                DeclarationsList = requestParamsData.Declarations,

                tenant = tenant,
                MyMoreParams = "",
                ResponseContentHeader = new DefaultResponseContentHeader()
                {
                    TransmitionDateTime = transmitionDateTime
                },
            };



            var body = XmlGenericUtil<DCAInUCB1170WithResponseContentHeader>.SerializeObject(myDCAInUCB1170WithResponseContentHeader);
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
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(" UCB1170 SameRequestInProgress!! " + myCustomsRequestsSheetServiceException.Message);


                    }
                    else if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.NoAvailableSignServer)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("UCB1170 SameRequestInProgress!!  " + myCustomsRequestsSheetServiceException.Message);
                    }
                    return "קיים מסר זהה בתהליך";
                    //throw;
                }
            }



        }

    }




    [XmlRoot(Namespace = "http://amital.com/customs/Prod/DCAInUCB1170WithResponseContentHeader", IsNullable = false)]
    [XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Prod/DCAInUCB1170WithResponseContentHeader")]
    public class DCAInUCB1170WithResponseContentHeader : IINF_MSG_Generic
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
        public string CourierDeclarationStatusCode { get; set; }
        public List<string> DeclarationsList { get; set; }
        public string MyMoreParams { get; set; }


        public string SelectedBOLValue { get; set; }
        public string SelectedStatusValue { get; set; }
        public string SelectedAvailableValue { get; set; }
        public string SelectedTotalInvoiceValue { get; set; }
        public string SelectedFastIndividualProcessValue { get; set; }
        public string SelectedCustomStatusValue { get; set; }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
