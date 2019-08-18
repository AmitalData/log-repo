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
var APInvoicePMService_1 = require("../../../../Invoice/Services/StandardPMs/APInvoicePMService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var Tools_2 = require("../../../../Invoice/Tools");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var VatTypeListService_1 = require("../../../../Common/Services/StandardLists/VatTypeListService");
var CurrencyRatesService_1 = require("../../../../Common/Services/CurrencyRatesService");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var PaymentTermListService_1 = require("../../../../Common/Services/StandardLists/PaymentTermListService");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var ChargesTypeListService_1 = require("../../../../Common/Services/StandardLists/ChargesTypeListService");
var InvoiceDomainService_1 = require("../../../../Invoice/Services/InvoiceDomainService");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var GLAccountPMService_1 = require("../../../../Accounting/Services/StandardPMs/GLAccountPMService");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var NewGeneralAPInvoiceComponent = /** @class */ (function (_super) {
    __extends(NewGeneralAPInvoiceComponent, _super);
    function NewGeneralAPInvoiceComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.ObjectTableName = "APInvoice";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.ValidationWarningsList = [];
        _this.IsResourcesReady = false;
        _this.IsAccountingActivated = false;
        _this.IsEditExchangeRateVisible = false;
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.AllVatTypes = [];
        // SetUIProperties
        _this.PaymentTermDisplayInLOV = true;
        // Load Data 
        _this.LastRatesList = [];
        _this.VatTypePercentagesList = [];
        _this.myRelativeRateDate = null;
        // UpdateCurrencyRate
        _this.RateIsEnabled = false;
        _this.vatTypeId = null;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.IsAccountingActivated = SessionLocator_1.SessionLocator.TenantPM.AccountingActivated;
        _this.InitializeServices();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            _this.IsEditExchangeRateVisible = true;
        }
        return _this;
    }
    NewGeneralAPInvoiceComponent.prototype.InitializeServices = function () {
        var _this = this;
        this.myCardListService = new CardListService_1.CardListService();
        this.myPaymentTermListService = new PaymentTermListService_1.PaymentTermListService();
        this.myCurrencyListService = new CurrencyListService_1.CurrencyListService();
        this.myChargesTypeListService = new ChargesTypeListService_1.ChargesTypeListService();
        this.myVatTypeListService = new VatTypeListService_1.VatTypeListService();
        this.myInvoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        this.myEntityPMService = new APInvoicePMService_1.APInvoicePMService();
        this.myGLAccountPMService = new GLAccountPMService_1.GLAccountPMService();
        this.myVatTypeListService.getAllFromCache().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.AllVatTypes = myResponse.Result;
            }
        });
    };
    NewGeneralAPInvoiceComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            _this.EntityPM = _this.myEntityPMService.GetNewEntityPM();
            _this.EntityPM.IsGeneralInvoice = true;
            _this.AccountingDate = todayDate;
            _this.EntityPM.MainEntityId = null;
            _this.EntityPM.MainEntityReference = null;
            _this.EntityPM.HouseNumber = null;
            _this.EntityPM.MasterNumber = null;
            Tools_2.InvoiceTool.ComputeAPInvoiceDueDate(_this.EntityPM);
            _this.IsResourcesReady = true;
            _this.SetUIProperties();
            _this.LoadData();
        });
    };
    NewGeneralAPInvoiceComponent.prototype.SetUIProperties = function () {
        var isVatNumberRequired = false;
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAP) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.VATNumber)) {
                isVatNumberRequired = true;
            }
        }
        this.UIProperties.SetRequired("VATNumber", "APInvoice", isVatNumberRequired);
        this.UIProperties.SetRequired("AccountingDate", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.AccountingDate));
        this.SetUIProperties_DueDate();
        this.SetUIProperties_ExchangeRate();
    };
    NewGeneralAPInvoiceComponent.prototype.SetUIProperties_DueDate = function () {
        var AllowManuallyDueDate = false;
        if (SessionLocator_1.SessionLocator.AccountingSystemPM) {
            AllowManuallyDueDate = SessionLocator_1.SessionLocator.AccountingSystemPM.AllowManuallyDueDate;
        }
        this.UIProperties.SetEnabled("DueDate", this.ObjectTableName, AllowManuallyDueDate);
        if (AllowManuallyDueDate) {
            this.PaymentTermDisplayInLOV = null;
        }
    };
    NewGeneralAPInvoiceComponent.prototype.SetUIProperties_ExchangeRate = function () {
        var isEnabled = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APInvoice", "APInvoiceEditExchangeRate")) {
            if (this.InvoiceCurrencyId) {
                if (this.InvoiceCurrencyId != SessionLocator_1.SessionLocator.TenantPM.CurrencyId) {
                    isEnabled = true;
                }
            }
        }
        this.RateIsEnabled = isEnabled;
        this.UIProperties.SetEnabled("InvoiceCurrencyExchangeRate", "APInvoice", isEnabled);
    };
    NewGeneralAPInvoiceComponent.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        var myCurrencyRatesService = new CurrencyRatesService_1.CurrencyRatesService();
        var myCommonDomainService = new CommonDomainService_1.CommonDomainService();
        var loadingDate = this.EntityPM.InvoiceDate;
        if (loadingDate == null) {
            loadingDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
        myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator_1.SessionLocator.LocalCurrencyId, loadingDate).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.LastRatesList = myResponse.Result;
                _this.SetCurrencyRateData();
                myCommonDomainService.GetVatTypePercentagePMByDate(loadingDate).subscribe(function (myResponse2) {
                    if (!myResponse2.HasError) {
                        _this.VatTypePercentagesList = myResponse2.Result;
                    }
                    _this.CurrentSession.StopBusyIndicator();
                });
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    NewGeneralAPInvoiceComponent.prototype.GetCurrencyRate = function (currencyId) {
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
    NewGeneralAPInvoiceComponent.prototype.GetCurrencyRateDate = function (currencyId) {
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
    NewGeneralAPInvoiceComponent.prototype.GetVatTypePercentage = function (vatTypeId) {
        var myResult = null;
        var vatTypePercentagePM = this.VatTypePercentagesList.filter(function (d) { return d.VatTypeId == vatTypeId; })[0];
        if (vatTypePercentagePM != null) {
            myResult = vatTypePercentagePM.Percentage;
        }
        return myResult;
    };
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "VendorDependencyProperty1", {
        // Vendor Properties
        get: function () { return Tools_2.InvoiceTool.GetVendorPartnerTypes(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "VendorId", {
        get: function () { return this.EntityPM.VendorId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.VendorId != value) {
                this.EntityPM.VendorId = value;
                this.CheckDuplication();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.VATNumber = null;
                    this.VendorName = null;
                    this.EntityPM.VendorPartnerTypeId = null;
                    this.InvoiceCurrencyId = SessionLocator_1.SessionLocator.AccountingCurrencyId;
                    this.PaymentTermId = SessionLocator_1.SessionLocator.TenantPM.PaymentTermId;
                    this.EntityPM.VendorGLAccountId = null;
                }
                else {
                    this.myCardListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.VATNumber = list.VatNumber;
                                _this.VendorName = list.EnglishName;
                                _this.EntityPM.VendorPartnerTypeId = list.PartnerTypeId;
                                if (!Tools_1.AppTool.IsNullOrEmpty(list.InvoiceCurrencyId)) {
                                    _this.InvoiceCurrencyId = list.InvoiceCurrencyId;
                                }
                                if (!Tools_1.AppTool.IsNullOrEmpty(list.PaymentTermId)) {
                                    _this.PaymentTermId = list.PaymentTermId;
                                }
                                if (!Tools_1.AppTool.IsNullOrEmpty(list.VatTypeId)) {
                                    _this.VatTypeId = list.VatTypeId;
                                }
                                if (!Tools_1.AppTool.IsNullOrEmpty(list.GLAccountId)) {
                                    _this.myGLAccountPMService.get(list.GLAccountId).subscribe(function (myResponse) {
                                        if (!myResponse.HasError) {
                                            var glaccount = myResponse.Result;
                                            if (glaccount != null) {
                                                _this.EntityPM.VendorGLAccountId = glaccount.Id;
                                            }
                                        }
                                    });
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
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "VendorName", {
        get: function () { return this.EntityPM.VendorName; },
        set: function (value) {
            if (this.EntityPM.VendorName != value) {
                this.EntityPM.VendorName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "InvoiceNumber", {
        get: function () { return this.EntityPM.InvoiceNumber; },
        set: function (value) {
            if (this.EntityPM.InvoiceNumber != value) {
                this.EntityPM.InvoiceNumber = value;
                this.CheckDuplication();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewGeneralAPInvoiceComponent.prototype.CheckDuplication = function () {
        var _this = this;
        var warnings = [];
        this.FillWarnings(warnings);
        if (!Tools_1.AppTool.IsNullOrEmpty(this.VendorId) && !Tools_1.AppTool.IsNullOrEmpty(this.InvoiceNumber)) {
            this.myInvoiceDomainService.CheckVendor_NumberDuplication(this.EntityPM.VendorId, this.EntityPM.InvoiceNumber, this.EntityPM.Id).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var isDuplicated = myResponse.Result;
                    if (isDuplicated) {
                        warnings.push(TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.M.SameInvoiceNumber"));
                        _this.FillWarnings(warnings);
                    }
                }
            });
        }
    };
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "InvoiceCurrencyId", {
        // Currency Properties
        get: function () { return this.EntityPM.InvoiceCurrencyId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.InvoiceCurrencyId != value) {
                this.EntityPM.InvoiceCurrencyId = value;
                this.SetCurrencyRateData();
                this.SetUIProperties();
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
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "InvoiceCurrencyCode", {
        get: function () { return this.EntityPM.InvoiceCurrencyCode; },
        set: function (value) {
            if (this.EntityPM.InvoiceCurrencyCode != value) {
                this.EntityPM.InvoiceCurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewGeneralAPInvoiceComponent.prototype.SetCurrencyRateData = function () {
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
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "InvoiceCurrencyExchangeRate", {
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
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "ExchangeRateDate", {
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
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "RelativeRateDate", {
        get: function () { return this.myRelativeRateDate; },
        set: function (value) {
            if (this.myRelativeRateDate != value) {
                this.myRelativeRateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewGeneralAPInvoiceComponent.prototype.ComputeRelativeRateDate = function () {
        this.RelativeRateDate = Tools_1.DateTool.GetRelativeRateDate(this.InvoiceDate, this.ExchangeRateDate, "old");
    };
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "ProfitCurrencyId", {
        get: function () { return this.EntityPM.ProfitCurrencyId; },
        set: function (value) {
            if (this.EntityPM.ProfitCurrencyId != value) {
                this.EntityPM.ProfitCurrencyId = value;
                this.InvoiceCurrencyExchangeRate = this.GetCurrencyRate(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "ProfitCurrencyExchangeRate", {
        get: function () { return this.EntityPM.ProfitCurrencyExchangeRate; },
        set: function (value) {
            if (this.EntityPM.ProfitCurrencyExchangeRate != value) {
                this.EntityPM.ProfitCurrencyExchangeRate = Tools_1.AppTool.Round(value, 5);
            }
        },
        enumerable: true,
        configurable: true
    });
    NewGeneralAPInvoiceComponent.prototype.UpdateCurrencyRateClicked = function () {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName("RatesTable").subscribe(function (res) {
            var loadingDate = _this.EntityPM.InvoiceDate;
            if (loadingDate == null) {
                loadingDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            }
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 350;
            logWindow.Title = "Update Currency Rate";
            logWindow.WindowArgs = { CurrencyId: _this.InvoiceCurrencyId, CurrencyCode: _this.InvoiceCurrencyCode, Rate: _this.InvoiceCurrencyExchangeRate, Date: loadingDate };
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
        });
    };
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "VATNumberRedDotVisibility", {
        // Properties
        get: function () {
            var myResult = false;
            if (SessionLocator_1.SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAP) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "VatTypeId", {
        get: function () { return this.vatTypeId; },
        set: function (value) {
            var _this = this;
            if (this.vatTypeId != value) {
                this.vatTypeId = value;
                if (this.EntityPM.InvoiceLines != null) {
                    this.EntityPM.InvoiceLines.forEach(function (item) {
                        item.VatTypeId = value;
                        if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                            item.VatTypeName = null;
                            item.VatPercentage = null;
                        }
                        else {
                            _this.myVatTypeListService.getSingleFromCache(_this.VatTypeId).subscribe(function (myResponse) {
                                if (!myResponse.HasError) {
                                    var list = myResponse.Result;
                                    if (list != null) {
                                        item.VatTypeName = list.EnglishName;
                                        item.VatPercentage = _this.GetVatTypePercentage(_this.VatTypeId);
                                    }
                                }
                            });
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "VATNumber", {
        get: function () { return this.EntityPM.VATNumber; },
        set: function (value) {
            if (this.EntityPM.VATNumber != value) {
                this.EntityPM.VATNumber = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "PaymentTermId", {
        get: function () { return this.EntityPM.PaymentTermId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.PaymentTermId != value) {
                this.EntityPM.PaymentTermId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.PaymentTermName = null;
                }
                else {
                    this.myPaymentTermListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.PaymentTermName = list.EnglishName;
                            }
                        }
                    });
                }
                if (!this.IsAccountingActivated) {
                    Tools_2.InvoiceTool.ComputeAPInvoiceDueDate(this.EntityPM);
                }
                else {
                    Tools_2.InvoiceTool.ComputeFullAccountingAPInvoiceDueDate(this.EntityPM);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "PaymentTermName", {
        get: function () { return this.EntityPM.PaymentTermName; },
        set: function (value) {
            if (this.EntityPM.PaymentTermName != value) {
                this.EntityPM.PaymentTermName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "InvoiceDate", {
        get: function () { return this.EntityPM.InvoiceDate; },
        set: function (value) {
            if (this.EntityPM.InvoiceDate != value) {
                this.EntityPM.InvoiceDate = value;
                if (!this.IsAccountingActivated) {
                    Tools_2.InvoiceTool.ComputeAPInvoiceDueDate(this.EntityPM);
                }
                this.ComputeRelativeRateDate();
                this.LoadData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "DueDate", {
        get: function () { return this.EntityPM.DueDate; },
        set: function (value) {
            if (this.EntityPM.DueDate != value) {
                this.EntityPM.DueDate = value;
                Tools_2.InvoiceTool.ComputeAPInvoicePaymentTerm(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "AccountingDate", {
        get: function () { return this.EntityPM.AccountingDate; },
        set: function (value) {
            if (this.EntityPM.AccountingDate != value) {
                this.EntityPM.AccountingDate = value;
                if (value == null) {
                    this.UIProperties.SetRequired("AccountingDate", this.ObjectTableName, true);
                }
                else {
                    this.UIProperties.SetRequired("AccountingDate", this.ObjectTableName, false);
                }
                if (this.IsAccountingActivated) {
                    Tools_2.InvoiceTool.ComputeFullAccountingAPInvoiceDueDate(this.EntityPM);
                    this.LoadData();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    // Commands    
    NewGeneralAPInvoiceComponent.prototype.FillWarnings = function (warnings) {
        var _this = this;
        this.ValidationWarningsList = [];
        if (warnings != null && warnings.length > 0) {
            warnings.forEach(function (item) {
                _this.ValidationWarningsList.push(item);
            });
        }
    };
    NewGeneralAPInvoiceComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewGeneralAPInvoiceComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.VendorId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.F.VendorId")));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.InvoiceNumber)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.F.InvoiceNumber")));
        }
        if (this.EntityPM.InvoiceExpectedAmount == null) {
            errors.push(msg.replace("%FieldName", "Invoice Amount"));
        }
        if (this.EntityPM.InvoiceDate > this.EntityPM.AccountingDate) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.O.CheckInvoiceDate"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.InvoiceCurrencyId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.F.InvoiceCurrencyId")));
        }
        if (this.EntityPM.InvoiceDate == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.F.InvoiceDate")));
        }
        else if (Tools_1.DateTool.GetDateParts(this.InvoiceDate).DateTicks > Tools_1.DateTool.GetCurrentDateAsUtc().valueOf()) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.M.CantReceiveFutureDateInvoice"));
        }
        if (Tools_1.DateTool.GetDateParts(this.AccountingDate).DateTicks > Tools_1.DateTool.GetCurrentDateAsUtc().valueOf()) {
            errors.push("Cant issue Invoice with Future Accounting Date");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PaymentTermId)) {
            errors.push(msg.replace("%FieldName", "Payment Term"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PaymentTermId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.F.PaymentTermId")));
        }
        if (this.EntityPM.DueDate == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.F.DueDate")));
        }
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAP) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.VATNumber)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.F.VATNumber")));
            }
        }
        if (this.IsAccountingActivated && this.AccountingDate == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.F.AccountingDate")));
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.IsAccountingActivated == true) {
                var invoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
                invoiceDomainService.ValidateAPInvoiceFullAccounting(this.EntityPM.InvoiceCurrencyId, this.EntityPM.VendorId, this.EntityPM.AccountingDate).subscribe(function (response) {
                    if (response != null) {
                        if (!response.HasError) {
                            _this.CompleteSubmission(errors);
                        }
                        else {
                            _this.ValidationErrorsList = response.ErrorsArray;
                        }
                    }
                });
            }
        }
    };
    NewGeneralAPInvoiceComponent.prototype.CompleteSubmission = function (errors) {
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Loading"));
            this.InitializeProfitCurrency();
            this.CurrentSession.CloseCurrentWindowEmit("Ok");
        }
    };
    NewGeneralAPInvoiceComponent.prototype.InitializeProfitCurrency = function () {
        var _this = this;
        if (this.EntityPM.IsMultipleEntities) {
            this.EntityPM.ProfitCurrencyId = SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyId;
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ProfitCurrencyId)) {
            this.EntityPM.ProfitCurrencyId = SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyId;
        }
        this.myCurrencyListService.getSingleFromCache(this.EntityPM.ProfitCurrencyId).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var list = myResponse.Result;
                if (list != null) {
                    _this.EntityPM.ProfitCurrencyCode = list.Code;
                }
            }
        });
        this.EntityPM.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
    };
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "AmountInInvoiceCurrency", {
        get: function () { return this.EntityPM.AmountInInvoiceCurrency; },
        set: function (value) {
            var setValue = Tools_1.AppTool.Round(value, 2);
            if (this.EntityPM.AmountInInvoiceCurrency != setValue) {
                this.EntityPM.AmountInInvoiceCurrency = setValue;
                this.EntityPM.InvoiceExpectedAmount = setValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "AmountInLocalCurrency", {
        get: function () { return this.EntityPM.AmountInLocalCurrency; },
        set: function (value) {
            var setValue = Tools_1.AppTool.Round(value, 2);
            if (this.EntityPM.AmountInLocalCurrency != setValue) {
                this.EntityPM.AmountInLocalCurrency = setValue;
                this.EntityPM.AmountDueInLocalCurrency = setValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "AmountInProfitCurrency", {
        get: function () { return this.EntityPM.AmountInProfitCurrency; },
        set: function (value) {
            var setValue = Tools_1.AppTool.Round(value, 2);
            if (this.EntityPM.AmountInProfitCurrency != setValue) {
                this.EntityPM.AmountInProfitCurrency = setValue;
                this.EntityPM.AmountDueInProfitCurrency = setValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "SubTotalInLocalCurrency", {
        get: function () {
            return this.EntityPM.SubTotalInLocalCurrency;
        },
        set: function (value) {
            var setValue = Tools_1.AppTool.Round(value, 2);
            if (this.EntityPM.SubTotalInLocalCurrency != setValue) {
                this.EntityPM.SubTotalInLocalCurrency = setValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralAPInvoiceComponent.prototype, "SubTotalInInvoiceCurrency", {
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
    NewGeneralAPInvoiceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewGeneralAPInvoiceComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], NewGeneralAPInvoiceComponent);
    return NewGeneralAPInvoiceComponent;
}(BaseComponent_1.BaseComponent));
exports.NewGeneralAPInvoiceComponent = NewGeneralAPInvoiceComponent;
//# sourceMappingURL=NewGeneralAPInvoiceComponent.js.map