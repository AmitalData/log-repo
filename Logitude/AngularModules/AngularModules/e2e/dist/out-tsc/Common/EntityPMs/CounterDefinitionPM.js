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
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ServiceLocator_1 = require("../../Infrastructure/Locators/ServiceLocator");
var core_1 = require("@angular/core");
var PropertyChangedArgs_1 = require("../../Infrastructure/EventEmitterArgs/PropertyChangedArgs");
var CounterDefinitionPM = /** @class */ (function () {
    function CounterDefinitionPM() {
        this.PropertyChanged = new core_1.EventEmitter();
        this.UIProperties = new UIProperties_1.UIProperties(this);
        this.IsDirty = false;
    }
    Object.defineProperty(CounterDefinitionPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { if (this.id != newValue) {
            this.id = newValue;
            this.MarkAsDirty("Id");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterDefinitionPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { if (this.tenant != newValue) {
            this.tenant = newValue;
            this.MarkAsDirty("Tenant");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterDefinitionPM.prototype, "Parameter1", {
        get: function () { return this.parameter1; },
        set: function (newValue) { if (this.parameter1 != newValue) {
            this.parameter1 = newValue;
            this.MarkAsDirty("Parameter1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterDefinitionPM.prototype, "Parameter2", {
        get: function () { return this.parameter2; },
        set: function (newValue) { if (this.parameter2 != newValue) {
            this.parameter2 = newValue;
            this.MarkAsDirty("Parameter2");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterDefinitionPM.prototype, "Prefix", {
        get: function () { return this.prefix; },
        set: function (newValue) { if (this.prefix != newValue) {
            this.prefix = newValue;
            this.MarkAsDirty("Prefix");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterDefinitionPM.prototype, "CounterId", {
        get: function () { return this.counterId; },
        set: function (newValue) { if (this.counterId != newValue) {
            this.counterId = newValue;
            this.MarkAsDirty("CounterId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterDefinitionPM.prototype, "StartNumber", {
        get: function () { return this.startNumber; },
        set: function (newValue) { if (this.startNumber != newValue) {
            this.startNumber = newValue;
            this.MarkAsDirty("StartNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterDefinitionPM.prototype, "StartNumber_Old", {
        get: function () { return this.startNumber_Old; },
        set: function (newValue) { if (this.startNumber_Old != newValue) {
            this.startNumber_Old = newValue;
            this.MarkAsDirty("StartNumber_Old");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterDefinitionPM.prototype, "IsUsed", {
        get: function () { return this.isUsed; },
        set: function (newValue) { if (this.isUsed != newValue) {
            this.isUsed = newValue;
            this.MarkAsDirty("IsUsed");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterDefinitionPM.prototype, "UniquePerPrefix", {
        get: function () { return this.uniquePerPrefix; },
        set: function (newValue) { if (this.uniquePerPrefix != newValue) {
            this.uniquePerPrefix = newValue;
            this.MarkAsDirty("UniquePerPrefix");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterDefinitionPM.prototype, "CounterSize", {
        get: function () { return this.counterSize; },
        set: function (newValue) { if (this.counterSize != newValue) {
            this.counterSize = newValue;
            this.MarkAsDirty("CounterSize");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterDefinitionPM.prototype, "Suffix", {
        get: function () { return this.suffix; },
        set: function (newValue) { if (this.suffix != newValue) {
            this.suffix = newValue;
            this.MarkAsDirty("Suffix");
        } },
        enumerable: true,
        configurable: true
    });
    CounterDefinitionPM.prototype.MarkAsDirty = function (propertyName) {
        if (propertyName === void 0) { propertyName = null; }
        this.IsDirty = true;
        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs_1.PropertyChangedArgs(propertyName, this));
            ServiceLocator_1.ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "CounterDefinition");
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], CounterDefinitionPM.prototype, "PropertyChanged", void 0);
    return CounterDefinitionPM;
}());
exports.CounterDefinitionPM = CounterDefinitionPM;
//# sourceMappingURL=CounterDefinitionPM.js.map