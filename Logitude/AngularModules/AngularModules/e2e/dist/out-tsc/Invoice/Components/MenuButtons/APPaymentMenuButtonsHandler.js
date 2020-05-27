"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var APPaymentValidator_1 = require("../../Validators/APPaymentValidator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var GeneralPrintHelper_1 = require("../../../Infrastructure/Helpers/GeneralPrintHelper");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var APPaymentMenuButtonsHandler = /** @class */ (function () {
    function APPaymentMenuButtonsHandler() {
        this.customValidator = new APPaymentValidator_1.APPaymentValidator();
        this.isOerationInProgrees = false;
    }
    APPaymentMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    };
    APPaymentMenuButtonsHandler.prototype.ResetAllFlags = function () {
        this.isApproval = false;
        this.isCancelApproval = false;
        this.isVoided = false;
        this.isPrintRequested = false;
        this.isOerationInProgrees = false;
    };
    APPaymentMenuButtonsHandler.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    if (_this.isApproval) {
                        _this.isApproval = false;
                    }
                    if (_this.isCancelApproval) {
                        _this.isCancelApproval = false;
                    }
                    if (_this.isVoided) {
                        _this.isVoided = false;
                    }
                    if (_this.isPrintRequested) {
                        _this.isPrintRequested = false;
                        _this.InitializePrinting();
                    }
                }
                _this.ResetAllFlags();
            });
        }
        this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
            if (isLoadSuccess) {
                _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
            }
            _this.ResetAllFlags();
        });
    };
    APPaymentMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'APPayment'; })[0];
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
                                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR") {
                                    button.IsDisabled = false;
                                }
                                else {
                                    button.IsDisabled = true;
                                }
                                break;
                            }
                        case "CancelApproval":
                            {
                                if (SessionLocator_1.SessionLocator.TenantPM.AccountingActivated) {
                                    button.IsHidden = true;
                                }
                                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR" || this.EntityPM.StatusCode == "VD" || (this.EntityPM.StatusCode == "AD" && (SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG"))) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "VoidAPPayment":
                            {
                                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id) || this.EntityPM.StatusCode == "VD") {
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
    };
    APPaymentMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
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
    };
    APPaymentMenuButtonsHandler.prototype.SendToQBO = function () {
        var _this = this;
        if (this.EntityPM.TransferStatusCode == "TR" || this.EntityPM.TransferStatusCode == "ET" || this.EntityPM.TransferStatusCode == "IP") {
            var myConfirmWindow = new ConfirmWindow_1.ConfirmWindow();
            myConfirmWindow.Width = 400;
            myConfirmWindow.Show("Resend this payment to QBO?");
            myConfirmWindow.WindowClosed.subscribe(function (s) {
                _this.ResetAllFlags();
                if (myConfirmWindow.Yes) {
                    _this.SendToQBOApproved("Resending payment to QBO");
                }
            });
        }
        else {
            this.SendToQBOApproved("Sending payment to QBO");
            this.ResetAllFlags();
        }
    };
    APPaymentMenuButtonsHandler.prototype.SendToQBOApproved = function (Text) {
        var _this = this;
        this.EntityPM.SetReSendQBO = true;
        this.EntityPM.SetVoided = false;
        this.EntityPM.SetApproved = false;
        this.EntityPM.SetCancelApproval = false;
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG") {
            var FlagNotTransfered = false;
            this.EntityPM.PaymentInvoices.forEach(function (item) {
                if (item.APInvoiceTransferStatusCode != "TR") {
                    FlagNotTransfered = true;
                }
            });
            if (FlagNotTransfered) {
                var window = new MessageWindow_1.MessageWindow();
                window.Show("Invoices that were not transferred to QBO will not be connected to the payment at QBO");
                window.WindowClosed.subscribe(function (event) {
                    _this.entityArgs.EditComponent.SaveChanges(Text);
                });
            }
            else {
                this.entityArgs.EditComponent.SaveChanges(Text);
            }
        }
        else {
            this.entityArgs.EditComponent.SaveChanges(Text);
        }
    };
    // [Approval]
    APPaymentMenuButtonsHandler.prototype.ApprovalMethod = function () {
        var _this = this;
        if (!FeatureLocator_1.FeatureLocator.HasEntityPermessions("APPayment", "UPDT", true)) {
            return;
        }
        if (!this.isOerationInProgrees) {
            this.isOerationInProgrees = true;
            if (SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG") {
                var FlagNotTransfered = false;
                this.EntityPM.PaymentInvoices.forEach(function (item) {
                    if (item.APInvoiceTransferStatusCode != "TR") {
                        FlagNotTransfered = true;
                    }
                });
                if (FlagNotTransfered) {
                    var window = new MessageWindow_1.MessageWindow();
                    window.Show("Invoices that were not transferred to QBO will not be connected to the payment at QBO");
                    window.WindowClosed.subscribe(function (event) {
                        _this.CompleteApprove();
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
    };
    APPaymentMenuButtonsHandler.prototype.CompleteApprove = function () {
        var _this = this;
        if (this.EntityPM.PaymentMethodCode == "FS" && (SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG")) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("This payments with payment method Offsetting will not be transfered to quickbooks online , transfer it manually");
            messageWindow.WindowClosed.subscribe(function (a) {
                _this.ApprovingLogic();
            });
        }
        else {
            this.ApprovingLogic();
        }
    };
    APPaymentMenuButtonsHandler.prototype.ApprovingLogic = function () {
        var _this = this;
        var message = "";
        var isValid = true;
        var errors = this.customValidator.Validate(this.EntityPM);
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
            if (this.entityArgs.EditComponent.ValidationErrorsList == null) {
                this.entityArgs.EditComponent.ValidationErrorsList = [];
            }
            errors.forEach(function (item) {
                _this.entityArgs.EditComponent.ValidationErrorsList.push(item);
            });
            this.isOerationInProgrees = false;
        }
    };
    // [Cancel Approval]
    APPaymentMenuButtonsHandler.prototype.CancelApproval = function () {
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
    };
    APPaymentMenuButtonsHandler.prototype.PrintPayment = function () {
        var validator = new APPaymentValidator_1.APPaymentValidator();
        var errors = validator.Validate(this.EntityPM);
        var isValid = true;
        if (errors != null && errors.length > 0) {
            isValid = false;
        }
        if (isValid == true) {
            this.isPrintRequested = true;
            this.entityArgs.EditComponent.SaveChanges();
        }
    };
    APPaymentMenuButtonsHandler.prototype.InitializePrinting = function () {
        var myEntityId = null;
        var myChildEntityId = null;
        var myObjectTableName = null;
        var myDocumentTypeCode = null;
        var myReference = null;
        var mychildObjectTableId = null;
        myEntityId = this.EntityPM.Id;
        myChildEntityId = null;
        myObjectTableName = "APPayment";
        myDocumentTypeCode = "APP";
        this.StartPrinting(myEntityId, myChildEntityId, myObjectTableName, mychildObjectTableId, myDocumentTypeCode, myReference);
    };
    APPaymentMenuButtonsHandler.prototype.StartPrinting = function (myEntityId, myChildEntityId, myObjectTableName, mychildObjectTableId, myDocumentTypeCode, myReference) {
        var myPrintHelper = new GeneralPrintHelper_1.GeneralPrintHelper(myObjectTableName, myDocumentTypeCode, myEntityId, myChildEntityId, myReference, mychildObjectTableId);
        if (myPrintHelper.IsLoadPrintControl) {
            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("APPayment", "PrintAPPayment");
            myPrintHelper.ShowPrintControl();
        }
    };
    APPaymentMenuButtonsHandler.prototype.PrintPaymentButtonLoaded = function () {
    };
    APPaymentMenuButtonsHandler.prototype.VoidingAPPayment = function () {
        var _this = this;
        var messageWindow;
        if (this.EntityPM.PaymentInvoices.length > 0) {
            var messageText = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.M.DisconnectInvoices");
            messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show(messageText);
        }
        else {
            var confirmVoid = new ConfirmWindow_1.ConfirmWindow();
            confirmVoid.Width = 400;
            var confirmMsg = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.M.ConfirmVoid");
            confirmVoid.ShowCancelButton = false;
            confirmVoid.WindowClosed.subscribe(function (c) {
                if (confirmVoid.Yes) {
                    _this.EntityPM.SetVoided = true;
                    _this.EntityPM.SetApproved = false;
                    _this.EntityPM.SetCancelApproval = false;
                    if (_this.CurrentDocument != null) {
                        _this.CurrentDocument.NeedsRebuild = true;
                        //CommonContext.SubmitChanges();
                    }
                    _this.entityArgs.EditComponent.SaveChanges();
                }
            });
            confirmVoid.Show(confirmMsg);
        }
    };
    // [Void]
    APPaymentMenuButtonsHandler.prototype.VoidMethod = function () {
        var _this = this;
        var messageWindow;
        if (!SessionLocator_1.SessionLocator.AccountingSettingPM.AllowVoidAPP) {
            var messageText = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.M.AccountingSettingsDontAllowVoid");
            messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show(messageText);
            return;
        }
        var errors = this.customValidator.Validate(this.EntityPM);
        var isValid = true;
        if (errors != null && errors.length > 0) {
            isValid = false;
        }
        if (isValid) {
            if (SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG") {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show("Please notice that QBO are not supporting void transmission for the APpayment, you can void it manually from QBO");
                messageWindow.WindowClosed.subscribe(function (p) {
                    _this.VoidingAPPayment();
                });
            }
            else {
                this.VoidingAPPayment();
            }
        }
    };
    return APPaymentMenuButtonsHandler;
}());
exports.APPaymentMenuButtonsHandler = APPaymentMenuButtonsHandler;
//# sourceMappingURL=APPaymentMenuButtonsHandler.js.map