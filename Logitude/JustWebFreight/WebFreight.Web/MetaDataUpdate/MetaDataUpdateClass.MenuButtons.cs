using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.InfrastructureModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.MetaDataUpdate.AddClasses;
using WebFreight.Web.MetaDataUpdate.DetailClasses;

namespace WebFreight.Web.MetaDataUpdate
{
    public partial class MetaDataUpdateClass
    {
        public List<MenuButtonGroupPM> CreateMenuButtonsForTenant(int tenant)
        {
            FeatureQuery featureQuery = new FeatureQuery();
            List<FeaturePM> features = featureQuery.GetFeaturePMsByTenant(tenant).ToList();

            Dictionary<string, TextCode> TextCodes = TextCodeRepository.GetTextCodesByTenant(tenant).Where(d => d.TextCodeTypeCode == "B").GroupBy(d => d.Code).ToDictionary(g => g.Key, a => a.FirstOrDefault());
            Dictionary<string, MenuButton> TenantMenuButtons = MenuButtonRepository.GetMenuButtonsByTenant(tenant).GroupBy(d => d.EventCode + d.MenuButtonGroupId).ToDictionary(g => g.Key, a => a.FirstOrDefault());
            Dictionary<string, MenuButtonGroup> TenantMenuButtonGroups = MenuButtonGroupRepository.GetMenuButtonGroupsByTenant(tenant).GroupBy(d => d.Name).ToDictionary(g => g.Key, a => a.FirstOrDefault());

            string ShipmentTableId = ObjectContext.ObjectTables.Where(f => f.Name == "Shipment" && f.Tenant == tenant).FirstOrDefault().Id;
            string MasterTableId = ObjectContext.ObjectTables.Where(f => f.Name == "Master" && f.Tenant == tenant).FirstOrDefault().Id;
            string QuoteTableId = ObjectContext.ObjectTables.Where(f => f.Name == "Quote" && f.Tenant == tenant).FirstOrDefault().Id;
            string UserTableId = ObjectContext.ObjectTables.Where(f => f.Name == "User" && f.Tenant == tenant).FirstOrDefault().Id;
            string ARInvoiceTableId = ObjectContext.ObjectTables.Where(f => f.Name == "ARInvoice" && f.Tenant == tenant).FirstOrDefault().Id;
            string APInvoiceTableId = ObjectContext.ObjectTables.Where(f => f.Name == "APInvoice" && f.Tenant == tenant).FirstOrDefault().Id;
            string ARPaymentTableId = ObjectContext.ObjectTables.Where(f => f.Name == "ARPayment" && f.Tenant == tenant).FirstOrDefault().Id;
            string APPaymentTableId = ObjectContext.ObjectTables.Where(f => f.Name == "APPayment" && f.Tenant == tenant).FirstOrDefault().Id;
            string CommLogTableId = ObjectContext.ObjectTables.Where(f => f.Name == "CommunicationLog" && f.Tenant == tenant).FirstOrDefault().Id;
            string CustomerTableId = ObjectContext.ObjectTables.Where(f => f.Name == "Customer" && f.Tenant == tenant).FirstOrDefault().Id;
            string MessagingStockTableId = ObjectContext.ObjectTables.Where(f => f.Name == "MessagingStock" && f.Tenant == tenant).FirstOrDefault().Id;
            string TenantManagementTableId = ObjectContext.ObjectTables.Where(d => d.Name == "TenantManagement" && d.Tenant == tenant).FirstOrDefault().Id;
            string ContactTableId = ObjectContext.ObjectTables.Where(f => f.Name == "User" && f.Tenant == tenant).FirstOrDefault().Id;
            #region Features

            FeaturePM ShipmentFeature_AccountingClose = features.Where(d => d.Code == "ACCOUNTINGCLOSE" && d.ObjectTableId == ShipmentTableId).FirstOrDefault();
            FeaturePM ShipmentFeature_OperationalClose = features.Where(d => d.Code == "OPERATIONALCLOSE" && d.ObjectTableId == ShipmentTableId).FirstOrDefault();
            FeaturePM ShipmentFeature_OperationalReopen = features.Where(d => d.Code == "OPERATIONALREOPEN" && d.ObjectTableId == ShipmentTableId).FirstOrDefault();
            FeaturePM ShipmentFeature_Reactivate = features.Where(d => d.Code == "REACTIVATE" && d.ObjectTableId == ShipmentTableId).FirstOrDefault();
            FeaturePM ShipmentFeature_ExceptionResolved = features.Where(d => d.Code == "EXCEPTIONRESOLVED" && d.ObjectTableId == ShipmentTableId).FirstOrDefault();
            FeaturePM ShipmentFeature_ConvertToCustomFile = features.Where(d => d.Code == "CONVERTTOCUSTOMFILE" && d.ObjectTableId == ShipmentTableId).FirstOrDefault();           
            FeaturePM ShipmentFeature_Cancel = features.Where(d => d.Code == "CANCEL" && d.ObjectTableId == ShipmentTableId).FirstOrDefault();
            FeaturePM ShipmentFeature_AccountingReopen = features.Where(d => d.Code == "ACCOUNTINGREOPEN" && d.ObjectTableId == ShipmentTableId).FirstOrDefault();
            FeaturePM ShipmentFeature_ConvertHouseToDirect = features.Where(d => d.Code == "CONVERTHOUSETODIRECT" && d.ObjectTableId == ShipmentTableId).FirstOrDefault();
            FeaturePM ShipmentFeature_ConvertDirectToHouse = features.Where(d => d.Code == "CONVERTDIRECTTOHOUSE" && d.ObjectTableId == ShipmentTableId).FirstOrDefault();
            FeaturePM ShipmentFeature_Copy = features.Where(d => d.Code == "COPY" && d.ObjectTableId == ShipmentTableId).FirstOrDefault();
            FeaturePM ShipmentFeature_Sendrequest = features.Where(d => d.Code == "SENDREQUEST" && d.ObjectTableId == ShipmentTableId).FirstOrDefault();
            FeaturePM ShipmentFeature_SendResponse = features.Where(d => d.Code == "SENDRESPONSE" && d.ObjectTableId == ShipmentTableId).FirstOrDefault();
            FeaturePM ShipmentFeature_SendToCustoms = features.Where(d => d.Code == "SendToCustoms" && d.ObjectTableId == ShipmentTableId).FirstOrDefault();
            FeaturePM ShipmentFeature_SendToArtemus = features.Where(d => d.Code == "SendToArtemus" && d.ObjectTableId == ShipmentTableId).FirstOrDefault();
            FeaturePM ShipmentFeature_CustomsTransmission = features.Where(d => d.Code == "ShipmentCustomsTransmission" && d.ObjectTableId == ShipmentTableId).FirstOrDefault();
            FeaturePM ShipmentFeature_SplitShipment = features.Where(d => d.Code == "SplitShipment" && d.ObjectTableId == ShipmentTableId).FirstOrDefault();

            FeaturePM MasterFeature_AccountingClose = features.Where(d => d.Code == "ACCOUNTINGCLOSE" && d.ObjectTableId == MasterTableId).FirstOrDefault();
            FeaturePM MasterFeature_OperationalClose = features.Where(d => d.Code == "OPERATIONALCLOSE" && d.ObjectTableId == MasterTableId).FirstOrDefault();
            FeaturePM MasterFeature_OperationalReopen = features.Where(d => d.Code == "OPERATIONALREOPEN" && d.ObjectTableId == MasterTableId).FirstOrDefault();
            FeaturePM MasterFeature_Reactivate = features.Where(d => d.Code == "REACTIVATE" && d.ObjectTableId == MasterTableId).FirstOrDefault();
            FeaturePM MasterFeature_Cancel = features.Where(d => d.Code == "CANCEL" && d.ObjectTableId == MasterTableId).FirstOrDefault();
            FeaturePM MasterFeature_AccountingReopen = features.Where(d => d.Code == "ACCOUNTINGREOPEN" && d.ObjectTableId == MasterTableId).FirstOrDefault();
            FeaturePM MasterFeature_Copy = features.Where(d => d.Code == "COPY" && d.ObjectTableId == MasterTableId).FirstOrDefault();
            FeaturePM MasterFeature_Sendrequest = features.Where(d => d.Code == "SENDREQUEST" && d.ObjectTableId == MasterTableId).FirstOrDefault();
            FeaturePM MasterFeature_SendResponse = features.Where(d => d.Code == "SENDRESPONSE" && d.ObjectTableId == MasterTableId).FirstOrDefault();

            FeaturePM QuoteFeature_SetAsSent = features.Where(d => d.Code == "SETASSENT" && d.ObjectTableId == QuoteTableId).FirstOrDefault();
            FeaturePM QuoteFeature_ReturnToDraft = features.Where(d => d.Code == "RETURNTODRAFT" && d.ObjectTableId == QuoteTableId).FirstOrDefault();
            FeaturePM QuoteFeature_Copy = features.Where(d => d.Code == "COPY" && d.ObjectTableId == QuoteTableId).FirstOrDefault();
            FeaturePM QuoteFeature_Cancel = features.Where(d => d.Code == "CANCEL" && d.ObjectTableId == QuoteTableId).FirstOrDefault();
            FeaturePM QuoteFeature_Reactivate = features.Where(d => d.Code == "REACTIVATE" && d.ObjectTableId == QuoteTableId).FirstOrDefault();
            FeaturePM QuoteFeature_BuildShipment = features.Where(d => d.Code == "BUILDSHIPMENT" && d.ObjectTableId == QuoteTableId).FirstOrDefault();
            FeaturePM QuoteFeature_Accepted = features.Where(d => d.Code == "QUOTEACCEPTED" && d.ObjectTableId == QuoteTableId).FirstOrDefault();
            FeaturePM QuoteFeature_Declined = features.Where(d => d.Code == "QUOTEDECLINED" && d.ObjectTableId == QuoteTableId).FirstOrDefault();
            FeaturePM QuoteFeature_Quotation = features.Where(d => d.Code == "QUOTEQUOTATION" && d.ObjectTableId == QuoteTableId).FirstOrDefault();
            
            FeaturePM UserFeature_ResetPassword = features.Where(d => d.Code == "RESETPASSWORD" && d.ObjectTableId == UserTableId).FirstOrDefault();
            FeaturePM UserFeature_Anonymize = features.Where(d => d.Code == "ANONYMIZE" && d.ObjectTableId == UserTableId).FirstOrDefault();

            FeaturePM ContactFeature_Anonymize = features.Where(d => d.Code == "ANONYMIZE" && d.ObjectTableId == ContactTableId).FirstOrDefault();


            FeaturePM ARInvoiceFeature_CancelDraft = features.Where(d => d.Code == "CancelDraft" && d.ObjectTableId == ARInvoiceTableId).FirstOrDefault();
            FeaturePM ARInvoiceFeature_AutoCredit = features.Where(d => d.Code == "AUTOCREDIT" && d.ObjectTableId == ARInvoiceTableId).FirstOrDefault();
            FeaturePM ARInvoiceFeature_SetAsSent = features.Where(d => d.Code == "SETASSENT" && d.ObjectTableId == ARInvoiceTableId).FirstOrDefault();
            FeaturePM ARInvoiceFeature_Void = features.Where(d => d.Code == "VOID" && d.ObjectTableId == ARInvoiceTableId).FirstOrDefault();
            FeaturePM ARInvoiceFeature_SetAsDraft = features.Where(d => d.Code == "SETASDRAFT" && d.ObjectTableId == ARInvoiceTableId).FirstOrDefault();
            FeaturePM ARInvoiceFeature_SaveAndApprove = features.Where(d => d.Code == "SAVEANDAPPROVE" && d.ObjectTableId == ARInvoiceTableId).FirstOrDefault();
            FeaturePM ARInvoiceFeature_Print = features.Where(d => d.Code == "PRINT" && d.ObjectTableId == ARInvoiceTableId).FirstOrDefault();
            FeaturePM ARInvoiceFeature_ReTransfer = features.Where(d => d.Code == "EnableReTransfer" && d.ObjectTableId == ARInvoiceTableId).FirstOrDefault();
            FeaturePM ARInvoiceFeature_SendQBO = features.Where(d => d.Code == "SendToQBO" && d.ObjectTableId == ARInvoiceTableId).FirstOrDefault();

            FeaturePM APInvoiceFeature_CancelApproval = features.Where(d => d.Code == "CANCELAPPROVAL" && d.ObjectTableId == APInvoiceTableId).FirstOrDefault();
            FeaturePM APInvoiceFeature_Void = features.Where(d => d.Code == "VOID" && d.ObjectTableId == APInvoiceTableId).FirstOrDefault();
            FeaturePM APInvoiceFeature_Approve = features.Where(d => d.Code == "APPROVE" && d.ObjectTableId == APInvoiceTableId).FirstOrDefault();
            FeaturePM APInvoiceFeature_ReTransfer = features.Where(d => d.Code == "EnableReTransfer" && d.ObjectTableId == APInvoiceTableId).FirstOrDefault();
            FeaturePM APInvoiceFeature_Print = features.Where(d => d.Code == "PRINT" && d.ObjectTableId == APInvoiceTableId).FirstOrDefault();
            FeaturePM ARInvoiceFeature_CHECKSATStatus = features.Where(d => d.Code == "CHECKSATSTATUS" && d.ObjectTableId == ARInvoiceTableId).FirstOrDefault();
            FeaturePM APInvoiceFeature_SendQBO = features.Where(d => d.Code == "SendToQBO" && d.ObjectTableId == APInvoiceTableId).FirstOrDefault();

            FeaturePM ARPaymentFeature_CancelApproval = features.Where(d => d.Code == "CANCELAPPROVAL" && d.ObjectTableId == ARPaymentTableId).FirstOrDefault();
            FeaturePM ARPaymentFeature_Void = features.Where(d => d.Code == "VOID" && d.ObjectTableId == ARPaymentTableId).FirstOrDefault();
            FeaturePM ARPaymentFeature_Approve = features.Where(d => d.Code == "APPROVE" && d.ObjectTableId == ARPaymentTableId).FirstOrDefault();
            FeaturePM ARPaymentFeature_ReTransfer = features.Where(d => d.Code == "EnableReTransfer" && d.ObjectTableId == ARPaymentTableId).FirstOrDefault();
            FeaturePM ARPaymentFeature_Print = features.Where(d => d.Code == "PRINT" && d.ObjectTableId == ARPaymentTableId).FirstOrDefault();
            FeaturePM ARPaymentFeature_SendToSAT = features.Where(d => d.Code == "SENDToSAT" && d.ObjectTableId == ARPaymentTableId).FirstOrDefault();
            FeaturePM ARPaymentFeature_CHECKSATStatus = features.Where(d => d.Code == "CHECKSATSTATUS" && d.ObjectTableId == ARPaymentTableId).FirstOrDefault();
            FeaturePM ARPaymentFeature_SendToQBO = features.Where(d => d.Code == "SendToQBO" && d.ObjectTableId == ARPaymentTableId).FirstOrDefault();

            

            FeaturePM APPaymentFeature_CancelApproval = features.Where(d => d.Code == "CANCELAPPROVAL" && d.ObjectTableId == APPaymentTableId).FirstOrDefault();
            FeaturePM APPaymentFeature_Void = features.Where(d => d.Code == "VOID" && d.ObjectTableId == APPaymentTableId).FirstOrDefault();
            FeaturePM APPaymentFeature_Approve = features.Where(d => d.Code == "APPROVE" && d.ObjectTableId == APPaymentTableId).FirstOrDefault();
            FeaturePM APPaymentFeature_Print = features.Where(d => d.Code == "PRINT" && d.ObjectTableId == APPaymentTableId).FirstOrDefault();
            FeaturePM APPaymentFeature_SendToQBO = features.Where(d => d.Code == "SendToQBO" && d.ObjectTableId == APPaymentTableId).FirstOrDefault();

            FeaturePM LogFeature_Resend = features.Where(d => d.Code == "RESEND" && d.ObjectTableId == CommLogTableId).FirstOrDefault();
            FeaturePM LogFeature_Messages = features.Where(d => d.Code == "MESSAGES" && d.ObjectTableId == CommLogTableId).FirstOrDefault();

            FeaturePM CustomerFeature_Activate = features.Where(d => d.Code == "ACTIVATE" && d.ObjectTableId == CustomerTableId).FirstOrDefault();
            FeaturePM CustomerFeature_Ready = features.Where(d => d.Code == "READYACTIVATION" && d.ObjectTableId == CustomerTableId).FirstOrDefault();
            FeaturePM CustomerFeature_InActive = features.Where(d => d.Code == "INACTIVE" && d.ObjectTableId == CustomerTableId).FirstOrDefault();
            FeaturePM CustomerFeature_ReActivate = features.Where(d => d.Code == "REACTIVATE" && d.ObjectTableId == CustomerTableId).FirstOrDefault();
            FeaturePM CustomerFeature_MyCustomer = features.Where(d => d.Code == "MYCUSTOMER" && d.ObjectTableId == CustomerTableId).FirstOrDefault();
            FeaturePM CustomerFeature_NotMyCustomer = features.Where(d => d.Code == "NOTMYCUSTOMER" && d.ObjectTableId == CustomerTableId).FirstOrDefault();
            FeaturePM CustomerFeature_ViewQuestionnaireAnswerCustomer = features.Where(d => d.Code == "QUESTIONNAIREANSWERS" && d.ObjectTableId == CustomerTableId).FirstOrDefault();
            FeaturePM CustomerFeature_TenantManagement = features.Where(d => d.Code == "TENANTMANAGEMENT" && d.ObjectTableId == CustomerTableId).FirstOrDefault();
            FeaturePM CustomerFeature_Totango = features.Where(d => d.Code == "TOTANGO" && d.ObjectTableId == CustomerTableId).FirstOrDefault();
            FeaturePM CustomerFeature_SetAsPotential = features.Where(d => d.Code == "SETASPOTENTIAL" && d.ObjectTableId == CustomerTableId).FirstOrDefault();

            FeaturePM MessagingStockFeature_Cancel = features.Where(d => d.Code == "MessagingStock.Action.Cancel" && d.ObjectTableId == MessagingStockTableId).FirstOrDefault();

            FeaturePM TenantManagementFeature_EraseData = features.Where(d => d.Code == "TenantManagement.Action.EraseData" && d.ObjectTableId == TenantManagementTableId).FirstOrDefault();
            #endregion

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                #region testButtons
                ObjectTable ShippinglineObject = ObjectContext.ObjectTables.Where(d => d.Name == "Trucker" && d.Tenant == tenant).FirstOrDefault();
                MenuButtonGroup ShippingMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
                {

                    MenuButtonGroupType = "TruckerLineEdit",
                    Name = "TruckerLineEditButtonsGroup",
                    ObjectTableId = ShippinglineObject.Id,
                    Tenant = tenant,
                }, MenuButtonGroupRepository, TenantMenuButtonGroups);


                MenuButton shippinglineEventButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
                {

                    EventCode = "Trucker.ShowEvents",
                    Index = 0,
                    IsActive = true,
                    LabelTextCodeCode = "Test_Trucker.MenuButtons.EventButton",
                    LabelTextCodeDefaultText = "Events",
                    ObjectTableId = ShippinglineObject.Id,
                    Tenant = tenant,
                    MenuButtonGroupId = ShippingMenuButtonGroup.Id,

                }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
                #endregion
            }

            #region Shipment
            ObjectTable shipmentObject = ObjectContext.ObjectTables.Where(d => d.Name == "Shipment" && d.Tenant == tenant).FirstOrDefault();

            MenuButtonGroup ShipmentMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "ShipmentEdit",
                Name = "ShipmentEditButtonsGroup",
                ObjectTableId = shipmentObject.Id,
                Tenant = tenant,
            }, MenuButtonGroupRepository, TenantMenuButtonGroups);

            #region actions button
            MenuButton actionButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Actions",
                Index = 100,
                IsActive = false,
                LabelTextCodeCode = "Shipment.B.Actions",
                LabelTextCodeDefaultText = "More",
                Tenant = tenant,
                MenuButtonGroupId = ShipmentMenuButtonGroup.Id,
                ObjectTableId = shipmentObject.Id,
                MenuButtonType = "dropdownbutton",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region operational close
            MenuButton closeButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "OperationalCloseShipment",
                Index = 2,
                IsActive = true,
                LabelTextCodeCode = "Shipment.B.OperationalClose",
                LabelTextCodeDefaultText = "Operational Close Shipment",
                ObjectTableId = shipmentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ShipmentMenuButtonGroup.Id,
                ParentMenuButtonId = actionButton.Id,
                FeatureId = ShipmentFeature_OperationalClose.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region accounting close
            MenuButton AccountingCloseButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "AccountingCloseShipment",
                Index = 3,
                IsActive = true,
                LabelTextCodeCode = "Shipment.B.AccountingClose",
                LabelTextCodeDefaultText = "Accounting Close Shipment",
                ObjectTableId = shipmentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ShipmentMenuButtonGroup.Id,
                ParentMenuButtonId = actionButton.Id,
                FeatureId = ShipmentFeature_AccountingClose.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region operational reopen
            MenuButton OperationalReopenButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "OperationalReopenShipment",
                Index = 4,
                IsActive = true,
                LabelTextCodeCode = "Shipment.B.OperationalReopen",
                LabelTextCodeDefaultText = "Operational Reopen Shipment",
                ObjectTableId = shipmentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ShipmentMenuButtonGroup.Id,
                ParentMenuButtonId = actionButton.Id,
                FeatureId = ShipmentFeature_OperationalReopen.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region accounting reopen
            MenuButton AccountedReopenButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "AccountedReopenShipment",
                Index = 5,
                IsActive = true,
                LabelTextCodeCode = "Shipment.B.AccountedReopen",
                LabelTextCodeDefaultText = "Accounted Reopen Shipment",
                ObjectTableId = shipmentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ShipmentMenuButtonGroup.Id,
                ParentMenuButtonId = actionButton.Id,
                FeatureId = ShipmentFeature_AccountingReopen.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region separator
            MenuButton ShipmentOperationSeperator = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ShipmentOperationSeparator",
                Index = 6,
                IsActive = false,
                LabelTextCodeCode = "Shipment.B.ShipmentOperationSeparator",
                LabelTextCodeDefaultText = "",
                ObjectTableId = shipmentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ShipmentMenuButtonGroup.Id,
                ParentMenuButtonId = actionButton.Id,
                MenuButtonType = "separator",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region ConvertShipmentFromHouseToDirectButton
            MenuButton ConvertShipmentFromHouseToDirectButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ConvertShipmentFromHouseToDirect",
                Index = 7,
                IsActive = true,
                LabelTextCodeCode = "Shipment.B.ConvertShipmentFromHouseToDirect",
                LabelTextCodeDefaultText = "Convert From House To Direct",
                ObjectTableId = shipmentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ShipmentMenuButtonGroup.Id,
                ParentMenuButtonId = actionButton.Id,
                FeatureId = ShipmentFeature_ConvertHouseToDirect.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region ConvertShipmentFromDirectToHouseButton
            MenuButton ConvertShipmentFromDirectToHouseButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ConvertShipmentFromDirectToHouse",
                Index = 8,
                IsActive = true,
                LabelTextCodeCode = "Shipment.B.ConvertShipmentFromDirectToHouse",
                LabelTextCodeDefaultText = "Convert From Direct To House",
                ObjectTableId = shipmentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ShipmentMenuButtonGroup.Id,
                ParentMenuButtonId = actionButton.Id,
                FeatureId = ShipmentFeature_ConvertDirectToHouse.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region CopyShipment button
            MenuButton CopyShipmentButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "CopyShipment",
                Index = 10,
                IsActive = true,
                LabelTextCodeCode = "Shipment.B.CopyShipment",
                LabelTextCodeDefaultText = "Copy Shipment",
                ObjectTableId = shipmentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ShipmentMenuButtonGroup.Id,
                ParentMenuButtonId = actionButton.Id,
                FeatureId = ShipmentFeature_Copy.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region separator
            MenuButton ShipmentCopySeparator = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ShipmentCopySeparator",
                Index = 11,
                IsActive = false,
                LabelTextCodeCode = "Shipment.B.ShipmentCopySeparator",
                LabelTextCodeDefaultText = "",
                ObjectTableId = shipmentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ShipmentMenuButtonGroup.Id,
                ParentMenuButtonId = actionButton.Id,
                MenuButtonType = "separator",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region SendRequest button
            //MenuButton SendRequestButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            //{
            //    EventCode = "SendRequest",
            //    Index = 12,
            //    IsActive = true,
            //    LabelTextCodeCode = "Shipment.B.SendRequest",
            //    LabelTextCodeDefaultText = " FSR - Status Request",
            //    ObjectTableId = shipmentObject.Id,
            //    Tenant = tenant,
            //    MenuButtonGroupId = ShipmentMenuButtonGroup.Id,
            //    ParentMenuButtonId = actionButton.Id,
            //    FeatureId = ShipmentFeature_Sendrequest.Id,
            //    MenuButtonType = "menuitem",
            //}, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region SendResponse button
            MenuButton SendResponseButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "SendResponse",
                Index = 13,
                IsActive = true,
                LabelTextCodeCode = "Shipment.B.SendResponse",
                LabelTextCodeDefaultText = " Send Response",
                ObjectTableId = shipmentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ShipmentMenuButtonGroup.Id,
                ParentMenuButtonId = actionButton.Id,
                FeatureId = ShipmentFeature_SendResponse.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region separator
            MenuButton ShipmentCommunicationsSeparator = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ShipmentCommunicationsSeparator",
                Index = 14,
                IsActive = false,
                LabelTextCodeCode = "Shipment.B.ShipmentCommunicationsSeparator",
                LabelTextCodeDefaultText = "",
                ObjectTableId = shipmentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ShipmentMenuButtonGroup.Id,
                ParentMenuButtonId = actionButton.Id,
                MenuButtonType = "separator",
                FeatureId = ShipmentFeature_Sendrequest.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region cancel region
            MenuButton CancelButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "CancelShipment",
                Index = 15,
                IsActive = true,
                LabelTextCodeCode = "Shipment.B.CancelShipment",
                LabelTextCodeDefaultText = "Cancel Shipment",
                ObjectTableId = shipmentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ShipmentMenuButtonGroup.Id,
                ParentMenuButtonId = actionButton.Id,
                FeatureId = ShipmentFeature_Cancel.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Reactivate
            MenuButton ReactivateButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ReactivateShipment",
                Index = 16,
                IsActive = true,
                LabelTextCodeCode = "Shipment.B.ReactivateShipment",
                LabelTextCodeDefaultText = "Reactivate Shipment",
                ObjectTableId = shipmentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ShipmentMenuButtonGroup.Id,
                ParentMenuButtonId = actionButton.Id,
                FeatureId = ShipmentFeature_Reactivate.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Exception Resolved
            MenuButton ExceptionresolvedButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ExceptionResolved",
                Index = 17,
                IsActive = true,
                LabelTextCodeCode = "Shipment.B.ExceptionResolved",
                LabelTextCodeDefaultText = "Exception Resolved",
                ObjectTableId = shipmentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ShipmentMenuButtonGroup.Id,
                ParentMenuButtonId = actionButton.Id,
                FeatureId = ShipmentFeature_ExceptionResolved.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Convert To Custom File
            MenuButton ConvertToCustomButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ConvertToCustomFile",
                Index = 18,
                IsActive = true,
                LabelTextCodeCode = "Shipment.B.ConvertToCustomFile",
                LabelTextCodeDefaultText = "Convert To Custom File",
                ObjectTableId = shipmentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ShipmentMenuButtonGroup.Id,
                ParentMenuButtonId = actionButton.Id,
                FeatureId = ShipmentFeature_ConvertToCustomFile.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Split Shipment
            MenuButton SplitShipmentButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "SplitShipment",
                Index = 19,
                IsActive = true,
                LabelTextCodeCode = "Shipment.B.SplitShipment",
                LabelTextCodeDefaultText = "Split Shipment",
                ObjectTableId = shipmentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ShipmentMenuButtonGroup.Id,
                ParentMenuButtonId = actionButton.Id,
                FeatureId = ShipmentFeature_SplitShipment.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #endregion

            #region Master
            ObjectTable theMasterObject = ObjectContext.ObjectTables.Where(d => d.Name == "Master" && d.Tenant == tenant).FirstOrDefault();
            MenuButtonGroup MasterMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "MasterEdit",
                Name = "MasterEditButtonsGroup",
                ObjectTableId = theMasterObject.Id,
                Tenant = tenant,
            }, MenuButtonGroupRepository, TenantMenuButtonGroups);

            #region actions button
            MenuButton MasterActionButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Actions",
                Index = 100,
                IsActive = false,
                LabelTextCodeCode = "Master.B.Actions",
                LabelTextCodeDefaultText = "More",
                Tenant = tenant,
                MenuButtonGroupId = MasterMenuButtonGroup.Id,
                ObjectTableId = theMasterObject.Id,
                MenuButtonType = "dropdownbutton",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region operational close
            MenuButton MasterCloseButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "OperationalCloseMaster",
                Index = 2,
                IsActive = true,
                LabelTextCodeCode = "Master.B.OperationalClose",
                LabelTextCodeDefaultText = "Operational Close Master",
                ObjectTableId = theMasterObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = MasterMenuButtonGroup.Id,
                ParentMenuButtonId = MasterActionButton.Id,
                FeatureId = MasterFeature_OperationalClose.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region accounting close
            MenuButton MasterAccountingCloseButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "AccountingCloseMaster",
                Index = 3,
                IsActive = true,
                LabelTextCodeCode = "Master.B.AccountingClose",
                LabelTextCodeDefaultText = "Accounting Close Master",
                ObjectTableId = theMasterObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = MasterMenuButtonGroup.Id,
                ParentMenuButtonId = MasterActionButton.Id,
                FeatureId = MasterFeature_AccountingClose.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region operational reopen
            MenuButton MasterOperationalReopenButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "OperationalReopenMaster",
                Index = 4,
                IsActive = true,
                LabelTextCodeCode = "Master.B.OperationalReopen",
                LabelTextCodeDefaultText = "Operational Reopen Master",
                ObjectTableId = theMasterObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = MasterMenuButtonGroup.Id,
                ParentMenuButtonId = MasterActionButton.Id,
                FeatureId = MasterFeature_OperationalReopen.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region accounting reopen
            MenuButton MasterAccountedReopenButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "AccountedReopenMaster",
                Index = 5,
                IsActive = true,
                LabelTextCodeCode = "Master.B.AccountedReopen",
                LabelTextCodeDefaultText = "Accounted Reopen Master",
                ObjectTableId = theMasterObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = MasterMenuButtonGroup.Id,
                ParentMenuButtonId = MasterActionButton.Id,
                FeatureId = MasterFeature_AccountingReopen.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region separator
            MenuButton MasterOperationSeparator = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "MasterOperationSeparator",
                Index = 6,
                IsActive = false,
                LabelTextCodeCode = "Master.B.MasterOperationSeparator",
                LabelTextCodeDefaultText = "",
                ObjectTableId = theMasterObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = MasterMenuButtonGroup.Id,
                ParentMenuButtonId = MasterActionButton.Id,
                MenuButtonType = "separator",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region CopyMaster button
            MenuButton CopyMasterButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "CopyMaster",
                Index = 7,
                IsActive = true,
                LabelTextCodeCode = "Master.B.CopyMaster",
                LabelTextCodeDefaultText = "Copy Master",
                ObjectTableId = theMasterObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = MasterMenuButtonGroup.Id,
                ParentMenuButtonId = MasterActionButton.Id,
                FeatureId = MasterFeature_Copy.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region separator
            MenuButton masterSendRequestSeparator = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "masterSendRequestSeparator",
                Index = 8,
                IsActive = false,
                LabelTextCodeCode = "Master.B.masterSendRequestSeparator",
                LabelTextCodeDefaultText = "",
                ObjectTableId = MasterObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = MasterMenuButtonGroup.Id,
                ParentMenuButtonId = MasterActionButton.Id,
                MenuButtonType = "separator",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region SendRequest button
            MenuButton MasterSendRequestButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "SendRequest",
                Index = 9,
                IsActive = true,
                LabelTextCodeCode = "Master.B.SendRequest",
                LabelTextCodeDefaultText = " FSR - Status Request",
                ObjectTableId = MasterObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = MasterMenuButtonGroup.Id,
                ParentMenuButtonId = MasterActionButton.Id,
                FeatureId = MasterFeature_Sendrequest.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region SendResponse button
            MenuButton MasterSendResponseButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "SendResponse",
                Index = 10,
                IsActive = true,
                LabelTextCodeCode = "Master.B.SendResponse",
                LabelTextCodeDefaultText = " Send Response",
                ObjectTableId = MasterObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = MasterMenuButtonGroup.Id,
                ParentMenuButtonId = MasterActionButton.Id,
                FeatureId = MasterFeature_SendResponse.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region separator
            MenuButton MasterCopySeparator = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "MasterCopySeparator",
                Index = 8,
                IsActive = false,
                LabelTextCodeCode = "Master.B.MasterCopySeparator",
                LabelTextCodeDefaultText = "",
                ObjectTableId = theMasterObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = MasterMenuButtonGroup.Id,
                ParentMenuButtonId = MasterActionButton.Id,
                MenuButtonType = "separator",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region cancel region
            MenuButton MasterCancelButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "CancelMaster",
                Index = 9,
                IsActive = true,
                LabelTextCodeCode = "Master.B.CancelMaster",
                LabelTextCodeDefaultText = "Cancel Master",
                ObjectTableId = theMasterObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = MasterMenuButtonGroup.Id,
                ParentMenuButtonId = MasterActionButton.Id,
                FeatureId = MasterFeature_Cancel.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region reactivate region
            MenuButton MasterReactivateButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ReactivateMaster",
                Index = 10,
                IsActive = true,
                LabelTextCodeCode = "Master.B.ReactivateMaster",
                LabelTextCodeDefaultText = "Reactivate Master",
                ObjectTableId = theMasterObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = MasterMenuButtonGroup.Id,
                ParentMenuButtonId = MasterActionButton.Id,
                FeatureId = MasterFeature_Reactivate.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion
            #endregion

            #region Quote Buttons

            ObjectTable quoteObject = ObjectContext.ObjectTables.Where(d => d.Name == "Quote" && d.Tenant == tenant).FirstOrDefault();

            MenuButtonGroup QuoteMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "QuoteEdit",
                Name = "QuoteEditButtonsGroup",
                ObjectTableId = quoteObject.Id,
                Tenant = tenant,
            }, MenuButtonGroupRepository, TenantMenuButtonGroups);

            #region More
            MenuButton QuoteActionButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Actions",
                Index = 100,
                IsActive = false,
                LabelTextCodeCode = "Quote.B.Actions",
                LabelTextCodeDefaultText = "More",
                ObjectTableId = quoteObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = QuoteMenuButtonGroup.Id,
                MenuButtonType = "dropdownbutton",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region SetAsSentToClient button
            MenuButton SetAsSentToClientButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "SetAsSentToCustomer",
                Index = 6,
                IsActive = true,
                LabelTextCodeCode = "Quote.B.SetAsSent",
                LabelTextCodeDefaultText = "Set As Sent to Customer",
                ObjectTableId = quoteObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = QuoteMenuButtonGroup.Id,
                ParentMenuButtonId = QuoteActionButton.Id,
                FeatureId = QuoteFeature_SetAsSent.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Return To Draft button
            MenuButton ReturnToDraftButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ReturnToDraft",
                Index = 7,
                IsActive = true,
                LabelTextCodeCode = "Quote.B.ReturnToDraft",
                LabelTextCodeDefaultText = "Return Quote To Draft",
                ObjectTableId = quoteObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = QuoteMenuButtonGroup.Id,
                ParentMenuButtonId = QuoteActionButton.Id,
                FeatureId = QuoteFeature_ReturnToDraft.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region CopyQuote button
            MenuButton CopyQuoteButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "CopyQuote",
                Index = 8,
                IsActive = true,
                LabelTextCodeCode = "Quote.B.CopyQuote",
                LabelTextCodeDefaultText = "Copy Quote",
                ObjectTableId = quoteObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = QuoteMenuButtonGroup.Id,
                ParentMenuButtonId = QuoteActionButton.Id,
                FeatureId = QuoteFeature_Copy.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region separator
            MenuButton QuoteCopySeparator = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "QuoteCopySeparator",
                Index = 9,
                IsActive = false,
                LabelTextCodeCode = "Quote.B.QuoteCopySeparator",
                LabelTextCodeDefaultText = "",
                ObjectTableId = quoteObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = QuoteMenuButtonGroup.Id,
                ParentMenuButtonId = QuoteActionButton.Id,
                MenuButtonType = "separator",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Cancel
            MenuButton CancelQuoteButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "CancelQuote",
                Index = 10,
                IsActive = true,
                LabelTextCodeCode = "Quote.B.CancelQuote",
                LabelTextCodeDefaultText = "Cancel Quote",
                ObjectTableId = quoteObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = QuoteMenuButtonGroup.Id,
                ParentMenuButtonId = QuoteActionButton.Id,
                FeatureId = QuoteFeature_Cancel.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Reactivate
            MenuButton ReactivateQuoteButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ReactivateQuote",
                Index = 11,
                IsActive = true,
                LabelTextCodeCode = "Quote.B.ReactivateQuote",
                LabelTextCodeDefaultText = "Reactivate Quote",
                ObjectTableId = quoteObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = QuoteMenuButtonGroup.Id,
                ParentMenuButtonId = QuoteActionButton.Id,
                FeatureId = QuoteFeature_Reactivate.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Build Shipment
            MenuButton QuoteBuildShipmentButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "BuildShipment",
                Index = 13,
                IsActive = true,
                LabelTextCodeCode = "Quote.B.BuildShipment",
                LabelTextCodeDefaultText = "Build Shipment",
                ObjectTableId = quoteObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = QuoteMenuButtonGroup.Id,
                MenuButtonType = "button",
                FeatureId = QuoteFeature_BuildShipment.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Accept button
            MenuButton AcceptButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Accept",
                Index = 12,
                IsActive = true,
                LabelTextCodeCode = "Quote.B.Accept",
                LabelTextCodeDefaultText = "Accept",
                ObjectTableId = quoteObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = QuoteMenuButtonGroup.Id,
                MenuButtonType = "button",
                Style = "ApproveButtonStyle",
                FeatureId = QuoteFeature_Accepted.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Decline button
            MenuButton DeclineButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Decline",
                Index = 12,
                IsActive = true,
                LabelTextCodeCode = "Quote.B.Decline",
                LabelTextCodeDefaultText = "Decline",
                ObjectTableId = quoteObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = QuoteMenuButtonGroup.Id,
                MenuButtonType = "button",
                Style = "RedButtonStyle",
                FeatureId = QuoteFeature_Declined.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Quotation button
            MenuButton QuotationButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Quotation",
                Index = 13,
                IsActive = true,
                LabelTextCodeCode = "Quote.B.Quotation",
                LabelTextCodeDefaultText = "Quotation",
                ObjectTableId = quoteObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = QuoteMenuButtonGroup.Id,
                MenuButtonType = "button",
                //Style = "RedButtonStyle",
                FeatureId = QuoteFeature_Quotation.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #endregion

            #region Airline Buttons
            ObjectTable airlineObject = ObjectContext.ObjectTables.Where(d => d.Name == "Airline" && d.Tenant == tenant).FirstOrDefault();
            MenuButtonGroup AirlineMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "AirlineEdit",
                Name = "AirlineEditButtonsGroup",
                ObjectTableId = airlineObject.Id,
                Tenant = tenant,
            }, MenuButtonGroupRepository, TenantMenuButtonGroups);

            //MenuButton airlineEventButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            //{
            //    EventCode = "ShowEvents",
            //    Index = 0,
            //    IsActive = true,
            //    LabelTextCodeCode = "Airline.MenuButtons.EventButton",
            //    LabelTextCodeDefaultText = "Events",
            //    ObjectTableId = airlineObject.Id,
            //    Tenant = tenant,
            //    MenuButtonGroupId = AirlineMenuButtonGroup.Id,
            //}, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion


            #region Contact Buttons
     
            ObjectTable contactObject = ObjectContext.ObjectTables.Where(d => d.Name == "Contact" && d.Tenant == tenant).FirstOrDefault();
            MenuButtonGroup ContactMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "ContactEdit",
                Name = "ContactEditButtonsGroup",
                ObjectTableId = contactObject.Id,
                Tenant = tenant,
            }, MenuButtonGroupRepository, TenantMenuButtonGroups);

            #region actions button
            MenuButton contactActionButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Actions",
                Index = 100,
                IsActive = false,
                LabelTextCodeCode = "Contact.B.Actions",
                LabelTextCodeDefaultText = "More",
                Tenant = tenant,
                MenuButtonGroupId = ContactMenuButtonGroup.Id,
                ObjectTableId = contactObject.Id,
                MenuButtonType = "dropdownbutton",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Anonymize

            MenuButton anonymizeContactButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Anonymize",
                Index = 3,
                IsActive = true,
                LabelTextCodeCode = "Contact.B.Anonymize",
                LabelTextCodeDefaultText = "Anonymize",
                ObjectTableId = contactObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ContactMenuButtonGroup.Id,
                ParentMenuButtonId = contactActionButton.Id,
                FeatureId = ContactFeature_Anonymize.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);

            #endregion

          

            #endregion

            #region User Buttons
            ObjectTable userObject = ObjectContext.ObjectTables.Where(d => d.Name == "User" && d.Tenant == tenant).FirstOrDefault();
            MenuButtonGroup UserMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "UserEdit",
                Name = "UserEditButtonsGroup",
                ObjectTableId = userObject.Id,
                Tenant = tenant,
            }, MenuButtonGroupRepository, TenantMenuButtonGroups);


   
            #region actions button
            MenuButton userActionButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Actions",
                Index = 100,
                IsActive = false,
                LabelTextCodeCode = "User.B.Actions",
                LabelTextCodeDefaultText = "More",
                Tenant = tenant,
                MenuButtonGroupId = UserMenuButtonGroup.Id,
                ObjectTableId = userObject.Id,
                MenuButtonType = "dropdownbutton",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region reset password
            MenuButton resetButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ResetUserPassword",
                Index = 2,
                IsActive = true,
                LabelTextCodeCode = "User.B.ResetPassword",
                LabelTextCodeDefaultText = "Reset Password",
                ObjectTableId = userObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = UserMenuButtonGroup.Id,
                ParentMenuButtonId = userActionButton.Id,
                FeatureId = UserFeature_ResetPassword.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);



            #endregion

            #region Anonymize
            MenuButton anonymizeUserButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Anonymize",
                Index = 3,
                IsActive = true,
                LabelTextCodeCode = "User.B.Anonymize",
                LabelTextCodeDefaultText = "Anonymize",
                ObjectTableId = userObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = UserMenuButtonGroup.Id,
                ParentMenuButtonId = userActionButton.Id,
                FeatureId = UserFeature_Anonymize.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion


            #endregion

            #region AR Invoice
            ObjectTable invoiceObject = ObjectContext.ObjectTables.Where(d => d.Name == "ARInvoice" && d.Tenant == tenant).FirstOrDefault();
            MenuButtonGroup InvoiceMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "InvoiceEdit",
                Name = "InvoiceEditButtonsGroup",
                ObjectTableId = invoiceObject.Id,
                Tenant = tenant,
            }, MenuButtonGroupRepository, TenantMenuButtonGroups);

            #region Actions
            MenuButton InvoiceActionButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Actions",
                Index = 100,
                IsActive = false,
                LabelTextCodeCode = "ARInvoice.B.Actions",
                LabelTextCodeDefaultText = "More",
                LocalDefaultText = "נוספים",
                Tenant = tenant,
                MenuButtonGroupId = InvoiceMenuButtonGroup.Id,
                ObjectTableId = invoiceObject.Id,
                MenuButtonType = "dropdownbutton",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Cancel draft
            MenuButton InvoiceCancelDraftButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "CancelDraft",
                Index = 2,
                IsActive = true,
                LabelTextCodeCode = "ARInvoice.B.CancelDraft",
                LabelTextCodeDefaultText = "Cancel Draft",
                LocalDefaultText = "ביטול טיוטה",
                ObjectTableId = invoiceObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = InvoiceMenuButtonGroup.Id,
                ParentMenuButtonId = InvoiceActionButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = ARInvoiceFeature_CancelDraft.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);

            #endregion

            #region Auto Credit
            MenuButton InvoiceAutoCreditButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "AutoCredit",
                Index = 3,
                IsActive = true,
                LabelTextCodeCode = "ARInvoice.B.AutoCredit",
                LabelTextCodeDefaultText = "Auto Credit",
                LocalDefaultText = "זיכוי",
                ObjectTableId = invoiceObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = InvoiceMenuButtonGroup.Id,
                ParentMenuButtonId = InvoiceActionButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = ARInvoiceFeature_AutoCredit.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);

            #endregion

            #region separator
            MenuButton InvoiceOperationsSeparator = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "InvoiceOperationsSeparator",
                Index = 5,
                IsActive = false,
                LabelTextCodeCode = "ARInvoice.B.InvoiceOperationsSeparator",
                LabelTextCodeDefaultText = "",
                ObjectTableId = invoiceObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = InvoiceMenuButtonGroup.Id,
                ParentMenuButtonId = InvoiceActionButton.Id,
                MenuButtonType = "separator",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Set as Sent
            MenuButton InvoiceSetAsSentButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "SetAsSent",
                Index = 6,
                IsActive = true,
                LabelTextCodeCode = "ARInvoice.B.SetAsSent",
                LabelTextCodeDefaultText = "Set as Sent",
                LocalDefaultText = "סמן כנשלח",
                ObjectTableId = invoiceObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = InvoiceMenuButtonGroup.Id,
                ParentMenuButtonId = InvoiceActionButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = ARInvoiceFeature_SetAsSent.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region ReTransfer
            MenuButton InvoiceReTransferButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ReTransfer",
                Index = 7,
                IsActive = true,
                LabelTextCodeCode = "ARInvoice.B.ReTransfer",
                LabelTextCodeDefaultText = "Enable accounting re-transfer",
                LocalDefaultText = "אפשר העברה מחדש להנה''ח",
                ObjectTableId = invoiceObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = InvoiceMenuButtonGroup.Id,
                ParentMenuButtonId = InvoiceActionButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = ARInvoiceFeature_ReTransfer.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion


            #region SendToQbo
            MenuButton InvoiceSendToQBOButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "SendToQBO",
                Index = 14,
                IsActive = true,
                LabelTextCodeCode = "ARInvoice.B.SendToQBO",
                LabelTextCodeDefaultText = "Send to QBO",
                LocalDefaultText = "Send to QBO",
                ObjectTableId = invoiceObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = InvoiceMenuButtonGroup.Id,
                ParentMenuButtonId = InvoiceActionButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = ARInvoiceFeature_SendQBO.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Separator
            MenuButton VoidInvoiceOperationsSeparator = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "VoidARInvoiceOperationsSeparator",
                Index = 8,
                IsActive = false,
                LabelTextCodeCode = "ARInvoice.B.VoidARInvoiceOperationsSeparator",
                LabelTextCodeDefaultText = "",
                ObjectTableId = invoiceObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = InvoiceMenuButtonGroup.Id,
                ParentMenuButtonId = InvoiceActionButton.Id,

                MenuButtonType = "separator",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);

            #endregion

            #region Void
            MenuButton VoidARInvoiceButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "VoidARInvoice",
                Index = 9,
                IsActive = true,
                LabelTextCodeCode = "ARInvoice.B.Void",
                LabelTextCodeDefaultText = "Void",
                LocalDefaultText = "ביטול",
                ObjectTableId = invoiceObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = InvoiceMenuButtonGroup.Id,
                ParentMenuButtonId = InvoiceActionButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = ARInvoiceFeature_Void.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Save Draft
            MenuButton SaveAsDraftButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "SaveAsDraft",
                Index = 10,
                IsActive = true,
                LabelTextCodeCode = "ARInvoice.B.SaveAsDraft",
                LabelTextCodeDefaultText = "Save as Draft",
                LocalDefaultText = "שמור כטיוטה",
                Tenant = tenant,
                MenuButtonGroupId = InvoiceMenuButtonGroup.Id,
                ObjectTableId = invoiceObject.Id,
                FeatureId = ARInvoiceFeature_SetAsDraft.Id,
                MenuButtonType = "button",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Approve
            MenuButton SaveAndApproveButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "SaveAndApprove",
                Index = 11,
                IsActive = true,
                LabelTextCodeCode = "ARInvoice.B.Approve",
                LabelTextCodeDefaultText = "Approve",
                LocalDefaultText = "אישור",
                Tenant = tenant,
                MenuButtonGroupId = InvoiceMenuButtonGroup.Id,
                ObjectTableId = invoiceObject.Id,
                Style = "ApproveButtonStyle",
                FeatureId = ARInvoiceFeature_SaveAndApprove.Id,
                MenuButtonType = "button",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Print
            MenuButton InvoiceEventButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "PrintInvoice",
                Index = 12,
                IsActive = true,
                LabelTextCodeCode = "ARInvoice.B.Print",
                LabelTextCodeDefaultText = "Print",
                LocalDefaultText = "הדפסה",
                Tenant = tenant,
                MenuButtonGroupId = InvoiceMenuButtonGroup.Id,
                ObjectTableId = invoiceObject.Id,
                FeatureId = ARInvoiceFeature_Print.Id,
                MenuButtonType = "button",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            MenuButton InvoiceCheckSATStatusStatusButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "CheckSATStatus",
                Index = 13,
                IsActive = true,
                LabelTextCodeCode = "ARInvoice.B.CheckSATStatus",
                LabelTextCodeDefaultText = "Check SAT Status",
                ObjectTableId = invoiceObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = InvoiceMenuButtonGroup.Id,
                MenuButtonType = "button",
                FeatureId = ARInvoiceFeature_CHECKSATStatus.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);


            #endregion

            #region AP invoice
            ObjectTable APinvoiceObject = ObjectContext.ObjectTables.Where(d => d.Name == "APInvoice" && d.Tenant == tenant).FirstOrDefault();
            MenuButtonGroup APInvoiceMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "APInvoiceEdit",
                Name = "APInvoiceEditButtonsGroup",
                ObjectTableId = APinvoiceObject.Id,
                Tenant = tenant,
            }, MenuButtonGroupRepository, TenantMenuButtonGroups);

            #region Actions
            MenuButton APInvoiceActionButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Actions",
                Index = 100,
                IsActive = false,
                LabelTextCodeCode = "APInvoice.B.Actions",
                LabelTextCodeDefaultText = "More",
                LocalDefaultText = "נוספים",
                Tenant = tenant,
                MenuButtonGroupId = APInvoiceMenuButtonGroup.Id,
                ObjectTableId = APinvoiceObject.Id,
                MenuButtonType = "dropdownbutton",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Cancel Approval
            MenuButton APInvoiceCancelApprovalButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "CancelApproval",
                Index = 2,
                IsActive = true,
                LabelTextCodeCode = "APInvoice.B.CancelApproval",
                LabelTextCodeDefaultText = "Cancel Approval",
                LocalDefaultText = "ביטול אישור", 
                ObjectTableId = APinvoiceObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = APInvoiceMenuButtonGroup.Id,
                ParentMenuButtonId = APInvoiceActionButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = APInvoiceFeature_CancelApproval.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region ReTransfer
            MenuButton APInvoiceReTransferButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ReTransfer",
                Index = 3,
                IsActive = true,
                LabelTextCodeCode = "APInvoice.B.ReTransfer",
                LabelTextCodeDefaultText = "Enable accounting re-transfer",
                LocalDefaultText = "אפשר העברה מחדש להנה''ח",
                ObjectTableId = APinvoiceObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = APInvoiceMenuButtonGroup.Id,
                ParentMenuButtonId = APInvoiceActionButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = APInvoiceFeature_ReTransfer.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Separator
            MenuButton VoidAPInvoiceOperationsSeparator = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "VoidAPInvoiceOperationsSeparator",
                Index = 4,
                IsActive = false,
                LabelTextCodeCode = "APInvoice.B.VoidAPInvoiceOperationsSeparator",
                LabelTextCodeDefaultText = "",
                ObjectTableId = APinvoiceObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = APInvoiceMenuButtonGroup.Id,
                ParentMenuButtonId = APInvoiceActionButton.Id,
                MenuButtonType = "separator",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Void
            MenuButton VoidAPInvoiceButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "VoidAPInvoice",
                Index = 5,
                IsActive = true,
                LabelTextCodeCode = "APInvoice.B.Void",
                LabelTextCodeDefaultText = "Void",
                LocalDefaultText = "ביטול",
                ObjectTableId = APinvoiceObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = APInvoiceMenuButtonGroup.Id,
                ParentMenuButtonId = APInvoiceActionButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = APInvoiceFeature_Void.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Save
            MenuButton APSaveButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "SaveAPInvoice",
                Index = 6,
                IsActive = true,
                LabelTextCodeCode = "APInvoice.B.Save",
                LabelTextCodeDefaultText = "Save",
                LocalDefaultText = "שמור",
                Tenant = tenant,
                MenuButtonGroupId = APInvoiceMenuButtonGroup.Id,
                ObjectTableId = APinvoiceObject.Id,
                MenuButtonType = "button",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Approve
            MenuButton APApproveButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ApproveAPInvoice",
                Index = 7,
                IsActive = true,
                LabelTextCodeCode = "APInvoice.B.Approve",
                LabelTextCodeDefaultText = "Approve",
                LocalDefaultText = "אישור",
                Tenant = tenant,
                MenuButtonGroupId = APInvoiceMenuButtonGroup.Id,
                ObjectTableId = APinvoiceObject.Id,
                Style = "ApproveButtonStyle",
                MenuButtonType = "button",
                FeatureId = APInvoiceFeature_Approve.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Print
            MenuButton APPrintButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "PrintAPInvoice",
                Index = 8,
                IsActive = true,
                LabelTextCodeCode = "APInvoice.B.Print",
                LabelTextCodeDefaultText = "Print",
                LocalDefaultText = "הדפסה",
                Tenant = tenant,
                MenuButtonGroupId = APInvoiceMenuButtonGroup.Id,
                ObjectTableId = APinvoiceObject.Id,
                FeatureId = APInvoiceFeature_Print.Id,
                MenuButtonType = "button",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion


            #region SendToQbo
            MenuButton APInvoiceSendToQBOButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "SendToQBO",
                Index = 14,
                IsActive = true,
                LabelTextCodeCode = "APInvoice.B.SendToQBO",
                LabelTextCodeDefaultText = "Send to QBO",
                LocalDefaultText = "Send to QBO",
                ObjectTableId = APinvoiceObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = APInvoiceMenuButtonGroup.Id,
                ParentMenuButtonId = APInvoiceActionButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = APInvoiceFeature_SendQBO.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #endregion

            #region AR Payment

            ObjectTable theARPaymentObject = ObjectContext.ObjectTables.Where(d => d.Name == "ARPayment" && d.Tenant == tenant).FirstOrDefault();
            MenuButtonGroup ARPaymentMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "ARPaymentEdit",
                Name = "ARPaymentEditButtonsGroup",
                ObjectTableId = theARPaymentObject.Id,
                Tenant = tenant,
            }, MenuButtonGroupRepository, TenantMenuButtonGroups);

            MenuButton ARPaymentActionButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Actions",
                Index = 100,
                IsActive = false,
                LabelTextCodeCode = "ARPayment.B.Actions",
                LabelTextCodeDefaultText = "More",
                LocalDefaultText = "נוספים",
                Tenant = tenant,
                MenuButtonGroupId = ARPaymentMenuButtonGroup.Id,
                ObjectTableId = theARPaymentObject.Id,
                MenuButtonType = "dropdownbutton",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);

            MenuButton ARPaymentCancelApprovalButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "CancelApproval",
                Index = 2,
                IsActive = true,
                LabelTextCodeCode = "ARPayment.B.CancelApproval",
                LabelTextCodeDefaultText = "Cancel Approval",
                LocalDefaultText = "ביטול אישור",
                ObjectTableId = theARPaymentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ARPaymentMenuButtonGroup.Id,
                ParentMenuButtonId = ARPaymentActionButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = ARPaymentFeature_CancelApproval.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);

            MenuButton ARPaymentReTransferButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ReTransfer",
                Index = 3,
                IsActive = true,
                LabelTextCodeCode = "ARPayment.B.ReTransfer",
                LabelTextCodeDefaultText = "Enable accounting re-transfer",
                LocalDefaultText = "אפשר העברה מחדש להנה''ח",
                ObjectTableId = theARPaymentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ARPaymentMenuButtonGroup.Id,
                ParentMenuButtonId = ARPaymentActionButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = ARPaymentFeature_ReTransfer.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);

            MenuButton VoidARPaymentOperationsSeparator = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "VoidARPaymentOperationsSeparator",
                Index = 4,
                IsActive = false,
                LabelTextCodeCode = "ARPayment.B.VoidARPaymentOperationsSeparator",
                LabelTextCodeDefaultText = "",
                ObjectTableId = theARPaymentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ARPaymentMenuButtonGroup.Id,
                ParentMenuButtonId = ARPaymentActionButton.Id,
                MenuButtonType = "separator",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);

            MenuButton VoidARPayemntButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "VoidARPayemnt",
                Index = 5,
                IsActive = true,
                LabelTextCodeCode = "ARPayment.B.Void",
                LabelTextCodeDefaultText = "Void",
                LocalDefaultText = "ביטול",
                ObjectTableId = theARPaymentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ARPaymentMenuButtonGroup.Id,
                ParentMenuButtonId = ARPaymentActionButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = ARPaymentFeature_Void.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);

            MenuButton ARPaymentApproveButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ApproveARPayment",
                Index = 6,
                IsActive = true,
                LabelTextCodeCode = "ARPayment.B.Approve",
                LabelTextCodeDefaultText = "Approve",
                LocalDefaultText = "אישור",
                Tenant = tenant,
                MenuButtonGroupId = ARPaymentMenuButtonGroup.Id,
                ObjectTableId = theARPaymentObject.Id,
                Style = "ApproveButtonStyle",
                FeatureId = ARPaymentFeature_Approve.Id,
                MenuButtonType = "button",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);

            MenuButton ARPaymentEventButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "PrintARPayment",
                Index = 7,
                IsActive = true,
                LabelTextCodeCode = "ARPayment.B.Print",
                LabelTextCodeDefaultText = "Print",
                LocalDefaultText = "הדפסה",
                Tenant = tenant,
                MenuButtonGroupId = ARPaymentMenuButtonGroup.Id,
                ObjectTableId = theARPaymentObject.Id,
                FeatureId = ARPaymentFeature_Print.Id,
                MenuButtonType = "button",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);



            MenuButton SENDToSATButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "SENDToSAT",
                Index = 8,
                IsActive = true,
                LabelTextCodeCode = "ARPayment.B.SENDToSAT",
                LabelTextCodeDefaultText = "Send to SAT",
                ObjectTableId = theARPaymentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ARPaymentMenuButtonGroup.Id,
                ParentMenuButtonId = ARPaymentActionButton.Id,
                MenuButtonType = "button",
                FeatureId = ARPaymentFeature_SendToSAT.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);

            MenuButton ARPaymentCheckSATStatusStatusButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "CheckSATStatus",
                Index = 9,
                IsActive = true,
                LabelTextCodeCode = "ARPayment.B.CheckSATStatus",
                LabelTextCodeDefaultText = "Check SAT Status",
                ObjectTableId = theARPaymentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ARPaymentMenuButtonGroup.Id,
                MenuButtonType = "button",
                FeatureId = ARPaymentFeature_CHECKSATStatus.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);

            MenuButton ARPaymentSendToQBOButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "SendToQBO",
                Index = 10,
                IsActive = true,
                LabelTextCodeCode = "ARPayment.B.SendToQBO",
                LabelTextCodeDefaultText = "Send To QBO",
                LocalDefaultText = "Send To QBO",
                ObjectTableId = theARPaymentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = ARPaymentMenuButtonGroup.Id,
                ParentMenuButtonId = ARPaymentActionButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = ARPaymentFeature_SendToQBO.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);

            #endregion

            #region APPayment menu buttons
            ObjectTable theAPPaymentObject = ObjectContext.ObjectTables.Where(d => d.Name == "APPayment" && d.Tenant == tenant).FirstOrDefault();
            MenuButtonGroup APPaymentMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "APPaymentEdit",
                Name = "APPaymentEditButtonsGroup",
                ObjectTableId = theAPPaymentObject.Id,
                Tenant = tenant,
            }, MenuButtonGroupRepository, TenantMenuButtonGroups);


            #region actions button
            MenuButton APPaymentActionButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Actions",
                Index = 100,
                IsActive = false,
                LabelTextCodeCode = "APPayment.B.Actions",
                LabelTextCodeDefaultText = "More",
                LocalDefaultText = "נוספים", 
                Tenant = tenant,
                MenuButtonGroupId = APPaymentMenuButtonGroup.Id,
                ObjectTableId = theAPPaymentObject.Id,
                MenuButtonType = "dropdownbutton",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Cancel Approval
            MenuButton APPaymentCancelApprovalButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "CancelApproval",
                Index = 2,
                IsActive = true,
                LabelTextCodeCode = "APPayment.B.CancelApproval",
                LabelTextCodeDefaultText = "Cancel Approval",
                LocalDefaultText = "ביטול אישור",
                ObjectTableId = theAPPaymentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = APPaymentMenuButtonGroup.Id,
                ParentMenuButtonId = APPaymentActionButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = APPaymentFeature_CancelApproval.Id,
             
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region separator
            MenuButton VoidAPPaymentOperationsSeparator = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "VoidAPPaymentOperationsSeparator",
                Index = 3,
                IsActive = false,
                LabelTextCodeCode = "APPayment.B.VoidAPPaymentOperationsSeparator",
                LabelTextCodeDefaultText = "",
                ObjectTableId = theAPPaymentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = APPaymentMenuButtonGroup.Id,
                ParentMenuButtonId = APPaymentActionButton.Id,
                MenuButtonType = "separator",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region VoidAPPaymentbutton
            MenuButton VoidAPPaymentButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "VoidAPPayment",
                Index = 4,
                IsActive = true,
                LabelTextCodeCode = "APPayment.B.Void",
                LabelTextCodeDefaultText = "Void",
                LocalDefaultText = "ביטול", 
                ObjectTableId = theAPPaymentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = APPaymentMenuButtonGroup.Id,
                ParentMenuButtonId = APPaymentActionButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = APPaymentFeature_Void.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Approve button
            MenuButton APPaymentApproveButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ApproveAPPayment",
                Index = 6,
                IsActive = true,
                LabelTextCodeCode = "APPayment.B.Approve",
                LabelTextCodeDefaultText = "Approve",
                LocalDefaultText = "אישור",
                Tenant = tenant,
                MenuButtonGroupId = APPaymentMenuButtonGroup.Id,
                ObjectTableId = theAPPaymentObject.Id,
                Style = "ApproveButtonStyle",
                FeatureId = APPaymentFeature_Approve.Id,
                MenuButtonType = "button",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region Printpayment button
            MenuButton APPaymentEventButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "PrintAPPayment",
                Index = 7,
                IsActive = true,
                LabelTextCodeCode = "APPayment.B.Print",
                LabelTextCodeDefaultText = "Print",
                LocalDefaultText = "הדפסה",
                Tenant = tenant,
                MenuButtonGroupId = APPaymentMenuButtonGroup.Id,
                ObjectTableId = theAPPaymentObject.Id,
                FeatureId = APPaymentFeature_Print.Id,
                MenuButtonType = "button",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion


            MenuButton APPaymentSendToQBOButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "SendToQBO",
                Index = 10,
                IsActive = true,
                LabelTextCodeCode = "APPayment.B.SendToQBO",
                LabelTextCodeDefaultText = "Send To QBO",
                LocalDefaultText = "Send To QBO",
                ObjectTableId = theAPPaymentObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = APPaymentMenuButtonGroup.Id,
                ParentMenuButtonId = APPaymentActionButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = APPaymentFeature_SendToQBO.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);


            #endregion

            #region communicationLog Buttons
            ObjectTable CommLogObject = ObjectContext.ObjectTables.Where(d => d.Name == "CommunicationLog" && d.Tenant == tenant).FirstOrDefault();
            MenuButtonGroup CommLogMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "CommunicationLogEdit",
                Name = "CommunicationLogEditButtonsGroup",
                ObjectTableId = CommLogObject.Id,
                Tenant = tenant,
            }, MenuButtonGroupRepository, TenantMenuButtonGroups);

            #region actions button
            MenuButton CommLogActionButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Actions",
                Index = 100,
                IsActive = false,
                LabelTextCodeCode = "CommunicationLog.B.Actions",
                LabelTextCodeDefaultText = "More",
                Tenant = tenant,
                MenuButtonGroupId = CommLogMenuButtonGroup.Id,
                ObjectTableId = CommLogObject.Id,
                MenuButtonType = "dropdownbutton",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region reset password
            MenuButton resendButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Resend",
                Index = 2,
                IsActive = true,
                LabelTextCodeCode = "CommunicationLog.B.Resend",
                LabelTextCodeDefaultText = "Resend",
                ObjectTableId = CommLogObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = CommLogMenuButtonGroup.Id,
                //ParentMenuButtonId = CommLogActionButton.Id,
                FeatureId = LogFeature_Resend.Id,
                MenuButtonType = "button",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region view messages
            MenuButton messagesButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ViewMessage",
                Index = 3,
                IsActive = true,
                LabelTextCodeCode = "CommunicationLog.B.Message",
                LabelTextCodeDefaultText = "View Message",
                ObjectTableId = CommLogObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = CommLogMenuButtonGroup.Id,
                ParentMenuButtonId = CommLogActionButton.Id,
                FeatureId = LogFeature_Messages.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion
            #endregion

            #region Customer Buttons

            ObjectTable customerObject = ObjectContext.ObjectTables.Where(d => d.Name == "Customer" && d.Tenant == tenant).FirstOrDefault();
            MenuButtonGroup customerMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "CustomerEdit",
                Name = "CustomerEditButtonsGroup",
                ObjectTableId = customerObject.Id,
                Tenant = tenant,
            }, MenuButtonGroupRepository, TenantMenuButtonGroups);

            #region [1] More
            MenuButton CustomerMoreButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Actions",
                Index = 100,
                IsActive = false,
                LabelTextCodeCode = "Customer.B.More",
                LabelTextCodeDefaultText = "More",
                Tenant = tenant,
                MenuButtonGroupId = customerMenuButtonGroup.Id,
                ObjectTableId = customerObject.Id,
                MenuButtonType = "dropdownbutton",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region [2] Activate
            MenuButton ActivateButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Activate",
                Index = 2,
                IsActive = true,
                LabelTextCodeCode = "Customer.B.Activate",
                LabelTextCodeDefaultText = "Activate",
                Tenant = tenant,
                MenuButtonGroupId = customerMenuButtonGroup.Id,
                ObjectTableId = customerObject.Id,
                FeatureId = CustomerFeature_Activate.Id,
                MenuButtonType="button",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region [3] Ready For Activation
            MenuButton ReadyButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ReadyActivate",
                Index = 3,
                IsActive = true,
                LabelTextCodeCode = "Customer.B.Ready",
                LabelTextCodeDefaultText = "Ready For Activation",
                Tenant = tenant,
                MenuButtonGroupId = customerMenuButtonGroup.Id,
                ObjectTableId = customerObject.Id,
                FeatureId = CustomerFeature_Ready.Id,
                MenuButtonType = "button",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region [4] Totango
            MenuButton TotangoButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Totango",
                Index = 4,
                IsActive = true,
                LabelTextCodeCode = "Customer.B.Totango",
                LabelTextCodeDefaultText = "Totango",
                Tenant = tenant,
                MenuButtonGroupId = customerMenuButtonGroup.Id,
                ObjectTableId = customerObject.Id,
                FeatureId = CustomerFeature_Totango.Id,
                MenuButtonType = "button",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region [5] TenantManagement
            MenuButton TenantManagementButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "TenantManagement",
                Index = 5,
                IsActive = true,
                LabelTextCodeCode = "Customer.B.TenantManagement",
                LabelTextCodeDefaultText = "Manage",
                Tenant = tenant,
                MenuButtonGroupId = customerMenuButtonGroup.Id,
                ObjectTableId = customerObject.Id,
                FeatureId = CustomerFeature_TenantManagement.Id,
                MenuButtonType = "button",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region [6] Set as Inactive
            MenuButton InActiveCustomerButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "InActiveCustomer",
                Index = 6,
                IsActive = true,
                LabelTextCodeCode = "Customer.B.InActive",
                LabelTextCodeDefaultText = "Set as Inactive",
                ObjectTableId = customerObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = customerMenuButtonGroup.Id,
                ParentMenuButtonId = CustomerMoreButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = CustomerFeature_InActive.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region [7] ReActivate
            MenuButton ReActivateCustomerButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ReActivateCustomer",
                Index = 7,
                IsActive = true,
                LabelTextCodeCode = "Customer.B.ReActivate",
                LabelTextCodeDefaultText = "Reactivate",
                ObjectTableId = customerObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = customerMenuButtonGroup.Id,
                ParentMenuButtonId = CustomerMoreButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = CustomerFeature_ReActivate.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region [8] Set as Customer
            MenuButton SetMyCustomerButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "SetMyCustomer",
                Index = 8,
                IsActive = true,
                LabelTextCodeCode = "Customer.B.SetMyCustomer",
                LabelTextCodeDefaultText = "Set as Customer",
                ObjectTableId = customerObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = customerMenuButtonGroup.Id,
                ParentMenuButtonId = CustomerMoreButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = CustomerFeature_MyCustomer.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region [9] Set as Potential
            MenuButton SetAsPotentialButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "SetAsPotential",
                Index = 9,
                IsActive = true,
                LabelTextCodeCode = "Customer.B.SetAsPotential",
                LabelTextCodeDefaultText = "Set as Potential",
                ObjectTableId = customerObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = customerMenuButtonGroup.Id,
                ParentMenuButtonId = CustomerMoreButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = CustomerFeature_SetAsPotential.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region [10] Set as Foreign Client
            MenuButton SetNotMyCustomerButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "SetNotMyCustomer",
                Index = 10,
                IsActive = true,
                LabelTextCodeCode = "Customer.B.SetNotMyCustomer",
                LabelTextCodeDefaultText = "Set as Foreign Client",
                ObjectTableId = customerObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = customerMenuButtonGroup.Id,
                ParentMenuButtonId = CustomerMoreButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = CustomerFeature_NotMyCustomer.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region [11] View Questionnaire Answers
            MenuButton ViewQuestionnaireAnswersButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ViewQuestionnaireAnswersCustomer",
                Index = 11,
                IsActive = true,
                LabelTextCodeCode = "Customer.B.ViewQuestionnaireAnswers",
                LabelTextCodeDefaultText = "View Questionnaire Answers",
                ObjectTableId = customerObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = customerMenuButtonGroup.Id,
                ParentMenuButtonId = CustomerMoreButton.Id,
                MenuButtonType = "menuitem",
                FeatureId = CustomerFeature_ViewQuestionnaireAnswerCustomer.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region [12]Create Tenant
            MenuButton CreateTenantButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "CreateTenant",
                Index = 12,
                IsActive = true,
                LabelTextCodeCode = "Customer.B.CreateTenant",
                LabelTextCodeDefaultText = "Create Tenant",
                Tenant = tenant,
                MenuButtonType = "button",
                MenuButtonGroupId = customerMenuButtonGroup.Id,
                ObjectTableId = customerObject.Id,
                FeatureId = CustomerFeature_Ready.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #endregion

            #region MessagingStock

            MenuButtonGroup MessagingStockMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "MessagingStockEdit",
                Name = "MessagingStockEditButtonsGroup",
                ObjectTableId = MessagingStockTableId,
                Tenant = tenant,
            }, MenuButtonGroupRepository, TenantMenuButtonGroups);

            MenuButton MessagingStockMoreButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Actions",
                Index = 100,
                LabelTextCodeCode = "MessagingStock.B.More",
                LabelTextCodeDefaultText = "More",
                Tenant = tenant,
                MenuButtonGroupId = MessagingStockMenuButtonGroup.Id,
                ObjectTableId = MessagingStockTableId,
                MenuButtonType = "dropdownbutton",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);

            MenuButton MessagingStockMenuItem1 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Cancel",
                Index = 2,
                IsActive = true,
                LabelTextCodeCode = "MessagingStock.B.Cancel",
                LabelTextCodeDefaultText = "Cancel",
                ObjectTableId = MessagingStockTableId,
                Tenant = tenant,
                MenuButtonGroupId = MessagingStockMenuButtonGroup.Id,
                ParentMenuButtonId = MessagingStockMoreButton.Id,
                FeatureId = MessagingStockFeature_Cancel.Id,
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);

            #endregion

            #region ApiLogs
            ObjectTable apiLogsObject = ObjectContext.ObjectTables.Where(d => d.Name == "APILogs" && d.Tenant == tenant).FirstOrDefault();
            MenuButtonGroup apiLogsMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "ApiLogsEdit",
                Name = "ApiLogsEditButtonsGroup",
                ObjectTableId = apiLogsObject.Id,
                Tenant = tenant,
            }, MenuButtonGroupRepository, TenantMenuButtonGroups);

            #region actions button
            MenuButton ApiActionButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Actions",
                Index = 100,
                IsActive = false,
                LabelTextCodeCode = "ApiLogs.B.Actions",
                LabelTextCodeDefaultText = "More",
                Tenant = tenant,
                MenuButtonGroupId = apiLogsMenuButtonGroup.Id,
                ObjectTableId = apiLogsObject.Id,
                MenuButtonType = "dropdownbutton",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion 
            MenuButton ReturnToQueueButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "ReturnToQueue",
                Index = 2,
                IsActive = true,
                LabelTextCodeCode = "ApiLogs.B.ReturnToQueue",
                LabelTextCodeDefaultText = "Return To Queue",
                ObjectTableId = apiLogsObject.Id,
                Tenant = tenant,
                MenuButtonGroupId = apiLogsMenuButtonGroup.Id,
                ParentMenuButtonId = ApiActionButton.Id,  
                MenuButtonType = "menuitem",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #region CustomerTenantAccess
            ObjectTable CustomerTenantAccessObject = ObjectContext.ObjectTables.Where(d => d.Name == "CustomerTenantAccess" && d.Tenant == tenant).FirstOrDefault();
            MenuButtonGroup CustomerTenantAccessMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "CustomerTenantAccessEdit",
                Name = "CustomerTenantAccessEditButtonsGroup",
                ObjectTableId = CustomerTenantAccessObject.Id,
                Tenant = tenant,
            }, MenuButtonGroupRepository, TenantMenuButtonGroups);

            #region Deny button
            MenuButton DenyButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Deny",
                Index = 100,
                IsActive = true,
                LabelTextCodeCode = "CustomerTenantAccess.B.Deny",
                LabelTextCodeDefaultText = "Deny",
                Tenant = tenant,
                MenuButtonGroupId = CustomerTenantAccessMenuButtonGroup.Id,
                ObjectTableId = CustomerTenantAccessObject.Id,
                MenuButtonType = "button"
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

            #endregion

            #region Tenant Management           
            MenuButtonGroup TenantManagementMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "TenantManagementEdit",
                Name = "TenantManagementEditButtonsGroup",
                ObjectTableId = TenantManagementTableId,
                Tenant = tenant,
            }, MenuButtonGroupRepository, TenantMenuButtonGroups);
            
            MenuButton EraseDataButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "EraseData",
                Index = 1,
                IsActive = true,
                LabelTextCodeCode = "TenantManagement.B.EraseData",
                LabelTextCodeDefaultText = "Erase Data",
                Tenant = tenant,
                MenuButtonType = "button",
                MenuButtonGroupId = TenantManagementMenuButtonGroup.Id,
                ObjectTableId = TenantManagementTableId,
                FeatureId = TenantManagementFeature_EraseData.Id,
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);

            #endregion

            #region AnalyzeQueue
            ObjectTable AnalyzeQueueObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AnalyzeQueue").FirstOrDefault();
            FeaturePM AnalyzeQueue_ResendFeature = features.Where(d => d.Code == "RESEND" && d.ObjectTableId == AnalyzeQueueObjectTable.Id).FirstOrDefault();

            MenuButtonGroup AnalyzeQueue_MenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            {
                MenuButtonGroupType = "AnalyzeQueueEdit",
                Name = "AnalyzeQueueEditButtonsGroup",
                ObjectTableId = AnalyzeQueueObjectTable.Id,
                Tenant = tenant,
            }, MenuButtonGroupRepository, TenantMenuButtonGroups);

            MenuButton AnalyzeQueue_MenuButton_Resend = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            {
                EventCode = "Resend",
                Index = 0,
                IsActive = true,
                LabelTextCodeCode = "AnalyzeQueue.B.Resend",
                LabelTextCodeDefaultText = "Resend",
                ObjectTableId = AnalyzeQueueObjectTable.Id,
                Tenant = tenant,
                MenuButtonGroupId = AnalyzeQueue_MenuButtonGroup.Id,
                FeatureId = AnalyzeQueue_ResendFeature.Id,
                MenuButtonType = "button",
            }, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);

            #endregion

            MenuButtonGroupRepository.SubmitChanges();
            TextCodeRepository.SubmitChanges();
            MenuButtonRepository.SubmitChanges();
            MenuButtonGroupQuery menuButtonGroupQuery = new MenuButtonGroupQuery(MenuButtonGroupRepository);
            return menuButtonGroupQuery.GetMenuButtonGroupPMsByTenant(tenant);
        }
    }
}