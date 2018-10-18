using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsWorkerRole.DCA
{
    public class IIGMessageSet : List<IIGMessagePM>
    {
        public IIGMessageSet()
        {
            this.Add(
                new IIGMessagePM()
                {
                    Id = "190",
                    Desc = "Physical Check Notice To Client",//"זימון בדיקה במכס"    , 
                    Interactive = IIGMessagePM.InteractiveMode.DCA,
                    PrefixFileName = "SendCH_MSG_190_NoticeToClient_Out.",//SendCH_MSG_190_NoticeToClient_Out.IL941079089.IL038623617.2013-05-16_22-35-07.89b23e14-de0d-4cdf-a69d-ccea7ec4735d.TST.xml
                    Environment = IIGMessagePM.EnvironmentType.Test | IIGMessagePM.EnvironmentType.Community | IIGMessagePM.EnvironmentType.Production,
                    //XSDName = "UnifreightIIG.Common.MessageLib.PhysicalCheck.CH_NG_190_MSG1_NoticeToClient.xsd",
                    //ImporterService = "UnifreightIIG.Client.ViewModels.Dca.MessagaeImport.PhysicalCheckImporterService",
                    DCAServiceAddress =ResolveDCAServiceAddress() 
                }
                );
            this.Add(
                //SendTSH_MSG3050_PaymentOrderReply_Out.IL941079089.2013-07-21_12-29-25.36824184-ca8e-4ff3-907f-57216e4e4308.TST.xml
                new IIGMessagePM()
                {
                    Id = "350",
                    Desc = "SendTSH_MSG3050_PaymentOrderReply_Out",//"זימון בדיקה במכס"    , 
                    Interactive = IIGMessagePM.InteractiveMode.DCA,
                    PrefixFileName = "SendTSH_MSG3050_PaymentOrderReply_Out.",//SendCH_MSG_190_NoticeToClient_Out.IL941079089.IL038623617.2013-05-16_22-35-07.89b23e14-de0d-4cdf-a69d-ccea7ec4735d.TST.xml
                    Environment = IIGMessagePM.EnvironmentType.Test ,///| IIGMessagePM.EnvironmentType.Community | IIGMessagePM.EnvironmentType.Production,
                    XSDName = "UnifreightIIG.Common.MessageLib.PhysicalCheck.CH_NG_190_MSG1_NoticeToClient.xsd",
                    //ClassName= "UnifreightIIG.Common.AgentPaymentRequestServiceReference.TSH_MSG2_PaymentOrderReply",
                    //ImporterService = "UnifreightIIG.Client.ViewModels.Dca.MessagaeImport.TSH_MSG2_PaymentOrderReplyImporterService",
                    DCAServiceAddress = ResolveDCAServiceAddress()
                }
                );
            this.Add(
                
               new IIGMessagePM()
               { //SendDE_MSG280_DebtNotificationMessage_Out.IL941079089.2013-08-07_16-34-38.6ea27a51-1ee0-4382-b9ae-7510e77a3d62.TST
                   Id = "280",
                   Desc = "SendDE_MSG280_DebtNotificationMessage_Out",
                   Interactive = IIGMessagePM.InteractiveMode.DCA,
                   PrefixFileName = "SendDE_MSG280_DebtNotificationMessage_Out.",
                   Environment = IIGMessagePM.EnvironmentType.Test,///| IIGMessagePM.EnvironmentType.Community | IIGMessagePM.EnvironmentType.Production,
                   XSDName = "UnifreightIIG.Common.MessageLib.Deficit.DE_NG_280_MSG11_DebtNotificationMessage.xsd",
                   //ClassName = "UnifreightIIG.Common.MessageLib.Deficit.DE_NG_280_MSG11_DebtNotificationMessage",
                   //ImporterService = "Logitude.CustomsMessaging.MessageAnalyzer.DE_NG_280_MSG11_DebtNotificationMessageAnalyzerService",
                   DCAServiceAddress = ResolveDCAServiceAddress()
               }
               );

            
            this.Add(

               new IIGMessagePM()
               { ///SendVE_MSG3681_VendorErrorOrCriticalChangeMessage_Out.IL941079089.2013-08-14_09-26-12.5081a87d-5d22-48c1-be0f-8c8fca1f3bed.TST.xml
                   Id = "3681",
                   Desc = "///SendVE_MSG3681_VendorErrorOrCriticalChangeMessage_Out",
                   Interactive = IIGMessagePM.InteractiveMode.DCA,
                   PrefixFileName = "SendVE_MSG3681_VendorErrorOrCriticalChangeMessage_Out.",
                   Environment = IIGMessagePM.EnvironmentType.Test,///| IIGMessagePM.EnvironmentType.Community | IIGMessagePM.EnvironmentType.Production,
                   XSDName = "UnifreightIIG.Common.MessageLib.Vendor.VE_MSG012_VendorErrorOrCriticalChangeMessage.xsd",
                   DCAServiceAddress = ResolveDCAServiceAddress()
               }
               );
        }

        private string ResolveDCAServiceAddress()
        {
            return @"http://itzik7:5050/Unifreight/DCAService/Basic";
            
            return @"http://localhost:5050/Unifreight/DCAService/Basic";
        }
    }
}
