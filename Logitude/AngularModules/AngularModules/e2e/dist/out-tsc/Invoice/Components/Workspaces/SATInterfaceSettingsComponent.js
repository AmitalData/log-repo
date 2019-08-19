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
var SATInterfaceSettingPMService_1 = require("../../Services/StandardPMs/SATInterfaceSettingPMService");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var CommonDomainService_1 = require("../../../Common/Services/CommonDomainService");
var Tools_1 = require("../../../Infrastructure/Tools");
var SATInterfaceSettingsComponent = /** @class */ (function () {
    function SATInterfaceSettingsComponent(entityResourceService) {
        this.entityResourceService = entityResourceService;
        this.IsResourcesReady = true;
        this.ObjectTableName = "SATInterfaceSetting";
        //public SATFolderName = "FromLogitude\SAT";
        this.IsDropboxConnected = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.sATInterfaceSettingPMService = new SATInterfaceSettingPMService_1.SATInterfaceSettingPMService();
        //entityResourceService.getEntityResourceByTableName("SATInterfaceSetting").subscribe(res1 => {
        this.LoadData();
        //});
    }
    SATInterfaceSettingsComponent.prototype.LoadData = function () {
        var _this = this;
        this.sATInterfaceSettingPMService.get(SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
            if (!response.HasError) {
                _this.EntityPM = response.Result;
            }
            _this.IsResourcesReady = true;
        });
    };
    SATInterfaceSettingsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SATInterfaceSettingsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.SATInterfaceCode)) {
            this.ValidationErrorsList.push("SAT Interface Code field is required!");
        }
        if ((this.EntityPM.SATInterfaceCode === "PROF" || this.EntityPM.SATInterfaceCode == "PROF33") && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Token)) {
            this.ValidationErrorsList.push("Token field is required!");
        }
        if (this.ValidationErrorsList.length > 0)
            return;
        if (this.EntityPM.SATInterfaceCode === "CONT") {
            this.CheckDropBoxAndSave();
        }
        else {
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
            this.sATInterfaceSettingPMService.update(this.EntityPM).subscribe(function (response) {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (response.HasError) {
                    _this.ValidationErrorsList = response.ErrorsArray;
                }
                else {
                    _this.sATInterfaceSettingPMService.get(SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
                        if (!response.HasError) {
                            SessionLocator_1.SessionLocator.SATInterfaceSettings = response.Result;
                        }
                        _this.CurrentSession.CloseCurrentWindow();
                    });
                }
            });
        }
    };
    SATInterfaceSettingsComponent.prototype.ViewDropboxConnection = function () {
        var windowTitle = "Dropbox Connection";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 350;
        logWindow.Height = 225;
        logWindow.Title = windowTitle;
        logWindow.IsShowCloseButton = false;
        logWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/DropBox/DropBoxConnectionComponent');
    };
    SATInterfaceSettingsComponent.prototype.CheckDropBoxAndSave = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
        var myService = new CommonDomainService_1.CommonDomainService();
        myService.GetDropBoxAccessTocken(SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResult) {
            if (myResult.HasError == false && !Tools_1.AppTool.IsNullOrEmpty(myResult.Result.DropBoxAccessToken)) {
                _this.IsDropboxConnected = true;
            }
            if (_this.IsDropboxConnected) {
                _this.sATInterfaceSettingPMService.update(_this.EntityPM).subscribe(function (response) {
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    if (response.HasError) {
                        _this.ValidationErrorsList = response.ErrorsArray;
                    }
                    else {
                        _this.CurrentSession.CloseCurrentWindow();
                    }
                });
            }
            else {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                _this.ValidationErrorsList.push("Dropbox is not connected!");
            }
        });
    };
    SATInterfaceSettingsComponent = __decorate([
        core_1.Component({
            selector: 'SATInterfaceSettingsComponent',
            moduleId: module.id,
            templateUrl: './SATInterfaceSettingsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], SATInterfaceSettingsComponent);
    return SATInterfaceSettingsComponent;
}());
exports.SATInterfaceSettingsComponent = SATInterfaceSettingsComponent;
//# sourceMappingURL=SATInterfaceSettingsComponent.js.map