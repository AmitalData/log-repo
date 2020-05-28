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
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var Cloner_1 = require("../../../../../Infrastructure/Utilities/Cloner");
var AddEditOtherChargeComponent = /** @class */ (function (_super) {
    __extends(AddEditOtherChargeComponent, _super);
    function AddEditOtherChargeComponent() {
        var _this = _super.call(this) || this;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    AddEditOtherChargeComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        if (this.DataContext != null) {
            this.DataContext.SetUIProperties();
        }
        this.DataContext.IsWindowMode = true;
        this.ObjectTableName = dataContext.ObjectTableName;
        this.TenantZeroAirlineId = dataContext.ShipmentPM.TenantZeroAirlineId;
        this.Clone();
    };
    AddEditOtherChargeComponent.prototype.ngAfterViewInit = function () {
    };
    AddEditOtherChargeComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.DataContext.IsWindowMode = false;
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditOtherChargeComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        if (this.DataContext.PayablePM != null) {
            Validator_1.Validator.TryValidateObject(this.DataContext.PayablePM, this.DataContext.ObjectTableName, errors);
        }
        else if (this.DataContext.ReceivablePM != null) {
            Validator_1.Validator.TryValidateObject(this.DataContext.ReceivablePM, this.DataContext.ObjectTableName, errors);
        }
        else if (this.DataContext.AWBPrintOnlyPM != null) {
            Validator_1.Validator.TryValidateObject(this.DataContext.AWBPrintOnlyPM, this.DataContext.ObjectTableName, errors);
            if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext.IATACodeId)) {
                errors.push("IATA code field is required");
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext.PrepaidCollectId)) {
                errors.push("P/C field is required");
            }
            if (this.DataContext.CurrencyId != this.DataContext.ShipmentPM.AWBCurrencyId) {
                errors.push("Currency is not matching the shipment awb currency");
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.DataContext.IsNewEntity) {
                this.DataContext.IsNewEntity = false;
                if (this.DataContext.AWBPrintOnlyPM != null) {
                    if (this.DataContext.ShipmentPM.ShipmentAWBPrintOnlies.indexOf(this.DataContext.AWBPrintOnlyPM) == -1) {
                        this.DataContext.ShipmentPM.AddAWBPrintOnly(this.DataContext.AWBPrintOnlyPM);
                    }
                    if (this.DataContext.fatherComponent.ItemsSource.indexOf(this.DataContext) == -1) {
                        this.DataContext.fatherComponent.ItemsSource.push(this.DataContext);
                    }
                }
            }
            this.DataContext.fatherComponent.ComputeTotals();
            this.CurrentSession.CloseCurrentWindow();
            this.DataContext.IsWindowMode = false;
        }
    };
    AddEditOtherChargeComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('IATACodeId');
        this.myCloner.AddField('ChargesTypeId');
        this.myCloner.AddField('PrepaidCollectId');
        this.myCloner.AddField('DueTypeCode');
        this.myCloner.AddField('CurrencyId');
        this.myCloner.AddField('MeasurementId');
        this.myCloner.AddField('Quantity');
        this.myCloner.AddField('UnitPrice');
        this.myCloner.AddField('Amount');
        this.myCloner.AddField('UnitPrice');
        this.myCloner.AddEntity(this.DataContext.PayablePM);
        this.myCloner.AddEntity(this.DataContext.ReceivablePM);
        this.myCloner.AddEntity(this.DataContext.AWBPrintOnlyPM);
        this.myCloner.AddEntity(this.DataContext.ShipmentPM);
    };
    AddEditOtherChargeComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditOtherChargeComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditOtherChargeComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditOtherChargeComponent);
    return AddEditOtherChargeComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditOtherChargeComponent = AddEditOtherChargeComponent;
//# sourceMappingURL=AddEditOtherChargeComponent.js.map