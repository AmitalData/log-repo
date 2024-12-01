import { CacheKey } from '../../Components/Maintenance/CacheLogComponent';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';

@Injectable()
export class GeneralLockService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/GeneralLock';
    }
  
    GetGeneralLock(entityId:string , objectTableName:string) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + '/GetGeneralLock/?userId=' + SessionInfo.LoggedUserId + '&entityId=' + entityId + '&objectTableName=' + objectTableName, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }

        );
    }
    PostCheckLock(entityId:string , objectTableName:string) {
        debugger
        var bb =  ServiceHelper.GetLoggedUserToken();
        return defer(() => {
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var url = this._apiUrl + '/PostCheckLock';

            return this._http.post(url + '?userId=' + SessionInfo.LoggedUserId + '&entityId=' + entityId + '&objectTableName=' + objectTableName,null,  ServiceHelper.GetHttpHeaders()).pipe(map((response) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
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
