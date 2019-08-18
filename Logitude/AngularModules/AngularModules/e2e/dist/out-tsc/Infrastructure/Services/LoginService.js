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
var http_1 = require("@angular/http");
require("rxjs/add/operator/map");
require("rxjs/add/operator/catch");
var SessionLocator_1 = require("../Utilities/SessionLocator");
var SessionInfo_1 = require("../Utilities/SessionInfo");
var Tools_1 = require("../Tools");
var LoginService = /** @class */ (function () {
    function LoginService() {
        this.logitudeURL = null;
        this.baseUrlApi = null;
        this.baseMetaUrlApi = null;
        this._iisBaseApiUrl = "http://192.168.1.100/main/api/";
        this._iisMetaDataApiUrl = "http://192.168.1.100/main/api/ngMetaData";
        this._http = SessionLocator_1.SessionLocator.Http;
        this.logitudeURL = Tools_1.AppTool.GetLogitudeURL();
        this.baseUrlApi = this.logitudeURL + "api/";
        this.baseMetaUrlApi = this.logitudeURL + "api/ngMetaData";
        this.AuthHeader = new http_1.Headers();
    }
    LoginService.prototype.GetOneUsePassword = function () {
        var url = this.logitudeURL + "api/OneTimePassword?id=" + SessionLocator_1.SessionLocator.ExternalParams.OneTimePasswordId;
        return this._http.get(url).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.PostUserValidation = function (loginParameters) {
        var url = this.baseUrlApi + "Authentication";
        loginParameters.IsAngularLogin = true;
        return this._http.post(url, JSON.stringify(loginParameters), { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.PostLoginData = function (loginParameters) {
        var url = this.baseUrlApi + "Authentication?tenant=" + this.CurrentTenant;
        this.AuthHeader = new http_1.Headers();
        this.AuthHeader.append('Content-Type', 'application/json');
        this.AuthHeader.append('Accept', 'application/json');
        var twoFactorKey = window.localStorage.getItem('TwoFactorkey');
        if (twoFactorKey) {
            this.AuthHeader.append('TwoFactorkey', twoFactorKey);
        }
        return this._http.post(url, JSON.stringify(loginParameters), { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.PostAuthenticationDeviceVerificationCode = function (deviceKey, verificationCode, tenant) {
        var url = this.baseUrlApi + "Authentication/PostAuthenticationDeviceVerificationCode?deviceKey=" + deviceKey + "&verificationCode=" + verificationCode + "&tenant=" + this.CurrentTenant;
        return this._http.post(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.PostResendAuthenticationDeviceVerificationCode = function (deviceKey, userId, tenant) {
        //PostResendAuthenticationDeviceVerificationCode(string deviceKey, string userId, int tenant)
        var url = this.baseUrlApi + "Authentication/PostResendAuthenticationDeviceVerificationCode?deviceKey=" + deviceKey + "&userId=" + userId + "&tenant=" + this.CurrentTenant;
        return this._http.post(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetLoggedUser = function () {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&useremail=' + this.LoggedUserEmail + '&getloggeduser=true';
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            var result = response.json();
            return result;
        });
    };
    LoginService.prototype.GetLoggedTenant = function () {
        //var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&getloggedtenant=true';
        var url = this.baseUrlApi + 'CommonDomain/GetLoggedTenantDB';
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetLastFilters = function () {
        var url = this.baseUrlApi + 'InfrastructureDomain/GetLastFilters';
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetTenantManagement = function () {
        var url = this.baseUrlApi + 'TenantManagement/GetSingleTenantManagementPM?id=' + this.CurrentTenant;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return this._http.get(url, { headers: authHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.CheckTenantMangmnt = function (loggedUserId) {
        var url = this.baseUrlApi + 'GlobalDomain/GetCheckTenantMangmnt?loggedUserId=' + loggedUserId;
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetQueries = function () {
        var url = this.logitudeURL + "api/ngMetaData?tenant=" + this.CurrentTenant + "&userid=" + this.LoggedUserId + "&objecttableid=dummy";
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetStatuses = function () {
        var url = this.baseMetaUrlApi + "?tenant=" + this.CurrentTenant + "&inActive=false&dumb2=dumb";
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetPreDefinedFilters = function () {
        var url = this.baseMetaUrlApi + "/GetAdvanceQueryFiltersPMs?tenant=" + this.CurrentTenant;
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetTenantTranslations = function () {
        var url = this.baseMetaUrlApi + "?translationTenant=" + this.CurrentTenant;
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetTenantLanguageTranslations = function () {
        var url = this.baseMetaUrlApi + "/GetTenantLanguageTranslations?tenant=" + this.CurrentTenant;
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetTransportModes = function () {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&dummy=dummy';
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetDirections = function () {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&dummy2=dummy2';
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetMenusTables = function () {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&menustables=dummy';
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetObjectTables = function () {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&objecttables=dummy';
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetScreens = function () {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&screens=dummy';
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetScreenFields = function () {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&screenfields=dummy';
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetObjectTableTabs = function () {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&objecttabletabs=dummy';
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetObjectFields = function () {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&objectTableName=Shipment&inActive=false';
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GeLoggedTenantObjectFields = function () {
        var url = this.baseMetaUrlApi + '/GetTenantObjectFields?loggedTenant=' + this.CurrentTenant;
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetTextCodesTranslations = function () {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&textcodetranslations=dummy';
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetAccountingSetting = function () {
        var url = this.baseMetaUrlApi + '?id=' + this.CurrentTenant + '&textcodetranslations=dummy';
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetCustomsInterfaceSetting = function () {
        var url = this.baseMetaUrlApi + '?InterfaceId=' + this.CurrentTenant + '&textcodetranslations=dummy';
        ;
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetSharedLogisticsSetting = function () {
        var url = this.baseMetaUrlApi + '?settingId=' + this.CurrentTenant + '&textcodetranslations=dummy';
        ;
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetAccountingSystem = function (AccountingSystemCode) {
        var url = this.logitudeURL + 'api/GlobalDomain/GetAccountingSystem?AccountingSystemCode=' + AccountingSystemCode;
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetTenantTextCode = function () {
        var url = this.baseMetaUrlApi + '/GetTenantTextCodes?tenant=' + this.CurrentTenant;
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetGlobalSetting = function () {
        var url = this.logitudeURL + 'api/GlobalDomain/GetGlobalSetting';
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetPrivateLableById = function (Id) {
        var url = this.logitudeURL + 'api/GlobalDomain/GetPrivateLableById?Id=' + Id;
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetTenantSetting = function () {
        var url = this.logitudeURL + 'api/GlobalDomain/GetTenantSetting';
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetTips = function () {
        var url = this.logitudeURL + 'api/Tips/GetTipsPMs?tenant=' + this.CurrentTenant;
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetTipsVisibilities = function () {
        var url = this.logitudeURL + 'api/TipsVisibility/GetTipsVisibilities?tenant=' + this.CurrentTenant + "&userid=" + this.LoggedUserId;
        return this._http.get(url, { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetSignOut = function () {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this.logitudeURL + 'api/Authentication/GetSignOut';
        return this._http.get(url, { headers: authHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.GetDocumentDownloadToken = function () {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this.logitudeURL + 'api/Authentication/GetDocumentDownloadToken?documentToken=' + SessionInfo_1.SessionInfo.DocumentDownloadToken;
        return this._http.get(url, { headers: authHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], LoginService);
    return LoginService;
}());
exports.LoginService = LoginService;
var LoginParameters = /** @class */ (function () {
    function LoginParameters() {
    }
    return LoginParameters;
}());
exports.LoginParameters = LoginParameters;
//# sourceMappingURL=LoginService.js.map