declare var window: any;
import {APPaymentPM} from '../../EntityPMs/APPaymentPM';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {AppTool} from '../../../Infrastructure/Tools';
import {APPaymentValidator} from '../../Validators/APPaymentValidator';
import {DocumentOutPM}  from '../../../Common/EntityPMs/DocumentOutPM';
import {DocumentTypePM} from '../../../Common/EntityPMs/DocumentTypePM';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {GeneralPrintHelper} from '../../../Infrastructure/Helpers/GeneralPrintHelper';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';

export class APPaymentMenuButtonsHandler {
    public EntityPM: APPaymentPM;
    public entityArgs: EntityArgs
    public customValidator: APPaymentValidator = new APPaymentValidator();
    private isApproval: boolean;
    private isCancelApproval: boolean;
    private isVoided: boolean;
    private isPrintRequested: boolean;
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


    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(d => d.Name === 'APPayment')[0];

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {

                        case "PrintAPPayment":
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
                        case "ApproveAPPayment":
                            {
                                if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR") {
                                    button.IsDisabled = false;
                                }

                                else {
                                    button.IsDisabled = true;
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
                        case "VoidAPPayment":
                            {
                                if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || AppTool.IsNullOrEmpty(this.EntityPM.Id) || this.EntityPM.StatusCode == "VD") {
                                    button.IsDisabled = true;
                                }

                                else {
                                    button.IsDisabled = false;
                                }

                                break;
                            }
                        case "CancelVoidAPPayment":
                            {
                                button.IsDisabled = true;
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
            case "CancelApproval":
                {
                    this.ResetAllFlags();
                    this.isCancelApproval = true;
                    this.CancelApproval();
                    break;
                }
            case "VoidAPPayment":
                {
                    this.ResetAllFlags();
                    this.isVoided = true;
                    this.VoidMethod();
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
        if (!FeatureLocator.HasEntityPermessions("APPayment", "UPDT", true)) {
            return;
        }

        var errors = this.customValidator.Validate(this.EntityPM);
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

            }
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
        }
    }
    PrintPaymentButtonLoaded() {

    }

    VoidingAPPayment() {
        var messageWindow: MessageWindow;
        if (this.EntityPM.PaymentInvoices.length > 0) {
            var messageText = TextCodeTranslator.Translate("APPayment.M.DisconnectInvoices");
            messageWindow = new MessageWindow();
            messageWindow.Show(messageText);
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
                        //CommonContext.SubmitChanges();
                    }

                    this.entityArgs.EditComponent.SaveChanges();
                }
            });
            confirmVoid.Show(confirmMsg);
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


            if (SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG") {
                var messageWindow = new MessageWindow();
                messageWindow.Show("Please notice that QBO are not supporting void transmission for the APpayment, you can void it manually from QBO");
                messageWindow.WindowClosed.subscribe(p => {
                    this.VoidingAPPayment();           
                });
            }                                    
            else {
                this.VoidingAPPayment();                  
             }
        }
    }
}
