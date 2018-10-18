import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';


import {Observable} from 'rxjs/Rx';
import 'rxjs/add/operator/map';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse'; 

import {ChangePasswordParameter} from '../../../Infrastructure/DataContracts/ChangePasswordParameter'; 


@Injectable()
export class PasswordChangeService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/PasswordChange';
    }



    CheckUserPassword(changePasswordParameter: ChangePasswordParameter) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.post(this._apiUrl + '/PostCheckUserPassword', JSON.stringify(changePasswordParameter), {
                headers: authHeader,

            }).map(response => {
                var result = response.json();

                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
        }

        );

    }




    ChangeUserPassword(changePasswordParameter: ChangePasswordParameter) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.post(this._apiUrl + '/PostChangeUserPassword', JSON.stringify(changePasswordParameter), {
                headers: authHeader,

            }).map(response => {
                var result = response.json();

                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
        }

        );

    }


    ResetUserPassword(userId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetResetUserPassword' + '?userId=' + userId + '&tenant=' + tenant, { headers: authHeader }).map(response => {

                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = response.json();
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
    }


    CheckIfUserIsExists(email: string, tenant: number, hasPassword: boolean, hasContact: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '/GetCheckIfUserIsExists' + '?email=' + email + '&tenant=' + tenant + '&hasPassword=' + hasPassword + '&hasContact=' + hasContact, { headers: authHeader }).map(response => {

                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = response.json();
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
    }



    GetSetUserLastLogin(password: string, contactId: string, tenant: number, computerId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetSetUserLastLogin' + '?password=' + password + '&contactId=' + contactId + '&tenant=' + tenant + '&computerId=' + computerId, { headers: authHeader }).map(response => {



                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = response.json();
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
    }


    
}

