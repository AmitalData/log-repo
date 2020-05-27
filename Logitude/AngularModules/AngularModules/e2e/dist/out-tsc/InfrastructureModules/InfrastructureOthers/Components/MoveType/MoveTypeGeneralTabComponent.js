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
var MoveTypeGeneralTabComponent = /** @class */ (function (_super) {
    __extends(MoveTypeGeneralTabComponent, _super);
    function MoveTypeGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "MoveType";
        _this.EntityPM = _this.entityArgs.EntityPM;
        return _this;
    }
    MoveTypeGeneralTabComponent.prototype.ngOnInit = function () {
        if (this.EntityPM != null) {
        }
    };
    Object.defineProperty(MoveTypeGeneralTabComponent.prototype, "Code", {
        // Properties
        get: function () { return this.EntityPM.Code; },
        set: function (newValue) {
            if (this.EntityPM.Code != newValue) {
                this.EntityPM.Code = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MoveTypeGeneralTabComponent.prototype, "MoveTypeEnglishName", {
        get: function () { return this.EntityPM.MoveTypeEnglishName; },
        set: function (newValue) {
            if (this.EntityPM.MoveTypeEnglishName != newValue) {
                this.EntityPM.MoveTypeEnglishName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MoveTypeGeneralTabComponent.prototype, "MoveTypeLocalName", {
        get: function () { return this.EntityPM.MoveTypeLocalName; },
        set: function (newValue) {
            if (this.EntityPM.MoveTypeLocalName != newValue) {
                this.EntityPM.MoveTypeLocalName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MoveTypeGeneralTabComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (newValue) {
            if (this.EntityPM.InActive != newValue) {
                this.EntityPM.InActive = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MoveTypeGeneralTabComponent.prototype, "IsAir", {
        get: function () { return this.EntityPM.IsAir; },
        set: function (newValue) {
            if (this.EntityPM.IsAir != newValue) {
                this.EntityPM.IsAir = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MoveTypeGeneralTabComponent.prototype, "IsInland", {
        get: function () { return this.EntityPM.IsInland; },
        set: function (newValue) {
            if (this.EntityPM.IsInland != newValue) {
                this.EntityPM.IsInland = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MoveTypeGeneralTabComponent.prototype, "IsOcean", {
        get: function () { return this.EntityPM.IsOcean; },
        set: function (newValue) {
            if (this.EntityPM.IsOcean != newValue) {
                this.EntityPM.IsOcean = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MoveTypeGeneralTabComponent.prototype, "TransportModeId", {
        get: function () { return this.EntityPM.TransportModeId; },
        set: function (newValue) {
            if (this.EntityPM.TransportModeId != newValue) {
                this.EntityPM.TransportModeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    MoveTypeGeneralTabComponent.prototype.SetTransportMode = function (value) {
        this.TransportModeId = value;
    };
    MoveTypeGeneralTabComponent = __decorate([
        core_1.Component({
            selector: 'MoveTypeGeneralTabComponent',
            moduleId: module.id,
            templateUrl: './MoveTypeGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], MoveTypeGeneralTabComponent);
    return MoveTypeGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.MoveTypeGeneralTabComponent = MoveTypeGeneralTabComponent;
//# sourceMappingURL=MoveTypeGeneralTabComponent.js.map