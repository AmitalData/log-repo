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
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var CreateTenantPackageSelectionComponent = /** @class */ (function (_super) {
    __extends(CreateTenantPackageSelectionComponent, _super);
    function CreateTenantPackageSelectionComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.IsStardLoadPage = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.packageCode = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "Test2") ? "IMPO" : "BUSN";
        _this.signUpService = new SignUpService_1.SignUpService();
        return _this;
    }
    CreateTenantPackageSelectionComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("Package").subscribe(function (response) {
            _this.IsStardLoadPage = true;
        });
    };
    CreateTenantPackageSelectionComponent.prototype.SetWindowArgs = function (args) {
        this.CustomerId = args.CustomerId;
        this.Email = args.Email;
        this.ContactName = args.ContactName;
        this.Phone = args.Phone;
        this.CustomerName = args.CustomerName;
        this.CountryName = args.CountryName;
        this.CountryCode = args.CountryCode;
        this.ObjecttableName = args.ObjecttableName;
    };
    Object.defineProperty(CreateTenantPackageSelectionComponent.prototype, "PackageCode", {
        get: function () { return this.packageCode; },
        set: function (newValue) {
            if (this.packageCode != newValue) {
                this.packageCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    CreateTenantPackageSelectionComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CreateTenantPackageSelectionComponent.prototype.SaveButtonClicked = function () {
        this.Buildtenant();
    };
    CreateTenantPackageSelectionComponent.prototype.Buildtenant = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        var SignUpInfo = new SignUpInfoClass();
        SignUpInfo.Email = this.Email;
        SignUpInfo.Phone = this.Phone;
        SignUpInfo.Company = this.CustomerName;
        SignUpInfo.Name = this.ContactName;
        SignUpInfo.IsCrmTenant = true;
        SignUpInfo.CustomerId = this.CustomerId;
        SignUpInfo.Tenant = SessionLocator_1.SessionLocator.Tenant;
        SignUpInfo.PackageCode = this.packageCode;
        SignUpInfo.CountryName = this.CountryName;
        SignUpInfo.CountryCode = this.CountryCode;
        SignUpInfo.ObjecttableName = this.ObjecttableName;
        this.signUpService.SendMessageToQueue(SignUpInfo).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.CurrentSession.CurrentEditComponent.SaveChanges();
                _this.CancelButtonClicked();
            }
            else
                _this.CurrentSession.StopBusyIndicator();
        });
    };
    CreateTenantPackageSelectionComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'CreateTenantPackageSelectionComponent',
            templateUrl: './CreateTenantPackageSelectionComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CreateTenantPackageSelectionComponent);
    return CreateTenantPackageSelectionComponent;
}(BaseComponent_1.BaseComponent));
exports.CreateTenantPackageSelectionComponent = CreateTenantPackageSelectionComponent;
var SignUpInfoClass = /** @class */ (function () {
    function SignUpInfoClass() {
    }
    return SignUpInfoClass;
}());
exports.SignUpInfoClass = SignUpInfoClass;
//# sourceMappingURL=CreateTenantPackageSelectionComponent.js.map