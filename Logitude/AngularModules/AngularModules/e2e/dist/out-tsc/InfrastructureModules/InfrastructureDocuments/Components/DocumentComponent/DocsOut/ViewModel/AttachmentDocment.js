"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var AttachmentDocment = /** @class */ (function () {
    // public AttachmentDocment(string fileName, double? fileSize, string documentId, int order)
    function AttachmentDocment(fileName, fileSize, documentId, order) {
        this.DocumentId = documentId;
        this.FileName = fileName;
        this.FileSize = fileSize;
        this.Order = order;
    }
    return AttachmentDocment;
}());
exports.AttachmentDocment = AttachmentDocment;
//# sourceMappingURL=AttachmentDocment.js.map