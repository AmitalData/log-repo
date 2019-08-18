"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var DocumentPM = /** @class */ (function () {
    function DocumentPM() {
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(DocumentPM.prototype, "FileName", {
        get: function () { return this.fileName; },
        set: function (newValue) { this.fileName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentPM.prototype, "FileSize", {
        get: function () { return this.fileSize; },
        set: function (newValue) { this.fileSize = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentPM.prototype, "Extension", {
        get: function () { return this.extension; },
        set: function (newValue) { this.extension = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentPM.prototype, "CalculatedFileName", {
        get: function () { return this.calculatedFileName; },
        set: function (newValue) { this.calculatedFileName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    DocumentPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
    };
    return DocumentPM;
}());
exports.DocumentPM = DocumentPM;
//# sourceMappingURL=DocumentPM.js.map