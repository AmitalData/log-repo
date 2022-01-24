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
    public class DCAInUCUDO_UpdateOpenDeclarationsMessagingService : MessagingServiceBase<
        UpdateOpenDeclarationsRequestParams,
        INF_MSG_GenericResponseData,
        SYSTBL_NG_9000_MSG_SystemTableRequest,
        UpdateOpenDeclarationsResponseContentHeader,
        UpdateOpenDeclarationsRequestService,
        DCAInUCUDO_UpdateOpenDeclarationsResponseService, RequestHeader>
    {

        protected override UpdateOpenDeclarationsRequestParams CreateDefaultRequestParamsFromCustomsResponse(UpdateOpenDeclarationsResponseContentHeader customsResponse)
        {
            var myGenericRequestParams = new UpdateOpenDeclarationsRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster"),
                LoggingEntityId = customsResponse.EntityId
            };
            return myGenericRequestParams;
        }

        protected override UpdateOpenDeclarationsResponseContentHeader CallWS(SYSTBL_NG_9000_MSG_SystemTableRequest customRequest, UpdateOpenDeclarationsRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "UCUDO"; }
        }


        public string CreateCRS(int tenant, string LoggingUserId, UpdateOpenDeclarationsRequestParams _UpdateOpenDeclarationsRequestParams)
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
                LogMessagingUtil.Instance.AppendLine("Build !!!Requestsheet  with Interface Type  = UCUDO  !!!");

                string uniComm = null;
                string fileName = null;
                var transmitionDateTime = DateTime.Now;
                string xmlESBResponseXmlClass = null;

                var myDCAInUCBCMSSWithResponseContentHeader = new UpdateOpenDeclarationsResponseContentHeader()
                {
                    //CustomFileNo="",
                    tenant = _UpdateOpenDeclarationsRequestParams.Tenant,
                    LoggingUserId = _UpdateOpenDeclarationsRequestParams.LoggingUserId,
                    EntityId = _UpdateOpenDeclarationsRequestParams.LoggingEntityId,
                    ResponseContentHeader = new DefaultResponseContentHeader()
                    {
                        TransmitionDateTime = transmitionDateTime
                    },
                };

                var body = XmlGenericUtil<UpdateOpenDeclarationsResponseContentHeader>.SerializeObject(myDCAInUCBCMSSWithResponseContentHeader);
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
                   
                        var InterfaceManagementQS = new InterfaceManagementQueryService(tenant);
                var InterfaceManagementPM = InterfaceManagementQS.GetSingleInterfaceManagementwithDefinition(
                    this.MainInterfaceCode, tenant);
                fileName = fileName.Replace("DcaPrefixName.", InterfaceManagementPM.DcaPrefixName);
                ourRef = this.DcaReceivedCustomResponseCorrelation(InterfaceManagementPM, tenant, new Customs.BL.Utils.DCAFileModel()
                {
                    SelectedFileDownload = fileName,
                    TimStamp = transmitionDateTime, 
                    
                }, xmlESBResponseXmlClass , futureSendDateTime: DateTime.Now.AddMinutes(10));


                        trans.Complete();
                        return "SUCCESS";
                     
                  
                }

            }
            catch(System.Exception ex)
            {
                return ex.Message +" : " + ex.InnerException +" : " + ex.StackTrace;
            }
        }



    }


    public class UpdateOpenDeclarationsRequestParams : GenericRequestParams
    {

        public string LOGICOMMDEC { get; set; }
        public string MoreParams { get; set; }

    }

    [XmlRoot(Namespace = "http://amital.com/customs/Prod/DCAInUCUDOResponseContentHeader", IsNullable = false)]
    [XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Prod/DCAInUCUDOResponseContentHeader")]
    public class UpdateOpenDeclarationsResponseContentHeader : IINF_MSG_Generic
    {

        public IResponseContentHeader GetResponseContentHeader()
        {

            return ResponseContentHeader;
        }
        public Logitude.CustomsMessaging.Testers.Messages.DefaultResponseContentHeader ResponseContentHeader { get; set; }

        public int tenant { get; set; }
        public string LoggingUserId { get; set; }
         public string EntityId { get; set; }
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


    public class  UpdateOpenDeclarationsRequestService : RequestServiceBase<SYSTBL_NG_9000_MSG_SystemTableRequest, UpdateOpenDeclarationsRequestParams>
    {
        public override SYSTBL_NG_9000_MSG_SystemTableRequest GetRequest(UpdateOpenDeclarationsRequestParams requestParams)
        {

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam()
            {
                RequestDescription = requestParams.RequestName
            };
            return new SYSTBL_NG_9000_MSG_SystemTableRequest() { };
        }
    }
}
