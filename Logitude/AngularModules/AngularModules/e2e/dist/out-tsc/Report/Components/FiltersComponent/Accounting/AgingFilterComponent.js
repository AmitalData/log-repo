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
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var AgingFilterComponent = /** @class */ (function (_super) {
    __extends(AgingFilterComponent, _super);
    function AgingFilterComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.RunReportEvent = new core_1.EventEmitter();
        _this.isReady = false;
        _this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        //#region Filters
        //row 1
        _this.agingForDate = null;
        //#region Category fields
        _this.IsCategoryDisabled = false;
        _this.CategoriesList = [
            'Category 1',
            'Category 2',
            'Category 3',
            'Category 4',
            'Category 5'
        ];
        // get requierd resources
        _this.entityResourceService.getEntityResourceByTableName("GLAccount").subscribe(function (response) { _this.isReady = true; });
        // salesman lov field filtera
        _this.SalesmanFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.SalesmanFilterItems.addAdditionalFilter("IsSalesman", true, null, null, "Equals", false, false, false, "boolean", false, false);
        // set default value for no of months
        var newDate = new Date();
        var currentMonth = newDate.getMonth() + 1;
        //this.NumberOfMonths = currentMonth - 6; // 6 backward
        _this.NumberOfMonths = 6; // 6 backward
        return _this;
    }
    AgingFilterComponent.prototype.ngOnInit = function () {
        this.SetUIProperties();
    };
    AgingFilterComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetRequired("AgingForDate", "GLAccount", true);
        //this.UIProperties.SetRequired("Customer", "GLAccount", true);
        //this.UIProperties.SetRequired("NumberOfMonths", "GLAccount", true);
    };
    Object.defineProperty(AgingFilterComponent.prototype, "AgingForDate", {
        get: function () { return this.agingForDate; },
        set: function (value) {
            if (this.agingForDate != value) {
                this.agingForDate = value;
                this.ValidationErrorsList = [];
                this.ValidateDate();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgingFilterComponent.prototype, "Customer", {
        get: function () { return this.customer; },
        set: function (value) {
            if (this.customer != value) {
                this.customer = value;
                if (value)
                    this.IsCategoryDisabled = true;
                else
                    this.IsCategoryDisabled = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    AgingFilterComponent.prototype.ValidateDate = function () {
        if (this.AgingForDate) {
            var newDate = new Date();
            var currentDate = new Date(newDate.getFullYear(), newDate.getMonth(), newDate.getDate() + 1, 0, 0, 0); // +1 is to include today date to allowed values
            if (this.AgingForDate > currentDate) {
                this.UIProperties.SetValidity("AgingForDate", "GLAccount", false, TextCodeTranslator_1.TextCodeTranslator.Translate("AgingReport.O.FutureDate"));
                return false;
            }
            else {
                this.UIProperties.SetValidity("AgingForDate", "GLAccount", true, "valid");
                this.UIProperties.SetRequired("AgingForDate", "GLAccount", false);
                return true;
            }
        }
        return true;
    };
    Object.defineProperty(AgingFilterComponent.prototype, "NumberOfMonths", {
        get: function () { return this.numberOfMonths; },
        set: function (value) {
            if (this.numberOfMonths != value) {
                this.numberOfMonths = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgingFilterComponent.prototype, "Collector", {
        get: function () { return this.collector; },
        set: function (value) {
            if (this.collector != value) {
                this.collector = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgingFilterComponent.prototype, "Salesman", {
        get: function () { return this.salesman; },
        set: function (value) {
            if (this.salesman != value) {
                this.salesman = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgingFilterComponent.prototype, "Category1", {
        get: function () { return this.category1; },
        set: function (value) {
            if (this.category1 != value) {
                this.category1 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgingFilterComponent.prototype, "Category2", {
        get: function () { return this.category2; },
        set: function (value) {
            if (this.category2 != value) {
                this.category2 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgingFilterComponent.prototype, "Category3", {
        get: function () { return this.category3; },
        set: function (value) {
            if (this.category3 != value) {
                this.category3 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgingFilterComponent.prototype, "Category4", {
        get: function () { return this.category4; },
        set: function (value) {
            if (this.category4 != value) {
                this.category4 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgingFilterComponent.prototype, "Category5", {
        get: function () { return this.category5; },
        set: function (value) {
            if (this.category5 != value) {
                this.category5 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgingFilterComponent.prototype, "CurrenciesDetailed", {
        get: function () { return this.currenciesDetailed; },
        set: function (value) {
            if (this.currenciesDetailed != value) {
                this.currenciesDetailed = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    AgingFilterComponent.prototype.RunButtonClicked = function () {
        this.SetUIProperties();
        var errors = [];
        var categoryValue = null;
        var categoryIndex = null;
        this.ValidationErrorsList = [];
        //#region requierd fields
        if (!this.AgingForDate) {
            errors.push("Aging for date field is requierd");
        }
        //if (!this.Customer) { errors.push("Customer field is requierd"); }
        if (!this.NumberOfMonths) {
            errors.push("Number of months field is requierd");
        }
        //#endregion
        //#region Date validation
        var isDateValid = this.ValidateDate();
        if (!isDateValid)
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("AgingReport.O.FutureDate"));
        //#endregion
        if (errors.length == 0) {
            // Selecting category
            if (this.SelectedCategory) {
                categoryIndex = this.SelectedCategory.replace(' ', ''); // remove space from selected category
                if (categoryIndex)
                    categoryValue = this.DataContext[categoryIndex]; // select the value from the context
            }
            var myFilterItems = [];
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("AgingForDate", this.AgingForDate, "Date"));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("CustomerId", this.Customer ? this.Customer : null));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("NumberOfMonths", this.NumberOfMonths, "Number"));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("CollectorId", this.Collector));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("SalesmanId", this.Salesman));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("Detailed", this.CurrenciesDetailed));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("CategoryIndex", categoryIndex)); // 'Category1' , 'Category2' , ...
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("CategoryValue", categoryValue));
            var myReportFliter = new ReportFliter_1.ReportFliter();
            myReportFliter.NumberOfPage = 1;
            myReportFliter.ProcessType = "GenerateReport";
            myReportFliter.QueryFilterItemLists = myFilterItems;
            this.RunReportEvent.emit(myReportFliter);
        }
        else {
            this.ValidationErrorsList = errors;
        }
    };
    AgingFilterComponent.prototype.SelectedItemChanged = function (item) {
        this.SelectedCategory = item;
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], AgingFilterComponent.prototype, "RunReportEvent", void 0);
    AgingFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AgingFilterComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AgingFilterComponent);
    return AgingFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.AgingFilterComponent = AgingFilterComponent;
//# sourceMappingURL=AgingFilterComponent.js.map