using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Utils;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Logitude.CustomsMessaging.MessageAnalyzer
{
    public class AnalyzeQueueMessage
    {
#if false
        static readonly IUnityContainer _UnityContainer;
        
        static AnalyzeQueueMessage()
        {
            _UnityContainer = new UnityContainer();

            _UnityContainer.RegisterType<IMessageAnalyzerService, CH_NG_190_MSG1_NoticeToClientAnalyzerService>("CH_NG_190_MSG1_NoticeToClient");
            _UnityContainer.RegisterType<IMessageAnalyzerService, TSH_MSG2_PaymentOrderReplyAnalyzerService>("TSH_MSG2_PaymentOrderReply");
            _UnityContainer.RegisterType<IMessageAnalyzerService, DE_NG_280_MSG11_DebtNotificationMessageAnalyzerService>("DE_NG_280_MSG11_DebtNotificationMessage");
            _UnityContainer.RegisterType<IMessageAnalyzerService, DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageAnalyzerService>("DF_NG_2470_DF_MSG16001_ReleaseGoodsMessage");
            _UnityContainer.RegisterType<IMessageAnalyzerService, CH_NG_196_MSG7_CargoExitFromCheckSiteAnalyzerService>("CH_NG_196_MSG7_CargoExitFromCheckSite");
            _UnityContainer.RegisterType<IMessageAnalyzerService, DF_NG_2754_MSG10004_ImportDeclarationAnalyzerService>("DF_NG_2754_MSG10004_ImportDeclarationResponse");
            _UnityContainer.RegisterType<IMessageAnalyzerService, DF_NG_2754_MSG10004_ImportDeclarationAnalyzerService>("DF_NG_2754_MSG10004_ImportDeclarationResponse");
            _UnityContainer.RegisterType<IMessageAnalyzerService, VE_MSG012_VendorErrorOrCriticalChangeMessageAnalyzeService>("VE_MSG012_VendorErrorOrCriticalChangeMessage");///SendVE_MSG3681_VendorErrorOrCriticalChangeMessage_Out.IL941079089.2013-08-14_09-26-12.5081a87d-5d22-48c1-be0f-8c8fca1f3bed.TST.xml                                                                                                                                                                              ///
            _UnityContainer.RegisterType<IMessageAnalyzerService, DF_MSG10040_CollateralRequestMsgAnalyzerService>("COLT_NG_8211_MSG10040_CollateralRequestMsg"); // DF_MSG10040_CollateralRequestMsg
            _UnityContainer.RegisterType<IMessageAnalyzerService, EV_NG_8215_MSG23002_ConstraintApprovalDecisionAnalyzerService>("EV_NG_8215_MSG23002_ConstraintApprovalDecision"); // DF_MSG10042_CollateralAnswerApprovalMsg
            _UnityContainer.RegisterType<IMessageAnalyzerService, Deficit_NG_5009_MSG14_FirstAndSeconderyRequirementsMessageAnalyzerService>("Deficit_NG_5009_MSG14_FirstAndSeconderyRequirementsMessage");
            _UnityContainer.RegisterType<IMessageAnalyzerService, GRNT_MSG15_createGurateeRequestInfoAnalyzerService>("GRNT_MSG15_createGurateeRequestInfo");

            _UnityContainer.RegisterType<IMessageAnalyzerService, ST_NG_70_MSG3_StorageResponseFromWarehouseAnalyzerService>("ST_NG_70_MSG3_StorageResponseFromWarehouse");            

            
        
        }
        public IMessageAnalyzerService MessageAnalyzerService { get; private set; }
        public void AnalyzeQueueCustomMessage(string customMessageXml)
        {
            var myXDocument = XDocument.Parse(customMessageXml);
            var rootName = myXDocument.Root.Name.LocalName;
            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("AnalyzeQueueCustomMessage rootName:" + rootName);
            MessageAnalyzerService = _UnityContainer.Resolve<IMessageAnalyzerService>(rootName);
            MessageAnalyzerService.Analyze(customMessageXml);
        }
#endif
        
        public INF_MSG_GenericResponseData ResponseData { get; private set; }
       
        public void AnalyzeQueueCustomMessage(string paramsHashXml ,string customMessageXml)
        {

            
            
            var h = UnifreightListsUtil.Deserialize(paramsHashXml);
            //h["tenant"] = 1;
            //h["selectedFile"] = "SendDF_MSG2470_ReleaseGoodsMessage_Out.IL941079089.2014-07-27_11-05-05-767.110d58a0-a5b2-4c74-9f67-1d6c3ae8f6b9.TST.xml";
            var stenant=UnifreightListsUtil.GetValue(ref h, "tenant");
            var selectedFile = UnifreightListsUtil.GetValue(ref h, "selectedFile");
            if (String.IsNullOrWhiteSpace(stenant))
            {
                throw new Exception("stenant ==null"); 
            }
            if (String.IsNullOrWhiteSpace(selectedFile))
            {
                throw new Exception("selectedFile ==null");
            }
            int tenant = 1;
            if (!int.TryParse(stenant, out tenant))
            {
                throw new Exception("!int.TryParse(stenant, out tenant)"); 
            }
            var selectedDcaFile = DCAFilePraser.GetDCAFileModel(selectedFile);
            var DcaPrefixName = selectedFile.Substring(0, selectedFile.IndexOf('.')+1);


            var qs = new InterfaceManagementQueryService(tenant);
            var messageDCA = qs.GetCodeByDcaPrefixName(DcaPrefixName);
            if (string.IsNullOrWhiteSpace(messageDCA.Code))
            {
                throw new Exception("Get InterfaceManagementCode ByDcaPrefixName return null ??!!! DcaPrefixName=" + DcaPrefixName); 
            }
            var interfaceCode = messageDCA.Code;
            var outmessagePm =qs.GetOutInterfaceType(interfaceCode);
            if (outmessagePm != null && !String.IsNullOrWhiteSpace(outmessagePm.Code))
            {
                interfaceCode = outmessagePm.Code;
            }
            
            
            //byte[] messageBytes = System.Text.UTF8Encoding.UTF8.GetBytes(customMessageXml);

            var anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(interfaceCode);

            var customsRequestsSheetId = anaO.DcaReceivedCustomResponseCorrelation(messageDCA, tenant, selectedDcaFile, customMessageXml, true);
            ResponseData = anaO.SendSheet(tenant, customsRequestsSheetId) as INF_MSG_GenericResponseData;
        }
        


    }
}
