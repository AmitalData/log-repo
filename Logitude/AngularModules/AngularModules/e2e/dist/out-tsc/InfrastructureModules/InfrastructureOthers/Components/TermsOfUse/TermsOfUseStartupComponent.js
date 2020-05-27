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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TermsofUseSignaturePM_1 = require("../../../../Common/EntityPMs/TermsofUseSignaturePM");
var TermsofUseSignaturePMService_1 = require("../../../../Common/Services/StandardPMs/TermsofUseSignaturePMService");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var Tools_1 = require("../../../../Infrastructure/Tools");
var Environment_1 = require("../../../../Infrastructure/Locators/Environment");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var DownloadManager_1 = require("../../../../Infrastructure/Utilities/DownloadManager");
var TermsOfUseStartupComponent = /** @class */ (function () {
    function TermsOfUseStartupComponent() {
        this.TermsOfUseCompleted = new core_1.EventEmitter();
        this.LogoURL = "./Images/LoginScreen/header.jpg";
        this.Name = "Logitude";
        if (this.termsofUseSignaturePMService == null) {
            this.termsofUseSignaturePMService = new TermsofUseSignaturePMService_1.TermsofUseSignaturePMService();
        }
        if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
            this.LogoURL = "data:image/JPEG;base64," + SessionLocator_1.SessionLocator.PrivateLableSettings.MainLogo;
            this.Name = SessionLocator_1.SessionLocator.PrivateLableSettings.PrivateLabelShortName;
        }
        else {
            this.LogoURL = Tools_1.AppTool.GetEnvironmentLogo(ObjectsLocator_1.ObjectsLocator.GlobalSetting.LogoCode);
            this.Name = Environment_1.Environment.GetEnvironmentName();
            ;
        }
    }
    TermsOfUseStartupComponent.prototype.ngOnInit = function () {
    };
    TermsOfUseStartupComponent.prototype.SetDataContext = function (data) {
    };
    TermsOfUseStartupComponent.prototype.Load = function (version) {
        this.Version = version;
    };
    TermsOfUseStartupComponent.prototype.DeclineButtonClicked = function () {
        this.TermsOfUseCompleted.emit("Decline");
    };
    TermsOfUseStartupComponent.prototype.AcceptButtonClicked = function () {
        var _this = this;
        this.ShowBusyIndicator = true;
        this.BusyIndicatorText = "Loading..";
        var termsofUseSignaturePM = new TermsofUseSignaturePM_1.TermsofUseSignaturePM();
        termsofUseSignaturePM.TermsofUseVersion = this.Version;
        termsofUseSignaturePM.ContactId = SessionInfo_1.SessionInfo.LoggedUserId;
        termsofUseSignaturePM.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        termsofUseSignaturePM.SignedDatetime = Tools_1.DateTool.GetCurrentDateAsUtc();
        this.termsofUseSignaturePMService.insert(termsofUseSignaturePM).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.ShowBusyIndicator = false;
                    _this.TermsOfUseCompleted.emit("Accept");
                }
            }
        });
    };
    TermsOfUseStartupComponent.prototype.TermsofUse = function () {
        if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
            var documentName = SessionLocator_1.SessionLocator.PrivateLableSettings.PrivateLabelShortName + "-" + this.Version + "_termsofuses"; // +"." + CurrentDocument.Extension;
            DownloadManager_1.DownloadManager.DownloadPage(documentName);
        }
        else {
            var documentName = this.Version + "_termsofuses"; // +"." + CurrentDocument.Extension;
            DownloadManager_1.DownloadManager.DownloadPage(documentName);
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], TermsOfUseStartupComponent.prototype, "TermsOfUseCompleted", void 0);
    TermsOfUseStartupComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'TermsOfUseStartupComponent',
            templateUrl: './TermsOfUseStartupComponent.html',
            providers: [TermsofUseSignaturePMService_1.TermsofUseSignaturePMService]
        }),
        __metadata("design:paramtypes", [])
    ], TermsOfUseStartupComponent);
    return TermsOfUseStartupComponent;
}());
exports.TermsOfUseStartupComponent = TermsOfUseStartupComponent;
//# sourceMappingURL=TermsOfUseStartupComponent.js.map