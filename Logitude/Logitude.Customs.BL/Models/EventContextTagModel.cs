using Logitude.Customs.Def.EntityPMs;
using System;

namespace Logitude.Customs.BL.Models
{
    public class EventContextTagModel
    {
        //public const string UpdateCH_NG_196_MSG7_CargoExitFromCheckSite = "Logitude.CustomsMessaging.ResponseServices.CH_NG_196_MSG7_CargoExitFromCheckSiteResponseService.Update()";
        public enum ProccessEnum
        {

            None,
            CH_NG_196_MSG7_CargoExitFromCheckSiteResponseServiceUpdate,
            DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageResponseServiceUpdate,
            CH_NG_190_MSG1_NoticeToClientResponseServiceInsert,
            CH_NG_190_MSG1_NoticeToClientResponseServiceUpdate,
            CH_NG_190_MSG1_NoticeToClientResponseServiceDelete,
            TSH_MSG7_AgentPaymentReplyResponseService,
            Deficit_NG_5009_MSG14_FirstAndSeconderyRequirementsMessageResponseService,
            GRNT_MSG15_createGurateeRequestInfoResponseService,
            DE_NG_5107_MSG10_AcceptanceOrRejectionMessageResponseService,
            TSH_MSG2_PaymentOrderReplyResponseServiceCancel,
            TSH_MSG2_PaymentOrderReplyResponseServiceUpdate,
            TSH_MSG2_PaymentOrderReplyResponseServiceCreate,
            EV_NG_8215_ConstraintApprovalDecisionResponseServiceDeny,
            EV_NG_8215_ConstraintApprovalDecisionResponseServiceApproved,
            EV_NG_8215_ConstraintApprovalDecisionResponseServiceConditionalApproval,
            VE_3681_VendorErrorOrCriticalChangeResponseServiceUpdate,
            VE_3681_VendorErrorOrCriticalChangeResponseServiceDefect,
            DF_8213_CollateralAnswerApprovalMsgResponseServiceReply,
            DEPO_2020_DepositRefundOrderInfoResponseServiceRefund,
            DEPO_2000_DepositForfeitOrderInfoResponseServiceForfeit,
            DEPO_MSG2030_DepositRequestCreatedInfoResponseServiceDeclaration,
            VE_3700_ImporterPeriodicDeclarationReplyResponseServiceUpdate,
            VE_3700_ImporterPeriodicDeclarationReplyResponseServiceNew,
            GRNT_MSG15_createGurateeRequestInfoResponseServiceUpdate,
            GRNT_MSG15_createGurateeRequestInfoResponseServiceNew,
            DF_NG_2754_MSG10004_SubmitDeclarationFuturePayment,
            EV_NG_8218_MSG14100_ProceduralFaultMsgUpdate,
            EV_NG_8218_MSG14100_ProceduralFaultMsgInsert,
            EV_NG_8218_MSG14100_ProceduralFaultMsgCancel,
            DEPO_NG_5110_DepositRequestFulfillednfoMsg,
            VAL_NG_8227_MSG_520_RequiredDocumentMessageInsert,
            VAL_NG_8227_MSG_520_RequiredDocumentMessageDelete,
            EV_NG_8214_MSG23001_ConstraintApproval,
            DF_NG_5018_MSG14004_ImportDeclarationCancellation,
            DF_8211_CollateralRequestMsgInsert,
            DF_8211_CollateralRequestMsgUpdate,
            DF_NG_5117_ImportDeclerationAmendmentReplyResponseService,
            VAL_NG_8228_RequiredDocumentVerificationDecisionVerified,
            VAL_NG_8228_RequiredDocumentVerificationDecisionVerifiedWithClient,
            VAL_NG_8228_RequiredDocumentVerificationDecisionReject,
            CLAIM_2300_MissingDocumentRequestResponseService,
            DF_NG_8251_Web02_DeclarationStatusResponseServiceCancel,
            CreateNewClaim,
            SentClaimToCustoms,
            MN_MSG4_SendManifestFeedBack_MessageResponseService,
            DOC_NG_5101_GNMessageToAgentResponseService,
            DeclarationClosure,
            CancelDeclarationClose,
            TSH_MSG7_AgentPaymentReplyResponseServiceBLD,
            DF_NG_8251_Web02_DeclarationStatusResponseServicePreClearance,
            DF_NG_8251_Web02_DeclarationStatusResponseServiceMessageToAgent,
        }
        public ProccessEnum CallProccessID { get; set; } //CargoExitFromCheckSite196
        //public PhysicalCheckPM DBOcc { get; set; } 
        public string EventCode { get; set; }
        public string FUStatusCode { get; set; }
        public string EventRemarks { get; set; }
        public string FUStatusRemarks { get; set; }

        public string StatusObjectTable { get; set; }
        public string StatusEntityId { get; set; }
        public string StatusCustomFileNo { get; set; }
        public string UnifreighTaskCode { get; set; }
        public DateTime StatusDateTime { get; set; }

        public NotificationPM MyNotificationPM { get; set; }
    }
}
