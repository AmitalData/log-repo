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
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var LedgerTransactionsFilterControl = /** @class */ (function (_super) {
    __extends(LedgerTransactionsFilterControl, _super);
    function LedgerTransactionsFilterControl(CD) {
        var _this = _super.call(this) || this;
        _this.CD = CD;
        _this.ObjectTableName = "LedgerTransaction";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.RunReportEvent = new core_1.EventEmitter();
        _this.isReady = false;
        _this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.isRTL = false;
        //#region Filters
        //row 1
        _this.agingForDate = null;
        //#endregion
        //#region Filter Methods
        _this.filterSelectedValue = 'filter_accounting';
        _this._dateTypeCode = '1';
        //#region Category fields
        _this.IsCategoryDisabled = false;
        _this.CategoriesList = [
            'Category 1',
            'Category 2',
            'Category 3',
            'Category 4',
            'Category 5'
        ];
        _this.attachedGLAccountCheckBox = false;
        _this.splittedByCurrencyCheckBox = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        // get requierd resources
        _this.entityResourceService.getEntityResourceByTableName("GLAccount").subscribe(function (response) {
            _this.entityResourceService.getEntityResourceByTableName("LedgerTransaction").subscribe(function (response) { _this.isReady = true; });
        });
        return _this;
    }
    LedgerTransactionsFilterControl.prototype.ngOnInit = function () {
        this.SetUIProperties();
        //#region Fill Date Default Values
        var today = new Date();
        this.ToDate = new Date();
        var lastmonth = today.setMonth(today.getMonth() - 1);
        this.FromDate = new Date(lastmonth);
        //#endregion
    };
    LedgerTransactionsFilterControl.prototype.SetUIProperties = function () {
        // this.UIProperties.SetRequired("AgingForDate", "GLAccount", true);
        this.UIProperties.SetRequired("GLAccountId", this.ObjectTableName, !this.GLAccountId);
        this.UIProperties.SetRequired("FromDate", this.ObjectTableName, !this.FromDate);
        this.UIProperties.SetRequired("ToDate", this.ObjectTableName, !this.ToDate);
        this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
    };
    Object.defineProperty(LedgerTransactionsFilterControl.prototype, "AgingForDate", {
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
    Object.defineProperty(LedgerTransactionsFilterControl.prototype, "Customer", {
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
    LedgerTransactionsFilterControl.prototype.ValidateDate = function () {
        var _this = this;
        if (this.FromDate > this.ToDate) {
            setTimeout(function () {
                _this.UIProperties.SetValidity("ToDate", _this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.ToDateMustGreaterFromDate"));
                _this.UIProperties.SetValidity("FromDate", _this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
                _this.CD.detectChanges();
            }, 200);
        }
        else {
            setTimeout(function () {
                _this.UIProperties.SetValidity("ToDate", _this.ObjectTableName, true, "");
                _this.UIProperties.SetValidity("FromDate", _this.ObjectTableName, true, "");
                _this.CD.detectChanges();
            }, 200);
        }
    };
    Object.defineProperty(LedgerTransactionsFilterControl.prototype, "NumberOfMonths", {
        get: function () { return this.numberOfMonths; },
        set: function (value) {
            if (this.numberOfMonths != value) {
                this.numberOfMonths = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LedgerTransactionsFilterControl.prototype, "Collector", {
        get: function () { return this.collector; },
        set: function (value) {
            if (this.collector != value) {
                this.collector = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LedgerTransactionsFilterControl.prototype, "Salesman", {
        get: function () { return this.salesman; },
        set: function (value) {
            if (this.salesman != value) {
                this.salesman = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LedgerTransactionsFilterControl.prototype, "Category1", {
        get: function () { return this.category1; },
        set: function (value) {
            if (this.category1 != value) {
                this.category1 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LedgerTransactionsFilterControl.prototype, "Category2", {
        get: function () { return this.category2; },
        set: function (value) {
            if (this.category2 != value) {
                this.category2 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LedgerTransactionsFilterControl.prototype, "Category3", {
        get: function () { return this.category3; },
        set: function (value) {
            if (this.category3 != value) {
                this.category3 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LedgerTransactionsFilterControl.prototype, "Category4", {
        get: function () { return this.category4; },
        set: function (value) {
            if (this.category4 != value) {
                this.category4 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LedgerTransactionsFilterControl.prototype, "Category5", {
        get: function () { return this.category5; },
        set: function (value) {
            if (this.category5 != value) {
                this.category5 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LedgerTransactionsFilterControl.prototype, "CurrenciesDetailed", {
        get: function () { return this.currenciesDetailed; },
        set: function (value) {
            if (this.currenciesDetailed != value) {
                this.currenciesDetailed = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    LedgerTransactionsFilterControl.prototype.FilterItemClicked = function (itemValue) {
        if (this.filterSelectedValue != itemValue) {
            this.filterSelectedValue = itemValue;
            this.FilterLines();
        }
    };
    LedgerTransactionsFilterControl.prototype.FilterLines = function () {
        //Task 46666: Transaction Tab - date filter new design
        // <DateTypeCode>2</DateTypeCode> 1/2/3
        // Accounting- - code 1- חשבונאי
        // Due - code 2 - לגביה
        // Reference -code-3-  אסמכתא
        switch (this.filterSelectedValue) {
            case 'filter_accounting':
                this._dateTypeCode = '1';
                break;
            case 'filter_due':
                this._dateTypeCode = '2';
                break;
            case 'filter_reference':
                this._dateTypeCode = '3';
                break;
            default:
                break;
        }
    };
    //#endregion
    LedgerTransactionsFilterControl.prototype.RunButtonClicked = function () {
        this.SetUIProperties();
        var errors = [];
        var categoryValue = null;
        var categoryIndex = null;
        this.ValidationErrorsList = [];
        //#region requierd fields
        if (!this.GLAccountId) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("GLTransactionReport.O.GLAccountZrequierd"));
        }
        //#endregion
        //#region Date validation
        if (!this.FromDate)
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.O.fromfieldrequired"));
        if (!this.ToDate)
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.O.tofieldrequired"));
        if (this.FromDate > this.ToDate) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
        }
        //#endregion
        if (errors.length == 0) {
            // // Selecting category
            // if (this.SelectedCategory) {
            //     categoryIndex = this.SelectedCategory.replace(' ', ''); // remove space from selected category
            //     if (categoryIndex)
            //         categoryValue = this.DataContext[categoryIndex]; // select the value from the context
            // }
            var myFilterItems = [];
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("FromDate", this.FromDate ? this.FromDate : null));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("ToDate", this.ToDate ? this.ToDate : null));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("GLAccountId", this.GLAccountId ? this.GLAccountId : null));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("CurrencyId", this.CurrencyId ? this.CurrencyId : null));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("IsReconciled", this.IsReconciled ? this.IsReconciled : null));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("IncludeChildAccounts", this.IncludeChildAccounts ? this.IncludeChildAccounts : null));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("SearchFields", this.SearchFields ? this.SearchFields : null));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("DateTypeCode", this._dateTypeCode ? this._dateTypeCode : null));
            // myFilterItems.push(new QueryFilterItem("CategoryIndex", categoryIndex)); // 'Category1' , 'Category2' , ...
            // myFilterItems.push(new QueryFilterItem("CategoryValue", categoryValue));
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
    LedgerTransactionsFilterControl.prototype.SelectedItemChanged = function (item) {
        this.SelectedCategory = item;
    };
    Object.defineProperty(LedgerTransactionsFilterControl.prototype, "OpenAmountHint", {
        get: function () { return this.openAmountHint; },
        set: function (value) {
            if (this.openAmountHint != value) {
                this.openAmountHint = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LedgerTransactionsFilterControl.prototype, "FromDate", {
        get: function () { return this.fromDate; },
        set: function (value) {
            if (this.fromDate != value) {
                this.fromDate = value;
                this.ValidateDate();
                this.UIProperties.SetRequired("FromDate", this.ObjectTableName, !value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LedgerTransactionsFilterControl.prototype, "ToDate", {
        get: function () { return this.toDate; },
        set: function (value) {
            if (this.toDate != value) {
                this.toDate = value;
                this.ValidateDate();
                this.UIProperties.SetRequired("ToDate", this.ObjectTableName, !value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LedgerTransactionsFilterControl.prototype, "CurrencyId", {
        get: function () { return this.currencyId; },
        set: function (value) {
            if (this.currencyId != value) {
                this.currencyId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LedgerTransactionsFilterControl.prototype, "AttachedGLAccountCheckBox", {
        get: function () { return this.attachedGLAccountCheckBox; },
        set: function (value) {
            if (this.attachedGLAccountCheckBox != value) {
                this.attachedGLAccountCheckBox = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LedgerTransactionsFilterControl.prototype, "SplittedByCurrencyCheckBox", {
        get: function () { return this.splittedByCurrencyCheckBox; },
        set: function (value) {
            if (this.splittedByCurrencyCheckBox != value) {
                this.splittedByCurrencyCheckBox = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LedgerTransactionsFilterControl.prototype, "GLAccountId", {
        get: function () { return this._GLAccountId; },
        set: function (value) {
            if (this._GLAccountId != value) {
                this._GLAccountId = value;
                this.UIProperties.SetRequired("GLAccountId", this.ObjectTableName, !value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LedgerTransactionsFilterControl.prototype, "IsReconciled", {
        get: function () {
            return this._IsReconciled;
        },
        set: function (v) {
            this._IsReconciled = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LedgerTransactionsFilterControl.prototype, "IncludeChildAccounts", {
        get: function () {
            return this._IncludeChildAccounts;
        },
        set: function (v) {
            this._IncludeChildAccounts = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LedgerTransactionsFilterControl.prototype, "SearchFields", {
        get: function () {
            return this._SearchFields;
        },
        set: function (v) {
            this._SearchFields = v;
        },
        enumerable: true,
        configurable: true
    });
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], LedgerTransactionsFilterControl.prototype, "RunReportEvent", void 0);
    LedgerTransactionsFilterControl = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './LedgerTransactionsFilterControl.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], LedgerTransactionsFilterControl);
    return LedgerTransactionsFilterControl;
}(BaseComponent_1.BaseComponent));
exports.LedgerTransactionsFilterControl = LedgerTransactionsFilterControl;
//# sourceMappingURL=LedgerTransactionsFilterControl.js.map