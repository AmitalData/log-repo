import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ContainerFollowUpList } from '../../EntityLists/ContainerFollowUpList';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { PerformanceLogger } from '../../../Infrastructure/Utilities/PerformanceLogger';

@Injectable()

export class ContainerFollowUpListService {
    private _http: Http;
    private _apiUrl: string;
    public static CachedData: Array<ContainerFollowUpList> = [];
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ContainerFollowUpViews';
    }

    getSingle(id: string) {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/getsingle/?' + 'id=' + id, { headers: authHeader }).map(response => {

                var list = response.json();

                var entity: ContainerFollowUpList;
                if (list) {
                    entity = this.MapJsonToEntityList(list);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;

                var servertime = response.headers.get('ServerExecutionTime');
                PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "ContainerFollowUp", "GetSingleList", 'id=' + id);

                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
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
                var _mappedListsArray: Array<ContainerFollowUpList> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: ContainerFollowUpList;
                        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;

                var servertime = response.headers.get('ServerExecutionTime');
                PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "ContainerFollowUp", "GetByFilters", "PageIndex:" + filters.PageIndex + ", PageSize:" + filters.PageSize + ", GetAll:" + filters.GetAll);

                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    MapJsonToEntityList(jsonList: any) {
        var entityList: ContainerFollowUpList;
        entityList = new ContainerFollowUpList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }

}
