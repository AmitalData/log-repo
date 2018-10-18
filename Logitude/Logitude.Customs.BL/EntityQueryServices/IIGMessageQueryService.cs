using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
#if false
    

    public class IIGMessageQueryService
    {
        private static readonly List<IIGMessagePM> _DBSet;
        static IIGMessageQueryService()
        {

            _DBSet = new List<IIGMessagePM>();
            //_DBSet.Add(
                
                //new IIGMessagePM()
                //{
                //    Id = "9001",
                //    InOut = IIGMessagePM.InOutType.In,
                //     PrefixFileName ="GetSYSTBL_MSG9000_9001_SystemTableRequest_Out." ,
                //    Desc = "SystemTableRequest",//"זימון בדיקה במכס"    , 
                //    Interactive = IIGMessagePM.InteractiveMode.DCA,
                //    MalamClass = "SendCH_MSG_190_NoticeToClient_Out.",//SendCH_MSG_190_NoticeToClient_Out.IL941079089.IL038623617.2013-05-16_22-35-07.89b23e14-de0d-4cdf-a69d-ccea7ec4735d.TST.xml
                //    Environment = IIGMessagePM.EnvironmentType.Test | IIGMessagePM.EnvironmentType.Community | IIGMessagePM.EnvironmentType.Production,
                //    //XSDName = "UnifreightIIG.Common.MessageLib.PhysicalCheck.CH_NG_190_MSG1_NoticeToClient.xsd",
                //    //ImporterService = "UnifreightIIG.Client.ViewModels.Dca.MessagaeImport.PhysicalCheckImporterService",
                //    DCAServiceAddress =ResolveDCAServiceAddress() 
                //}
                //);
            //_DBSet.Add(
            //    new IIGMessagePM()
            //    {
            //        Id = "190", InOut= IIGMessagePM.InOutType.In ,
            //        Desc = "Physical Check Notice To Client",//"זימון בדיקה במכס"    , 
            //        Interactive = IIGMessagePM.InteractiveMode.DCA,
            //        PrefixFileName = "SendCH_MSG_190_NoticeToClient_Out.",//SendCH_MSG_190_NoticeToClient_Out.IL941079089.IL038623617.2013-05-16_22-35-07.89b23e14-de0d-4cdf-a69d-ccea7ec4735d.TST.xml
            //        MalamClass = "SendCH_MSG_190_NoticeToClient_Out.",//SendCH_MSG_190_NoticeToClient_Out.IL941079089.IL038623617.2013-05-16_22-35-07.89b23e14-de0d-4cdf-a69d-ccea7ec4735d.TST.xml
            //        Environment = IIGMessagePM.EnvironmentType.Test | IIGMessagePM.EnvironmentType.Community | IIGMessagePM.EnvironmentType.Production,
            //        //XSDName = "UnifreightIIG.Common.MessageLib.PhysicalCheck.CH_NG_190_MSG1_NoticeToClient.xsd",
            //        //ImporterService = "UnifreightIIG.Client.ViewModels.Dca.MessagaeImport.PhysicalCheckImporterService",
            //        DCAServiceAddress =ResolveDCAServiceAddress() 
            //    }
            //    );

            _DBSet.Add(
                new IIGMessagePM()
                {
                    Id = "196",InOut= IIGMessagePM.InOutType.In ,
                    Desc = "SendCH_MSG_196_CargoExitFromCheckSite_Out",//"זימון בדיקה במכס"    , 
                    Interactive = IIGMessagePM.InteractiveMode.DCA,
                    MalamClass = "SendCH_MSG_196_CargoExitFromCheckSite_Out.",//SendCH_MSG_190_NoticeToClient_Out.IL941079089.IL038623617.2013-05-16_22-35-07.89b23e14-de0d-4cdf-a69d-ccea7ec4735d.TST.xml
                    Environment = IIGMessagePM.EnvironmentType.Test | IIGMessagePM.EnvironmentType.Community | IIGMessagePM.EnvironmentType.Production,
                    //XSDName = "UnifreightIIG.Common.MessageLib.PhysicalCheck.CH_NG_190_MSG1_NoticeToClient.xsd",
                    //ImporterService = "UnifreightIIG.Client.ViewModels.Dca.MessagaeImport.PhysicalCheckImporterService",
                    DCAServiceAddress =ResolveDCAServiceAddress() 
                }
                );

            _DBSet.Add(
                new IIGMessagePM()
                {
                    Id = "1812",InOut= IIGMessagePM.InOutType.In ,
                    Desc = "SendGRNT_MSG1812_createGurateeRequestInfo_Out",//"זימון בדיקה במכס"    , 
                    Interactive = IIGMessagePM.InteractiveMode.DCA,
                    MalamClass = "SendGRNT_MSG1812_createGurateeRequestInfo_Out.",
                    Environment = IIGMessagePM.EnvironmentType.Test | IIGMessagePM.EnvironmentType.Community | IIGMessagePM.EnvironmentType.Production,
                    //XSDName = "UnifreightIIG.Common.MessageLib.PhysicalCheck.CH_NG_190_MSG1_NoticeToClient.xsd",
                    //ImporterService = "UnifreightIIG.Client.ViewModels.Dca.MessagaeImport.PhysicalCheckImporterService",
                    DCAServiceAddress =ResolveDCAServiceAddress() 
                }
                );


            _DBSet.Add(
                new IIGMessagePM()
                {
                    Id = "8218",
                    InOut = IIGMessagePM.InOutType.In,
                    Desc = "EV_NG_8218_MSG14100_ProceduralFaultMsg",//"זימון בדיקה במכס"    , 
                    Interactive = IIGMessagePM.InteractiveMode.DCA,
                    MalamClass = "SendEV__MSG8218_ProceduralFaultMsg_Out.",
                    Environment = IIGMessagePM.EnvironmentType.Test | IIGMessagePM.EnvironmentType.Community | IIGMessagePM.EnvironmentType.Production,
                    //XSDName = "UnifreightIIG.Common.MessageLib.PhysicalCheck.CH_NG_190_MSG1_NoticeToClient.xsd",
                    //ImporterService = "UnifreightIIG.Client.ViewModels.Dca.MessagaeImport.PhysicalCheckImporterService",
                    DCAServiceAddress = ResolveDCAServiceAddress()
                }
                );

            _DBSet.Add(
                new IIGMessagePM()
                {
                    Id = "70",
                    InOut = IIGMessagePM.InOutType.In,
                    Desc = "UnifreightIIG.Common.MessageLib.Storage.ST_NG_40_MSG7_SpecialActivityRequestMessage",//"זימון בדיקה במכס"    , 
                    Interactive = IIGMessagePM.InteractiveMode.DCA,
                    MalamClass = "SendST_MSG70_StorageResponseFromWarehouse_Out.",
                    Environment = IIGMessagePM.EnvironmentType.Test | IIGMessagePM.EnvironmentType.Community | IIGMessagePM.EnvironmentType.Production,
                    //XSDName = "UnifreightIIG.Common.MessageLib.PhysicalCheck.CH_NG_190_MSG1_NoticeToClient.xsd",
                    //ImporterService = "UnifreightIIG.Client.ViewModels.Dca.MessagaeImport.PhysicalCheckImporterService",
                    DCAServiceAddress = ResolveDCAServiceAddress()
                }
                );
            

            _DBSet.Add(
                new IIGMessagePM()
                {
                    Id = "8215",
                    InOut = IIGMessagePM.InOutType.In,
                    Desc = "SendEV_MSG8215_ConstraintApprovalDecision_Out",//"זימון בדיקה במכס"    , 
                    Interactive = IIGMessagePM.InteractiveMode.DCA,
                    MalamClass = "SendEV_MSG8215_ConstraintApprovalDecision_Out.",//SendCH_MSG_190_NoticeToClient_Out.IL941079089.IL038623617.2013-05-16_22-35-07.89b23e14-de0d-4cdf-a69d-ccea7ec4735d.TST.xml
                    Environment = IIGMessagePM.EnvironmentType.Test | IIGMessagePM.EnvironmentType.Community | IIGMessagePM.EnvironmentType.Production,
                    //XSDName = "UnifreightIIG.Common.MessageLib.PhysicalCheck.CH_NG_190_MSG1_NoticeToClient.xsd",
                    //ImporterService = "UnifreightIIG.Client.ViewModels.Dca.MessagaeImport.PhysicalCheckImporterService",
                    DCAServiceAddress = ResolveDCAServiceAddress()
                }
                );



            _DBSet.Add(
                //SendTSH_MSG3050_PaymentOrderReply_Out.IL941079089.2013-07-21_12-29-25.36824184-ca8e-4ff3-907f-57216e4e4308.TST.xml
                new IIGMessagePM()
                {
                    Id = "350",
                    InOut = IIGMessagePM.InOutType.In,
                    Desc = "SendTSH_MSG3050_PaymentOrderReply_Out",//"זימון בדיקה במכס"    , 
                    Interactive = IIGMessagePM.InteractiveMode.DCA,
                    MalamClass = "SendTSH_MSG3050_PaymentOrderReply_Out.",//SendCH_MSG_190_NoticeToClient_Out.IL941079089.IL038623617.2013-05-16_22-35-07.89b23e14-de0d-4cdf-a69d-ccea7ec4735d.TST.xml
                    Environment = IIGMessagePM.EnvironmentType.Test ,///| IIGAsyncMessagePM.EnvironmentType.Community | IIGAsyncMessagePM.EnvironmentType.Production,
                    XSDName = "UnifreightIIG.Common.MessageLib.PhysicalCheck.CH_NG_190_MSG1_NoticeToClient.xsd",
                    //ClassName= "UnifreightIIG.Common.AgentPaymentRequestServiceReference.TSH_MSG2_PaymentOrderReply",
                    //ImporterService = "UnifreightIIG.Client.ViewModels.Dca.MessagaeImport.TSH_MSG2_PaymentOrderReplyImporterService",
                    DCAServiceAddress = ResolveDCAServiceAddress()
                }
                );
            _DBSet.Add(
                
               new IIGMessagePM()
               { //SendDE_MSG280_DebtNotificationMessage_Out.IL941079089.2013-08-07_16-34-38.6ea27a51-1ee0-4382-b9ae-7510e77a3d62.TST
                   Id = "280",
                   InOut = IIGMessagePM.InOutType.In,
                   Desc = "SendDE_MSG280_DebtNotificationMessage_Out",
                   Interactive = IIGMessagePM.InteractiveMode.DCA,
                   MalamClass = "SendDE_MSG280_DebtNotificationMessage_Out.",
                   Environment = IIGMessagePM.EnvironmentType.Test,///| IIGAsyncMessagePM.EnvironmentType.Community | IIGAsyncMessagePM.EnvironmentType.Production,
                   XSDName = "UnifreightIIG.Common.MessageLib.Deficit.DE_NG_280_MSG11_DebtNotificationMessage.xsd",
                   //ClassName = "UnifreightIIG.Common.MessageLib.Deficit.DE_NG_280_MSG11_DebtNotificationMessage",
                   //ImporterService = "Logitude.CustomsMessaging.MessageAnalyzer.DE_NG_280_MSG11_DebtNotificationMessageAnalyzerService",
                   DCAServiceAddress = ResolveDCAServiceAddress()
               }
               );

            
            _DBSet.Add(
               new IIGMessagePM()
               { ///SendVE_MSG3681_VendorErrorOrCriticalChangeMessage_Out.IL941079089.2013-08-14_09-26-12.5081a87d-5d22-48c1-be0f-8c8fca1f3bed.TST.xml
                   Id = "3681",
                   InOut = IIGMessagePM.InOutType.In,
                   Desc = "///SendVE_MSG3681_VendorErrorOrCriticalChangeMessage_Out",
                   Interactive = IIGMessagePM.InteractiveMode.DCA,
                   MalamClass = "SendVE_MSG3681_VendorErrorOrCriticalChangeMessage_Out.",
                   Environment = IIGMessagePM.EnvironmentType.Test,///| IIGAsyncMessagePM.EnvironmentType.Community | IIGAsyncMessagePM.EnvironmentType.Production,
                   XSDName = "UnifreightIIG.Common.MessageLib.Vendor.VE_MSG012_VendorErrorOrCriticalChangeMessage.xsd",
                   DCAServiceAddress = ResolveDCAServiceAddress()
               }
               );


            _DBSet.Add(
               new IIGMessagePM()
               { ///SendVE_MSG3681_VendorErrorOrCriticalChangeMessage_Out.IL941079089.2013-08-14_09-26-12.5081a87d-5d22-48c1-be0f-8c8fca1f3bed.TST.xml
                   Id = "2470",
                   InOut = IIGMessagePM.InOutType.In,
                   Desc = "SendDF_MSG2470_ReleaseGoodsMessage",
                   Interactive = IIGMessagePM.InteractiveMode.DCA,
                   MalamClass = "SendDF_MSG2470_ReleaseGoodsMessage_Out.",
                   Environment = IIGMessagePM.EnvironmentType.Test,///| IIGAsyncMessagePM.EnvironmentType.Community | IIGAsyncMessagePM.EnvironmentType.Production,
                   XSDName = "UnifreightIIG.Common.MessageLib.DeclarationDeal.DF_NG_2470_DF_MSG16001_ReleaseGoodsMessage.xsd",
                   DCAServiceAddress = ResolveDCAServiceAddress()
               }
               );


            _DBSet.Add(
               new IIGMessagePM()
               { ///SendVE_MSG3681_VendorErrorOrCriticalChangeMessage_Out.IL941079089.2013-08-14_09-26-12.5081a87d-5d22-48c1-be0f-8c8fca1f3bed.TST.xml
                   Id = "2754",
                   InOut = IIGMessagePM.InOutType.Out,
                   
                   Desc = "DF_MSG10000_ImportDeclaration",
                   Interactive = IIGMessagePM.InteractiveMode.Interactive,
                   MalamClass = "DF_MSG10000_ImportDeclaration",
                   Environment = IIGMessagePM.EnvironmentType.Test,///| IIGAsyncMessagePM.EnvironmentType.Community | IIGAsyncMessagePM.EnvironmentType.Production,
                   XSDName = "",
                   DCAServiceAddress = ResolveDCAServiceAddress()
               }

               );


            _DBSet.Add(
               new IIGMessagePM()
               { ///SendVE_MSG3681_VendorErrorOrCriticalChangeMessage_Out.IL941079089.2013-08-14_09-26-12.5081a87d-5d22-48c1-be0f-8c8fca1f3bed.TST.xml
                   Id = "2755",
                   InOut = IIGMessagePM.InOutType.Out,
                   
                   Desc = "DF_NG_2755_MSG12001_SubmitDeclaration",
                   Interactive = IIGMessagePM.InteractiveMode.Interactive,
                   MalamClass = "DF_NG_2755_MSG12001_SubmitDeclaration",
                   Environment = IIGMessagePM.EnvironmentType.Test,///| IIGAsyncMessagePM.EnvironmentType.Community | IIGAsyncMessagePM.EnvironmentType.Production,
                   XSDName = "",
                   DCAServiceAddress = ResolveDCAServiceAddress()
               }
               );


            _DBSet.Add(
              new IIGMessagePM()
              { ///GetDOC_MSG2715_2716_AddAttachmentResponse_In.IL512320953.2014-05-18_16-51-14.A3ACB642-2A38-4B10-B656-77F5F46EE088.PRD.xml
                  Id = "2715",
                  InOut = IIGMessagePM.InOutType.Out,
                  
                  Desc = "D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntity",
                  Interactive = IIGMessagePM.InteractiveMode.Interactive ,
                  MalamClass = "D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntity",
                  PrefixFileName = "GetDOC_MSG2715_2716_AddAttachmentResponse_In",
                  Environment = IIGMessagePM.EnvironmentType.Test,///| IIGAsyncMessagePM.EnvironmentType.Community | IIGAsyncMessagePM.EnvironmentType.Production,
                  XSDName = "",
                  DCAServiceAddress = ResolveDCAServiceAddress()
              }




              );


            _DBSet.Add(
              new IIGMessagePM()
              { ///GetDOC_MSG2715_2716_AddAttachmentResponse_In.IL512320953.2014-05-18_16-51-14.A3ACB642-2A38-4B10-B656-77F5F46EE088.PRD.xml
                  Id = "1000",
                  InOut = IIGMessagePM.InOutType.Out,

                  Desc = "DF_MSG10000_ImportDeclaration",
                  Interactive = IIGMessagePM.InteractiveMode.Interactive,
                  MalamClass = "D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntity",
                  PrefixFileName = "GetDOC_MSG2715_2716_AddAttachmentResponse_In",
                  Environment = IIGMessagePM.EnvironmentType.Test,///| IIGAsyncMessagePM.EnvironmentType.Community | IIGAsyncMessagePM.EnvironmentType.Production,
                  XSDName = "",
                  DCAServiceAddress = ResolveDCAServiceAddress()
              }




              );
            
        }

        public List<IIGMessagePM> GetAll()
        {
            foreach (var item in _DBSet)
            {
                item.Tenant = 1;
            }
            return _DBSet;
        }
        private static string ResolveDCAServiceAddress()
        {
            return @"http://itzik7:5050/Unifreight/DCAService/Basic";
            
            return @"http://localhost:5050/Unifreight/DCAService/Basic";
        }
        private static string ResolveSignServiceAddress()
        {
            return @"http://itzik7:5050/Unifreight/DCAService/Basic";

            return @"http://localhost:5050/Unifreight/DCAService/Basic";
        }
    }
#endif
}
