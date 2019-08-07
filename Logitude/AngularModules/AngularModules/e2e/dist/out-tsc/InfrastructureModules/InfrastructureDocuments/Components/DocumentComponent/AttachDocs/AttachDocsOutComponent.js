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
var AttachmentsList_1 = require("../DocsOut/Filters/AttachmentsList");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var Guid_1 = require("../../../../../Infrastructure/Utilities/Guid");
var AttachDocsOutComponent = /** @class */ (function () {
    function AttachDocsOutComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    AttachDocsOutComponent.prototype.ngOnInit = function () {
    };
    AttachDocsOutComponent.prototype.SetDataContext = function (dataContext) {
        var _this = this;
        this.DataContext = dataContext;
        this.DocumentCopiesList = [];
        this.DataContext.DocumentCopiesList.forEach(function (item) {
            item.Key = Guid_1.Guid.newGuid();
            _this.DocumentCopiesList.push(item);
        });
        this.DocumentCopiesList.sort(function (a, b) {
            if (a.DocumentTypeCopyNameWithDocumentTypeName.toLowerCase() < b.DocumentTypeCopyNameWithDocumentTypeName.toLowerCase()) {
                return -1;
            }
            else if (a.DocumentTypeCopyNameWithDocumentTypeName.toLowerCase() > b.DocumentTypeCopyNameWithDocumentTypeName.toLowerCase()) {
                return 1;
            }
            else {
                return 0;
            }
        });
        this.SelectedDocumentsList = new Array();
    };
    AttachDocsOutComponent.prototype.CloseButtonClicked = function () {
        this.DataContext.ReloadFroalaEditor();
        if (this.DataContext.AttachmentsLists.length != null && this.DataContext.AttachmentsLists.length > 0) {
            this.DataContext.IsShowAttachmentList = true;
        }
        this.DataContext.IsEnableLinkDocOout = true;
        this.CurrentSession.CurrentWindow.Close("");
    };
    AttachDocsOutComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        console.log(this.SelectedDocumentsList);
        this.AttachmentsLists = new Array();
        this.SelectedDocumentsList.forEach(function (doc) {
            var item = new AttachmentsList_1.AttachmentsList();
            item.Id = doc.Id;
            item.Tenant = doc.Tenant;
            item.FileSize = doc.FileSize;
            item.DocumentTypeCopyNameWithDocumentTypeName = doc.DocumentTypeCopyNameWithDocumentTypeName;
            item.ShowRemoveLink = true;
            _this.AttachmentsLists.push(item);
        });
        console.log(this.AttachmentsLists);
        this.DataContext.BliudAttachmentList(this.AttachmentsLists);
        this.DataContext.IsEnableLinkDocOout = true;
        this.CloseButtonClicked();
    };
    AttachDocsOutComponent.prototype.CheckboxClick = function (item) {
        var selectitem = this.SelectedDocumentsList.filter(function (d) { return d.Id == item.Id; })[0];
        if (!item.IsAttachSelect) {
            if (selectitem == null) {
                this.SelectedDocumentsList.push(item);
            }
            item.IsAttachSelect = true;
        }
        else {
            if (selectitem != null) {
                this.SelectedDocumentsList = this.SelectedDocumentsList.filter(function (d) { return d.Id != selectitem.Id; });
            }
            item.IsAttachSelect = false;
        }
    };
    AttachDocsOutComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AttachDocsOut',
            templateUrl: './AttachDocsOutComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AttachDocsOutComponent);
    return AttachDocsOutComponent;
}());
exports.AttachDocsOutComponent = AttachDocsOutComponent;
//# sourceMappingURL=AttachDocsOutComponent.js.map