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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var CodeNameClass_1 = require("../../../../Infrastructure/DataContracts/CodeNameClass");
var Tools_1 = require("../../../../Infrastructure/Tools");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TenantAddOnPM_1 = require("../../../../Infrastructure/EntityPMs/TenantAddOnPM");
var TenantManagementLicensePM_1 = require("../../../../Infrastructure/EntityPMs/TenantManagementLicensePM");
var PackageListService_1 = require("../../../../Common/Services/StandardLists/PackageListService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var GlobalDomainService_1 = require("../../../../Common/Services/GlobalDomainService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var TenantManagementGeneralTabComponent = /** @class */ (function (_super) {
    __extends(TenantManagementGeneralTabComponent, _super);
    function TenantManagementGeneralTabComponent(entityArgs, entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.entityResourceService = entityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "TenantManagement";
        _this.SessionEvent = null;
        _this.TabChangedEvent = null;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.ParentComboEnabled = true;
        //Parent Tenant
        _this.ParentTenantsList = [];
        _this.EntityPM = _this.entityArgs.EntityPM;
        _this.iGlobalDomainService = new GlobalDomainService_1.GlobalDomainService();
        _this.LoadParentTenants();
        _this.Listen();
        _this.BluesnapContractIdFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.BluesnapContractIdFilterItems.addAdditionalFilter("BluesnapContractTypeCode", "BA", null, null, "Equals", false, false, false, "string", false, true);
        _this.BluesnapCRMContractIdFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.BluesnapCRMContractIdFilterItems.addAdditionalFilter("BluesnapContractTypeCode", "CRM", null, null, "Equals", false, false, false, "string", false, true);
        _this.BluesnapEAWBContractIdFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.BluesnapEAWBContractIdFilterItems.addAdditionalFilter("BluesnapContractTypeCode", "EAWB", null, null, "Equals", false, false, false, "string", false, true);
        _this.BluesnapEAWBSContractIdFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.BluesnapEAWBSContractIdFilterItems.addAdditionalFilter("BluesnapContractTypeCode", "EABS", null, null, "Equals", false, false, false, "string", false, true);
        _this.BluesnapInttraStockContractIdFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.BluesnapInttraStockContractIdFilterItems.addAdditionalFilter("BluesnapContractTypeCode", "INTS", null, null, "Equals", false, false, false, "string", false, true);
        return _this;
    }
    TenantManagementGeneralTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.iGlobalDomainService.UpdateTenantManagementJS(_this.EntityPM);
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.iGlobalDomainService.UpdateTenantManagementJS(_this.EntityPM);
                }
            });
        }
    };
    TenantManagementGeneralTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    TenantManagementGeneralTabComponent.prototype.ngOnInit = function () {
        if (this.EntityPM != null) {
            this.BuildPackagesList();
            this.BuildAddOnsList();
            this.SetUIProperties();
            this.FillTechnologiesList();
        }
    };
    TenantManagementGeneralTabComponent.prototype.SetUIProperties = function () {
        this.isTenantManagementEditable = this.IsTenantManagementEditable();
        this.SetUIProperties_Distributor();
        this.SetUIProperties_NumberOfUsers();
        this.SetUIProperties_ManageLicencesPerUser();
        if (this.isTenantManagementEditable) {
            this.SetUIProperties_PaymentFailure();
            this.SetUIProperties_IsTrial();
            this.SetUIProperties_IsRecurring();
            this.SetUIProperties_Plimus();
            this.SetUIProperties_TemporalPackage();
            this.SetUIProperties_TenantType();
            this.SetUIProperties_ParentTenant();
        }
        else {
            this.ParentComboEnabled = false;
            this.UIProperties.SetEnabled("Name", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("FreeUsers", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PackageCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TTY", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsAWBStockPrepaid", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsINTTRAStockPrepaid", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsActive", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsSystemSupportEnabled", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsDistributorSupportEnabled", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("DistributorCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Notes", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaymentFailure", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("SuspendDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("InternalNotes", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsTrial", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TrialStartDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TrialEndDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsRecurring", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("RecurringPeriodCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("FirstPaymentDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaidUntilDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaymentCurrencyCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaymentChannelCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaymentMethodCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PlimusAccount", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("MainContract", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("LicensePrice", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TemporalPackageCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TemporalStartDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TemporalEndDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TenantTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TenantConnectedToAirlineCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsParentTenant", this.ObjectTableName, false);
        }
        if (SessionLocator_1.SessionLocator.Tenant == 0) {
            this.UIProperties.SetEnabled("IsSystemSupportEnabled", this.ObjectTableName, false);
        }
    };
    TenantManagementGeneralTabComponent.prototype.SetUIProperties_NumberOfUsers = function () {
        var isEditable = false;
        if (this.isTenantManagementEditable) {
            if (!this.IsMultiPackage) {
                isEditable = true;
            }
        }
        this.UIProperties.SetEnabled("NumberOfUsers", this.ObjectTableName, isEditable);
    };
    TenantManagementGeneralTabComponent.prototype.SetUIProperties_Distributor = function () {
        if (SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare) {
            var objectTable = window.ObjectTables.filter(function (d) { return d.Name == "TenantManagement"; })[0];
            var enableSysDisChoices = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return f.Code == "SYSDISENABLED" && f.ObjectTableId == objectTable.Id; })[0];
            if (enableSysDisChoices) {
                this.UIProperties.SetVisibility("IsSystemSupportEnabled", "TenantManagement", true);
                this.UIProperties.SetVisibility("IsDistributorSupportEnabled", "TenantManagement", true);
                this.UIProperties.SetVisibility("DistributorCode", "TenantManagement", true);
            }
            else {
                this.UIProperties.SetVisibility("IsSystemSupportEnabled", "TenantManagement", false);
                this.UIProperties.SetVisibility("IsDistributorSupportEnabled", "TenantManagement", false);
                this.UIProperties.SetVisibility("DistributorCode", "TenantManagement", false);
            }
        }
    };
    TenantManagementGeneralTabComponent.prototype.SetUIProperties_PaymentFailure = function () {
        if (this.EntityPM.PaymentFailure) {
            this.UIProperties.SetEnabled("SuspendDate", "TenantManagement", true);
            this.UIProperties.SetEnabled("InternalNotes", "TenantManagement", true);
        }
        else {
            this.UIProperties.SetEnabled("SuspendDate", "TenantManagement", false);
            this.UIProperties.SetEnabled("InternalNotes", "TenantManagement", false);
        }
    };
    TenantManagementGeneralTabComponent.prototype.SetUIProperties_IsTrial = function () {
        if (this.EntityPM.IsTrial) {
            if (this.TrialStartDate == null) {
                this.UIProperties.SetRequired("TrialStartDate", this.ObjectTableName, true);
                this.UIProperties.SetRequired("TrialEndDate", this.ObjectTableName, true);
            }
            this.UIProperties.SetEnabled("TrialStartDate", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("TrialEndDate", this.ObjectTableName, true);
        }
        else {
            this.UIProperties.SetRequired("TrialStartDate", this.ObjectTableName, false);
            this.UIProperties.SetRequired("TrialEndDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TrialStartDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TrialEndDate", this.ObjectTableName, false);
        }
    };
    TenantManagementGeneralTabComponent.prototype.SetUIProperties_IsRecurring = function () {
        if (this.EntityPM.IsRecurring) {
            this.UIProperties.SetRequired("RecurringPeriodCode", "TenantManagement", Tools_1.AppTool.IsNullOrEmpty(this.RecurringPeriodCode));
            this.UIProperties.SetEnabled("RecurringPeriodCode", "TenantManagement", true);
        }
        else {
            this.UIProperties.SetRequired("RecurringPeriodCode", "TenantManagement", false);
            this.UIProperties.SetEnabled("RecurringPeriodCode", "TenantManagement", false);
        }
    };
    TenantManagementGeneralTabComponent.prototype.SetUIProperties_TemporalPackage = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.TemporalPackageCode)) {
            if (this.TemporalStartDate == null && this.TemporalEndDate == null) {
                this.UIProperties.SetRequired("TemporalStartDate", this.ObjectTableName, true);
                this.UIProperties.SetRequired("TemporalEndDate", this.ObjectTableName, true);
            }
            this.UIProperties.SetEnabled("TemporalStartDate", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("TemporalEndDate", this.ObjectTableName, true);
        }
        else {
            this.UIProperties.SetRequired("TemporalStartDate", this.ObjectTableName, false);
            this.UIProperties.SetRequired("TemporalEndDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TemporalStartDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TemporalEndDate", this.ObjectTableName, false);
        }
    };
    TenantManagementGeneralTabComponent.prototype.SetUIProperties_Plimus = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.PaymentChannelCode)) {
            if (this.PaymentChannelCode == "PL") {
                this.UIProperties.SetEnabled("PlimusAccount", "TenantManagement", true);
            }
            else {
                this.UIProperties.SetEnabled("PlimusAccount", "TenantManagement", false);
            }
        }
        else {
            this.UIProperties.SetEnabled("PlimusAccount", "TenantManagement", false);
        }
    };
    TenantManagementGeneralTabComponent.prototype.SetUIProperties_ManageLicencesPerUser = function () {
        var isFieldEnabled = false;
        if (this.isTenantManagementEditable) {
            if (!this.IsMultiPackage) {
                isFieldEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("ManageLicencesPerUser", this.ObjectTableName, isFieldEnabled);
    };
    TenantManagementGeneralTabComponent.prototype.SetUIProperties_TenantType = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.TenantTypeCode)) {
            this.UIProperties.SetEnabled("TenantConnectedToAirlineCode", this.ObjectTableName, false);
        }
        else {
            if (this.TenantTypeCode == "AIR") {
                this.UIProperties.SetEnabled("TenantConnectedToAirlineCode", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetEnabled("TenantConnectedToAirlineCode", this.ObjectTableName, false);
            }
        }
    };
    TenantManagementGeneralTabComponent.prototype.SetUIProperties_ParentTenant = function () {
        if (this.IsParentTenant) {
            this.ParentComboEnabled = false;
        }
        else {
            this.ParentComboEnabled = true;
        }
        if (this.ParentTenantId != null) {
            this.UIProperties.SetEnabled("IsParentTenant", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetEnabled("IsParentTenant", this.ObjectTableName, true);
        }
    };
    TenantManagementGeneralTabComponent.prototype.CloseBillingFields = function (close) {
        this.UIProperties.SetEnabled("IsRecurring", "TenantManagement", !close);
        this.UIProperties.SetEnabled("RecurringPeriodCode", "TenantManagement", !close);
        this.UIProperties.SetEnabled("FirstPaymentDate", "TenantManagement", !close);
        this.UIProperties.SetEnabled("PaidUntilDate", "TenantManagement", !close);
        this.UIProperties.SetEnabled("PaymentChannelCode", "TenantManagement", !close);
        this.UIProperties.SetEnabled("PaymentMethodCode", "TenantManagement", !close);
        this.UIProperties.SetEnabled("BluesnapAccount", "TenantManagement", !close);
        this.UIProperties.SetEnabled("MainContract", "TenantManagement", !close);
        this.UIProperties.SetEnabled("LicensePrice", "TenantManagement", !close);
        this.UIProperties.SetEnabled("PaymentCurrencyCode", "TenantManagement", !close);
    };
    TenantManagementGeneralTabComponent.prototype.IsTenantManagementEditable = function () {
        var myResult = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("TenantManagement", "EnableTenantManagementEdit")) {
            myResult = true;
        }
        return myResult;
    };
    TenantManagementGeneralTabComponent.prototype.LoadParentTenants = function () {
        var _this = this;
        this.iGlobalDomainService.GetParentTenants().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var myList = myResponse.Result;
                _this.BuildParentTenantsList(myList);
            }
        });
    };
    TenantManagementGeneralTabComponent.prototype.BuildParentTenantsList = function (myList) {
        var _this = this;
        this.ParentTenantsList = [];
        if (myList) {
            if (myList.length > 0) {
                var noneItem = new CodeNameClass_1.CodeNameClass();
                noneItem.Code = "None";
                noneItem.Name = "None";
                noneItem.DisplyText = "None";
                this.ParentTenantsList.push(noneItem);
                myList.forEach(function (item) {
                    var newItem = new CodeNameClass_1.CodeNameClass();
                    newItem.Code_Int = item.Id;
                    newItem.Name = item.Name;
                    newItem.DisplyText = item.Name + " (" + item.Id + ")";
                    _this.ParentTenantsList.push(newItem);
                });
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ParentTenantId)) {
                    this.selectedParentTenant = this.ParentTenantsList.filter(function (d) { return d.Code_Int == _this.EntityPM.ParentTenantId; })[0];
                }
                else {
                    this.selectedParentTenant = this.ParentTenantsList.filter(function (d) { return d.Code == "None"; })[0];
                }
            }
        }
    };
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "SelectedParentTenant", {
        get: function () { return this.selectedParentTenant; },
        set: function (value) {
            if (this.selectedParentTenant != value) {
                this.selectedParentTenant = value;
                if (value) {
                    if (value.Code == "None") {
                        this.ParentTenantId = null;
                    }
                    else {
                        this.ParentTenantId = value.Code_Int;
                    }
                }
                else {
                    this.ParentTenantId = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "IsParentTenant", {
        get: function () { return this.EntityPM.IsParentTenant; },
        set: function (newValue) {
            if (this.EntityPM.IsParentTenant != newValue) {
                this.EntityPM.IsParentTenant = newValue;
                this.SetUIProperties_ParentTenant();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "ParentTenantId", {
        get: function () { return this.EntityPM.ParentTenantId; },
        set: function (newValue) {
            if (this.EntityPM.ParentTenantId != newValue) {
                this.EntityPM.ParentTenantId = newValue;
                this.SetUIProperties_ParentTenant();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "Name", {
        //General Details
        get: function () { return this.EntityPM.Name; },
        set: function (newValue) {
            if (this.EntityPM.Name != newValue) {
                this.EntityPM.Name = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "TenantTypeCode", {
        get: function () { return this.EntityPM.TenantTypeCode; },
        set: function (newValue) {
            if (this.EntityPM.TenantTypeCode != newValue) {
                this.EntityPM.TenantTypeCode = newValue;
                this.SetUIProperties_TenantType();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "TenantConnectedToAirlineCode", {
        get: function () { return this.EntityPM.TenantConnectedToAirlineCode; },
        set: function (newValue) {
            if (this.EntityPM.TenantConnectedToAirlineCode != newValue) {
                this.EntityPM.TenantConnectedToAirlineCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "IsAWBStockPrepaid", {
        get: function () { return this.EntityPM.IsAWBStockPrepaid; },
        set: function (newValue) {
            if (this.EntityPM.IsAWBStockPrepaid != newValue) {
                this.EntityPM.IsAWBStockPrepaid = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "IsINTTRAStockPrepaid", {
        get: function () { return this.EntityPM.IsINTTRAStockPrepaid; },
        set: function (newValue) {
            if (this.EntityPM.IsINTTRAStockPrepaid != newValue) {
                this.EntityPM.IsINTTRAStockPrepaid = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "ManageLicencesPerUser", {
        get: function () { return this.EntityPM.ManageLicencesPerUser; },
        set: function (newValue) {
            if (this.EntityPM.ManageLicencesPerUser != newValue) {
                this.EntityPM.ManageLicencesPerUser = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "ManagesRegisteredAgent", {
        get: function () { return this.EntityPM.ManagesRegisteredAgent; },
        set: function (newValue) {
            if (this.EntityPM.ManagesRegisteredAgent != newValue) {
                this.EntityPM.ManagesRegisteredAgent = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "IsActive", {
        get: function () { return this.EntityPM.IsActive; },
        set: function (newValue) {
            if (this.EntityPM.IsActive != newValue) {
                this.EntityPM.IsActive = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "IsSystemSupportEnabled", {
        get: function () { return this.EntityPM.IsSystemSupportEnabled; },
        set: function (newValue) {
            if (this.EntityPM.IsSystemSupportEnabled != newValue) {
                this.EntityPM.IsSystemSupportEnabled = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "IsDistributorSupportEnabled", {
        get: function () { return this.EntityPM.IsDistributorSupportEnabled; },
        set: function (newValue) {
            if (this.EntityPM.IsDistributorSupportEnabled != newValue) {
                this.EntityPM.IsDistributorSupportEnabled = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "DistributorCode", {
        get: function () { return this.EntityPM.DistributorCode; },
        set: function (newValue) {
            if (this.EntityPM.DistributorCode != newValue) {
                this.EntityPM.DistributorCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "PrivateLabelId", {
        get: function () { return this.EntityPM.PrivateLabelId; },
        set: function (newValue) {
            if (this.EntityPM.PrivateLabelId != newValue) {
                this.EntityPM.PrivateLabelId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (newValue) {
            if (this.EntityPM.Notes != newValue) {
                this.EntityPM.Notes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "Technology", {
        get: function () { return this.EntityPM.Technology; },
        set: function (newValue) {
            if (this.EntityPM.Technology != newValue) {
                this.EntityPM.Technology = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    TenantManagementGeneralTabComponent.prototype.FillTechnologiesList = function () {
        var _this = this;
        this.TechnologiesList = [];
        this.TechnologiesList.push(new CodeNameClass_1.CodeNameClass("AG", "Angular"));
        this.TechnologiesList.push(new CodeNameClass_1.CodeNameClass("SL", "SliverLight"));
        this.TechnologiesList.push(new CodeNameClass_1.CodeNameClass("PR", "Prompt"));
        this.selectedTechnology = this.TechnologiesList.filter(function (d) { return d.Code == _this.EntityPM.Technology; })[0];
    };
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "SelectedTechnology", {
        get: function () { return this.selectedTechnology; },
        set: function (value) {
            if (this.selectedTechnology != value) {
                this.selectedTechnology = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.Technology = value.Code;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "IsTrial", {
        //Trial Details
        get: function () { return this.EntityPM.IsTrial; },
        set: function (newValue) {
            if (this.EntityPM.IsTrial != newValue) {
                this.EntityPM.IsTrial = newValue;
                if (newValue) {
                    this.IsRecurring = !newValue;
                }
                this.CloseBillingFields(newValue);
                this.SetUIProperties_IsTrial();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "TrialStartDate", {
        get: function () { return this.EntityPM.TrialStartDate; },
        set: function (newValue) {
            if (this.EntityPM.TrialStartDate != newValue) {
                this.EntityPM.TrialStartDate = newValue;
                if (newValue == null) {
                    this.UIProperties.SetRequired("TrialStartDate", this.ObjectTableName, true);
                }
                else {
                    this.UIProperties.SetRequired("TrialStartDate", this.ObjectTableName, false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "TrialEndDate", {
        get: function () { return this.EntityPM.TrialEndDate; },
        set: function (newValue) {
            if (this.EntityPM.TrialEndDate != newValue) {
                this.EntityPM.TrialEndDate = newValue;
                if (newValue == null) {
                    this.UIProperties.SetRequired("TrialEndDate", this.ObjectTableName, true);
                }
                else {
                    this.UIProperties.SetRequired("TrialEndDate", this.ObjectTableName, false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "SilverlightEndDate", {
        get: function () { return this.EntityPM.SilverlightEndDate; },
        set: function (newValue) {
            if (this.EntityPM.SilverlightEndDate != newValue) {
                this.EntityPM.SilverlightEndDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "ChangeHeaderColor", {
        get: function () { return this.EntityPM.ChangeHeaderColor; },
        set: function (newValue) {
            if (this.EntityPM.ChangeHeaderColor != newValue) {
                this.EntityPM.ChangeHeaderColor = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "IsTestTenant", {
        get: function () { return this.EntityPM.IsTestTenant; },
        set: function (newValue) {
            if (this.EntityPM.IsTestTenant != newValue) {
                this.EntityPM.IsTestTenant = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "DocumentShareAsDefault", {
        get: function () { return this.EntityPM.DocumentShareAsDefault; },
        set: function (newValue) {
            if (this.EntityPM.DocumentShareAsDefault != newValue) {
                this.EntityPM.DocumentShareAsDefault = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "TrialBoxIsEnabled", {
        get: function () {
            var result = true;
            if (this.PaidUntilDate != null || this.IsRecurring) {
                result = false;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "PaymentFailure", {
        //Payment Failure
        get: function () { return this.EntityPM.PaymentFailure; },
        set: function (newValue) {
            if (this.EntityPM.PaymentFailure != newValue) {
                this.EntityPM.PaymentFailure = newValue;
                if (!newValue) {
                    this.SuspendDate = null;
                    this.InternalNotes = null;
                }
                this.SetUIProperties_PaymentFailure();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "SuspendDate", {
        get: function () { return this.EntityPM.SuspendDate; },
        set: function (newValue) {
            if (this.EntityPM.SuspendDate != newValue) {
                this.EntityPM.SuspendDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "InternalNotes", {
        get: function () { return this.EntityPM.InternalNotes; },
        set: function (newValue) {
            if (this.EntityPM.InternalNotes != newValue) {
                this.EntityPM.InternalNotes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "PaymentFailureBoxIsEnabled", {
        get: function () {
            var result = false;
            if (this.IsRecurring) {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "IsMultiPackage", {
        //Subscription Details
        get: function () { return this.EntityPM.IsMultiPackage; },
        set: function (newValue) {
            if (this.EntityPM.IsMultiPackage != newValue) {
                this.EntityPM.IsMultiPackage = newValue;
                this.SetUIProperties_NumberOfUsers();
                this.SetUIProperties_ManageLicencesPerUser();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "NumberOfUsers", {
        get: function () { return this.EntityPM.NumberOfUsers; },
        set: function (newValue) {
            if (this.EntityPM.NumberOfUsers != newValue) {
                this.EntityPM.NumberOfUsers = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "FreeUsers", {
        get: function () { return this.EntityPM.FreeUsers; },
        set: function (newValue) {
            if (this.EntityPM.FreeUsers != newValue) {
                this.EntityPM.FreeUsers = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "BluesnapContractQTYs", {
        get: function () { return this.EntityPM.BluesnapContractQTY; },
        set: function (newValue) {
            if (this.EntityPM.BluesnapContractQTY != newValue) {
                if (newValue == null)
                    newValue = 0;
                this.EntityPM.BluesnapContractQTY = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "BluesnapInttraStockContractQTYs", {
        get: function () { return this.EntityPM.BluesnapInttraStockContractQTY; },
        set: function (newValue) {
            if (this.EntityPM.BluesnapInttraStockContractQTY != newValue) {
                if (newValue == null)
                    newValue = 0;
                this.EntityPM.BluesnapInttraStockContractQTY = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "BluesnapCRMContractQTYs", {
        get: function () { return this.EntityPM.BluesnapCRMContractQTY; },
        set: function (newValue) {
            if (this.EntityPM.BluesnapCRMContractQTY != newValue) {
                if (newValue == null)
                    newValue = 0;
                this.EntityPM.BluesnapCRMContractQTY = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "BluesnapEAWBContractQTYs", {
        get: function () { return this.EntityPM.BluesnapEAWBContractQTY; },
        set: function (newValue) {
            if (this.EntityPM.BluesnapEAWBContractQTY != newValue) {
                if (newValue == null)
                    newValue = 0;
                this.EntityPM.BluesnapEAWBContractQTY = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "BluesnapEAWBSContractQTYs", {
        get: function () { return this.EntityPM.BluesnapEAWBSContractQTY; },
        set: function (newValue) {
            if (this.EntityPM.BluesnapEAWBSContractQTY != newValue) {
                if (newValue == null)
                    newValue = 0;
                this.EntityPM.BluesnapEAWBSContractQTY = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "BluesnapOneTimeContractQTYs", {
        get: function () { return this.EntityPM.BluesnapOneTimeContractQTY; },
        set: function (newValue) {
            if (this.EntityPM.BluesnapOneTimeContractQTY != newValue) {
                if (newValue == null)
                    newValue = 0;
                this.EntityPM.BluesnapOneTimeContractQTY = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "PackageCode", {
        get: function () { return this.EntityPM.PackageCode; },
        set: function (newValue) {
            if (this.EntityPM.PackageCode != newValue) {
                this.EntityPM.PackageCode = newValue;
                if (newValue == "EAWB") {
                    this.IsAWBStockPrepaid = true;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    TenantManagementGeneralTabComponent.prototype.AddPackageClicked = function (packageType) {
        var _this = this;
        if (packageType == "AD") {
            this.entityResourceService.getEntityResourceByTableName("TenantAddOn").subscribe(function (res1) {
                var newAddOnPM = new TenantAddOnPM_1.TenantAddOnPM(_this.EntityPM);
                newAddOnPM.Tenant = _this.EntityPM.Id;
                var logeWindow = new LogitudeWindow_1.LogitudeWindow();
                logeWindow.Title = "Add Add-On";
                logeWindow.DataContext = new AddOnItem(newAddOnPM, _this, true);
                logeWindow.Show("../InfrastructureModules/InfrastructureTenantManagement/Components/TenantManagement/AddEditAddOnComponent");
                logeWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.BuildAddOnsList();
                    }
                });
            });
        }
        else {
            this.entityResourceService.getEntityResourceByTableName("TenantManagementLicense").subscribe(function (res1) {
                var newLicencePM = new TenantManagementLicensePM_1.TenantManagementLicensePM(_this.EntityPM);
                newLicencePM.Tenant = _this.EntityPM.Id;
                var logeWindow = new LogitudeWindow_1.LogitudeWindow();
                logeWindow.Title = "Add Package";
                logeWindow.DataContext = new PackageItem(newLicencePM, _this, true);
                logeWindow.Show("./InfrastructureModules/InfrastructureTenantManagement/Components/TenantManagement/AddEditLicenceComponent");
                logeWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.BuildPackagesList();
                    }
                });
            });
        }
    };
    TenantManagementGeneralTabComponent.prototype.BuildPackagesList = function () {
        var _this = this;
        if (this.PackagesList == null) {
            this.PackagesList = new Array();
        }
        else {
            this.PackagesList = [];
        }
        var service = new PackageListService_1.PackageListService();
        service.getAllFromCache().subscribe(function (result) {
            var allPackages = result.Result;
            _this.EntityPM.TenantManagementLicenses.forEach(function (item) {
                var list = allPackages.filter(function (d) { return d.Code == item.PackageCode; })[0];
                if (list != null) {
                    _this.PackagesList.push(new PackageItem(item, _this, false));
                }
            });
        });
    };
    TenantManagementGeneralTabComponent.prototype.BuildAddOnsList = function () {
        var _this = this;
        if (this.AddOnsList == null) {
            this.AddOnsList = new Array();
        }
        else {
            this.AddOnsList = [];
        }
        var service = new PackageListService_1.PackageListService();
        service.getAllFromCache().subscribe(function (result) {
            var allPackages = result.Result;
            _this.EntityPM.AddOns.forEach(function (item) {
                var list = allPackages.filter(function (d) { return d.Code == item.PackageCode; })[0];
                if (list != null) {
                    _this.AddOnsList.push(new AddOnItem(item, _this, false));
                }
            });
        });
    };
    TenantManagementGeneralTabComponent.prototype.EditAddOn = function (itemViewModel) {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName("TenantAddOn").subscribe(function (res1) {
            itemViewModel.SetOldData();
            var logeWindow = new LogitudeWindow_1.LogitudeWindow();
            logeWindow.Title = "Edit Add-On";
            logeWindow.DataContext = itemViewModel;
            logeWindow.Show("./InfrastructureModules/InfrastructureTenantManagement/Components/TenantManagement/AddEditAddOnComponent");
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.BuildAddOnsList();
                }
            });
        });
    };
    TenantManagementGeneralTabComponent.prototype.DeleteAddOn = function (itemViewModel) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Delete this Add-On?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                var itemIndex = _this.AddOnsList.indexOf(itemViewModel);
                if (itemIndex > -1) {
                    _this.AddOnsList.splice(itemIndex, 1);
                }
                _this.EntityPM.RemoveTenantAddOnPM(itemViewModel.EntityPM);
                _this.BuildAddOnsList();
            }
        });
    };
    TenantManagementGeneralTabComponent.prototype.EditPackage = function (itemViewModel) {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName("TenantManagementLicense").subscribe(function (res1) {
            itemViewModel.SetOldData();
            var logeWindow = new LogitudeWindow_1.LogitudeWindow();
            logeWindow.Title = "Edit Package";
            logeWindow.DataContext = itemViewModel;
            logeWindow.Show("./InfrastructureModules/InfrastructureTenantManagement/Components/TenantManagement/AddEditLicenceComponent");
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.BuildPackagesList();
                }
            });
        });
    };
    TenantManagementGeneralTabComponent.prototype.DeletePackage = function (itemViewModel) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Delete this Package?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                var itemIndex = _this.PackagesList.indexOf(itemViewModel);
                if (itemIndex > -1) {
                    _this.PackagesList.splice(itemIndex, 1);
                }
                _this.EntityPM.RemoveTenantManagementLicensePM(itemViewModel.EntityPM);
                _this.BuildPackagesList();
            }
        });
    };
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "IsEditable", {
        get: function () {
            var isEditable = false;
            if (this.isTenantManagementEditable) {
                if (this.IsMultiPackage) {
                    isEditable = true;
                }
            }
            return isEditable;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "IsRecurring", {
        //Billing Details
        get: function () { return this.EntityPM.IsRecurring; },
        set: function (newValue) {
            if (this.EntityPM.IsRecurring != newValue) {
                this.EntityPM.IsRecurring = newValue;
                //if (newValue)
                //    this.IsTrial = !newValue;
            }
            this.SetUIProperties_IsRecurring();
            this.SetUIProperties_IsTrial();
            this.SetUIProperties_PaymentFailure();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "RecurringPeriodCode", {
        get: function () { return this.EntityPM.RecurringPeriodCode; },
        set: function (newValue) {
            if (this.EntityPM.RecurringPeriodCode != newValue) {
                this.EntityPM.RecurringPeriodCode = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.UIProperties.SetRequired("RecurringPeriodCode", "TenantManagement", true);
                }
                else {
                    this.UIProperties.SetRequired("RecurringPeriodCode", "TenantManagement", false);
                    if (newValue == "MO") {
                        this.UIProperties.SetEnabled("IsRecurring", "TenantManagement", false);
                    }
                    else {
                        this.UIProperties.SetEnabled("IsRecurring", "TenantManagement", true);
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "FirstPaymentDate", {
        get: function () { return this.EntityPM.FirstPaymentDate; },
        set: function (newValue) {
            if (this.EntityPM.FirstPaymentDate != newValue) {
                this.EntityPM.FirstPaymentDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "PaidUntilDate", {
        get: function () { return this.EntityPM.PaidUntilDate; },
        set: function (newValue) {
            if (this.EntityPM.PaidUntilDate != newValue) {
                this.EntityPM.PaidUntilDate = newValue;
                this.SetUIProperties_IsTrial();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "PaymentCurrencyCode", {
        get: function () { return this.EntityPM.PaymentCurrencyCode; },
        set: function (newValue) {
            if (this.EntityPM.PaymentCurrencyCode != newValue) {
                this.EntityPM.PaymentCurrencyCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "PaymentChannelCode", {
        get: function () { return this.EntityPM.PaymentChannelCode; },
        set: function (newValue) {
            if (this.EntityPM.PaymentChannelCode != newValue) {
                this.EntityPM.PaymentChannelCode = newValue;
                this.SetUIProperties_Plimus();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "PaymentMethodCode", {
        get: function () { return this.EntityPM.PaymentMethodCode; },
        set: function (newValue) {
            if (this.EntityPM.PaymentMethodCode != newValue) {
                this.EntityPM.PaymentMethodCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "BluesnapAccount", {
        get: function () { return this.EntityPM.BluesnapAccount; },
        set: function (newValue) {
            if (this.EntityPM.BluesnapAccount != newValue) {
                this.EntityPM.BluesnapAccount = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "MainContract", {
        get: function () { return this.EntityPM.MainContract; },
        set: function (newValue) {
            if (this.EntityPM.MainContract != newValue) {
                this.EntityPM.MainContract = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "LicensePrice", {
        get: function () { return this.EntityPM.LicensePrice; },
        set: function (newValue) {
            if (this.EntityPM.LicensePrice != newValue) {
                this.EntityPM.LicensePrice = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "BluesnapContractId", {
        get: function () {
            return this.EntityPM.BluesnapContractId;
        },
        set: function (newValue) {
            if (this.EntityPM.BluesnapContractId != newValue) {
                this.EntityPM.BluesnapContractId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "BluesnapCRMContractId", {
        get: function () {
            return this.EntityPM.BluesnapCRMContractId;
        },
        set: function (newValue) {
            if (this.EntityPM.BluesnapCRMContractId != newValue) {
                this.EntityPM.BluesnapCRMContractId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "BluesnapEAWBContractId", {
        get: function () {
            return this.EntityPM.BluesnapEAWBContractId;
        },
        set: function (newValue) {
            if (this.EntityPM.BluesnapEAWBContractId != newValue) {
                this.EntityPM.BluesnapEAWBContractId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "BluesnapEAWBSContractId", {
        get: function () {
            return this.EntityPM.BluesnapEAWBSContractId;
        },
        set: function (newValue) {
            if (this.EntityPM.BluesnapEAWBSContractId != newValue) {
                this.EntityPM.BluesnapEAWBSContractId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "BluesnapOneTimeContract", {
        get: function () {
            return this.EntityPM.BluesnapOneTimeContract;
        },
        set: function (newValue) {
            if (this.EntityPM.BluesnapOneTimeContract != newValue) {
                this.EntityPM.BluesnapOneTimeContract = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "BluesnapInttraStockContractId", {
        get: function () {
            return this.EntityPM.BluesnapInttraStockContractId;
        },
        set: function (newValue) {
            if (this.EntityPM.BluesnapInttraStockContractId != newValue) {
                this.EntityPM.BluesnapInttraStockContractId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "BillingByLogitude", {
        get: function () { return this.EntityPM.BillingByLogitude; },
        set: function (newValue) {
            if (this.EntityPM.BillingByLogitude != newValue) {
                this.EntityPM.BillingByLogitude = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    TenantManagementGeneralTabComponent.prototype.SetBillingByLogitude = function (billingByLogitude) {
        this.BillingByLogitude = billingByLogitude;
    };
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "ResellerCommission", {
        get: function () { return this.EntityPM.ResellerCommission; },
        set: function (newValue) {
            if (this.EntityPM.ResellerCommission != newValue) {
                this.EntityPM.ResellerCommission = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "TemporalPackageCode", {
        //Temporal Package Details
        get: function () { return this.EntityPM.TemporalPackageCode; },
        set: function (newValue) {
            if (this.EntityPM.TemporalPackageCode != newValue) {
                this.EntityPM.TemporalPackageCode = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.TemporalStartDate = null;
                    this.TemporalEndDate = null;
                }
                this.SetUIProperties_TemporalPackage();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "TemporalStartDate", {
        get: function () { return this.EntityPM.TemporalStartDate; },
        set: function (newValue) {
            if (this.EntityPM.TemporalStartDate != newValue) {
                this.EntityPM.TemporalStartDate = newValue;
                if (newValue == null) {
                    this.UIProperties.SetRequired("TemporalStartDate", this.ObjectTableName, true);
                }
                else {
                    this.UIProperties.SetRequired("TemporalStartDate", this.ObjectTableName, false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementGeneralTabComponent.prototype, "TemporalEndDate", {
        get: function () { return this.EntityPM.TemporalEndDate; },
        set: function (newValue) {
            if (this.EntityPM.TemporalEndDate != newValue) {
                this.EntityPM.TemporalEndDate = newValue;
                if (newValue == null) {
                    this.UIProperties.SetRequired("TemporalEndDate", this.ObjectTableName, true);
                }
                else {
                    this.UIProperties.SetRequired("TemporalEndDate", this.ObjectTableName, false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    TenantManagementGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'TenantManagementGeneralTabComponent',
            templateUrl: './TenantManagementGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], TenantManagementGeneralTabComponent);
    return TenantManagementGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.TenantManagementGeneralTabComponent = TenantManagementGeneralTabComponent;
var PackageItem = /** @class */ (function (_super) {
    __extends(PackageItem, _super);
    function PackageItem(entity, fatherComponent, isNew) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.ObjectTableName = "TenantManagementLicense";
        _this.EntityPM = entity;
        _this.IsNew = isNew;
        _this.TenantManagementPM = fatherComponent.EntityPM;
        _this.GetPackageName();
        return _this;
    }
    PackageItem.prototype.SetOldData = function () {
        this.old_PackageCode = this.PackageCode;
        this.old_NumberOfUsers = this.NumberOfUsers;
    };
    PackageItem.prototype.ResetOldData = function () {
        this.PackageCode = this.old_PackageCode;
        this.NumberOfUsers = this.old_NumberOfUsers;
    };
    Object.defineProperty(PackageItem.prototype, "PackageCode", {
        get: function () { return this.EntityPM.PackageCode; },
        set: function (newValue) {
            if (this.EntityPM.PackageCode != newValue) {
                this.EntityPM.PackageCode = newValue;
                this.GetPackageName();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackageItem.prototype, "NumberOfUsers", {
        get: function () { return this.EntityPM.NumberOfUsers; },
        set: function (newValue) {
            if (this.EntityPM.NumberOfUsers != newValue) {
                this.EntityPM.NumberOfUsers = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackageItem.prototype, "PackageName", {
        get: function () { return this.packageName; },
        set: function (newValue) {
            if (this.packageName != newValue) {
                this.packageName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    PackageItem.prototype.GetPackageName = function () {
        var _this = this;
        var service = new PackageListService_1.PackageListService();
        service.getAllFromCache().subscribe(function (result) {
            var allPackages = result.Result;
            var list = allPackages.filter(function (d) { return d.Code == _this.PackageCode; })[0];
            if (list != null) {
                _this.PackageName = list.Name;
            }
        });
    };
    return PackageItem;
}(BaseComponent_1.BaseComponent));
exports.PackageItem = PackageItem;
var AddOnItem = /** @class */ (function (_super) {
    __extends(AddOnItem, _super);
    function AddOnItem(entity, fatherComponent, isNew) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.ObjectTableName = "TenantAddOn";
        _this.EntityPM = entity;
        _this.IsNew = isNew;
        _this.TenantManagementPM = fatherComponent.EntityPM;
        _this.GetPackageName();
        return _this;
    }
    AddOnItem.prototype.SetOldData = function () {
        this.old_PackageCode = this.PackageCode;
    };
    AddOnItem.prototype.ResetOldData = function () {
        this.PackageCode = this.old_PackageCode;
    };
    Object.defineProperty(AddOnItem.prototype, "PackageCode", {
        get: function () { return this.EntityPM.PackageCode; },
        set: function (newValue) {
            if (this.EntityPM.PackageCode != newValue) {
                this.EntityPM.PackageCode = newValue;
                this.GetPackageName();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddOnItem.prototype, "PackageName", {
        get: function () { return this.packageName; },
        set: function (newValue) {
            if (this.packageName != newValue) {
                this.packageName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddOnItem.prototype.GetPackageName = function () {
        var _this = this;
        var service = new PackageListService_1.PackageListService();
        service.getAllFromCache().subscribe(function (result) {
            var allPackages = result.Result;
            var list = allPackages.filter(function (d) { return d.Code == _this.PackageCode; })[0];
            if (list != null) {
                _this.PackageName = list.Name;
            }
        });
    };
    return AddOnItem;
}(BaseComponent_1.BaseComponent));
exports.AddOnItem = AddOnItem;
//# sourceMappingURL=TenantManagementGeneralTabComponent.js.map