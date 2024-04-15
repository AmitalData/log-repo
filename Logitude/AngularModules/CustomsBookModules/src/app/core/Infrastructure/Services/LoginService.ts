import { HttpClient, HttpHeaders } from '@angular/common/http';
import { SessionLocator } from '../Utilities/SessionLocator';
import { ServiceHelper } from '../Utilities/ServiceHelper';
import { SessionInfo } from '../Utilities/SessionInfo';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { AppTool } from '../Tools';

@Injectable()
export class LoginService {
    private _http: HttpClient;
    logitudeURL: string = null;
    baseUrlApi: string = null;
    baseMetaUrlApi: string = null;
    private _iisBaseApiUrl: string = "http://192.168.1.100/main/api/";
    private _iisMetaDataApiUrl: string = "http://192.168.1.100/main/api/ngMetaData";
    public CurrentTenant: number;
    public AuthHeader;
    public LoggedUserId: string;
    public LoggedUserEmail: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this.logitudeURL = AppTool.GetLogitudeURL();
        this.baseUrlApi = this.logitudeURL + "api/";
        this.baseMetaUrlApi = this.logitudeURL + "api/ngMetaData";
    }

    GetOneUsePassword() {
        var url = this.logitudeURL + "api/OneTimePassword?id=" + SessionLocator.ExternalParams.OneTimePasswordId 

        return this._http.get(url).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }
    PostAuthentication(loginParameters: any) {
        var url = this.baseUrlApi + "Authentication?dummy=user";
        //loginParameters.IsAngularLogin = true;
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            })
        };

        return this._http.post(url, JSON.stringify(loginParameters), httpOptions).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }


    PostUserValidation(loginParameters: LoginParameters) {
        var url = this.baseUrlApi + "Authentication";
        loginParameters.IsAngularLogin = true;
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            })
        };

        return this._http.post(url, JSON.stringify(loginParameters), httpOptions).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    PostLoginData(loginParameters: LoginParameters) {
        var url = this.baseUrlApi + "Authentication?tenant=" + this.CurrentTenant;
        var twoFactorKey = window.localStorage.getItem('TwoFactorkey');
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            })
        };

        if (twoFactorKey) {
            httpOptions.headers.append('TwoFactorkey', twoFactorKey);
        }
        
        return this._http.post(url, JSON.stringify(loginParameters), httpOptions).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    PostAuthenticationDeviceVerificationCode(deviceKey: string, verificationCode: string, tenant: number) {
        var url = this.baseUrlApi + "Authentication/PostAuthenticationDeviceVerificationCode?deviceKey=" + deviceKey + "&verificationCode=" + verificationCode + "&tenant=" + this.CurrentTenant;

        return this._http.post(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    PostResendAuthenticationDeviceVerificationCode(deviceKey: string, userId: string, tenant: number) {
        //PostResendAuthenticationDeviceVerificationCode(string deviceKey, string userId, int tenant)
        var url = this.baseUrlApi + "Authentication/PostResendAuthenticationDeviceVerificationCode?deviceKey=" + deviceKey +  "&userId=" + userId + "&tenant=" + this.CurrentTenant;

        return this._http.post(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetLoggedUser() {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&useremail=' + this.LoggedUserEmail + '&getloggeduser=true';

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result = response;
            return result;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetLoggedTenant() {
        //var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&getloggedtenant=true';
        var url = this.baseUrlApi + 'CommonDomain/GetLoggedTenantDB';

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetLastFilters() {
        var url = this.baseUrlApi + 'InfrastructureDomain/GetLastFilters';

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetTenantManagement() {
        var url = this.baseUrlApi + 'TenantManagement/GetSingleTenantManagementPM?id=' + this.CurrentTenant;

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    CheckTenantMangmnt(loggedUserId) {
        var url = this.baseUrlApi + 'GlobalDomain/GetCheckTenantMangmnt?loggedUserId=' + loggedUserId;

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));

    }

    GetQueries() {

        var url = this.logitudeURL + "api/ngMetaData?tenant=" + this.CurrentTenant + "&userid=" + this.LoggedUserId + "&objecttableid=dummy";

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetStatuses() {

        var url = this.baseMetaUrlApi + "?tenant=" + this.CurrentTenant + "&inActive=false&dumb2=dumb";

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetPreDefinedFilters() {
        var url = this.baseMetaUrlApi + "/GetAdvanceQueryFiltersPMs?tenant=" + this.CurrentTenant;

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetTenantTranslations() {
        var url = this.baseMetaUrlApi + "?translationTenant=" + this.CurrentTenant;

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetTenantLanguageTranslations() {
        var url = this.baseMetaUrlApi + "/GetTenantLanguageTranslations?tenant=" + this.CurrentTenant;

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetTransportModes() {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&dummy=dummy';

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetDirections() {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&dummy2=dummy2';

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetMenusTables() {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&menustables=dummy';

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetObjectTables() {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&objecttables=dummy';

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetScreens() {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&screens=dummy';

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetScreenFields() {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&screenfields=dummy';

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetObjectTableTabs() {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&objecttabletabs=dummy';

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetObjectFields() {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&objectTableName=Shipment&inActive=false';

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GeLoggedTenantObjectFields() {
        var url = this.baseMetaUrlApi + '/GetTenantObjectFields?loggedTenant=' + this.CurrentTenant;

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetTextCodesTranslations() {
        var url = this.baseMetaUrlApi + '?tenant=' + this.CurrentTenant + '&textcodetranslations=dummy';

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetAccountingSetting() {
        var url = this.baseMetaUrlApi + '?id=' + this.CurrentTenant + '&textcodetranslations=dummy';

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }   

    GetCustomsInterfaceSetting() {
        var url = this.baseMetaUrlApi + '?InterfaceId=' + this.CurrentTenant + '&textcodetranslations=dummy';;
        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetSharedLogisticsSetting() {
        var url = this.baseMetaUrlApi + '?settingId=' + this.CurrentTenant + '&textcodetranslations=dummy';;
        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }
    
    GetAccountingSystem(AccountingSystemCode: string) {
        var url = this.logitudeURL + 'api/GlobalDomain/GetAccountingSystem?AccountingSystemCode=' + AccountingSystemCode;

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetTenantTextCode() {
        var url = this.baseMetaUrlApi + '/GetTenantTextCodes?tenant=' + this.CurrentTenant;

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetGlobalSetting() {
        var url = this.logitudeURL + 'api/GlobalDomain/GetGlobalSetting';

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetPrivateLableById(Id : string) {
        var url = this.logitudeURL + 'api/GlobalDomain/GetPrivateLableById?Id=' + Id;

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetTenantSetting() {
        var url = this.logitudeURL + 'api/GlobalDomain/GetTenantSetting';

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetTips() {
        var url = this.logitudeURL + 'api/Tips/GetTipsPMs?tenant=' + this.CurrentTenant;

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetTipsVisibilities() {
        var url = this.logitudeURL + 'api/TipsVisibility/GetTipsVisibilities?tenant=' + this.CurrentTenant + "&userid=" + this.LoggedUserId;

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }
 
    GetSignOut() {
        var url = this.logitudeURL + 'api/Authentication/GetSignOut';
        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetDocumentDownloadToken() {
        var url = this.logitudeURL + 'api/Authentication/GetDocumentDownloadToken?documentToken=' + SessionInfo.DocumentDownloadToken;
        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
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
