import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {Observable}     from 'rxjs/Rx';
import {ApiQueryFilters} from '../DataContracts/ApiQueryFilters';
import {ServiceHelper} from '../Utilities/ServiceHelper';
import {ServiceResponse} from '../DataContracts/ServiceResponse';

@Injectable()

export class ModulesService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
    }


    GetUserFollowEntityLists(entityid: string, objecttableid: string) {

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/FollowerExtended';

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetUserFollowEntityLists/?' + 'entityid=' + entityid + '&objecttableid=' + objecttableid, { headers: authHeader }).map(response => {

            var result = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;

            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }
    AddFollowEntity(entityid: string, objecttableid: string, followerUserId: string) {

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/FollowerExtended';

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetAddFollowEntity/?' + 'entityid=' + entityid + '&objecttableid=' + objecttableid + '&followerUserId=' + followerUserId, { headers: authHeader }).map(response => {

            var result = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;

            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }
    DeleteFollowEntity(userid: string) {

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/FollowerExtended';

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

}