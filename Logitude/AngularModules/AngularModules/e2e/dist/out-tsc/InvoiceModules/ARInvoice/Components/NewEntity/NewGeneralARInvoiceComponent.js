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
var ARInvoicePM_1 = require("../../../../Invoice/EntityPMs/ARInvoicePM");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var CurrencyRatesService_1 = require("../../../../Common/Services/CurrencyRatesService");
var Tools_2 = require("../../../../Invoice/Tools");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var PaymentTermListService_1 = require("../../../../Common/Services/StandardLists/PaymentTermListService");
var VatTypeListService_1 = require("../../../../Common/Services/StandardLists/VatTypeListService");
var ChargesTypeListService_1 = require("../../../../Common/Services/StandardLists/ChargesTypeListService");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var GLAccountPMService_1 = require("../../../../Accounting/Services/StandardPMs/GLAccountPMService");
var AccountingPeriodExtendedListService_1 = require("../../../../Accounting/Services/ExtendedLists/AccountingPeriodExtendedListService");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var q_1 = require("q");
var NewGeneralARInvoiceComponent = /** @class */ (function (_super) {
    __extends(NewGeneralARInvoiceComponent, _super);
    function NewGeneralARInvoiceComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.ObjectTableName = "ARInvoice";
        _this.DataContext = _this;
        _this.InvoicePartners = [];
        _this.ValidationErrorsList = [];
        _this.IsResourcesReady = false;
        _this.DisplaySATPaymentMethod = false;
        _this.IsEditExchangeRateVisible = false;
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.TypeCode = "";
        // SetUIProperties
        _this.RateIsEnabled = false;
        _this.PaymentTermDisplayInLOV = true;
        // BillTo
        _this.BillToDependencyProperty1 = null;
        _this.BillToDependencyProperty1IsList = true;
        _this.SelectedPartnerType = null;
        _this.cardList = null;
        _this.glaccount = null;
        _this.myRelativeRateDate = null;
        // Load Date
        _this.LastRatesList = [];
        _this.VatTypePercentagesList = [];
        _this.myCurrencyRatesService = new CurrencyRatesService_1.CurrencyRatesService();
        //Commands
        _this.isOkClicked = false;
        _this.errors = [];
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.entityResourceService.getEntityResourceByTableName(_this.ObjectTableName).subscribe(function (res) {
            _this.InitializeServices();
        });
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            _this.DisplaySATPaymentMethod = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            _this.IsEditExchangeRateVisible = true;
        }
        return _this;
    }
    NewGeneralARInvoiceComponent.prototype.SetWindowArgs = function (args) {
        this.TypeCode = args["InvoiceTypeCode"];
        this.CreateNewEntity();
    };
    NewGeneralARInvoiceComponent.prototype.InitializeServices = function () {
        this.myCardListService = new CardListService_1.CardListService();
        this.myCurrencyListService = new CurrencyListService_1.CurrencyListService();
        this.myPaymentTermListService = new PaymentTermListService_1.PaymentTermListService();
        this.myVatTypeListService = new VatTypeListService_1.VatTypeListService();
        this.myChargesTypeListService = new ChargesTypeListService_1.ChargesTypeListService();
        this.myCommonDomainService = new CommonDomainService_1.CommonDomainService();
        this.myGLAccountPMService = new GLAccountPMService_1.GLAccountPMService();
        this.myAccountingPeriodListService = new AccountingPeriodExtendedListService_1.AccountingPeriodExtendedListService();
    };
    NewGeneralARInvoiceComponent.prototype.GetClosedMonth = function () {
        var _this = this;
        return new Promise(function (resolve) {
            var periodTypeCode = "1"; // 1-Regular
            _this.myAccountingPeriodListService.getByYear(_this.InvoiceDate.getFullYear(), periodTypeCode).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        _this.accountingPeriod = myResponse.Result;
                        resolve(myResponse.Result);
                    }
                }
            });
        });
    };
    NewGeneralARInvoiceComponent.prototype.CreateNewEntity = function () {
        var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        this.EntityPM = new ARInvoicePM_1.ARInvoicePM();
        this.EntityPM.BillToPartnerTypeId = "CS";
        this.EntityPM.StatusCode = "DR";
        this.EntityPM.StatusName = "Draft";
        this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        this.EntityPM.IssuedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.EntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.EntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.EntityPM.CreateDate = todayDate;
        this.EntityPM.UpdateDate = todayDate;
        this.EntityPM.InvoiceDate = todayDate;
        this.EntityPM.BranchId = SessionLocator_1.SessionLocator.LoggedUserPM.BranchId;
        this.EntityPM.LocalCurrencyId = SessionLocator_1.SessionLocator.TenantPM.CurrencyId;
        this.EntityPM.ARInvoiceTypeCode = this.TypeCode;
        //this.EntityPM.PrepaidCollectId = this.args.InvoiceTypeCode == "MN" ? "C" : "B";
        this.EntityPM.MainEntityId = null;
        this.EntityPM.MainEntityReference = null;
        this.EntityPM.HouseNumber = null;
        this.EntityPM.MasterNumber = null;
        //this.EntityPM.ProfitCurrencyId = this.shipmentPM.ProfitCurrencyId;
        //this.EntityPM.OperationalDate = InvoiceTool.GetOperationalDate(this.shipmentPM);
        var myDescription = null;
        //switch (this.shipmentPM.DirectionId) {
        //    case "E": { myDescription = "Export to " + this.shipmentPM.MainCarriageToPortCode; break; }
        //    case "I": { myDescription = "Import from " + this.shipmentPM.MainCarriageFromPortCode; break; }
        //    case "D": { myDescription = "Ship to " + this.shipmentPM.ToPartnerCity; break; }
        //}
        this.EntityPM.Description = myDescription;
        this.EntityPM.IsGeneralInvoice = true;
        this.EntityPM.IsFullAccounting = true;
        this.InvoiceCurrencyId = SessionLocator_1.SessionLocator.TenantPM.CurrencyId;
        this.PaymentTermId = SessionLocator_1.SessionLocator.TenantPM.PaymentTermId;
        this.EntityPM.ProfitCurrencyId = SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyId;
        this.SetUIProperties();
        this.BuildPartnersTypes();
        this.LoadCurrencyRates();
        this.IsResourcesReady = true;
    };
    NewGeneralARInvoiceComponent.prototype.SetUIProperties = function () {
        this.SetUIProperties_BillToAddress();
        this.SetUIProperties_VatNumber();
        this.SetUIProperties_ExchangeRate();
        //this.SetUIProperties_General(false);
        this.SetUIProperties_DueDate();
        this.SetUIProperties_Payment();
    };
    NewGeneralARInvoiceComponent.prototype.SetUIProperties_BillToAddress = function () {
        var isFieldtEnabled = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.BillToId)) {
            isFieldtEnabled = true;
        }
        this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, isFieldtEnabled);
    };
    NewGeneralARInvoiceComponent.prototype.SetUIProperties_VatNumber = function () {
        var isFieldRequired = false;
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAR) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.VatNumber)) {
                isFieldRequired = true;
            }
            this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, isFieldRequired);
        }
    };
    NewGeneralARInvoiceComponent.prototype.SetUIProperties_ExchangeRate = function () {
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
    NewGeneralARInvoiceComponent.prototype.SetUIProperties_General = function (isEnabled) {
        this.UIProperties.SetEnabled("IsConstituentInvoice", this.ObjectTableName, isEnabled);
    };
    NewGeneralARInvoiceComponent.prototype.SetUIProperties_DueDate = function () {
        var AllowManuallyDueDate = false;
        if (SessionLocator_1.SessionLocator.AccountingSystemPM) {
            AllowManuallyDueDate = SessionLocator_1.SessionLocator.AccountingSystemPM.AllowManuallyDueDate;
        }
        this.UIProperties.SetEnabled("DueDate", this.ObjectTableName, AllowManuallyDueDate);
        if (AllowManuallyDueDate) {
            this.PaymentTermDisplayInLOV = null;
        }
    };
    NewGeneralARInvoiceComponent.prototype.SetUIProperties_Payment = function () {
        this.UIProperties.SetRequired("SATPaymentMethodCode", this.ObjectTableName, false);
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.SATPaymentMethodCode)) {
                this.UIProperties.SetRequired("SATPaymentMethodCode", this.ObjectTableName, true);
            }
        }
    };
    NewGeneralARInvoiceComponent.prototype.BuildPartnersTypes = function () {
        this.BillToDependencyProperty1 = "CS";
        if (this.EntityPM.Id == null) {
            //if (this.args.EntityTableName == "Shipment") {
            //    var customer: InvoicePartnerType = this.InvoicePartners.filter(d => d.Code == "CUS")[0];
            //    this.SelectedPartnerType = customer;
            //    this.PartnersTypeSelectionMethod(customer);
            //}
            //else if (this.args.EntityTableName == "Master") {
            //    var agent: InvoicePartnerType = this.InvoicePartners.filter(d => d.Code == "AGE")[0];
            //    this.SelectedPartnerType = agent;
            //    this.PartnersTypeSelectionMethod(agent);
            //}
        }
    };
    Object.defineProperty(NewGeneralARInvoiceComponent.prototype, "BillToPartnerTypeId", {
        get: function () { return this.billToPartnerTypeId; },
        set: function (newValue) {
            if (this.billToPartnerTypeId != newValue) {
                this.billToPartnerTypeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralARInvoiceComponent.prototype, "BillToId", {
        get: function () { return this.EntityPM.BillToId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.BillToId != newValue) {
                this.EntityPM.BillToId = newValue;
                this.SetUIProperties_BillToAddress();
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.VatNumber = null;
                    this.BillToName = null;
                    this.BillToAddressId = null;
                    this.SATPaymentMethodCode = null;
                    this.InvoiceCurrencyId = SessionLocator_1.SessionLocator.TenantPM.CurrencyId;
                    this.PaymentTermId = SessionLocator_1.SessionLocator.TenantPM.PaymentTermId;
                }
                else {
                    this.myCardListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            _this.cardList = list;
                            if (list != null) {
                                _this.VatNumber = list.VatNumber;
                                _this.BillToName = list.EnglishName;
                                if (!Tools_1.AppTool.IsNullOrEmpty(list.SATPaymentMethodCode)) {
                                    _this.SATPaymentMethodCode = list.SATPaymentMethodCode;
                                }
                                if (_this.isOkClicked == false) {
                                    if (_this.ValidationErrorsList != null && _this.ValidationErrorsList.length == 0) {
                                        _this.ValidationErrorsList = [];
                                    }
                                }
                                else {
                                    _this.ValidateEntity();
                                }
                                if (Tools_1.AppTool.IsNullOrEmpty(list.GLAccountId)) {
                                    if (_this.isOkClicked == false) {
                                        _this.ValidationErrorsList = [];
                                        _this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.NewGeneralInvoiceErrorMsg1"));
                                    }
                                }
                                else {
                                    _this.ValidationErrorsList = null;
                                    _this.myGLAccountPMService.get(list.GLAccountId).subscribe(function (myResponse) {
                                        if (!myResponse.HasError) {
                                            _this.glaccount = myResponse.Result;
                                            if (_this.glaccount != null) {
                                                if (_this.glaccount.IsMultiCurrency == true) {
                                                    _this.InvoiceCurrencyId = null;
                                                }
                                                else {
                                                    _this.InvoiceCurrencyId = _this.glaccount.CurrencyId;
                                                }
                                            }
                                        }
                                    });
                                }
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
                                if (_this.isOkClicked == true) {
                                    _this.ValidateEntity();
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
    Object.defineProperty(NewGeneralARInvoiceComponent.prototype, "BillToName", {
        get: function () { return this.EntityPM.BillToName; },
        set: function (newValue) {
            if (this.EntityPM.BillToName != newValue) {
                this.EntityPM.BillToName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralARInvoiceComponent.prototype, "BillToAddressId", {
        get: function () { return this.EntityPM.BillToAddressId; },
        set: function (newValue) {
            if (this.EntityPM.BillToAddressId != newValue) {
                this.EntityPM.BillToAddressId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralARInvoiceComponent.prototype, "InvoiceCurrencyId", {
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
                if (this.isOkClicked == false) {
                    if (this.ValidationErrorsList == null) {
                        this.ValidationErrorsList = [];
                    }
                    // Check if the same as glaccount currency
                    if (this.glaccount != null && this.glaccount.IsMultiCurrency == false && newValue != this.glaccount.CurrencyId) {
                        this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.NewGeneralInvoiceErrorMsg2"));
                    }
                }
                else {
                    this.ValidateEntity();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralARInvoiceComponent.prototype, "InvoiceCurrencyCode", {
        get: function () { return this.EntityPM.InvoiceCurrencyCode; },
        set: function (value) {
            if (this.EntityPM.InvoiceCurrencyCode != value) {
                this.EntityPM.InvoiceCurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralARInvoiceComponent.prototype, "InvoiceCurrencyExchangeRate", {
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
    Object.defineProperty(NewGeneralARInvoiceComponent.prototype, "ExchangeRateDate", {
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
    Object.defineProperty(NewGeneralARInvoiceComponent.prototype, "RelativeRateDate", {
        get: function () { return this.myRelativeRateDate; },
        set: function (value) {
            if (this.myRelativeRateDate != value) {
                this.myRelativeRateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewGeneralARInvoiceComponent.prototype.ComputeRelativeRateDate = function () {
        this.RelativeRateDate = Tools_1.DateTool.GetRelativeRateDate(this.InvoiceDate, this.ExchangeRateDate, "old");
    };
    Object.defineProperty(NewGeneralARInvoiceComponent.prototype, "VatTypeId", {
        get: function () { return this.vatTypeId; },
        set: function (newValue) {
            if (this.vatTypeId != newValue) {
                this.vatTypeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralARInvoiceComponent.prototype, "VatNumber", {
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
    Object.defineProperty(NewGeneralARInvoiceComponent.prototype, "PaymentTermId", {
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
    Object.defineProperty(NewGeneralARInvoiceComponent.prototype, "PaymentTermName", {
        get: function () { return this.EntityPM.PaymentTermName; },
        set: function (newValue) {
            if (this.EntityPM.PaymentTermName != newValue) {
                this.EntityPM.PaymentTermName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralARInvoiceComponent.prototype, "InvoiceDate", {
        get: function () { return this.EntityPM.InvoiceDate; },
        set: function (newValue) {
            if (this.EntityPM.InvoiceDate != newValue) {
                this.EntityPM.InvoiceDate = newValue;
                Tools_2.InvoiceTool.ComputeARInvoiceDueDate(this.EntityPM);
                this.ComputeRelativeRateDate();
                this.LoadCurrencyRates();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralARInvoiceComponent.prototype, "DueDate", {
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
    Object.defineProperty(NewGeneralARInvoiceComponent.prototype, "CustomerRef", {
        get: function () { return this.EntityPM.CustomerRef; },
        set: function (newValue) {
            if (this.EntityPM.CustomerRef != newValue) {
                this.EntityPM.CustomerRef = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralARInvoiceComponent.prototype, "IsConstituentInvoice", {
        get: function () { return this.EntityPM.IsConstituentInvoice; },
        set: function (newValue) {
            if (this.EntityPM.IsConstituentInvoice != newValue) {
                this.EntityPM.IsConstituentInvoice = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGeneralARInvoiceComponent.prototype, "SATPaymentMethodCode", {
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
    NewGeneralARInvoiceComponent.prototype.LoadCurrencyRates = function () {
        var _this = this;
        return new Promise(function (resolve) {
            var loadingDate = _this.EntityPM.InvoiceDate || Tools_1.DateTool.GetCurrentDateAsUtc();
            _this.myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator_1.SessionLocator.TenantPM.CurrencyId, loadingDate).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.LastRatesList = myResponse.Result;
                    _this.SetCurrencyRateData();
                    resolve(myResponse.Result);
                }
                else {
                    q_1.reject();
                }
            });
        });
    };
    NewGeneralARInvoiceComponent.prototype.SetCurrencyRateData = function () {
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
    NewGeneralARInvoiceComponent.prototype.GetCurrencyRate = function (currencyId) {
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
    NewGeneralARInvoiceComponent.prototype.GetCurrencyRateDate = function (currencyId) {
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
    NewGeneralARInvoiceComponent.prototype.GetVatTypePercentage = function (vatTypeId) {
        var myResult = null;
        var vatTypePercentagePM = this.VatTypePercentagesList.filter(function (d) { return d.VatTypeId == vatTypeId; })[0];
        if (vatTypePercentagePM != null) {
            myResult = vatTypePercentagePM.Percentage;
        }
        return myResult;
    };
    NewGeneralARInvoiceComponent.prototype.UpdateCurrencyRateClicked = function () {
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
    NewGeneralARInvoiceComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewGeneralARInvoiceComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.GetClosedMonth().then(function (res) {
            _this.CurrentSession.StopBusyIndicator();
            _this.isOkClicked = true;
            var isEntityValid = _this.ValidateEntity();
            if (isEntityValid) {
                _this.CurrentSession.StartBusyIndicatorLoading();
                _this.LoadCurrencyRates().then(function (res) {
                    _this.SubmitChanges();
                });
            }
        });
    };
    NewGeneralARInvoiceComponent.prototype.ValidateEntity = function () {
        this.errors = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var isValid = this.IsMonthOpenForAccountingDate();
        if (!isValid)
            this.errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("AccountingPeriods.O.ClosedMonth")); // closed month
        if (this.glaccount != null && this.glaccount.IsMultiCurrency == false && this.InvoiceCurrencyId != this.glaccount.CurrencyId) {
            this.errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.NewGeneralInvoiceErrorMsg2"));
        }
        if (this.cardList != null && Tools_1.AppTool.IsNullOrEmpty(this.cardList.GLAccountId)) {
            this.errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.NewGeneralInvoiceErrorMsg3"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.BillToId)) {
            this.errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.F.BillToId")));
        }
        //if (AppTool.IsNullOrEmpty(this.BillToAddressId)) {
        //    this.errors.push(msg.replace("%FieldName", "Address"));
        //}
        if (Tools_1.AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
            this.errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.F.InvoiceCurrencyId")));
        }
        if (this.InvoiceDate == null) {
            this.errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.F.InvoiceDate")));
        }
        else {
            var date1 = new Date(this.InvoiceDate.toString());
            var date2 = Tools_1.DateTool.GetCurrentDateAsUtc();
            if (date1.valueOf() > date2.valueOf()) {
                this.errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.CantIssueInvoiceWithFutureDate"));
            }
        }
        if (this.DueDate == null) {
            this.errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.F.DueDate")));
        }
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAR) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.VatNumber)) {
                this.errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.F.VatNumber")));
            }
        }
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.SATPaymentMethodCode)) {
                this.errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.F.SATPaymentMethodCode")));
            }
        }
        if (this.DueDate && this.InvoiceDate) {
            if (this.DueDate.valueOf() < this.InvoiceDate.valueOf()) {
                this.errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.DueDateLowerThanInvoiceDate"));
            }
        }
        this.ValidationErrorsList = this.errors;
        return this.errors.length == 0;
    };
    NewGeneralARInvoiceComponent.prototype.IsMonthOpenForAccountingDate = function () {
        var valid = true;
        if (this.accountingPeriod == null) {
            valid = false;
            //errorsList.Add(transText);
        }
        else {
            var accountingDateMonth = this.EntityPM.InvoiceDate.getMonth() + 1;
            if (accountingDateMonth > this.accountingPeriod.ClosedMonth) {
                //Valid ... AccountingDateMonth must be greater than close Mounth
            }
            else {
                //Not Valid ... AccountingDateMonth must be greater than close Mounth
                //not valid  8>=8
                //not valid  0>=1 - Must Open mounth before work on year !!
                valid = false;
                //errorsList.Add(transText); //ClosedMonth Must B
            }
            if (accountingDateMonth == this.accountingPeriod.OpenMonth) {
                //valid ... accountingDateMonth can be  equal to OpenMonth
            }
            else if (accountingDateMonth < this.accountingPeriod.OpenMonth) {
                //valid ... accountingDateMonth can be  less than OpenMonth
            }
            else {
                valid = false;
                //errorsList.Add(transText);
            }
        }
        return valid;
    };
    NewGeneralARInvoiceComponent.prototype.SubmitChanges = function () {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.InitializeComponent();
    };
    NewGeneralARInvoiceComponent.prototype.InitializeComponent = function () {
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
        this.EntityPM.StatusCode = "DR";
        this.EntityPM.StatusName = "Draft";
        this.CurrentSession.CloseCurrentWindowEmit("Ok");
    };
    NewGeneralARInvoiceComponent = __decorate([
        core_1.Component({
            selector: 'NewGeneralARInvoiceComponent',
            moduleId: module.id,
            templateUrl: './NewGeneralARInvoiceComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], NewGeneralARInvoiceComponent);
    return NewGeneralARInvoiceComponent;
}(BaseComponent_1.BaseComponent));
exports.NewGeneralARInvoiceComponent = NewGeneralARInvoiceComponent;
//# sourceMappingURL=NewGeneralARInvoiceComponent.js.map