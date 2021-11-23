import { GLAccountPMService } from './../../../Accounting/Services/StandardPMs/GLAccountPMService';
declare var window: any;
import {APPaymentPM} from '../../EntityPMs/APPaymentPM';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {APPaymentValidator} from '../../Validators/APPaymentValidator';
import {DocumentOutPM}  from '../../../Common/EntityPMs/DocumentOutPM';
import {DocumentTypePM} from '../../../Common/EntityPMs/DocumentTypePM';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {GeneralPrintHelper} from '../../../Infrastructure/Helpers/GeneralPrintHelper';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { FullAccountingSettingPM } from '../../../Accounting/EntityPMs/FullAccountingSettingPM';
import { FullAccountingSettingPMService } from '../../../Accounting/Services/StandardPMs/FullAccountingSettingPMService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { GLAccountPM } from '../../../Accounting/EntityPMs/GLAccountPM';
import { InvoiceTool } from '../../Tools';

import { reject } from 'q';
import { InvoiceDomainService } from '../../Services/InvoiceDomainService';

export class APPaymentMenuButtonsHandler {
    public EntityPM: APPaymentPM;
    public entityArgs: EntityArgs
    public customValidator: APPaymentValidator = new APPaymentValidator();
    ReconcileInternalTransIds:string;
    private isApproval: boolean;
    private isCancelApproval: boolean;
    private isVoided: boolean;
    private isPrintRequested: boolean;
    private CurrentSession = SessionLocator.SelectedSession;
    fullAccountingSettingPMService: FullAccountingSettingPMService = new FullAccountingSettingPMService();
    // private isFullAccountingGranted: boolean;
    private isOerationInProgrees: boolean = false;

    constructor(){
        // this.isFullAccountingGranted = this.GetFullAccountingFeature();
    }

    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }

    private ResetAllFlags() {
        this.isApproval = false;
        this.isCancelApproval = false;
        this.isVoided = false;
        this.isPrintRequested = false;
        this.isOerationInProgrees = false;
    }

    private isValid: boolean = false;
    Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    if(this.EntityPM.ReconcileInternalTransIds) {
                        this.ReconcileInternalTransIds = this.EntityPM.ReconcileInternalTransIds;
                    }
                    if (this.isApproval) {
                        this.isApproval = false;

                    }

                    if (this.isCancelApproval) {
                        this.isCancelApproval = false;
                    }

                    if (this.isVoided) {
                        this.isVoided = false;
                    }


                    if (this.isPrintRequested) {
                        this.isPrintRequested = false;
                        this.InitializePrinting();
                    }
                }

                this.ResetAllFlags();
            });
        }

        this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
            if (isLoadSuccess) {
                this.EntityPM = this.entityArgs.EditComponent.EntityPM;
            }

            this.ResetAllFlags();
        });
    }
    Validate() {
        var validator = new APPaymentValidator();
        var errors: string[] = validator.Validate(this.EntityPM);

        this.isValid = errors.length == 0 ? true : false;

        this.entityArgs.EditComponent.ValidationErrorsList = errors;

        if (!this.isValid) {
            this.ResetAllFlags();
        }
    }

    GetFullAccountingFeature(){
        var table = window.ObjectTables.filter(d => d.Name === 'FullAccountingSetting')[0];
        var fullAccountingFeature = FeatureLocator.Features.filter(f => (f.Code == "UPDATE") && f.ObjectTableId == table.Id)[0];
        return !!fullAccountingFeature;
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(d => d.Name === 'APPayment')[0];
                var isEditingAnabled = InvoiceTool.IsEditingAPPaymentEnabled(this.EntityPM);

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {

                        case "PrintAPPayment": {
                            this.PrintPaymentButtonLoaded();
                            if (this.EntityPM.Id == null && this.EntityPM.StatusCode == "VD") {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                            break;
                        }

                        case "ApproveAPPayment": {
                            if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR") {
                                button.IsDisabled = false;
                            }

                            else {
                                button.IsDisabled = true;
                            }
                            break;
                        }

                        case "CancelApproval": {
                            if (SessionLocator.TenantPM.AccountingActivated) {
                                button.IsHidden = true;
                            }
                            if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR" || this.EntityPM.StatusCode == "VD" || (this.EntityPM.StatusCode == "AD" && (SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG"))) {
                                button.IsDisabled = true;
                            }

                            else {
                                button.IsDisabled = false;
                            }
                            break;
                        }

                        case "VoidAPPayment": {
                            if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || AppTool.IsNullOrEmpty(this.EntityPM.Id) || this.EntityPM.StatusCode == "VD") {
                                button.IsDisabled = true;
                            }

                            else {
                                button.IsDisabled = false;
                            }

                            break;
                        }

                        case "CancelVoidAPPayment": {
                            button.IsDisabled = true;
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
                            if (this.EntityPM.StatusCode == "DR") {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }

                            break;
                        }

                        case "EnterExternalPayment": {
                            var isHidden = true;

                            if (SessionLocator.AccountingSettingPM.EnableAPPaymentExternalPayment == true) {
                                isHidden = false;
                            }

                            button.IsHidden = isHidden;

                            if (this.EntityPM.StatusCode == "VD") {
                                button.IsDisabled = true;
                            }

                            else {
                                button.IsDisabled = false;
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
                }
            }
        }
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {
        switch (menuButton.EventCode) {
            case "PrintAPPayment": {
                this.PrintPayment();
                break;
            }

            case "ApproveAPPayment": {
                this.ResetAllFlags();
                this.isApproval = true;
                this.ApprovalMethod();
                break;
            }

            case "CancelApproval": {
                this.ResetAllFlags();
                this.isCancelApproval = true;
                this.CancelApproval();
                break;
            }

            case "VoidAPPayment": {
                this.ResetAllFlags();
                this.isVoided = true;
                this.VoidMethod();
                break;
            }

            case "SendToQBO": {
                this.SendToQBO();
                break;
            }

            case "EnterExternalPayment": {
                this.EnterExternalPaymentClicked();
                break;
            }

            case "BlockFromTransfer": {
                this.BlockFromTransferToQBO();
                break;
            }
        }
    }



    SendToQBO() {
        if (this.EntityPM.TransferStatusCode == "TR" || this.EntityPM.TransferStatusCode == "ET" || this.EntityPM.TransferStatusCode == "IP") {
            var myConfirmWindow = new ConfirmWindow();
            myConfirmWindow.Width = 400;
            myConfirmWindow.Show("Resend this payment to QBO?");
            myConfirmWindow.WindowClosed.subscribe(s => {
                this.ResetAllFlags();
                if (myConfirmWindow.Yes) {
                    this.SendToQBOApproved("Resending payment to QBO");

                }
            });
        }

        else {
            this.SendToQBOApproved("Sending payment to QBO");
            this.ResetAllFlags();
        }
    }


    SendToQBOApproved(Text: string) {
        this.EntityPM.SetReSendQBO = true;
        this.EntityPM.SetVoided = false;
        this.EntityPM.SetApproved = false;
        this.EntityPM.SetCancelApproval = false;

        if (SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG") {
            var FlagNotTransfered: boolean = false;
            this.EntityPM.PaymentInvoices.forEach(item => {
                if (item.APInvoiceTransferStatusCode != "TR") {
                    FlagNotTransfered = true;
                }
            });

            if (FlagNotTransfered) {
                var window: MessageWindow = new MessageWindow();
                window.Show("Invoices that were not transferred to QBO will not be connected to the payment at QBO");
                window.WindowClosed.subscribe((event: any) => {
                    this.entityArgs.EditComponent.SaveChanges(Text);
                });
            }
            else {
                this.entityArgs.EditComponent.SaveChanges(Text);
            }

        }
        else {
            this.entityArgs.EditComponent.SaveChanges(Text);
        }



    }

    // [Approval]
    ApprovalMethod() {

        if (!FeatureLocator.HasEntityPermessions("APPayment", "UPDT", true)) {
            return;
        }

        if (!this.isOerationInProgrees) {
            this.isOerationInProgrees = true;

            if (SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG") {
                var FlagNotTransfered: boolean = false;

                this.EntityPM.PaymentInvoices.forEach(item => {
                    if (item.APInvoiceTransferStatusCode != "TR") {
                        FlagNotTransfered = true;
                    }
                });

                if (FlagNotTransfered) {
                    var window: MessageWindow = new MessageWindow();
                    window.Show("Invoices that were not transferred to QBO will not be connected to the payment at QBO");
                    window.WindowClosed.subscribe((event: any) => {
                        this.CompleteApprove();
                    });
                }

                else {
                    this.CompleteApprove();
                }
            }

            else {
                this.CompleteApprove();
            }
        }
    }

    CompleteApprove() {
        if (this.EntityPM.PaymentMethodCode == "FS" && (SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG")) {
            var messageWindow = new MessageWindow();
            messageWindow.Show("This payments with payment method Offsetting will not be transfered to quickbooks online , transfer it manually");
            messageWindow.WindowClosed.subscribe(a => {
                this.ApprovingLogic();
            });
        }

        else {
            this.ApprovingLogic();
        }
    }

    private ApprovingLogic() {
        var message = "";
        var isValid = true;

        var errors = this.customValidator.Validate(this.EntityPM);
        if (errors != null && errors.length > 0) {
            isValid = false;
        }

        if (isValid) {
            this.entityArgs.EditComponent.ValidationErrorsList = [];

            this.GetFullAccountingSettingsAndApprove();
        }

        else {
            if (this.entityArgs.EditComponent.ValidationErrorsList == null) {
                this.entityArgs.EditComponent.ValidationErrorsList = [];
            }

            errors.forEach(item => {
                this.entityArgs.EditComponent.ValidationErrorsList.push(item);
            });

            this.isOerationInProgrees = false;
        }
    }
    // [Cancel Approval]
    CancelApproval() {
        var isValid = true;
        if (isValid) {
            this.EntityPM.SetVoided = false;
            this.EntityPM.SetApproved = false;
            this.EntityPM.SetCancelApproval = true;
            if (this.CurrentDocument != null) {
                this.CurrentDocument.NeedsRebuild = true;
            }

            this.entityArgs.EditComponent.SaveChanges();
        }
    }
    OpenEditPaymentChequeScreen() {
        this.CurrentSession.StartBusyIndicatorLoading();


        var windowTitle = TextCodeTranslator.Translate("PaymentCheque");
        var logWindow = new LogitudeWindow();
        var windowArgs: any = {};
        windowArgs = this.SetPaymentChequeWindowArgs(windowArgs);
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 520;
        logWindow.Height = 450;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = true;

        logWindow.WindowClosed.subscribe(($event: any) => this.ContinueSaving($event));
        logWindow.Show('./InvoiceModules/APPayment/Components/EditTabs/EditPaymentChequeComponent');
        this.CurrentSession.StopBusyIndicator();

    }

    public GetFullAccountingSettingsAndApprove() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.fullAccountingSettingPMService.get(SessionLocator.TenantPM.Id.toString()).subscribe((myResult:any) =>
        {
            var myResponse: ServiceResponse = myResult;
            this.CurrentSession.StopBusyIndicator();

            if (myResponse != null) {

                var res = myResponse.Result;
                var fullAccountingSetting: FullAccountingSettingPM = res;

                if(fullAccountingSetting)
                    this.Approve(fullAccountingSetting);
                else
                    this.ContinueSaving(null);


            }else{
                this.ContinueSaving(null);
            }

        });

    }
    NameForPrintingCheques: string;
    private Approve(fullAccountingSetting: FullAccountingSettingPM)
    {
        if (fullAccountingSetting.AccountingActivated) {
            this.GetGLAccount(this.EntityPM.VendorGLAccountId).then((glaccount: GLAccountPM) => {


                if (fullAccountingSetting != null && glaccount != null) {
                    if (fullAccountingSetting.IsPaymentChequesActivated && glaccount.AllowEditChequePayToName && this.EntityPM.PaymentMethodCode == "CH") {
                        this.NameForPrintingCheques = glaccount.NameForPrintingCheques != null ? glaccount.NameForPrintingCheques : (glaccount.LocalName != null ? glaccount.LocalName : glaccount.EnglishName);
                        this.OpenEditPaymentChequeScreen();
                    }
                    else {
                        this.ContinueSaving(null);
                    }
                }
                else {
                    this.ContinueSaving(null);
                }


            });
        }
        else {
            this.ContinueSaving(null);
        }
    }

    SetPaymentChequeWindowArgs(windowArgs: any) {
        windowArgs.PayToGLAccountId = this.EntityPM.VendorGLAccountId;
        windowArgs.BankAccountId = this.EntityPM.BankAccountId;
        windowArgs.LocalAmount = this.EntityPM.TaxDeductionLocalAmount;
        windowArgs.CurrencyId = this.EntityPM.PaymentCurrencyId;
        windowArgs.ForeignAmount = this.EntityPM.AmountInPaymentCurrency;
        windowArgs.ValueDate = this.EntityPM.ValueDate;
        windowArgs.APPayment = this.EntityPM;
        windowArgs.NameForPrintingCheques = this.NameForPrintingCheques;

        return windowArgs;
    }

    ContinueSaving(event: string) {
        if(this.ReconcileInternalTransIds) {
            this.EntityPM.ReconcileInternalTransIds = this.ReconcileInternalTransIds;
        }
        if (event && event != "Cancel") {
            var splittedstring = event.split(",");
            this.EntityPM.PaymentChequeCreationPayToName = splittedstring[0];
            this.EntityPM.PaymentChequeCreationNotes = splittedstring[1];
        }
        if (event != "Cancel") {
            this.EntityPM.SetVoided = false;
            this.EntityPM.SetApproved = true;
            this.EntityPM.SetCancelApproval = false;

            if (this.CurrentDocument != null) {
                this.CurrentDocument.NeedsRebuild = true;

            }

            this.entityArgs.EditComponent.SaveChanges();


        }
    }
    // [Print]
    CurrentDocument: DocumentOutPM;
    objectTableName: string;
    DocumentTypeId: string;
    CurrentDocumentType: DocumentTypePM;
    PrintPayment() {
        var validator = new APPaymentValidator();
        var errors = validator.Validate(this.EntityPM);
        var isValid = true;
        if (errors != null && errors.length > 0) {
            isValid = false;
        }
        if (isValid == true) {
            this.isPrintRequested = true;
            this.entityArgs.EditComponent.SaveChanges();
        }
    }
    InitializePrinting() {
        var myEntityId: string = null;
        var myChildEntityId: string = null;
        var myObjectTableName: string = null;
        var myDocumentTypeCode: string = null;
        var myReference: string = null;
        var mychildObjectTableId: string = null;
        myEntityId = this.EntityPM.Id;
        myChildEntityId = null;
        myObjectTableName = "APPayment";
        myDocumentTypeCode = "APP";
        this.StartPrinting(myEntityId, myChildEntityId, myObjectTableName, mychildObjectTableId, myDocumentTypeCode, myReference);

    }
    StartPrinting(myEntityId: string, myChildEntityId: string, myObjectTableName: string, mychildObjectTableId: string, myDocumentTypeCode: string, myReference: string) {
        var myPrintHelper = new GeneralPrintHelper(myObjectTableName, myDocumentTypeCode, myEntityId, myChildEntityId, myReference, mychildObjectTableId);
        if (myPrintHelper.IsLoadPrintControl) {
            ServiceLocator.SendTotangoUserActivity("APPayment", "PrintAPPayment");
            myPrintHelper.ShowPrintControl();
            this.EntityPM.PrintDate = DateTool.GetCurrentDateTimeAsUtc();
            this.entityArgs.EditComponent.SaveChanges();

        }
    }
    PrintPaymentButtonLoaded() {

    }

    VoidingAPPayment(event: any) {
        if (event == null || event == "Ok") {
            var invoiceDomainService: InvoiceDomainService = new InvoiceDomainService();
            invoiceDomainService.GetConnectedAPInvoicestoPayments(this.EntityPM.Id).subscribe((response: any) => {
                if (!response.HasError) {
                    var hasConnectedInvoices: boolean = response.Result;
                    var hasExternalPaymentAmount: boolean = (this.EntityPM.ExternalPaymentAmount && this.EntityPM.ExternalPaymentAmount != 0) ? true : false;

                    if (hasConnectedInvoices || hasExternalPaymentAmount) {
                        var msg: string = null;

                        if (hasConnectedInvoices && hasExternalPaymentAmount) {
                            msg = "Please disconnect all invoices and external payment amount";
                        }

                        else if (hasConnectedInvoices && !hasExternalPaymentAmount) {
                            msg = TextCodeTranslator.Translate("APPayment.M.DisconnectInvoices");
                        }

                        else if (!hasConnectedInvoices && hasExternalPaymentAmount) {
                            msg = "Please disconnect external payment amount";
                        }

                        var messageWindow = new MessageWindow();
                        messageWindow.Show(msg);
                    }

                    else {
                        var confirmVoid = new ConfirmWindow();
                        confirmVoid.Width = 400;
                        var confirmMsg = TextCodeTranslator.Translate("APPayment.M.ConfirmVoid");
                        confirmVoid.ShowCancelButton = false;
                        confirmVoid.WindowClosed.subscribe(c => {
                            if (confirmVoid.Yes) {
                                this.EntityPM.SetVoided = true;
                                this.EntityPM.SetApproved = false;
                                this.EntityPM.SetCancelApproval = false;
                                if (this.CurrentDocument != null) {
                                    this.CurrentDocument.NeedsRebuild = true;
                                }

                                this.entityArgs.EditComponent.SaveChanges();
                            }
                        });

                        confirmVoid.Show(confirmMsg);
                    }
                }
            });
        }
    }

    // [Void]
    VoidMethod() {
        var messageWindow: MessageWindow;
        if (!SessionLocator.AccountingSettingPM.AllowVoidAPP) {
            var messageText = TextCodeTranslator.Translate("APPayment.M.AccountingSettingsDontAllowVoid");
            messageWindow = new MessageWindow();
            messageWindow.Show(messageText);
            return;
        }

        var errors = this.customValidator.Validate(this.EntityPM);
        var isValid = true;
        if (errors != null && errors.length > 0) {
            isValid = false;
        }

        if (isValid) {

            var isQuickBooks: boolean = false;
            var isTransferingToQuickBooks: boolean = false;

            if (SessionLocator.AccountingSystemPM.Code == "QBO" || SessionLocator.AccountingSystemPM.Code == "QBOG") {
                isQuickBooks = true;
                isTransferingToQuickBooks = true;

                if (this.EntityPM.StatusCode == null || this.EntityPM.StatusCode == "WA") {
                    isTransferingToQuickBooks = false;
                }

                else if (this.EntityPM.TransferStatusCode == "ET") {
                    isTransferingToQuickBooks = false;
                }
            }

            if (isQuickBooks && isTransferingToQuickBooks) {
                var messageWindow = new MessageWindow();
                messageWindow.Show("Please notice that QBO are not supporting void transmission for the APpayment, you can void it manually from QBO");
                messageWindow.WindowClosed.subscribe(p => {
                    this.VoidingAPPayment(null);
                });
            }

            else if (SessionLocator.TenantPM.AccountingActivated) {
                this.OpenCancelAPPaymentScreen();
            }

            else { this.VoidingAPPayment(null); }
        }
    }


    PayToGLAccount: GLAccountPM;
    GetGLAccount(id: string)
    {
        return new Promise(resolve =>
        {

            var service = new GLAccountPMService();
            service.get(id).subscribe((response:any) =>
            {
                console.log("[GLAccountPMService.Get", response);

                var result: ServiceResponse = response;
                if (!result.HasError) {
                    this.PayToGLAccount = result.Result;
                    resolve(this.PayToGLAccount);
                }
                else {
                    console.error(result.ErrorsArray);
                    reject();
                }
            });


        });
    }


    OpenCancelAPPaymentScreen() {
        this.CurrentSession.StartBusyIndicatorLoading();


        var windowTitle = TextCodeTranslator.Translate("APPayment.O.CancelAPPayment");
        var logWindow = new LogitudeWindow();
        var windowArgs: any = {};
        windowArgs.PaymentDate = this.EntityPM.RegisterDate;
        windowArgs.PaymentPM = this.EntityPM;
        // windowArgs = this.SetPaymentChequeWindowArgs(windowArgs);
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 480;
        logWindow.Height = 280;
        logWindow.Title = windowTitle;
        //  logWindow.ShowCloseButton = true;

        logWindow.WindowClosed.subscribe(($event: any) => this.VoidingAPPayment($event));
        logWindow.Show('./InvoiceModules/APPayment/Components/Other/CancelAPPaymentComponent');
        this.CurrentSession.StopBusyIndicator();

    }

    EnterExternalPaymentClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { EntityPM: this.EntityPM};        
        logWindow.Title = "External Payment";
        logWindow.Width = 650;
        logWindow.Height = 450;
        logWindow.Show('./InvoiceModules/APPayment/Components/Other/ExternalPaymentComponent');
    }

    BlockFromTransferToQBO() {

        this.Validate();

        if (this.isValid) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Show("Please make sure that you've created the record manually at QBO online before marking as 'blocked for transfer', it is recommended to fix any issues and resend from the communication log rather than marking as blocked");
            confirmWindow.WindowClosed.subscribe(s => {

                if (confirmWindow.Yes) {
                    this.EntityPM.TransferStatusCode = "BL";
                    this.EntityPM.TransferStatusName = "Blocked";
                    this.entityArgs.EditComponent.SaveChanges("Blocking...");
                }
            });
        }
    }
}
