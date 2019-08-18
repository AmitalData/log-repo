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
var FeatureLocator_1 = require("../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var ObjectsLocator_1 = require("../../Infrastructure/Locators/ObjectsLocator");
var Guid_1 = require("../../Infrastructure/Utilities/Guid");
var SharedLogisticsService_1 = require("../Services/Others/SharedLogisticsService");
var TenantPMService_1 = require("../../Common/Services/StandardPMs/TenantPMService");
var Cloner_1 = require("../../Infrastructure/Utilities/Cloner");
var SharedLogisticsSettingComponent = /** @class */ (function () {
    function SharedLogisticsSettingComponent() {
        this.IsShowActivateWebAccessArea = true;
        this.IsShowMobileActivateArea = true;
        this.OnCloseWindowEvent = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (this.tenantPMService == null) {
            this.tenantPMService = new TenantPMService_1.TenantPMService();
        }
    }
    Object.defineProperty(SharedLogisticsSettingComponent.prototype, "IsSharedLogisticsActivated", {
        get: function () {
            if (this.TenantPM) {
                return this.TenantPM.IsSharedLogisticsActivated;
            }
            else
                return false;
        },
        set: function (value) {
            if (this.TenantPM != null) {
                if (this.TenantPM.IsSharedLogisticsActivated != value) {
                    this.TenantPM.IsSharedLogisticsActivated = value;
                    this.SetPropertiesEnable();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedLogisticsSettingComponent.prototype, "IsWebAccessActivated", {
        get: function () {
            if (this.TenantPM) {
                return this.TenantPM.IsWebAccessActivated;
            }
            else
                return false;
        },
        set: function (value) {
            if (this.TenantPM) {
                this.TenantPM.IsWebAccessActivated = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedLogisticsSettingComponent.prototype, "IsMobileActivated", {
        get: function () {
            if (this.TenantPM) {
                return this.TenantPM.IsMobileActivated;
            }
            else
                return false;
        },
        set: function (value) {
            if (this.TenantPM) {
                this.TenantPM.IsMobileActivated = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedLogisticsSettingComponent.prototype, "SharedLogisticsMessageLink", {
        get: function () {
            if (this.TenantPM) {
                return this.TenantPM.SharedLogisticsMessageLink;
            }
            else
                return false;
        },
        set: function (value) {
            if (this.TenantPM) {
                this.TenantPM.SharedLogisticsMessageLink = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    SharedLogisticsSettingComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.TenantPM) {
            this.Run();
        }
        this.OnCloseWindowEvent.subscribe(function ($event) {
            _this.SaveButtonClicked();
        });
    };
    SharedLogisticsSettingComponent.prototype.Run = function () {
        this.IsSharedLogisticsActivated = this.TenantPM.IsSharedLogisticsActivated;
        this.IsWebAccessActivated = this.TenantPM.IsWebAccessActivated;
        this.IsMobileActivated = this.TenantPM.IsMobileActivated;
        this.SharedLogisticsMessageLink = this.TenantPM.SharedLogisticsMessageLink;
        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "MOBILE")) {
            this.IsShowMobileActivateArea = false;
        }
        else {
            this.IsShowMobileActivateArea = true;
        }
        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "SHAREDLOGISTICS")) {
            this.IsShowActivateWebAccessArea = false;
        }
        else {
            this.IsShowActivateWebAccessArea = true;
        }
        this.IsSharedLogisticsActivatedCheckboxBoxId = Guid_1.Guid.newGuid();
        this.IsMobileActivatedCheckboxBoxId = Guid_1.Guid.newGuid();
        this.IsWebAccessActivatedCheckboxBoxId = Guid_1.Guid.newGuid();
        this.SharedLogisticsMessageLinkCheckboxBoxId = Guid_1.Guid.newGuid();
        this.SetPropertiesEnable();
    };
    SharedLogisticsSettingComponent.prototype.SetPropertiesEnable = function () {
        if (this.IsSharedLogisticsActivated) {
            this.IsSharedLogisticsActivatedEnable = false;
            this.SharedLogisticsMessageLinkEnable = true;
            if (ObjectsLocator_1.ObjectsLocator.GlobalSetting.WorkEnvironment != "cloud" && this.TenantPM.CountryCode == "IL") {
                this.IsMobileActivatedEnable = false;
            }
            else {
                this.IsMobileActivatedEnable = true;
            }
        }
        else {
            this.IsSharedLogisticsActivatedEnable = true;
            this.IsMobileActivatedEnable = false;
            this.SharedLogisticsMessageLinkEnable = false;
        }
    };
    SharedLogisticsSettingComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.StopBusyIndicator();
        if (this.TenantPM.IsDirty) {
            this.RejectChanges();
        }
        this.CurrentSession.CloseCurrentWindow();
    };
    SharedLogisticsSettingComponent.prototype.OnShardLogisticChange = function () {
        this.TenantPM.IsSharedLogisticsActivated = !this.TenantPM.IsSharedLogisticsActivated;
        this.IsSharedLogisticsActivated = !this.IsSharedLogisticsActivated;
        if (this.IsSharedLogisticsActivated) {
            this.SetPropertiesEnable();
        }
    };
    SharedLogisticsSettingComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        if (this.TenantPM.IsDirty) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.tenantPMService.update(this.TenantPM).subscribe(function (res) {
                _this.CloseButtonClicked();
            });
        }
        else
            this.CloseButtonClicked();
    };
    SharedLogisticsSettingComponent.prototype.SetWindowArgs = function (args) {
        this.TenantPM = args.TenantPM;
        this.IsShowAreaColseAndCancelButton = true;
        this.Clone();
        // this.Run();
    };
    SharedLogisticsSettingComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.TenantPM);
        this.myCloner.AddField('IsWebAccessActivated');
        this.myCloner.AddField('IsSharedLogisticsActivated');
        this.myCloner.AddField('IsMobileActivated');
        this.myCloner.AddField('SharedLogisticsMessageLink');
        this.myCloner.AddEntity(this.TenantPM);
    };
    SharedLogisticsSettingComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    SharedLogisticsSettingComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SharedLogisticsSetting',
            templateUrl: './SharedLogisticsSettingComponent.html',
            inputs: ['TenantPM', 'OnCloseWindowEvent'],
            providers: [SharedLogisticsService_1.SharedLogisticsService],
        }),
        __metadata("design:paramtypes", [])
    ], SharedLogisticsSettingComponent);
    return SharedLogisticsSettingComponent;
}());
exports.SharedLogisticsSettingComponent = SharedLogisticsSettingComponent;
//# sourceMappingURL=SharedLogisticsSettingComponent.js.map