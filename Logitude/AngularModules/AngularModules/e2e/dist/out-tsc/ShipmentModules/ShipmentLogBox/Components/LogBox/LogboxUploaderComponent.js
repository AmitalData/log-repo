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
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var core_1 = require("@angular/core");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var DocumentsFilingPMService_1 = require("../../../../Common/Services/StandardPMs/DocumentsFilingPMService");
var forms_1 = require("@angular/forms");
var ImageLibraryService_1 = require("../../../../Common/Services/Others/ImageLibraryService");
var ImageParameter_1 = require("../../../../Infrastructure/DataContracts/ImageParameter");
var ServiceLocator_1 = require("../../../../Infrastructure/Locators/ServiceLocator");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var LogboxUploaderComponent = /** @class */ (function (_super) {
    __extends(LogboxUploaderComponent, _super);
    function LogboxUploaderComponent(_imageLibraryService, fb) {
        var _this = _super.call(this) || this;
        _this._imageLibraryService = _imageLibraryService;
        _this.DataContext = _this;
        _this.DependencyValue2 = true;
        _this.CurrentDocument = null;
        _this.childEntityId = "";
        _this.UploadedSuccessfully = false;
        _this.UploadingErrorsVisibility = false;
        _this.UploadButtonIsEnabled = true;
        _this.IsUploadDone = false;
        _this.IsCloseButtonVisibile = false;
        _this.IsCancelVisibile = true;
        _this.ProgressBarId = Guid_1.Guid.newGuid();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ShareWithAgent = false;
        _this.myForm = fb.group({});
        _this.UploadFileId = Guid_1.Guid.NewRandomString();
        if (_this.documentsFilingPMService == null) {
            _this.documentsFilingPMService = new DocumentsFilingPMService_1.DocumentsFilingPMService();
        }
        return _this;
    }
    LogboxUploaderComponent.prototype.ngOnInit = function () {
    };
    LogboxUploaderComponent.prototype.SetWindowArgs = function (args) {
        this.ShareWithAgent = args.ShareWithAgent;
        this.File = args.File;
        this.FileSize = args.FileSize;
    };
    LogboxUploaderComponent.prototype.SetDataContext = function (dataContext) {
        this.DataViewModel = dataContext;
        this.EntityId = this.DataViewModel.EntityId;
        this.CurrentDocument = this.DataViewModel.EntityPm;
        if (this.File) {
            this.ContinueUploading(this.File);
        }
    };
    LogboxUploaderComponent.prototype.ShowMessage = function (message) {
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Show(message);
    };
    LogboxUploaderComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.SessionEvent.emit({ Name: "LogBoxUploader", IsUploadDone: this.IsUploadDone, IsUploadCanceled: this.IsUploadCanceled, FileName: this.FileName });
        this.CurrentSession.CloseCurrentWindow();
    };
    LogboxUploaderComponent.prototype.CancelButtonClicked = function () {
        var _this = this;
        if (this.IsUploadDone) {
            this.CurrentSession.SessionEvent.emit({ Name: "LogBoxUploader", IsUploadDone: this.IsUploadDone, IsUploadCanceled: this.IsUploadCanceled, FileName: this.FileName });
            this.CurrentSession.CloseCurrentWindow();
        }
        else {
            if (this.IsUploadInProgress) {
                this.IsUploadCanceled = true;
                this._imageLibraryService.CancelUpload(this.CurrentDocument.DocumentId, this.CurrentDocument.Tenant).subscribe(function (result) {
                    _this.CurrentDocument.DocumentId = null;
                    _this.CurrentDocument.HasFile = false;
                    _this.CurrentDocument.Received = false;
                    _this.CurrentDocument.ReceivedDate = null;
                    _this.CurrentDocument.ReceivedByUserId = null;
                    _this.CurrentDocument.FileExtension = null;
                    _this.CurrentDocument.IsRequested = true;
                    _this.documentsFilingPMService.update(_this.CurrentDocument).subscribe(function (myResult) {
                        _this.IsUploadInProgress = false;
                        _this.IsUploadDone = false;
                        _this.IsUploadCanceled = true;
                        _this.CurrentSession.SessionEvent.emit({ Name: "LogBoxUploader", IsUploadDone: _this.IsUploadDone, IsUploadCanceled: _this.IsUploadCanceled });
                        _this.CurrentSession.CloseCurrentWindow();
                    });
                });
            }
            else {
                this.CurrentSession.SessionEvent.emit({ Name: "LogBoxUploader", IsUploadDone: this.IsUploadDone, IsUploadCanceled: true, FileName: this.FileName ? this.FileName.split('.')[0] : this.FileName });
                this.CurrentSession.CloseCurrentWindow();
            }
        }
    };
    LogboxUploaderComponent.prototype.ContinueUploading = function (file) {
        if (file && file.size > 0) {
            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("LogBox", "Upload Document");
            var temp = file.name.split('.');
            this.FileExtension = temp[temp.length - 1];
            this.FileName = file.name.replace("." + this.FileExtension, "");
            this.UploadButtonIsEnabled = false;
            this.IsUploadInProgress = true;
            this.filterImageParameter = new ImageParameter_1.ImageParameter();
            this.filterImageParameter.IsFirstTry = true;
            this.filterImageParameter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
            this.filterImageParameter.Extension = this.FileExtension;
            this.filterImageParameter.UploadMode = "Block";
            this.filterImageParameter.EntityId = this.CurrentDocument.Id;
            //ServiceLocator.SendTotangoUserActivity("LogBox", "Upload Document");
            this.File = file;
            var filebuffer = null;
            this.filterImageParameter.PartsNumber = this.File.size / 100000;
            if (this.filterImageParameter.PartsNumber > 1) {
                filebuffer = this.File.slice(0, 100000);
            }
            else {
                filebuffer = this.File.slice(0, file.size);
            }
            this.filterImageParameter.FileSize = file.size;
            this.filterImageParameter.SendPartNumber = 1;
            this.filterImageParameter.BufferNumber = -1;
            this.filterImageParameter.SentSize = 0;
            this.filterImageParameter.Buffersize = 100000;
            this.filterImageParameter.IsFirstTry = true;
            this.filterImageParameter.UploadMode = "Block";
            this.filterImageParameter.FileName = this.FileName;
            this.ArrayBufferToBase64(filebuffer, this);
        }
    };
    LogboxUploaderComponent.prototype.ArrayBufferToBase64 = function (file, viewmodel) {
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
    LogboxUploaderComponent.prototype.SendBlockToServer = function (filter) {
        var _this = this;
        this._imageLibraryService.UploadPdfFile(filter).subscribe(function (res) {
            var pmResponse = res;
            var result;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    result = myResult;
                }
            }
            if (result) {
                _this.IncreaseProgressBar(result);
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
                else if (result) {
                    if (_this.CurrentDocument) {
                        if (result.Name) {
                            _this.CurrentDocument.DocumentId = result.Name.split('.')[0];
                        }
                        _this.CurrentDocument.HasFile = true;
                        _this.CurrentDocument.Received = true;
                        _this.CurrentDocument.ReceivedDate = new Date();
                        _this.CurrentDocument.ReceivedByUserId = SessionInfo_1.SessionInfo.LoggedUserId;
                        _this.CurrentDocument.FileExtension = _this.FileExtension;
                        _this.CurrentDocument.IsRequested = false;
                        _this.CurrentDocument.IsDigitallySigned = result.isDigitallySigned;
                        _this.CurrentDocument.SignersList = result.signersList;
                        _this.CurrentDocument.IsSharedWithForwarder = _this.ShareWithAgent;
                        _this.documentsFilingPMService.update(_this.CurrentDocument).subscribe(function (myResult) {
                            _this.IsUploadDone = true;
                            _this.IsUploadInProgress = false;
                            _this.UploadedSuccessfully = true;
                            _this.CloseButtonClicked();
                        });
                    }
                }
            }
        });
    };
    LogboxUploaderComponent.prototype.IncreaseProgressBar = function (filter) {
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
    LogboxUploaderComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'LogBoxUploader',
            templateUrl: './LogboxUploaderComponent.html',
            providers: [ImageLibraryService_1.ImageLibraryService, DocumentsFilingPMService_1.DocumentsFilingPMService],
        }),
        __metadata("design:paramtypes", [ImageLibraryService_1.ImageLibraryService, forms_1.FormBuilder])
    ], LogboxUploaderComponent);
    return LogboxUploaderComponent;
}(BaseComponent_1.BaseComponent));
exports.LogboxUploaderComponent = LogboxUploaderComponent;
//# sourceMappingURL=LogboxUploaderComponent.js.map