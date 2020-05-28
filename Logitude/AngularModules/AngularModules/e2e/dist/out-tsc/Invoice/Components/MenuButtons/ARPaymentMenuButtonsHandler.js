"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var InvoiceDomainService_1 = require("../../Services/InvoiceDomainService");
var Tools_1 = require("../../../Infrastructure/Tools");
var ARPaymentValidator_1 = require("../../Validators/ARPaymentValidator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var GeneralPrintHelper_1 = require("../../../Infrastructure/Helpers/GeneralPrintHelper");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ARPaymentMenuButtonsHandler = /** @class */ (function () {
    function ARPaymentMenuButtonsHandler() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isValid = false;
    }
    ARPaymentMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    };
    ARPaymentMenuButtonsHandler.prototype.ResetAllFlags = function () {
        this.isApproval = false;
        this.isCancelApproval = false;
        this.isVoided = false;
        this.isPrintRequested = false;
        this.isSATSendRequest = false;
        //this.EntityPM.SetReSendQBO = false;
    };
    ARPaymentMenuButtonsHandler.prototype.Listen = function () {
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
                    if (_this.isSATSendRequest) {
                        _this.RunSendToSAT();
                    }
                }
                _this.ResetAllFlags();
            });
        }
        this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
            if (isLoadSuccess) {
                _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
            }
        });
    };
    ARPaymentMenuButtonsHandler.prototype.Validate = function () {
        var validator = new ARPaymentValidator_1.ARPaymentValidator();
        var errors = validator.Validate(this.EntityPM);
        this.isValid = errors.length == 0 ? true : false;
        this.entityArgs.EditComponent.ValidationErrorsList = errors;
        if (!this.isValid) {
            this.ResetAllFlags();
        }
    };
    ARPaymentMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'ARPayment'; })[0];
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
                                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR") {
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
                        case "VoidARPayemnt":
                            {
                                if (SessionLocator_1.SessionLocator.TenantPM.AccountingActivated) {
                                    if (this.EntityPM.StatusCode == "AD") {
                                        button.IsDisabled = false;
                                    }
                                    else {
                                        button.IsDisabled = true;
                                    }
                                }
                                else {
                                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id) || this.EntityPM.StatusCode == "VD") {
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
                                button.IsDisabled = true;
                                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode)) {
                                    if (this.EntityPM.StatusCode != "DR" && this.EntityPM.StatusCode != "VD") {
                                        if (this.EntityPM.TransferStatusCode == "TR") {
                                            button.IsDisabled = false;
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
                                if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "NONE") {
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
                                if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "NONE") {
                                    button.IsHidden = true;
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
    ARPaymentMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
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
    };
    ARPaymentMenuButtonsHandler.prototype.SendToQBO = function () {
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
            this.SendToQBOApproved("Sending Invoice to QBO");
            this.ResetAllFlags();
        }
    };
    ARPaymentMenuButtonsHandler.prototype.SendToQBOApproved = function (Text) {
        var _this = this;
        this.EntityPM.SetReSendQBO = true;
        this.EntityPM.SetVoided = false;
        this.EntityPM.SetApproved = false;
        this.EntityPM.SetReTransfer = false;
        this.EntityPM.SetCancelApproval = false;
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG") {
            var FlagNotTransfered = false;
            this.EntityPM.PaymentInvoices.forEach(function (item) {
                if (item.ARInvoiceTransferStatusCode != "TR") {
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
    ARPaymentMenuButtonsHandler.prototype.CheckSATStatus = function () {
        var invoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        var invoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        invoiceDomainService.GetARPaymentSATCancellationStatus(this.EntityPM.Id).subscribe(function (response) {
        });
    };
    ARPaymentMenuButtonsHandler.prototype.RunSendToSAT = function () {
        var windowArgs = {};
        windowArgs.EnttiyPM = this.EntityPM;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 400;
        logWindow.Title = "Send to SAT";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./Invoice/Components/SAT/SendPaymentWindowComponent');
        logWindow.WindowClosed.subscribe(function ($event) {
            //this.StopBusyIndicator();
        });
    };
    ARPaymentMenuButtonsHandler.prototype.SaveSendToSAT = function () {
        var _this = this;
        var message = "";
        if (!FeatureLocator_1.FeatureLocator.HasEntityPermessions("ARPayment", "UPDT", true)) {
            return;
        }
        var errors = ARPaymentValidator_1.ARPaymentValidator.ValidateCurrenctEntity(this.EntityPM);
        var isValid = true;
        if (errors != null && errors.length > 0) {
            isValid = false;
        }
        if (isValid) {
            this.isSATSendRequest = true;
            this.entityArgs.EditComponent.SaveChanges();
        }
        else {
            errors.forEach(function (item) {
                if (_this.entityArgs.EditComponent.ValidationErrorsList == null) {
                    _this.entityArgs.EditComponent.ValidationErrorsList = [];
                }
                _this.entityArgs.EditComponent.ValidationErrorsList.push(item);
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
    };
    // [Approval]
    ARPaymentMenuButtonsHandler.prototype.ApprovalMethod = function () {
        var _this = this;
        // full accounting validation
        //lines validation
        if (SessionLocator_1.SessionLocator.TenantPM.AccountingActivated) {
            var _edit = this.CurrentSession.CurrentEditComponent;
            if (!_edit.IsEditValid) {
                _edit.ValidationErrorsList = [TextCodeTranslator_1.TextCodeTranslator.Translate('Reconciliations.O.ErrorsInSelectedLines')];
                return;
            }
            else {
                _edit.ValidationErrorsList = [];
            }
        }
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG") {
            var FlagNotTransfered = false;
            this.EntityPM.PaymentInvoices.forEach(function (item) {
                if (item.ARInvoiceTransferStatusCode != "TR") {
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
    };
    ARPaymentMenuButtonsHandler.prototype.CompleteApprove = function () {
        var _this = this;
        if (this.EntityPM.AccountingPaymentMethodCode == "FS" && (SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG")) {
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
    ARPaymentMenuButtonsHandler.prototype.ApprovingLogic = function () {
        var _this = this;
        var message = "";
        if (!FeatureLocator_1.FeatureLocator.HasEntityPermessions("ARPayment", "UPDT", true)) {
            return;
        }
        var errors = ARPaymentValidator_1.ARPaymentValidator.ValidateCurrenctEntity(this.EntityPM);
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
        }
        else {
            errors.forEach(function (item) {
                if (_this.entityArgs.EditComponent.ValidationErrorsList == null) {
                    _this.entityArgs.EditComponent.ValidationErrorsList = [];
                }
                _this.entityArgs.EditComponent.ValidationErrorsList.push(item);
            });
        }
    };
    ARPaymentMenuButtonsHandler.prototype.CreateARPaymentCheque = function () {
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
        var invoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        invoiceDomainService.PostARPaymentChequeAndCashBook(this.EntityPM).subscribe(function (response) {
            if (response != null) {
                if (!response.HasError) {
                }
                else {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Show(response.ErrorsArray.toString());
                }
            }
        });
    };
    // [Cancel Approval]
    ARPaymentMenuButtonsHandler.prototype.CancelApproval = function () {
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
    };
    ARPaymentMenuButtonsHandler.prototype.PrintPayment = function () {
        var errors = ARPaymentValidator_1.ARPaymentValidator.ValidateCurrenctEntity(this.EntityPM);
        var isValid = true;
        if (errors != null && errors.length > 0) {
            isValid = false;
        }
        if (isValid == true) {
            this.isPrintRequested = true;
            this.entityArgs.EditComponent.SaveChanges();
        }
    };
    ARPaymentMenuButtonsHandler.prototype.InitializePrinting = function () {
        var myEntityId = null;
        var myChildEntityId = null;
        var myObjectTableName = null;
        var myDocumentTypeCode = null;
        var myReference = null;
        var mychildObjectTableId = null;
        myEntityId = this.EntityPM.Id;
        myChildEntityId = null;
        myObjectTableName = "ARPayment";
        myDocumentTypeCode = "ARP";
        myReference = this.EntityPM.PaymentNo;
        this.StartPrinting(myEntityId, myChildEntityId, myObjectTableName, mychildObjectTableId, myDocumentTypeCode, myReference);
    };
    ARPaymentMenuButtonsHandler.prototype.StartPrinting = function (myEntityId, myChildEntityId, myObjectTableName, mychildObjectTableId, myDocumentTypeCode, myReference) {
        var myPrintHelper = new GeneralPrintHelper_1.GeneralPrintHelper(myObjectTableName, myDocumentTypeCode, myEntityId, myChildEntityId, myReference, mychildObjectTableId);
        if (myPrintHelper.IsLoadPrintControl) {
            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("ARPayment", "PrintARPayment");
            myPrintHelper.ShowPrintControl();
        }
    };
    ARPaymentMenuButtonsHandler.prototype.PrintPaymentButtonLoaded = function () {
    };
    // [Void]
    ARPaymentMenuButtonsHandler.prototype.VoidMethod = function () {
        var _this = this;
        var messageWindow;
        if (!SessionLocator_1.SessionLocator.AccountingSettingPM.AllowVoidARP) {
            var messageText = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.AccountingSettingsDontAllowVoid");
            messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show(messageText);
            return;
        }
        var errors = ARPaymentValidator_1.ARPaymentValidator.ValidateCurrenctEntity(this.EntityPM);
        var isValid = true;
        if (errors != null && errors.length > 0) {
            isValid = false;
        }
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE" && (this.EntityPM.SATTransferStatusCode == "TD" || this.EntityPM.SATTransferStatusCode == "TG") && (this.EntityPM.StatusCode == "AD" || this.EntityPM.StatusCode == "CL")) {
            var messageText = "This Payment is connected to SAT, Please cancel payment approval before voiding it"; //TextCodeTranslator.Translate("ARPayment.M.AccountingSettingsDontAllowVoid");
            messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show(messageText);
            return;
        }
        if (isValid) {
            if (this.EntityPM.PaymentInvoices.length > 0) {
                var messageText = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.DisconnectInvoices");
                messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show(messageText);
            }
            else {
                var confirmVoid = new ConfirmWindow_1.ConfirmWindow();
                confirmVoid.Width = 400;
                var confirmMsg = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.ConfirmVoid");
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
        }
    };
    //[ReTransfer]
    ARPaymentMenuButtonsHandler.prototype.ReTransferClicked = function () {
        this.Validate();
        if (this.isValid) {
            this.EntityPM.SetVoided = false;
            this.EntityPM.SetApproved = false;
            this.EntityPM.SetReTransfer = true;
            this.EntityPM.SetCancelApproval = false;
            this.entityArgs.EditComponent.SaveChanges();
        }
    };
    return ARPaymentMenuButtonsHandler;
}());
exports.ARPaymentMenuButtonsHandler = ARPaymentMenuButtonsHandler;
//# sourceMappingURL=ARPaymentMenuButtonsHandler.js.map