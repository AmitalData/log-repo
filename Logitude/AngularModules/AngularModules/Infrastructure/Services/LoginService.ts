import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {Observable} from 'rxjs/Observable';
import {SessionLocator} from '../Utilities/SessionLocator';
import {SessionInfo} from '../Utilities/SessionInfo';
import {AppTool} from '../Tools';

@Injectable()

export class LoginService {
    private _http: Http;
    logitudeURL: string = null;
    baseUrlApi: string = null;
    baseMetaUrlApi: string = null;
    private _iisBaseApiUrl: string = "http://192.168.1.100/main/api/";
    private _iisMetaDataApiUrl: string = "http://192.168.1.100/main/api/ngMetaData";
    public AuthHeader: Headers;
    public CurrentTenant: number;
    public LoggedUserId: string;
    public LoggedUserEmail: string;
    constructor() {
        this._http = SessionLocator.Http;
        this.logitudeURL = AppTool.GetLogitudeURL();
        this.baseUrlApi = this.logitudeURL + "api/";
        this.baseMetaUrlApi = this.logitudeURL + "api/ngMetaData";
        this.AuthHeader = new Headers();
    }

    GetOneUsePassword() {
        var url = this.logitudeURL + "api/OneTimePassword?id=" + SessionLocator.ExternalParams.OneTimePasswordId 

        return this._http.get(url).map(response => {
            return response.json();
        });
    }

    PostUserValidation(loginParameters: LoginParameters) {

        var url = this.baseUrlApi + "Authentication";

        loginParameters.IsAngularLogin = true;
        return this._http.post(url, JSON.stringify(loginParameters), { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }

    PostLoginData(loginParameters: LoginParameters) {
        var url = this.baseUrlApi + "Authentication?tenant=" + this.CurrentTenant;

        this.AuthHeader = new Headers();
        this.AuthHeader.append('Content-Type', 'application/json');
        this.AuthHeader.append('Accept', 'application/json');
        var twoFactorKey = window.localStorage.getItem('TwoFactorkey');
        if (twoFactorKey) {
            this.AuthHeader.append('TwoFactorkey', twoFactorKey);
        }

        return this._http.post(url, JSON.stringify(loginParameters), { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }

    PostAuthenticationDeviceVerificationCode(deviceKey: string, verificationCode: string, tenant: number) {

        var url = this.baseUrlApi + "Authentication/PostAuthenticationDeviceVerificationCode?deviceKey=" + deviceKey + "&verificationCode=" + verificationCode + "&tenant=" + this.CurrentTenant;

        return this._http.post(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }
    PostResendAuthenticationDeviceVerificationCode(deviceKey: string, userId: string, tenant: number) {
        //PostResendAuthenticationDeviceVerificationCode(string deviceKey, string userId, int tenant)
        var url = this.baseUrlApi + "Authentication/PostResendAuthenticationDeviceVerificationCode?deviceKey=" + deviceKey +  "&userId=" + userId + "&tenant=" + this.CurrentTenant;

        return this._http.post(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }

    GetLoggedUser() {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&useremail=' + this.LoggedUserEmail + '&getloggeduser=true';

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            var result = response.json();
            return result;
        });
    }
    GetLoggedTenant() {

        //var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&getloggedtenant=true';
        var url = this.baseUrlApi + 'CommonDomain/GetLoggedTenantDB';

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }
    GetLastFilters() {
        var url = this.baseUrlApi + 'InfrastructureDomain/GetLastFilters';

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }
    GetTenantManagement() {

        var url = this.baseUrlApi + 'TenantManagement/GetSingleTenantManagementPM?id=' + this.CurrentTenant;
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        return this._http.get(url, { headers: authHeader }).map(response => {
            return response.json();
        });
    }

    CheckTenantMangmnt(loggedUserId) {
        var url = this.baseUrlApi + 'GlobalDomain/GetCheckTenantMangmnt?loggedUserId=' + loggedUserId;

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });

    }
    GetQueries() {

        var url = this.logitudeURL + "api/ngMetaData?tenant=" + this.CurrentTenant + "&userid=" + this.LoggedUserId + "&objecttableid=dummy";

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }
    GetStatuses() {

        var url = this.baseMetaUrlApi + "?tenant=" + this.CurrentTenant + "&inActive=false&dumb2=dumb";

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }
    GetPreDefinedFilters() {
        var url = this.baseMetaUrlApi + "/GetAdvanceQueryFiltersPMs?tenant=" + this.CurrentTenant;

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }
    GetTenantTranslations() {
        var url = this.baseMetaUrlApi + "?translationTenant=" + this.CurrentTenant;

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }

    GetTenantLanguageTranslations() {
        var url = this.baseMetaUrlApi + "/GetTenantLanguageTranslations?tenant=" + this.CurrentTenant;

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }

    GetTransportModes() {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&dummy=dummy';

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }
    GetDirections() {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&dummy2=dummy2';

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }
    GetMenusTables() {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&menustables=dummy';

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }
    GetObjectTables() {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&objecttables=dummy';

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }
    GetScreens() {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&screens=dummy';

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }
    GetScreenFields() {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&screenfields=dummy';

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }
    GetObjectTableTabs() {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&objecttabletabs=dummy';

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }
    GetObjectFields() {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&objectTableName=Shipment&inActive=false';

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }

    GeLoggedTenantObjectFields() {
        var url = this.baseMetaUrlApi + '/GetTenantObjectFields?loggedTenant=' + this.CurrentTenant;

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }

    GetTextCodesTranslations() {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&textcodetranslations=dummy';

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }

    GetAccountingSetting() {
        var url = this.baseMetaUrlApi + '?id=' + this.CurrentTenant + '&textcodetranslations=dummy';

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }   

    GetCustomsInterfaceSetting() {
        var url = this.baseMetaUrlApi + '?InterfaceId=' + this.CurrentTenant + '&textcodetranslations=dummy';;
        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }

    GetSharedLogisticsSetting() {
        var url = this.baseMetaUrlApi + '?settingId=' + this.CurrentTenant + '&textcodetranslations=dummy';;
        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }
    
    GetAccountingSystem(AccountingSystemCode: string) {
        var url = this.logitudeURL + 'api/GlobalDomain/GetAccountingSystem?AccountingSystemCode=' + AccountingSystemCode;

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }

    GetTenantTextCode() {
        var url = this.baseMetaUrlApi + '/GetTenantTextCodes?tenant=' + this.CurrentTenant;

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }

    GetGlobalSetting() {
        var url = this.logitudeURL + 'api/GlobalDomain/GetGlobalSetting';

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }

    GetPrivateLableById(Id : string) {
        var url = this.logitudeURL + 'api/GlobalDomain/GetPrivateLableById?Id=' + Id;

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }

    GetTenantSetting() {
        var url = this.logitudeURL + 'api/GlobalDomain/GetTenantSetting';

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }

    GetTips() {

        var url = this.logitudeURL + 'api/Tips/GetTipsPMs?tenant=' + this.CurrentTenant;

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }

    GetTipsVisibilities() {

        var url = this.logitudeURL + 'api/TipsVisibility/GetTipsVisibilities?tenant=' + this.CurrentTenant + "&userid=" + this.LoggedUserId;

        return this._http.get(url, { headers: this.AuthHeader }).map(response => {
            return response.json();
        });
    }
 
    GetSignOut() {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var url = this.logitudeURL + 'api/Authentication/GetSignOut';
        return this._http.get(url, { headers: authHeader }).map(response => {
            return response.json();
        });
    }

    GetDocumentDownloadToken() {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var url = this.logitudeURL + 'api/Authentication/GetDocumentDownloadToken?documentToken=' + SessionInfo.DocumentDownloadToken;
        return this._http.get(url, { headers: authHeader }).map(response => {
            return response.json();
        });
    }
}

export class LoginParameters {
    
    Email: string;
    Password: string;
    IsUser: boolean;
    CardId: string;
    CardType: string;
    ByToken: boolean;
    IsMobileLogin: boolean;
    GetToken: boolean;
    IsAngularLogin: boolean;
    ClientType: string;

    //contructor() {
    //    this.IsAngularLogin = true;
    //}
}
