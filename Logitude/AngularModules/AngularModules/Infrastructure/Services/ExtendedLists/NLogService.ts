import { CacheKey } from '../../Components/Maintenance/CacheLogComponent';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';
import { Rule } from 'Infrastructure/Components/Maintenance/NLogSettingsComponent';

@Injectable()
export class NLogService {
    private _http: HttpClient;
    private _apiUrl: string;

    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/NLog';
    }

    getRules() {
        var url = this._apiUrl + '/GetRulesList';
        var callTime = new Date();
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                if (response) {
                    var allLists = response;
                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = allLists;
                    serviceResponse.CallTime = callTime;
                    return serviceResponse;
                }
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    addLog(logPattern: string) {
        var url = this._apiUrl + '/AddLog?' + 'logPattern=' + logPattern;
        var callTime = new Date();
        return defer(() => {
            return this._http.put(url, null,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                if (response) {
                    var allLists = response;
                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = allLists;
                    serviceResponse.CallTime = callTime;
                    return serviceResponse;
                }
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    killRule() {
        var url = this._apiUrl + '/KillRule';
        var callTime = new Date();
        return defer(() => {
            return this._http.put(url,null, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                return response;
            }), catchError(ServiceHelper.HandleServiceError));
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
