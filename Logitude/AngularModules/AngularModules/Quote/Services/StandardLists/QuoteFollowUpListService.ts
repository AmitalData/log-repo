
import {Injectable} from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import 'rxjs/add/operator/map';
import {Observable} from 'rxjs/Rx';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {QuoteList} from '../../EntityLists/QuoteList';
import {ViewResponse} from '../../../Infrastructure/DataContracts/ViewResponse';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';

@Injectable()

export class QuoteFollowUpListService {

    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {        
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuoteFollowUpsViews';
    }

    getCount() {

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '?tenant=' + SessionLocator.Tenant.toString(), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                return response;
            }));
        }

        );
    }

    getSingle(id: string) {

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/getsingle/?' + 'id=' + id, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var list = response;

                var entity: QuoteList;
                entity = this.MapJsonToEntityList(list);

                return list;
            }));
        }

        );
    }

    getAll() {

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/getall', ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;
                var _mappedListsArray: Array<QuoteList> = [];

                for (var key in allLists) {

                    var entity: QuoteList;
                    entity = this.MapJsonToEntityList(allLists[key]);
                    _mappedListsArray.push(entity);

                }

                return _mappedListsArray;
            }));
        }

        );
    }

    getByFilters(filters: ApiQueryFilters) {
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

        var callUrl = this._apiUrl.concat(urlparameters);

        return Observable.defer(() => {
            return this._http.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var viewResponse: any = response;
                //viewResponse = response.json();
                //var allLists = response.json();
                var _mappedListsArray: Array<QuoteList> = [];

                for (var key in viewResponse.Data) {
                    var entity: QuoteList;
                    entity = this.MapJsonToEntityList(viewResponse.Data[key]);
                    _mappedListsArray.push(entity);
                }
                viewResponse.Data = _mappedListsArray;

                return viewResponse;//{Data: _mappedListsArray,DataCount:response.headers };
            }));
        }
        );
    }
    
    MapJsonToEntityList(jsonList: any) {

        var entityList: QuoteList;
        entityList = new QuoteList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

}

