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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
require("rxjs/add/operator/map");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SignUpService_1 = require("../../../../Infrastructure/Services/ExtendedPMs/SignUpService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var CreateTenantPackageSelectionComponent_1 = require("../../../../InfrastructureModules/InfrastructureOthers/Components/CreateTenant/CreateTenantPackageSelectionComponent");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var CreateTenantComponent = /** @class */ (function (_super) {
    __extends(CreateTenantComponent, _super);
    function CreateTenantComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.IsStardLoadPage = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.packageCode = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "Test2") ? "IMPO" : "BUSN";
        _this.country = null;
        _this.signUpService = new SignUpService_1.SignUpService();
        return _this;
    }
    CreateTenantComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("Package").subscribe(function (response) {
            _this.IsStardLoadPage = true;
        });
    };
    Object.defineProperty(CreateTenantComponent.prototype, "PackageCode", {
        get: function () { return this.packageCode; },
        set: function (newValue) {
            if (this.packageCode != newValue) {
                this.packageCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CreateTenantComponent.prototype, "Country", {
        get: function () { return this.country; },
        set: function (newValue) {
            if (this.country != newValue) {
                this.country = newValue;
                this.OnCountryChanged(newValue);
            }
        },
        enumerable: true,
        configurable: true
    });
    CreateTenantComponent.prototype.OnCountryChanged = function (list) {
        if (list == null) {
            this.CountryCode = null;
            this.CountryName = null;
            this.CountryId = null;
        }
        else {
            this.CountryCode = list.Code;
            this.CountryName = list.EnglishName;
            this.CountryId = list.Id;
        }
    };
    CreateTenantComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CreateTenantComponent.prototype.SaveButtonClicked = function () {
        this.ValidationErrorsList = [];
        this.ValidationFields();
        if (this.ValidationErrorsList.length == 0)
            this.Buildtenant();
    };
    CreateTenantComponent.prototype.ValidationFields = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.Email)) {
            this.ValidationErrorsList.push("Email field is required");
        }
        else if (!this.CheckIsValidEmail(this.Email)) {
            this.ValidationErrorsList.push("Invalid Email address");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.ContactName)) {
            this.ValidationErrorsList.push("Name field is required");
        }
        else if (this.ContactName.length > 60) {
            this.ValidationErrorsList.push("Contact Name field max length is 60");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.CompanyName)) {
            this.ValidationErrorsList.push("Company field is required");
        }
        else if (this.CompanyName.length > 60) {
            this.ValidationErrorsList.push("Company field max length is 60");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.Phone)) {
            this.ValidationErrorsList.push("Phone field is required");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.PackageCode)) {
            this.ValidationErrorsList.push("Package Code field is required");
        }
    };
    CreateTenantComponent.prototype.CheckIsValidEmail = function (email) {
        var IsOk = true;
        var EMAIL_REGEXP1 = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
        var EMAIL_REGEXP2 = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (email) {
            if (email) {
                if (!EMAIL_REGEXP1.test(email)) {
                    IsOk = false;
                    return;
                }
                else if (!EMAIL_REGEXP2.test(email)) {
                    IsOk = false;
                    return;
                }
            }
        }
        return IsOk;
    };
    CreateTenantComponent.prototype.Buildtenant = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        var SignUpInfo = new CreateTenantPackageSelectionComponent_1.SignUpInfoClass();
        SignUpInfo.Email = this.Email;
        SignUpInfo.Phone = this.Phone;
        SignUpInfo.Company = this.CompanyName;
        SignUpInfo.Name = this.ContactName;
        SignUpInfo.PackageCode = this.packageCode;
        SignUpInfo.CountryName = this.CountryName;
        SignUpInfo.CountryCode = this.CountryCode;
        SignUpInfo.Tenant = SessionLocator_1.SessionLocator.Tenant;
        this.signUpService.CreateTenant(SignUpInfo).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                _this.CurrentSession.CloseCurrentWindow();
            }
            else {
                if (myResponse.ErrorsArray && myResponse.ErrorsArray.length > 0) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Title = "Logitude Message";
                    messageWindow.Show(myResponse.ErrorsArray[0]);
                }
            }
        });
    };
    CreateTenantComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'CreateTenantComponent',
            templateUrl: './CreateTenantComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CreateTenantComponent);
    return CreateTenantComponent;
}(BaseComponent_1.BaseComponent));
exports.CreateTenantComponent = CreateTenantComponent;
//# sourceMappingURL=CreateTenantComponent.js.map