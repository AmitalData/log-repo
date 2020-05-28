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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var ClientGeneralTabComponent = /** @class */ (function (_super) {
    __extends(ClientGeneralTabComponent, _super);
    function ClientGeneralTabComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Customs.Client";
        _this.DataContext = _this;
        return _this;
    }
    ClientGeneralTabComponent.prototype.InitTab = function (EntityPM, IsNew) {
        this.entityPM = EntityPM;
        this.isNew = IsNew;
        this.controlEnabled = IsNew;
        this.SetPropertiesEnabled();
        this.isPassport = (Tools_1.AppTool.IsNullOrEmpty(this.entityPM.Code)) && (!Tools_1.AppTool.IsNullOrEmpty(this.entityPM.PassportNumber));
        if (!Tools_1.AppTool.IsNullOrEmpty(this.entityPM.Code)) {
            this.isCorporation = this.entityPM.Code.startsWith("5");
            this.isCitizen = (!this.entityPM.Code.startsWith("5")) && (this.entityPM.Code != "");
        }
    };
    ClientGeneralTabComponent.prototype.SetPropertiesEnabled = function () {
        this.UIProperties.SetEnabled("Code", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("LocalCorporationName", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("EnglishCorporationName", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("DunsNumber", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("ClientTypeSpecificCode", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("IsActive", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("IsExporter", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("IsImporter", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("LocalFirstName", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("LocalLastName", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("EnglishFirstName", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("EnglishLastName", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("BirthDate", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("PassportTypeCode", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("PassportNumber", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("PassportCountryCode", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("PassportFirstName", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("PassportLastName", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("EnglishFatherName", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("EnglishBirthPlace", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("PassportIssueDate", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("PassportExpirationDate", this.ObjectTableName, this.controlEnabled);
    };
    Object.defineProperty(ClientGeneralTabComponent.prototype, "Corporation_Visibility", {
        //#region properties
        get: function () {
            return (this.isCorporation == true) ? true : false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "Citizen_Visibility", {
        get: function () {
            return (this.isCitizen == true) ? true : false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "Passport_Visibility", {
        get: function () {
            return (this.isPassport == true) ? true : false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "Code", {
        get: function () { return this.entityPM.Code; },
        set: function (newValue) { this.entityPM.Code = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "LocalCorporationName", {
        get: function () { return this.entityPM.LocalCorporationName; },
        set: function (newValue) { this.entityPM.LocalCorporationName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "EnglishCorporationName", {
        get: function () { return this.entityPM.EnglishCorporationName; },
        set: function (newValue) { this.entityPM.EnglishCorporationName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "DunsNumber", {
        get: function () { return this.entityPM.DunsNumber; },
        set: function (newValue) { this.entityPM.DunsNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "ClientTypeSpecificCode", {
        get: function () { return this.entityPM.ClientTypeSpecificCode; },
        set: function (newValue) { this.entityPM.ClientTypeSpecificCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "IsActive", {
        get: function () { return this.entityPM.IsActive; },
        set: function (newValue) { this.entityPM.IsActive = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "IsExporter", {
        get: function () { return this.entityPM.IsExporter; },
        set: function (newValue) { this.entityPM.IsExporter = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "IsImporter", {
        get: function () { return this.entityPM.IsImporter; },
        set: function (newValue) { this.entityPM.IsImporter = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "LocalFirstName", {
        get: function () { return this.entityPM.LocalFirstName; },
        set: function (newValue) { this.entityPM.LocalFirstName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "LocalLastName", {
        get: function () { return this.entityPM.LocalLastName; },
        set: function (newValue) { this.entityPM.LocalLastName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "EnglishFirstName", {
        get: function () { return this.entityPM.EnglishFirstName; },
        set: function (newValue) { this.entityPM.EnglishFirstName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "EnglishLastName", {
        get: function () { return this.entityPM.EnglishLastName; },
        set: function (newValue) { this.entityPM.EnglishLastName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "BirthDate", {
        get: function () { return this.entityPM.BirthDate; },
        set: function (newValue) { this.entityPM.BirthDate = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "PassportTypeCode", {
        get: function () { return this.entityPM.PassportTypeCode; },
        set: function (newValue) { this.entityPM.PassportTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "PassportNumber", {
        get: function () { return this.entityPM.PassportNumber; },
        set: function (newValue) { this.entityPM.PassportNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "PassportCountryCode", {
        get: function () { return this.entityPM.PassportCountryCode; },
        set: function (newValue) { this.entityPM.PassportCountryCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "PassportFirstName", {
        get: function () { return this.entityPM.PassportFirstName; },
        set: function (newValue) { this.entityPM.PassportFirstName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "PassportLastName", {
        get: function () { return this.entityPM.PassportLastName; },
        set: function (newValue) { this.entityPM.PassportLastName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "EnglishFatherName", {
        get: function () { return this.entityPM.EnglishFatherName; },
        set: function (newValue) { this.entityPM.EnglishFatherName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "EnglishBirthPlace", {
        get: function () { return this.entityPM.EnglishBirthPlace; },
        set: function (newValue) { this.entityPM.EnglishBirthPlace = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "PassportIssueDate", {
        get: function () { return this.entityPM.PassportIssueDate; },
        set: function (newValue) { this.entityPM.PassportIssueDate = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientGeneralTabComponent.prototype, "PassportExpirationDate", {
        get: function () { return this.entityPM.PassportExpirationDate; },
        set: function (newValue) { this.entityPM.PassportExpirationDate = newValue; },
        enumerable: true,
        configurable: true
    });
    ClientGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ClientGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ClientGeneralTabComponent);
    return ClientGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ClientGeneralTabComponent = ClientGeneralTabComponent;
//# sourceMappingURL=ClientGeneralTabComponent.js.map