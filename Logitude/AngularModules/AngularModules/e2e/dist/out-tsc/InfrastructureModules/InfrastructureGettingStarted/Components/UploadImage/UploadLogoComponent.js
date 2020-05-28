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
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ImageLibraryService_1 = require("../../../../Common/Services/Others/ImageLibraryService");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var ImageParameter_1 = require("../../../../Infrastructure/DataContracts/ImageParameter");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var UploadLogoComponent = /** @class */ (function () {
    function UploadLogoComponent(_imageLibraryService) {
        this._imageLibraryService = _imageLibraryService;
        this.IsShowMessageComplate = false;
        this.IsShowProgressLoading = false;
        this.ShowUploadVerySmallLogo = false;
        this.SharedLogisticsLogoHtmlId = Guid_1.Guid.newGuid();
        this.MobilelogoHtmlId = Guid_1.Guid.newGuid();
        this.logoHtmlId = Guid_1.Guid.newGuid();
        this.SmalllogoHtmlId = Guid_1.Guid.newGuid();
        this.MobileLogoFileHtmlId = Guid_1.Guid.NewRandomString();
        this.SharedLogisticsLogoFileHtmlId = Guid_1.Guid.NewRandomString();
        this.LogoFileHtmlId = Guid_1.Guid.NewRandomString();
        this.LogoHelpText = TextCodeTranslator_1.TextCodeTranslator.Translate("Tenant.LogoHelpText");
        this.ShowUploadSharedLogisLogo = false;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.IsHideAreaCloseButton = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(function (response) {
        });
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "MOBILELOGO")) {
            this.ShowUploadVerySmallLogo = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "SHAREDLOGISTICSLOGO")) {
            this.ShowUploadSharedLogisLogo = true;
        }
        if (SessionLocator_1.SessionLocator.Tenant == 65) {
            this.DemoMessageVisibility = true;
            if (SessionLocator_1.SessionLocator.LoggedUserPM.Email.toLowerCase() == "customercare@logitudeworld.com‏") {
                this.DemoMessageVisibility = false;
            }
        }
        this.CurrentSession.StartBusyIndicator("loading...");
    }
    UploadLogoComponent.prototype.ngAfterViewInit = function () {
        if (this.ShowUploadVerySmallLogo)
            this.LoadMobileLogo(false);
        if (this.ShowUploadSharedLogisLogo)
            this.LoadSharedLogtsitcsLogo(false);
        this.LoadLogo(false);
    };
    UploadLogoComponent.prototype.SetDataContext = function (dataContext) {
    };
    UploadLogoComponent.prototype.LoadLogo = function (isload) {
        var _this = this;
        if (isload) {
            this.CurrentSession.StartBusyIndicator("loading...");
        }
        this._imageLibraryService.DownloadFile("logo" + SessionInfo_1.SessionInfo.LoggedUserTenant, "jpg", "logos", SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    SetImage(_this.logoHtmlId, result, false);
                }
                else {
                    _this.CurrentSession.StopBusyIndicator();
                    HideImage(_this.logoHtmlId);
                }
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
                HideImage(_this.logoHtmlId);
            }
            _this._imageLibraryService.DownloadFile("smalllogo" + SessionInfo_1.SessionInfo.LoggedUserTenant, "jpg", "logos", SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                var pmResponse = res;
                _this.CurrentSession.StopBusyIndicator();
                if (!pmResponse.HasError) {
                    var result = pmResponse.Result;
                    if (result) {
                        SetImage(_this.SmalllogoHtmlId, result, false);
                    }
                    else
                        HideImage(_this.SmalllogoHtmlId);
                }
                else
                    HideImage(_this.SmalllogoHtmlId);
            });
        });
    };
    UploadLogoComponent.prototype.OpenUpLoadLogo = function () {
        document.getElementById(this.LogoFileHtmlId).click();
    };
    UploadLogoComponent.prototype.UploadogoFile = function (event) {
        var file = UploadLogoFile(this.LogoFileHtmlId);
        if (file && (file.type == "image/jpeg" || file.type == "image/jpg")) {
            this.IsShowMessageComplate = false;
            this.IsShowProgressLoading = true;
            this.ArrayBufferToBase64(file, "logo", 300, 300, "jpg", this);
        }
    };
    UploadLogoComponent.prototype.LoadMobileLogo = function (isload) {
        var _this = this;
        if (isload) {
            this.CurrentSession.StartBusyIndicator("Loading...");
        }
        this._imageLibraryService.DownloadFile("verysmalllogo" + SessionInfo_1.SessionInfo.LoggedUserTenant, "png", "logos", SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    SetImage(_this.MobilelogoHtmlId, result, false);
                }
                else
                    HideImage(_this.MobilelogoHtmlId);
            }
            else
                HideImage(_this.MobilelogoHtmlId);
            if (isload) {
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    UploadLogoComponent.prototype.LoadSharedLogtsitcsLogo = function (isload) {
        var _this = this;
        if (isload) {
            this.CurrentSession.StartBusyIndicator("Loading...");
        }
        this._imageLibraryService.DownloadFile("sharedLogtsitcslogo" + SessionInfo_1.SessionInfo.LoggedUserTenant, "png", "logos", SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    SetImage(_this.SharedLogisticsLogoHtmlId, result, false);
                }
                else
                    HideImage(_this.SharedLogisticsLogoHtmlId);
            }
            else
                HideImage(_this.SharedLogisticsLogoHtmlId);
            if (isload) {
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    UploadLogoComponent.prototype.OpenUpLoadMobileLogo = function () {
        document.getElementById(this.MobileLogoFileHtmlId).click();
    };
    UploadLogoComponent.prototype.UploadMobileLogoFile = function (event) {
        var file = UploadLogoFile(this.MobileLogoFileHtmlId);
        if (file && (file.type == "image/png" || file.type == "image/Png")) {
            this.IsShowMessageComplate = false;
            this.IsShowProgressLoading = true;
            this.ArrayBufferToBase64(file, "verysmalllogo", 65, 65, "png", this);
        }
    };
    UploadLogoComponent.prototype.OpenUpLoadSharedLogisticsLogo = function () {
        document.getElementById(this.SharedLogisticsLogoFileHtmlId).click();
    };
    UploadLogoComponent.prototype.UploadSharedLogisticsLogoFile = function (event) {
        var file = UploadLogoFile(this.SharedLogisticsLogoFileHtmlId);
        if (file && (file.type == "image/png" || file.type == "image/Png")) {
            this.IsShowMessageComplate = false;
            this.IsShowProgressLoading = true;
            this.ArrayBufferToBase64(file, "sharedLogtsitcslogo", 65, 65, "png", this);
        }
    };
    UploadLogoComponent.prototype.SendBlockToServer = function (data, filename, widht, height, extension) {
        var _this = this;
        var filter = new ImageParameter_1.ImageParameter();
        filter.Base64String = data;
        filter.FileName = filename;
        filter.BufferNumber = 0;
        filter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        filter.Width = widht;
        filter.Height = height;
        filter.Extension = extension;
        filter.UploadMode = "CompanyLogos";
        this._imageLibraryService.UploadFile(filter).subscribe(function (res) {
            var pmResponse = res;
            var result;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    if (filename == "logo") {
                        _this.SendBlockToServer(filter.Base64String, "smalllogo", 150, 150, "jpg");
                    }
                    else {
                        _this.IsShowMessageComplate = true;
                        _this.IsShowProgressLoading = false;
                        if (filename == "logo" || filename == "smalllogo") {
                            _this.LoadLogo(true);
                        }
                        else if (filename == "verysmalllogo") {
                            _this.LoadMobileLogo(true);
                        }
                        else if (filename == "sharedLogtsitcslogo") {
                            _this.LoadSharedLogtsitcsLogo(true);
                        }
                    }
                }
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    UploadLogoComponent.prototype.ArrayBufferToBase64 = function (file, filename, widht, height, extension, viewmode) {
        if (file) {
            var reader = new FileReader();
            var reader = new FileReader();
            reader.onload = function (e) {
                var binary = '';
                var result = ArrayBufferToBase64(e);
                var bytes = new Uint8Array(result);
                var len = bytes.byteLength;
                for (var i = 0; i < len; i++) {
                    binary += String.fromCharCode(bytes[i]);
                }
                viewmode.SendBlockToServer(window.btoa(binary), filename, widht, height, extension);
            };
            reader.onerror = function (e) {
                console.log(e);
            };
            reader.readAsArrayBuffer(file);
        }
    };
    UploadLogoComponent.prototype.SaveButtonClicked = function () {
        this.CloseButtonClicked();
    };
    UploadLogoComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    UploadLogoComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'UploadLogo',
            templateUrl: './UploadLogoComponent.html',
            providers: [ImageLibraryService_1.ImageLibraryService]
        }),
        __metadata("design:paramtypes", [ImageLibraryService_1.ImageLibraryService])
    ], UploadLogoComponent);
    return UploadLogoComponent;
}());
exports.UploadLogoComponent = UploadLogoComponent;
//# sourceMappingURL=UploadLogoComponent.js.map