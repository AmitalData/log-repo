declare var window: any;
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { ARInvoicePM } from '../../EntityPMs/ARInvoicePM';
import { ARInvoiceLinePM } from '../../EntityPMs/ARInvoiceLinePM';
import { MenuButtonPM } from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { InvoiceTool, CreditLimitHelper } from '../../Tools';
import { ARInvoiceValidator } from '../../Validators/ARInvoiceValidator';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { InvoiceDomainService } from '../../Services/InvoiceDomainService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { GeneralPrintHelper } from '../../../Infrastructure/Helpers/GeneralPrintHelper';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';
import { ARInvoicePMService } from '../../Services/StandardPMs/ARInvoicePMService';
import { BatchTaskExecutionList } from '../../../Infrastructure/EntityLists/BatchTaskExecutionList';
import { BatchTaskExecutionListService } from '../../../Infrastructure/Services/StandardLists/BatchTaskExecutionListService';
import { DocumentsFilingExtendedPMService } from '../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import { DownloadManager } from '../../../Infrastructure/Utilities/DownloadManager';
import { ConsilidationInvoiceDomainService } from '../../Services/ConsilidationInvoiceDomainService';
import { ShipmentDomainService } from '../../../Shipment/Services/ShipmentDomainService';
import { escapeLeadingUnderscores } from 'typescript';
import { BuildDocumentComponent } from 'Accounting/Utilities/BuildDocumentComponent';
export class ARInvoiceMenuButtonsHandler {
    private CurrentSession = SessionLocator.SelectedSession;
    public EntityPM: ARInvoicePM;
    public entityArgs: EntityArgs
    private isRunningBatchTaskExecution: boolean = false;

    private IsConfirmationMessageForCriedtNoteVisible:boolean=false;
    private menuButtonClicked: MenuButtonPM;

    DocumentsFilingExtendedPMService: DocumentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();

    //private RelativeRateDate: String; 

    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }

    private CheckIsConfirmationMessageForCriedtNoteVisible() {
        this.IsConfirmationMessageForCriedtNoteVisible = FeatureLocator.HasFeaturePermession("ARInvoice", "ConfirmationForAutoCreditForCreditNotes") ? true : false;
    }
    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    var myButtonIsDisabled = false;


                    switch (button.EventCode) {
                        case "SaveAsDraft": {
                            if (this.EntityPM.ARInvoiceTypeCode == 'IT') {
                                myButtonIsDisabled = true;
                                button.IsHidden = true;
                            }
                            else {


                                myButtonIsDisabled = !InvoiceTool.IsEditingARInvoiceEnabled(this.EntityPM);

                                if (this.EntityPM.IsConstituentInvoice) {
                                    if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                                        if (this.EntityPM.IsAutoCredit) {
                                            myButtonIsDisabled = false;
                                        }
                                    }
                                }

                                button.LabelTextCodeCode = (this.EntityPM.IsConstituentInvoice) ? "General.B.Save" : "ARInvoice.B.SaveAsDraft";
                            }
                            break;
                        }

                        case "CancelDraft": {


                            if (SessionLocator.TenantPM.AccountingActivated == true) {
                                button.IsHidden = true;
                            }
                            else {
                                button.IsHidden = this.EntityPM.IsConstituentInvoice ? true : false;

                                myButtonIsDisabled = true;
                                if (!AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                                    if (this.EntityPM.StatusCode != "LL") {
                                        if (this.EntityPM.IsConstituentInvoice) {
                                            if (AppTool.IsNullOrEmpty(this.EntityPM.ConsolidationInvoiceId) && this.EntityPM.StatusCode == "NT") {
                                                myButtonIsDisabled = false;
                                            }
                                        }

                                        else if (this.EntityPM.StatusCode == "DR") {
                                            myButtonIsDisabled = false;
                                        }
                                    }
                                }
                            }

                            break;
                        }

                        case "SaveAndApprove": {
                            button.IsHidden = this.EntityPM.IsConstituentInvoice ? true : false;
                            myButtonIsDisabled = !InvoiceTool.IsEditingARInvoiceEnabled(this.EntityPM);

                            if (AppTool.IsNullOrEmpty(this.EntityPM.Id) && this.EntityPM.IsAutoCredit) {
                                myButtonIsDisabled = false;
                            }
                            break;
                        }

                        case "SetAsSent": {
                            if (SessionLocator.TenantPM.AccountingActivated == true) {
                                button.IsHidden = true;
                            }
                            else {
                                if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                                    myButtonIsDisabled = true;
                                }

                                else if (this.EntityPM.Sent || this.EntityPM.StatusCode == "VD" || this.EntityPM.StatusCode == "LL") {
                                    myButtonIsDisabled = true;
                                }
                            }
                            break;
                        }

                        case "VoidARInvoice": {
                            if (SessionLocator.TenantPM.AccountingActivated == true) {
                                button.IsHidden = true;
                            }
                            else if (this.EntityPM.ARInvoiceTypeCode == 'IT') {
                                myButtonIsDisabled = true;
                            }
                            else {
                                if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                                    myButtonIsDisabled = true;
                                }

                                else if (this.EntityPM.StatusCode == "DR" || this.EntityPM.StatusCode == "VD" || this.EntityPM.StatusCode == "LL" || this.EntityPM.StatusCode == "AC" || this.EntityPM.StatusCode == "AR" || this.EntityPM.StatusCode == "IP") {
                                    myButtonIsDisabled = true;
                                }

                                else {
                                    if (this.EntityPM.IsConstituentInvoice) {
                                        if (!AppTool.IsNullOrEmpty(this.EntityPM.ConsolidationInvoiceId)) {
                                            myButtonIsDisabled = true;
                                        }
                                    }

                                    else {
                                        if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode)) {
                                            myButtonIsDisabled = true;
                                        }
                                    }
                                }
                            }

                            break;
                        }

                        case "AutoCredit": {
                            if (this.EntityPM.ARInvoiceTypeCode == 'IT') {
                                myButtonIsDisabled = true;
                            }
                            else {
                                if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                                    myButtonIsDisabled = true;
                                }

                                else if (this.EntityPM.StatusCode == "LL" || this.EntityPM.StatusCode == "VD" || this.EntityPM.StatusCode == "AC" || this.EntityPM.StatusCode == "AR") {
                                    myButtonIsDisabled = true;
                                }

                                else if (this.EntityPM.ARInvoiceTypeCode == "IN") {
                                    var isEnabled = false;

                                    if (!this.EntityPM.IsCancelled) {
                                        if (this.EntityPM.IsConstituentInvoice) {
                                            if (AppTool.IsNullOrEmpty(this.EntityPM.ConsolidationInvoiceId)) {
                                                isEnabled = true;
                                            }
                                        }

                                        else {
                                            if (this.EntityPM.StatusCode == "PP" || this.EntityPM.StatusCode == "PD" || this.EntityPM.StatusCode == "AD") {
                                                isEnabled = true;
                                            }
                                        }
                                    }

                                    myButtonIsDisabled = !isEnabled;

                                }

                                else if (this.EntityPM.ARInvoiceTypeCode == "CC") {
                                    myButtonIsDisabled = true;
                                }

                                if (SessionLocator.TenantPM.AccountingActivated == true) {
                                    if (this.EntityPM != null && this.EntityPM.IsExternalEntity) {
                                        myButtonIsDisabled = true;
                                    }
                                }
                            }
                            // myButtonIsDisabled = false;  
                            break;
                        }

                        case "PrintInvoice": {
                            myButtonIsDisabled = true;

                            if (this.EntityPM.Id != null) {
                                myButtonIsDisabled = false;

                                if (!this.EntityPM.IsConsolidationInvoice) {
                                    //this.PrintInvoiceButtonLoaded();
                                }
                            }

                            break;
                        }

                        case "ReTransfer": {
                            if (SessionLocator.TenantPM.AccountingActivated == true) {
                                button.IsHidden = true;
                            }
                            else {
                                myButtonIsDisabled = true;
                                button.IsHidden = this.EntityPM.IsConstituentInvoice;
                                if (!AppTool.IsNullOrEmpty(this.EntityPM.Id) && !AppTool.IsNullOrEmpty(this.EntityPM.StatusCode)) {
                                    if (this.EntityPM.StatusCode != "DR" && this.EntityPM.StatusCode != "VD") {
                                        if (this.EntityPM.TransferStatusCode == "TR") {
                                            myButtonIsDisabled = false;
                                        }
                                    }
                                }
                            }

                            break;
                        }

                        case "CheckSATStatus": {
                            let sATCanceledStatusCode = "CS";
                            myButtonIsDisabled = this.EntityPM.SATTransferStatusCode != sATCanceledStatusCode;
                            button.IsHidden = SessionLocator.SATInterfaceSettings.SATInterfaceCode == "NONE" || AppTool.IsNullOrEmpty(this.EntityPM.SATXML);
                            break;
                        }

                        case "SendToQBO": {
                            if (SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG") {
                                button.IsHidden = false;
                            }
                            else {
                                button.IsHidden = true;
                            }
                            if (this.EntityPM.TransferStatusCode == "RD" || this.EntityPM.TransferStatusCode == "NR") {
                                button.DisplayText = "Send to QBO";
                            }
                            else if (this.EntityPM.TransferStatusCode == "TR" || this.EntityPM.TransferStatusCode == "ET" || this.EntityPM.TransferStatusCode == "IP") {
                                button.LabelTextCodeCode = null;
                                button.DisplayText = "Resend to QBO";
                            }
                            if (this.EntityPM.ApprovedDate == null) {
                                if (this.EntityPM.ARInvoiceTypeCode == "CD" || this.EntityPM.ARInvoiceTypeCode == "CC")
                                    myButtonIsDisabled = false;
                                else
                                    myButtonIsDisabled = true;
                            }
                            else {
                                myButtonIsDisabled = false;
                            }

                            break;
                        }

                        case "ResendToSAT": {
                            button.IsHidden = this.IsHiddenResendToSAT();
                            button.DisplayText = "Resend to SAT";

                            break;
                        }

                        case "SolvedManual": {
                            const sATTransferWithErrorStatusCode: string = "TE";
                            const sATSolvedManualStatusCode: string = "SM";
                            button.IsHidden = this.EntityPM.SATTransferStatusCode != sATTransferWithErrorStatusCode && this.EntityPM.SATTransferStatusCode != sATSolvedManualStatusCode;
                            if (!button.IsHidden) myButtonIsDisabled = this.EntityPM.SATTransferStatusCode == sATSolvedManualStatusCode;

                            break;
                        }

                        case "InvoiceOperationsSeparator":
                        case "VoidARInvoiceOperationsSeparator": {
                            if (SessionLocator.TenantPM.AccountingActivated == true) {
                                button.IsHidden = true;

                            }
                            break;
                        }

                        case "BlockFromTransfer": {
                            var isHidden: boolean = true;

                            if (this.EntityPM.TransferStatusCode == "ET") {
                                if (SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG") {
                                    isHidden = false;
                                }
                            }

                            button.IsHidden = isHidden;
                            break;
                        }
                    }

                    button.IsDisabled = myButtonIsDisabled;
                }
            }
        }
    }

    private IsHiddenResendToSAT(): boolean {
        const SATTransferWithErrorStatusCode: string = "TE";
        const SATNotTransferedStatusCode: string = "NT";
        const SATCancelErrorsInRelationReasonCode: string = "01";
        const profact4SATInterfaceCode: string = "PROF40";

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode != profact4SATInterfaceCode) return true;
        if (this.EntityPM.SATCancelReasonCode == SATCancelErrorsInRelationReasonCode && this.EntityPM.SATTransferStatusCode == SATNotTransferedStatusCode) return false;
        if (AppTool.IsNullOrEmpty(this.EntityPM.TransmissionError)) return true;

        return this.EntityPM.SATTransferStatusCode != SATTransferWithErrorStatusCode;
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {
        if (!this.isButtonClicked) {

            this.StopFlags();
            this.StopFields();
            this.isButtonClicked = true;
            this.menuButtonClicked = menuButton;

            switch (menuButton.EventCode) {
                case "SaveAsDraft": {
                    this.SaveDraftClicked();
                    break;
                }

                case "SaveAndApprove": {
                    this.ApproveClicked();
                    break;
                }

                case "CancelDraft": {
                    this.CancelDraftClicked();
                    break;
                }

                case "SetAsSent": {
                    this.SetAsSentClicked();
                    break;
                }

                case "VoidARInvoice": {
                    this.ShowInvoiceCancelaReason();
                    break;
                }

                case "AutoCredit": {
                    this.AutoCreditClicked();
                    break;
                }

                case "PrintInvoice": {
                    this.PrintClicked();
                    break;
                }

                case "ReTransfer": {
                    this.ReTransferClicked();
                    break;
                }

                case "CheckSATStatus": {
                    this.CheckSATStatus();
                    break;
                }

                case "SendToQBO": {
                    this.SendToQBO();
                    break;
                }

                case "ResendToSAT": {
                    this.ResendToSAT();
                    break;
                }

                case "BlockFromTransfer": {
                    this.BlockFromTransferToQBO();
                    break;
                }

                case "SolvedManual": {
                    this.SolvedManual();
                    break;
                }
            }
        }
    }

    CheckSATStatus() {
        let invoiceDomainService: InvoiceDomainService = new InvoiceDomainService();
        invoiceDomainService.GetARInvoiceSATCancellationStatus(this.EntityPM.Id).subscribe((response: any) => {
            if (response.HasError) return;
            this.entityArgs?.EditComponent?.ReloadEntityPM();
        });
    }


    SendToQBO() {

        var invoiceDomainService: InvoiceDomainService = new InvoiceDomainService();
        invoiceDomainService.getConnectedARPayments(this.EntityPM.Id).subscribe((response: any) => {
            if (!response.HasError) {
                if (this.EntityPM.TransferStatusCode == "TR" || this.EntityPM.TransferStatusCode == "ET" || this.EntityPM.TransferStatusCode == "IP") {
                    var messageText: string = "Resend this invoice to QBO?";
                    if (response.Result) {
                        messageText = messageText.concat(" Please note that any connected Transferred payments will be resend after the successful transfer of this invoice");
                    }
                    var myConfirmWindow = new ConfirmWindow();
                    myConfirmWindow.Width = 400;
                    myConfirmWindow.Show(messageText);
                    myConfirmWindow.WindowClosed.subscribe(s => {
                        this.StopFlags();
                        if (myConfirmWindow.Yes) {
                            this.SendToQBOApproved("Resending Invoice to QBO");

                        }
                    });
                }
                else {
                    if (response.Result) {
                        var messageText: string = "Please note that any connected Transferred payments will be resend after the successful transfer of this invoice";
                        var myConfirmWindow = new ConfirmWindow();
                        myConfirmWindow.Width = 400;
                        myConfirmWindow.Show(messageText);
                        myConfirmWindow.WindowClosed.subscribe(s => {
                            this.StopFlags();
                            if (myConfirmWindow.Yes) {
                                this.SendToQBOApproved("Sending Invoice to QBO");
                            }
                        });
                    }
                    else {
                        this.SendToQBOApproved("Sending Invoice to QBO");
                    }
                    this.StopFlags();
                }
            }
        });
    }

    ResendToSAT() {
        this.EntityPM.ResendToSAT = true;
        this.EntityPM.SetVoided = false;
        this.EntityPM.SetApproved = false;
        this.EntityPM.SetReTransfer = false;
        this.EntityPM.SetCancelDraft = false;
        this.EntityPM.SATTransferStatusCode = this.GetSATTransferStatusCode(false);
        this.entityArgs.EditComponent.SaveChanges("Resending Invoice to SAT");
    }

    private GetSATTransferStatusCode(IsSetVoid) {
        const SATCancelErrorsInRelationReasonCode: string = "01";
        const SATNotTransferedStatusCode: string = "NT";
        const SATInTransferStatusCode: string = "TG";

        if (this.EntityPM.SATCancelReasonCode == SATCancelErrorsInRelationReasonCode && AppTool.IsNullOrEmpty(this.EntityPM.RelatedInvoice)) {
            return SATNotTransferedStatusCode;
        }

        if (IsSetVoid) {
            return this.EntityPM.SATTransferStatusCode;
        }

        return SATInTransferStatusCode;
    }

    SolvedManual() {
        this.EntityPM.SATTransferStatusCode = "SM";
        //this.EntityPM.TransmissionError = ""; // or ng if
        this.entityArgs.EditComponent.SaveChanges("Solved Manual");
    }

    SendToQBOApproved(Text: string) {
        this.EntityPM.SetReSendQBO = true;
        this.EntityPM.SetVoided = false;
        this.EntityPM.SetApproved = false;
        this.EntityPM.SetReTransfer = false;
        this.EntityPM.SetCancelDraft = false;
        this.entityArgs.EditComponent.SaveChanges(Text);
    }

    isValid: boolean = false;
    isButtonClicked: boolean = false;
    isPrintRequested: boolean = false;
    addDocumentFilling: boolean = false;

    StopFlags() {
        this.isButtonClicked = false;
        this.isPrintRequested = false;
    }
    StopFields() {
        this.AutoCreditId = null;
        this.AutoCreditDate = null;
        this.AutoCreditManualNumber = null;
    }
    Validate() {
        var validator = new ARInvoiceValidator();
        var errors: string[] = validator.Validate(this.EntityPM);

        this.isValid = errors.length == 0 ? true : false;

        if (this.entityArgs.EditComponent != null) {
            if (this.entityArgs.EditComponent.ValidationErrorsList == null) {
                this.entityArgs.EditComponent.ValidationErrorsList = [];
            }
            this.entityArgs.EditComponent.ValidationErrorsList = errors;
        }

        if (!this.isValid) {
            this.StopFlags();
        }
    }
    Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {

                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                    if (this.isPrintRequested) {
                        var isShowPrintWindow:Boolean = this.menuButtonClicked.EventCode == "PrintInvoice";
                        this.InitializePrinting(true,isShowPrintWindow);
                    }
                    else {
                        if (this.addDocumentFilling)
                            this.InitializePrinting(false);
                    }
                    if (this.isRunningBatchTaskExecution) {

                        this.isRunningBatchTaskExecution = false;

                        //if (this.EntityPM.BatchTaskExecutionId) {

                        //    this.CurrentSession.StartBusyIndicator("Updating Shipments. It may take a few minutes...");

                        //    this.CheckBatchTaskExecution(this.EntityPM.BatchTaskExecutionId);
                        //}
                    }
                }

                this.StopFlags();
            });
        }

        this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {

            if (isLoadSuccess) {
                this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                if (this.isPrintRequested && this.IsHaveARInvoicePrintToogleFeature()) {
                    this.InitializePrinting(true,false);
                }
                if (this.IsAutoCreditConsolidation) {
                    this.IsAutoCreditConsolidation = false;

                    this.CurrentSession.FireEvent("ResetARInvoiceBaseDeailsTab");
                }
            }

            this.StopFlags();
        });
    }

    SaveDraftClicked() {
        if (!FeatureLocator.HasEntityPermessions("ARInvoice", "UPDT", true)) {
            this.StopFlags();
        }

        else {
            this.Validate();

            if (this.isValid) {
                if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                    ServiceLocator.SendTotangoUserActivity("ARInvoice", "New Draft Invoice");
                }

                if (this.EntityPM.IsDirty) {
                    this.EntityPM.SetVoided = false;
                    this.EntityPM.SetApproved = false;
                    this.EntityPM.SetReTransfer = false;
                    this.EntityPM.SetCancelDraft = false;
                    this.EntityPM.SetReSendQBO = false;
                    this.entityArgs.EditComponent.SaveChanges();
                }

                else {
                    this.StopFlags();
                }
            }
        }
    }

    IsHaveARInvoicePrintToogleFeature() {
        return SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "ARP")[0];
    }

    ApproveClicked() {

        if (!FeatureLocator.HasEntityPermessions("ARInvoice", "UPDT", true)) {
            this.StopFlags();
        }

        else {
            this.Validate();

            if (this.isValid) {
                if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                    ServiceLocator.SendTotangoUserActivity("ARInvoice", "New Invoice");
                }

                var helper = new CreditLimitHelper(this.EntityPM);

                //var isCardBlockingNewInvoiceCreation: boolean = false;
                //if (helper.HasCreditLimitFeature && helper.IsCreditLimitActivated) {
                //    if (this.EntityPM.BillToIsCreditLimitEnabled && this.EntityPM.BillToBlockNewInvoiceCreation) {
                //        isCardBlockingNewInvoiceCreation = true;
                //    }
                //}

                if (this.EntityPM.BillToBlockNewInvoiceCreation) {

                    var errorText_Blocking = "Credit limit setting is blocking invoice for bill to: " + this.EntityPM.BillToName;

                    var errors: string[] = [];
                    var warnings: string[] = [];

                    if (ObjectsLocator.CreditLimitSettingPM.InvoiceCreationBlock) {
                        errors.push(errorText_Blocking);
                    }

                    else if (ObjectsLocator.CreditLimitSettingPM.InvoiceCreationWarning) {
                        warnings.push(errorText_Blocking);
                    }

                    if (errors.length > 0 || warnings.length > 0) {

                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 450;
                        logWindow.Height = 200;
                        logWindow.Title = TextCodeTranslator.Translate("ARInvoice.S.CreditLimit");
                        logWindow.WindowArgs = { Errors: errors, Warnings: warnings };

                        logWindow.WindowClosed.subscribe(s => {
                            if (s) {
                                this.ApplyApproveClicked();
                            }

                            else {
                                this.StopFlags();
                            }
                        });

                        logWindow.Show('./Invoice/Components/NewEntity/CreditLimitPopupComponent');
                    }

                    else {
                        this.ApplyApproveClicked();
                    }
                }

                else if (helper.IsActivated) {

                    this.CurrentSession.StartBusyIndicatorLoading();

                    var myService = new InvoiceDomainService();
                    myService.GetCustomerCreditLimitActualAmount(this.EntityPM.BillToId, this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {

                        this.CurrentSession.StopBusyIndicator();

                        if (!myResponse.HasError) {
                            helper.Run(myResponse.Result);

                            if (helper.IsValid) {
                                this.ApplyApproveClicked();
                            }

                            else {

                                var logWindow = new LogitudeWindow();
                                logWindow.Width = 450;
                                logWindow.Height = 200;
                                logWindow.Title = TextCodeTranslator.Translate("ARInvoice.S.CreditLimit");
                                logWindow.WindowArgs = { Errors: helper.Errors, Warnings: helper.Warnings, IsBlockingShipment: helper.IsBlockingShipment, ShipmentId: this.EntityPM.MainEntityId };
                                logWindow.WindowClosed.subscribe(s => {
                                    if (s) {
                                        this.ApplyApproveClicked();
                                    }

                                    else {
                                        this.StopFlags();
                                    }
                                });

                                logWindow.Show('./Invoice/Components/NewEntity/CreditLimitPopupComponent');
                            }
                        }
                    });
                }

                else {
                    this.ApplyApproveClicked();
                }
            }

            else {
                this.StopFlags();
            }
        }
    }

    ComputeRelativeRateDate() {
        return DateTool.GetRelativeRateDate(this.EntityPM.InvoiceDate, this.EntityPM.ExchangeRateDate, "old");
    }

    ApplyApproveClicked() {

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode !== "NONE") {
            this.CheckVendorExpenseCharges();
        }
        else {
            this.CheckAutoCreditInvoice();
        }
    }

    private CheckVendorExpenseCharges() {
        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode != "PROF40") {
            this.CheckExchageRateLastUpdate();
            return;
        }

        if (!SessionLocator.SATInterfaceSettings.TransferExpenseCharges) {
            this.CheckExchageRateLastUpdate();
            return;
        }

        if (!this.EntityPM.InvoiceLines.some(item => item.IsExpense)) {
            this.CheckExchageRateLastUpdate();
            return;
        }

        this.CurrentSession.StartBusyIndicatorLoading();
        let shipmentDomainService = new ShipmentDomainService();
        shipmentDomainService.GetShipmentReceivablePMsByShipmentId(this.EntityPM.MainEntityId, SessionLocator.Tenant).subscribe((serviceResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (serviceResponse.HasError) {
                this.CheckExchageRateLastUpdate();
                return;
            }

            if (!this.HasExpenseLineWithoutPayableVendor(serviceResponse.Result)) {
                this.CheckExchageRateLastUpdate();
                return;
            }
            this.OpenExpenseLineWithoutPayableVendorWarningWindow();
        });
    }

    private HasExpenseLineWithoutPayableVendor(allShipmentReceivables) {
        let hasExpenseLineWithoutPayableVendor = false;
        this.EntityPM.InvoiceLines.forEach(line => {
            hasExpenseLineWithoutPayableVendor = hasExpenseLineWithoutPayableVendor ? hasExpenseLineWithoutPayableVendor : allShipmentReceivables.some(receivable => (receivable.IsExpense || receivable.IsExpenseCharge) && receivable.Id == line.ReceivableId && AppTool.IsNullOrEmpty(receivable.PayableVendorId));
        });

        return hasExpenseLineWithoutPayableVendor;
    }

    private OpenExpenseLineWithoutPayableVendorWarningWindow() {
        const confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 350;
        confirmWindow.ShowCancelButton = true;
        confirmWindow.CancelButtonText = "Cancel";
        confirmWindow.ShowNoButton = false;
        confirmWindow.YesButtonText = "Continue";
        confirmWindow.ShowWarningImage = true;
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.Warning");
        confirmWindow.Show("Some of the Expense charges don't have Vendor, do you want to proceed approving?");

        confirmWindow.WindowClosed.subscribe(event => {
            if (confirmWindow.Yes) {
                this.CheckExchageRateLastUpdate();
            }
            else if (confirmWindow.Cancel) {
                this.StopFlags();
            }
        });
    }

    private CheckExchageRateLastUpdate() {

        if (this.ComputeRelativeRateDate()) {
            const confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 290;
            confirmWindow.ShowCancelButton = true;
            confirmWindow.CancelButtonText = "Cancel";
            confirmWindow.ShowNoButton = false;
            confirmWindow.YesButtonText = "Continue";
            confirmWindow.ShowWarningImage = true;
            confirmWindow.Title = TextCodeTranslator.Translate("General.O.Warning");
            confirmWindow.Show("The invoice exchange rate is not up-to-date.");

            confirmWindow.WindowClosed.subscribe(s => {
                if (confirmWindow.Yes) {
                    this.CheckAutoCreditInvoice();
                }
                else if (confirmWindow.Cancel) {
                    // nth
                    this.StopFlags();
                }
            });
        }
        else {
            this.CheckAutoCreditInvoice();
        }
    }

    private CheckAutoCreditInvoice() {
        if (this.EntityPM.IsAutoCredit) {
            this.CheckIsConfirmationMessageForCriedtNoteVisible();
            const myConfirmWindow = new ConfirmWindow();
            myConfirmWindow.Width = 400;
            let Text = TextCodeTranslator.Translate("ARInvoice.M.ConfirmAutoCredit");
            if ((this.EntityPM.AutoCreditedByInvoiceTypeCode === "CD" || this.EntityPM.AutoCreditedByInvoiceTypeCode === "CC") && this.IsConfirmationMessageForCriedtNoteVisible) {
                Text = TextCodeTranslator.Translate("ARInvoice.M.ConfirmAutoCreditForAutoCredit");
            }
            myConfirmWindow.Show(Text);
            myConfirmWindow.WindowClosed.subscribe(s => {
                if (myConfirmWindow.Yes) {
                    this.ValidateApprovalSendToSAT(TextCodeTranslator.Translate("ARInvoice.M.CreatingAutoCredit"));
                }
            });

        }
        else
            this.ValidateApprovalSendToSAT("Approving...");
    }

    ValidateApprovalSendToSAT(ProceedToApproveMessage) {
        if (!SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "AVC")[0]) {
            this.ProceedToApprove(ProceedToApproveMessage);
            return;
        }
        this.CurrentSession.StartBusyIndicatorLoading();
        let invoiceDomainService = new InvoiceDomainService();
        invoiceDomainService.ValidateApprovalSendToSAT(this.EntityPM).subscribe((serviceResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (serviceResponse.HasError) {
                this.ProceedToApprove(ProceedToApproveMessage);
                return;
            }

            if (!this.HasApprovalSendToSATValidateError(serviceResponse.Result)) {
                this.ProceedToApprove(ProceedToApproveMessage);
                return;
            }
            this.OpenValidateApprovalSendToSATErrorWindow(serviceResponse.Result, ProceedToApproveMessage);
        });
    }

    private HasApprovalSendToSATValidateError(result) {
        if (result.IsValidToSendToSAT) return false;
        if (result.CorrectedARInvoiceTrasladoLines?.length > 0) return true;
        if (result.CorrectedARInvoiceRetencionLines?.length > 0) return true;
        if (result.CorrectedARInvoiceRetencionDRLines?.length > 0) return true;

        return false;
    }

    private OpenValidateApprovalSendToSATErrorWindow(result, ProceedToApproveMessage) {
        let logWindow = new LogitudeWindow();
        logWindow.Width = 490;
        logWindow.Height = 180;
        logWindow.Title = "Adjustments to VAT Amounts";
        logWindow.WindowArgs = { ARInvoicePM: this.EntityPM, ARInvoiceLinesVATAmountsAdjustments: result };
        logWindow.Show('./Invoice/Components/SAT/ARInvoiceLinesVATAmountsAdjustmentsComponent');
        logWindow.WindowClosed.subscribe(s => {
            if (s != "UpdateInvoice") {
                this.StopFlags();
                return;
            }

            this.ProceedToApprove(ProceedToApproveMessage);
        });

    }


    ProceedToApprove(msg: string) {
        
        this.EntityPM.SetVoided = false;
        this.EntityPM.SetApproved = true;
        this.EntityPM.SetReTransfer = false;
        this.EntityPM.SetCancelDraft = false;
        this.EntityPM.SetReSendQBO = false;

        if (this.EntityPM.IsConsolidationInvoice) {
            this.SaveConsolidation(msg);
            //this.isRunningBatchTaskExecution = true;
        }

        else {
            if (this.IsHaveARInvoicePrintToogleFeature()) this.isPrintRequested = true;
            else { this.addDocumentFilling = true }
            this.entityArgs.EditComponent.SaveChanges(msg);
        }
        // this.InitializePrinting(false);
    }

    CancelDraftClicked() {
        this.Validate();

        if (this.isValid) {
            var myConfirmWindow = new ConfirmWindow();
            myConfirmWindow.Width = 400;
            myConfirmWindow.Show(TextCodeTranslator.Translate("ARInvoice.M.CancelARInvoice"));
            myConfirmWindow.WindowClosed.subscribe(s => {

                this.StopFlags();

                if (myConfirmWindow.Yes) {
                    this.EntityPM.SetVoided = false;
                    this.EntityPM.SetApproved = false;
                    this.EntityPM.SetReTransfer = false;
                    this.EntityPM.SetCancelDraft = true;

                    this.entityArgs.EditComponent.SaveChanges();
                }
            });
        }

        else {
            this.StopFlags();
        }
    }
    SetAsSentClicked() {
        this.Validate();

        if (this.isValid) {
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Title = TextCodeTranslator.Translate("ARInvoice.S.SetInvoiceAsSent");
            logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, EventCode: "SetAsSent" };
            logitudeWindow.Show("./Invoice/Components/MenuButtonsComponents/ARInvoiceMenuButtonsComponent");
            logitudeWindow.WindowClosed.subscribe(s => {

                this.StopFlags();

                if (s) {
                    this.EntityPM.Sent = true;
                    this.EntityPM.SetAsSent = true;
                    this.entityArgs.EditComponent.SaveChanges();
                }
            });
        }

        else {
            this.StopFlags();
        }
    }
    VoidClicked() {

        var isQuickBooks: boolean = false;
        var isTransferingToQuickBooks: boolean = false;

        if (SessionLocator.AccountingSystemPM.Code == "QBO" || SessionLocator.AccountingSystemPM.Code == "QBOG") {
            isQuickBooks = true;
            isTransferingToQuickBooks = true;

            if (this.EntityPM.StatusCode == null || this.EntityPM.StatusCode == "DR") {
                isTransferingToQuickBooks = false;
            }

            else if (this.EntityPM.TransferStatusCode == "ET") {
                isTransferingToQuickBooks = false;
            }
        }

        if (this.EntityPM.TransferStatusCode == "TR" && SessionLocator.AccountingSettingPM.AccountingSystemCode != "QBO" && SessionLocator.AccountingSettingPM.AccountingSystemCode != "QBOG") {
            var messageWindow = new MessageWindow();
            messageWindow.Show(TextCodeTranslator.Translate("ARInvoice.M.AlreadyTransferredInvoicesMsg"));
            this.StopFlags();
        }

        else if (!SessionLocator.AccountingSettingPM.AllowVoidARI) {
            var messageWindow = new MessageWindow();
            messageWindow.Show(TextCodeTranslator.Translate("ARInvoice.M.AccountingSettingsDontAllowVoid"));
            this.StopFlags();
        }

        else if (this.EntityPM.InvoicePayments.length > 0) {
            var messageWindow = new MessageWindow();
            messageWindow.Show(TextCodeTranslator.Translate("ARInvoice.M.DisconnectPayments"));
            this.StopFlags();
        }

        else {
            this.Validate();

            if (this.isValid) {

                if ((this.EntityPM.ARInvoiceTypeCode == "CD" || this.EntityPM.ARInvoiceTypeCode == "CC") && isQuickBooks && isTransferingToQuickBooks) {
                    var messageWindow = new MessageWindow();
                    messageWindow.Show("Please notice that QBO are not supporting void transmission for the credit note, you can void it manually from QBO");
                    messageWindow.WindowClosed.subscribe(p => {
                        this.AccountingCheck();
                    });
                }

                else {
                    this.AccountingCheck();
                }
            }

            else {
                this.StopFlags();
            }
        }
    }



    ShowInvoiceCancelaReason() {

        if (this.EntityPM.SATTransferStatusCode != "TD" || SessionLocator.SATInterfaceSettings.SATInterfaceCode != "PROF40") {
            this.VoidClicked();
            return;
        }

        var logWindow = new LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 170;
        logWindow.Title = "Void";
        logWindow.Show('./Invoice/Components/MenuButtons/ARInvoice/ARInvoiceCancellationReasionComponent');

        logWindow.WindowClosed.subscribe(cancelReason => {
            this.SetSATCancelReason(cancelReason);
        });
    }

    ShowBlockVoidingInvoicesInSATMessage() {
        var messageWindow = new MessageWindow();
        messageWindow.Show(TextCodeTranslator.Translate("ARInvoice.M.BlockVoidingInvoicesInSAT"));
        this.StopFlags();
    }

    private SetSATCancelReason(cancelReason: any) {
        if (cancelReason == "Reject") {
            this.StopFlags();
            return;
        }

        this.EntityPM.SATCancelReasonCode = cancelReason;
        this.EntityPM.SATTransferStatusCode = this.GetSATTransferStatusCode(true);
        this.VoidClicked();
    }

    AccountingCheck() {
        if (this.EntityPM.MainEntityId) {
            this.CurrentSession.StartBusyIndicatorLoading();

            var myService = new InvoiceDomainService();
            myService.GetShipmentIsAccountingClosed(this.EntityPM.MainEntityId).subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                if (!myResponse.HasError) {
                    var IsAccountingClosed = myResponse.Result;

                    if (IsAccountingClosed) {
                        var messageWindow = new MessageWindow();
                        messageWindow.Show("Shipment is closed for accounting, it is not possible to perform this action");
                        this.StopFlags();
                    }

                    else {
                        this.VoidClickedProccess();
                    }
                }

                else {
                    this.StopFlags();
                }
            });
        }

        else {
            this.VoidClickedProccess();
        }
    }
    VoidClickedProccess() {
        this.ShowConfirmVoidMessage();
    }

    ShowConfirmVoidMessage() {
        var myConfirmWindow = new ConfirmWindow();
        myConfirmWindow.Width = 400;
        myConfirmWindow.Show(TextCodeTranslator.Translate("ARInvoice.M.ConfirmVoid"));
        myConfirmWindow.WindowClosed.subscribe(s => {
            this.StopFlags();
            if (myConfirmWindow.Yes) {
                this.EntityPM.SetVoided = true;
                this.EntityPM.SetApproved = false;
                this.EntityPM.SetReTransfer = false;
                this.EntityPM.SetCancelDraft = false;
                this.EntityPM.SetReSendQBO = false;

                if (this.EntityPM.IsConsolidationInvoice) {
                    this.SaveConsolidation("Voiding...");
                    //this.isRunningBatchTaskExecution = true;
                }

                else {
                    this.entityArgs.EditComponent.SaveChanges("Voiding...");
                }
            }
        });
    }
    ReTransferClicked() {
        this.Validate();

        if (this.isValid) {
            this.EntityPM.SetVoided = false;
            this.EntityPM.SetApproved = false;
            this.EntityPM.SetReTransfer = true;
            this.EntityPM.SetCancelDraft = false;
            this.EntityPM.SetReSendQBO = false;
            this.entityArgs.EditComponent.SaveChanges();
        }

        else {
            this.StopFlags();
        }
    }

    // AutoCredit
    IsAutoCreditConsolidation: boolean = false;
    AutoCreditId: string = null;
    AutoCreditDate: Date = null;
    AutoCreditManualNumber: string = null;
    AutoCreditClicked() {

        if (this.EntityPM.InvoicePayments.length > 0) {
            var messageWindow = new MessageWindow();
            messageWindow.Show(TextCodeTranslator.Translate("ARInvoice.S.AutoCreditingMsg1"));
            this.StopFlags();
        }

        else if (this.EntityPM.StatusCode == "PD" || this.EntityPM.StatusCode == "PP") {
            var messageWindow = new MessageWindow();
            messageWindow.Show(TextCodeTranslator.Translate("ARInvoice.S.AutoCreditingMsg4"));
            this.StopFlags();
        }

        else {
            this.Validate();

            if (this.isValid) {
                if (this.EntityPM.MainEntityId) {
                    this.CurrentSession.StartBusyIndicatorLoading();

                    var myService = new InvoiceDomainService();
                    myService.GetShipmentIsAccountingClosed(this.EntityPM.MainEntityId).subscribe((myResponse: ServiceResponse) => {

                        this.CurrentSession.StopBusyIndicator();

                        if (!myResponse.HasError) {
                            var IsAccountingClosed = myResponse.Result;

                            if (IsAccountingClosed) {
                                var messageWindow = new MessageWindow();
                                messageWindow.Show("Shipment is closed for accounting, it is not possible to perform this action");
                                this.StopFlags();
                            }

                            else {
                                this.AutoCreditClickedProccess();
                            }
                        }

                        else {
                            this.StopFlags();
                        }
                    });
                }

                else {
                    this.AutoCreditClickedProccess();
                }
            }

            else {
                this.StopFlags();
            }
        }
    }
    AutoCreditClickedProccess() {
        var newAutoCreditInvoice: ARInvoicePM = this.CreateAutoCreditInvoice();

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityPM: newAutoCreditInvoice, ObjectTableName: 'ARInvoice', BackButtonLabel: "ARInvoice" + ": " + this.EntityPM.InvoiceNumber });

                let isEditComponentSaved = false;

                cmpRef.instance.BackCompleted.subscribe(bk => {
                    if (isEditComponentSaved) {

                        if (this.EntityPM.IsConsolidationInvoice) {
                            this.IsAutoCreditConsolidation = true;
                        }

                        this.entityArgs.EditComponent.IsReloadNeeded = true;
                        this.entityArgs.EditComponent.ReloadEntityPM();
                    }

                    else if (cmpRef.instance.NeedRefresh) {
                        this.entityArgs.EditComponent.IsReloadNeeded = true;
                        this.entityArgs.EditComponent.ReloadEntityPM();
                    }

                    else {
                        this.StopFlags();
                    }
                });

                cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        isEditComponentSaved = true;
                    }
                });

                cmpRef.instance.SaveAndCloseCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        isEditComponentSaved = true;
                    }
                });
            });
    }
    CreateAutoCreditInvoice(): ARInvoicePM {

        var note: string = this.EntityPM.ARInvoiceTypeCode == "CD" ? TextCodeTranslator.Translate("ARInvoice.O.CreditARInvoiceForCreditNote") : TextCodeTranslator.Translate("ARInvoice.O.AutoCreditInvoice");
        var myEntityPMService: ARInvoicePMService = new ARInvoicePMService()
        var AutoCreditInvoice: ARInvoicePM = myEntityPMService.GetNewEntityPM();
        AutoCreditInvoice.StatusCode = "AC";
        AutoCreditInvoice.StatusName = "Auto Credit";
        AutoCreditInvoice.IsAutoCredit = true;
        AutoCreditInvoice.ARInvoiceTypeCode = this.EntityPM.ARInvoiceTypeCode == "CI" ? "CC" : "CD";
        AutoCreditInvoice.DebitAccount = this.EntityPM.DebitAccount;
        AutoCreditInvoice.TransferStatusCode = this.EntityPM.TransferStatusCode;
        AutoCreditInvoice.BillToAddressId = this.EntityPM.BillToAddressId;
        AutoCreditInvoice.PartnerId = this.EntityPM.PartnerId;
        AutoCreditInvoice.BillToId = this.EntityPM.BillToId;
        AutoCreditInvoice.InternalNotes = note.replace("%InvoiceNumber", this.EntityPM.InvoiceNumber);// this.EntityPM.InternalNotes;
        AutoCreditInvoice.InvoiceCurrencyExchangeRate = this.EntityPM.InvoiceCurrencyExchangeRate;
        AutoCreditInvoice.InvoiceCurrencyId = this.EntityPM.InvoiceCurrencyId;
        AutoCreditInvoice.InvoiceCurrencyCode = this.EntityPM.InvoiceCurrencyCode;
        AutoCreditInvoice.PrintNotes = note.replace("%InvoiceNumber", this.EntityPM.InvoiceNumber);// this.EntityPM.PrintNotes;
        AutoCreditInvoice.PaymentTermId = this.EntityPM.PaymentTermId;
        AutoCreditInvoice.PrepaidCollectId = this.EntityPM.PrepaidCollectId;
        AutoCreditInvoice.LocalCurrencyId = this.EntityPM.LocalCurrencyId;
        AutoCreditInvoice.VatNumber = this.EntityPM.VatNumber;
        AutoCreditInvoice.CreatedByUserId = SessionLocator.LoggedUserId;
        AutoCreditInvoice.IssuedByUserId = SessionLocator.LoggedUserId;
        AutoCreditInvoice.PrintByUserId = this.EntityPM.PrintByUserId;
        AutoCreditInvoice.InvoiceDate = this.AutoCreditDate != null ? this.AutoCreditDate : DateTool.GetCurrentDateAsUtc();
        AutoCreditInvoice.DueDate = this.EntityPM.DueDate;
        AutoCreditInvoice.PrintDate = this.EntityPM.PrintDate;
        AutoCreditInvoice.Sent = this.EntityPM.Sent;
        AutoCreditInvoice.ExchangeRateDate = this.EntityPM.ExchangeRateDate;
        AutoCreditInvoice.BranchId = this.EntityPM.BranchId;
        AutoCreditInvoice.ExpectedPaymentDate = this.EntityPM.ExpectedPaymentDate;
        AutoCreditInvoice.ProfitCurrencyId = this.EntityPM.ProfitCurrencyId;
        AutoCreditInvoice.ProfitCurrencyCode = this.EntityPM.ProfitCurrencyCode;
        AutoCreditInvoice.ProfitCurrencyExchangeRate = this.EntityPM.ProfitCurrencyExchangeRate;
        AutoCreditInvoice.MainEntityId = this.EntityPM.MainEntityId;
        AutoCreditInvoice.MainEntityReference = this.EntityPM.MainEntityReference;
        AutoCreditInvoice.MainEntityStatus = this.EntityPM.MainEntityStatus;
        AutoCreditInvoice.AccountingExternalCode = this.EntityPM.AccountingExternalCode;
        AutoCreditInvoice.IsConstituentInvoice = this.EntityPM.IsConstituentInvoice;
        AutoCreditInvoice.IsConsolidationInvoice = this.EntityPM.IsConsolidationInvoice;
        AutoCreditInvoice.SubTotalInInvoiceCurrency = this.EntityPM.SubTotalInInvoiceCurrency * -1;
        AutoCreditInvoice.SubTotalInLocalCurrency = this.EntityPM.SubTotalInLocalCurrency * -1;
        AutoCreditInvoice.AmountInInvoiceCurrency = this.EntityPM.AmountInInvoiceCurrency * -1;
        AutoCreditInvoice.AmountInLocalCurrency = this.EntityPM.AmountInLocalCurrency * -1;
        AutoCreditInvoice.AmountInProfitCurrency = this.EntityPM.AmountInProfitCurrency * -1;
        AutoCreditInvoice.AmountDue = 0;
        AutoCreditInvoice.AmountDueInLocalCurrency = 0;
        AutoCreditInvoice.AmountDueInProfitCurrency = 0;
        AutoCreditInvoice.CreditedByARInvoiceId = this.EntityPM.Id;
        AutoCreditInvoice.AutoCreditByARInvoiceNumber = this.EntityPM.InvoiceNumber;
        AutoCreditInvoice.AutoCreditByARInvoiceId = this.EntityPM.Id;
        AutoCreditInvoice.IsGeneralInvoice = this.EntityPM.IsGeneralInvoice;
        AutoCreditInvoice.SalesmanUserId = this.EntityPM.SalesmanUserId;
        AutoCreditInvoice.SATPaymentMethodCode = this.EntityPM.SATPaymentMethodCode;
        AutoCreditInvoice.MetodoPagoCode = this.EntityPM.MetodoPagoCode;
        AutoCreditInvoice.IsInvoiceNumberFromStock = this.EntityPM.IsInvoiceNumberFromStock;
        AutoCreditInvoice.IsInvoiceNumberManuallySet = this.EntityPM.IsInvoiceNumberManuallySet;
        AutoCreditInvoice.AutoCreditedByInvoiceTypeCode = this.EntityPM.ARInvoiceTypeCode;
        AutoCreditInvoice.HasInterestFeature = this.EntityPM.ARInvoiceTypeCode == 'IT' ? true : false;
        AutoCreditInvoice.RegionalTaxId = this.EntityPM.RegionalTaxId;
        AutoCreditInvoice.RegionalTaxPercentage = this.EntityPM.RegionalTaxPercentage;
        this.CreateAutoCreditInvoiceLines(AutoCreditInvoice);
        return AutoCreditInvoice;
    }
    CreateAutoCreditInvoiceLines(AutoCreditInvoice: ARInvoicePM) {
        var index: number = 1;
        this.EntityPM.InvoiceLines.forEach(item => {
            var newInvoiceLine: ARInvoiceLinePM = new ARInvoiceLinePM(AutoCreditInvoice);
            newInvoiceLine.Tenant = item.Tenant;
            newInvoiceLine.ChargesTypeId = item.ChargesTypeId;
            newInvoiceLine.CreditAccount = item.CreditAccount;
            newInvoiceLine.Description = item.Description;
            newInvoiceLine.ForiegnCurrencyId = item.ForiegnCurrencyId;
            newInvoiceLine.ForiegnExchangeRate = item.ForiegnExchangeRate;
            newInvoiceLine.VatTypeId = item.VatTypeId;
            newInvoiceLine.LineNumber = index;
            newInvoiceLine.MeasurementId = item.MeasurementId;
            newInvoiceLine.EntityId = item.EntityId;
            newInvoiceLine.EntityReference = item.EntityReference;
            newInvoiceLine.ViewOrder = item.ViewOrder;
            newInvoiceLine.ExternalTAXItemId = item.ExternalTAXItemId;
            newInvoiceLine.ExternalVATCard = item.ExternalVATCard;
            newInvoiceLine.ForiegnCurrencyCode = item.ForiegnCurrencyCode;
            newInvoiceLine.InvoiceCurrencyCode = item.InvoiceCurrencyCode;
            newInvoiceLine.InvoiceLocalCurrencyCode = item.InvoiceLocalCurrencyCode;
            newInvoiceLine.MeasurementCode = item.MeasurementCode;
            newInvoiceLine.VatTypeName = item.VatTypeName;
            newInvoiceLine.IsExchangeRateFixed = item.IsExchangeRateFixed;
            newInvoiceLine.LocalDescription = item.LocalDescription;
            newInvoiceLine.PrepaidCollectId = item.PrepaidCollectId;
            newInvoiceLine.VatPercentage = item.VatPercentage;
            newInvoiceLine.Quantity = item.Quantity;
            newInvoiceLine.UnitPrice = item.UnitPrice * -1;
            newInvoiceLine.ForiegnCurrencyAmount = item.ForiegnCurrencyAmount * -1;
            newInvoiceLine.LocalCurrencyAmount = item.LocalCurrencyAmount * -1;
            newInvoiceLine.ProfitCurrencyAmount = item.ProfitCurrencyAmount * -1;
            newInvoiceLine.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount * -1;
            newInvoiceLine.IsExpense = item.IsExpense;
            newInvoiceLine.GLAccountId = item.GLAccountId;
            newInvoiceLine.LineActionCode = "1";
            newInvoiceLine.IsRegionalTax = item.IsRegionalTax;
            AutoCreditInvoice.AddARInvoiceLinePM(newInvoiceLine);
            index++;
        });
    }

    // Print
    PrintClicked() {
        
        this.Validate();

        if (this.isValid) {
            this.isPrintRequested = true;
            this.entityArgs.EditComponent.SaveChanges();
        }

        else {
            this.StopFlags();
        }
    }

    InitializePrinting(showController: boolean = true,ShowPrintWindow:Boolean = true) {
        
        this.addDocumentFilling=false

        var myEntityId: string = null;
        var myChildEntityId: string = null;
        var myObjectTableName: string = null;
        var mychildObjectTableId: string = null;
        var myDocumentTypeCode: string = null;
        var myReference: string = null;
        var StatusCode: string = null;
        var ApprovedDate: Date = null;

        if (this.EntityPM != null) {
            StatusCode = this.EntityPM.StatusCode;
            ApprovedDate = this.EntityPM.ApprovedDate;
        }


        if (this.EntityPM.IsConsolidationInvoice) {
            myEntityId = this.EntityPM.Id;
            myChildEntityId = null;
            mychildObjectTableId = null;
            myObjectTableName = "ARInvoice";
            myDocumentTypeCode = "999C";
            myReference = !AppTool.IsNullOrEmpty(this.EntityPM.InvoiceNumber) ? this.EntityPM.InvoiceNumber : "Draft: " + this.EntityPM.DraftNumber;

            this.StartPrinting(myEntityId, myChildEntityId, myObjectTableName, mychildObjectTableId, myDocumentTypeCode, myReference, StatusCode, ApprovedDate, showController,ShowPrintWindow);

        }
        else if (this.EntityPM.IsGeneralInvoice) {

            myEntityId = this.EntityPM.Id;
            myChildEntityId = null;
            mychildObjectTableId = null;
            myObjectTableName = "ARInvoice";
            myDocumentTypeCode = "999G";
            myReference = !AppTool.IsNullOrEmpty(this.EntityPM.InvoiceNumber) ? this.EntityPM.InvoiceNumber : "Draft: " + this.EntityPM.DraftNumber;

            if (SessionLocator.TenantPM.AccountingActivated == true) {
                this.PrintFullAccountingInvoice(showController);
            }
            else {


                this.StartPrinting(myEntityId, myChildEntityId, myObjectTableName, mychildObjectTableId, myDocumentTypeCode, myReference, StatusCode, ApprovedDate, showController,ShowPrintWindow);


            }
        }
        else {

            mychildObjectTableId = window.ObjectTables.filter(d => d.Name == "ARInvoice")[0].Id;
            myEntityId = this.EntityPM.MainEntityId;
            myChildEntityId = this.EntityPM.Id;
            myReference = !AppTool.IsNullOrEmpty(this.EntityPM.InvoiceNumber) ? this.EntityPM.InvoiceNumber : "Draft: " + this.EntityPM.DraftNumber;

            if (this.EntityPM.ARInvoiceTypeCode == "MN") {
                myObjectTableName = "Master";
                myDocumentTypeCode = "999M";

                this.StartPrinting(myEntityId, myChildEntityId, myObjectTableName, mychildObjectTableId, myDocumentTypeCode, myReference, StatusCode, ApprovedDate, showController,ShowPrintWindow);

            }

            else if (this.EntityPM.ARInvoiceTypeCode == "CI" || this.EntityPM.ARInvoiceTypeCode == "CC") {
                myObjectTableName = "Shipment";
                myDocumentTypeCode = "999CI";

                this.StartPrinting(myEntityId, myChildEntityId, myObjectTableName, mychildObjectTableId, myDocumentTypeCode, myReference, StatusCode, ApprovedDate, showController,ShowPrintWindow);

            }

            else {
                this.CurrentSession.StartBusyIndicatorLoading();

                var myService = new InvoiceDomainService();
                myService.GetShipmentLevelCode(this.EntityPM.MainEntityId).subscribe((myResponse: ServiceResponse) => {

                    this.CurrentSession.StopBusyIndicator();

                    if (!myResponse.HasError) {
                        var myShipmentLevelCode = myResponse.Result;
                        myObjectTableName = myShipmentLevelCode == "C" ? "Master" : "Shipment";
                        myDocumentTypeCode = "999S";


                        this.StartPrinting(myEntityId, myChildEntityId, myObjectTableName, mychildObjectTableId, myDocumentTypeCode, myReference, StatusCode, ApprovedDate, showController,ShowPrintWindow);

                    }
                });
            }
        }
    }

    StartPrinting(myEntityId: string, myChildEntityId: string, myObjectTableName: string, mychildObjectTableId: string, myDocumentTypeCode: string, myReference: string, StatusCode: string, ApprovedDate: Date = null, showController: boolean = true,ShowPrintWindow:Boolean = true) {

        
        var myPrintHelper = new GeneralPrintHelper(myObjectTableName, myDocumentTypeCode, myEntityId, myChildEntityId, myReference, mychildObjectTableId);
        if (myPrintHelper.IsLoadPrintControl) {
            ServiceLocator.SendTotangoUserActivity("ARInvoice", "PrintInvoice");


            myPrintHelper.ShowPrintControl(this.EntityPM.DocumentTemplateId, StatusCode, ApprovedDate, showController,ShowPrintWindow);



        }
    }
    GetDocument() {



        this.DocumentsFilingExtendedPMService.getDocumentsFilingsById(this.EntityPM.DocumentFilingId).subscribe((myResult: any) => {

            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                var documentFiling = mm.Result;
                if (documentFiling) {
                    var securityIds = documentFiling.SecurityId;
                    DownloadManager.DownloadPage(null, securityIds);
                }

            }
        });


    }
    private PrintFullAccountingInvoice(showController: boolean = true) {
        var StatusCode: string = null;
        var ApprovedDate: Date = null;
        if (this.EntityPM != null) {
            StatusCode = this.EntityPM.StatusCode;
            ApprovedDate = this.EntityPM.ApprovedDate;
        }

        if (this.EntityPM.IsExternalEntity) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.DocumentFilingId)) {
                this.ShowWarnigMessageForMissingDocument();
            }

            else {
                this.GetDocument();
            }
        }
        else {
            this.StartPrinting(this.EntityPM.Id, null, "ARInvoice", null, "999G", !AppTool.IsNullOrEmpty(this.EntityPM.InvoiceNumber) ? this.EntityPM.InvoiceNumber : "Draft: " + this.EntityPM.DraftNumber, StatusCode, ApprovedDate, showController);
        }
    }

    private ShowWarnigMessageForMissingDocument() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 290;
        confirmWindow.ShowCancelButton = false;
        confirm
        confirmWindow.ShowNoButton = false;
        confirmWindow.YesButtonText = TextCodeTranslator.Translate("ARInvoice.B.Ok");
        confirmWindow.ShowWarningImage = true;
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.Warning");
        confirmWindow.Show(TextCodeTranslator.Translate("ARInvoice.O.MissingDocument"));
    }

    private savedConsolidationEntity;
    SaveConsolidation(msg: string) {
        
        this.entityArgs.EditComponent.StartBusyIndicator(msg);

        var myService = new ConsilidationInvoiceDomainService();

        myService.post(this.EntityPM).subscribe((response: ServiceResponse) => {

            this.entityArgs.EditComponent.StopBusyIndicator();

            if (response.HasError) {
                this.entityArgs.EditComponent.ValidationErrorsList = response.ErrorsArray;
            }

            else {
                
                this.savedConsolidationEntity = response.Result;
                this.StopFlags();

                if (response.Result['BatchTaskExecutionId']) {

                    this.entityArgs.EditComponent.StartBusyIndicator("Updating Shipments...");

                    this.StopTimer();

                    this.timer = setInterval(() => {
                        this.CheckBatchTaskExecution(this.EntityPM.BatchTaskExecutionId);
                    }, this.timerInterval);

                }
                else if (this.IsHaveARInvoicePrintToogleFeature()) this.InitializePrinting(true,false);
            }
        });
    }

    timer: any;
    timerInterval: number = 1000;
    StopTimer() {
        if (this.timer) {
            clearInterval(this.timer);
        }
    }

    CheckBatchTaskExecution(BatchTaskExecutionId: string) {

        var iBatchService: BatchTaskExecutionListService = new BatchTaskExecutionListService();

        iBatchService.getSingle(BatchTaskExecutionId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: BatchTaskExecutionList = myResponse.Result;

                if (list.StatusCode == "D") {

                    this.StopTimer();

                    this.EntityPM = this.savedConsolidationEntity;

                    this.entityArgs.EditComponent.EntityId = this.EntityPM.Id;
                    this.entityArgs.EditComponent.EntityPM = this.EntityPM;
                    this.entityArgs.EditComponent.NeedRefresh = true;
                    this.entityArgs.EditComponent.StopBusyIndicator();
                    if (this.IsHaveARInvoicePrintToogleFeature()) this.isPrintRequested = true;
                    this.entityArgs.EditComponent.ReloadEntityPM();
                }

                else if (list.StatusCode == "F") {
                    this.StopTimer();
                    this.entityArgs.EditComponent.StopBusyIndicator();

                    var errors: string[] = [];
                    errors.push(list.ErrorLog);
                    this.entityArgs.EditComponent.ValidationErrorsList = errors;
                }

                else {
                    this.entityArgs.EditComponent.StopBusyIndicator();
                    this.entityArgs.EditComponent.StartBusyIndicator("Updating Shipments... " + list.ProgressPercentage + "%");
                }
            }

            else {
                this.StopTimer();
                this.entityArgs.EditComponent.StopBusyIndicator();
                this.entityArgs.EditComponent.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    }

    BlockFromTransferToQBO() {

        this.Validate();

        if (this.isValid) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Show("Please make sure that you've created the record manually at QBO online before marking as 'blocked for transfer', it is recommended to fix any issues and resend from the communication log rather than marking as blocked");
            confirmWindow.WindowClosed.subscribe(s => {

                this.StopFlags();

                if (confirmWindow.Yes) {
                    this.EntityPM.TransferStatusCode = "BL";
                    this.EntityPM.TransferStatusName = "Blocked";
                    this.entityArgs.EditComponent.SaveChanges("Blocking...");
                }
            });
        }
    }
}
