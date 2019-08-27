"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CommunicationLogExtendedPMService_1 = require("../../../Common/Services/ExtendedPMs/CommunicationLogExtendedPMService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var CommunicationLogMenuButtonsHandler = /** @class */ (function () {
    function CommunicationLogMenuButtonsHandler() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    CommunicationLogMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.communicationLogExtendedPMService = new CommunicationLogExtendedPMService_1.CommunicationLogExtendedPMService();
    };
    CommunicationLogMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        var _this = this;
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                menuButtons.forEach(function (menuButton) {
                    switch (menuButton.EventCode) {
                        case "Resend": {
                            if (_this.EntityPM.InOut == "I") {
                                menuButton.IsDisabled = true;
                            }
                            else if (_this.EntityPM.CommunicationStatusTypeCode == "D" && _this.EntityPM.To == "QBO")
                                menuButton.IsDisabled = true;
                            else if (_this.EntityPM.To == "Profact")
                                menuButton.IsDisabled = true;
                            break;
                        }
                        case "ViewMessage": {
                            menuButton.IsHidden = true;
                            break;
                        }
                        case "Actions": {
                            menuButton.IsHidden = true;
                            break;
                        }
                    }
                });
            }
        }
        return menuButtons;
    };
    CommunicationLogMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                switch (menuButton.EventCode) {
                    case "Resend": {
                        this.ResendButtonClcik();
                        break;
                    }
                    case "ViewMessage": {
                        if (this.EntityPM.CommunicationLogTypeCode == "E") {
                            this.ViewMessage();
                        }
                        break;
                    }
                }
            }
        }
    };
    CommunicationLogMenuButtonsHandler.prototype.ResendButtonClcik = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Resending...");
        this.communicationLogExtendedPMService.SendCommunicationLogToQueue(this.EntityPM.Id, this.EntityPM.Tenant).subscribe(function (res) {
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    CommunicationLogMenuButtonsHandler.prototype.ViewMessage = function () {
    };
    return CommunicationLogMenuButtonsHandler;
}());
exports.CommunicationLogMenuButtonsHandler = CommunicationLogMenuButtonsHandler;
//# sourceMappingURL=CommunicationLogMenuButtonsHandler.js.map