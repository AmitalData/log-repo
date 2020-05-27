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
var ServiceHelper_1 = require("../../../../Infrastructure/Utilities/ServiceHelper");
var ImageLibraryService_1 = require("../../../../Common/Services/Others/ImageLibraryService");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var forms_1 = require("@angular/forms");
var APILogsResponceBodyComponent = /** @class */ (function (_super) {
    __extends(APILogsResponceBodyComponent, _super);
    function APILogsResponceBodyComponent(entityArgs, fb, _documentExtendedService, _imageLibraryService) {
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
    APILogsResponceBodyComponent.prototype.ngOnInit = function () {
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM) {
            this.MessageBody = this.EntityPM.ResponseData;
        }
    };
    APILogsResponceBodyComponent.prototype.GetFileName = function () {
        var HeaderData = this.EntityPM.Tenant + "_" + this.EntityPM.Id + "_Res"; // +"." + CurrentDocument.Extension;
        //string uri = SessionLocator.GetServerPath() + "/WebPages/APILogsDownLoadPage.aspx?header=" + HeaderData;
        var url = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "/WebPages/APILogsDownLoadPage.aspx?header=" + HeaderData;
        window.open(url);
        //this._documentExtendedService.GetDocumentById(this.EntityPM.DocumentId, this.EntityPM.Tenant).subscribe(res => {
        //    var pmResponse: ServiceResponse = res;
        //    if (!pmResponse.HasError) {
        //        var document = pmResponse.Result;
        //        if (document) {
        //            var filename = document.Id + "." + document.Extension;
        //            this.UpdateScreen(document);
        //        }
        //    }
        //});
    };
    APILogsResponceBodyComponent.prototype.UpdateScreen = function (document) {
        //this._imageLibraryService.DownloadFile(document.Id, document.Extension, document.Folder, this.EntityPM.Tenant).subscribe(res => {
        //    var pmResponse: ServiceResponse = res;
        //    if (!pmResponse.HasError) {
        //        var myResult = pmResponse.Result;
        //        if (myResult) {
        //            this.MessageBody = myResult;//window.atob(myResult);
        //            console.log(this.MessageBody);
        //        }
        //    }
        //});
    };
    APILogsResponceBodyComponent.prototype.ViewButtonClicked = function () {
        var HeaderData = this.EntityPM.Tenant + "_" + this.EntityPM.Id + "_Res"; // +"." + CurrentDocument.Extension;
        //string uri = SessionLocator.GetServerPath() + "/WebPages/APILogsDownLoadPage.aspx?header=" + HeaderData;
        var url = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "/WebPages/APILogsDownLoadPage.aspx?header=" + HeaderData;
        window.open(url);
    };
    APILogsResponceBodyComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'APILogsResponceBody',
            templateUrl: './APILogsResponceBodyComponent.html',
            providers: [DocumentExtendedService_1.DocumentExtendedService, ImageLibraryService_1.ImageLibraryService],
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, forms_1.FormBuilder, DocumentExtendedService_1.DocumentExtendedService, ImageLibraryService_1.ImageLibraryService])
    ], APILogsResponceBodyComponent);
    return APILogsResponceBodyComponent;
}(BaseComponent_1.BaseComponent));
exports.APILogsResponceBodyComponent = APILogsResponceBodyComponent;
//# sourceMappingURL=APILogsResponceBodyComponent.js.map