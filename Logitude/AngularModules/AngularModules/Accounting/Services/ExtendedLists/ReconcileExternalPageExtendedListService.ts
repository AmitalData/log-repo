import {Injectable} from '@angular/core';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {LedgerTransactionList} from '../../EntityLists/LedgerTransactionList';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
 
@Injectable()

export class ReconcileExternalPageExtendedListService {
  
    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
   
        this.httpClient = ServiceHelper.HttpClient;
         this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReconcileExternalPagesExtended';
    }


    getExternalReoncilioationsByFilter(objectTableId: string, entityId: string, filters: ApiQueryFilters)
    {
   
        var url = this._apiUrl + "/getExternalReoncilioationsByFilter";

        var urlparameters = '?objectTableId=' + objectTableId + '&entityId=' + entityId;

        urlparameters = this.ParseFiltersIntoURI(filters, urlparameters);


        var callUrl = url.concat(urlparameters);
        return this.httpClient.get(callUrl,  ServiceHelper.GetHttpHeaders()).pipe(
            map((response:ServiceResponse) => {
                var serviceResponse: ServiceResponse;
                serviceResponse = response;
                console.log("serviceResponse: ", serviceResponse);

                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError)); 
  
    }

    private ParseFiltersIntoURI(filters: ApiQueryFilters, urlparameters: string)
    {
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        var callTime = new Date();
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];
            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");
            if (urlparameters != "?") {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter) {
                propValue = encodeURIComponent(propValue);
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
            }
            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);
        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }
        return urlparameters;
    }

    getBankPageLinesByIds(Ids: string[]) {
    

        var params: string = "";
        for (var id of Ids) {
            params += "Ids[]=" + id + "&";
        }

        var url = this._apiUrl + '/getBankPageLinesByIds?' + params;

        return this.httpClient.get(url,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

   
    }

    MapJsonToEntityList(jsonList: any) {

        var entityList: LedgerTransactionList;
        entityList = new LedgerTransactionList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }


}
