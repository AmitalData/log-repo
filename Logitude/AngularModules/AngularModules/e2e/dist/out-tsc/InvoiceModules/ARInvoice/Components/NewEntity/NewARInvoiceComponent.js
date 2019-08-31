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
var ARInvoiceLinePM_1 = require("../../../../Invoice/EntityPMs/ARInvoiceLinePM");
var ARInvoiceTotalVATPM_1 = require("../../../../Invoice/EntityPMs/ARInvoiceTotalVATPM");
var ARInvoicePMService_1 = require("../../../../Invoice/Services/StandardPMs/ARInvoicePMService");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var CurrencyRatesService_1 = require("../../../../Common/Services/CurrencyRatesService");
var Tools_2 = require("../../../../Invoice/Tools");
var Args_1 = require("../../../../Invoice/Args");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var PaymentTermListService_1 = require("../../../../Common/Services/StandardLists/PaymentTermListService");
var VatTypeListService_1 = require("../../../../Common/Services/StandardLists/VatTypeListService");
var ChargesTypeListService_1 = require("../../../../Common/Services/StandardLists/ChargesTypeListService");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var InvoiceDomainService_1 = require("../../../../Invoice/Services/InvoiceDomainService");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var NewARInvoiceComponent = /** @class */ (function (_super) {
    __extends(NewARInvoiceComponent, _super);
    function NewARInvoiceComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.ObjectTableName = "ARInvoice";
        _this.DataContext = _this;
        _this.InvoicePartners = [];
        _this.ValidationErrorsList = [];
        _this.IsResourcesReady = false;
        _this.IsEditExchangeRateVisible = false;
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.DisplaySATSettings = false;
        _this.IsIntercompanyVisible = false;
        _this.AllVatTypes = [];
        _this.InvoiceTypeCode = null;
        _this.EntityLevelCode = null;
        _this.EntityTableName = null;
        _this.EntityReceivables = [];
        // SetUIProperties
        _this.RateIsEnabled = false;
        _this.PaymentTermDisplayInLOV = true;
        _this.IsConstituentInvoiceVisible = true;
        // BillTo
        _this.BillToDependencyValue1 = null;
        _this.BillToDependencyValue1IsList = true;
        _this.SelectedPartnerType = null;
        _this.myRelativeRateDate = null;
        // Load Date 
        _this.LastRatesList = [];
        _this.VatTypePercentagesList = [];
        // CreditLimit
        _this.HasCreditLimitFeature = false;
        _this.HasCreditOverrideFeature = false;
        _this.IsCreditLimitActivated = false;
        _this.IsCreditLimitHasAction = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.InitializeServices();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession(_this.ObjectTableName, "Intercompany")) {
            _this.IsIntercompanyVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            _this.IsEditExchangeRateVisible = true;
        }
        return _this;
    }
    NewARInvoiceComponent.prototype.InitializeServices = function () {
        var _this = this;
        this.myCardListService = new CardListService_1.CardListService();
        this.myCurrencyListService = new CurrencyListService_1.CurrencyListService();
        this.myPaymentTermListService = new PaymentTermListService_1.PaymentTermListService();
        this.myVatTypeListService = new VatTypeListService_1.VatTypeListService();
        this.myChargesTypeListService = new ChargesTypeListService_1.ChargesTypeListService();
        this.myCommonDomainService = new CommonDomainService_1.CommonDomainService();
        this.myInvoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        this.myEntityPMService = new ARInvoicePMService_1.ARInvoicePMService();
        this.myVatTypeListService.getAllFromCache().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.AllVatTypes = myResponse.Result;
            }
        });
    };
    NewARInvoiceComponent.prototype.SetWindowArgs = function (myarguments) {
        var _this = this;
        this.shipmentPM = myarguments["Shipment"];
        this.InvoiceTypeCode = myarguments["InvoiceTypeCode"];
        this.EntityLevelCode = myarguments["EntityLevelCode"];
        this.EntityTableName = myarguments["EntityTableName"];
        this.EntityReceivables = myarguments["EntityReceivables"];
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            _this.EntityPM = _this.myEntityPMService.GetNewEntityPM();
            _this.EntityPM.ARInvoiceTypeCode = _this.InvoiceTypeCode;
            _this.EntityPM.PrepaidCollectId = _this.InvoiceTypeCode == "MN" ? "C" : "B";
            _this.EntityPM.IsCustomsChargesOnly = (_this.InvoiceTypeCode == "CI" || _this.InvoiceTypeCode == "CC") ? true : false;
            _this.EntityPM.MainEntityId = _this.shipmentPM.Id;
            _this.EntityPM.MainEntityReference = _this.shipmentPM.ShipmentNumber;
            _this.EntityPM.HouseNumber = _this.shipmentPM.House;
            _this.EntityPM.MasterNumber = _this.shipmentPM.LongMaster;
            _this.EntityPM.ProfitCurrencyId = _this.shipmentPM.ProfitCurrencyId;
            _this.EntityPM.OperationalDate = Tools_2.InvoiceTool.GetOperationalDate(_this.shipmentPM);
            _this.EntityPM.BranchId = _this.shipmentPM.BranchId;
            var myDescription = null;
            switch (_this.shipmentPM.DirectionId) {
                case "E": {
                    myDescription = "Export to " + _this.shipmentPM.MainCarriageFinalDestinationPortCode;
                    break;
                }
                case "I": {
                    myDescription = "Import from " + _this.shipmentPM.MainCarriageFromPortCode;
                    break;
                }
                case "D": {
                    myDescription = "Ship to " + _this.shipmentPM.ToPartnerCity;
                    break;
                }
            }
            _this.EntityPM.Description = myDescription;
            if (SessionLocator_1.SessionLocator.TenantPM.AccountingActivated) {
                _this.EntityPM.IsFullAccounting = true;
            }
            if (_this.InvoiceTypeCode == "MN" || _this.shipmentPM.ShipmentLevelCode == "C") {
                _this.EntityTableName = "Master";
            }
            else {
                _this.EntityTableName = "Shipment";
            }
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
    NewARInvoiceComponent.prototype.SetUIProperties = function () {
        this.SetUIProperties_BillTo();
        this.SetUIProperties_VatNumber();
        this.SetUIProperties_ExchangeRate();
        this.SetUIProperties_Constituent(false);
        this.SetUIProperties_DueDate();
        this.SetUIProperties_Payment();
    };
    NewARInvoiceComponent.prototype.SetUIProperties_BillTo = function () {
        var isBillToEnabled = false;
        var isBillToAddressEnabled = false;
        if (this.SelectedPartnerType) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.BillToPartnerTypeId)) {
                if (this.SelectedPartnerType.Code == "OTH") {
                    isBillToEnabled = true;
                }
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.BillToId)) {
            isBillToAddressEnabled = true;
        }
        this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, isBillToEnabled);
        this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, isBillToAddressEnabled);
    };
    NewARInvoiceComponent.prototype.SetUIProperties_VatNumber = function () {
        var isFieldRequired = false;
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAR) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.VatNumber)) {
                isFieldRequired = true;
            }
            this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, isFieldRequired);
        }
    };
    NewARInvoiceComponent.prototype.SetUIProperties_ExchangeRate = function () {
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
    NewARInvoiceComponent.prototype.SetUIProperties_Constituent = function (isBillToHasConstituentEnabled) {
        var isFieldVisible = false;
        var isFieldtEnabled = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "Consolidation.Constituent")) {
            isFieldVisible = true;
            isFieldtEnabled = true;
            if (this.EntityPM.ARInvoiceTypeCode == "CI" || this.EntityPM.ARInvoiceTypeCode == "CC") {
                isFieldVisible = false;
            }
            //isFieldtEnabled = isBillToHasConstituentEnabled;
            if (Tools_1.AppTool.IsNullOrEmpty(this.BillToId)) {
                isFieldtEnabled = false;
            }
        }
        this.IsConstituentInvoiceVisible = isFieldVisible;
        this.UIProperties.SetEnabled("IsConstituentInvoice", this.ObjectTableName, isFieldtEnabled);
        //this.UIProperties.SetVisibility("IsConstituentInvoice", this.ObjectTableName, isFieldVisible);
    };
    NewARInvoiceComponent.prototype.SetUIProperties_DueDate = function () {
        var AllowManuallyDueDate = false;
        if (SessionLocator_1.SessionLocator.AccountingSystemPM) {
            AllowManuallyDueDate = SessionLocator_1.SessionLocator.AccountingSystemPM.AllowManuallyDueDate;
        }
        this.UIProperties.SetEnabled("DueDate", this.ObjectTableName, AllowManuallyDueDate);
        if (AllowManuallyDueDate) {
            this.PaymentTermDisplayInLOV = null;
        }
    };
    NewARInvoiceComponent.prototype.SetUIProperties_Payment = function () {
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
    NewARInvoiceComponent.prototype.BuildPartnersTypes = function () {
        this.BillToDependencyValue1 = Tools_2.InvoiceTool.GetBillToPartnerTypes();
        this.InvoicePartners = Tools_2.InvoiceTool.GetARInvoicePartners(this.shipmentPM);
        if (this.EntityPM.Id == null) {
            if (this.EntityTableName == "Shipment") {
                this.PartnersTypeSelectionMethod(this.InvoicePartners.filter(function (d) { return d.Code == "CUS"; })[0]);
            }
            else if (this.EntityTableName == "Master") {
                this.PartnersTypeSelectionMethod(this.InvoicePartners.filter(function (d) { return d.Code == "AGE"; })[0]);
            }
        }
    };
    NewARInvoiceComponent.prototype.PartnersTypeSelectionMethod = function (selected) {
        if (this.SelectedPartnerType != selected) {
            this.SelectedPartnerType = selected;
            this.BillToId = null;
            this.BillToAddressId = null;
            this.BillToPartnerTypeId = null;
            var myReference = null;
            if (selected) {
                this.BillToPartnerTypeId = selected.PartnerTypeId;
                switch (selected.Code) {
                    case "CUS":
                        {
                            this.BillToId = this.shipmentPM.CustomerId;
                            if (!Tools_1.AppTool.IsNullOrEmpty(this.shipmentPM.CustomerReference1)) {
                                myReference = this.shipmentPM.CustomerReference1;
                            }
                            if (!Tools_1.AppTool.IsNullOrEmpty(this.shipmentPM.CustomerReference2)) {
                                myReference = Tools_1.AppTool.IsNullOrEmpty(myReference) ? this.shipmentPM.CustomerReference2 : myReference + "," + this.shipmentPM.CustomerReference2;
                            }
                            break;
                        }
                    case "SHI":
                        {
                            this.BillToId = this.shipmentPM.ShipperId;
                            if (!Tools_1.AppTool.IsNullOrEmpty(this.shipmentPM.ShipperReference1)) {
                                myReference = this.shipmentPM.ShipperReference1;
                            }
                            if (!Tools_1.AppTool.IsNullOrEmpty(this.shipmentPM.ShipperReference2)) {
                                myReference = Tools_1.AppTool.IsNullOrEmpty(myReference) ? this.shipmentPM.ShipperReference2 : myReference + "," + this.shipmentPM.ShipperReference2;
                            }
                            break;
                        }
                    case "CON":
                        {
                            this.BillToId = this.shipmentPM.ConsigneeId;
                            if (!Tools_1.AppTool.IsNullOrEmpty(this.shipmentPM.ConsigneeReference1)) {
                                myReference = this.shipmentPM.ConsigneeReference1;
                            }
                            if (!Tools_1.AppTool.IsNullOrEmpty(this.shipmentPM.ConsigneeReference2)) {
                                myReference = Tools_1.AppTool.IsNullOrEmpty(myReference) ? this.shipmentPM.ConsigneeReference2 : myReference + "," + this.shipmentPM.ConsigneeReference2;
                            }
                            break;
                        }
                    case "AGE":
                        {
                            this.BillToId = this.shipmentPM.AgentId;
                            if (!Tools_1.AppTool.IsNullOrEmpty(this.shipmentPM.AgentReference1)) {
                                myReference = this.shipmentPM.AgentReference1;
                            }
                            if (!Tools_1.AppTool.IsNullOrEmpty(this.shipmentPM.AgentReference2)) {
                                myReference = Tools_1.AppTool.IsNullOrEmpty(myReference) ? this.shipmentPM.AgentReference2 : myReference + "," + this.shipmentPM.AgentReference2;
                            }
                            break;
                        }
                    case "CGE": {
                        this.BillToId = this.shipmentPM.CustomAgentExportId;
                        myReference = this.shipmentPM.CustomAgentExportReference;
                        break;
                    }
                    case "CGI": {
                        this.BillToId = this.shipmentPM.CustomAgentImportId;
                        myReference = this.shipmentPM.CustomAgentImportReference;
                        break;
                    }
                    case "NOT1": {
                        this.BillToId = this.shipmentPM.Notify1Id;
                        break;
                    }
                    case "NOT2": {
                        this.BillToId = this.shipmentPM.Notify2Id;
                        break;
                    }
                    case "SNE": {
                        this.BillToId = this.shipmentPM.ShipperNotExporterId;
                        myReference = this.shipmentPM.ShipperReference1;
                        break;
                    }
                    case "CNI": {
                        this.BillToId = this.shipmentPM.ConsigneeNotImporterId;
                        myReference = this.shipmentPM.ConsigneeReference1;
                        break;
                    }
                    case "FFW": {
                        this.BillToId = this.shipmentPM.FreightForwarderId;
                        myReference = this.shipmentPM.FreightForwarderReference;
                        break;
                    }
                    case "DTR": {
                        this.BillToId = this.shipmentPM.ConsolidatorId;
                        myReference = this.shipmentPM.ConsolidatorReference;
                        break;
                    }
                    case "OTH": {
                        this.BillToId = null;
                        break;
                    }
                    case "AL": {
                        this.BillToId = this.shipmentPM.MainCarriageCarrierId;
                        break;
                    }
                    case "SL": {
                        this.BillToId = this.shipmentPM.MainCarriageCarrierId;
                        break;
                    }
                    case "TR": {
                        this.BillToId = this.shipmentPM.MainCarriageCarrierId;
                        break;
                    }
                    default: {
                        break;
                    }
                }
            }
            this.CustomerRef = myReference;
            this.SetUIProperties();
        }
    };
    Object.defineProperty(NewARInvoiceComponent.prototype, "BillToPartnerTypeId", {
        get: function () { return this.EntityPM.BillToPartnerTypeId; },
        set: function (newValue) {
            if (this.EntityPM.BillToPartnerTypeId != newValue) {
                this.EntityPM.BillToPartnerTypeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARInvoiceComponent.prototype, "BillToId", {
        get: function () { return this.EntityPM.BillToId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.BillToId != newValue) {
                this.EntityPM.BillToId = newValue;
                this.SetUIProperties_BillTo();
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.VatTypeId = null;
                    this.VatNumber = null;
                    this.BillToName = null;
                    this.BillToAddressId = null;
                    this.SATPaymentMethodCode = null;
                    this.UsoCFDICode = null;
                    this.InvoiceCurrencyId = SessionLocator_1.SessionLocator.TenantPM.CurrencyId;
                    this.PaymentTermId = SessionLocator_1.SessionLocator.TenantPM.PaymentTermId;
                    this.IsConstituentInvoice = false;
                    this.SetUIProperties_Constituent(this.IsConstituentInvoice);
                    this.EntityPM.BillToIsCreditLimitEnabled = false;
                    this.EntityPM.BillToCreditLimitAmount = null;
                    this.EntityPM.BillToCreditLimitOpenBalance = null;
                    this.EntityPM.BillToCreditLimitWarningPercentage = null;
                    this.EntityPM.BillToBlockNewInvoiceCreation = false;
                    if (SessionLocator_1.SessionLocator.SATInterfaceSettings) {
                        this.MetodoPagoCode = SessionLocator_1.SessionLocator.SATInterfaceSettings.MetodoPagoCode;
                    }
                }
                else {
                    this.myCardListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.VatTypeId = list.VatTypeId;
                                _this.VatNumber = list.VatNumber;
                                _this.BillToName = list.EnglishName;
                                if (_this.EntityPM.ARInvoiceTypeCode != "CI" && _this.EntityPM.ARInvoiceTypeCode != "CC") {
                                    _this.IsConstituentInvoice = list.EnableConsolidationInvoices;
                                }
                                _this.EntityPM.BillToIsCreditLimitEnabled = list.IsCreditLimitEnabled;
                                _this.EntityPM.BillToCreditLimitAmount = list.CreditLimitAmount;
                                _this.EntityPM.BillToCreditLimitOpenBalance = list.CreditLimitOpenBalance;
                                _this.EntityPM.BillToCreditLimitWarningPercentage = list.CreditLimitWarningPercentage;
                                _this.EntityPM.BillToBlockNewInvoiceCreation = list.BlockNewInvoiceCreation;
                                _this.EntityPM.SalesmanUserId = list.SalesmanUserId;
                                if (!Tools_1.AppTool.IsNullOrEmpty(list.SATPaymentMethodCode)) {
                                    _this.SATPaymentMethodCode = list.SATPaymentMethodCode;
                                }
                                if (!Tools_1.AppTool.IsNullOrEmpty(list.MetodoPagoCode)) {
                                    _this.MetodoPagoCode = list.MetodoPagoCode;
                                }
                                else if (SessionLocator_1.SessionLocator.SATInterfaceSettings != null && !Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.SATInterfaceSettings.MetodoPagoCode)) {
                                    _this.MetodoPagoCode = SessionLocator_1.SessionLocator.SATInterfaceSettings.MetodoPagoCode;
                                }
                                //if (!AppTool.IsNullOrEmpty(list.UsoCFDICode)) {
                                _this.UsoCFDICode = list.UsoCFDICode;
                                //}
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
                                _this.SetUIProperties_Constituent(_this.IsConstituentInvoice);
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARInvoiceComponent.prototype, "BillToName", {
        get: function () { return this.EntityPM.BillToName; },
        set: function (newValue) {
            if (this.EntityPM.BillToName != newValue) {
                this.EntityPM.BillToName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARInvoiceComponent.prototype, "BillToAddressId", {
        get: function () { return this.EntityPM.BillToAddressId; },
        set: function (newValue) {
            if (this.EntityPM.BillToAddressId != newValue) {
                this.EntityPM.BillToAddressId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARInvoiceComponent.prototype, "InvoiceCurrencyId", {
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
    Object.defineProperty(NewARInvoiceComponent.prototype, "InvoiceCurrencyCode", {
        get: function () { return this.EntityPM.InvoiceCurrencyCode; },
        set: function (value) {
            if (this.EntityPM.InvoiceCurrencyCode != value) {
                this.EntityPM.InvoiceCurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARInvoiceComponent.prototype, "InvoiceCurrencyExchangeRate", {
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
    Object.defineProperty(NewARInvoiceComponent.prototype, "ExchangeRateDate", {
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
    Object.defineProperty(NewARInvoiceComponent.prototype, "RelativeRateDate", {
        get: function () { return this.myRelativeRateDate; },
        set: function (value) {
            if (this.myRelativeRateDate != value) {
                this.myRelativeRateDate = value;
                this.SetUIProperties_BillTo();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewARInvoiceComponent.prototype.ComputeRelativeRateDate = function () {
        this.RelativeRateDate = Tools_1.DateTool.GetRelativeRateDate(this.InvoiceDate, this.ExchangeRateDate, "old");
    };
    Object.defineProperty(NewARInvoiceComponent.prototype, "VatTypeId", {
        get: function () { return this.vatTypeId; },
        set: function (newValue) {
            if (this.vatTypeId != newValue) {
                this.vatTypeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARInvoiceComponent.prototype, "VatNumber", {
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
    Object.defineProperty(NewARInvoiceComponent.prototype, "PaymentTermId", {
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
    Object.defineProperty(NewARInvoiceComponent.prototype, "PaymentTermName", {
        get: function () { return this.EntityPM.PaymentTermName; },
        set: function (newValue) {
            if (this.EntityPM.PaymentTermName != newValue) {
                this.EntityPM.PaymentTermName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARInvoiceComponent.prototype, "InvoiceDate", {
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
    Object.defineProperty(NewARInvoiceComponent.prototype, "DueDate", {
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
    Object.defineProperty(NewARInvoiceComponent.prototype, "CustomerRef", {
        get: function () { return this.EntityPM.CustomerRef; },
        set: function (newValue) {
            if (this.EntityPM.CustomerRef != newValue) {
                this.EntityPM.CustomerRef = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARInvoiceComponent.prototype, "IsConstituentInvoice", {
        get: function () { return this.EntityPM.IsConstituentInvoice; },
        set: function (newValue) {
            if (this.EntityPM.IsConstituentInvoice != newValue) {
                this.EntityPM.IsConstituentInvoice = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARInvoiceComponent.prototype, "Intercompany", {
        get: function () { return this.EntityPM.Intercompany; },
        set: function (newValue) {
            if (this.EntityPM.Intercompany != newValue) {
                this.EntityPM.Intercompany = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARInvoiceComponent.prototype, "SATPaymentMethodCode", {
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
    Object.defineProperty(NewARInvoiceComponent.prototype, "MetodoPagoCode", {
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
    Object.defineProperty(NewARInvoiceComponent.prototype, "UsoCFDICode", {
        get: function () { return this.EntityPM.UsoCFDICode; },
        set: function (newValue) {
            if (this.EntityPM) {
                if (this.EntityPM.UsoCFDICode != newValue) {
                    this.EntityPM.UsoCFDICode = newValue;
                    //this.SetUIProperties_Payment();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    NewARInvoiceComponent.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        if (this.myCurrencyRatesService == null) {
            this.myCurrencyRatesService = new CurrencyRatesService_1.CurrencyRatesService();
        }
        var loadingDate = this.EntityPM.InvoiceDate;
        if (loadingDate == null) {
            loadingDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
        this.myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator_1.SessionLocator.LocalCurrencyId, loadingDate).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.LastRatesList = myResponse.Result;
                _this.SetCurrencyRateData();
                _this.myCommonDomainService.GetVatTypePercentagePMByDate(loadingDate).subscribe(function (myResponse2) {
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
    NewARInvoiceComponent.prototype.SetCurrencyRateData = function () {
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
    NewARInvoiceComponent.prototype.GetCurrencyRate = function (currencyId) {
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
    NewARInvoiceComponent.prototype.GetCurrencyRateDate = function (currencyId) {
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
    NewARInvoiceComponent.prototype.GetVatTypePercentage = function (vatTypeId) {
        var myResult = null;
        var vatTypePercentagePM = this.VatTypePercentagesList.filter(function (d) { return d.VatTypeId == vatTypeId; })[0];
        if (vatTypePercentagePM != null) {
            myResult = vatTypePercentagePM.Percentage;
        }
        return myResult;
    };
    NewARInvoiceComponent.prototype.UpdateCurrencyRateClicked = function () {
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
    NewARInvoiceComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewARInvoiceComponent.prototype.OkButtonClicked = function () {
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
        }
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            //if (AppTool.IsNullOrEmpty(this.SATPaymentMethodCode)) {
            //  errors.push(msg.replace("%FieldName", "Forma Pago"));
            //}
            if (Tools_1.AppTool.IsNullOrEmpty(this.MetodoPagoCode)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.F.MetodoPagoCode")));
            }
            if (this.MetodoPagoCode == "PUE" && this.SATPaymentMethodCode == "99") {
                errors.push("Since the metodo pago was set as PUE, you can't select Por definir (99). Please choose another value for the forma Pago.");
            }
        }
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
    NewARInvoiceComponent.prototype.ValidateFullAccounting = function () {
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
    NewARInvoiceComponent.prototype.ValidateCreditLimit = function () {
        var _this = this;
        if (this.EntityPM.BillToBlockNewInvoiceCreation) {
            var errorText_Blocking = "Credit limit setting is blocking invoice for bill to: " + this.EntityPM.BillToName;
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
                        var LimitWarning = "The customer exceeded the credit limit available.";
                        LimitError += "Bill To exceeded its credit limit of " + Tools_1.FormatTool.FormatNumber(LimitAmount) + " (" + _this.EntityPM.LocalCurrencyCode + ").";
                        LimitError += " ";
                        LimitError += "The current balance stands on " + Tools_1.FormatTool.FormatNumber(ActualBalance) + " (" + _this.EntityPM.LocalCurrencyCode + ").";
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
                            var PercentageWarning = "The remaining credit limit for this customer is (" + RemainingLimit + ")";
                            warnings.push(PercentageWarning);
                        }
                    }
                    if (errors.length > 0 || warnings.length > 0) {
                        var isBlockingShipment = false;
                        if (errors.length > 0) {
                            if (!_this.shipmentPM.IsNewARInvoiceBlocked) {
                                _this.shipmentPM.IsNewARInvoiceBlocked = true;
                                _this.shipmentPM.IsDirty = false;
                                isBlockingShipment = true;
                            }
                        }
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Width = 450;
                        logWindow.Height = 200;
                        logWindow.Title = "Credit limit";
                        logWindow.WindowArgs = { Errors: errors, Warnings: warnings, IsBlockingShipment: isBlockingShipment, ShipmentId: _this.shipmentPM.Id };
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
    NewARInvoiceComponent.prototype.OnEntityValid = function () {
        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "Consolidation.Constituent")) {
            this.IsConstituentInvoice = false;
        }
        this.CurrentSession.StartBusyIndicatorLoading();
        this.InitializeComponent();
    };
    NewARInvoiceComponent.prototype.InitializeComponent = function () {
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
        if (this.IsConstituentInvoice) {
            this.EntityPM.StatusCode = "NT";
            this.EntityPM.StatusName = "Not Connected";
        }
        else {
            this.EntityPM.StatusCode = "DR";
            this.EntityPM.StatusName = "Draft";
        }
        this.BuildOpenAmounts();
    };
    NewARInvoiceComponent.prototype.BuildOpenAmounts = function () {
        var _this = this;
        var filteredReceivables = this.EntityReceivables.filter(function (d) { return d.ShipmentReceivableParentId == null; });
        if (this.EntityPM.ARInvoiceTypeCode != "MN") {
            filteredReceivables = filteredReceivables.filter(function (f) { return f.ShipmentReceivableLineStatusCode == "OAMT" && f.Quantity != null && f.UnitPrice != null && f.ARInvoiceId == null && f.ARInvoiceLineId == null; });
            switch (this.EntityPM.ARInvoiceTypeCode) {
                case "IN":
                case "CI":
                    {
                        if (!SessionLocator_1.SessionLocator.AccountingSettingPM.AllowMinusInvoicelines) {
                            filteredReceivables = filteredReceivables.filter(function (f) { return f.UnitPrice > 0; });
                        }
                        break;
                    }
                case "CD":
                case "CC":
                    {
                        if (!SessionLocator_1.SessionLocator.AccountingSettingPM.AllowPositiveAmountsInTheCreditNote) {
                            filteredReceivables = filteredReceivables.filter(function (f) { return f.UnitPrice < 0; });
                        }
                        break;
                    }
            }
            if (this.EntityPM.PrepaidCollectId != null && this.EntityPM.PrepaidCollectId != "B") {
                filteredReceivables = filteredReceivables.filter(function (f) { return f.PrepaidCollectId == _this.EntityPM.PrepaidCollectId; });
            }
        }
        this.BuildInvoiceLines(filteredReceivables);
    };
    NewARInvoiceComponent.prototype.BuildInvoiceLines = function (filteredReceivables) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            filteredReceivables.forEach(function (receivable) {
                var invoiceLine = new ARInvoiceLinePM_1.ARInvoiceLinePM(_this.EntityPM);
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
                _this.myChargesTypeListService.getSingleFromCache(invoiceLine.ChargesTypeId).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        _this.SetInvoiceLineVatType(list, receivable, invoiceLine);
                        if (list != null) {
                            invoiceLine.Description = list.EnglishName;
                            invoiceLine.LocalDescription = list.LocalName;
                            invoiceLine.IsCustomsCharge = list.IsCustoms;
                        }
                    }
                });
                _this.EntityPM.AddARInvoiceLinePM(invoiceLine);
            });
        }
        this.BuildTotalVATs();
        this.ComputeTotals();
        this.CurrentSession.CloseCurrentWindowEmit("Ok");
    };
    NewARInvoiceComponent.prototype.SetInvoiceLineVatType = function (list, myReceivable, invoiceLine) {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.VatTypeId)) {
            invoiceLine.VatTypeId = this.VatTypeId;
        }
        else if (myReceivable.IsFromQuote && !Tools_1.AppTool.IsNullOrEmpty(myReceivable.VatTypeId)) {
            invoiceLine.VatTypeId = myReceivable.VatTypeId;
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
    NewARInvoiceComponent.prototype.BuildTotalVATs = function () {
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
    NewARInvoiceComponent.prototype.ComputeTotals = function () {
        this.EntityPM.SubTotalInLocalCurrency = Tools_1.AppTool.Round(Tools_1.ArrayTool.Sum(this.EntityPM.InvoiceLines, "LocalCurrencyAmount"), 2);
        this.EntityPM.SubTotalInInvoiceCurrency = Tools_1.AppTool.Round(Tools_1.ArrayTool.Sum(this.EntityPM.InvoiceLines, "InvoiceCurrencyAmount"), 2);
        this.EntityPM.AmountInLocalCurrency = Tools_1.AppTool.Round(this.EntityPM.SubTotalInLocalCurrency + Tools_1.ArrayTool.Sum(this.EntityPM.TotalVATs, "LocalVATAmount"), 2);
        this.EntityPM.AmountInInvoiceCurrency = Tools_1.AppTool.Round(this.EntityPM.SubTotalInLocalCurrency + Tools_1.ArrayTool.Sum(this.EntityPM.TotalVATs, "InvoiceCurrencyVATAmount"), 2);
        if (this.EntityPM.ProfitCurrencyId == SessionLocator_1.SessionLocator.LocalCurrencyId) {
            this.EntityPM.AmountInProfitCurrency = this.EntityPM.AmountInLocalCurrency;
        }
        else if (this.EntityPM.ProfitCurrencyId == this.EntityPM.InvoiceCurrencyId) {
            this.EntityPM.AmountInProfitCurrency = this.EntityPM.AmountInInvoiceCurrency;
        }
        else {
            if (this.EntityPM.ProfitCurrencyExchangeRate != 0) {
                this.EntityPM.AmountInProfitCurrency = Tools_1.AppTool.Round(this.EntityPM.AmountInLocalCurrency / this.EntityPM.ProfitCurrencyExchangeRate, 2);
            }
        }
    };
    NewARInvoiceComponent.prototype.UpdateCreditLimitFlags = function () {
        this.HasCreditLimitFeature = FeatureLocator_1.FeatureLocator.HasFeaturePermession("CreditLimitSetting", "Module");
        this.HasCreditOverrideFeature = FeatureLocator_1.FeatureLocator.HasFeaturePermession("CreditLimitSetting", "Override");
        this.EntityPM.HasCreditLimitOverrideFeature = this.HasCreditOverrideFeature;
        if (this.HasCreditLimitFeature) {
            this.IsCreditLimitActivated = ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.IsCreditLimitEnabled;
            this.IsCreditLimitHasAction = (ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.InvoiceCreationBlock == true || ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.InvoiceCreationWarning == true) ? true : false;
        }
    };
    NewARInvoiceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewARInvoiceComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], NewARInvoiceComponent);
    return NewARInvoiceComponent;
}(BaseComponent_1.BaseComponent));
exports.NewARInvoiceComponent = NewARInvoiceComponent;
//# sourceMappingURL=NewARInvoiceComponent.js.map