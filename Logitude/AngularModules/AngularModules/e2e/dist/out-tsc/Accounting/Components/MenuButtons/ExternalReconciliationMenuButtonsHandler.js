"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var ExternalReconciliationMenuButtonsHandler = /** @class */ (function () {
    function ExternalReconciliationMenuButtonsHandler() {
        this.ObjectTableName = "ExternalReconciliation";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    ExternalReconciliationMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    };
    ExternalReconciliationMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'ExternalReconciliation'; })[0];
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "CancelExtReco":
                            {
                                if (this.EntityPM.IsCancelled) {
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
    ExternalReconciliationMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        var _this = this;
        switch (menuButton.EventCode) {
            case "CancelExtReco":
                {
                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.WantToCancelCurrentReconciliation");
                    confirmWindow.Show(msg);
                    confirmWindow.WindowClosed.subscribe(function (event) {
                        if (confirmWindow.Yes) {
                            _this.EntityPM.IsCancelled = true;
                            _this.entityArgs.EditComponent.SaveChanges();
                            _this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                                if (isSaveSuccess) {
                                    _this.entityArgs.EditComponent.ReloadEntityPM();
                                }
                            });
                        }
                    });
                    break;
                }
        }
    };
    ExternalReconciliationMenuButtonsHandler.prototype.StartBusyIndicator = function (message) {
        this.CurrentSession.StartBusyIndicator(message);
    };
    ExternalReconciliationMenuButtonsHandler.prototype.StopBusyIndicator = function () {
        this.CurrentSession.StopBusyIndicator();
    };
    return ExternalReconciliationMenuButtonsHandler;
}());
exports.ExternalReconciliationMenuButtonsHandler = ExternalReconciliationMenuButtonsHandler;
//# sourceMappingURL=ExternalReconciliationMenuButtonsHandler.js.map