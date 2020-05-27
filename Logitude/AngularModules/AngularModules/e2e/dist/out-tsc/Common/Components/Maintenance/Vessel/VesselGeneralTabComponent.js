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
var Tools_1 = require("../../../../Infrastructure/Tools");
var VesselGeneralTabComponent = /** @class */ (function (_super) {
    __extends(VesselGeneralTabComponent, _super);
    function VesselGeneralTabComponent(args) {
        var _this = _super.call(this) || this;
        _this.args = args;
        _this.DataContext = _this;
        _this.ObjectTableName = "Vessel";
        _this.IsNewEntity = true;
        _this.EntityPM = args.EntityPM;
        if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Id)) {
            _this.IsNewEntity = true;
        }
        else {
            _this.IsNewEntity = false;
        }
        return _this;
    }
    Object.defineProperty(VesselGeneralTabComponent.prototype, "Code", {
        get: function () { return this.EntityPM.Code; },
        set: function (value) {
            if (this.EntityPM.Code != value) {
                this.EntityPM.Code = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VesselGeneralTabComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (value) {
            if (this.EntityPM.EnglishName != value) {
                this.EntityPM.EnglishName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VesselGeneralTabComponent.prototype, "IMOCode", {
        get: function () { return this.EntityPM.IMOCode; },
        set: function (value) {
            if (this.EntityPM.IMOCode != value) {
                this.EntityPM.IMOCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VesselGeneralTabComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (value) {
            if (this.EntityPM.LocalName != value) {
                this.EntityPM.LocalName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VesselGeneralTabComponent.prototype, "CountryId", {
        get: function () { return this.EntityPM.CountryId; },
        set: function (value) {
            if (this.EntityPM.CountryId != value) {
                this.EntityPM.CountryId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VesselGeneralTabComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (value) {
            if (this.EntityPM.Notes != value) {
                this.EntityPM.Notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VesselGeneralTabComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (value) {
            if (this.EntityPM.InActive != value) {
                this.EntityPM.InActive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    VesselGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './VesselGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], VesselGeneralTabComponent);
    return VesselGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.VesselGeneralTabComponent = VesselGeneralTabComponent;
//# sourceMappingURL=VesselGeneralTabComponent.js.map