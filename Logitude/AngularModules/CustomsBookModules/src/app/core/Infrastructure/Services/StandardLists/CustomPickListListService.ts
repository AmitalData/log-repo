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
import {PerformanceLogger} from '../../Utilities/PerformanceLogger';
import {CustomPickListList} from '../../EntityLists/CustomPickListList';
import { LocalStorageManager } from '../../Utilities/LocalStorageManager';
@Injectable()

export class CustomPickListListService {
	private _http: HttpClient;
    private _apiUrl: string;   
	public static CachedData: Array<CustomPickListList> = [];
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/custompicklistviews';  
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

                        var entity: CustomPickListList;
                        if (list) {
                            entity = this.MapJsonToEntityList(list);
                        }

                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = entity;

                        var servertime = response.headers.get('ServerExecutionTime');
                        PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "CustomPickList", "GetSingleList", 'id=' + id);

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
                       var _mappedListsArray: Array<CustomPickListList> = [];
                       if (allLists) {
                           for (var key in allLists) {
                               var entity: CustomPickListList;
                               entity = this.MapJsonToEntityList(allLists[key]);
                               _mappedListsArray.push(entity);
                           }
                       }

                       var serviceResponse: ServiceResponse;
                       serviceResponse = new ServiceResponse();
                       serviceResponse.Result = _mappedListsArray;

                       var servertime = response.headers.get('ServerExecutionTime');
                       PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "CustomPickList", "GetAllLists", "");

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
                        var _mappedListsArray: Array<CustomPickListList> = [];
                        if (serviceResponse.Result) {
                            for (var key in serviceResponse.Result) {

                                var entity: CustomPickListList;
                                entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                                _mappedListsArray.push(entity);

                            }
                        }

                        serviceResponse.Result = _mappedListsArray;

                        var servertime = response.headers.get('ServerExecutionTime');
                        PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "CustomPickList", "GetByFilters", "PageIndex:" + filters.PageIndex + ", PageSize:" + filters.PageSize + ", GetAll:" + filters.GetAll);

                        return serviceResponse;
                    }),

                    catchError(ServiceHelper.HandleServiceError));
        });        
    }

    getSingleFromCache(id: string) {

	        var callTime = new Date();
		 if (!SessionLocator.UseCachedData) {
            return this.getSingle(id);
        }
	    
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
		var exists = CustomPickListListService.CachedData.filter(a => a.Id === id).length;

        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse(); 
        if (exists === 0) {
        return defer(() => {
            var cacheKey = "CustomPickList_CachedData_" + SessionLocator.Tenant;
            var _mappedListsArray: Array<CustomPickListList> = [];
            var cachedString = LocalStorageManager.GetItem(cacheKey);
                if (cachedString) {
                    var cachedJson = JSON.parse(cachedString);
                    for (var key in cachedJson) {

                        var entity: CustomPickListList;
                        entity = this.MapJsonToEntityList(cachedJson[key]);
                        _mappedListsArray.push(entity);
                    }

                    CustomPickListListService.CachedData = _mappedListsArray;
                    serviceResponse = new ServiceResponse();
                    
                    var filteredData = CustomPickListListService.CachedData.filter(a => a.Id === id)[0];
                    serviceResponse.Result = filteredData;

 
                    PerformanceLogger.InsertPerformanceLog(callTime, new Date(), 0, "CustomPickList", "GetSingleListFromCache", 'id=' + id); 


                    return of(serviceResponse);

                    
                }
				else
				{

                    return this._http.get(this._apiUrl + '/getsingle/?' + 'id=' + id, ServiceHelper.GetHttpFullHeaders())
                        .pipe(
                            map((response: HttpResponse<any>) => {
                                var list = response;

                                var entity: CustomPickListList;
                                if (list) {
                                    entity = this.MapJsonToEntityList(list);
                                }

                                serviceResponse.Result = entity;

                                var servertime = response.headers.get('ServerExecutionTime');
                                PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "CustomPickList", "GetSingleList", 'id=' + id);

                                return serviceResponse;
                            }),

                            catchError(ServiceHelper.HandleServiceError));
			}
        });
		}
		else
		{
		   var filteredData = CustomPickListListService.CachedData.filter(a => a.Id === id)[0];
		    serviceResponse.Result = filteredData;
		   return of(serviceResponse);
		}
    }

    getAllFromCache(filters: ApiQueryFilters = new ApiQueryFilters(true)) {

		        var callTime = new Date();
		 if (!SessionLocator.UseCachedData) {
            return this.getByFilters(filters);
        }

  var exists = CustomPickListListService.CachedData.length;
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
				if (exists === 0 || filters.ForceCacheRefresh) {
					propValue = encodeURIComponent(propValue);
				}

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
        
       
        if (exists === 0 || filters.ForceCacheRefresh) {
            var cacheKey = "CustomPickList_CachedData_" + filters.Tenant;
            var _mappedListsArray: Array<CustomPickListList> = [];
            var serviceResponse: ServiceResponse;

            if (!filters.ForceCacheRefresh) {
                var cachedString = LocalStorageManager.GetItem(cacheKey);
                if (cachedString) {
                    var cachedJson = JSON.parse(cachedString);
                    for (var key in cachedJson) {

                        var entity: CustomPickListList;
                        entity = this.MapJsonToEntityList(cachedJson[key]);
                        _mappedListsArray.push(entity);
                    }

                    CustomPickListListService.CachedData = _mappedListsArray;
                    serviceResponse = new ServiceResponse();
                     if (!filters.GetAll) {
                        _mappedListsArray = InfraGenericFilter.GetFilteredArray(_mappedListsArray, filters);
                    }
                    serviceResponse.Result = _mappedListsArray;

                    
                    PerformanceLogger.InsertPerformanceLog(callTime, new Date(), 0, "CustomPickList", "GetAllFromCache", ""); 

                    
                }
            }
            if (serviceResponse) {
                return of(serviceResponse);
            }
            else {
                return defer(() => {
                    return this._http.get(callUrl, ServiceHelper.GetHttpFullHeaders())
                        .pipe(
                            map((response: HttpResponse<any>) => {

                                var serviceResponse: ServiceResponse;
                                serviceResponse = response.body;

                                if (serviceResponse.Result) {
                                    for (var key in serviceResponse.Result) {

                                        var entity: CustomPickListList;
                                        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                                        _mappedListsArray.push(entity);

                                    }
                                }
                                if (filters.GetAll) {
                                    LocalStorageManager.SetItem(cacheKey, JSON.stringify(_mappedListsArray))
                                    CustomPickListListService.CachedData = _mappedListsArray;
                                }
                                else {

                                    _mappedListsArray = InfraGenericFilter.GetFilteredArray(_mappedListsArray, filters);
                                }
                                serviceResponse.Result = _mappedListsArray;


                                var servertime = response.headers.get('ServerExecutionTime');
                                PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "CustomPickList", "GetAll", "");

                                return serviceResponse;
                            }),

                            catchError(ServiceHelper.HandleServiceError));
                });
            }
        }
        else {
            var filteredData = CustomPickListListService.CachedData;
            if (!filters.GetAll) {
	
                filteredData = InfraGenericFilter.GetFilteredArray(filteredData, filters);
            }
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            serviceResponse.Result = filteredData;

            return of(serviceResponse);
        }
    }
	
	    MapJsonToEntityList(jsonList: any) {
       
            var entityList: CustomPickListList;
            entityList = new CustomPickListList();
            var jsonListKeys = Object.keys(jsonList);

            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];
                entityList[property] = jsonList[property];
            }
			

        return entityList;
    }

}

