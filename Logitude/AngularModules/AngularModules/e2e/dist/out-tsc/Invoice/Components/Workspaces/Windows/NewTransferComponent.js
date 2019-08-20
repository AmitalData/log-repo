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
var Tools_1 = require("../../../../Infrastructure/Tools");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var AccountingTransferHeaderPM_1 = require("../../../EntityPMs/AccountingTransferHeaderPM");
var AccountingTransferLinePM_1 = require("../../../EntityPMs/AccountingTransferLinePM");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var ARInvoiceListService_1 = require("../../../Services/StandardLists/ARInvoiceListService");
var APInvoiceListService_1 = require("../../../Services/StandardLists/APInvoiceListService");
var ARPaymentListService_1 = require("../../../Services/StandardLists/ARPaymentListService");
var APPaymentListService_1 = require("../../../Services/StandardLists/APPaymentListService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var NewTransferComponent = /** @class */ (function (_super) {
    __extends(NewTransferComponent, _super);
    function NewTransferComponent() {
        var _this = _super.call(this) || this;
        _this.EntityPM = null;
        _this.ObjectTableName = "AccountingTransferHeader";
        _this.DataContext = _this;
        _this.TransferTypeCode = null;
        _this.ValidationErrorsList = [];
        _this.ItemsSource = [];
        _this.SelectedItem = null;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.HeaderLabel_Date = null;
        _this.HeaderLabel_Number = null;
        _this.HeaderLabel_Partner = null;
        _this.HeaderLabel_Status = null;
        _this.HeaderLabel_Amount = null;
        // Filters
        _this.isAllChecked = true;
        _this.searchText = null;
        _this.entityListService = null;
        _this.SelectedCount = 0;
        _this.ExportButtonIsEnabled = false;
        _this.IsFirstTimeLoading = true;
        _this.EntityPM = new AccountingTransferHeaderPM_1.AccountingTransferHeaderPM();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.EntityPM.UserId = SessionLocator_1.SessionLocator.LoggedUserId;
        _this.EntityPM.TransferDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        _this.Listen();
        return _this;
    }
    NewTransferComponent.prototype.Listen = function () {
        var _this = this;
        this.CurrentSession.SessionEvent.subscribe(function (s) {
            if (s == "TransferExportFirstTime") {
                _this.IsFirstTimeLoading = true;
                _this.LoadData();
            }
        });
    };
    NewTransferComponent.prototype.SetWindowArgs = function (transferTypeCode) {
        this.TransferTypeCode = transferTypeCode;
        this.EntityPM.AccountingTransferTypeCode = transferTypeCode;
        this.SetLabels();
        this.LoadData();
    };
    NewTransferComponent.prototype.SetLabels = function () {
        switch (this.TransferTypeCode) {
            case "ARIN": {
                this.HeaderLabel_Date = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.CH.InvoiceDateListLable");
                this.HeaderLabel_Number = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.CH.InvoiceNumberListLable");
                this.HeaderLabel_Partner = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.CH.BillToNameListLable");
                this.HeaderLabel_Status = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.CH.StatusNameRateListLable");
                this.HeaderLabel_Amount = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.CH.AmountInInvoiceCurrencyListLable");
                break;
            }
            case "APIN": {
                this.HeaderLabel_Date = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.CH.InvoiceDateListLable");
                this.HeaderLabel_Number = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.CH.InvoiceNumberListLable");
                this.HeaderLabel_Partner = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.CH.VendorNameListLable");
                this.HeaderLabel_Status = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.CH.StatusNameListLable");
                this.HeaderLabel_Amount = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.CH.AmountInInvoiceCurrencyListLable");
                break;
            }
            case "ARPA": {
                this.HeaderLabel_Date = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.CH.RegisterDateListLable");
                this.HeaderLabel_Number = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.CH.PaymentNoListLable");
                this.HeaderLabel_Partner = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.CH.BillToNameListLable");
                this.HeaderLabel_Status = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.CH.StatusNameListLable");
                this.HeaderLabel_Amount = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.CH.AmountInPaymentCurrencyListLable");
                break;
            }
            case "APPA": {
                this.HeaderLabel_Date = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.CH.RegisterDateListLable");
                this.HeaderLabel_Number = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.CH.PaymentNoListLable");
                this.HeaderLabel_Partner = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.CH.VendorNameListLable");
                this.HeaderLabel_Status = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.CH.StatusNameListLable");
                this.HeaderLabel_Amount = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.CH.AmountInPaymentCurrencyListLable");
                break;
            }
        }
    };
    Object.defineProperty(NewTransferComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (value) {
            if (this.EntityPM.Notes != value) {
                this.EntityPM.Notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTransferComponent.prototype, "IsAllChecked", {
        get: function () { return this.isAllChecked; },
        set: function (value) {
            if (this.isAllChecked != value) {
                this.isAllChecked = value;
                this.ItemsSource.forEach(function (item) {
                    item.IsChecked = value;
                });
                this.OnLinesSelected();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTransferComponent.prototype, "FromDate", {
        get: function () { return this.fromDate; },
        set: function (value) {
            if (this.fromDate != value) {
                this.fromDate = value;
                this.LoadData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTransferComponent.prototype, "ToDate", {
        get: function () { return this.toDate; },
        set: function (value) {
            if (this.toDate != value) {
                this.toDate = value;
                this.LoadData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTransferComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (newValue) {
            if (this.searchText != newValue) {
                this.searchText = newValue;
                this.LoadData();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewTransferComponent.prototype.LoadData = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        switch (this.TransferTypeCode) {
            case "ARIN": {
                filters.addAdditionalFilter("StatusCode", "AD,PP,PD,AC,AR", null, null, "InList", false, true, false, "string");
                filters.addAdditionalFilter("TransferStatusCode", "RD", null, null, "Equals", false, true, false, "string");
                filters.addAdditionalFilter("IsConstituentInvoice", false, null, null, "Equals", false, false, false, "Boolean");
                this.AppendDateFilter(filters, "InvoiceDate");
                if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
                    filters.addAdditionalFilter("SearchReadyInvoices", this.SearchText, null, null, "Equals", true, false, false, "string");
                }
                if (this.entityListService == null) {
                    this.entityListService = new ARInvoiceListService_1.ARInvoiceListService();
                }
                this.entityListService.getByFilters(filters).subscribe(function (myResponse) {
                    _this.BuildItemsSource(myResponse);
                });
                break;
            }
            case "APIN": {
                filters.addAdditionalFilter("StatusCode", "AD,PP,PD", null, null, "InList", false, true, false, "string");
                filters.addAdditionalFilter("TransferStatusCode", "RD", null, null, "Equals", false, true, false, "string");
                this.AppendDateFilter(filters, "InvoiceDate");
                if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
                    filters.addAdditionalFilter("SearchReadyInvoices", this.SearchText, null, null, "Equals", true, false, false, "string");
                }
                if (this.entityListService == null) {
                    this.entityListService = new APInvoiceListService_1.APInvoiceListService();
                }
                this.entityListService.getByFilters(filters).subscribe(function (myResponse) {
                    _this.BuildItemsSource(myResponse);
                });
                break;
            }
            case "ARPA": {
                filters.addAdditionalFilter("StatusCode", "AD,CL", null, null, "InList", false, true, false, "string");
                filters.addAdditionalFilter("TransferStatusCode", "RD", null, null, "Equals", false, true, false, "string");
                this.AppendDateFilter(filters, "RegisterDate");
                if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
                    filters.addAdditionalFilter("SearchReadyPayments", this.SearchText, null, null, "Equals", true, false, false, "string");
                }
                if (this.entityListService == null) {
                    this.entityListService = new ARPaymentListService_1.ARPaymentListService();
                }
                this.entityListService.getByFilters(filters).subscribe(function (myResponse) {
                    _this.BuildItemsSource(myResponse);
                });
                break;
            }
            case "APPA": {
                filters.addAdditionalFilter("StatusCode", "AD,CL", null, null, "InList", false, true, false, "string");
                filters.addAdditionalFilter("TransferStatusCode", "RD", null, null, "Equals", false, true, false, "string");
                this.AppendDateFilter(filters, "RegisterDate");
                if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
                    filters.addAdditionalFilter("SearchReadyPayments", this.SearchText, null, null, "Equals", true, false, false, "string");
                }
                if (this.entityListService == null) {
                    this.entityListService = new APPaymentListService_1.APPaymentListService();
                }
                this.entityListService.getByFilters(filters).subscribe(function (myResponse) {
                    _this.BuildItemsSource(myResponse);
                });
                break;
            }
        }
    };
    NewTransferComponent.prototype.AppendDateFilter = function (filters, myFilterField) {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.FromDate) && !Tools_1.AppTool.IsNullOrEmpty(this.ToDate)) {
            filters.addAdditionalFilter(myFilterField, this.FromDate, this.ToDate, null, "Between", false, true, false, "date");
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(this.FromDate)) {
            filters.addAdditionalFilter(myFilterField, this.FromDate, null, null, "GreaterThanOrEqual", false, true, false, "date");
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(this.ToDate)) {
            filters.addAdditionalFilter(myFilterField, this.ToDate, null, null, "LessThanOrEqual", false, true, false, "date");
        }
    };
    NewTransferComponent.prototype.BuildItemsSource = function (myResponse) {
        var _this = this;
        this.ItemsSource = [];
        this.SelectedItem = null;
        var myResultList = [];
        if (!myResponse.HasError) {
            if (myResponse.Result.length > 0) {
                switch (this.TransferTypeCode) {
                    case "ARIN": {
                        myResponse.Result.forEach(function (item) {
                            var myResultItem = new NewTransferLine(_this);
                            myResultItem.Id = item.Id;
                            myResultItem.Date = item.InvoiceDate;
                            myResultItem.DateTicks = Tools_1.DateTool.GetDateParts(item.InvoiceDate).DateTicks;
                            myResultItem.Reference = item.InvoiceNumber;
                            myResultItem.Partner = item.BillToName;
                            myResultItem.Status = item.StatusName;
                            myResultItem.Amount = item.AmountInInvoiceCurrency;
                            myResultItem.CurrencyCode = item.InvoiceCurrencyCode;
                            myResultItem.TransferError = item.TransferError;
                            myResultItem.ReadyForTransfer = item.TransferStatusCode == "RD" ? true : false;
                            myResultList.push(myResultItem);
                        });
                        break;
                    }
                    case "APIN": {
                        myResponse.Result.forEach(function (item) {
                            var myResultItem = new NewTransferLine(_this);
                            myResultItem.Id = item.Id;
                            myResultItem.Date = item.InvoiceDate;
                            myResultItem.DateTicks = Tools_1.DateTool.GetDateParts(item.InvoiceDate).DateTicks;
                            myResultItem.Reference = item.InvoiceNumber;
                            myResultItem.Partner = item.VendorName;
                            myResultItem.Status = item.StatusName;
                            myResultItem.Amount = item.AmountInInvoiceCurrency;
                            myResultItem.CurrencyCode = item.InvoiceCurrencyCode;
                            myResultItem.TransferError = item.TransferError;
                            myResultItem.ReadyForTransfer = item.TransferStatusCode == "RD" ? true : false;
                            myResultList.push(myResultItem);
                        });
                        break;
                    }
                    case "ARPA": {
                        myResponse.Result.forEach(function (item) {
                            var myResultItem = new NewTransferLine(_this);
                            myResultItem.Id = item.Id;
                            myResultItem.Date = item.RegisterDate;
                            myResultItem.DateTicks = Tools_1.DateTool.GetDateParts(item.RegisterDate).DateTicks;
                            myResultItem.Reference = item.PaymentNo;
                            myResultItem.Partner = item.BillToName;
                            myResultItem.Status = item.StatusName;
                            myResultItem.Amount = item.AmountInPaymentCurrency;
                            myResultItem.CurrencyCode = item.PaymentCurrencyCode;
                            myResultItem.TransferError = item.TransferError;
                            myResultItem.ReadyForTransfer = item.TransferStatusCode == "RD" ? true : false;
                            myResultList.push(myResultItem);
                        });
                        break;
                    }
                    case "APPA": {
                        myResponse.Result.forEach(function (item) {
                            var myResultItem = new NewTransferLine(_this);
                            myResultItem.Id = item.Id;
                            myResultItem.Date = item.RegisterDate;
                            myResultItem.DateTicks = Tools_1.DateTool.GetDateParts(item.RegisterDate).DateTicks;
                            myResultItem.Reference = item.PaymentNo;
                            myResultItem.Partner = item.VendorName;
                            myResultItem.Status = item.StatusName;
                            myResultItem.Amount = item.AmountInPaymentCurrency;
                            myResultItem.CurrencyCode = item.PaymentCurrencyCode;
                            myResultItem.TransferError = item.TransferError;
                            myResultItem.ReadyForTransfer = item.TransferStatusCode == "RD" ? true : false;
                            myResultList.push(myResultItem);
                        });
                        break;
                    }
                }
            }
        }
        this.ItemsSource = myResultList.sort(function (a, b) { return a.DateTicks == b.DateTicks ? 0 : a.DateTicks < b.DateTicks ? -1 : 1; });
        this.IsFirstTimeLoading = false;
        this.OnLinesSelected();
    };
    NewTransferComponent.prototype.OnLinesSelected = function () {
        this.SelectedCount = this.ItemsSource.filter(function (f) { return f.IsChecked == true; }).length;
        this.ExportButtonIsEnabled = this.SelectedCount > 0 ? true : false;
    };
    NewTransferComponent.prototype.ExportButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.SelectedCount = this.ItemsSource.filter(function (f) { return f.IsChecked == true; }).length;
        if (this.SelectedCount == 0) {
            errors.push("You must select 1 line at least");
        }
        else if (this.SelectedCount > 100) {
            errors.push("You must select 100 line max");
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            var logWindowTitle = null;
            switch (this.TransferTypeCode) {
                case "ARIN": {
                    logWindowTitle = "Exporting AR Invoice Transfer";
                    break;
                }
                case "APIN": {
                    logWindowTitle = "Exporting AP Invoice Transfer";
                    break;
                }
                case "ARPA": {
                    logWindowTitle = "Exporting AR Payment Transfer";
                    break;
                }
                case "APPA": {
                    logWindowTitle = "Exporting AP Payment Transfer";
                    break;
                }
            }
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = logWindowTitle;
            logWindow.Width = 500;
            logWindow.Height = 200;
            logWindow.Show('./Invoice/Components/Workspaces/Windows/ExportTransferComponent');
            logWindow.ComponentLoaded.subscribe(function (comp) {
                _this.ItemsSource.filter(function (f) { return f.IsChecked == true; }).forEach(function (item) {
                    var TransferLinePM = new AccountingTransferLinePM_1.AccountingTransferLinePM(null);
                    TransferLinePM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    TransferLinePM.EntityId = item.Id;
                    TransferLinePM.EntityReference = item.Reference;
                    _this.EntityPM.AddAccountingTransferLinePM(TransferLinePM);
                });
                comp.Export(_this.EntityPM);
            });
        }
    };
    NewTransferComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewTransferComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewTransferComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewTransferComponent);
    return NewTransferComponent;
}(BaseComponent_1.BaseComponent));
exports.NewTransferComponent = NewTransferComponent;
var NewTransferLine = /** @class */ (function () {
    function NewTransferLine(fatherComponent) {
        this.fatherComponent = fatherComponent;
        this.isChecked = false;
        if (fatherComponent.IsFirstTimeLoading) {
            this.isChecked = true;
        }
    }
    Object.defineProperty(NewTransferLine.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (value) {
            if (this.isChecked != value) {
                this.isChecked = value;
                this.fatherComponent.OnLinesSelected();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewTransferLine.prototype.MarkAsBlockedClicked = function () {
    };
    return NewTransferLine;
}());
exports.NewTransferLine = NewTransferLine;
//# sourceMappingURL=NewTransferComponent.js.map