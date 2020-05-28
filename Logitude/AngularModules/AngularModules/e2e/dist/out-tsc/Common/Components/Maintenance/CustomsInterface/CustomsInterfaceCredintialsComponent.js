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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var CustomsInterfaceCredintialsComponent = /** @class */ (function (_super) {
    __extends(CustomsInterfaceCredintialsComponent, _super);
    function CustomsInterfaceCredintialsComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    CustomsInterfaceCredintialsComponent.prototype.SetWindowArgs = function (entityPM) {
        this.EntityPM = entityPM;
        this.ObjectTableName = "CustomsInterfaceSetting";
        this.SetUIProperties();
        this.Clone();
    };
    CustomsInterfaceCredintialsComponent.prototype.SetUIProperties = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.LocalCustomsInterfaceCode)) {
            this.UIProperties.SetRequired("LocalCompanyId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.LocalCompanyId));
            this.UIProperties.SetRequired("LocalUserId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.LocalUserId));
            this.UIProperties.SetRequired("LocalPassword", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.LocalPassword));
        }
    };
    Object.defineProperty(CustomsInterfaceCredintialsComponent.prototype, "LocalCompanyId", {
        get: function () { return this.EntityPM.LocalCompanyId; },
        set: function (value) {
            if (this.EntityPM.LocalCompanyId != value) {
                this.EntityPM.LocalCompanyId = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsInterfaceCredintialsComponent.prototype, "LocalUserId", {
        get: function () { return this.EntityPM.LocalUserId; },
        set: function (value) {
            if (this.EntityPM.LocalUserId != value) {
                this.EntityPM.LocalUserId = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsInterfaceCredintialsComponent.prototype, "LocalPassword", {
        get: function () { return this.EntityPM.LocalPassword; },
        set: function (value) {
            if (this.EntityPM.LocalPassword != value) {
                this.EntityPM.LocalPassword = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomsInterfaceCredintialsComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    CustomsInterfaceCredintialsComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.LocalCustomsInterfaceCode)) {
            if (this.EntityPM.LocalCustomsInterfaceCode != "NO") {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.LocalCompanyId)) {
                    this.ValidationErrorsList.push("Company field is required");
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.LocalUserId)) {
                    this.ValidationErrorsList.push("User field is required");
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.LocalPassword)) {
                    this.ValidationErrorsList.push("Password field is required");
                }
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    };
    CustomsInterfaceCredintialsComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('LocalCompanyId');
        this.myCloner.AddField('LocalUserId');
        this.myCloner.AddField('LocalPassword');
        this.myCloner.AddEntity(this.EntityPM);
    };
    CustomsInterfaceCredintialsComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    CustomsInterfaceCredintialsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomsInterfaceCredintialsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CustomsInterfaceCredintialsComponent);
    return CustomsInterfaceCredintialsComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomsInterfaceCredintialsComponent = CustomsInterfaceCredintialsComponent;
//# sourceMappingURL=CustomsInterfaceCredintialsComponent.js.map