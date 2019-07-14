using Microsoft.Practices.Unity;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.BL.Messaging.Customs;
using UnifreightIIG.Common.BankAccountToRefundUpdateReplayServiceReference;

namespace Logitude.CustomsMessaging.Utils
{
    public class CRSUtil
    {

        public string CreateCRS_DCAIn<TCustomResponse>(TCustomResponse customResponse, RequestParamsBase requestParams)
            where TCustomResponse : class

        {

            LogMessagingUtil.Instance.AppendLine($"Build !!!Requestsheet  with Interface Type  = {requestParams.MainInterfaceCode}  !!!");

            string uniComm = null;
            string fileName = null;
            var transmitionDateTime = DateTime.Now;
            string xmlESBResponseXmlClass = null;



            var body = XmlGenericUtil<TCustomResponse>.SerializeObject(customResponse);
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

            try
            {
                var InterfaceManagementQS = new InterfaceManagementQueryService(requestParams.Tenant);
                var InterfaceManagementPM = InterfaceManagementQS.GetSingleInterfaceManagementwithDefinition(
                    requestParams.MainInterfaceCode, requestParams.Tenant);
                fileName = fileName.Replace("DcaPrefixName.", InterfaceManagementPM.DcaPrefixName);

                var anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(requestParams.MainInterfaceCode);





                ourRef = anaO.DcaReceivedCustomResponseCorrelation(InterfaceManagementPM, requestParams.Tenant, new Customs.BL.Utils.DCAFileModel()
                {
                    SelectedFileDownload = fileName,
                    TimStamp = transmitionDateTime

                }, xmlESBResponseXmlClass);


                return "המסר נבנה בהצלחה וישלח בתהליך רקע";
            }
            catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
            {
                if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                {
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(" UCB2750 SameRequestInProgress!! " + myCustomsRequestsSheetServiceException.Message);
                }
                throw myCustomsRequestsSheetServiceException;
                return "קיים מסר זהה בתהליך";
            }

        }

    
    }
}
