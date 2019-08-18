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
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var AdvancedDangerousGoodsComponent = /** @class */ (function (_super) {
    __extends(AdvancedDangerousGoodsComponent, _super);
    function AdvancedDangerousGoodsComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    AdvancedDangerousGoodsComponent.prototype.SetWindowArgs = function (entityPM) {
        this.EntityPM = entityPM;
        this.ObjectTableName = "ShipmentPackage";
        this.SetUIProperties();
        this.Clone();
    };
    AdvancedDangerousGoodsComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("CeficClass", this.ObjectTableName, this.EntityPM.IsDangerous);
        this.UIProperties.SetEnabled("KelmerCode", this.ObjectTableName, this.EntityPM.IsDangerous);
        this.UIProperties.SetEnabled("EMS", this.ObjectTableName, this.EntityPM.IsDangerous);
        this.UIProperties.SetEnabled("ProperShippingName", this.ObjectTableName, this.EntityPM.IsDangerous);
        this.UIProperties.SetEnabled("MarinePollutant", this.ObjectTableName, this.EntityPM.IsDangerous);
    };
    Object.defineProperty(AdvancedDangerousGoodsComponent.prototype, "CeficClass", {
        get: function () { return this.EntityPM.CeficClass; },
        set: function (newValue) {
            if (this.EntityPM.CeficClass != newValue) {
                this.EntityPM.CeficClass = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdvancedDangerousGoodsComponent.prototype, "KelmerCode", {
        get: function () { return this.EntityPM.KelmerCode; },
        set: function (newValue) {
            if (this.EntityPM.KelmerCode != newValue) {
                this.EntityPM.KelmerCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdvancedDangerousGoodsComponent.prototype, "EMS", {
        get: function () { return this.EntityPM.EMS; },
        set: function (newValue) {
            if (this.EntityPM.EMS != newValue) {
                this.EntityPM.EMS = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdvancedDangerousGoodsComponent.prototype, "ProperShippingName", {
        get: function () { return this.EntityPM.ProperShippingName; },
        set: function (newValue) {
            if (this.EntityPM.ProperShippingName != newValue) {
                this.EntityPM.ProperShippingName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdvancedDangerousGoodsComponent.prototype, "MarinePollutant", {
        get: function () { return this.EntityPM.MarinePollutant; },
        set: function (newValue) {
            if (this.EntityPM.MarinePollutant != newValue) {
                this.EntityPM.MarinePollutant = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    AdvancedDangerousGoodsComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AdvancedDangerousGoodsComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    };
    AdvancedDangerousGoodsComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('CeficClass');
        this.myCloner.AddField('KelmerCode');
        this.myCloner.AddField('EMS');
        this.myCloner.AddField('ProperShippingName');
        this.myCloner.AddField('MarinePollutant');
        this.myCloner.AddEntity(this.EntityPM);
    };
    AdvancedDangerousGoodsComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AdvancedDangerousGoodsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AdvancedDangerousGoodsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AdvancedDangerousGoodsComponent);
    return AdvancedDangerousGoodsComponent;
}(BaseComponent_1.BaseComponent));
exports.AdvancedDangerousGoodsComponent = AdvancedDangerousGoodsComponent;
//# sourceMappingURL=AdvancedDangerousGoodsComponent.js.map