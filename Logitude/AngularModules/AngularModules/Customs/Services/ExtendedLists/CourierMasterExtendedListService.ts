import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {InfraGenericFilter} from '../../../Infrastructure/Utilities/InfraGenericFilter';
import {CachedDataManager} from '../../../Infrastructure/Utilities/CachedDataManager';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

//import { CourierMasterFiltersDataCount } from '../../DataContract/CourierMasterFiltersDataCount';
import { CourierMasterPM } from '../../EntityPMs/CourierMasterPM';
import { CourierMasterList } from '../../EntityLists/CourierMasterList';
//import { NotificationFiltersDataCount } from '../../DataContract/NotificationFiltersDataCount';

export class CourierMasterExtendedListService{

    private _http: Http;
    private _apiUrl: string;
    public static CachedData: Array<CourierMasterList> = [];
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CourierMaster';
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
                var _mappedListsArray: Array<CourierMasterList> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: CourierMasterList;
                        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }


    getCountByFilters(filters: ApiQueryFilters) {

        var urlparameters = '/getCountByFilters?';
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

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                var entity = this.MapJsonToEntity(serviceResponse.Result);
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

     MapJsonToEntityList(jsonList: any) {

        var entityList: CourierMasterList;
        entityList = new CourierMasterList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: CourierMasterPM;
        entityPM = new CourierMasterPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        return entityPM;
    }

    MapJsonToEntity(json: any) {

        //var entity: NotificationFiltersDataCount;
        //entity = new NotificationFiltersDataCount();
        //var jsonListKeys = Object.keys(json);

        //for (var key in jsonListKeys) {
        //    var property = jsonListKeys[key];
        //    entity[property] = json[property];
        //}


        //return entity;
        return null;
    }

}
