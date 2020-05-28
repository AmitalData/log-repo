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
var DangerousPackageComponent = /** @class */ (function (_super) {
    __extends(DangerousPackageComponent, _super);
    function DangerousPackageComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    DangerousPackageComponent.prototype.SetWindowArgs = function (entityPM) {
        this.EntityPM = entityPM;
        this.ObjectTableName = "Booking";
        this.SetUIProperties();
        this.Clone();
    };
    DangerousPackageComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("DangerousClassNumber", this.ObjectTableName, this.IsDangerous);
        this.UIProperties.SetEnabled("DangerousUnNumber", this.ObjectTableName, this.IsDangerous);
        this.UIProperties.SetEnabled("DangerousPackagingGroup", this.ObjectTableName, this.IsDangerous);
        this.UIProperties.SetEnabled("DangerousIMDGCode", this.ObjectTableName, this.IsDangerous);
        this.UIProperties.SetEnabled("DangerousFlashPoint", this.ObjectTableName, this.IsDangerous);
        this.UIProperties.SetEnabled("DangerousMaterialDescription", this.ObjectTableName, this.IsDangerous);
    };
    Object.defineProperty(DangerousPackageComponent.prototype, "IsDangerous", {
        get: function () { return this.EntityPM.IsDangerous; },
        set: function (newValue) {
            if (this.EntityPM.IsDangerous != newValue) {
                this.EntityPM.IsDangerous = newValue;
                if (!newValue) {
                    this.DangerousClassNumber = null;
                    this.DangerousUnNumber = null;
                    this.DangerousPackagingGroup = null;
                    this.DangerousIMDGCode = null;
                    this.DangerousFlashPoint = null;
                    this.DangerousMaterialDescription = null;
                }
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DangerousPackageComponent.prototype, "DangerousClassNumber", {
        get: function () { return this.EntityPM.DangerousClassNumber; },
        set: function (newValue) {
            if (this.EntityPM.DangerousClassNumber != newValue) {
                this.EntityPM.DangerousClassNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DangerousPackageComponent.prototype, "DangerousUnNumber", {
        get: function () { return this.EntityPM.DangerousUnNumber; },
        set: function (newValue) {
            if (this.EntityPM.DangerousUnNumber != newValue) {
                this.EntityPM.DangerousUnNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DangerousPackageComponent.prototype, "DangerousPackagingGroup", {
        get: function () { return this.EntityPM.DangerousPackagingGroup; },
        set: function (newValue) {
            if (this.EntityPM.DangerousPackagingGroup != newValue) {
                this.EntityPM.DangerousPackagingGroup = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DangerousPackageComponent.prototype, "DangerousIMDGCode", {
        get: function () { return this.EntityPM.DangerousIMDGCode; },
        set: function (newValue) {
            if (this.EntityPM.DangerousIMDGCode != newValue) {
                this.EntityPM.DangerousIMDGCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DangerousPackageComponent.prototype, "DangerousFlashPoint", {
        get: function () { return this.EntityPM.DangerousFlashPoint; },
        set: function (newValue) {
            if (this.EntityPM.DangerousFlashPoint != newValue) {
                this.EntityPM.DangerousFlashPoint = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DangerousPackageComponent.prototype, "DangerousMaterialDescription", {
        get: function () { return this.EntityPM.DangerousMaterialDescription; },
        set: function (newValue) {
            if (this.EntityPM.DangerousMaterialDescription != newValue) {
                this.EntityPM.DangerousMaterialDescription = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    DangerousPackageComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    DangerousPackageComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DangerousFlashPoint) && this.DangerousFlashPoint.length > 8) {
            errors.push("Flash Point field max length is 8");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DangerousIMDGCode) && this.DangerousIMDGCode.length > 4) {
            errors.push("IMDG Code field max length is 4");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DangerousUnNumber) && this.DangerousUnNumber.length > 4) {
            errors.push("Un Number field max length is 4");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DangerousClassNumber) && this.DangerousClassNumber.length > 10) {
            errors.push("Class Number field max length is 10");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DangerousPackagingGroup) && this.DangerousPackagingGroup.length > 10) {
            errors.push("Packaging Group field max length is 10");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DangerousMaterialDescription) && this.DangerousMaterialDescription.length > 30) {
            errors.push("Material Description field max length is 30");
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    };
    DangerousPackageComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('IsDangerous');
        this.myCloner.AddField('DangerousClassNumber');
        this.myCloner.AddField('DangerousUnNumber');
        this.myCloner.AddField('DangerousPackagingGroup');
        this.myCloner.AddField('DangerousIMDGCode');
        this.myCloner.AddField('DangerousFlashPoint');
        this.myCloner.AddField('DangerousMaterialDescription');
        this.myCloner.AddEntity(this.EntityPM);
    };
    DangerousPackageComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    DangerousPackageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DangerousPackageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DangerousPackageComponent);
    return DangerousPackageComponent;
}(BaseComponent_1.BaseComponent));
exports.DangerousPackageComponent = DangerousPackageComponent;
//# sourceMappingURL=DangerousPackageComponent.js.map