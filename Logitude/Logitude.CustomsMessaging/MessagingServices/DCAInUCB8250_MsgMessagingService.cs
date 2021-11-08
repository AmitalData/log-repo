using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
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
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInUCB8250_MsgMessagingService : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        SYSTBL_NG_9000_MSG_SystemTableRequest,
        DCAInUCB8250WithResponseContentHeader,
        DCAInCustomReturnNullRequestService,
        UniCourierBatchSend8250_MsgResponseService, RequestHeader>

    {

        public override string MainInterfaceCode
        {
            get { return "UCB8250"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DCAInUCB8250WithResponseContentHeader customsResponse)
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
                LoggingUserId = customsResponse.LoggingUserId,
                RequestName = $" שידור סטטוס הצהרות לבלדר " + customsResponse.CourierMasterId + " "
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

        protected override DCAInUCB8250WithResponseContentHeader CallWS(SYSTBL_NG_9000_MSG_SystemTableRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }


        public string CreateCRS(int tenant, string LoggingUserId, string CourierMasterId, string testerSendOption)
        {
            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
            var objectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");

            var customsRequestsSheetQS = new CustomsRequestsSheetQueryService(tenant);
            var RequestInProgressList = customsRequestsSheetQS.GetRequestInProgress(tenant, this.MainInterfaceCode, objectTableId, CourierMasterId, null, null, null, true);
            if (RequestInProgressList != null && RequestInProgressList.Count > 0)
            {
                ///throw new System.Exception("Requestsheet  with Interface Type  = UCB8250  already in progress  !!!");
                return "קיים מסר זהה בתהליך";
            }
            LogMessagingUtil.Instance.AppendLine("Build !!!Requestsheet  with Interface Type  = UCB8250  !!!");
            var repo = new DeclarationCourierStatusRepository(tenant);

            //List<DeclarationCourierStatus> listPoco = repo.GetByMasterIDDeclarationCourierStatus(tenant, CourierMasterId);
            //if(listPoco!= null)
            //{
            //List<string> Ids = listPoco.Select(x => x.DeclarationId).ToList();

            //foreach (var item in listPoco)
            //{
            //var RequestInProgressList2 = customsRequestsSheetQS.GetRequestInProgressByIds(tenant, "8250", objectTableId2, Ids, false);
            List<CustomsRequestsSheetPM> RequestInProgressList2 ;
            FeatureQuery featureQuery = new FeatureQuery();

            var features = featureQuery.GetAllowedFeaturesForLoggedUser(LoggingUserId, tenant);

            var feature = features.Features.FirstOrDefault(x => x.Code == "StatusDeclarationOldVersion");
            if (feature != null)
            {
                  RequestInProgressList2 = customsRequestsSheetQS.GetRequestInProgress(tenant, "8250", null, null, objectTableId, CourierMasterId, null, false);
            }
            else
            {
                  RequestInProgressList2 = customsRequestsSheetQS.GetRequestInProgress(tenant, "8250", objectTableId, CourierMasterId, null, null, null, false);

            }


            if (RequestInProgressList2 != null && RequestInProgressList2.Count > 0)
                {
                    ///throw new System.Exception("Requestsheet  with Interface Type  = UCB8250  already in progress  !!!");
                    return "קיים מסר זהה בתהליך";
                }
                // }

            //}


            string uniComm = null;
            string fileName = null;
            var transmitionDateTime = DateTime.Now;
            string xmlESBResponseXmlClass = null;

            var myDCAInUCB8250WithResponseContentHeader = new DCAInUCB8250WithResponseContentHeader()
            {
                CourierMasterId = CourierMasterId,
                TesterSendOption= testerSendOption,
                LoggingUserId = LoggingUserId,
                tenant = tenant,
                MyMoreParams = "",
                ResponseContentHeader = new DefaultResponseContentHeader()
                {
                    TransmitionDateTime = transmitionDateTime
                },
            };


            var body = XmlGenericUtil<DCAInUCB8250WithResponseContentHeader>.SerializeObject(myDCAInUCB8250WithResponseContentHeader);
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
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(" UCB8250 SameRequestInProgress!! " + myCustomsRequestsSheetServiceException.Message);
                    }
                    else if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.NoAvailableSignServer)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("UCB8250 SameRequestInProgress!!  " + myCustomsRequestsSheetServiceException.Message);
                    }
                    return "קיים מסר זהה בתהליך";
                }
            }

        }
    }



    [XmlRoot(Namespace = "http://amital.com/customs/Prod/DCAInUCB8250WithResponseContentHeader", IsNullable = false)]
    [XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Prod/DCAInUCB8250WithResponseContentHeader")]
    public class DCAInUCB8250WithResponseContentHeader : IINF_MSG_Generic
    {

        public IResponseContentHeader GetResponseContentHeader()
        {
            return ResponseContentHeader;
        }

        public Logitude.CustomsMessaging.Testers.Messages.DefaultResponseContentHeader ResponseContentHeader { get; set; }

        public int tenant { get; set; }
        public string LoggingUserId { get; set; }
        public string CourierMasterId { get; set; }
        public string MyMoreParams { get; set; }
        public List<string> ServerSplitDeclarationsList { get; set; }
        public string TesterSendOption { get;  set; }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}

