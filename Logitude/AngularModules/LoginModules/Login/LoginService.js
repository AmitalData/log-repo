import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { SessionInfo } from './SessionInfo';
export var LoginService = (function () {
    function LoginService(_http) {
        this._http = _http;
        this.logitudeURL = null;
        this.baseUrlApi = null;
        this.baseMetaUrlApi = null;
        this._iisBaseApiUrl = "http://192.168.1.100/main/api/";
        this._iisMetaDataApiUrl = "http://192.168.1.100/main/api/ngMetaData";
        //this._http = ServiceHelper.Http;
        this.logitudeURL = SessionInfo.GetLogitudeURL();
        this.baseUrlApi = this.logitudeURL + "api/";
        this.baseMetaUrlApi = this.logitudeURL + "api/ngMetaData";
        this.AuthHeader = new Headers();
        this.AuthHeader.append('Content-Type', 'application/json');
        this.AuthHeader.append('Accept', 'application/json');
        console.log("this.AuthHeader From Const" + this.AuthHeader.get('Content-Type'));
    }
    LoginService.prototype.PostUserValidation = function (loginParameters) {
        var url = this.baseUrlApi + "Authentication";
        loginParameters.IsAngularLogin = true;
        //this.AuthHeader = new Headers();
        //this.AuthHeader.append('Content-Type', 'application/json');
        //this.AuthHeader.append('Accept', 'application/json');
        console.log("this.AuthHeader " + this.AuthHeader.get('Content-Type'));
        return this._http.post(url, JSON.stringify(loginParameters), { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.LoginUsingAuthenticaionToken = function (loginTokenParameter) {
        var url = this.baseUrlApi + "Authentication?isAngular=" + true;
        return this._http.post(url, JSON.stringify(loginTokenParameter), { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.PostLoginData = function (loginParameters) {
        var url = this.baseUrlApi + "Authentication?tenant=" + this.CurrentTenant;
        loginParameters.IsAngularLogin = true;
        return this._http.post(url, JSON.stringify(loginParameters), { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    LoginService.prototype.PostRequestResetUserPassword = function (resetPasswordParameters) {
        var url = this.baseUrlApi + "ResetPassword?PostResetPassword";
        return this._http.post(url, JSON.stringify(resetPasswordParameters), { headers: this.AuthHeader }).map(function (response) {
            var result = response.json();
            return result;
        });
    };
    LoginService.prototype.CheckUserPassword = function (changePasswordParameter) {
        var url = this.baseUrlApi + "PasswordChange/PostCheckPasswordUser";
        return this._http.post(url, JSON.stringify(changePasswordParameter), { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        });
    };
    //PostCheckUserPassword
    LoginService.prototype.PostChangePassword = function (email, ResetPasswordParameters) {
        var url = this.baseUrlApi + "Authentication?email=" + email;
        return this._http.post(url, JSON.stringify(ResetPasswordParameters), { headers: this.AuthHeader }).map(function (response) {
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
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&getloggedtenant=true';
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
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
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
    LoginService.prototype.GetFeatures = function () {
        //var authHeader = new Headers();
        //authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        //var url = this.baseUrlApi + 'InfrastructureDomain/GetAllowedFeaturesForLoggedUser';
        //return this._http.get(url, { headers: authHeader }).map(response => {
        //    return response.json();
        //});
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
    LoginService.decorators = [
        { type: Injectable },
    ];
    /** @nocollapse */
    LoginService.ctorParameters = [
        { type: Http, },
    ];
    return LoginService;
}());
export var LoginParameters = (function () {
    function LoginParameters() {
    }
    return LoginParameters;
}());
export var LoginTokenParameter = (function () {
    function LoginTokenParameter() {
    }
    return LoginTokenParameter;
}());
export var ChangePasswordParameter = (function () {
    function ChangePasswordParameter() {
    }
    return ChangePasswordParameter;
}());
//# sourceMappingURL=LoginService.js.map