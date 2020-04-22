
import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';

@Injectable()

export class FollowerExtendedPMService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/FollowerExtended';
    }

    AddDeleteFollower(followeeUserId: string, followerUserId: string, isDelete: boolean, tenant: number) {

        return this._http.get(this._apiUrl + '/GetAddDeleteFollower/?' + 'followeeUserId=' + followeeUserId + '&followerUserId=' + followerUserId + '&isDelete=' + isDelete + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;

            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    AddFollowEntity(entityid: string, objecttableid: string, followerUserId: string) {


        return this._http.get(this._apiUrl + '/GetAddFollowEntity/?' + 'entityid=' + entityid + '&objecttableid=' + objecttableid + '&followerUserId=' + followerUserId , ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;

            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    DeleteFollowEntity(userid: string) {


        return this._http.get(this._apiUrl + '/GetDeleteFollowEntity/?' + 'userid=' + userid, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;

            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetUserFollowEntityLists(entityid: string, objecttableid: string){


        return this._http.get(this._apiUrl + '/GetUserFollowEntityLists/?' + 'entityid=' + entityid + '&objecttableid=' + objecttableid , ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;

            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    


}
