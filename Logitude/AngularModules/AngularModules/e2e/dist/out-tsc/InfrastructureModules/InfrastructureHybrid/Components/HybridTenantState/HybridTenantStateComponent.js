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
require("rxjs/add/operator/map");
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var HybridTenantStateListExtendedService_1 = require("../../../../Common/Services/ExtendedLists/HybridTenantStateListExtendedService");
var HybridTenantStateComponent = /** @class */ (function (_super) {
    __extends(HybridTenantStateComponent, _super);
    function HybridTenantStateComponent() {
        var _this = _super.call(this) || this;
        _this.HybridTenantStateLists = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.hybridTenantStateListExtendedService = new HybridTenantStateListExtendedService_1.HybridTenantStateListExtendedService();
        _this.CurrentSession.StartBusyIndicatorLoading();
        return _this;
    }
    HybridTenantStateComponent.prototype.ngOnInit = function () {
        this.LoadData();
    };
    HybridTenantStateComponent.prototype.LoadData = function () {
        var _this = this;
        this.HybridTenantStateLists = [];
        this.hybridTenantStateListExtendedService.GetHybridTenantStateLists().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.HybridTenantStateLists = myResponse.Result;
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    HybridTenantStateComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    HybridTenantStateComponent.prototype.SaveButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    HybridTenantStateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'HybridTenantStateComponent',
            templateUrl: './HybridTenantStateComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], HybridTenantStateComponent);
    return HybridTenantStateComponent;
}(BaseComponent_1.BaseComponent));
exports.HybridTenantStateComponent = HybridTenantStateComponent;
//# sourceMappingURL=HybridTenantStateComponent.js.map