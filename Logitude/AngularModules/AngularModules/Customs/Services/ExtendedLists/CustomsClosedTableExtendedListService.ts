
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { InfraGenericFilter } from '../../../Infrastructure/Utilities/InfraGenericFilter';
import { CachedDataManager } from '../../../Infrastructure/Utilities/CachedDataManager';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { CustomsClosedTableList } from '../../EntityLists/CustomsClosedTableList';

@Injectable()

export class CustomsClosedTableExtendedListService {
    private _http: HttpClient;
    private _apiUrl: string;
    public static CachedData: Array<CustomsClosedTableList> = [];
    constructor() {
        this._http = ServiceHelper.HttpClient;
        //CustomsClosedTableViewsController
        //CustomsClosedTableViews
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsClosedTableViews';
    }

    getSingle(declarationid: string, invoicecounterkey: number, lineNumber: number) {

        return defer(() => {
            return this._http.get(this._apiUrl + '/getsingle/?' + 'id=' + declarationid , ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var list = response;

                var entity: CustomsClosedTableList;
                if (list) {
                    entity = this.MapJsonToEntityList(list);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    getAll() {

        return defer(() => {
            return this._http.get(this._apiUrl + '/getall', ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;
                var _mappedListsArray: Array<CustomsClosedTableList> = [];
                if (allLists) {
                    for (var key in allLists) {
                        var entity: CustomsClosedTableList;
                        entity = this.MapJsonToEntityList(allLists[key]);
                        _mappedListsArray.push(entity);
                    }
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
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


        var callUrl = this._apiUrl.concat(urlparameters);//


        return defer(() => {
            return this._http.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(map((response:any) => {

                var serviceResponse: ServiceResponse;
                serviceResponse = response;
                var _mappedListsArray: Array<CustomsClosedTableList> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: CustomsClosedTableList;
                        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }


    MapJsonToEntityList(jsonList: any) {

        var entityList: CustomsClosedTableList;
        entityList = new CustomsClosedTableList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

}

