using Logitude.Customs.Def.ClosedTable;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using System.Diagnostics;
using System.Linq;
using Logitude.SystemLogs;
using System;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class MessagingServiceFactoryHelper
    {
        static MessagingServiceFactoryHelper()
        {

            //Add OnStart 
            //Like C:\LogitudeWorld\Amital\CustomsWorkerRole\CustomsMessagingOutWR.cs

            var myDF_MSG10000_ImportDeclarationMessagingService = new DF_MSG10000_ImportDeclarationMessagingService();
            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType, DF_MSG10000_ImportDeclarationMessagingService>
                ((new DF_MSG10000_ImportDeclarationMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DF_NG_2755_MSG12001_SubmitExportDeclarationMessagingService>
                ((new DF_NG_2755_MSG12001_SubmitExportDeclarationMessagingService()).MainInterfaceCode);


            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DF_NG_2755_MSG12001_SubmitDeclarationMessagingService>
                ((new DF_NG_2755_MSG12001_SubmitDeclarationMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
        DF_NG_2751_MSG10000_ExportDeclarationMessagingService>
        ((new DF_NG_2751_MSG10000_ExportDeclarationMessagingService()).MainInterfaceCode);


            //2715
            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityMessagingService>
                ((new D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
            DCAInCH_NG_190_MSG1_NoticeToClientMessagingService>
            ((new DCAInCH_NG_190_MSG1_NoticeToClientMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                CH_NG_191_MSG2_ChangingTimeRequestMessagingService>
                ((new CH_NG_191_MSG2_ChangingTimeRequestMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                SYSTBL_NG_9000_MSG_SystemTableRequestMessageService>
                ((new SYSTBL_NG_9000_MSG_SystemTableRequestMessageService()).MainInterfaceCode);


            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                TSH_NG_3053_MSG8_AgentPaymentRequestMessageService>
                ((new TSH_NG_3053_MSG8_AgentPaymentRequestMessageService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                TSH_NG_3053_MSG8_AgentPaymentRequestMessageService>
                ("3050");// because alalyze DCA MEssaage its not 3053 its 3050 !!!

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                CL_MSG101_GetCustomerByEntityCustomerIdentificationMassagingService>
                ((new CL_MSG101_GetCustomerByEntityCustomerIdentificationMassagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                VE_MSG010_VendorInsertUpdateDeleteMessagingService>
                ((new VE_MSG010_VendorInsertUpdateDeleteMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                VE_MSG051_VendorSearchByCustomsAgentMessagingService>
                ((new VE_MSG051_VendorSearchByCustomsAgentMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                VE_MSG013_VendorAddCommunicationDeviceMessageService>
                ((new VE_MSG013_VendorAddCommunicationDeviceMessageService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DF_NG_8250_Web01_DeclarationStatus_RequestMessagingService>
                ((new DF_NG_8250_Web01_DeclarationStatus_RequestMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInDE_NG_5107_MSG10_AcceptanceOrRejectionMessagingService>
                ((new DCAInDE_NG_5107_MSG10_AcceptanceOrRejectionMessagingService()).MainInterfaceCode);


            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInCH_NG_196_MSG7_CargoExitFromCheckSiteMassageService>
                ((new DCAInCH_NG_196_MSG7_CargoExitFromCheckSiteMassageService()).MainInterfaceCode);


            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInVAL_NG_8227_MSG_520_RequiredDocumentMassagingService>
                ((new DCAInVAL_NG_8227_MSG_520_RequiredDocumentMassagingService()).MainInterfaceCode);


            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInDF_NG_2470_DF_MSG16001_ReleaseGoodsMessagingService>
                ((new DCAInDF_NG_2470_DF_MSG16001_ReleaseGoodsMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInEV_NG_8219_MSG14100_ProceduralFaultCancelMassagingService>
                ((new DCAInEV_NG_8219_MSG14100_ProceduralFaultCancelMassagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInEV_NG_8218_ProceduralFaultMessagingService>
                ((new DCAInEV_NG_8218_ProceduralFaultMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInDF_8211_CollateralRequestMsgMessagingServices>
                ((new DCAInDF_8211_CollateralRequestMsgMessagingServices()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                COLT_NG_8212_MSG10041_CollateralAnswerMsgMessagingService>
                ((new COLT_NG_8212_MSG10041_CollateralAnswerMsgMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInDF_8213_CollateralAnswerApprovalMsgMessagingServices>
                ((new DCAInDF_8213_CollateralAnswerApprovalMsgMessagingServices()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                EV_NG_8214_MSG23001_ConstraintApprovalRequestMessagingService>
                ((new EV_NG_8214_MSG23001_ConstraintApprovalRequestMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInEV_NG_8215_MSG23002_ConstraintApprovalDecisionMessagingServices>
                ((new DCAInEV_NG_8215_MSG23002_ConstraintApprovalDecisionMessagingServices()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInST_NG_70_MSG3_StorageResponseFromWarehouseMessagingServices>
                ((new DCAInST_NG_70_MSG3_StorageResponseFromWarehouseMessagingServices()).MainInterfaceCode);


            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInNG_5009_FirstAndSeconderyRequirementsMessagingService>
                ((new DCAInNG_5009_FirstAndSeconderyRequirementsMessagingService()).MainInterfaceCode);

            /*ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInTSH_MSG2_3050_PaymentOrderReplyMessagingService>
                ((new DCAInTSH_MSG2_3050_PaymentOrderReplyMessagingService()).MainInterfaceCode);*/

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                TSH_MSG6_AgentPaymentMessageService>
                ((new TSH_MSG6_AgentPaymentMessageService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInDE_NG_280_MSG11_DebtNotificationMessageMessagingServices>
                ((new DCAInDE_NG_280_MSG11_DebtNotificationMessageMessagingServices()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInDEPO_MSG2030_DepositRequestCreatedInfoMessagingServices>
                ((new DCAInDEPO_MSG2030_DepositRequestCreatedInfoMessagingServices()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                CD_NG_8347_Web01_CurrencyRateSearchParamMessagingService>
                ((new CD_NG_8347_Web01_CurrencyRateSearchParamMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                MM_Web01_MorningMessagesListMessagingService>
                ((new MM_Web01_MorningMessagesListMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                ST_8330_BlockListInWarehouseFilterMessagingService>
                ((new ST_8330_BlockListInWarehouseFilterMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            DCAInVE_3681_VendorErrorOrCriticalChangeMessagingService>
                            ((new DCAInVE_3681_VendorErrorOrCriticalChangeMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            DCAInDOC_NG_5101_GNMessageToAgentMessageService>
                            ((new DCAInDOC_NG_5101_GNMessageToAgentMessageService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            DOC_NG_5101_GNMessageToAgentMessagingService>
                            ((new DOC_NG_5101_GNMessageToAgentMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                          MN_NG_8240_CargoQueryMessagingService>
                          ((new MN_NG_8240_CargoQueryMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            SYSTBL_NG_9010_MSG_MessageRestoreRequestMessagingService>
                            ((new SYSTBL_NG_9010_MSG_MessageRestoreRequestMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                          EV_NG_8216_MSG23003_ConstraintAgentAnswerMessagingService>
                          ((new EV_NG_8216_MSG23003_ConstraintAgentAnswerMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                          DCAInDEPO_2020_DepositRefundOrderInfoMessagingService>
                          ((new DCAInDEPO_2020_DepositRefundOrderInfoMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                          DCAInDEPO_2000_DepositForfeitOrderInfoMessagingService>
                          ((new DCAInDEPO_2000_DepositForfeitOrderInfoMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                        DCAInVE_3700_ImporterPeriodicDeclarationReplyMessagingService>
                        ((new DCAInVE_3700_ImporterPeriodicDeclarationReplyMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                          DCAInLO_NG_3720_MSG313_PoaUpdateForCustomsAgentMessagingService>
                          ((new DCAInLO_NG_3720_MSG313_PoaUpdateForCustomsAgentMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                          DF_NG_8373_Web05_RetrieveImportDeclarationMessagingService>
                          ((new DF_NG_8373_Web05_RetrieveImportDeclarationMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
              DF_NG_Web8332_FaultProceduralParamMessagingService>
              ((new DF_NG_Web8332_FaultProceduralParamMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
              DCAInGRNT_1812_CreateGurateeRequestInfoMessagingService>
              ((new DCAInGRNT_1812_CreateGurateeRequestInfoMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                MN_MSG1_MANIFESTMessagingService>
                ((new MN_MSG1_MANIFESTMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
              GRNT_5004_GuaranteeReturnRequestMessagingService>
              ((new GRNT_5004_GuaranteeReturnRequestMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
              TPG_NG_2018_BankAccountToRefundUpdateReplayMessagingService>
              ((new TPG_NG_2018_BankAccountToRefundUpdateReplayMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                MM_Web01_MorningMessagesListMessagingService>
                ((new MM_Web01_MorningMessagesListMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
              ST_8328_Web01_WarehouseBlockBalanceMessagingService>
              ((new ST_8328_Web01_WarehouseBlockBalanceMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DF_NG_8302_Web03_DeclarationPrintMessagingService>
                ((new DF_NG_8302_Web03_DeclarationPrintMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                VE_8326_ImporterDeclarationMessagingService>
                ((new VE_8326_ImporterDeclarationMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                TSH_MSG6_AgentPaymentMessageService>
                ((new TSH_MSG6_AgentPaymentMessageService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                CL_3630_AddUpdateDeleteAddressContactMassagingService>
                ((new CL_3630_AddUpdateDeleteAddressContactMassagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInDEPO_NG_5110_DepositRequestFulfillednfoMsgMessagingService>
                ((new DCAInDEPO_NG_5110_DepositRequestFulfillednfoMsgMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInDF_5018_ImportExportDeclarationCancellationMessagingServices>
                ((new DCAInDF_5018_ImportExportDeclarationCancellationMessagingServices()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                VP_NG_2690_VehicleInMessagingService>
                ((new VP_NG_2690_VehicleInMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                MN_NG_9020_MasterBOLQueryMessagingService>
                ((new MN_NG_9020_MasterBOLQueryMessagingService()).MainInterfaceCode);
            // moran 28.6.15 Task 13281 -->
            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                TSH_NG_8285_Web01_PaymentFilterParamMessagingService>
                ((new TSH_NG_8285_Web01_PaymentFilterParamMessagingService()).MainInterfaceCode);
            // moran 28.6.15 Task 13281 <--
            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInDF_NG_5117_ImportDeclerationAmendmentReplyMessagingService>
                ((new DCAInDF_NG_5117_ImportDeclerationAmendmentReplyMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
          DCAInDF_NG_5118_MSG14004_ImportDeclarationCancellationReplyMsg>
          ((new DCAInDF_NG_5118_MSG14004_ImportDeclarationCancellationReplyMsg()).MainInterfaceCode);


            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                TPG_8304_DeficitFileFilterParamMessagingService>
                ((new TPG_8304_DeficitFileFilterParamMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInDE_NG_5108_DecisionMessageMessagingService>
                ((new DCAInDE_NG_5108_DecisionMessageMessagingService()).MainInterfaceCode);

            //<--- Yuval Chalup 23.06.2015 TASK-13278
            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                TSH_NG_8289_Web05_CreditQueryMessagingService>
                ((new TSH_NG_8289_Web05_CreditQueryMessagingService()).MainInterfaceCode);
            //Yuval Chalup 23.06.2015 TASK-13278 --->

            //<--- Yuval Chalup 25.06.2015 TASK-8907
            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                ST_NG_40_MSG7_SpecialActivityRequestMessagingService>
                ((new ST_NG_40_MSG7_SpecialActivityRequestMessagingService()).MainInterfaceCode);
            //Yuval Chalup 25.06.2015 TASK-8907 --->

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                TSH_8368_MasavPaymentsToAgentMessagingService>
                ((new TSH_8368_MasavPaymentsToAgentMessagingService()).MainInterfaceCode);
            // moran 5.7.15 - Task 13442 -->
            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                TPG_NG_8305_Web05_GuaranteeFileFilterParamMessagingService>
                ((new TPG_NG_8305_Web05_GuaranteeFileFilterParamMessagingService()).MainInterfaceCode);
            // moran 5.7.15 - Task 13442 <--

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInVAL_NG_8228_RequiredDocumentVerificationDecisionMassagingService>
                ((new DCAInVAL_NG_8228_RequiredDocumentVerificationDecisionMassagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInST_NG_10_SpecialActivityResponseMessagingServices>
                ((new DCAInST_NG_10_SpecialActivityResponseMessagingServices()).MainInterfaceCode);

            //<--- Yuval Chalup 07.09.2015 TASK-15037 
            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                TPG_NG_8307_Web09_DeclarationFilterMessagingService>
                ((new TPG_NG_8307_Web09_DeclarationFilterMessagingService()).MainInterfaceCode);
            //Yuval Chalup 07.09.2015 TASK-15037  --->

            //<--- Yuval Chalup 08.09.2015 TASK-15038
            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                TPG_NG_8306_Web07_GuaranteeCertificateMessagingService>
                ((new TPG_NG_8306_Web07_GuaranteeCertificateMessagingService()).MainInterfaceCode);
            //Yuval Chalup 08.09.2015 TASK-15038  --->
            // moran 20.10.15 - Task 17106 -->
            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInLP_NG_8400_MSG01_LogisticPermitMessageMessagingService>
                ((new DCAInLP_NG_8400_MSG01_LogisticPermitMessageMessagingService()).MainInterfaceCode);
            // moran 20.10.15 - Task 17106 <--

            //<--- Yuval Chalup 29.10.2015 TASK-16002
            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                MN_NG_9022_CourierBOLQueryMessagingService>
                ((new MN_NG_9022_CourierBOLQueryMessagingService()).MainInterfaceCode);
            //Yuval Chalup 29.10.2015 TASK-16002  --->

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInST_60_SpecialActivityExecutionReportMessagingService>
                ((new DCAInST_60_SpecialActivityExecutionReportMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                TPG_NG_8244_ClaimFileFilterParamMessagingService>
                ((new TPG_NG_8244_ClaimFileFilterParamMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                CBC_NG_8361_MSG01_CustomsBookInMessagingService>
                ((new CBC_NG_8361_MSG01_CustomsBookInMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                CLAIM_2340_ClaimRequestMessagingService>
                ((new CLAIM_2340_ClaimRequestMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                CLAIM_5005_ContinuousRequestOnClaimFileMessagingService>
                ((new CLAIM_5005_ContinuousRequestOnClaimFileMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInCLAIM_5115_ContinuousMessageMessagingServices>
                ((new DCAInCLAIM_5115_ContinuousMessageMessagingServices()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInCLAIM_2300_MissingDocumentRequestMessagingServices>
                ((new DCAInCLAIM_2300_MissingDocumentRequestMessagingServices()).MainInterfaceCode);


            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
              Unifreight_L2US01_US2L01_SivugMessagingService>
              ((new Unifreight_L2US01_US2L01_SivugMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
           DCAInUCUW2L_OpenDeclarationsByIntegratorInterfaceMessagingService>
           ((new DCAInUCUW2L_OpenDeclarationsByIntegratorInterfaceMessagingService()).MainInterfaceCode);


            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
           DCAInUCUDO_UpdateOpenDeclarationsMessagingService>
           ((new DCAInUCUDO_UpdateOpenDeclarationsMessagingService()).MainInterfaceCode);



            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
              DCAInUniDebug01_MsgMessagingService>
              ((new DCAInUniDebug01_MsgMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            CB_NG_8316_CustomItemLegalDemandsMessagingService>
                            ((new CB_NG_8316_CustomItemLegalDemandsMessagingService()).MainInterfaceCode);

            // moran 8.1.17 - Task 25399 -->
            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            CL_MSG100_AddClientMassagingService>
                            ((new CL_MSG100_AddClientMassagingService()).MainInterfaceCode);
            // moran 8.1.17 - Task 25399 <--

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            DF_9070_ExportDeclarationDataMessagingService>
                            ((new DF_9070_ExportDeclarationDataMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            TSH_WEB8289_9060_RTGSInfoQueryMessagingService>
                            ((new TSH_WEB8289_9060_RTGSInfoQueryMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            CL_NG_8343_ClientSearchByIDParamMessagingService>
                            ((new CL_NG_8343_ClientSearchByIDParamMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            ST_MSG05_StorageEntranceUnloadingMassagingService>
                            ((new ST_MSG05_StorageEntranceUnloadingMassagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            DCAInUniCustomTableZip_MsgMessagingService>
                            ((new DCAInUniCustomTableZip_MsgMessagingService()).MainInterfaceCode);



            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            DCAInUCB1170_MsgMessagingService>
                            ((new DCAInUCB1170_MsgMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            DCAInUCB2750_MsgMessagingService>
                            ((new DCAInUCB2750_MsgMessagingService()).MainInterfaceCode);

            //ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
            //                DCAInUCB2751_MsgMessagingService>
            //                ((new DCAInUCB2751_MsgMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            DCAInUCB2755_MsgMessagingService>
                            ((new DCAInUCB2755_MsgMessagingService()).MainInterfaceCode);
            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInUCB8212_MsgMessagingService>
                ((new DCAInUCB8212_MsgMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            DCAInUCB8250_MsgMessagingService>
                            ((new DCAInUCB8250_MsgMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            DCAInUCB2715_MsgMessagingService>
                            ((new DCAInUCB2715_MsgMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            DCAInUCBStorageSite_MsgMessagingService>
                            ((new DCAInUCBStorageSite_MsgMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            MN_MSG8370_CargoSplitMessagingService>
                            ((new MN_MSG8370_CargoSplitMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            DCAInCLAIM_5114_AcceptanceOrRejectionClaimMessageMessagingServices>
                            ((new DCAInCLAIM_5114_AcceptanceOrRejectionClaimMessageMessagingServices()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            DCAInDEPO_NG_2753_MSG4_DepositBankAccountToRefundUpdateRequestMessagingServices>
                            ((new DCAInDEPO_NG_2753_MSG4_DepositBankAccountToRefundUpdateRequestMessagingServices()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            GP_1030_GatepassRequestMessageMessagingService>
                            ((new GP_1030_GatepassRequestMessageMessagingService()).MainInterfaceCode);



            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                            DCAInUCBStorageSite_MsgMessagingService>
                            ((new DCAInUCBStorageSite_MsgMessagingService()).MainInterfaceCode);


            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInUCBUD2LT_MsgMessagingService>
                ((new DCAInUCBUD2LT_MsgMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInUCBCTML_MsgMessagingService>
                ((new DCAInUCBCTML_MsgMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInDE_NG_5108_DecisionMessageMessagingService>
                ((new DCAInDE_NG_5108_DecisionMessageMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DCAInDE_NG_5108_DecisionMessageMessagingService>
                ((new DCAInDE_NG_5108_DecisionMessageMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                DF_MSG2892_ImportDeclarationAmendmentMessagingService>
                ((new DF_MSG2892_ImportDeclarationAmendmentMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
              DF_MSG2892_ImportDeclarationAmendmentMessagingService>
              ((new DF_MSG2892_ImportDeclarationAmendmentMessagingService()).MainInterfaceCode);

            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                SE_6001_SealUpdateMessagingService>
                ((new SE_6001_SealUpdateMessagingService()).MainInterfaceCode);



            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,
                SaveDF_MSG5002_DeclarationCancellationRequestMsgService>
                ((new SaveDF_MSG5002_DeclarationCancellationRequestMsgService()).MainInterfaceCode);


            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,

              DCAInUCB9999ReAnAnalysis_MsgMessagingService>
              ((new DCAInUCB9999ReAnAnalysis_MsgMessagingService()).MainInterfaceCode);


            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,

               DCAInUCSBondedDocument_MessagingService>
               ((new DCAInUCSBondedDocument_MessagingService()).MainInterfaceCode);


            ContainerAccessor.Container.RegisterType<IMessagingServiceInterfaceType,

          SaveCC_MSG2450_ContainerizationMessageMessagingService>
          ((new SaveCC_MSG2450_ContainerizationMessageMessagingService()).MainInterfaceCode);


        }
        public static void InitContainer()
        {
            Debug.WriteLine("this method its to enshur static constractor is up ");
        }
        //public static void ResolveAndExecute(string mainInterfaceCode, int tenant, string correlationId,
        //    CustomsCommandEnum myCustomsCommandEnum)
        //{
        //    MessagingServiceFactoryHelper.InitContainer();
        //    var anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(mainInterfaceCode);
        //    anaO.CurrentCustomsCommandWR = myCustomsCommandEnum;

        //}

        public static IMessagingServiceInterfaceType GetMessagingService(string mainInterfaceCode, string correlationId = "")
        {
            MessagingServiceFactoryHelper.InitContainer();

            if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(mainInterfaceCode))
            {
                var inst = new InterfaceManagementDetails();
                var row = inst.GetAll().FirstOrDefault(r => r.ResponseInterfaceCode == mainInterfaceCode);
                if (row == null)
                {
                    throw new System.Exception("ResolveAndExecute(" + mainInterfaceCode + " , " + correlationId + ") But if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(mainInterfaceCode)), No ResponseInterfaceCode");
                }

                mainInterfaceCode = row.Code;
                if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(mainInterfaceCode))
                {
                    throw new System.Exception("ResolveAndExecute(" + mainInterfaceCode + " , " + correlationId + ") is response of  But if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(mainInterfaceCode))");
                }
            }
            var anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(mainInterfaceCode);
            return anaO;

        }
        public static void ResolveAndExecute(string mainInterfaceCode, int tenant, string correlationId,
            CustomsCommandEnum myCustomsCommandEnum, OverrideControllerModel debugModel = null)
        {
            //MessagingServiceFactoryHelper.InitContainer();

            //if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(mainInterfaceCode))
            //{
            //    var inst = new InterfaceManagementDetails();
            //    var row=inst.GetAll().FirstOrDefault(r => r.ResponseInterfaceCode == mainInterfaceCode);
            //    if (row == null)
            //    {
            //        throw new System.Exception("ResolveAndExecute(" + mainInterfaceCode + " , " + correlationId + ") But if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(mainInterfaceCode)), No ResponseInterfaceCode");
            //}

            //    mainInterfaceCode = row.Code;
            //    if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(mainInterfaceCode))
            //    {
            //        throw new System.Exception("ResolveAndExecute(" + mainInterfaceCode + " , " + correlationId + ") is response of  But if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(mainInterfaceCode))");
            //    }
            //}
            //var anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(mainInterfaceCode);
            var anaO = GetMessagingService(mainInterfaceCode, correlationId);
            if (anaO == null)
            {

                mainInterfaceCode = GetMainInteface(mainInterfaceCode);

                anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(mainInterfaceCode);
            }
            if (anaO==null)
            {
                throw new Exception("CustomsMessagingSheetWR: anaO==null >>ProcessMessage():!ContainerAccessor.Container.IsRegistered :analyzeClass=" + mainInterfaceCode );
                
            }

            anaO.CurrentCustomsCommandWR = myCustomsCommandEnum;

            anaO.MyOverrideControllerModel = debugModel;
            if (anaO.MyOverrideControllerModel != null)
            {
                anaO.MyOverrideControllerModel.CurrentCustomsCommandWR = anaO.CurrentCustomsCommandWR;
            }

            var resDat = anaO.SendSheet(tenant, correlationId);
            var responseDataBase = resDat as Logitude.CustomsMessaging.Common.ResponseData.ResponseDataBase;
            if (responseDataBase != null && responseDataBase.HasException)
            {
                Debug.WriteLine(responseDataBase.UserMessage);
            }
        }


        public static void ResolveAndReQueue(string mainInterfaceCode, int tenant, string correlationId,
            //CustomsCommandEnum myCustomsCommandEnum, 
            OverrideControllerModel debugModel = null)
        {
            MessagingServiceFactoryHelper.InitContainer();

            if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(mainInterfaceCode))
            {
                var inst = new InterfaceManagementDetails();
                var row = inst.GetAll().FirstOrDefault(r => r.ResponseInterfaceCode == mainInterfaceCode);
                if (row == null)
                {
                    throw new System.Exception("ResolveAndExecute(" + mainInterfaceCode + " , " + correlationId + ") But if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(mainInterfaceCode)), No ResponseInterfaceCode");
                }

                mainInterfaceCode = row.Code;
                if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(mainInterfaceCode))
                {
                    throw new System.Exception("ResolveAndExecute(" + mainInterfaceCode + " , " + correlationId + ") is response of  But if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(mainInterfaceCode))");
                }
            }
            var anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(mainInterfaceCode);
            if (anaO == null)
            {

                mainInterfaceCode = GetMainInteface(mainInterfaceCode);

                anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(mainInterfaceCode);
            }


            var resDat = anaO.ReQueue(tenant, correlationId);
            var responseDataBase = resDat as Logitude.CustomsMessaging.Common.ResponseData.ResponseDataBase;
            if (responseDataBase != null && responseDataBase.HasException)
            {
                Debug.WriteLine(responseDataBase.UserMessage);
            }
        }
#if false
        public static void SetDocumentsFilingService(Func<
            //WebFreight.Web.CommonDataModel.Tools.EntityService.IDocumentsFilingService
            object
            > 
            returnIDocumentsFilingService )
        {
            if (!ContainerAccessor.Container.IsRegistered<WebFreight.Web.CommonDataModel.Tools.EntityService.IDocumentsFilingService>())
            {
                ContainerAccessor.Container.RegisterType< WebFreight.Web.CommonDataModel.Tools.EntityService.IDocumentsFilingService>(
                    new InjectionFactory(c => { return 
                        returnIDocumentsFilingService() as 
                        WebFreight.Web.CommonDataModel.Tools.EntityService.IDocumentsFilingService; }));
            }
        }
        public static WebFreight.Web.CommonDataModel.Tools.EntityService.IDocumentsFilingService GetDocumentsFilingService()
        {
            if (!ContainerAccessor.Container.IsRegistered<WebFreight.Web.CommonDataModel.Tools.EntityService.IDocumentsFilingService>())
            {
                throw new Logitude.Server.Tools.Models.BusinessErrorException("No IsRegistered<WebFreight.Web.CommonDataModel.Tools.EntityService.IDocumentsFilingService>");


            }
            var documentsFilingService = ContainerAccessor.Container.Resolve<WebFreight.Web.CommonDataModel.Tools.EntityService.IDocumentsFilingService>();
            return documentsFilingService;
        }
#endif



        public static string GetMainInteface(string interfaceTypeCode)
        {

            var interfaceManagementDetails = new InterfaceManagementDetails();
            var requestinterface = interfaceManagementDetails.GetAll().FirstOrDefault(r => r.ResponseInterfaceCode == interfaceTypeCode);
            if (requestinterface == null)
            {
                throw new System.Exception("interfaceTypeCode: " + interfaceTypeCode + "  is not not registered  !!!!");
            }
            LogMessagingUtil.Instance.AppendLine("interfaceTypeCode: " + interfaceTypeCode + "  is not not registered  Found Main:" + requestinterface.Code);
            return requestinterface.Code;
        }
    }

}

