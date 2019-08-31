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
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var WarehouseGeneralTabComponent = /** @class */ (function (_super) {
    __extends(WarehouseGeneralTabComponent, _super);
    function WarehouseGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "Warehouse";
        _this.DataContext = _this;
        _this.IsWarehouseFirmCodeVisible = false;
        _this.EntityPM = entityArgs.EntityPM;
        if (SessionLocator_1.SessionLocator.TenantPM.CountryCode.toUpperCase() == "US") {
            _this.IsWarehouseFirmCodeVisible = true;
        }
        return _this;
    }
    Object.defineProperty(WarehouseGeneralTabComponent.prototype, "Code", {
        get: function () { return this.EntityPM.Code; },
        set: function (newValue) {
            if (this.EntityPM.Code != newValue) {
                this.EntityPM.Code = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseGeneralTabComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (newValue) {
            if (this.EntityPM.EnglishName != newValue) {
                this.EntityPM.EnglishName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseGeneralTabComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (newValue) {
            if (this.EntityPM.LocalName != newValue) {
                this.EntityPM.LocalName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseGeneralTabComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (newValue) {
            if (this.EntityPM.Notes != newValue) {
                this.EntityPM.Notes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseGeneralTabComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (newValue) {
            if (this.EntityPM.InActive != newValue) {
                this.EntityPM.InActive = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseGeneralTabComponent.prototype, "MyWarehouse", {
        get: function () { return this.EntityPM.MyWarehouse; },
        set: function (newValue) {
            if (this.EntityPM.MyWarehouse != newValue) {
                this.EntityPM.MyWarehouse = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseGeneralTabComponent.prototype, "TypeCode", {
        get: function () { return this.EntityPM.TypeCode; },
        set: function (newValue) {
            if (this.EntityPM.TypeCode != newValue) {
                this.EntityPM.TypeCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseGeneralTabComponent.prototype, "FirmCode", {
        get: function () { return this.EntityPM.FirmCode; },
        set: function (newValue) {
            if (this.EntityPM.FirmCode != newValue) {
                this.EntityPM.FirmCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    WarehouseGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './WarehouseGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], WarehouseGeneralTabComponent);
    return WarehouseGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.WarehouseGeneralTabComponent = WarehouseGeneralTabComponent;
//# sourceMappingURL=WarehouseGeneralTabComponent.js.map