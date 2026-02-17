
import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { InfraGenericFilter } from '../../../Infrastructure/Utilities/InfraGenericFilter';
import { CachedDataManager } from '../../../Infrastructure/Utilities/CachedDataManager';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { CustomsRequestsSheetList } from '../../EntityLists/CustomsRequestsSheetList';

@Injectable()

export class CustomsRequestsSheetExtendedListService {
    private _http: Http;
    private _apiUrl: string;
    public static CachedData: Array<CustomsRequestsSheetList> = [];
    constructor() {
        this._http = ServiceHelper.Http;
        //CustomsRequestsSheetViewsController
        //CustomsRequestsSheetViews
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsRequestsSheetViews';
    }

    getSingle(declarationid: string, invoicecounterkey: number, lineNumber: number) {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/getsingle/?' + 'declarationid=' + declarationid + '&' + 'invoicecounterkey=' + invoicecounterkey + '&' + 'lineNumber=' + lineNumber, { headers: authHeader }).map(response => {
                var list = response.json();

                var entity: CustomsRequestsSheetList;
                if (list) {
                    entity = this.MapJsonToEntityList(list);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    getAll() {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/getall', { headers: authHeader }).map(response => {

                var allLists = response.json();
                var _mappedListsArray: Array<CustomsRequestsSheetList> = [];
                if (allLists) {
                    for (var key in allLists) {
                        var entity: CustomsRequestsSheetList;
                        entity = this.MapJsonToEntityList(allLists[key]);
                        _mappedListsArray.push(entity);
                    }
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    getByFilters(filters: ApiQueryFilters) {

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

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callUrl = this._apiUrl.concat(urlparameters);//


        return Observable.defer(() => {
            return this._http.get(callUrl, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse;
                serviceResponse = response.json();
                var _mappedListsArray: Array<CustomsRequestsSheetList> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: CustomsRequestsSheetList;
                        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }


    MapJsonToEntityList(jsonList: any) {

        var entityList: CustomsRequestsSheetList;
        entityList = new CustomsRequestsSheetList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

}

