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
var core_1 = require("@angular/core");
var ImageLibraryService_1 = require("../../../Common/Services/Others/ImageLibraryService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var DownloadManager_1 = require("../../../Infrastructure/Utilities/DownloadManager");
var DocumentsFilingTemplateComponent = /** @class */ (function () {
    function DocumentsFilingTemplateComponent(cd) {
        this.cd = cd;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    DocumentsFilingTemplateComponent.prototype.setVariables = function (rowData, fieldName) {
        this.rowData = rowData;
        this.FieldName = fieldName;
        this.cd.detectChanges();
    };
    DocumentsFilingTemplateComponent.prototype.FirePreventSelect = function () {
        this.CurrentSession.PseventRowSelectEvent.emit("document");
    };
    DocumentsFilingTemplateComponent.prototype.DownloadDocumentFile = function (documentFiling) {
        this.FirePreventSelect();
        this._ImageLibraryService = new ImageLibraryService_1.ImageLibraryService();
        this._ImageLibraryService.DownloadFile(documentFiling.DocumentId, documentFiling.FileExtension, documentFiling.Folder, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var documentName = documentFiling.DocumentId;
            DownloadManager_1.DownloadManager.DownloadPage(documentName);
        });
    };
    DocumentsFilingTemplateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DocumentsFilingTemplateComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], DocumentsFilingTemplateComponent);
    return DocumentsFilingTemplateComponent;
}());
exports.DocumentsFilingTemplateComponent = DocumentsFilingTemplateComponent;
//# sourceMappingURL=DocumentsFilingTemplateComponent.js.map