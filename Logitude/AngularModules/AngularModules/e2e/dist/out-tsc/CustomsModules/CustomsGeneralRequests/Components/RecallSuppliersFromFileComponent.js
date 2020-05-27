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
var core_1 = require("@angular/core");
var CustomMessageWrapperComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var VendorMessagesService_1 = require("../../../Customs/Services/WebServices/VendorMessagesService");
var CreditQueryRequestParams_1 = require("../../../Customs/DataContract/RequestParams/CreditQueryRequestParams");
var Tools_1 = require("../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageProgressComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var ImageParameter_1 = require("../../../Infrastructure/DataContracts/ImageParameter");
var DocumentsFilingExtendedPMService_1 = require("../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var RecallSuppliersFromFileComponent = /** @class */ (function (_super) {
    __extends(RecallSuppliersFromFileComponent, _super);
    function RecallSuppliersFromFileComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.CustomsVendor";
        _this.UploadButtonIsEnabled = true;
        _this._VendorMessagesService = new VendorMessagesService_1.VendorMessagesService();
        _this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService();
        _this.UploadFileId = Guid_1.Guid.NewRandomString();
        _this.IsShowProgressBar = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.TransactionsResultList = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    RecallSuppliersFromFileComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    RecallSuppliersFromFileComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new CreditQueryRequestParams_1.CreditQueryRequestParams();
        }
        if (this.ResponseData) {
            if (this.ResponseData.TransactionsList) {
                this.TransactionsResultList.InsertCollection(this.ResponseData.TransactionsList);
            }
        }
    };
    Object.defineProperty(RecallSuppliersFromFileComponent.prototype, "AgentExternalID", {
        //#region Properties
        get: function () { return this.RequestParams ? this.RequestParams.AgentExternalId : null; },
        set: function (value) {
            if (this.RequestParams.AgentExternalId != value) {
                this.RequestParams.AgentExternalId = value;
            }
            if (value) {
                this.UIProperties.SetEnabled("AgentExternalID", null, false);
            }
            else {
                this.UIProperties.SetEnabled("AgentExternalID", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion Properties
    //#region General Commands
    RecallSuppliersFromFileComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    RecallSuppliersFromFileComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        if (this.filterImageParameter != null && this.filterImageParameter.Base64String != null) {
            this.SendRecallMessageToServer(this.filterImageParameter);
        }
    };
    //CancelButtonClicked() {
    //    if (this.IsUploadDone) {
    //        this.CloseButtonClicked();
    //    }
    //    else {
    //        if (this.IsUploadInProgress) {
    //            this.IsUploadCanceled = true;
    //            this._imageLibraryService.CancelUpload(this.CurrentDocument.Id, this.CurrentDocument.Tenant).subscribe(result => {
    //                this.IsUploadInProgress = false;
    //                this.IsUploadDone = false;
    //                this.IsUploadCanceled = true;
    //                this.CloseButtonClicked();
    //            });
    //        }
    //        else {
    //            this.CloseButtonClicked();
    //        }
    //    }
    //}
    RecallSuppliersFromFileComponent.prototype.ShowMessage = function (message) {
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Show(message);
    };
    RecallSuppliersFromFileComponent.prototype.OpenUpLoadFile = function () {
        document.getElementById(this.UploadFileId).click();
    };
    RecallSuppliersFromFileComponent.prototype.UploadFile = function (event) {
        var file = attachmentUploader(this.UploadFileId);
        if (file) {
            var temp = file.name.split('.');
            this.FileExtension = temp[temp.length - 1];
            this.FileName = file.name.replace("." + this.FileExtension, "");
            if (this.FileExtension != "csv") {
                this.ShowMessage("חובה קובץ CSV");
                return;
            }
            this.File = file;
            if (this.FileExtension && this.FileExtension.length > 10) {
                this.ShowMessage("File extension should be less than or equal 10 characters");
            }
            else {
                this.IsShowProgressBar = true;
                this.UploadButtonIsEnabled = false;
                this.filterImageParameter = new ImageParameter_1.ImageParameter();
                this.filterImageParameter.Key = Guid_1.Guid.newGuid();
                this.filterImageParameter.IsFirstTry = true;
                this.filterImageParameter.Extension = this.FileExtension;
                this.filterImageParameter.UploadMode = "Block";
                this.filterImageParameter.FileSize = file.size;
                this.filterImageParameter.Tenant = SessionLocator_1.SessionLocator.Tenant;
                this.ArrayBufferToBase64(file, this);
            }
        }
    };
    RecallSuppliersFromFileComponent.prototype.ArrayBufferToBase64 = function (file, viewmodel) {
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
            viewmodel.IncreaseProgressBar(100);
        };
        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
    };
    RecallSuppliersFromFileComponent.prototype.SendRecallMessageToServer = function (filter) {
        var _this = this;
        this.ProgressBarPercentText = "0%";
        var myCustomMessageProgressHelper = new CustomMessageProgressComponent_1.CustomMessageProgressHelper();
        myCustomMessageProgressHelper.BasicResponse = true;
        myCustomMessageProgressHelper.StartProgress(filter.Key, 5, true);
        this._VendorMessagesService.PutRecallSuppliersFromFileRequest(filter).subscribe(function (myServiceResponse) {
            console.log("[Send] Response/PutRecallSuppliersFromFileRequest : ", myServiceResponse.Result);
            var response = myServiceResponse.Result;
            myCustomMessageProgressHelper.MessageArrived = true;
            _this.CurrentSession.StopBusyIndicator();
            if (!Tools_1.AppTool.IsNullOrEmpty(response)) {
            }
        });
    };
    RecallSuppliersFromFileComponent.prototype.IncreaseProgressBar = function (ProgressBarValue) {
        var elem = document.getElementById("myBar");
        if (ProgressBarValue == 100) {
            elem.style.width = (ProgressBarValue - 0.1) + '%';
            this.ProgressBarPercentText = ProgressBarValue.toString() + ' %';
        }
        else {
            elem.style.width = ProgressBarValue + '%';
            this.ProgressBarPercentText = ProgressBarValue.toFixed(2).toString() + ' %';
        }
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], RecallSuppliersFromFileComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    RecallSuppliersFromFileComponent = __decorate([
        core_1.Component({
            selector: 'RecallSuppliersFromFileComponent',
            moduleId: module.id,
            templateUrl: './RecallSuppliersFromFileComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], RecallSuppliersFromFileComponent);
    return RecallSuppliersFromFileComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.RecallSuppliersFromFileComponent = RecallSuppliersFromFileComponent;
//# sourceMappingURL=RecallSuppliersFromFileComponent.js.map