"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var GLAccountPMService_1 = require("../../Services/StandardPMs/GLAccountPMService");
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var BankAccountPMService_1 = require("../../Services/StandardPMs/BankAccountPMService");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var GeneralPrintHelper_1 = require("../../../Infrastructure/Helpers/GeneralPrintHelper");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var PaymentChequeMenuButtonsHandler = /** @class */ (function () {
    function PaymentChequeMenuButtonsHandler() {
        this.ObjectTableName = "PaymentCheque";
        this.gLAccountPMService = new GLAccountPMService_1.GLAccountPMService();
        this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        this.bankAccountPMService = new BankAccountPMService_1.BankAccountPMService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    PaymentChequeMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    };
    PaymentChequeMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'PaymentCheque'; })[0];
                var buttonEnabled = true;
                var eventsTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "UPDATE") && f.ObjectTableId == table.Id; })[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "SaveAsDraft":
                            {
                                if (this.EntityPM.PaymentChequeStatusCode == "2" || this.EntityPM.IsCancelled) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "Approve":
                            {
                                //  button.Width = 120;
                                if (this.EntityPM.PaymentChequeStatusCode == "2" || this.EntityPM.IsCancelled) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "More":
                            {
                                //     button.Width = 500;
                                break;
                            }
                        case "CancelCheque":
                            {
                                if (this.EntityPM.PaymentChequeStatusCode == "3" || this.EntityPM.IsCancelled || !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.APPaymentId)) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "PrintCheque":
                            {
                                if (this.EntityPM.PaymentChequeStatusCode != "2") {
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
        return menuButtons;
    };
    PaymentChequeMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        var _this = this;
        var errors = [];
        switch (menuButton.EventCode) {
            case "SaveAsDraft":
                {
                    this.EntityPM.PaymentChequeStatusCode = "1";
                    this.CheckCurrency();
                    Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
                    for (var _i = 0, _a = this.EntityPM.PaymentChequeLines; _i < _a.length; _i++) {
                        var item = _a[_i];
                        if (Tools_1.AppTool.IsNullOrEmpty(item.Amount)) {
                            var s = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("PaymentChequeLine.F.Amount"));
                            errors.push(s + " " + TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Line") + " " + item.SequenceNumeric);
                        }
                        if (Tools_1.AppTool.IsNullOrEmpty(item.Notes)) {
                            var s = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("PaymentChequeLine.F.Notes"));
                            errors.push(s + " " + TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Line") + " " + item.SequenceNumeric);
                        }
                    }
                    this.entityArgs.EditComponent.ValidationErrorsList = errors;
                    this.SaveChanges();
                    break;
                }
            case "Approve":
                {
                    Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
                    if (this.EntityPM.CurrencyId != this.EntityPM.BankGLAccountCurrencyId) {
                        errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.BankAccountDifferentCurrencies"));
                    }
                    var totalAmount = 0;
                    for (var _b = 0, _c = this.EntityPM.PaymentChequeLines; _b < _c.length; _b++) {
                        var item = _c[_b];
                        if (Tools_1.AppTool.IsNullOrEmpty(item.Amount)) {
                            var s = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("PaymentChequeLine.F.Amount"));
                            errors.push(s + " " + TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Line") + " " + item.SequenceNumeric);
                        }
                        if (Tools_1.AppTool.IsNullOrEmpty(item.Notes)) {
                            var s = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("PaymentChequeLine.F.Notes"));
                            errors.push(s + " " + TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Line") + " " + item.SequenceNumeric);
                        }
                        totalAmount = totalAmount + item.Amount;
                    }
                    if (totalAmount != this.EntityPM.LocalAmount) {
                        errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.DifferentAmounts"));
                    }
                    this.entityArgs.EditComponent.ValidationErrorsList = errors;
                    if (this.entityArgs.EditComponent.ValidationErrorsList.length == 0) {
                        this.bankAccountPMService.get(this.EntityPM.BankAccountId).subscribe(function (res) {
                            if (res) {
                                if (res.Result) {
                                    if (res.Result.ChequeCounter == null) {
                                        errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NoChequeCounter"));
                                        _this.entityArgs.EditComponent.ValidationErrorsList = errors;
                                    }
                                    else {
                                        //   this.EntityPM.ChequeNumber = res.Result.ChequeCounter;
                                        _this.EntityPM.PaymentChequeStatusCode = "2";
                                        _this.EntityPM.ApproveDate = new Date();
                                        _this.EntityPM.ApprovedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                                        //this.SaveChanges();
                                        _this.entityArgs.EditComponent.SaveChanges();
                                        _this.entityArgs.EditComponent.SaveCompleted.subscribe(function ($event) {
                                            if ($event == true) {
                                                _this.CurrentSession.DisableFieldsEvent.emit({});
                                                _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                            }
                                        });
                                    }
                                }
                            }
                        });
                    }
                    break;
                }
            case "CancelCheque": {
                var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.CancellationReason");
                var windowArgs = {};
                windowArgs.PaymentChequePM = this.EntityPM;
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 520;
                logWindow.Height = 200;
                logWindow.Title = windowTitle;
                logWindow.WindowArgs = windowArgs;
                logWindow.WindowClosed.subscribe(function ($event) { return _this.ReloadEntityPM($event); });
                logWindow.Show('./Accounting/Components/Others/CancelChequeComponent');
                break;
            }
            case "PrintCheque": {
                this.PrintPaymentCheque();
                break;
            }
        }
    };
    PaymentChequeMenuButtonsHandler.prototype.ReloadEntityPM = function (key) {
        if (key == "ok") {
            this.CurrentSession.DisableFieldsEvent.emit({});
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        }
    };
    PaymentChequeMenuButtonsHandler.prototype.SaveChanges = function () {
        if (this.entityArgs.EditComponent.ValidationErrorsList.length == 0) {
            this.entityArgs.EditComponent.SaveChanges();
        }
    };
    PaymentChequeMenuButtonsHandler.prototype.CheckCurrency = function () {
        if (!this.EntityPM.IsGLAccountMultiCurrency) {
            if (this.EntityPM.CurrencyId != null) {
                if (this.EntityPM.GLAccountCurrencyId != this.EntityPM.CurrencyId) {
                    this.entityArgs.EditComponent.ValidationErrorsList = [];
                    this.entityArgs.EditComponent.ValidationErrorsList.push("the payment currency does not match to the bill to GLAccount Currency!");
                }
                else {
                    this.entityArgs.EditComponent.ValidationErrorsList = [];
                }
            }
        }
        if (!this.EntityPM.IsBankGlAccountMultiCur) {
            if (this.EntityPM.CurrencyId != null) {
                if (this.EntityPM.BankGLAccountCurrencyId != this.EntityPM.CurrencyId) {
                    this.entityArgs.EditComponent.ValidationErrorsList = [];
                    this.entityArgs.EditComponent.ValidationErrorsList.push("the payment currency does not match to the bank  GLAccount Currency!");
                }
                else {
                    this.entityArgs.EditComponent.ValidationErrorsList = [];
                }
            }
        }
    };
    PaymentChequeMenuButtonsHandler.prototype.PrintPaymentCheque = function () {
        var myPrintHelper = new GeneralPrintHelper_1.GeneralPrintHelper("PaymentCheque", "PCDR", this.EntityPM.Id, null, this.EntityPM.ChequeNumber, null);
        if (myPrintHelper.IsLoadPrintControl) {
            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("PaymentCheque", "PrintCheque");
            myPrintHelper.ShowPrintControl();
        }
        //var printService = new DocumentsPrintHelper(this.ObjectTableName, this.EntityPM.Id, this.EntityPM.Tenant);
        //printService.BuildAndPrintDocument("PCDR");
    };
    return PaymentChequeMenuButtonsHandler;
}());
exports.PaymentChequeMenuButtonsHandler = PaymentChequeMenuButtonsHandler;
//# sourceMappingURL=PaymentChequeMenuButtonsHandler.js.map