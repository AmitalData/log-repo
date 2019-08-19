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
var QuoteDocumentVersionPM = /** @class */ (function () {
    function QuoteDocumentVersionPM(_entityParentPM) {
        this.PropertyChanged = new core_1.EventEmitter();
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties_1.UIProperties(this);
        this.IsDirty = false;
    }
    Object.defineProperty(QuoteDocumentVersionPM.prototype, "QuoteId", {
        get: function () { return this.quoteId; },
        set: function (newValue) { if (this.quoteId != newValue) {
            this.quoteId = newValue;
            this.MarkAsDirty("QuoteId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDocumentVersionPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { if (this.tenant != newValue) {
            this.tenant = newValue;
            this.MarkAsDirty("Tenant");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDocumentVersionPM.prototype, "VersionNumber", {
        get: function () { return this.versionNumber; },
        set: function (newValue) { if (this.versionNumber != newValue) {
            this.versionNumber = newValue;
            this.MarkAsDirty("VersionNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDocumentVersionPM.prototype, "CreateDate", {
        get: function () { return this.createDate; },
        set: function (newValue) { if (this.createDate != newValue) {
            this.createDate = newValue;
            this.MarkAsDirty("CreateDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDocumentVersionPM.prototype, "UpdateDate", {
        get: function () { return this.updateDate; },
        set: function (newValue) { if (this.updateDate != newValue) {
            this.updateDate = newValue;
            this.MarkAsDirty("UpdateDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDocumentVersionPM.prototype, "CreatedByUserId", {
        get: function () { return this.createdByUserId; },
        set: function (newValue) { if (this.createdByUserId != newValue) {
            this.createdByUserId = newValue;
            this.MarkAsDirty("CreatedByUserId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDocumentVersionPM.prototype, "UpdatedByUserId", {
        get: function () { return this.updatedByUserId; },
        set: function (newValue) { if (this.updatedByUserId != newValue) {
            this.updatedByUserId = newValue;
            this.MarkAsDirty("UpdatedByUserId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDocumentVersionPM.prototype, "VersionType", {
        get: function () { return this.versionType; },
        set: function (newValue) { if (this.versionType != newValue) {
            this.versionType = newValue;
            this.MarkAsDirty("VersionType");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDocumentVersionPM.prototype, "DocumentId", {
        get: function () { return this.documentId; },
        set: function (newValue) { if (this.documentId != newValue) {
            this.documentId = newValue;
            this.MarkAsDirty("DocumentId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDocumentVersionPM.prototype, "SendDate", {
        get: function () { return this.sendDate; },
        set: function (newValue) { if (this.sendDate != newValue) {
            this.sendDate = newValue;
            this.MarkAsDirty("SendDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDocumentVersionPM.prototype, "IsSent", {
        get: function () { return this.isSent; },
        set: function (newValue) { if (this.isSent != newValue) {
            this.isSent = newValue;
            this.MarkAsDirty("IsSent");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDocumentVersionPM.prototype, "QuoteTemplateId", {
        get: function () { return this.quoteTemplateId; },
        set: function (newValue) { if (this.quoteTemplateId != newValue) {
            this.quoteTemplateId = newValue;
            this.MarkAsDirty("QuoteTemplateId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDocumentVersionPM.prototype, "CreatedByUserName", {
        get: function () { return this.createdByUserName; },
        set: function (newValue) { if (this.createdByUserName != newValue) {
            this.createdByUserName = newValue;
            this.MarkAsDirty("CreatedByUserName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDocumentVersionPM.prototype, "UpdateByUserName", {
        get: function () { return this.updateByUserName; },
        set: function (newValue) { if (this.updateByUserName != newValue) {
            this.updateByUserName = newValue;
            this.MarkAsDirty("UpdateByUserName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDocumentVersionPM.prototype, "VersionTypeName", {
        get: function () { return this.versionTypeName; },
        set: function (newValue) { if (this.versionTypeName != newValue) {
            this.versionTypeName = newValue;
            this.MarkAsDirty("VersionTypeName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDocumentVersionPM.prototype, "FileSize", {
        get: function () { return this.fileSize; },
        set: function (newValue) { if (this.fileSize != newValue) {
            this.fileSize = newValue;
            this.MarkAsDirty("FileSize");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDocumentVersionPM.prototype, "FileName", {
        get: function () { return this.fileName; },
        set: function (newValue) { if (this.fileName != newValue) {
            this.fileName = newValue;
            this.MarkAsDirty("FileName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDocumentVersionPM.prototype, "DisplayVersionTypeName", {
        get: function () { return this.displayVersionTypeName; },
        set: function (newValue) { if (this.displayVersionTypeName != newValue) {
            this.displayVersionTypeName = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDocumentVersionPM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDocumentVersionPM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { if (this.changeSetOp != newValue) {
            this.changeSetOp = newValue;
            this.MarkAsDirty("ChangeSetOp");
        } },
        enumerable: true,
        configurable: true
    });
    QuoteDocumentVersionPM.prototype.MarkAsDirty = function (propertyName) {
        if (propertyName === void 0) { propertyName = null; }
        this.IsDirty = true;
        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }
        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs_1.PropertyChangedArgs(propertyName, this));
            ServiceLocator_1.ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "QuoteDocumentVersionPM");
        }
    };
    QuoteDocumentVersionPM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    QuoteDocumentVersionPM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], QuoteDocumentVersionPM.prototype, "PropertyChanged", void 0);
    return QuoteDocumentVersionPM;
}());
exports.QuoteDocumentVersionPM = QuoteDocumentVersionPM;
//# sourceMappingURL=QuoteDocumentVersionPM.js.map