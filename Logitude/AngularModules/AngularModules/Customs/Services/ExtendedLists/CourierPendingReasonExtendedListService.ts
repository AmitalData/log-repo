import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { CourierPendingReasonPM } from '../../EntityPMs/CourierPendingReasonPM';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { CourierPendingReasonList } from 'Customs/EntityLists/CourierPendingReasonList';
import { PerformanceLogger } from 'Infrastructure/Utilities/PerformanceLogger';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { CourierPendingReasonListService } from '../StandardLists/CourierPendingReasonListService';
import { LocalStorageManager } from 'Infrastructure/Utilities/LocalStorageManager';

@Injectable()

export class CourierPendingReasonExtendedListService {
    private _http: HttpClient
    private _apiUrl: string;
    public myCourierPendingReasonListService = new CourierPendingReasonListService();
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CourierPendingReason';
    }

    GetCourierPendingReasonByUnifreightStatus(unifreightStatusCode: string) {

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetCourierPendingReasonByUnifreightStatus/?' + 'unifreightStatusCode=' + unifreightStatusCode, ServiceHelper.GetHttpHeaders()).pipe(map((response:any) => {
                var serviceResponse: ServiceResponse = response;
                var _mappedListsArray: Array<CourierPendingReasonPM> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: CourierPendingReasonPM;
                        entity = this.MapJsonToEntityPM(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);
                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    DeleteCourierPendingReasonUnifreightStatus(id: string) {

        return defer(() => {
            return this._http.delete(this._apiUrl + '/DeleteCourierPendingReasonUnifreightStatus/?' + 'id=' + id, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetSingleCourierPendingReasonPMByCode(code: string) {

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleCourierPendingReasonByCode/?' + 'code=' + code, ServiceHelper.GetHttpHeaders()).pipe(map((response:any) => {
                var entity: CourierPendingReasonPM;

                if (response.Result) {
                    entity = response.Result;
                }
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
      
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetSingleByCode(code: string) {
	   
		var callTime = new Date();

		return defer(() => {
			return this._http.get(this._apiUrl + '/GetSingleByCode/?' + 'code=' + code, ServiceHelper.GetHttpFullHeaders())
				.pipe(			
					map((response: HttpResponse<any>) => {

						var list = response.body;                   
						var entity: CourierPendingReasonList;
						if (list) {
							entity = this.MapJsonToEntityList(list);
						}   

						var serviceResponse: ServiceResponse = new ServiceResponse(); 
						serviceResponse.Result = entity;  
						serviceResponse.CallTime = callTime;

						return serviceResponse;
					}),
			
					catchError(ServiceHelper.HandleServiceError));
		});
	}

    GetSingleFromCacheByCode(code: string) {

		var callTime = new Date();

		if (!SessionLocator.UseCachedData) {
            return this.GetSingleByCode(code);
        }
	    
		var exists = CourierPendingReasonListService.CachedData.filter(a => a.Code === code && a.Tenant == SessionLocator.Tenant).length;

        var serviceResponse: ServiceResponse = new ServiceResponse(); 

        if (exists === 0) {
			return defer(() => {
				var cacheKey = "CourierPendingReason_CachedData_" + SessionLocator.Tenant;
				var _mappedListsArray: Array<CourierPendingReasonList> = [];
                var cachedString = LocalStorageManager.GetItem(cacheKey);

                if (cachedString) {
                    var cachedJson = JSON.parse(cachedString);
                    for (var key in cachedJson) {
                        var entity: CourierPendingReasonList = this.MapJsonToEntityList(cachedJson[key]);
                        _mappedListsArray.push(entity);
                    }

                    CourierPendingReasonListService.CachedData = _mappedListsArray;
                    serviceResponse = new ServiceResponse();
                    
                    var filteredData = CourierPendingReasonListService.CachedData.filter(a => a.Code === code && a.Tenant == SessionLocator.Tenant)[0];
                    serviceResponse.Result = filteredData;
					serviceResponse.CallTime = callTime;
 

                    return of(serviceResponse);                    
                }

				else {
					return this._http.get(this._apiUrl+'/GetSingleByCode/?'+'code=' + code, ServiceHelper.GetHttpFullHeaders())
					.pipe(
						map((response: HttpResponse<any>) => {
							var list = response.body;
                    
							var entity: CourierPendingReasonList;
							if (list)
							{
								entity = this.MapJsonToEntityList(list);
							}   

							serviceResponse.Result = entity;
							serviceResponse.CallTime = callTime;

							          
							return serviceResponse;
						}), 
						
						catchError(ServiceHelper.HandleServiceError));
				}
			});
		}

		else {
		   var filteredData = CourierPendingReasonListService.CachedData.filter(a => a.Code === code && a.Tenant == SessionLocator.Tenant)[0];
		    serviceResponse.Result = filteredData;
			serviceResponse.CallTime = callTime;
		   return of(serviceResponse);
		}
	}

    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: CourierPendingReasonPM;
        entityPM = new CourierPendingReasonPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        return entityPM;
    }

    MapJsonToEntityList(jsonList: any) {
       
        var entityList: CourierPendingReasonList;
        entityList = new CourierPendingReasonList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        

    return entityList;
}

}
