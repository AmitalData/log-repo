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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var HybridPartnerExtendedListService_1 = require("../../../../Common/Services/ExtendedLists/HybridPartnerExtendedListService");
var PermissionsHybridPartnerTabComponent = /** @class */ (function (_super) {
    __extends(PermissionsHybridPartnerTabComponent, _super);
    function PermissionsHybridPartnerTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.AllowdHybridPartnerLists = [];
        _this.AllowingHybridPartnerLists = [];
        _this.IsCompleteLoadAllowdHybrid = false;
        _this.IsCompleteLoadAllowingHybrid = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.hybridPartnerExtendedListService = new HybridPartnerExtendedListService_1.HybridPartnerExtendedListService();
        if (_this.entityArgs.EntityPM) {
            _this.LoadData(_this.entityArgs.EntityPM.Id);
        }
        return _this;
    }
    PermissionsHybridPartnerTabComponent.prototype.LoadData = function (hybridPartnerId) {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.LoadAllowdHybridPartner(hybridPartnerId);
        this.LoadAllowingHybridPartner(hybridPartnerId);
    };
    PermissionsHybridPartnerTabComponent.prototype.LoadAllowdHybridPartner = function (hybridPartnerId) {
        var _this = this;
        this.IsCompleteLoadAllowdHybrid = false;
        this.AllowdHybridPartnerLists = [];
        this.hybridPartnerExtendedListService.GetAllowdHybridPartnerLists(hybridPartnerId).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                _this.AllowdHybridPartnerLists = pmResponse.Result;
            }
            _this.IsCompleteLoadAllowdHybrid = true;
            _this.StopLoading();
        });
    };
    PermissionsHybridPartnerTabComponent.prototype.LoadAllowingHybridPartner = function (hybridPartnerId) {
        var _this = this;
        this.IsCompleteLoadAllowingHybrid = false;
        this.AllowingHybridPartnerLists = [];
        this.hybridPartnerExtendedListService.GetAllowingHybridPartnerLists(hybridPartnerId).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                _this.AllowingHybridPartnerLists = pmResponse.Result;
            }
            _this.IsCompleteLoadAllowingHybrid = true;
            _this.StopLoading();
        });
    };
    PermissionsHybridPartnerTabComponent.prototype.StopLoading = function () {
        if (this.IsCompleteLoadAllowingHybrid && this.IsCompleteLoadAllowdHybrid) {
            this.CurrentSession.StopBusyIndicator();
        }
    };
    PermissionsHybridPartnerTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './PermissionsHybridPartnerTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], PermissionsHybridPartnerTabComponent);
    return PermissionsHybridPartnerTabComponent;
}(BaseComponent_1.BaseComponent));
exports.PermissionsHybridPartnerTabComponent = PermissionsHybridPartnerTabComponent;
//# sourceMappingURL=PermissionsHybridPartnerTabComponent.js.map