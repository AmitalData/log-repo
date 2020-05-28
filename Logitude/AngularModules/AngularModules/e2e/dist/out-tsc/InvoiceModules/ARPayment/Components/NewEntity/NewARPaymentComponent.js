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
var GLAccountListService_1 = require("./../../../../Accounting/Services/StandardLists/GLAccountListService");
var core_1 = require("@angular/core");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ARPaymentPM_1 = require("../../../../Invoice/EntityPMs/ARPaymentPM");
var ARInvoicePM_1 = require("../../../../Invoice/EntityPMs/ARInvoicePM");
var ARPaymentInvoicePM_1 = require("../../../../Invoice/EntityPMs/ARPaymentInvoicePM");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var CurrencyRatesService_1 = require("../../../../Common/Services/CurrencyRatesService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var CardList_1 = require("../../../../Common/EntityLists/CardList");
var AddressListService_1 = require("../../../../Common/Services/StandardLists/AddressListService");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var InvoiceDomainService_1 = require("../../../../Invoice/Services/InvoiceDomainService");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var AccountingPaymentMethodListService_1 = require("../../../../Invoice/Services/StandardLists/AccountingPaymentMethodListService");
var GLAccountPMService_1 = require("../../../../Accounting/Services/StandardPMs/GLAccountPMService");
var NewARPaymentComponent = /** @class */ (function (_super) {
    __extends(NewARPaymentComponent, _super);
    function NewARPaymentComponent(_entityResourceService) {
        var _this = _super.call(this) || this;
        _this._entityResourceService = _entityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "ARPayment";
        _this.newARPaymentPM = new ARPaymentPM_1.ARPaymentPM();
        _this.invoicePm = new ARInvoicePM_1.ARInvoicePM();
        _this.LastRatesList = [];
        _this.AllMethods = [];
        _this.IsLoadCurrencyList = false;
        _this.customerId = null;
        _this.DisplaySATSettings = false;
        _this.EnableNegativeOffsetARPayments = false;
        _this.IsEditExchangeRateVisible = false;
        _this.IsCreatedFromInvoiceSide = false;
        _this.isRTL = false;
        _this._glaService = new GLAccountListService_1.GLAccountListService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.CurrencyList = [];
        _this.AddressList = [];
        _this.CardList = [];
        _this.RateIsEnabled = false;
        _this.IsVisible = false;
        _this.TipoCadenaPagoList = [];
        _this.selectedTipoCadenaPago = null;
        _this.TodayDate = new Date();
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.TodayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        _this.EnableNegativeOffsetARPayments = ObjectsLocator_1.ObjectsLocator.AccountingSettingPM.EnableNegativeOffsetARPayments;
        if (_this.invoicePm == null) {
            _this.invoicePm = new ARInvoicePM_1.ARInvoicePM();
        }
        _this.accountingActivated = SessionLocator_1.SessionLocator.TenantPM.AccountingActivated;
        if (SessionLocator_1.SessionLocator.TenantPM.AccountingActivated)
            _this.invoicePm.IsFullAccounting = true;
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
            _this.DisplaySATSettings = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            _this.IsEditExchangeRateVisible = true;
        }
        return _this;
    }
    Object.defineProperty(NewARPaymentComponent.prototype, "IsNegativeAmountEnabled", {
        get: function () { return this.EnableNegativeOffsetARPayments == true && this.AccountingPaymentMethodCode == "FS" ? true : false; },
        enumerable: true,
        configurable: true
    });
    NewARPaymentComponent.prototype.ngOnInit = function () {
        this.Initialize();
    };
    NewARPaymentComponent.prototype.SetWindowArgs = function (args) {
        if (args) {
            this.invoicePm = args['ARInvoice'];
            this.customerId = args['CustomerId'];
        }
        if (this.invoicePm != null) {
            this.IsCreatedFromInvoiceSide = true;
            this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaymentCurrencyId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaymentCurrencyExchangeRate", this.ObjectTableName, false);
        }
    };
    NewARPaymentComponent.prototype.LoadCurrencyListMethod = function () {
        var _this = this;
        var myService = new CurrencyListService_1.CurrencyListService();
        myService.getAll().subscribe(function (myResult) {
            if (myResult) {
                _this.CurrencyList = myResult.Result;
                _this.IsLoadCurrencyList = true;
                _this.Initialize();
            }
        });
    };
    NewARPaymentComponent.prototype.LoadAddressListMethod = function () {
        var _this = this;
        var myService = new AddressListService_1.AddressListService();
        myService.getAll().subscribe(function (myResult) {
            if (myResult) {
                _this.AddressList = myResult.Result;
            }
        });
    };
    NewARPaymentComponent.prototype.LoadCardListMethod = function () {
        var _this = this;
        var myService = new CardListService_1.CardListService();
        myService.getAll().subscribe(function (myResult) {
            if (myResult) {
                _this.CardList = myResult.Result;
            }
        });
    };
    NewARPaymentComponent.prototype.SetUIProperties = function () {
        var isRateEnabled = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARPayment", "ARPaymentEditExchangeRate")) {
            if (this.PaymentCurrencyId) {
                if (this.PaymentCurrencyId != this.TenantPM.CurrencyId) {
                    if (this.IsCreatedFromInvoiceSide == false) {
                        isRateEnabled = true;
                    }
                }
            }
        }
        this.RateIsEnabled = isRateEnabled;
        this.UIProperties.SetEnabled("PaymentCurrencyExchangeRate", this.ObjectTableName, isRateEnabled);
        this.SetUIProperties_Payment();
    };
    NewARPaymentComponent.prototype.LoadData = function () {
        var _this = this;
        if (this.invoicePm != null && Tools_1.AppTool.IsNullOrEmpty(this.invoicePm.Id)) {
            var loadingDate = this.newARPaymentPM.RegisterDate;
            if (loadingDate == null) {
                loadingDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            }
            var myService = new CurrencyRatesService_1.CurrencyRatesService();
            myService.GetCurrenciesExchangeRateByValueDate(SessionLocator_1.SessionLocator.TenantPM.CurrencyId, loadingDate).subscribe(function (resp) {
                if (resp != null) {
                    if (!resp.HasError) {
                        _this.LastRatesList = resp.Result;
                        _this.SetCurrencyRateData();
                    }
                }
            });
        }
        var myService1 = new AccountingPaymentMethodListService_1.AccountingPaymentMethodListService();
        myService1.getAll().subscribe(function (response) {
            if (response != null) {
                _this.AllMethods = response.Result;
            }
        });
    };
    NewARPaymentComponent.prototype.Initialize = function () {
        this.FillTipoCadenaPagoList();
        this.CreateARPayment();
        if (this.IsCreatedFromInvoiceSide) {
            this.newARPaymentPM.BillToId = this.invoicePm.BillToId;
            this.newARPaymentPM.BillToName = this.invoicePm.BillToName;
            this.newARPaymentPM.BillToAddressId = this.invoicePm.BillToAddressId;
            this.newARPaymentPM.BillToPartnerTypeId = this.invoicePm.BillToPartnerTypeId;
            this.newARPaymentPM.PaymentCurrencyId = this.invoicePm.InvoiceCurrencyId;
            this.newARPaymentPM.PaymentCurrencyCode = this.invoicePm.InvoiceCurrencyCode;
            this.newARPaymentPM.ExchangeRateDate = this.invoicePm.ExchangeRateDate;
            this.newARPaymentPM.PaymentCurrencyExchangeRate = this.invoicePm.InvoiceCurrencyExchangeRate;
            if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.invoicePm.MetodoPagoCode)) {
                    this.MetodoPagoCode = this.invoicePm.MetodoPagoCode;
                    this.UIProperties.SetEnabled("MetodoPagoCode", this.ObjectTableName, false);
                }
            }
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.customerId)) {
                this.BillToId = this.customerId;
            }
            this.PaymentCurrencyId = SessionLocator_1.SessionLocator.TenantPM.CurrencyId;
            this.newARPaymentPM.PaymentCurrencyExchangeRate = 1;
        }
        if (SessionLocator_1.SessionLocator.TenantPM.AccountingActivated)
            this.newARPaymentPM.IsFullAccounting = true;
        this.LoadData();
        this.SetUIProperties();
        this.IsVisible = true;
    };
    NewARPaymentComponent.prototype.FillTipoCadenaPagoList = function () {
        this.TipoCadenaPagoList.push({ Code: null, Name: null });
        this.TipoCadenaPagoList.push({ Code: "01", Name: "SPEI (Electronic Payment System between Banks)" });
    };
    Object.defineProperty(NewARPaymentComponent.prototype, "SelectedTipoCadenaPago", {
        get: function () {
            var tipoCadenaPago = null;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.TipoCadenaPago)) {
                tipoCadenaPago = this.TipoCadenaPago.toUpperCase();
            }
            switch (tipoCadenaPago) {
                case "01":
                    {
                        this.selectedTipoCadenaPago = this.TipoCadenaPagoList.filter(function (d) { return d.Code == tipoCadenaPago; })[0];
                        break;
                    }
                default:
                    {
                        this.selectedTipoCadenaPago = this.TipoCadenaPagoList.filter(function (d) { return d.Code == null; })[0];
                        break;
                    }
            }
            return this.selectedTipoCadenaPago;
        },
        set: function (newValue) {
            if (this.selectedTipoCadenaPago != newValue) {
                this.selectedTipoCadenaPago = newValue;
                if (newValue == null) {
                    this.TipoCadenaPago = null;
                }
                else {
                    this.TipoCadenaPago = newValue.Code;
                }
            }
            this.ValidateTipoCadenaPagoFields();
        },
        enumerable: true,
        configurable: true
    });
    NewARPaymentComponent.prototype.ValidateTipoCadenaPagoFields = function () {
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.TipoCadenaPago) && this.TipoCadenaPago == "01" && this.SATPaymentMethodCode == "03") {
                if (Tools_1.AppTool.IsNullOrEmpty(this.CertPago))
                    this.UIProperties.SetRequired("CertPago", "ARPayment", true);
                else
                    this.UIProperties.SetRequired("CertPago", "ARPayment", false);
                if (Tools_1.AppTool.IsNullOrEmpty(this.CadPago))
                    this.UIProperties.SetRequired("CadPago", "ARPayment", true);
                else
                    this.UIProperties.SetRequired("CadPago", "ARPayment", false);
                if (Tools_1.AppTool.IsNullOrEmpty(this.SelloPago))
                    this.UIProperties.SetRequired("SelloPago", "ARPayment", true);
                else
                    this.UIProperties.SetRequired("SelloPago", "ARPayment", false);
            }
            else {
                this.UIProperties.SetRequired("CertPago", "ARPayment", false);
                this.UIProperties.SetRequired("CadPago", "ARPayment", false);
                this.UIProperties.SetRequired("SelloPago", "ARPayment", false);
            }
        }
    };
    Object.defineProperty(NewARPaymentComponent.prototype, "TipoCadenaPago", {
        get: function () {
            if (this.newARPaymentPM != null) {
                return this.newARPaymentPM.TipoCadenaPago;
            }
            else
                return null;
        },
        set: function (newValue) {
            if (this.newARPaymentPM.TipoCadenaPago != newValue) {
                this.newARPaymentPM.TipoCadenaPago = newValue;
                this.ValidateTipoCadenaPagoFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "CadPago", {
        get: function () {
            if (this.newARPaymentPM != null) {
                return this.newARPaymentPM.CadPago;
            }
            else
                return null;
        },
        set: function (newValue) {
            if (this.newARPaymentPM.CadPago != newValue) {
                this.newARPaymentPM.CadPago = newValue;
                this.ValidateTipoCadenaPagoFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "CertPago", {
        get: function () {
            if (this.newARPaymentPM != null) {
                return this.newARPaymentPM.CertPago;
            }
            else
                return null;
        },
        set: function (newValue) {
            if (this.newARPaymentPM.CertPago != newValue) {
                this.newARPaymentPM.CertPago = newValue;
                this.ValidateTipoCadenaPagoFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "SelloPago", {
        get: function () {
            if (this.newARPaymentPM != null) {
                return this.newARPaymentPM.SelloPago;
            }
            else
                return null;
        },
        set: function (newValue) {
            if (this.newARPaymentPM.SelloPago != newValue) {
                this.newARPaymentPM.SelloPago = newValue;
                this.ValidateTipoCadenaPagoFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewARPaymentComponent.prototype.SetCurrencyRateData = function () {
        var _this = this;
        if (this.invoicePm != null && Tools_1.AppTool.IsNullOrEmpty(this.invoicePm.Id)) {
            var rate = null;
            var rateDate = null;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.PaymentCurrencyId)) {
                if (this.PaymentCurrencyId == this.TenantPM.CurrencyId) {
                    rate = 1;
                }
                else {
                    if (this.LastRatesList != null) {
                        var lastRate = this.LastRatesList.filter(function (d) { return d.ForeignCurrencyId == _this.PaymentCurrencyId; })[0];
                        if (lastRate != null) {
                            rate = lastRate.Rate;
                            rateDate = lastRate.ValueDate;
                        }
                    }
                }
            }
            this.PaymentCurrencyExchangeRate = rate;
            this.ExchangeRateDate = rateDate;
        }
    };
    NewARPaymentComponent.prototype.CreateARPayment = function () {
        this.newARPaymentPM = new ARPaymentPM_1.ARPaymentPM();
        this.newARPaymentPM.Tenant = this.TenantPM.Id;
        this.newARPaymentPM.StatusCode = "DR";
        this.newARPaymentPM.StatusName = "Draft";
        this.newARPaymentPM.SATTransferStatusCode = "NT";
        this.newARPaymentPM.SATTransferStatusName = "Not Transfered";
        this.newARPaymentPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.newARPaymentPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.newARPaymentPM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        this.newARPaymentPM.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        this.newARPaymentPM.LocalCurrencyId = SessionLocator_1.SessionLocator.TenantPM.CurrencyId;
        this.newARPaymentPM.RegisterDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        if (!SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare) {
            this.newARPaymentPM.BranchId = SessionLocator_1.SessionLocator.LoggedUserPM.BranchId;
        }
    };
    Object.defineProperty(NewARPaymentComponent.prototype, "RegisterDate", {
        get: function () { return this.newARPaymentPM.RegisterDate; },
        set: function (newValue) {
            if (this.newARPaymentPM.RegisterDate != newValue) {
                this.newARPaymentPM.RegisterDate = newValue;
                this.LoadData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "SelectableDateStart", {
        get: function () { return this.TodayDate.setFullYear(this.TodayDate.getFullYear() - 100); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "SelectableDateEnd", {
        get: function () { return this.TodayDate.setFullYear(this.TodayDate.getFullYear() + 100); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "SelectablePaymentDateEnd", {
        get: function () { return this.TodayDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "AccountingPaymentMethodId", {
        get: function () { return this.newARPaymentPM.AccountingPaymentMethodId; },
        set: function (newValue) {
            if (this.newARPaymentPM.AccountingPaymentMethodId != newValue) {
                this.newARPaymentPM.AccountingPaymentMethodId = newValue;
                this.RefreshPaymentMethodFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "AccountingPaymentMethodCode", {
        get: function () { return this.newARPaymentPM.AccountingPaymentMethodCode; },
        set: function (newValue) {
            if (this.newARPaymentPM.AccountingPaymentMethodCode != newValue) {
                this.newARPaymentPM.AccountingPaymentMethodCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "SATPaymentMethodCode", {
        get: function () {
            if (this.newARPaymentPM == null) {
                return null;
            }
            return this.newARPaymentPM.SATPaymentMethodCode;
        },
        set: function (value) {
            if (this.newARPaymentPM != null) {
                if (this.newARPaymentPM.SATPaymentMethodCode != value) {
                    this.newARPaymentPM.SATPaymentMethodCode = value;
                    this.SetUIProperties_Payment();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "DebitAccountId", {
        get: function () { return this.newARPaymentPM.DebitAccountId; },
        set: function (newValue) {
            if (this.newARPaymentPM.DebitAccountId != newValue) {
                this.newARPaymentPM.DebitAccountId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "DebitAccountDependencyProperty1", {
        get: function () { return "AR,BN"; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "BillToId", {
        // BillTo Properties
        get: function () { return this.newARPaymentPM.BillToId; },
        set: function (newValue) {
            if (this.IsCreatedFromInvoiceSide == false) {
                if (this.newARPaymentPM.BillToId != newValue) {
                    this.newARPaymentPM.BillToId = newValue;
                    this.GetCardProperties();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "BillToName", {
        get: function () { return this.newARPaymentPM.BillToName; },
        set: function (newValue) {
            if (this.newARPaymentPM.BillToName != newValue) {
                this.newARPaymentPM.BillToName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "MetodoPagoCode", {
        get: function () { return this.newARPaymentPM.MetodoPagoCode; },
        set: function (newValue) {
            if (this.newARPaymentPM) {
                if (this.newARPaymentPM.MetodoPagoCode != newValue) {
                    this.newARPaymentPM.MetodoPagoCode = newValue;
                    this.SetUIProperties_Payment();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    NewARPaymentComponent.prototype.GetCardProperties = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.newARPaymentPM.BillToId)) {
            this.FillDataFromCardList(new CardList_1.CardList());
        }
        else {
            var myService = new CardListService_1.CardListService();
            myService.getSingle(this.newARPaymentPM.BillToId).subscribe(function (resp) {
                if (resp != null) {
                    if (!resp.HasError) {
                        var cardList = resp.Result;
                        if (cardList != null) {
                            _this.billtoCard = cardList;
                            _this.FillDataFromCardList(cardList);
                        }
                    }
                }
            });
        }
    };
    NewARPaymentComponent.prototype.FillDataFromCardList = function (list) {
        var _this = this;
        if (list == null) {
            this.BillToAddressId = null;
            this.BillToName = null;
            this.AccountingPaymentMethodId = null;
            this.AccountingPaymentMethodCode = null;
            this.SATPaymentMethodCode = null;
            this.PaymentCurrencyId = SessionLocator_1.SessionLocator.TenantPM.CurrencyId;
            this.newARPaymentPM.BillToPartnerTypeId = null;
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(list.InvoiceCurrencyId)) {
                this.PaymentCurrencyId = list.InvoiceCurrencyId;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(list.SATPaymentMethodCode)) {
                // this.ARPaymentMethodCode = list.SATPaymentMethodCode;
            }
            this.newARPaymentPM.BillToPartnerTypeId = list.PartnerTypeId;
            this.BillToName = list.EnglishName;
            this.LoadAddress();
            if (SessionLocator_1.SessionLocator.TenantPM.AccountingActivated == true) {
                if (!Tools_1.AppTool.IsNullOrEmpty(list.GLAccountId)) {
                    var myGLAccountPMService = new GLAccountPMService_1.GLAccountPMService();
                    myGLAccountPMService.get(list.GLAccountId).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var glaccount = myResponse.Result;
                            if (glaccount != null && !glaccount.IsMultiCurrency) {
                                _this.PaymentCurrencyId = glaccount.CurrencyId;
                            }
                        }
                    });
                }
            }
            //if (!AppTool.IsNullOrEmpty(list.SATPaymentMethodCode)) {
            //    this.SATPaymentMethodCode = list.SATPaymentMethodCode;
            //}
        }
    };
    NewARPaymentComponent.prototype.LoadAddress = function () {
        var _this = this;
        var myService = new PartnersDomainService_1.PartnersDomainService();
        myService.GetAddressByCardAndType(this.newARPaymentPM.BillToId, "B").subscribe(function (resp) {
            var billingAddress = resp;
            if (billingAddress != null) {
                var item = billingAddress;
                _this.BillToAddressId = item.Id;
            }
            else {
                _this.GetBillingAddress();
            }
        });
    };
    NewARPaymentComponent.prototype.GetBillingAddress = function () {
        var _this = this;
        var myService = new PartnersDomainService_1.PartnersDomainService();
        myService.GetBillingAddressListByCardId(this.newARPaymentPM.BillToId).subscribe(function (resp) {
            var billingAddress = resp;
            if (billingAddress != null) {
                _this.BillToAddressId = billingAddress.Id;
            }
            else {
                var myService = new PartnersDomainService_1.PartnersDomainService();
                myService.GetAddressByCardAndType(_this.newARPaymentPM.BillToId, "M").subscribe(function (resp) {
                    if (resp != null) {
                        var mainAddress = resp;
                        if (mainAddress != null) {
                            var item = mainAddress;
                            _this.BillToAddressId = item.Id;
                        }
                        else {
                            _this.GetMainAddressListByCardId();
                        }
                    }
                });
            }
        });
    };
    NewARPaymentComponent.prototype.GetMainAddressListByCardId = function () {
        var _this = this;
        var myService = new PartnersDomainService_1.PartnersDomainService();
        myService.GetMainAddressListByCardId(this.newARPaymentPM.BillToId).subscribe(function (resp) {
            if (resp != null) {
                var mainAddress = resp;
                if (mainAddress != null) {
                    _this.BillToAddressId = mainAddress.Id;
                }
                else {
                    _this.BillToAddressId = null;
                }
            }
        });
    };
    Object.defineProperty(NewARPaymentComponent.prototype, "BillToAddressId", {
        get: function () { return this.newARPaymentPM.BillToAddressId; },
        set: function (newValue) {
            if (this.IsCreatedFromInvoiceSide == false) {
                if (this.newARPaymentPM.BillToAddressId != newValue) {
                    this.newARPaymentPM.BillToAddressId = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "PaymentCurrencyIsEnabled", {
        // Currency Properties
        get: function () {
            if (this.invoicePm != null) {
                return Tools_1.AppTool.IsNullOrEmpty(this.invoicePm.Id);
            }
            else
                return false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "PaymentCurrencyId", {
        get: function () { return this.newARPaymentPM.PaymentCurrencyId; },
        set: function (newValue) {
            if (this.IsCreatedFromInvoiceSide == false) {
                if (this.newARPaymentPM.PaymentCurrencyId != newValue) {
                    this.newARPaymentPM.PaymentCurrencyId = newValue;
                    this.SetUIProperties();
                    this.SetCurrencyCode();
                    this.SetCurrencyRateData();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "PaymentCurrencyExchangeRate", {
        get: function () { return this.newARPaymentPM.PaymentCurrencyExchangeRate; },
        set: function (newValue) {
            if (this.IsCreatedFromInvoiceSide == false) {
                if (this.newARPaymentPM.PaymentCurrencyExchangeRate != newValue) {
                    this.newARPaymentPM.PaymentCurrencyExchangeRate = Tools_1.AppTool.Round(newValue, 5);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "ExchangeRateDate", {
        get: function () { return this.newARPaymentPM.ExchangeRateDate; },
        set: function (newValue) {
            if (this.newARPaymentPM.ExchangeRateDate != newValue) {
                this.newARPaymentPM.ExchangeRateDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "RelativeRateDate", {
        get: function () { return Tools_1.DateTool.GetRelativeRateDate(this.newARPaymentPM.RegisterDate, this.ExchangeRateDate, "old"); },
        enumerable: true,
        configurable: true
    });
    NewARPaymentComponent.prototype.SetCurrencyCode = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.PaymentCurrencyId)) {
            var myService = new CurrencyListService_1.CurrencyListService();
            myService.getSingleFromCache(this.PaymentCurrencyId).subscribe(function (response) {
                if (!response.HasError) {
                    if (response != null) {
                        var currency = response.Result;
                        if (currency != null) {
                            _this.newARPaymentPM.PaymentCurrencyCode = currency.Code;
                        }
                    }
                }
            });
        }
        else {
            var myService = new CurrencyListService_1.CurrencyListService();
            myService.getSingle(this.PaymentCurrencyId).subscribe(function (response) {
                if (!response.HasError) {
                    if (response != null) {
                        var list = response.Result;
                        if (list != null && list.length > 0) {
                            _this.newARPaymentPM.PaymentCurrencyCode = list.Code;
                        }
                    }
                }
            });
        }
    };
    // Payment Line Properties
    NewARPaymentComponent.prototype.RefreshPaymentMethodFields = function () {
        var _this = this;
        this.newARPaymentPM.Bank = null;
        this.newARPaymentPM.BankBranch = null;
        this.newARPaymentPM.Account = null;
        this.newARPaymentPM.ChequeOrPaymentRef = null;
        this.newARPaymentPM.ValueDate = null;
        var lists = this.AllMethods.filter(function (d) { return d.Id == _this.AccountingPaymentMethodId; });
        if (lists) {
            var list = lists[0];
            if (list) {
                this.AccountingPaymentMethodCode = list.Code;
            }
        }
        if (this.AccountingPaymentMethodCode == "CA" || this.AccountingPaymentMethodCode == "FS") {
            this.newARPaymentPM.ValueDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
    };
    Object.defineProperty(NewARPaymentComponent.prototype, "AmountInPaymentCurrency", {
        get: function () { return this.newARPaymentPM.AmountInPaymentCurrency; },
        set: function (newValue) {
            if (this.newARPaymentPM.AmountInPaymentCurrency != newValue) {
                this.newARPaymentPM.AmountInPaymentCurrency = Tools_1.AppTool.Round(newValue, 2);
                this.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "AmountInLocalCurrency", {
        get: function () { return this.newARPaymentPM.AmountInLocalCurrency; },
        set: function (newValue) {
            if (this.newARPaymentPM.AmountInLocalCurrency != newValue) {
                this.newARPaymentPM.AmountInLocalCurrency = Tools_1.AppTool.Round(newValue, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "OpenAmount", {
        get: function () { return this.newARPaymentPM.OpenAmount; },
        set: function (newValue) {
            if (this.newARPaymentPM.OpenAmount != newValue) {
                this.newARPaymentPM.OpenAmount = Tools_1.AppTool.Round(newValue, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARPaymentComponent.prototype, "BankAccountLiteId", {
        get: function () { return this.newARPaymentPM.BankAccountLiteId; },
        set: function (newValue) {
            if (this.newARPaymentPM.BankAccountLiteId != newValue) {
                this.newARPaymentPM.BankAccountLiteId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewARPaymentComponent.prototype.ComputeTotals = function () {
        this.OpenAmount = this.AmountInPaymentCurrency;
        this.AmountInLocalCurrency = this.PaymentCurrencyExchangeRate == null ? 0 : this.AmountInPaymentCurrency * this.PaymentCurrencyExchangeRate;
    };
    //Commands
    NewARPaymentComponent.prototype.UpdateCurrencyRateClicked = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("RatesTable", 0).subscribe(function (response) {
            var loadingDate = _this.newARPaymentPM.RegisterDate;
            if (loadingDate == null) {
                loadingDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            }
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 350;
            logWindow.Title = "Update Currency Rate";
            logWindow.WindowArgs = { CurrencyId: _this.newARPaymentPM.PaymentCurrencyId, CurrencyCode: _this.newARPaymentPM.PaymentCurrencyCode, Rate: _this.newARPaymentPM.PaymentCurrencyExchangeRate, Date: loadingDate };
            logWindow.ComponentLoaded.subscribe(function (comp) {
                logWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.PaymentCurrencyExchangeRate = comp.Rate;
                        _this.ExchangeRateDate = comp.RateDate;
                    }
                });
            });
            logWindow.Show('./CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent');
        });
    };
    NewARPaymentComponent.prototype.SetUIProperties_Payment = function () {
        this.UIProperties.SetRequired("SATPaymentMethodCode", this.ObjectTableName, false);
        this.UIProperties.SetRequired("MetodoPagoCode", this.ObjectTableName, false);
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.SATPaymentMethodCode)) {
                this.UIProperties.SetRequired("SATPaymentMethodCode", this.ObjectTableName, true);
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.MetodoPagoCode)) {
                this.UIProperties.SetRequired("MetodoPagoCode", this.ObjectTableName, true);
                this.ValidateTipoCadenaPagoFields();
            }
        }
    };
    NewARPaymentComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewARPaymentComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (Tools_1.AppTool.IsNullOrEmpty(this.newARPaymentPM.BillToId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.F.BillToId")));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.newARPaymentPM.BillToAddressId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.F.BillToAddressId")));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.newARPaymentPM.PaymentCurrencyId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.F.PaymentCurrencyCode")));
        }
        if (this.RegisterDate == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.F.RegisterDate")));
        }
        else if (Tools_1.DateTool.GetDateParts(this.RegisterDate).DateTicks > Tools_1.DateTool.GetCurrentDateAsUtc().valueOf()) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.CantSetFutureDatePayment"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.newARPaymentPM.AccountingPaymentMethodId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.F.ARPaymentMethodCode")));
        }
        if (this.newARPaymentPM.AmountInPaymentCurrency == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.F.AmountInPaymentCurrency")));
        }
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.SATPaymentMethodCode)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.F.SATPaymentMethodCode")));
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.MetodoPagoCode)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.F.MetodoPagoCode")));
            }
            if (this.SATPaymentMethodCode == "99") {
                errors.push("Forma Pago value can't be 'Por Definir'.Please choose another value.");
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.TipoCadenaPago) && this.TipoCadenaPago == "01" && this.SATPaymentMethodCode == "03") {
                if (Tools_1.AppTool.IsNullOrEmpty(this.CertPago))
                    errors.push(msg.replace("%FieldName", "Cert Pago"));
                if (Tools_1.AppTool.IsNullOrEmpty(this.CadPago))
                    errors.push(msg.replace("%FieldName", "Cad Pago"));
                if (Tools_1.AppTool.IsNullOrEmpty(this.SelloPago))
                    errors.push(msg.replace("%FieldName", "Sello Pago"));
            }
        }
        if (this.newARPaymentPM.AmountInPaymentCurrency == 0) {
            var isAllowed = false;
            if (this.newARPaymentPM.AccountingPaymentMethodCode != null) {
                if (this.newARPaymentPM.AccountingPaymentMethodCode.toUpperCase() == "FS") {
                    isAllowed = true;
                }
            }
            if (!isAllowed) {
                errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.CantSetZeroAmount"));
            }
        }
        if (this.newARPaymentPM.AmountInPaymentCurrency < 0) {
            if (!this.IsNegativeAmountEnabled) {
                errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.CantSetMinusAmount"));
            }
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            if (this.IsCreatedFromInvoiceSide) {
                if (this.BillToId != this.invoicePm.BillToId) {
                    errors.push("Bill to doesn't match the invoice bill to");
                }
                if (this.PaymentCurrencyId != this.invoicePm.InvoiceCurrencyId) {
                    errors.push("Payment currency doesn't match the invoice currency");
                }
            }
        }
        if (this.ValidationErrorsList.length == 0) {
            // Check Full Accounting
            if (SessionLocator_1.SessionLocator.TenantPM.AccountingActivated == true /*&& (this.newARPaymentPM.ARPaymentMethodCode == "CH" || this.newARPaymentPM.ARPaymentMethodCode == "CA")*/) {
                var invoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
                invoiceDomainService.ValidateARPaymentFullAccounting(this.newARPaymentPM.AccountingPaymentMethodCode, this.newARPaymentPM.PaymentCurrencyId, this.newARPaymentPM.BillToId, this.newARPaymentPM.AccountingPaymentMethodCode, this.newARPaymentPM.RegisterDate, this.newARPaymentPM.BankAccountId).subscribe(function (response) {
                    if (response != null) {
                        if (!response.HasError) {
                            //var validate = response.Result;
                            //if (validate == null) {
                            //    errors.push("There is no appropriate Cashbook for this payment, You have to create one");
                            //}
                            _this.CompleteSubmission(errors);
                        }
                        else {
                            _this.ValidationErrorsList = response.ErrorsArray;
                        }
                    }
                });
            }
            else {
                this.CompleteSubmission(errors);
            }
        }
    };
    NewARPaymentComponent.prototype.CompleteSubmission = function (errors) {
        var _this = this;
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.invoicePm != null && !Tools_1.AppTool.IsNullOrEmpty(this.invoicePm.Id)) {
                this.ConnectARInvoiceToPayment(this.newARPaymentPM, this.invoicePm);
            }
            if (SessionLocator_1.SessionLocator.TenantPM.AccountingActivated) {
                this.fetchGLAccount()
                    .then(function (res) {
                    _this.newARPaymentPM.GLAccountId = res.Id;
                    _this.newARPaymentPM.GLAccountRecoMethodCode = res.ReconcileMethodCode;
                    _this.RunEditWindow();
                }, function (err) {
                    _this.ValidationErrorsList = ['Somthing wrong! no gl account found for this bill to account'];
                    return;
                });
            }
            else {
                this.RunEditWindow();
            }
        }
    };
    NewARPaymentComponent.prototype.ConnectARInvoiceToPayment = function (entityPM, invoicePM) {
        var connectAmount = this.GetSmallestAmount(entityPM, invoicePM);
        var connectAmountLocal = connectAmount * invoicePM.InvoiceCurrencyExchangeRate;
        var record = new ARPaymentInvoicePM_1.ARPaymentInvoicePM(null);
        record.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
        record.ARInvoiceId = invoicePM.Id;
        record.ARPaymentId = entityPM.Id;
        record.ForeignCurrencyId = invoicePM.InvoiceCurrencyId;
        record.ForeignAmount = connectAmount == null ? 0 : connectAmount;
        record.LocalAmount = connectAmountLocal == null ? 0 : connectAmountLocal;
        record.PaymentAmount = record.ForeignAmount;
        record.ExchangeRate = invoicePM.InvoiceCurrencyExchangeRate;
        record.ARInvoiceMetodoPagoCode = invoicePM.MetodoPagoCode;
        entityPM.PaymentInvoices = [];
        entityPM.PaymentInvoices.push(record);
        if (entityPM.OpenAmount > connectAmount) {
            entityPM.OpenAmount = entityPM.OpenAmount - connectAmount;
        }
        else {
            entityPM.OpenAmount = 0;
        }
    };
    NewARPaymentComponent.prototype.GetSmallestAmount = function (paymentPM, invoicePM) {
        var invoiceAmount = invoicePM.AmountDue;
        var paymentAmount = paymentPM.OpenAmount == null ? 0 : paymentPM.OpenAmount;
        var smallestAmount = null;
        if (invoiceAmount <= paymentAmount) {
            smallestAmount = invoiceAmount;
        }
        else {
            smallestAmount = paymentAmount;
        }
        return smallestAmount;
    };
    NewARPaymentComponent.prototype.RunEditWindow = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.WindowClosed.subscribe(function (s) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: _this.newARPaymentPM.Id, EntityPM: _this.newARPaymentPM, ObjectTableName: 'ARPayment' });
                var isEditComponentSaved = false;
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                    if (isEditComponentSaved) {
                        _this.CurrentSession.FireEvent("NewARPaymentInvoiceTabCreated");
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
        });
        this.CurrentSession.CloseCurrentWindow();
    };
    NewARPaymentComponent.prototype.fetchGLAccount = function () {
        var _this = this;
        return new Promise(function (resolve, reject) {
            var _glaId = _this.billtoCard.GLAccountId;
            _this.CurrentSession.StartBusyIndicatorLoading();
            _this._glaService.getSingle(_glaId)
                .subscribe(function (response) {
                var res = response;
                if (!res.HasError) {
                    var glaccount = res.Result;
                    resolve(glaccount);
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    reject();
                    _this.ValidationErrorsList = res.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        });
    };
    NewARPaymentComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewARPaymentComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], NewARPaymentComponent);
    return NewARPaymentComponent;
}(BaseComponent_1.BaseComponent));
exports.NewARPaymentComponent = NewARPaymentComponent;
var TipoCadenaPagoClass = /** @class */ (function () {
    function TipoCadenaPagoClass() {
    }
    return TipoCadenaPagoClass;
}());
//# sourceMappingURL=NewARPaymentComponent.js.map