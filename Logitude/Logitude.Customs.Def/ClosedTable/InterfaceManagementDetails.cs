using Logitude.BL.Helpers;
using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;

namespace Logitude.Customs.Def.ClosedTable
{
    public class InterfaceManagementDetails : InterfaceManagement, ICloseTable<InterfaceManagement, InterfaceManagementDetails>
    {

        public List<InterfaceManagementDetails> GetAll()
        {
            const int CONST_DefaultPriority = 89;
            var all = new List<InterfaceManagementDetails>();
            all.Add(new InterfaceManagementDetails()
            {
                Code = "2750",
                InOut = InOutEnum.O.ToString(),
                Description = "הצהרת יבוא",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                // NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "2754",
                //   NeedSignature = false,  
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "2755",
                InOut = InOutEnum.O.ToString(),
                Description = "מסר הגשה",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "2754",
                //   NeedSignature = false,
                SignatureTypeCode = "C"
            });
            all.Add(new InterfaceManagementDetails()
            {
                Code = "2755E",
                InOut = InOutEnum.O.ToString(),
                Description = "מסר הגשה ליצוא",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "2754E",
                //   NeedSignature = false,
                SignatureTypeCode = "C"
            });
            all.Add(new InterfaceManagementDetails()
            {
                Code = "2754",
                InOut = InOutEnum.I.ToString(),
                Description = "מסר תשובה להגשה / הצהרה יבוא",
                //itzik+yaronc DcaPrefixName = ///"GetDF_MSG2755_2754_SubmitImportDeclarationRequest_Out.",//SaveDF_MSG2750_2754_ImportDeclarationRequest_Out.IL941079089.2014-07-20_20-15-27-909.1-68.TST
                DcaPrefixName = "SaveDF_MSG2750_2754_ImportDeclarationRequest_Out.",
                DcaPrefixName2 = "GetDF_MSG2755_2754_SubmitImportDeclarationRequest",
                DcaPrefixName3 = "SendDF_MSG2754_ImportDeclarationResponse",
                ///MOVE TO  ...DcaPrefixName4 = "GetDF_Web8373_2754_RetrieveImportDeclaration",

                DefaultSendOptionsCode = null,// Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false
            });
            all.Add(new InterfaceManagementDetails()
            {
                Code = "2754E",
                InOut = InOutEnum.I.ToString(),
                Description = "מסר תשובה להגשה / הצהרה יצוא",
                //itzik+yaronc DcaPrefixName = ///"GetDF_MSG2755_2754_SubmitImportDeclarationRequest_Out.",//SaveDF_MSG2750_2754_ImportDeclarationRequest_Out.IL941079089.2014-07-20_20-15-27-909.1-68.TST
                DcaPrefixName = "SaveDF_MSG2750_2754_ImportDeclarationRequest_Out.",
                DcaPrefixName2 = "GetDF_MSG2755_2754_SubmitImportDeclarationRequest",
                DcaPrefixName3 = "SendDF_MSG2754_ImportDeclarationResponse",
                ///MOVE TO  ...DcaPrefixName4 = "GetDF_Web8373_2754_RetrieveImportDeclaration",

                DefaultSendOptionsCode = null,// Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false
            });
            //D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityMessagingService
            all.Add(new InterfaceManagementDetails()
            {
                Code = "2715",
                InOut = InOutEnum.O.ToString(),
                Description = "קלוט צרופה",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "2716",
                //NeedSignature = true,
                SignatureTypeCode = "C"
            });

            //DCAInD_NG_2716_MSG22001_AddAttachmentMessagingService
            all.Add(new InterfaceManagementDetails()
            {
                Code = "2716", //D_NG_2716_MSG22001_AddAttachmentResponse
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לקלוט צרופה",
                DcaPrefixName = "GetDOC_MSG2715_2716_AddAttachmentResponse_Out.", // to check with itzik
                DefaultSendOptionsCode = null,// Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //    NeedSignature = false
            });


            ///DCAInCH_NG_190_MSG1_NoticeToClientMessagingService
            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "190",
                InOut = InOutEnum.I.ToString(),
                Description = "זימון בדיקה במכס",
                DcaPrefixName = "SendCH_MSG_190_NoticeToClient_Out.",
                DcaPrefixName2 = "SendCH_MSG_190_NoticeToClient_EX_Out",
                DefaultSendOptionsCode = null,// Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //   NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "3051",
                InOut = InOutEnum.O.ToString(),
                Description = "בקשת תשלום",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "3052",
                //   NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "3052",
                InOut = InOutEnum.I.ToString(),
                Description = "תשלום הוראה",
                DcaPrefixName = "SendTSH_MSG7_AgentPaymentReply_Out.",
                //GetTSH_MSG3051_3052_AgentPaymentReply_Out.IL941079089.2016-06-15_10-05-19-312.7d54100a-2058-4791-aab0-13a15ef76053.PLT
                DcaPrefixName2 = "GetTSH_MSG3051_3052_AgentPaymentReply_Out.",
                DefaultSendOptionsCode = null,// Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "3053",
                InOut = InOutEnum.O.ToString(),
                Description = "בקשת אחזור תשלום",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "3050",
                //       NeedSignature = false
            });

            //DCAInTSH_MSG2_3050_PaymentOrderReplyMessagingService
            all.Add(new InterfaceManagementDetails()
            {
                Code = "3050",
                InOut = InOutEnum.I.ToString(),
                Description = "הודעה לסוכן על הוראת תשלום שנוצרה",
                DcaPrefixName = "SendTSH_MSG3050_PaymentOrderReply_Out.",
                //SaveTSH_MSG3053_3050_AgentPaymentRequest_Out.IL941079089.2016-06-15_14-01-22-604.32431015-a1d4-4fa6-9894-93d2eb1b0f9c.PLT
                DcaPrefixName2 = "SaveTSH_MSG3053_3050_AgentPaymentRequest_Out.",
                DefaultSendOptionsCode = null,// Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "9000",
                InOut = InOutEnum.O.ToString(),
                Description = "אחזור טבלאות",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "9001",
                //   NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "9001",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לאחזור טבלאות",
                DcaPrefixName = "GetSYSTBL_MSG9000_9001_SystemTableRequest_Out.",
                DefaultSendOptionsCode = null,// Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //   NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false
            });


            all.Add(new InterfaceManagementDetails()
            {
                Code = "3610",
                InOut = InOutEnum.O.ToString(),
                Description = "פרטי זיהוי לקוח",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //   NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "3620",
                //  NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "3620",
                InOut = InOutEnum.I.ToString(),
                Description = "פרטי לקוח לסוכן המכס",
                DcaPrefixName = "GetCL_MSG3610_3620_WholeClientForCustomsAgent_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //     NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "3630",
                InOut = InOutEnum.O.ToString(),
                Description = "הוספת/עדכון/ביטול כתובת לקוח",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //   NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                //   NeedSignature = false
            });

            //VE_MSG051_VendorSearchByCustomsAgentMessagingService
            all.Add(new InterfaceManagementDetails()
            {
                Code = "3650",
                InOut = InOutEnum.O.ToString(),
                Description = "פרמטרים לחיפוש ספק / לקוח חול",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //   NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "3660",
                //  NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "3660",
                InOut = InOutEnum.I.ToString(),
                Description = "תוצאות חיפוש ספק / לקוח חול",
                DcaPrefixName = "GetVE_MSG3650_3660_VendorSearchResultsForCustomsAgentMessage_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //   NeedSignature = false
            });

            //VE_MSG010_VendorInsertUpdateDeleteMessagingService
            all.Add(new InterfaceManagementDetails()
            {
                Code = "3670",
                InOut = InOutEnum.O.ToString(),
                Description = "הקמת/עדכון/ביטול ספק/לקוח חול",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "3670I",
                //   NeedSignature = false
            });
            //C:\CyberArk_DCA\msd-il510785884\download\msd\SaveVE_MSG3670_VendorInsertUpdateDeleteMessage_Out.IL941079089.2016-05-25_13-49-35-357.8b1e6660-7df3-405c-a5f9-1201337449c1.PLT.xml
            all.Add(new InterfaceManagementDetails()
            {
                Code = "3670I",
                InOut = InOutEnum.I.ToString(),
                Description = "הקמת/עדכון/ביטול ספק/לקוח חול",
                DcaPrefixName = "SaveVE_MSG3670_VendorInsertUpdateDeleteMessage_Out.",
                //SaveVE_MSG3670_VendorInsertUpdateDeleteMessage_Out.IL941079089.2016-06-28_14-36-22-468.c7a8c3f5-9ec1-4930-8d2d-fc039c35d8c2.PLT
                DcaPrefixName2 = "",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //   NeedSignature = false
            });

            //VE_MSG013_VendorAddCommunicationDeviceMessageService
            all.Add(new InterfaceManagementDetails()
            {
                Code = "3680",
                InOut = InOutEnum.O.ToString(),
                Description = "הוספת פרטי התקשרות לספק/לקוח חול",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "3680I",
                //   NeedSignature = false
            });
            all.Add(new InterfaceManagementDetails()
            {
                Code = "3680I",
                InOut = InOutEnum.I.ToString(),
                Description = "הוספת פרטי התקשרות לספק/לקוח חול- חוזר",
                //SaveVE_MSG3680_VendorAddCommunicationDevice_Out.IL941079089.2016-06-15_10-20-21-915.3d7d3456-1f81-4f21-897c-38d3220c8f65.PLT
                DcaPrefixName = "SaveVE_MSG3680_VendorAddCommunicationDevice_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //   NeedSignature = false
            });

            //DCAInVE_3681_VendorErrorOrCriticalChangeMessagingService
            all.Add(new InterfaceManagementDetails()
            {
                Code = "3681",
                InOut = InOutEnum.I.ToString(),
                Description = "טעות או שינוי מהותי בספק / לקוח חול",
                DcaPrefixName = "SendVE_MSG3681_VendorErrorOrCriticalChangeMessage_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //   NeedSignature = false
            });

            //DF_NG_8250_Web01_DeclarationStatus_RequestMessagingService
            all.Add(new InterfaceManagementDetails()
            {
                Code = "8250",
                InOut = InOutEnum.O.ToString(),
                Description = "שאילתא לסטטוס הצהרה",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //   NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8251",
                //      NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8251",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לשאילתא לסטטוס הצהרה",
                DcaPrefixName = "GetDF_WEB8250_8251_DeclarationStatusQuery_Request_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                DcaPrefixName2 = "SendDF_MSG8251__DeclarationStatus_Response_Out.",
                DcaPrefixName3 = "SendDF_MSG8251__DeclarationStatus_Response_EX_Out."
                //    NeedSignature = false
            });


            //DCAInDE_NG_5107_MSG10_AcceptanceOrRejectionMessagingService
            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "5107",
                InOut = InOutEnum.I.ToString(),
                Description = "מסר דחיה או אישור גרעון עצמי",
                DcaPrefixName = "SendDE_MSG5107_AcceptanceOrRejectionMessage_Out.",
                DefaultSendOptionsCode = null,// Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //   NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //   NeedSignature = false,
            });

            //DCAInDEPO_NG_5110_DepositRequestFulfillednfoMsgMessagingService
            all.Add(new InterfaceManagementDetails()
            {
                Code = "5110",
                InOut = InOutEnum.I.ToString(),
                Description = "יידוע על קבלת פיקדון ופתיחת תיק פיקדון",
                DcaPrefixName = "SendDEPO_MSG5110_DepositRequestFulfillednfoMsg_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //   NeedSignature = false,
            });

            //DCAInCLAIM_5115_ContinuousMessageMessagingServices
            all.Add(new InterfaceManagementDetails()
            {
                Code = "5115",
                InOut = InOutEnum.I.ToString(),
                Description = "הודעות שוטפות",
                DcaPrefixName = "SendCLAIM_MSG5115_ContinuousMessage_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //   NeedSignature = false,
            });
            all.Add(new InterfaceManagementDetails()
            {
                Code = "5118",
                InOut = InOutEnum.I.ToString(),
                Description = "מענה לבקשת ביטול הצהרת יבוא",
                DcaPrefixName = "SendDF_MSG5118_DeclarationCancellationReplyMsg_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false,
            });
            //DCAInDF_NG_5117_ImportDeclerationAmendmentReplyMessagingService
            all.Add(new InterfaceManagementDetails()
            {
                Code = "5117",
                InOut = InOutEnum.I.ToString(),
                Description = "מענה לבקשה לתיקון הצהרה יבוא",
                DcaPrefixName = "SendDF_MSG5117_ImportDeclerationAmendmentReplyMsg_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false,
            });
            all.Add(new InterfaceManagementDetails()
            {
                Code = "8237",
                InOut = InOutEnum.I.ToString(),
                Description = "מענה לבקשה לתיקון הצהרה יצוא",
                DcaPrefixName = "SendDF_MSG8237_ExportDeclarationAmendmentReplyMsg_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = 5,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false,
            });
            //DCAInCH_NG_196_MSG7_CargoExitFromCheckSiteMassageService
            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "196",
                InOut = InOutEnum.I.ToString(),
                Description = "שחרור מטען מאתר בדיקה",
                DcaPrefixName = "SendCH_MSG_196_CargoExitFromCheckSite_Out.",
                DcaPrefixName2 = "SendCH_MSG_196_CargoExitFromCheckSite_EX_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //   NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                // NeedSignature = false
            });


            //DCAInDF_NG_2470_DF_MSG16001_ReleaseGoodsMessagingService
            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "2470",
                InOut = InOutEnum.I.ToString(),
                Description = "התרה",
                DcaPrefixName = "SendDF_MSG2470_ReleaseGoodsMessage_Out.",
                DcaPrefixName2= "SendDF_MSG2470_ReleaseGoodsMessage_EX_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //    NeedSignature = false
            });
            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "2791",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לתעודת אחסנה",
                DcaPrefixName = "SendMN_MSG2791_ExportDeliveryAnswerMessage_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //    NeedSignature = false
            });

            //DCAInEV_NG_8219_MSG14100_ProceduralFaultCancelMassagingService
            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "8219",
                InOut = InOutEnum.I.ToString(),
                Description = "מסר ביטול ליקוי מנהלי",
                DcaPrefixName = "SendEV_MSG8219_ProceduralFaultCancelMsg_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //   NeedSignature = false
            });

            //DCAInVAL_NG_8227_MSG_520_RequiredDocumentMassagingService
            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "8227",
                InOut = InOutEnum.I.ToString(),
                Description = "מסר מסמך נדרש",
                DcaPrefixName = "SendVAL_MSG8227_RequiredDocumentMessage_Out.",
                DcaPrefixName2 = "SendVAL_MSG8227_RequiredDocumentMessage_EX_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false
            });
           
            //DCAInVAL_NG_8228_RequiredDocumentVerificationDecisionMassagingService
            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "8228",
                InOut = InOutEnum.I.ToString(),
                Description = "מסר אימות מסמך",
                DcaPrefixName = "SendVAL_MSG8228_RequiredDocumentVerificationDecisionMessage_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
            });

            //DCAInDE_NG_280_MSG11_DebtNotificationMessageMessagingServices
            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "280",
                InOut = InOutEnum.I.ToString(),
                Description = "הודעת חיוב",
                DcaPrefixName = "SendDE_MSG280_DebtNotificationMessage_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //   NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                // NeedSignature = false
            });

            //DCAInDF_8211_CollateralRequestMsgMessagingServices
            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "8211",
                InOut = InOutEnum.I.ToString(),
                Description = "מסר דרישה לבטוחה",
                DcaPrefixName = "SendCOLT_MSG_8211_CollateralRequestMsg_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //    NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8212",
                InOut = InOutEnum.O.ToString(),
                Description = "מסר מענה לדרישה לבטוחה",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                // NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8212I",  // 8213 - dca
                //  NeedSignature = false,
            });
            //SaveCOLT_MSG8212_CollateralAnswerMsg_Out.IL941079089.2016-05-25_14-29-07-895.a8557b83-430b-419e-b328-f3a62be1ddbb.PLT

            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "8212I",
                InOut = InOutEnum.I.ToString(),
                Description = " מענה לדרישה לבטוחה",
                DcaPrefixName = "SaveCOLT_MSG8212_CollateralAnswerMsg_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //    NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false
            });

            //DCAInDF_8213_CollateralAnswerApprovalMsgMessagingServices
            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "8213",
                InOut = InOutEnum.I.ToString(),
                Description = "מסר אישור או דחיית המענה לדרישה לבטוחה",
                DcaPrefixName = "SendCOLT_MSG_8213_CollateralAnswerApprovalMsg_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //    NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false
            });

            //EV_NG_8214_MSG23001_ConstraintApprovalRequestMessagingService
            all.Add(new InterfaceManagementDetails()
            {
                Code = "8214",
                InOut = InOutEnum.O.ToString(),
                Description = "מסר בקשה של סוכן לאישור אילוצים",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                // NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8214I", // 8215
                //  NeedSignature = false,
                SignatureTypeCode = "C"
            });
            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "8214I",
                InOut = InOutEnum.I.ToString(),
                Description = "מסר (חוזר) של החלטת גורם מאשר לאישור האילוץ",
                //SaveEV_MSG_8214_ConstraintApprovalRequest_Out.IL941079089.2016-06-15_10-51-20-948.178f2915-9ad7-446b-be9b-85e57bfe242c.PLT
                DcaPrefixName = "SaveEV_MSG_8214_ConstraintApprovalRequest_Out.", // "GetDF_WEB8250_8251_DeclarationStatusQuery_Request_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //   NeedSignature = false
            });
            //DCAInEV_NG_8215_MSG23002_ConstraintApprovalDecisionMessagingServices
            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "8215",
                InOut = InOutEnum.I.ToString(),
                Description = "מסר של החלטת גורם מאשר לאישור האילוץ",
                DcaPrefixName = "SendEV_MSG8215_ConstraintApprovalDecision_Out.", // "GetDF_WEB8250_8251_DeclarationStatusQuery_Request_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //   NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8216",
                InOut = InOutEnum.O.ToString(),
                Description = "מסר ערעור על החלטת אילוץ",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                // NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8216I",
                //    NeedSignature = false,
            });


            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "8216I",
                InOut = InOutEnum.I.ToString(),
                Description = "מסר חוזר ערעור על החלטת אילוץ",
                //SaveEV_MSG_8216_ConstraintAgentAnswer_Out.IL941079089.2016-06-15_10-51-20-700.26ced195-4382-4ebf-b8ed-c5e4a5ee2f9d.PLT
                DcaPrefixName = "SaveEV_MSG_8216_ConstraintAgentAnswer_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //     NeedSignature = false
            });

            //DCAInEV_NG_8218_ProceduralFaultMessagingService
            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "8218",
                InOut = InOutEnum.I.ToString(),
                Description = "מסר רישום/עדכון ליקוי מנהלי",
                DcaPrefixName = "SendEV__MSG8218_ProceduralFaultMsg_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //     NeedSignature = false
            });

            //DCAInST_NG_70_MSG3_StorageResponseFromWarehouseMessagingServices
            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "70",
                InOut = InOutEnum.I.ToString(),
                Description = "מסר אישור/דחייה בקשת אחסנה",
                DcaPrefixName = "SendST_MSG70_StorageResponseFromWarehouse_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //    NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false
            });

            //DCAInCLAIM_2300_MissingDocumentRequestMessagingServices
            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "2300",
                InOut = InOutEnum.I.ToString(),
                Description = "בקשת מסמכים חסרים",
                DcaPrefixName = "SendCLAIM_MSG2300_MissingDocumentRequest_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //    NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //    NeedSignature = false
            });

            //DCAIn...
            //all.Add(
            //new InterfaceManagementDetails()
            //{
            //    Code = "3720",
            //    InOut = InOutEnum.I.ToString(),
            //    Description = "עדכון סוכן מכס אודות כתב הרשאה שבו הוא המורשה",
            //    DcaPrefixName = null, // to check???
            //    DefaultSendOptionsCode = null,
            //    DefaultPriority = CONSTDefaultPriority,
            //    AllowRestore = true,
            //    //NotificationId = "",
            //    Active = true,
            //    SendAsDual = false,
            //    ResponseInterfaceCode = null,
            //    NeedSignature = false
            //});

            //DCAInNG_5009_FirstAndSeconderyRequirementsMessagingService
            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "5009",
                InOut = InOutEnum.I.ToString(),
                Description = "הודעה על דרישה ראשונה (חוב בהתראה) ודרישה שניה (חוב בהרשאה)",
                DcaPrefixName = "SendDE_MSG5009_FirstAndSeconderyRequirementsMessage_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8330",
                InOut = InOutEnum.O.ToString(),
                Description = "שאילתת גושים במחסן",
                DcaPrefixName = "",
                DefaultSendOptionsCode = null,// Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8331",
                //   NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8331",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לגושים במחסן",
                //GetST_Web8330_8331_BlockListInWarehouseDetail_Out.IL941079089.2016-06-26_17-05-43-269.ee21272c-ea71-4077-a143-05740022ec85.PLT
                DcaPrefixName = "GetST_Web8330_8331_BlockListInWarehouseDetail_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false
            });

            //DCAInDEPO_MSG2030_DepositRequestCreatedInfoMessagingServices
            all.Add(new InterfaceManagementDetails()
            {
                Code = "2030",
                InOut = InOutEnum.I.ToString(),
                Description = "דרישה לתשלום פיקדון",
                DcaPrefixName = "SendDEPO_MSG2030_DepositRequestCreatedInfoMsg_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //   NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "2018",
                InOut = InOutEnum.O.ToString(),
                Description = "בקשה להחזר פיקדון ",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //    NeedSignature = false,
            });

            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "8347",
                InOut = InOutEnum.O.ToString(),
                Description = "שאילתא לשערי מטבע",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8348",
                //   NeedSignature = false,

            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8348",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לשערי מטבע",
                //GetCD_8347_8348_Web01_02_CurrencyRateSearch_Out.IL941079089.2016-06-27_15-34-52-416.b9f7a6b0-bce4-407a-88c1-f9bf4c41d461.PLT
                DcaPrefixName = "GetCD_8347_8348_Web01_02_CurrencyRateSearch_Out.",
                DefaultSendOptionsCode = null,// Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false
            });

            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "191",
                InOut = InOutEnum.O.ToString(),
                Description = "בקשת שינוי מועד בדיקה",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "192",
                //   NeedSignature = false,

            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "192",
                InOut = InOutEnum.I.ToString(),
                Description = "תשובה בדבר בקשת שינוי מועד בדיקה",
                DcaPrefixName = "SendCH_MSG_192_ApproveChangeTimeRequest_Out.",
                //SaveCH_MSG_191__192ChangingTimeRequest_Out.IL941079089.2016-06-15_08-58-18-134.294e63fa-b4b4-432c-84b9-11cac2141356.PLT
                DcaPrefixName2 = "SaveCH_MSG_191__192ChangingTimeRequest_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //    NeedSignature = false
            });

            //DCAInST_NG_10_SpecialActivityResponseMessagingServices
            all.Add(new InterfaceManagementDetails()
            {
                Code = "10",
                InOut = InOutEnum.I.ToString(),
                Description = "אישור/דחייה ביצוע פעולה מיוחדת",
                DcaPrefixName = "SendST_MSG10_SpecialActivityResponseMessage_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //    NeedSignature = false
            });


            //all.Add(
            //new InterfaceManagementDetails()
            //{
            //    Code = "40",
            //    InOut = InOutEnum.O.ToString(),
            //    Description = "בקשת ביצוע פעולה מיוחדת",
            //    DcaPrefixName = "",
            //    DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
            //    DefaultPriority = CONSTDefaultPriority,
            //    AllowRestore = true,
            //    //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
            //    Active = true,
            //    SendAsDual = false,
            //    ResponseInterfaceCode = null,
            // //   NeedSignature = false,
            //});

            //DCAInST_60_SpecialActivityExecutionReportMessagingService
            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "60",
                InOut = InOutEnum.I.ToString(),
                Description = "דיווח ביצוע פעולה מיוחדת",
                DcaPrefixName = "SendST_MSG60_SpecialActivityExecutionReportMessage_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false,
            });

            //DCAInGRNT_1812_CreateGurateeRequestInfoMessagingService
            all.Add(new InterfaceManagementDetails()
            {
                Code = "1812",
                InOut = InOutEnum.I.ToString(),
                Description = "בקשה להמצאת ערבות שנוצרה במערכת",
                DcaPrefixName = "SendGRNT_MSG1812_createGurateeRequestInfo_Out.",
                DefaultSendOptionsCode = null,// Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //   NeedSignature = false
            });

            //DCAInDEPO_2000_DepositForfeitOrderInfoMessagingService
            all.Add(new InterfaceManagementDetails()
            {
                Code = "2000",
                InOut = InOutEnum.I.ToString(),
                Description = "יידוע בדבר ביצוע חילוט פיקדון",
                DcaPrefixName = "SendDEPO_MSG2000_DepositForfeitOrderInfo_Out.",
                DefaultSendOptionsCode = null,// Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                // NeedSignature = false
            });

            //DCAInDEPO_2020_DepositRefundOrderInfoMessagingService
            all.Add(new InterfaceManagementDetails()
            {
                Code = "2020",
                InOut = InOutEnum.I.ToString(),
                Description = "הודעה לסוכן בדבר ביצוע החזר פיקדון או תשובה לסוכן בדבר בקשתו להחזרת פיקדון",
                DcaPrefixName = "SendDEPO_MSG2020_DepositRefundOrderInfo_Out.",
                DefaultSendOptionsCode = null,// Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false
            });
            //#if after_massaging_create_not_b4 
            all.Add(new InterfaceManagementDetails()
            {
                Code = "2340",
                InOut = InOutEnum.O.ToString(),
                Description = "הגשת בקשת תביעה",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "2345",
                //    NeedSignature = false,
                SignatureTypeCode = "C"
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "2345",
                InOut = InOutEnum.I.ToString(),
                Description = "תשובה לבקשת תביעה",
                //GetCLAIM_MSG2340_2345_ClaimAnswer_Out.IL941079089.2016-06-15_09-08-23-928.680a1486-1003-45b8-a9ee-5ee39fd3fb39.PLT
                DcaPrefixName = "GetCLAIM_MSG2340_2345_ClaimAnswer_Out.",
                DefaultSendOptionsCode = null,// Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //   NeedSignature = false
            });
            //#endif
            all.Add(new InterfaceManagementDetails()
            {
                Code = "3600",
                InOut = InOutEnum.O.ToString(),
                Description = "הוספת לקוח",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,//"2345",
                //  NeedSignature = false,

            });

            //DCAInVE_3700_ImporterPeriodicDeclarationReplyMessagingService
            all.Add(new InterfaceManagementDetails()
            {
                Code = "3700",
                InOut = InOutEnum.I.ToString(),
                Description = "תשובה לתצהיר יבואן תקופתי",
                DcaPrefixName = "SendVE_MSG3700_ImporterPeriodicDeclarationReplyMessage_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //   NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "3720",
                InOut = InOutEnum.I.ToString(),
                Description = "עדכון סוכן מכס אודות כתב הרשאה שבו הוא המורשה",
                DcaPrefixName = "SendLO_MSG3720_PoaUpdateForCustomsAgent_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //    NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "5004",
                InOut = InOutEnum.O.ToString(),
                Description = "בקשה להחזרת ערבות",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                // NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //    NeedSignature = false,
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8361",
                InOut = InOutEnum.O.ToString(),
                Description = "מסר קבלת טבלאות ספר המכס",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8362",
                //  NeedSignature = false,
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8362",
                InOut = InOutEnum.I.ToString(),
                Description = "מסר קבלת טבלאות ספר המכס",
                //Get_CBC_MSG_8361_8362_CustomsBook_Out.IL941079089.2016-06-15_11-42-23-221.4e005696-13da-4e78-92ad-334bff5f791f.PLT
                DcaPrefixName = "Get_CBC_MSG_8361_8362_CustomsBook_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //     NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8370",
                InOut = InOutEnum.O.ToString(),
                Description = "מסר בקשה לפיצול מטען",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8374",
                //      NeedSignature = false,
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8374",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לפיצול מטען",
                DcaPrefixName = "SendMN_MSG8374_CargoSplitRequestFeedBack_Message_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                //      NeedSignature = false,
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "2753",
                InOut = InOutEnum.I.ToString(),
                Description = "השלמת פרטי החזר פקדון",
                DcaPrefixName = "SendDEPO_MSG2753_DepositBankAccountToRefundUpdateRequest_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                //      NeedSignature = false,
            });

            //all.Add(new InterfaceManagementDetails()
            //{
            //    Code = "9000",
            //    InOut = InOutEnum.O.ToString(),
            //    Description = "אחזור טבלאות",
            //    DcaPrefixName = "",
            //    DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
            //    DefaultPriority = CONSTDefaultPriority,
            //    AllowRestore = true,
            //    //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
            //    Active = true,
            //    SendAsDual = false,
            //    ResponseInterfaceCode = "9001",
            //    NeedSignature = false,
            //});

            //all.Add(new InterfaceManagementDetails()
            //{
            //    Code = "9001",
            //    InOut = InOutEnum.I.ToString(),
            //    Description = "משוב לאחזור טבלאות",
            //    DcaPrefixName = "GetSYSTBL_MSG9000_9001_SystemTableRequest_Out.",
            //    DefaultSendOptionsCode = null,
            //    DefaultPriority = CONSTDefaultPriority,
            //    AllowRestore = true,
            //    //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
            //    Active = true,
            //    SendAsDual = false,
            //    ResponseInterfaceCode = null,
            //    NeedSignature = false
            //});

            all.Add(new InterfaceManagementDetails()
            {
                Code = "9010",
                InOut = InOutEnum.O.ToString(),
                Description = "אחזור מסרים",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "9011",
                //   NeedSignature = false,
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "9011",
                InOut = InOutEnum.I.ToString(),
                Description = "אחזור מסרים",
                DcaPrefixName = "GetSYSTBL_MSG9010_9011_MessageRestoreRequest_Out.",//TODO TASK:8061 get from yaron true prefix
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //     NeedSignature = false
            });



            all.Add(new InterfaceManagementDetails()
            {
                Code = "9100",
                InOut = InOutEnum.O.ToString(),
                Description = "הודעות ממתינות",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "9101",
                //   NeedSignature = false,
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "9101",
                InOut = InOutEnum.I.ToString(),
                Description = "אחזור הודעות ממתינות",
                DcaPrefixName = "UNKNOWN_CHECKING GetSYSTBL_MSG9010_9011_MessageRestoreRequest_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = false,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //     NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "5101I",
                InOut = InOutEnum.I.ToString(),
                Description = "הודעות לסוכן",
                DcaPrefixName = "GetDOC_NG_5101_GNMessageToAgentMsg_Out.",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                // NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                //  NeedSignature = false,
            });



            all.Add(new InterfaceManagementDetails()
            {
                Code = "5101O",
                InOut = InOutEnum.O.ToString(),
                Description = "מענה להודעות לסוכן",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "5101I",
                //    NeedSignature = false,
            });

            //DCAInDOC_NG_5101_GNMessageToAgentMessageService
            all.Add(new InterfaceManagementDetails()
            {
                Code = "5101O_I",
                InOut = InOutEnum.I.ToString(),
                Description = "הודעות לסוכן",
                DcaPrefixName = "SendDOC_MSG5101_GNMessageToAgent_Out.", //"SendDOC_NG_5101_GNMessageToAgentMsg_Out.",
                DcaPrefixName2= "SendDOC_MSG5101_GNMessageToAgent_EX_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //   NeedSignature = false
            });


            all.Add(new InterfaceManagementDetails()
            {
                Code = "195",
                InOut = InOutEnum.O.ToString(),
                Description = "תשובה לבדיקה פיזית",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = 5,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "195",
                //  NeedSignature = false,
            });


            all.Add(new InterfaceManagementDetails()
            {
                Code = "8240",
                InOut = InOutEnum.O.ToString(),
                Description = "שאילתא למצהר",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8241",
                //  NeedSignature = false,
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8241",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לשאילתא למצהר",
                DcaPrefixName = "GetMN_MSG_8240_8241_CargoQuery_Message_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //   NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {// moran 2.11.14 - Task 7933
                Code = "8328",
                InOut = InOutEnum.O.ToString(),
                Description = "שאילתא ליתרות מלאי בגוש",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8329",
                //    NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {// moran 2.11.14 - Task 7933
                Code = "8329",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב ליתרות מלאי בגוש",
                ///GetST_Web8328_8329_WarehouseBlockBalanceFilterParam_Out.IL941079089.2016-06-27_15-29-51-726.6b09a6cf-ebad-4efe-b023-e95d55acd94b.PLT
                DcaPrefixName = "GetST_Web8328_8329_WarehouseBlockBalanceFilterParam_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //        NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8326",
                InOut = InOutEnum.O.ToString(),
                Description = "שאילתא לתצהיר יבואן",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8327",
                //  NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8327",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לתצהיר יבואן",
                //GetVE_Web8326_8327_ImporterDeclarations_Out.IL941079089.2016-06-27_15-29-52-544.c53ca9d4-a241-457b-a8dd-ec4a60f3091e.PLT
                DcaPrefixName = "GetVE_Web8326_8327_ImporterDeclarations_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false
            });

            //<--- Yuval Chalup 23.12.2014 TASK-9972
            all.Add(new InterfaceManagementDetails()
            {
                Code = "8373",
                InOut = InOutEnum.O.ToString(),
                Description = "שאילתא לשחזור נתוני הצהרה",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                // NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8373I",
                //  NeedSignature = false,
            });


            all.Add(new InterfaceManagementDetails()
            {
                Code = "8373I",
                InOut = InOutEnum.I.ToString(),
                Description = "מסר תשובה לשחזור נתוני הצהרה",
                //itzik+yaronc DcaPrefixName = ///"GetDF_MSG2755_2754_SubmitImportDeclarationRequest_Out.",//SaveDF_MSG2750_2754_ImportDeclarationRequest_Out.IL941079089.2014-07-20_20-15-27-909.1-68.TST
                DcaPrefixName = "GetDF_Web8373_2754_RetrieveImportDeclaration_Out.",
                //GetTSH_MSG8368_8356_MasavPaymentsToAgent_Out.IL941079089.2016-06-26_17-05-42-369.6336820c-a2d6-48f5-8b9d-4e3dcda26d8e.PLT


                DefaultSendOptionsCode = null,// Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false
            });
            //Yuval Chalup 23.12.2014 TASK-9972 --->

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8332",
                InOut = InOutEnum.O.ToString(),
                Description = "שאילתא לליקויים",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8333",
                //  NeedSignature = false,
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8333",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לשאילתא לליקויים",
                //        GetDF_Web8332_8333_FaultProceduralDetails_Out.IL941079089.2016-06-23_16-56-02-910.55ef358e-fe75-4d13-889e-1431ee10dee0.PLT.xml
                DcaPrefixName = "GetDF_Web8332_8333_FaultProceduralDetails_Out.",

                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false
            });

            //<--- Yuval Chalup 23.12.2014 TASK-9972
            //all.Add(new InterfaceManagementDetails() 
            //{
            //    Code = "8373",
            //    InOut = InOutEnum.O.ToString(),
            //    Description = "שאילתא לשחזור נתוני הצהרה",
            //    DcaPrefixName = "",
            //    DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
            //    DefaultPriority = CONSTDefaultPriority,
            //    AllowRestore = true,
            //    // NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
            //    Active = true,
            //    SendAsDual = false,
            //    ResponseInterfaceCode = "2754",
            //   // NeedSignature = false,
            //});
            //Yuval Chalup 23.12.2014 TASK-9972 --->

            //all.Add(new InterfaceManagementDetails()
            //{
            //    Code = "8332",
            //    InOut = InOutEnum.O.ToString(),
            //    Description = "שאילתא לליקויים",
            //    DcaPrefixName = "",
            //    DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
            //    DefaultPriority = CONSTDefaultPriority,
            //    AllowRestore = true,
            //    //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
            //    Active = true,
            //    SendAsDual = false,
            //    ResponseInterfaceCode = "8333",
            // //   NeedSignature = false,
            //});

            // all.Add(new InterfaceManagementDetails()
            // {
            //     Code = "8333",
            //     InOut = InOutEnum.I.ToString(),
            //     Description = "משוב לשאילתא לליקויים",
            //     DcaPrefixName = "GetDF_Web8332_8333_FaultProceduralDetails",
            //     DefaultSendOptionsCode = null,
            //     DefaultPriority = CONSTDefaultPriority,
            //     AllowRestore = true,
            //     //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
            //     Active = true,
            //     SendAsDual = false,
            //     ResponseInterfaceCode = null,
            ////     NeedSignature = false
            // });

            // moran 8.1.15 - Task 10004 -->

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8302",
                InOut = InOutEnum.O.ToString(),
                Description = "בקשה לטופס הצהרה",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8303",
                //   NeedSignature = false,
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8303",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לטופס הצהרה",
                DcaPrefixName = "GetDF_WEB8302_8303_DeclarationPrint_Request_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //   NeedSignature = false
            });

            // moran 8.1.15 - Task 10004 <--

            // moran 25.1.15 - Task 9967 -->
            //DCAInLP_NG_8400_MSG01_LogisticPermitMessageMessagingService
            all.Add(new InterfaceManagementDetails()
            {
                Code = "8400",
                InOut = InOutEnum.I.ToString(),
                Description = "היתר לוגיסטי",
                DcaPrefixName = "SendLP_NG_8400_MSG01_LogisticPermitMessage_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //   NeedSignature = false
            });
            // moran 25.1.15 - Task 9967 <--

            all.Add(new InterfaceManagementDetails()
            {
                Code = "0102",
                DcaPrefixName = "",
                InOut = InOutEnum.O.ToString(),
                Description = "שאילתא להודעות בוקר",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "0122",
                //    NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "0122",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לשאילתא להודעות בוקר",
                //GetMM_Web01_02_MorningMessagesList_Out.IL941079089.2016-06-28_14-36-19-040.37dc49b0-8c5e-4241-9fe7-b76f1ec5bb23.PLT
                DcaPrefixName = "GetMM_Web01_02_MorningMessagesList_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //    NeedSignature = false
            });

            //all.Add(new InterfaceManagementDetails()
            //{
            //    Code = "1170",
            //    InOut = InOutEnum.O.ToString(),
            //    Description = "מסר מניפסט",
            //    DcaPrefixName = "",
            //    DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
            //    DefaultPriority = CONSTDefaultPriority,
            //    AllowRestore = true,
            //    //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
            //    Active = true,
            //    SendAsDual = false,
            //    ResponseInterfaceCode = "1171",
            //    //    NeedSignature = false,
            //});

            //all.Add(new InterfaceManagementDetails()
            //{
            //    Code = "1171",
            //    InOut = InOutEnum.I.ToString(),
            //    Description = "משוב למסר מניפסט",
            //    DcaPrefixName = "",
            //    DefaultSendOptionsCode = null,
            //    DefaultPriority = CONSTDefaultPriority,
            //    AllowRestore = true,
            //    //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
            //    Active = true,
            //    SendAsDual = false,
            //    ResponseInterfaceCode = null,
            //    //   NeedSignature = false
            //});

            //DCAInDF_5018_InportExportDeclarationCancellationMessagingServices
            all.Add(new InterfaceManagementDetails()
            {
                Code = "5018",
                InOut = InOutEnum.I.ToString(),
                Description = "תיקון / ביטול הצהרה",
                DcaPrefixName = "SendDF_MSG5018_ImportDeclarationCancellationReplyMsg_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //    NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "2690",
                InOut = InOutEnum.O.ToString(),
                Description = "מסר הקמת רכב",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "2691",
                //    NeedSignature = false,
                SignatureTypeCode = "C"
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "2691",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב למסר הקמת רכב",
                DcaPrefixName = "GetVP_MSG2690_2691_VehicleIn_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //    NeedSignature = false
            });

            //MN_NG_9020_MasterBOLQueryMessagingService
            all.Add(new InterfaceManagementDetails()
            {
                Code = "9020",
                InOut = InOutEnum.O.ToString(),
                Description = "שאילתא לשטרי מטען",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //   NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "9021",
                //   NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "9021",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לשאילתא לשטרי מטען",
                //GetMN_MSG9020_9021_MasterBOLQuery_Message_Out.IL941079089.2016-06-26_17-04-39-109.f058f720-e541-494a-8d14-aea78d1c812b.PLT
                DcaPrefixName = "GetMN_MSG9020_9021_MasterBOLQuery_Message_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //    NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8304",
                InOut = InOutEnum.O.ToString(),
                Description = "שאילתא לגרעונות",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8246",
                //    NeedSignature = false,
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8246",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לשאילתא לגרעונות",
                //SaveNG_Web_8304_8246_DeficitFileFilterParam_Out.IL941079089.2016-06-26_17-05-51-361.eef07e20-4c18-428b-8637-9a7ecdc20d82.PLT
                DcaPrefixName = "SaveNG_Web_8304_8246_DeficitFileFilterParam_Out.", // GetTPG_NG_8246_Web04_DeficitFilesDetail
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //    NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8368",
                InOut = InOutEnum.O.ToString(),
                Description = "שאילתא בקשה לדוח קופה",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8356",
                //   NeedSignature = false,
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8356",
                InOut = InOutEnum.I.ToString(),
                Description = "דוח קופה לסוכן",
                DcaPrefixName = "SendTSH_MSG8356_MasavPaymentsToAgent_Out.",
                //               GetTSH_MSG8368_8356_MasavPaymentsToAgent_Out.IL941079089.2016-06-26_17-05-42-369.6336820c-a2d6-48f5-8b9d-4e3dcda26d8e.PLT
                DcaPrefixName2 = "GetTSH_MSG8368_8356_MasavPaymentsToAgent_Out.",

                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //     NeedSignature = false
            });

            //<--- Yuval Chalup 22.06.2015 TASK-13278
            all.Add(new InterfaceManagementDetails()
            {
                Code = "8289",
                InOut = InOutEnum.O.ToString(),
                Description = "שאילתא לתקרת אשראי",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8290",
                //          NeedSignature = false,
            });
            all.Add(new InterfaceManagementDetails()
            {
                Code = "2892",
                InOut = InOutEnum.O.ToString(),
                Description = "מסר תקן/בטל",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "2892",
                //   NeedSignature = false,
                //     SignatureTypeCode = "C"
            });
            all.Add(new InterfaceManagementDetails()
            {
                Code = "8235",
                InOut = InOutEnum.O.ToString(),
                Description = "מסר תקן/בטל יצוא",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = 5,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8237",
                //   NeedSignature = false,
                //     SignatureTypeCode = "C"
            });
            all.Add(new InterfaceManagementDetails()
            {
                Code = "5002",
                InOut = InOutEnum.O.ToString(),
                Description = "ביטול הצהרה",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "5002",
                //   NeedSignature = false,
                //     SignatureTypeCode = "C"
            });
            all.Add(new InterfaceManagementDetails()
            {
                Code = "8290",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לשאילתא לתקרת אשראי",
                //GetTSH_WEB8289_8290_CreditQuery_Out.IL941079089.2016-06-15_11-01-19-703.0166c542-a5ee-412a-b083-29318b71ab36.PLT
                DcaPrefixName = "GetTSH_WEB8289_8290_CreditQuery_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //       NeedSignature = false
            });
            //Yuval Chalup 22.06.2015 TASK-13278 --->

            //<--- Yuval Chalup 25.06.2015 TASK-8907
            all.Add(new InterfaceManagementDetails()
            {
                Code = "40",
                InOut = InOutEnum.O.ToString(),
                Description = "בקשה לפעולות מיוחדות",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "40I",
                //     NeedSignature = false,
            });



            all.Add(new InterfaceManagementDetails()
            {
                Code = "40I",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לבקשה לפעולות מיוחדות",
                ///SaveST_MSG40_SpecialActivityRequestMessage_Out.IL941079089.2016-06-26_17-06-05-400.d27e4d9a-0870-49a6-9a78-e136946c6f40.PLT.xml
                DcaPrefixName = "SaveST_MSG40_SpecialActivityRequestMessage_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //NeedSignature = false
            });
            //Yuval Chalup 25.06.2015 TASK-8907 --->

            // moran 17.6.15 - Task 13281 -->
            //TSH_NG_8285_Web01_PaymentFilterParam_MessagingService
            all.Add(new InterfaceManagementDetails()
            {
                Code = "8285",
                InOut = InOutEnum.O.ToString(),
                Description = "שאילתא להוראות תשלום",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //   NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8286",
                //NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8286",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לשאילתא להוראות תשלום",
                //GetTSH_WEB8285_8286_PaymentFilterParam_Out.IL941079089.2016-06-26_17-05-45-052.732bd801-77b2-4d81-bb0a-64fa2324a67b.PLT
                DcaPrefixName = "GetTSH_WEB8285_8286_PaymentFilterParam_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //NeedSignature = false
            });
            // moran 17.6.15 - Task 13281 <--

            // moran 5.7.15 - Task 13442 -->
            all.Add(new InterfaceManagementDetails()
            {
                Code = "8305",
                InOut = InOutEnum.O.ToString(),
                Description = "שאילתא לערבויות",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8247",
                //NeedSignature = false,
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8247",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לשאילתא לערבויות",
                DcaPrefixName = "",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //NeedSignature = false
            });
            // moran 5.7.15 - Task 13442 <--

            //<--- Yuval Chalup 07.09.2015 TASK-15037
            all.Add(new InterfaceManagementDetails()
            {
                Code = "8307",
                InOut = InOutEnum.O.ToString(),
                Description = @"שאילתא לנתוני תפ""ג להצהרה",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8249",
                //  NeedSignature = false,
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8249",
                InOut = InOutEnum.I.ToString(),
                Description = @"משוב לשאילתא לנתוני תפ""ג להצהרה",
                //GetTPG_Web8307_8249_DeclarationFilterParam_Out.IL941079089.2016-06-27_15-29-53-574.013c3b8c-c5cb-48cb-b0ca-072fe37ccc2d.PLT
                DcaPrefixName = "GetTPG_Web8307_8249_DeclarationFilterParam_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //   NeedSignature = false
            });
            //Yuval Chalup 07.09.2015 TASK-15037 --->
            //<--- Yuval Chalup 07.09.2015 TASK-15038
            all.Add(new InterfaceManagementDetails()
            {
                Code = "8306",
                InOut = InOutEnum.O.ToString(),
                Description = "שאילתא לנתוני כתב ערבות",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8248",
                //  NeedSignature = false,
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8248",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לשאילתא לנתוני כתב ערבות",
                //GetGRNT_MSG8306_8248_GuaranteeCertificateFilterParam_Out.IL941079089.2016-06-15_11-22-19-970.8be9b8b0-28c1-4ad1-8e1b-ea623c8da3bf.PLT
                DcaPrefixName = "GetGRNT_MSG8306_8248_GuaranteeCertificateFilterParam_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //   NeedSignature = false
            });
            //Yuval Chalup 07.09.2015 TASK-15038 --->

            //<--- Yuval Chalup 29.10.2015 TASK-16002
            all.Add(new InterfaceManagementDetails()
            {
                Code = "9022",
                InOut = InOutEnum.O.ToString(),
                Description = "שאילתא לשטרי מטען בלדר",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "9023",
                //          NeedSignature = false,
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "9023",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לשאילתא לשטרי מטען בלדר",
                ///GetMN_MSG9022_9023_CourierBOLQuery_Message_Out.IL941079089.2016-06-26_17-04-38-881.4e49ff7a-7cd0-45d6-a3ce-3a1ac583f945.PLT
                DcaPrefixName = "GetMN_MSG9022_9023_CourierBOLQuery_Message_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //       NeedSignature = false
            });
            //Yuval Chalup 29.10.2015 TASK-16002 --->

            //TPG_NG_8244_ClaimFileFilterParamMessagingService
            all.Add(new InterfaceManagementDetails()
            {
                Code = "8244",
                InOut = InOutEnum.O.ToString(),
                Description = "שאילתא לנתוני תביעה",
                DcaPrefixName = "",
                DefaultSendOptionsCode = null,// Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //  NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8245",
                //   NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8245",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לשאילתא לנתוני תביעה",
                ///SaveCLAIM_MSG_8244_8245_GetClaimEntityForInternetForm_Out.IL941079089.2016-06-26_17-05-53-161.a12d8583-edcc-40a5-a628-1dc79414f1f6.PLT
                DcaPrefixName = "SaveCLAIM_MSG_8244_8245_GetClaimEntityForInternetForm_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false
            });



            all.Add(new InterfaceManagementDetails()
            {
                Code = "UIT01",
                InOut = InOutEnum.I.ToString(),
                Description = "UnifreightTester01",
                DcaPrefixName = "UnifreightTester01_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,

                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                //  NeedSignature = false
            });


            all.Add(new InterfaceManagementDetails()
            {
                Code = "US2L01",
                InOut = InOutEnum.O.ToString(),
                Description = "Sivug Batch",
                //DcaPrefixName = "UnifreightTester01_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,

                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "US2L01I",
                //  NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "US2L01I",
                InOut = InOutEnum.I.ToString(),
                Description = "מסר סיווג פריטים",
                DcaPrefixName = "US2L01I_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,

                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                //  NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "UCUW2L",
                InOut = InOutEnum.I.ToString(),
                Description = "מסר פתיחת הצהרה ממסר אינטגרטור",
                DcaPrefixName = "UCUW2L_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,

                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                //  NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "UCUDO",
                InOut = InOutEnum.I.ToString(),
                Description = "עדכון כמות הצהרות פתוחות בטיסה",
                DcaPrefixName = "UCUDO_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,

                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                //  NeedSignature = false
            });


            all.Add(new InterfaceManagementDetails()
            {
                Code = "8316",
                InOut = InOutEnum.O.ToString(),
                Description = "משוב לשאילתא לדרישת חוקיות",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8316I",
            });

            //Get_CB_MSG_8316_CustomItemLegalDemands_Out.IL941079089.2016-06-26_17-05-46-951.8a3fafae-8dee-47ec-a33b-77e0f5f679b1.PLT
            all.Add(new InterfaceManagementDetails()
            {
                Code = "8316I",
                InOut = InOutEnum.I.ToString(),
                Description = "שאילתא לדרישת חוקיות",
                DcaPrefixName = "Get_CB_MSG_8316_CustomItemLegalDemands_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,

                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                //  NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8289Z",
                InOut = InOutEnum.O.ToString(),
                Description = "שאילתא לתקרת זהב",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "9060",
                SignatureTypeCode = "C"
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "9060",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לשאילתא לתקרת זהב",
                DcaPrefixName = "TSH_Web07_RTGSInfo_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //       NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "9070",
                InOut = InOutEnum.O.ToString(),
                Description = "שאילתא להצהרת יצוא",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "9071",
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "9071",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לשאילתא להצהרת יצוא",
                DcaPrefixName = "DF_MSG_9071_ExportDeclarationDataResponse_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "UCTZIP",
                InOut = InOutEnum.I.ToString(),
                Description = "Unifreight Table Custom ZIP",
                DcaPrefixName = "UnifreightCustomTableZIP_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,

                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                //  NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "1170",
                InOut = InOutEnum.O.ToString(),
                Description = "מסר מניפסט",
                //odi "מסר מצהר",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "1171",
                InterfaceType = "B",
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "1171",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב למסר מצהר",
                DcaPrefixName = "SaveMN_MSG1170_1171_MANIFESTRequest_Out.",
                DcaPrefixName2 = "SendMN_MSG1171_SendManifestFeedBack_Message_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                InterfaceType = "B",
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8343",
                InOut = InOutEnum.O.ToString(),
                Description = "שאילתא לנתונים נוספים ליבואן",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "8344",
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "8344",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לשאילתא לנתונים נוספים ליבואן",
                DcaPrefixName = "CL_NG_8344_Web02_ClientSearchByIDDetail_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "20",
                InOut = InOutEnum.O.ToString(),
                Description = "מסר קליטה באתר",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
            });

            //DCAInCLAIM_5114_AcceptanceOrRejectionClaimMessageMessagingServices
            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "5114",
                InOut = InOutEnum.I.ToString(),
                Description = "אישור/דחיה תביעה",
                DcaPrefixName = "SendCLAIM_MSG5114_AcceptanceOrRejectionClaimMessage_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //   NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                // NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "1030",
                InOut = InOutEnum.O.ToString(),
                Description = "בקשה/ביטול להעברת טובין",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "1035",
                InterfaceType = "B",
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "1035",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לבקשת העברה",
                DcaPrefixName = "SendGP_MSG1035_GatepassFeedbackMessage_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                InterfaceType = "B",
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "UCB1170",

                ///DCAInUniCourierBatchSend_MsgMessagingService
                InOut = InOutEnum.I.ToString(),
                Description = "שידור מצהר בלדר",// "Unifreight Courier *1170* Batch Send",
                DcaPrefixName = "UnifreightCourier_UCB1170_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,

                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                InterfaceType = "B",
                //  NeedSignature = false
            });


            all.Add(new InterfaceManagementDetails()
            {
                Code = "UCB2750",

                ///DCAInUniCourierBatchSend_MsgMessagingService
                InOut = InOutEnum.I.ToString(),
                Description = "שידור הצהרות בלדר ",// "Unifreight Courier *2750* Batch Send",
                DcaPrefixName = "UnifreightCourier_UCB2750_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,

                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                //  NeedSignature = false
                InterfaceType = "B",
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "UCB8250",
                InOut = InOutEnum.I.ToString(),
                Description = "שידור סטטוס הצהרות לבלדר",
                DcaPrefixName = "UnifreightCourier_UCB8250_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                InterfaceType = "B",
            });


            all.Add(new InterfaceManagementDetails()
            {
                Code = "UCB2755",

                ///DCAInUniCourierBatchSend_MsgMessagingService
                InOut = InOutEnum.I.ToString(),
                Description = "שידור הגשה בלדר ",// "Unifreight Courier *2750* Batch Send",
                DcaPrefixName = "UnifreightCourier_UCB2755_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,

                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                InterfaceType = "B",
                //  NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "UCB8212",

                ///DCAInUniCourierBatchSend_MsgMessagingService
                InOut = InOutEnum.I.ToString(),
                Description = "שידור מענה מרוכז ",// "Unifreight Courier *2750* Batch Send",
                DcaPrefixName = "UnifreightCustoms_UCB8212_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,

                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
            //    InterfaceType = "C",
                //  NeedSignature = false

            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "UCB9999",

                ///DCAInUniCourierBatchSend_MsgMessagingService
                InOut = InOutEnum.I.ToString(),
                Description = "ניתוח מחדש",// "Unifreight Courier *2750* Batch Send",
                DcaPrefixName = "UnifreightCustoms_UCB9999_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,

                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                //    InterfaceType = "C",
                //  NeedSignature = false

            });
            all.Add(new InterfaceManagementDetails()
            {
                Code = "UCB2715",

                ///DCAInUniCourierBatchSend_MsgMessagingService
                InOut = InOutEnum.I.ToString(),
                Description = "שידור מסמכים שגויים ",// "Unifreight Courier *2715* Batch Send",
                DcaPrefixName = "UnifreightCourier_UCB2715_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                InterfaceType = "B",
                //  NeedSignature = false
            });
            all.Add(new InterfaceManagementDetails()
            {
                Code = "UCB2715SendNow",

                ///DCAInUniCourierBatchSend_MsgMessagingService
                InOut = InOutEnum.I.ToString(),
                Description = "שידור מסמכים ממתינים בתור ",// "Unifreight Courier *2715* Batch Send",
                DcaPrefixName = "UnifreightCourier_UCB2715SendNow_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                InterfaceType = "B",
                //  NeedSignature = false
            });
            all.Add(new InterfaceManagementDetails()
            {
                //TML ==>https://www.abbreviations.com/abbreviation/terminal
                Code = "UCBCTML",

                ///DCAInUniCourierBatchSend_MsgMessagingService
                InOut = InOutEnum.I.ToString(),
                Description = "שידור הגשה בלדר ",// "Unifreight Courier *UCBCTML* Batch Send",
                DcaPrefixName = "UnifreightCourierBatchTerminal_UCBCTML_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,

                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                //  NeedSignature = false
                InterfaceType = "B",
            });

            all.Add(new InterfaceManagementDetails()
            {
                //TML ==>https://www.abbreviations.com/abbreviation/terminal
                Code = "UCBCMSS",

                ///DCAInUniCourierBatchSend_MsgMessagingService
                InOut = InOutEnum.I.ToString(),
                Description = "שינוי אתר איחסון לבלדר ",// "Unifreight Courier *UCBCTML* Batch Send",
                DcaPrefixName = "UnifreightCourierBatchTerminal_UCBCMSS_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,

                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                //  NeedSignature = false
                InterfaceType = "B",
            });
            all.Add(new InterfaceManagementDetails()
            {
                //TML ==>https://www.abbreviations.com/abbreviation/terminal
                Code = "ClosePending",

                ///DCAInUniCourierBatchSend_MsgMessagingService
                InOut = InOutEnum.I.ToString(),
                Description = "סגירה גורפת ל-Pending",
                DcaPrefixName = "UnifreightCourierBatchTerminal_ClosePending_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,

                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                //  NeedSignature = false
                InterfaceType = "B",
            });
            all.Add(new InterfaceManagementDetails()
            {
                //TML ==>https://www.abbreviations.com/abbreviation/terminal
                Code = "DCAMU",

                ///DCAInUniCourierBatchSend_MsgMessagingService
                InOut = InOutEnum.I.ToString(),
                Description = "עדכון קוד תהליך/הנחה פטור",
                DcaPrefixName = "DCAMU_OUT",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,

                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                //  NeedSignature = false
                InterfaceType = "C",
            });
            all.Add(new InterfaceManagementDetails()
            {
                Code = "DCAUAC",
                InOut = InOutEnum.I.ToString(),
                Description = "עדכון פטור 92 גורף",
                DcaPrefixName = "DCAUAC_OUT",
                DefaultSendOptionsCode = null,
                DefaultPriority = 5,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                InterfaceType = "C",
            });            
            all.Add(new InterfaceManagementDetails()
            {
                Code = "UCADPE",
                InOut = InOutEnum.I.ToString(),
                Description = "נה גורפת PENDING",
                DcaPrefixName = "UCADPE_OUT",
                DefaultSendOptionsCode = null,
                DefaultPriority = 5,
                AllowRestore = true,
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                InterfaceType = "B",
            });
            //CLAIM_5005_ContinuousRequestOnClaimFileMessagingService
            all.Add(new InterfaceManagementDetails()
            {
                Code = "5005",
                InOut = InOutEnum.O.ToString(),
                Description = "מסר בקשה לביטול/ערר תביעה",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //   NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "5013",
                //      NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "5013",
                InOut = InOutEnum.I.ToString(),
                Description = "משוב לביטול/ערר תביעה",
                DcaPrefixName = "GetCLAIM_MSG9_ContinuousRequestOnClaimFile_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //NotificationId = "",
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //    NeedSignature = false
            });

            ///DCAInDE_NG_5108_DecisionMessageMessagingService
            all.Add(
            new InterfaceManagementDetails()
            {
                Code = "5108",
                InOut = InOutEnum.I.ToString(),
                Description = "מסר החלטה בתיק גרעון",
                DcaPrefixName = "SendDE_MSG5108_DecisionMessage_Out.",
                DefaultSendOptionsCode = null,// Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //   NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //  NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                //TML ==>https://www.abbreviations.com/abbreviation/terminal
                Code = "UCBUD2LT",

                ///DCAInUniCourierBatchSend_MsgMessagingService
                InOut = InOutEnum.I.ToString(),
                Description = "קישור מסמך לטיקט",// "Unifreight Courier *UCBCTML* Batch Send",
                DcaPrefixName = "UnifreightCourierBatchTerminal_UCBUD2LT_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,

                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                //  NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "6001",
                InOut = InOutEnum.O.ToString(),
                Description = "עדכון סגרים",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                // NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
                //    NeedSignature = false,
            });




            all.Add(new InterfaceManagementDetails()
            {
                //TML ==>https://www.abbreviations.com/abbreviation/terminal
                Code = "UCBNDCD",

                ///DCAInUniCourierBatchSend_MsgMessagingService
                InOut = InOutEnum.I.ToString(),
                Description = "העלאת מסמך מבונדד",
                DcaPrefixName = "UnifreightCustomBatch_UCBNDCD_Out.",
                DefaultSendOptionsCode = null,
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,

                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "",
                //  NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "2751",
                InOut = InOutEnum.O.ToString(),
                Description = "הצהרת יצוא",
                DcaPrefixName = "SaveDF_MSG2751_ExportDeclaration_Out",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                 Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "2757",
             });



            all.Add(new InterfaceManagementDetails()
            {
                Code = "2450",
                InOut = InOutEnum.O.ToString(),
                Description = "מסר המכלה",
                DcaPrefixName = "",
                DefaultSendOptionsCode = Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI.ToString(),
                DefaultPriority = CONST_DefaultPriority,
                AllowRestore = true,
                //   NotificationId = "",//NotificationDefinitionDetails.GetAll().FirstOrDefault(rec=> rec.Code="").Code
                Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = "2451",
                //      NeedSignature = false
            });

            all.Add(new InterfaceManagementDetails()
            {
                Code = "2757",
                InOut = InOutEnum.I.ToString(),
                Description = "מסר תשובה הצהרה יצוא",
                DefaultSendOptionsCode = null,

                DefaultPriority = CONST_DefaultPriority,
                DcaPrefixName = "SaveDF_MSG2751_ExportDeclaration_Out.",

                AllowRestore = true,
                 Active = true,
                SendAsDual = false,
                ResponseInterfaceCode = null,
             });
            //C:\LogitudeWorld\Amital\Logitude.Customs.BL\EntityPMs\InterfaceManagementPM.cs
            var pm = new Logitude.Customs.Def.EntityPMs.InterfaceManagementPM();
            var myRequestCode = pm.ResponseInterfaceCode;
            if (LogitudeSettings.IsCostomsDeploy)
            {
                //MessagingServiceFactoryHelper.InitContainer();
                //TableLastUpdateClass.UpdateTableHistory(0, "Customs.CustomsRequestsSheet", new TableLastUpdateM()
                //{
                //    //ObjectTableId = item.Id,
                //    //AlternativeUserTenant = 0,
                //    AlternativeUserId = "1-1" ///in oracle  //"admin@fnarsoft.com"=1-1

                //});
            }
            all.ForEach(rec => rec.SearchFields = GetSearchFields(rec));
            return all;
        }

        public void MapPoco(InterfaceManagement newPoco)
        {
            newPoco.Code = this.Code;
            newPoco.InOut = this.InOut;
            newPoco.Description = this.Description;
            newPoco.DcaPrefixName = this.DcaPrefixName;
            newPoco.DcaPrefixName2 = this.DcaPrefixName2;
            newPoco.DcaPrefixName3 = this.DcaPrefixName3;
            newPoco.DcaPrefixName4 = this.DcaPrefixName4;

            newPoco.DefaultSendOptionsCode = this.DefaultSendOptionsCode;
            newPoco.DefaultPriority = this.DefaultPriority;

            newPoco.AllowRestore = this.AllowRestore;
            newPoco.Active = this.Active;
            newPoco.SearchFields = GetSearchFields(this);
            newPoco.SendAsDual = this.SendAsDual;
            newPoco.ResponseInterfaceCode = this.ResponseInterfaceCode;
            newPoco.SignatureTypeCode = this.SignatureTypeCode;
            newPoco.InterfaceType = this.InterfaceType;
            //   newPoco.NeedSignature = this.NeedSignature;
        }




        public string GetSearchFields(InterfaceManagement rec)
        {
            return String.Concat(rec.Code, ",", rec.Description, ",", rec.DcaPrefixName).ToLower();
        }
    }
}
