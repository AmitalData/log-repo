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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var ReportFliter_1 = require("../../Components/Filters/ReportFliter");
var QueryFilterItem_1 = require("../../Components/Filters/QueryFilterItem");
var core_1 = require("@angular/core");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var CodeNameClass_1 = require("../../../Infrastructure/DataContracts/CodeNameClass");
var TrailBalanceFiltersComponent = /** @class */ (function (_super) {
    __extends(TrailBalanceFiltersComponent, _super);
    function TrailBalanceFiltersComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "GLAccount";
        _this.ValidationErrorsList = [];
        _this.IsDisplayOnly = false;
        _this.showLocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
        _this.level = "ChartOfAccountType";
        _this.FilterSelectedValue = 'ChartOfAccountType';
        //#region Category fields
        _this.IsCategoryDisabled = false;
        _this.CategoriesList = [
            'Category 1',
            'Category 2',
            'Category 3',
            'Category 4',
            'Category 5'
        ];
        _this.chartofaccounttypeHtmlinputId = Guid_1.Guid.newGuid();
        _this.GLAccountHtmlinputId = Guid_1.Guid.newGuid();
        _this.ChartofaccountHtmlinputId = Guid_1.Guid.newGuid();
        if (_this.showLocal) {
            _this.Name = "LocalName";
        }
        else {
            _this.Name = "Name";
        }
        _this.VendorDetailedControlList = [];
        _this.VendorDetailedControlList.push(new CodeNameClass_1.CodeNameClass("1", "Show", "הצג פירוט"));
        _this.VendorDetailedControlList.push(new CodeNameClass_1.CodeNameClass("2", "Dont show", "ללא פירוט"));
        _this.VendorDetailedControlFilter = _this.VendorDetailedControlList[1];
        _this.CustomerDetailedControlList = [];
        _this.CustomerDetailedControlList.push(new CodeNameClass_1.CodeNameClass("1", "Show", "הצג פירוט"));
        _this.CustomerDetailedControlList.push(new CodeNameClass_1.CodeNameClass("2", "Dont show", "ללא פירוט"));
        _this.CustomerDetailedControlFilter = _this.CustomerDetailedControlList[1];
        return _this;
    }
    Object.defineProperty(TrailBalanceFiltersComponent.prototype, "VendorDetailedControlFilter", {
        get: function () { return this.vendorDetailedControlFilter; },
        set: function (value) {
            if (this.vendorDetailedControlFilter != value) {
                this.vendorDetailedControlFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrailBalanceFiltersComponent.prototype, "CustomerDetailedControlFilter", {
        get: function () { return this.customerDetailedControlFilter; },
        set: function (value) {
            if (this.customerDetailedControlFilter != value) {
                this.customerDetailedControlFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    TrailBalanceFiltersComponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.ReportsPreview = myReportsPreview;
        //this.BuildFilterList();
    };
    Object.defineProperty(TrailBalanceFiltersComponent.prototype, "FromDate", {
        get: function () { return this.fromDate; },
        set: function (value) {
            if (this.fromDate != value) {
                this.fromDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrailBalanceFiltersComponent.prototype, "Level", {
        get: function () { return this.level; },
        set: function (value) {
            if (this.level != value) {
                this.level = value;
                if (this.level == "ChartOfAccount" || this.level == "ChartOfAccountType") {
                    this.UIProperties.SetEnabled("ChartOfAccountId", this.ObjectTableName, false);
                    this.UIProperties.SetEnabled("Category1", this.ObjectTableName, false);
                    this.UIProperties.SetEnabled("Category2", this.ObjectTableName, false);
                    this.UIProperties.SetEnabled("Category3", this.ObjectTableName, false);
                    this.UIProperties.SetEnabled("Category4", this.ObjectTableName, false);
                    this.UIProperties.SetEnabled("Category5", this.ObjectTableName, false);
                    this.UIProperties.SetEnabled("Category4", this.ObjectTableName, false);
                    this.IsDisplayOnly = true;
                    this.IsCategoryDisabled = true;
                }
                else {
                    this.UIProperties.SetEnabled("ChartOfAccountId", this.ObjectTableName, true);
                    this.UIProperties.SetEnabled("Category1", this.ObjectTableName, true);
                    this.UIProperties.SetEnabled("Category2", this.ObjectTableName, true);
                    this.UIProperties.SetEnabled("Category3", this.ObjectTableName, true);
                    this.UIProperties.SetEnabled("Category4", this.ObjectTableName, true);
                    this.UIProperties.SetEnabled("Category5", this.ObjectTableName, true);
                    this.UIProperties.SetEnabled("Category4", this.ObjectTableName, true);
                    this.IsDisplayOnly = false;
                    this.IsCategoryDisabled = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrailBalanceFiltersComponent.prototype, "ToDate", {
        get: function () { return this.toDate; },
        set: function (value) {
            if (this.toDate != value) {
                this.toDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrailBalanceFiltersComponent.prototype, "UseBalanceFilter", {
        get: function () { return this.useBalanceFilter; },
        set: function (value) {
            if (this.useBalanceFilter != value) {
                this.useBalanceFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrailBalanceFiltersComponent.prototype, "UseCustomerFilter", {
        get: function () { return this.useCustomerFilter; },
        set: function (value) {
            if (this.useCustomerFilter != value) {
                this.useCustomerFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrailBalanceFiltersComponent.prototype, "UseVendorFilter", {
        get: function () { return this.useVendorFilter; },
        set: function (value) {
            if (this.useVendorFilter != value) {
                this.useVendorFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrailBalanceFiltersComponent.prototype, "UseZeroBalanceFilter", {
        get: function () { return this.useZeroBalanceFilter; },
        set: function (value) {
            if (this.useZeroBalanceFilter != value) {
                this.useZeroBalanceFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrailBalanceFiltersComponent.prototype, "CurrencyFilter", {
        get: function () { return this.currencyFilter; },
        set: function (value) {
            if (this.currencyFilter != value) {
                this.currencyFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrailBalanceFiltersComponent.prototype, "Category1", {
        get: function () { return this.category1; },
        set: function (value) {
            if (this.category1 != value) {
                this.category1 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrailBalanceFiltersComponent.prototype, "Category2", {
        get: function () { return this.category2; },
        set: function (value) {
            if (this.category2 != value) {
                this.category2 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrailBalanceFiltersComponent.prototype, "Category3", {
        get: function () { return this.category3; },
        set: function (value) {
            if (this.category3 != value) {
                this.category3 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrailBalanceFiltersComponent.prototype, "Category4", {
        get: function () { return this.category4; },
        set: function (value) {
            if (this.category4 != value) {
                this.category4 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrailBalanceFiltersComponent.prototype, "Category5", {
        get: function () { return this.category5; },
        set: function (value) {
            if (this.category5 != value) {
                this.category5 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrailBalanceFiltersComponent.prototype, "ChartOfAccountId", {
        get: function () { return this.chartOfAccountId; },
        set: function (value) {
            if (this.chartOfAccountId != value) {
                this.chartOfAccountId = value;
                if (value != null) {
                    this.IsCategoryDisabled = true;
                }
                else {
                    this.IsCategoryDisabled = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    TrailBalanceFiltersComponent.prototype.FilterItemClicked = function (itemValue) {
        if (this.FilterSelectedValue != itemValue) {
            this.FilterSelectedValue = itemValue;
            this.Level = itemValue;
        }
    };
    TrailBalanceFiltersComponent.prototype.SelectedItemChanged = function (item) {
        this.SelectedCategory = item;
    };
    //#endregion
    TrailBalanceFiltersComponent.prototype.RunReport = function () {
        this.ValidationErrorsList = [];
        if (this.ToDate == null) {
            var FIELD_IS_REQUIERD = null;
            FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            var s = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.ToDate"));
            this.ValidationErrorsList.push(s);
        }
        if (this.FromDate == null) {
            var FIELD_IS_REQUIERD = null;
            FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            var s = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.FromDate"));
            this.ValidationErrorsList.push(s);
        }
        if (this.ToDate < this.FromDate) {
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.ToDateMustGreaterFromDate"));
        }
        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array();
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ToDate";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "LessThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);
            if (!this.Level)
                this.Level = "ChartOfAccountType";
            this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("Level", this.Level));
            //if (!this.UseBalanceFilter && this.SelectedBalanceOptionFilter.Code == "WITHOUT") {
            //    this.queryFilterItems.push(new QueryFilterItem("CardFilter", "0"));
            //    this.queryFilterItems.push(new QueryFilterItem("CardFilter", "0"));
            //}
            //else if (this.UseBalanceFilter && this.SelectedBalanceOptionFilter.Code == "WITHOUT") {
            //    this.queryFilterItems.push(new QueryFilterItem("CardFilter", "2"));
            //}
            //else if (this.UseBalanceFilter && this.SelectedBalanceOptionFilter.Code == "WITH") {
            //    this.queryFilterItems.push(new QueryFilterItem("CardFilter", "1"));
            //}
            this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("FromDate", this.FromDate, "Date"));
            this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("Category1", this.Category1, "String"));
            this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("Category2", this.Category2, "String"));
            this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("Category3", this.Category3, "String"));
            this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("Category4", this.Category4, "String"));
            this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("Category5", this.Category5, "String"));
            this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("CurrencyDetailed", this.CurrencyFilter, "boolean"));
            if (this.CustomerDetailedControlFilter != null && this.CustomerDetailedControlFilter.Code == "1") {
                this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("Customer", true, "boolean"));
            }
            if (this.VendorDetailedControlFilter != null && this.VendorDetailedControlFilter.Code == "1") {
                this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("Vendor", true, "boolean"));
            }
            this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("UseBalanceFilter", this.UseBalanceFilter, "boolean"));
            this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("ChartOfAccountId", this.ChartOfAccountId, "String"));
            this.reportFliter = new ReportFliter_1.ReportFliter();
            //this.reportFliter.Level = this.Level;
            this.reportFliter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";
            this.ReportsPreview.CleanPartnersObslist();
            this.ReportsPreview.GenerateReport(this.reportFliter, true);
        }
    };
    TrailBalanceFiltersComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'TrailBalanceFiltersComponent',
            templateUrl: './TrailBalanceFiltersComponent.html',
            inputs: ['ReportsPreview']
        }),
        __metadata("design:paramtypes", [])
    ], TrailBalanceFiltersComponent);
    return TrailBalanceFiltersComponent;
}(BaseComponent_1.BaseComponent));
exports.TrailBalanceFiltersComponent = TrailBalanceFiltersComponent;
//# sourceMappingURL=TrailBalanceFiltersComponent.js.map