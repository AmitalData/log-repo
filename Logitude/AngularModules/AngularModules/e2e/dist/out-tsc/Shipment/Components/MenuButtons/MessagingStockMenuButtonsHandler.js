"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var MessagingStockMenuButtonsHandler = /** @class */ (function () {
    function MessagingStockMenuButtonsHandler() {
    }
    MessagingStockMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    };
    MessagingStockMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        var _this = this;
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                menuButtons.forEach(function (menuButton) {
                    switch (menuButton.EventCode) {
                        case "Cancel": {
                            if (_this.EntityPM.IsCancelled) {
                                menuButton.IsDisabled = true;
                            }
                            else {
                                menuButton.IsDisabled = false;
                            }
                            break;
                        }
                    }
                });
            }
        }
        return menuButtons;
    };
    MessagingStockMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                switch (menuButton.EventCode) {
                    case "Cancel": {
                        this.Cancel();
                        break;
                    }
                }
            }
        }
    };
    MessagingStockMenuButtonsHandler.prototype.Cancel = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Show('Are you sure you want to cancel this Stock ?');
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.EntityPM.IsCancelled = true;
                _this.entityArgs.EditComponent.SaveChanges();
            }
        });
    };
    return MessagingStockMenuButtonsHandler;
}());
exports.MessagingStockMenuButtonsHandler = MessagingStockMenuButtonsHandler;
//# sourceMappingURL=MessagingStockMenuButtonsHandler.js.map