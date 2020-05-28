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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var BankDepositListService_1 = require("../../../Services/StandardLists/BankDepositListService");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var CashBookManageDepoTabComponent = /** @class */ (function (_super) {
    __extends(CashBookManageDepoTabComponent, _super);
    function CashBookManageDepoTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.ObjectTableName = "CashBook";
        _this.DataContext = _this;
        _this._BankDepositListService = new BankDepositListService_1.BankDepositListService();
        _this.ItemsSource = [];
        _this.TotalSum = 0;
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.tenantCurrency = "";
        _this.EntityPM = entityArgs.EntityPM;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        var today = new Date();
        _this.ToDate = new Date();
        var lastmonth = today.setMonth(today.getMonth() - 1);
        _this.FromDate = new Date(lastmonth);
        _this.tenantCurrency = SessionLocator_1.SessionLocator.TenantPM.CurrencyCode;
        return _this;
    }
    Object.defineProperty(CashBookManageDepoTabComponent.prototype, "FromDate", {
        get: function () { return this.fromDate; },
        set: function (value) {
            if (this.fromDate != value) {
                this.fromDate = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ToDate) && !Tools_1.AppTool.IsNullOrEmpty(this.FromDate)) {
                    this.dateFilter = new ApiQueryFilters_1.FilterItem("CreateDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
                    this.GetDeposits();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CashBookManageDepoTabComponent.prototype, "ToDate", {
        get: function () { return this.toDate; },
        set: function (value) {
            if (this.toDate != value) {
                this.toDate = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ToDate) && !Tools_1.AppTool.IsNullOrEmpty(this.FromDate)) {
                    this.dateFilter = new ApiQueryFilters_1.FilterItem("CreateDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
                    this.GetDeposits();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    CashBookManageDepoTabComponent.prototype.GetDeposits = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters;
        if (this.dateFilter) {
            filters.AdditionalFilters.push(this.dateFilter);
        }
        if (this.searchFieldFilter) {
            filters.AdditionalFilters.push(this.searchFieldFilter);
        }
        filters.GetAll = true;
        filters.SortBy = "CreateDate";
        filters.SortDirection = "Descending";
        filters.addAdditionalFilter("CashBookId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        this._BankDepositListService.getByFilters(filters).subscribe(function (myResult) {
            console.log("Response: ", myResult);
            if (myResult == null) {
                _this.ItemsSource = [];
            }
            else {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    _this.ItemsSource = myResponse.Result;
                    // Calculate commulative sums 
                    _this.TotalSum = 0;
                    for (var i = 0; i < _this.ItemsSource.length; i++) {
                        _this.TotalSum += _this.ItemsSource[i].ForeignAmount;
                    }
                }
            }
        });
    };
    CashBookManageDepoTabComponent.prototype.RowClicked = function (item) {
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: item.Id,
                    ObjectTableName: 'BankDeposit',
                    BackButtonLabel: 'Cashbook'
                });
            });
        }
    };
    CashBookManageDepoTabComponent.prototype.TextChanged = function (searchtext) {
        var _this = this;
        if (searchtext != null || searchtext != undefined) {
            this.timerToken = setTimeout(function () {
                _this.searchFieldFilter = new ApiQueryFilters_1.FilterItem("SearchFields", searchtext, null, null, "Contains", false, false, false, "string", false);
                _this.GetDeposits();
            }, 700);
        }
        else {
            this.searchFieldFilter = null;
            this.GetDeposits();
        }
    };
    CashBookManageDepoTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CashBookManageDepoTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], CashBookManageDepoTabComponent);
    return CashBookManageDepoTabComponent;
}(BaseComponent_1.BaseComponent));
exports.CashBookManageDepoTabComponent = CashBookManageDepoTabComponent;
//# sourceMappingURL=CashBookManageDepoTabComponent.js.map