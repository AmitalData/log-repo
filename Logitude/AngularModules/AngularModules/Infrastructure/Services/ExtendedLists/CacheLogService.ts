import { CacheKey } from '../../Components/Maintenance/CacheLogComponent';
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import {SessionInfo} from '../../Utilities/SessionInfo';
import {PerformanceLogger} from '../../Utilities/PerformanceLogger';

//
@Injectable()

export class CacheLogService {
    private _http: Http;
    private _apiUrl: string;

    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CacheLog';
    }

    getKeys() {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetKeysList', { headers: authHeader }).map(response => {

                var allLists = response.json();
                var _mappedListsArray: Array<CacheKey> = [];
                if (allLists) {
                    for (var key in allLists) {
                        var entity: CacheKey;
                        entity = this.MapJsonToEntityList(allLists[key]);
                        _mappedListsArray.push(entity);
                    }
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                serviceResponse.CallTime = callTime;

                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }


    ResetLog() {
        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(this._apiUrl + '/PostResetLog', JSON.stringify(""),
                { headers: authHeader }).map((res) => {
                    var pm = res.json();
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        }
        );

    }

    EnableLog(enabled: boolean) {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.put(this._apiUrl + '/PutEnableLog?enabled=' + enabled , JSON.stringify(""),  { headers: authHeader })
            .map((res) => {
                var pm = res.json();
                return pm;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    IsLoggerEnabled() {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetIsLoggerEnabled', { headers: authHeader }).map(response => {

                var res = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = res;

                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    MapJsonToEntityList(jsonList: any) {

        var entityList: CacheKey;
        entityList = new CacheKey();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }
}
