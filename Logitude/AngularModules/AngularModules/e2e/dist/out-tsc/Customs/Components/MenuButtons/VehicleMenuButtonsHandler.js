"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var DeclarationPMService_1 = require("../../Services/StandardPMs/DeclarationPMService");
var MenuButtonsEvents_1 = require("../../../Infrastructure/Utilities/events/MenuButtonsEvents");
var VehicleMenuButtonsHandler = /** @class */ (function () {
    function VehicleMenuButtonsHandler() {
        //-----------------properties---------------------------//
        this.isValid = false;
        this.isButtonClicked = false;
        this.MenuButtonCode = null;
        this.checkTransfer = ""; // moran 4.8.16 - AMI-56804
        //------------------------------------------------------//
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //Services
        this.declarationPMService = new DeclarationPMService_1.DeclarationPMService();
    }
    VehicleMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
        this.IdentityKey = Tools_1.AppTool.GetNewGuid();
    };
    VehicleMenuButtonsHandler.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    switch (_this.MenuButtonCode) {
                    }
                }
            });
            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });
        }
        this.CurrentSession.SubscriptionAdd(this.MenuButtonsStateChangedEvent = MenuButtonsEvents_1.MenuButtonsEvents.MenuButtonsStateChanged.subscribe(function (args) {
            if (!_this.IsDisplayOnly) {
                if (!Tools_1.AppTool.IsNullOrEmpty(_this.CurrentSession.CurrentEditComponent.EditComponentController)) {
                    _this.IsDisplayOnly = _this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
                }
                _this.CheckButtonState(_this.MenuButtons);
            }
        }));
    };
    VehicleMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        this.MenuButtons = menuButtons;
        this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
        if (this.EntityPM != null) {
            if (this.CurrentSession.CurrentEditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'Customs.Vehicle'; })[0];
                var buttonEnabled = true;
                var eventsTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "UPDATE") && f.ObjectTableId == table.Id; })[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    if (button.EventCode == "SendVehicle") {
                        button.IsDisabled = false;
                        button.IsHidden = false;
                        if (this.IsDisplayOnly) {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "DeleteVehicle") {
                        button.IsDisabled = false;
                        button.IsHidden = false;
                        if (this.IsDisplayOnly) {
                            button.IsDisabled = true;
                        }
                    }
                }
                return menuButtons;
            }
        }
    };
    VehicleMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        if (true) { //!this.isButtonClicked) { this is temporary for testing.
            this.isButtonClicked = true;
            this.MenuButtonCode = menuButton.EventCode;
            if (true) { //this.isValid) { this is also for testing temp of course
                switch (this.MenuButtonCode) {
                    case "SendVehicle":
                        {
                            ////SendDeclaration();
                            // SendDeclaration(declarationViewModel);
                            break;
                        }
                    case "DeleteVehicle":
                        {
                            ////SendDeclaration();
                            // SendDeclaration(declarationViewModel);
                            break;
                        }
                }
            }
        }
    };
    return VehicleMenuButtonsHandler;
}());
exports.VehicleMenuButtonsHandler = VehicleMenuButtonsHandler;
//# sourceMappingURL=VehicleMenuButtonsHandler.js.map