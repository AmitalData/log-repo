import {Injectable} from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ApiQueryFilters} from '../../DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import {InfraGenericFilter} from '../../Utilities/InfraGenericFilter';
import {CachedDataManager} from '../../Utilities/CachedDataManager';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {SessionInfo} from '../../Utilities/SessionInfo';
import {LocalStorageManager} from '../../Utilities/LocalStorageManager';
import {PerformanceLogger} from '../../Utilities/PerformanceLogger';
import {BusinessHoursHolidayList} from '../../EntityLists/BusinessHoursHolidayList';

@Injectable()

export class BusinessHoursHolidayListService {
	private _http: HttpClient;
    private _apiUrl: string;   
	public static CachedData: Array<BusinessHoursHolidayList> = [];
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/businesshoursholidayviews';  
    }

    getSingle(id: string) {
	   
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/getsingle/?' + 'id=' + id, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {

                        var list = response.body;

                        var entity: BusinessHoursHolidayList;
                        if (list) {
                            entity = this.MapJsonToEntityList(list);
                        }

                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = entity;

                        var servertime = response.headers.get('ServerExecutionTime');
                        PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "BusinessHoursHoliday", "GetSingleList", 'id=' + id);

                        return serviceResponse;
                    }),

                    catchError(ServiceHelper.HandleServiceError));
        });
    }

    getAll() {
        
	   var authHeader = new Headers();
       authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
       return defer(() => {
           return this._http.get(this._apiUrl + '/getall', ServiceHelper.GetHttpFullHeaders())
               .pipe(
                   map((response: HttpResponse<any>) => {

                       var allLists = response.body;
                       var _mappedListsArray: Array<BusinessHoursHolidayList> = [];
                       if (allLists) {
                           for (var key in allLists) {
                               var entity: BusinessHoursHolidayList;
                               entity = this.MapJsonToEntityList(allLists[key]);
                               _mappedListsArray.push(entity);
                           }
                       }

                       var serviceResponse: ServiceResponse;
                       serviceResponse = new ServiceResponse();
                       serviceResponse.Result = _mappedListsArray;

                       var servertime = response.headers.get('ServerExecutionTime');
                       PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "BusinessHoursHoliday", "GetAllLists", "");

                       return serviceResponse;
                   }),

                   catchError(ServiceHelper.HandleServiceError));
        });
    }
	
    getByFilters(filters: ApiQueryFilters) {

        var callTime = new Date();
		                        
        var urlparameters = '/getbyfilters?';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];

            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

            if (urlparameters != "?") {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter)
                {
					propValue = encodeURIComponent(propValue);
					urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
				}

            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);


        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callUrl = this._apiUrl.concat(urlparameters);//
        
		
	   return defer(() => {
           return this._http.get(callUrl, ServiceHelper.GetHttpFullHeaders())
               .pipe(
                   map((response: HttpResponse<any>) => {

                       var serviceResponse: ServiceResponse;
                       serviceResponse = response.body;
                       var _mappedListsArray: Array<BusinessHoursHolidayList> = [];
                       if (serviceResponse.Result) {
                           for (var key in serviceResponse.Result) {

                               var entity: BusinessHoursHolidayList;
                               entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                               _mappedListsArray.push(entity);

                           }
                       }

                       serviceResponse.Result = _mappedListsArray;

                       var servertime = response.headers.get('ServerExecutionTime');
                       PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "BusinessHoursHoliday", "GetByFilters", "PageIndex:" + filters.PageIndex + ", PageSize:" + filters.PageSize + ", GetAll:" + filters.GetAll);

                       return serviceResponse;
                   }),

                   catchError(ServiceHelper.HandleServiceError));
        });        
    }

	
	    MapJsonToEntityList(jsonList: any) {
       
            var entityList: BusinessHoursHolidayList;
            entityList = new BusinessHoursHolidayList();
            var jsonListKeys = Object.keys(jsonList);

            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];
                entityList[property] = jsonList[property];
            }
			

        return entityList;
    }

}

