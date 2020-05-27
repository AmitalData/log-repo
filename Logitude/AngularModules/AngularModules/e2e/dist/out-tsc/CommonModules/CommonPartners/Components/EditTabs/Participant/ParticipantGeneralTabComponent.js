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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var ParticipantGeneralTabComponent = /** @class */ (function (_super) {
    __extends(ParticipantGeneralTabComponent, _super);
    function ParticipantGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "Participant";
        _this.ScreenCode = "Participant.AdditionalFields";
        _this.Retries = 0;
        _this.EntityPM = _this.entityArgs.EntityPM;
        _this.SetUIProperties();
        _this.RunComponent();
        return _this;
    }
    ParticipantGeneralTabComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("IsDirect", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("RegistrationRequested", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Code", this.ObjectTableName, false);
    };
    ParticipantGeneralTabComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    ParticipantGeneralTabComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.Run(_this.entityArgs.EntityPM, _this.entityArgs.ObjectTableName, _this.ScreenCode);
        });
    };
    ParticipantGeneralTabComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    Object.defineProperty(ParticipantGeneralTabComponent.prototype, "Code", {
        get: function () { return this.EntityPM.Code; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ParticipantGeneralTabComponent.prototype, "RegistrationRequested", {
        get: function () { return this.EntityPM.RegistrationRequested; },
        set: function (value) { if (this.EntityPM.RegistrationRequested != value)
            this.EntityPM.RegistrationRequested = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ParticipantGeneralTabComponent.prototype, "IsDirect", {
        get: function () { return this.EntityPM.IsDirect; },
        set: function (value) { if (this.EntityPM.IsDirect != value)
            this.EntityPM.IsDirect = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ParticipantGeneralTabComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (value) { if (this.EntityPM.EnglishName != value)
            this.EntityPM.EnglishName = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ParticipantGeneralTabComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (value) { if (this.EntityPM.LocalName != value)
            this.EntityPM.LocalName = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ParticipantGeneralTabComponent.prototype, "TTY", {
        get: function () { return this.EntityPM.TTY; },
        set: function (value) { if (this.EntityPM.TTY != value)
            this.EntityPM.TTY = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ParticipantGeneralTabComponent.prototype, "Registered", {
        get: function () { return this.EntityPM.Registered; },
        set: function (value) { if (this.EntityPM.Registered != value)
            this.EntityPM.Registered = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ParticipantGeneralTabComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (value) { if (this.EntityPM.InActive != value)
            this.EntityPM.InActive = value; },
        enumerable: true,
        configurable: true
    });
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], ParticipantGeneralTabComponent.prototype, "viewContainerRef", void 0);
    ParticipantGeneralTabComponent = __decorate([
        core_1.Component({
            selector: 'NewCurrencyComponent',
            moduleId: module.id,
            templateUrl: './ParticipantGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ParticipantGeneralTabComponent);
    return ParticipantGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ParticipantGeneralTabComponent = ParticipantGeneralTabComponent;
//# sourceMappingURL=ParticipantGeneralTabComponent.js.map