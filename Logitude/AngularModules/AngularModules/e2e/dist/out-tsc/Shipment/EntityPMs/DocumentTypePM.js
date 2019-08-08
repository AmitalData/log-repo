"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var DocumentTypePM = /** @class */ (function () {
    function DocumentTypePM() {
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(DocumentTypePM.prototype, "Id", {
        get: function () { return this._Id; },
        set: function (newValue) { this._Id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypePM.prototype, "Tenant", {
        get: function () { return this._Tenant; },
        set: function (newValue) { this._Tenant = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypePM.prototype, "Code", {
        get: function () { return this._Code; },
        set: function (newValue) { this._Code = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypePM.prototype, "Name", {
        get: function () { return this._Name; },
        set: function (newValue) { this._Name = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypePM.prototype, "Notes", {
        get: function () { return this._Notes; },
        set: function (newValue) { this._Notes = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypePM.prototype, "IsAir", {
        get: function () { return this._IsAir; },
        set: function (newValue) { this._IsAir = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypePM.prototype, "IsOcean", {
        get: function () { return this._IsOcean; },
        set: function (newValue) { this._IsOcean = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypePM.prototype, "IsInland", {
        get: function () { return this._IsInland; },
        set: function (newValue) { this._IsInland = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypePM.prototype, "IsDocIn", {
        get: function () { return this._IsDocIn; },
        set: function (newValue) { this._IsDocIn = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypePM.prototype, "IsDocOut", {
        get: function () { return this._IsDocOut; },
        set: function (newValue) { this._IsDocOut = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypePM.prototype, "InActive", {
        get: function () { return this._InActive; },
        set: function (newValue) { this._InActive = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypePM.prototype, "TemplateFormatCode", {
        get: function () { return this._TemplateFormatCode; },
        set: function (newValue) { this._TemplateFormatCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypePM.prototype, "IsMaster", {
        get: function () { return this._IsMaster; },
        set: function (newValue) { this._IsMaster = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypePM.prototype, "IsDirect", {
        get: function () { return this._IsDirect; },
        set: function (newValue) { this._IsDirect = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypePM.prototype, "IsHouse", {
        get: function () { return this._IsHouse; },
        set: function (newValue) { this._IsHouse = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypePM.prototype, "IsReadOnly", {
        get: function () { return this._IsReadOnly; },
        set: function (newValue) { this._IsReadOnly = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypePM.prototype, "Subject", {
        get: function () { return this._Subject; },
        set: function (newValue) { this._Subject = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypePM.prototype, "FollowUpTypeId", {
        get: function () { return this._FollowUpTypeId; },
        set: function (newValue) { this._FollowUpTypeId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypePM.prototype, "FollowUpTypeName", {
        get: function () { return this._FollowUpTypeName; },
        set: function (newValue) { this._FollowUpTypeName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypePM.prototype, "ObjectTableId", {
        get: function () { return this._ObjectTableId; },
        set: function (newValue) { this._ObjectTableId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypePM.prototype, "ChildEntityId", {
        get: function () { return this._ChildEntityId; },
        set: function (newValue) { this._ChildEntityId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypePM.prototype, "ChildEntityReference", {
        get: function () { return this._ChildEntityReference; },
        set: function (newValue) { this._ChildEntityReference = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    DocumentTypePM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
    };
    return DocumentTypePM;
}());
exports.DocumentTypePM = DocumentTypePM;
//# sourceMappingURL=DocumentTypePM.js.map