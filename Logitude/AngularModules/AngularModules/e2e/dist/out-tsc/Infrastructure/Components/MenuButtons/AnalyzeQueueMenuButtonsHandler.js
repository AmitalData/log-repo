"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var InfrastructureDomainService_1 = require("../../Services/InfrastructureDomainService");
var AnalyzeQueueMenuButtonsHandler = /** @class */ (function () {
    function AnalyzeQueueMenuButtonsHandler() {
        this.ObjectTableName = "AnalyzeQueue";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isButtonClicked = false;
    }
    AnalyzeQueueMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.iService = new InfrastructureDomainService_1.InfrastructureDomainService();
        //this.Listen();
    };
    AnalyzeQueueMenuButtonsHandler.prototype.StopFlags = function () {
        this.isButtonClicked = false;
    };
    AnalyzeQueueMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM) {
            if (this.entityArgs.EditComponent) {
                menuButtons.forEach(function (button) {
                    switch (button.EventCode) {
                        case "Resend":
                            {
                                break;
                            }
                    }
                });
            }
        }
    };
    AnalyzeQueueMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        if (this.EntityPM) {
            if (this.entityArgs.EditComponent) {
                if (menuButton) {
                    if (!this.isButtonClicked) {
                        this.StopFlags();
                        this.isButtonClicked = true;
                        switch (menuButton.EventCode) {
                            case "Resend":
                                {
                                    this.ResendButtonClicked();
                                    break;
                                }
                        }
                    }
                }
            }
        }
    };
    AnalyzeQueueMenuButtonsHandler.prototype.ResendButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Resending...");
        this.iService.ResendAnalyzeQueue(this.EntityPM.Id).subscribe(function (myResponse) {
            _this.StopFlags();
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    return AnalyzeQueueMenuButtonsHandler;
}());
exports.AnalyzeQueueMenuButtonsHandler = AnalyzeQueueMenuButtonsHandler;
//# sourceMappingURL=AnalyzeQueueMenuButtonsHandler.js.map