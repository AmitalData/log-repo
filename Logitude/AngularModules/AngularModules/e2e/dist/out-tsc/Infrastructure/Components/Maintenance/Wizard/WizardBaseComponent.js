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
var BaseComponent_1 = require("../../LogitudeComponents/BaseComponent");
var Validator_1 = require("../../../Validators/Validator");
var InfraSettings_1 = require("../../../Utilities/InfraSettings");
var SessionLocator_1 = require("../../../Utilities/SessionLocator");
var LocationDirective_1 = require("../../../Utilities/LocationDirective");
var Tools_1 = require("../../../Tools");
var AgentPM_1 = require("../../../../Common/EntityPMs/AgentPM");
var AddressPM_1 = require("../../../../Common/EntityPMs/AddressPM");
var VatTypePercentagePM_1 = require("../../../../Common/EntityPMs/VatTypePercentagePM");
var AgentPMService_1 = require("../../../../Common/Services/StandardPMs/AgentPMService");
var TenantPMService_1 = require("../../../../Common/Services/StandardPMs/TenantPMService");
var AddressPMService_1 = require("../../../../Common/Services/StandardPMs/AddressPMService");
var VatTypePMService_1 = require("../../../../Common/Services/StandardPMs/VatTypePMService");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var EntityResourceService_1 = require("../../../Services/EntityResourceService");
var RatesTablePM_1 = require("../../../EntityPMs/RatesTablePM");
var RatesTablePMService_1 = require("../../../Services/StandardPMs/RatesTablePMService");
var CachedDataManager_1 = require("../../../Utilities/CachedDataManager");
var ObjectsLocator_1 = require("../../../Locators/ObjectsLocator");
var ObjectsUpdater_1 = require("../../../Locators/ObjectsUpdater");
var WizardBaseComponent = /** @class */ (function (_super) {
    __extends(WizardBaseComponent, _super);
    function WizardBaseComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.AgentPM = null;
        _this.TenantPM = null;
        _this.AddressPM = null;
        _this.VatTypePM = null;
        _this.PercentagePM = null;
        _this.IsResourcesReady = false;
        _this.ValidationErrorsList = [];
        _this.SaveCompleted = new core_1.EventEmitter();
        _this.SignOutCompleted = new core_1.EventEmitter();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Retries = 0;
        _this.PageChild_ADD = null;
        _this.PageChild_LOG = null;
        _this.PageChild_ACC = null;
        _this.LogoTabIndexColor = "rgba(110, 113, 114, 0.37)";
        _this.LogoTabTextColor = "rgba(110, 113, 114, 0.37)";
        _this.LogoTabBackground = "rgba(110, 113, 114, 0.12)";
        _this.FinishTabIndexColor = "rgba(110, 113, 114, 0.37)";
        _this.FinishTabTextColor = "rgba(110, 113, 114, 0.37)";
        _this.FinishTabBackground = "rgba(110, 113, 114, 0.12)";
        _this.AccountingSettingsTabIndexColor = "rgba(110, 113, 114, 0.37)";
        _this.AccountingSettingsTabTextColor = "rgba(110, 113, 114, 0.37)";
        _this.AccountingSettingsTabBackground = "rgba(110, 113, 114, 0.12)";
        _this.InitServices();
        return _this;
    }
    WizardBaseComponent.prototype.InitServices = function () {
        this.myAgentPMService = new AgentPMService_1.AgentPMService();
        this.myTenantPMService = new TenantPMService_1.TenantPMService();
        this.myAddressPMService = new AddressPMService_1.AddressPMService();
        this.myVatTypePMService = new VatTypePMService_1.VatTypePMService();
        this.myCommonDomainService = new CommonDomainService_1.CommonDomainService();
    };
    WizardBaseComponent.prototype.RunComponent = function () {
        var _this = this;
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(function (resp1) {
                    _this.entityResourceService.getEntityResourceByTableName("Address", 0).subscribe(function (resp2) {
                        _this.InitObjectsData();
                    });
                });
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    WizardBaseComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    WizardBaseComponent.prototype.InitObjectsData = function () {
        var _this = this;
        this.myTenantPMService.get(SessionLocator_1.SessionLocator.TenantPM.Id).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.TenantPM = myResponse.Result;
                _this.TenantPM.ProfitCurrencyRate = null;
                if (Tools_1.AppTool.IsNullOrEmpty(_this.TenantPM.ProfitCurrencyRate)) {
                    if (_this.TenantPM.PackageCode == "IMPO") {
                        _this.TenantPM.ProfitCurrencyRate = 4;
                    }
                }
                _this.InitSTDVat();
            }
        });
    };
    WizardBaseComponent.prototype.InitSTDVat = function () {
        var _this = this;
        this.myCommonDomainService.GetSingleVatTypeByCode("STD").subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.VatTypePM = myResponse.Result;
                if (_this.VatTypePM) {
                    var myPercentagePM = null;
                    if (_this.VatTypePM.VatTypePercentages.length == 0) {
                        myPercentagePM = new VatTypePercentagePM_1.VatTypePercentagePM(_this.VatTypePM);
                        myPercentagePM.Tenant = _this.VatTypePM.Tenant;
                        myPercentagePM.VatTypeId = _this.VatTypePM.Id;
                        myPercentagePM.FromDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                        _this.VatTypePM.AddVatTypePercentagePM(myPercentagePM);
                    }
                    else {
                        myPercentagePM = Tools_1.ArrayTool.SortByDate(_this.VatTypePM.VatTypePercentages, "FromDate")[0];
                    }
                    if (_this.TenantPM.STDVatPercentage != myPercentagePM.Percentage) {
                        _this.TenantPM.STDVatPercentage = myPercentagePM.Percentage;
                    }
                    _this.PercentagePM = myPercentagePM;
                    _this.InitAgentObject();
                }
            }
        });
    };
    WizardBaseComponent.prototype.InitAgentObject = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantPM.AgentId)) {
            this.AgentPM = new AgentPM_1.AgentPM();
            this.AgentPM.Code = "new";
            this.AgentPM.PartnerTypeId = "AG";
            this.AgentPM.EnglishName = SessionLocator_1.SessionLocator.TenantPM.Company;
            this.AgentPM.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
            this.InitAddressObject();
        }
        else {
            this.myAgentPMService.get(SessionLocator_1.SessionLocator.TenantPM.AgentId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.AgentPM = myResponse.Result;
                    _this.InitAddressObject();
                }
            });
        }
    };
    WizardBaseComponent.prototype.InitAddressObject = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantPM.AddressId)) {
            this.AddressPM = new AddressPM_1.AddressPM();
            this.AddressPM.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
            this.AddressPM.AddressTypeId = "M";
            this.AddressPM.Name = SessionLocator_1.SessionLocator.TenantPM.Company;
            this.AddressPM.Description = SessionLocator_1.SessionLocator.TenantPM.Company;
            this.AddressPM.PhoneNumber = SessionLocator_1.SessionLocator.LoggedUserPM.BusinessPhone;
            this.InitScreenView();
        }
        else {
            this.myAddressPMService.get(SessionLocator_1.SessionLocator.TenantPM.AddressId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.AddressPM = myResponse.Result;
                    _this.InitScreenView();
                }
            });
        }
    };
    WizardBaseComponent.prototype.InitScreenView = function () {
        this.IsResourcesReady = true;
        this.SelectedTabCode = "ADD";
    };
    Object.defineProperty(WizardBaseComponent.prototype, "SelectedTabCode", {
        get: function () { return this.selectedTabCode; },
        set: function (value) {
            if (this.selectedTabCode != value) {
                this.selectedTabCode = value;
                this.SelectionChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    WizardBaseComponent.prototype.SelectionChanged = function () {
        var _this = this;
        var location = this.AllLocations.toArray().filter(function (f) { return f.Code == _this.SelectedTabCode; })[0];
        if (location) {
            switch (location.Code) {
                case "ADD": {
                    if (this.PageChild_ADD == null) {
                        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/Maintenance/Wizard/WizardAddressCompnent', location.viewContainerRef)
                            .then(function (cmpRef) {
                            _this.PageChild_ADD = cmpRef.instance;
                            _this.PageChild_ADD.InitializeComponent(_this.TenantPM, _this.AddressPM, _this.AgentPM);
                        });
                    }
                    break;
                }
                case "LOG": {
                    if (this.PageChild_LOG == null) {
                        SessionLocator_1.SessionLocator.DynamicLoader.Load('./InfrastructureModules/InfrastructureGettingStarted/Components/UploadImage/UploadLogoComponent', location.viewContainerRef)
                            .then(function (cmpRef) {
                            _this.PageChild_LOG = cmpRef.instance;
                            _this.PageChild_LOG.IsHideAreaCloseButton = true;
                        });
                    }
                    break;
                }
                case "ACC": {
                    if (this.PageChild_ACC == null) {
                        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/Maintenance/Wizard/WizardAccountingComponent', location.viewContainerRef)
                            .then(function (cmpRef) {
                            _this.PageChild_ACC = cmpRef.instance;
                            _this.PageChild_ACC.InitializeComponent(_this.TenantPM);
                        });
                    }
                    break;
                }
            }
        }
    };
    WizardBaseComponent.prototype.NextButtonClicked = function () {
        var errors = [];
        switch (this.SelectedTabCode) {
            case "ADD": {
                if (this.PageChild_ADD) {
                    this.PageChild_ADD.Validate(errors);
                }
                break;
            }
            case "ACC": {
                if (this.PageChild_ACC) {
                    this.PageChild_ACC.Validate(errors);
                }
                break;
            }
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            if (SessionLocator_1.SessionLocator.TenantPM.PackageCode == "IMPO" && this.SelectedTabCode == "ADD") {
                this.SelectedTabCode = "ACC";
            }
            else {
                if (this.SelectedTabCode == "ADD") {
                    this.SelectedTabCode = "LOG";
                    this.LogoTabBackground = "#1B90CB";
                    this.LogoTabIndexColor = "#FFFFFF";
                    this.LogoTabTextColor = "#1B90CB";
                }
                else if (this.SelectedTabCode == "LOG") {
                    this.SelectedTabCode = "ACC";
                    this.AccountingSettingsTabBackground = "#1B90CB";
                    this.AccountingSettingsTabIndexColor = "#FFFFFF";
                    this.AccountingSettingsTabTextColor = "#1B90CB";
                }
                else if (this.SelectedTabCode == "ACC") {
                    this.SelectedTabCode = "FIN";
                    this.FinishTabBackground = "#1B90CB";
                    this.FinishTabIndexColor = "#FFFFFF";
                    this.FinishTabTextColor = "#1B90CB";
                }
            }
        }
    };
    WizardBaseComponent.prototype.BackButtonClicked = function () {
        if (SessionLocator_1.SessionLocator.TenantPM.PackageCode == "IMPO" && this.SelectedTabCode == "ACC") {
            this.SelectedTabCode = "ADD";
        }
        else {
            if (this.SelectedTabCode == "LOG")
                this.SelectedTabCode = "ADD";
            else if (this.SelectedTabCode == "ACC")
                this.SelectedTabCode = "LOG";
            else if (this.SelectedTabCode == "FIN")
                this.SelectedTabCode = "ACC";
        }
    };
    WizardBaseComponent.prototype.SignoutClicked = function () {
        this.SignOutCompleted.emit(true);
    };
    WizardBaseComponent.prototype.FinishButtonClicked = function () {
        var errors = [];
        this.PageChild_ADD.Validate(errors);
        this.PageChild_ACC.Validate(errors);
        Validator_1.Validator.TryValidateObject(this.AgentPM, "Agent", errors);
        Validator_1.Validator.TryValidateObject(this.TenantPM, "Tenant", errors);
        Validator_1.Validator.TryValidateObject(this.AddressPM, "Address", errors);
        if (this.TenantPM.DayLightOffset != 0) {
            if (this.TenantPM.DayLightEndDate == null || this.TenantPM.DayLightStartDate == null) {
                errors.push("DayLightStartDate and DayLightEndDate should have values");
            }
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var isSavingVAT = false;
            if (this.PercentagePM) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.PercentagePM.Id)) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.TenantPM.STDVatPercentage)) {
                        this.PercentagePM.Percentage = this.TenantPM.STDVatPercentage;
                        isSavingVAT = true;
                    }
                }
                else {
                    if (this.PercentagePM.Percentage != this.TenantPM.STDVatPercentage) {
                        this.PercentagePM.Percentage = this.TenantPM.STDVatPercentage;
                        isSavingVAT = true;
                    }
                }
            }
            if (isSavingVAT) {
                this.SaveVatType();
            }
            else {
                this.SaveAgent();
            }
        }
    };
    WizardBaseComponent.prototype.SaveVatType = function () {
        var _this = this;
        this.myVatTypePMService.update(this.VatTypePM).subscribe(function (myResponse) {
            if (myResponse.HasError) {
                _this.ShowServiceErrors(myResponse);
            }
            else {
                _this.SaveAgent();
            }
        });
    };
    WizardBaseComponent.prototype.SaveAgent = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.AgentPM.Id)) {
            this.myAgentPMService.insert(this.AgentPM).subscribe(function (myResponse) {
                if (myResponse.HasError) {
                    _this.ShowServiceErrors(myResponse);
                }
                else {
                    _this.TenantPM.AgentId = _this.AgentPM.Id;
                    _this.SaveAddress();
                }
            });
        }
        else {
            this.myAgentPMService.update(this.AgentPM).subscribe(function (myResponse) {
                if (myResponse.HasError) {
                    _this.ShowServiceErrors(myResponse);
                }
                else {
                    _this.TenantPM.AgentId = _this.AgentPM.Id;
                    _this.SaveAddress();
                }
            });
        }
    };
    WizardBaseComponent.prototype.SaveAddress = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.AddressPM.Id)) {
            this.myAddressPMService.insert(this.AddressPM).subscribe(function (myResponse) {
                if (myResponse.HasError) {
                    _this.ShowServiceErrors(myResponse);
                }
                else {
                    _this.TenantPM.AddressId = _this.AddressPM.Id;
                    _this.SaveTenant();
                }
            });
        }
        else {
            this.myAddressPMService.update(this.AddressPM).subscribe(function (myResponse) {
                if (myResponse.HasError) {
                    _this.ShowServiceErrors(myResponse);
                }
                else {
                    _this.TenantPM.AddressId = _this.AddressPM.Id;
                    _this.SaveTenant();
                }
            });
        }
    };
    WizardBaseComponent.prototype.SaveTenant = function () {
        var _this = this;
        this.myTenantPMService.update(this.TenantPM).subscribe(function (myResponse) {
            if (myResponse.HasError) {
                _this.ShowServiceErrors(myResponse);
            }
            else {
                _this.SaveCurrencyRate();
            }
        });
    };
    WizardBaseComponent.prototype.SaveCurrencyRate = function () {
        var _this = this;
        var newRatesTablePM = new RatesTablePM_1.RatesTablePM();
        newRatesTablePM.Tenant = this.TenantPM.Id;
        newRatesTablePM.BaseCurrencyId = this.TenantPM.CurrencyId;
        newRatesTablePM.ForeignCurrencyId = this.TenantPM.ProfitCurrencyId;
        newRatesTablePM.Rate = this.TenantPM.ProfitCurrencyRate;
        newRatesTablePM.LogDateTime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        newRatesTablePM.ValueDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        var myService = new RatesTablePMService_1.RatesTablePMService();
        myService.insert(newRatesTablePM).subscribe(function (myResponse) {
            if (myResponse.HasError) {
                _this.ShowServiceErrors(myResponse);
            }
            else {
                CachedDataManager_1.CachedDataManager.RefreshTableData("Currency", true);
                _this.myTenantPMService.get(_this.TenantPM.Id).subscribe(function (myResponse) {
                    if (myResponse.HasError) {
                        _this.ShowServiceErrors(myResponse);
                    }
                    else {
                        InfraSettings_1.InfraSettings.TenantPM = _this.TenantPM = myResponse.Result;
                        if (_this.TenantPM.CountryCode == "MX") {
                            _this.OnCreatingMexicanTenant();
                        }
                        else if (_this.TenantPM.CountryCode == "US") {
                            _this.OnCreatingUSTenant();
                        }
                        else if (_this.TenantPM.CountryCode == "MA") {
                            _this.OnCreatingMoroccoTenant();
                        }
                        else if (_this.TenantPM.CountryCode == "IL") {
                            _this.OnCreatingIsraelTenant();
                        }
                        else {
                            _this.CurrentSession.StopBusyIndicator();
                            _this.SaveCompleted.emit(true);
                        }
                    }
                });
            }
        });
    };
    WizardBaseComponent.prototype.OnCreatingMexicanTenant = function () {
        var _this = this;
        this.myCommonDomainService.GetOnCreatingMexicanTenant().subscribe(function (myResponse) {
            if (myResponse.HasError) {
                _this.ShowServiceErrors(myResponse);
            }
            else {
                CachedDataManager_1.CachedDataManager.RefreshTableData("VatType", true);
                if (SessionLocator_1.SessionLocator.AccountingSettingPM) {
                    SessionLocator_1.SessionLocator.AccountingSettingPM.EnableMultiPercentageVATTypes = true;
                }
                _this.myCommonDomainService.GetAllVatTypesGroups().subscribe(function (myResponse2) {
                    if (!myResponse2.HasError) {
                        SessionLocator_1.SessionLocator.AllVatTypesGroups = myResponse2.Result;
                    }
                    _this.CurrentSession.StopBusyIndicator();
                    _this.SaveCompleted.emit(true);
                });
            }
        });
    };
    WizardBaseComponent.prototype.OnCreatingUSTenant = function () {
        var _this = this;
        this.myCommonDomainService.GetOnCreatingUSTenant().subscribe(function (myResponse) {
            if (myResponse.HasError) {
                _this.ShowServiceErrors(myResponse);
            }
            else {
                if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM) {
                    ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ActivateCustomsManagementInShipments = true;
                }
                _this.CurrentSession.StopBusyIndicator();
                _this.SaveCompleted.emit(true);
            }
        });
    };
    WizardBaseComponent.prototype.OnCreatingMoroccoTenant = function () {
        var _this = this;
        this.myCommonDomainService.OnCreatingMoroccoTenant().subscribe(function (myResponse) {
            if (myResponse.HasError) {
                _this.ShowServiceErrors(myResponse);
            }
            else {
                if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM) {
                    ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ActivateCustomsManagementInShipments = true;
                }
                _this.CurrentSession.StopBusyIndicator();
                _this.SaveCompleted.emit(true);
            }
        });
    };
    WizardBaseComponent.prototype.OnCreatingIsraelTenant = function () {
        var _this = this;
        this.myCommonDomainService.OnCreatingIsraelTenant().subscribe(function (myResponse) {
            if (myResponse.HasError) {
                _this.ShowServiceErrors(myResponse);
            }
            else {
                ObjectsUpdater_1.ObjectsUpdater.UpdateAccountingSettingPM(myResponse.Result);
                _this.CurrentSession.StopBusyIndicator();
                _this.SaveCompleted.emit(true);
            }
        });
    };
    WizardBaseComponent.prototype.ShowServiceErrors = function (myResponse) {
        if (myResponse) {
            this.ValidationErrorsList = myResponse.ErrorsArray;
            this.CurrentSession.StopBusyIndicator();
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], WizardBaseComponent.prototype, "SaveCompleted", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], WizardBaseComponent.prototype, "SignOutCompleted", void 0);
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], WizardBaseComponent.prototype, "AllLocations", void 0);
    WizardBaseComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './WizardBaseComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], WizardBaseComponent);
    return WizardBaseComponent;
}(BaseComponent_1.BaseComponent));
exports.WizardBaseComponent = WizardBaseComponent;
//# sourceMappingURL=WizardBaseComponent.js.map