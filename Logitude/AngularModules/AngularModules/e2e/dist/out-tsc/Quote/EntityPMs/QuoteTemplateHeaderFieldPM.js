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
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var ServiceLocator_1 = require("../../Infrastructure/Locators/ServiceLocator");
var core_1 = require("@angular/core");
var PropertyChangedArgs_1 = require("../../Infrastructure/EventEmitterArgs/PropertyChangedArgs");
var QuoteTemplateHeaderFieldPM = /** @class */ (function () {
    function QuoteTemplateHeaderFieldPM() {
        this.PropertyChanged = new core_1.EventEmitter();
        this.UIProperties = new UIProperties_1.UIProperties(this);
        this.IsDirty = false;
    }
    Object.defineProperty(QuoteTemplateHeaderFieldPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { if (this.id != newValue) {
            this.id = newValue;
            this.MarkAsDirty("Id");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateHeaderFieldPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { if (this.tenant != newValue) {
            this.tenant = newValue;
            this.MarkAsDirty("Tenant");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateHeaderFieldPM.prototype, "QuoteTemplateId", {
        get: function () { return this.quoteTemplateId; },
        set: function (newValue) { if (this.quoteTemplateId != newValue) {
            this.quoteTemplateId = newValue;
            this.MarkAsDirty("QuoteTemplateId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateHeaderFieldPM.prototype, "FieldCode", {
        get: function () { return this.fieldCode; },
        set: function (newValue) { if (this.fieldCode != newValue) {
            this.fieldCode = newValue;
            this.MarkAsDirty("FieldCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateHeaderFieldPM.prototype, "Row", {
        get: function () { return this.row; },
        set: function (newValue) { if (this.row != newValue) {
            this.row = newValue;
            this.MarkAsDirty("Row");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateHeaderFieldPM.prototype, "Column", {
        get: function () { return this.column; },
        set: function (newValue) { if (this.column != newValue) {
            this.column = newValue;
            this.MarkAsDirty("Column");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateHeaderFieldPM.prototype, "IsDelete", {
        get: function () { return this.isDelete; },
        set: function (newValue) { if (this.isDelete != newValue) {
            this.isDelete = newValue;
            this.MarkAsDirty("IsDelete");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateHeaderFieldPM.prototype, "IsAdd", {
        get: function () { return this.isAdd; },
        set: function (newValue) { if (this.isAdd != newValue) {
            this.isAdd = newValue;
            this.MarkAsDirty("IsAdd");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateHeaderFieldPM.prototype, "IsEdit", {
        get: function () { return this.isEdit; },
        set: function (newValue) { if (this.isEdit != newValue) {
            this.isEdit = newValue;
            this.MarkAsDirty("IsEdit");
        } },
        enumerable: true,
        configurable: true
    });
    QuoteTemplateHeaderFieldPM.prototype.MarkAsDirty = function (propertyName) {
        if (propertyName === void 0) { propertyName = null; }
        this.IsDirty = true;
        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs_1.PropertyChangedArgs(propertyName, this));
            ServiceLocator_1.ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "QuoteTemplateHeaderField");
        }
    };
    QuoteTemplateHeaderFieldPM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    QuoteTemplateHeaderFieldPM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], QuoteTemplateHeaderFieldPM.prototype, "PropertyChanged", void 0);
    return QuoteTemplateHeaderFieldPM;
}());
exports.QuoteTemplateHeaderFieldPM = QuoteTemplateHeaderFieldPM;
//# sourceMappingURL=QuoteTemplateHeaderFieldPM.js.map