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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
 public   class DCAInUCB9999ReAnAnalysis_MsgMessagingService : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        SYSTBL_NG_9000_MSG_SystemTableRequest,
        DCAInUCB9999WithResponseContentHeader,
        DCAInCustomReturnNullRequestService,
        UniDecReAnalysisBatchSend9999_MsgResponseService, RequestHeader>
         
    {
        public override string MainInterfaceCode
        {
            get { return "UCB9999"; }
        }

 

        public string CreateCRS(int tenant, string LoggingUserId, List<string> RequestsList = null)
        {

           // var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CustomsCollateralsAnswer");
            var customsRequestsSheetQS = new CustomsRequestsSheetQueryService(tenant);
         //   var RequestInProgressList = customsRequestsSheetQS.GetRequestInProgress(tenant, this.MainInterfaceCode, objectTableId, null, null, null, null, true);
            //if (RequestInProgressList != null && RequestInProgressList.Count > 0)
            //{

            //    ///throw new System.Exception("Requestsheet  with Interface Type  = UCB2755  already in progress  !!!");
            //    return "קיים מסר זהה בתהליך";

            //}
            LogMessagingUtil.Instance.AppendLine("Build !!!Requestsheet  with Interface Type  = UCB9999  !!!");


            string uniComm = null;
            string fileName = null;
            var transmitionDateTime = DateTime.Now;
            string xmlESBResponseXmlClass = null;

            var myDCAInUCB9999WithResponseContentHeader = new DCAInUCB9999WithResponseContentHeader()
            {
                LoggingUserId = LoggingUserId,
                tenant = tenant,
                RequestsList = RequestsList,
                 ResponseContentHeader = new DefaultResponseContentHeader()
                {
                    TransmitionDateTime = transmitionDateTime
                },
            };

            var body = XmlGenericUtil<DCAInUCB9999WithResponseContentHeader>.SerializeObject(myDCAInUCB9999WithResponseContentHeader);

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
                    
             return "המסר נבנה בהצלחה וישלח בתהליך רקע";
                }
                catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
                {
                    if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(" UCB9999 SameRequestInProgress!! " + myCustomsRequestsSheetServiceException.Message);


                    }
                    else if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.NoAvailableSignServer)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("UCB9999 SameRequestInProgress!!  " + myCustomsRequestsSheetServiceException.Message);
                    }
                  
                    return "קיים מסר זהה בתהליך";
                 }
            }



         }
        protected override DCAInUCB9999WithResponseContentHeader CallWS(SYSTBL_NG_9000_MSG_SystemTableRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }
    }

    [XmlRoot(Namespace = "http://amital.com/customs/Prod/DCAInUCB2755WithResponseContentHeader", IsNullable = false)]
    [XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Prod/DCAInUCB2755WithResponseContentHeader")]
    public class DCAInUCB9999WithResponseContentHeader : IINF_MSG_Generic
    {
        public IResponseContentHeader GetResponseContentHeader()
        {

            return ResponseContentHeader;
        }
        public Logitude.CustomsMessaging.Testers.Messages.DefaultResponseContentHeader ResponseContentHeader { get; set; }

        public int tenant { get; set; }
        public string LoggingUserId { get; set; }

        public List<string> RequestsList { get; set; }
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;


    }
}
