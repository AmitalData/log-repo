"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var UserExtendedPMService_1 = require("../../Services/ExtendedPMs/UserExtendedPMService");
var ContactMenuButtonsHandler = /** @class */ (function () {
    function ContactMenuButtonsHandler() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isButtonClicked = false;
    }
    ContactMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    };
    ContactMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                menuButtons.forEach(function (menuButton) {
                    switch (menuButton.EventCode) {
                        case "Anonymize": {
                            break;
                        }
                    }
                });
            }
        }
        return menuButtons;
    };
    ContactMenuButtonsHandler.prototype.StopFlags = function () {
        this.isAnonymizeContact = false;
        this.isButtonClicked = false;
    };
    ContactMenuButtonsHandler.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    if (_this.isAnonymizeContact) {
                        _this.ContactAnonymize();
                    }
                }
                _this.StopFlags();
            });
        }
    };
    ContactMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        if (!this.isButtonClicked) {
            this.StopFlags();
            this.isButtonClicked = true;
            switch (menuButton.EventCode) {
                case "Anonymize":
                    {
                        this.isAnonymizeContact = true;
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
    ContactMenuButtonsHandler.prototype.ContactAnonymize = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Contact.M.Anonymize"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                var service = new UserExtendedPMService_1.UserExtendedPMService();
                service.Anonymization(_this.EntityPM.Id).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }
                });
            }
        });
    };
    return ContactMenuButtonsHandler;
}());
exports.ContactMenuButtonsHandler = ContactMenuButtonsHandler;
//# sourceMappingURL=ContactMenuButtonsHandler.js.map