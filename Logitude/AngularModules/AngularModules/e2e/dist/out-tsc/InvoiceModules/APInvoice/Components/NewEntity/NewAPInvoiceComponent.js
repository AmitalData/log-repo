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
var APInvoiceLinePM_1 = require("../../../../Invoice/EntityPMs/APInvoiceLinePM");
var APInvoiceTotalVATPM_1 = require("../../../../Invoice/EntityPMs/APInvoiceTotalVATPM");
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
var Args_1 = require("../../../../Invoice/Args");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var NewAPInvoiceComponent = /** @class */ (function (_super) {
    __extends(NewAPInvoiceComponent, _super);
    function NewAPInvoiceComponent(entityResourceService) {
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
        _this.IsFullAccounting = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.AllVatTypes = [];
        _this.shipmentPM = null;
        _this.isMultipleEntities = false;
        // SetUIProperties
        _this.PaymentTermDisplayInLOV = true;
        // Load Data 
        _this.LastRatesList = [];
        _this.VatTypePercentagesList = [];
        _this.myRelativeRateDate = null;
        // UpdateCurrencyRate
        _this.RateIsEnabled = false;
        _this.vatTypeId = null;
        _this.IsFullAccounting = SessionLocator_1.SessionLocator.TenantPM.AccountingActivated;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.IsAccountingActivated = SessionLocator_1.SessionLocator.TenantPM.AccountingActivated;
        _this.InitializeServices();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            _this.IsEditExchangeRateVisible = true;
        }
        return _this;
    }
    NewAPInvoiceComponent.prototype.InitializeServices = function () {
        var _this = this;
        this.myCardListService = new CardListService_1.CardListService();
        this.myPaymentTermListService = new PaymentTermListService_1.PaymentTermListService();
        this.myCurrencyListService = new CurrencyListService_1.CurrencyListService();
        this.myChargesTypeListService = new ChargesTypeListService_1.ChargesTypeListService();
        this.myVatTypeListService = new VatTypeListService_1.VatTypeListService();
        this.myInvoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        this.myEntityPMService = new APInvoicePMService_1.APInvoicePMService();
        this.myVatTypeListService.getAllFromCache().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.AllVatTypes = myResponse.Result;
            }
        });
    };
    NewAPInvoiceComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.shipmentPM = args['ShipmentPM'];
        this.isMultipleEntities = args['IsMultipleEntities'];
        if (this.shipmentPM) {
            this.isMultipleEntities = false;
        }
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            _this.EntityPM = _this.myEntityPMService.GetNewEntityPM();
            _this.EntityPM.IsMultipleEntities = _this.isMultipleEntities;
            _this.IsResourcesReady = true;
            _this.GetShipmentData(_this.shipmentPM);
            _this.SetUIProperties();
            _this.LoadData();
        });
    };
    NewAPInvoiceComponent.prototype.GetShipmentData = function (shipmentPM) {
        if (!this.EntityPM.IsMultipleEntities) {
            this.shipmentPM = shipmentPM;
            if (shipmentPM.ShipmentLevelCode == "C") {
                this.myMainEntityObjectTableName = "Master";
            }
            else {
                this.myMainEntityObjectTableName = "Shipment";
            }
            this.EntityPM.MainEntityId = shipmentPM.Id;
            this.EntityPM.MainEntityReference = shipmentPM.ShipmentNumber;
            this.EntityPM.HouseNumber = shipmentPM.House;
            this.EntityPM.MasterNumber = shipmentPM.LongMaster;
            this.EntityPM.ProfitCurrencyId = shipmentPM.ProfitCurrencyId;
            this.EntityPM.BranchId = shipmentPM.BranchId;
            var myDescription = null;
            switch (shipmentPM.DirectionId) {
                case "E": {
                    myDescription = "Export to " + shipmentPM.MainCarriageFinalDestinationPortCode;
                    break;
                }
                case "I": {
                    myDescription = "Import from " + shipmentPM.MainCarriageFromPortCode;
                    break;
                }
                case "D": {
                    myDescription = "Ship to " + shipmentPM.ToPartnerCity;
                    break;
                }
            }
            this.EntityPM.Description = myDescription;
            this.EntityPM.OperationalDate = Tools_2.InvoiceTool.GetOperationalDate(shipmentPM);
            this.EntityPM.ShipmentConcurrencyGUID = this.shipmentPM.ConcurrencyGUID;
            this.EntityPM.ShipmentNewConcurrencyGUID = this.shipmentPM.NewConcurrencyGUID;
        }
    };
    NewAPInvoiceComponent.prototype.SetUIProperties = function () {
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
    NewAPInvoiceComponent.prototype.SetUIProperties_DueDate = function () {
        var AllowManuallyDueDate = false;
        if (SessionLocator_1.SessionLocator.AccountingSystemPM) {
            AllowManuallyDueDate = SessionLocator_1.SessionLocator.AccountingSystemPM.AllowManuallyDueDate;
        }
        this.UIProperties.SetEnabled("DueDate", this.ObjectTableName, AllowManuallyDueDate);
        if (AllowManuallyDueDate) {
            this.PaymentTermDisplayInLOV = null;
        }
    };
    NewAPInvoiceComponent.prototype.SetUIProperties_ExchangeRate = function () {
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
    NewAPInvoiceComponent.prototype.LoadData = function () {
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
    NewAPInvoiceComponent.prototype.GetCurrencyRate = function (currencyId) {
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
    NewAPInvoiceComponent.prototype.GetCurrencyRateDate = function (currencyId) {
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
    NewAPInvoiceComponent.prototype.GetVatTypePercentage = function (vatTypeId) {
        var myResult = null;
        var vatTypePercentagePM = this.VatTypePercentagesList.filter(function (d) { return d.VatTypeId == vatTypeId; })[0];
        if (vatTypePercentagePM != null) {
            myResult = vatTypePercentagePM.Percentage;
        }
        return myResult;
    };
    Object.defineProperty(NewAPInvoiceComponent.prototype, "VendorDependencyProperty1", {
        // Vendor Properties
        get: function () { return Tools_2.InvoiceTool.GetVendorPartnerTypes(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewAPInvoiceComponent.prototype, "VendorId", {
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
                    this.VatTypeId = null;
                }
                else {
                    this.myCardListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.VATNumber = list.VatNumber;
                                _this.VendorName = list.EnglishName;
                                _this.EntityPM.VendorPartnerTypeId = list.PartnerTypeId;
                                _this.VatTypeId = list.VatTypeId;
                                if (!Tools_1.AppTool.IsNullOrEmpty(list.InvoiceCurrencyId)) {
                                    _this.InvoiceCurrencyId = list.InvoiceCurrencyId;
                                }
                                if (!Tools_1.AppTool.IsNullOrEmpty(list.PaymentTermId)) {
                                    _this.PaymentTermId = list.PaymentTermId;
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
    Object.defineProperty(NewAPInvoiceComponent.prototype, "VendorName", {
        get: function () { return this.EntityPM.VendorName; },
        set: function (value) {
            if (this.EntityPM.VendorName != value) {
                this.EntityPM.VendorName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewAPInvoiceComponent.prototype, "InvoiceNumber", {
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
    NewAPInvoiceComponent.prototype.CheckDuplication = function () {
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
    Object.defineProperty(NewAPInvoiceComponent.prototype, "InvoiceCurrencyId", {
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
    Object.defineProperty(NewAPInvoiceComponent.prototype, "InvoiceCurrencyCode", {
        get: function () { return this.EntityPM.InvoiceCurrencyCode; },
        set: function (value) {
            if (this.EntityPM.InvoiceCurrencyCode != value) {
                this.EntityPM.InvoiceCurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewAPInvoiceComponent.prototype.SetCurrencyRateData = function () {
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
    Object.defineProperty(NewAPInvoiceComponent.prototype, "InvoiceCurrencyExchangeRate", {
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
    Object.defineProperty(NewAPInvoiceComponent.prototype, "ExchangeRateDate", {
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
    Object.defineProperty(NewAPInvoiceComponent.prototype, "RelativeRateDate", {
        get: function () { return this.myRelativeRateDate; },
        set: function (value) {
            if (this.myRelativeRateDate != value) {
                this.myRelativeRateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewAPInvoiceComponent.prototype.ComputeRelativeRateDate = function () {
        this.RelativeRateDate = Tools_1.DateTool.GetRelativeRateDate(this.InvoiceDate, this.ExchangeRateDate, "old");
    };
    Object.defineProperty(NewAPInvoiceComponent.prototype, "ProfitCurrencyId", {
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
    Object.defineProperty(NewAPInvoiceComponent.prototype, "ProfitCurrencyExchangeRate", {
        get: function () { return this.EntityPM.ProfitCurrencyExchangeRate; },
        set: function (value) {
            if (this.EntityPM.ProfitCurrencyExchangeRate != value) {
                this.EntityPM.ProfitCurrencyExchangeRate = Tools_1.AppTool.Round(value, 5);
            }
        },
        enumerable: true,
        configurable: true
    });
    NewAPInvoiceComponent.prototype.UpdateCurrencyRateClicked = function () {
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
    Object.defineProperty(NewAPInvoiceComponent.prototype, "VATNumberRedDotVisibility", {
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
    Object.defineProperty(NewAPInvoiceComponent.prototype, "VatTypeId", {
        get: function () { return this.vatTypeId; },
        set: function (value) {
            if (this.vatTypeId != value) {
                this.vatTypeId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewAPInvoiceComponent.prototype, "VATNumber", {
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
    Object.defineProperty(NewAPInvoiceComponent.prototype, "PaymentTermId", {
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
                if (!this.IsFullAccounting) {
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
    Object.defineProperty(NewAPInvoiceComponent.prototype, "PaymentTermName", {
        get: function () { return this.EntityPM.PaymentTermName; },
        set: function (value) {
            if (this.EntityPM.PaymentTermName != value) {
                this.EntityPM.PaymentTermName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewAPInvoiceComponent.prototype, "InvoiceDate", {
        get: function () { return this.EntityPM.InvoiceDate; },
        set: function (value) {
            if (this.EntityPM.InvoiceDate != value) {
                this.EntityPM.InvoiceDate = value;
                if (!this.IsFullAccounting) {
                    Tools_2.InvoiceTool.ComputeAPInvoiceDueDate(this.EntityPM);
                }
                this.ComputeRelativeRateDate();
                this.LoadData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewAPInvoiceComponent.prototype, "DueDate", {
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
    Object.defineProperty(NewAPInvoiceComponent.prototype, "AccountingDate", {
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
                if (this.IsFullAccounting) {
                    Tools_2.InvoiceTool.ComputeFullAccountingAPInvoiceDueDate(this.EntityPM);
                    this.LoadData();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    // Commands    
    NewAPInvoiceComponent.prototype.FillWarnings = function (warnings) {
        var _this = this;
        this.ValidationWarningsList = [];
        if (warnings != null && warnings.length > 0) {
            warnings.forEach(function (item) {
                _this.ValidationWarningsList.push(item);
            });
        }
    };
    NewAPInvoiceComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewAPInvoiceComponent.prototype.OkButtonClicked = function () {
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
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.F.AmountInInvoiceCurrency")));
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
            else {
                this.CompleteSubmission(errors);
            }
        }
    };
    NewAPInvoiceComponent.prototype.CompleteSubmission = function (errors) {
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Loading"));
            this.InitializeProfitCurrency();
            if (this.EntityPM.IsMultipleEntities) {
                this.ComputeTotals();
                this.CurrentSession.CloseCurrentWindowEmit("Ok");
            }
            else {
                this.BuildOpenAmounts();
            }
        }
    };
    NewAPInvoiceComponent.prototype.InitializeProfitCurrency = function () {
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
    // BuildInvoiceLines
    NewAPInvoiceComponent.prototype.BuildOpenAmounts = function () {
        var filteredPayables = this.shipmentPM.ShipmentPayables;
        filteredPayables = filteredPayables.filter(function (d) { return d.ShipmentPayableParentId == null && (d.ShipmentPayableLineStatusCode == "OAMT" || d.ShipmentPayableLineStatusCode == "PACC"); });
        this.BuildInvoiceLines(filteredPayables);
    };
    NewAPInvoiceComponent.prototype.BuildInvoiceLines = function (filteredPayables) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            filteredPayables.forEach(function (payable) {
                var invoiceLine = new APInvoiceLinePM_1.APInvoiceLinePM(_this.EntityPM);
                invoiceLine.APInvoiceId = _this.EntityPM.Id;
                invoiceLine.Tenant = payable.Tenant;
                invoiceLine.ChargesTypeId = payable.ChargesTypeId;
                invoiceLine.ChargesTypeCode = payable.ChargesTypeCode;
                invoiceLine.ChargesTypeName = payable.ChargesTypeName;
                invoiceLine.EntityPayableId = payable.Id;
                invoiceLine.EntityId = payable.ShipmentId;
                invoiceLine.EntityReference = payable.ShipmentNumber;
                invoiceLine.VendorId = payable.VendorId;
                invoiceLine.VendorName = payable.VendorName;
                invoiceLine.ExpectedAmount = payable.ExpectedAmount;
                invoiceLine.OtherInvoicesAmounts = payable.AccountedAmount;
                invoiceLine.OpenAmount = payable.OpenAmount;
                invoiceLine.CorrectionAmount = payable.CorrectionAmount;
                invoiceLine.CorrectionNote = payable.CorrectionNote;
                invoiceLine.CorrectionByUserId = payable.CorrectionByUserId;
                invoiceLine.CorrectionDate = payable.CorrectionDate;
                invoiceLine.AmountTypeCode = payable.ShipmentPayableAmountTypeCode;
                invoiceLine.ForiegnCurrencyId = payable.CurrencyId;
                invoiceLine.ForiegnCurrencyCode = payable.CurrencyCode;
                invoiceLine.Notes = payable.Notes;
                invoiceLine.PrepaidCollectId = payable.PrepaidCollectId;
                if (invoiceLine.ForiegnCurrencyId == _this.EntityPM.InvoiceCurrencyId) {
                    invoiceLine.ForiegnExchangeRate = _this.EntityPM.InvoiceCurrencyExchangeRate;
                }
                else {
                    invoiceLine.ForiegnExchangeRate = _this.GetCurrencyRate(payable.CurrencyId);
                }
                _this.myChargesTypeListService.getSingleFromCache(invoiceLine.ChargesTypeId).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        _this.SetInvoiceLineVatType(list, payable, invoiceLine);
                        if (list != null) {
                            invoiceLine.Description = list.EnglishName;
                            invoiceLine.LocalDescription = list.LocalName;
                        }
                    }
                });
                _this.EntityPM.InvoiceLines.push(invoiceLine);
            });
        }
        this.ComputeTotals();
        this.CurrentSession.CloseCurrentWindowEmit("Ok");
    };
    NewAPInvoiceComponent.prototype.SetInvoiceLineVatType = function (list, payable, invoiceLine) {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.VatTypeId)) {
            invoiceLine.VatTypeId = this.VatTypeId;
        }
        else if (payable.IsFromQuote && !Tools_1.AppTool.IsNullOrEmpty(payable.VatTypeId)) {
            invoiceLine.VatTypeId = payable.VatTypeId;
        }
        else if (list) {
            invoiceLine.VatTypeId = list.VatTypeId;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(invoiceLine.VatTypeId)) {
            var list_VAT = this.AllVatTypes.filter(function (f) { return f.Id == invoiceLine.VatTypeId; })[0];
            if (list_VAT) {
                invoiceLine.VatTypeName = list_VAT.EnglishName;
                invoiceLine.VatIsMultiPercentage = list_VAT.IsMultiPercentage;
                if (!list_VAT.IsMultiPercentage) {
                    invoiceLine.VatPercentage = this.GetVatTypePercentage(invoiceLine.VatTypeId);
                }
            }
        }
    };
    //  ComputeTotals
    NewAPInvoiceComponent.prototype.ComputeTotals = function () {
        this.ComputeAmounts();
        this.BuildTotalVATs();
        this.SubTotalInLocalCurrency = Tools_1.AppTool.Round(Tools_1.ArrayTool.Sum(this.EntityPM.InvoiceLines, "LocalCurrencyAmount"), 2);
        this.SubTotalInInvoiceCurrency = Tools_1.AppTool.Round(Tools_1.ArrayTool.Sum(this.EntityPM.InvoiceLines, "InvoiceCurrencyAmount"), 2);
        this.EntityPM.AmountInLocalCurrency_Summary = Tools_1.AppTool.Round(this.EntityPM.SubTotalInLocalCurrency + Tools_1.ArrayTool.Sum(this.EntityPM.TotalVATs, "LocalVATAmount"), 2);
        this.EntityPM.AmountInInvoiceCurrency_Summary = Tools_1.AppTool.Round(this.EntityPM.SubTotalInInvoiceCurrency + Tools_1.ArrayTool.Sum(this.EntityPM.TotalVATs, "InvoiceCurrencyVATAmount"), 2);
    };
    NewAPInvoiceComponent.prototype.ComputeAmounts = function () {
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
    };
    NewAPInvoiceComponent.prototype.BuildTotalVATs = function () {
        var _this = this;
        this.EntityPM.TotalVATs = [];
        var myDataLines = this.EntityPM.InvoiceLines.filter(function (f) { return f.VatTypeId != null; });
        if (myDataLines.length > 0) {
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
    Object.defineProperty(NewAPInvoiceComponent.prototype, "AmountInInvoiceCurrency", {
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
    Object.defineProperty(NewAPInvoiceComponent.prototype, "AmountInLocalCurrency", {
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
    Object.defineProperty(NewAPInvoiceComponent.prototype, "AmountInProfitCurrency", {
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
    Object.defineProperty(NewAPInvoiceComponent.prototype, "SubTotalInLocalCurrency", {
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
    Object.defineProperty(NewAPInvoiceComponent.prototype, "SubTotalInInvoiceCurrency", {
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
    NewAPInvoiceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewAPInvoiceComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], NewAPInvoiceComponent);
    return NewAPInvoiceComponent;
}(BaseComponent_1.BaseComponent));
exports.NewAPInvoiceComponent = NewAPInvoiceComponent;
//# sourceMappingURL=NewAPInvoiceComponent.js.map