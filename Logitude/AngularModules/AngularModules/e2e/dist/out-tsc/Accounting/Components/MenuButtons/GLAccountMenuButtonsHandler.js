"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var GLAccountListService_1 = require("../../Services/StandardLists/GLAccountListService");
var GLAccountExtendedListService_1 = require("../../Services/ExtendedLists/GLAccountExtendedListService");
var LedgerTransactionExtendedListService_1 = require("../../Services/ExtendedLists/LedgerTransactionExtendedListService");
var GLAccountMenuButtonsHandler = /** @class */ (function () {
    function GLAccountMenuButtonsHandler() {
        this.ObjectTableName = "GLAccount";
        this.TotalSum = 0;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this._LedgerTransactionExtendedListService = new LedgerTransactionExtendedListService_1.LedgerTransactionExtendedListService();
        this.glAccountExtendedListService = new GLAccountExtendedListService_1.GLAccountExtendedListService();
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.TabSelectedEvent = null;
    }
    GLAccountMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    };
    GLAccountMenuButtonsHandler.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                    }
                });
            }
        }
    };
    GLAccountMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'GLAccount'; })[0];
                var buttonEnabled = true;
                var eventsTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "UPDATE") && f.ObjectTableId == table.Id; })[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "GLAccountInactive":
                            {
                                if (this.EntityPM.Inactive == true) {
                                    button.IsDisabled = true;
                                }
                                else if (this.EntityPM.AccountTypeCode == '4' || this.EntityPM.AccountTypeCode == '5') { // Job / File
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "GLAccountPrintCardIndex":
                            {
                                //                                if (this.EntityPM.Inactive == true) {
                                //                                    button.IsDisabled = true;
                                //                               }
                                //                                else {
                                button.IsDisabled = false;
                                //                                }
                                break;
                            }
                        case "Reconcile":
                            {
                                button.DisplayText = TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccount.B.Reconcile") + " (" + this.EntityPM.ReconcilationCount + ")";
                                if (this.EntityPM.IsControlAccount == true || this.EntityPM.ReconcilationCount == 0) {
                                    button.IsDisabled = true;
                                }
                                break;
                            }
                    }
                }
            }
        }
        return menuButtons;
    };
    GLAccountMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        var _this = this;
        var errors = [];
        if (errors.length == 0) {
            switch (menuButton.EventCode) {
                case "GLAccountInactive":
                    {
                        var myGLAccountListService = new GLAccountListService_1.GLAccountListService();
                        myGLAccountListService.getSingle(this.EntityPM.Id)
                            .subscribe(function (myResponse) {
                            var myGLAccountList = myResponse.Result;
                            if (!myGLAccountList.BalanceInLocalCurrency || myGLAccountList.BalanceInLocalCurrency == 0) {
                                _this.EntityPM.Inactive = true;
                                _this.EntityPM.ActiveStatusName = TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.Q.Inactive");
                                _this.entityArgs.EditComponent.SaveChanges();
                            }
                            else {
                                _this.entityArgs.EditComponent.ValidationErrorsList = [];
                                _this.entityArgs.EditComponent.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.GLABalanceNotEqual0"));
                            }
                        });
                        break;
                    }
                case "GLAccountPrintCardIndex":
                    {
                        this.entityArgs.EditComponent.SaveChanges();
                        // Here to start the report filter screen
                        break;
                    }
                case "Reconcile":
                    {
                        this.ReconcileButtonClicked();
                        break;
                    }
            }
        }
        else {
            this.entityArgs.EditComponent.ValidationErrorsList = [];
            this.entityArgs.EditComponent.ValidationErrorsList = errors;
        }
    };
    GLAccountMenuButtonsHandler.prototype.StartBusyIndicator = function (message) {
        this.CurrentSession.StartBusyIndicator(message);
    };
    GLAccountMenuButtonsHandler.prototype.StopBusyIndicator = function () {
        this.CurrentSession.StopBusyIndicator();
    };
    GLAccountMenuButtonsHandler.prototype.ReconcileButtonClicked = function () {
        var _this = this;
        var screenWidth = this.getScreenWidth();
        var screenHeight = this.getScreenHeight();
        this._LedgerTransactionExtendedListService.GetFirstLedgerTransaction(this.EntityPM.Id).subscribe(function (serviceResponse) {
            if (serviceResponse.Result) {
                var result = serviceResponse.Result;
                var transaction = result.Result; // get the data
                var openAmountCurrency = transaction.OpenAmountCurrencySign;
                // original amount currency
                var originalAmountCurrency;
                if (_this.EntityPM.ReconcileMethodCode == "0")
                    originalAmountCurrency = SessionLocator_1.SessionLocator.TenantPM.CurrencySign;
                else if (_this.EntityPM.ReconcileMethodCode == "1")
                    originalAmountCurrency = transaction.CurrencySign;
                var windowArgs = {};
                windowArgs.GLAccountPM = _this.EntityPM;
                windowArgs.openAmountCurrency = openAmountCurrency;
                windowArgs.originalAmountCurrency = originalAmountCurrency;
                var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                logitudeWindow.Width = (screenWidth > 1024) ? (screenWidth > 1200 ? 1500 : screenWidth - 20) : 900;
                logitudeWindow.Height = (screenHeight > 768) ? (screenHeight > 800 ? 700 : screenHeight - 70) : screenHeight - 70;
                logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Reconcile"); //"Reconcile";
                logitudeWindow.WindowArgs = windowArgs;
                logitudeWindow.Show('./Accounting/Components/Others/ReconcileComponent');
                logitudeWindow.WindowClosed.subscribe(function ($event) {
                    if ($event == 'ok') {
                        // show alert
                    }
                    _this.GetNonReconciledTransactionsCount();
                });
            }
        });
    };
    GLAccountMenuButtonsHandler.prototype.getScreenHeight = function () {
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
    GLAccountMenuButtonsHandler.prototype.getScreenWidth = function () {
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
    GLAccountMenuButtonsHandler.prototype.GetNonReconciledTransactionsCount = function () {
        var _this = this;
        this.glAccountExtendedListService.GetAccountReconcilesCount(this.EntityPM.Id).subscribe(function (myResult) {
            if (!Tools_1.AppTool.IsNullOrEmpty(myResult)) {
                _this.EntityPM.ReconcilationCount = myResult;
                _this.SaveChenges();
            }
        });
    };
    GLAccountMenuButtonsHandler.prototype.SaveChenges = function () {
        var _this = this;
        // the validation will be in PM Service (custom validator)
        this.entityArgs.EditComponent.SaveChanges();
        this.entityArgs.EditComponent.SaveCompleted.subscribe(function ($event) {
            if ($event == true) {
                _this.entityArgs.EditComponent.ReloadEntityPM();
            }
        });
    };
    return GLAccountMenuButtonsHandler;
}());
exports.GLAccountMenuButtonsHandler = GLAccountMenuButtonsHandler;
//# sourceMappingURL=GLAccountMenuButtonsHandler.js.map