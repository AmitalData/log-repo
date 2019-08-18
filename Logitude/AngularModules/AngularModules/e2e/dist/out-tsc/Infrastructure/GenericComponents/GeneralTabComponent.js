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
var EntityArgs_1 = require("../DataContracts/EntityArgs");
var SessionLocator_1 = require("../Utilities/SessionLocator");
var GeneralTabComponent = /** @class */ (function () {
    function GeneralTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.ObjectTableName = null;
        this.IsNewEntity = false;
        this.Retries = 0;
        this.IsNewEntity = entityArgs.IsNewEntity;
        this.ObjectTableName = entityArgs.ObjectTableName;
        this.RunComponent();
    }
    GeneralTabComponent.prototype.RunComponent = function () {
        var _this = this;
        if (this.viewContainerRef) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
                .then(function (cmpRef) {
                var ScreenCode = _this.ObjectTableName + ".GeneralTabScreen";
                if (_this.ObjectTableName == "Shipment" || _this.ObjectTableName == "Master") {
                    if (_this.entityArgs.EntityPM.ShipmentLevelCode == "C") {
                        ScreenCode = "Master.GeneralTabScreen";
                    }
                }
                if (_this.ObjectTableName == "BatchTaskExecution") {
                    ScreenCode = "BatchTaskExecutionGeneralTabScreen";
                }
                cmpRef.instance.EntityArgs = _this.entityArgs;
                cmpRef.instance.Run(_this.entityArgs.EntityPM, _this.ObjectTableName, ScreenCode, _this.IsNewEntity);
            });
        }
        else {
            this.RunComponentTimer();
        }
    };
    GeneralTabComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], GeneralTabComponent.prototype, "viewContainerRef", void 0);
    GeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './GeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], GeneralTabComponent);
    return GeneralTabComponent;
}());
exports.GeneralTabComponent = GeneralTabComponent;
//# sourceMappingURL=GeneralTabComponent.js.map