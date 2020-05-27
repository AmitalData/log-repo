import { HttpClient, HttpEvent, HttpResponse } from '@angular/common/http';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { PerformanceLogger } from '../../Utilities/PerformanceLogger';
import { BusinessRoleList } from '../../EntityLists/BusinessRoleList';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';

@Injectable()
export class BusinessRoleExtendedListService {
    private _http: HttpClient;
    private _apiUrl: string;
    public static CachedData: Array<BusinessRoleList> = [];
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/businessroleextendedviews';
    }

    getToggleBusinessRoles(memberId: string) {
        var url = this._apiUrl + '/GetToggleBusinessRoles?memberId=' + memberId;
        var callTime = new Date();

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpEvent<any>) => {
                if (response instanceof HttpResponse) {
                    var allLists = response;
                    var _mappedListsArray: Array<BusinessRoleList> = [];
                    if (allLists) {
                        for (var key in allLists) {
                            var entity: BusinessRoleList;
                            entity = this.MapJsonToEntityList(allLists[key]);
                            _mappedListsArray.push(entity);
                        }
                    }

                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = _mappedListsArray;
                    serviceResponse.CallTime = callTime;
                    var servertime = response.headers.get('ServerExecutionTime');
                    PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "BusinessRole", "GetAllLists", "");

                    return serviceResponse;
                }
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToEntityList(jsonList: any) {
        var entityList: BusinessRoleList;
        entityList = new BusinessRoleList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
}
