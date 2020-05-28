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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var AttachmentsList_1 = require("../DocsOut/Filters/AttachmentsList");
var DownloadManager_1 = require("../../../../../Infrastructure/Utilities/DownloadManager");
var AttachmentDocsInComponent = /** @class */ (function () {
    function AttachmentDocsInComponent() {
        this.OnCloseAttachmentDocsInEvent = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    AttachmentDocsInComponent.prototype.ngOnInit = function () {
    };
    AttachmentDocsInComponent.prototype.SetWindowArgs = function (args) {
        this.DocumentsFilingList = args.DocumentsFilingList;
        this.OnCloseAttachmentDocsInEvent = args.OnCloseAttachmentDocsInEvent;
        this.SelectDocumentsFilingPM = new Array();
    };
    AttachmentDocsInComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CurrentWindow.Close("");
    };
    AttachmentDocsInComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.AttachmentsLists = new Array();
        this.SelectDocumentsFilingPM.forEach(function (doc) {
            var item = new AttachmentsList_1.AttachmentsList();
            item.Id = doc.DocumentId;
            item.DocumentFilingId = doc.Id;
            item.Tenant = doc.Tenant;
            item.FileSize = doc.FileSize;
            item.FileExtension = doc.FileExtension;
            item.DocumentTypeCopyNameWithDocumentTypeName = doc.DocumentTypeName;
            item.ShowRemoveLink = true;
            _this.AttachmentsLists.push(item);
        });
        this.OnCloseAttachmentDocsInEvent.emit(this.AttachmentsLists);
        this.CloseButtonClicked();
    };
    AttachmentDocsInComponent.prototype.CheckboxClick = function (item) {
        var selectitem = this.SelectDocumentsFilingPM.filter(function (d) { return d.Id == item.Id; })[0];
        if (!item.IsAttachSelect) {
            if (selectitem == null) {
                this.SelectDocumentsFilingPM.push(item);
            }
            item.IsAttachSelect = true;
        }
        else {
            if (selectitem != null) {
                this.SelectDocumentsFilingPM = this.SelectDocumentsFilingPM.filter(function (d) { return d.Id != selectitem.Id; });
            }
            item.IsAttachSelect = false;
        }
    };
    AttachmentDocsInComponent.prototype.View = function (item) {
        if (item != null) {
            DownloadManager_1.DownloadManager.DownloadPage("", item.SecurityId);
        }
    };
    AttachmentDocsInComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AttachDocsIn',
            templateUrl: './AttachmentDocsInComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AttachmentDocsInComponent);
    return AttachmentDocsInComponent;
}());
exports.AttachmentDocsInComponent = AttachmentDocsInComponent;
//# sourceMappingURL=AttachmentDocsInComponent.js.map