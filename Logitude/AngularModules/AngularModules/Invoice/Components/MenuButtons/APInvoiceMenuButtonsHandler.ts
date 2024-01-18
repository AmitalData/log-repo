declare var window: any;
import {APInvoicePM} from '../../EntityPMs/APInvoicePM';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {AppTool} from '../../../Infrastructure/Tools';
import {APInvoiceValidator}  from '../../Validators/APInvoiceValidator';
import {InvoiceDomainService} from '../../Services/InvoiceDomainService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {GeneralPrintHelper} from '../../../Infrastructure/Helpers/GeneralPrintHelper';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';

export class APInvoiceMenuButtonsHandler {
    private CurrentSession = SessionLocator.SelectedSession;
    public EntityPM: APInvoicePM;
    public entityArgs: EntityArgs
    isFullAccounting: boolean = false;
    public approvedStatusCode: string = "AD";
    public VoidStatusCode: string = "VD";
    
    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();

        this.isFullAccounting = SessionLocator.TenantPM.AccountingActivated;
    }
    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(d => d.Name === 'APInvoice')[0];
                for (var i = 0; i < menuButtons.length; i++) {

                    var button = menuButtons[i];
                    var myButtonIsDisabled = false;

                    switch (button.EventCode) {
                        case "SaveAPInvoice": {
                            myButtonIsDisabled = true;

                            if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "WA" || (this.EntityPM.StatusCode == this.approvedStatusCode && SessionLocator.TenantPM.AccountingActivated == true)) {
                                myButtonIsDisabled = false;
                            }

                            break;
                        }

                        case "ApproveAPInvoice": {
                            myButtonIsDisabled = true;

                            if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "WA") {
                                myButtonIsDisabled = false;
                            }

                            break;
                        }

                        case "CancelApproval": {
                            if (SessionLocator.TenantPM.AccountingActivated == true) {
                                button.IsHidden = true;
                            }
                            else {
                                if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                                    myButtonIsDisabled = true;
                                }

                                else if (this.EntityPM.TransferStatusCode == "TR") {
                                    myButtonIsDisabled = true;
                                }

                                else if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "WA" || this.EntityPM.StatusCode == "VD") {
                                    myButtonIsDisabled = true;
                                }
                            }

                            break;
                        }

                        case "VoidAPInvoice": {
                            if (SessionLocator.TenantPM.AccountingActivated == true) {
                                if ((this.EntityPM != null && this.EntityPM.IsExternalEntity) || this.EntityPM.StatusCode == this.VoidStatusCode) {
                                    myButtonIsDisabled = true;
                                }
                            }
                            else {
                                if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                                    myButtonIsDisabled = true;
                                }

                                else if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "VD") {
                                    myButtonIsDisabled = true;
                                }

                            }
                            break;
                        }

                        case "ReTransfer": {
                            if (this.isFullAccounting) {
                                button.IsHidden = true;
                            }

                            else {

                                myButtonIsDisabled = true;

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

                        case "PrintAPInvoice": {
                            myButtonIsDisabled = true;
                            if (this.EntityPM.Id != null) {
                                myButtonIsDisabled = false;
                            }
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
                                myButtonIsDisabled = true;
                            }
                            else {
                                myButtonIsDisabled = false;
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

                        case "CopyInvoice": {
                            button.IsHidden = !SessionLocator.TenantPM.AccountingActivated;
                            myButtonIsDisabled = this.SetEnableForCopyInvoiceButton(myButtonIsDisabled);
                            break;
                        }
                    }

                    button.IsDisabled = myButtonIsDisabled;

                }
            }
        }
    }
    private SetEnableForCopyInvoiceButton(myButtonIsDisabled: boolean) {
        if (this.EntityPM != null && this.EntityPM.IsExternalEntity) {
            myButtonIsDisabled = true;
        }
        return myButtonIsDisabled;
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {
        if (!this.isButtonClicked) {

            this.StopFlags();
            this.StopFields();
            this.isButtonClicked = true;
            this.ClickedButtonCode = menuButton.EventCode;

            switch (menuButton.EventCode) {
                case "SaveAPInvoice": {
                    this.SaveAPInvoiceClicked();
                    break;
                }

                case "ApproveAPInvoice": {
                    this.ApproveClicked();
                    break;
                }

                case "CancelApproval": {
                    this.CancelApprovalClicked();
                    break;
                }

                case "VoidAPInvoice": {
                    this.VoidClicked();
                    break;
                }

                case "ReTransfer": {
                    this.EnableReTransferClicked();
                    break;
                }

                case "PrintAPInvoice": {
                    this.PrintClicked();

                    break;
                }

                case "SendToQBO": {
                    this.SendToQBO();
                    break;
                }

                case "BlockFromTransfer": {
                    this.BlockFromTransferToQBO();
                    break;
                }

                case "CopyInvoice":{
                     this.OpenCopyInvoiceScreen();
                     break;
                }

                default: {
                    this.StopFlags();
                    break;
                }
            }
        }
    }


    SendToQBO() {
        var invoiceDomainService: InvoiceDomainService = new InvoiceDomainService();
        invoiceDomainService.getConnectedAPPayments(this.EntityPM.Id).subscribe((response:any) => {
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
        this.entityArgs.EditComponent.SaveChanges(Text);
    }


    isValid: boolean = false;
    isButtonClicked: boolean = false;
    isPrintRequested: boolean = false;
    ClickedButtonCode: string = null;
    StopFlags() {
        this.isButtonClicked = false;
        this.isPrintRequested = false;
        this.ClickedButtonCode = null;
    }
    StopFields() {

    }
    Validate() {
        var validator = new APInvoiceValidator();
        var errors: string[] = validator.Validate(this.EntityPM);

        this.isValid = errors.length == 0 ? true : false;

        this.entityArgs.EditComponent.ValidationErrorsList = errors;

        if (!this.isValid) {
            this.StopFlags();
        }
    }
    Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                    if (this.ClickedButtonCode == "PrintAPInvoice") {
                        this.InitializePrinting();
                    }
                }
                this.StopFlags();
            });
        }

        this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
            if (isLoadSuccess) {
                this.EntityPM = this.entityArgs.EditComponent.EntityPM;
            }

            this.StopFlags();
        });
    }

    SaveAPInvoiceClicked() {
        if (!FeatureLocator.HasEntityPermessions("APInvoice", "UPDT", true)) {
            this.StopFlags();
        }

        else {
            this.Validate();

            if (this.isValid) {

                if (this.EntityPM.IsDirty) {
                    this.CheckDuplication();
                }

                else {
                    this.StopFlags();
                }
            }
        }
    }
    ApproveClicked() {
        if (!FeatureLocator.HasEntityPermessions("APInvoice", "UPDT", true)) {
            this.StopFlags();
        }

        this.Validate();

        if (this.isValid) {
            this.ValidateInvoiceDate();

        }

        else {
            this.StopFlags();
        }
    }

    ValidateInvoiceDate() {
        //this.CurrentSession.StartBusyIndicatorLoading();

        this.entityArgs.EditComponent.StartBusyIndicatorLoading();

        var service: InvoiceDomainService = new InvoiceDomainService();
        service.ValidateInvoiceDate(this.EntityPM.InvoiceDate).subscribe((response: ServiceResponse) => {


if (response != null) {
 this.CurrentSession.StopBusyIndicator();
                if (!response.HasError) {
                    if (response.Result != null) {
                        this.ShowConfirmWindow(response.Result);
                    }
                    else {
                        this.CheckDuplication();
                    }
                }
                else {
                    this.entityArgs.EditComponent.ValidationErrorsList  = response.ErrorsArray;
                }
            }

        });

    }

    private ShowConfirmWindow(warningMessage: string) {

        let confirmWindow = new ConfirmWindow();
        confirmWindow.ShowWarningImage = true;

        confirmWindow.NoButtonText = TextCodeTranslator.Translate("General.B.Cancel");
        confirmWindow.YesButtonText = TextCodeTranslator.Translate("General.B.Ok");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.CheckDuplication();

            }
            if (confirmWindow.No)
                this.StopFlags();
        });
        confirmWindow.Show(warningMessage);

    }

    CheckDuplication() {
        var service: InvoiceDomainService = new InvoiceDomainService();
        service.CheckVendor_NumberDuplication(this.EntityPM.VendorId, this.EntityPM.InvoiceNumber, this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                this.StopFlags();
                this.entityArgs.EditComponent.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {
                    var isDuplicated: boolean = myResponse.Result;
                    if (isDuplicated) {
                        var confirmWindow = new ConfirmWindow();
                        confirmWindow.Title = "Warning";
                        confirmWindow.Width = 450;
                        confirmWindow.Height = 190;
                        confirmWindow.YesButtonText = TextCodeTranslator.Translate("General.B.Save");
                        confirmWindow.NoButtonText = TextCodeTranslator.Translate("General.B.Cancel");
                        confirmWindow.ShowCancelButton = false;
                        confirmWindow.Show(TextCodeTranslator.Translate("APInvoice.M.SameInvoiceNumber"));

                        confirmWindow.WindowClosed.subscribe(c => {
                            if (confirmWindow.Yes) {
                                this.ContinueSaving();
                            }

                            if (confirmWindow.No) {
                                this.StopFlags();
                            }
                        });
                    }

                    else {
                        this.ContinueSaving();
                    }
                }

        });
    }
    ContinueSaving() {

        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            ServiceLocator.SendTotangoUserActivity("APInvoice", "NewInvoice");
        }

        if (this.ClickedButtonCode == "SaveAPInvoice") {

            if (this.EntityPM.IsDirty) {
                this.EntityPM.SetVoided = false;
                this.EntityPM.SetApproved = false;
                this.EntityPM.SetReTransfer = false;
                this.EntityPM.SetCancelApproval = false;

                this.entityArgs.EditComponent.SaveChanges();
            }

            else {
                this.StopFlags();
            }
        }

        else {
            this.EntityPM.SetVoided = false;
            this.EntityPM.SetApproved = true;
            this.EntityPM.SetReTransfer = false;
            this.EntityPM.SetCancelApproval = false;

            this.entityArgs.EditComponent.SaveChanges("Approving...");
        }
    }

    CancelApprovalClicked() {
        if (!FeatureLocator.HasEntityPermessions("APInvoice", "UPDT", true)) {
            this.StopFlags();
        }

        if (this.EntityPM.InvoicePayments.length > 0) {
            var messageText = TextCodeTranslator.Translate("APInvoice.M.DisconnectPayments");
            var messageWindow = new MessageWindow();
            messageWindow.Width = 450;
            messageWindow.Height = 190;
            messageWindow.Title = "Logitude Message";
            messageWindow.Show(messageText);
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
                                this.CancelApprovalClickedProccess();
                            }
                        }

                        else {
                            this.StopFlags();
                        }
                    });
                }

                else {
                    this.CancelApprovalClickedProccess();
                }
            }

            else {
                this.StopFlags();
            }
        }
    }

    CancelApprovalClickedProccess() {
        this.EntityPM.SetVoided = false;
        this.EntityPM.SetApproved = false;
        this.EntityPM.SetReTransfer = false;
        this.EntityPM.SetCancelApproval = true;
        this.entityArgs.EditComponent.SaveChanges();
    }

    VoidClicked() {
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
            messageWindow.Show("Please notice that QBO are not supporting void transmission for the APInvoice, you can void it manually from QBO");
            messageWindow.WindowClosed.subscribe(p => {
                this.ShowConfirmVoidMessage();
            });
        }

        else if (this.EntityPM.TransferStatusCode == "TR") {
            var messageWindow = new MessageWindow();
            messageWindow.Show("Already transferred invoices can't be voided.");
            this.StopFlags();
        }

        else if (!SessionLocator.AccountingSettingPM.AllowVoidAPI) {
            var messageWindow = new MessageWindow();
            messageWindow.Show(TextCodeTranslator.Translate("APInvoice.M.AccountingSettingsDontAllowVoid"));
            this.StopFlags();
        }

        else if (this.EntityPM.InvoicePayments.length > 0) {
            var messageWindow = new MessageWindow();
            messageWindow.Show(TextCodeTranslator.Translate("APInvoice.M.DisconnectPayments"));
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

            else {
                this.StopFlags();
            }
        }
    }
    VoidClickedProccess() {
        this.ShowConfirmVoidMessage();
    }

    ShowConfirmVoidMessage() {
        this.Validate();

        if (this.isValid) {
            var myConfirmWindow = new ConfirmWindow();
            myConfirmWindow.Width = 400;
            myConfirmWindow.Show(TextCodeTranslator.Translate("APInvoice.M.ConfirmVoid"));
            myConfirmWindow.WindowClosed.subscribe(s => {

                this.StopFlags();

                if (myConfirmWindow.Yes) {
                    this.EntityPM.SetVoided = true;
                    this.EntityPM.SetApproved = false;
                    this.EntityPM.SetReTransfer = false;
                    this.EntityPM.SetCancelApproval = false;
                    this.EntityPM.SetReSendQBO = false;

                    this.entityArgs.EditComponent.SaveChanges("Voiding...");
                }
            });
        }

        else {
            this.StopFlags();
        }
    }

    EnableReTransferClicked() {
        this.Validate();

        if (this.isValid) {
            this.EntityPM.SetVoided = false;
            this.EntityPM.SetApproved = false;
            this.EntityPM.SetReTransfer = true;
            this.EntityPM.SetCancelApproval = false;
            this.EntityPM.SetReSendQBO = false;

            this.entityArgs.EditComponent.SaveChanges();
        }

        else {
            this.StopFlags();
        }
    }

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
        var myEntityReference: string = null;
        var myEntityTableName: string = null;
        var myDocumentTypeCode: string = null;
        var myChildEntityId: string = null;
        var myChildObjectTableId: string = null;

        if (this.EntityPM.IsMultipleEntities) {
            myEntityId = this.EntityPM.Id;
            myEntityReference = this.EntityPM.InvoiceNumber;
            myEntityTableName = "APInvoice";
            myDocumentTypeCode = "999MP";
            this.StartPrinting(myEntityId, myChildEntityId, myEntityTableName, myChildObjectTableId, myDocumentTypeCode, myEntityReference);
        }
        else {
            myEntityId = this.EntityPM.MainEntityId;
            myEntityReference = this.EntityPM.MainEntityReference;
            myEntityTableName = "Shipment";
            myDocumentTypeCode = "999P";
            myChildEntityId = this.EntityPM.Id;
            myChildObjectTableId = window.ObjectTables.filter(d => d.Name == "APInvoice")[0].Id;
            this.StartPrinting(myEntityId, myChildEntityId, myEntityTableName, myChildObjectTableId, myDocumentTypeCode, myEntityReference);
        }
    }

    StartPrinting(myEntityId: string, myChildEntityId: string, myObjectTableName: string, mychildObjectTableId: string, myDocumentTypeCode: string, myReference: string) {
        var myPrintHelper = new GeneralPrintHelper(myObjectTableName, myDocumentTypeCode, myEntityId, myChildEntityId, myReference, mychildObjectTableId);
        if (myPrintHelper.IsLoadPrintControl) {
            ServiceLocator.SendTotangoUserActivity("APInvoice", "PrintInvoice");
            myPrintHelper.ShowPrintControl();
        }
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

    OpenCopyInvoiceScreen() {
        var windowTitle = TextCodeTranslator.Translate("APInvoice.O.CopyInvoice");
        var windowArgs: any = {};
        windowArgs.APInvoicePM = this.EntityPM;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 610;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        });
        logWindow.Show('./Accounting/Components/Others/CopyInvoiceComponent');
    }
}
