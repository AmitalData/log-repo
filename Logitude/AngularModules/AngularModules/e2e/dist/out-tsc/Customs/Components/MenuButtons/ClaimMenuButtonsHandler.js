"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ClaimMenuButtonsHandler = /** @class */ (function () {
    function ClaimMenuButtonsHandler() {
        this.isValid = false;
        this.isButtonClicked = false;
        this.MenuButtonCode = null;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    ClaimMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    };
    ClaimMenuButtonsHandler.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });
            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });
        }
    };
    ClaimMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.CurrentSession.CurrentEditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'Shipment'; })[0];
                var buttonEnabled = true;
                var eventsTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "UPDATE") && f.ObjectTableId == table.Id; })[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    if (button.EventCode == "SendClaim") {
                        button.IsDisabled = false;
                        button.IsHidden = false;
                    }
                    if (button.EventCode == "CloseClaim") {
                        if (this.EntityPM.IsClosed) {
                            button.IsDisabled = true;
                        }
                        else {
                            button.IsDisabled = false;
                        }
                        button.IsHidden = false;
                    }
                    if (button.EventCode == "CancelCloseClaim") {
                        if (this.EntityPM.IsClosed) {
                            button.IsDisabled = false;
                        }
                        else {
                            button.IsDisabled = true;
                        }
                        button.IsHidden = false;
                    }
                }
                return menuButtons;
            }
        }
    };
    ClaimMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        if (true) { //if (!this.isButtonClicked) {
            this.isButtonClicked = true;
            this.MenuButtonCode = menuButton.EventCode;
            if (true) { //if (this.isValid) {
                switch (this.MenuButtonCode) {
                    case "CloseClaim":
                        {
                            this.CloseClaimMethod();
                            break;
                        }
                    case "CancelCloseClaim":
                        {
                            this.CancelCloseClaimMethod();
                            break;
                        }
                }
            }
        }
    };
    ClaimMenuButtonsHandler.prototype.CloseClaimMethod = function () {
        var _this = this;
        var isCloseClaimMethod = false;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Claim.O.IsCloseClaim"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.EntityPM.IsClosed = true;
                isCloseClaimMethod = true;
                _this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSave) {
                    if (isCloseClaimMethod) {
                        _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        var messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Width = 300;
                        messageWindow.Height = 180;
                        messageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Claim.O.CloseClaim"));
                        isCloseClaimMethod = false;
                    }
                });
                _this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
        });
    };
    ClaimMenuButtonsHandler.prototype.CancelCloseClaimMethod = function () {
        var _this = this;
        var isCancelCloseClaimMethod = false;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Claim.O.IsCancelCloseClaim"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.EntityPM.IsClosed = false;
                isCancelCloseClaimMethod = true;
                _this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSave) {
                    if (isCancelCloseClaimMethod) {
                        _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        var messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Width = 300;
                        messageWindow.Height = 180;
                        messageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Claim.O.CancelCloseClaim"));
                        isCancelCloseClaimMethod = false;
                    }
                });
                _this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
        });
    };
    return ClaimMenuButtonsHandler;
}());
exports.ClaimMenuButtonsHandler = ClaimMenuButtonsHandler;
//# sourceMappingURL=ClaimMenuButtonsHandler.js.map