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
var QuoteFollowUpPM = /** @class */ (function () {
    function QuoteFollowUpPM(_entityParentPM) {
        this.PropertyChanged = new core_1.EventEmitter();
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties_1.UIProperties(this);
        this.IsDirty = false;
    }
    Object.defineProperty(QuoteFollowUpPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "ShipmentId", {
        get: function () { return this.shipmentId; },
        set: function (newValue) { this.shipmentId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "JobId", {
        get: function () { return this.jobId; },
        set: function (newValue) { this.jobId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "QuoteId", {
        get: function () { return this.quoteId; },
        set: function (newValue) { this.quoteId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "ExternalDocumentId", {
        get: function () { return this.externalDocumentId; },
        set: function (newValue) { this.externalDocumentId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "Date", {
        get: function () { return this.date; },
        set: function (newValue) { this.date = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "IsNew", {
        get: function () { return this.isNew; },
        set: function (newValue) { this.isNew = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "Note", {
        get: function () { return this.note; },
        set: function (newValue) { this.note = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "DoneNote", {
        get: function () { return this.doneNote; },
        set: function (newValue) { this.doneNote = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "DoneDateTime", {
        get: function () { return this.doneDateTime; },
        set: function (newValue) { this.doneDateTime = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "Done", {
        get: function () { return this.done; },
        set: function (newValue) { this.done = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "InternalDocumentId", {
        get: function () { return this.internalDocumentId; },
        set: function (newValue) { this.internalDocumentId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "LegType", {
        get: function () { return this.legType; },
        set: function (newValue) { this.legType = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "Deleted", {
        get: function () { return this.deleted; },
        set: function (newValue) { this.deleted = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "EntityDateId", {
        get: function () { return this.entityDateId; },
        set: function (newValue) { this.entityDateId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "EventTypeId", {
        get: function () { return this.eventTypeId; },
        set: function (newValue) { this.eventTypeId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "EventTypeFollowUpName", {
        get: function () { return this.eventTypeFollowUpName; },
        set: function (newValue) { this.eventTypeFollowUpName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "ManualActivatedFollowUp", {
        get: function () { return this.manualActivatedFollowUp; },
        set: function (newValue) { this.manualActivatedFollowUp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "OwnerUserId", {
        get: function () { return this.ownerUserId; },
        set: function (newValue) { this.ownerUserId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "OwnerUserName", {
        get: function () { return this.ownerUserName; },
        set: function (newValue) { this.ownerUserName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "DocumentTypeId", {
        get: function () { return this.documentTypeId; },
        set: function (newValue) { this.documentTypeId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "AutomationId", {
        get: function () { return this.automationId; },
        set: function (newValue) { this.automationId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "Area", {
        get: function () { return this.area; },
        set: function (newValue) { this.area = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteFollowUpPM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { if (this.changeSetOp != newValue) {
            this.changeSetOp = newValue;
            this.MarkAsDirty("ChangeSetOp");
        } },
        enumerable: true,
        configurable: true
    });
    QuoteFollowUpPM.prototype.MarkAsDirty = function (propertyName) {
        if (propertyName === void 0) { propertyName = null; }
        this.IsDirty = true;
        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }
        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs_1.PropertyChangedArgs(propertyName, this));
            ServiceLocator_1.ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "ShipmentReceivable");
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], QuoteFollowUpPM.prototype, "PropertyChanged", void 0);
    return QuoteFollowUpPM;
}());
exports.QuoteFollowUpPM = QuoteFollowUpPM;
//# sourceMappingURL=QuoteFollowUpPM.js.map