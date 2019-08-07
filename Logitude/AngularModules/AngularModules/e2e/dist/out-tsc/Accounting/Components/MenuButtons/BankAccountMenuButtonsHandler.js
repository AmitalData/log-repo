"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var LedgerTransactionExtendedListService_1 = require("../../Services/ExtendedLists/LedgerTransactionExtendedListService");
var CurrencyPMService_1 = require("../../../Common/Services/StandardPMs/CurrencyPMService");
var BankAccountMenuButtonsHandler = /** @class */ (function () {
    function BankAccountMenuButtonsHandler() {
        this.ObjectTableName = "BankAccount";
        this.TotalSum = 0;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this._LedgerTransactionExtendedListService = new LedgerTransactionExtendedListService_1.LedgerTransactionExtendedListService();
        this._CurrencyPMService = new CurrencyPMService_1.CurrencyPMService();
    }
    BankAccountMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    };
    BankAccountMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'BankAccount'; })[0];
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "BankAccountReconcile":
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
    BankAccountMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        var _this = this;
        var errors = [];
        if (errors.length == 0) {
            switch (menuButton.EventCode) {
                case "BankAccountReconcile":
                    {
                        this.CurrentSession.StartBusyIndicatorLoading();
                        if (!this.EntityPM.GLAccountCurrencyId || this.EntityPM.GLAccountCurrencyId == "multi") {
                            this._LedgerTransactionExtendedListService.GetFirstLedgerTransaction(this.EntityPM.GLAccountId).subscribe(function (serviceResponse) {
                                if (serviceResponse.Result) {
                                    var result = serviceResponse.Result;
                                    var transaction = result.Result; // get the data
                                    var openAmountCurrency = transaction ? transaction.OpenAmountCurrencySign : "";
                                    _this.showReconcileWindow(openAmountCurrency);
                                }
                                _this.CurrentSession.StopBusyIndicator();
                            });
                        }
                        else {
                            this._CurrencyPMService.get(this.EntityPM.GLAccountCurrencyId).subscribe(function (myResult) {
                                var currency = myResult.Result;
                                var openAmountCurrency = currency ? currency.Sign : "";
                                _this.showReconcileWindow(openAmountCurrency);
                                _this.CurrentSession.StopBusyIndicator();
                            });
                        }
                        break;
                    }
            }
        }
        else {
            this.entityArgs.EditComponent.ValidationErrorsList = [];
            this.entityArgs.EditComponent.ValidationErrorsList = errors;
        }
    };
    BankAccountMenuButtonsHandler.prototype.getScreenHeight = function () {
        if (self.innerHeight) {
            return self.innerHeight;
        }
        if (document.documentElement && document.documentElement.clientHeight) {
            return document.documentElement.clientHeight;
        }
        if (document.body) {
            return document.body.clientHeight;
        }
    };
    BankAccountMenuButtonsHandler.prototype.getScreenWidth = function () {
        if (self.innerWidth) {
            return self.innerWidth;
        }
        if (document.documentElement && document.documentElement.clientWidth) {
            return document.documentElement.clientWidth;
        }
        if (document.body) {
            return document.body.clientWidth;
        }
    };
    BankAccountMenuButtonsHandler.prototype.showReconcileWindow = function (currency) {
        var _this = this;
        var screenWidth = this.getScreenWidth();
        var screenHeight = this.getScreenHeight();
        var windowArgs = {};
        windowArgs.BankAccountPM = this.EntityPM;
        windowArgs.openAmountCurrency = currency; // CurrencySign
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = (screenWidth > 1024) ? (screenWidth > 1200 ? 1500 : screenWidth - 20) : 900;
        logitudeWindow.Height = (screenHeight > 768) ? (screenHeight > 850 ? 700 : screenHeight - 70) : screenHeight - 70;
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.ExternalReconcile");
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Accounting/Components/Others/ExternalReconcileComponent');
        logitudeWindow.WindowClosed.subscribe(function ($event) {
            // show alert
            _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        });
    };
    return BankAccountMenuButtonsHandler;
}());
exports.BankAccountMenuButtonsHandler = BankAccountMenuButtonsHandler;
//# sourceMappingURL=BankAccountMenuButtonsHandler.js.map