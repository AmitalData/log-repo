import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { EntityStatusList } from '../../EntityLists/EntityStatusList';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';

@Injectable()
export class EntityStatusExtendedListService {
    private _http: HttpClient;
    private _apiUrl: string;
    public static CachedData: Array<EntityStatusList> = [];
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/EntityStatusExtendedList';
    }

    getSingle(code: string) {
        var url = this._apiUrl + '/getsingle/?' + 'code=' + code;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var list = response;

                var entity: EntityStatusList;
                if (list) {
                    entity = this.MapJsonToEntityList(list);
                }
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;

                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToEntityList(jsonList: any) {
        var entityList: EntityStatusList;
        entityList = new EntityStatusList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
}
