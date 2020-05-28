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
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var ChargesTypeListService_1 = require("../../../../Common/Services/StandardLists/ChargesTypeListService");
var AddEditTariffChargeComponent = /** @class */ (function (_super) {
    __extends(AddEditTariffChargeComponent, _super);
    function AddEditTariffChargeComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "TarrifCharge";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.chargesTypeListService = new ChargesTypeListService_1.ChargesTypeListService();
        return _this;
    }
    Object.defineProperty(AddEditTariffChargeComponent.prototype, "ChargesTypeId", {
        get: function () { return this.EntityPM.ChargesTypeId; },
        set: function (value) {
            if (this.EntityPM.ChargesTypeId != value) {
                this.EntityPM.ChargesTypeId = value;
                this.GetChargesTypeData();
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditTariffChargeComponent.prototype.GetChargesTypeData = function () {
        var _this = this;
        if (this.EntityPM.ChargesTypeId == null) {
            this.ChargesGroupCode = null;
            this.ChargesTypeCode = null;
            this.ChargesTypeName = null;
            this.MeasurementId = null;
            this.CurrencyId = null;
        }
        else {
            this.chargesTypeListService.getAllFromCache().subscribe(function (p) {
                var list = p.Result.filter(function (p) { return p.id == _this.EntityPM.ChargesTypeId; })[0];
                if (list != null) {
                    _this.ChargesGroupCode = list.ChargesGroupCode;
                    _this.ChargesTypeCode = list.Code;
                    _this.ChargesTypeName = list.EnglishName;
                    _this.MeasurementId = list.MeasurementId;
                    if (list.ChargesGroupCode == "FRT" || list.ChargesGroupCode == "SCH") {
                        _this.CurrencyId = SessionLocator_1.SessionLocator.TenantPM.FreightCurrencyId;
                    }
                    else {
                        _this.CurrencyId = SessionLocator_1.SessionLocator.TenantPM.OtherChargesCurrencyId;
                    }
                }
            });
        }
    };
    AddEditTariffChargeComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('ChargesTypeId');
        this.myCloner.AddField('MeasurementId');
        this.myCloner.AddField('CurrencyId');
        this.myCloner.AddField('UnitPrice');
        this.myCloner.AddField('MinPrice');
        this.myCloner.AddField('MaxPrice');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.fatherComponent.EntityPM);
    };
    AddEditTariffChargeComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    Object.defineProperty(AddEditTariffChargeComponent.prototype, "ChargesGroupCode", {
        get: function () { return this.chargesGroupCode; },
        set: function (value) { this.chargesGroupCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffChargeComponent.prototype, "ChargesTypeCode", {
        get: function () { return this.EntityPM.ChargesTypeCode; },
        set: function (value) { this.EntityPM.ChargesTypeCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffChargeComponent.prototype, "ChargesTypeName", {
        get: function () { return this.EntityPM.ChargesTypeName; },
        set: function (value) { this.EntityPM.ChargesTypeName = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffChargeComponent.prototype, "MeasurementId", {
        get: function () { return this.EntityPM.MeasurementId; },
        set: function (value) { this.EntityPM.MeasurementId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffChargeComponent.prototype, "CurrencyId", {
        get: function () { return this.EntityPM.CurrencyId; },
        set: function (value) { this.EntityPM.CurrencyId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffChargeComponent.prototype, "ChargeType", {
        get: function () {
            var str = null;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ChargesTypeId)) {
                str = "(" + this.ChargesTypeCode + ") " + this.ChargesTypeName;
            }
            return str;
        },
        enumerable: true,
        configurable: true
    });
    AddEditTariffChargeComponent.prototype.SetDataContext = function (dataContext) {
        this.TarrifHeaderPM = dataContext.TarrifHeaderPM;
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.ChargeTypesQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
        if (this.TarrifHeaderPM.TarrifTypeCode == "S") {
            this.ChargeTypesQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
            this.ChargeTypesQueryFilters.addAdditionalFilter("ChargesGroupCode", "SCH", null, null, "Equals", false, false, false, "string");
        }
        this.Clone();
        //  this.EntityPM.CloneMe();
        //this.SetLabels();
    };
    //Commands 
    AddEditTariffChargeComponent.prototype.CancelButtonClicked = function () {
        // this.EntityPM.RejectChanges();
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditTariffChargeComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.DataContext.EntityPM, this.DataContext.ObjectTableName, errors);
        if (this.TarrifHeaderPM.TarrifTypeCode == "S") {
            if (this.DataContext.ChargesGroupCode == "FRT") {
                errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("TarrifCharge.M.CantAddFreightToSurcharge"));
            }
        }
        if (this.DataContext.ChargesTypeId != null) {
            if (this.DataContext.fatherComponent.TarrifChargesObsList.filter(function (p) { return p.EntityPM != _this.EntityPM && p.ChargesTypeId == _this.ChargesTypeId; })[0]) {
                errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("TarrifCharge.M.TarrifChargeTypeAlreadyAdded"));
            }
        }
        if (this.DataContext.MinPrice != null && this.DataContext.MaxPrice != null && this.DataContext.MinPrice > this.DataContext.MaxPrice) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("TarrifCharge.M.MinLessThanMax"));
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.DataContext.IsNewEntity) {
                this.DataContext.IsNewEntity = false;
                if (this.DataContext.TarrifHeaderPM.TarrifCharges.indexOf(this.EntityPM) == -1) {
                    this.TarrifHeaderPM.TarrifCharges.push(this.EntityPM);
                    this.DataContext.fatherComponent.BuildData();
                }
            }
            this.CurrentSession.CloseCurrentWindow();
            //this.SubmitChanges();
        }
    };
    AddEditTariffChargeComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditTariffChargeComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditTariffChargeComponent);
    return AddEditTariffChargeComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditTariffChargeComponent = AddEditTariffChargeComponent;
var ChargeWindowArgs = /** @class */ (function () {
    function ChargeWindowArgs() {
    }
    return ChargeWindowArgs;
}());
exports.ChargeWindowArgs = ChargeWindowArgs;
//# sourceMappingURL=AddEditTariffChargeComponent.js.map