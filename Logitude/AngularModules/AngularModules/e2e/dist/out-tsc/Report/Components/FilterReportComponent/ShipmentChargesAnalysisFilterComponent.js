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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var core_1 = require("@angular/core");
var CodeNameClass_1 = require("../../../Infrastructure/DataContracts/CodeNameClass");
var Tools_1 = require("../../../Infrastructure/Tools");
var ShipmentChargesAnalysisFilterComponent = /** @class */ (function (_super) {
    __extends(ShipmentChargesAnalysisFilterComponent, _super);
    function ShipmentChargesAnalysisFilterComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Report";
        _this.DataContext = _this;
        _this.ReceivablesFilterEnabled = true;
        _this.PayablesFilterEnabled = true;
        _this.SelectedCurrencyCode = null;
        _this.selectedOperationalCode = "All";
        _this.selectedAccountingCode = "All";
        _this.FilterId_OO = "operational_open";
        _this.FilterId_OC = "operational_close";
        _this.FilterId_AO = "accounting_open";
        _this.FilterId_AC = "accounting_close";
        return _this;
    }
    ShipmentChargesAnalysisFilterComponent.prototype.ngOnInit = function () {
    };
    ShipmentChargesAnalysisFilterComponent.prototype.InitializeComponent = function (myReportsPreview) {
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
        this.BuildFunnelFilters();
        this.FillFiltersList();
        this.SetFiltersEnabled();
    };
    ShipmentChargesAnalysisFilterComponent.prototype.DaysInMonth = function (aDate) {
        return (new Date(aDate.getFullYear(), aDate.getMonth(), 0)).getDate();
    };
    ShipmentChargesAnalysisFilterComponent.prototype.SetDate = function (year, month, day) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    };
    ShipmentChargesAnalysisFilterComponent.prototype.BuildFunnelFilters = function () {
        this.DateFilterList = [];
        this.DateFilterList.push(new CodeNameClass_1.CodeNameClass("CRT", "Create Date"));
        this.DateFilterList.push(new CodeNameClass_1.CodeNameClass("ARR", "Actual Arrival Date"));
        this.DateFilterList.push(new CodeNameClass_1.CodeNameClass("DEP", "Actual Departure Date"));
        this.DateFilterList.push(new CodeNameClass_1.CodeNameClass("OPE", "Operational Date"));
        this.selectedDateFilter = this.DateFilterList.filter(function (d) { return d.Code == "CRT"; })[0];
    };
    Object.defineProperty(ShipmentChargesAnalysisFilterComponent.prototype, "SelectedDateFilter", {
        get: function () { return this.selectedDateFilter; },
        set: function (value) {
            if (this.selectedDateFilter != value) {
                this.selectedDateFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentChargesAnalysisFilterComponent.prototype.FillFiltersList = function () {
        this.FiltersComboList = [];
        this.FiltersComboList.push(new CodeNameClass_1.CodeNameClass("NOFI", "No Filter"));
        this.FiltersComboList.push(new CodeNameClass_1.CodeNameClass("OPEN", "Open Amount"));
        this.FiltersComboList.push(new CodeNameClass_1.CodeNameClass("ACCT", "Accounted Amount"));
        this.FiltersComboList.push(new CodeNameClass_1.CodeNameClass("OPAT", "Open or Accounted"));
        this.FiltersComboList.push(new CodeNameClass_1.CodeNameClass("MISS", "Missing"));
        this.receivablesFilterSelectedItem = this.FiltersComboList.filter(function (d) { return d.Code == "NOFI"; })[0];
        this.payablesFilterSelectedItem = this.FiltersComboList.filter(function (d) { return d.Code == "NOFI"; })[0];
    };
    ShipmentChargesAnalysisFilterComponent.prototype.SetFiltersEnabled = function () {
        var receivablesIsEnabled = true;
        var payablesIsEnabled = true;
        if (Tools_1.AppTool.IsNullOrEmpty(this.ChargesTypeId)) {
            receivablesIsEnabled = false;
            payablesIsEnabled = false;
        }
        else {
            if (this.PayablesFilterSelectedItem != null) {
                if (this.PayablesFilterSelectedItem.Code == "MISS") {
                    receivablesIsEnabled = false;
                }
            }
            if (this.ReceivablesFilterSelectedItem != null) {
                if (this.ReceivablesFilterSelectedItem.Code == "MISS") {
                    payablesIsEnabled = false;
                }
            }
        }
        this.ReceivablesFilterEnabled = receivablesIsEnabled;
        this.PayablesFilterEnabled = payablesIsEnabled;
    };
    ShipmentChargesAnalysisFilterComponent.prototype.RefereshFiltersValues = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.ChargesTypeId)) {
            this.ReceivablesFilterSelectedItem = this.FiltersComboList.filter(function (d) { return d.Code == "NOFI"; })[0];
            this.PayablesFilterSelectedItem = this.FiltersComboList.filter(function (d) { return d.Code == "NOFI"; })[0];
        }
        else {
            if (this.ReceivablesFilterSelectedItem != null) {
                if (this.ReceivablesFilterSelectedItem.Code == "MISS") {
                    this.PayablesFilterSelectedItem = this.FiltersComboList.filter(function (d) { return d.Code == "NOFI"; })[0];
                }
            }
            else if (this.PayablesFilterSelectedItem != null) {
                if (this.PayablesFilterSelectedItem.Code == "MISS") {
                    this.ReceivablesFilterSelectedItem = this.FiltersComboList.filter(function (d) { return d.Code == "NOFI"; })[0];
                }
            }
        }
    };
    Object.defineProperty(ShipmentChargesAnalysisFilterComponent.prototype, "ReceivablesFilterSelectedItem", {
        get: function () { return this.receivablesFilterSelectedItem; },
        set: function (value) {
            if (this.receivablesFilterSelectedItem != value) {
                this.receivablesFilterSelectedItem = value;
                this.SetFiltersEnabled();
                this.RefereshFiltersValues();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentChargesAnalysisFilterComponent.prototype, "PayablesFilterSelectedItem", {
        get: function () { return this.payablesFilterSelectedItem; },
        set: function (value) {
            if (this.payablesFilterSelectedItem != value) {
                this.payablesFilterSelectedItem = value;
                this.SetFiltersEnabled();
                this.RefereshFiltersValues();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentChargesAnalysisFilterComponent.prototype, "ChargesTypeId", {
        get: function () { return this.chargesTypeId; },
        set: function (value) {
            if (this.chargesTypeId != value) {
                this.chargesTypeId = value;
                this.SetFiltersEnabled();
                this.RefereshFiltersValues();
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentChargesAnalysisFilterComponent.prototype.OnSelectCurrency = function (myCurrencyCode) {
        this.SelectedCurrencyCode = myCurrencyCode;
    };
    Object.defineProperty(ShipmentChargesAnalysisFilterComponent.prototype, "SelectedOperationalCode", {
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
    ShipmentChargesAnalysisFilterComponent.prototype.OnOperationalClicked = function (myCode) {
        this.SelectedOperationalCode = myCode;
    };
    Object.defineProperty(ShipmentChargesAnalysisFilterComponent.prototype, "SelectedAccountingCode", {
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
    ShipmentChargesAnalysisFilterComponent.prototype.OnAccountingClicked = function (myCode) {
        this.SelectedAccountingCode = myCode;
    };
    ShipmentChargesAnalysisFilterComponent.prototype.RunReport = function (isloading) {
        this.ValidationErrorsList = [];
        if (!this.SelectedDateFilter) {
            this.ValidationErrorsList.push("Date field is required");
        }
        if (!this.ReceivablesFilterSelectedItem) {
            this.ValidationErrorsList.push("Receivables field is required");
        }
        if (!this.PayablesFilterSelectedItem) {
            this.ValidationErrorsList.push("Payables field is required");
        }
        if (this.FromDate != null && this.ToDate != null) {
            if (this.ToDate < this.FromDate) {
                this.ValidationErrorsList.push("From date must be less than to date");
            }
            else {
                var total = 0;
                if (((this.ToDate.valueOf() - this.FromDate.valueOf()) / (1000 * 60 * 60 * 24)) > 365)
                    this.ValidationErrorsList.push("Dates should be within one year");
            }
        }
        else {
            if (this.FromDate == null) {
                this.ValidationErrorsList.push("From Date is required");
            }
            if (this.ToDate == null) {
                this.ValidationErrorsList.push("To Date is required");
            }
        }
        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array();
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CustomerId";
            this.queryFilterItem.FieldValue = this.CustomerId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ChargesTypeId";
            this.queryFilterItem.FieldValue = this.ChargesTypeId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "DateType";
            this.queryFilterItem.FieldValue = this.SelectedDateFilter.Code;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            var IsProfitCurrency = false;
            if (this.SelectedCurrencyCode == this.ProfitCurrencyCode) {
                IsProfitCurrency = true;
            }
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "IsProfitCurrecny";
            this.queryFilterItem.FieldValue = IsProfitCurrency;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CurrencyCode";
            this.queryFilterItem.FieldValue = this.SelectedCurrencyCode;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "FromDate";
            this.queryFilterItem.FieldValue = this.FromDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ToDate";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "OperationalType";
            this.queryFilterItem.FieldValue = this.SelectedOperationalCode;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "AccountingType";
            this.queryFilterItem.FieldValue = this.SelectedAccountingCode;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ReceivablesType";
            this.queryFilterItem.FieldValue = this.ReceivablesFilterSelectedItem.Code;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "PayablesType";
            this.queryFilterItem.FieldValue = this.PayablesFilterSelectedItem.Code;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            var reportFliter = new ReportFliter_1.ReportFliter();
            reportFliter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
            reportFliter.QueryFilterItemLists = this.queryFilterItems;
            reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            reportFliter.NumberOfPage = 1;
            reportFliter.ProcessType = "GenerateReport";
            this.ReportsPreview.CleanPartnersObslist();
            //if (!AppTool.IsNullOrEmpty(this.CustomerId)) this.ReportsPreview.AddPartner("Customer", this.CustomerId);
            this.ReportsPreview.GenerateReport(reportFliter, isloading);
        }
    };
    ShipmentChargesAnalysisFilterComponent.prototype.OperationalMouseOver = function (itemValue) {
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
    ShipmentChargesAnalysisFilterComponent.prototype.OperationalMouseLeave = function (itemValue) {
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
    ShipmentChargesAnalysisFilterComponent.prototype.AccountingMouseOver = function (itemValue) {
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
    ShipmentChargesAnalysisFilterComponent.prototype.AccountingMouseLeave = function (itemValue) {
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
    ShipmentChargesAnalysisFilterComponent.prototype.ApplySelectedStyle = function (mode) {
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
    ShipmentChargesAnalysisFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'ShipmentChargesAnalysisFilterComponent',
            templateUrl: './ShipmentChargesAnalysisFilterComponent.html',
            inputs: ['ReportsPreview']
        }),
        __metadata("design:paramtypes", [])
    ], ShipmentChargesAnalysisFilterComponent);
    return ShipmentChargesAnalysisFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.ShipmentChargesAnalysisFilterComponent = ShipmentChargesAnalysisFilterComponent;
//# sourceMappingURL=ShipmentChargesAnalysisFilterComponent.js.map