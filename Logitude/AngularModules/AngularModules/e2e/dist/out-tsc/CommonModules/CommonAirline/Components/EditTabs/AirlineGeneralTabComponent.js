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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var AirlineGeneralTabComponent = /** @class */ (function (_super) {
    __extends(AirlineGeneralTabComponent, _super);
    function AirlineGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "Airline";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ScreenCode = "Airline.GeneralTabScreen";
        _this.ImageId = "";
        _this.EntityId = "";
        _this.EntityName = "";
        _this.Retries = 0;
        _this.EntityPM = entityArgs.EntityPM;
        _this.ImageId = _this.EntityPM.ImageDetailId;
        _this.EntityName = "Airline";
        _this.EntityId = _this.EntityPM.Id;
        _this.RunComponent();
        return _this;
    }
    AirlineGeneralTabComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    AirlineGeneralTabComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    AirlineGeneralTabComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.Run(_this.entityArgs.EntityPM, _this.entityArgs.ObjectTableName, _this.ScreenCode);
        });
    };
    AirlineGeneralTabComponent.prototype.ImageUploadedCompleted = function (code) {
        this.ImageId = code;
        this.EntityPM.ImageDetailId = code;
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], AirlineGeneralTabComponent.prototype, "viewContainerRef", void 0);
    AirlineGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AirlineGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], AirlineGeneralTabComponent);
    return AirlineGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.AirlineGeneralTabComponent = AirlineGeneralTabComponent;
//# sourceMappingURL=AirlineGeneralTabComponent.js.map