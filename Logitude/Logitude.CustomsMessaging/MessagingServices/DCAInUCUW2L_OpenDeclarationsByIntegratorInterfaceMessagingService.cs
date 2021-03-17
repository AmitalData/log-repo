using Logitude.AmitalMessaging.Customs.CustomFile.CommDecFile;
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
using UnifreightIIG.Common.MessageLib.Ransom;
 using UnifreightIIG.Common.SystemTableServiceReference;


namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInUCUW2L_OpenDeclarationsByIntegratorInterfaceMessagingService : MessagingServiceBase<
        DCAInUCUW2LRequestParams,
        INF_MSG_GenericResponseData,
        SYSTBL_NG_9000_MSG_SystemTableRequest,
        DCAInUCUW2LResponseContentHeader,
        DCAInUCUW2LRequestService,
        DCAInUCUW2L_OpenDeclarationsByIntegratorInterfaceResponseService, RequestHeader>
    {

        protected override DCAInUCUW2LRequestParams CreateDefaultRequestParamsFromCustomsResponse(DCAInUCUW2LResponseContentHeader customsResponse)
        {
            var myGenericRequestParams = new DCAInUCUW2LRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                  LoggingEntityId = customsResponse.CustomFileNo
            };
            return myGenericRequestParams;

        }

        protected override DCAInUCUW2LResponseContentHeader CallWS(SYSTBL_NG_9000_MSG_SystemTableRequest customRequest, DCAInUCUW2LRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "UCUW2L"; }
        }


        public string CreateCRS(int tenant, string LoggingUserId, DCAInUCUW2LRequestParams _DCAInUCUW2LRequestParams)
        {

            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var customsRequestsSheetQS = new CustomsRequestsSheetQueryService(tenant);
            //var RequestInProgressList = customsRequestsSheetQS.GetRequestInProgress(tenant, this.MainInterfaceCode, objectTableId, mySendALLStorageSiteRequestParams.CourierMasterId, null, null, null, true);
            //if (RequestInProgressList != null && RequestInProgressList.Count > 0)
            //{
            //    return "קיים מסר זהה בתהליך";
            //}
            try
            {
                LogMessagingUtil.Instance.AppendLine("Build !!!Requestsheet  with Interface Type  = UCUW2L  !!!");

                string uniComm = null;
                string fileName = null;
                var transmitionDateTime = DateTime.Now;
                string xmlESBResponseXmlClass = null;

                var myDCAInUCBCMSSWithResponseContentHeader = new DCAInUCUW2LResponseContentHeader()
                {
                    //CustomFileNo="",
                    MoreParams = _DCAInUCUW2LRequestParams.MoreParams,
                    tenant = _DCAInUCUW2LRequestParams.Tenant,
                    LOGICOMMDEC = _DCAInUCUW2LRequestParams.LOGICOMMDEC,
                    LoggingUserId = _DCAInUCUW2LRequestParams.LoggingUserId,
                    ResponseContentHeader = new DefaultResponseContentHeader()
                    {
                        TransmitionDateTime = transmitionDateTime
                    },
                };

                var body = XmlGenericUtil<DCAInUCUW2LResponseContentHeader>.SerializeObject(myDCAInUCBCMSSWithResponseContentHeader);
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
                //using (var trans = TransactionFactory.GetNewTransaction())
                //{
                //    try
                //    {
                var InterfaceManagementQS = new InterfaceManagementQueryService(tenant);
                var InterfaceManagementPM = InterfaceManagementQS.GetSingleInterfaceManagementwithDefinition(
                    this.MainInterfaceCode, tenant);
                fileName = fileName.Replace("DcaPrefixName.", InterfaceManagementPM.DcaPrefixName);
                ourRef = this.DcaReceivedCustomResponseCorrelation(InterfaceManagementPM, tenant, new Customs.BL.Utils.DCAFileModel()
                {
                    SelectedFileDownload = fileName,
                    TimStamp = transmitionDateTime

                }, xmlESBResponseXmlClass);

                //  trans.Complete();
                return "SUCCESS";
                //  }
                //  catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
                //  {
                //if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                //{
                //    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(" UCBCMSS SameRequestInProgress!! " + myCustomsRequestsSheetServiceException.Message);
                //}
                //else if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.NoAvailableSignServer)
                //{
                //    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("UCBCTML SameRequestInProgress!!  " + myCustomsRequestsSheetServiceException.Message);
                //}
                //return "קיים מסר זהה בתהליך";
                // }
                // }

            }
            catch(System.Exception ex)
            {
                return ex.Message +" : " + ex.InnerException +" : " + ex.StackTrace;
            }
        }



    }


    public class DCAInUCUW2LRequestParams : GenericRequestParams
    {

        public string LOGICOMMDEC { get; set; }
        public string MoreParams { get; set; }

    }

    [XmlRoot(Namespace = "http://amital.com/customs/Prod/DCAInUCUW2LResponseContentHeader", IsNullable = false)]
    [XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Prod/DCAInUCUW2LResponseContentHeader")]
    public class DCAInUCUW2LResponseContentHeader : IINF_MSG_Generic
    {

        public IResponseContentHeader GetResponseContentHeader()
        {

            return ResponseContentHeader;
        }
        public Logitude.CustomsMessaging.Testers.Messages.DefaultResponseContentHeader ResponseContentHeader { get; set; }

        public int tenant { get; set; }
        public string LoggingUserId { get; set; }
         public string CustomFileNo { get; set; }
        public string LOGICOMMDEC { get; set; }
        public string MoreParams { get; set; }
        //public string master { get; set; }

        //public LOGIDOCS MyLOGIDOCS { get; set; }

        //public string MyMoreParams { get; set; }
        //public string DocumentsFilingCode { get; set; }
        //public string DocumentsFilingId { get; set; }
        //public string DOCUMENTTYPEID { get; set; }
        //public string DocumentTypeCode { get; set; }

        //public string DocumentTypeId { get; set; }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }


    public class DCAInUCUW2LRequestService : RequestServiceBase<SYSTBL_NG_9000_MSG_SystemTableRequest, DCAInUCUW2LRequestParams>
    {
        public override SYSTBL_NG_9000_MSG_SystemTableRequest GetRequest(DCAInUCUW2LRequestParams requestParams)
        {

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam()
            {
                RequestDescription = requestParams.RequestName
            };
            return new SYSTBL_NG_9000_MSG_SystemTableRequest() { };
        }
    }
}
