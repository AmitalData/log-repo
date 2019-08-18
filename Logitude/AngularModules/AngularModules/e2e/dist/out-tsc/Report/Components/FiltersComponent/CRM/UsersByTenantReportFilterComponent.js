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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ReportFliter_1 = require("../../../Components/Filters/ReportFliter");
var QueryFilterItem_1 = require("../../../Components/Filters/QueryFilterItem");
var UsersByTenantReportFilterComponent = /** @class */ (function (_super) {
    __extends(UsersByTenantReportFilterComponent, _super);
    function UsersByTenantReportFilterComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.RunReportEvent = new core_1.EventEmitter();
        // Filters
        _this.distributorCode = null;
        _this.packageCode = null;
        _this.addOnPackageCode = null;
        _this.includeInactiveUsers = false;
        _this.includeInactiveTenants = false;
        return _this;
    }
    UsersByTenantReportFilterComponent.prototype.ngOnInit = function () {
    };
    Object.defineProperty(UsersByTenantReportFilterComponent.prototype, "DistributorCode", {
        get: function () { return this.distributorCode; },
        set: function (value) {
            if (this.distributorCode != value) {
                this.distributorCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UsersByTenantReportFilterComponent.prototype, "PackageCode", {
        get: function () { return this.packageCode; },
        set: function (value) {
            if (this.packageCode != value) {
                this.packageCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UsersByTenantReportFilterComponent.prototype, "AddOnPackageCode", {
        get: function () { return this.addOnPackageCode; },
        set: function (value) {
            if (this.addOnPackageCode != value) {
                this.addOnPackageCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UsersByTenantReportFilterComponent.prototype, "IncludeInactiveUsers", {
        get: function () { return this.includeInactiveUsers; },
        set: function (value) {
            if (this.includeInactiveUsers != value) {
                this.includeInactiveUsers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UsersByTenantReportFilterComponent.prototype, "IncludeInactiveTenants", {
        get: function () { return this.includeInactiveTenants; },
        set: function (value) {
            if (this.includeInactiveTenants != value) {
                this.includeInactiveTenants = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    UsersByTenantReportFilterComponent.prototype.RunButtonClicked = function (arg) {
        var myFilterItems = [];
        myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("DistributorCode", this.DistributorCode));
        myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("PackageCode", this.PackageCode));
        myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("AddOnPackageCode", this.AddOnPackageCode));
        myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("IncludeInactiveUsers", this.IncludeInactiveUsers));
        myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("IncludeInactiveTenants", this.IncludeInactiveTenants));
        var myReportFliter = new ReportFliter_1.ReportFliter();
        myReportFliter.NumberOfPage = 1;
        myReportFliter.ProcessType = "GenerateReport";
        myReportFliter.QueryFilterItemLists = myFilterItems;
        this.RunReportEvent.emit(myReportFliter);
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], UsersByTenantReportFilterComponent.prototype, "RunReportEvent", void 0);
    UsersByTenantReportFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './UsersByTenantReportFilterComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], UsersByTenantReportFilterComponent);
    return UsersByTenantReportFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.UsersByTenantReportFilterComponent = UsersByTenantReportFilterComponent;
//# sourceMappingURL=UsersByTenantReportFilterComponent.js.map