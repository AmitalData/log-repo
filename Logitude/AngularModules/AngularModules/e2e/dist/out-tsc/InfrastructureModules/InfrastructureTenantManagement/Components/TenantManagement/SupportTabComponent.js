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
var SupportTabComponent = /** @class */ (function (_super) {
    __extends(SupportTabComponent, _super);
    function SupportTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "TenantManagement";
        _this.IsEditingAllowed = false;
        _this.EntityPM = _this.entityArgs.EntityPM;
        return _this;
    }
    SupportTabComponent.prototype.ngOnInit = function () {
        if (this.EntityPM != null) {
            this.SetUIProperties();
        }
    };
    SupportTabComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("SupportEmail", this.ObjectTableName, this.SupportActivated);
    };
    Object.defineProperty(SupportTabComponent.prototype, "SupportEmail", {
        get: function () { return this.EntityPM.SupportEmail; },
        set: function (newValue) {
            if (this.EntityPM.SupportEmail != newValue) {
                this.EntityPM.SupportEmail = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupportTabComponent.prototype, "SupportActivated", {
        get: function () { return this.EntityPM.SupportActivated; },
        set: function (newValue) {
            if (this.EntityPM.SupportActivated != newValue) {
                this.EntityPM.SupportActivated = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    SupportTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SupportTabComponent',
            templateUrl: './SupportTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], SupportTabComponent);
    return SupportTabComponent;
}(BaseComponent_1.BaseComponent));
exports.SupportTabComponent = SupportTabComponent;
//# sourceMappingURL=SupportTabComponent.js.map