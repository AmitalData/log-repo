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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ReportFliter_1 = require("../../../Components/Filters/ReportFliter");
var QueryFilterItem_1 = require("../../../Components/Filters/QueryFilterItem");
var CodeNameClass_1 = require("../../../../Infrastructure/DataContracts/CodeNameClass");
var VendorChargesAnalysisFilterComponent = /** @class */ (function (_super) {
    __extends(VendorChargesAnalysisFilterComponent, _super);
    function VendorChargesAnalysisFilterComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.RunReportEvent = new core_1.EventEmitter();
        _this.IncludeAccountedOnly = false;
        _this.mySelectedTransportFilter = "All";
        _this.mySelectedDirectionFilter = "All";
        _this.SelectedCurrencyCode = null;
        _this.selectedOperationalCode = "All";
        _this.selectedAccountingCode = "All";
        _this.FilterId_OO = "operational_open";
        _this.FilterId_OC = "operational_close";
        _this.FilterId_AO = "accounting_open";
        _this.FilterId_AC = "accounting_close";
        return _this;
    }
    VendorChargesAnalysisFilterComponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.ReportsPreview = myReportsPreview;
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.DaysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month - 1, daysofmonth);
        this.ProfitCurrencyCode = SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyCode;
        this.LocalCurrencyCode = SessionLocator_1.SessionLocator.TenantPM.AccountingCurrencyCode;
        this.SelectedCurrencyCode = this.LocalCurrencyCode;
        this.SelectedOperationalCode = "All";
        this.SelectedAccountingCode = "All";
        this.BuildDateFilter();
    };
    VendorChargesAnalysisFilterComponent.prototype.DaysInMonth = function (aDate) {
        return (new Date(aDate.getFullYear(), aDate.getMonth(), 0)).getDate();
    };
    VendorChargesAnalysisFilterComponent.prototype.SetDate = function (year, month, day) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    };
    VendorChargesAnalysisFilterComponent.prototype.BuildDateFilter = function () {
        this.DateFilterList = [];
        this.DateFilterList.push(new CodeNameClass_1.CodeNameClass("CRT", "Create Date"));
        this.DateFilterList.push(new CodeNameClass_1.CodeNameClass("OPE", "Operational Date"));
        this.selectedDateFilter = this.DateFilterList.filter(function (d) { return d.Code == "CRT"; })[0];
    };
    Object.defineProperty(VendorChargesAnalysisFilterComponent.prototype, "SelectedDateFilter", {
        get: function () { return this.selectedDateFilter; },
        set: function (value) {
            if (this.selectedDateFilter != value) {
                this.selectedDateFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VendorChargesAnalysisFilterComponent.prototype, "SelectedTransportFilter", {
        get: function () { return this.mySelectedTransportFilter; },
        set: function (value) {
            if (this.mySelectedTransportFilter != value) {
                this.mySelectedTransportFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VendorChargesAnalysisFilterComponent.prototype, "SelectedDirectionFilter", {
        get: function () { return this.mySelectedDirectionFilter; },
        set: function (value) {
            if (this.mySelectedDirectionFilter != value) {
                this.mySelectedDirectionFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    VendorChargesAnalysisFilterComponent.prototype.OnSelectCurrency = function (myCurrencyCode) {
        this.SelectedCurrencyCode = myCurrencyCode;
    };
    Object.defineProperty(VendorChargesAnalysisFilterComponent.prototype, "SelectedOperationalCode", {
        get: function () { return this.selectedOperationalCode; },
        set: function (value) {
            if (this.selectedOperationalCode != value) {
                this.selectedOperationalCode = value;
                this.ApplySelectedStyle("OPE");
            }
        },
        enumerable: true,
        configurable: true
    });
    VendorChargesAnalysisFilterComponent.prototype.OnOperationalClicked = function (myCode) {
        this.SelectedOperationalCode = myCode;
    };
    Object.defineProperty(VendorChargesAnalysisFilterComponent.prototype, "SelectedAccountingCode", {
        get: function () { return this.selectedAccountingCode; },
        set: function (value) {
            if (this.selectedAccountingCode != value) {
                this.selectedAccountingCode = value;
                this.ApplySelectedStyle("ACC");
            }
        },
        enumerable: true,
        configurable: true
    });
    VendorChargesAnalysisFilterComponent.prototype.OnAccountingClicked = function (myCode) {
        this.SelectedAccountingCode = myCode;
    };
    VendorChargesAnalysisFilterComponent.prototype.OperationalMouseOver = function (itemValue) {
        if (this.SelectedOperationalCode != itemValue) {
            var img_O = document.getElementById(this.FilterId_OO);
            var img_C = document.getElementById(this.FilterId_OC);
            switch (itemValue) {
                case "All": {
                    break;
                }
                case "Open": {
                    img_O.setAttribute("src", "./Images/Icons/opened_h.png");
                    break;
                }
                case "Close": {
                    img_C.setAttribute("src", "./Images/Icons/closed_h.png");
                    break;
                }
            }
        }
    };
    VendorChargesAnalysisFilterComponent.prototype.OperationalMouseLeave = function (itemValue) {
        if (this.SelectedOperationalCode != itemValue) {
            var img_O = document.getElementById(this.FilterId_OO);
            var img_C = document.getElementById(this.FilterId_OC);
            switch (itemValue) {
                case "All": {
                    break;
                }
                case "Open": {
                    img_O.setAttribute("src", "./Images/Icons/opened_n.png");
                    break;
                }
                case "Close": {
                    img_C.setAttribute("src", "./Images/Icons/closed_n.png");
                    break;
                }
            }
        }
    };
    VendorChargesAnalysisFilterComponent.prototype.AccountingMouseOver = function (itemValue) {
        if (this.SelectedAccountingCode != itemValue) {
            var img_O = document.getElementById(this.FilterId_AO);
            var img_C = document.getElementById(this.FilterId_AC);
            switch (itemValue) {
                case "All": {
                    break;
                }
                case "Open": {
                    img_O.setAttribute("src", "./Images/Icons/opened_h.png");
                    break;
                }
                case "Close": {
                    img_C.setAttribute("src", "./Images/Icons/closed_h.png");
                    break;
                }
            }
        }
    };
    VendorChargesAnalysisFilterComponent.prototype.AccountingMouseLeave = function (itemValue) {
        if (this.SelectedAccountingCode != itemValue) {
            var img_O = document.getElementById(this.FilterId_AO);
            var img_C = document.getElementById(this.FilterId_AC);
            switch (itemValue) {
                case "All": {
                    break;
                }
                case "Open": {
                    img_O.setAttribute("src", "./Images/Icons/opened_n.png");
                    break;
                }
                case "Close": {
                    img_C.setAttribute("src", "./Images/Icons/closed_n.png");
                    break;
                }
            }
        }
    };
    VendorChargesAnalysisFilterComponent.prototype.ApplySelectedStyle = function (mode) {
        var img_O;
        var img_C;
        switch (mode) {
            case "OPE": {
                img_O = document.getElementById(this.FilterId_OO);
                img_C = document.getElementById(this.FilterId_OC);
                img_O.setAttribute("src", "./Images/Icons/opened_n.png");
                img_C.setAttribute("src", "./Images/Icons/closed_n.png");
                switch (this.SelectedOperationalCode) {
                    case "All": {
                        break;
                    }
                    case "Open": {
                        img_O.setAttribute("src", "./Images/Icons/opened_s.png");
                        break;
                    }
                    case "Close": {
                        img_C.setAttribute("src", "./Images/Icons/closed_s.png");
                        break;
                    }
                }
                break;
            }
            case "ACC": {
                img_O = document.getElementById(this.FilterId_AO);
                img_C = document.getElementById(this.FilterId_AC);
                img_O.setAttribute("src", "./Images/Icons/opened_n.png");
                img_C.setAttribute("src", "./Images/Icons/closed_n.png");
                switch (this.SelectedAccountingCode) {
                    case "All": {
                        break;
                    }
                    case "Open": {
                        img_O.setAttribute("src", "./Images/Icons/opened_s.png");
                        break;
                    }
                    case "Close": {
                        img_C.setAttribute("src", "./Images/Icons/closed_s.png");
                        break;
                    }
                }
                break;
            }
        }
    };
    VendorChargesAnalysisFilterComponent.prototype.RunButtonClicked = function () {
        var errors = [];
        if (!this.SelectedDateFilter) {
            errors.push("Date field is required");
        }
        if (this.FromDate != null && this.ToDate != null) {
            if (this.ToDate < this.FromDate) {
                errors.push("From date must be less than to date");
            }
        }
        else {
            if (this.FromDate == null) {
                errors.push("From Date is required");
            }
            if (this.ToDate == null) {
                errors.push("To Date is required");
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            var myFilterItems = [];
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("VendorId", this.VendorId));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("DateType", this.SelectedDateFilter.Code));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("FromDate", this.FromDate));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("ToDate", this.ToDate));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("ChargesTypeId", this.ChargesTypeId));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("IncludeAccountedOnly", this.IncludeAccountedOnly));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("IsLocalCurrency", this.SelectedCurrencyCode == this.LocalCurrencyCode ? true : false));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("SelectedCurrencyCode", this.SelectedCurrencyCode));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("OperationalType", this.SelectedOperationalCode));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("AccountingType", this.SelectedAccountingCode));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("Direction", this.SelectedDirectionFilter));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("TransportMode", this.SelectedTransportFilter));
            var myReportFliter = new ReportFliter_1.ReportFliter();
            myReportFliter.NumberOfPage = 1;
            myReportFliter.ProcessType = "GenerateReport";
            myReportFliter.QueryFilterItemLists = myFilterItems;
            this.RunReportEvent.emit(myReportFliter);
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], VendorChargesAnalysisFilterComponent.prototype, "RunReportEvent", void 0);
    VendorChargesAnalysisFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './VendorChargesAnalysisFilterComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], VendorChargesAnalysisFilterComponent);
    return VendorChargesAnalysisFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.VendorChargesAnalysisFilterComponent = VendorChargesAnalysisFilterComponent;
//# sourceMappingURL=VendorChargesAnalysisFilterComponent.js.map