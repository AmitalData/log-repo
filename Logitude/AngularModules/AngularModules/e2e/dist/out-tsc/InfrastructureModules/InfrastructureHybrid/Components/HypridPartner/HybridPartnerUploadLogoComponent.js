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
var HybridPartnerPMService_1 = require("../../../../Common/Services/StandardPMs/HybridPartnerPMService");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var HybridPartnerUploadLogoComponent = /** @class */ (function () {
    function HybridPartnerUploadLogoComponent(_imageLibraryService, CD) {
        this._imageLibraryService = _imageLibraryService;
        this.CD = CD;
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
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.Source = "";
        this._HybridPartnerPMService = new HybridPartnerPMService_1.HybridPartnerPMService();
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
        this.CurrentSession.CurrentWindow.StartBusyIndicator("loading...");
    }
    HybridPartnerUploadLogoComponent.prototype.ngAfterViewInit = function () {
        //if (this.ShowUploadVerySmallLogo) this.LoadMobileLogo(false);
        //if (this.ShowUploadSharedLogisLogo) this.LoadSharedLogtsitcsLogo(false);
        this.LoadLogo(false);
    };
    HybridPartnerUploadLogoComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args.EntityPM;
    };
    HybridPartnerUploadLogoComponent.prototype.SetDataContext = function (dataContext) {
    };
    HybridPartnerUploadLogoComponent.prototype.LoadLogo = function (isload) {
        var _this = this;
        if (isload) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("loading...");
        }
        //var myService: WebFreightDomainService = new WebFreightDomainService();
        //myService.getHypridPartnerLogo(this.EntityPM.LogoId).subscribe(myResult => {
        //    if (myResult) {
        //        this.Source = "data:image/JPEG;base64," + myResult;
        //        var isDestroyed: boolean = this.CD['destroyed'];
        //        if (!isDestroyed) {
        //            this.CD.detectChanges();
        //        }
        //    }
        //});
        this._imageLibraryService.DownloadFile(this.EntityPM.LogoId, "jpg", "images", SessionInfo_1.SessionInfo.LoggedUserTenant, "Base64").subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    SetImage(_this.logoHtmlId, result, false);
                }
                else {
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    HideImage(_this.logoHtmlId);
                }
            }
            else {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                HideImage(_this.logoHtmlId);
            }
            _this._imageLibraryService.DownloadFile(_this.EntityPM.SmallLogoId, "jpg", "images", SessionInfo_1.SessionInfo.LoggedUserTenant, "Base64").subscribe(function (res) {
                var pmResponse = res;
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
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
    HybridPartnerUploadLogoComponent.prototype.OpenUpLoadLogo = function () {
        document.getElementById(this.LogoFileHtmlId).click();
    };
    HybridPartnerUploadLogoComponent.prototype.UploadogoFile = function (event) {
        var file = UploadLogoFile(this.LogoFileHtmlId);
        if (file && (file.type == "image/jpeg" || file.type == "image/jpg")) {
            this.IsShowMessageComplate = false;
            this.IsShowProgressLoading = true;
            this.ArrayBufferToBase64(file, "logo", 220, 80, "jpg", this);
        }
    };
    HybridPartnerUploadLogoComponent.prototype.LoadMobileLogo = function (isload) {
        var _this = this;
        if (isload) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
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
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            }
        });
    };
    HybridPartnerUploadLogoComponent.prototype.LoadSharedLogtsitcsLogo = function (isload) {
        var _this = this;
        if (isload) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
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
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            }
        });
    };
    HybridPartnerUploadLogoComponent.prototype.OpenUpLoadMobileLogo = function () {
        document.getElementById(this.MobileLogoFileHtmlId).click();
    };
    HybridPartnerUploadLogoComponent.prototype.UploadMobileLogoFile = function (event) {
        var file = UploadLogoFile(this.MobileLogoFileHtmlId);
        if (file && (file.type == "image/png" || file.type == "image/Png")) {
            this.IsShowMessageComplate = false;
            this.IsShowProgressLoading = true;
            this.ArrayBufferToBase64(file, "verysmalllogo", 65, 65, "png", this);
        }
    };
    HybridPartnerUploadLogoComponent.prototype.OpenUpLoadSharedLogisticsLogo = function () {
        document.getElementById(this.SharedLogisticsLogoFileHtmlId).click();
    };
    HybridPartnerUploadLogoComponent.prototype.UploadSharedLogisticsLogoFile = function (event) {
        var file = UploadLogoFile(this.SharedLogisticsLogoFileHtmlId);
        if (file && (file.type == "image/png" || file.type == "image/Png")) {
            this.IsShowMessageComplate = false;
            this.IsShowProgressLoading = true;
            this.ArrayBufferToBase64(file, "sharedLogtsitcslogo", 65, 65, "png", this);
        }
    };
    HybridPartnerUploadLogoComponent.prototype.SendBlockToServer = function (data, filename, widht, height, extension) {
        var _this = this;
        var filter = new ImageParameter_1.ImageParameter();
        filter.Base64String = data;
        filter.FileName = filename;
        filter.BufferNumber = 0;
        filter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        filter.Width = widht;
        filter.Height = height;
        filter.Extension = extension;
        filter.UploadMode = "Image";
        this._imageLibraryService.UploadFile(filter).subscribe(function (res) {
            var pmResponse = res;
            var result;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    _this.EntityPM.LogoId = result;
                    filter = new ImageParameter_1.ImageParameter();
                    filter.Base64String = data;
                    filter.FileName = filename;
                    filter.BufferNumber = 0;
                    filter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                    filter.Width = 70;
                    filter.Height = 35;
                    filter.Extension = extension;
                    filter.UploadMode = "Image";
                    _this._imageLibraryService.UploadFile(filter).subscribe(function (myres) {
                        var mypmResponse = myres;
                        var myresult;
                        if (!mypmResponse.HasError) {
                            myresult = mypmResponse.Result;
                            _this.EntityPM.SmallLogoId = myresult;
                            _this._HybridPartnerPMService.update(_this.EntityPM).subscribe(function (myResult) {
                                if (!myResult.HasError) {
                                    _this.IsShowMessageComplate = true;
                                    _this.IsShowProgressLoading = false;
                                    _this.LoadLogo(true);
                                }
                            });
                        }
                    });
                    //if (filename == "smalllogo") {
                    //    this.SendBlockToServer(filter.Base64String, "images", 150, 150, "jpg");
                    //}
                    //else if (filename == "logo") {
                    //    this.SendBlockToServer(filter.Base64String, "images", 300, 300, "jpg");
                    //}
                    //else {
                    //    this.IsShowMessageComplate = true;
                    //    this.IsShowProgressLoading = false;
                    //    if (filename == "logo" || filename == "smalllogo") {
                    //        this.LoadLogo(true);
                    //    }
                    //    else if (filename == "verysmalllogo") {
                    //        this.LoadMobileLogo(true);
                    //    } else if (filename == "sharedLogtsitcslogo") {
                    //        this.LoadSharedLogtsitcsLogo(true);
                    //    }
                    //}
                }
            }
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
        });
    };
    HybridPartnerUploadLogoComponent.prototype.ArrayBufferToBase64 = function (file, filename, widht, height, extension, viewmode) {
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
    HybridPartnerUploadLogoComponent.prototype.SaveButtonClicked = function () {
        this.CloseButtonClicked();
    };
    HybridPartnerUploadLogoComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    HybridPartnerUploadLogoComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'HybridPartnerUploadLogo',
            templateUrl: './HybridPartnerUploadLogoComponent.html',
            providers: [ImageLibraryService_1.ImageLibraryService]
        }),
        __metadata("design:paramtypes", [ImageLibraryService_1.ImageLibraryService, core_1.ChangeDetectorRef])
    ], HybridPartnerUploadLogoComponent);
    return HybridPartnerUploadLogoComponent;
}());
exports.HybridPartnerUploadLogoComponent = HybridPartnerUploadLogoComponent;
//# sourceMappingURL=HybridPartnerUploadLogoComponent.js.map