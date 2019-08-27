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
var ARInvoiceLinePM_1 = require("../../../../Invoice/EntityPMs/ARInvoiceLinePM");
var ARInvoiceTotalVATPM_1 = require("../../../../Invoice/EntityPMs/ARInvoiceTotalVATPM");
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
var VatTypeListService_1 = require("../../../../Common/Services/StandardLists/VatTypeListService");
var ChargesTypeListService_1 = require("../../../../Common/Services/StandardLists/ChargesTypeListService");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var NumbersPipe_1 = require("../../../../Infrastructure/Pipes/NumbersPipe");
var ShipmentDomainService_1 = require("../../../../Shipment/Services/ShipmentDomainService");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var InvoiceDomainService_1 = require("../../../../Invoice/Services/InvoiceDomainService");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var CodeNameClass_1 = require("../../../../Infrastructure/DataContracts/CodeNameClass");
var ARInvoiceDetailsTabNormal = /** @class */ (function (_super) {
    __extends(ARInvoiceDetailsTabNormal, _super);
    function ARInvoiceDetailsTabNormal(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.ObjectTableName = "ARInvoice";
        _this.DataContext = _this;
        _this.ItemsSource = [];
        _this.IsManifest = false;
        _this.EntityWarningsList = [];
        _this.EntityWarning = "";
        _this.IsCustomsInvoice = false;
        _this.IsEditExchangeRateVisible = false;
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.InvoiceNumberFilterList = [];
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.AllVatTypes = [];
        _this.LocalAmountHeader = null;
        _this.InvoiceAmountHeader = null;
        _this.IsInvoiceAmountHeaderVisible = false;
        _this.VATColumnWidth = 100;
        _this.RateColumnWidth = 100;
        // SetUIProperties
        _this.IsEditingEnabled = false;
        _this.IsStockEnabled = false;
        _this.RateIsEnabled = false;
        _this.VatTypeFilterIsEnabled = false;
        _this.AllowManualInvoiceNumber = false;
        _this.AllowStockInvoiceNumber = false;
        _this.PaymentTermDisplayInLOV = true;
        // Bill To
        _this.BillToDependencyValue1 = null;
        _this.myRelativeRateDate = null;
        // Load Date 
        _this.LastRatesList = [];
        _this.VatTypePercentagesList = [];
        _this.isTotalInLocalCurrency = false;
        _this.SummaryItems = [];
        _this.isInvoiceNumberComboBoxEnabled = true;
        _this.IsgetFromStockAfterSaving = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.EntityPM = entityArgs.EntityPM;
        _this.IsManifest = _this.EntityPM.ARInvoiceTypeCode == "MN" ? true : false;
        _this.IsCustomsInvoice = (_this.EntityPM.ARInvoiceTypeCode == "CI" || _this.EntityPM.ARInvoiceTypeCode == "CC") ? true : false;
        _this.ObservableItems = new ObservableCollection_1.ObservableCollection([]);
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
            if (!_this.EntityPM.IsConstituentInvoice) {
                _this.AllowStockInvoiceNumber = true;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(_this.ARInvoiceStockId)) {
            _this.IsInvoiceNumberComboBoxEnabled = false;
        }
        _this.BuildInvoiceNumberFilters();
        return _this;
    }
    ARInvoiceDetailsTabNormal.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.BuildInvoiceLines();
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
                    _this.BuildInvoiceLines();
                }
            });
        }
    };
    ARInvoiceDetailsTabNormal.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    ARInvoiceDetailsTabNormal.prototype.InitializeServices = function () {
        this.myCardListService = new CardListService_1.CardListService();
        this.myCurrencyListService = new CurrencyListService_1.CurrencyListService();
        this.myPaymentTermListService = new PaymentTermListService_1.PaymentTermListService();
        this.myVatTypeListService = new VatTypeListService_1.VatTypeListService();
        this.myChargesTypeListService = new ChargesTypeListService_1.ChargesTypeListService();
        this.myCommonDomainService = new CommonDomainService_1.CommonDomainService();
        this.myInvoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
    };
    ARInvoiceDetailsTabNormal.prototype.InitializeComponent = function () {
        this.SetGridColumns();
        this.ComputeRelativeRateDate();
        this.BillToDependencyValue1 = Tools_2.InvoiceTool.GetBillToPartnerTypes();
        this.GetAllVatTypes();
    };
    ARInvoiceDetailsTabNormal.prototype.GetAllVatTypes = function () {
        var _this = this;
        this.myVatTypeListService.getAllFromCache().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.AllVatTypes = myResponse.Result;
            }
        });
    };
    ARInvoiceDetailsTabNormal.prototype.SetGridColumns = function () {
        this.LocalAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoiceLine.CH.LocalCurrencyAmount").replace("%LocalCurrencyCode", this.LocalCurrencyCode);
        var invoiceAmountHeader = null;
        var isInvoiceAmountHeaderVisible = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
            if (this.InvoiceCurrencyId != this.LocalCurrencyId) {
                isInvoiceAmountHeaderVisible = true;
                invoiceAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoiceLine.CH.AmountInvoice").replace("%InvoiceCurrencyCode", this.InvoiceCurrencyCode);
            }
        }
        this.InvoiceAmountHeader = invoiceAmountHeader;
        this.IsInvoiceAmountHeaderVisible = isInvoiceAmountHeaderVisible;
    };
    ARInvoiceDetailsTabNormal.prototype.SetGridColumnsWidth = function () {
        var myRateColumnWidth = 100;
        var myVATColumnWidth = 100;
        var pipe = new NumbersPipe_1.NumbersPipe();
        this.ItemsSource.forEach(function (item) {
            // Rate
            var myRate = pipe.transform(item.ForiegnExchangeRate, 'N5');
            var myRelativeRateDate = item.RelativeRateDate;
            if (Tools_1.AppTool.IsNullOrEmpty(myRate)) {
                myRate = "";
            }
            if (Tools_1.AppTool.IsNullOrEmpty(myRelativeRateDate)) {
                myRelativeRateDate = "";
            }
            var myRateCellText = myRate + myRelativeRateDate;
            var myRateCellWidth = Tools_1.AppTool.GetTextWidth(myRateCellText, 12) + 10;
            if (myRateCellWidth > myRateColumnWidth) {
                myRateColumnWidth = myRateCellWidth;
            }
            // VAT
            var myVATTextWidth = Tools_1.AppTool.GetTextWidth(item.VatTypeCell, 12) + 10;
            if (item.VatTypeUpdateIsVisible) {
                myVATTextWidth += 20;
            }
            if (myVATTextWidth > myVATColumnWidth) {
                myVATColumnWidth = myVATTextWidth;
            }
        });
        this.VATColumnWidth = myVATColumnWidth;
        this.RateColumnWidth = myRateColumnWidth;
    };
    ARInvoiceDetailsTabNormal.prototype.SetUIProperties = function () {
        var isEditingEnabled = Tools_2.InvoiceTool.IsEditingARInvoiceEnabled(this.EntityPM);
        var isInvoiceDateEnabled = isEditingEnabled;
        if (this.EntityPM.IsAutoCredit && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            isInvoiceDateEnabled = true;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.BillToAddressId)) {
            this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, isEditingEnabled);
        }
        this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("PaymentTermId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("InvoiceCurrencyId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("VatNumber", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("InvoiceDate", this.ObjectTableName, isInvoiceDateEnabled);
        this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, isEditingEnabled);
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
        }
        this.IsEditingEnabled = isEditingEnabled;
        this.SetUIProperties_BillToAddress();
        this.SetUIProperties_VatNumber();
        this.SetUIProperties_ExchangeRate();
        this.SetUIProperties_PrintNotes();
        this.SetUIProperties_ManuallySet();
        this.SetUIProperties_InvoiceNumber();
        this.SetUIProperties_VatTypeFilter();
        this.SetUIProperties_DueDate();
    };
    ARInvoiceDetailsTabNormal.prototype.SetUIProperties_BillToAddress = function () {
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
    ARInvoiceDetailsTabNormal.prototype.SetUIProperties_VatNumber = function () {
        var isFieldRequired = false;
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAR) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.VatNumber)) {
                isFieldRequired = true;
            }
            this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, isFieldRequired);
        }
    };
    ARInvoiceDetailsTabNormal.prototype.SetUIProperties_ExchangeRate = function () {
        var isFieldtEnabled = false;
        if (this.IsEditingEnabled) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "ARInvoiceEditExchangeRate")) {
                if (this.InvoiceCurrencyId) {
                    if (this.InvoiceCurrencyId != SessionLocator_1.SessionLocator.TenantPM.CurrencyId) {
                        isFieldtEnabled = true;
                    }
                }
            }
        }
        this.RateIsEnabled = isFieldtEnabled;
        this.UIProperties.SetEnabled("InvoiceCurrencyExchangeRate", this.ObjectTableName, isFieldtEnabled);
    };
    ARInvoiceDetailsTabNormal.prototype.SetUIProperties_PrintNotes = function () {
        var isFieldtEnabled = true;
        if (this.EntityPM.StatusCode == "VD") {
            isFieldtEnabled = false;
        }
        else if (this.EntityPM.IsPrinted) {
            isFieldtEnabled = false;
        }
        this.UIProperties.SetEnabled("PrintNotes", this.ObjectTableName, isFieldtEnabled);
    };
    ARInvoiceDetailsTabNormal.prototype.SetUIProperties_ManuallySet = function () {
        var isFieldtEnabled = this.IsEditingEnabled;
        var isFieldtVisible = false;
        if (this.IsInvoiceNumberManuallySet) {
            isFieldtVisible = true;
        }
        else {
            if (SessionLocator_1.SessionLocator.AccountingSettingPM.AllowManualInvoiceNumber) {
                if (!this.EntityPM.IsConstituentInvoice) {
                    isFieldtVisible = true;
                }
            }
        }
        this.AllowManualInvoiceNumber = isFieldtVisible;
        this.UIProperties.SetEnabled("IsInvoiceNumberManuallySet", this.ObjectTableName, isFieldtEnabled);
        this.UIProperties.SetVisibility("IsInvoiceNumberManuallySet", this.ObjectTableName, isFieldtVisible);
    };
    ARInvoiceDetailsTabNormal.prototype.SetUIProperties_InvoiceNumber = function () {
        var isFieldEnabled = false;
        this.IsStockEnabled = false;
        if (this.IsEditingEnabled || (this.EntityPM.IsAutoCredit && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id))) {
            this.IsStockEnabled = true;
            if (this.IsInvoiceNumberManuallySet) {
                isFieldEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("InvoiceNumber", this.ObjectTableName, isFieldEnabled);
    };
    ARInvoiceDetailsTabNormal.prototype.SetUIProperties_VatTypeFilter = function () {
        var isFieldtEnabled = this.IsEditingEnabled;
        if (isFieldtEnabled) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.VatTypeId)) {
                isFieldtEnabled = false;
            }
        }
        this.VatTypeFilterIsEnabled = isFieldtEnabled;
    };
    ARInvoiceDetailsTabNormal.prototype.SetUIProperties_DueDate = function () {
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
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "BillToId", {
        get: function () { return this.EntityPM.BillToId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.BillToId != newValue) {
                this.EntityPM.BillToId = newValue;
                this.EntityPM.CustomerRef = null;
                this.SetUIProperties_BillToAddress();
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.VatNumber = null;
                    this.BillToName = null;
                    this.BillToAddressId = null;
                    this.InvoiceCurrencyId = SessionLocator_1.SessionLocator.AccountingCurrencyId;
                    this.PaymentTermId = SessionLocator_1.SessionLocator.TenantPM.PaymentTermId;
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
                                if (!Tools_1.AppTool.IsNullOrEmpty(list.VatTypeId)) {
                                    _this.VatTypeId = list.VatTypeId;
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
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "BillToName", {
        get: function () { return this.EntityPM.BillToName; },
        set: function (newValue) {
            if (this.EntityPM.BillToName != newValue) {
                this.EntityPM.BillToName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "BillToAddressId", {
        get: function () { return this.EntityPM.BillToAddressId; },
        set: function (newValue) {
            if (this.EntityPM.BillToAddressId != newValue) {
                this.EntityPM.BillToAddressId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "InvoiceCurrencyId", {
        // Currency
        get: function () { return this.EntityPM.InvoiceCurrencyId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.InvoiceCurrencyId != newValue) {
                this.EntityPM.InvoiceCurrencyId = newValue;
                this.SetUIProperties_ExchangeRate();
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.InvoiceCurrencyCode = null;
                    this.SetGridColumns();
                    this.SetCurrencyRateData();
                }
                else {
                    this.myCurrencyListService.getSingleFromCache(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.InvoiceCurrencyCode = list.Code;
                                _this.SetCurrencyRateData();
                                _this.SetGridColumns();
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "InvoiceCurrencyCode", {
        get: function () { return this.EntityPM.InvoiceCurrencyCode; },
        set: function (value) {
            if (this.EntityPM.InvoiceCurrencyCode != value) {
                this.EntityPM.InvoiceCurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "InvoiceCurrencyExchangeRate", {
        get: function () { return this.EntityPM.InvoiceCurrencyExchangeRate; },
        set: function (value) {
            var setValue = Tools_1.AppTool.Round(value, 5);
            if (this.EntityPM.InvoiceCurrencyExchangeRate != setValue) {
                this.EntityPM.InvoiceCurrencyExchangeRate = setValue;
                this.ItemsSource.forEach(function (item) {
                    item.OnInvoiceExchangeRateChanged();
                });
                this.SetGridColumnsWidth();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "ExchangeRateDate", {
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
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "RelativeRateDate", {
        get: function () { return this.myRelativeRateDate; },
        set: function (value) {
            if (this.myRelativeRateDate != value) {
                this.myRelativeRateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceDetailsTabNormal.prototype.ComputeRelativeRateDate = function () {
        this.RelativeRateDate = Tools_1.DateTool.GetRelativeRateDate(this.InvoiceDate, this.ExchangeRateDate, "old");
    };
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "ProfitCurrencyId", {
        get: function () { return this.EntityPM.ProfitCurrencyId; },
        set: function (newValue) {
            if (this.EntityPM.ProfitCurrencyId != newValue) {
                this.EntityPM.ProfitCurrencyId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "ProfitCurrencyExchangeRate", {
        get: function () { return this.EntityPM.ProfitCurrencyExchangeRate; },
        set: function (newValue) {
            if (this.EntityPM.ProfitCurrencyExchangeRate != newValue) {
                this.EntityPM.ProfitCurrencyExchangeRate = Tools_1.AppTool.Round(newValue, 5);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "SATPaymentMethodCode", {
        get: function () { return this.EntityPM.SATPaymentMethodCode; },
        set: function (newValue) {
            if (this.EntityPM.SATPaymentMethodCode != newValue) {
                this.EntityPM.SATPaymentMethodCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceDetailsTabNormal.prototype.UpdateRateFromLine = function (line) {
        if (this.InvoiceCurrencyId == line.ForiegnCurrencyId) {
            var myValue = Tools_1.AppTool.Round(line.ForiegnExchangeRate, 5);
            if (this.InvoiceCurrencyExchangeRate != myValue) {
                this.EntityPM.InvoiceCurrencyExchangeRate = myValue;
                this.ItemsSource.forEach(function (item) {
                    if (item != line) {
                        item.OnInvoiceExchangeRateChanged();
                    }
                });
            }
        }
    };
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "VatNumber", {
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
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "PaymentTermId", {
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
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "PaymentTermName", {
        get: function () { return this.EntityPM.PaymentTermName; },
        set: function (newValue) {
            if (this.EntityPM.PaymentTermName != newValue) {
                this.EntityPM.PaymentTermName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "InvoiceDate", {
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
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "DueDate", {
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
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "DraftNumber", {
        get: function () { return this.EntityPM.DraftNumber; },
        set: function (newValue) {
            if (this.EntityPM.DraftNumber != newValue) {
                this.EntityPM.DraftNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "StatusCode", {
        get: function () { return this.EntityPM.StatusCode; },
        set: function (newValue) {
            if (this.EntityPM.StatusCode != newValue) {
                this.EntityPM.StatusCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "PrintNotes", {
        get: function () { return this.EntityPM.PrintNotes; },
        set: function (newValue) {
            if (this.EntityPM.PrintNotes != newValue) {
                this.EntityPM.PrintNotes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "ARInvoiceStockId", {
        get: function () { return this.EntityPM.ARInvoiceStockId; },
        set: function (newValue) {
            if (this.EntityPM.ARInvoiceStockId != newValue) {
                this.EntityPM.ARInvoiceStockId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "VatTypeId", {
        get: function () { return this.vatTypeId; },
        set: function (newValue) {
            if (this.vatTypeId != newValue) {
                this.vatTypeId = newValue;
                this.SetUIProperties_VatTypeFilter();
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceDetailsTabNormal.prototype.VatTypeFilterClicked = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.VatTypeId)) {
            var vatType = this.VatTypeId;
            var loadingDate = this.EntityPM.InvoiceDate;
            if (loadingDate == null) {
                loadingDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            }
            this.VatTypeId = null;
            this.myCommonDomainService.GetVatTypePercentagePMByDate(loadingDate).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.VatTypePercentagesList = myResponse.Result;
                    _this.ItemsSource.forEach(function (item) {
                        item.EntityPM.VatTypeId = vatType;
                        item.GetVatTypeData();
                    });
                    _this.SetGridColumnsWidth();
                    _this.ComputeTotals();
                }
            });
        }
    };
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "PrepaidCollectId", {
        // Filter Invoice lines
        get: function () { return this.EntityPM.PrepaidCollectId; },
        set: function (value) {
            if (this.EntityPM.PrepaidCollectId != value) {
                this.EntityPM.PrepaidCollectId = value;
                this.FilterInvoiceLines();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "IsCustomsChargesOnly", {
        get: function () { return this.EntityPM.IsCustomsChargesOnly; },
        set: function (value) {
            if (this.EntityPM.IsCustomsChargesOnly != value) {
                this.EntityPM.IsCustomsChargesOnly = value;
                this.FilterInvoiceLines();
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceDetailsTabNormal.prototype.FilterInvoiceLines = function () {
        var _this = this;
        this.ItemsSource.forEach(function (item) {
            var isLineMatched = true;
            if (_this.IsCustomsInvoice) {
                if (_this.IsCustomsChargesOnly == true) {
                    if (item.IsCustomsCharge == false) {
                        isLineMatched = false;
                    }
                }
            }
            if (isLineMatched) {
                switch (_this.PrepaidCollectId) {
                    case "B":
                        {
                            break;
                        }
                    default: {
                        if (item.PrepaidCollectId != _this.PrepaidCollectId) {
                            isLineMatched = false;
                        }
                        break;
                    }
                }
            }
            if (isLineMatched) {
                if (_this.EntityPM.InvoiceLines.indexOf(item.EntityPM) == -1) {
                    _this.EntityPM.AddARInvoiceLinePM(item.EntityPM);
                }
            }
            else {
                if (_this.EntityPM.InvoiceLines.indexOf(item.EntityPM) > -1) {
                    _this.EntityPM.RemoveARInvoiceLinePM(item.EntityPM);
                }
            }
            item.RefreshLine();
        });
        this.ComputeTotals();
    };
    ARInvoiceDetailsTabNormal.prototype.LoadData = function () {
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
                _this.myCommonDomainService.GetVatTypePercentagePMByDate(loadingDate).subscribe(function (myResponse2) {
                    if (myResponse2.HasError) {
                        _this.CurrentSession.StopBusyIndicator();
                    }
                    else {
                        _this.VatTypePercentagesList = myResponse2.Result;
                        _this.BuildInvoiceLines();
                        _this.CurrentSession.StopBusyIndicator();
                        _this.CheckNotifyPastDateOnInvoiceEdit();
                    }
                });
            }
        });
    };
    ARInvoiceDetailsTabNormal.prototype.UpdateData = function () {
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
                _this.myCommonDomainService.GetVatTypePercentagePMByDate(loadingDate).subscribe(function (myResponse2) {
                    if (!myResponse2.HasError) {
                        _this.VatTypePercentagesList = myResponse2.Result;
                    }
                    _this.ItemsSource.forEach(function (item) {
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
    ARInvoiceDetailsTabNormal.prototype.SetCurrencyRateData = function () {
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
    ARInvoiceDetailsTabNormal.prototype.GetCurrencyRate = function (currencyId) {
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
    ARInvoiceDetailsTabNormal.prototype.GetCurrencyRateDate = function (currencyId) {
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
    ARInvoiceDetailsTabNormal.prototype.GetVatTypePercentage = function (vatTypeId) {
        var myResult = null;
        var vatTypePercentagePM = this.VatTypePercentagesList.filter(function (d) { return d.VatTypeId == vatTypeId; })[0];
        if (vatTypePercentagePM != null) {
            myResult = vatTypePercentagePM.Percentage;
        }
        return myResult;
    };
    ARInvoiceDetailsTabNormal.prototype.UpdateCurrencyRateClicked = function () {
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
    ARInvoiceDetailsTabNormal.prototype.CheckNotifyPastDateOnInvoiceEdit = function () {
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
    ARInvoiceDetailsTabNormal.prototype.BuildScreenData = function () {
        if (this.IsEditingEnabled) {
            this.LoadData();
        }
        else {
            this.BuildInvoiceLines();
        }
    };
    ARInvoiceDetailsTabNormal.prototype.BuildInvoiceLines = function () {
        var _this = this;
        this.ItemsSource = [];
        this.ObservableItems.Clear();
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            var tempList = [];
            if (this.EntityPM.ARInvoiceTypeCode != "CD" && this.EntityPM.ARInvoiceTypeCode != "CC") {
                this.EntityPM.InvoiceLines.filter(function (f) { return f.UnitPrice < 0; }).forEach(function (item) {
                    tempList.push(item);
                    _this.EntityPM.RemoveARInvoiceLinePM(item);
                });
            }
            if (this.EntityPM.PrepaidCollectId != "B") {
                this.EntityPM.InvoiceLines.filter(function (f) { return f.PrepaidCollectId != _this.EntityPM.PrepaidCollectId; }).forEach(function (item) {
                    tempList.push(item);
                    _this.EntityPM.RemoveARInvoiceLinePM(item);
                });
            }
            if (this.EntityPM.ARInvoiceTypeCode == "CI" || this.EntityPM.ARInvoiceTypeCode == "CC") {
                if (this.EntityPM.IsCustomsChargesOnly) {
                    this.EntityPM.InvoiceLines.filter(function (f) { return f.IsCustomsCharge == false; }).forEach(function (item) {
                        tempList.push(item);
                        _this.EntityPM.RemoveARInvoiceLinePM(item);
                    });
                }
            }
            this.EntityPM.InvoiceLines.forEach(function (item) {
                _this.ItemsSource.push(new ARInvoiceLineItem(item, _this));
            });
            tempList.forEach(function (item) {
                _this.ItemsSource.push(new ARInvoiceLineItem(item, _this));
            });
            this.ItemsSource.forEach(function (item) {
                _this.ObservableItems.Insert(item);
            });
            this.ComputeTotals();
        }
        else {
            if (this.EntityPM != null && this.EntityPM.InvoiceLines != null) {
                this.EntityPM.InvoiceLines.forEach(function (line) {
                    _this.ItemsSource.push(new ARInvoiceLineItem(line, _this));
                });
                this.ItemsSource.forEach(function (item) {
                    _this.ObservableItems.Insert(item);
                });
            }
            if (!this.EntityPM.IsAutoCredit) {
                if (this.IsEditingEnabled) {
                    this.LoadEntityOpenReceivables();
                }
            }
            this.BuildSummary();
        }
        this.SetGridColumnsWidth();
    };
    ARInvoiceDetailsTabNormal.prototype.LoadEntityOpenReceivables = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        var myService = new ShipmentDomainService_1.ShipmentDomainService();
        myService.GetInvoiceOpenAmountReceivables(this.EntityPM.ARInvoiceTypeCode, this.EntityPM.MainEntityId).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var allReceivables = myResponse.Result.filter(function (d) { return d.ShipmentReceivableParentId == null; });
                var openReceivables = allReceivables.filter(function (f) { return f.ShipmentReceivableLineStatusCode == "OAMT" && !Tools_1.AppTool.IsNullOrEmpty(f.UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(f.Quantity); });
                if (_this.EntityPM.ARInvoiceTypeCode == "CD") {
                    if (!SessionLocator_1.SessionLocator.AccountingSettingPM.AllowPositiveAmountsInTheCreditNote) {
                        openReceivables = openReceivables.filter(function (f) { return f.UnitPrice < 0; });
                    }
                }
                else {
                    if (!SessionLocator_1.SessionLocator.AccountingSettingPM.AllowMinusInvoicelines) {
                        openReceivables = openReceivables.filter(function (f) { return f.UnitPrice > 0; });
                    }
                }
                if (openReceivables.length > 0) {
                    _this.GetAllVatTypes();
                    openReceivables.forEach(function (receivable) {
                        var invoiceLine = new ARInvoiceLinePM_1.ARInvoiceLinePM(null);
                        invoiceLine.Tenant = receivable.Tenant;
                        invoiceLine.ChargesTypeId = receivable.ChargesTypeId;
                        invoiceLine.ForiegnCurrencyId = receivable.CurrencyId;
                        invoiceLine.ForiegnCurrencyCode = receivable.CurrencyCode;
                        invoiceLine.ReceivableId = receivable.Id;
                        invoiceLine.MeasurementId = receivable.MeasurementId;
                        invoiceLine.MeasurementCode = receivable.MeasurementCode;
                        invoiceLine.PrepaidCollectId = receivable.PrepaidCollectId;
                        invoiceLine.IsExchangeRateFixed = receivable.IsExchangeRateFixed;
                        invoiceLine.EntityId = receivable.ShipmentId;
                        invoiceLine.EntityReference = receivable.ShipmentNumber;
                        invoiceLine.Quantity = Tools_1.AppTool.Round(receivable.Quantity, 3);
                        invoiceLine.UnitPrice = Tools_1.AppTool.Round(receivable.UnitPrice, 3);
                        invoiceLine.ForiegnCurrencyAmount = Tools_1.AppTool.Round(receivable.TotalAmount, 2);
                        //invoiceLine.IsBackToBack = receivable.IsBackToBack;
                        invoiceLine.IsExpense = receivable.IsExpense;
                        invoiceLine.Notes = receivable.Notes;
                        var itemExchangeRate = null;
                        if (invoiceLine.IsExchangeRateFixed) {
                            itemExchangeRate = receivable.Rate;
                        }
                        else if (invoiceLine.ForiegnCurrencyId == _this.InvoiceCurrencyId) {
                            itemExchangeRate = _this.InvoiceCurrencyExchangeRate;
                        }
                        else {
                            itemExchangeRate = _this.GetCurrencyRate(invoiceLine.ForiegnCurrencyId);
                        }
                        var itemExchangeRateRounded = Tools_1.AppTool.Round(itemExchangeRate, 5);
                        invoiceLine.ForiegnExchangeRate = itemExchangeRateRounded;
                        invoiceLine.ExchangeRateDate = _this.GetCurrencyRateDate(invoiceLine.ForiegnCurrencyId);
                        // Local Amount
                        if (SessionLocator_1.SessionLocator.LocalCurrencyId == invoiceLine.ForiegnCurrencyId) {
                            invoiceLine.LocalCurrencyAmount = invoiceLine.ForiegnCurrencyAmount;
                        }
                        else {
                            invoiceLine.LocalCurrencyAmount = Tools_1.AppTool.Round((invoiceLine.ForiegnCurrencyAmount * invoiceLine.ForiegnExchangeRate), 2);
                        }
                        // Profit Amount
                        if (_this.EntityPM.ProfitCurrencyId == invoiceLine.ForiegnCurrencyId) {
                            invoiceLine.ProfitCurrencyAmount = invoiceLine.ForiegnCurrencyAmount;
                        }
                        else if (_this.EntityPM.ProfitCurrencyId == SessionLocator_1.SessionLocator.LocalCurrencyId) {
                            invoiceLine.ProfitCurrencyAmount = invoiceLine.LocalCurrencyAmount;
                        }
                        else {
                            invoiceLine.ProfitCurrencyAmount = Tools_1.AppTool.Round((invoiceLine.LocalCurrencyAmount / _this.EntityPM.ProfitCurrencyExchangeRate), 2);
                        }
                        // Invoice Amount
                        if (_this.EntityPM.InvoiceCurrencyId == invoiceLine.ForiegnCurrencyId) {
                            invoiceLine.InvoiceCurrencyAmount = invoiceLine.ForiegnCurrencyAmount;
                        }
                        else if (_this.EntityPM.InvoiceCurrencyId == SessionLocator_1.SessionLocator.LocalCurrencyId) {
                            invoiceLine.InvoiceCurrencyAmount = invoiceLine.LocalCurrencyAmount;
                        }
                        else if (_this.EntityPM.InvoiceCurrencyId == _this.EntityPM.ProfitCurrencyId) {
                            invoiceLine.InvoiceCurrencyAmount = invoiceLine.ProfitCurrencyAmount;
                        }
                        else {
                            invoiceLine.InvoiceCurrencyAmount = Tools_1.AppTool.Round((invoiceLine.LocalCurrencyAmount / _this.EntityPM.InvoiceCurrencyExchangeRate), 2);
                        }
                        // VAT
                        invoiceLine.VatTypeId = receivable.VatTypeId;
                        _this.myChargesTypeListService.getSingleFromCache(receivable.ChargesTypeId).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                var list = myResponse.Result;
                                if (list != null) {
                                    invoiceLine.Description = list.EnglishName;
                                    invoiceLine.LocalDescription = list.LocalName;
                                    invoiceLine.IsCustomsCharge = list.IsCustoms;
                                    if (Tools_1.AppTool.IsNullOrEmpty(invoiceLine.VatTypeId)) {
                                        invoiceLine.VatTypeId = list.VatTypeId;
                                    }
                                }
                            }
                        });
                        if (!Tools_1.AppTool.IsNullOrEmpty(invoiceLine.VatTypeId)) {
                            var list_VAT = _this.AllVatTypes.filter(function (f) { return f.Id == invoiceLine.VatTypeId; })[0];
                            if (list_VAT) {
                                invoiceLine.VatTypeName = list_VAT.EnglishName;
                                invoiceLine.VatIsMultiPercentage = list_VAT.IsMultiPercentage;
                                if (!list_VAT.IsMultiPercentage) {
                                    invoiceLine.VatPercentage = _this.GetVatTypePercentage(invoiceLine.VatTypeId);
                                }
                            }
                        }
                        var newItemClass = new ARInvoiceLineItem(invoiceLine, _this);
                        _this.ItemsSource.push(newItemClass);
                        _this.ObservableItems.Insert(newItemClass);
                    });
                }
            }
            _this.SetGridColumnsWidth();
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "IsCurrencyFilterVisible", {
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
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "IsTotalInLocalCurrency", {
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
    ARInvoiceDetailsTabNormal.prototype.ComputeTotals = function () {
        this.BuildTotalVATs();
        this.SubTotalInLocalCurrency = Tools_1.AppTool.Round(Tools_1.ArrayTool.Sum(this.EntityPM.InvoiceLines, "LocalCurrencyAmount"), 2);
        this.SubTotalInInvoiceCurrency = Tools_1.AppTool.Round(Tools_1.ArrayTool.Sum(this.EntityPM.InvoiceLines, "InvoiceCurrencyAmount"), 2);
        this.AmountInLocalCurrency = Tools_1.AppTool.Round(this.EntityPM.SubTotalInLocalCurrency + Tools_1.ArrayTool.Sum(this.EntityPM.TotalVATs, "LocalVATAmount"), 2);
        this.AmountInInvoiceCurrency = Tools_1.AppTool.Round(this.EntityPM.SubTotalInInvoiceCurrency + Tools_1.ArrayTool.Sum(this.EntityPM.TotalVATs, "InvoiceCurrencyVATAmount"), 2);
        if (this.EntityPM.ProfitCurrencyId == SessionLocator_1.SessionLocator.LocalCurrencyId) {
            this.AmountInProfitCurrency = this.EntityPM.AmountInLocalCurrency;
        }
        else if (this.EntityPM.ProfitCurrencyId == this.EntityPM.InvoiceCurrencyId) {
            this.AmountInProfitCurrency = this.EntityPM.AmountInInvoiceCurrency;
        }
        else {
            if (this.EntityPM.ProfitCurrencyExchangeRate != 0) {
                this.AmountInProfitCurrency = Tools_1.AppTool.Round(this.EntityPM.AmountInLocalCurrency / this.EntityPM.ProfitCurrencyExchangeRate, 2);
            }
        }
        this.AmountDue = this.AmountInInvoiceCurrency == null ? 0 : this.AmountInInvoiceCurrency;
        this.AmountDueInLocalCurrency = this.AmountInLocalCurrency == null ? 0 : this.AmountInLocalCurrency;
        this.AmountDueInProfitCurrency = this.AmountInProfitCurrency == null ? 0 : this.AmountInProfitCurrency;
        this.BuildSummary();
    };
    ARInvoiceDetailsTabNormal.prototype.BuildTotalVATs = function () {
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
                        myQroupItem.ExternalVatCard = SessionLocator_1.SessionLocator.AccountingSettingPM.ReceivableVATCard;
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
                            myQroupItem.ExternalVatCard = SessionLocator_1.SessionLocator.AccountingSettingPM.ReceivableVATCard;
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
                var itemTotalVAT = new ARInvoiceTotalVATPM_1.ARInvoiceTotalVATPM(null);
                itemTotalVAT.Tenant = SessionLocator_1.SessionLocator.Tenant;
                itemTotalVAT.ARInvoiceId = _this.EntityPM.Id;
                itemTotalVAT.VatTypeId = item.VatTypeId;
                itemTotalVAT.VatTypeName = itemVatType.EnglishName;
                itemTotalVAT.ExternalVATCard = item.ExternalVatCard;
                itemTotalVAT.ExternalTAXItemId = item.ExternalTAXItemId;
                itemTotalVAT.VATPercent = Tools_1.AppTool.Round(item.VatTypePercentage, 2);
                itemTotalVAT.LocalVatableAmount = Tools_1.AppTool.Round(item.LocalCurrencyAmount, 2);
                itemTotalVAT.InvoiceCurrencyVatableAmount = Tools_1.AppTool.Round(item.InvoiceCurrencyAmount, 2);
                itemTotalVAT.ProfitVatableAmount = Tools_1.AppTool.Round(item.ProfitCurrencyAmount, 2);
                itemTotalVAT.LocalVATAmount = Tools_1.AppTool.Round((itemTotalVAT.LocalVatableAmount * itemTotalVAT.VATPercent / 100), 2);
                itemTotalVAT.InvoiceCurrencyVATAmount = Tools_1.AppTool.Round((itemTotalVAT.InvoiceCurrencyVatableAmount * itemTotalVAT.VATPercent / 100), 2);
                itemTotalVAT.ProfitCurrencyVATAmount = Tools_1.AppTool.Round((itemTotalVAT.ProfitVatableAmount * itemTotalVAT.VATPercent / 100), 2);
                itemTotalVAT.VatTypeCell = itemTotalVAT.VatTypeName + " (" + itemTotalVAT.VATPercent + "%)";
                _this.EntityPM.AddARInvoiceTotalVATPM(itemTotalVAT);
            });
        }
    };
    ARInvoiceDetailsTabNormal.prototype.BuildSummary = function () {
        var _this = this;
        this.SummaryItems = [];
        var pipe = new NumbersPipe_1.NumbersPipe();
        var selectedCurrencyCode = this.IsTotalInLocalCurrency ? "(" + this.LocalCurrencyCode + ")" : "(" + this.InvoiceCurrencyCode + ")";
        if (this.EntityPM.InvoiceLines.length > 0) {
            var mySummaryItem_Sub = new Args_1.SummaryItem();
            mySummaryItem_Sub.Label = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.S.Details.Subtotal");
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
        mySummaryItem_All.Label = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.F.AmountInInvoiceCurrency") + " " + selectedCurrencyCode;
        mySummaryItem_All.Value = this.IsTotalInLocalCurrency ? pipe.transform(this.EntityPM.AmountInLocalCurrency, "N2") : pipe.transform(this.EntityPM.AmountInInvoiceCurrency, "N2");
        this.SummaryItems.push(mySummaryItem_All);
    };
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "SubTotalInLocalCurrency", {
        get: function () { return this.EntityPM.SubTotalInLocalCurrency; },
        set: function (value) {
            if (this.EntityPM.SubTotalInLocalCurrency != value) {
                this.EntityPM.SubTotalInLocalCurrency = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "SubTotalInInvoiceCurrency", {
        get: function () { return this.EntityPM.SubTotalInInvoiceCurrency; },
        set: function (value) {
            if (this.EntityPM.SubTotalInInvoiceCurrency != value) {
                this.EntityPM.SubTotalInInvoiceCurrency = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "AmountInLocalCurrency", {
        get: function () { return this.EntityPM.AmountInLocalCurrency; },
        set: function (value) {
            if (this.EntityPM.AmountInLocalCurrency != value) {
                this.EntityPM.AmountInLocalCurrency = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "AmountInInvoiceCurrency", {
        get: function () { return this.EntityPM.AmountInInvoiceCurrency; },
        set: function (value) {
            if (this.EntityPM.AmountInInvoiceCurrency != value) {
                this.EntityPM.AmountInInvoiceCurrency = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "AmountInProfitCurrency", {
        get: function () { return this.EntityPM.AmountInProfitCurrency; },
        set: function (value) {
            if (this.EntityPM.AmountInProfitCurrency != value) {
                this.EntityPM.AmountInProfitCurrency = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "AmountDue", {
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
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "AmountDueInLocalCurrency", {
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
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "AmountDueInProfitCurrency", {
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
    ARInvoiceDetailsTabNormal.prototype.EditLineClicked = function (item) {
        if (item != null) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoiceLine.O.EditInvoiceLine");
            logWindow.DataContext = item;
            logWindow.Show('./InvoiceModules/ARInvoice/Components/EditTabs/AddEditARInvoiceLineComponent');
        }
    };
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "JournalNumber", {
        // Journal Process
        get: function () {
            return this.EntityPM.JournalNumber;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "JournalId", {
        get: function () {
            return this.EntityPM.JournalId;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "IsFullAccounting", {
        get: function () {
            var result = false;
            if (SessionLocator_1.SessionLocator.TenantPM.AccountingActivated == true && !Tools_1.AppTool.IsNullOrEmpty(this.JournalNumber)) {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceDetailsTabNormal.prototype.EditJournal = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: _this.JournalId, ObjectTableName: 'Journal' });
        });
    };
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "IsInvoiceNumberManuallySet", {
        //Invoice Number
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
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "IsInvoiceNumberFromStock", {
        get: function () { return this.EntityPM.IsInvoiceNumberFromStock; },
        set: function (value) {
            if (this.EntityPM.IsInvoiceNumberFromStock != value) {
                this.EntityPM.IsInvoiceNumberFromStock = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "InvoiceNumber", {
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
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "IsInvoiceNumberComboBoxEnabled", {
        get: function () { return this.isInvoiceNumberComboBoxEnabled; },
        set: function (value) {
            if (this.isInvoiceNumberComboBoxEnabled != value) {
                this.isInvoiceNumberComboBoxEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabNormal.prototype, "SelectedInvoiceNumberFilter", {
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
                this.SetUIProperties_InvoiceNumber();
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceDetailsTabNormal.prototype.BuildInvoiceNumberFilters = function () {
        this.InvoiceNumberFilterList = [];
        this.InvoiceNumberFilterList.push(new CodeNameClass_1.CodeNameClass("CNR", "Counter"));
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.EnableInvoiceStocksManagement) {
            this.InvoiceNumberFilterList.push(new CodeNameClass_1.CodeNameClass("STK", "Stock"));
        }
        if (this.AllowManualInvoiceNumber) {
            this.InvoiceNumberFilterList.push(new CodeNameClass_1.CodeNameClass("MAS", "Manually Set"));
        }
        if (this.EntityPM != null && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ARInvoiceStockId)) {
            this.selectedInvoiceNumberFilter = this.InvoiceNumberFilterList.filter(function (a) { return a.Code == "STK"; })[0];
        }
        else if (this.EntityPM != null && this.EntityPM.IsInvoiceNumberFromStock && this.EntityPM.IsAutoCredit) {
            this.selectedInvoiceNumberFilter = this.InvoiceNumberFilterList.filter(function (a) { return a.Code == "STK"; })[0];
        }
        else if (this.EntityPM != null && this.EntityPM.IsInvoiceNumberManuallySet == true) {
            this.selectedInvoiceNumberFilter = this.InvoiceNumberFilterList.filter(function (a) { return a.Code == "MAS"; })[0];
        }
        else {
            this.selectedInvoiceNumberFilter = this.InvoiceNumberFilterList.filter(function (a) { return a.Code == "CNR"; })[0];
        }
    };
    ARInvoiceDetailsTabNormal.prototype.GetInvoiceNumberFromStock = function () {
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
    ARInvoiceDetailsTabNormal.prototype.ReturnInvoiceNumberToStock = function () {
        this.ARInvoiceStockId = null;
        this.InvoiceNumber = null;
        this.IsInvoiceNumberComboBoxEnabled = true;
        this.SelectedInvoiceNumberFilter = this.InvoiceNumberFilterList.filter(function (a) { return a.Code == "CNR"; })[0];
        this.CurrentSession.CurrentEditComponent.SaveChanges();
    };
    ARInvoiceDetailsTabNormal.prototype.SetStockProperties = function (stockLineSelectedItem, msg) {
        if (msg === void 0) { msg = null; }
        this.IsgetFromStockAfterSaving = true;
        this.InvoiceNumber = stockLineSelectedItem.Number;
        this.ARInvoiceStockId = stockLineSelectedItem.Id;
        this.IsInvoiceNumberComboBoxEnabled = false;
        this.CurrentSession.CurrentEditComponent.SaveChanges(msg);
    };
    ARInvoiceDetailsTabNormal = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ARInvoiceDetailsTabNormal.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ARInvoiceDetailsTabNormal);
    return ARInvoiceDetailsTabNormal;
}(BaseComponent_1.BaseComponent));
exports.ARInvoiceDetailsTabNormal = ARInvoiceDetailsTabNormal;
var ARInvoiceLineItem = /** @class */ (function (_super) {
    __extends(ARInvoiceLineItem, _super);
    function ARInvoiceLineItem(entityPM, fatherComponent) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.EntityPM = null;
        _this.ObjectTableName = "ARInvoiceLine";
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsEditingEnabled = false;
        _this.IsRateEnabled = false;
        _this.IsEditExchangeRateVisible = false;
        _this.IsMatched = false;
        _this.IsMatchedPrepaidCollect = false;
        _this.VatTypeUpdateIsVisible = false;
        _this.VatTypeMultiIconVisible = false;
        _this.VatTypesGroups = [];
        _this.myRelativeRateDate = null;
        _this.EntityPM = entityPM;
        _this.LocalCurrencyId = fatherComponent.LocalCurrencyId;
        _this.ReadIsMatched();
        _this.ReadCellBackground();
        _this.ReadVatTypeData();
        _this.ComputeRelativeRateDate();
        _this.SetUIProperties();
        return _this;
    }
    ARInvoiceLineItem.prototype.SetUIProperties = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "ARInvoiceEditExchangeRate")) {
            this.IsEditExchangeRateVisible = true;
        }
        //if (this.EntityPM.IsBackToBack) {
        //    this.IsEditingEnabled = false;
        //} else {
        //    this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;
        //}
        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;
        this.UIProperties.SetEnabled("ForiegnCurrencyAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("LocalCurrencyAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("InvoiceCurrencyAmount", this.ObjectTableName, false);
        this.UIProperties.SetVisibility("InvoiceCurrencyAmount", this.ObjectTableName, false);
        if (this.fatherComponent.InvoiceCurrencyId != this.LocalCurrencyId) {
            this.UIProperties.SetVisibility("InvoiceCurrencyAmount", this.ObjectTableName, true);
        }
        this.UIProperties.SetEnabled("Description", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("LocalDescription", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("UnitPrice", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.IsEditingEnabled);
        this.SetUIProperties_Rate();
        this.SetUIProperties_VAT();
    };
    ARInvoiceLineItem.prototype.SetUIProperties_Rate = function () {
        var isFieldEnabled = false;
        if (this.IsEditingEnabled) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "ARInvoiceEditExchangeRate")) {
                if (this.ForiegnCurrencyId) {
                    if (this.ForiegnCurrencyId != this.LocalCurrencyId) {
                        if (!this.IsExchangeRateFixed) {
                            isFieldEnabled = true;
                        }
                    }
                }
            }
        }
        this.IsRateEnabled = isFieldEnabled;
        this.UIProperties.SetEnabled("ForiegnExchangeRate", this.ObjectTableName, isFieldEnabled);
    };
    ARInvoiceLineItem.prototype.SetUIProperties_VAT = function () {
        this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("VatPercentage", this.ObjectTableName, this.IsEditingEnabled);
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
    ARInvoiceLineItem.prototype.RefreshLine = function () {
        this.ReadIsMatched();
        this.ReadCellBackground();
    };
    ARInvoiceLineItem.prototype.ReadIsMatched = function () {
        var isMatched = false;
        var isMatchedCustomsOnly = true;
        var isMatchedPrepaidCollect = false;
        if (this.fatherComponent.IsCustomsInvoice) {
            if (this.fatherComponent.IsCustomsChargesOnly) {
                if (!this.IsCustomsCharge) {
                    isMatchedCustomsOnly = false;
                }
            }
        }
        if (this.fatherComponent.PrepaidCollectId == "B") {
            isMatchedPrepaidCollect = true;
        }
        else if (this.fatherComponent.PrepaidCollectId == this.PrepaidCollectId) {
            isMatchedPrepaidCollect = true;
        }
        if (isMatchedCustomsOnly && isMatchedPrepaidCollect) {
            isMatched = true;
        }
        this.IsMatched = isMatched;
        this.IsMatchedPrepaidCollect = isMatchedPrepaidCollect;
    };
    ARInvoiceLineItem.prototype.ReadCellBackground = function () {
        var myResult = "transparent";
        if (this.Exists) {
            myResult = "rgba(208, 224, 234, 0.4)";
        }
        this.CellBackground = myResult;
    };
    Object.defineProperty(ARInvoiceLineItem.prototype, "Exists", {
        get: function () {
            var myResult = false;
            if (this.fatherComponent.EntityPM.InvoiceLines.indexOf(this.EntityPM) > -1) {
                myResult = true;
            }
            return myResult;
        },
        set: function (newValue) {
            if (newValue == true) {
                this.fatherComponent.EntityPM.AddARInvoiceLinePM(this.EntityPM);
                this.OnInvoiceExchangeRateChanged();
            }
            else {
                this.fatherComponent.EntityPM.RemoveARInvoiceLinePM(this.EntityPM);
            }
            this.ReadCellBackground();
            this.fatherComponent.ComputeTotals();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "EntityReference", {
        // Properties
        get: function () { return this.EntityPM.EntityReference; },
        set: function (newValue) {
            if (this.EntityPM.EntityReference != newValue) {
                this.EntityPM.EntityReference = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "ForiegnCurrencyId", {
        get: function () { return this.EntityPM.ForiegnCurrencyId; },
        set: function (newValue) {
            if (this.EntityPM.ForiegnCurrencyId != newValue) {
                this.EntityPM.ForiegnCurrencyId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "ForiegnCurrencyCode", {
        get: function () { return this.EntityPM.ForiegnCurrencyCode; },
        set: function (newValue) {
            if (this.EntityPM.ForiegnCurrencyCode != newValue) {
                this.EntityPM.ForiegnCurrencyCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "LocalCurrencyCode", {
        get: function () { return this.EntityPM.InvoiceLocalCurrencyCode; },
        set: function (newValue) {
            if (this.EntityPM.InvoiceLocalCurrencyCode != newValue) {
                this.EntityPM.InvoiceLocalCurrencyCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "InvoiceCurrencyCode", {
        get: function () { return this.EntityPM.InvoiceCurrencyCode; },
        set: function (newValue) {
            if (this.EntityPM.InvoiceCurrencyCode != newValue) {
                this.EntityPM.InvoiceCurrencyCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "PrepaidCollectId", {
        get: function () { return this.EntityPM.PrepaidCollectId; },
        set: function (newValue) {
            if (this.EntityPM.PrepaidCollectId != newValue) {
                this.EntityPM.PrepaidCollectId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "MeasurementId", {
        get: function () { return this.EntityPM.MeasurementId; },
        set: function (newValue) {
            if (this.EntityPM.MeasurementId != newValue) {
                this.EntityPM.MeasurementId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "MeasurementCode", {
        get: function () { return this.EntityPM.MeasurementCode; },
        set: function (newValue) {
            if (this.EntityPM.MeasurementCode != newValue) {
                this.EntityPM.MeasurementCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "ChargesTypeId", {
        get: function () { return this.EntityPM.ChargesTypeId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.ChargesTypeId != value) {
                this.EntityPM.ChargesTypeId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.Description = null;
                    this.LocalDescription = null;
                    this.IsCustomsCharge = false;
                }
                else {
                    this.fatherComponent.myChargesTypeListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.Description = list.EnglishName;
                                _this.LocalDescription = list.LocalName;
                                _this.IsCustomsCharge = list.IsCustoms;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (newValue) {
            if (this.EntityPM.Description != newValue) {
                this.EntityPM.Description = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "LocalDescription", {
        get: function () { return this.EntityPM.LocalDescription; },
        set: function (newValue) {
            if (this.EntityPM.LocalDescription != newValue) {
                this.EntityPM.LocalDescription = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "IsCustomsCharge", {
        get: function () { return this.EntityPM.IsCustomsCharge; },
        set: function (value) {
            if (this.EntityPM.IsCustomsCharge != value) {
                this.EntityPM.IsCustomsCharge = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (newValue) {
            if (this.EntityPM.Notes != newValue) {
                this.EntityPM.Notes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "IsExchangeRateFixed", {
        get: function () { return this.EntityPM.IsExchangeRateFixed; },
        set: function (newValue) {
            if (this.EntityPM.IsExchangeRateFixed != newValue) {
                this.EntityPM.IsExchangeRateFixed = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceLineItem.prototype.UpdateCurrencyRateClicked = function () {
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
                    _this.ExchangeRateDate = comp.RateDate;
                }
            });
        });
        logWindow.Show('./CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent');
    };
    Object.defineProperty(ARInvoiceLineItem.prototype, "VatTypeId", {
        // VAT Type
        get: function () { return this.EntityPM.VatTypeId; },
        set: function (newValue) {
            if (this.EntityPM.VatTypeId != newValue) {
                this.EntityPM.VatTypeId = newValue;
                this.fatherComponent.ItemsSource.filter(function (f) { return f.VatTypeId == newValue; }).forEach(function (item) {
                    item.GetVatTypeData();
                });
                this.GetVatTypeData();
                this.fatherComponent.SetGridColumnsWidth();
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceLineItem.prototype.GetVatTypeData = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.VatTypeId)) {
            this.VatTypeName = null;
            this.VatPercentage = null;
            this.VatIsMultiPercentage = false;
            //this.EntityPM.ExternalVATCard = null;
            this.EntityPM.ExternalTAXItemId = null;
            this.ReadVatTypeData();
            this.fatherComponent.ComputeTotals();
            this.SetUIProperties_VAT();
        }
        else {
            this.fatherComponent.myVatTypeListService.getSingleFromCache(this.VatTypeId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        _this.VatTypeName = list.EnglishName;
                        _this.VatIsMultiPercentage = list.IsMultiPercentage;
                        //this.EntityPM.ExternalVATCard = list.ExternalVATCard;
                        _this.EntityPM.ExternalTAXItemId = list.ExternalTAXItemId;
                        if (list.IsMultiPercentage) {
                            _this.VatPercentage = null;
                        }
                        else {
                            _this.VatPercentage = _this.fatherComponent.GetVatTypePercentage(_this.VatTypeId);
                        }
                        _this.ReadVatTypeData();
                        _this.fatherComponent.ComputeTotals();
                        _this.SetUIProperties_VAT();
                    }
                }
            });
        }
    };
    ARInvoiceLineItem.prototype.SetVatPercentage = function (myPercentage) {
        this.VatPercentage = myPercentage;
    };
    Object.defineProperty(ARInvoiceLineItem.prototype, "VatTypeName", {
        get: function () { return this.EntityPM.VatTypeName; },
        set: function (newValue) {
            if (this.EntityPM.VatTypeName != newValue) {
                this.EntityPM.VatTypeName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "VatPercentage", {
        get: function () { return this.EntityPM.VatPercentage; },
        set: function (newValue) {
            if (this.EntityPM.VatPercentage != newValue) {
                this.EntityPM.VatPercentage = Tools_1.AppTool.Round(newValue, 2);
                this.ReadVatTypeData();
                this.ReCalculateTotals();
                this.SetUIProperties_VAT();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "VatIsMultiPercentage", {
        get: function () { return this.EntityPM.VatIsMultiPercentage; },
        set: function (value) {
            if (this.EntityPM.VatIsMultiPercentage != value) {
                this.EntityPM.VatIsMultiPercentage = value;
                this.SetUIProperties_VAT();
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceLineItem.prototype.ReadVatTypeData = function () {
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
    ARInvoiceLineItem.prototype.UpdateVatPercentageClicked = function () {
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
                        _this.fatherComponent.ItemsSource.filter(function (f) { return f.VatTypeId == _this.VatTypeId; }).forEach(function (item) {
                            item.SetVatPercentage(comp.Percentage);
                        });
                    }
                });
            });
            logWindow.Show('./CommonModules/CommonOthers/Components/UpdateVATPercentage/UpdateVATPercentageComponent');
        }
    };
    Object.defineProperty(ARInvoiceLineItem.prototype, "Quantity", {
        // Amounts
        get: function () { return this.EntityPM.Quantity; },
        set: function (newValue) {
            if (this.EntityPM.Quantity != newValue) {
                this.EntityPM.Quantity = Tools_1.AppTool.Round(newValue, 3);
                this.ComputeTotal();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "UnitPrice", {
        get: function () { return this.EntityPM.UnitPrice; },
        set: function (newValue) {
            if (this.EntityPM.UnitPrice != newValue) {
                this.EntityPM.UnitPrice = Tools_1.AppTool.Round(newValue, 3);
                this.ComputeTotal();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "ForiegnCurrencyAmount", {
        get: function () { return this.EntityPM.ForiegnCurrencyAmount; },
        set: function (newValue) {
            if (this.EntityPM.ForiegnCurrencyAmount != newValue) {
                this.EntityPM.ForiegnCurrencyAmount = Tools_1.AppTool.Round(newValue, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "ForiegnExchangeRate", {
        get: function () { return this.EntityPM.ForiegnExchangeRate; },
        set: function (value) {
            if (!this.EntityPM.IsExchangeRateFixed) {
                if (this.EntityPM.ForiegnExchangeRate != value) {
                    this.EntityPM.ForiegnExchangeRate = Tools_1.AppTool.Round(value, 5);
                    if (this.ForiegnCurrencyId == this.fatherComponent.ProfitCurrencyId) {
                        if (this.fatherComponent.ProfitCurrencyExchangeRate != value) {
                            this.fatherComponent.ProfitCurrencyExchangeRate = value;
                        }
                    }
                    if (this.ForiegnCurrencyId == this.fatherComponent.InvoiceCurrencyId) {
                        this.fatherComponent.UpdateRateFromLine(this);
                        this.ExchangeRateDate = this.fatherComponent.GetCurrencyRateDate(this.ForiegnCurrencyId);
                    }
                    this.CalculateLocalCurrencyAmount();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "ExchangeRateDate", {
        get: function () { return this.EntityPM.ExchangeRateDate; },
        set: function (newValue) {
            if (this.EntityPM.ExchangeRateDate != newValue) {
                this.EntityPM.ExchangeRateDate = newValue;
                this.ComputeRelativeRateDate();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "RelativeRateDate", {
        get: function () { return this.myRelativeRateDate; },
        set: function (value) {
            if (this.myRelativeRateDate != value) {
                this.myRelativeRateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceLineItem.prototype.ComputeRelativeRateDate = function () {
        this.RelativeRateDate = Tools_1.DateTool.GetRelativeRateDate(this.fatherComponent.InvoiceDate, this.ExchangeRateDate, "old");
    };
    Object.defineProperty(ARInvoiceLineItem.prototype, "LocalCurrencyAmount", {
        get: function () { return this.EntityPM.LocalCurrencyAmount; },
        set: function (newValue) {
            if (this.EntityPM.LocalCurrencyAmount != newValue) {
                this.EntityPM.LocalCurrencyAmount = Tools_1.AppTool.Round(newValue, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "ProfitCurrencyAmount", {
        get: function () { return this.EntityPM.ProfitCurrencyAmount; },
        set: function (newValue) {
            if (this.EntityPM.ProfitCurrencyAmount != newValue) {
                this.EntityPM.ProfitCurrencyAmount = Tools_1.AppTool.Round(newValue, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceLineItem.prototype, "InvoiceCurrencyAmount", {
        get: function () { return this.EntityPM.InvoiceCurrencyAmount; },
        set: function (newValue) {
            if (this.EntityPM.InvoiceCurrencyAmount != newValue) {
                this.EntityPM.InvoiceCurrencyAmount = Tools_1.AppTool.Round(newValue, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceLineItem.prototype.ReCalculateTotals = function () {
        if (this.Exists) {
            this.fatherComponent.ComputeTotals();
        }
    };
    ARInvoiceLineItem.prototype.ComputeTotal = function () {
        var foriegnCurrencyAmount = null;
        var localCurrencyAmount = null;
        var profitCurrencyAmount = null;
        var invoiceCurrencyAmount = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Quantity) && !Tools_1.AppTool.IsNullOrEmpty(this.UnitPrice)) {
            if (this.MeasurementCode == "PRVL" || this.MeasurementCode == "PRFR") {
                var price = this.UnitPrice / 100;
                foriegnCurrencyAmount = this.Quantity * price;
            }
            else {
                foriegnCurrencyAmount = this.Quantity * this.UnitPrice;
            }
            localCurrencyAmount = foriegnCurrencyAmount * this.ForiegnExchangeRate;
            profitCurrencyAmount = 0;
            invoiceCurrencyAmount = 0;
            if (this.fatherComponent.InvoiceCurrencyId == this.ForiegnCurrencyId) {
                invoiceCurrencyAmount = foriegnCurrencyAmount;
            }
            else if (this.fatherComponent.InvoiceCurrencyExchangeRate > 0) {
                invoiceCurrencyAmount = localCurrencyAmount / this.fatherComponent.InvoiceCurrencyExchangeRate;
            }
            if (this.fatherComponent.ProfitCurrencyId == this.ForiegnCurrencyId) {
                profitCurrencyAmount = foriegnCurrencyAmount;
            }
            else if (this.fatherComponent.ProfitCurrencyExchangeRate > 0) {
                profitCurrencyAmount = localCurrencyAmount / this.fatherComponent.ProfitCurrencyExchangeRate;
            }
        }
        this.ForiegnCurrencyAmount = foriegnCurrencyAmount;
        this.LocalCurrencyAmount = localCurrencyAmount;
        this.ProfitCurrencyAmount = profitCurrencyAmount;
        this.InvoiceCurrencyAmount = invoiceCurrencyAmount;
        this.fatherComponent.ComputeTotals();
    };
    ARInvoiceLineItem.prototype.CalculateLocalCurrencyAmount = function () {
        var localCurrencyAmount = this.ForiegnCurrencyAmount * this.ForiegnExchangeRate;
        var profitCurrencyAmount = localCurrencyAmount / this.fatherComponent.ProfitCurrencyExchangeRate;
        var invoiceCurrencyAmount = 0;
        if (this.ForiegnCurrencyId == this.fatherComponent.InvoiceCurrencyId) {
            invoiceCurrencyAmount = this.ForiegnCurrencyAmount;
        }
        else if (this.fatherComponent.InvoiceCurrencyExchangeRate > 0) {
            invoiceCurrencyAmount = localCurrencyAmount / this.fatherComponent.InvoiceCurrencyExchangeRate;
        }
        this.LocalCurrencyAmount = localCurrencyAmount;
        this.ProfitCurrencyAmount = profitCurrencyAmount;
        this.InvoiceCurrencyAmount = invoiceCurrencyAmount;
        this.fatherComponent.ComputeTotals();
    };
    ARInvoiceLineItem.prototype.OnInvoiceExchangeRateChanged = function () {
        if (this.ForiegnCurrencyId == this.fatherComponent.InvoiceCurrencyId) {
            this.ForiegnExchangeRate = this.fatherComponent.InvoiceCurrencyExchangeRate;
        }
        else {
            this.ForiegnExchangeRate = this.fatherComponent.GetCurrencyRate(this.ForiegnCurrencyId);
        }
        this.ExchangeRateDate = this.fatherComponent.GetCurrencyRateDate(this.ForiegnCurrencyId);
        this.ComputeRelativeRateDate();
        this.CalculateInvoiceCurrencyAmount();
    };
    ARInvoiceLineItem.prototype.CalculateInvoiceCurrencyAmount = function () {
        if (this.ForiegnCurrencyId == this.fatherComponent.InvoiceCurrencyId) {
            this.InvoiceCurrencyAmount = this.ForiegnCurrencyAmount;
        }
        else {
            if (this.fatherComponent.InvoiceCurrencyExchangeRate > 0) {
                this.InvoiceCurrencyAmount = this.LocalCurrencyAmount / this.fatherComponent.InvoiceCurrencyExchangeRate;
            }
            else {
                this.InvoiceCurrencyAmount = 0;
            }
        }
        this.fatherComponent.ComputeTotals();
    };
    return ARInvoiceLineItem;
}(BaseComponent_1.BaseComponent));
exports.ARInvoiceLineItem = ARInvoiceLineItem;
//# sourceMappingURL=ARInvoiceDetailsTabNormal.js.map