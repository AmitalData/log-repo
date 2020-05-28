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
var TermsofUseSignatureExtendedPM_1 = require("../../../../Common/Services/ExtendedPMs/TermsofUseSignatureExtendedPM");
var DownloadManager_1 = require("../../../../Infrastructure/Utilities/DownloadManager");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var TermsofUseSignatureComponent = /** @class */ (function () {
    function TermsofUseSignatureComponent(_termsofUseSignatureExtendedPM) {
        this._termsofUseSignatureExtendedPM = _termsofUseSignatureExtendedPM;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    TermsofUseSignatureComponent.prototype.ngOnInit = function () {
    };
    TermsofUseSignatureComponent.prototype.SetDataContext = function (data) {
        this.LoadData();
    };
    TermsofUseSignatureComponent.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this.TermsofUseSignaturePMLists = [];
        this._termsofUseSignatureExtendedPM.GetTermsofUseSignatures(SessionInfo_1.SessionInfo.LoggedUserTenant, SessionInfo_1.SessionInfo.LoggedUserId).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    myResult.forEach(function (item) {
                        _this.TermsofUseSignaturePMLists.push(new TermsofUseSignaturePMViewModel(item));
                    });
                }
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            }
            else {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            }
        });
    };
    TermsofUseSignatureComponent.prototype.ViewFile = function (item) {
        if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
            var documentName = SessionLocator_1.SessionLocator.PrivateLableSettings.PrivateLabelShortName + "-" + item.TermsofUseVersion + "_termsofuses"; // +"." + CurrentDocument.Extension;
            DownloadManager_1.DownloadManager.DownloadPage(documentName);
        }
        else {
            var documentName = item.TermsofUseVersion + "_termsofuses"; // +"." + CurrentDocument.Extension;
            DownloadManager_1.DownloadManager.DownloadPage(documentName);
        }
    };
    TermsofUseSignatureComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'TermsofUseSignature',
            templateUrl: './TermsofUseSignatureComponent.html',
            providers: [TermsofUseSignatureExtendedPM_1.TermsofUseSignatureExtendedPM]
        }),
        __metadata("design:paramtypes", [TermsofUseSignatureExtendedPM_1.TermsofUseSignatureExtendedPM])
    ], TermsofUseSignatureComponent);
    return TermsofUseSignatureComponent;
}());
exports.TermsofUseSignatureComponent = TermsofUseSignatureComponent;
var TermsofUseSignaturePMViewModel = /** @class */ (function () {
    function TermsofUseSignaturePMViewModel(item) {
        this.SignedDatetime = item.SignedDatetime;
        this.TermsofUseVersion = item.TermsofUseVersion;
    }
    return TermsofUseSignaturePMViewModel;
}());
//# sourceMappingURL=TermsofUseSignatureComponent.js.map