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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var core_1 = require("@angular/core");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var DocumentsFilingExtendedPMService_1 = require("../../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var DocumentsFilingPMService_1 = require("../../../../../Common/Services/StandardPMs/DocumentsFilingPMService");
var forms_1 = require("@angular/forms");
var ImageLibraryService_1 = require("../../../../../Common/Services/Others/ImageLibraryService");
var ImageParameter_1 = require("../../../../../Infrastructure/DataContracts/ImageParameter");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var Guid_1 = require("../../../../../Infrastructure/Utilities/Guid");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var ServiceLocator_1 = require("../../../../../Infrastructure/Locators/ServiceLocator");
var AttachmentUploaderComponent = /** @class */ (function (_super) {
    __extends(AttachmentUploaderComponent, _super);
    function AttachmentUploaderComponent(_imageLibraryService, fb, _documentsFilingExtendedPMService) {
        var _this = _super.call(this) || this;
        _this._imageLibraryService = _imageLibraryService;
        _this._documentsFilingExtendedPMService = _documentsFilingExtendedPMService;
        _this.DataContext = _this;
        _this.DependencyValue2 = true;
        _this.IsUploadVisibile = false;
        _this.CurrentDocument = null;
        _this.DocumentsFilingHasFile = false;
        _this.childEntityId = "";
        _this.IsShowProgressBar = false;
        _this.UploadedSuccessfully = false;
        _this.UploadingErrorsVisibility = false;
        _this.UploadButtonIsEnabled = true;
        _this.IsUploadDone = false;
        _this.IsCloseButtonVisibile = false;
        _this.IsCancelVisibile = true;
        _this.ProgressBarId = Guid_1.Guid.newGuid();
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.myForm = fb.group({});
        _this.UploadFileId = Guid_1.Guid.NewRandomString();
        if (_this.documentsFilingPMService == null) {
            _this.documentsFilingPMService = new DocumentsFilingPMService_1.DocumentsFilingPMService();
        }
        return _this;
    }
    AttachmentUploaderComponent.prototype.ngOnInit = function () {
        this._entityResourceService.getEntityResourceByTableName("DocsIn").subscribe(function (response) {
        });
    };
    AttachmentUploaderComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.CurrentDocument = args.CurrentDocument;
        this.EntityId = args.EntityId;
        this.ObjectTableId = args.ObjectTableId;
        this.Tenant = SessionLocator_1.SessionLocator.Tenant;
        this.RequsetPageName = args.RequsetPageName;
        this.TiggerViewModel = args.TiggerViewModel;
        this.Entity = args.Entity;
        var table = window.ObjectTables.filter(function (d) { return d.Id == _this.ObjectTableId; })[0];
        if (table)
            this.ObjectTableName = table.Name;
        this.Run();
    };
    AttachmentUploaderComponent.prototype.Run = function () {
        if (this.RequsetPageName == "SendControl")
            this.LoadDocumentsFiling();
        else {
            this.IsUploadVisibile = true;
        }
    };
    AttachmentUploaderComponent.prototype.LoadDocumentsFiling = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this._documentsFilingExtendedPMService.getDocumentsFilingsByEntityIdAndObjectTable(this.EntityId, this.childEntityId, this.ObjectTableId, "I", this.Tenant, false).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.externalDocs = myResult;
                }
            }
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
        });
    };
    AttachmentUploaderComponent.prototype.NextButtonClcik = function () {
        var _this = this;
        if (this.DocumentTypeId) {
            if (this.externalDocs != null) {
                this.CurrentDocument = this.externalDocs.filter(function (d) { return d.DocumentTypeId == _this.DocumentTypeId; })[0];
                if (this.CurrentDocument == null) {
                    this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
                    this._documentsFilingExtendedPMService.CreateDocumentsFiling(this.DocumentTypeId, this.EntityId, this.childEntityId, "", this.ObjectTableId, "I", this.Tenant).subscribe(function (res) {
                        var pmResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;
                            if (myResult) {
                                _this.CurrentDocument = myResult;
                            }
                        }
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        _this.IsUploadVisibile = true;
                    });
                }
                else {
                    this.IsUploadVisibile = true;
                }
            }
        }
        else {
            this.ShowMessage("Please choose a document type to upload");
        }
    };
    AttachmentUploaderComponent.prototype.ShowMessage = function (message) {
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Show(message);
    };
    AttachmentUploaderComponent.prototype.CloseButtonClicked = function () {
        if (this.RequsetPageName != "DocIn" && this.RequsetPageName != "SharedDocument") {
            if (this.TiggerViewModel)
                this.TiggerViewModel.OnUploadComplete(this);
        }
        this.CurrentSession.CurrentWindow.Close("");
    };
    AttachmentUploaderComponent.prototype.CancelButtonClicked = function () {
        var _this = this;
        if (this.IsUploadDone) {
            this.CloseButtonClicked();
        }
        else {
            if (this.IsUploadInProgress) {
                this.IsUploadCanceled = true;
                this._imageLibraryService.CancelUpload(this.CurrentDocument.Id, this.CurrentDocument.Tenant).subscribe(function (result) {
                    _this.IsUploadInProgress = false;
                    _this.IsUploadDone = false;
                    _this.IsUploadCanceled = true;
                    _this.CloseButtonClicked();
                });
            }
            else {
                this.CloseButtonClicked();
            }
        }
    };
    AttachmentUploaderComponent.prototype.OpenUpLoadFile = function () {
        document.getElementById(this.UploadFileId).click();
    };
    AttachmentUploaderComponent.prototype.UploadFile = function (event) {
        var _this = this;
        var file = attachmentUploader(this.UploadFileId);
        //document.querySelector('#UploadFile').files[0];
        if (file && file.size > 0) {
            this.FileName = file.name;
            var fileInfo = file.name.split('.');
            if (fileInfo.length > 1) {
                this.FileExtension = fileInfo[fileInfo.length - 1];
            }
            else {
                this.FileExtension = fileInfo[1];
            }
            this._documentsFilingExtendedPMService.GetFileSizeAndUnit(file.size).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        _this.FileSize = myResult;
                    }
                }
                if (_this.FileExtension && _this.FileExtension.length > 10) {
                    _this.ShowMessage("File extension should be less than or equal 10 characters");
                }
                else {
                    _this.IsUploadVisibile = true;
                    _this.IsShowProgressBar = true;
                    _this.UploadButtonIsEnabled = false;
                    _this.IsUploadInProgress = true;
                    _this.filterImageParameter = new ImageParameter_1.ImageParameter();
                    _this.filterImageParameter.IsFirstTry = true;
                    _this.filterImageParameter.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    _this.filterImageParameter.Extension = _this.FileExtension;
                    _this.filterImageParameter.UploadMode = "AttachmentUploader";
                    _this.filterImageParameter.EntityId = _this.CurrentDocument.Id;
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity(_this.ObjectTableName, "UploadDocsIn");
                    _this.File = file;
                    var filebuffer = null;
                    _this.filterImageParameter.PartsNumber = _this.File.size / 100000;
                    if (_this.filterImageParameter.PartsNumber > 1)
                        filebuffer = _this.File.slice(0, 100000);
                    else
                        filebuffer = _this.File.slice(0, file.size);
                    _this.filterImageParameter.FileSize = file.size;
                    _this.filterImageParameter.SendPartNumber = 1;
                    _this.filterImageParameter.BufferNumber = -1;
                    _this.filterImageParameter.SentSize = 0;
                    _this.filterImageParameter.IsFirstTry = true;
                    _this.filterImageParameter.FileName = _this.FileName;
                    _this.ArrayBufferToBase64(filebuffer, _this);
                }
            });
        }
    };
    AttachmentUploaderComponent.prototype.ArrayBufferToBase64 = function (file, viewmodel) {
        var reader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(ResultAsArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }
            viewmodel.filterImageParameter.Base64String = window.btoa(binary);
            viewmodel.SendBlockToServer(viewmodel.filterImageParameter);
        };
        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
    };
    AttachmentUploaderComponent.prototype.SendBlockToServer = function (filter) {
        var _this = this;
        this._imageLibraryService.UploadFile(filter).subscribe(function (res) {
            var pmResponse = res;
            var result = null;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    result = myResult;
                }
            }
            if (result) {
                _this.filterImageParameter = result;
                if (result.SentSize < result.FileSize && !_this.IsUploadCanceled) {
                    var filebuffer = null;
                    if ((result.FileSize - result.SentSize) >= 100000) {
                        filebuffer = _this.File.slice(result.SentSize, result.SentSize + 100000);
                    }
                    else {
                        filebuffer = _this.File.slice(result.SentSize, result.FileSize);
                    }
                    _this.ArrayBufferToBase64(filebuffer, _this);
                    _this.IsUploadInProgress = true;
                }
                else {
                    if (result.Result) {
                        if (_this.CurrentDocument) {
                            _this.CurrentDocument.DocumentId = result.Result.split('.')[0];
                            _this.CurrentDocument.HasFile = true;
                            _this.CurrentDocument.Received = true;
                            _this.CurrentDocument.ReceivedDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                            _this.CurrentDocument.ReceivedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                            _this.CurrentDocument.FileExtension = _this.FileExtension;
                            _this.CurrentDocument.FileSize = result.FileSize;
                            _this.CurrentDocument.FileName = _this.FileName;
                            _this.CurrentDocument.ReceivedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                            if (_this.RequsetPageName == "DocIn" || _this.RequsetPageName == "SharedDocument") {
                                if (_this.CurrentDocument.IsSharedOut) {
                                    _this.CurrentDocument.IsUpdateSharedDocument = true;
                                }
                            }
                            _this.documentsFilingPMService.update(_this.CurrentDocument).subscribe(function (myownResult) {
                                var pmResponse = myownResult;
                                if (!pmResponse.HasError) {
                                    var myResult1 = pmResponse.Result;
                                    if (myResult1) {
                                        _this.CurrentDocument.IsUpdateSharedDocument = false;
                                        _this.IsCloseButtonVisibile = true;
                                        _this.IsCancelVisibile = false;
                                        _this.IsUploadDone = true;
                                        _this.IsUploadInProgress = false;
                                        _this.UploadedSuccessfully = true;
                                        if ((_this.RequsetPageName == "DocIn" || _this.RequsetPageName == "SharedDocument") && _this.TiggerViewModel) {
                                            if (_this.RequsetPageName == "DocIn")
                                                _this.TiggerViewModel.OnUploadComplete();
                                            else if (_this.RequsetPageName == "SharedDocument")
                                                _this.TiggerViewModel.OnUploadComplete(_this.Entity);
                                        }
                                    }
                                }
                                else {
                                    if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                                        _this.ShowMessage(pmResponse.ErrorsArray[0]);
                                    }
                                }
                            });
                        }
                    }
                }
                _this.IncreaseProgressBar(result);
            }
        });
    };
    AttachmentUploaderComponent.prototype.IncreaseProgressBar = function (filter) {
        if (!this.IsUploadCanceled) {
            var pre = 100 / filter.BlocksNumber;
            var ProgressBarValue = (filter.BufferNumber + 1) * pre;
            var elem = document.getElementById(this.ProgressBarId);
            if (elem) {
                if (ProgressBarValue == 100) {
                    elem.style.width = (ProgressBarValue - 0.6) + '%';
                    this.ProgressBarPercentText = ProgressBarValue.toString() + ' %';
                }
                else {
                    elem.style.width = ProgressBarValue + '%';
                    this.ProgressBarPercentText = ProgressBarValue.toFixed(2).toString() + ' %';
                }
            }
        }
    };
    AttachmentUploaderComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AttachExternal',
            templateUrl: './AttachmentUploaderComponent.html',
            providers: [DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService, ImageLibraryService_1.ImageLibraryService, DocumentsFilingPMService_1.DocumentsFilingPMService],
        }),
        __metadata("design:paramtypes", [ImageLibraryService_1.ImageLibraryService, forms_1.FormBuilder, DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService])
    ], AttachmentUploaderComponent);
    return AttachmentUploaderComponent;
}(BaseComponent_1.BaseComponent));
exports.AttachmentUploaderComponent = AttachmentUploaderComponent;
//# sourceMappingURL=AttachmentUploaderComponent.js.map