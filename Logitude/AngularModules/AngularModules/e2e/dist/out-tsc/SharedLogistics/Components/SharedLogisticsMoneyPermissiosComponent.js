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
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SharedLogisticsSettingPM_1 = require("../../Infrastructure/EntityPMs/SharedLogisticsSettingPM");
var SharedLogisticsSettingPMService_1 = require("../../Infrastructure/Services/StandardPMs/SharedLogisticsSettingPMService");
var EntityResourceService_1 = require("../../Infrastructure/Services/EntityResourceService");
var ObjectsLocator_1 = require("../../Infrastructure/Locators/ObjectsLocator");
var SharedLogisticsMoneyPermissiosComponent = /** @class */ (function (_super) {
    __extends(SharedLogisticsMoneyPermissiosComponent, _super);
    function SharedLogisticsMoneyPermissiosComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.DataContext = _this;
        _this.IsResourcesReady = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ValidationErrorsList = [];
        _this.myService = new SharedLogisticsSettingPMService_1.SharedLogisticsSettingPMService();
        _this.ObjectTableName = "SharedLogisticsSetting";
        return _this;
    }
    SharedLogisticsMoneyPermissiosComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            _this.myService.get(SessionLocator_1.SessionLocator.Tenant.toString()).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    if (myResponse.Result) {
                        _this.EntityPM = myResponse.Result;
                    }
                    else {
                        _this.EntityPM = new SharedLogisticsSettingPM_1.SharedLogisticsSettingPM();
                        _this.EntityPM.IsDirty = false;
                    }
                    _this.IsResourcesReady = true;
                }
            });
        });
    };
    Object.defineProperty(SharedLogisticsMoneyPermissiosComponent.prototype, "IsInvoicesMenuEnabled", {
        get: function () { return this.EntityPM.IsInvoicesMenuEnabled; },
        set: function (value) {
            if (this.EntityPM.IsInvoicesMenuEnabled != value) {
                this.EntityPM.IsInvoicesMenuEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedLogisticsMoneyPermissiosComponent.prototype, "IsMoneyTabEnabled", {
        get: function () { return this.EntityPM.IsMoneyTabEnabled; },
        set: function (value) {
            if (this.EntityPM.IsMoneyTabEnabled != value) {
                this.EntityPM.IsMoneyTabEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    SharedLogisticsMoneyPermissiosComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SharedLogisticsMoneyPermissiosComponent.prototype.OkButtonClicked = function () {
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
                            ObjectsLocator_1.ObjectsLocator.SharedLogisticsSettingPM = _this.EntityPM;
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
                            ObjectsLocator_1.ObjectsLocator.SharedLogisticsSettingPM = _this.EntityPM;
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
    SharedLogisticsMoneyPermissiosComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SharedLogisticsMoneyPermissiosComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], SharedLogisticsMoneyPermissiosComponent);
    return SharedLogisticsMoneyPermissiosComponent;
}(BaseComponent_1.BaseComponent));
exports.SharedLogisticsMoneyPermissiosComponent = SharedLogisticsMoneyPermissiosComponent;
//# sourceMappingURL=SharedLogisticsMoneyPermissiosComponent.js.map