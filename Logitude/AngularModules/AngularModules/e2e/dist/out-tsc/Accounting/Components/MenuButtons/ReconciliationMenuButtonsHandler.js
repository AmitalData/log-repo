"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var ReconciliationMenuButtonsHandler = /** @class */ (function () {
    function ReconciliationMenuButtonsHandler() {
        this.ObjectTableName = "Reconciliation";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    ReconciliationMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    };
    ReconciliationMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'Reconciliation'; })[0];
                var buttonEnabled = true;
                var eventsTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "UPDATE") && f.ObjectTableId == table.Id; })[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "CancelReco":
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
    ReconciliationMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        var _this = this;
        switch (menuButton.EventCode) {
            case "CancelReco":
                {
                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.WantToCancelCurrentReconciliation");
                    confirmWindow.Show(msg);
                    confirmWindow.WindowClosed.subscribe(function (event) {
                        if (confirmWindow.Yes) {
                            _this.EntityPM.IsCancelled = true;
                            _this.entityArgs.EditComponent.SaveChanges();
                        }
                    });
                    break;
                }
        }
    };
    ReconciliationMenuButtonsHandler.prototype.StartBusyIndicator = function (message) {
        this.CurrentSession.StartBusyIndicator(message);
    };
    ReconciliationMenuButtonsHandler.prototype.StopBusyIndicator = function () {
        this.CurrentSession.StopBusyIndicator();
    };
    return ReconciliationMenuButtonsHandler;
}());
exports.ReconciliationMenuButtonsHandler = ReconciliationMenuButtonsHandler;
//# sourceMappingURL=ReconciliationMenuButtonsHandler.js.map