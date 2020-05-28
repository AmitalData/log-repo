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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TenantPM_1 = require("../../../Common/EntityPMs/TenantPM");
var TenantPMService_1 = require("../../../Common/Services/StandardPMs/TenantPMService");
var InfraSettings_1 = require("../../../Infrastructure/Utilities/InfraSettings");
var Tools_1 = require("../../../Infrastructure/Tools");
var QuestionnaireList_1 = require("../../../CRM/EntityLists/QuestionnaireList");
var QuestionnaireListService_1 = require("../../../CRM/Services/StandardLists/QuestionnaireListService");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var CustomerActivationSettingsComponent = /** @class */ (function (_super) {
    __extends(CustomerActivationSettingsComponent, _super);
    function CustomerActivationSettingsComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Tenant";
        _this.QuestionnaireList = [];
        _this.IsVisibile = false;
        _this.IsShowAreaDefaultQuestionnaire = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.CurrentSession.StartBusyIndicator("Loading...");
        _this.QuestionnaireList = [];
        _this.tenantPM = new TenantPM_1.TenantPM();
        _this.LoadTenantPMMethod();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "QUESTIONNAIRE")) {
            _this.IsShowAreaDefaultQuestionnaire = true;
        }
        return _this;
    }
    // Load Tenant 
    CustomerActivationSettingsComponent.prototype.LoadTenantPMMethod = function () {
        var _this = this;
        var myService = new TenantPMService_1.TenantPMService();
        myService.get(SessionLocator_1.SessionLocator.TenantPM.Id).subscribe(function (response) {
            _this.CurrentSession.StopBusyIndicator();
            if (!response.HasError) {
                _this.tenantPM = response.Result;
                _this.GetQuestionnairesByTenant();
                _this.SetUIProperties();
                _this.IsVisibile = true;
            }
        });
    };
    //SetUIProperties
    CustomerActivationSettingsComponent.prototype.SetUIProperties = function () {
        this.SetCustomerPotentialTelProperties();
        this.SetCustomerPotentialFaxProperties();
    };
    CustomerActivationSettingsComponent.prototype.SetCustomerPotentialTelProperties = function () {
        if (!this.IsCustomerTelRequired) {
            if (this.IsPotentialTelRequired) {
                this.IsCustomerTelRequired = true;
                this.UIProperties.SetEnabled("IsCustomerTelRequired", this.ObjectTableName, false);
            }
            else {
                this.IsCustomerTelRequired = false;
                this.UIProperties.SetEnabled("IsCustomerTelRequired", this.ObjectTableName, true);
            }
        }
        else {
            if (this.IsPotentialTelRequired) {
                this.UIProperties.SetEnabled("IsCustomerTelRequired", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetEnabled("IsCustomerTelRequired", this.ObjectTableName, true);
            }
        }
    };
    CustomerActivationSettingsComponent.prototype.SetCustomerPotentialFaxProperties = function () {
        if (!this.IsCustomerFaxRequired) {
            if (this.IsPotentialFaxRequired) {
                this.IsCustomerFaxRequired = true;
                this.UIProperties.SetEnabled("IsCustomerFaxRequired", this.ObjectTableName, false);
            }
            else {
                this.IsCustomerFaxRequired = false;
                this.UIProperties.SetEnabled("IsCustomerFaxRequired", this.ObjectTableName, true);
            }
            //FirePropertyChanged("IsCustomerFaxRequired");
        }
        else {
            if (this.IsPotentialFaxRequired) {
                this.UIProperties.SetEnabled("IsCustomerFaxRequired", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetEnabled("IsCustomerFaxRequired", this.ObjectTableName, true);
            }
        }
    };
    //Questionnaire
    CustomerActivationSettingsComponent.prototype.GetQuestionnairesByTenant = function () {
        var _this = this;
        this.QuestionnaireList = [];
        var service = new QuestionnaireListService_1.QuestionnaireListService();
        service.getAll().subscribe(function (response) {
            if (!response.HasError) {
                var list = response.Result;
                if (list != null) {
                    list = list.filter(function (q) { return q.InActive == false; });
                }
                list.forEach(function (item) {
                    _this.QuestionnaireList.push(item);
                });
                var None = new QuestionnaireList_1.QuestionnaireList();
                None.Name = "None";
                _this.QuestionnaireList.push(None);
            }
        });
    };
    Object.defineProperty(CustomerActivationSettingsComponent.prototype, "QuestionnaireSelected", {
        get: function () {
            var _this = this;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.tenantPM.DefaultQuestionnaireId)) {
                if (QuestionnaireList_1.QuestionnaireList != null) {
                    this.questionnaireSelected = this.QuestionnaireList.filter(function (d) { return d.Id == _this.tenantPM.DefaultQuestionnaireId; })[0];
                }
            }
            else if (this.tenantPM.DefaultQuestionnaireId == null) {
                this.questionnaireSelected = this.QuestionnaireList.filter(function (d) { return d.Name == "None"; })[0];
            }
            return this.questionnaireSelected;
        },
        set: function (value) {
            this.questionnaireSelected = value;
            if (this.questionnaireSelected.Name == "None") {
                this.tenantPM.DefaultQuestionnaireId = null;
            }
            else {
                this.tenantPM.DefaultQuestionnaireId = this.questionnaireSelected.Id;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerActivationSettingsComponent.prototype, "DefaultQuestionnaireId", {
        get: function () {
            return this.tenantPM.DefaultQuestionnaireId;
        },
        set: function (value) {
            this.tenantPM.DefaultQuestionnaireId = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerActivationSettingsComponent.prototype, "IsCustomerTelRequired", {
        //Settings
        get: function () { return this.tenantPM.IsCustomerTelRequired; },
        set: function (value) {
            if (this.tenantPM.IsCustomerTelRequired != value) {
                this.tenantPM.IsCustomerTelRequired = value;
                this.SetCustomerPotentialTelProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerActivationSettingsComponent.prototype, "IsCustomerFaxRequired", {
        get: function () { return this.tenantPM.IsCustomerFaxRequired; },
        set: function (value) {
            if (this.tenantPM.IsCustomerFaxRequired != value) {
                this.tenantPM.IsCustomerFaxRequired = value;
                this.SetCustomerPotentialFaxProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerActivationSettingsComponent.prototype, "IsPickDelAdrsRequired", {
        get: function () { return this.tenantPM.IsPickDelAdrsRequired; },
        set: function (value) {
            if (this.tenantPM.IsPickDelAdrsRequired != value) {
                this.tenantPM.IsPickDelAdrsRequired = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerActivationSettingsComponent.prototype, "IsCustomerAddress1Required", {
        get: function () { return this.tenantPM.IsCustomerAddress1Required; },
        set: function (value) {
            if (this.tenantPM.IsCustomerAddress1Required != value) {
                this.tenantPM.IsCustomerAddress1Required = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerActivationSettingsComponent.prototype, "HasPrimaryContact", {
        get: function () { return this.tenantPM.HasPrimaryContact; },
        set: function (value) {
            if (this.tenantPM.HasPrimaryContact != value) {
                this.tenantPM.HasPrimaryContact = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerActivationSettingsComponent.prototype, "IsPotentialTelRequired", {
        get: function () { return this.tenantPM.IsPotentialTelRequired; },
        set: function (value) {
            if (this.tenantPM.IsPotentialTelRequired != value) {
                this.tenantPM.IsPotentialTelRequired = value;
                this.SetCustomerPotentialTelProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerActivationSettingsComponent.prototype, "IsPotentialFaxRequired", {
        get: function () { return this.tenantPM.IsPotentialFaxRequired; },
        set: function (value) {
            if (this.tenantPM.IsPotentialFaxRequired != value) {
                this.tenantPM.IsPotentialFaxRequired = value;
                this.SetCustomerPotentialFaxProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    // Commands 
    CustomerActivationSettingsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CustomerActivationSettingsComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.tenantPM, this.DataContext.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("CompanyDefaults", "Edit");
            this.SubmitTenantChanges();
        }
    };
    CustomerActivationSettingsComponent.prototype.SubmitTenantChanges = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Saving...");
        var myService = new TenantPMService_1.TenantPMService();
        myService.update(this.tenantPM).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    InfraSettings_1.InfraSettings.TenantPM = _this.tenantPM;
                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    };
    CustomerActivationSettingsComponent = __decorate([
        core_1.Component({
            selector: 'CustomerActivationSettingsComponent',
            moduleId: module.id,
            templateUrl: './CustomerActivationSettingsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CustomerActivationSettingsComponent);
    return CustomerActivationSettingsComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomerActivationSettingsComponent = CustomerActivationSettingsComponent;
//# sourceMappingURL=CustomerActivationSettingsComponent.js.map