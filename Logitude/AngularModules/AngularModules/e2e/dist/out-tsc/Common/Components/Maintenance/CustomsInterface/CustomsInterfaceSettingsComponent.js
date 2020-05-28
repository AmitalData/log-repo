"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var CustomsInterfaceSettingPM_1 = require("../../../EntityPMs/CustomsInterfaceSettingPM");
var CustomsInterfaceSettingPMService_1 = require("../../../Services/StandardPMs/CustomsInterfaceSettingPMService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var CustomsInterfaceSettingsComponent = /** @class */ (function (_super) {
    __extends(CustomsInterfaceSettingsComponent, _super);
    function CustomsInterfaceSettingsComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.ObjectTableName = "CustomsInterfaceSetting";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.IsResourcesReady = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsLocalDetailsEnabled = false;
        _this.IsImportDetailsEnabled = false;
        _this.IsExportDetailsEnabled = false;
        _this.IsLocalTickVisible = false;
        _this.IsImportTickVisible = false;
        _this.IsExportTickVisible = false;
        _this.myService = new CustomsInterfaceSettingPMService_1.CustomsInterfaceSettingPMService();
        return _this;
    }
    CustomsInterfaceSettingsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            _this.myService.get(SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    if (myResponse.Result) {
                        _this.EntityPM = myResponse.Result;
                    }
                    else {
                        _this.EntityPM = new CustomsInterfaceSettingPM_1.CustomsInterfaceSettingPM();
                        _this.EntityPM.IsDirty = false;
                    }
                    _this.SetUIProperties();
                    _this.SetTickProperties();
                    _this.IsResourcesReady = true;
                }
            });
        });
    };
    CustomsInterfaceSettingsComponent.prototype.SetUIProperties = function () {
        var localDetailsEnabled = false;
        var importDetailsEnabled = false;
        var exportDetailsEnabled = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.LocalCustomsInterfaceCode)) {
            if (this.EntityPM.LocalCustomsInterfaceCode != "NO") {
                localDetailsEnabled = true;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ImportToUSAInterfaceCode)) {
            if (this.EntityPM.ImportToUSAInterfaceCode != "NO") {
                importDetailsEnabled = true;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ExportFromUSAInterfaceCode)) {
            if (this.EntityPM.ExportFromUSAInterfaceCode != "NO") {
                exportDetailsEnabled = true;
            }
        }
        this.IsLocalDetailsEnabled = localDetailsEnabled;
        this.IsImportDetailsEnabled = importDetailsEnabled;
        this.IsExportDetailsEnabled = exportDetailsEnabled;
    };
    CustomsInterfaceSettingsComponent.prototype.SetTickProperties = function () {
        var localTickVisible = false;
        var importTickVisible = false;
        var exportTickVisible = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.LocalCustomsInterfaceCode)) {
            if (this.EntityPM.LocalCustomsInterfaceCode != "NO") {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.LocalCompanyId) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.LocalUserId) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.LocalPassword)) {
                    localTickVisible = true;
                }
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ImportToUSAInterfaceCode)) {
            if (this.EntityPM.ImportToUSAInterfaceCode != "NO") {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ArtemusOutSettingsId) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ArtemusInSettingsId)) {
                    importTickVisible = true;
                }
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ExportFromUSAInterfaceCode)) {
            if (this.EntityPM.ExportFromUSAInterfaceCode != "NO") {
            }
        }
        this.IsLocalTickVisible = localTickVisible;
        this.IsImportTickVisible = importTickVisible;
        this.IsExportTickVisible = exportTickVisible;
    };
    Object.defineProperty(CustomsInterfaceSettingsComponent.prototype, "LocalCustomsInterfaceCode", {
        get: function () { return this.EntityPM.LocalCustomsInterfaceCode; },
        set: function (value) {
            if (this.EntityPM.LocalCustomsInterfaceCode != value) {
                this.EntityPM.LocalCustomsInterfaceCode = value;
                if (value == "NO") {
                    this.EntityPM.LocalCompanyId = null;
                    this.EntityPM.LocalUserId = null;
                    this.EntityPM.LocalPassword = null;
                    this.ActivateCustomsManagementInShipments = false;
                }
                else {
                    this.ActivateCustomsManagementInShipments = true;
                }
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsInterfaceSettingsComponent.prototype, "ImportToUSAInterfaceCode", {
        get: function () { return this.EntityPM.ImportToUSAInterfaceCode; },
        set: function (value) {
            if (this.EntityPM.ImportToUSAInterfaceCode != value) {
                this.EntityPM.ImportToUSAInterfaceCode = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsInterfaceSettingsComponent.prototype, "ExportFromUSAInterfaceCode", {
        get: function () { return this.EntityPM.ExportFromUSAInterfaceCode; },
        set: function (value) {
            if (this.EntityPM.ExportFromUSAInterfaceCode != value) {
                this.EntityPM.ExportFromUSAInterfaceCode = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsInterfaceSettingsComponent.prototype, "ActivateCustomsManagementInShipments", {
        get: function () { return this.EntityPM.ActivateCustomsManagementInShipments; },
        set: function (value) {
            if (this.EntityPM.ActivateCustomsManagementInShipments != value) {
                this.EntityPM.ActivateCustomsManagementInShipments = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomsInterfaceSettingsComponent.prototype.DetailsClicked = function (type) {
        var _this = this;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        switch (type) {
            case "Local": {
                logitudeWindow.Title = "Local Interface Credintials";
                logitudeWindow.WindowArgs = this.EntityPM;
                logitudeWindow.WindowClosed.subscribe(function ($event) { return _this.OnWindowClosed($event); });
                logitudeWindow.Show('./Common/Components/Maintenance/CustomsInterface/CustomsInterfaceCredintialsComponent');
                break;
            }
            case "Import": {
                this.entityResourceService.getEntityResourceByTableName("FTPDetail", 0).subscribe(function (response) {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Title = "Artemus Settings";
                    logitudeWindow.WindowArgs = _this.EntityPM;
                    logitudeWindow.WindowClosed.subscribe(function ($event) { return _this.OnWindowClosed($event); });
                    logitudeWindow.Show('./Common/Components/Maintenance/CustomsInterface/ArtemusSettingsComponent');
                });
                break;
            }
        }
    };
    CustomsInterfaceSettingsComponent.prototype.OnWindowClosed = function (message) {
        if (message == "ok") {
            this.SetTickProperties();
        }
    };
    CustomsInterfaceSettingsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CustomsInterfaceSettingsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (this.ValidationErrorsList.length == 0) {
            if (this.EntityPM.IsDirty) {
                this.CurrentSession.StartBusyIndicatorSaving();
                if (this.EntityPM.Tenant == null) {
                    this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    this.myService.insert(this.EntityPM).subscribe(function (myRespone) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (!myRespone.HasError) {
                            ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM = _this.EntityPM;
                            _this.CurrentSession.CloseCurrentWindowEmit("OK");
                        }
                        else {
                            _this.ValidationErrorsList = myRespone.ErrorsArray;
                        }
                    });
                }
                else {
                    this.myService.update(this.EntityPM).subscribe(function (myRespone) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (!myRespone.HasError) {
                            ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM = _this.EntityPM;
                            _this.CurrentSession.CloseCurrentWindowEmit("OK");
                        }
                        else {
                            _this.ValidationErrorsList = myRespone.ErrorsArray;
                        }
                    });
                }
            }
            else {
                this.CurrentSession.CloseCurrentWindow();
            }
        }
    };
    CustomsInterfaceSettingsComponent = __decorate([
        core_1.Component({
            selector: 'CustomsInterfaceSettingsComponent',
            moduleId: module.id,
            templateUrl: './CustomsInterfaceSettingsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], CustomsInterfaceSettingsComponent);
    return CustomsInterfaceSettingsComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomsInterfaceSettingsComponent = CustomsInterfaceSettingsComponent;
//# sourceMappingURL=CustomsInterfaceSettingsComponent.js.map