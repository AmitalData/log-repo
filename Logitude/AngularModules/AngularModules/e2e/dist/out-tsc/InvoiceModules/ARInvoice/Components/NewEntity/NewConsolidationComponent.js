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
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ARInvoicePMService_1 = require("../../../../Invoice/Services/StandardPMs/ARInvoicePMService");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var CurrencyRatesService_1 = require("../../../../Common/Services/CurrencyRatesService");
var Tools_2 = require("../../../../Invoice/Tools");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var PaymentTermListService_1 = require("../../../../Common/Services/StandardLists/PaymentTermListService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var InvoiceDomainService_1 = require("../../../../Invoice/Services/InvoiceDomainService");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var ServiceLocator_1 = require("../../../../Infrastructure/Locators/ServiceLocator");
var NewConsolidationComponent = /** @class */ (function (_super) {
    __extends(NewConsolidationComponent, _super);
    function NewConsolidationComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.ObjectTableName = "ARInvoice";
        _this.DataContext = _this;
        _this.InvoicePartners = [];
        _this.ValidationErrorsList = [];
        _this.IsResourcesReady = false;
        _this.DisplaySATSettings = false;
        _this.IsEditExchangeRateVisible = false;
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // SetUIProperties
        _this.RateIsEnabled = false;
        _this.PaymentTermDisplayInLOV = true;
        // BillTo
        _this.BillToDependencyValue1 = "CS";
        _this.BillToDependencyValue2 = false;
        _this.BillToDependencyValue1IsList = false;
        _this.SelectedPartnerType = null;
        _this.myRelativeRateDate = null;
        // Load Date 
        _this.LastRatesList = [];
        // CreditLimit
        _this.HasCreditLimitFeature = false;
        _this.HasCreditOverrideFeature = false;
        _this.IsCreditLimitActivated = false;
        _this.IsCreditLimitHasAction = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.InitializeServices();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            _this.IsEditExchangeRateVisible = true;
        }
        return _this;
    }
    NewConsolidationComponent.prototype.InitializeServices = function () {
        this.myCardListService = new CardListService_1.CardListService();
        this.myCurrencyListService = new CurrencyListService_1.CurrencyListService();
        this.myPaymentTermListService = new PaymentTermListService_1.PaymentTermListService();
        this.myInvoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        this.myEntityPMService = new ARInvoicePMService_1.ARInvoicePMService();
    };
    NewConsolidationComponent.prototype.SetWindowArgs = function (typeCode) {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            _this.EntityPM = _this.myEntityPMService.GetNewEntityPM();
            _this.EntityPM.ARInvoiceTypeCode = typeCode;
            _this.EntityPM.IsConsolidationInvoice = true;
            _this.IsResourcesReady = true;
            _this.InvoiceCurrencyId = SessionLocator_1.SessionLocator.TenantPM.CurrencyId;
            _this.PaymentTermId = SessionLocator_1.SessionLocator.TenantPM.PaymentTermId;
            _this.SetUIProperties();
            _this.BuildPartnersTypes();
            _this.LoadData();
            if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
                _this.DisplaySATSettings = true;
                _this.MetodoPagoCode = SessionLocator_1.SessionLocator.SATInterfaceSettings.MetodoPagoCode;
                if (Tools_1.AppTool.IsNullOrEmpty(_this.MetodoPagoCode)) {
                    _this.UIProperties.SetRequired("MetodoPagoCode", _this.ObjectTableName, true);
                }
            }
        });
    };
    NewConsolidationComponent.prototype.SetUIProperties = function () {
        this.SetUIProperties_BillTo();
        this.SetUIProperties_VatNumber();
        this.SetUIProperties_ExchangeRate();
        this.SetUIProperties_DueDate();
        this.SetUIProperties_Payment();
    };
    NewConsolidationComponent.prototype.SetUIProperties_BillTo = function () {
        var isBillToEnabled = false;
        var isBillToAddressEnabled = false;
        if (this.SelectedPartnerType) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.BillToPartnerTypeId)) {
                isBillToEnabled = true;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.BillToId)) {
            isBillToAddressEnabled = true;
        }
        this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, isBillToEnabled);
        this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, isBillToAddressEnabled);
    };
    NewConsolidationComponent.prototype.SetUIProperties_VatNumber = function () {
        var isFieldRequired = false;
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAR) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.VatNumber)) {
                isFieldRequired = true;
            }
            this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, isFieldRequired);
        }
    };
    NewConsolidationComponent.prototype.SetUIProperties_ExchangeRate = function () {
        var isFieldtEnabled = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "ARInvoiceEditExchangeRate")) {
            if (this.InvoiceCurrencyId) {
                if (this.InvoiceCurrencyId != SessionLocator_1.SessionLocator.TenantPM.CurrencyId) {
                    isFieldtEnabled = true;
                }
            }
        }
        this.RateIsEnabled = isFieldtEnabled;
        this.UIProperties.SetEnabled("InvoiceCurrencyExchangeRate", this.ObjectTableName, isFieldtEnabled);
    };
    NewConsolidationComponent.prototype.SetUIProperties_DueDate = function () {
        var AllowManuallyDueDate = false;
        if (SessionLocator_1.SessionLocator.AccountingSystemPM) {
            AllowManuallyDueDate = SessionLocator_1.SessionLocator.AccountingSystemPM.AllowManuallyDueDate;
        }
        this.UIProperties.SetEnabled("DueDate", this.ObjectTableName, AllowManuallyDueDate);
        if (AllowManuallyDueDate) {
            this.PaymentTermDisplayInLOV = null;
        }
    };
    NewConsolidationComponent.prototype.SetUIProperties_Payment = function () {
        this.UIProperties.SetRequired("SATPaymentMethodCode", this.ObjectTableName, false);
        this.UIProperties.SetRequired("MetodoPagoCode", this.ObjectTableName, false);
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.SATPaymentMethodCode)) {
                this.UIProperties.SetRequired("SATPaymentMethodCode", this.ObjectTableName, true);
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.MetodoPagoCode)) {
                this.UIProperties.SetRequired("MetodoPagoCode", this.ObjectTableName, true);
            }
        }
    };
    Object.defineProperty(NewConsolidationComponent.prototype, "MetodoPagoCode", {
        get: function () { return this.EntityPM.MetodoPagoCode; },
        set: function (newValue) {
            if (this.EntityPM) {
                if (this.EntityPM.MetodoPagoCode != newValue) {
                    this.EntityPM.MetodoPagoCode = newValue;
                    this.SetUIProperties_Payment();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    NewConsolidationComponent.prototype.BuildPartnersTypes = function () {
        this.InvoicePartners = Tools_2.InvoiceTool.GetARInvoicePartners(null);
        this.PartnersTypeSelectionMethod(this.InvoicePartners[0]);
    };
    NewConsolidationComponent.prototype.PartnersTypeSelectionMethod = function (selected) {
        if (this.SelectedPartnerType != selected) {
            this.SelectedPartnerType = selected;
            this.BillToId = null;
            this.BillToAddressId = null;
            this.BillToPartnerTypeId = null;
            if (selected) {
                this.BillToPartnerTypeId = selected.PartnerTypeId;
                this.BillToDependencyValue1 = selected.PartnerTypeId;
                this.BillToDependencyValue2 = selected.IsCustomer;
            }
            this.SetUIProperties();
        }
    };
    Object.defineProperty(NewConsolidationComponent.prototype, "BillToPartnerTypeId", {
        get: function () { return this.EntityPM.BillToPartnerTypeId; },
        set: function (newValue) {
            if (this.EntityPM.BillToPartnerTypeId != newValue) {
                this.EntityPM.BillToPartnerTypeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewConsolidationComponent.prototype, "BillToId", {
        get: function () { return this.EntityPM.BillToId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.BillToId != newValue) {
                this.EntityPM.BillToId = newValue;
                this.SetUIProperties_BillTo();
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.VatNumber = null;
                    this.BillToName = null;
                    this.BillToAddressId = null;
                    this.SATPaymentMethodCode = null;
                    this.InvoiceCurrencyId = SessionLocator_1.SessionLocator.TenantPM.CurrencyId;
                    this.PaymentTermId = SessionLocator_1.SessionLocator.TenantPM.PaymentTermId;
                    this.EntityPM.IsBillToAllowConsolidation = false;
                    this.EntityPM.BillToIsCreditLimitEnabled = false;
                    this.EntityPM.BillToCreditLimitAmount = null;
                    this.EntityPM.BillToCreditLimitOpenBalance = null;
                    this.EntityPM.BillToCreditLimitWarningPercentage = null;
                    this.EntityPM.BillToBlockNewInvoiceCreation = false;
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
                                if (!Tools_1.AppTool.IsNullOrEmpty(list.SATPaymentMethodCode)) {
                                    _this.SATPaymentMethodCode = list.SATPaymentMethodCode;
                                }
                                if (!Tools_1.AppTool.IsNullOrEmpty(list.MetodoPagoCode)) {
                                    _this.MetodoPagoCode = list.MetodoPagoCode;
                                }
                                else if (SessionLocator_1.SessionLocator.SATInterfaceSettings != null && !Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.SATInterfaceSettings.MetodoPagoCode)) {
                                    _this.MetodoPagoCode = SessionLocator_1.SessionLocator.SATInterfaceSettings.MetodoPagoCode;
                                }
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
                                else {
                                    _this.BillToAddressId = null;
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
    Object.defineProperty(NewConsolidationComponent.prototype, "BillToName", {
        get: function () { return this.EntityPM.BillToName; },
        set: function (newValue) {
            if (this.EntityPM.BillToName != newValue) {
                this.EntityPM.BillToName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewConsolidationComponent.prototype, "BillToAddressId", {
        get: function () { return this.EntityPM.BillToAddressId; },
        set: function (newValue) {
            if (this.EntityPM.BillToAddressId != newValue) {
                this.EntityPM.BillToAddressId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewConsolidationComponent.prototype, "InvoiceCurrencyId", {
        // Currency
        get: function () { return this.EntityPM.InvoiceCurrencyId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.InvoiceCurrencyId != newValue) {
                this.EntityPM.InvoiceCurrencyId = newValue;
                this.SetCurrencyRateData();
                this.SetUIProperties_ExchangeRate();
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.InvoiceCurrencyCode = null;
                }
                else {
                    this.myCurrencyListService.getSingleFromCache(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.InvoiceCurrencyCode = list.Code;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewConsolidationComponent.prototype, "InvoiceCurrencyCode", {
        get: function () { return this.EntityPM.InvoiceCurrencyCode; },
        set: function (value) {
            if (this.EntityPM.InvoiceCurrencyCode != value) {
                this.EntityPM.InvoiceCurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewConsolidationComponent.prototype, "InvoiceCurrencyExchangeRate", {
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
    Object.defineProperty(NewConsolidationComponent.prototype, "ExchangeRateDate", {
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
    Object.defineProperty(NewConsolidationComponent.prototype, "RelativeRateDate", {
        get: function () { return this.myRelativeRateDate; },
        set: function (value) {
            if (this.myRelativeRateDate != value) {
                this.myRelativeRateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewConsolidationComponent.prototype.ComputeRelativeRateDate = function () {
        this.RelativeRateDate = Tools_1.DateTool.GetRelativeRateDate(this.InvoiceDate, this.ExchangeRateDate, "old");
    };
    Object.defineProperty(NewConsolidationComponent.prototype, "VatNumber", {
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
    Object.defineProperty(NewConsolidationComponent.prototype, "PaymentTermId", {
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
    Object.defineProperty(NewConsolidationComponent.prototype, "PaymentTermName", {
        get: function () { return this.EntityPM.PaymentTermName; },
        set: function (newValue) {
            if (this.EntityPM.PaymentTermName != newValue) {
                this.EntityPM.PaymentTermName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewConsolidationComponent.prototype, "InvoiceDate", {
        get: function () { return this.EntityPM.InvoiceDate; },
        set: function (newValue) {
            if (this.EntityPM.InvoiceDate != newValue) {
                this.EntityPM.InvoiceDate = newValue;
                Tools_2.InvoiceTool.ComputeARInvoiceDueDate(this.EntityPM);
                this.ComputeRelativeRateDate();
                this.LoadData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewConsolidationComponent.prototype, "DueDate", {
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
    Object.defineProperty(NewConsolidationComponent.prototype, "CustomerRef", {
        get: function () { return this.EntityPM.CustomerRef; },
        set: function (newValue) {
            if (this.EntityPM.CustomerRef != newValue) {
                this.EntityPM.CustomerRef = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewConsolidationComponent.prototype, "SATPaymentMethodCode", {
        get: function () { return this.EntityPM.SATPaymentMethodCode; },
        set: function (newValue) {
            if (this.EntityPM.SATPaymentMethodCode != newValue) {
                this.EntityPM.SATPaymentMethodCode = newValue;
                this.SetUIProperties_Payment();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewConsolidationComponent.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        if (this.myCurrencyRatesService == null) {
            this.myCurrencyRatesService = new CurrencyRatesService_1.CurrencyRatesService();
        }
        var loadingDate = this.EntityPM.InvoiceDate;
        if (loadingDate == null) {
            loadingDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
        this.myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator_1.SessionLocator.TenantPM.CurrencyId, loadingDate).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.LastRatesList = myResponse.Result;
                _this.SetCurrencyRateData();
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    NewConsolidationComponent.prototype.SetCurrencyRateData = function () {
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
    NewConsolidationComponent.prototype.GetCurrencyRate = function (currencyId) {
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
    NewConsolidationComponent.prototype.GetCurrencyRateDate = function (currencyId) {
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
    NewConsolidationComponent.prototype.UpdateCurrencyRateClicked = function () {
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
    //Commands 
    NewConsolidationComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewConsolidationComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (Tools_1.AppTool.IsNullOrEmpty(this.BillToPartnerTypeId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.F.PartnerType")));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.BillToId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.F.BillToId")));
        }
        //if (AppTool.IsNullOrEmpty(this.BillToAddressId)) {
        //    errors.push(msg.replace("%FieldName", "Address"));
        //}
        if (Tools_1.AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.F.InvoiceCurrencyId")));
        }
        if (this.InvoiceDate == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.F.InvoiceDate")));
        }
        else if (Tools_1.DateTool.GetDateParts(this.InvoiceDate).DateTicks > Tools_1.DateTool.GetCurrentDateAsUtc().valueOf()) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.CantIssueInvoiceWithFutureDate"));
        }
        if (this.DueDate == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.F.DueDate")));
        }
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAR) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.VatNumber)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.F.VatNumber")));
            }
        }
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.SATPaymentMethodCode)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.F.SATPaymentMethodCode")));
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.MetodoPagoCode)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.F.MetodoPagoCode")));
            }
            if (this.MetodoPagoCode == "PUE" && this.SATPaymentMethodCode == "99") {
                errors.push("Since the metodo pago was set as PUE, you can't select Por definir (99). Please choose another value for the forma Pago.");
            }
        }
        //if (errors.length == 0) {
        //    if (!this.EntityPM.IsBillToAllowConsolidation) {
        //        errors.push(TextCodeTranslator.Translate("ARInvoice.M.NewConsolidationInvoiceErrorMsg1"));
        //    }
        //}
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.UpdateCreditLimitFlags();
            if (SessionLocator_1.SessionLocator.TenantPM.AccountingActivated) {
                this.ValidateFullAccounting();
            }
            else if (this.HasCreditLimitFeature && this.IsCreditLimitActivated && this.IsCreditLimitHasAction && this.EntityPM.BillToIsCreditLimitEnabled) {
                this.ValidateCreditLimit();
            }
            else {
                this.OnEntityValid();
            }
        }
    };
    NewConsolidationComponent.prototype.ValidateFullAccounting = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Checking ...");
        this.myInvoiceDomainService.ValidateARInvoiceFullAccounting(this.EntityPM.InvoiceCurrencyId, this.EntityPM.BillToId, this.EntityPM.InvoiceDate).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    if (_this.HasCreditLimitFeature && _this.IsCreditLimitActivated && _this.IsCreditLimitHasAction) {
                        _this.ValidateCreditLimit();
                    }
                    else {
                        _this.OnEntityValid();
                    }
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            }
        });
    };
    NewConsolidationComponent.prototype.ValidateCreditLimit = function () {
        var _this = this;
        if (this.EntityPM.BillToBlockNewInvoiceCreation) {
            var errorText_Blocking = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.NewConsolidationInvoiceErrorMsg6") + ": " + this.EntityPM.BillToName;
            var errors = [];
            var warnings = [];
            if (ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.InvoiceCreationBlock) {
                errors.push(errorText_Blocking);
            }
            else if (ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.InvoiceCreationWarning) {
                warnings.push(errorText_Blocking);
            }
            if (errors.length > 0 || warnings.length > 0) {
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 450;
                logWindow.Height = 200;
                logWindow.Title = "Credit limit";
                logWindow.WindowArgs = { Errors: errors, Warnings: warnings };
                logWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.OnEntityValid();
                    }
                });
                logWindow.Show('./Invoice/Components/NewEntity/CreditLimitPopupComponent');
            }
            else {
                this.OnEntityValid();
            }
        }
        else {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.myInvoiceDomainService.GetCustomerCreditLimitActualAmount(this.BillToId).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    var errors = [];
                    var warnings = [];
                    _this.EntityPM.BillToCreditLimitActualAmount = myResponse.Result;
                    _this.EntityPM.BillToCreditLimitActualBalance = Tools_1.AppTool.AddAmounts(_this.EntityPM.BillToCreditLimitOpenBalance, _this.EntityPM.BillToCreditLimitActualAmount);
                    var LimitAmount = Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.BillToCreditLimitAmount) ? 0 : _this.EntityPM.BillToCreditLimitAmount;
                    var WarningPercentage = Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.BillToCreditLimitWarningPercentage) ? 0 : _this.EntityPM.BillToCreditLimitWarningPercentage;
                    var ActualBalance = Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.BillToCreditLimitActualBalance) ? 0 : _this.EntityPM.BillToCreditLimitActualBalance;
                    var isBillToHasLimitAmount = Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.BillToCreditLimitAmount) ? false : true;
                    var isBillToHasWarningPercentage = Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.BillToCreditLimitWarningPercentage) ? false : true;
                    if (isBillToHasLimitAmount && ActualBalance > LimitAmount) {
                        var LimitError = "";
                        var LimitWarning = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.NewConsolidationInvoiceErrorMsg2");
                        LimitError += TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.NewConsolidationInvoiceErrorMsg3") + " " + Tools_1.FormatTool.FormatNumber(LimitAmount) + " (" + _this.EntityPM.LocalCurrencyCode + ").";
                        LimitError += " ";
                        LimitError += TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.NewConsolidationInvoiceErrorMsg4") + " " + Tools_1.FormatTool.FormatNumber(ActualBalance) + " (" + _this.EntityPM.LocalCurrencyCode + ").";
                        if (ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.InvoiceCreationBlock) {
                            errors.push(LimitError);
                        }
                        else if (ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.InvoiceCreationWarning) {
                            warnings.push(LimitWarning);
                        }
                    }
                    else if (isBillToHasWarningPercentage && ActualBalance > (WarningPercentage * LimitAmount / 100)) {
                        if (ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.InvoiceCreationWarning) {
                            var RemainingLimit = Tools_1.FormatTool.FormatNumber(LimitAmount - ActualBalance);
                            var PercentageWarning = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.NewConsolidationInvoiceErrorMsg5") + " (" + RemainingLimit + ")";
                            warnings.push(PercentageWarning);
                        }
                    }
                    if (errors.length > 0 || warnings.length > 0) {
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Width = 450;
                        logWindow.Height = 200;
                        logWindow.Title = "Credit limit";
                        logWindow.WindowArgs = { Errors: errors, Warnings: warnings };
                        logWindow.WindowClosed.subscribe(function (s) {
                            if (s) {
                                _this.OnEntityValid();
                            }
                        });
                        logWindow.Show('./Invoice/Components/NewEntity/CreditLimitPopupComponent');
                    }
                    else {
                        _this.OnEntityValid();
                    }
                }
            });
        }
    };
    NewConsolidationComponent.prototype.OnEntityValid = function () {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.InitializeComponent();
    };
    NewConsolidationComponent.prototype.InitializeComponent = function () {
        var _this = this;
        this.myCurrencyListService.getSingleFromCache(this.EntityPM.ProfitCurrencyId).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var list = myResponse.Result;
                if (list != null) {
                    _this.EntityPM.ProfitCurrencyCode = list.Code;
                }
            }
        });
        this.EntityPM.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, "New " + this.ObjectTableName);
        this.CurrentSession.CloseCurrentWindowEmit("Ok");
    };
    NewConsolidationComponent.prototype.UpdateCreditLimitFlags = function () {
        this.HasCreditLimitFeature = FeatureLocator_1.FeatureLocator.HasFeaturePermession("CreditLimitSetting", "Module");
        this.HasCreditOverrideFeature = FeatureLocator_1.FeatureLocator.HasFeaturePermession("CreditLimitSetting", "Override");
        this.EntityPM.HasCreditLimitOverrideFeature = this.HasCreditOverrideFeature;
        if (this.HasCreditLimitFeature) {
            this.IsCreditLimitActivated = ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.IsCreditLimitEnabled;
            this.IsCreditLimitHasAction = (ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.InvoiceCreationBlock == true || ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.InvoiceCreationWarning == true) ? true : false;
        }
    };
    NewConsolidationComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewConsolidationComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], NewConsolidationComponent);
    return NewConsolidationComponent;
}(BaseComponent_1.BaseComponent));
exports.NewConsolidationComponent = NewConsolidationComponent;
//# sourceMappingURL=NewConsolidationComponent.js.map