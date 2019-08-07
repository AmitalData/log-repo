"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var SettingsComponent = /** @class */ (function () {
    function SettingsComponent(entityResourceService) {
        this.entityResourceService = entityResourceService;
        this.IsResourcesReady = true;
        this.IsVisible_SATInterface = false;
        this.HasTransferFeature = false;
        this.AccountingSystemName = null;
        this.SATSettingsName = null;
        this.IsVisible_AccountingSetting = false;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.IsVisible_SATInterface = FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "SATINTERFACE") ? true : false;
        this.IsVisible_AccountingSetting = FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "HOWTOACCOUNTINGSETTINGS") ? true : false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "ACCOUNTINGTRANSFER")) {
            this.HasTransferFeature = true;
        }
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings) {
            this.SATSettingsName = SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceName;
        }
        if (SessionLocator_1.SessionLocator.AccountingSystemPM) {
            this.AccountingSystemName = SessionLocator_1.SessionLocator.AccountingSystemPM.Name;
        }
    }
    SettingsComponent.prototype.onSATSettingsClicked = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("SATInterfaceSetting", 0).subscribe(function (response) {
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Width = 800;
            logitudeWindow.Height = 550;
            logitudeWindow.Title = "SAT Interface Settings";
            logitudeWindow.Show('./Invoice/Components/Workspaces/SATInterfaceSettingsComponent');
            logitudeWindow.WindowClosed.subscribe(function (e) {
                if (SessionLocator_1.SessionLocator.SATInterfaceSettings) {
                    _this.SATSettingsName = SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceName;
                }
            });
        });
    };
    SettingsComponent.prototype.OpenAccountingSystem = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Accounting Transfer Settings";
        logWindow.Show('./Invoice/Components/Workspaces/Windows/TransferSettingsComponent');
        logWindow.WindowClosed.subscribe(function (e) {
            _this.AccountingSystemName = SessionLocator_1.SessionLocator.AccountingSystemPM.Name;
        });
    };
    SettingsComponent.prototype.OpenAccountingSettings = function () {
        this._entityResourceService.getEntityResourceByTableName("AccountingSetting", 0).subscribe(function (response) {
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Width = 800;
            logitudeWindow.Height = 550;
            logitudeWindow.Title = "Accounting Settings";
            logitudeWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/AccountingSettings/AccountingSettingsComponent');
        });
    };
    SettingsComponent = __decorate([
        core_1.Component({
            selector: 'SettingsComponent',
            moduleId: module.id,
            templateUrl: './SettingsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], SettingsComponent);
    return SettingsComponent;
}());
exports.SettingsComponent = SettingsComponent;
//# sourceMappingURL=SettingsComponent.js.map