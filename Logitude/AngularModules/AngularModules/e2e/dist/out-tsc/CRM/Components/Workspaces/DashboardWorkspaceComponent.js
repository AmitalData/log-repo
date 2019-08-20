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
var LocationDirective_1 = require("../../../Infrastructure/Utilities/LocationDirective");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var DashboardWorkspaceComponent = /** @class */ (function (_super) {
    __extends(DashboardWorkspaceComponent, _super);
    function DashboardWorkspaceComponent() {
        var _this = _super.call(this) || this;
        _this.isViewInited = false;
        _this.Retries = 0;
        _this.PageChild_BCD = null;
        _this.PageChild_INP = null;
        _this.PageChild_COP = null;
        _this.selectedTabCode = "BCD";
        _this.RunComponent();
        return _this;
    }
    DashboardWorkspaceComponent.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.isViewInited = true;
                this.InitializeComponent();
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    DashboardWorkspaceComponent.prototype.InitializeComponent = function () {
        if (this.isViewInited) {
            this.SelectionChanged();
        }
    };
    DashboardWorkspaceComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    Object.defineProperty(DashboardWorkspaceComponent.prototype, "SelectedTabCode", {
        get: function () { return this.selectedTabCode; },
        set: function (newValue) {
            if (this.selectedTabCode != newValue) {
                this.selectedTabCode = newValue;
                this.SelectionChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    DashboardWorkspaceComponent.prototype.SelectionChanged = function () {
        var _this = this;
        if (this.isViewInited) {
            if (this.SelectedTabCode != null) {
                var myLocation = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedTabCode; })[0];
                if (myLocation != null) {
                    switch (this.SelectedTabCode) {
                        case "BCD": {
                            if (this.PageChild_BCD == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/DashboardTabComponents/ByCreateDateComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.PageChild_BCD = cmpRef.instance;
                                    _this.PageChild_BCD.InitTab(_this);
                                });
                            }
                            else {
                                this.PageChild_BCD.RefreshTab();
                            }
                            break;
                        }
                        case "INP": {
                            if (this.PageChild_INP == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/DashboardTabComponents/ByInProgressComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.PageChild_INP = cmpRef.instance;
                                    _this.PageChild_INP.InitTab(_this);
                                });
                            }
                            else {
                                this.PageChild_INP.RefreshTab();
                            }
                            break;
                        }
                        case "COP": {
                            if (this.PageChild_COP == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/DashboardTabComponents/CompanyPerformanceComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.PageChild_COP = cmpRef.instance;
                                    _this.PageChild_COP.InitTab(_this);
                                });
                            }
                            else {
                                this.PageChild_COP.RefreshTab();
                            }
                            break;
                        }
                    }
                }
            }
        }
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], DashboardWorkspaceComponent.prototype, "AllLocations", void 0);
    DashboardWorkspaceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DashboardWorkspaceComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DashboardWorkspaceComponent);
    return DashboardWorkspaceComponent;
}(BaseComponent_1.BaseComponent));
exports.DashboardWorkspaceComponent = DashboardWorkspaceComponent;
//# sourceMappingURL=DashboardWorkspaceComponent.js.map