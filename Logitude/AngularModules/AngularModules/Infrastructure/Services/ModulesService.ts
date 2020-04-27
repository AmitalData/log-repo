import { ServiceResponse } from '../DataContracts/ServiceResponse';
import { ServiceHelper } from '../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';


@Injectable()
export class ModulesService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/FollowerExtended';
    }

    GetUserFollowEntityLists(entityid: string, objecttableid: string) {
        var url = this._apiUrl + '/GetUserFollowEntityLists/?' + 'entityid=' + entityid + '&objecttableid=' + objecttableid;
        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result = response;
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            serviceResponse.Result = result;

            return serviceResponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    AddFollowEntity(entityid: string, objecttableid: string, followerUserId: string) {
        var url = this._apiUrl + '/GetAddFollowEntity/?' + 'entityid=' + entityid + '&objecttableid=' + objecttableid + '&followerUserId=' + followerUserId;

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;

            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    DeleteFollowEntity(userid: string) {
        var url = this._apiUrl + '/GetDeleteFollowEntity/?' + 'userid=' + userid;

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;

            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }
}
