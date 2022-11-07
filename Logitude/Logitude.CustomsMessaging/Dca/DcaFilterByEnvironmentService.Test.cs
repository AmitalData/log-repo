using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.OutgoingMessageRequestServiceReference;

namespace Logitude.CustomsMessaging.Dca
{
    public partial class DcaFilterByEnvironmentService
    {
        public static void UnitTest()
        {
            int Tenant = 6;

            string myData = @"
SendDF_MSG2470_ReleaseGoodsMessage_EX_Out.IL941079089.IL513094649.2022-11-06_10-51-21-920.7d6671a7-2c22-43a5-9ee9-5362ea3343fe.Pre.xml
SendLP_NG_8400_MSG01_LogisticPermitMessage_EX_Out.IL941079089.IL513094649.2022-11-06_10-51-22-567.d6e66d0e-8c11-41fd-b981-e526a8e43e07.Pre.xml
SendDF_MSG8251__DeclarationStatus_Response_EX_Out.IL941079089.IL511068256.2022-11-06_10-51-39-287.7a96afe1-c3da-475f-bd06-483c757b6fcd.Pre.xml
SendMN_MSG2791_ExportDeliveryAnswerMessage_Out.IL941079089.IL513251751.2022-11-06_10-53-43-758.146263.Pre.xml
SendMN_MSG2791_ExportDeliveryAnswerMessage_Out.IL941079089.IL513251751.2022-11-06_10-53-44-945.146264.Pre.xml
SendMN_MSG2791_ExportDeliveryAnswerMessage_Out.IL941079089.IL513251751.2022-11-06_10-53-47-258.146233.Pre.xml";
            var res=myData.Split( new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var OutgoingMessage=res.Select(r => new NG_9101_MSG_OutgoingMessageResponseOutgoingMessage() { Filename = r }).ToList();
            var dcaUtil = new DcaFilterByEnvironmentService();
            var resFilterByEnvironmentOutGoing = dcaUtil.FilterByEnvironmentOutGoing(Tenant, OutgoingMessage);
            var outgoingMessageFilterByEnvironment = resFilterByEnvironmentOutGoing.OutgoingMessage;

            var DCAFileModelList = res.Select(r => new Customs.BL.Utils.DCAFileModel() { SelectedFileDownload = r }).ToList();
            
            var resFilterByEnvironmentListOfDCAFile = dcaUtil.FilterByEnvironmentListOfDCAFile(Tenant, DCAFileModelList);
            //sbFilenameQueue.Enqueue(resFilterByEnvironmentOutGoing.SbLocal.ToString());

        }
    }
}
