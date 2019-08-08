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
var MeasurementGeneralComponent = /** @class */ (function (_super) {
    __extends(MeasurementGeneralComponent, _super);
    function MeasurementGeneralComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "Measurement";
        _this.LabelColumnWidth = 160;
        _this.ControlColumnWidth = 240;
        _this.DataContext = _this;
        _this.EntityPM = entityArgs.EntityPM;
        return _this;
    }
    MeasurementGeneralComponent.prototype.ngOnInit = function () {
        this.SetUIProperties();
    };
    MeasurementGeneralComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("Code", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("InActive", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("IsContainer", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("IsContainerMeasurement", this.ObjectTableName, false);
    };
    Object.defineProperty(MeasurementGeneralComponent.prototype, "Name", {
        //Props
        get: function () { return this.EntityPM.Name; },
        set: function (newValue) {
            if (this.EntityPM.Name != newValue) {
                this.EntityPM.Name = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MeasurementGeneralComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (newValue) {
            if (this.EntityPM.LocalName != newValue) {
                this.EntityPM.LocalName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MeasurementGeneralComponent.prototype, "ShortName", {
        get: function () { return this.EntityPM.ShortName; },
        set: function (newValue) {
            if (this.EntityPM.ShortName != newValue) {
                this.EntityPM.ShortName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MeasurementGeneralComponent.prototype, "Code", {
        get: function () { return this.EntityPM.Code; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MeasurementGeneralComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MeasurementGeneralComponent.prototype, "IsContainer", {
        get: function () { return this.EntityPM.IsContainer; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MeasurementGeneralComponent.prototype, "IsContainerMeasurement", {
        get: function () { return this.EntityPM.IsContainerMeasurement; },
        enumerable: true,
        configurable: true
    });
    MeasurementGeneralComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './MeasurementGeneralComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], MeasurementGeneralComponent);
    return MeasurementGeneralComponent;
}(BaseComponent_1.BaseComponent));
exports.MeasurementGeneralComponent = MeasurementGeneralComponent;
//# sourceMappingURL=MeasurementGeneralComponent.js.map