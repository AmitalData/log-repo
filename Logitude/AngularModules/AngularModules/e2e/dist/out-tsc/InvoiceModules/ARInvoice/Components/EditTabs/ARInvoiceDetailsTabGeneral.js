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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
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
var GLAccountPMService_1 = require("../../../../Accounting/Services/StandardPMs/GLAccountPMService");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var ARInvoiceDetailsTabGeneral = /** @class */ (function (_super) {
    __extends(ARInvoiceDetailsTabGeneral, _super);
    function ARInvoiceDetailsTabGeneral(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.ObjectTableName = "ARInvoice";
        _this.DataContext = _this;
        _this.ItemsSource = [];
        _this.IsManifest = false;
        _this.IsCustomsInvoice = false;
        _this.IsEditExchangeRateVisible = false;
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.LocalAmountHeader = null;
        _this.InvoiceAmountHeader = null;
        _this.IsInvoiceAmountHeaderVisible = false;
        _this.VATColumnWidth = 100;
        _this.RateColumnWidth = 100;
        // SetUIProperties
        _this.IsEditingEnabled = false;
        _this.RateIsEnabled = false;
        _this.VatTypeFilterIsEnabled = false;
        _this.AllowManualInvoiceNumber = false;
        _this.PaymentTermDisplayInLOV = true;
        // Bill To
        _this.BillToDependencyProperty1 = null;
        _this.cardList = null;
        _this.glaccount = null;
        _this.myRelativeRateDate = null;
        // Load Date 
        _this.LastRatesList = [];
        _this.VatTypePercentagesList = [];
        _this.isTotalInLocalCurrency = false;
        _this.TotalsList = [];
        _this.SummaryItems = [];
        // this.CurrentSession.StartBusyIndicatorLoading();
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.EntityPM = entityArgs.EntityPM;
        _this.IsManifest = _this.EntityPM.ARInvoiceTypeCode == "MN" ? true : false;
        _this.IsCustomsInvoice = (_this.EntityPM.ARInvoiceTypeCode == "CI" || _this.EntityPM.ARInvoiceTypeCode == "CC") ? true : false;
        _this.ObservableItems = new ObservableCollection_1.ObservableCollection([]);
        _this.LocalCurrencyCode = SessionLocator_1.SessionLocator.LocalCurrencyCode;
        _this.InitializeServices();
        _this.InitializeComponent();
        _this.SetUIProperties();
        _this.BuildScreenData();
        _this.Listen();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            _this.IsEditExchangeRateVisible = true;
        }
        return _this;
    }
    ARInvoiceDetailsTabGeneral.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.BuildScreenData();
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.BuildScreenData();
                }
            });
        }
    };
    ARInvoiceDetailsTabGeneral.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    ARInvoiceDetailsTabGeneral.prototype.InitializeServices = function () {
        this.myCardListService = new CardListService_1.CardListService();
        this.myCurrencyListService = new CurrencyListService_1.CurrencyListService();
        this.myPaymentTermListService = new PaymentTermListService_1.PaymentTermListService();
        this.myVatTypeListService = new VatTypeListService_1.VatTypeListService();
        this.myChargesTypeListService = new ChargesTypeListService_1.ChargesTypeListService();
        this.myCommonDomainService = new CommonDomainService_1.CommonDomainService();
        this.myGLAccountPMService = new GLAccountPMService_1.GLAccountPMService();
    };
    ARInvoiceDetailsTabGeneral.prototype.InitializeComponent = function () {
        this.SetGridColumns();
        this.ComputeRelativeRateDate();
        this.BillToDependencyProperty1 = Tools_2.InvoiceTool.GetBillToPartnerTypes();
    };
    ARInvoiceDetailsTabGeneral.prototype.SetGridColumns = function () {
        this.LocalAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoiceLine.CH.LocalCurrencyAmount").replace("%LocalCurrencyCode", SessionLocator_1.SessionLocator.LocalCurrencyCode);
        var invoiceAmountHeader = null;
        var isInvoiceAmountHeaderVisible = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
            if (this.InvoiceCurrencyId != SessionLocator_1.SessionLocator.LocalCurrencyId) {
                isInvoiceAmountHeaderVisible = true;
                invoiceAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoiceLine.CH.AmountInvoice").replace("%InvoiceCurrencyCode", this.InvoiceCurrencyCode);
            }
        }
        this.InvoiceAmountHeader = invoiceAmountHeader;
        this.IsInvoiceAmountHeaderVisible = isInvoiceAmountHeaderVisible;
    };
    ARInvoiceDetailsTabGeneral.prototype.SetGridColumnsWidth = function () {
        var isRateExists = this.ItemsSource.filter(function (f) { return !Tools_1.AppTool.IsNullOrEmpty(f.RelativeRateDate); }).length > 0 ? true : false;
        if (isRateExists) {
            this.RateColumnWidth = 120;
        }
        else {
            this.RateColumnWidth = 100;
        }
        var isVATExists = this.ItemsSource.filter(function (f) { return f.VatTypeUpdateIsVisible == true; }).length > 0 ? true : false;
        if (isVATExists) {
            this.VATColumnWidth = 150;
        }
        else {
            var vATColumnWidth = 100;
            this.ItemsSource.forEach(function (item) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item.VatTypeCell)) {
                    var widthOfLabel = Tools_1.AppTool.GetTextWidth(item.VatTypeCell) + 10;
                    if (widthOfLabel > vATColumnWidth) {
                        vATColumnWidth = widthOfLabel;
                    }
                }
            });
            if (vATColumnWidth > 150) {
                vATColumnWidth = 150;
            }
            this.VATColumnWidth = vATColumnWidth;
        }
    };
    ARInvoiceDetailsTabGeneral.prototype.SetUIProperties = function () {
        var isEditingEnabled = Tools_2.InvoiceTool.IsEditingARInvoiceEnabled(this.EntityPM);
        if (!Tools_1.AppTool.IsNullOrEmpty(this.BillToAddressId)) {
            this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, isEditingEnabled);
        }
        this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("PaymentTermId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("InvoiceCurrencyId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("VatNumber", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("InvoiceDate", this.ObjectTableName, isEditingEnabled);
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
    ARInvoiceDetailsTabGeneral.prototype.SetUIProperties_BillToAddress = function () {
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
    ARInvoiceDetailsTabGeneral.prototype.SetUIProperties_VatNumber = function () {
        var isFieldRequired = false;
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAR) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.VatNumber)) {
                isFieldRequired = true;
            }
            this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, isFieldRequired);
        }
    };
    ARInvoiceDetailsTabGeneral.prototype.SetUIProperties_ExchangeRate = function () {
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
    ARInvoiceDetailsTabGeneral.prototype.SetUIProperties_PrintNotes = function () {
        var isFieldtEnabled = true;
        if (this.EntityPM.StatusCode == "VD") {
            isFieldtEnabled = false;
        }
        else if (this.EntityPM.IsPrinted) {
            isFieldtEnabled = false;
        }
        this.UIProperties.SetEnabled("PrintNotes", this.ObjectTableName, isFieldtEnabled);
    };
    ARInvoiceDetailsTabGeneral.prototype.SetUIProperties_ManuallySet = function () {
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
    ARInvoiceDetailsTabGeneral.prototype.SetUIProperties_InvoiceNumber = function () {
        var isFieldEnabled = false;
        if (this.IsEditingEnabled) {
            if (this.IsInvoiceNumberManuallySet) {
                isFieldEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("InvoiceNumber", this.ObjectTableName, isFieldEnabled);
    };
    ARInvoiceDetailsTabGeneral.prototype.SetUIProperties_VatTypeFilter = function () {
        var isFieldtEnabled = this.IsEditingEnabled;
        if (isFieldtEnabled) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.VatTypeId)) {
                isFieldtEnabled = false;
            }
        }
        this.VatTypeFilterIsEnabled = isFieldtEnabled;
    };
    ARInvoiceDetailsTabGeneral.prototype.SetUIProperties_DueDate = function () {
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
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "BillToId", {
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
                }
                else {
                    this.myCardListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.cardList = myResponse.Result;
                            if (_this.cardList != null) {
                                _this.VatNumber = _this.cardList.VatNumber;
                                _this.BillToName = _this.cardList.EnglishName;
                                _this.EntityPM.SalesmanUserId = _this.cardList.SalesmanUserId;
                                if (!Tools_1.AppTool.IsNullOrEmpty(_this.cardList.GLAccountId)) {
                                    _this.myGLAccountPMService.get(_this.cardList.GLAccountId).subscribe(function (myResponse) {
                                        if (!myResponse.HasError) {
                                            _this.glaccount = myResponse.Result;
                                        }
                                    });
                                }
                                if (!Tools_1.AppTool.IsNullOrEmpty(_this.cardList.InvoiceCurrencyId)) {
                                    _this.InvoiceCurrencyId = _this.cardList.InvoiceCurrencyId;
                                }
                                if (!Tools_1.AppTool.IsNullOrEmpty(_this.cardList.PaymentTermId)) {
                                    _this.PaymentTermId = _this.cardList.PaymentTermId;
                                }
                                if (!Tools_1.AppTool.IsNullOrEmpty(_this.cardList.VatTypeId)) {
                                    _this.VatTypeId = _this.cardList.VatTypeId;
                                }
                                if (!Tools_1.AppTool.IsNullOrEmpty(_this.cardList.BillingAddressId)) {
                                    _this.BillToAddressId = _this.cardList.BillingAddressId;
                                }
                                else if (!Tools_1.AppTool.IsNullOrEmpty(_this.cardList.MainAddressId)) {
                                    _this.BillToAddressId = _this.cardList.MainAddressId;
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
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "BillToName", {
        get: function () { return this.EntityPM.BillToName; },
        set: function (newValue) {
            if (this.EntityPM.BillToName != newValue) {
                this.EntityPM.BillToName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "BillToAddressId", {
        get: function () { return this.EntityPM.BillToAddressId; },
        set: function (newValue) {
            if (this.EntityPM.BillToAddressId != newValue) {
                this.EntityPM.BillToAddressId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "InvoiceCurrencyId", {
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
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "InvoiceCurrencyCode", {
        get: function () { return this.EntityPM.InvoiceCurrencyCode; },
        set: function (value) {
            if (this.EntityPM.InvoiceCurrencyCode != value) {
                this.EntityPM.InvoiceCurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "InvoiceCurrencyExchangeRate", {
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
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "ExchangeRateDate", {
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
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "RelativeRateDate", {
        get: function () { return this.myRelativeRateDate; },
        set: function (value) {
            if (this.myRelativeRateDate != value) {
                this.myRelativeRateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceDetailsTabGeneral.prototype.ComputeRelativeRateDate = function () {
        this.RelativeRateDate = Tools_1.DateTool.GetRelativeRateDate(this.InvoiceDate, this.ExchangeRateDate, "old");
    };
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "ProfitCurrencyId", {
        get: function () { return this.EntityPM.ProfitCurrencyId; },
        set: function (newValue) {
            if (this.EntityPM.ProfitCurrencyId != newValue) {
                this.EntityPM.ProfitCurrencyId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "ProfitCurrencyExchangeRate", {
        get: function () { return this.EntityPM.ProfitCurrencyExchangeRate; },
        set: function (newValue) {
            if (this.EntityPM.ProfitCurrencyExchangeRate != newValue) {
                this.EntityPM.ProfitCurrencyExchangeRate = Tools_1.AppTool.Round(newValue, 5);
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceDetailsTabGeneral.prototype.UpdateRateFromLine = function (line) {
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
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "VatNumber", {
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
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "PaymentTermId", {
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
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "PaymentTermName", {
        get: function () { return this.EntityPM.PaymentTermName; },
        set: function (newValue) {
            if (this.EntityPM.PaymentTermName != newValue) {
                this.EntityPM.PaymentTermName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "InvoiceDate", {
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
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "DueDate", {
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
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "DraftNumber", {
        get: function () { return this.EntityPM.DraftNumber; },
        set: function (newValue) {
            if (this.EntityPM.DraftNumber != newValue) {
                this.EntityPM.DraftNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "InvoiceNumber", {
        get: function () {
            if (this.EntityPM.Id == this.EntityPM.InvoiceNumber) {
                return null;
            }
            else {
                return this.EntityPM.InvoiceNumber;
            }
        },
        set: function (newValue) {
            if (this.EntityPM.InvoiceNumber != newValue) {
                this.EntityPM.InvoiceNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "StatusCode", {
        get: function () { return this.EntityPM.StatusCode; },
        set: function (newValue) {
            if (this.EntityPM.StatusCode != newValue) {
                this.EntityPM.StatusCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "IsInvoiceNumberManuallySet", {
        get: function () { return this.EntityPM.IsInvoiceNumberManuallySet; },
        set: function (newValue) {
            if (this.EntityPM.IsInvoiceNumberManuallySet != newValue) {
                this.EntityPM.IsInvoiceNumberManuallySet = newValue;
                this.UIProperties.SetEnabled("InvoiceNumber", this.ObjectTableName, newValue);
                if (!newValue) {
                    this.InvoiceNumber = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "PrintNotes", {
        get: function () { return this.EntityPM.PrintNotes; },
        set: function (newValue) {
            if (this.EntityPM.PrintNotes != newValue) {
                this.EntityPM.PrintNotes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "VatTypeId", {
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
    ARInvoiceDetailsTabGeneral.prototype.VatTypeFilterClicked = function () {
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
                        item.VatTypeId = vatType;
                    });
                    _this.SetGridColumnsWidth();
                }
            });
        }
    };
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "PrepaidCollectId", {
        // Prepaid Collect Filter
        get: function () { return this.EntityPM.PrepaidCollectId; },
        set: function (newValue) {
            if (this.EntityPM.PrepaidCollectId != newValue) {
                this.EntityPM.PrepaidCollectId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceDetailsTabGeneral.prototype.OnSelectPrepaidCollect = function (myPrepaidCollectId) {
        if (this.PrepaidCollectId != myPrepaidCollectId) {
            this.PrepaidCollectId = myPrepaidCollectId;
            this.RunPrepaidCollectFilter();
        }
    };
    ARInvoiceDetailsTabGeneral.prototype.RunPrepaidCollectFilter = function () {
        var _this = this;
        this.ItemsSource.forEach(function (item) {
            if (_this.PrepaidCollectId == "B") {
                if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Id)) {
                    if (_this.EntityPM.InvoiceLines.indexOf(item.EntityPM) == -1) {
                        _this.EntityPM.AddARInvoiceLinePM(item.EntityPM);
                    }
                }
                else {
                    if (!Tools_1.AppTool.IsNullOrEmpty(item.EntityPM.Id)) {
                        if (_this.EntityPM.InvoiceLines.indexOf(item.EntityPM) == -1) {
                            _this.EntityPM.AddARInvoiceLinePM(item.EntityPM);
                        }
                    }
                }
            }
            else {
                if (item.PrepaidCollectId == _this.PrepaidCollectId) {
                    if (_this.EntityPM.InvoiceLines.indexOf(item.EntityPM) == -1) {
                        _this.EntityPM.AddARInvoiceLinePM(item.EntityPM);
                    }
                }
                else {
                    if (_this.EntityPM.InvoiceLines.indexOf(item.EntityPM) > -1) {
                        _this.EntityPM.RemoveARInvoiceLinePM(item.EntityPM);
                    }
                }
            }
            item.RefreshLine();
        });
        this.ComputeTotals();
    };
    ARInvoiceDetailsTabGeneral.prototype.LoadData = function () {
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
    ARInvoiceDetailsTabGeneral.prototype.UpdateData = function () {
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
    ARInvoiceDetailsTabGeneral.prototype.SetCurrencyRateData = function () {
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
    ARInvoiceDetailsTabGeneral.prototype.GetCurrencyRate = function (currencyId) {
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
    ARInvoiceDetailsTabGeneral.prototype.GetCurrencyRateDate = function (currencyId) {
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
    ARInvoiceDetailsTabGeneral.prototype.GetVatTypePercentage = function (vatTypeId) {
        var myResult = null;
        var vatTypePercentagePM = this.VatTypePercentagesList.filter(function (d) { return d.VatTypeId == vatTypeId; })[0];
        if (vatTypePercentagePM != null) {
            myResult = vatTypePercentagePM.Percentage;
        }
        return myResult;
    };
    ARInvoiceDetailsTabGeneral.prototype.UpdateCurrencyRateClicked = function () {
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
    ARInvoiceDetailsTabGeneral.prototype.BuildScreenData = function () {
        if (this.IsEditingEnabled) {
            this.LoadData();
        }
        else {
            this.BuildInvoiceLines();
        }
    };
    ARInvoiceDetailsTabGeneral.prototype.BuildInvoiceLines = function () {
        var _this = this;
        this.ItemsSource = [];
        this.ObservableItems.Clear();
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            var tempList = [];
            var list = this.EntityPM.InvoiceLines;
            list.forEach(function (line) {
                if (line.UnitPrice < 0) {
                    if (_this.EntityPM.ARInvoiceTypeCode != "CD") {
                        if (_this.EntityPM.InvoiceLines.indexOf(line) > -1) {
                            tempList.push(line);
                            _this.EntityPM.RemoveARInvoiceLinePM(line);
                        }
                    }
                }
                else if (_this.EntityPM.PrepaidCollectId != "B") {
                    if (line.PrepaidCollectId != _this.EntityPM.PrepaidCollectId) {
                        if (_this.EntityPM.InvoiceLines.indexOf(line) > -1) {
                            tempList.push(line);
                            _this.EntityPM.RemoveARInvoiceLinePM(line);
                        }
                    }
                }
                if (tempList.indexOf(line) == -1) {
                    _this.ItemsSource.push(new ARInvoiceLineItem(line, _this, false));
                }
            });
            tempList.forEach(function (item) {
                _this.ItemsSource.push(new ARInvoiceLineItem(item, _this, false));
            });
            this.ItemsSource.forEach(function (item) {
                _this.ObservableItems.Insert(item);
            });
            this.ComputeTotals();
        }
        else {
            if (this.EntityPM != null && this.EntityPM.InvoiceLines != null) {
                this.EntityPM.InvoiceLines.forEach(function (line) {
                    _this.ItemsSource.push(new ARInvoiceLineItem(line, _this, false));
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
            this.BuildTotalsCollection();
            this.BuildTotalsControl();
        }
        this.SetGridColumnsWidth();
    };
    ARInvoiceDetailsTabGeneral.prototype.LoadEntityOpenReceivables = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        var myService = new ShipmentDomainService_1.ShipmentDomainService();
        myService.GetInvoiceOpenAmountReceivables(this.EntityPM.ARInvoiceTypeCode, this.EntityPM.MainEntityId).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var allReceivables = myResponse.Result;
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
                    var allVatTypes = [];
                    _this.myVatTypeListService.getAllFromCache().subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            allVatTypes = myResponse.Result;
                        }
                    });
                    openReceivables.forEach(function (receivable) {
                        var invoiceLine = new ARInvoiceLinePM_1.ARInvoiceLinePM(null);
                        invoiceLine.Tenant = receivable.Tenant;
                        invoiceLine.ChargesTypeId = receivable.ChargesTypeId;
                        invoiceLine.ForiegnCurrencyId = receivable.CurrencyId;
                        invoiceLine.ForiegnCurrencyCode = receivable.CurrencyCode;
                        invoiceLine.Description = receivable.ChargesTypeName;
                        invoiceLine.ReceivableId = receivable.Id;
                        invoiceLine.VatTypeId = receivable.VatTypeId;
                        invoiceLine.VatPercentage = _this.GetVatTypePercentage(receivable.VatTypeId);
                        invoiceLine.MeasurementId = receivable.MeasurementId;
                        invoiceLine.MeasurementCode = receivable.MeasurementCode;
                        invoiceLine.PrepaidCollectId = receivable.PrepaidCollectId;
                        invoiceLine.IsExchangeRateFixed = receivable.IsExchangeRateFixed;
                        invoiceLine.EntityId = receivable.ShipmentId;
                        invoiceLine.EntityReference = receivable.ShipmentNumber;
                        invoiceLine.Quantity = Tools_1.AppTool.Round(receivable.Quantity, 3);
                        invoiceLine.UnitPrice = Tools_1.AppTool.Round(receivable.Quantity, 3);
                        invoiceLine.ForiegnCurrencyAmount = Tools_1.AppTool.Round(receivable.Quantity, 2);
                        invoiceLine.IsExpense = receivable.IsExpense;
                        var itemExchangeRate = invoiceLine.IsExchangeRateFixed ? receivable.Rate : _this.GetCurrencyRate(invoiceLine.ForiegnCurrencyId);
                        var itemExchangeRateRounded = Tools_1.AppTool.Round(itemExchangeRate, 5);
                        invoiceLine.ForiegnExchangeRate = itemExchangeRateRounded;
                        var localAmount = invoiceLine.ForiegnCurrencyAmount * invoiceLine.ForiegnExchangeRate;
                        var localAmountRounded = localAmount == null ? 0 : Tools_1.AppTool.Round(localAmount, 2);
                        invoiceLine.LocalCurrencyAmount = localAmountRounded;
                        var profitCurrencyAmount = invoiceLine.LocalCurrencyAmount / _this.ProfitCurrencyExchangeRate;
                        var profitCurrencyAmountRounded = profitCurrencyAmount == null ? 0 : Tools_1.AppTool.Round(profitCurrencyAmount, 2);
                        invoiceLine.ProfitCurrencyAmount = profitCurrencyAmountRounded;
                        var invoiceCurrencyAmount = invoiceLine.LocalCurrencyAmount / _this.InvoiceCurrencyExchangeRate;
                        var invoiceCurrencyAmountRounded = invoiceCurrencyAmount == null ? 0 : Tools_1.AppTool.Round(invoiceCurrencyAmount, 2);
                        invoiceLine.InvoiceCurrencyAmount = invoiceCurrencyAmountRounded;
                        _this.myChargesTypeListService.getSingleFromCache(receivable.ChargesTypeId).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                var list = myResponse.Result;
                                if (list != null) {
                                    invoiceLine.LocalDescription = list.LocalName;
                                    invoiceLine.VatTypeName = list.VatTypeName;
                                }
                            }
                        });
                        _this.ItemsSource.push(new ARInvoiceLineItem(invoiceLine, _this, false));
                        _this.ObservableItems.Insert(new ARInvoiceLineItem(invoiceLine, _this, false));
                    });
                }
            }
            _this.SetGridColumnsWidth();
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "IsCurrencyFilterVisible", {
        get: function () {
            var myResult = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
                if (SessionLocator_1.SessionLocator.LocalCurrencyId != this.InvoiceCurrencyId) {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "IsTotalInLocalCurrency", {
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
    ARInvoiceDetailsTabGeneral.prototype.ComputeTotals = function () {
        this.BuildTotalsCollection(true);
        this.BuildTotalsControl();
    };
    ARInvoiceDetailsTabGeneral.prototype.BuildTotalsCollection = function (isComputingTotals) {
        if (isComputingTotals === void 0) { isComputingTotals = false; }
        var totalsList = [];
        var invoicelinesList = this.EntityPM.InvoiceLines;
        var sumLocalCurrencyAmount = 0;
        var sumInvoiceCurrencyAmount = 0;
        if (this.EntityPM != null && this.EntityPM.InvoiceLines != null) {
            invoicelinesList.forEach(function (item) {
                if (item.LocalCurrencyAmount != null) {
                    sumLocalCurrencyAmount = sumLocalCurrencyAmount + item.LocalCurrencyAmount;
                }
                if (item.InvoiceCurrencyAmount != null) {
                    sumInvoiceCurrencyAmount = sumInvoiceCurrencyAmount + item.InvoiceCurrencyAmount;
                }
            });
        }
        var subtotal = new Args_1.InvoiceTotalsClass();
        subtotal.RowLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.S.Details.Subtotal");
        subtotal.LocalCurrencyAmount = sumLocalCurrencyAmount;
        subtotal.InvoiceCurrencyAmount = sumInvoiceCurrencyAmount;
        var dataGroupList = [];
        this.ItemsSource.filter(function (f) { return f.VatTypeId != null && f.Exists == true; }).forEach(function (item) {
            var dataGroupItem = dataGroupList.filter(function (d) { return d.VatTypeCell == item.VatTypeCell; })[0];
            if (dataGroupItem == null) {
                dataGroupItem = new Args_1.InvoiceTotalsClass();
                dataGroupItem.RowLabel = item.VatTypeCell;
                dataGroupItem.VatTypeCell = item.VatTypeCell;
                dataGroupItem.LocalCurrencyAmount = 0;
                dataGroupItem.InvoiceCurrencyAmount = 0;
                dataGroupList.push(dataGroupItem);
            }
            if (item.VatPercentage != null) {
                if (item.LocalCurrencyAmount != null) {
                    dataGroupItem.LocalCurrencyAmount = dataGroupItem.LocalCurrencyAmount + (item.VatPercentage * item.LocalCurrencyAmount / 100);
                }
                if (item.InvoiceCurrencyAmount != null) {
                    dataGroupItem.InvoiceCurrencyAmount = dataGroupItem.InvoiceCurrencyAmount + (item.VatPercentage * item.InvoiceCurrencyAmount / 100);
                }
            }
        });
        if (dataGroupList.length > 0) {
            totalsList.push(subtotal);
            dataGroupList.forEach(function (item) {
                totalsList.push(item);
                if (item.LocalCurrencyAmount != null) {
                    sumLocalCurrencyAmount = sumLocalCurrencyAmount + item.LocalCurrencyAmount;
                }
                if (item.InvoiceCurrencyAmount != null) {
                    sumInvoiceCurrencyAmount = sumInvoiceCurrencyAmount + item.InvoiceCurrencyAmount;
                }
            });
        }
        if (isComputingTotals) {
            this.SubTotalInLocalCurrency = Tools_1.AppTool.Round(subtotal.LocalCurrencyAmount, 2);
            this.SubTotalInInvoiceCurrency = Tools_1.AppTool.Round(subtotal.InvoiceCurrencyAmount, 2);
            this.AmountInLocalCurrency = Tools_1.AppTool.Round(sumLocalCurrencyAmount, 2);
            this.AmountInInvoiceCurrency = Tools_1.AppTool.Round(sumInvoiceCurrencyAmount, 2);
            if (this.ProfitCurrencyId == this.InvoiceCurrencyId) {
                this.AmountInProfitCurrency = this.AmountInInvoiceCurrency;
            }
            else {
                this.AmountInProfitCurrency = this.AmountInLocalCurrency / this.ProfitCurrencyExchangeRate;
            }
            this.AmountDue = this.AmountInInvoiceCurrency == null ? 0 : this.AmountInInvoiceCurrency;
            this.AmountDueInLocalCurrency = this.AmountInLocalCurrency == null ? 0 : this.AmountInLocalCurrency;
            this.AmountDueInProfitCurrency = this.AmountInProfitCurrency == null ? 0 : this.AmountInProfitCurrency;
        }
        this.TotalsList = totalsList;
        this.BuildTotalsControl();
    };
    ARInvoiceDetailsTabGeneral.prototype.BuildTotalsControl = function () {
        this.SummaryItems = [];
        var pipe = new NumbersPipe_1.NumbersPipe();
        for (var i = 0; i < this.TotalsList.length; i++) {
            var item = this.TotalsList[i];
            var mySummaryItem = new Args_1.SummaryItem();
            mySummaryItem.Label = item.RowLabel;
            mySummaryItem.Value = this.IsTotalInLocalCurrency ? pipe.transform(item.LocalCurrencyAmount, "N2") : pipe.transform(item.InvoiceCurrencyAmount, "N2");
            this.SummaryItems.push(mySummaryItem);
            if (i + 1 < this.TotalsList.length) {
                var myOperatorItem = new Args_1.SummaryItem();
                myOperatorItem.Value = "+";
                this.SummaryItems.push(myOperatorItem);
            }
            else if (i + 1 == this.TotalsList.length) {
                var myOperatorItem = new Args_1.SummaryItem();
                myOperatorItem.Value = "=";
                this.SummaryItems.push(myOperatorItem);
            }
        }
        // Total
        var myCurrencyCode = this.IsTotalInLocalCurrency ? "(" + SessionLocator_1.SessionLocator.LocalCurrencyCode + ")" : "(" + this.InvoiceCurrencyCode + ")";
        var myTotalItem = new Args_1.SummaryItem();
        myTotalItem.Label = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.F.AmountInInvoiceCurrency");
        myTotalItem.Label += " " + myCurrencyCode;
        myTotalItem.Value = this.IsTotalInLocalCurrency ? pipe.transform(this.EntityPM.AmountInLocalCurrency, "N2") : pipe.transform(this.EntityPM.AmountInInvoiceCurrency, "N2");
        this.SummaryItems.push(myTotalItem);
    };
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "SubTotalInLocalCurrency", {
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
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "SubTotalInInvoiceCurrency", {
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
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "AmountInLocalCurrency", {
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
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "AmountInInvoiceCurrency", {
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
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "AmountInProfitCurrency", {
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
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "AmountDue", {
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
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "AmountDueInLocalCurrency", {
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
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "AmountDueInProfitCurrency", {
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
    ARInvoiceDetailsTabGeneral.prototype.EditLineClicked = function (item) {
        if (item != null) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoiceLine.O.EditInvoiceLine");
            logWindow.DataContext = item;
            logWindow.Show('./InvoiceModules/ARInvoice/Components/EditTabs/AddEditARGeneralInvoiceLineComponent');
        }
    };
    ARInvoiceDetailsTabGeneral.prototype.AddARInvoiceLineClicked = function () {
        var line = new ARInvoiceLinePM_1.ARInvoiceLinePM(null);
        line.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
        line.ARInvoiceId = this.EntityPM.Id;
        line.ForiegnCurrencyId = this.InvoiceCurrencyId;
        line.ForiegnCurrencyCode = this.InvoiceCurrencyCode;
        line.ForiegnExchangeRate = this.InvoiceCurrencyExchangeRate;
        line.LineActionCode = "1";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoiceLine.O.EditInvoiceLine");
        var addEditViewModel = new ARInvoiceLineItem(line, this, true);
        logWindow.DataContext = addEditViewModel;
        logWindow.Show('./InvoiceModules/ARInvoice/Components/EditTabs/AddEditARGeneralInvoiceLineComponent');
    };
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "JournalNumber", {
        // Journal Process
        get: function () {
            return this.EntityPM.JournalNumber;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "JournalId", {
        get: function () {
            return this.EntityPM.JournalId;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceDetailsTabGeneral.prototype, "IsFullAccounting", {
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
    ARInvoiceDetailsTabGeneral.prototype.EditJournal = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: _this.JournalId, ObjectTableName: 'Journal' });
        });
    };
    ARInvoiceDetailsTabGeneral = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ARInvoiceDetailsTabGeneral.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ARInvoiceDetailsTabGeneral);
    return ARInvoiceDetailsTabGeneral;
}(BaseComponent_1.BaseComponent));
exports.ARInvoiceDetailsTabGeneral = ARInvoiceDetailsTabGeneral;
var ARInvoiceLineItem = /** @class */ (function (_super) {
    __extends(ARInvoiceLineItem, _super);
    function ARInvoiceLineItem(entityPM, fatherComponent, AddNewLineMode) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.AddNewLineMode = AddNewLineMode;
        _this.EntityPM = null;
        _this.ObjectTableName = "ARInvoiceLine";
        _this.DataContext = _this;
        _this.IsEditingEnabled = false;
        _this.IsRateEnabled = false;
        _this.IsEditExchangeRateVisible = false;
        _this.IsMatched = false;
        _this.chargesTypeList = null;
        _this.VatTypeUpdateIsVisible = false;
        _this.myRelativeRateDate = null;
        _this.EntityPM = entityPM;
        _this.ReadIsMatched();
        _this.ReadCellBackground();
        _this.ReadVatTypeData();
        _this.ComputeRelativeRateDate();
        _this.SetUIProperties();
        return _this;
    }
    ARInvoiceLineItem.prototype.SetUIProperties = function () {
        this.IsEditExchangeRateVisible = this.fatherComponent.IsEditExchangeRateVisible;
        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;
        this.UIProperties.SetEnabled("ForiegnCurrencyAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("LocalCurrencyAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("InvoiceCurrencyAmount", this.ObjectTableName, false);
        this.UIProperties.SetVisibility("InvoiceCurrencyAmount", this.ObjectTableName, false);
        if (this.fatherComponent.InvoiceCurrencyId != SessionLocator_1.SessionLocator.LocalCurrencyId) {
            this.UIProperties.SetVisibility("InvoiceCurrencyAmount", this.ObjectTableName, true);
        }
        this.UIProperties.SetEnabled("Description", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("LocalDescription", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("UnitPrice", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("VatPercentage", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ForiegnCurrencyId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.IsEditingEnabled);
        this.SetUIProperties_Rate();
    };
    ARInvoiceLineItem.prototype.SetUIProperties_Rate = function () {
        var isFieldEnabled = false;
        if (this.IsEditingEnabled) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "ARInvoiceEditExchangeRate")) {
                if (this.ForiegnCurrencyId != SessionLocator_1.SessionLocator.LocalCurrencyId && !this.IsExchangeRateFixed) {
                    isFieldEnabled = true;
                }
            }
        }
        this.IsRateEnabled = isFieldEnabled;
        this.UIProperties.SetEnabled("ForiegnExchangeRate", this.ObjectTableName, isFieldEnabled);
    };
    ARInvoiceLineItem.prototype.RefreshLine = function () {
        this.ReadIsMatched();
        this.ReadCellBackground();
    };
    ARInvoiceLineItem.prototype.ReadIsMatched = function () {
        var isMatched = false;
        if (this.fatherComponent.PrepaidCollectId == "B") {
            isMatched = true;
        }
        else if (this.fatherComponent.PrepaidCollectId == this.PrepaidCollectId) {
            isMatched = true;
        }
        this.IsMatched = isMatched;
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
                this.SetCurrencyRateData();
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceLineItem.prototype.SetCurrencyRateData = function () {
        var _this = this;
        var myRate = null;
        var myRateDate = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ForiegnCurrencyId)) {
            if (this.ForiegnCurrencyId == SessionLocator_1.SessionLocator.TenantPM.CurrencyId) {
                myRate = 1;
            }
            else {
                var lastRate = this.fatherComponent.LastRatesList.filter(function (d) { return d.ForeignCurrencyId == _this.ForiegnCurrencyId; })[0];
                if (lastRate != null) {
                    myRate = lastRate.Rate;
                    myRateDate = lastRate.ValueDate;
                }
            }
        }
        this.ForiegnExchangeRate = myRate;
        this.ExchangeRateDate = myRateDate;
    };
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
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.ChargesTypeId != newValue) {
                this.EntityPM.ChargesTypeId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.Description = null;
                    this.LocalDescription = null;
                    this.VatTypeId = null;
                }
                else {
                    this.fatherComponent.myChargesTypeListService.getSingleFromCache(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.chargesTypeList = myResponse.Result;
                            if (_this.chargesTypeList != null) {
                                _this.Description = _this.chargesTypeList.EnglishName;
                                _this.LocalDescription = _this.chargesTypeList.LocalName;
                                _this.VatTypeId = _this.chargesTypeList.VatTypeId;
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
            this.ReadVatTypeData();
            this.fatherComponent.ComputeTotals();
        }
        else {
            this.fatherComponent.myVatTypeListService.getSingleFromCache(this.VatTypeId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        _this.VatTypeName = list.EnglishName;
                        _this.VatPercentage = _this.fatherComponent.GetVatTypePercentage(_this.VatTypeId);
                        _this.ReadVatTypeData();
                        _this.fatherComponent.ComputeTotals();
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
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceLineItem.prototype.ReadVatTypeData = function () {
        var myValue = null;
        var myColor = "#282E30";
        var isUpdateVisible = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.VatTypeId)) {
            if (this.VatPercentage != null) {
                myValue = this.VatTypeName + " (" + this.VatPercentage + "%)";
                myColor = "#282E30";
                isUpdateVisible = false;
            }
            else {
                myValue = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.S.Details.NoVat");
                myColor = "#E53030";
                isUpdateVisible = true;
            }
        }
        this.VatTypeCell = myValue;
        this.VatTypeCellColor = myColor;
        this.VatTypeUpdateIsVisible = isUpdateVisible;
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
        set: function (newValue) {
            if (this.EntityPM.ForiegnExchangeRate != newValue) {
                this.EntityPM.ForiegnExchangeRate = Tools_1.AppTool.Round(newValue, 5);
                if (this.ForiegnCurrencyId == this.fatherComponent.ProfitCurrencyId) {
                    if (this.fatherComponent.ProfitCurrencyExchangeRate != newValue) {
                        this.fatherComponent.ProfitCurrencyExchangeRate = newValue;
                    }
                }
                if (this.ForiegnCurrencyId == this.fatherComponent.InvoiceCurrencyId) {
                    this.fatherComponent.UpdateRateFromLine(this);
                    this.ExchangeRateDate = this.fatherComponent.GetCurrencyRateDate(this.ForiegnCurrencyId);
                }
                this.CalculateLocalCurrencyAmount();
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
        var profitCurrencyAmount = 0;
        var invoiceCurrencyAmount = 0;
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
//# sourceMappingURL=ARInvoiceDetailsTabGeneral.js.map