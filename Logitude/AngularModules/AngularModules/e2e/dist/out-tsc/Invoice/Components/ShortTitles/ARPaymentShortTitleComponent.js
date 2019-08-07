"use strict";
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
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var ARPaymentShortTitleComponent = /** @class */ (function () {
    function ARPaymentShortTitleComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.DisplaySATSettings = false;
        this.isRTL = false;
        this.EntityPM = this.entityArgs.EntityPM;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        if (this.EntityPM != null) {
            this.BuildComponent();
        }
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            this.DisplaySATSettings = true;
        }
    }
    ARPaymentShortTitleComponent.prototype.BuildComponent = function () {
    };
    Object.defineProperty(ARPaymentShortTitleComponent.prototype, "EntityNumber", {
        get: function () {
            var myResult = "";
            if (this.EntityPM) {
                if (this.EntityPM.PaymentNo) {
                    myResult = this.EntityPM.PaymentNo + ", ";
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    ARPaymentShortTitleComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: "./ARPaymentShortTitleComponent.html",
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ARPaymentShortTitleComponent);
    return ARPaymentShortTitleComponent;
}());
exports.ARPaymentShortTitleComponent = ARPaymentShortTitleComponent;
//# sourceMappingURL=ARPaymentShortTitleComponent.js.map