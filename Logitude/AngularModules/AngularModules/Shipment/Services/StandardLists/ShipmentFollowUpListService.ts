
import {Injectable} from '@angular/core';
import { defer, of } from 'rxjs';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ShipmentList} from '../../EntityLists/ShipmentList';
import {ViewResponse} from '../../../Infrastructure/DataContracts/ViewResponse';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';

@Injectable()
export class ShipmentFollowUpListService {

    private _apiUrl: string;
    private _http: HttpClient;
    private _serviceArgs: ServiceArgs;
    constructor() {
        console.log("constructing ShipmentFollowUpsListService");
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/FollowUpsViews';
    }

    setServiceArgs(serviceArgs: ServiceArgs) {
        this._serviceArgs = serviceArgs;
        this._http = serviceArgs.http;
        
         
        //this._apiUrl = logitude_url + 'api/FollowUpsViews';
      
    }

    getCount() {
        console.log('--------------------------------------> calling getSingle:');
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return defer(() => {
            return this._http.get(this._apiUrl + '?tenant=' + SessionLocator.Tenant.toString(), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                return response;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    getSingle(id: string) {
        console.log('--------------------------------------> calling getSingle:');
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return defer(() => {
            return this._http.get(this._apiUrl + '/getsingle/?' + 'id=' + id, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var list = response;

                var entity: ShipmentList;
                entity = this.MapJsonToEntityList(list);

                return list;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    getAll() {
        console.log('--------------------------------------> calling getAllEntityListsFromServer:');
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/getall', ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;
                var _mappedListsArray: Array<ShipmentList> = [];

                for (var key in allLists) {

                    var entity: ShipmentList;
                    entity = this.MapJsonToEntityList(allLists[key]);
                    _mappedListsArray.push(entity);

                }

                return _mappedListsArray;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    getByFilters(filters: ApiQueryFilters) {
        //console.log('--------------------------------------> calling the server with filters:');
        var urlparameters = '/getbyfilters?';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];

            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

            if (!urlparameters.endsWith('?')) {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter)
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));

            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);


        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }
        //console.log("addtionalFiltersValues", addtionalFiltersValues);
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var callUrl = this._apiUrl.concat(urlparameters);
        //console.log("Calling Url:" + callUrl);
        //console.log("abol 3abed");
        return defer(() => {
            return this._http.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                //console.log("i'm the response yo ", response);
                var viewResponse: any = response;
                //viewResponse = response.json();
                //var allLists = response.json();
                var _mappedListsArray: Array<ShipmentList> = [];

                for (var key in viewResponse.Data) {
                    var entity: ShipmentList;
                    entity = this.MapJsonToEntityList(viewResponse.Data[key]);
                    _mappedListsArray.push(entity);
                }
                viewResponse.Data = _mappedListsArray;
                //console.log(_mappedListsArray);
                return viewResponse;//{Data: _mappedListsArray,DataCount:response.headers };
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    
    MapJsonToEntityList(jsonList: any) {

        var entityList: ShipmentList;
        entityList = new ShipmentList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

}

