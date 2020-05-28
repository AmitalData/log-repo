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
Object.defineProperty(exports, "__esModule", { value: true });
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Guid_1 = require("../../../../../Infrastructure/Utilities/Guid");
var DocumentTypeCustomFieldPMViewModel = /** @class */ (function (_super) {
    __extends(DocumentTypeCustomFieldPMViewModel, _super);
    function DocumentTypeCustomFieldPMViewModel(documentTypeCustomFieldPM, tiger) {
        var _this = _super.call(this) || this;
        _this.IsFirstTime = true;
        _this.EntityPM = documentTypeCustomFieldPM;
        _this.Tiger = tiger;
        _this.MultiLine = documentTypeCustomFieldPM.MultiLine;
        _this.keyNo = Guid_1.Guid.newGuid();
        _this.keyYes = Guid_1.Guid.newGuid();
        _this.Id = documentTypeCustomFieldPM.Id;
        _this.FieldCode = documentTypeCustomFieldPM.FieldCode;
        _this.FieldDataTypeCode = documentTypeCustomFieldPM.FieldDataTypeCode;
        _this.Name = documentTypeCustomFieldPM.Name;
        return _this;
    }
    Object.defineProperty(DocumentTypeCustomFieldPMViewModel.prototype, "FieldValue", {
        get: function () { return this.fieldValue; },
        set: function (newValue) {
            if (this.fieldValue != newValue) {
                this.fieldValue = newValue;
                if (this.Tiger && (this.FieldDataTypeCode == "Date" || this.FieldDataTypeCode == "DateTime") && !this.IsFirstTime) {
                    this.Tiger.EditCustomField(this);
                }
                else
                    this.IsFirstTime = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    return DocumentTypeCustomFieldPMViewModel;
}(BaseComponent_1.BaseComponent));
exports.DocumentTypeCustomFieldPMViewModel = DocumentTypeCustomFieldPMViewModel;
//# sourceMappingURL=DocumentTypeCustomFieldPMViewModel.js.map