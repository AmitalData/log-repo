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
var BankCodeGeneralTabComponent = /** @class */ (function (_super) {
    __extends(BankCodeGeneralTabComponent, _super);
    function BankCodeGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.ObjectTableName = "BankCode";
        _this.DataContext = _this;
        _this.EntityPM = entityArgs.EntityPM;
        _this.EntityId = _this.EntityPM.Id;
        _this.ImageId = _this.EntityPM.LogoId;
        return _this;
    }
    Object.defineProperty(BankCodeGeneralTabComponent.prototype, "Code", {
        get: function () { return this.EntityPM.Code; },
        set: function (value) {
            if (this.EntityPM.Code != value) {
                this.EntityPM.Code = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankCodeGeneralTabComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (value) {
            if (this.EntityPM.EnglishName != value) {
                this.EntityPM.EnglishName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankCodeGeneralTabComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (value) {
            if (this.EntityPM.LocalName != value) {
                this.EntityPM.LocalName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankCodeGeneralTabComponent.prototype, "LogoId", {
        get: function () { return this.EntityPM.LogoId; },
        set: function (value) {
            if (this.EntityPM.LogoId != value) {
                this.EntityPM.LogoId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankCodeGeneralTabComponent.prototype, "Inactive", {
        get: function () { return this.EntityPM.Inactive; },
        set: function (value) {
            if (this.EntityPM.Inactive != value) {
                this.EntityPM.Inactive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    BankCodeGeneralTabComponent.prototype.InactiveChecked = function (checked) {
        if (checked) {
            this.Inactive = true;
        }
        else
            this.Inactive = false;
    };
    BankCodeGeneralTabComponent.prototype.ImageUploadedCompleted = function (code) {
        this.ImageId = code;
        this.EntityPM.LogoId = code;
    };
    BankCodeGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './BankCodeGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], BankCodeGeneralTabComponent);
    return BankCodeGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.BankCodeGeneralTabComponent = BankCodeGeneralTabComponent;
//# sourceMappingURL=BankCodeGeneralTabComponent.js.map