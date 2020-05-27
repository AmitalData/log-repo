
import {Injectable} from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {InfraGenericFilter} from '../../../Infrastructure/Utilities/InfraGenericFilter';
import {CachedDataManager} from '../../../Infrastructure/Utilities/CachedDataManager';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {PerformanceLogger} from '../../../Infrastructure/Utilities/PerformanceLogger';
import {LocalStorageManager} from '../../../Infrastructure/Utilities/LocalStorageManager';
import {APInvoiceTotalVATList} from '../../EntityLists/APInvoiceTotalVATList';

@Injectable()

export class APInvoiceTotalVATListService {
    private _http: HttpClient;
    private _apiUrl: string;   
	public static CachedData: Array<APInvoiceTotalVATList> = [];
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/apinvoicetotalvatviews';  
    }

    getSingle(id: string) {
	   
   
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/getsingle/?' + 'id=' + id, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {

                var list = response.body;
                    
                var entity: APInvoiceTotalVATList;
				if(list)
				{
                   entity = this.MapJsonToEntityList(list);
                }   

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse(); 
                serviceResponse.Result = entity;  

                var servertime = response.headers.get('ServerExecutionTime');
                PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "APInvoiceTotalVAT", "GetSingleList", 'id=' + id); 

                return serviceResponse;
                }),

                catchError(ServiceHelper.HandleServiceError));
        });
    }

    getAll() {
        

        var callTime = new Date();
       return defer(() => {
           return this._http.get(this._apiUrl + '/getall', ServiceHelper.GetHttpFullHeaders())
               .pipe(
                   map((response: HttpResponse<any>) => {

              var allLists = response.body;
              var _mappedListsArray: Array< APInvoiceTotalVATList> = [];
		      if(allLists)
			  {
				for (var key in  allLists) {				
				   var entity: APInvoiceTotalVATList;
                   entity = this.MapJsonToEntityList(allLists[key]);
				   _mappedListsArray.push(entity);
				 }
               }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse(); 
                serviceResponse.Result = _mappedListsArray;

                var servertime = response.headers.get('ServerExecutionTime');
                PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "APInvoiceTotalVAT", "GetAllLists", ""); 

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


        var callUrl = this._apiUrl.concat(urlparameters);
        
		
	   return defer(() => {
           return this._http.get(callUrl, ServiceHelper.GetHttpFullHeaders())
               .pipe(
                   map((response: HttpResponse<any>) => {

                var serviceResponse: ServiceResponse;
                serviceResponse = response.body;
                var _mappedListsArray: Array< APInvoiceTotalVATList> = [];
				if(serviceResponse.Result)
				{
                for (var key in serviceResponse.Result) {
				
				   var entity: APInvoiceTotalVATList;
                   entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
				   _mappedListsArray.push(entity);

				 }
                }   

                serviceResponse.Result = _mappedListsArray;       
				
                var servertime = response.headers.get('ServerExecutionTime');
                PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "APInvoiceTotalVAT", "GetByFilters", "PageIndex:" +filters.PageIndex +", PageSize:"+filters.PageSize + ", GetAll:" + filters.GetAll);
				           
                return serviceResponse;
               }),

               catchError(ServiceHelper.HandleServiceError));
        });        
    }

	
	    MapJsonToEntityList(jsonList: any) {
       
            var entityList: APInvoiceTotalVATList;
            entityList = new APInvoiceTotalVATList();
            var jsonListKeys = Object.keys(jsonList);

            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];
                entityList[property] = jsonList[property];
            }
			

        return entityList;
    }

}

