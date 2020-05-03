import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';


import { defer, of } from 'rxjs';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse'; 

import {ChangePasswordParameter} from '../../../Infrastructure/DataContracts/ChangePasswordParameter'; 


@Injectable()
export class PasswordChangeService {

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/PasswordChange';
    }



    CheckUserPassword(changePasswordParameter: ChangePasswordParameter) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return defer(() => {
            return this._http.post(this._apiUrl + '/PostCheckUserPassword', JSON.stringify(changePasswordParameter),ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result :any = response;

                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }




    ChangeUserPassword(changePasswordParameter: ChangePasswordParameter) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return defer(() => {
            return this._http.post(this._apiUrl + '/PostChangeUserPassword', JSON.stringify(changePasswordParameter),ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result :any = response;

                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }


    ResetUserPassword(userId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetResetUserPassword' + '?userId=' + userId + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = response;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
    }


    CheckIfUserIsExists(email: string, tenant: number, hasPassword: boolean, hasContact: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '/GetCheckIfUserIsExists' + '?email=' + email + '&tenant=' + tenant + '&hasPassword=' + hasPassword + '&hasContact=' + hasContact,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = response;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
    }



    GetSetUserLastLogin(password: string, contactId: string, tenant: number, computerId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetSetUserLastLogin' + '?password=' + password + '&contactId=' + contactId + '&tenant=' + tenant + '&computerId=' + computerId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {



                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = response;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
    }


    
}

