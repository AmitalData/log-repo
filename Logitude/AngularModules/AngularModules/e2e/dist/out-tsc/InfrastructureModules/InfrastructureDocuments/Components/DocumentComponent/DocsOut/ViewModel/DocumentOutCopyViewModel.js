"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var DocumentOutCopyViewModel = /** @class */ (function () {
    function DocumentOutCopyViewModel(documentOutCopyPM) {
        this.Id = documentOutCopyPM.Id;
        this.Tenant = documentOutCopyPM.Tenant;
        this.DocumentId = documentOutCopyPM.DocumentId;
        this.DocumentOutId = documentOutCopyPM.DocumentOutId;
        this.DocumentTypeCopyId = documentOutCopyPM.DocumentTypeCopyId;
        this.DocumentTypeCopyNameWithDocumentTypeName = documentOutCopyPM.DocumentTypeCopyNameWithDocumentTypeName;
        this.LastPrintedByUserId = documentOutCopyPM.LastPrintedByUserId;
        this.LastPrintDate = documentOutCopyPM.LastPrintDate;
        this.FileSize = documentOutCopyPM.FileSize;
        this.FileName = documentOutCopyPM.FileName;
        this.LastPrintedByUserName = documentOutCopyPM.LastPrintedByUserName;
        this.IsAttachSelect = documentOutCopyPM.IsAttachSelect;
        console.log(this.Id);
    }
    return DocumentOutCopyViewModel;
}());
exports.DocumentOutCopyViewModel = DocumentOutCopyViewModel;
//# sourceMappingURL=DocumentOutCopyViewModel.js.map