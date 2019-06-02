declare var window: any;
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { ARInvoicePM } from '../../EntityPMs/ARInvoicePM';
import { ARInvoiceLinePM } from '../../EntityPMs/ARInvoiceLinePM';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {InvoiceTool, CreditLimitHelper} from '../../Tools';
import {ARInvoiceValidator}  from '../../Validators/ARInvoiceValidator';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {InvoiceDomainService} from '../../Services/InvoiceDomainService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {GeneralPrintHelper} from '../../../Infrastructure/Helpers/GeneralPrintHelper';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
import { ARInvoicePMService } from '../../Services/StandardPMs/ARInvoicePMService';

export class ARInvoiceMenuButtonsHandler {
    private CurrentSession = SessionLocator.SelectedSession;
    public EntityPM: ARInvoicePM;
    public entityArgs: EntityArgs
    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }
    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    var myButtonIsDisabled = false;

                    switch (button.EventCode) {
                        case "SaveAsDraft":
                            {
                                myButtonIsDisabled = !InvoiceTool.IsEditingARInvoiceEnabled(this.EntityPM);
                                button.LabelTextCodeCode = (this.EntityPM.IsConstituentInvoice) ? "General.B.Save" : "ARInvoice.B.SaveAsDraft";

                                //if()

                                break;
                            }

                        case "CancelDraft":
                            {
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

                                break;
                            }

                        case "SaveAndApprove":
                            {
                                button.IsHidden = this.EntityPM.IsConstituentInvoice ? true : false;
                                myButtonIsDisabled = !InvoiceTool.IsEditingARInvoiceEnabled(this.EntityPM);

                                if (AppTool.IsNullOrEmpty(this.EntityPM.Id) && this.EntityPM.IsAutoCredit) {
                                    myButtonIsDisabled = false;
                                }
                                break;
                            }

                        case "SetAsSent":
                            {
                                if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                                    myButtonIsDisabled = true;
                                }

                                else if (this.EntityPM.Sent || this.EntityPM.StatusCode == "VD" || this.EntityPM.StatusCode == "LL") {
                                    myButtonIsDisabled = true;
                                }

                                break;
                            }

                        case "VoidARInvoice":
                            {
                                if (SessionLocator.TenantPM.AccountingActivated == true) {
                                    button.IsHidden = true;
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

                        case "AutoCredit":
                            {
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

                                break;
                            }

                        case "PrintInvoice":
                            {
                                myButtonIsDisabled = true;

                                if (this.EntityPM.Id != null) {
                                    myButtonIsDisabled = false;

                                    if (!this.EntityPM.IsConsolidationInvoice) {
                                        //this.PrintInvoiceButtonLoaded();
                                    }
                                }

                                break;
                            }

                        case "ReTransfer":
                            {
                                myButtonIsDisabled = true;
                                button.IsHidden = this.EntityPM.IsConstituentInvoice;
                                if (!AppTool.IsNullOrEmpty(this.EntityPM.Id) && !AppTool.IsNullOrEmpty(this.EntityPM.StatusCode)) {
                                    if (this.EntityPM.StatusCode != "DR" && this.EntityPM.StatusCode != "VD") {
                                        if (this.EntityPM.TransferStatusCode == "TR") {
                                            myButtonIsDisabled = false;
                                        }
                                    }
                                }

                                break;
                            }

                        case "CheckSATStatus":
                            {
                                if (this.EntityPM.SATTransferStatusCode == "CS") {
                                    myButtonIsDisabled = false;
                                }
                                else {
                                    myButtonIsDisabled = true;
                                }
                                if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "NONE") {
                                    button.IsHidden = true;
                                }


                                break;
                            }

                        case "SendToQBO":
                            {
                                if (SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG" ) {
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
                    }

                    button.IsDisabled = myButtonIsDisabled;
                }
            }
        }
    }
    public MenuButtonClick(menuButton: MenuButtonPM) {
        if (!this.isButtonClicked) {

            this.StopFlags();
            this.StopFields();
            this.isButtonClicked = true;

            switch (menuButton.EventCode) {
                case "SaveAsDraft":
                    {
                        this.SaveDraftClicked();
                        break;
                    }

                case "SaveAndApprove":
                    {
                        this.ApproveClicked();
                        break;
                    }

                case "CancelDraft":
                    {
                        this.CancelDraftClicked();
                        break;
                    }

                case "SetAsSent":
                    {
                        this.SetAsSentClicked();
                        break;
                    }

                case "VoidARInvoice":
                    {
                        this.VoidClicked();
                        break;
                    }

                case "AutoCredit":
                    {
                        this.AutoCreditClicked();
                        break;
                    }

                case "PrintInvoice":
                    {
                        this.PrintClicked();
                        break;
                    }

                case "ReTransfer":
                    {
                        this.ReTransferClicked();
                        break;
                    }
                case "CheckSATStatus":
                    {
                        this.CheckSATStatus();
                        break;
                    }

                case "SendToQBO":
                    {
                        this.SendToQBO();
                        break;
                    }
            }
        }
    }

    CheckSATStatus() {
        var invoiceDomainService: InvoiceDomainService = new InvoiceDomainService();
        invoiceDomainService.GetARInvoiceSATCancellationStatus(this.EntityPM.Id).subscribe(response => {

        });

    }


    SendToQBO() {

        var invoiceDomainService: InvoiceDomainService = new InvoiceDomainService();
        invoiceDomainService.getConnectedARPayments(this.EntityPM.Id).subscribe(response => {
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
                        this.InitializePrinting();
                    }
                }

                this.StopFlags();
            });
        }

        this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {

            if (isLoadSuccess) {
                this.EntityPM = this.entityArgs.EditComponent.EntityPM;

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
                    myService.GetCustomerCreditLimitActualAmount(this.EntityPM.BillToId).subscribe((myResponse: ServiceResponse) => {

                        this.CurrentSession.StopBusyIndicator();

                        if (!myResponse.HasError) {
                            helper.Run(myResponse.Result);
                        }

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
                    });
                }

                else {
                    this.ApplyApproveClicked();

                    if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                        var notes = "Ayman.!!!TST45@";

                        if (this.EntityPM.InternalNotes == notes && this.EntityPM.PrintNotes == notes) {
                            this.ApplyApproveClicked();
                        }
                    }
                }
            }

            else {
                this.StopFlags();
            }
        }
    }
    ApplyApproveClicked() {
        if (this.EntityPM.IsAutoCredit) {
            var myConfirmWindow = new ConfirmWindow();
            myConfirmWindow.Width = 400;
            myConfirmWindow.Show(TextCodeTranslator.Translate("ARInvoice.M.ConfirmAutoCredit"));
            myConfirmWindow.WindowClosed.subscribe(s => {
                if (myConfirmWindow.Yes) {
                    this.ProceedToApprove(TextCodeTranslator.Translate("ARInvoice.M.CreatingAutoCredit"));                    
                }
            });
        }

        else {
            this.ProceedToApprove("Approving...");
        }
    }
    ProceedToApprove(msg: string) {
        this.EntityPM.SetVoided = false;
        this.EntityPM.SetApproved = true;
        this.EntityPM.SetReTransfer = false;
        this.EntityPM.SetCancelDraft = false;
        this.EntityPM.SetReSendQBO = false;
        this.entityArgs.EditComponent.SaveChanges(msg);
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

                if ((this.EntityPM.ARInvoiceTypeCode == "CD" || this.EntityPM.ARInvoiceTypeCode == "CC") && (SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG")) {
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
                this.entityArgs.EditComponent.SaveChanges("Voiding...");
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
        var myEntityPMService: ARInvoicePMService = new ARInvoicePMService()
        var AutoCreditInvoice: ARInvoicePM = myEntityPMService.GetNewEntityPM();
        AutoCreditInvoice.StatusCode = "AC";
        AutoCreditInvoice.StatusName = "Auto Credit";
        AutoCreditInvoice.IsAutoCredit = true;
        AutoCreditInvoice.ARInvoiceTypeCode = this.EntityPM.ARInvoiceTypeCode == "CI" ? "CC" : "CD";
        AutoCreditInvoice.DebitAccount = this.EntityPM.DebitAccount;
        AutoCreditInvoice.TransferStatusCode = this.EntityPM.TransferStatusCode;
        AutoCreditInvoice.BillToAddressId = this.EntityPM.BillToAddressId;
        AutoCreditInvoice.BillToId = this.EntityPM.BillToId;
        AutoCreditInvoice.InternalNotes = this.EntityPM.InternalNotes;
        AutoCreditInvoice.InvoiceCurrencyExchangeRate = this.EntityPM.InvoiceCurrencyExchangeRate;
        AutoCreditInvoice.InvoiceCurrencyId = this.EntityPM.InvoiceCurrencyId;
        AutoCreditInvoice.InvoiceCurrencyCode = this.EntityPM.InvoiceCurrencyCode;
        AutoCreditInvoice.PrintNotes = this.EntityPM.PrintNotes;
        AutoCreditInvoice.PaymentTermId = this.EntityPM.PaymentTermId;
        AutoCreditInvoice.PrepaidCollectId = this.EntityPM.PrepaidCollectId;
        AutoCreditInvoice.LocalCurrencyId = this.EntityPM.LocalCurrencyId;
        AutoCreditInvoice.VatNumber = this.EntityPM.VatNumber;
        AutoCreditInvoice.CreatedByUserId = this.EntityPM.CreatedByUserId;
        AutoCreditInvoice.IssuedByUserId = this.EntityPM.IssuedByUserId;
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
        AutoCreditInvoice.AutoCreditByARInvoiceNumber = this.EntityPM.InvoiceNumber
        AutoCreditInvoice.IsGeneralInvoice = this.EntityPM.IsGeneralInvoice;
        AutoCreditInvoice.SalesmanUserId = this.EntityPM.SalesmanUserId;
        AutoCreditInvoice.SATPaymentMethodCode = this.EntityPM.SATPaymentMethodCode;
        AutoCreditInvoice.MetodoPagoCode = this.EntityPM.MetodoPagoCode;
        AutoCreditInvoice.IsInvoiceNumberFromStock = this.EntityPM.IsInvoiceNumberFromStock;
        AutoCreditInvoice.IsInvoiceNumberManuallySet = this.EntityPM.IsInvoiceNumberManuallySet;

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
    InitializePrinting() {
        var myEntityId: string = null;
        var myChildEntityId: string = null;
        var myObjectTableName: string = null;
        var mychildObjectTableId: string = null;
        var myDocumentTypeCode: string = null;
        var myReference: string = null;

        if (this.EntityPM.IsConsolidationInvoice) {
            myEntityId = this.EntityPM.Id;
            myChildEntityId = null;
            mychildObjectTableId = null;
            myObjectTableName = "ARInvoice";
            myDocumentTypeCode = "999C";
            myReference = !AppTool.IsNullOrEmpty(this.EntityPM.InvoiceNumber) ? this.EntityPM.InvoiceNumber : "Draft: " + this.EntityPM.DraftNumber;
            this.StartPrinting(myEntityId, myChildEntityId, myObjectTableName, mychildObjectTableId, myDocumentTypeCode, myReference);
        }
        else if (this.EntityPM.IsGeneralInvoice) {
            myEntityId = this.EntityPM.Id;
            myChildEntityId = null;
            mychildObjectTableId = null;
            myObjectTableName = "ARInvoice";
            myDocumentTypeCode = "999G";
            myReference = !AppTool.IsNullOrEmpty(this.EntityPM.InvoiceNumber) ? this.EntityPM.InvoiceNumber : "Draft: " + this.EntityPM.DraftNumber;
            this.StartPrinting(myEntityId, myChildEntityId, myObjectTableName, mychildObjectTableId, myDocumentTypeCode, myReference);
        }
        else {

            mychildObjectTableId = window.ObjectTables.filter(d => d.Name == "ARInvoice")[0].Id;
            myEntityId = this.EntityPM.MainEntityId;
            myChildEntityId = this.EntityPM.Id;
            myReference = !AppTool.IsNullOrEmpty(this.EntityPM.InvoiceNumber) ? this.EntityPM.InvoiceNumber : "Draft: " + this.EntityPM.DraftNumber;

            if (this.EntityPM.ARInvoiceTypeCode == "MN") {
                myObjectTableName = "Master";
                myDocumentTypeCode = "999M";
                this.StartPrinting(myEntityId, myChildEntityId, myObjectTableName, mychildObjectTableId, myDocumentTypeCode, myReference);
            }

            else if (this.EntityPM.ARInvoiceTypeCode == "CI" || this.EntityPM.ARInvoiceTypeCode == "CC") {
                myObjectTableName = "Shipment";
                myDocumentTypeCode = "999CI";
                this.StartPrinting(myEntityId, myChildEntityId, myObjectTableName, mychildObjectTableId, myDocumentTypeCode, myReference);
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

                        this.StartPrinting(myEntityId, myChildEntityId, myObjectTableName, mychildObjectTableId, myDocumentTypeCode, myReference);
                    }
                });
            }
        }
    }
    StartPrinting(myEntityId: string, myChildEntityId: string, myObjectTableName: string, mychildObjectTableId: string, myDocumentTypeCode: string, myReference: string) {
        var myPrintHelper = new GeneralPrintHelper(myObjectTableName, myDocumentTypeCode, myEntityId, myChildEntityId, myReference, mychildObjectTableId);
        if (myPrintHelper.IsLoadPrintControl) {
            ServiceLocator.SendTotangoUserActivity("ARInvoice", "PrintInvoice");
            myPrintHelper.ShowPrintControl();
        }
    }
}
