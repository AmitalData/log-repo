"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var CustomerTenantAccessExtendedPMService_1 = require("../../Services/ExtendedPMs/CustomerTenantAccessExtendedPMService");
var CustomerTenantAccessMenuButtonsHandler = /** @class */ (function () {
    function CustomerTenantAccessMenuButtonsHandler() {
        this.ObjectTableName = "CustomerTenantAccess";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    CustomerTenantAccessMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    };
    CustomerTenantAccessMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                ;
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'CustomerTenantAccess'; })[0];
                var buttonEnabled = true;
                var eventsTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "UPDATE") && f.ObjectTableId == table.Id; })[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "Deny":
                            {
                                button.IsDisabled = this.EntityPM.Status.toUpperCase() != "W";
                                break;
                            }
                    }
                }
            }
        }
        return menuButtons;
    };
    CustomerTenantAccessMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        switch (menuButton.EventCode) {
            case "Deny":
                {
                    this.Deny();
                    break;
                }
        }
    };
    CustomerTenantAccessMenuButtonsHandler.prototype.Deny = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Title = "Deny Request";
        confirmWindow.Yes = true;
        confirmWindow.No = true;
        confirmWindow.Cancel = true;
        confirmWindow.Show("Are you sure you want to Deny this Request ?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                var service = new CustomerTenantAccessExtendedPMService_1.CustomerTenantAccessExtendedPMService();
                service.DenyRequest(_this.EntityPM.Id).subscribe(function (p) {
                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                });
                ;
            }
        });
    };
    return CustomerTenantAccessMenuButtonsHandler;
}());
exports.CustomerTenantAccessMenuButtonsHandler = CustomerTenantAccessMenuButtonsHandler;
//# sourceMappingURL=CustomerTenantAccessMenuButtonsHandler.js.map