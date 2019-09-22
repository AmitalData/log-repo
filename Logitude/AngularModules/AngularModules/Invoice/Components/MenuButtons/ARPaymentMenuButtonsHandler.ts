import { ARPaymentEventManager } from './../../../Accounting/Utilities/ARPaymentEventManager';
declare var window: any;
import {ARPaymentPM} from '../../EntityPMs/ARPaymentPM';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {ARPaymentChequePM} from '../../../Accounting/EntityPMs/ARPaymentChequePM';
import {CashBookLinePM} from '../../../Accounting/EntityPMs/CashBookLinePM';
import {InvoiceDomainService} from '../../Services/InvoiceDomainService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../Infrastructure/Tools';
import {ARPaymentValidator} from '../../Validators/ARPaymentValidator';
import {DocumentOutPM}  from '../../../Common/EntityPMs/DocumentOutPM';
import {DocumentTypePM} from '../../../Common/EntityPMs/DocumentTypePM';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {GeneralPrintHelper} from '../../../Infrastructure/Helpers/GeneralPrintHelper';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';

export class ARPaymentMenuButtonsHandler {
    public EntityPM: ARPaymentPM;
    public entityArgs: EntityArgs
    private CurrentSession = SessionLocator.SelectedSession;
    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }

    private isValid: boolean = false;
    private isApproval: boolean;
    private isCancelApproval: boolean;
    private isVoided: boolean;
    private isPrintRequested: boolean;
    private isSATSendRequest:boolean;
    private ResetAllFlags() {
        this.isApproval = false;
        this.isCancelApproval = false;
        this.isVoided = false;
        this.isPrintRequested = false;
        this.isSATSendRequest = false;
        //this.EntityPM.SetReSendQBO = false;
    }

    Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
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
                    if (this.isSATSendRequest) {
                        this.RunSendToSAT();
                    }
                }

                this.ResetAllFlags();
            });
        }

        this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
            if (isLoadSuccess) {
                this.EntityPM = this.entityArgs.EditComponent.EntityPM;
            }
        });
    }
    Validate() {
        var validator = new ARPaymentValidator();
        var errors: string[] = validator.Validate(this.EntityPM);

        this.isValid = errors.length == 0 ? true : false;

        this.entityArgs.EditComponent.ValidationErrorsList = errors;

        if (!this.isValid) {
            this.ResetAllFlags();
        }
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(d => d.Name === 'ARPayment')[0];

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {

                        case "PrintARPayment":
                            {
                                this.PrintPaymentButtonLoaded();
                                if (this.EntityPM.Id == null && this.EntityPM.StatusCode == "VD") {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "ApproveARPayment":
                            {

                                if (this.EntityPM.SATTransferStatusCode == "TD" && (this.EntityPM.StatusCode == "VD" || this.EntityPM.StatusCode == "DR")) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR") {

                                        button.IsDisabled = false;
                                    }

                                    else {
                                        button.IsDisabled = true;
                                    }
                                }

                                break;
                            }
                        case "CancelApproval":
                            {
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
                        case "VoidARPayemnt":
                            {
                                if (SessionLocator.TenantPM.AccountingActivated) {
                                    if (this.EntityPM.StatusCode == "AD") {
                                        button.IsDisabled = false;
                                    }
                                    else {
                                        button.IsDisabled = true;
                                    }
                                }
                                else {

                                    if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || AppTool.IsNullOrEmpty(this.EntityPM.Id) || this.EntityPM.StatusCode == "VD") {
                                        button.IsDisabled = true;
                                    }
                                    else {
                                        button.IsDisabled = false;
                                    }
                                }
                                break;
                            }
                        case "CancelVoidARPayment":
                            {
                                button.IsDisabled = true;
                                break;
                            }

                        case "ReTransfer":
                            {
                                if (SessionLocator.TenantPM.AccountingActivated) {

                                    button.IsHidden = true;
                                }
                                else {
                                button.IsDisabled = true;
                                    if (!AppTool.IsNullOrEmpty(this.EntityPM.Id) && !AppTool.IsNullOrEmpty(this.EntityPM.StatusCode)) {
                                        if (this.EntityPM.StatusCode != "DR" && this.EntityPM.StatusCode != "VD") {
                                            if (this.EntityPM.TransferStatusCode == "TR") {
                                                button.IsDisabled = false;
                                            }
                                        }
                                    }
                                }
                                break;
                            }

                        case "SENDToSAT":
                            {
                                if (this.EntityPM.SATTransferStatusCode == "TD" && (this.EntityPM.StatusCode == "VD" || this.EntityPM.StatusCode == "DR")) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                //  if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || AppTool.IsNullOrEmpty(this.EntityPM.Id) ||// this.EntityPM.StatusCode == "VD") {
                                //     button.IsDisabled = true;
                                // }

                                // else {


                                if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "NONE") {
                                    button.IsHidden = true;
                                }

                                // }
                                break;
                            }

                        case "CheckSATStatus":
                            {
                                if (this.EntityPM.SATTransferStatusCode == "CS") {
                                    button.IsDisabled = false;
                                }
                                else {
                                    button.IsDisabled = true;
                                }
                                if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "NONE") {
                                    button.IsHidden = true;
                                }


                                break;
                            }


                        case "SendToQBO":
                            {
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
                    }
                }
            }
        }
    }
    public MenuButtonClick(menuButton: MenuButtonPM) {
        switch (menuButton.EventCode) {
            case "PrintARPayment": {
                this.PrintPayment();
                break;
            }
            case "ApproveARPayment": {
                this.ResetAllFlags();
                this.isApproval = true;
                this.ApprovalMethod();
                break;
            }
            case "CancelApproval":
                {
                    this.ResetAllFlags();
                    this.isCancelApproval = true;
                    this.CancelApproval();
                    break;
                }
            case "VoidARPayemnt":
                {
                    this.ResetAllFlags();
                    this.isVoided = true;
                    this.VoidMethod();
                    break;
                }
            case "ReTransfer":
                {
                    this.ReTransferClicked();
                    break;
                }
            case "SENDToSAT":
                {
                    this.SaveSendToSAT();
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
            this.SendToQBOApproved("Sending Invoice to QBO");
            this.ResetAllFlags();
        }
    }

    SendToQBOApproved(Text: string) {
        this.EntityPM.SetReSendQBO = true;
        this.EntityPM.SetVoided = false;
        this.EntityPM.SetApproved = false;
        this.EntityPM.SetReTransfer = false;
        this.EntityPM.SetCancelApproval = false;

        if (SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG") {
            var FlagNotTransfered: boolean = false;
            this.EntityPM.PaymentInvoices.forEach(item => {
                if (item.ARInvoiceTransferStatusCode != "TR") {
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



    CheckSATStatus() {
        var invoiceDomainService: InvoiceDomainService = new InvoiceDomainService();
        var invoiceDomainService: InvoiceDomainService = new InvoiceDomainService();
        invoiceDomainService.GetARPaymentSATCancellationStatus(this.EntityPM.Id).subscribe(response => {

        });
    }

    RunSendToSAT() {
        var windowArgs: any = {};
        windowArgs.EnttiyPM = this.EntityPM;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 400;
        logWindow.Title = "Send to SAT";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./Invoice/Components/SAT/SendPaymentWindowComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {

            //this.StopBusyIndicator();
        });
    }

    SaveSendToSAT() {

        var message = "";
        if (!FeatureLocator.HasEntityPermessions("ARPayment", "UPDT", true)) {
            return;
        }

        var errors = ARPaymentValidator.ValidateCurrenctEntity(this.EntityPM);
        var isValid = true;
        if (errors != null && errors.length > 0) {
            isValid = false;
        }
        if (isValid) {
            this.isSATSendRequest = true;
            this.entityArgs.EditComponent.SaveChanges();
        }
        else {
            errors.forEach(item => {
                if (this.entityArgs.EditComponent.ValidationErrorsList == null) {
                    this.entityArgs.EditComponent.ValidationErrorsList = [];
                }
                this.entityArgs.EditComponent.ValidationErrorsList.push(item);
            });
        }
        ////if (this.EntityPM.StatusCode == "AD" || this.EntityPM.StatusCode == "CL") {
        //    //if (this.EntityPM.PaymentInvoices.length > 0) {
        //        var windowArgs: any = {};
        //        windowArgs.EnttiyPM = this.EntityPM;

        //        var logWindow = new LogitudeWindow();
        //        //logWindow.Width = 500;
        //        //logWindow.Height = 300;
        //        logWindow.Title = "Send to SAT";
        //        logWindow.WindowArgs = windowArgs;
        //        logWindow.Show('./Invoice/Components/SAT/SendPaymentWindowComponent');
        //        logWindow.WindowClosed.subscribe(($event: any) => {

        //            //this.StopBusyIndicator();
        //        });
        //    //}
        //    //else {
        //    //    var messageWindow: MessageWindow;
        //    //    var messageText = "There is no connected invoices";//TextCodeTranslator.Translate("ARPayment.M.AccountingSettingsDontAllowVoid");
        //    //    messageWindow = new MessageWindow();
        //    //    messageWindow.Show(messageText);
        //    //    return;
        //    //}
        ////}
        ////else {
        ////    var messageWindow: MessageWindow;
        ////    var messageText = "Payment should be Approved before sending it to SAT";//TextCodeTranslator.Translate("ARPayment.M.AccountingSettingsDontAllowVoid");
        ////    messageWindow = new MessageWindow();
        ////    messageWindow.Show(messageText);
        ////    return;

        ////}


    }

    // [Approval]
    ApprovalMethod() {

        // full accounting validation
        //lines validation
        if(SessionLocator.TenantPM.AccountingActivated){
            var _edit = this.CurrentSession.CurrentEditComponent;
            if(!_edit.IsEditValid){
                _edit.ValidationErrorsList = [TextCodeTranslator.Translate('Reconciliations.O.ErrorsInSelectedLines')];
                return;
            }else{
                _edit.ValidationErrorsList = [];
            }

        }

        if (SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG") {
            var FlagNotTransfered: boolean = false;
            this.EntityPM.PaymentInvoices.forEach(item => {
                if (item.ARInvoiceTransferStatusCode != "TR") {
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

    private CompleteApprove() {
    if (this.EntityPM.AccountingPaymentMethodCode == "FS" && (SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG")) {
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
        if (!FeatureLocator.HasEntityPermessions("ARPayment", "UPDT",true)) {
            return;
        }

        var errors = ARPaymentValidator.ValidateCurrenctEntity(this.EntityPM);
        var isValid = true;
        if (errors != null && errors.length > 0) {
            isValid = false;
        }
        if (isValid) {
            this.EntityPM.SetVoided = false;
            this.EntityPM.SetApproved = true;
            this.EntityPM.SetCancelApproval = false;

            if (this.CurrentDocument != null) {
                this.CurrentDocument.NeedsRebuild = true;
                //CommonContext.SubmitChanges();
            }
            this.entityArgs.EditComponent.SaveChanges();
            ARPaymentEventManager.ARPaymentApproved.emit();
        }
        else {
            errors.forEach(item => {
                if (this.entityArgs.EditComponent.ValidationErrorsList == null) {
                    this.entityArgs.EditComponent.ValidationErrorsList = [];
                }
                this.entityArgs.EditComponent.ValidationErrorsList.push(item);
            });
        }

    }
    CreateARPaymentCheque() {
        //var arPaymentcheque: ARPaymentChequePM = new ARPaymentChequePM();
        //arPaymentcheque.PaymentId = this.EntityPM.Id;
        //arPaymentcheque.ChequeNumber = this.EntityPM.ChequeOrPaymentRef;
        //arPaymentcheque.ValueDate = this.EntityPM.ValueDate;
        //arPaymentcheque.BankBranch = this.EntityPM.BankBranch;
        //arPaymentcheque.BankAccount = this.EntityPM.Account;
        //arPaymentcheque.CurrencyId = this.EntityPM.PaymentCurrencyId;
        //arPaymentcheque.LocalAmount = this.EntityPM.AmountInLocalCurrency;
        //arPaymentcheque.ForeignAmount = this.EntityPM.AmountInPaymentCurrency;

        //var cashBookLine: CashBookLinePM = new CashBookLinePM(null);
        //cashBookLine.CashBookId = "";
        //cashBookLine.ARPChequeId = arPaymentcheque.Id;
        //cashBookLine.IsDeposited = false;

        //// Create Journal
        var invoiceDomainService: InvoiceDomainService = new InvoiceDomainService();
        invoiceDomainService.PostARPaymentChequeAndCashBook(this.EntityPM).subscribe((response: ServiceResponse) => {
            if (response != null) {
                if (!response.HasError) {

                }
                else {
                    var messageWindow = new MessageWindow();
                    messageWindow.Show(response.ErrorsArray.toString());
                }
            }

        });
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
                //CommonContext.SubmitChanges();
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
        var errors = ARPaymentValidator.ValidateCurrenctEntity(this.EntityPM);
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
        myObjectTableName = "ARPayment";
        myDocumentTypeCode = "ARP";
        myReference = this.EntityPM.PaymentNo;
        this.StartPrinting(myEntityId, myChildEntityId, myObjectTableName, mychildObjectTableId, myDocumentTypeCode, myReference);

    }
    StartPrinting(myEntityId: string, myChildEntityId: string, myObjectTableName: string, mychildObjectTableId:string, myDocumentTypeCode: string, myReference: string) {
        var myPrintHelper = new GeneralPrintHelper(myObjectTableName, myDocumentTypeCode, myEntityId, myChildEntityId, myReference, mychildObjectTableId);
        if (myPrintHelper.IsLoadPrintControl) {
            ServiceLocator.SendTotangoUserActivity("ARPayment", "PrintARPayment");
            myPrintHelper.ShowPrintControl();
        }
    }
    PrintPaymentButtonLoaded() {

    }

    // [Void]
    VoidMethod() {

        var messageWindow: MessageWindow;
        if (!SessionLocator.AccountingSettingPM.AllowVoidARP) {
            var messageText = TextCodeTranslator.Translate("ARPayment.M.AccountingSettingsDontAllowVoid");
            messageWindow = new MessageWindow();
            messageWindow.Show(messageText);
            return;
        }

        var errors = ARPaymentValidator.ValidateCurrenctEntity(this.EntityPM);
        var isValid = true;
        if (errors != null && errors.length > 0) {
            isValid = false;
        }

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE" && (this.EntityPM.SATTransferStatusCode == "TD" || this.EntityPM.SATTransferStatusCode == "TG") && (this.EntityPM.StatusCode == "AD" || this.EntityPM.StatusCode == "CL"
            )) {

            var messageText = "This Payment is connected to SAT, Please cancel payment approval before voiding it";//TextCodeTranslator.Translate("ARPayment.M.AccountingSettingsDontAllowVoid");
            messageWindow = new MessageWindow();
            messageWindow.Show(messageText);
            return;
        }

        if (isValid) {
            if (this.EntityPM.PaymentInvoices.length > 0) {
                var messageText = TextCodeTranslator.Translate("ARPayment.M.DisconnectInvoices");
                messageWindow = new MessageWindow();
                messageWindow.Show(messageText);
            }

            else {
                var confirmVoid = new ConfirmWindow();
                confirmVoid.Width = 400;
                var confirmMsg = TextCodeTranslator.Translate("ARPayment.M.ConfirmVoid");
                confirmVoid.ShowCancelButton = false;
                confirmVoid.WindowClosed.subscribe(c => {
                    if (confirmVoid.Yes) {
                        this.EntityPM.SetVoided = true;
                        this.EntityPM.SetApproved = false;
                        this.EntityPM.SetCancelApproval = false;
                        if (this.CurrentDocument != null) {
                            this.CurrentDocument.NeedsRebuild = true;
                            //CommonContext.SubmitChanges();
                        }

                        this.entityArgs.EditComponent.SaveChanges();
                    }
                });
                confirmVoid.Show(confirmMsg);
            }
        }
    }

    //[ReTransfer]
    ReTransferClicked() {
        this.Validate();
        if (this.isValid) {
            this.EntityPM.SetVoided = false;
            this.EntityPM.SetApproved = false;
            this.EntityPM.SetReTransfer = true;
            this.EntityPM.SetCancelApproval = false;
            this.entityArgs.EditComponent.SaveChanges();
        }
    }
}
