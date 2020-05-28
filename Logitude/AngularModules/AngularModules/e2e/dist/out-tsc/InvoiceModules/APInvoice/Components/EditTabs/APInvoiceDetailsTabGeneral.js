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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var APInvoiceLinePM_1 = require("../../../../Invoice/EntityPMs/APInvoiceLinePM");
var APInvoiceTotalVATPM_1 = require("../../../../Invoice/EntityPMs/APInvoiceTotalVATPM");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_2 = require("../../../../Invoice/Tools");
var Args_1 = require("../../../../Invoice/Args");
var CurrencyRatesService_1 = require("../../../../Common/Services/CurrencyRatesService");
var NumbersPipe_1 = require("../../../../Infrastructure/Pipes/NumbersPipe");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var PaymentTermListService_1 = require("../../../../Common/Services/StandardLists/PaymentTermListService");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var VatTypeListService_1 = require("../../../../Common/Services/StandardLists/VatTypeListService");
var ChargesTypeListService_1 = require("../../../../Common/Services/StandardLists/ChargesTypeListService");
var UserListService_1 = require("../../../../Common/Services/StandardLists/UserListService");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var GLAccountPMService_1 = require("../../../../Accounting/Services/StandardPMs/GLAccountPMService");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var APInvoiceDetailsTabGeneral = /** @class */ (function (_super) {
    __extends(APInvoiceDetailsTabGeneral, _super);
    function APInvoiceDetailsTabGeneral(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.ObjectTableName = "APInvoice";
        _this.DataContext = _this;
        _this.LocalCurrencyCode = SessionLocator_1.SessionLocator.LocalCurrencyCode;
        _this.isBaseDataLoaded = false;
        _this.IsEditExchangeRateVisible = false;
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.AllVatTypes = [];
        // SetUIProperties
        _this.PaymentTermDisplayInLOV = true;
        _this.RateIsEnabled = false;
        // LoadVatsPercentages
        // Load Date 
        _this.LastRatesList = [];
        _this.VatTypePercentagesList = [];
        _this.isTotalInLocalCurrency = false;
        _this.SummaryItems = [];
        // Properties
        _this.VendorDependencyProperty1 = Tools_2.InvoiceTool.GetVendorPartnerTypes();
        _this.glaccount = null;
        _this.myRelativeRateDate = null;
        // VAT Type Filter
        _this.vatTypeId = null;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.EntityPM = entityArgs.EntityPM;
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        _this.isBaseDataLoaded = false;
        _this.InitializeServices();
        _this.SetUIProperties();
        _this.BuildScreenData();
        _this.Listen();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            _this.IsEditExchangeRateVisible = true;
        }
        return _this;
    }
    APInvoiceDetailsTabGeneral.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.BuildInvoiceLines();
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.BuildInvoiceLines();
                }
            });
        }
    };
    APInvoiceDetailsTabGeneral.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    APInvoiceDetailsTabGeneral.prototype.InitializeServices = function () {
        this.myCardListService = new CardListService_1.CardListService();
        this.myCommonDomainService = new CommonDomainService_1.CommonDomainService();
        this.myPaymentTermListService = new PaymentTermListService_1.PaymentTermListService();
        this.myCurrencyListService = new CurrencyListService_1.CurrencyListService();
        this.myVatTypeListService = new VatTypeListService_1.VatTypeListService();
        this.myChargesTypeListService = new ChargesTypeListService_1.ChargesTypeListService();
        this.myGLAccountPMService = new GLAccountPMService_1.GLAccountPMService();
        this.GetAllVatTypes();
    };
    APInvoiceDetailsTabGeneral.prototype.GetAllVatTypes = function () {
        var _this = this;
        this.myVatTypeListService.getAllFromCache().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.AllVatTypes = myResponse.Result;
            }
        });
    };
    APInvoiceDetailsTabGeneral.prototype.SetUIProperties = function () {
        if (!this.IsScreenEnabled) {
            this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaymentTermId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AmountInInvoiceCurrency", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("InvoiceNumber", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VATNumber", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("InvoiceDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("InvoiceCurrencyId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ExchangeRateDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("LocalDescription", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PaymentTermId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("AmountInInvoiceCurrency", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("InvoiceNumber", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("VATNumber", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("InvoiceDate", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("InvoiceCurrencyId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("LocalDescription", this.ObjectTableName, true);
            if (this.EntityPM.InvoicePayments.length > 0) {
                this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("InvoiceCurrencyId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("ExchangeRateDate", this.ObjectTableName, false);
            }
            if (this.EntityPM.InvoiceCurrencyId == SessionLocator_1.SessionLocator.TenantPM.CurrencyId) {
                this.UIProperties.SetEnabled("ExchangeRateDate", this.ObjectTableName, false);
            }
        }
        this.SetUIProperties_DueDate();
        this.SetUIProperties_VATNumber();
        this.SetUIProperties_ExchangeRate();
    };
    APInvoiceDetailsTabGeneral.prototype.SetUIProperties_DueDate = function () {
        var AllowManuallyDueDate = false;
        if (SessionLocator_1.SessionLocator.AccountingSystemPM) {
            AllowManuallyDueDate = SessionLocator_1.SessionLocator.AccountingSystemPM.AllowManuallyDueDate;
        }
        if (AllowManuallyDueDate) {
            this.PaymentTermDisplayInLOV = null;
        }
        if (!this.IsScreenEnabled) {
            AllowManuallyDueDate = false;
        }
        this.UIProperties.SetEnabled("DueDate", this.ObjectTableName, AllowManuallyDueDate);
    };
    APInvoiceDetailsTabGeneral.prototype.SetUIProperties_VATNumber = function () {
        var isRequired = false;
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAP) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.VATNumber)) {
                isRequired = true;
            }
        }
        this.UIProperties.SetRequired("VATNumber", this.ObjectTableName, isRequired);
    };
    APInvoiceDetailsTabGeneral.prototype.SetUIProperties_ExchangeRate = function () {
        var isEnabled = false;
        if (this.IsScreenEnabled) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APInvoice", "APInvoiceEditExchangeRate")) {
                if (this.InvoiceCurrencyId) {
                    if (this.InvoiceCurrencyId != SessionLocator_1.SessionLocator.TenantPM.CurrencyId) {
                        if (this.EntityPM.InvoicePayments.length == 0) {
                            isEnabled = true;
                        }
                    }
                }
            }
        }
        this.RateIsEnabled = isEnabled;
        this.UIProperties.SetEnabled("InvoiceCurrencyExchangeRate", this.ObjectTableName, isEnabled);
    };
    // Refresh Screen
    APInvoiceDetailsTabGeneral.prototype.RefreshScreen = function () {
        this.SetUIProperties();
        this.BuildInvoiceLines();
    };
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "IsScreenEnabled", {
        get: function () {
            var result = true;
            if (this.EntityPM != null) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "WA") {
                    result = true;
                }
                else {
                    result = false;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "VatTypeFilterButtonIsEnabled", {
        get: function () {
            var result = true;
            if (this.EntityPM != null) {
                if (!this.IsScreenEnabled) {
                    result = false;
                }
                else if (Tools_1.AppTool.IsNullOrEmpty(this.VatTypeId)) {
                    result = false;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    APInvoiceDetailsTabGeneral.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        if (this.myCurrencyRatesService == null) {
            this.myCurrencyRatesService = new CurrencyRatesService_1.CurrencyRatesService();
        }
        var loadingDate = this.EntityPM.InvoiceDate;
        if (loadingDate == null) {
            loadingDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
        this.myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator_1.SessionLocator.AccountingCurrencyId, loadingDate).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.LastRatesList = myResponse.Result;
                _this.myCommonDomainService.GetVatTypePercentagePMByDate(loadingDate).subscribe(function (myResponse2) {
                    if (!myResponse2.HasError) {
                        _this.VatTypePercentagesList = myResponse2.Result;
                    }
                    _this.BuildInvoiceLines();
                    _this.CurrentSession.StopBusyIndicator();
                });
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    APInvoiceDetailsTabGeneral.prototype.UpdateData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        if (this.myCurrencyRatesService == null) {
            this.myCurrencyRatesService = new CurrencyRatesService_1.CurrencyRatesService();
        }
        var loadingDate = this.EntityPM.InvoiceDate;
        if (loadingDate == null) {
            loadingDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
        this.myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator_1.SessionLocator.AccountingCurrencyId, loadingDate).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.LastRatesList = myResponse.Result;
                _this.SetCurrencyRateData();
                _this.myCommonDomainService.GetVatTypePercentagePMByDate(loadingDate).subscribe(function (myResponse2) {
                    if (!myResponse2.HasError) {
                        _this.VatTypePercentagesList = myResponse2.Result;
                    }
                    _this.ItemsSource.Collection.forEach(function (item) {
                        item.SetVatPercentage(_this.GetVatTypePercentage(item.VatTypeId));
                    });
                    _this.CurrentSession.StopBusyIndicator();
                });
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    APInvoiceDetailsTabGeneral.prototype.SetCurrencyRateData = function () {
        var _this = this;
        var myRate = null;
        var myRateDate = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
            if (this.InvoiceCurrencyId == SessionLocator_1.SessionLocator.TenantPM.CurrencyId) {
                myRate = 1;
            }
            else {
                var lastRate = this.LastRatesList.filter(function (d) { return d.ForeignCurrencyId == _this.InvoiceCurrencyId; })[0];
                if (lastRate != null) {
                    myRate = lastRate.Rate;
                    myRateDate = lastRate.ValueDate;
                }
            }
        }
        this.InvoiceCurrencyExchangeRate = myRate;
        this.ExchangeRateDate = myRateDate;
    };
    APInvoiceDetailsTabGeneral.prototype.GetCurrencyRate = function (currencyId) {
        var myResult = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(currencyId)) {
            if (currencyId == SessionLocator_1.SessionLocator.TenantPM.CurrencyId) {
                myResult = 1;
            }
            else {
                var lastRate = this.LastRatesList.filter(function (d) { return d.ForeignCurrencyId == currencyId; })[0];
                if (lastRate != null) {
                    myResult = lastRate.Rate;
                }
            }
        }
        return myResult;
    };
    APInvoiceDetailsTabGeneral.prototype.GetCurrencyRateDate = function (currencyId) {
        var myResult = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(currencyId)) {
            if (currencyId == SessionLocator_1.SessionLocator.TenantPM.CurrencyId) {
                myResult = null;
            }
            else {
                var lastRate = this.LastRatesList.filter(function (d) { return d.ForeignCurrencyId == currencyId; })[0];
                if (lastRate != null) {
                    myResult = lastRate.ValueDate;
                }
            }
        }
        return myResult;
    };
    APInvoiceDetailsTabGeneral.prototype.GetVatTypePercentage = function (vatTypeId) {
        var myResult = null;
        var vatTypePercentagePM = this.VatTypePercentagesList.filter(function (d) { return d.VatTypeId == vatTypeId; })[0];
        if (vatTypePercentagePM != null) {
            myResult = vatTypePercentagePM.Percentage;
        }
        return myResult;
    };
    APInvoiceDetailsTabGeneral.prototype.UpdateCurrencyRateClicked = function () {
        var _this = this;
        var loadingDate = this.EntityPM.InvoiceDate;
        if (loadingDate == null) {
            loadingDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 350;
        logWindow.Title = "Update Currency Rate";
        logWindow.WindowArgs = { CurrencyId: this.InvoiceCurrencyId, CurrencyCode: this.InvoiceCurrencyCode, Rate: this.InvoiceCurrencyExchangeRate, Date: loadingDate };
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.LastRatesList = comp.RatesList;
                    _this.InvoiceCurrencyExchangeRate = comp.Rate;
                    _this.ExchangeRateDate = comp.RateDate;
                }
            });
        });
        logWindow.Show('./CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent');
    };
    APInvoiceDetailsTabGeneral.prototype.BuildScreenData = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "WA") {
            if (!this.isBaseDataLoaded) {
                this.isBaseDataLoaded = true;
                this.LoadData();
            }
        }
        else {
            this.BuildInvoiceLines();
        }
    };
    APInvoiceDetailsTabGeneral.prototype.BuildInvoiceLines = function () {
        var _this = this;
        this.ItemsSource.Clear();
        this.ComputeRelativeRateDate();
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            var defaultConnectedLines = this.EntityPM.InvoiceLines.filter(function (d) { return d.VendorId == _this.EntityPM.VendorId; });
            var otherLines = this.EntityPM.InvoiceLines.filter(function (d) { return d.VendorId != _this.VendorId; });
            defaultConnectedLines.forEach(function (line) {
                _this.ItemsSource.Insert(new APInvoiceLineItem(line, _this, false));
            });
            otherLines.forEach(function (line) {
                _this.EntityPM.RemoveAPInvoiceLinePM(line);
                _this.ItemsSource.Insert(new APInvoiceLineItem(line, _this, false));
            });
            this.ComputeTotals();
        }
        else {
            this.EntityPM.InvoiceLines.forEach(function (line) {
                _this.ItemsSource.Insert(new APInvoiceLineItem(line, _this, false));
            });
            this.BuildSummary();
        }
    };
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "SummaryCurrencyButtonsVisibility", {
        // Totals
        get: function () {
            var myResult = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.InvoiceCurrencyId)) {
                if (SessionLocator_1.SessionLocator.TenantPM.CurrencyId != this.EntityPM.InvoiceCurrencyId) {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "IsTotalInLocalCurrency", {
        get: function () { return this.isTotalInLocalCurrency; },
        set: function (value) {
            if (this.isTotalInLocalCurrency != value) {
                this.isTotalInLocalCurrency = value;
                this.BuildSummary();
            }
        },
        enumerable: true,
        configurable: true
    });
    APInvoiceDetailsTabGeneral.prototype.ComputeTotals = function () {
        this.BuildTotalVATs();
        this.SubTotalInLocalCurrency = Tools_1.AppTool.Round(Tools_1.ArrayTool.Sum(this.EntityPM.InvoiceLines, "LocalCurrencyAmount"), 2);
        this.SubTotalInInvoiceCurrency = Tools_1.AppTool.Round(Tools_1.ArrayTool.Sum(this.EntityPM.InvoiceLines, "InvoiceCurrencyAmount"), 2);
        if (this.EntityPM.TotalVATs.length > 0) {
            this.EntityPM.AmountInLocalCurrency_Summary = Tools_1.AppTool.Round(this.EntityPM.SubTotalInLocalCurrency + Tools_1.ArrayTool.Sum(this.EntityPM.TotalVATs, "LocalVATAmount"), 2);
            this.EntityPM.AmountInInvoiceCurrency_Summary = Tools_1.AppTool.Round(this.EntityPM.SubTotalInInvoiceCurrency + Tools_1.ArrayTool.Sum(this.EntityPM.TotalVATs, "InvoiceCurrencyVATAmount"), 2);
        }
        else {
            this.EntityPM.AmountInLocalCurrency_Summary = 0;
            this.EntityPM.AmountInInvoiceCurrency_Summary = 0;
        }
        // Local
        if (SessionLocator_1.SessionLocator.LocalCurrencyId == this.EntityPM.InvoiceCurrencyId) {
            this.EntityPM.AmountInLocalCurrency = this.AmountInInvoiceCurrency;
        }
        else {
            this.EntityPM.AmountInLocalCurrency = Tools_1.AppTool.Round(this.AmountInInvoiceCurrency * this.InvoiceCurrencyExchangeRate, 2);
        }
        // Profit
        if (this.EntityPM.ProfitCurrencyId == this.InvoiceCurrencyId) {
            this.EntityPM.AmountInProfitCurrency = this.AmountInInvoiceCurrency;
        }
        else if (this.EntityPM.ProfitCurrencyId == SessionLocator_1.SessionLocator.LocalCurrencyId) {
            this.EntityPM.AmountInProfitCurrency = this.EntityPM.AmountInLocalCurrency;
        }
        else {
            this.EntityPM.AmountInProfitCurrency = Tools_1.AppTool.Round(this.EntityPM.AmountInLocalCurrency / this.ProfitCurrencyExchangeRate, 2);
        }
        this.EntityPM.AmountDue = this.EntityPM.AmountInInvoiceCurrency == null ? 0 : this.EntityPM.AmountInInvoiceCurrency;
        this.EntityPM.AmountDueInLocalCurrency = this.EntityPM.AmountInLocalCurrency == null ? 0 : this.EntityPM.AmountInLocalCurrency;
        this.EntityPM.AmountDueInProfitCurrency = this.EntityPM.AmountInProfitCurrency == null ? 0 : this.EntityPM.AmountInProfitCurrency;
        this.BuildSummary();
    };
    APInvoiceDetailsTabGeneral.prototype.BuildTotalVATs = function () {
        var _this = this;
        this.EntityPM.TotalVATs = [];
        var myDataLines = this.EntityPM.InvoiceLines.filter(function (f) { return f.VatTypeId != null; });
        if (myDataLines.length > 0) {
            this.GetAllVatTypes();
            // Build Totals Class
            var group_Source = [];
            myDataLines.forEach(function (item) {
                var lineVatType = _this.AllVatTypes.filter(function (f) { return f.Id == item.VatTypeId; })[0];
                if (lineVatType) {
                    if (Tools_1.AppTool.IsNullOrEmpty(item.LocalCurrencyAmount)) {
                        item.LocalCurrencyAmount = 0;
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(item.InvoiceCurrencyAmount)) {
                        item.InvoiceCurrencyAmount = 0;
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(item.ProfitCurrencyAmount)) {
                        item.ProfitCurrencyAmount = 0;
                    }
                    if (!lineVatType.IsMultiPercentage) {
                        var myQroupItem = new Args_1.InvoiceTotalsClass();
                        myQroupItem.Id = lineVatType.Id;
                        myQroupItem.VatTypeId = lineVatType.Id;
                        myQroupItem.VatTypePercentage = item.VatPercentage;
                        myQroupItem.LocalCurrencyAmount = item.LocalCurrencyAmount;
                        myQroupItem.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
                        myQroupItem.ProfitCurrencyAmount = item.ProfitCurrencyAmount;
                        myQroupItem.ExternalVatCard = SessionLocator_1.SessionLocator.AccountingSettingPM.PayableVATCard;
                        myQroupItem.ExternalTAXItemId = lineVatType.ExternalTAXItemId;
                        group_Source.push(myQroupItem);
                    }
                    else {
                        var myVatGroups = SessionLocator_1.SessionLocator.AllVatTypesGroups.filter(function (f) { return f.GroupVATTypeId == item.VatTypeId; });
                        myVatGroups.forEach(function (itemGroup) {
                            var myQroupItem = new Args_1.InvoiceTotalsClass();
                            myQroupItem.Id = itemGroup.SingleVATTypeId;
                            myQroupItem.VatTypeId = itemGroup.SingleVATTypeId;
                            myQroupItem.LocalCurrencyAmount = item.LocalCurrencyAmount;
                            myQroupItem.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
                            myQroupItem.ProfitCurrencyAmount = item.ProfitCurrencyAmount;
                            myQroupItem.ExternalVatCard = SessionLocator_1.SessionLocator.AccountingSettingPM.PayableVATCard;
                            var vatType = _this.AllVatTypes.filter(function (f) { return f.Id == itemGroup.SingleVATTypeId; })[0];
                            if (vatType) {
                                myQroupItem.ExternalTAXItemId = vatType.ExternalTAXItemId;
                                myQroupItem.VatTypePercentage = _this.GetVatTypePercentage(vatType.Id);
                            }
                            group_Source.push(myQroupItem);
                        });
                    }
                }
            });
            // Group Totals Class
            var group_data = [];
            group_Source.forEach(function (item) {
                var record = group_data.filter(function (f) { return f.VatTypeId == item.VatTypeId && f.VatTypePercentage == item.VatTypePercentage && f.ExternalVatCard == item.ExternalVatCard && f.ExternalTAXItemId == item.ExternalTAXItemId; })[0];
                if (record) {
                    record.LocalCurrencyAmount += item.LocalCurrencyAmount;
                    record.InvoiceCurrencyAmount += item.InvoiceCurrencyAmount;
                    record.ProfitCurrencyAmount += item.ProfitCurrencyAmount;
                }
                else {
                    record = new Args_1.InvoiceTotalsClass();
                    record.Id = item.Id;
                    record.VatTypeId = item.VatTypeId;
                    record.VatTypePercentage = item.VatTypePercentage;
                    record.ExternalVatCard = item.ExternalVatCard;
                    record.ExternalTAXItemId = item.ExternalTAXItemId;
                    record.LocalCurrencyAmount = item.LocalCurrencyAmount;
                    record.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
                    record.ProfitCurrencyAmount = item.ProfitCurrencyAmount;
                    group_data.push(record);
                }
            });
            // Build Invoice Total VATs
            group_data.forEach(function (item) {
                var itemVatType = _this.AllVatTypes.filter(function (f) { return f.Id == item.VatTypeId; })[0];
                var itemTotalVAT = new APInvoiceTotalVATPM_1.APInvoiceTotalVATPM(null);
                itemTotalVAT.Tenant = SessionLocator_1.SessionLocator.Tenant;
                itemTotalVAT.APInvoiceId = _this.EntityPM.Id;
                itemTotalVAT.VatTypeId = item.VatTypeId;
                itemTotalVAT.VatTypeName = itemVatType.EnglishName;
                itemTotalVAT.ExternalVATCard = item.ExternalVatCard;
                itemTotalVAT.ExternalTAXItemId = item.ExternalTAXItemId;
                itemTotalVAT.VatPercent = Tools_1.AppTool.Round(item.VatTypePercentage, 2);
                itemTotalVAT.LocalVatableAmount = Tools_1.AppTool.Round(item.LocalCurrencyAmount, 2);
                itemTotalVAT.InvoiceCurrencyVatableAmount = Tools_1.AppTool.Round(item.InvoiceCurrencyAmount, 2);
                itemTotalVAT.ProfitVatableAmount = Tools_1.AppTool.Round(item.ProfitCurrencyAmount, 2);
                itemTotalVAT.LocalVATAmount = Tools_1.AppTool.Round((itemTotalVAT.LocalVatableAmount * itemTotalVAT.VatPercent / 100), 2);
                itemTotalVAT.InvoiceCurrencyVATAmount = Tools_1.AppTool.Round((itemTotalVAT.InvoiceCurrencyVatableAmount * itemTotalVAT.VatPercent / 100), 2);
                itemTotalVAT.ProfitCurrencyVATAmount = Tools_1.AppTool.Round((itemTotalVAT.ProfitVatableAmount * itemTotalVAT.VatPercent / 100), 2);
                itemTotalVAT.VatTypeCell = itemTotalVAT.VatTypeName + " (" + itemTotalVAT.VatPercent + "%)";
                _this.EntityPM.AddAPInvoiceTotalVATPM(itemTotalVAT);
            });
        }
    };
    APInvoiceDetailsTabGeneral.prototype.BuildSummary = function () {
        var _this = this;
        this.SummaryItems = [];
        var pipe = new NumbersPipe_1.NumbersPipe();
        var selectedCurrencyCode = this.IsTotalInLocalCurrency ? "(" + this.LocalCurrencyCode + ")" : "(" + this.InvoiceCurrencyCode + ")";
        if (this.EntityPM.TotalVATs.length > 0) {
            var mySummaryItem_Sub = new Args_1.SummaryItem();
            mySummaryItem_Sub.Label = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.S.Details.Subtotal");
            mySummaryItem_Sub.Value = this.IsTotalInLocalCurrency ? pipe.transform(this.EntityPM.SubTotalInLocalCurrency, "N2") : pipe.transform(this.EntityPM.SubTotalInInvoiceCurrency, "N2");
            this.SummaryItems.push(mySummaryItem_Sub);
            this.EntityPM.TotalVATs.forEach(function (item) {
                var myOperatorItem = new Args_1.SummaryItem();
                myOperatorItem.Value = "+";
                _this.SummaryItems.push(myOperatorItem);
                var mySummaryItem = new Args_1.SummaryItem();
                mySummaryItem.Label = item.VatTypeCell;
                mySummaryItem.Value = _this.IsTotalInLocalCurrency ? pipe.transform(item.LocalVATAmount, "N2") : pipe.transform(item.InvoiceCurrencyVATAmount, "N2");
                _this.SummaryItems.push(mySummaryItem);
            });
            var myOperatorItem = new Args_1.SummaryItem();
            myOperatorItem.Value = "=";
            this.SummaryItems.push(myOperatorItem);
        }
        var mySummaryItem_All = new Args_1.SummaryItem();
        mySummaryItem_All.Label = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.F.AmountInInvoiceCurrency") + " " + selectedCurrencyCode;
        mySummaryItem_All.Value = this.IsTotalInLocalCurrency ? pipe.transform(this.EntityPM.AmountInLocalCurrency_Summary, "N2") : pipe.transform(this.EntityPM.AmountInInvoiceCurrency_Summary, "N2");
        this.SummaryItems.push(mySummaryItem_All);
    };
    APInvoiceDetailsTabGeneral.prototype.OnInvoiceDateChangedLoad = function () {
        this.UpdateData();
    };
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "SubTotalInLocalCurrency", {
        get: function () { return this.EntityPM.SubTotalInLocalCurrency; },
        set: function (value) {
            var setValue = Tools_1.AppTool.Round(value, 2);
            if (this.EntityPM.SubTotalInLocalCurrency != setValue) {
                this.EntityPM.SubTotalInLocalCurrency = setValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "SubTotalInInvoiceCurrency", {
        get: function () { return this.EntityPM.SubTotalInInvoiceCurrency; },
        set: function (value) {
            var setValue = Tools_1.AppTool.Round(value, 2);
            if (this.EntityPM.SubTotalInInvoiceCurrency != setValue) {
                this.EntityPM.SubTotalInInvoiceCurrency = setValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "AmountInInvoiceCurrency", {
        get: function () { return this.EntityPM.AmountInInvoiceCurrency; },
        set: function (value) {
            var setValue = Tools_1.AppTool.Round(value, 2);
            if (this.EntityPM.AmountInInvoiceCurrency != setValue) {
                this.EntityPM.AmountInInvoiceCurrency = setValue;
                this.EntityPM.InvoiceExpectedAmount = setValue;
                //// Local
                //if (SessionLocator.LocalCurrencyId == this.EntityPM.InvoiceCurrencyId) {
                //    this.EntityPM.AmountInLocalCurrency = setValue;
                //}
                //else {
                //    this.EntityPM.AmountInLocalCurrency = AppTool.Round(setValue * this.InvoiceCurrencyExchangeRate, 2);
                //}
                //// Profit
                //if (this.EntityPM.ProfitCurrencyId == this.InvoiceCurrencyId) {
                //    this.EntityPM.AmountInProfitCurrency = setValue;
                //}
                //else if (this.EntityPM.ProfitCurrencyId == SessionLocator.LocalCurrencyId) {
                //    this.EntityPM.AmountInProfitCurrency = this.EntityPM.AmountInLocalCurrency;
                //}
                //else {
                //    this.EntityPM.AmountInProfitCurrency = AppTool.Round(this.EntityPM.AmountInLocalCurrency / this.ProfitCurrencyExchangeRate, 2);
                //}
                //this.EntityPM.AmountDue = this.EntityPM.AmountInInvoiceCurrency == null ? 0 : this.EntityPM.AmountInInvoiceCurrency;
                //this.EntityPM.AmountDueInLocalCurrency = this.EntityPM.AmountInLocalCurrency == null ? 0 : this.EntityPM.AmountInLocalCurrency;
                //this.EntityPM.AmountDueInProfitCurrency = this.EntityPM.AmountInProfitCurrency == null ? 0 : this.EntityPM.AmountInProfitCurrency;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "AmountInLocalCurrency", {
        get: function () { return this.EntityPM.AmountInLocalCurrency; },
        set: function (value) {
            var setValue = Tools_1.AppTool.Round(value, 2);
            if (this.EntityPM.AmountInLocalCurrency != setValue) {
                this.EntityPM.AmountInLocalCurrency = setValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "AmountInProfitCurrency", {
        get: function () { return this.EntityPM.AmountInProfitCurrency; },
        set: function (value) {
            var setValue = Tools_1.AppTool.Round(value, 2);
            if (this.EntityPM.AmountInProfitCurrency != setValue) {
                this.EntityPM.AmountInProfitCurrency = setValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "AmountDue", {
        get: function () { return this.EntityPM.AmountDue; },
        set: function (value) {
            var setValue = Tools_1.AppTool.Round(value, 2);
            if (setValue == null) {
                setValue = 0;
            }
            if (this.EntityPM.AmountDue != setValue) {
                this.EntityPM.AmountDue = setValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "AmountDueInLocalCurrency", {
        get: function () { return this.EntityPM.AmountDueInLocalCurrency; },
        set: function (value) {
            var setValue = Tools_1.AppTool.Round(value, 2);
            if (setValue == null) {
                setValue = 0;
            }
            if (this.EntityPM.AmountDueInLocalCurrency != setValue) {
                this.EntityPM.AmountDueInLocalCurrency = setValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "AmountDueInProfitCurrency", {
        get: function () { return this.EntityPM.AmountDueInProfitCurrency; },
        set: function (value) {
            var setValue = Tools_1.AppTool.Round(value, 2);
            if (setValue == null) {
                setValue = 0;
            }
            if (this.EntityPM.AmountDueInProfitCurrency != setValue) {
                this.EntityPM.AmountDueInProfitCurrency = setValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "VendorId", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.VendorId;
        },
        set: function (value) {
            var _this = this;
            if (this.EntityPM != null) {
                if (this.EntityPM.VendorId != value) {
                    this.EntityPM.VendorId = value;
                    if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                        this.ItemsSource.Collection.forEach(function (item) {
                            if (Tools_1.AppTool.IsNullOrEmpty(item.VendorId)) {
                                item.Exists = true;
                            }
                            else {
                                item.Exists = (item.VendorId == _this.EntityPM.VendorId);
                            }
                        });
                    }
                    this.GetCardProperties();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    APInvoiceDetailsTabGeneral.prototype.GetCardProperties = function () {
        var _this = this;
        this.myCardListService.getSingle(this.EntityPM.VendorId).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var list = myResponse.Result;
                if (list == null) {
                    _this.VATNumber = null;
                    _this.InvoiceCurrencyId = SessionLocator_1.SessionLocator.TenantPM.CurrencyId;
                    _this.PaymentTermId = SessionLocator_1.SessionLocator.TenantPM.PaymentTermId;
                    _this.glaccount = null;
                    _this.EntityPM.VendorGLAccountId = null;
                }
                else {
                    _this.VATNumber = list.VatNumber;
                    _this.EntityPM.VendorName = list.EnglishName;
                    if (!Tools_1.AppTool.IsNullOrEmpty(list.InvoiceCurrencyId)) {
                        _this.InvoiceCurrencyId = list.InvoiceCurrencyId;
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(list.PaymentTermId)) {
                        _this.PaymentTermId = list.PaymentTermId;
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(list.VatTypeId)) {
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(list.GLAccountId)) {
                        _this.myGLAccountPMService.get(list.GLAccountId).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                _this.glaccount = myResponse.Result;
                                _this.EntityPM.VendorGLAccountId = _this.glaccount.Id;
                            }
                        });
                    }
                }
            }
        });
    };
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "VATNumber", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.VATNumber;
        },
        set: function (newValue) {
            if (this.EntityPM.VATNumber != newValue) {
                this.EntityPM.VATNumber = newValue;
                this.SetUIProperties_VATNumber();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "InvoiceNumber", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.InvoiceNumber;
        },
        set: function (newValue) {
            if (this.EntityPM.InvoiceNumber != newValue) {
                this.EntityPM.InvoiceNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "PaymentTermId", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.PaymentTermId;
        },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.PaymentTermId != newValue) {
                this.EntityPM.PaymentTermId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.PaymentTermName = null;
                }
                else {
                    this.myPaymentTermListService.getSingleFromCache(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.PaymentTermName = list.EnglishName;
                            }
                        }
                    });
                }
                Tools_2.InvoiceTool.ComputeAPInvoiceDueDate(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "PaymentTermName", {
        get: function () { return this.EntityPM.PaymentTermName; },
        set: function (newValue) {
            if (this.EntityPM.PaymentTermName != newValue) {
                this.EntityPM.PaymentTermName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "DueDate", {
        get: function () { return this.EntityPM.DueDate; },
        set: function (newValue) {
            if (this.EntityPM.DueDate != newValue) {
                this.EntityPM.DueDate = newValue;
                Tools_2.InvoiceTool.ComputeAPInvoicePaymentTerm(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "InvoiceDate", {
        get: function () { return this.EntityPM.InvoiceDate; },
        set: function (newValue) {
            if (this.EntityPM.InvoiceDate != newValue) {
                this.EntityPM.InvoiceDate = newValue;
                Tools_2.InvoiceTool.ComputeAPInvoiceDueDate(this.EntityPM);
                this.OnInvoiceDateChangedLoad();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "InvoiceCurrencyId", {
        // Currency 
        get: function () { return this.EntityPM.InvoiceCurrencyId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.InvoiceCurrencyId != value) {
                this.EntityPM.InvoiceCurrencyId = value;
                this.InvoiceCurrencyExchangeRate = this.GetCurrencyRate(value);
                this.ExchangeRateDate = this.GetCurrencyRateDate(value);
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.InvoiceCurrencyCode = null;
                }
                else {
                    this.myCurrencyListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.InvoiceCurrencyCode = list.Code;
                            }
                        }
                        _this.ItemsSource.Collection.forEach(function (item) {
                            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                                if (Tools_1.AppTool.IsNullOrEmpty(item.VendorId)) {
                                    item.Exists = true;
                                }
                                else {
                                    item.Exists = (item.VendorId == _this.EntityPM.VendorId);
                                }
                            }
                            item.OnInvoiceCurrencyChanged();
                        });
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "InvoiceCurrencyCode", {
        get: function () { return this.EntityPM.InvoiceCurrencyCode; },
        set: function (value) {
            if (this.EntityPM.InvoiceCurrencyCode != value) {
                this.EntityPM.InvoiceCurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "InvoiceCurrencyExchangeRate", {
        get: function () { return this.EntityPM.InvoiceCurrencyExchangeRate; },
        set: function (value) {
            var setValue = Tools_1.AppTool.Round(value, 5);
            if (this.EntityPM.InvoiceCurrencyExchangeRate != setValue) {
                this.EntityPM.InvoiceCurrencyExchangeRate = setValue;
                this.ItemsSource.Collection.forEach(function (item) {
                    item.OnInvoiceExchangeRateChanged();
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "ExchangeRateDate", {
        get: function () { return this.EntityPM.ExchangeRateDate; },
        set: function (value) {
            if (this.EntityPM.ExchangeRateDate != value) {
                this.EntityPM.ExchangeRateDate = value;
                this.ComputeRelativeRateDate();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "RelativeRateDate", {
        get: function () { return this.myRelativeRateDate; },
        set: function (value) {
            if (this.myRelativeRateDate != value) {
                this.myRelativeRateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    APInvoiceDetailsTabGeneral.prototype.ComputeRelativeRateDate = function () {
        this.RelativeRateDate = Tools_1.DateTool.GetRelativeRateDate(this.InvoiceDate, this.ExchangeRateDate, "old");
    };
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "ProfitCurrencyId", {
        get: function () { return this.EntityPM.ProfitCurrencyId; },
        set: function (newValue) {
            if (this.EntityPM.ProfitCurrencyId != newValue) {
                this.EntityPM.ProfitCurrencyId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "ProfitCurrencyExchangeRate", {
        get: function () { return this.EntityPM.ProfitCurrencyExchangeRate; },
        set: function (newValue) {
            if (this.EntityPM.ProfitCurrencyExchangeRate != newValue) {
                this.EntityPM.ProfitCurrencyExchangeRate = Tools_1.AppTool.Round(newValue, 5);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceDetailsTabGeneral.prototype, "VatTypeId", {
        get: function () { return this.vatTypeId; },
        set: function (value) {
            if (this.vatTypeId != value) {
                this.vatTypeId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    APInvoiceDetailsTabGeneral.prototype.RunVatTypeFilterMethod = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.VatTypeId)) {
            var vatType = this.VatTypeId;
            this.VatTypeId = null;
            if (this.ItemsSource != null) {
                this.ItemsSource.Collection.forEach(function (item) {
                    item.VatTypeId = vatType;
                });
            }
            this.ComputeTotals();
        }
    };
    APInvoiceDetailsTabGeneral.prototype.RefreshLinesVat = function (vatId, vatPercentage) {
        if (this.ItemsSource != null) {
            this.ItemsSource.Collection.forEach(function (item) {
                if (item.VatTypeId == vatId) {
                    item.VatPercentage = vatPercentage;
                }
            });
        }
    };
    APInvoiceDetailsTabGeneral.prototype.AddButtonClicked = function () {
        var _this = this;
        var line = new APInvoiceLinePM_1.APInvoiceLinePM(null);
        line.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
        line.APInvoiceId = this.EntityPM.Id;
        line.VendorId = this.VendorId;
        line.EntityId = this.EntityPM.MainEntityId;
        line.EntityReference = this.EntityPM.MainEntityReference;
        line.AmountTypeCode = "NEXP";
        line.ForiegnCurrencyId = this.InvoiceCurrencyId;
        line.ForiegnCurrencyCode = this.InvoiceCurrencyCode;
        line.ForiegnExchangeRate = this.InvoiceCurrencyExchangeRate;
        var myService = new CardListService_1.CardListService();
        myService.getSingle(this.VendorId).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var card = myResponse.Result;
                if (card != null) {
                    line.VendorName = card.EnglishName;
                }
                var addEditViewModel = new APInvoiceLineItem(line, _this, true);
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.DataContext = addEditViewModel;
                var title = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoiceLine.O.AddInvoiceLine");
                logWindow.Title = title;
                logWindow.Show('./InvoiceModules/APInvoice/Components/EditTabs/AddEditAPGeneralInvoiceLineComponent');
            }
        });
    };
    APInvoiceDetailsTabGeneral.prototype.EditLineClicked = function (item) {
        if (item != null && item.EditControlIsEnabled) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoiceLine.O.EditInvoiceLine");
            logWindow.DataContext = item;
            logWindow.Show('./InvoiceModules/APInvoice/Components/EditTabs/AddEditAPGeneralInvoiceLineComponent');
        }
    };
    APInvoiceDetailsTabGeneral = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './APInvoiceDetailsTabGeneral.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], APInvoiceDetailsTabGeneral);
    return APInvoiceDetailsTabGeneral;
}(BaseComponent_1.BaseComponent));
exports.APInvoiceDetailsTabGeneral = APInvoiceDetailsTabGeneral;
var APInvoiceLineItem = /** @class */ (function (_super) {
    __extends(APInvoiceLineItem, _super);
    function APInvoiceLineItem(line, fatherComponent, AddNewLineMode) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.AddNewLineMode = AddNewLineMode;
        _this.invoiceLinePM = null;
        _this.ObjectTableName = "APInvoiceLine";
        _this.DataContext = _this;
        _this.CorrectionByUserName = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // SetUIProperties
        _this.IsRateEnabled = false;
        _this.IsEditingEnabled = false;
        _this.IsEditExchangeRateVisible = false;
        _this.CellReadOnlyBackground = "#E6E7E8";
        _this.CellReadOnlyForeground = "#6E7172";
        // ChargeType 
        _this.chargesTypeList = null;
        _this.Glaccount = null;
        _this.VatTypeUpdateIsVisible = false;
        _this.VatTypeMultiIconVisible = false;
        _this.VatTypesGroups = [];
        _this.invoiceLinePM = line;
        _this.invoicePM = fatherComponent.EntityPM;
        _this.LocalCurrencyId = SessionLocator_1.SessionLocator.LocalCurrencyId;
        _this.IsScreenEnabled = fatherComponent.IsScreenEnabled;
        _this.SetUIProperties();
        _this.GetUserName();
        _this.setColors();
        _this.ReadVatTypeData();
        return _this;
    }
    APInvoiceLineItem.prototype.GetUserName = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.invoiceLinePM.CorrectionByUserId)) {
            var myService = new UserListService_1.UserListService();
            myService.getSingleFromCache(this.invoiceLinePM.CorrectionByUserId).subscribe(function (resp) {
                if (!resp.HasError) {
                    var result = resp;
                    var list = result.Result;
                    if (list != null) {
                        _this.CorrectionByUserName = list.EnglishName;
                    }
                }
            });
        }
    };
    APInvoiceLineItem.prototype.OnInvoiceCurrencyChanged = function () {
        // this.GetInvoiceCurrencyCode();
    };
    APInvoiceLineItem.prototype.ReCalculateTotals = function () {
        if (this.Exists) {
            this.fatherComponent.ComputeTotals();
        }
    };
    Object.defineProperty(APInvoiceLineItem.prototype, "ProfitCurrencyId", {
        // Currencies
        get: function () {
            return this.invoicePM.ProfitCurrencyId;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "InvoiceCurrencyId", {
        get: function () {
            return this.invoicePM.InvoiceCurrencyId;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "ForiegnCurrencyId", {
        get: function () { return this.invoiceLinePM.ForiegnCurrencyId; },
        set: function (value) {
            if (this.invoiceLinePM.ForiegnCurrencyId != value) {
                this.invoiceLinePM.ForiegnCurrencyId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "ForiegnExchangeRate", {
        get: function () { return this.invoiceLinePM.ForiegnExchangeRate; },
        set: function (value) {
            if (this.invoiceLinePM != null) {
                if (this.invoiceLinePM.ForiegnExchangeRate != value) {
                    this.invoiceLinePM.ForiegnExchangeRate = Tools_1.AppTool.Round(value, 5);
                    this.ComputeOtherAmounts();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "ForiegnCurrencyCode", {
        get: function () {
            return this.invoiceLinePM.ForiegnCurrencyCode;
        },
        enumerable: true,
        configurable: true
    });
    APInvoiceLineItem.prototype.UpdateCurrencyRateClicked = function () {
        var _this = this;
        var loadingDate = this.fatherComponent.InvoiceDate;
        if (loadingDate == null) {
            loadingDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 350;
        logWindow.Title = "Update Currency Rate";
        logWindow.WindowArgs = { CurrencyId: this.ForiegnCurrencyId, CurrencyCode: this.ForiegnCurrencyCode, Rate: this.ForiegnExchangeRate, Date: loadingDate };
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.ForiegnExchangeRate = comp.Rate;
                }
            });
        });
        logWindow.Show('./CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent');
    };
    APInvoiceLineItem.prototype.SetUIProperties = function () {
        this.IsEditExchangeRateVisible = this.fatherComponent.IsEditExchangeRateVisible;
        this.IsEditingEnabled = this.EditControlIsEnabled;
        this.UIProperties.SetEnabled("OpenAmount", this.ObjectTableName, this.OpenAmountIsEnabled);
        this.UIProperties.SetEnabled("ForiegnCurrencyAmount", this.ObjectTableName, this.EditControlIsEnabled);
        this.UIProperties.SetEnabled("InvoiceCurrencyAmount", this.ObjectTableName, this.EditControlIsEnabled);
        this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ForiegnCurrencyId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, true);
        if (!this.IsScreenEnabled) {
            this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, false);
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(this.invoiceLinePM.EntityPayableId)) {
            this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, false);
        }
        this.SetUIProperties_Rate();
        this.SetUIProperties_OpenAmount();
        this.SetUIProperties_EditControls();
        this.SetUIProperties_VAT();
        this.SetUIProperties_Description();
    };
    APInvoiceLineItem.prototype.SetUIProperties_Rate = function () {
        var isFieldEnabled = false;
        if (this.IsEditingEnabled) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APInvoice", "APInvoiceEditExchangeRate")) {
                if (this.ForiegnCurrencyId) {
                    if (this.ForiegnCurrencyId != this.LocalCurrencyId) {
                        isFieldEnabled = true;
                    }
                }
            }
        }
        this.IsRateEnabled = isFieldEnabled;
        this.UIProperties.SetEnabled("ForiegnExchangeRate", this.ObjectTableName, isFieldEnabled);
    };
    APInvoiceLineItem.prototype.SetUIProperties_EditControls = function () {
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("InvoiceCurrencyAmount", this.ObjectTableName, true);
        var result = true;
        if (!this.IsScreenEnabled) {
            result = false;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(this.VendorId) && this.VendorId != this.invoicePM.VendorId) {
            result = false;
        }
        if (!result) {
            this.UIProperties.SetEnabled("Notes", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("InvoiceCurrencyAmount", this.ObjectTableName, false);
        }
    };
    APInvoiceLineItem.prototype.SetUIProperties_OpenAmount = function () {
        this.UIProperties.SetEnabled("OpenAmount", this.ObjectTableName, true);
        if (this.invoicePM.StatusCode == "VD") {
            this.UIProperties.SetEnabled("OpenAmount", this.ObjectTableName, false);
        }
        else if (this.invoiceLinePM.AmountTypeCode == "NEXP") {
            this.UIProperties.SetEnabled("OpenAmount", this.ObjectTableName, false);
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(this.VendorId) && this.VendorId != this.invoicePM.VendorId) {
            this.UIProperties.SetEnabled("OpenAmount", this.ObjectTableName, false);
        }
    };
    APInvoiceLineItem.prototype.SetUIProperties_VAT = function () {
        this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, this.IsScreenEnabled);
        this.UIProperties.SetEnabled("VatPercentage", this.ObjectTableName, this.IsScreenEnabled);
        if (this.VatIsMultiPercentage) {
            this.UIProperties.SetEnabled("VatPercentage", this.ObjectTableName, false);
        }
        var isVatPercentageRequired = false;
        if (Tools_1.AppTool.IsNullOrEmpty(this.VatPercentage)) {
            isVatPercentageRequired = true;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.VatTypeId)) {
                if (this.VatIsMultiPercentage) {
                    isVatPercentageRequired = false;
                }
            }
        }
        this.UIProperties.SetRequired("VatPercentage", this.ObjectTableName, isVatPercentageRequired);
    };
    APInvoiceLineItem.prototype.SetUIProperties_Description = function () {
        this.UIProperties.SetRequired("LocalDescription", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.LocalDescription));
    };
    // Line Properties
    APInvoiceLineItem.prototype.RefreshLine = function () {
        //FirePropertyChanged("Exists");
        //FirePropertyChanged("CheckBoxVisibility");
        //FirePropertyChanged("NotMatchedVisibility");
        //FirePropertyChanged("CellBackground");
        //FirePropertyChanged("AmountCellBackground");
        //FirePropertyChanged("CurrencyCellBackground");
        //FirePropertyChanged("IsScreenEnabled");
        //FirePropertyChanged("VendorCellBackground");
        //FirePropertyChanged("EditControlIsEnabled");
        //FirePropertyChanged("OpenAmountIsEnabled");
        this.setColors();
        this.SetUIProperties();
    };
    Object.defineProperty(APInvoiceLineItem.prototype, "EditControlIsEnabled", {
        get: function () {
            var result = true;
            if (!this.IsScreenEnabled) {
                result = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.VendorId) && this.VendorId != this.invoicePM.VendorId) {
                result = false;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "OpenAmountIsEnabled", {
        get: function () {
            var result = true;
            if (this.invoicePM.StatusCode == "VD") {
                result = false;
            }
            else if (this.ExpectedAmount == null) {
                result = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.VendorId) && this.VendorId != this.invoicePM.VendorId) {
                result = false;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "Exists", {
        get: function () {
            var myResult = false;
            if (this.invoicePM.InvoiceLines.indexOf(this.invoiceLinePM) > -1) {
                myResult = true;
            }
            return myResult;
        },
        set: function (newValue) {
            if (newValue == true) {
                this.invoicePM.AddAPInvoiceLinePM(this.invoiceLinePM);
            }
            else {
                this.InvoiceCurrencyAmount = null;
                this.invoicePM.RemoveAPInvoiceLinePM(this.invoiceLinePM);
            }
            this.RefreshLine();
            this.setColors();
            this.fatherComponent.ComputeTotals();
        },
        enumerable: true,
        configurable: true
    });
    APInvoiceLineItem.prototype.setColors = function () {
        this.ReadCellBackground();
        this.ReadAmountCellBackground();
        this.ReadCurrencyCellBackground();
        this.ReadVendorCellBackground();
    };
    APInvoiceLineItem.prototype.ReadCellBackground = function () {
        var myResult = this.CellReadOnlyBackground;
        if (this.Exists) {
            myResult = "rgba(208, 224, 234, 0.4)";
        }
        this.CellBackgroundColor = myResult;
    };
    APInvoiceLineItem.prototype.ReadAmountCellBackground = function () {
        var myResult = "transparent";
        if (!this.IsScreenEnabled) {
            myResult = this.CellReadOnlyBackground;
        }
        else {
            if (this.Exists) {
                myResult = "rgba(208, 224, 234, 0.4)";
            }
            else {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.VendorId) && this.VendorId != this.invoicePM.VendorId) {
                    myResult = this.CellReadOnlyBackground;
                }
                myResult = "transparent";
            }
        }
        this.AmountCellBackground = myResult;
    };
    APInvoiceLineItem.prototype.ReadCurrencyCellBackground = function () {
        this.CurrencyCellBackground = "transparent";
    };
    APInvoiceLineItem.prototype.ReadVendorCellBackground = function () {
        var myResult = "transparent";
        if (this.Exists) {
            myResult = "transparent";
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.VendorId) && this.VendorId != this.invoicePM.VendorId) {
                myResult = "rgba(255, 171, 3, 0.6)";
            }
        }
        this.VendorCellBackground = myResult;
    };
    Object.defineProperty(APInvoiceLineItem.prototype, "NotMatchedVisibility", {
        get: function () {
            var result = false;
            if (!this.Exists) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.VendorId) && this.VendorId != this.invoicePM.VendorId) {
                    result = true;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "CheckBoxVisibility", {
        get: function () {
            var result = false;
            if (this.Exists) {
                result = true;
            }
            else {
                if (Tools_1.AppTool.IsNullOrEmpty(this.VendorId)) {
                    result = true;
                }
                else if (this.VendorId == this.invoicePM.VendorId) {
                    result = true;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "ChargesTypeId", {
        get: function () {
            return this.invoiceLinePM == null ? null : this.invoiceLinePM.ChargesTypeId;
        },
        set: function (value) {
            if (this.invoiceLinePM != null) {
                if (this.invoiceLinePM.ChargesTypeId != value) {
                    this.invoiceLinePM.ChargesTypeId = value;
                }
                this.OnChargeTypeChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    APInvoiceLineItem.prototype.OnChargeTypeChanged = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.ChargesTypeId)) {
            this.ChargesTypeCode = null;
            this.ChargesTypeName = null;
            this.VatTypeId = null;
            this.Description = null;
            this.LocalDescription = null;
            this.Glaccount = null;
        }
        else {
            var chargesTypeService = new ChargesTypeListService_1.ChargesTypeListService();
            chargesTypeService.getSingle(this.ChargesTypeId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.chargesTypeList = myResponse.Result;
                    if (_this.chargesTypeList != null) {
                        _this.ChargesTypeCode = _this.chargesTypeList.Code;
                        _this.ChargesTypeName = _this.chargesTypeList.EnglishName;
                        _this.VatTypeId = _this.chargesTypeList.VatTypeId;
                        _this.Description = _this.chargesTypeList.EnglishName;
                        _this.LocalDescription = _this.chargesTypeList.LocalName;
                        if (!Tools_1.AppTool.IsNullOrEmpty(_this.chargesTypeList.PayableDebitGLAcountId)) {
                            _this.fatherComponent.myGLAccountPMService.get(_this.chargesTypeList.PayableDebitGLAcountId).subscribe(function (myResponse) {
                                if (!myResponse.HasError) {
                                    _this.Glaccount = myResponse.Result;
                                }
                            });
                        }
                    }
                }
            });
        }
    };
    Object.defineProperty(APInvoiceLineItem.prototype, "ChargesTypeCode", {
        get: function () { return this.invoiceLinePM.ChargesTypeCode; },
        set: function (value) {
            if (this.invoiceLinePM.ChargesTypeCode != value) {
                this.invoiceLinePM.ChargesTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "ChargesTypeName", {
        get: function () { return this.invoiceLinePM.ChargesTypeName; },
        set: function (value) {
            if (this.invoiceLinePM.ChargesTypeName != value) {
                this.invoiceLinePM.ChargesTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "Description", {
        get: function () { return this.invoiceLinePM.Description; },
        set: function (value) {
            if (this.invoiceLinePM.Description != value) {
                this.invoiceLinePM.Description = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "LocalDescription", {
        get: function () { return this.invoiceLinePM.LocalDescription; },
        set: function (value) {
            if (this.invoiceLinePM.LocalDescription != value) {
                this.invoiceLinePM.LocalDescription = value;
                this.SetUIProperties_Description();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "VatTypeId", {
        // VAT Type
        get: function () {
            return this.invoiceLinePM == null ? null : this.invoiceLinePM.VatTypeId;
        },
        set: function (value) {
            if (this.invoiceLinePM != null) {
                if (this.invoiceLinePM.VatTypeId != value) {
                    this.invoiceLinePM.VatTypeId = value;
                    this.GetVatTypeData();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    APInvoiceLineItem.prototype.GetVatTypeData = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.VatTypeId)) {
            this.VatTypeName = null;
            this.VatPercentage = null;
            this.VatIsMultiPercentage = false;
            //this.invoiceLinePM.ExternalVATCard = null;
            this.invoiceLinePM.ExternalTAXItemId = null;
            this.ReadVatTypeData();
            this.SetUIProperties_VAT();
        }
        else {
            this.fatherComponent.myVatTypeListService.getSingleFromCache(this.VatTypeId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        _this.VatTypeName = list.EnglishName;
                        _this.VatIsMultiPercentage = list.IsMultiPercentage;
                        //this.invoiceLinePM.ExternalVATCard = list.ExternalVATCard;
                        _this.invoiceLinePM.ExternalTAXItemId = list.ExternalTAXItemId;
                        if (list.IsMultiPercentage) {
                            _this.VatPercentage = null;
                        }
                        else {
                            _this.VatPercentage = _this.fatherComponent.GetVatTypePercentage(_this.VatTypeId);
                        }
                        _this.ReadVatTypeData();
                        _this.SetUIProperties_VAT();
                    }
                }
            });
        }
    };
    APInvoiceLineItem.prototype.SetVatPercentage = function (myPercentage) {
        this.VatPercentage = myPercentage;
    };
    Object.defineProperty(APInvoiceLineItem.prototype, "VatTypeName", {
        get: function () { return this.invoiceLinePM.VatTypeName; },
        set: function (newValue) {
            if (this.invoiceLinePM.VatTypeName != newValue) {
                this.invoiceLinePM.VatTypeName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "VatPercentage", {
        get: function () { return this.invoiceLinePM.VatPercentage; },
        set: function (newValue) {
            if (this.invoiceLinePM.VatPercentage != newValue) {
                this.invoiceLinePM.VatPercentage = Tools_1.AppTool.Round(newValue, 2);
                this.ReadVatTypeData();
                this.ReCalculateTotals();
                this.SetUIProperties_VAT();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "VatIsMultiPercentage", {
        get: function () { return this.invoiceLinePM.VatIsMultiPercentage; },
        set: function (value) {
            if (this.invoiceLinePM.VatIsMultiPercentage != value) {
                this.invoiceLinePM.VatIsMultiPercentage = value;
                this.SetUIProperties_VAT();
            }
        },
        enumerable: true,
        configurable: true
    });
    APInvoiceLineItem.prototype.ReadVatTypeData = function () {
        var _this = this;
        var myValue = null;
        var myColor = Tools_1.FontTool.Black;
        var isUpdateVisible = false;
        var isMultiIconVisible = false;
        this.VatTypesGroups = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.VatTypeId)) {
            if (this.VatIsMultiPercentage) {
                myValue = this.VatTypeName;
                myColor = Tools_1.FontTool.Black;
                isMultiIconVisible = true;
                this.VatTypesGroups = SessionLocator_1.SessionLocator.AllVatTypesGroups.filter(function (f) { return f.GroupVATTypeId == _this.VatTypeId; });
            }
            else if (this.VatPercentage != null) {
                myValue = this.VatTypeName + " (" + this.VatPercentage + "%)";
                myColor = Tools_1.FontTool.Black;
            }
            else {
                myValue = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.S.Details.NoVat");
                myColor = Tools_1.FontTool.Red;
                isUpdateVisible = true;
            }
        }
        this.VatTypeCell = myValue;
        this.VatTypeCellColor = myColor;
        this.VatTypeUpdateIsVisible = isUpdateVisible;
        this.VatTypeMultiIconVisible = isMultiIconVisible;
    };
    Object.defineProperty(APInvoiceLineItem.prototype, "ExpectedAmount", {
        //Amounts
        get: function () {
            return this.invoiceLinePM.ExpectedAmount;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "OtherInvoicesAmounts", {
        get: function () {
            return this.invoiceLinePM.OtherInvoicesAmounts == null ? 0 : this.invoiceLinePM.OtherInvoicesAmounts;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "CorrectionIconVisibility", {
        get: function () {
            var result = false;
            if (this.CorrectionAmount < 0 || this.CorrectionAmount > 0) {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "CorrectionForeground", {
        get: function () {
            var myResult = "#282E30";
            if (this.CorrectionAmount != null) {
                if (this.CorrectionAmount > 0) {
                    myResult = "Orange";
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "CorrectionAmount", {
        get: function () {
            return this.invoiceLinePM.CorrectionAmount;
        },
        set: function (value) {
            if (this.invoiceLinePM.CorrectionAmount != value) {
                this.invoiceLinePM.CorrectionAmount = Tools_1.AppTool.Round(value, 2);
                this.CorrectionByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                this.CorrectionDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "CorrectionByUserId", {
        get: function () {
            return this.invoiceLinePM.CorrectionByUserId;
        },
        set: function (value) {
            if (this.invoiceLinePM.CorrectionByUserId != value) {
                this.invoiceLinePM.CorrectionByUserId = value;
                this.GetUserName();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "CorrectionDate", {
        //get CorrectionByUserName() {
        //    return this.connectionUserName;
        //}
        get: function () {
            return this.invoiceLinePM.CorrectionDate;
        },
        set: function (value) {
            if (this.invoiceLinePM.CorrectionDate != value) {
                this.invoiceLinePM.CorrectionDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "CorrectionNote", {
        get: function () {
            return this.invoiceLinePM.CorrectionNote;
        },
        set: function (value) {
            if (this.invoiceLinePM.CorrectionNote != value) {
                this.invoiceLinePM.CorrectionNote = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "InvoiceCurrencyCode", {
        get: function () {
            return this.fatherComponent.InvoiceCurrencyCode;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "OpenAmount", {
        //set InvoiceCurrencyCode(newValue: string) {
        //    if (this.invoiceLinePM.InvoiceCurrencyCode != newValue) {
        //        this.invoiceLinePM.InvoiceCurrencyCode = newValue;
        //    }
        //}
        get: function () { return this.invoiceLinePM.OpenAmount; },
        set: function (value) {
            var xValue = value == null ? 0 : value;
            if (this.invoiceLinePM.OpenAmount != value) {
                this.invoiceLinePM.OpenAmount = Tools_1.AppTool.Round(value, 2);
                var expect = this.ExpectedAmount == null ? 0 : this.ExpectedAmount;
                var amount = this.ForiegnCurrencyAmount == null ? 0 : this.ForiegnCurrencyAmount;
                var others = this.OtherInvoicesAmounts == null ? 0 : this.OtherInvoicesAmounts;
                var corre = expect - others - amount - xValue;
                this.CorrectionAmount = corre;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "InvoiceCurrencyAmount", {
        get: function () { return this.invoiceLinePM.InvoiceCurrencyAmount; },
        set: function (value) {
            if (this.invoiceLinePM.InvoiceCurrencyAmount != value) {
                this.invoiceLinePM.InvoiceCurrencyAmount = Tools_1.AppTool.Round(value, 2);
                this.OnInvoiceCurrencyAmountChanged(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    APInvoiceLineItem.prototype.OnInvoiceCurrencyAmountChanged = function (value) {
        var valueInLocal = value * this.invoicePM.InvoiceCurrencyExchangeRate;
        var foriegnAmount = valueInLocal / this.invoiceLinePM.ForiegnExchangeRate;
        this.invoiceLinePM.ForiegnCurrencyAmount = Tools_1.AppTool.Round(foriegnAmount, 2);
        if (!this.AddNewLineMode) {
            this.Exists = foriegnAmount < 0 || foriegnAmount > 0;
        }
        this.ComputeOpenAmount();
        this.LocalCurrencyAmount = valueInLocal;
        if (this.ForiegnCurrencyId == this.ProfitCurrencyId) {
            this.ProfitCurrencyAmount = this.ForiegnCurrencyAmount;
        }
        else {
            this.ProfitCurrencyAmount = this.LocalCurrencyAmount / this.invoicePM.ProfitCurrencyExchangeRate;
        }
        this.ReCalculateTotals();
    };
    Object.defineProperty(APInvoiceLineItem.prototype, "ForiegnCurrencyAmount", {
        get: function () { return this.invoiceLinePM.ForiegnCurrencyAmount; },
        set: function (value) {
            if (this.invoiceLinePM.ForiegnCurrencyAmount != value) {
                this.invoiceLinePM.ForiegnCurrencyAmount = Tools_1.AppTool.Round(value, 2);
                this.ComputeOpenAmount();
                this.ComputeOtherAmounts();
                if (!this.AddNewLineMode) {
                    this.Exists = value < 0 || value > 0;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    APInvoiceLineItem.prototype.OnInvoiceExchangeRateChanged = function () {
        if (this.ForiegnCurrencyId == this.fatherComponent.InvoiceCurrencyId) {
            this.ForiegnExchangeRate = this.fatherComponent.InvoiceCurrencyExchangeRate;
        }
        else {
            this.ForiegnExchangeRate = this.fatherComponent.GetCurrencyRate(this.ForiegnCurrencyId);
        }
        this.ComputeOtherAmounts();
    };
    APInvoiceLineItem.prototype.ComputeOtherAmounts = function () {
        var invoiceAmount = 0;
        this.LocalCurrencyAmount = this.ForiegnCurrencyAmount * this.ForiegnExchangeRate;
        if (this.ForiegnCurrencyId == this.ProfitCurrencyId) {
            this.ProfitCurrencyAmount = this.ForiegnCurrencyAmount;
        }
        else {
            this.ProfitCurrencyAmount = this.LocalCurrencyAmount / this.invoicePM.ProfitCurrencyExchangeRate;
        }
        if (this.ForiegnCurrencyId == this.InvoiceCurrencyId) {
            invoiceAmount = this.ForiegnCurrencyAmount;
        }
        else {
            invoiceAmount = this.LocalCurrencyAmount / this.invoicePM.InvoiceCurrencyExchangeRate;
        }
        this.invoiceLinePM.InvoiceCurrencyAmount = Tools_1.AppTool.Round(invoiceAmount, 2);
        this.ReCalculateTotals();
    };
    APInvoiceLineItem.prototype.ComputeOpenAmount = function () {
        if (this.invoiceLinePM.AmountTypeCode == "NEXP") {
            this.invoiceLinePM.OpenAmount = null;
        }
        else {
            var expect = this.ExpectedAmount;
            var amount = this.ForiegnCurrencyAmount == null ? 0 : this.ForiegnCurrencyAmount;
            var others = this.OtherInvoicesAmounts == null ? 0 : this.OtherInvoicesAmounts;
            var corre = this.CorrectionAmount == null ? 0 : this.CorrectionAmount;
            var open = expect - others - amount - corre;
            this.invoiceLinePM.OpenAmount = Tools_1.AppTool.Round(open, 2);
        }
    };
    Object.defineProperty(APInvoiceLineItem.prototype, "LocalCurrencyAmount", {
        get: function () { return this.invoiceLinePM.LocalCurrencyAmount; },
        set: function (value) {
            if (this.invoiceLinePM.LocalCurrencyAmount != value) {
                this.invoiceLinePM.LocalCurrencyAmount = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "ProfitCurrencyAmount", {
        get: function () { return this.invoiceLinePM.ProfitCurrencyAmount; },
        set: function (value) {
            if (this.invoiceLinePM.ProfitCurrencyAmount != value) {
                this.invoiceLinePM.ProfitCurrencyAmount = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "VendorId", {
        // Properties
        get: function () {
            return this.invoiceLinePM.VendorId;
        },
        set: function (value) {
            if (this.invoiceLinePM.VendorId != value) {
                this.invoiceLinePM.VendorId = value;
                this.getVendorCardData();
                this.ReadVendorCellBackground();
            }
        },
        enumerable: true,
        configurable: true
    });
    APInvoiceLineItem.prototype.getVendorCardData = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.VendorId)) {
            this.VendorName = null;
        }
        else {
            var myCardListService = new CardListService_1.CardListService();
            myCardListService.getSingle(this.VendorId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list == null) {
                        _this.VendorName = list.EnglishName;
                    }
                }
            });
        }
    };
    Object.defineProperty(APInvoiceLineItem.prototype, "VendorName", {
        get: function () {
            return this.invoiceLinePM.VendorName;
        },
        set: function (value) {
            if (this.invoiceLinePM.VendorName != value) {
                this.invoiceLinePM.VendorName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "Notes", {
        get: function () {
            return this.invoiceLinePM.Notes;
        },
        set: function (value) {
            if (this.invoiceLinePM.Notes != value) {
                this.invoiceLinePM.Notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineItem.prototype, "NotesIconVisibility", {
        get: function () {
            var result = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.invoiceLinePM.Notes)) {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    APInvoiceLineItem.prototype.UpdateVatPercentageClicked = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.VatTypeId)) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 350;
            logWindow.Title = "Add VAT Type Percentage";
            logWindow.WindowArgs = this.VatTypeId;
            logWindow.ComponentLoaded.subscribe(function (comp) {
                logWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.fatherComponent.ItemsSource.Collection.filter(function (f) { return f.VatTypeId == _this.VatTypeId; }).forEach(function (item) {
                            item.SetVatPercentage(comp.Percentage);
                        });
                    }
                });
            });
            logWindow.Show('./CommonModules/CommonOthers/Components/UpdateVATPercentage/UpdateVATPercentageComponent');
        }
    };
    return APInvoiceLineItem;
}(BaseComponent_1.BaseComponent));
exports.APInvoiceLineItem = APInvoiceLineItem;
//# sourceMappingURL=APInvoiceDetailsTabGeneral.js.map