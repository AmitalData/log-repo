"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var DocumentTypeCustomFieldsViewModel = /** @class */ (function () {
    function DocumentTypeCustomFieldsViewModel(entityPM) {
        this.EntityPM = entityPM;
        this.Id = entityPM.Id;
        this.KeyInActive = entityPM.Id + "Active";
        this.KeyInRequired = entityPM.Id + "Required";
        this.DefaultValue = entityPM.DefaultValue;
        this.FieldCode = entityPM.FieldCode;
        this.FieldDataTypeCode = entityPM.FieldDataTypeCode;
        this.Name = entityPM.Name;
        this.IsRequired = entityPM.IsRequired;
        this.InActive = entityPM.InActive;
    }
    return DocumentTypeCustomFieldsViewModel;
}());
exports.DocumentTypeCustomFieldsViewModel = DocumentTypeCustomFieldsViewModel;
//# sourceMappingURL=DocumentTypeCustomFieldsViewModel.js.map