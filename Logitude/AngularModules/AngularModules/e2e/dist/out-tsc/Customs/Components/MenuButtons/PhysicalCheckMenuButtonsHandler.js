"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var PhysicalCheckWebService_1 = require("../../../Customs/Services/WebServices/PhysicalCheckWebService");
var PhysicalCheckMenuButtonsHandler = /** @class */ (function () {
    function PhysicalCheckMenuButtonsHandler() {
        this.isValid = false;
        this.isButtonClicked = false;
        this.MenuButtonCode = null;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this._PhysicalCheckWebService = new PhysicalCheckWebService_1.PhysicalCheckWebService;
    }
    PhysicalCheckMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    };
    PhysicalCheckMenuButtonsHandler.prototype.Listen = function () {
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
    PhysicalCheckMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
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
                    if (button.EventCode == "Actions") {
                        button.Width = 70;
                    }
                    if (button.EventCode == "ClosePhysicalCheck") {
                        if (this.EntityPM.IsClosed) {
                            button.IsDisabled = true;
                        }
                        else {
                            button.IsDisabled = false;
                        }
                        button.IsHidden = false;
                    }
                }
                return menuButtons;
            }
        }
    };
    PhysicalCheckMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        if (true) { //if (!this.isButtonClicked) {
            this.isButtonClicked = true;
            this.MenuButtonCode = menuButton.EventCode;
            switch (this.MenuButtonCode) {
                case "ClosePhysicalCheck":
                    {
                        this.ClosePhysicalCheckMethod();
                        break;
                    }
            }
        }
    };
    PhysicalCheckMenuButtonsHandler.prototype.ClosePhysicalCheckMethod = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Width = 300;
        confirmWindow.Show("האם ברצונך לסגור את הבדיקה ?"); //TextCodeTranslator.Translate("Customs.PhysicalCheck.O.IsClosePhysicalCheck"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this._PhysicalCheckWebService.PostClosePhysicalCheck(_this.EntityPM.Id, _this.EntityPM.Tenant)
                    .subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        var messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Width = 300;
                        messageWindow.Height = 180;
                        messageWindow.Show("הבדיקה נסגרה בהצלחה"); //TextCodeTranslator.Translate("Customs.PhysicalCheck.O.ClosePhysicalCheck"));
                    }
                });
            }
        });
    };
    return PhysicalCheckMenuButtonsHandler;
}());
exports.PhysicalCheckMenuButtonsHandler = PhysicalCheckMenuButtonsHandler;
//# sourceMappingURL=PhysicalCheckMenuButtonsHandler.js.map