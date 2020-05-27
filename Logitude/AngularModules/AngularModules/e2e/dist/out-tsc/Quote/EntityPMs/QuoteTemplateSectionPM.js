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
var QuoteTemplateSectionPM = /** @class */ (function () {
    function QuoteTemplateSectionPM() {
        this.PropertyChanged = new core_1.EventEmitter();
        this.UIProperties = new UIProperties_1.UIProperties(this);
        this.IsDirty = false;
    }
    Object.defineProperty(QuoteTemplateSectionPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { if (this.id != newValue) {
            this.id = newValue;
            this.MarkAsDirty("Id");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateSectionPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { if (this.tenant != newValue) {
            this.tenant = newValue;
            this.MarkAsDirty("Tenant");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateSectionPM.prototype, "QuoteTemplateId", {
        get: function () { return this.quoteTemplateId; },
        set: function (newValue) { if (this.quoteTemplateId != newValue) {
            this.quoteTemplateId = newValue;
            this.MarkAsDirty("QuoteTemplateId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateSectionPM.prototype, "SectionDocId", {
        get: function () { return this.sectionDocId; },
        set: function (newValue) { if (this.sectionDocId != newValue) {
            this.sectionDocId = newValue;
            this.MarkAsDirty("SectionDocId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateSectionPM.prototype, "Order", {
        get: function () { return this.order; },
        set: function (newValue) { if (this.order != newValue) {
            this.order = newValue;
            this.MarkAsDirty("Order");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateSectionPM.prototype, "Name", {
        get: function () { return this.name; },
        set: function (newValue) { if (this.name != newValue) {
            this.name = newValue;
            this.MarkAsDirty("Name");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateSectionPM.prototype, "QuoteTemplateSectionTypeCode", {
        get: function () { return this.quoteTemplateSectionTypeCode; },
        set: function (newValue) { if (this.quoteTemplateSectionTypeCode != newValue) {
            this.quoteTemplateSectionTypeCode = newValue;
            this.MarkAsDirty("QuoteTemplateSectionTypeCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateSectionPM.prototype, "Templatedata", {
        get: function () { return this.templatedata; },
        set: function (newValue) { if (this.templatedata != newValue) {
            this.templatedata = newValue;
            this.MarkAsDirty("Templatedata");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateSectionPM.prototype, "Description", {
        get: function () { return this.description; },
        set: function (newValue) { if (this.description != newValue) {
            this.description = newValue;
            this.MarkAsDirty("Description");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateSectionPM.prototype, "QuoteId", {
        get: function () { return this.quoteId; },
        set: function (newValue) { if (this.quoteId != newValue) {
            this.quoteId = newValue;
            this.MarkAsDirty("QuoteId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateSectionPM.prototype, "IschangeBodySection", {
        get: function () { return this.ischangeBodySection; },
        set: function (newValue) { if (this.ischangeBodySection != newValue) {
            this.ischangeBodySection = newValue;
            this.MarkAsDirty("IschangeBodySection");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateSectionPM.prototype, "IsSettingTypeCodeS", {
        get: function () { return this.isSettingTypeCodeS; },
        set: function (newValue) { if (this.isSettingTypeCodeS != newValue) {
            this.isSettingTypeCodeS = newValue;
            this.MarkAsDirty("IsSettingTypeCodeS");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateSectionPM.prototype, "IsSettingTypeCodeP", {
        get: function () { return this.isSettingTypeCodeP; },
        set: function (newValue) { if (this.isSettingTypeCodeP != newValue) {
            this.isSettingTypeCodeP = newValue;
            this.MarkAsDirty("IsSettingTypeCodeP");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateSectionPM.prototype, "IsCancel", {
        get: function () { return this.isCancel; },
        set: function (newValue) { if (this.isCancel != newValue) {
            this.isCancel = newValue;
            this.MarkAsDirty("IsCancel");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateSectionPM.prototype, "IsQuoteEdited", {
        get: function () { return this.isQuoteEdited; },
        set: function (newValue) { if (this.isQuoteEdited != newValue) {
            this.isQuoteEdited = newValue;
            this.MarkAsDirty("IsQuoteEdited");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateSectionPM.prototype, "IsExcluded", {
        get: function () { return this.isExcluded; },
        set: function (newValue) { if (this.isExcluded != newValue) {
            this.isExcluded = newValue;
            this.MarkAsDirty("IsExcluded");
        } },
        enumerable: true,
        configurable: true
    });
    QuoteTemplateSectionPM.prototype.MarkAsDirty = function (propertyName) {
        if (propertyName === void 0) { propertyName = null; }
        this.IsDirty = true;
        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs_1.PropertyChangedArgs(propertyName, this));
            ServiceLocator_1.ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "QuoteTemplateSection");
        }
    };
    QuoteTemplateSectionPM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    QuoteTemplateSectionPM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], QuoteTemplateSectionPM.prototype, "PropertyChanged", void 0);
    return QuoteTemplateSectionPM;
}());
exports.QuoteTemplateSectionPM = QuoteTemplateSectionPM;
//# sourceMappingURL=QuoteTemplateSectionPM.js.map