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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var CouriersVatPM_1 = require("../../../../Customs/EntityPMs/CouriersVatPM");
var CouriersVatPMService_1 = require("../../../../Customs/Services/StandardPMs/CouriersVatPMService");
var AddEditCouriersVatComponent = /** @class */ (function (_super) {
    __extends(AddEditCouriersVatComponent, _super);
    function AddEditCouriersVatComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.CouriersVat";
        _this.isWindowMode = false;
        _this.ValidationErrorsList = [];
        _this._CouriersVatPMService = new CouriersVatPMService_1.CouriersVatPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (Tools_1.AppTool.IsNullOrEmpty(entityArgs.EntityPM)) {
            _this.EntityPM = new CouriersVatPM_1.CouriersVatPM();
            _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            _this.isWindowMode = true;
        }
        else {
            _this.EntityPM = _this.entityArgs.EntityPM;
        }
        return _this;
    }
    AddEditCouriersVatComponent.prototype.SetWindowArgs = function (args) {
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.isWindowMode = true;
        }
    };
    Object.defineProperty(AddEditCouriersVatComponent.prototype, "VatNumber", {
        //#region Properties
        get: function () { return this.EntityPM.VatNumber; },
        set: function (newValue) {
            this.EntityPM.VatNumber = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCouriersVatComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (newValue) {
            this.EntityPM.EnglishName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCouriersVatComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (newValue) {
            this.EntityPM.LocalName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCouriersVatComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (newValue) {
            this.EntityPM.InActive = newValue;
        },
        enumerable: true,
        configurable: true
    });
    //#endregion\
    AddEditCouriersVatComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (errors.length > 0) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
        }
        else {
            this._CouriersVatPMService.insert(this.EntityPM).subscribe(function (myResult) {
                var mm = myResult;
                if (!mm.HasError) {
                    var entity = mm.Result;
                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
                else {
                    _this.ValidationErrorsList = mm.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    };
    AddEditCouriersVatComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditCouriersVatComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditCouriersVatComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], AddEditCouriersVatComponent);
    return AddEditCouriersVatComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditCouriersVatComponent = AddEditCouriersVatComponent;
//# sourceMappingURL=AddEditCouriersVatComponent.js.map