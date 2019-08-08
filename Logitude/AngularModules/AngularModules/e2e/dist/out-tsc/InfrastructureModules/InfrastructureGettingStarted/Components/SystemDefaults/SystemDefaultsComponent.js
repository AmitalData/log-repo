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
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TenantPM_1 = require("../../../../Common/EntityPMs/TenantPM");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var TenantPMService_1 = require("../../../../Common/Services/StandardPMs/TenantPMService");
var PaymentTermListService_1 = require("../../../../Common/Services/StandardLists/PaymentTermListService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var ServiceLocator_1 = require("../../../../Infrastructure/Locators/ServiceLocator");
var SystemDefaultsComponent = /** @class */ (function (_super) {
    __extends(SystemDefaultsComponent, _super);
    function SystemDefaultsComponent(_entityResourceService) {
        var _this = _super.call(this) || this;
        _this._entityResourceService = _entityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "Tenant";
        _this.TenantPm = new TenantPM_1.TenantPM();
        _this.IsVisibile = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // Load Tenant 
        _this.IsTenantUS = false;
        _this.DimensionsDependencyProperty1 = null;
        _this.DimensionsDependencyProperty1IsList = false;
        // Cach Lists 
        _this.PaymentTermList = [];
        _this.iVatMandatoryForPotentialCustomers = false;
        _this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(function (response) {
            _this.LoadTenantPMMethod();
        });
        return _this;
    }
    SystemDefaultsComponent.prototype.LoadTenantPMMethod = function () {
        var _this = this;
        var myService = new TenantPMService_1.TenantPMService();
        myService.get(SessionLocator_1.SessionLocator.TenantPM.Id).subscribe(function (response) {
            _this.TenantPm = response.Result;
            _this.LoadCachedLists();
            _this.IsVisibile = true;
            if (_this.TenantPm.CountryCode.toUpperCase() == "US") {
                _this.IsTenantUS = true;
            }
        });
    };
    SystemDefaultsComponent.prototype.SetUIProperties = function () {
        this.SetUIProperties_VatUnique();
        this.SetUIProperties_VatMandatory();
        this.SetUIProperties_DemoAgent();
        this.SetUIProperties_DimensionsUnitCode();
        this.UIProperties.SetEnabled("RegulatedAgentNumber", "Tenant", this.RegulatedAgentRegimeActivated);
        if (this.TenantPm.IsDocumentsArchive) {
            this.UIProperties.SetVisibility("CustomerId", "Tenant", true);
            this.UIProperties.SetVisibility("AgentId", "Tenant", false);
        }
        else {
            this.UIProperties.SetVisibility("CustomerId", "Tenant", false);
            this.UIProperties.SetVisibility("AgentId", "Tenant", true);
        }
        if (this.TenantPm.Id == 65 && SessionLocator_1.SessionLocator.LoggedUserPM.Email.toLowerCase() != "customercare@logitudeworld.com‏") {
            this.SetUIPropertiesHitVisible();
        }
    };
    SystemDefaultsComponent.prototype.SetUIPropertiesHitVisible = function () {
        this.UIProperties.SetEnabled("ExportFreightPrepaidCollectId", "Tenant", false);
        this.UIProperties.SetEnabled("ImportFreightPrepaidCollectId", "Tenant", false);
        this.UIProperties.SetEnabled("ExportOtherPrepaidCollectId", "Tenant", false);
        this.UIProperties.SetEnabled("ImportOtherPrepaidCollectId", "Tenant", false);
        this.UIProperties.SetEnabled("MasterExportFreightPrepaidCollectId", "Tenant", false);
        this.UIProperties.SetEnabled("MasterImportFreightPrepaidCollectId", "Tenant", false);
        this.UIProperties.SetEnabled("MasterExportOtherPrepaidCollectId", "Tenant", false);
        this.UIProperties.SetEnabled("MasterImportOtherPrepaidCollectId", "Tenant", false);
        this.UIProperties.SetEnabled("VatMandatoryTypeCode", "Tenant", false);
        this.UIProperties.SetEnabled("VatMandatoryCountryId", "Tenant", false);
        this.UIProperties.SetEnabled("VatMandatoryForPotentialCustomers", "Tenant", false);
        this.IsVatMandatoryForPotentialCustomers = false;
        this.UIProperties.SetEnabled("VatUniqueTypeCode", "Tenant", false);
        this.UIProperties.SetEnabled("VatUniqueCountryId", "Tenant", false);
        this.UIProperties.SetEnabled("VolumeUnitCode", "Tenant", false);
        this.UIProperties.SetEnabled("GrossWeightUnitCode", "Tenant", false);
        this.UIProperties.SetEnabled("ChargeableWeightUnitCode", "Tenant", false);
        this.UIProperties.SetEnabled("WeightMeasurementUnitCode", "Tenant", false);
        this.UIProperties.SetEnabled("IATA", "Tenant", false);
        this.UIProperties.SetEnabled("CASSCode", "Tenant", false);
        this.UIProperties.SetEnabled("LocalCustomsCode", "Tenant", false);
        this.UIProperties.SetEnabled("AgentId", "Tenant", false);
        this.UIProperties.SetEnabled("CustomerId", "Tenant", false);
        this.UIProperties.SetEnabled("IsCustomerTenantShare", "Tenant", false);
        this.UIProperties.SetEnabled("CustomerTenantShareImportFile", "Tenant", false);
        this.UIProperties.SetEnabled("CustomerTenantShareExportFile", "Tenant", false);
        this.UIProperties.SetEnabled("RegulatedAgentRegimeActivated", "Tenant", false);
        this.UIProperties.SetEnabled("RegulatedAgentNumber", "Tenant", false);
    };
    SystemDefaultsComponent.prototype.SetUIProperties_DemoAgent = function () {
        if (this.TenantPm.Id == 65) {
            this.UIProperties.SetEnabled("AgentId", "Tenant", false);
        }
        else {
            this.UIProperties.SetEnabled("AgentId", "Tenant", true);
        }
    };
    SystemDefaultsComponent.prototype.SetUIProperties_DimensionsUnitCode = function () {
        var isEditingEnabled = true;
        var isFieldEnabled = false;
        if (this.TenantPm.Id == 65 && SessionLocator_1.SessionLocator.LoggedUserPM.Email.toLowerCase() != "customercare@logitudeworld.com‏") {
            isEditingEnabled = false;
        }
        if (isEditingEnabled && this.VolumeUnitCode == "CBF") {
            isFieldEnabled = true;
        }
        if (this.VolumeUnitCode == "CBF") {
            this.DimensionsDependencyProperty1 = "Ft,Inc";
            this.DimensionsDependencyProperty1IsList = true;
        }
        else {
            this.DimensionsDependencyProperty1 = null;
            this.DimensionsDependencyProperty1IsList = false;
        }
        //this.UIProperties.SetEnabled("DimensionsUnitCode", this.ObjectTableName, isFieldEnabled);
    };
    SystemDefaultsComponent.prototype.LoadCachedLists = function () {
        this.LoadPaymentTermListMethod();
    };
    SystemDefaultsComponent.prototype.LoadPaymentTermListMethod = function () {
        var _this = this;
        var myService = new PaymentTermListService_1.PaymentTermListService();
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.GetAll = true;
        myService.getAllFromCache(filters).subscribe(function (myResult) {
            _this.PaymentTermList = myResult;
            _this.SetUIProperties();
            _this.UIProperties.SetVisibility("IsCustomerTenantShare", "Tenant", FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "CUSTOMERTENANTACCESSES"));
            _this.UIProperties.SetVisibility("CustomerTenantShareImportFile", "Tenant", FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "CUSTOMERTENANTACCESSES"));
            _this.UIProperties.SetVisibility("CustomerTenantShareExportFile", "Tenant", FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "CUSTOMERTENANTACCESSES"));
        });
    };
    Object.defineProperty(SystemDefaultsComponent.prototype, "DemoMessageVisibility", {
        // region Tenant 65
        get: function () {
            var result = false;
            if (this.TenantPm.Id == 65) {
                result = true;
                if (SessionLocator_1.SessionLocator.LoggedUserPM.Email.toLowerCase() == "customercare@logitudeworld.com‏") {
                    result = false;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "ExportFreightPrepaidCollectId", {
        // Prepaid | Collect
        get: function () { return this.TenantPm.ExportFreightPrepaidCollectId; },
        set: function (value) {
            if (this.TenantPm.ExportFreightPrepaidCollectId != value) {
                this.TenantPm.ExportFreightPrepaidCollectId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "ImportFreightPrepaidCollectId", {
        get: function () { return this.TenantPm.ImportFreightPrepaidCollectId; },
        set: function (value) {
            if (this.TenantPm.ImportFreightPrepaidCollectId != value) {
                this.TenantPm.ImportFreightPrepaidCollectId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "ExportOtherPrepaidCollectId", {
        get: function () { return this.TenantPm.ExportOtherPrepaidCollectId; },
        set: function (value) {
            if (this.TenantPm.ExportOtherPrepaidCollectId != value) {
                this.TenantPm.ExportOtherPrepaidCollectId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "ImportOtherPrepaidCollectId", {
        get: function () { return this.TenantPm.ImportOtherPrepaidCollectId; },
        set: function (value) {
            if (this.TenantPm.ImportOtherPrepaidCollectId != value) {
                this.TenantPm.ImportOtherPrepaidCollectId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "MasterImportOtherPrepaidCollectId", {
        get: function () { return this.TenantPm.MasterImportOtherPrepaidCollectId; },
        set: function (value) {
            if (this.TenantPm.MasterImportOtherPrepaidCollectId != value) {
                this.TenantPm.MasterImportOtherPrepaidCollectId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "MasterExportFreightPrepaidCollectId", {
        get: function () { return this.TenantPm.MasterExportFreightPrepaidCollectId; },
        set: function (value) {
            if (this.TenantPm.MasterExportFreightPrepaidCollectId != value) {
                this.TenantPm.MasterExportFreightPrepaidCollectId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "MasterImportFreightPrepaidCollectId", {
        get: function () { return this.TenantPm.MasterImportFreightPrepaidCollectId; },
        set: function (value) {
            if (this.TenantPm.MasterImportFreightPrepaidCollectId != value) {
                this.TenantPm.MasterImportFreightPrepaidCollectId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "MasterExportOtherPrepaidCollectId", {
        get: function () { return this.TenantPm.MasterExportOtherPrepaidCollectId; },
        set: function (value) {
            if (this.TenantPm.MasterExportOtherPrepaidCollectId != value) {
                this.TenantPm.MasterExportOtherPrepaidCollectId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "VatUniqueTypeCode", {
        // VAT# is unique by country
        get: function () { return this.TenantPm.VatUniqueTypeCode; },
        set: function (value) {
            if (this.TenantPm.VatUniqueTypeCode != value) {
                this.TenantPm.VatUniqueTypeCode = value;
                this.SetUIProperties_VatUnique();
                this.VatUniqueCountryId = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "VatUniqueCountryId", {
        get: function () { return this.TenantPm.VatUniqueCountryId; },
        set: function (value) {
            if (this.TenantPm.VatUniqueCountryId != value) {
                this.TenantPm.VatUniqueCountryId = value;
                this.SetUIProperties_VatUnique();
            }
        },
        enumerable: true,
        configurable: true
    });
    SystemDefaultsComponent.prototype.SetUIProperties_VatUnique = function () {
        var isEnabled = false;
        var isRequired = false;
        if (this.VatUniqueTypeCode == "USC") {
            isEnabled = true;
            if (Tools_1.AppTool.IsNullOrEmpty(this.VatUniqueCountryId)) {
                isRequired = true;
            }
        }
        this.UIProperties.SetEnabled("VatUniqueCountryId", "Tenant", isEnabled);
        this.UIProperties.SetRequired("VatUniqueCountryId", "Tenant", isRequired);
    };
    Object.defineProperty(SystemDefaultsComponent.prototype, "VatMandatoryTypeCode", {
        // VAT# is mandatory for customers
        get: function () { return this.TenantPm.VatMandatoryTypeCode; },
        set: function (value) {
            if (this.TenantPm.VatMandatoryTypeCode != value) {
                this.TenantPm.VatMandatoryTypeCode = value;
                this.SetUIProperties_VatMandatory();
                this.VatMandatoryCountryId = null;
                if (value == "MNT") {
                    this.VatMandatoryForPotentialCustomers = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "VatMandatoryForPotentialCustomers", {
        get: function () { return this.TenantPm.VatMandatoryForPotentialCustomers; },
        set: function (value) {
            if (this.TenantPm.VatMandatoryForPotentialCustomers != value) {
                this.TenantPm.VatMandatoryForPotentialCustomers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "VatMandatoryCountryId", {
        get: function () { return this.TenantPm.VatMandatoryCountryId; },
        set: function (value) {
            if (this.TenantPm.VatMandatoryCountryId != value) {
                this.TenantPm.VatMandatoryCountryId = value;
                this.SetUIProperties_VatMandatory();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "IsVatMandatoryForPotentialCustomers", {
        get: function () {
            return this.iVatMandatoryForPotentialCustomers;
        },
        set: function (value) {
            this.iVatMandatoryForPotentialCustomers = value;
        },
        enumerable: true,
        configurable: true
    });
    SystemDefaultsComponent.prototype.SetUIProperties_VatMandatory = function () {
        var isEnabled = false;
        var isRequired = false;
        if (this.VatMandatoryTypeCode == "MSC") {
            isEnabled = true;
            if (Tools_1.AppTool.IsNullOrEmpty(this.VatMandatoryCountryId)) {
                isRequired = true;
            }
        }
        this.UIProperties.SetEnabled("VatMandatoryCountryId", "Tenant", isEnabled);
        this.UIProperties.SetEnabled("VatMandatoryForPotentialCustomers", "Tenant", this.VatMandatoryTypeCode != "MNT");
        this.IsVatMandatoryForPotentialCustomers = this.VatMandatoryTypeCode != "MNT";
        this.UIProperties.SetRequired("VatMandatoryCountryId", "Tenant", isRequired);
    };
    Object.defineProperty(SystemDefaultsComponent.prototype, "VolumeUnitCode", {
        // Default Units
        get: function () { return this.TenantPm.VolumeUnitCode; },
        set: function (value) {
            if (this.TenantPm.VolumeUnitCode != value) {
                this.TenantPm.VolumeUnitCode = value;
                this.SetUIProperties_DimensionsUnitCode();
                this.TenantPm.DimensionsUnitCode = Tools_1.AppTool.GetDimentionsCodeFromVolumeCode(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "DimensionsUnitCode", {
        get: function () { return this.TenantPm.DimensionsUnitCode; },
        set: function (value) {
            if (this.TenantPm.DimensionsUnitCode != value) {
                this.TenantPm.DimensionsUnitCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "GrossWeightUnitCode", {
        get: function () { return this.TenantPm.GrossWeightUnitCode; },
        set: function (value) {
            if (this.TenantPm.GrossWeightUnitCode != value) {
                this.TenantPm.GrossWeightUnitCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "ChargeableWeightUnitCode", {
        get: function () { return this.TenantPm.ChargeableWeightUnitCode; },
        set: function (value) {
            if (this.TenantPm.ChargeableWeightUnitCode != value) {
                this.TenantPm.ChargeableWeightUnitCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "WeightMeasurementUnitCode", {
        get: function () { return this.TenantPm.WeightMeasurementUnitCode; },
        set: function (value) {
            if (this.TenantPm.WeightMeasurementUnitCode != value) {
                this.TenantPm.WeightMeasurementUnitCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "TemperatureUnitCode", {
        get: function () { return this.TenantPm.TemperatureUnitCode; },
        set: function (value) {
            if (this.TenantPm.TemperatureUnitCode != value) {
                this.TenantPm.TemperatureUnitCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "PaymentTermId", {
        // Others 
        get: function () {
            var _this = this;
            if (this.TenantPm.PaymentTermId == null) {
                var list = this.PaymentTermList.filter(function (d) { return d.EnglishName == "Net 30" && d.Tenant == _this.TenantPm.Id; })[0];
                if (list != null) {
                    this.TenantPm.PaymentTermId = list.Id;
                }
            }
            return this.TenantPm.CASSCode;
        },
        set: function (value) {
            if (this.TenantPm.PaymentTermId != value) {
                this.TenantPm.PaymentTermId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "IATA", {
        get: function () { return this.TenantPm.IATA; },
        set: function (value) {
            if (this.TenantPm.IATA != value) {
                this.TenantPm.IATA = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "CASSCode", {
        get: function () { return this.TenantPm.CASSCode; },
        set: function (value) {
            if (this.TenantPm.CASSCode != value) {
                this.TenantPm.CASSCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "SCACCode", {
        get: function () { return this.TenantPm.SCACCode; },
        set: function (value) {
            if (this.TenantPm.SCACCode != value) {
                this.TenantPm.SCACCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "CBSA", {
        get: function () { return this.TenantPm.CBSA; },
        set: function (value) {
            if (this.TenantPm.CBSA != value) {
                this.TenantPm.CBSA = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "CAAT", {
        get: function () { return this.TenantPm.CAAT; },
        set: function (value) {
            if (this.TenantPm.CAAT != value) {
                this.TenantPm.CAAT = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "FMCNumber", {
        get: function () { return this.TenantPm.FMCNumber; },
        set: function (value) {
            if (this.TenantPm.FMCNumber != value) {
                this.TenantPm.FMCNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "LocalCustomsCode", {
        get: function () { return this.TenantPm.LocalCustomsCode; },
        set: function (value) {
            if (this.TenantPm.LocalCustomsCode != value) {
                this.TenantPm.LocalCustomsCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "VatNumber", {
        get: function () { return this.TenantPm.VatNumber; },
        set: function (value) {
            if (this.TenantPm.VatNumber != value) {
                this.TenantPm.VatNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "AgentId", {
        get: function () { return this.TenantPm.AgentId; },
        set: function (value) {
            if (this.TenantPm.AgentId != value) {
                this.TenantPm.AgentId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "CustomerId", {
        get: function () { return this.TenantPm.CustomerId; },
        set: function (value) {
            if (this.TenantPm.CustomerId != value) {
                this.TenantPm.CustomerId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "IsCustomerTenantShare", {
        get: function () { return this.TenantPm.IsCustomerTenantShare; },
        set: function (value) {
            if (this.TenantPm.IsCustomerTenantShare != value) {
                this.TenantPm.IsCustomerTenantShare = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "CustomerTenantShareImportFile", {
        get: function () { return this.TenantPm.CustomerTenantShareImportFile; },
        set: function (value) {
            if (this.TenantPm.CustomerTenantShareImportFile != value) {
                this.TenantPm.CustomerTenantShareImportFile = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "CustomerTenantShareExportFile", {
        get: function () { return this.TenantPm.CustomerTenantShareExportFile; },
        set: function (value) {
            if (this.TenantPm.CustomerTenantShareExportFile != value) {
                this.TenantPm.CustomerTenantShareExportFile = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "IsQuoteSubjectEdited", {
        get: function () { return this.TenantPm.IsQuoteSubjectEdited; },
        set: function (value) {
            if (this.TenantPm.IsQuoteSubjectEdited != value) {
                this.TenantPm.IsQuoteSubjectEdited = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "AllowAgentInCustomersLOV", {
        get: function () { return this.TenantPm.AllowAgentInCustomersLOV; },
        set: function (value) {
            if (this.TenantPm.AllowAgentInCustomersLOV != value) {
                this.TenantPm.AllowAgentInCustomersLOV = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "IsCorrespondenceRightToLeftEnabled", {
        get: function () { return this.TenantPm.IsCorrespondenceRightToLeftEnabled; },
        set: function (value) {
            this.TenantPm.IsCorrespondenceRightToLeftEnabled = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "RightToLeftVisible", {
        get: function () {
            var result = false;
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "TICKET")) {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "IsNotesRightToLeftEnabled", {
        get: function () { return this.TenantPm.IsNotesRightToLeftEnabled; },
        set: function (value) {
            this.TenantPm.IsNotesRightToLeftEnabled = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "NotesRightToLeftVisible", {
        get: function () {
            var result = false;
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "NotesRightToLeftEnabled")) {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "AllowAgentInCustomersLOVVisible", {
        get: function () {
            var result = false;
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "AllowAgentInCustomersLOV")) {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "RegulatedAgentRegimeActivated", {
        // Regulated Agent
        get: function () { return this.TenantPm.RegulatedAgentRegimeActivated; },
        set: function (value) {
            if (this.TenantPm.RegulatedAgentRegimeActivated != value) {
                this.TenantPm.RegulatedAgentRegimeActivated = value;
                this.UIProperties.SetEnabled("RegulatedAgentNumber", "Tenant", value);
                if (!value) {
                    this.RegulatedAgentNumber = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "RegulatedAgentNumber", {
        get: function () { return this.TenantPm.RegulatedAgentNumber; },
        set: function (value) {
            if (this.TenantPm.RegulatedAgentNumber != value) {
                this.TenantPm.RegulatedAgentNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "CustomerIdVisibility", {
        get: function () {
            var customerIdVisibility = false;
            if (this.TenantPm.IsDocumentsArchive) {
                customerIdVisibility = true;
            }
            return customerIdVisibility;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "RegulatedAgentVisibility", {
        get: function () {
            var myResult = false;
            if (SessionLocator_1.SessionLocator.TenantManagementJS.ManagesRegisteredAgent) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "AgentIdVisibility", {
        get: function () {
            var agentIdVisibility = false;
            if (!this.TenantPm.IsDocumentsArchive) {
                agentIdVisibility = true;
            }
            return agentIdVisibility;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "CustomerTenantShareImportFileVisible", {
        get: function () {
            var result = false;
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "CUSTOMERTENANTACCESSES")) {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "CustomerTenantShareExportFileVisible", {
        get: function () {
            var result = false;
            //var FeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "LEX" && d.TenantNumber == SessionLocator.Tenant)[0];
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "CUSTOMERTENANTACCESSES")) {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SystemDefaultsComponent.prototype, "IsCustomerTenantShareVisible", {
        get: function () {
            var result = false;
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "CUSTOMERTENANTACCESSES")) {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    //Commands 
    SystemDefaultsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SystemDefaultsComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.DataContext.TenantPm, this.DataContext.ObjectTableName, errors);
        if (this.VatUniqueTypeCode == "USC") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.VatUniqueCountryId)) {
                errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Tenant.F.VatUniqueCountryId"));
            }
        }
        if (this.VatMandatoryTypeCode == "MSC") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.VatMandatoryCountryId)) {
                errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Tenant.F.VatMandatoryCountryId"));
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("CompanyDefaults", "Edit");
            this.SubmitTenantChanges();
        }
    };
    SystemDefaultsComponent.prototype.SubmitTenantChanges = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Saving...");
        var myService = new TenantPMService_1.TenantPMService();
        myService.update(this.TenantPm).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    InfraSettings_1.InfraSettings.TenantPM = _this.TenantPm;
                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    };
    SystemDefaultsComponent = __decorate([
        core_1.Component({
            selector: 'SystemDefaultsComponent',
            moduleId: module.id,
            templateUrl: './SystemDefaultsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], SystemDefaultsComponent);
    return SystemDefaultsComponent;
}(BaseComponent_1.BaseComponent));
exports.SystemDefaultsComponent = SystemDefaultsComponent;
//# sourceMappingURL=SystemDefaultsComponent.js.map