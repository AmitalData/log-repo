import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';


import { defer, of } from 'rxjs';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse'; 


@Injectable()
export class UserExtendedPMService {


    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/UserExtended';
    

    }

    GetUsersWorkspaceSummary(tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '/GetUsersWorkspaceSummary?tenant=' + tenant + '&type=',ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetUsersTwoFactorAuthenticationEnabled(tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '/GetUsersTwoFactorAuthenticationEnabled/?tenant=' + tenant + '&type=',ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));

    }

    PostUpdateTwoFactorAuthenticationEnabled(tenant: number, userIds: string[]) {
        

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.post(this._apiUrl + '/PostUpdateTwoFactorAuthenticationEnabled?tenant=' + tenant + '&userIds='  + userIds, JSON.stringify(userIds),ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));

    }



    GetUserLoginHistory(userId:string , tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '?userId=' + userId + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    Anonymization(userId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '/GetAnonymizationUser?userId='  + userId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }




    GetUsersWorkspaceRecentLogins(tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '/GetUsersWorkspaceRecentLogins?tenant=' + tenant ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));

   
        }


    GetCustomQueriesList(userId: string, objectTableId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '?userId=' + userId + '&objectTableId=' + objectTableId + '&tenant=' + tenant ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));


    }

    GetUserLicenses() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetUserLicenses';

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    update(entityPM: any) {

        var callTime = new Date();
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            
                //var mappedEntity: UserPM;
                //mappedEntity = this.MapJsonToEntityPM(entityPM, false);

            return this._http.put(this._apiUrl, JSON.stringify(entityPM), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {


                        var pm = response;
                        if (pm) {
                            serviceResponse.Result = pm;
                        }
                         
                        return serviceResponse;

                    }),catchError(ServiceHelper.HandleServiceError));
            
        }

        );

    }

    AddUserToReleaseNotesUsers(userId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '/GetAddUserToReleaseNotesUsers?userId=' + userId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    CheckUserReleaseNotesToolTip(userId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '/GetCheckUserReleaseNotesToolTip?userId=' + userId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = response;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
    }
}

