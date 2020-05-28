"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var APInvoiceValidator_1 = require("../../Validators/APInvoiceValidator");
var InvoiceDomainService_1 = require("../../Services/InvoiceDomainService");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var GeneralPrintHelper_1 = require("../../../Infrastructure/Helpers/GeneralPrintHelper");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var APInvoiceMenuButtonsHandler = /** @class */ (function () {
    function APInvoiceMenuButtonsHandler() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isValid = false;
        this.isButtonClicked = false;
        this.isPrintRequested = false;
        this.ClickedButtonCode = null;
    }
    APInvoiceMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    };
    APInvoiceMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'APInvoice'; })[0];
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    var myButtonIsDisabled = false;
                    switch (button.EventCode) {
                        case "SaveAPInvoice":
                            {
                                myButtonIsDisabled = true;
                                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "WA") {
                                    myButtonIsDisabled = false;
                                }
                                break;
                            }
                        case "ApproveAPInvoice":
                            {
                                myButtonIsDisabled = true;
                                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "WA") {
                                    myButtonIsDisabled = false;
                                }
                                break;
                            }
                        case "CancelApproval":
                            {
                                if (SessionLocator_1.SessionLocator.TenantPM.AccountingActivated == true) {
                                    button.IsHidden = true;
                                }
                                else {
                                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                                        myButtonIsDisabled = true;
                                    }
                                    else if (this.EntityPM.TransferStatusCode == "TR") {
                                        myButtonIsDisabled = true;
                                    }
                                    else if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "WA" || this.EntityPM.StatusCode == "VD") {
                                        myButtonIsDisabled = true;
                                    }
                                }
                                break;
                            }
                        case "VoidAPInvoice":
                            {
                                if (SessionLocator_1.SessionLocator.TenantPM.AccountingActivated == true) {
                                    if (this.EntityPM != null && this.EntityPM.IsExternalEntity) {
                                        myButtonIsDisabled = true;
                                    }
                                }
                                else {
                                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                                        myButtonIsDisabled = true;
                                    }
                                    else if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "VD") {
                                        myButtonIsDisabled = true;
                                    }
                                }
                                break;
                            }
                        case "ReTransfer":
                            {
                                myButtonIsDisabled = true;
                                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode)) {
                                    if (this.EntityPM.StatusCode != "DR" && this.EntityPM.StatusCode != "VD") {
                                        if (this.EntityPM.TransferStatusCode == "TR") {
                                            myButtonIsDisabled = false;
                                        }
                                    }
                                }
                                break;
                            }
                        case "PrintAPInvoice":
                            {
                                myButtonIsDisabled = true;
                                if (this.EntityPM.Id != null) {
                                    myButtonIsDisabled = false;
                                }
                                break;
                            }
                        case "SendToQBO":
                            {
                                if (SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG") {
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
                    }
                    button.IsDisabled = myButtonIsDisabled;
                }
            }
        }
    };
    APInvoiceMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        if (!this.isButtonClicked) {
            this.StopFlags();
            this.StopFields();
            this.isButtonClicked = true;
            this.ClickedButtonCode = menuButton.EventCode;
            switch (menuButton.EventCode) {
                case "SaveAPInvoice":
                    {
                        this.SaveAPInvoiceClicked();
                        break;
                    }
                case "ApproveAPInvoice":
                    {
                        this.ApproveClicked();
                        break;
                    }
                case "CancelApproval":
                    {
                        this.CancelApprovalClicked();
                        break;
                    }
                case "VoidAPInvoice":
                    {
                        this.VoidClicked();
                        break;
                    }
                case "ReTransfer":
                    {
                        this.EnableReTransferClicked();
                        break;
                    }
                case "PrintAPInvoice":
                    {
                        this.PrintClicked();
                        break;
                    }
                case "SendToQBO":
                    {
                        this.SendToQBO();
                        break;
                    }
                default: {
                    this.StopFlags();
                    break;
                }
            }
        }
    };
    APInvoiceMenuButtonsHandler.prototype.SendToQBO = function () {
        var _this = this;
        var invoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        invoiceDomainService.getConnectedAPPayments(this.EntityPM.Id).subscribe(function (response) {
            if (!response.HasError) {
                if (_this.EntityPM.TransferStatusCode == "TR" || _this.EntityPM.TransferStatusCode == "ET" || _this.EntityPM.TransferStatusCode == "IP") {
                    var messageText = "Resend this invoice to QBO?";
                    if (response.Result) {
                        messageText = messageText.concat(" Please note that any connected Transferred payments will be resend after the successful transfer of this invoice");
                    }
                    var myConfirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    myConfirmWindow.Width = 400;
                    myConfirmWindow.Show(messageText);
                    myConfirmWindow.WindowClosed.subscribe(function (s) {
                        _this.StopFlags();
                        if (myConfirmWindow.Yes) {
                            _this.SendToQBOApproved("Resending Invoice to QBO");
                        }
                    });
                }
                else {
                    if (response.Result) {
                        var messageText = "Please note that any connected Transferred payments will be resend after the successful transfer of this invoice";
                        myConfirmWindow.Width = 400;
                        myConfirmWindow.Show(messageText);
                        myConfirmWindow.WindowClosed.subscribe(function (s) {
                            _this.StopFlags();
                            if (myConfirmWindow.Yes) {
                                _this.SendToQBOApproved("Sending Invoice to QBO");
                            }
                        });
                    }
                    else {
                        _this.SendToQBOApproved("Sending Invoice to QBO");
                    }
                    _this.StopFlags();
                }
            }
        });
    };
    APInvoiceMenuButtonsHandler.prototype.SendToQBOApproved = function (Text) {
        this.EntityPM.SetReSendQBO = true;
        this.EntityPM.SetVoided = false;
        this.EntityPM.SetApproved = false;
        this.EntityPM.SetReTransfer = false;
        this.entityArgs.EditComponent.SaveChanges(Text);
    };
    APInvoiceMenuButtonsHandler.prototype.StopFlags = function () {
        this.isButtonClicked = false;
        this.isPrintRequested = false;
        this.ClickedButtonCode = null;
    };
    APInvoiceMenuButtonsHandler.prototype.StopFields = function () {
    };
    APInvoiceMenuButtonsHandler.prototype.Validate = function () {
        var validator = new APInvoiceValidator_1.APInvoiceValidator();
        var errors = validator.Validate(this.EntityPM);
        this.isValid = errors.length == 0 ? true : false;
        this.entityArgs.EditComponent.ValidationErrorsList = errors;
        if (!this.isValid) {
            this.StopFlags();
        }
    };
    APInvoiceMenuButtonsHandler.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    if (_this.ClickedButtonCode == "PrintAPInvoice") {
                        _this.InitializePrinting();
                    }
                }
                _this.StopFlags();
            });
        }
        this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
            if (isLoadSuccess) {
                _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
            }
            _this.StopFlags();
        });
    };
    APInvoiceMenuButtonsHandler.prototype.SaveAPInvoiceClicked = function () {
        if (!FeatureLocator_1.FeatureLocator.HasEntityPermessions("APInvoice", "UPDT", true)) {
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
    };
    APInvoiceMenuButtonsHandler.prototype.ApproveClicked = function () {
        if (!FeatureLocator_1.FeatureLocator.HasEntityPermessions("APInvoice", "UPDT", true)) {
            this.StopFlags();
        }
        this.Validate();
        if (this.isValid) {
            this.CheckDuplication();
        }
        else {
            this.StopFlags();
        }
    };
    APInvoiceMenuButtonsHandler.prototype.CheckDuplication = function () {
        var _this = this;
        var service = new InvoiceDomainService_1.InvoiceDomainService();
        service.CheckVendor_NumberDuplication(this.EntityPM.VendorId, this.EntityPM.InvoiceNumber, this.EntityPM.Id).subscribe(function (myResponse) {
            if (myResponse.HasError) {
                _this.StopFlags();
                _this.entityArgs.EditComponent.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                var isDuplicated = myResponse.Result;
                if (isDuplicated) {
                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    confirmWindow.Title = "Warning";
                    confirmWindow.Width = 450;
                    confirmWindow.Height = 190;
                    confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Save");
                    confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Cancel");
                    confirmWindow.ShowCancelButton = false;
                    confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.M.SameInvoiceNumber"));
                    confirmWindow.WindowClosed.subscribe(function (c) {
                        if (confirmWindow.Yes) {
                            _this.ContinueSaving();
                        }
                        if (confirmWindow.No) {
                            _this.StopFlags();
                        }
                    });
                }
                else {
                    _this.ContinueSaving();
                }
            }
        });
    };
    APInvoiceMenuButtonsHandler.prototype.ContinueSaving = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("APInvoice", "NewInvoice");
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
    };
    APInvoiceMenuButtonsHandler.prototype.CancelApprovalClicked = function () {
        var _this = this;
        if (!FeatureLocator_1.FeatureLocator.HasEntityPermessions("APInvoice", "UPDT", true)) {
            this.StopFlags();
        }
        if (this.EntityPM.InvoicePayments.length > 0) {
            var messageText = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.M.DisconnectPayments");
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Width = 450;
            messageWindow.Height = 190;
            messageWindow.Title = "Logitude Message";
            messageWindow.Show(messageText);
        }
        else {
            this.Validate();
            if (this.isValid) {
                if (this.EntityPM.MainEntityId) {
                    this.CurrentSession.StartBusyIndicatorLoading();
                    var myService = new InvoiceDomainService_1.InvoiceDomainService();
                    myService.GetShipmentIsAccountingClosed(this.EntityPM.MainEntityId).subscribe(function (myResponse) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (!myResponse.HasError) {
                            var IsAccountingClosed = myResponse.Result;
                            if (IsAccountingClosed) {
                                var messageWindow = new MessageWindow_1.MessageWindow();
                                messageWindow.Show("Shipment is closed for accounting, it is not possible to perform this action");
                                _this.StopFlags();
                            }
                            else {
                                _this.CancelApprovalClickedProccess();
                            }
                        }
                        else {
                            _this.StopFlags();
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
    };
    APInvoiceMenuButtonsHandler.prototype.CancelApprovalClickedProccess = function () {
        this.EntityPM.SetVoided = false;
        this.EntityPM.SetApproved = false;
        this.EntityPM.SetReTransfer = false;
        this.EntityPM.SetCancelApproval = true;
        this.entityArgs.EditComponent.SaveChanges();
    };
    APInvoiceMenuButtonsHandler.prototype.VoidClicked = function () {
        var _this = this;
        if (SessionLocator_1.SessionLocator.AccountingSystemPM.Code == "QBO" || SessionLocator_1.SessionLocator.AccountingSystemPM.Code == "QBOG") {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("Please notice that QBO are not supporting void transmission for the APInvoice, you can void it manually from QBO");
            messageWindow.WindowClosed.subscribe(function (p) {
                _this.ShowConfirmVoidMessage();
            });
        }
        else if (this.EntityPM.TransferStatusCode == "TR") {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("Already transferred invoices can't be voided.");
            this.StopFlags();
        }
        else if (!SessionLocator_1.SessionLocator.AccountingSettingPM.AllowVoidAPI) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.M.AccountingSettingsDontAllowVoid"));
            this.StopFlags();
        }
        else if (this.EntityPM.InvoicePayments.length > 0) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.M.DisconnectPayments"));
            this.StopFlags();
        }
        else {
            this.Validate();
            if (this.isValid) {
                if (this.EntityPM.MainEntityId) {
                    this.CurrentSession.StartBusyIndicatorLoading();
                    var myService = new InvoiceDomainService_1.InvoiceDomainService();
                    myService.GetShipmentIsAccountingClosed(this.EntityPM.MainEntityId).subscribe(function (myResponse) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (!myResponse.HasError) {
                            var IsAccountingClosed = myResponse.Result;
                            if (IsAccountingClosed) {
                                var messageWindow = new MessageWindow_1.MessageWindow();
                                messageWindow.Show("Shipment is closed for accounting, it is not possible to perform this action");
                                _this.StopFlags();
                            }
                            else {
                                _this.VoidClickedProccess();
                            }
                        }
                        else {
                            _this.StopFlags();
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
    };
    APInvoiceMenuButtonsHandler.prototype.VoidClickedProccess = function () {
        this.ShowConfirmVoidMessage();
    };
    APInvoiceMenuButtonsHandler.prototype.ShowConfirmVoidMessage = function () {
        var _this = this;
        this.Validate();
        if (this.isValid) {
            var myConfirmWindow = new ConfirmWindow_1.ConfirmWindow();
            myConfirmWindow.Width = 400;
            myConfirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.M.ConfirmVoid"));
            myConfirmWindow.WindowClosed.subscribe(function (s) {
                _this.StopFlags();
                if (myConfirmWindow.Yes) {
                    _this.EntityPM.SetVoided = true;
                    _this.EntityPM.SetApproved = false;
                    _this.EntityPM.SetReTransfer = false;
                    _this.EntityPM.SetCancelApproval = false;
                    _this.EntityPM.SetReSendQBO = false;
                    _this.entityArgs.EditComponent.SaveChanges("Voiding...");
                }
            });
        }
        else {
            this.StopFlags();
        }
    };
    APInvoiceMenuButtonsHandler.prototype.EnableReTransferClicked = function () {
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
    };
    APInvoiceMenuButtonsHandler.prototype.PrintClicked = function () {
        this.Validate();
        if (this.isValid) {
            this.isPrintRequested = true;
            this.entityArgs.EditComponent.SaveChanges();
        }
        else {
            this.StopFlags();
        }
    };
    APInvoiceMenuButtonsHandler.prototype.InitializePrinting = function () {
        var myEntityId = null;
        var myEntityReference = null;
        var myEntityTableName = null;
        var myDocumentTypeCode = null;
        var myChildEntityId = null;
        var myChildObjectTableId = null;
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
            myChildObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "APInvoice"; })[0].Id;
            this.StartPrinting(myEntityId, myChildEntityId, myEntityTableName, myChildObjectTableId, myDocumentTypeCode, myEntityReference);
        }
    };
    APInvoiceMenuButtonsHandler.prototype.StartPrinting = function (myEntityId, myChildEntityId, myObjectTableName, mychildObjectTableId, myDocumentTypeCode, myReference) {
        var myPrintHelper = new GeneralPrintHelper_1.GeneralPrintHelper(myObjectTableName, myDocumentTypeCode, myEntityId, myChildEntityId, myReference, mychildObjectTableId);
        if (myPrintHelper.IsLoadPrintControl) {
            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("APInvoice", "PrintInvoice");
            myPrintHelper.ShowPrintControl();
        }
    };
    return APInvoiceMenuButtonsHandler;
}());
exports.APInvoiceMenuButtonsHandler = APInvoiceMenuButtonsHandler;
//# sourceMappingURL=APInvoiceMenuButtonsHandler.js.map