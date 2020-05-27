import { CacheKey } from '../../Components/Maintenance/CacheLogComponent';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';

@Injectable()
export class CacheLogService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CacheLog';
    }

    getKeys() {
        var url = this._apiUrl + '/GetKeysList';
        var callTime = new Date();
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;
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
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    ResetLog() {
        return defer(() => {
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var url = this._apiUrl + '/PostResetLog';

            return this._http.post(url, JSON.stringify(""), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {
                //var pm = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    EnableLog(enabled: boolean) {
        var url = this._apiUrl + '/PutEnableLog?enabled=' + enabled;
        return defer(() => {
            return this._http.put(url, JSON.stringify(""), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {
                var pm = response;
                return pm;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    IsLoggerEnabled() {
        var url = this._apiUrl + '/GetIsLoggerEnabled';
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;

                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetCacheLogExcelFile() {
        var url = this._apiUrl + '/GetCacheLogExcelFile?';
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;

                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
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
