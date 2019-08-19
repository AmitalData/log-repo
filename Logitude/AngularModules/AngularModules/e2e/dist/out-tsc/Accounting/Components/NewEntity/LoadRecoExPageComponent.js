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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var ReconcileExternalPagePMService_1 = require("../../Services/StandardPMs/ReconcileExternalPagePMService");
var CurrencyPMService_1 = require("../../../Common/Services/StandardPMs/CurrencyPMService");
var CurrencyListService_1 = require("../../../Common/Services/StandardLists/CurrencyListService");
var ReconcileExternalPageExtendedPMService_1 = require("../../Services/ExtendedPMs/ReconcileExternalPageExtendedPMService");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var Tools_1 = require("../../../Infrastructure/Tools");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var ImageParameter_1 = require("../../../Infrastructure/DataContracts/ImageParameter");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var LoadRecoExPageComponent = /** @class */ (function (_super) {
    __extends(LoadRecoExPageComponent, _super);
    function LoadRecoExPageComponent(CD, entityListService) {
        var _this = _super.call(this) || this;
        _this.CD = CD;
        _this.entityListService = entityListService;
        _this.DataContext = _this;
        _this.ObjectTableName = "ReconcileExternalPage";
        _this.ValidationErrorsList = [];
        _this.isNewEntity = false;
        _this.IsCancelApprovedEnabled = false;
        _this.IsDisplayOnly = false;
        _this.IsMultiCurrency = false;
        _this.AMOUNT_TEXT = TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPageLine.F.Amount");
        _this.isRTL = false;
        _this.TotalSum = 0.0;
        _this.Difference = 0.0;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this._ReconcileExternalPagePMService = new ReconcileExternalPagePMService_1.ReconcileExternalPagePMService();
        _this._ReconcileExternalPageExtendedPMService = new ReconcileExternalPageExtendedPMService_1.ReconcileExternalPageExtendedPMService();
        _this._CurrencyPMService = new CurrencyPMService_1.CurrencyPMService();
        _this.currencyListService = new CurrencyListService_1.CurrencyListService();
        _this.UploadFileId = Guid_1.Guid.NewRandomString();
        _this.IsShowProgressBar = false;
        _this.UploadButtonIsEnabled = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.PageLinesList = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    LoadRecoExPageComponent.prototype.SetWindowArgs = function (args) {
    };
    Object.defineProperty(LoadRecoExPageComponent.prototype, "TenantCurrency", {
        get: function () { return this.tenantCurrency; },
        set: function (value) {
            if (this.tenantCurrency != value) {
                this.tenantCurrency = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    //#region Buttons Handlers
    LoadRecoExPageComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    //* grid handlers in seperate region
    //#endregion
    //#region Prev Bank Page
    //#endregion
    LoadRecoExPageComponent.prototype.SendBankFile = function () {
        var _this = this;
        //throw new Error("Method not implemented.");
        this.CurrentSession.StartBusyIndicatorCreating();
        if (this.fileUploadParamerter != null && this.fileUploadParamerter.Base64String != null) {
            this._ReconcileExternalPageExtendedPMService.LoadBankPages(this.fileUploadParamerter)
                .subscribe(function (myServiceResponse) {
                console.log("[Send] Response/LoadBankPages: ", myServiceResponse.Result);
                var response = myServiceResponse.Result;
                _this.CurrentSession.StopBusyIndicator();
                if (myServiceResponse.HasError) {
                    _this.ValidationErrorsList = myServiceResponse.ErrorsArray;
                    _this.ShowMessage(response);
                }
                else {
                    if (!Tools_1.AppTool.IsNullOrEmpty(response)) {
                        _this.ShowMessage(JSON.stringify(response.Result));
                        _this.CancelButtonClicked();
                    }
                }
            });
        }
    };
    //#region upload
    LoadRecoExPageComponent.prototype.ShowMessage = function (message) {
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Show(message);
    };
    LoadRecoExPageComponent.prototype.OpenUpLoadFile = function () {
        document.getElementById(this.UploadFileId).click();
    };
    LoadRecoExPageComponent.prototype.UploadFile = function (event) {
        var file = attachmentUploader(this.UploadFileId);
        if (file) {
            var temp = file.name.split('.');
            this.FileExtension = temp[temp.length - 1];
            this.FileName = file.name.replace("." + this.FileExtension, "");
            if (this.FileExtension.toLowerCase() != "txt") {
                this.ShowMessage("חובה קובץ TXT");
                return;
            }
            this.File = file;
            if (this.FileExtension && this.FileExtension.length > 10) {
                this.ShowMessage("File extension should be less than or equal 10 characters");
            }
            else {
                this.IsShowProgressBar = true;
                this.UploadButtonIsEnabled = false;
                this.fileUploadParamerter = new ImageParameter_1.ImageParameter();
                this.fileUploadParamerter.Key = Guid_1.Guid.newGuid();
                this.fileUploadParamerter.IsFirstTry = true;
                this.fileUploadParamerter.Extension = this.FileExtension;
                this.fileUploadParamerter.UploadMode = "Block";
                this.fileUploadParamerter.FileSize = file.size;
                this.fileUploadParamerter.Tenant = SessionLocator_1.SessionLocator.Tenant;
                this.ArrayBufferToBase64(file, this);
            }
        }
    };
    LoadRecoExPageComponent.prototype.ArrayBufferToBase64 = function (file, viewmodel) {
        var reader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var decodedString = '';
            var bytes = new Uint8Array(ResultAsArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                decodedString += String.fromCharCode(bytes[i]);
            }
            viewmodel._DecodedLoadedString = decodedString;
            viewmodel.fileUploadParamerter.Base64String = window.btoa(decodedString);
            viewmodel.IncreaseProgressBar(100);
            viewmodel.TryParseLocally();
        };
        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
    };
    LoadRecoExPageComponent.prototype.TryParseLocally = function () {
        //throw new Error("Method not implemented.");
        if (Tools_1.AppTool.IsNullOrEmpty(this._DecodedLoadedString)) {
            this.ShowMessage("File Is Empty");
            return;
        }
        var headers = [];
        var aryLine = this._DecodedLoadedString.split("\n");
        aryLine.forEach(function (currLine) {
            if (currLine.startsWith("031")) {
                headers.push({ 'L31': currLine, 'L32': "" });
            }
            else if (currLine.startsWith("032")) {
                var rec = headers[headers.length - 1];
                rec.L32 = currLine;
            }
        });
        if (headers.length < 0) {
            this.ShowMessage("Incorrect file format");
            return;
        }
        this.SendBankFile();
    };
    LoadRecoExPageComponent.prototype.IncreaseProgressBar = function (ProgressBarValue) {
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
    LoadRecoExPageComponent = __decorate([
        core_1.Component({
            selector: 'LoadRecoExPageComponent',
            moduleId: module.id,
            providers: [EntityListService_1.EntityListService],
            templateUrl: './LoadRecoExPageComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef, EntityListService_1.EntityListService])
    ], LoadRecoExPageComponent);
    return LoadRecoExPageComponent;
}(BaseComponent_1.BaseComponent));
exports.LoadRecoExPageComponent = LoadRecoExPageComponent;
//# sourceMappingURL=LoadRecoExPageComponent.js.map