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
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TenantManagmentPrivateLabelsPM_1 = require("../../../../Infrastructure/EntityPMs/TenantManagmentPrivateLabelsPM");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TenantManagmentPrivateLabelsPMService_1 = require("../../../../Infrastructure/Services/StandardPMs/TenantManagmentPrivateLabelsPMService");
var LocationDirective_1 = require("../../../../Infrastructure/Utilities/LocationDirective");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var ImageParameter_1 = require("../../../../Infrastructure/DataContracts/ImageParameter");
var ImageLibraryService_1 = require("../../../../Common/Services/Others/ImageLibraryService");
var AddEditPrivateLabelsComponent = /** @class */ (function (_super) {
    __extends(AddEditPrivateLabelsComponent, _super);
    function AddEditPrivateLabelsComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "TenantManagmentPrivateLabels";
        _this.ValidationErrorsList = [];
        _this.EntityId = null;
        _this.IsEditMode = false;
        _this.LogoMainFileHtmlId = Guid_1.Guid.NewRandomString();
        _this.LogoSmallFileHtmlId = Guid_1.Guid.NewRandomString();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Retries = 0;
        _this.isViewInited = false;
        _this.IsShowMessageComplate = false;
        _this.IsShowProgressLoading = false;
        _this.DemoMessageVisibility = false;
        _this.imageParameter = new ImageParameter_1.ImageParameter();
        _this.MainLogoImageHtmlId = Guid_1.Guid.newGuid();
        _this.SmallLogoImageHtmlId = Guid_1.Guid.newGuid();
        _this.EntityPM = new TenantManagmentPrivateLabelsPM_1.TenantManagmentPrivateLabelsPM();
        return _this;
    }
    AddEditPrivateLabelsComponent.prototype.ngOnInit = function () {
        this.UIProperties.SetRequired("HybridPartnerId", this.ObjectTableName, true);
    };
    AddEditPrivateLabelsComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.EntityPM = windowArgs.Entity;
        this.IsEditMode = true;
        this.RunComponent();
    };
    AddEditPrivateLabelsComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    AddEditPrivateLabelsComponent.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations) {
                this.isViewInited = true;
                this.InitializeComponent();
            }
            else {
                this.RunComponentTimer();
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    AddEditPrivateLabelsComponent.prototype.UploadogoFileSmall = function (event) {
        var file = UploadLogoFile(this.LogoSmallFileHtmlId);
        if (file && (file.type == "image/jpeg" || file.type == "image/jpg")) {
            this.IsShowMessageComplate = false;
            this.IsShowProgressLoading = true;
            this.imageParameter = new ImageParameter_1.ImageParameter();
            this.imageParameter.Extension = file.type.split('/')[1];
            this.ArrayBufferToBase64(file, this, 2);
        }
    };
    AddEditPrivateLabelsComponent.prototype.UploadogoFileMain = function (event) {
        var file = UploadLogoFile(this.LogoMainFileHtmlId);
        if (file && (file.type == "image/jpeg" || file.type == "image/jpg")) {
            this.imageParameter = new ImageParameter_1.ImageParameter();
            this.imageParameter.Extension = file.type.split('/')[1];
            this.IsShowMessageComplate = false;
            this.IsShowProgressLoading = true;
            this.ArrayBufferToBase64(file, this, 1);
        }
    };
    AddEditPrivateLabelsComponent.prototype.OpenUpLoadMainLogo = function () {
        document.getElementById(this.LogoMainFileHtmlId).click();
    };
    AddEditPrivateLabelsComponent.prototype.OpenUpLoadSmallLogo = function () {
        document.getElementById(this.LogoSmallFileHtmlId).click();
    };
    AddEditPrivateLabelsComponent.prototype.ArrayBufferToBase64 = function (file, viewmode, imageIndex) {
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
                if (imageIndex == 1)
                    viewmode.MainLogoData = window.btoa(binary);
                else
                    viewmode.SmallLogoData = window.btoa(binary);
                viewmode.SetImage(imageIndex);
            };
            reader.onerror = function (e) {
                console.log(e);
            };
            reader.readAsArrayBuffer(file);
        }
    };
    AddEditPrivateLabelsComponent.prototype.SetImage = function (imageIndex) {
        var _this = this;
        var service = new ImageLibraryService_1.ImageLibraryService();
        this.imageParameter.Base64String = imageIndex == 1 ? this.MainLogoData : this.SmallLogoData;
        this.imageParameter.Width = 239;
        this.imageParameter.Height = 85;
        service.PostImageAfterResize(this.imageParameter).subscribe(function (Result) {
            if (!Result.HasError) {
                var image = "data:image/" + "jpg" + ";base64," + Result.Result.Base64String;
                imageIndex == 1 ? _this.EntityPM.MainLogo = Result.Result.Base64String : _this.EntityPM.SmallLogo = Result.Result.Base64String;
                SetImage(imageIndex == 1 ? _this.MainLogoImageHtmlId : _this.SmallLogoImageHtmlId, image, false);
            }
        });
        // LogoMainFileHtmlId
        // "data:image/" + documentExtension + ";base64,"
    };
    AddEditPrivateLabelsComponent.prototype.InitializeComponent = function () {
        if (this.isViewInited) {
            if (!this.EntityPM)
                this.EntityPM = new TenantManagmentPrivateLabelsPM_1.TenantManagmentPrivateLabelsPM();
            else {
                this.UIProperties.SetRequired("HybridPartnerId", this.ObjectTableName, false);
                if (this.MainLogo != null) {
                    SetImage(this.MainLogoImageHtmlId, "data:image/" + "jpg" + ";base64," + this.MainLogo, false);
                }
                if (this.SmallLogo != null) {
                    SetImage(this.SmallLogoImageHtmlId, "data:image/" + "jpg" + ";base64," + this.SmallLogo, false);
                }
            }
        }
    };
    Object.defineProperty(AddEditPrivateLabelsComponent.prototype, "PrivateLabelName", {
        get: function () {
            return this.EntityPM.PrivateLabelName;
        },
        set: function (value) {
            if (value != this.EntityPM.PrivateLabelName)
                this.EntityPM.PrivateLabelName = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelsComponent.prototype, "PrivateLabelShortName", {
        get: function () {
            return this.EntityPM.PrivateLabelShortName;
        },
        set: function (value) {
            if (value != this.EntityPM.PrivateLabelShortName)
                this.EntityPM.PrivateLabelShortName = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelsComponent.prototype, "PrivateLabelUrl", {
        get: function () {
            return this.EntityPM.PrivateLabelUrl;
        },
        set: function (value) {
            if (value != this.EntityPM.PrivateLabelUrl) {
                this.EntityPM.PrivateLabelUrl = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelsComponent.prototype, "ContactUsEmail", {
        get: function () {
            return this.EntityPM.ContactUsEmail;
        },
        set: function (value) {
            if (value != this.EntityPM.ContactUsEmail)
                this.EntityPM.ContactUsEmail = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelsComponent.prototype, "HybridPartnerId", {
        get: function () {
            return this.EntityPM.HybridPartnerId;
        },
        set: function (value) {
            if (value != this.EntityPM.HybridPartnerId) {
                this.EntityPM.HybridPartnerId = value;
                if (value != null) {
                    this.UIProperties.SetRequired("HybridPartnerId", this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetRequired("HybridPartnerId", this.ObjectTableName, true);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelsComponent.prototype, "ReceiveAllStatuses", {
        get: function () {
            return this.EntityPM.ReceiveAllStatuses;
        },
        set: function (value) {
            if (value != this.EntityPM.ReceiveAllStatuses)
                this.EntityPM.ReceiveAllStatuses = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelsComponent.prototype, "InActive", {
        get: function () {
            return this.EntityPM.InActive;
        },
        set: function (value) {
            if (value != this.EntityPM.InActive)
                this.EntityPM.InActive = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelsComponent.prototype, "MainLogo", {
        get: function () {
            return this.EntityPM.MainLogo;
        },
        set: function (value) {
            if (value != this.EntityPM.MainLogo) {
                this.EntityPM.MainLogo = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelsComponent.prototype, "SmallLogo", {
        get: function () {
            return this.EntityPM.SmallLogo;
        },
        set: function (value) {
            if (value != this.EntityPM.SmallLogo) {
                this.EntityPM.SmallLogo = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditPrivateLabelsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditPrivateLabelsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.HybridPartnerId == null || this.HybridPartnerId == "") {
            errors.push("Hybrid Partner Field is Required");
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            var service = new TenantManagmentPrivateLabelsPMService_1.TenantManagmentPrivateLabelsPMService();
            if (!this.IsEditMode) {
                service.insert(this.EntityPM).subscribe(function (response) {
                    if (response) {
                        if (!response.HasError) {
                            _this.CurrentSession.CloseCurrentWindowEmit("OK");
                        }
                        else {
                            _this.ValidationErrorsList = response.ErrorsArray;
                        }
                    }
                });
            }
            else {
                service.update(this.EntityPM).subscribe(function (response) {
                    if (response) {
                        if (!response.HasError) {
                            _this.CurrentSession.CloseCurrentWindowEmit("OK");
                        }
                        else {
                            _this.ValidationErrorsList = response.ErrorsArray;
                        }
                    }
                });
            }
        }
    };
    AddEditPrivateLabelsComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('PackageCode');
        this.myCloner.AddField('NumberOfUsers');
        this.myCloner.AddEntity(this.EntityPM);
    };
    AddEditPrivateLabelsComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", LocationDirective_1.LocationDirective)
    ], AddEditPrivateLabelsComponent.prototype, "AllLocations", void 0);
    AddEditPrivateLabelsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditPrivateLabelsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditPrivateLabelsComponent);
    return AddEditPrivateLabelsComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditPrivateLabelsComponent = AddEditPrivateLabelsComponent;
//# sourceMappingURL=AddEditPrivateLabelsComponent.js.map