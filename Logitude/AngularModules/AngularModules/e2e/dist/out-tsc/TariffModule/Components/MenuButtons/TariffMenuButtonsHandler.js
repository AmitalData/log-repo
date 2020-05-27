"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TariffMenuButtonsHandler = /** @class */ (function () {
    function TariffMenuButtonsHandler() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isButtonClicked = false;
    }
    TariffMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    };
    TariffMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        var _this = this;
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                menuButtons.forEach(function (menuButton) {
                    menuButton.IsDisabled = false;
                    switch (menuButton.EventCode) {
                        case "Inactive": {
                            if (_this.EntityPM.InActive)
                                menuButton.IsDisabled = true;
                            break;
                        }
                        case "Reactivate": {
                            if (!_this.EntityPM.InActive) {
                                menuButton.IsDisabled = true;
                            }
                            break;
                        }
                    }
                });
            }
        }
        return menuButtons;
    };
    TariffMenuButtonsHandler.prototype.StopFlags = function () {
        this.isButtonClicked = false;
    };
    TariffMenuButtonsHandler.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                }
                _this.StopFlags();
            });
        }
    };
    TariffMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        if (!this.isButtonClicked) {
            this.StopFlags();
            this.isButtonClicked = true;
            switch (menuButton.EventCode) {
                case "Inactive":
                    {
                        this.EntityPM.SetAsInActive = true;
                        this.EntityPM.InActive = true;
                        this.entityArgs.EditComponent.SaveChanges();
                        break;
                    }
                case "Reactivate":
                    {
                        this.EntityPM.SetAsReActive = true;
                        this.EntityPM.InActive = false;
                        this.entityArgs.EditComponent.SaveChanges();
                        break;
                    }
                default: {
                    this.isButtonClicked = false;
                    break;
                }
            }
        }
    };
    return TariffMenuButtonsHandler;
}());
exports.TariffMenuButtonsHandler = TariffMenuButtonsHandler;
//# sourceMappingURL=TariffMenuButtonsHandler.js.map