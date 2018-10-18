"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var PasswordChangeService_1 = require("../../Services/Others/PasswordChangeService");
var UserMenuButtonsHandler = (function () {
    function UserMenuButtonsHandler() {
    }
    UserMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    };
    UserMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                menuButtons.forEach(function (menuButton) {
                    switch (menuButton.EventCode) {
                        case "ResetUserPassword": {
                            //if (this.EntityPM.IsCancelled) {
                            //    menuButton.IsDisabled = true;
                            //}
                            //else {
                            //    menuButton.IsDisabled = false;
                            //}
                            break;
                        }
                    }
                });
            }
        }
        return menuButtons;
    };
    UserMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                switch (menuButton.EventCode) {
                    case "ResetUserPassword": {
                        this.ResetUserPassword();
                        break;
                    }
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
    return UserMenuButtonsHandler;
}());
exports.UserMenuButtonsHandler = UserMenuButtonsHandler;
//# sourceMappingURL=UserMenuButtonsHandler.js.map