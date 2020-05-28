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
var Tools_1 = require("../../../Infrastructure/Tools");
var AccountingOpService_1 = require("../../Services/Others/AccountingOpService");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var ImageParameter_1 = require("../../../Infrastructure/DataContracts/ImageParameter");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var Receiving1000Component = /** @class */ (function (_super) {
    __extends(Receiving1000Component, _super);
    function Receiving1000Component() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "GLAccount";
        _this.UploadFileId = Guid_1.Guid.NewRandomString();
        _this.IsShowProgressBar = false;
        _this.UploadButtonIsEnabled = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.UIProperties.SetRequired("Email", _this.ObjectTableName, true);
        _this.CurrentSession.StopBusyIndicator();
        _this._AccountingOpService = new AccountingOpService_1.AccountingOpService();
        return _this;
    }
    Receiving1000Component.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    Receiving1000Component.prototype.OnKeyUp = function (key) {
        if (!Tools_1.AppTool.IsNullOrEmpty(key)) {
            if (key.keyCode == '13') {
                this.OkButtonClicked();
            }
        }
    };
    Receiving1000Component.prototype.FillErrors = function () {
        this.ValidationErrorsList = [];
        if (this.fileUploadParamerter != null && this.fileUploadParamerter.Base64String != null) {
            //this.Year = new Date().getFullYear();
            this.ValidationErrorsList = [];
            //}
            //else if (!FormatTool.IsEmail(this.Email)) {
            //    this.ValidationErrorsList.push("Email is not valid");
        }
        else {
            this.ValidationErrorsList.push("טען קובץ לפני העלאה ");
        }
    };
    Receiving1000Component.prototype.OkButtonClicked = function () {
        var _this = this;
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.CurrentSession.StartBusyIndicatorCreating();
        if (this.fileUploadParamerter != null && this.fileUploadParamerter.Base64String != null) {
            this._AccountingOpService.PutSystem1000File(this.fileUploadParamerter)
                .subscribe(function (myServiceResponse) {
                console.log("[Send] Response/PutSystem1000File: ", myServiceResponse.Result);
                var response = myServiceResponse.Result;
                _this.CurrentSession.StopBusyIndicator();
                if (myServiceResponse.HasError) {
                    _this.ValidationErrorsList = myServiceResponse.ErrorsArray;
                    //this.ShowMessage(response);
                }
                else {
                    if (!Tools_1.AppTool.IsNullOrEmpty(response)) {
                        _this.ShowMessage(response.Message);
                        _this.CancelButtonClicked();
                    }
                }
            });
        }
    };
    //#region upload
    Receiving1000Component.prototype.ShowMessage = function (message) {
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Show(message);
    };
    Receiving1000Component.prototype.OpenUpLoadFile = function () {
        document.getElementById(this.UploadFileId).click();
    };
    Receiving1000Component.prototype.UploadFile = function (event) {
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
    Receiving1000Component.prototype.ArrayBufferToBase64 = function (file, viewmodel) {
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
    Receiving1000Component.prototype.TryParseLocally = function () {
        //throw new Error("Method not implemented.");
        if (Tools_1.AppTool.IsNullOrEmpty(this._DecodedLoadedString)) {
            this.ShowMessage("File Is Empty");
            return;
        }
        var headers = [];
        var aryLine = this._DecodedLoadedString.split("\n");
        //aryLine.forEach(currLine => {
        //    if (currLine.startsWith("031")) {
        //        headers.push({ 'L31': currLine, 'L32': "" });
        //    } else if (currLine.startsWith("032")) {
        //        var rec = headers[headers.length - 1];
        //        rec.L32 = currLine;
        //    }
        //});
        //if (headers.length < 0) {
        //    this.ShowMessage("Incorrect file format");
        //    return;
        //}
        this.OkButtonClicked();
    };
    Receiving1000Component.prototype.IncreaseProgressBar = function (ProgressBarValue) {
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
    Receiving1000Component = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './Receiving1000Component.html',
        }),
        __metadata("design:paramtypes", [])
    ], Receiving1000Component);
    return Receiving1000Component;
}(BaseComponent_1.BaseComponent));
exports.Receiving1000Component = Receiving1000Component;
//# sourceMappingURL=Receiving1000Component.js.map