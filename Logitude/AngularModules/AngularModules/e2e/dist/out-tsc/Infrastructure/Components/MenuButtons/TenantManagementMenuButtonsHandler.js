"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var TenantManagementMenuButtonsHandler = /** @class */ (function () {
    function TenantManagementMenuButtonsHandler() {
        this.ObjectTableName = "TenantManagement";
        this.isEraseData = false;
    }
    TenantManagementMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    };
    TenantManagementMenuButtonsHandler.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    if (_this.isEraseData) {
                        _this.isEraseData = false;
                        _this.OpenEraseDataComponent();
                    }
                }
            });
        }
    };
    TenantManagementMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'Customer'; })[0];
                var buttonEnabled = true;
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "EraseData":
                            {
                                if (this.EntityPM.IsTrial) {
                                    button.IsDisabled = false;
                                }
                                else {
                                    button.IsDisabled = true;
                                }
                                break;
                            }
                    }
                }
            }
        }
        return menuButtons;
    };
    TenantManagementMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        var _this = this;
        switch (menuButton.EventCode) {
            case "EraseData":
                {
                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    confirmWindow.Height = 200;
                    confirmWindow.Width = 350;
                    confirmWindow.ShowCheckBox = true;
                    confirmWindow.IsYesEnabled = false;
                    confirmWindow.Show("Please notice that records can't be restored after deleting, the deletion will result in losing this data forever");
                    confirmWindow.WindowClosed.subscribe(function (event) {
                        if (confirmWindow.Yes && confirmWindow.IsChecked) {
                            _this.isEraseData = true;
                            _this.entityArgs.EditComponent.SaveChanges();
                        }
                    });
                    break;
                }
        }
    };
    TenantManagementMenuButtonsHandler.prototype.OpenEraseDataComponent = function () {
        if (this.EntityPM != null) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.WindowArgs = this.EntityPM.Id;
            logWindow.Title = "Erase Data";
            logWindow.Show('./Infrastructure/Components/MenuButtons/EraseTenantManagementDataComponent');
        }
    };
    return TenantManagementMenuButtonsHandler;
}());
exports.TenantManagementMenuButtonsHandler = TenantManagementMenuButtonsHandler;
//# sourceMappingURL=TenantManagementMenuButtonsHandler.js.map