"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var PasswordChangeService_1 = require("../../Services/Others/PasswordChangeService");
var UserExtendedPMService_1 = require("../../Services/ExtendedPMs/UserExtendedPMService");
var UserMenuButtonsHandler = /** @class */ (function () {
    function UserMenuButtonsHandler() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isButtonClicked = false;
    }
    UserMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    };
    UserMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
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
    UserMenuButtonsHandler.prototype.StopFlags = function () {
        this.isAnonymizeUser = false;
        this.isButtonClicked = false;
    };
    UserMenuButtonsHandler.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    if (_this.isAnonymizeUser) {
                        _this.UserAnonymize();
                    }
                }
                _this.StopFlags();
            });
        }
    };
    UserMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        if (!this.isButtonClicked) {
            this.StopFlags();
            this.isButtonClicked = true;
            switch (menuButton.EventCode) {
                case "ResetUserPassword":
                    {
                        this.ResetUserPassword();
                        this.isButtonClicked = false;
                        break;
                    }
                case "Anonymize":
                    {
                        this.isAnonymizeUser = true;
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
    UserMenuButtonsHandler.prototype.ResetUserPassword = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("User.O.ResetUserPassword");
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("User.M.YouWantToResetPassword"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.ResetPassword();
            }
        });
    };
    UserMenuButtonsHandler.prototype.ResetPassword = function () {
        var myService = new PasswordChangeService_1.PasswordChangeService();
        myService.ResetUserPassword(this.EntityPM.Id, this.EntityPM.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("User.M.UserPasswordResetCompletedSuccessfully") + ": " + result + ".");
                }
            }
        });
    };
    UserMenuButtonsHandler.prototype.UserAnonymize = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("User.M.Anonymize"));
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
    return UserMenuButtonsHandler;
}());
exports.UserMenuButtonsHandler = UserMenuButtonsHandler;
//# sourceMappingURL=UserMenuButtonsHandler.js.map