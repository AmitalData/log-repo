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
var CounterPM = /** @class */ (function () {
    function CounterPM() {
        this.PropertyChanged = new core_1.EventEmitter();
        this.UIProperties = new UIProperties_1.UIProperties(this);
        this.IsDirty = false;
    }
    Object.defineProperty(CounterPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { if (this.id != newValue) {
            this.id = newValue;
            this.MarkAsDirty("Id");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { if (this.tenant != newValue) {
            this.tenant = newValue;
            this.MarkAsDirty("Tenant");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterPM.prototype, "Code", {
        get: function () { return this.code; },
        set: function (newValue) { if (this.code != newValue) {
            this.code = newValue;
            this.MarkAsDirty("Code");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterPM.prototype, "Name", {
        get: function () { return this.name; },
        set: function (newValue) { if (this.name != newValue) {
            this.name = newValue;
            this.MarkAsDirty("Name");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterPM.prototype, "ObjectTableId", {
        get: function () { return this.objectTableId; },
        set: function (newValue) { if (this.objectTableId != newValue) {
            this.objectTableId = newValue;
            this.MarkAsDirty("ObjectTableId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterPM.prototype, "ChangedByUserId", {
        get: function () { return this.changedByUserId; },
        set: function (newValue) { if (this.changedByUserId != newValue) {
            this.changedByUserId = newValue;
            this.MarkAsDirty("ChangedByUserId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterPM.prototype, "ChangedDate", {
        get: function () { return this.changedDate; },
        set: function (newValue) { if (this.changedDate != newValue) {
            this.changedDate = newValue;
            this.MarkAsDirty("ChangedDate");
        } },
        enumerable: true,
        configurable: true
    });
    CounterPM.prototype.MarkAsDirty = function (propertyName) {
        if (propertyName === void 0) { propertyName = null; }
        this.IsDirty = true;
        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs_1.PropertyChangedArgs(propertyName, this));
            ServiceLocator_1.ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "Counter");
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], CounterPM.prototype, "PropertyChanged", void 0);
    return CounterPM;
}());
exports.CounterPM = CounterPM;
//# sourceMappingURL=CounterPM.js.map