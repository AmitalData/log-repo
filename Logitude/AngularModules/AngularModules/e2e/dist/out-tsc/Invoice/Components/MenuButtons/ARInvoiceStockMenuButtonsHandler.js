"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ARInvoiceStockMenuButtonsHandler = /** @class */ (function () {
    function ARInvoiceStockMenuButtonsHandler() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    ARInvoiceStockMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    };
    ARInvoiceStockMenuButtonsHandler.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    if (_this.isCancelled) {
                        _this.isCancelled = false;
                        _this.SetIsCancelled();
                    }
                    if (_this.isReActivated) {
                        _this.isReActivated = false;
                        _this.SetReActivated();
                    }
                    if (_this.actionCompleted) {
                        _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        //this.CurrentSession.FireEvent("RefreshARInvoiceStockScreen");
                    }
                }
            });
        }
    };
    ARInvoiceStockMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'ARInvoiceStock'; })[0];
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "Cancel":
                            {
                                if (this.EntityPM.StatusCode == "C") {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "ReActivate":
                            {
                                if (this.EntityPM.StatusCode == "C") {
                                    button.IsDisabled = false;
                                }
                                else {
                                    button.IsDisabled = true;
                                }
                                break;
                            }
                    }
                }
            }
        }
    };
    ARInvoiceStockMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        switch (menuButton.EventCode) {
            case "Cancel": {
                this.ResetAllFlags();
                this.isCancelled = true;
                this.entityArgs.EditComponent.SaveChanges();
                break;
            }
            case "ReActivate":
                {
                    this.ResetAllFlags();
                    this.isReActivated = true;
                    this.entityArgs.EditComponent.SaveChanges();
                    break;
                }
        }
    };
    ARInvoiceStockMenuButtonsHandler.prototype.ResetAllFlags = function () {
        this.isCancelled = false;
        this.isReActivated = false;
        this.actionCompleted = false;
    };
    ARInvoiceStockMenuButtonsHandler.prototype.SetIsCancelled = function () {
        this.EntityPM.StatusCode = "C";
        this.EntityPM.Inactive = true;
        this.EntityPM.Cancelled = true;
        this.actionCompleted = true;
        this.entityArgs.EditComponent.SaveChanges();
    };
    ARInvoiceStockMenuButtonsHandler.prototype.SetReActivated = function () {
        this.EntityPM.StatusCode = "A";
        this.EntityPM.Inactive = false;
        this.EntityPM.Reactivated = true;
        this.actionCompleted = true;
        this.entityArgs.EditComponent.SaveChanges();
    };
    return ARInvoiceStockMenuButtonsHandler;
}());
exports.ARInvoiceStockMenuButtonsHandler = ARInvoiceStockMenuButtonsHandler;
//# sourceMappingURL=ARInvoiceStockMenuButtonsHandler.js.map