"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var DateTimeToTimePipe_1 = require("../../../../../../Infrastructure/Pipes/DateTimeToTimePipe");
var DateTimeToDatePipe_1 = require("../../../../../../Controls/Pipes/DateTimeToDatePipe");
var Tools_1 = require("../../../../../../Infrastructure/Tools");
var Guid_1 = require("../../../../../../Infrastructure/Utilities/Guid");
var DocumentCopiesViewModel = /** @class */ (function () {
    function DocumentCopiesViewModel(documentTypeCopy, documentOutPM, currentEntityId, childEntityId, currentObjectTableId, childObjectTableId, documenttype, entityReference) {
        this.IsDiableSelctedDocumentTypeCopy = false;
        this.IsPrintButtonEnabled = false;
        this.IsCheckBoxesVisible = true;
        this.DivSelectBackground = "";
        this.Key = Guid_1.Guid.newGuid();
        this.CurrentDocumentOut = documentOutPM;
        this.IsHideSetSelectedAsDefaultBtn = true;
        this.Id = documentTypeCopy.Id;
        this.CurrentObjectTableId = currentObjectTableId;
        this.ChildEntityId = childEntityId;
        this.CurrentEntityId = currentEntityId;
        this.ChildObjectTableId = childObjectTableId;
        this.CurrentDocumentType = documenttype;
        if (this.CurrentDocumentType) {
            if (this.CurrentDocumentType.IsDocumentOneTimePrintLimited) {
                this.IsCheckBoxesVisible = false;
            }
        }
        this.CurrentDocumentTypeCopy = documentTypeCopy;
        this.IndexOrder = this.CurrentDocumentTypeCopy.IndexOrder;
        this.Name = documentTypeCopy.Name;
        this.Code = documentTypeCopy.Code;
        if (documentOutPM) {
            this.DocumentOutCopies = documentOutPM.DocumentOutCopies;
            this.CurrentDocumentOutCopy = this.DocumentOutCopies.filter(function (d) { return d.DocumentTypeCopyId == documentTypeCopy.Id; })[0];
            if (this.CurrentDocumentOutCopy != null) {
                this.Exists = true;
                this.IsButtonsStackPanelVisible = true;
            }
            else {
                this.Exists = false;
                this.IsButtonsStackPanelVisible = false;
            }
            this.IsSelected = this.Exists;
            this.PrintedByMessage = "";
            if (this.CurrentDocumentOutCopy != null) {
                if (this.CurrentDocumentType.IsDocumentOneTimePrintLimited && this.CurrentDocumentType.LimitedPrintCopyId == this.CurrentDocumentOutCopy.DocumentTypeCopyId && this.CurrentDocumentOutCopy.LastPrintedByUserId != null && this.CurrentDocumentOutCopy.LastPrintedByUserId != undefined) {
                    var data = "";
                    if (this.CurrentDocumentOutCopy.LastPrintDate) {
                        var date = DateTimeToDatePipe_1.DateTimeToDatePipe.Pipe(this.CurrentDocumentOutCopy.LastPrintDate);
                        var time = DateTimeToTimePipe_1.DateTimeToTimePipe.Pipe(this.CurrentDocumentOutCopy.LastPrintDate);
                        date = date + " " + time;
                    }
                    this.PrintedByMessage = "This document is already printed by " + this.CurrentDocumentOutCopy.LastPrintedByUserName + " at " + date;
                }
            }
        }
        if (this.CurrentDocumentType) {
            if (this.CurrentDocumentType.DocumentTypeCopies.length == 1) {
                this.CurrentDocumentTypeCopy.IsSelectedByDefault = true;
                this.IsDiableSelctedDocumentTypeCopy = true;
                this.IsHideSetSelectedAsDefaultBtn = true;
            }
            else {
                this.IsDiableSelctedDocumentTypeCopy = false;
                this.IsHideSetSelectedAsDefaultBtn = false;
            }
        }
        this.IsSelectedByDefault = this.CurrentDocumentTypeCopy.IsSelectedByDefault;
        this.IsPrintButtonEnabled = true;
        if (this.CurrentDocumentOutCopy != null) {
            if (this.CurrentDocumentType.IsDocumentOneTimePrintLimited && this.CurrentDocumentType.LimitedPrintCopyId == this.CurrentDocumentOutCopy.DocumentTypeCopyId && !Tools_1.AppTool.IsNullOrEmpty(this.CurrentDocumentOutCopy.LastPrintedByUserId)) {
                this.IsPrintButtonEnabled = false;
            }
        }
        this.PrintedByMessage = "";
        if (this.CurrentDocumentOutCopy) {
            if (this.CurrentDocumentType.IsDocumentOneTimePrintLimited && this.CurrentDocumentType.LimitedPrintCopyId == this.CurrentDocumentOutCopy.DocumentTypeCopyId && !Tools_1.AppTool.IsNullOrEmpty(this.CurrentDocumentOutCopy.LastPrintedByUserId)) {
                var data = "";
                if (this.CurrentDocumentOutCopy.LastPrintDate) {
                    var date = DateTimeToDatePipe_1.DateTimeToDatePipe.Pipe(this.CurrentDocumentOutCopy.LastPrintDate);
                    var time = DateTimeToTimePipe_1.DateTimeToTimePipe.Pipe(this.CurrentDocumentOutCopy.LastPrintDate);
                    date = date + " " + time;
                }
                this.PrintedByMessage = "This document is already printed by " + this.CurrentDocumentOutCopy.LastPrintedByUserName + " at " + date;
            }
        }
    }
    DocumentCopiesViewModel.prototype.RefereshDocumentOutCopies = function (item) {
        var _this = this;
        this.CurrentDocumentOut = item;
        this.DocumentOutCopies = this.CurrentDocumentOut.DocumentOutCopies;
        this.CurrentDocumentOutCopy = this.CurrentDocumentOut.DocumentOutCopies.filter(function (d) { return d.DocumentTypeCopyId == _this.CurrentDocumentTypeCopy.Id; })[0];
        if (this.CurrentDocumentType.DocumentTypeCopies.length == 1) {
            this.IsDiableSelctedDocumentTypeCopy = true;
            this.IsHideSetSelectedAsDefaultBtn = true;
        }
        else {
            this.IsDiableSelctedDocumentTypeCopy = false;
            this.IsHideSetSelectedAsDefaultBtn = false;
        }
        if (this.CurrentDocumentOutCopy != null) {
            this.IsHideSetSelectedAsDefaultBtn = true;
            this.Exists = true;
            this.IsSelected = this.Exists;
            this.IsButtonsStackPanelVisible = true;
        }
        else {
            // this.IsHideSetSelectedAsDefaultBtn = true;
            this.IsButtonsStackPanelVisible = false;
        }
    };
    return DocumentCopiesViewModel;
}());
exports.DocumentCopiesViewModel = DocumentCopiesViewModel;
//# sourceMappingURL=DocumentCopiesViewModel.js.map