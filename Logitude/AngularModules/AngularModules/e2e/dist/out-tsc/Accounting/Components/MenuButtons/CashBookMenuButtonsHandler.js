"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var Tools_1 = require("../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var CashBookMenuButtonsHandler = /** @class */ (function () {
    function CashBookMenuButtonsHandler() {
        this.ObjectTableName = "CashBook";
        this.TotalSum = 0;
        this.isRTL = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    CashBookMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    };
    CashBookMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'CashBook'; })[0];
                var buttonEnabled = true;
                var eventsTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "UPDATE") && f.ObjectTableId == table.Id; })[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "CashBookInactive":
                            {
                                if (this.EntityPM.Inactive == true) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "CashBookDeposite":
                            {
                                if (this.EntityPM.Inactive == true) {
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
    CashBookMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        var errors = [];
        if (errors.length == 0) {
            switch (menuButton.EventCode) {
                case "CashBookInactive":
                    {
                        this.CalculateTotals();
                        //if (this.TotalSum != 0) {
                        if (this.EntityPM.TotalAmount != 0) {
                            this.entityArgs.EditComponent.ValidationErrorsList = [];
                            this.entityArgs.EditComponent.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.BalanceOfCashbookUnequalZeroCantBlocked")); //"The balance of the cashbook is unequal to zero, can’t be blocked");
                        }
                        else {
                            this.EntityPM.Inactive = true;
                            this.entityArgs.EditComponent.SaveChanges();
                        }
                        break;
                    }
                case "CashBookDeposite":
                    {
                        this.RunNewDepositWizard();
                        break;
                    }
            }
        }
        else {
            this.entityArgs.EditComponent.ValidationErrorsList = [];
            this.entityArgs.EditComponent.ValidationErrorsList = errors;
        }
    };
    CashBookMenuButtonsHandler.prototype.CalculateTotals = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CashBookLines)) {
            for (var _i = 0, _a = this.EntityPM.CashBookLines; _i < _a.length; _i++) {
                var line = _a[_i];
                this.TotalSum += line.LocalAmount;
            }
        }
    };
    CashBookMenuButtonsHandler.prototype.RunNewDepositWizard = function () {
        var _this = this;
        if (this.EntityPM.CashBookTypeCode == "2") { // 2-Cheque
            var exist = this.EntityPM.CashBookLines.find(function (d) { return d.IsDeposited == false; });
            if (!exist) {
                var msg = new MessageWindow_1.MessageWindow();
                msg.Width = 350;
                msg.RTL = this.isRTL;
                msg.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.NoChequesInCashbook")); //"There are no Cheques in the Cashbook");
                return;
            }
        }
        else if (this.EntityPM.CashBookTypeCode == "1") { // 1-Cash
            if (Tools_1.AppTool.IsNullOrZero(this.EntityPM.TotalAmount)) {
                var msg = new MessageWindow_1.MessageWindow();
                msg.Width = 350;
                msg.RTL = this.isRTL;
                msg.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.NoCashInCashbook")); //"There are no Cash in the Cashbook");
                return;
            }
        }
        var windowTitle = "New Deposit";
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NewDeposit");
        var windowArgs = new Args;
        windowArgs.CashBookId = this.EntityPM.Id;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 520;
        logWindow.Height = 240;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) {
            _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        });
        logWindow.Show('./Accounting/Components/NewEntity/NewBankDepositComponent');
    };
    CashBookMenuButtonsHandler.prototype.StartBusyIndicator = function (message) {
        this.CurrentSession.StartBusyIndicator(message);
    };
    CashBookMenuButtonsHandler.prototype.StopBusyIndicator = function () {
        this.CurrentSession.StopBusyIndicator();
    };
    return CashBookMenuButtonsHandler;
}());
exports.CashBookMenuButtonsHandler = CashBookMenuButtonsHandler;
var Args = /** @class */ (function () {
    function Args() {
    }
    return Args;
}());
exports.Args = Args;
//# sourceMappingURL=CashBookMenuButtonsHandler.js.map