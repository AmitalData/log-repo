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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var CurrencyRatesService_1 = require("../../../Common/Services/CurrencyRatesService");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var RatesTableListService_1 = require("../../../Infrastructure/Services/StandardLists/RatesTableListService");
var RatesHistoryComponent = /** @class */ (function (_super) {
    __extends(RatesHistoryComponent, _super);
    function RatesHistoryComponent() {
        var _this = _super.call(this) || this;
        _this.ItemsSource = [];
        _this.LastRate = new CurrencyRatesService_1.LastRate();
        _this.count = 0;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        /*Pager & Provider*/
        _this.queryPageIndex = 0;
        _this.pageIndex = 1;
        _this.totalPagesCount = 1;
        _this.pageSize = 10;
        /* Pager Buttons States */
        _this.isHitStateFirstButton = false;
        _this.opacityFirstButton = 0.5;
        _this.isHitStatePrevButton = false;
        _this.opacityPrevButton = 0.5;
        _this.isHitStateNextButton = false;
        _this.opacityNextButton = 0.5;
        _this.isHitStateLastButton = false;
        _this.opacityLastButton = 0.5;
        return _this;
    }
    RatesHistoryComponent.prototype.SetWindowArgs = function (args) {
        this.LastRate = args;
        this.BuildData();
    };
    RatesHistoryComponent.prototype.BuildData = function () {
        if (this.RatesTableListService == null) {
            this.RatesTableListService = new RatesTableListService_1.RatesTableListService();
        }
        this.filters = new ApiQueryFilters_1.ApiQueryFilters();
        this.filters.SortBy = "ValueDate";
        this.filters.SortDirection = "Descending";
        this.filters.GetCount = true;
        var value = this.LastRate.ForeignCurrencyId;
        this.filters.addAdditionalFilter("ForeignCurrencyId", value, null, null, "Equals", false, false, false, "Text");
        this.PageIndex = 1;
        this.QueryPageIndex = 0;
        this.LoadData();
        // this.LoadDateCount();
    };
    RatesHistoryComponent.prototype.LoadData = function () {
        var _this = this;
        //this.ItemsSource = [];
        this.filters.PageIndex = this.QueryPageIndex;
        this.filters.PageSize = this.PageSize;
        this.RatesTableListService.getByFilters(this.filters).subscribe(function (myResult) {
            if (myResult == null) {
                _this.ItemsSource = [];
            }
            else {
                _this.ItemsSource = myResult.Result;
                _this.count = myResult.Count;
                var size = _this.pageSize;
                _this.TotalPagesCount = Math.ceil(_this.count / size);
                if (_this.TotalPagesCount == 0) {
                    _this.TotalPagesCount = 1;
                }
                _this.SetPagerButtonsStates();
            }
        });
    };
    RatesHistoryComponent.prototype.LoadDateCount = function () {
        var size = this.pageSize;
        this.TotalPagesCount = Math.ceil(this.count / size);
        if (this.TotalPagesCount == 0) {
            this.TotalPagesCount = 1;
        }
        this.SetPagerButtonsStates();
    };
    Object.defineProperty(RatesHistoryComponent.prototype, "QueryPageIndex", {
        get: function () {
            return this.queryPageIndex;
        },
        set: function (value) {
            this.queryPageIndex = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesHistoryComponent.prototype, "PageIndex", {
        get: function () {
            return this.pageIndex;
        },
        set: function (value) {
            this.pageIndex = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesHistoryComponent.prototype, "TotalPagesCount", {
        get: function () {
            return this.totalPagesCount;
        },
        set: function (value) {
            this.totalPagesCount = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesHistoryComponent.prototype, "PageSize", {
        get: function () {
            return this.pageSize;
        },
        set: function (value) {
            this.pageSize = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesHistoryComponent.prototype, "IsHitState_FirstButton", {
        get: function () {
            return this.isHitStateFirstButton;
        },
        set: function (value) {
            this.isHitStateFirstButton = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesHistoryComponent.prototype, "Opacity_FirstButton", {
        get: function () {
            return this.opacityFirstButton;
        },
        set: function (value) {
            this.opacityFirstButton = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesHistoryComponent.prototype, "IsHitState_PrevButton", {
        get: function () {
            return this.isHitStatePrevButton;
        },
        set: function (value) {
            this.isHitStatePrevButton = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesHistoryComponent.prototype, "Opacity_PrevButton", {
        get: function () {
            return this.opacityPrevButton;
        },
        set: function (value) {
            this.opacityPrevButton = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesHistoryComponent.prototype, "IsHitState_NextButton", {
        get: function () {
            return this.isHitStateNextButton;
        },
        set: function (value) {
            this.isHitStateNextButton = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesHistoryComponent.prototype, "Opacity_NextButton", {
        get: function () {
            return this.opacityNextButton;
        },
        set: function (value) {
            this.opacityNextButton = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesHistoryComponent.prototype, "IsHitState_LastButton", {
        get: function () {
            return this.isHitStateLastButton;
        },
        set: function (value) {
            this.isHitStateLastButton = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesHistoryComponent.prototype, "Opacity_LastButton", {
        get: function () {
            return this.opacityLastButton;
        },
        set: function (value) {
            this.opacityLastButton = value;
        },
        enumerable: true,
        configurable: true
    });
    /* First Page */
    RatesHistoryComponent.prototype.SetPagerButtonsStates = function () {
        if (this.PageIndex == 1 && this.PageIndex == this.TotalPagesCount) {
            this.IsHitState_FirstButton = false;
            this.IsHitState_PrevButton = false;
            this.IsHitState_NextButton = false;
            this.IsHitState_LastButton = false;
            this.Opacity_FirstButton = 0.5;
            this.Opacity_PrevButton = 0.5;
            this.Opacity_NextButton = 0.5;
            this.Opacity_LastButton = 0.5;
        }
        else if (this.PageIndex == 1 && this.PageIndex < this.TotalPagesCount) {
            this.IsHitState_FirstButton = false;
            this.IsHitState_PrevButton = false;
            this.Opacity_FirstButton = 0.5;
            this.Opacity_PrevButton = 0.5;
            this.IsHitState_NextButton = true;
            this.IsHitState_LastButton = true;
            this.Opacity_NextButton = 1;
            this.Opacity_LastButton = 1;
        }
        else if (this.PageIndex > 1 && this.PageIndex == this.TotalPagesCount) {
            this.IsHitState_FirstButton = true;
            this.IsHitState_PrevButton = true;
            this.Opacity_FirstButton = 1;
            this.Opacity_PrevButton = 1;
            this.IsHitState_NextButton = false;
            this.IsHitState_LastButton = false;
            this.Opacity_NextButton = 0.5;
            this.Opacity_LastButton = 0.5;
        }
        else if (this.PageIndex > 1 && this.PageIndex < this.TotalPagesCount) {
            this.IsHitState_FirstButton = true;
            this.IsHitState_PrevButton = true;
            this.IsHitState_NextButton = true;
            this.IsHitState_LastButton = true;
            this.Opacity_FirstButton = 1;
            this.Opacity_PrevButton = 1;
            this.Opacity_NextButton = 1;
            this.Opacity_LastButton = 1;
        }
    };
    RatesHistoryComponent.prototype.FirstPageClick = function () {
        this.PageIndex = 1;
        this.QueryPageIndex = 0;
        this.SetPagerButtonsStates();
        this.LoadData();
    };
    RatesHistoryComponent.prototype.PreviousPageClick = function () {
        this.PageIndex = this.PageIndex - 1;
        this.QueryPageIndex = this.QueryPageIndex - 10;
        this.SetPagerButtonsStates();
        this.LoadData();
    };
    RatesHistoryComponent.prototype.NextPageClick = function () {
        this.PageIndex = this.PageIndex + 1;
        this.QueryPageIndex = this.QueryPageIndex + 10;
        this.SetPagerButtonsStates();
        this.LoadData();
    };
    RatesHistoryComponent.prototype.LastPageClick = function () {
        this.PageIndex = this.TotalPagesCount;
        this.QueryPageIndex = (this.TotalPagesCount - 1) * this.pageSize;
        this.SetPagerButtonsStates();
        this.LoadData();
    };
    Object.defineProperty(RatesHistoryComponent.prototype, "ForeignCurrencyCode", {
        // Props
        get: function () {
            return this.LastRate.ForeignCurrencyCode;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesHistoryComponent.prototype, "ValueDate", {
        get: function () {
            return this.LastRate.ValueDate;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesHistoryComponent.prototype, "Rate", {
        get: function () {
            return this.LastRate.Rate;
        },
        enumerable: true,
        configurable: true
    });
    // Commands
    RatesHistoryComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    RatesHistoryComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './RatesHistoryComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], RatesHistoryComponent);
    return RatesHistoryComponent;
}(BaseComponent_1.BaseComponent));
exports.RatesHistoryComponent = RatesHistoryComponent;
//# sourceMappingURL=RatesHistoryComponent.js.map