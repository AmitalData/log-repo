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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var core_1 = require("@angular/core");
var DocumentExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentExtendedService");
var DownloadManager_1 = require("../../../../Infrastructure/Utilities/DownloadManager");
var ImageLibraryService_1 = require("../../../../Common/Services/Others/ImageLibraryService");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var forms_1 = require("@angular/forms");
var Tools_1 = require("../../../../Infrastructure/Tools");
var CommunicationLogMessageBodyComponent = /** @class */ (function (_super) {
    __extends(CommunicationLogMessageBodyComponent, _super);
    function CommunicationLogMessageBodyComponent(entityArgs, fb, _documentExtendedService, _imageLibraryService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this._documentExtendedService = _documentExtendedService;
        _this._imageLibraryService = _imageLibraryService;
        _this.myForm = fb.group({});
        if (window.innerWidth > 1380) {
            _this.MessageWidth = "1380px";
        }
        else {
            _this.MessageWidth = (window.innerWidth - 250).toString();
        }
        return _this;
    }
    CommunicationLogMessageBodyComponent.prototype.ngOnInit = function () {
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM) {
            if (!this.EntityPM.IsBodySecured) {
                this.GetMessageBodyFileName();
            }
            else {
                this.MessageBody = "The body of this message is secured and cannot be displayed";
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ResponseDocumentId)) {
                this.GetResponseBodyFileName();
            }
            if (this.EntityPM.To == "FTP")
                this.Logs = this.EntityPM.Logs;
        }
    };
    CommunicationLogMessageBodyComponent.prototype.GetMessageBodyFileName = function () {
        var _this = this;
        this._documentExtendedService.GetDocumentById(this.EntityPM.DocumentId, this.EntityPM.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var document = pmResponse.Result;
                if (document) {
                    var filename = document.Id + "." + document.Extension;
                    _this.UpdateScreen(document, "MessageBody");
                }
            }
        });
    };
    CommunicationLogMessageBodyComponent.prototype.GetResponseBodyFileName = function () {
        var _this = this;
        this._documentExtendedService.GetDocumentById(this.EntityPM.ResponseDocumentId, this.EntityPM.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var document = pmResponse.Result;
                if (document) {
                    var filename = document.Id + "." + document.Extension;
                    _this.UpdateScreen(document, "ResponseBody");
                }
            }
        });
    };
    CommunicationLogMessageBodyComponent.prototype.UpdateScreen = function (document, type) {
        var _this = this;
        this._imageLibraryService.DownloadFile(document.Id, document.Extension, document.Folder, this.EntityPM.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    if (type == "MessageBody") {
                        _this.MessageBody = myResult;
                    }
                    if (type == "ResponseBody") {
                        _this.ResponseBody = myResult;
                    }
                    //window.atob(myResult);
                }
            }
        });
    };
    CommunicationLogMessageBodyComponent.prototype.ViewMessageBodyButtonClicked = function () {
        if (this.EntityPM && !this.EntityPM.IsBodySecured) {
            DownloadManager_1.DownloadManager.DownloadPage(this.EntityPM.DocumentId);
        }
    };
    CommunicationLogMessageBodyComponent.prototype.ViewResponseBodyButtonClicked = function () {
        if (this.EntityPM && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ResponseDocumentId)) {
            DownloadManager_1.DownloadManager.DownloadPage(this.EntityPM.ResponseDocumentId);
        }
    };
    CommunicationLogMessageBodyComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'CommunicationLogMessageBody',
            templateUrl: './CommunicationLogMessageBodyComponent.html',
            providers: [DocumentExtendedService_1.DocumentExtendedService, ImageLibraryService_1.ImageLibraryService],
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, forms_1.FormBuilder, DocumentExtendedService_1.DocumentExtendedService, ImageLibraryService_1.ImageLibraryService])
    ], CommunicationLogMessageBodyComponent);
    return CommunicationLogMessageBodyComponent;
}(BaseComponent_1.BaseComponent));
exports.CommunicationLogMessageBodyComponent = CommunicationLogMessageBodyComponent;
//# sourceMappingURL=CommunicationLogMessageBodyComponent.js.map