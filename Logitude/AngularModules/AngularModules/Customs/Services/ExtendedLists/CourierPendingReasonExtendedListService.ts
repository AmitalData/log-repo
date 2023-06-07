import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { CourierPendingReasonPM } from '../../EntityPMs/CourierPendingReasonPM';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { CourierPendingReasonListService } from '../StandardLists/CourierPendingReasonListService';
import { LocalStorageManager } from 'Infrastructure/Utilities/LocalStorageManager';
import { CourierPendingReasonList } from 'Customs/EntityLists/CourierPendingReasonList';
import { PerformanceLogger } from 'Infrastructure/Utilities/PerformanceLogger';

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

    DeleteCourierPendingReasonUnifreightStatus(courierPendingReasonList: string) {

        return defer(() => {
            return this._http.delete(this._apiUrl + '/DeleteCourierPendingReasonUnifreightStatus/?' + 'courierPendingReasonList=' + courierPendingReasonList, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
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


    getSingleFromCacheByCode(code: string) {
        
         var callTime = new Date();
         if (!SessionLocator.UseCachedData) {
             return this.getSingleByCode(code);
         }
         
         var exists = CourierPendingReasonListService.CachedData.filter(a => a.Code === code && a.Tenant==SessionLocator.Tenant).length;
 
         var serviceResponse: ServiceResponse = new ServiceResponse(); 
 
         if (exists === 0) {
             return defer(() => {
                 var cacheKey = "CourierPendingReason_CachedData_" + SessionLocator.Tenant;
                 var _mappedListsArray: Array<CourierPendingReasonList> = [];
                 var cachedString = LocalStorageManager.GetItem(cacheKey);
 
                 if (cachedString) {
                     var cachedJson = JSON.parse(cachedString);
                     for (var key in cachedJson) {
                         var entity: CourierPendingReasonList = this.myCourierPendingReasonListService.MapJsonToEntityList(cachedJson[key]);
                         _mappedListsArray.push(entity);
                     }
 
                     CourierPendingReasonListService.CachedData = _mappedListsArray;
                     serviceResponse = new ServiceResponse();
                     
                     var filteredData = CourierPendingReasonListService.CachedData.filter(a => a.Code === code && a.Tenant==SessionLocator.Tenant)[0];
                     serviceResponse.Result = filteredData;
                     serviceResponse.CallTime = callTime;
  
                    // PerformanceLogger.InsertPerformanceLog(callTime, new Date(), 0, "CourierPendingReason", "GetSingleListFromCache", 'id=' + id); 
 
                     return of(serviceResponse);                    
                 }
 
                 else {
                     return this._http.get(this._apiUrl+'/GetCourierPendingReasonByCodeAndTenant/?'+'code=' + code, ServiceHelper.GetHttpFullHeaders())
                     .pipe(
                         map((response: HttpResponse<any>) => {
                             var list = response.body;
                     
                             var entity: CourierPendingReasonList;
                             if (list)
                             {
                                 entity = this.myCourierPendingReasonListService.MapJsonToEntityList(list);
                             }   
 
                             serviceResponse.Result = entity;
                             serviceResponse.CallTime = callTime;
 
                             var servertime = response.headers.get('ServerExecutionTime');
                            // PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "CourierPendingReason", "GetSingleList", 'id=' + id); 
                                       
                             return serviceResponse;
                         }), 
                         
                         catchError(ServiceHelper.HandleServiceError));
                 }
             });
         }
 
         else {
            var filteredData = CourierPendingReasonListService.CachedData.filter(a => a.Code === code && a.Tenant==SessionLocator.Tenant)[0];
             serviceResponse.Result = filteredData;
             serviceResponse.CallTime = callTime;
            return of(serviceResponse);
         }
     }

     getSingleByCode(code: string) {
	   
		var callTime = new Date();

		return defer(() => {
			return this._http.get(this._apiUrl + '/GetCourierPendingReasonByCodeAndTenant/?' + 'code=' + code, ServiceHelper.GetHttpFullHeaders())
				.pipe(			
					map((response: HttpResponse<any>) => {

						var list = response.body;                   
						var entity: CourierPendingReasonList;
						if (list) {
							entity = this.myCourierPendingReasonListService.MapJsonToEntityList(list);
						}   

						var serviceResponse: ServiceResponse = new ServiceResponse(); 
						serviceResponse.Result = entity;  
						serviceResponse.CallTime = callTime;

						var servertime = response.headers.get('ServerExecutionTime');
						PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "CourierPendingReason", "GetSingleList", 'code=' + code); 

						return serviceResponse;
					}),
			
					catchError(ServiceHelper.HandleServiceError));
		});
	}

}
