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
var ConstituentPM_1 = require("../../../../Invoice/EntityPMs/ConstituentPM");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var CurrencyRatesService_1 = require("../../../../Common/Services/CurrencyRatesService");
var Args_1 = require("../../../../Invoice/Args");
var Tools_2 = require("../../../../Invoice/Tools");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var PaymentTermListService_1 = require("../../../../Common/Services/StandardLists/PaymentTermListService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var NumbersPipe_1 = require("../../../../Infrastructure/Pipes/NumbersPipe");
var InvoiceDomainService_1 = require("../../../../Invoice/Services/InvoiceDomainService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var ARInvoiceListService_1 = require("../../../../Invoice/Services/StandardLists/ARInvoiceListService");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var CodeNameClass_1 = require("../../../../Infrastructure/DataContracts/CodeNameClass");
var ARInvoiceDetailsTabConsolidation = /** @class */ (function (_super) {
    __extends(ARInvoiceDetailsTabConsolidation, _super);
    function ARInvoiceDetailsTabConsolidation(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.ObjectTableName = "ARInvoice";
        _this.DataContext = _this;
        _this.ItemsSource = [];
        _this.EntityWarningsList = [];
        _this.EntityWarning = "";
        _this.IsEditExchangeRateVisible = false;
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.InvoiceNumberFilterList = [];
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        // SetUIProperties
        _this.IsEditingEnabled = false;
        _this.RateIsEnabled = false;
        _this.AllowManualInvoiceNumber = false;
        _this.AllowStockInvoiceNumber = false;
        _this.PaymentTermDisplayInLOV = true;
        // Bill To
        _this.BillToDependencyValue1 = null;
        _this.myRelativeRateDate = null;
        // Load Date 
        _this.LastRatesList = [];
        _this.IsNoDataTextVisible = false;
        _this.AllConnectedInvoices = [];
        _this.isByInvoiceDate = true;
        _this.isByCreateDate = false;
        _this.isTotalInLocalCurrency = false;
        _this.TotalsList = [];
        _this.SummaryItems = [];
        _this.isInvoiceNumberComboBoxEnabled = true;
        _this.IsgetFromStockAfterSaving = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.EntityPM = entityArgs.EntityPM;
        _this.LocalCurrencyId = SessionLocator_1.SessionLocator.LocalCurrencyId;
        _this.LocalCurrencyCode = SessionLocator_1.SessionLocator.LocalCurrencyCode;
        _this.InitializeServices();
        _this.InitializeComponent();
        _this.SetUIProperties();
        _this.BuildScreenData();
        _this.Listen();
        if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.TransmissionError)) {
            _this.EntityWarningsList.push(_this.EntityPM.TransmissionError);
            _this.EntityWarning = _this.EntityPM.TransmissionError;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            _this.IsEditExchangeRateVisible = true;
        }
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.EnableInvoiceStocksManagement) {
            _this.AllowStockInvoiceNumber = true;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(_this.ARInvoiceStockId)) {
            _this.IsInvoiceNumberComboBoxEnabled = false;
        }
        _this.BuildInvoiceNumberFilters();
        return _this;
    }
    ARInvoiceDetailsTabConsolidation.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.LoadInvoices();
                }
                else {
                    if (_this.IsgetFromStockAfterSaving) {
                        _this.InvoiceNumber = null;
                        _this.ARInvoiceStockId = null;
                        _this.IsInvoiceNumberComboBoxEnabled = true;
                    }
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.LoadInvoices();
                }
            });
        }
    };
    ARInvoiceDetailsTabConsolidation.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    ARInvoiceDetailsTabConsolidation.prototype.InitializeServices = function () {
        this.myCardListService = new CardListService_1.CardListService();
        this.myCurrencyListService = new CurrencyListService_1.CurrencyListService();
        this.myPaymentTermListService = new PaymentTermListService_1.PaymentTermListService();
        this.myInvoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        this.myARInvoiceListService = new ARInvoiceListService_1.ARInvoiceListService();
    };
    ARInvoiceDetailsTabConsolidation.prototype.InitializeComponent = function () {
        this.ComputeRelativeRateDate();
        this.BillToDependencyValue1 = Tools_2.InvoiceTool.GetBillToPartnerTypes();
    };
    ARInvoiceDetailsTabConsolidation.prototype.SetUIProperties = function () {
        var isEditingEnabled = Tools_2.InvoiceTool.IsEditingARInvoiceEnabled(this.EntityPM);
        this.UIProperties.SetEnabled("PaymentTermId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("VatNumber", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("InvoiceDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("FromDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("ToDate", this.ObjectTableName, isEditingEnabled);
        // Generated General Tab
        if (this.EntityPM != null) {
            this.EntityPM.UIProperties.SetEnabled("UpdateDate", this.ObjectTableName, false);
            this.EntityPM.UIProperties.SetEnabled("UpdatedByUserId", this.ObjectTableName, false);
            this.EntityPM.UIProperties.SetEnabled("Sent", this.ObjectTableName, isEditingEnabled);
            this.EntityPM.UIProperties.SetEnabled("HouseNumber", this.ObjectTableName, isEditingEnabled);
            this.EntityPM.UIProperties.SetEnabled("MasterNumber", this.ObjectTableName, isEditingEnabled);
            this.EntityPM.UIProperties.SetEnabled("CustomerRef", this.ObjectTableName, isEditingEnabled);
            this.EntityPM.UIProperties.SetEnabled("BranchId", this.ObjectTableName, isEditingEnabled);
            this.EntityPM.UIProperties.SetEnabled("SATPaymentMethodCode", this.ObjectTableName, isEditingEnabled);
            //this.EntityPM.UIProperties.SetEnabled("SalesmanUserId", this.ObjectTableName, isEditingEnabled);
        }
        this.IsEditingEnabled = isEditingEnabled;
        this.SetUIProperties_Connected();
        this.SetUIProperties_BillToAddress();
        this.SetUIProperties_VatNumber();
        this.SetUIProperties_ExchangeRate();
        this.SetUIProperties_PrintNotes();
        this.SetUIProperties_ManuallySet();
        this.SetUIProperties_InvoiceNumber();
        this.SetUIProperties_DueDate();
    };
    ARInvoiceDetailsTabConsolidation.prototype.SetUIProperties_Connected = function () {
        var isFieldtEnabled = false;
        if (this.IsEditingEnabled) {
            if (this.EntityPM.ConstituentInvoices.length == 0) {
                isFieldtEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, isFieldtEnabled);
        this.UIProperties.SetEnabled("InvoiceCurrencyId", this.ObjectTableName, isFieldtEnabled);
    };
    ARInvoiceDetailsTabConsolidation.prototype.SetUIProperties_BillToAddress = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.BillToAddressId)) {
            var isFieldtEnabled = false;
            if (this.IsEditingEnabled) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.BillToId)) {
                    isFieldtEnabled = true;
                }
            }
            this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, isFieldtEnabled);
        }
    };
    ARInvoiceDetailsTabConsolidation.prototype.SetUIProperties_VatNumber = function () {
        var isFieldRequired = false;
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAR) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.VatNumber)) {
                isFieldRequired = true;
            }
            this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, isFieldRequired);
        }
    };
    ARInvoiceDetailsTabConsolidation.prototype.SetUIProperties_ExchangeRate = function () {
        var isFieldEnabled = false;
        if (this.IsEditingEnabled) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "ARInvoiceEditExchangeRate")) {
                if (this.InvoiceCurrencyId) {
                    if (this.InvoiceCurrencyId != SessionLocator_1.SessionLocator.TenantPM.CurrencyId) {
                        isFieldEnabled = true;
                    }
                }
            }
        }
        this.RateIsEnabled = isFieldEnabled;
        this.UIProperties.SetEnabled("InvoiceCurrencyExchangeRate", this.ObjectTableName, isFieldEnabled);
    };
    ARInvoiceDetailsTabConsolidation.prototype.SetUIProperties_PrintNotes = function () {
        var isFieldtEnabled = true;
        if (this.EntityPM.StatusCode == "VD") {
            isFieldtEnabled = false;
        }
        else if (this.EntityPM.IsPrinted) {
            isFieldtEnabled = false;
        }
        this.UIProperties.SetEnabled("PrintNotes", this.ObjectTableName, isFieldtEnabled);
    };
    ARInvoiceDetailsTabConsolidation.prototype.SetUIProperties_ManuallySet = function () {
        var isFieldtEnabled = this.IsEditingEnabled;
        var isFieldtVisible = false;
        if (this.IsInvoiceNumberManuallySet) {
            isFieldtVisible = true;
        }
        else if (SessionLocator_1.SessionLocator.AccountingSettingPM.AllowManualInvoiceNumber) {
            isFieldtVisible = true;
        }
        this.AllowManualInvoiceNumber = isFieldtVisible;
        this.UIProperties.SetEnabled("IsInvoiceNumberManuallySet", this.ObjectTableName, isFieldtEnabled);
        this.UIProperties.SetVisibility("IsInvoiceNumberManuallySet", this.ObjectTableName, isFieldtVisible);
    };
    ARInvoiceDetailsTabConsolidation.prototype.SetUIProperties_InvoiceNumber = function () {
        var isFieldEnabled = false;
        if (this.IsEditingEnabled) {
            if (this.IsInvoiceNumberManuallySet) {
                isFieldEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("InvoiceNumber", this.ObjectTableName, isFieldEnabled);
    };
    ARInvoiceDetailsTabConsolidation.prototype.SetUIProperties_DueDate = function () {
        var AllowManuallyDueDate = false;
        if (SessionLocator_1.SessionLocator.AccountingSystemPM) {
            AllowManuallyDueDate = SessionLocator_1.SessionLocator.AccountingSystemPM.AllowManuallyDueDate;
        }
        if (AllowManuallyDueDate) {
            this.PaymentTermDisplayInLOV = null;
        }
        if (!this.IsEditingEnabled) {
            AllowManuallyDueDate = false;
        }
        this.UIProperties.SetEnabled("DueDate", this.ObjectTableName, AllowManuallyDueDate);
    };
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "BillToId", {
        get: function () { return this.EntityPM.BillToId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.BillToId != newValue) {
                this.EntityPM.BillToId = newValue;
                this.EntityPM.CustomerRef = null;
                this.SetUIProperties_BillToAddress();
                this.LoadInvoices();
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.VatNumber = null;
                    this.BillToName = null;
                    this.BillToAddressId = null;
                    this.InvoiceCurrencyId = SessionLocator_1.SessionLocator.AccountingCurrencyId;
                    this.PaymentTermId = SessionLocator_1.SessionLocator.TenantPM.PaymentTermId;
                    this.EntityPM.IsBillToAllowConsolidation = false;
                    this.EntityPM.BillToIsCreditLimitEnabled = false;
                    this.EntityPM.BillToCreditLimitAmount = null;
                    this.EntityPM.BillToCreditLimitOpenBalance = null;
                    this.EntityPM.BillToCreditLimitWarningPercentage = null;
                    this.EntityPM.BillToBlockNewInvoiceCreation = false;
                }
                else {
                    this.myCardListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.VatNumber = list.VatNumber;
                                _this.BillToName = list.EnglishName;
                                _this.EntityPM.IsBillToAllowConsolidation = list.EnableConsolidationInvoices;
                                _this.EntityPM.BillToIsCreditLimitEnabled = list.IsCreditLimitEnabled;
                                _this.EntityPM.BillToCreditLimitAmount = list.CreditLimitAmount;
                                _this.EntityPM.BillToCreditLimitOpenBalance = list.CreditLimitOpenBalance;
                                _this.EntityPM.BillToCreditLimitWarningPercentage = list.CreditLimitWarningPercentage;
                                _this.EntityPM.BillToBlockNewInvoiceCreation = list.BlockNewInvoiceCreation;
                                _this.EntityPM.SalesmanUserId = list.SalesmanUserId;
                                if (!Tools_1.AppTool.IsNullOrEmpty(list.InvoiceCurrencyId)) {
                                    _this.InvoiceCurrencyId = list.InvoiceCurrencyId;
                                }
                                if (!Tools_1.AppTool.IsNullOrEmpty(list.PaymentTermId)) {
                                    _this.PaymentTermId = list.PaymentTermId;
                                }
                                if (!Tools_1.AppTool.IsNullOrEmpty(list.BillingAddressId)) {
                                    _this.BillToAddressId = list.BillingAddressId;
                                }
                                else if (!Tools_1.AppTool.IsNullOrEmpty(list.MainAddressId)) {
                                    _this.BillToAddressId = list.MainAddressId;
                                }
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "BillToName", {
        get: function () { return this.EntityPM.BillToName; },
        set: function (newValue) {
            if (this.EntityPM.BillToName != newValue) {
                this.EntityPM.BillToName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "BillToAddressId", {
        get: function () { return this.EntityPM.BillToAddressId; },
        set: function (newValue) {
            if (this.EntityPM.BillToAddressId != newValue) {
                this.EntityPM.BillToAddressId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "InvoiceCurrencyId", {
        // Currency
        get: function () { return this.EntityPM.InvoiceCurrencyId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.InvoiceCurrencyId != newValue) {
                this.EntityPM.InvoiceCurrencyId = newValue;
                this.SetUIProperties_ExchangeRate();
                this.ItemsSource.forEach(function (item) {
                    item.SetIsMatch();
                });
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.InvoiceCurrencyCode = null;
                    this.SetCurrencyRateData();
                }
                else {
                    this.myCurrencyListService.getSingleFromCache(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.InvoiceCurrencyCode = list.Code;
                                _this.SetCurrencyRateData();
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "InvoiceCurrencyCode", {
        get: function () { return this.EntityPM.InvoiceCurrencyCode; },
        set: function (value) {
            if (this.EntityPM.InvoiceCurrencyCode != value) {
                this.EntityPM.InvoiceCurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "InvoiceCurrencyExchangeRate", {
        get: function () { return this.EntityPM.InvoiceCurrencyExchangeRate; },
        set: function (value) {
            var setValue = Tools_1.AppTool.Round(value, 5);
            if (this.EntityPM.InvoiceCurrencyExchangeRate != setValue) {
                this.EntityPM.InvoiceCurrencyExchangeRate = setValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "ExchangeRateDate", {
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
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "RelativeRateDate", {
        get: function () { return this.myRelativeRateDate; },
        set: function (value) {
            if (this.myRelativeRateDate != value) {
                this.myRelativeRateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceDetailsTabConsolidation.prototype.ComputeRelativeRateDate = function () {
        this.RelativeRateDate = Tools_1.DateTool.GetRelativeRateDate(this.InvoiceDate, this.ExchangeRateDate, "old");
    };
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "ProfitCurrencyId", {
        get: function () { return this.EntityPM.ProfitCurrencyId; },
        set: function (newValue) {
            if (this.EntityPM.ProfitCurrencyId != newValue) {
                this.EntityPM.ProfitCurrencyId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "ProfitCurrencyExchangeRate", {
        get: function () { return this.EntityPM.ProfitCurrencyExchangeRate; },
        set: function (newValue) {
            if (this.EntityPM.ProfitCurrencyExchangeRate != newValue) {
                this.EntityPM.ProfitCurrencyExchangeRate = Tools_1.AppTool.Round(newValue, 5);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "VatNumber", {
        // Properties
        get: function () { return this.EntityPM.VatNumber; },
        set: function (newValue) {
            if (this.EntityPM.VatNumber != newValue) {
                this.EntityPM.VatNumber = newValue;
                this.SetUIProperties_VatNumber();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "PaymentTermId", {
        get: function () { return this.EntityPM.PaymentTermId; },
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
                Tools_2.InvoiceTool.ComputeARInvoiceDueDate(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "PaymentTermName", {
        get: function () { return this.EntityPM.PaymentTermName; },
        set: function (newValue) {
            if (this.EntityPM.PaymentTermName != newValue) {
                this.EntityPM.PaymentTermName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "InvoiceDate", {
        get: function () { return this.EntityPM.InvoiceDate; },
        set: function (newValue) {
            if (this.EntityPM.InvoiceDate != newValue) {
                this.EntityPM.InvoiceDate = newValue;
                Tools_2.InvoiceTool.ComputeARInvoiceDueDate(this.EntityPM);
                this.ComputeRelativeRateDate();
                this.UpdateData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "DueDate", {
        get: function () { return this.EntityPM.DueDate; },
        set: function (newValue) {
            if (this.EntityPM.DueDate != newValue) {
                this.EntityPM.DueDate = newValue;
                Tools_2.InvoiceTool.ComputeARInvoicePaymentTerm(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "DraftNumber", {
        get: function () { return this.EntityPM.DraftNumber; },
        set: function (newValue) {
            if (this.EntityPM.DraftNumber != newValue) {
                this.EntityPM.DraftNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "StatusCode", {
        get: function () { return this.EntityPM.StatusCode; },
        set: function (newValue) {
            if (this.EntityPM.StatusCode != newValue) {
                this.EntityPM.StatusCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "PrintNotes", {
        get: function () { return this.EntityPM.PrintNotes; },
        set: function (newValue) {
            if (this.EntityPM.PrintNotes != newValue) {
                this.EntityPM.PrintNotes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceDetailsTabConsolidation.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        if (this.myCurrencyRatesService == null) {
            this.myCurrencyRatesService = new CurrencyRatesService_1.CurrencyRatesService();
        }
        var loadingDate = this.EntityPM.InvoiceDate;
        if (loadingDate == null) {
            loadingDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
        this.myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator_1.SessionLocator.AccountingCurrencyId, loadingDate).subscribe(function (myResponse1) {
            if (myResponse1.HasError) {
                _this.CurrentSession.StopBusyIndicator();
            }
            else {
                _this.LastRatesList = myResponse1.Result;
                _this.LoadInvoices();
                _this.CurrentSession.StopBusyIndicator();
                _this.CheckNotifyPastDateOnInvoiceEdit();
            }
        });
    };
    ARInvoiceDetailsTabConsolidation.prototype.UpdateData = function () {
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
                _this.ProfitCurrencyExchangeRate = _this.GetCurrencyRate(_this.EntityPM.ProfitCurrencyId);
                _this.SetCurrencyRateData();
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    ARInvoiceDetailsTabConsolidation.prototype.SetCurrencyRateData = function () {
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
    ARInvoiceDetailsTabConsolidation.prototype.GetCurrencyRate = function (currencyId) {
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
    ARInvoiceDetailsTabConsolidation.prototype.GetCurrencyRateDate = function (currencyId) {
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
    ARInvoiceDetailsTabConsolidation.prototype.UpdateCurrencyRateClicked = function () {
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
    ARInvoiceDetailsTabConsolidation.prototype.CheckNotifyPastDateOnInvoiceEdit = function () {
        var _this = this;
        if (this.IsEditingEnabled) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                if (SessionLocator_1.SessionLocator.AccountingSettingPM.NotifyPastDateOnInvoiceEdit) {
                    if (Tools_1.DateTool.GetDateParts(this.EntityPM.InvoiceDate).DateTicks != Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateAsUtc()).DateTicks) {
                        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                        var message = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.UpdateInvoiceDate");
                        if (message.indexOf("%Date") > -1) {
                            message = message.replace("%Date", Tools_1.DateTool.GetDateFormats(this.InvoiceDate).ShortDateString);
                        }
                        confirmWindow.Show(message);
                        confirmWindow.WindowClosed.subscribe(function (event) {
                            if (confirmWindow.Yes) {
                                _this.InvoiceDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                            }
                        });
                    }
                }
            }
        }
    };
    ARInvoiceDetailsTabConsolidation.prototype.BuildScreenData = function () {
        if (this.IsEditingEnabled) {
            this.LoadData();
        }
        else {
            this.LoadInvoices();
        }
    };
    ARInvoiceDetailsTabConsolidation.prototype.LoadInvoices = function () {
        var _this = this;
        this.ItemsSource = [];
        this.IsNoDataTextVisible = false;
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            this.LoadOtherInvoices();
        }
        else {
            var filters = new ApiQueryFilters_1.ApiQueryFilters();
            filters.PageIndex = 0;
            filters.PageSize = 1000;
            filters.addAdditionalFilter("BillToId", this.BillToId, null, null, "Equals", false, false, false, "string");
            filters.addAdditionalFilter("StatusCode", "CN", null, null, "Equals", false, true, false, "string");
            filters.addAdditionalFilter("IsConstituentInvoice", true, null, null, "Equals", false, false, false, "Boolean");
            filters.addAdditionalFilter("ConsolidationInvoiceId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
            this.myARInvoiceListService.getByFilters(filters).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.AllConnectedInvoices = myResponse.Result;
                    _this.AllConnectedInvoices = _this.AllConnectedInvoices.sort(function (a, b) { return a.InvoiceNumber == b.InvoiceNumber ? 0 : a.InvoiceNumber < b.InvoiceNumber ? -1 : 1; });
                    _this.LoadOtherInvoices();
                }
            });
        }
    };
    ARInvoiceDetailsTabConsolidation.prototype.LoadOtherInvoices = function () {
        var _this = this;
        this.ItemsSource = [];
        this.IsNoDataTextVisible = false;
        this.AllConnectedInvoices.forEach(function (item) {
            _this.ItemsSource.push(new SubInvoiceLine(item, _this));
        });
        if (this.IsEditingEnabled) {
            var filters = new ApiQueryFilters_1.ApiQueryFilters();
            filters.PageIndex = 0;
            filters.PageSize = 1000;
            filters.addAdditionalFilter("BillToId", this.BillToId, null, null, "Equals", false, false, false, "string");
            filters.addAdditionalFilter("StatusCode", "NT", null, null, "Equals", false, true, false, "string");
            filters.addAdditionalFilter("IsConstituentInvoice", true, null, null, "Equals", false, false, false, "Boolean");
            filters.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "Boolean");
            filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
            // Type Filter
            if (this.EntityPM.ARInvoiceTypeCode == "CD") {
                if (SessionLocator_1.SessionLocator.AccountingSettingPM.AllowPositiveAmountsInTheCreditNote) {
                    filters.addAdditionalFilter("ARInvoiceTypeCode", "IN,CD", null, null, "InList", false, true, false, "string");
                }
                else {
                    filters.addAdditionalFilter("ARInvoiceTypeCode", "CD", null, null, "Equals", false, true, false, "string");
                }
            }
            else {
                if (SessionLocator_1.SessionLocator.AccountingSettingPM.AllowMinusInvoicelines) {
                    filters.addAdditionalFilter("ARInvoiceTypeCode", "IN,CD", null, null, "InList", false, true, false, "string");
                }
                else {
                    filters.addAdditionalFilter("ARInvoiceTypeCode", "IN", null, null, "Equals", false, true, false, "string");
                }
            }
            // Date Filter
            var myFilterField = this.IsByInvoiceDate ? "InvoiceDate" : "CreateDate";
            if (!Tools_1.AppTool.IsNullOrEmpty(this.FromDate) && !Tools_1.AppTool.IsNullOrEmpty(this.ToDate)) {
                var myToDate = Tools_1.DateTool.GetDateParts(this.ToDate).DateObject;
                myToDate.setUTCHours(23);
                myToDate.setUTCMinutes(59);
                myToDate.setUTCSeconds(59);
                filters.addAdditionalFilter(myFilterField, this.FromDate, myToDate, null, "Between", false, true, false, "date");
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.FromDate)) {
                var myFromDate = Tools_1.DateTool.GetDateParts(this.FromDate).DateObject;
                filters.addAdditionalFilter(myFilterField, this.FromDate, null, null, "GreaterThanOrEqual", false, true, false, "date");
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.ToDate)) {
                var myToDate = Tools_1.DateTool.GetDateParts(this.ToDate).DateObject;
                myToDate.setUTCHours(23);
                myToDate.setUTCMinutes(59);
                myToDate.setUTCSeconds(59);
                filters.addAdditionalFilter(myFilterField, myToDate, null, null, "LessThanOrEqual", false, true, false, "date");
            }
            this.myARInvoiceListService.getByFilters(filters).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    list = list.sort(function (a, b) { return a.InvoiceNumber == b.InvoiceNumber ? 0 : a.InvoiceNumber < b.InvoiceNumber ? -1 : 1; });
                    list.filter(function (f) { return f.InvoiceCurrencyId == _this.InvoiceCurrencyId; }).forEach(function (item) {
                        _this.ItemsSource.push(new SubInvoiceLine(item, _this));
                    });
                    list.filter(function (f) { return f.InvoiceCurrencyId != _this.InvoiceCurrencyId; }).forEach(function (item) {
                        _this.ItemsSource.push(new SubInvoiceLine(item, _this));
                    });
                }
                if (_this.ItemsSource.length == 0) {
                    _this.IsNoDataTextVisible = true;
                }
                _this.BuildTotalsCollection();
            });
        }
        else {
            if (this.ItemsSource.length == 0) {
                this.IsNoDataTextVisible = true;
            }
            this.BuildTotalsCollection();
        }
    };
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "FromDate", {
        get: function () { return this.fromDate; },
        set: function (value) {
            if (this.fromDate != value) {
                this.fromDate = value;
                this.LoadOtherInvoices();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "ToDate", {
        get: function () { return this.toDate; },
        set: function (value) {
            if (this.toDate != value) {
                this.toDate = value;
                this.LoadOtherInvoices();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "IsByInvoiceDate", {
        get: function () { return this.isByInvoiceDate; },
        set: function (value) {
            if (this.isByInvoiceDate != value) {
                this.isByInvoiceDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "IsByCreateDate", {
        get: function () { return this.isByCreateDate; },
        set: function (value) {
            if (this.isByCreateDate != value) {
                this.isByCreateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceDetailsTabConsolidation.prototype.SetByDateFilter = function (myCode) {
        this.IsByInvoiceDate = false;
        this.IsByCreateDate = false;
        if (myCode == "IN") {
            this.IsByInvoiceDate = true;
        }
        else {
            this.IsByCreateDate = true;
        }
        this.LoadOtherInvoices();
    };
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "IsCurrencyFilterVisible", {
        // Totals
        get: function () {
            var myResult = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
                if (this.LocalCurrencyId != this.InvoiceCurrencyId) {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "IsTotalInLocalCurrency", {
        get: function () { return this.isTotalInLocalCurrency; },
        set: function (value) {
            if (this.isTotalInLocalCurrency != value) {
                this.isTotalInLocalCurrency = value;
                this.BuildTotalsControl();
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceDetailsTabConsolidation.prototype.ComputeTotals = function () {
        this.BuildTotalsCollection(true);
    };
    ARInvoiceDetailsTabConsolidation.prototype.BuildTotalsCollection = function (isComputingTotals) {
        if (isComputingTotals === void 0) { isComputingTotals = false; }
        var totalsList = [];
        var myDataList = this.ItemsSource.filter(function (f) { return f.IsConnected == true; });
        var subTotalItem = new Args_1.InvoiceTotalsClass();
        var vatTotalItem = new Args_1.InvoiceTotalsClass();
        var allTotalItem = new Args_1.InvoiceTotalsClass();
        subTotalItem.RowLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.S.Details.Subtotal");
        vatTotalItem.RowLabel = "VAT";
        allTotalItem.RowLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.F.AmountInInvoiceCurrency");
        subTotalItem.LocalCurrencyAmount = Tools_1.ArrayTool.Sum(myDataList, "SubTotalInLocalCurrency");
        subTotalItem.InvoiceCurrencyAmount = Tools_1.ArrayTool.Sum(myDataList, "SubTotalInInvoiceCurrency");
        totalsList.push(subTotalItem);
        vatTotalItem.LocalCurrencyAmount = Tools_1.ArrayTool.Sum(myDataList, "VATAmountInLocalCurrency");
        vatTotalItem.InvoiceCurrencyAmount = Tools_1.ArrayTool.Sum(myDataList, "VATAmountInInvoiceCurrency");
        totalsList.push(vatTotalItem);
        allTotalItem.LocalCurrencyAmount = Tools_1.ArrayTool.Sum(myDataList, "AmountInLocalCurrency");
        allTotalItem.InvoiceCurrencyAmount = Tools_1.ArrayTool.Sum(myDataList, "AmountInInvoiceCurrency");
        totalsList.push(allTotalItem);
        if (isComputingTotals) {
            this.SubTotalInLocalCurrency = Tools_1.AppTool.Round(subTotalItem.LocalCurrencyAmount, 2);
            this.SubTotalInInvoiceCurrency = Tools_1.AppTool.Round(subTotalItem.InvoiceCurrencyAmount, 2);
            this.AmountInLocalCurrency = Tools_1.AppTool.Round(allTotalItem.LocalCurrencyAmount, 2);
            this.AmountInInvoiceCurrency = Tools_1.AppTool.Round(allTotalItem.InvoiceCurrencyAmount, 2);
            if (this.ProfitCurrencyId == this.InvoiceCurrencyId) {
                this.AmountInProfitCurrency = this.AmountInInvoiceCurrency;
            }
            else {
                if (Tools_1.AppTool.IsNullOrZero(this.ProfitCurrencyExchangeRate)) {
                    this.AmountInProfitCurrency = 0;
                }
                else {
                    this.AmountInProfitCurrency = Tools_1.AppTool.Round(this.AmountInLocalCurrency / this.ProfitCurrencyExchangeRate, 2);
                }
            }
            this.AmountDue = this.AmountInInvoiceCurrency == null ? 0 : this.AmountInInvoiceCurrency;
            this.AmountDueInLocalCurrency = this.AmountInLocalCurrency == null ? 0 : this.AmountInLocalCurrency;
            this.AmountDueInProfitCurrency = this.AmountInProfitCurrency == null ? 0 : this.AmountInProfitCurrency;
        }
        this.TotalsList = totalsList;
        this.BuildTotalsControl();
    };
    ARInvoiceDetailsTabConsolidation.prototype.BuildTotalsControl = function () {
        this.SummaryItems = [];
        var pipe = new NumbersPipe_1.NumbersPipe();
        var selectedCurrencyCode = this.IsTotalInLocalCurrency ? "(" + this.LocalCurrencyCode + ")" : "(" + this.InvoiceCurrencyCode + ")";
        for (var i = 0; i < this.TotalsList.length; i++) {
            var item = this.TotalsList[i];
            var mySummaryItem = new Args_1.SummaryItem();
            mySummaryItem.Label = item.RowLabel;
            mySummaryItem.Value = this.IsTotalInLocalCurrency ? pipe.transform(item.LocalCurrencyAmount, "N2") : pipe.transform(item.InvoiceCurrencyAmount, "N2");
            this.SummaryItems.push(mySummaryItem);
            if (i + 2 < this.TotalsList.length) {
                var myOperatorItem = new Args_1.SummaryItem();
                myOperatorItem.Value = "+";
                this.SummaryItems.push(myOperatorItem);
            }
            else if (i + 1 < this.TotalsList.length) {
                var myOperatorItem = new Args_1.SummaryItem();
                myOperatorItem.Value = "=";
                this.SummaryItems.push(myOperatorItem);
            }
            else if (i + 1 == this.TotalsList.length) {
                mySummaryItem.Label += " " + selectedCurrencyCode;
            }
        }
    };
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "SubTotalInLocalCurrency", {
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
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "SubTotalInInvoiceCurrency", {
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
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "AmountInLocalCurrency", {
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
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "AmountInInvoiceCurrency", {
        get: function () { return this.EntityPM.AmountInInvoiceCurrency; },
        set: function (value) {
            var setValue = Tools_1.AppTool.Round(value, 2);
            if (this.EntityPM.AmountInInvoiceCurrency != setValue) {
                this.EntityPM.AmountInInvoiceCurrency = setValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "AmountInProfitCurrency", {
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
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "AmountDue", {
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
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "AmountDueInLocalCurrency", {
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
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "AmountDueInProfitCurrency", {
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
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "IsInvoiceNumberManuallySet", {
        get: function () { return this.EntityPM.IsInvoiceNumberManuallySet; },
        set: function (value) {
            if (this.EntityPM.IsInvoiceNumberManuallySet != value) {
                if (!value) {
                    this.InvoiceNumber = null;
                }
                else if (this.EntityPM.InvoiceNumber == this.EntityPM.Id) {
                    this.InvoiceNumber = null;
                }
                this.EntityPM.IsInvoiceNumberManuallySet = value;
                this.UIProperties.SetEnabled("InvoiceNumber", this.ObjectTableName, value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "IsInvoiceNumberFromStock", {
        get: function () { return this.EntityPM.IsInvoiceNumberFromStock; },
        set: function (value) {
            if (this.EntityPM.IsInvoiceNumberFromStock != value) {
                this.EntityPM.IsInvoiceNumberFromStock = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "InvoiceNumber", {
        get: function () {
            if (this.IsInvoiceNumberManuallySet || this.IsInvoiceNumberFromStock) {
                return this.EntityPM.InvoiceNumber;
            }
            else if (this.EntityPM.Id == this.EntityPM.InvoiceNumber) {
                return null;
            }
            else {
                return this.EntityPM.InvoiceNumber;
            }
        },
        set: function (value) {
            if (this.EntityPM.InvoiceNumber != value) {
                if (this.IsInvoiceNumberManuallySet || (this.IsInvoiceNumberFromStock)) {
                    this.EntityPM.InvoiceNumber = value;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "ARInvoiceStockId", {
        get: function () { return this.EntityPM.ARInvoiceStockId; },
        set: function (newValue) {
            if (this.EntityPM.ARInvoiceStockId != newValue) {
                this.EntityPM.ARInvoiceStockId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "IsInvoiceNumberComboBoxEnabled", {
        get: function () { return this.isInvoiceNumberComboBoxEnabled; },
        set: function (value) {
            if (this.isInvoiceNumberComboBoxEnabled != value) {
                this.isInvoiceNumberComboBoxEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabConsolidation.prototype, "SelectedInvoiceNumberFilter", {
        get: function () { return this.selectedInvoiceNumberFilter; },
        set: function (value) {
            if (this.selectedInvoiceNumberFilter != value) {
                this.selectedInvoiceNumberFilter = value;
                this.EntityPM.InvoiceNumber = null;
                this.EntityPM.IsInvoiceNumberFromStock = false;
                this.EntityPM.IsInvoiceNumberManuallySet = false;
                if (value.Code == "MAS") {
                    this.IsInvoiceNumberManuallySet = true;
                }
                else if (value.Code == "STK") {
                    this.IsInvoiceNumberFromStock = true;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceDetailsTabConsolidation.prototype.BuildInvoiceNumberFilters = function () {
        this.InvoiceNumberFilterList = [];
        this.InvoiceNumberFilterList.push(new CodeNameClass_1.CodeNameClass("CNR", "Counter"));
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.EnableInvoiceStocksManagement) {
            this.InvoiceNumberFilterList.push(new CodeNameClass_1.CodeNameClass("STK", "Stock"));
        }
        if (this.AllowManualInvoiceNumber) {
            this.InvoiceNumberFilterList.push(new CodeNameClass_1.CodeNameClass("MAS", "Manually Set"));
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM != null && this.EntityPM.ARInvoiceStockId)) {
            this.selectedInvoiceNumberFilter = this.InvoiceNumberFilterList.filter(function (a) { return a.Code == "STK"; })[0];
        }
        else if (this.EntityPM != null && this.EntityPM.IsInvoiceNumberManuallySet == true) {
            this.selectedInvoiceNumberFilter = this.InvoiceNumberFilterList.filter(function (a) { return a.Code == "MAS"; })[0];
        }
        else {
            this.selectedInvoiceNumberFilter = this.InvoiceNumberFilterList.filter(function (a) { return a.Code == "CNR"; })[0];
        }
    };
    ARInvoiceDetailsTabConsolidation.prototype.GetInvoiceNumberFromStock = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Select Invoice Number From Stock";
        logWindow.Width = 1200;
        logWindow.Height = 600;
        logWindow.Show('./InvoiceModules/InvoiceStocks/Components/StockSelection/ARInvoiceStockSelectionComponent');
        logWindow.ComponentLoaded.subscribe(function (s) {
            logWindow.WindowClosed.subscribe(function (d) {
                if ((d != null && d != "cancel")) {
                    if (_this.EntityPM.IsAutoCredit) {
                        var myConfirmWindow = new ConfirmWindow_1.ConfirmWindow();
                        myConfirmWindow.Width = 400;
                        myConfirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.ConfirmAutoCredit"));
                        myConfirmWindow.WindowClosed.subscribe(function (event) {
                            if (myConfirmWindow.Yes) {
                                _this.SetStockProperties(s.StockLineSelectedItem, TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.CreatingAutoCredit"));
                            }
                        });
                    }
                    else {
                        _this.SetStockProperties(s.StockLineSelectedItem);
                    }
                }
            });
        });
    };
    ARInvoiceDetailsTabConsolidation.prototype.ReturnInvoiceNumberToStock = function () {
        this.ARInvoiceStockId = null;
        this.InvoiceNumber = null;
        this.IsInvoiceNumberComboBoxEnabled = true;
        this.SelectedInvoiceNumberFilter = this.InvoiceNumberFilterList.filter(function (a) { return a.Code == "CNR"; })[0];
        this.CurrentSession.CurrentEditComponent.SaveChanges();
    };
    ARInvoiceDetailsTabConsolidation.prototype.SetStockProperties = function (stockLineSelectedItem, msg) {
        if (msg === void 0) { msg = null; }
        this.IsgetFromStockAfterSaving = true;
        this.InvoiceNumber = stockLineSelectedItem.Number;
        this.ARInvoiceStockId = stockLineSelectedItem.Id;
        this.IsInvoiceNumberComboBoxEnabled = false;
        this.CurrentSession.CurrentEditComponent.SaveChanges(msg);
    };
    ARInvoiceDetailsTabConsolidation = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ARInvoiceDetailsTabConsolidation.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ARInvoiceDetailsTabConsolidation);
    return ARInvoiceDetailsTabConsolidation;
}(BaseComponent_1.BaseComponent));
exports.ARInvoiceDetailsTabConsolidation = ARInvoiceDetailsTabConsolidation;
var SubInvoiceLine = /** @class */ (function () {
    function SubInvoiceLine(item, fatherComponent) {
        var _this = this;
        this.fatherComponent = fatherComponent;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isMatched = false;
        this.isConnected = false;
        this.entityList = item;
        if (this.fatherComponent.InvoiceCurrencyId == item.InvoiceCurrencyId) {
            this.IsMatched = true;
        }
        if (this.fatherComponent.EntityPM.ConstituentInvoices.filter(function (f) { return f.ConsolidationInvoiceId == _this.fatherComponent.EntityPM.Id && f.Id == _this.entityList.Id; }).length > 0) {
            this.isConnected = true;
        }
        this.SetIsMatch();
    }
    SubInvoiceLine.prototype.SetIsMatch = function () {
        this.IsMatched = this.fatherComponent.InvoiceCurrencyId == this.InvoiceCurrencyId ? true : false;
    };
    Object.defineProperty(SubInvoiceLine.prototype, "IsMatched", {
        get: function () { return this.isMatched; },
        set: function (value) {
            if (this.isMatched != value) {
                this.isMatched = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SubInvoiceLine.prototype, "Id", {
        get: function () { return this.entityList.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SubInvoiceLine.prototype, "InvoiceCurrencyId", {
        get: function () { return this.entityList.InvoiceCurrencyId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SubInvoiceLine.prototype, "InvoiceCurrencyCode", {
        get: function () { return this.entityList.InvoiceCurrencyCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SubInvoiceLine.prototype, "InvoiceNumber", {
        get: function () { return this.entityList.InvoiceNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SubInvoiceLine.prototype, "CustomerRef", {
        get: function () { return this.entityList.CustomerRef; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SubInvoiceLine.prototype, "MasterNumber", {
        get: function () { return this.entityList.MasterNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SubInvoiceLine.prototype, "HouseNumber", {
        get: function () { return this.entityList.HouseNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SubInvoiceLine.prototype, "MainEntityReference", {
        get: function () { return this.entityList.MainEntityReference; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SubInvoiceLine.prototype, "SubTotalInInvoiceCurrency", {
        get: function () { return this.entityList.SubTotalInInvoiceCurrency; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SubInvoiceLine.prototype, "SubTotalInLocalCurrency", {
        get: function () { return this.entityList.SubTotalInLocalCurrency; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SubInvoiceLine.prototype, "AmountInInvoiceCurrency", {
        get: function () { return this.entityList.AmountInInvoiceCurrency; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SubInvoiceLine.prototype, "AmountInLocalCurrency", {
        get: function () { return this.entityList.AmountInLocalCurrency; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SubInvoiceLine.prototype, "AmountInProfitCurrency", {
        get: function () { return this.entityList.AmountInProfitCurrency; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SubInvoiceLine.prototype, "AmountDue", {
        get: function () { return this.entityList.AmountDue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SubInvoiceLine.prototype, "AmountDueInLocalCurrency", {
        get: function () { return this.entityList.AmountDueInLocalCurrency; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SubInvoiceLine.prototype, "AmountDueInProfitCurrency", {
        get: function () { return this.entityList.AmountDueInProfitCurrency; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SubInvoiceLine.prototype, "VATAmountInInvoiceCurrency", {
        get: function () {
            var myResult = 0;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.AmountInInvoiceCurrency)) {
                myResult += this.AmountInInvoiceCurrency;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SubTotalInInvoiceCurrency)) {
                myResult -= this.SubTotalInInvoiceCurrency;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SubInvoiceLine.prototype, "VATAmountInLocalCurrency", {
        get: function () {
            var myResult = 0;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.AmountInLocalCurrency)) {
                myResult += this.AmountInLocalCurrency;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SubTotalInLocalCurrency)) {
                myResult -= this.SubTotalInLocalCurrency;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SubInvoiceLine.prototype, "IsConnected", {
        get: function () { return this.isConnected; },
        set: function (value) {
            var _this = this;
            if (this.isConnected != value) {
                this.isConnected = value;
                if (value == true) {
                    var itemPM = new ConstituentPM_1.ConstituentPM(null);
                    itemPM.Id = this.entityList.Id;
                    itemPM.Tenant = this.entityList.Tenant;
                    itemPM.ConsolidationInvoiceId = this.fatherComponent.EntityPM.Id;
                    this.fatherComponent.EntityPM.AddConstituentPM(itemPM);
                }
                else {
                    var itemPM = this.fatherComponent.EntityPM.ConstituentInvoices.filter(function (f) { return f.ConsolidationInvoiceId == _this.fatherComponent.EntityPM.Id && f.Id == _this.Id; })[0];
                    if (itemPM) {
                        this.fatherComponent.EntityPM.RemoveConstituentPM(itemPM);
                    }
                }
                this.fatherComponent.ComputeTotals();
                this.fatherComponent.SetUIProperties_Connected();
                if (!Tools_1.AppTool.IsNullOrEmpty(this.fatherComponent.EntityPM.Id)) {
                    if (this.CurrentSession.CurrentEditComponent) {
                        this.CurrentSession.CurrentEditComponent.SaveChanges();
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    SubInvoiceLine.prototype.ViewEntityClicked = function () {
        var _this = this;
        var myBackButtonLabel = "A/R Invoice";
        if (!Tools_1.AppTool.IsNullOrEmpty(this.fatherComponent.EntityPM.InvoiceNumber)) {
            myBackButtonLabel += ": " + this.fatherComponent.EntityPM.InvoiceNumber;
        }
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: _this.Id, ObjectTableName: 'ARInvoice', BackButtonLabel: myBackButtonLabel });
            var isEditComponentSaved = false;
            cmpRef.instance.BackCompleted.subscribe(function (bk) {
                if (isEditComponentSaved) {
                    _this.fatherComponent.LoadInvoices();
                }
            });
            cmpRef.instance.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    isEditComponentSaved = true;
                }
            });
            cmpRef.instance.SaveAndCloseCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    isEditComponentSaved = true;
                }
            });
        });
    };
    return SubInvoiceLine;
}());
exports.SubInvoiceLine = SubInvoiceLine;
//# sourceMappingURL=ARInvoiceDetailsTabConsolidation.js.map