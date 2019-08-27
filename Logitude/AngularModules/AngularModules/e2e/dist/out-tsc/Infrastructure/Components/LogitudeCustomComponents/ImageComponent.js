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
var Tools_1 = require("../../../Infrastructure/Tools");
var ImageLibraryService_1 = require("../../../Common/Services/Others/ImageLibraryService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var core_1 = require("@angular/core");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var ContactPMService_1 = require("../../../Common/Services/StandardPMs/ContactPMService");
var ImageParameter_1 = require("../../../Infrastructure/DataContracts/ImageParameter");
var ImageComponent = /** @class */ (function () {
    function ImageComponent(_imageLibraryService, cd) {
        this._imageLibraryService = _imageLibraryService;
        this.cd = cd;
        this.HideBorder = false;
        this.IsLoadingImage = false;
        this.DefultImageHeight = "auto";
        this.ImageKey = Guid_1.Guid.newGuid();
        this.ImageFileHtmlId = Guid_1.Guid.NewRandomString();
        this.ProgressDownloadId = Guid_1.Guid.newGuid();
        this.DisplayOnly = false;
        this.Tooltip = "Click to add the photo"; // 
        this.CursorImage = "pointer";
        this.BorderStyle = "1px solid #d3d3d3";
        this.WidthImage = "100%";
        this.HeightImage = "100%";
        this.HeightSocialImage = "";
        this.WidthSocialImage = "";
        this.ColSpanArea3 = "";
        this.IsShowSocialMessageAreaImage1 = true;
        this.IsShowSocialMessageAreaImage2 = true;
        this.IsShowSocialMessageAreaImage3 = true;
        this.IsShowSocialMessageAreaImage4 = true;
        this.UploadCompleted = new core_1.EventEmitter();
        //SocialMessage
        this.ParticipantsImageId = "";
        this.ParticipantsImageIdList = [];
        this.IsShowOtherTextArea = false;
        this.OtherAreaText = "";
        this.IsDefultFontSize = false;
        this.ParticipantsImageList = [];
        this.IsShowSocialMessageImage1 = true;
        this.IsShowSocialMessageImage2 = true;
        this.IsShowSocialMessageImage3 = true;
        this.IsShowSocialMessageImage4 = true;
        this.SocialMessageImage1Key = Guid_1.Guid.newGuid();
        this.SocialMessageImage2Key = Guid_1.Guid.newGuid();
        this.SocialMessageImage3Key = Guid_1.Guid.newGuid();
        this.SocialMessageImage4Key = Guid_1.Guid.newGuid();
        this.contactPMService = new ContactPMService_1.ContactPMService();
    }
    ImageComponent.prototype.ngOnInit = function () {
        if (this.HideBorder) {
            this.BorderStyle = "";
        }
        if (this.DisplayOnly) {
            this.CursorImage = "";
            this.Tooltip = "";
        }
    };
    ImageComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        if (this.EntityName == "SocialMessage" && !Tools_1.AppTool.IsNullOrEmpty(this.ConversationHeaderId)) {
            this.LoadParticipantsImageId();
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ImageId)) {
                this.ShowLogosIfExist(this.ImageId);
            }
            else {
                if (this.EntityName == "Contact") {
                    ShowHideProgressDownload(true, this.ProgressDownloadId);
                    this.contactPMService.get(this.EntityId).subscribe(function (myResponse) {
                        ShowHideProgressDownload(false, _this.ProgressDownloadId);
                        if (!myResponse.HasError) {
                            var contact = myResponse.Result;
                            if (contact) {
                                _this.ImageId = contact.ImageDetailId;
                                _this.cd.detectChanges();
                                if (!Tools_1.AppTool.IsNullOrEmpty(_this.ImageId)) {
                                    _this.ShowLogosIfExist(_this.ImageId);
                                }
                            }
                        }
                    });
                }
                if (this.EntityName == "Quotation") {
                    this.DefultImageHeight = "250px";
                }
            }
        }
    };
    ImageComponent.prototype.ShowLogosIfExist = function (imageId, isUseCach) {
        if (isUseCach === void 0) { isUseCach = true; }
        if (!imageId)
            imageId = this.ImageId;
        if (isUseCach) {
            var imageByte = SessionLocator_1.SessionLocator.UserIcons[imageId];
            if (!imageByte) {
                this.GetImageFile(imageId);
            }
            else {
                ShowHideProgressDownload(false, this.ProgressDownloadId);
                SetImage(this.ImageKey, imageByte, true);
            }
        }
        else {
            this.GetImageFile(imageId, !isUseCach);
        }
        if (!this.DisplayOnly) {
            this.Tooltip = "Click to change the photo";
        }
        this.cd.detectChanges();
        // this.GetImageFile(imageId);
    };
    ImageComponent.prototype.GetImageFile = function (imageId, isFirEvent) {
        var _this = this;
        if (isFirEvent === void 0) { isFirEvent = false; }
        var type = "Base64";
        if (this.EntityName == "Quotation") {
            type += ("^ImageDetail");
        }
        ShowHideProgressDownload(true, this.ProgressDownloadId);
        this._imageLibraryService.DownloadFile(this.ImageId, "jpg", "images", SessionInfo_1.SessionInfo.LoggedUserTenant, type).subscribe(function (res) {
            var pmResponse = res;
            ShowHideProgressDownload(false, _this.ProgressDownloadId);
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    SessionLocator_1.SessionLocator.UserIcons[imageId] = result;
                    SetImage(_this.ImageKey, result, true);
                    if (isFirEvent)
                        _this.UploadCompleted.emit(_this.ImageId);
                }
            }
        });
    };
    ImageComponent.prototype.OpenUpLoadLogo = function () {
        if (!this.DisplayOnly) {
            document.getElementById(this.ImageFileHtmlId).click();
        }
    };
    ImageComponent.prototype.UploadogoFile = function (event) {
        if (this.EntityName == "Quotation") {
            this.DefultImageHeight = "auto";
        }
        var height = this.ImageResizeHeight ? this.ImageResizeHeight : 150;
        var width = this.ImageResizeWidth ? this.ImageResizeWidth : 150;
        var file = UploadLogoFile(this.ImageFileHtmlId);
        if (file) {
            if (file.type == "image/jpeg" || file.type == "image/jpg") {
                this.ArrayBufferToBase64(file, "images", width, height, this);
            }
            else if (this.EntityName == "Quotation" && (file.type == "image/png" || file.type == "image/PNG")) {
                this.ArrayBufferToBase64(file, "images", width, height, this);
            }
        }
    };
    ImageComponent.prototype.ArrayBufferToBase64 = function (file, filename, widht, height, viewmode) {
        if (file) {
            var reader = new FileReader();
            var extension = "";
            var fileInfo = file.name.split('.');
            if (fileInfo.length > 1) {
                extension = fileInfo[fileInfo.length - 1];
            }
            else
                extension = fileInfo[1];
            var reader = new FileReader();
            reader.onload = function (e) {
                var binary = '';
                var result = ArrayBufferToBase64(e);
                var bytes = new Uint8Array(result);
                var len = bytes.byteLength;
                for (var i = 0; i < len; i++) {
                    binary += String.fromCharCode(bytes[i]);
                }
                ShowHideProgressDownload(true, viewmode.ProgressDownloadId);
                viewmode.SendBlockToServer(window.btoa(binary), filename, widht, height, extension);
            };
            reader.onerror = function (e) {
                console.log(e);
            };
            reader.readAsArrayBuffer(file);
        }
    };
    ImageComponent.prototype.SendBlockToServer = function (data, filename, widht, height, extension) {
        var _this = this;
        var filter = new ImageParameter_1.ImageParameter();
        filter.Base64String = data;
        filter.FileName = filename;
        filter.BufferNumber = 0;
        filter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        filter.Width = widht;
        filter.Height = height;
        filter.Extension = extension;
        filter.UploadMode = "ImageComponent";
        if (this.EntityName == "Customer") {
            filter.EntityId = this.EntityId;
            filter.ContactId = null;
            filter.Key = null;
        }
        else if (this.EntityName == "Contact") {
            filter.EntityId = null;
            filter.ContactId = this.EntityId;
            filter.Key = null;
        }
        else {
            filter.EntityId = null;
            filter.ContactId = null;
            filter.Key = this.ImageId;
        }
        this._imageLibraryService.UploadFile(filter).subscribe(function (res) {
            var pmResponse = res;
            var result;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    if (_this.ImageId) {
                        var empty = "";
                        SessionLocator_1.SessionLocator.UserIcons[_this.ImageId] = empty;
                    }
                    _this.ImageId = result;
                    _this.ShowLogosIfExist(_this.ImageId, false);
                }
                else
                    ShowHideProgressDownload(false, _this.ProgressDownloadId);
            }
            else
                ShowHideProgressDownload(false, _this.ProgressDownloadId);
        });
    };
    ImageComponent.prototype.LoadParticipantsImageId = function () {
        var _this = this;
        this._imageLibraryService.GetAllParticipantsConversationHeaderMessageId(this.ConversationHeaderId).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                _this.ParticipantsImageId = pmResponse.Result;
                if (!Tools_1.AppTool.IsNullOrEmpty(_this.ParticipantsImageId)) {
                    _this.ParticipantsImageIdList = _this.ParticipantsImageId.split(',');
                    _this.BluidImage();
                }
            }
        });
    };
    ImageComponent.prototype.BluidImage = function () {
        var _this = this;
        if (this.ParticipantsImageIdList.length > 0) {
            if (this.ParticipantsImageIdList.length == 2) {
                this.HeightSocialImage = "100%";
                this.WidthSocialImage = "100%";
                var ParticipantsList1 = this.ParticipantsImageIdList[0].split('_');
                var ParticipantsList2 = this.ParticipantsImageIdList[1].split('_');
                if (ParticipantsList1[1] == SessionLocator_1.SessionLocator.LoggedUserId) {
                    this.ShowSocialMessageLogosIfExist(ParticipantsList2[0], 1, ParticipantsList2[2], ParticipantsList2[3]);
                }
                else if (ParticipantsList2[1] == SessionLocator_1.SessionLocator.LoggedUserId) {
                    this.ShowSocialMessageLogosIfExist(ParticipantsList1[0], 1, ParticipantsList1[2], ParticipantsList1[3]);
                }
                this.IsShowSocialMessageAreaImage2 = false;
                this.IsShowSocialMessageAreaImage3 = false;
                this.IsShowSocialMessageAreaImage4 = false;
            }
            else {
                this.HeightSocialImage = "20px";
                this.ColSpanArea3 = "1";
                if (!Tools_1.AppTool.IsNullOrEmpty(this.HeightImage)) {
                    var height = Number(this.HeightImage.replace("px", ""));
                    this.HeightSocialImage = (height / 2).toString() + "px";
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.WidthImage)) {
                    var width = Number(this.WidthImage.replace("px", ""));
                    this.WidthSocialImage = (width / 2).toString() + "px";
                }
                this.IsDefultFontSize = true;
                this.ParticipantsImageIdList.forEach(function (item) {
                    _this.ParticipantsImageList.push(item.split('_'));
                });
                var count = 1;
                if (this.ParticipantsImageList.length > 4) {
                    this.IsShowOtherTextArea = true;
                    this.OtherAreaText = "+" + (this.ParticipantsImageList.length - 3).toString();
                }
                else {
                    this.IsShowSocialMessageAreaImage4 = false;
                }
                if (this.ParticipantsImageList.length < 2) {
                    this.IsShowSocialMessageAreaImage3 = false;
                }
                if (this.ParticipantsImageList.length == 3) {
                    this.ColSpanArea3 = "2";
                }
                this.ParticipantsImageList.forEach(function (item) {
                    if (count < 5) {
                        _this.ShowSocialMessageLogosIfExist(item[0], count, item[1], item[2]);
                    }
                    count += 1;
                });
            }
        }
    };
    ImageComponent.prototype.ShowSocialMessageLogosIfExist = function (imageId, imageNumber, name, color) {
        if (!Tools_1.AppTool.IsNullOrEmpty(imageId)) {
            var imageByte = SessionLocator_1.SessionLocator.UserIcons[imageId];
            if (!imageByte) {
                this.GetSocialMessageImageFile(imageId, imageNumber);
            }
            else {
                //ShowHideProgressDownload(false, this.ProgressDownloadId);
                this.SetSocialMessageImage(imageNumber, imageByte);
            }
        }
        else {
            this.SetSocialMessageAreaText(imageNumber, color, name);
        }
    };
    ImageComponent.prototype.GetSocialMessageImageFile = function (imageId, imageNumber) {
        var _this = this;
        ShowHideProgressDownload(true, this.ProgressDownloadId);
        this._imageLibraryService.DownloadFile(imageId, "jpg", "images", SessionInfo_1.SessionInfo.LoggedUserTenant, "Base64").subscribe(function (res) {
            var pmResponse = res;
            ShowHideProgressDownload(false, _this.ProgressDownloadId);
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    SessionLocator_1.SessionLocator.UserIcons[imageId] = result;
                    _this.SetSocialMessageImage(imageNumber, result);
                }
            }
        });
    };
    ImageComponent.prototype.SetSocialMessageImage = function (imageNumber, imageByte) {
        switch (imageNumber) {
            case 1:
                this.IsShowSocialMessageAreaImage1 = true;
                this.IsShowSocialMessageImage1 = true;
                SetImage(this.SocialMessageImage1Key, imageByte, true);
                break;
            case 2:
                this.IsShowSocialMessageAreaImage2 = true;
                this.IsShowSocialMessageImage2 = true;
                SetImage(this.SocialMessageImage2Key, imageByte, true);
                break;
            case 3:
                this.IsShowSocialMessageAreaImage3 = true;
                this.IsShowSocialMessageImage3 = true;
                SetImage(this.SocialMessageImage3Key, imageByte, true);
                break;
            case 4:
                this.IsShowSocialMessageAreaImage4 = true;
                this.IsShowSocialMessageImage4 = true;
                SetImage(this.SocialMessageImage4Key, imageByte, true);
                break;
            default:
                break;
        }
    };
    ImageComponent.prototype.SetSocialMessageAreaText = function (imageNumber, color, nameCode) {
        switch (imageNumber) {
            case 1:
                this.IsShowSocialMessageAreaImage1 = true;
                this.IsShowSocialMessageImage1 = false;
                this.SocialMessageImage1Color = color;
                this.SocialMessageImage1Text = !Tools_1.AppTool.IsNullOrEmpty(nameCode) ? nameCode.toUpperCase() : "";
                break;
            case 2:
                this.IsShowSocialMessageAreaImage2 = true;
                this.IsShowSocialMessageImage2 = false;
                this.SocialMessageImage2Color = color;
                this.SocialMessageImage2Text = !Tools_1.AppTool.IsNullOrEmpty(nameCode) ? nameCode.toUpperCase() : "";
                break;
            case 3:
                this.IsShowSocialMessageAreaImage3 = true;
                this.IsShowSocialMessageImage3 = false;
                this.SocialMessageImage3Color = color;
                this.SocialMessageImage3Text = !Tools_1.AppTool.IsNullOrEmpty(nameCode) ? nameCode.toUpperCase() : "";
                break;
            case 4:
                this.IsShowSocialMessageAreaImage4 = true;
                this.IsShowSocialMessageImage4 = false;
                this.SocialMessageImage4Color = color;
                this.SocialMessageImage4Text = !Tools_1.AppTool.IsNullOrEmpty(nameCode) ? nameCode.toUpperCase() : "";
                break;
            default:
                break;
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], ImageComponent.prototype, "UploadCompleted", void 0);
    ImageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'ImageComponent',
            templateUrl: './ImageComponent.html',
            inputs: ['EntityId', 'ImageId', "EntityName", 'ImageId', 'WidthImage', 'HeightImage', 'ImageResizeWidth', 'ImageResizeHeight', 'HideBorder', 'DisplayOnly', 'ConversationHeaderId'],
            providers: [ImageLibraryService_1.ImageLibraryService],
        }),
        __metadata("design:paramtypes", [ImageLibraryService_1.ImageLibraryService, core_1.ChangeDetectorRef])
    ], ImageComponent);
    return ImageComponent;
}());
exports.ImageComponent = ImageComponent;
//# sourceMappingURL=ImageComponent.js.map