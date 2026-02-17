
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';

@Injectable()

export class FollowerExtendedPMService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/FollowerExtended';
    }

    AddDeleteFollower(followeeUserId: string, followerUserId: string, isDelete: boolean, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetAddDeleteFollower/?' + 'followeeUserId=' + followeeUserId + '&followerUserId=' + followerUserId + '&isDelete=' + isDelete + '&tenant=' + tenant, { headers: authHeader }).map(response => {

            var result = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;

            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    AddFollowEntity(entityid: string, objecttableid: string, followerUserId: string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetAddFollowEntity/?' + 'entityid=' + entityid + '&objecttableid=' + objecttableid + '&followerUserId=' + followerUserId , { headers: authHeader }).map(response => {

            var result = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;

            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    DeleteFollowEntity(userid: string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetDeleteFollowEntity/?' + 'userid=' + userid, { headers: authHeader }).map(response => {

            var result = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;

            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    GetUserFollowEntityLists(entityid: string, objecttableid: string){

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetUserFollowEntityLists/?' + 'entityid=' + entityid + '&objecttableid=' + objecttableid , { headers: authHeader }).map(response => {

            var result = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;

            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    


}
