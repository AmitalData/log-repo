"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../../Infrastructure/Tools");
var ARInvoiceLinePM_1 = require("../../EntityPMs/ARInvoiceLinePM");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_2 = require("../../Tools");
var ARInvoiceValidator_1 = require("../../Validators/ARInvoiceValidator");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var InvoiceDomainService_1 = require("../../Services/InvoiceDomainService");
var GeneralPrintHelper_1 = require("../../../Infrastructure/Helpers/GeneralPrintHelper");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var ARInvoicePMService_1 = require("../../Services/StandardPMs/ARInvoicePMService");
var BatchTaskExecutionListService_1 = require("../../../Infrastructure/Services/StandardLists/BatchTaskExecutionListService");
var ARInvoiceMenuButtonsHandler = /** @class */ (function () {
    function ARInvoiceMenuButtonsHandler() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isRunningBatchTaskExecution = false;
        this.isValid = false;
        this.isButtonClicked = false;
        this.isPrintRequested = false;
        // AutoCredit
        this.IsAutoCreditConsolidation = false;
        this.AutoCreditId = null;
        this.AutoCreditDate = null;
        this.AutoCreditManualNumber = null;
    }
    ARInvoiceMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    };
    ARInvoiceMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    var myButtonIsDisabled = false;
                    switch (button.EventCode) {
                        case "SaveAsDraft":
                            {
                                myButtonIsDisabled = !Tools_2.InvoiceTool.IsEditingARInvoiceEnabled(this.EntityPM);
                                if (this.EntityPM.IsConstituentInvoice) {
                                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                                        if (this.EntityPM.IsAutoCredit) {
                                            myButtonIsDisabled = false;
                                        }
                                    }
                                }
                                button.LabelTextCodeCode = (this.EntityPM.IsConstituentInvoice) ? "General.B.Save" : "ARInvoice.B.SaveAsDraft";
                                break;
                            }
                        case "CancelDraft":
                            {
                                button.IsHidden = this.EntityPM.IsConstituentInvoice ? true : false;
                                myButtonIsDisabled = true;
                                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                                    if (this.EntityPM.StatusCode != "LL") {
                                        if (this.EntityPM.IsConstituentInvoice) {
                                            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsolidationInvoiceId) && this.EntityPM.StatusCode == "NT") {
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
                                myButtonIsDisabled = !Tools_2.InvoiceTool.IsEditingARInvoiceEnabled(this.EntityPM);
                                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id) && this.EntityPM.IsAutoCredit) {
                                    myButtonIsDisabled = false;
                                }
                                break;
                            }
                        case "SetAsSent":
                            {
                                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                                    myButtonIsDisabled = true;
                                }
                                else if (this.EntityPM.Sent || this.EntityPM.StatusCode == "VD" || this.EntityPM.StatusCode == "LL") {
                                    myButtonIsDisabled = true;
                                }
                                break;
                            }
                        case "VoidARInvoice":
                            {
                                if (SessionLocator_1.SessionLocator.TenantPM.AccountingActivated == true) {
                                    button.IsHidden = true;
                                }
                                else {
                                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                                        myButtonIsDisabled = true;
                                    }
                                    else if (this.EntityPM.StatusCode == "DR" || this.EntityPM.StatusCode == "VD" || this.EntityPM.StatusCode == "LL" || this.EntityPM.StatusCode == "AC" || this.EntityPM.StatusCode == "AR" || this.EntityPM.StatusCode == "IP") {
                                        myButtonIsDisabled = true;
                                    }
                                    else {
                                        if (this.EntityPM.IsConstituentInvoice) {
                                            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsolidationInvoiceId)) {
                                                myButtonIsDisabled = true;
                                            }
                                        }
                                        else {
                                            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode)) {
                                                myButtonIsDisabled = true;
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        case "AutoCredit":
                            {
                                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                                    myButtonIsDisabled = true;
                                }
                                else if (this.EntityPM.StatusCode == "LL" || this.EntityPM.StatusCode == "VD" || this.EntityPM.StatusCode == "AC" || this.EntityPM.StatusCode == "AR") {
                                    myButtonIsDisabled = true;
                                }
                                else if (this.EntityPM.ARInvoiceTypeCode == "IN") {
                                    var isEnabled = false;
                                    if (!this.EntityPM.IsCancelled) {
                                        if (this.EntityPM.IsConstituentInvoice) {
                                            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsolidationInvoiceId)) {
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
                                if (SessionLocator_1.SessionLocator.TenantPM.AccountingActivated == true) {
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
                                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode)) {
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
    };
    ARInvoiceMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
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
    };
    ARInvoiceMenuButtonsHandler.prototype.CheckSATStatus = function () {
        var invoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        invoiceDomainService.GetARInvoiceSATCancellationStatus(this.EntityPM.Id).subscribe(function (response) {
        });
    };
    ARInvoiceMenuButtonsHandler.prototype.SendToQBO = function () {
        var _this = this;
        var invoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        invoiceDomainService.getConnectedARPayments(this.EntityPM.Id).subscribe(function (response) {
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
    ARInvoiceMenuButtonsHandler.prototype.SendToQBOApproved = function (Text) {
        this.EntityPM.SetReSendQBO = true;
        this.EntityPM.SetVoided = false;
        this.EntityPM.SetApproved = false;
        this.EntityPM.SetReTransfer = false;
        this.EntityPM.SetCancelDraft = false;
        this.entityArgs.EditComponent.SaveChanges(Text);
    };
    ARInvoiceMenuButtonsHandler.prototype.StopFlags = function () {
        this.isButtonClicked = false;
        this.isPrintRequested = false;
    };
    ARInvoiceMenuButtonsHandler.prototype.StopFields = function () {
        this.AutoCreditId = null;
        this.AutoCreditDate = null;
        this.AutoCreditManualNumber = null;
    };
    ARInvoiceMenuButtonsHandler.prototype.Validate = function () {
        var validator = new ARInvoiceValidator_1.ARInvoiceValidator();
        var errors = validator.Validate(this.EntityPM);
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
    };
    ARInvoiceMenuButtonsHandler.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    if (_this.isPrintRequested) {
                        _this.InitializePrinting();
                    }
                    if (_this.isRunningBatchTaskExecution) {
                        _this.isRunningBatchTaskExecution = false;
                        //if (this.EntityPM.BatchTaskExecutionId) {
                        //    this.CurrentSession.StartBusyIndicator("Updating Shipments. It may take a few minutes...");
                        //    this.CheckBatchTaskExecution(this.EntityPM.BatchTaskExecutionId);
                        //}
                    }
                }
                _this.StopFlags();
            });
        }
        this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
            if (isLoadSuccess) {
                _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                if (_this.IsAutoCreditConsolidation) {
                    _this.IsAutoCreditConsolidation = false;
                    _this.CurrentSession.FireEvent("ResetARInvoiceBaseDeailsTab");
                }
            }
            _this.StopFlags();
        });
    };
    ARInvoiceMenuButtonsHandler.prototype.CheckBatchTaskExecution = function (BatchTaskExecutionId) {
        var _this = this;
        var iBatchService = new BatchTaskExecutionListService_1.BatchTaskExecutionListService();
        iBatchService.getSingle(BatchTaskExecutionId).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var list = myResponse.Result;
                if (list.StatusCode == "D") {
                    _this.CurrentSession.StopBusyIndicator();
                    var window = new MessageWindow_1.MessageWindow();
                    window.Show("Shipments updated successfully");
                }
                else if (list.StatusCode == "F") {
                    _this.CurrentSession.StopBusyIndicator();
                    var window = new MessageWindow_1.MessageWindow();
                    window.Show("There was an error updating shipments and saving the invoice. Please try again later");
                }
                else {
                    _this.CheckBatchTaskExecution(BatchTaskExecutionId);
                }
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
                var window = new MessageWindow_1.MessageWindow();
                window.Show(myResponse.ErrorsArray[0]);
            }
        });
    };
    ARInvoiceMenuButtonsHandler.prototype.SaveDraftClicked = function () {
        if (!FeatureLocator_1.FeatureLocator.HasEntityPermessions("ARInvoice", "UPDT", true)) {
            this.StopFlags();
        }
        else {
            this.Validate();
            if (this.isValid) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("ARInvoice", "New Draft Invoice");
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
    };
    ARInvoiceMenuButtonsHandler.prototype.ApproveClicked = function () {
        var _this = this;
        if (!FeatureLocator_1.FeatureLocator.HasEntityPermessions("ARInvoice", "UPDT", true)) {
            this.StopFlags();
        }
        else {
            this.Validate();
            if (this.isValid) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("ARInvoice", "New Invoice");
                }
                var helper = new Tools_2.CreditLimitHelper(this.EntityPM);
                //var isCardBlockingNewInvoiceCreation: boolean = false;
                //if (helper.HasCreditLimitFeature && helper.IsCreditLimitActivated) {
                //    if (this.EntityPM.BillToIsCreditLimitEnabled && this.EntityPM.BillToBlockNewInvoiceCreation) {
                //        isCardBlockingNewInvoiceCreation = true;
                //    }
                //}
                if (this.EntityPM.BillToBlockNewInvoiceCreation) {
                    var errorText_Blocking = "Credit limit setting is blocking invoice for bill to: " + this.EntityPM.BillToName;
                    var errors = [];
                    var warnings = [];
                    if (ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.InvoiceCreationBlock) {
                        errors.push(errorText_Blocking);
                    }
                    else if (ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.InvoiceCreationWarning) {
                        warnings.push(errorText_Blocking);
                    }
                    if (errors.length > 0 || warnings.length > 0) {
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Width = 450;
                        logWindow.Height = 200;
                        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.S.CreditLimit");
                        logWindow.WindowArgs = { Errors: errors, Warnings: warnings };
                        logWindow.WindowClosed.subscribe(function (s) {
                            if (s) {
                                _this.ApplyApproveClicked();
                            }
                            else {
                                _this.StopFlags();
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
                    var myService = new InvoiceDomainService_1.InvoiceDomainService();
                    myService.GetCustomerCreditLimitActualAmount(this.EntityPM.BillToId).subscribe(function (myResponse) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (!myResponse.HasError) {
                            helper.Run(myResponse.Result);
                        }
                        if (helper.IsValid) {
                            _this.ApplyApproveClicked();
                        }
                        else {
                            var logWindow = new LogitudeWindow_1.LogitudeWindow();
                            logWindow.Width = 450;
                            logWindow.Height = 200;
                            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.S.CreditLimit");
                            logWindow.WindowArgs = { Errors: helper.Errors, Warnings: helper.Warnings, IsBlockingShipment: helper.IsBlockingShipment, ShipmentId: _this.EntityPM.MainEntityId };
                            logWindow.WindowClosed.subscribe(function (s) {
                                if (s) {
                                    _this.ApplyApproveClicked();
                                }
                                else {
                                    _this.StopFlags();
                                }
                            });
                            logWindow.Show('./Invoice/Components/NewEntity/CreditLimitPopupComponent');
                        }
                    });
                }
                else {
                    this.ApplyApproveClicked();
                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
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
    };
    ARInvoiceMenuButtonsHandler.prototype.ApplyApproveClicked = function () {
        var _this = this;
        if (this.EntityPM.IsAutoCredit) {
            var myConfirmWindow = new ConfirmWindow_1.ConfirmWindow();
            myConfirmWindow.Width = 400;
            myConfirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.ConfirmAutoCredit"));
            myConfirmWindow.WindowClosed.subscribe(function (s) {
                if (myConfirmWindow.Yes) {
                    _this.ProceedToApprove(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.CreatingAutoCredit"));
                }
            });
        }
        else {
            this.ProceedToApprove("Approving...");
        }
    };
    ARInvoiceMenuButtonsHandler.prototype.ProceedToApprove = function (msg) {
        this.EntityPM.SetVoided = false;
        this.EntityPM.SetApproved = true;
        this.EntityPM.SetReTransfer = false;
        this.EntityPM.SetCancelDraft = false;
        this.EntityPM.SetReSendQBO = false;
        if (this.EntityPM.IsConsolidationInvoice) {
            this.isRunningBatchTaskExecution = true;
        }
        this.entityArgs.EditComponent.SaveChanges(msg);
    };
    ARInvoiceMenuButtonsHandler.prototype.CancelDraftClicked = function () {
        var _this = this;
        this.Validate();
        if (this.isValid) {
            var myConfirmWindow = new ConfirmWindow_1.ConfirmWindow();
            myConfirmWindow.Width = 400;
            myConfirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.CancelARInvoice"));
            myConfirmWindow.WindowClosed.subscribe(function (s) {
                _this.StopFlags();
                if (myConfirmWindow.Yes) {
                    _this.EntityPM.SetVoided = false;
                    _this.EntityPM.SetApproved = false;
                    _this.EntityPM.SetReTransfer = false;
                    _this.EntityPM.SetCancelDraft = true;
                    _this.entityArgs.EditComponent.SaveChanges();
                }
            });
        }
        else {
            this.StopFlags();
        }
    };
    ARInvoiceMenuButtonsHandler.prototype.SetAsSentClicked = function () {
        var _this = this;
        this.Validate();
        if (this.isValid) {
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.S.SetInvoiceAsSent");
            logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, EventCode: "SetAsSent" };
            logitudeWindow.Show("./Invoice/Components/MenuButtonsComponents/ARInvoiceMenuButtonsComponent");
            logitudeWindow.WindowClosed.subscribe(function (s) {
                _this.StopFlags();
                if (s) {
                    _this.EntityPM.Sent = true;
                    _this.EntityPM.SetAsSent = true;
                    _this.entityArgs.EditComponent.SaveChanges();
                }
            });
        }
        else {
            this.StopFlags();
        }
    };
    ARInvoiceMenuButtonsHandler.prototype.VoidClicked = function () {
        var _this = this;
        if (this.EntityPM.TransferStatusCode == "TR" && SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode != "QBO" && SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode != "QBOG") {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.AlreadyTransferredInvoicesMsg"));
            this.StopFlags();
        }
        else if (!SessionLocator_1.SessionLocator.AccountingSettingPM.AllowVoidARI) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.AccountingSettingsDontAllowVoid"));
            this.StopFlags();
        }
        else if (this.EntityPM.InvoicePayments.length > 0) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.DisconnectPayments"));
            this.StopFlags();
        }
        else {
            this.Validate();
            if (this.isValid) {
                if ((this.EntityPM.ARInvoiceTypeCode == "CD" || this.EntityPM.ARInvoiceTypeCode == "CC") && (SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG")) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Show("Please notice that QBO are not supporting void transmission for the credit note, you can void it manually from QBO");
                    messageWindow.WindowClosed.subscribe(function (p) {
                        _this.AccountingCheck();
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
    };
    ARInvoiceMenuButtonsHandler.prototype.AccountingCheck = function () {
        var _this = this;
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
    };
    ARInvoiceMenuButtonsHandler.prototype.VoidClickedProccess = function () {
        this.ShowConfirmVoidMessage();
    };
    ARInvoiceMenuButtonsHandler.prototype.ShowConfirmVoidMessage = function () {
        var _this = this;
        var myConfirmWindow = new ConfirmWindow_1.ConfirmWindow();
        myConfirmWindow.Width = 400;
        myConfirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.ConfirmVoid"));
        myConfirmWindow.WindowClosed.subscribe(function (s) {
            _this.StopFlags();
            if (myConfirmWindow.Yes) {
                _this.EntityPM.SetVoided = true;
                _this.EntityPM.SetApproved = false;
                _this.EntityPM.SetReTransfer = false;
                _this.EntityPM.SetCancelDraft = false;
                _this.EntityPM.SetReSendQBO = false;
                if (_this.EntityPM.IsConsolidationInvoice) {
                    _this.isRunningBatchTaskExecution = true;
                }
                _this.entityArgs.EditComponent.SaveChanges("Voiding...");
            }
        });
    };
    ARInvoiceMenuButtonsHandler.prototype.ReTransferClicked = function () {
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
    };
    ARInvoiceMenuButtonsHandler.prototype.AutoCreditClicked = function () {
        var _this = this;
        if (this.EntityPM.InvoicePayments.length > 0) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.S.AutoCreditingMsg1"));
            this.StopFlags();
        }
        else if (this.EntityPM.StatusCode == "PD" || this.EntityPM.StatusCode == "PP") {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.S.AutoCreditingMsg4"));
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
                                _this.AutoCreditClickedProccess();
                            }
                        }
                        else {
                            _this.StopFlags();
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
    };
    ARInvoiceMenuButtonsHandler.prototype.AutoCreditClickedProccess = function () {
        var _this = this;
        var newAutoCreditInvoice = this.CreateAutoCreditInvoice();
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityPM: newAutoCreditInvoice, ObjectTableName: 'ARInvoice', BackButtonLabel: "ARInvoice" + ": " + _this.EntityPM.InvoiceNumber });
            var isEditComponentSaved = false;
            cmpRef.instance.BackCompleted.subscribe(function (bk) {
                if (isEditComponentSaved) {
                    if (_this.EntityPM.IsConsolidationInvoice) {
                        _this.IsAutoCreditConsolidation = true;
                    }
                    _this.entityArgs.EditComponent.IsReloadNeeded = true;
                    _this.entityArgs.EditComponent.ReloadEntityPM();
                }
                else {
                    _this.StopFlags();
                }
            });
            cmpRef.instance.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    isEditComponentSaved = true;
                }
            });
            cmpRef.instance.SaveAndCloseCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    isEditComponentSaved = true;
                }
            });
        });
    };
    ARInvoiceMenuButtonsHandler.prototype.CreateAutoCreditInvoice = function () {
        var myEntityPMService = new ARInvoicePMService_1.ARInvoicePMService();
        var AutoCreditInvoice = myEntityPMService.GetNewEntityPM();
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
        AutoCreditInvoice.InvoiceDate = this.AutoCreditDate != null ? this.AutoCreditDate : Tools_1.DateTool.GetCurrentDateAsUtc();
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
        AutoCreditInvoice.IsGeneralInvoice = this.EntityPM.IsGeneralInvoice;
        AutoCreditInvoice.SalesmanUserId = this.EntityPM.SalesmanUserId;
        AutoCreditInvoice.SATPaymentMethodCode = this.EntityPM.SATPaymentMethodCode;
        AutoCreditInvoice.MetodoPagoCode = this.EntityPM.MetodoPagoCode;
        AutoCreditInvoice.IsInvoiceNumberFromStock = this.EntityPM.IsInvoiceNumberFromStock;
        AutoCreditInvoice.IsInvoiceNumberManuallySet = this.EntityPM.IsInvoiceNumberManuallySet;
        this.CreateAutoCreditInvoiceLines(AutoCreditInvoice);
        return AutoCreditInvoice;
    };
    ARInvoiceMenuButtonsHandler.prototype.CreateAutoCreditInvoiceLines = function (AutoCreditInvoice) {
        var index = 1;
        this.EntityPM.InvoiceLines.forEach(function (item) {
            var newInvoiceLine = new ARInvoiceLinePM_1.ARInvoiceLinePM(AutoCreditInvoice);
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
    };
    // Print
    ARInvoiceMenuButtonsHandler.prototype.PrintClicked = function () {
        this.Validate();
        if (this.isValid) {
            this.isPrintRequested = true;
            this.entityArgs.EditComponent.SaveChanges();
        }
        else {
            this.StopFlags();
        }
    };
    ARInvoiceMenuButtonsHandler.prototype.InitializePrinting = function () {
        var _this = this;
        var myEntityId = null;
        var myChildEntityId = null;
        var myObjectTableName = null;
        var mychildObjectTableId = null;
        var myDocumentTypeCode = null;
        var myReference = null;
        if (this.EntityPM.IsConsolidationInvoice) {
            myEntityId = this.EntityPM.Id;
            myChildEntityId = null;
            mychildObjectTableId = null;
            myObjectTableName = "ARInvoice";
            myDocumentTypeCode = "999C";
            myReference = !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.InvoiceNumber) ? this.EntityPM.InvoiceNumber : "Draft: " + this.EntityPM.DraftNumber;
            this.StartPrinting(myEntityId, myChildEntityId, myObjectTableName, mychildObjectTableId, myDocumentTypeCode, myReference);
        }
        else if (this.EntityPM.IsGeneralInvoice) {
            myEntityId = this.EntityPM.Id;
            myChildEntityId = null;
            mychildObjectTableId = null;
            myObjectTableName = "ARInvoice";
            myDocumentTypeCode = "999G";
            myReference = !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.InvoiceNumber) ? this.EntityPM.InvoiceNumber : "Draft: " + this.EntityPM.DraftNumber;
            this.StartPrinting(myEntityId, myChildEntityId, myObjectTableName, mychildObjectTableId, myDocumentTypeCode, myReference);
        }
        else {
            mychildObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "ARInvoice"; })[0].Id;
            myEntityId = this.EntityPM.MainEntityId;
            myChildEntityId = this.EntityPM.Id;
            myReference = !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.InvoiceNumber) ? this.EntityPM.InvoiceNumber : "Draft: " + this.EntityPM.DraftNumber;
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
                var myService = new InvoiceDomainService_1.InvoiceDomainService();
                myService.GetShipmentLevelCode(this.EntityPM.MainEntityId).subscribe(function (myResponse) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (!myResponse.HasError) {
                        var myShipmentLevelCode = myResponse.Result;
                        myObjectTableName = myShipmentLevelCode == "C" ? "Master" : "Shipment";
                        myDocumentTypeCode = "999S";
                        _this.StartPrinting(myEntityId, myChildEntityId, myObjectTableName, mychildObjectTableId, myDocumentTypeCode, myReference);
                    }
                });
            }
        }
    };
    ARInvoiceMenuButtonsHandler.prototype.StartPrinting = function (myEntityId, myChildEntityId, myObjectTableName, mychildObjectTableId, myDocumentTypeCode, myReference) {
        var myPrintHelper = new GeneralPrintHelper_1.GeneralPrintHelper(myObjectTableName, myDocumentTypeCode, myEntityId, myChildEntityId, myReference, mychildObjectTableId);
        if (myPrintHelper.IsLoadPrintControl) {
            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("ARInvoice", "PrintInvoice");
            myPrintHelper.ShowPrintControl();
        }
    };
    return ARInvoiceMenuButtonsHandler;
}());
exports.ARInvoiceMenuButtonsHandler = ARInvoiceMenuButtonsHandler;
//# sourceMappingURL=ARInvoiceMenuButtonsHandler.js.map