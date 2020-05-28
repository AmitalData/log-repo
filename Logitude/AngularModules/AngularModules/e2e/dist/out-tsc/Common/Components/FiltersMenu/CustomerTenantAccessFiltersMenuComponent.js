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
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var CustomerTenantAccessFiltersMenuComponent = /** @class */ (function () {
    function CustomerTenantAccessFiltersMenuComponent() {
        this.SelectedValue = "All";
        this.SelectedValueChanged = new core_1.EventEmitter();
        this.apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (this.CurrentSession == null) {
            this.ImpoterFilter_ALL = "ImpoterFilter_ALL-1_-1";
            this.ImpoterFilter_A = "ImpoterFilter_A-1_-1";
            this.ImpoterFilter_W = "ImpoterFilter_W-1_-1";
            this.ImpoterFilter_IP = "ImpoterFilter_IP-1_-1";
        }
        else {
            var idIndex = this.CurrentSession.GetNewId("ImpoterFilter");
            this.ImpoterFilter_ALL = "ImpoterFilter_ALL_" + idIndex;
            this.ImpoterFilter_A = "ImpoterFilter_A_" + idIndex;
            this.ImpoterFilter_W = "ImpoterFilter_W_" + idIndex;
            this.ImpoterFilter_IP = "ImpoterFilter_IP_" + idIndex;
        }
        this.apiQueryFilters.SortBy = "RequestDateTime";
        this.apiQueryFilters.SortDirection = "Descending";
    }
    CustomerTenantAccessFiltersMenuComponent.prototype.itemClicked = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            if (this.apiQueryFilters.AdditionalFilters.length > 0) {
                this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(function (a) { return a.FieldName != "Status"; });
            }
            this.apiQueryFilters.SortBy = "RequestDateTime";
            this.apiQueryFilters.SortDirection = "Descending";
            this.apiQueryFilters.addAdditionalFilter("Status", itemValue, null, null, "Equals", false, true, false, "string", (itemValue == "All" ? true : false));
            this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters });
        }
    };
    CustomerTenantAccessFiltersMenuComponent.prototype.itemMouseOver = function (itemValue) {
        if (this.SelectedValue != itemValue) {
        }
    };
    CustomerTenantAccessFiltersMenuComponent.prototype.itemMouseLeave = function (itemValue) {
        if (this.SelectedValue != itemValue) {
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], CustomerTenantAccessFiltersMenuComponent.prototype, "SelectedValueChanged", void 0);
    CustomerTenantAccessFiltersMenuComponent = __decorate([
        core_1.Component({
            selector: 'CustomerTenantAccessFiltersMenuComponent',
            inputs: ['SelectedValue'],
            template: "\n    <ul class=\"FiltersMenu\" style=\"float:right\">\n        <li  (click)=\"itemClicked('All')\" (mouseover)=\"itemMouseOver('All')\" [class.SelectedFilter]=\"SelectedValue === 'All'\">\n            All\n        </li>\n        <li style=\"color:green\" (click)=\"itemClicked('A')\" (mouseover)=\"itemMouseOver('A')\" (mouseleave)=\"itemMouseLeave('A')\" [class.SelectedFilter]=\"SelectedValue === 'A'\" title=\"Accepted\">\n        A\n        </li>\n        <li  style=\"color:orange\"  (click)=\"itemClicked('W')\" (mouseover)=\"itemMouseOver('W')\" (mouseleave)=\"itemMouseLeave('W')\" [class.SelectedFilter]=\"SelectedValue === 'W'\" title=\"Waiting\">\n        W\n        </li>\n        <li style=\"color:red\" (click)=\"itemClicked('IA')\" (mouseover)=\"itemMouseOver('IA')\" (mouseleave)=\"itemMouseLeave('IA')\" style=\"color:'red'\" [class.SelectedFilter]=\"SelectedValue === 'IA'\"  title=\"InActive\">\n        I\n        </li>\n      \n    </ul>\n    "
        }),
        __metadata("design:paramtypes", [])
    ], CustomerTenantAccessFiltersMenuComponent);
    return CustomerTenantAccessFiltersMenuComponent;
}());
exports.CustomerTenantAccessFiltersMenuComponent = CustomerTenantAccessFiltersMenuComponent;
//# sourceMappingURL=CustomerTenantAccessFiltersMenuComponent.js.map